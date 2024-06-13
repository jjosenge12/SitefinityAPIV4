using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DXC.TFSM.DataAccess;
using DXC.TFSM.Business.Model;
using Microsoft.Office.Interop.Excel;// referencia la libreria
using System.Windows.Forms.VisualStyles;

namespace DXC.TFSM.Business
{
    public class BssPrice
    {
        private readonly TFSM_PortalCotizadorEntities tFSM_PortalEntities = new TFSM_PortalCotizadorEntities();

        public Dictionary<string, string> GetHitch(DataPrice Datos)
        {
            //var lstAutos = GetCatFromSitefinity("home-autos", "auto");
            //var lstModeloAutos = GetCatFromSitefinity("home-autos", "modelo");
            Dictionary<string, string> Resultado = new Dictionary<string, string>();
            double Maximo = GetParametro("EngMaximo");
            double Minimo = GetParametro("EngMinimo");
            double PrecioUnidad = GetPrecioAuto(Datos);
            double CostoSeguro = GetSeguroAuto(Datos);
            double Suma = PrecioUnidad + CostoSeguro;
            Maximo = Math.Round((Suma * Maximo) / 10000, 1) * 10000;
            Minimo = Math.Round((Suma * Minimo) / 10000, 1) * 10000;
            Resultado.Add(Minimo.ToString(), Maximo.ToString());
            return Resultado;
        }

        public ResponseProcess GetPrice(DataPrice Datos)
        {
            ResponseProcess responseProcess = new ResponseProcess();
            string message = "Ok";
            int status = 200;

            switch (Datos.PlanCotizar)
            {
                case "Tradicional":
                    responseProcess.Prices = GetPriceTradicional(Datos, ref message, ref status);
                    responseProcess.Message = message;
                    responseProcess.Status = status;
                    return responseProcess;

                case "Balloon":
                    responseProcess.Prices = GetPriceBalloon(Datos, ref message, ref status);
                    responseProcess.Message = message;
                    responseProcess.Status = status;
                    return responseProcess;

                case "Anualidades":
                    responseProcess.Prices = GetPriceAnualidades(Datos, ref message, ref status);
                    responseProcess.Message = message;
                    responseProcess.Status = status;
                    return responseProcess;

                case "Arrendamiento financiero":
                    responseProcess.Prices = GetPriceArrendamientoFinanciero(Datos, ref message, ref status);
                    responseProcess.Message = message;
                    responseProcess.Status = status;
                    return responseProcess;

                case "Arrendamiento puro":
                    responseProcess.Prices = GetPriceArrendamientoPuro(Datos, ref message, ref status);
                    responseProcess.Message = message;
                    responseProcess.Status = status;
                    return responseProcess;

                default:
                    responseProcess.Message = "Plan Not found";
                    responseProcess.Status = 400;
                    return responseProcess;
            }
        }

        private double GetAnualidad(int NumeroPlazo)
        {
            double Resultado = 0;
            try
            {
                string CoeficienteAnual = tFSM_PortalEntities.tbl_portal_C_CoeficienteAnualidad.Where(x => x.plazo == NumeroPlazo).FirstOrDefault().Minimo.ToString();
                double.TryParse(CoeficienteAnual, out Resultado);
            }
            catch
            {
                Resultado = -1;
            }
            return Resultado;
        }

        private static double PMT(double yearlyInterestRate, int totalNumberOfMonths, double loanAmount)
        {
            var rate = (double)yearlyInterestRate / 12;
            var denominator = Math.Pow((1 + rate), totalNumberOfMonths);
            return Math.Round((rate * denominator * loanAmount) / (denominator - 1), 12);
        }

        private static double VF(double yearlyInterestRate, int totalNumberOfMonths, double loanAmount)
        {
            var rate = (double)yearlyInterestRate / 12;
            var denominator = Math.Pow((1 + rate), totalNumberOfMonths);
            return Math.Round(loanAmount * (rate / (denominator - 1)), 12);
        }

        private double GetPrecioAuto(DataPrice Datos)
        {
            double Resultado = 0;
            try
            {
                string PrecioAuto = tFSM_PortalEntities.tbl_portal_C_Modelos.Where(x => x.des_auto == Datos.Marca && x.descripcion_tipo.ToUpper() == Datos.Vesion.ToUpper() && x.descripcion == Datos.Modelo).FirstOrDefault().precio.ToString();
                double.TryParse(PrecioAuto, out Resultado);
            }
            catch
            {
                Resultado = -1;
            }
            return Resultado;
        }

        public double GetParametro(string Parametro)
        {
            double ResultadoParametro = 0;
            try
            {
                string vParametro = tFSM_PortalEntities.tbl_portal_parametros.Where(x => x.nombre == Parametro).FirstOrDefault().valor.ToString();
                double.TryParse(vParametro, out ResultadoParametro);
            }
            catch
            {
                ResultadoParametro = -1;
            }
            return ResultadoParametro;
        }

        private double GetSeguroAuto(DataPrice Datos)
        {
            double Resultado = 0;             //--execute sp_costo_total_seguro 'DF'         , 'QualitasT'      ,'AMPLIA'        ,'YARIS'      ,'SD S CVT'     ,'2018'        ,''            ,'30000'
            try
            {
                var ConsultaCostoSeguro = tFSM_PortalEntities.sp_costo_total_seguro(Datos.Estado, Datos.Aseguradora, Datos.Cobertura, Datos.Marca, Datos.Vesion, Datos.Modelo, Datos.TipoUso, Datos.EngancheDeposito, Datos.Plazo).FirstOrDefault();
                if (ConsultaCostoSeguro.Column1 == "Succes")
                    double.TryParse(ConsultaCostoSeguro.Column2.ToString(), out Resultado);
            }
            catch
            {
                Resultado = -1;
            }

            return Resultado;
        }

        private List<ResultPrice> GetPriceTradicional(DataPrice Datos, ref string msg, ref int status)
        {
            ResultPrice lPrice;
            List<ResultPrice> lsPrice = new List<ResultPrice>();

            double PrecioUnidad = GetPrecioAuto(Datos);
            double CostoSeguro = GetSeguroAuto(Datos);
            double TasaFijaAnualConIva = ((GetParametro("TasaFijaSinIVA") * 365) / 360) * (GetParametro("IVA") + 1);

            if (PrecioUnidad <= 0)
            {
                msg = "GetPriceTradicional(); >> El modelo seleccionado no tiene un precio definido >> " + Datos.Vesion + " >> " + Datos.Marca; status = 400;
            }
            if (CostoSeguro <= 0)
            {
                msg += "|| GetPriceTradicional(); >> Error al obtener el seguro del auto" + Datos.Vesion + " >> " + Datos.Marca; status = 400;
            }
            if (TasaFijaAnualConIva <= 0)
            {
                msg += "|| GetPriceTradicional(); >> " + Datos.Vesion + " >> " + Datos.Marca + " >> Error al obtener TasaFijaSinIVA, Iva"; status = 400;
            }

            lPrice = new ResultPrice();

            try
            {
                lPrice.Enganche = double.Parse(Datos.EngancheDeposito);
                lPrice.DepositoGarantia = double.Parse(Datos.EngancheDeposito);
                lPrice.PrecioTotal = PrecioUnidad + CostoSeguro - lPrice.Enganche;
                lPrice.Marca = Datos.Marca;
                lPrice.Modelo = Datos.Modelo;
                lPrice.Vesion = Datos.Vesion;
                lPrice.Anio = Datos.Anio;
                lPrice.TipoPersona = Datos.TipoPersona;
                lPrice.Plan = Datos.PlanCotizar;
                lPrice.Estado = Datos.Estado;
                lPrice.Aseguradora = Datos.Aseguradora;
                lPrice.Cobertura = Datos.Cobertura;
                lPrice.PorcentajeComision = GetParametro("PorcentajeComision");
                lPrice.Comision = (lPrice.PrecioTotal * lPrice.PorcentajeComision);
                lPrice.CAT = GetParametro("CAT");
                lPrice.PagoMensual = 0;
                lPrice.Anualidad = 0;
                //lPrice.PrecioTotal = PrecioUnidad + CostoSeguro - lPrice.Enganche - lPrice.Comision;

                /*****Cálculo a 24 meses******/

                Datos.Plazo = "24";

                ResultPrice lPrice24 = new ResultPrice();
                lPrice24.Plazo = 24;
                CostoSeguro = GetSeguroAuto(Datos);
                if (CostoSeguro <= 0)
                {
                    msg += "|| GetPriceTradicional(); >> " + Datos.Vesion + " >> " + Datos.Marca + " >> Error al obtener Seguro del auto >> a 24 meses"; status = 400;
                }

                lPrice24.Enganche = lPrice.Enganche;
                lPrice24.DepositoGarantia = lPrice.DepositoGarantia;
                lPrice24.PrecioTotal = PrecioUnidad + CostoSeguro - lPrice24.Enganche;
                lPrice24.Marca = Datos.Marca;
                lPrice24.Modelo = Datos.Modelo;
                lPrice24.Vesion = Datos.Vesion;
                lPrice24.Anio = Datos.Anio;
                lPrice24.TipoPersona = Datos.TipoPersona;
                lPrice24.Plan = Datos.PlanCotizar;
                lPrice24.Estado = Datos.Estado;
                lPrice24.Aseguradora = Datos.Aseguradora;
                lPrice24.Cobertura = lPrice.Cobertura;
                lPrice24.PorcentajeComision = lPrice.PorcentajeComision;
                lPrice24.Comision = (lPrice24.PrecioTotal * lPrice.PorcentajeComision);
                lPrice24.CAT = lPrice.CAT;
                lPrice24.PagoMensual = 0;
                lPrice24.Anualidad = 0;
                lPrice24.Mensualidad = PMT(TasaFijaAnualConIva, lPrice24.Plazo, lPrice24.PrecioTotal);
                lPrice24.Cobertura = Datos.Cobertura;
                lPrice24.Movil = Datos.Telefono;
                lPrice24.Nombre = Datos.Nombre;
                lPrice24.Apellido = Datos.Apellido;
                lsPrice.Add(lPrice24);

                /*****Cálculo a 36 meses******/

                Datos.Plazo = "36";

                ResultPrice lPrice36 = new ResultPrice();
                lPrice36.Plazo = 36;
                CostoSeguro = GetSeguroAuto(Datos);
                if (CostoSeguro <= 0)
                {
                    msg += "GetPriceTradicional(); >> " + Datos.Vesion + " >> " + Datos.Marca + " >> Error al obtener Seguro del auto >> a 36 meses"; status = 400;
                }

                lPrice36.Enganche = lPrice.Enganche;
                lPrice36.DepositoGarantia = lPrice.DepositoGarantia;
                lPrice36.PrecioTotal = PrecioUnidad + CostoSeguro - lPrice36.Enganche;
                lPrice36.Marca = Datos.Marca;
                lPrice36.Modelo = Datos.Modelo;
                lPrice36.Vesion = Datos.Vesion;
                lPrice36.Anio = Datos.Anio;
                lPrice36.TipoPersona = Datos.TipoPersona;
                lPrice36.Plan = Datos.PlanCotizar;
                lPrice36.Estado = Datos.Estado;
                lPrice36.Aseguradora = Datos.Aseguradora;
                lPrice36.Cobertura = lPrice.Cobertura;
                lPrice36.PorcentajeComision = lPrice.PorcentajeComision;
                lPrice36.Comision = (lPrice36.PrecioTotal * lPrice.PorcentajeComision);
                lPrice36.CAT = lPrice.CAT;
                lPrice36.PagoMensual = 0;
                lPrice36.Anualidad = 0;
                lPrice36.Mensualidad = PMT(TasaFijaAnualConIva, lPrice36.Plazo, lPrice36.PrecioTotal);
                lPrice36.Cobertura = Datos.Cobertura;
                lPrice36.Movil = Datos.Telefono;
                lPrice36.Nombre = Datos.Nombre;
                lPrice36.Apellido = Datos.Apellido;
                lsPrice.Add(lPrice36);

                /*****Cálculo a 48 meses******/

                Datos.Plazo = "48";

                ResultPrice lPrice48 = new ResultPrice();
                lPrice48.Plazo = 48;
                CostoSeguro = GetSeguroAuto(Datos);
                if (CostoSeguro <= 0)
                {
                    msg += "||GetPriceTradicional(); >> " + Datos.Vesion + " >> " + Datos.Marca + " >> Error al obtener Seguro del auto >> a 48 meses"; status = 400;
                }

                lPrice48.Enganche = lPrice.Enganche;
                lPrice48.DepositoGarantia = lPrice.DepositoGarantia;
                lPrice48.PrecioTotal = PrecioUnidad + CostoSeguro - lPrice48.Enganche;
                lPrice48.Marca = Datos.Marca;
                lPrice48.Modelo = Datos.Modelo;
                lPrice48.Vesion = Datos.Vesion;
                lPrice48.Anio = Datos.Anio;
                lPrice48.TipoPersona = Datos.TipoPersona;
                lPrice48.Plan = Datos.PlanCotizar;
                lPrice48.Estado = Datos.Estado;
                lPrice48.Aseguradora = Datos.Aseguradora;
                lPrice48.Cobertura = lPrice.Cobertura;
                lPrice48.PorcentajeComision = lPrice.PorcentajeComision;
                lPrice48.Comision = (lPrice48.PrecioTotal * lPrice.PorcentajeComision);
                lPrice48.CAT = lPrice.CAT;
                lPrice48.PagoMensual = 0;
                lPrice48.Anualidad = 0;
                lPrice48.Mensualidad = PMT(TasaFijaAnualConIva, lPrice48.Plazo, lPrice48.PrecioTotal);
                lPrice48.Cobertura = Datos.Cobertura;
                lPrice48.Movil = Datos.Telefono;
                lPrice48.Nombre = Datos.Nombre;
                lPrice48.Apellido = Datos.Apellido;
                lsPrice.Add(lPrice48);
            }
            catch (Exception ex)
            {
                msg += "||GetPriceTradicional(); >> Error: " + ex.Message + " >> " + ex.InnerException.ToString();
                status = 400;
                throw ex;
            }
            return lsPrice;
        }

        private List<ResultPrice> GetPriceBalloon(DataPrice Datos, ref string msg, ref int status)
        {
            ResultPrice lPrice;
            List<ResultPrice> lsPrice = new List<ResultPrice>();

            //Application aExcel = new Application();
            double PrecioUnidad = GetPrecioAuto(Datos);
            double CostoSeguro = GetSeguroAuto(Datos);
            double dBallon = GetParametro("PorcentajeBalloon");
            double MontoBallon = PrecioUnidad * dBallon;
            double TasaFijaAnualConIva = ((GetParametro("TasaFijaSinIVA") * 365) / 360) * (GetParametro("IVA") + 1);

            if (PrecioUnidad <= 0)
            {
                msg = "||GetPriceBalloon/(); >> Error al obtener el precio de la unidad >> " + Datos.Marca + " >> " + Datos.Modelo + " >> " + Datos.Vesion; status = 400;
            }
            if (CostoSeguro <= 0)
            {
                msg += "||GetPriceBalloon/(); >> Error al obtener el costo del seguro >> " + Datos.Marca + " >> " + Datos.Modelo + " >> " + Datos.Vesion; status = 400;
            }
            if (dBallon <= 0)
            {
                msg += "||GetPriceBalloon/(); >> Error al obtener el parámetro PorcentajeBalloon >> " + Datos.Marca + " >> " + Datos.Modelo + " >> " + Datos.Vesion; status = 400;
            }
            if (MontoBallon <= 0)
            {
                msg += "||GetPriceBalloon/(); >> Error al obtener el parámetro PorcentajeBalloon >> " + Datos.Marca + " >> " + Datos.Modelo + " >> " + Datos.Vesion; status = 400;
            }
            if (TasaFijaAnualConIva <= 0)
            {
                msg += "GetPriceBalloon/(); >> Error al obtener el parámetro TasaFijaSinIVA,IVA  >> " + Datos.Marca + " >> " + Datos.Modelo + " >> " + Datos.Vesion; status = 400;
            }

            lPrice = new ResultPrice();

            try
            {
                lPrice.Enganche = double.Parse(Datos.EngancheDeposito);
                lPrice.DepositoGarantia = double.Parse(Datos.EngancheDeposito);
                lPrice.PrecioTotal = PrecioUnidad + CostoSeguro - lPrice.Enganche;
                lPrice.Marca = Datos.Marca;
                lPrice.Modelo = Datos.Modelo;
                lPrice.Vesion = Datos.Vesion;
                lPrice.Anio = Datos.Anio;
                lPrice.TipoPersona = Datos.TipoPersona;
                lPrice.Plan = Datos.PlanCotizar;
                lPrice.Estado = Datos.Estado;
                lPrice.Aseguradora = Datos.Aseguradora;
                lPrice.Cobertura = Datos.Cobertura;
                lPrice.PorcentajeComision = GetParametro("PorcentajeComision");
                lPrice.Comision = (lPrice.PrecioTotal * lPrice.PorcentajeComision);
                lPrice.CAT = GetParametro("CAT");
                lPrice.PagoMensual = 0;
                lPrice.Anualidad = 0;
                //lPrice.PrecioTotal = PrecioUnidad + CostoSeguro - lPrice.Enganche - lPrice.Comision;

                /*****Cálculo a 24 meses******/

                Datos.Plazo = "24";

                ResultPrice lPrice24 = new ResultPrice();
                lPrice24.Plazo = 24;
                CostoSeguro = GetSeguroAuto(Datos);
                if (CostoSeguro <= 0)
                {
                    msg += "GetPriceBalloon(); >> " + Datos.Vesion + " >> " + Datos.Marca + " >> " + Datos.Vesion + " >> Error al obtener Seguro del auto >> a 24 meses"; status = 400;
                }
                lPrice24.Enganche = lPrice.Enganche;
                lPrice24.DepositoGarantia = lPrice.DepositoGarantia;
                lPrice24.PrecioTotal = PrecioUnidad + CostoSeguro - lPrice24.Enganche;
                lPrice24.Marca = Datos.Marca;
                lPrice24.Modelo = Datos.Modelo;
                lPrice24.Vesion = Datos.Vesion;
                lPrice24.Anio = Datos.Anio;
                lPrice24.TipoPersona = Datos.TipoPersona;
                lPrice24.Plan = Datos.PlanCotizar;
                lPrice24.Estado = Datos.Estado;
                lPrice24.Aseguradora = Datos.Aseguradora;
                lPrice24.Cobertura = lPrice.Cobertura;
                lPrice24.PorcentajeComision = lPrice.PorcentajeComision;
                lPrice24.Comision = (lPrice24.PrecioTotal * lPrice.PorcentajeComision);
                lPrice24.CAT = lPrice.CAT;
                lPrice24.PagoMensual = 0;
                lPrice24.Anualidad = 0;
                //lPrice24.Mensualidad = aExcel.WorksheetFunction.Pmt(TasaFijaAnualConIva / 12, lPrice24.Plazo, -lPrice24.PrecioTotal, MontoBallon);
                lPrice24.Mensualidad = PMT(TasaFijaAnualConIva, lPrice24.Plazo, lPrice24.PrecioTotal) - VF(TasaFijaAnualConIva, lPrice24.Plazo, MontoBallon);
                lPrice24.Cobertura = Datos.Cobertura;
                lPrice24.Movil = Datos.Telefono;
                lPrice24.Nombre = Datos.Nombre;
                lPrice24.Apellido = Datos.Apellido;
                lPrice24.Ballon = MontoBallon;
                lPrice24.PBallon = dBallon;
                lsPrice.Add(lPrice24);

                /*****Cálculo a 36 meses******/

                Datos.Plazo = "36";

                ResultPrice lPrice36 = new ResultPrice();
                lPrice36.Plazo = 36;
                CostoSeguro = GetSeguroAuto(Datos);
                if (CostoSeguro <= 0)
                {
                    msg += "GetPriceBalloon(); >> " + Datos.Vesion + " >> " + Datos.Marca + " >> Error al obtener Seguro del auto >> a 36 meses"; status = 400;
                }
                lPrice36.Enganche = lPrice.Enganche;
                lPrice36.DepositoGarantia = lPrice.DepositoGarantia;
                lPrice36.PrecioTotal = PrecioUnidad + CostoSeguro - lPrice36.Enganche;
                lPrice36.Marca = Datos.Marca;
                lPrice36.Modelo = Datos.Modelo;
                lPrice36.Vesion = Datos.Vesion;
                lPrice36.Anio = Datos.Anio;
                lPrice36.TipoPersona = Datos.TipoPersona;
                lPrice36.Plan = Datos.PlanCotizar;
                lPrice36.Estado = Datos.Estado;
                lPrice36.Aseguradora = Datos.Aseguradora;
                lPrice36.Cobertura = lPrice.Cobertura;
                lPrice36.PorcentajeComision = lPrice.PorcentajeComision;
                lPrice36.Comision = (lPrice36.PrecioTotal * lPrice.PorcentajeComision);
                lPrice36.CAT = lPrice.CAT;
                lPrice36.PagoMensual = 0;
                lPrice36.Anualidad = 0;
                //lPrice36.Mensualidad = aExcel.WorksheetFunction.Pmt(TasaFijaAnualConIva / 12, lPrice36.Plazo, -lPrice36.PrecioTotal, MontoBallon);
                lPrice36.Mensualidad = PMT(TasaFijaAnualConIva, lPrice36.Plazo, lPrice36.PrecioTotal) - VF(TasaFijaAnualConIva, lPrice36.Plazo, MontoBallon);
                lPrice36.Cobertura = Datos.Cobertura;
                lPrice36.Movil = Datos.Telefono;
                lPrice36.Nombre = Datos.Nombre;
                lPrice36.Apellido = Datos.Apellido;
                lPrice36.Ballon = MontoBallon;
                lPrice36.PBallon = dBallon;
                lsPrice.Add(lPrice36);

                /*****Cálculo a 48 meses******/

                Datos.Plazo = "48";

                ResultPrice lPrice48 = new ResultPrice();
                lPrice48.Plazo = 48;
                CostoSeguro = GetSeguroAuto(Datos);
                if (CostoSeguro <= 0)
                {
                    msg += "GetPriceBalloon(); >> " + Datos.Vesion + " >> " + Datos.Marca + " >> Error al obtener Seguro del auto >> a 48 meses"; status = 400;
                }
                lPrice48.Enganche = lPrice.Enganche;
                lPrice48.DepositoGarantia = lPrice.DepositoGarantia;
                lPrice48.PrecioTotal = PrecioUnidad + CostoSeguro - lPrice48.Enganche;
                lPrice48.Marca = Datos.Marca;
                lPrice48.Modelo = Datos.Modelo;
                lPrice48.Vesion = Datos.Vesion;
                lPrice48.Anio = Datos.Anio;
                lPrice48.TipoPersona = Datos.TipoPersona;
                lPrice48.Plan = Datos.PlanCotizar;
                lPrice48.Estado = Datos.Estado;
                lPrice48.Aseguradora = Datos.Aseguradora;
                lPrice48.Cobertura = lPrice.Cobertura;
                lPrice48.PorcentajeComision = lPrice.PorcentajeComision;
                lPrice48.Comision = (lPrice48.PrecioTotal * lPrice.PorcentajeComision);
                lPrice48.CAT = lPrice.CAT;
                lPrice48.PagoMensual = 0;
                lPrice48.Anualidad = 0;
                //lPrice48.Mensualidad = aExcel.WorksheetFunction.Pmt(TasaFijaAnualConIva / 12, lPrice48.Plazo, -lPrice48.PrecioTotal, MontoBallon);
                lPrice48.Mensualidad = PMT(TasaFijaAnualConIva, lPrice48.Plazo, lPrice48.PrecioTotal) - VF(TasaFijaAnualConIva, lPrice48.Plazo, MontoBallon);
                lPrice48.Cobertura = Datos.Cobertura;
                lPrice48.Movil = Datos.Telefono;
                lPrice48.Nombre = Datos.Nombre;
                lPrice48.Apellido = Datos.Apellido;
                lPrice48.Ballon = MontoBallon;
                lPrice48.PBallon = dBallon;
                lsPrice.Add(lPrice48);
            }
            catch (Exception ex)
            {
                msg += "GetPriceBalloon();  >> Error: " + Datos.Vesion + " >> " + Datos.Marca + " >> " + ex.Message + "  >>  " + ex.InnerException.ToString();
                status = 400;
                throw ex;
            }
            return lsPrice;
        }

        private List<ResultPrice> GetPriceArrendamientoFinanciero(DataPrice Datos, ref string msg, ref int status)
        {
            ResultPrice lPrice;
            List<ResultPrice> lsPrice = new List<ResultPrice>();

            double PrecioUnidad = GetPrecioAuto(Datos);
            double CostoSeguro = GetSeguroAuto(Datos);
            double TasaFijaAnualConIva = ((GetParametro("TasaFijaSinIVA") * 365) / 360) * (GetParametro("IVA") + 1);

            if (PrecioUnidad <= 0)
            {
                msg += "||GetPriceArrendamientoFinanciero(); >> " + Datos.Vesion + " >> " + Datos.Marca + " >> Error al obtener Precio del auto."; status = 400;
            }
            if (CostoSeguro <= 0)
            {
                msg += "||GetPriceArrendamientoFinanciero(); >> " + Datos.Vesion + " >> " + Datos.Marca + " >> Error al obtener el costo del seguro del auto."; status = 400;
            }
            if (TasaFijaAnualConIva <= 0)
            {
                msg += "||GetPriceArrendamientoFinanciero(); >> " + Datos.Vesion + " >> " + Datos.Marca + " >> Error al obtener parámetros TasaFijaSinIVA,IVA."; status = 400;
            }

            lPrice = new ResultPrice();

            try
            {
                lPrice.Enganche = 0;//double.Parse(Datos.EngancheDeposito);
                lPrice.DepositoGarantia = Datos.CantidadDepositosGarantia;
                lPrice.PrecioTotal = PrecioUnidad + CostoSeguro - lPrice.Enganche;
                lPrice.Marca = Datos.Marca;
                lPrice.Modelo = Datos.Modelo;
                lPrice.Vesion = Datos.Vesion;
                lPrice.Anio = Datos.Anio;
                lPrice.TipoPersona = Datos.TipoPersona;
                lPrice.Plan = Datos.PlanCotizar;
                lPrice.Estado = Datos.Estado;
                lPrice.Aseguradora = Datos.Aseguradora;
                lPrice.Cobertura = Datos.Cobertura;
                lPrice.PorcentajeComision = GetParametro("PorcentajeComision");
                if (lPrice.PorcentajeComision < 0)
                {
                    msg += "||GetPriceArrendamientoFinanciero(); >> " + Datos.Vesion + " >> " + Datos.Marca + " >> Error al obtener parámetros PorcentajeComision"; status = 400;
                }

                lPrice.Comision = (lPrice.PrecioTotal * lPrice.PorcentajeComision);
                lPrice.CAT = GetParametro("CAT");
                lPrice.PagoMensual = 0;
                lPrice.Anualidad = 0;

                /*****Cálculo a 24 meses******/

                Datos.Plazo = "24";

                ResultPrice lPrice24 = new ResultPrice();
                lPrice24.Plazo = 24;

                CostoSeguro = GetSeguroAuto(Datos);
                if (CostoSeguro <= 0)
                {
                    msg += "|| GetPriceArrendamientoFinanciero(); >> " + Datos.Vesion + " >> " + Datos.Marca + " >> Error al obtener Seguro del auto >> a 24 meses"; status = 400;
                }

                lPrice24.Enganche = 0;
                lPrice24.DepositoGarantia = lPrice.DepositoGarantia;
                lPrice24.PrecioTotal = PrecioUnidad + CostoSeguro - lPrice24.Enganche;
                lPrice24.Marca = Datos.Marca;
                lPrice24.Modelo = Datos.Modelo;
                lPrice24.Vesion = Datos.Vesion;
                lPrice24.Anio = Datos.Anio;
                lPrice24.TipoPersona = Datos.TipoPersona;
                lPrice24.Plan = Datos.PlanCotizar;
                lPrice24.Estado = Datos.Estado;
                lPrice24.Aseguradora = Datos.Aseguradora;
                lPrice24.Cobertura = lPrice.Cobertura;
                lPrice24.PorcentajeComision = lPrice.PorcentajeComision;
                lPrice24.Comision = (lPrice24.PrecioTotal * lPrice24.PorcentajeComision);
                lPrice24.CAT = lPrice.CAT;
                lPrice24.PagoMensual = 0;
                lPrice24.Anualidad = 0;
                lPrice24.Mensualidad = PMT(TasaFijaAnualConIva, lPrice24.Plazo, lPrice24.PrecioTotal);
                lPrice24.Cobertura = Datos.Cobertura;
                lPrice24.Movil = Datos.Telefono;
                lPrice24.Nombre = Datos.Nombre;
                lPrice24.Apellido = Datos.Apellido;
                lPrice24.Enganche = lPrice24.DepositoGarantia * lPrice24.Mensualidad;

                lsPrice.Add(lPrice24);

                /*****Cálculo a 36 meses******/

                Datos.Plazo = "36";

                ResultPrice lPrice36 = new ResultPrice();
                lPrice36.Plazo = 36;

                CostoSeguro = GetSeguroAuto(Datos);
                if (CostoSeguro <= 0)
                {
                    msg += "|| GetPriceArrendamientoFinanciero(); >> " + Datos.Vesion + " >> " + Datos.Marca + " >> Error al obtener Seguro del auto >> a 36 meses"; status = 400;
                }

                lPrice36.Enganche = 0;
                lPrice36.DepositoGarantia = lPrice.DepositoGarantia;
                lPrice36.PrecioTotal = PrecioUnidad + CostoSeguro - lPrice36.Enganche;
                lPrice36.Marca = Datos.Marca;
                lPrice36.Modelo = Datos.Modelo;
                lPrice36.Vesion = Datos.Vesion;
                lPrice36.Anio = Datos.Anio;
                lPrice36.TipoPersona = Datos.TipoPersona;
                lPrice36.Plan = Datos.PlanCotizar;
                lPrice36.Estado = Datos.Estado;
                lPrice36.Aseguradora = Datos.Aseguradora;
                lPrice36.Cobertura = lPrice.Cobertura;
                lPrice36.PorcentajeComision = lPrice.PorcentajeComision;
                lPrice36.Comision = (lPrice36.PrecioTotal * lPrice36.PorcentajeComision);
                lPrice36.CAT = lPrice.CAT;
                lPrice36.PagoMensual = 0;
                lPrice36.Anualidad = 0;
                lPrice36.Mensualidad = PMT(TasaFijaAnualConIva, lPrice36.Plazo, lPrice36.PrecioTotal);
                lPrice36.Cobertura = Datos.Cobertura;
                lPrice36.Movil = Datos.Telefono;
                lPrice36.Nombre = Datos.Nombre;
                lPrice36.Apellido = Datos.Apellido;
                lPrice36.Enganche = lPrice36.DepositoGarantia * lPrice36.Mensualidad;

                lsPrice.Add(lPrice36);

                /*****Cálculo a 48 meses******/

                Datos.Plazo = "48";

                ResultPrice lPrice48 = new ResultPrice();
                lPrice48.Plazo = 48;

                CostoSeguro = GetSeguroAuto(Datos);
                if (CostoSeguro <= 0)
                {
                    msg += "|| GetPriceArrendamientoFinanciero(); >> " + Datos.Vesion + " >> " + Datos.Marca + " >> Error al obtener Seguro del auto >> a 48 meses"; status = 400;
                }

                lPrice48.Enganche = 0;
                lPrice48.DepositoGarantia = lPrice.DepositoGarantia;
                lPrice48.PrecioTotal = PrecioUnidad + CostoSeguro - lPrice48.Enganche;
                lPrice48.Marca = Datos.Marca;
                lPrice48.Modelo = Datos.Modelo;
                lPrice48.Vesion = Datos.Vesion;
                lPrice48.Anio = Datos.Anio;
                lPrice48.TipoPersona = Datos.TipoPersona;
                lPrice48.Plan = Datos.PlanCotizar;
                lPrice48.Estado = Datos.Estado;
                lPrice48.Aseguradora = Datos.Aseguradora;
                lPrice48.Cobertura = lPrice.Cobertura;
                lPrice48.PorcentajeComision = lPrice.PorcentajeComision;
                lPrice48.Comision = (lPrice48.PrecioTotal * lPrice48.PorcentajeComision);
                lPrice48.CAT = lPrice.CAT;
                lPrice48.PagoMensual = 0;
                lPrice48.Anualidad = 0;
                lPrice48.Mensualidad = PMT(TasaFijaAnualConIva, lPrice48.Plazo, lPrice48.PrecioTotal);
                lPrice48.Cobertura = Datos.Cobertura;
                lPrice48.Movil = Datos.Telefono;
                lPrice48.Nombre = Datos.Nombre;
                lPrice48.Apellido = Datos.Apellido;
                lPrice48.Enganche = lPrice48.DepositoGarantia * lPrice48.Mensualidad;

                lsPrice.Add(lPrice48);

            }
            catch (Exception ex)
            {
                msg += "GetPriceArrendamientoFinanciero(); >> Error: " + ex.Message + " >> " + ex.InnerException.ToString();
                throw ex;
            }

            return lsPrice;
        }

        private List<ResultPrice> GetPriceAnualidades(DataPrice Datos, ref string msg, ref int status)
        {
            ResultPrice lPrice;
            List<ResultPrice> lsPrice = new List<ResultPrice>();

            double PrecioUnidad = GetPrecioAuto(Datos);
            double CostoSeguro = GetSeguroAuto(Datos);
            double TasaFijaAnualConIva = ((GetParametro("TasaFijaSinIVA") * 365) / 360) * (GetParametro("IVA") + 1);

            if (PrecioUnidad <= 0)
            {
                msg = "||GetPriceAnualidades(); >> Error al obtener el precio del auto >> " + Datos.Marca + " " + Datos.Modelo + " >> "; status = 400;
            }
            if (CostoSeguro <= 0)
            {
                msg += "||GetPriceAnualidades(); >> Error al obtener el costo del seguro del auto >> " + Datos.Marca + " " + Datos.Modelo + " >> "; status = 400;
            }
            if (TasaFijaAnualConIva <= 0)
            {
                msg += "||GetPriceAnualidades(); >> Error al obtener parámetros TasaFijaSinIVAAnu,IVA del auto >> " + Datos.Marca + " " + Datos.Modelo + " >> "; status = 400;
            }

            int i, plazo;

            lPrice = new ResultPrice();

            try
            {

                lPrice.Enganche = double.Parse(Datos.EngancheDeposito);
                lPrice.DepositoGarantia = double.Parse(Datos.EngancheDeposito);
                lPrice.PrecioTotal = PrecioUnidad + CostoSeguro - lPrice.Enganche;
                lPrice.Marca = Datos.Marca;
                lPrice.Modelo = Datos.Modelo;
                lPrice.Vesion = Datos.Vesion;
                lPrice.Anio = Datos.Anio;
                lPrice.TipoPersona = Datos.TipoPersona;
                lPrice.Plan = Datos.PlanCotizar;
                lPrice.Estado = Datos.Estado;
                lPrice.Aseguradora = Datos.Aseguradora;
                lPrice.Cobertura = Datos.Cobertura;
                lPrice.PorcentajeComision = GetParametro("PorcentajeComision");
                lPrice.Comision = (lPrice.PrecioTotal * lPrice.PorcentajeComision);
                lPrice.CAT = GetParametro("CAT");
                lPrice.PagoMensual = 0;
                lPrice.Anualidad = 0;

                if (lPrice.PorcentajeComision < 0)
                {
                    msg += "||GetPriceAnualidades(); >> Error al obtener parámetros TasaFijaSinIVAAnu,IVA del auto >> " + Datos.Marca + " " + Datos.Modelo + " >> "; status = 400;
                }
                if (lPrice.CAT <= 0)
                {
                    msg += "||GetPriceAnualidades(); >> Error al obtener parámetros CAT del auto >> " + Datos.Marca + " " + Datos.Modelo + " >> "; status = 400;
                }

                //const int MesDeAnualidad = 12;
                DateTime PrimerVencimiento;
                DateTime PrimerAnualidad;
                //double TasaEfectiva = Math.Round(GetParametro("TasaFijaSinIVA") * 365 / 360 * 30 / 360, 12);
                double TasaPeriodoAnualIVA = Math.Round(TasaFijaAnualConIva * 365 / 360 * 30 / 360, 12);
                //double TasaEfectivaAnualIVA = Math.Round(TasaFijaAnualConIva * 365 / 360, 12);
                double SumaPagosAnuales;

                if (DateTime.Now.Day < 16)
                {
                    if (DateTime.Now.Month < 12)
                        PrimerVencimiento = DateTime.Parse(DateTime.Now.Year.ToString() + "/" + DateTime.Now.AddMonths(1).Month.ToString() + "/16");
                    else
                        PrimerVencimiento = DateTime.Parse(DateTime.Now.AddYears(1).Year.ToString() + "/" + DateTime.Now.AddMonths(1).Month.ToString() + "/16");
                }
                else
                {
                    if (DateTime.Now.Month < 12)
                        PrimerVencimiento = DateTime.Parse(DateTime.Now.Year.ToString() + "/" + DateTime.Now.AddMonths(1).Month.ToString() + "/01");
                    else
                        PrimerVencimiento = DateTime.Parse(DateTime.Now.AddYears(1).Year.ToString() + "/" + DateTime.Now.AddMonths(1).Month.ToString() + "/01");
                }

                double TasaEfectivaPeriodoIVA = Math.Round(((((GetParametro("TasaFijaSinIVA") * 365) / 360) * (GetParametro("IVA") + 1)) * 30) / 360, 12);
                //double TasaPeriodoAnualIVA = Math.Round(TasaFijaAnualConIva * 365 / 360 * 30 / 360, 12);
                //double TasaEfectivaAnualIVA = Math.Round(TasaFijaAnualConIva * 365 / 360, 12);

                //if (PrimerVencimiento.Month > MesDeAnualidad)
                //    PrimerAnualidad = DateTime.Parse(PrimerVencimiento.AddYears(1).Year.ToString() + "/" + MesDeAnualidad.ToString() + "/" + PrimerVencimiento.Day.ToString());
                //else
                //    PrimerAnualidad = DateTime.Parse(PrimerVencimiento.Year.ToString() + "/" + MesDeAnualidad.ToString() + "/" + PrimerVencimiento.Day.ToString());
                /*
                PrimerAnualidad = DateTime.Now.AddYears(1);
                if (DateTime.Now.Day < 16)
                {
                    PrimerAnualidad = DateTime.Parse(PrimerAnualidad.Year.ToString() + "/" + PrimerAnualidad.Month.ToString() + "/16");
                }
                else
                {
                    //PrimerAnualidad = DateTime.Parse(PrimerAnualidad.Year.ToString() + "/" + PrimerAnualidad.Month.ToString() + "/01");
                    PrimerAnualidad = DateTime.Parse(PrimerAnualidad.Year.ToString() + "/" + (PrimerAnualidad.Month+1).ToString() + "/01");
                }

                switch (PrimerAnualidad.Month)
                {
                    case 1:
                        lPrice.MesAnualidad = "Enero";
                        break;
                    case 2:
                        lPrice.MesAnualidad = "Febrero";
                        break;
                    case 3:
                        lPrice.MesAnualidad = "Marzo";
                        break;
                    case 4:
                        lPrice.MesAnualidad = "Abril";
                        break;
                    case 5:
                        lPrice.MesAnualidad = "Mayo";
                        break;
                    case 6:
                        lPrice.MesAnualidad = "Junio";
                        break;
                    case 7:
                        lPrice.MesAnualidad = "Julio";
                        break;
                    case 8:
                        lPrice.MesAnualidad = "Agosto";
                        break;
                    case 9:
                        lPrice.MesAnualidad = "Septiembre";
                        break;
                    case 10:
                        lPrice.MesAnualidad = "Octubre";
                        break;
                    case 11:
                        lPrice.MesAnualidad = "Noviembre";
                        break;
                    case 12:
                        lPrice.MesAnualidad = "Diciembre";
                        break;
                    default:
                        lPrice.MesAnualidad = "N/A";
                        break;
                }
                */
                #region calculosAnualidades
                DateTime fechaVencimiento;
                DateTime hoy = DateTime.Now;



                int mesVencimiento = hoy.Month + (hoy.Day > 15 ? 2 : 1);
                int dia = hoy.Day <= 15 ? 16 : 1;
                int anio = hoy.Year;

                if (mesVencimiento > 12)
                {
                    anio += 1;
                    mesVencimiento -= 12;
                }

                fechaVencimiento = DateTime.Parse(anio.ToString() + "/" + mesVencimiento.ToString() + "/" + dia.ToString());
                // --- SE COMENTAN LAS LINEAS PARA ARREGLAR COMPARACION CON COTIZADOR 5 DE ANUALIDADES --- se comentan estas 2 lineas el 19-04-2024 
                //int mesAnualidad = hoy.Month;
                //int anioAnualidad = anio + (mesVencimiento >= mesAnualidad ? 1 : 0); 

                int mesAnualidad = 12; // SE LE DEJA EL MES 12 PORQUE EL COTIZADOR SOLO COTIZA EN EL MES DE DICIEMBRE PARA ANUALIDADES
                int anioAnualidad = anio ;
                DateTime fechaAnualidad = DateTime.Parse(anioAnualidad.ToString() + "/" + mesAnualidad.ToString() + "/" + dia.ToString());
                int mesPrimeraAnualidad = (fechaAnualidad.Year - fechaVencimiento.Year) * 12 + fechaAnualidad.Month - fechaVencimiento.Month + 1;


                switch (fechaAnualidad.Month)
                {
                    case 1:
                        lPrice.MesAnualidad = "Enero";
                        break;
                    case 2:
                        lPrice.MesAnualidad = "Febrero";
                        break;
                    case 3:
                        lPrice.MesAnualidad = "Marzo";
                        break;
                    case 4:
                        lPrice.MesAnualidad = "Abril";
                        break;
                    case 5:
                        lPrice.MesAnualidad = "Mayo";
                        break;
                    case 6:
                        lPrice.MesAnualidad = "Junio";
                        break;
                    case 7:
                        lPrice.MesAnualidad = "Julio";
                        break;
                    case 8:
                        lPrice.MesAnualidad = "Agosto";
                        break;
                    case 9:
                        lPrice.MesAnualidad = "Septiembre";
                        break;
                    case 10:
                        lPrice.MesAnualidad = "Octubre";
                        break;
                    case 11:
                        lPrice.MesAnualidad = "Noviembre";
                        break;
                    case 12:
                        lPrice.MesAnualidad = "Diciembre";
                        break;
                    default:
                        lPrice.MesAnualidad = "N/A";
                        break;
                }

                //int iContador = 0;
                //int[] NumPago = new int[10];
                //double[] MontoPago = new double[10];
                //NumPago[iContador] = (PrimerAnualidad.Year - PrimerVencimiento.Year) * 12 + (PrimerAnualidad.Month - PrimerVencimiento.Month) + 1;

                /*****Cálculo a 24 meses******/

                Datos.Plazo = "24";
                SumaPagosAnuales = 0;

                plazo = 12 - (DateTime.Now.Month + 1);
                //plazo = ((PrimerAnualidad.Year - DateTime.Now.Year) * 12 + (-1))+1;
                plazo = mesPrimeraAnualidad; 

                ResultPrice lPrice24 = new ResultPrice();
                lPrice24.Plazo = 24;
                CostoSeguro = GetSeguroAuto(Datos);

                lPrice24.Enganche = lPrice.Enganche;
                lPrice24.DepositoGarantia = lPrice.DepositoGarantia;
                lPrice24.PrecioTotal = PrecioUnidad + CostoSeguro - lPrice24.Enganche;
                lPrice24.Marca = Datos.Marca;
                lPrice24.Modelo = Datos.Modelo;
                lPrice24.Vesion = Datos.Vesion;
                lPrice24.Anio = Datos.Anio;
                lPrice24.TipoPersona = Datos.TipoPersona;
                lPrice24.Plan = Datos.PlanCotizar;
                lPrice24.Estado = Datos.Estado;
                lPrice24.Aseguradora = Datos.Aseguradora;
                lPrice24.Cobertura = lPrice.Cobertura;
                lPrice24.PorcentajeComision = lPrice.PorcentajeComision;
                lPrice24.Comision = (lPrice24.PrecioTotal * lPrice24.PorcentajeComision);
                lPrice24.CAT = lPrice.CAT;
                lPrice24.PagoMensual = 0;
                lPrice24.Cobertura = Datos.Cobertura;
                lPrice24.Movil = Datos.Telefono;
                lPrice24.Nombre = Datos.Nombre;
                lPrice24.Apellido = Datos.Apellido;
                lPrice24.Anualidad = lPrice24.PrecioTotal * GetAnualidad(lPrice24.Plazo) / 100;
                lPrice24.MesAnualidad = lPrice.MesAnualidad;

                for (i = 0; i < (lPrice24.Plazo / 12); i++)
                {
                    if (plazo <= lPrice24.Plazo)
                    {
                        SumaPagosAnuales += (lPrice24.Anualidad / Math.Pow((1 + TasaEfectivaPeriodoIVA), plazo));
                        plazo += 12;
                    }
                }

                /*
                    iContador = 0;
                SumaPagosAnuales = 0;
                lPrice.Anualidad = lPrice.PrecioTotal * GetAnualidad(lPrice24.Plazo) / 100;
                if (NumPago[iContador] > 0)
                {
                    MontoPago[iContador] = lPrice.Anualidad / Math.Pow(1 + TasaEfectivaPeriodoIVA, NumPago[iContador]);
                    SumaPagosAnuales = MontoPago[iContador];
                    while ((NumPago[iContador] + 12) < lPrice24.Plazo)
                    {
                        iContador++;
                        NumPago[iContador] = NumPago[iContador - 1] + 12;
                        MontoPago[iContador] = lPrice.Anualidad / Math.Pow(1 + TasaEfectivaPeriodoIVA, NumPago[iContador]);
                        SumaPagosAnuales = MontoPago[iContador] + SumaPagosAnuales;
                    }
                }
                */

                //lPrice24.Anualidad = SumaPagosAnuales;
                lPrice24.Mensualidad = (lPrice24.PrecioTotal - SumaPagosAnuales) / ((1 / TasaEfectivaPeriodoIVA) - (1 / (TasaEfectivaPeriodoIVA * Math.Pow(TasaEfectivaPeriodoIVA + 1, lPrice24.Plazo))));

                lsPrice.Add(lPrice24);

                /*****Cálculo a 36 meses******/

                Datos.Plazo = "36";
                SumaPagosAnuales = 0;

                plazo = 12 - (DateTime.Now.Month + 1);
                plazo = mesPrimeraAnualidad;


                ResultPrice lPrice36 = new ResultPrice();
                lPrice36.Plazo = 36;
                CostoSeguro = GetSeguroAuto(Datos);

                lPrice36.Enganche = lPrice.Enganche;
                lPrice36.DepositoGarantia = lPrice.DepositoGarantia;
                lPrice36.PrecioTotal = PrecioUnidad + CostoSeguro - lPrice36.Enganche;
                lPrice36.Marca = Datos.Marca;
                lPrice36.Modelo = Datos.Modelo;
                lPrice36.Vesion = Datos.Vesion;
                lPrice36.Anio = Datos.Anio;
                lPrice36.TipoPersona = Datos.TipoPersona;
                lPrice36.Plan = Datos.PlanCotizar;
                lPrice36.Estado = Datos.Estado;
                lPrice36.Aseguradora = Datos.Aseguradora;
                lPrice36.Cobertura = lPrice.Cobertura;
                lPrice36.PorcentajeComision = lPrice.PorcentajeComision;
                lPrice36.Comision = (lPrice36.PrecioTotal * lPrice36.PorcentajeComision);
                lPrice36.CAT = lPrice.CAT;
                lPrice36.PagoMensual = 0;
                lPrice36.Cobertura = Datos.Cobertura;
                lPrice36.Movil = Datos.Telefono;
                lPrice36.Nombre = Datos.Nombre;
                lPrice36.Apellido = Datos.Apellido;
                lPrice36.Anualidad = lPrice36.PrecioTotal * GetAnualidad(lPrice36.Plazo) / 100;
                lPrice36.MesAnualidad = lPrice.MesAnualidad;

                for (i = 0; i < (lPrice36.Plazo / 12); i++)
                {
                    if (plazo <= lPrice36.Plazo)
                    {
                        SumaPagosAnuales = SumaPagosAnuales + (lPrice36.Anualidad / Math.Pow((1 + TasaEfectivaPeriodoIVA), plazo));
                        plazo = plazo + 12;
                    }
                }

                lPrice36.Mensualidad = (lPrice36.PrecioTotal - SumaPagosAnuales) / ((1 / TasaEfectivaPeriodoIVA) - (1 / (TasaEfectivaPeriodoIVA * Math.Pow(TasaEfectivaPeriodoIVA + 1, lPrice36.Plazo))));

                lsPrice.Add(lPrice36);

                /*****Cálculo a 48 meses******/

                Datos.Plazo = "48";
                SumaPagosAnuales = 0;
                plazo = 12 - (DateTime.Now.Month + 1);
                plazo = mesPrimeraAnualidad; 


                ResultPrice lPrice48 = new ResultPrice();
                lPrice48.Plazo = 48;
                CostoSeguro = GetSeguroAuto(Datos);

                lPrice48.Enganche = lPrice.Enganche;
                lPrice48.DepositoGarantia = lPrice.DepositoGarantia;
                lPrice48.PrecioTotal = PrecioUnidad + CostoSeguro - lPrice48.Enganche;
                lPrice48.Marca = Datos.Marca;
                lPrice48.Modelo = Datos.Modelo;
                lPrice48.Vesion = Datos.Vesion;
                lPrice48.Anio = Datos.Anio;
                lPrice48.TipoPersona = Datos.TipoPersona;
                lPrice48.Plan = Datos.PlanCotizar;
                lPrice48.Estado = Datos.Estado;
                lPrice48.Aseguradora = Datos.Aseguradora;
                lPrice48.Cobertura = lPrice.Cobertura;
                lPrice48.PorcentajeComision = lPrice.PorcentajeComision;
                lPrice48.Comision = (lPrice48.PrecioTotal * lPrice48.PorcentajeComision);
                lPrice48.CAT = lPrice.CAT;
                lPrice48.PagoMensual = 0;
                lPrice48.Cobertura = Datos.Cobertura;
                lPrice48.Movil = Datos.Telefono;
                lPrice48.Nombre = Datos.Nombre;
                lPrice48.Apellido = Datos.Apellido;
                lPrice48.Anualidad = lPrice48.PrecioTotal * GetAnualidad(lPrice48.Plazo) / 100;
                lPrice48.MesAnualidad = lPrice.MesAnualidad;

                for (i = 0; i < (lPrice48.Plazo / 12); i++)
                {
                    if (plazo <= lPrice48.Plazo)
                    {
                        SumaPagosAnuales = SumaPagosAnuales + (lPrice48.Anualidad / Math.Pow((1 + TasaEfectivaPeriodoIVA), plazo));
                        plazo = plazo + 12;
                    }
                }

                lPrice48.Mensualidad = (lPrice48.PrecioTotal - SumaPagosAnuales) / ((1 / TasaEfectivaPeriodoIVA) - (1 / (TasaEfectivaPeriodoIVA * Math.Pow(TasaEfectivaPeriodoIVA + 1, lPrice48.Plazo))));

                lsPrice.Add(lPrice48);
                #endregion
            }

            catch (Exception ex)
            {
                msg += "||GetPriceAnualidades(); >> Error: " + ex.Message + " >> " + ex.InnerException.ToString();
                status = 400;
                throw ex;
            }
            return lsPrice;

        }
        private List<ResultPrice> GetPriceArrendamientoPuro(DataPrice Datos, ref string msg, ref int status)
        {
            ResultPrice lPrice;
            List<ResultPrice> lsPrice = new List<ResultPrice>();

            //Application aExcel = new Application();
            double PrecioUnidad = GetPrecioAuto(Datos);
            double CostoSeguro = GetSeguroAuto(Datos);
            double TasaFijaAnualConIva = ((GetParametro("TasaFijaSinIVA") * 365) / 360) * (GetParametro("IVA") + 1);
            double TasaPura = (GetParametro("TasaFijaSinIVA") / 360) * 30.4166;
            double PorcentajeValorResidual;
            double ValorResidual;

            if (PrecioUnidad <= 0)
            {
                msg += "GetPriceArrendamientoPuro(); Error al obtener el precio del auto: " + Datos.Marca + " >> " + Datos.Modelo;
            }
            if (CostoSeguro <= 0)
            {
                msg += "GetPriceArrendamientoPuro(); Error al obtener el costo del seguro del auto: " + Datos.Marca + " >> " + Datos.Vesion;
            }
            if (TasaFijaAnualConIva <= 0)
            {
                msg += "GetPriceArrendamientoPuro(); Error al obtener el parametro TasaFijaSinIVA,IVA del auto: " + Datos.Marca + " >> " + Datos.Vesion;
            }

            lPrice = new ResultPrice();

            try
            {
                lPrice.Enganche = 0;// double.Parse(Datos.EngancheDeposito);
                lPrice.DepositoGarantia = Datos.CantidadDepositosGarantia;
                lPrice.PrecioTotal = (PrecioUnidad / (GetParametro("IVA") + 1)) + CostoSeguro - lPrice.Enganche;
                lPrice.Marca = Datos.Marca;
                lPrice.Modelo = Datos.Modelo;
                lPrice.Vesion = Datos.Vesion;
                lPrice.Anio = Datos.Anio;
                lPrice.TipoPersona = Datos.TipoPersona;
                lPrice.Plan = Datos.PlanCotizar;
                lPrice.Estado = Datos.Estado;
                lPrice.Aseguradora = Datos.Aseguradora;
                lPrice.Cobertura = Datos.Cobertura;
                lPrice.PorcentajeComision = GetParametro("PorcentajeComision");
                if (lPrice.PorcentajeComision < 0)
                {
                    msg += "||GetPriceArrendamientoPuro(); >> " + Datos.Vesion + " >> " + Datos.Marca + " >> Error al obtener parámetros PorcentajeComision"; status = 400;
                }

                lPrice.Comision = (lPrice.PrecioTotal * lPrice.PorcentajeComision);
                lPrice.CAT = GetParametro("CAT");
                lPrice.PagoMensual = 0;
                lPrice.Anualidad = 0;

                /*****Cálculo a 24 meses******/

                Datos.Plazo = "24";

                PorcentajeValorResidual = 0.40;

                ResultPrice lPrice24 = new ResultPrice();
                lPrice24.Plazo = 24;

                CostoSeguro = GetSeguroAuto(Datos);
                if (CostoSeguro <= 0)
                {
                    msg += "|| GetPriceArrendamientoPuro(); >> " + Datos.Vesion + " >> " + Datos.Marca + " >> Error al obtener Seguro del auto >> a 24 meses"; status = 400;
                }

                lPrice24.Enganche = 0;
                lPrice24.DepositoGarantia = lPrice.DepositoGarantia;
                lPrice24.PrecioTotal = (PrecioUnidad / (GetParametro("IVA") + 1)) + CostoSeguro - lPrice24.Enganche;
                lPrice24.Marca = Datos.Marca;
                lPrice24.Modelo = Datos.Modelo;
                lPrice24.Vesion = Datos.Vesion;
                lPrice24.Anio = Datos.Anio;
                lPrice24.TipoPersona = Datos.TipoPersona;
                lPrice24.Plan = Datos.PlanCotizar;
                lPrice24.Estado = Datos.Estado;
                lPrice24.Aseguradora = Datos.Aseguradora;
                lPrice24.Cobertura = lPrice.Cobertura;
                lPrice24.PorcentajeComision = lPrice.PorcentajeComision;
                lPrice24.Comision = (lPrice24.PrecioTotal * lPrice24.PorcentajeComision);
                lPrice24.CAT = lPrice.CAT;
                lPrice24.PagoMensual = 0;
                lPrice24.Anualidad = 0;
                ValorResidual = (PrecioUnidad / (GetParametro("IVA") + 1)) * PorcentajeValorResidual;
                //lPrice24.Mensualidad = aExcel.WorksheetFunction.Pmt(TasaPura, lPrice24.Plazo, -lPrice24.PrecioTotal, ValorResidual);
                lPrice24.Mensualidad = PMT(TasaPura * 12, lPrice24.Plazo, lPrice24.PrecioTotal) - VF(TasaPura * 12, lPrice24.Plazo, ValorResidual);
                lPrice24.Cobertura = Datos.Cobertura;
                lPrice24.Movil = Datos.Telefono;
                lPrice24.Nombre = Datos.Nombre;
                lPrice24.Apellido = Datos.Apellido;
                lPrice24.Enganche = lPrice24.DepositoGarantia * lPrice24.Mensualidad * (GetParametro("IVA") + 1);

                lsPrice.Add(lPrice24);

                /*****Cálculo a 36 meses******/

                Datos.Plazo = "36";

                PorcentajeValorResidual = 0.35;

                ResultPrice lPrice36 = new ResultPrice();
                lPrice36.Plazo = 36;

                CostoSeguro = GetSeguroAuto(Datos);
                if (CostoSeguro <= 0)
                {
                    msg += "|| GetPriceArrendamientoPuro(); >> " + Datos.Vesion + " >> " + Datos.Marca + " >> Error al obtener Seguro del auto >> a 36 meses"; status = 400;
                }

                lPrice36.Enganche = 0;
                lPrice36.DepositoGarantia = lPrice.DepositoGarantia;
                lPrice36.PrecioTotal = (PrecioUnidad / (GetParametro("IVA") + 1)) + CostoSeguro - lPrice36.Enganche;
                lPrice36.Marca = Datos.Marca;
                lPrice36.Modelo = Datos.Modelo;
                lPrice36.Vesion = Datos.Vesion;
                lPrice36.Anio = Datos.Anio;
                lPrice36.TipoPersona = Datos.TipoPersona;
                lPrice36.Plan = Datos.PlanCotizar;
                lPrice36.Estado = Datos.Estado;
                lPrice36.Aseguradora = Datos.Aseguradora;
                lPrice36.Cobertura = lPrice.Cobertura;
                lPrice36.PorcentajeComision = lPrice.PorcentajeComision;
                lPrice36.Comision = (lPrice36.PrecioTotal * lPrice36.PorcentajeComision);
                lPrice36.CAT = lPrice.CAT;
                lPrice36.PagoMensual = 0;
                lPrice36.Anualidad = 0;
                ValorResidual = (PrecioUnidad / (GetParametro("IVA") + 1)) * PorcentajeValorResidual;
                //lPrice36.Mensualidad = aExcel.WorksheetFunction.Pmt(TasaPura, lPrice36.Plazo, -lPrice36.PrecioTotal, ValorResidual);
                lPrice36.Mensualidad = PMT(TasaPura * 12, lPrice36.Plazo, lPrice36.PrecioTotal) - VF(TasaPura * 12, lPrice36.Plazo, ValorResidual);
                lPrice36.Cobertura = Datos.Cobertura;
                lPrice36.Movil = Datos.Telefono;
                lPrice36.Nombre = Datos.Nombre;
                lPrice36.Apellido = Datos.Apellido;
                lPrice36.Enganche = lPrice36.DepositoGarantia * lPrice36.Mensualidad * (GetParametro("IVA") + 1);

                lsPrice.Add(lPrice36);

                /*****Cálculo a 48 meses******/

                Datos.Plazo = "48";

                PorcentajeValorResidual = 0.25;

                ResultPrice lPrice48 = new ResultPrice();
                lPrice48.Plazo = 48;

                CostoSeguro = GetSeguroAuto(Datos);
                if (CostoSeguro <= 0)
                {
                    msg += "|| GetPriceArrendamientoPuro(); >> " + Datos.Vesion + " >> " + Datos.Marca + " >> Error al obtener Seguro del auto >> a 48 meses"; status = 400;
                }

                lPrice48.Enganche = 0;
                lPrice48.DepositoGarantia = lPrice.DepositoGarantia;
                lPrice48.PrecioTotal = (PrecioUnidad / (GetParametro("IVA") + 1)) + CostoSeguro - lPrice48.Enganche;
                lPrice48.Marca = Datos.Marca;
                lPrice48.Modelo = Datos.Modelo;
                lPrice48.Vesion = Datos.Vesion;
                lPrice48.Anio = Datos.Anio;
                lPrice48.TipoPersona = Datos.TipoPersona;
                lPrice48.Plan = Datos.PlanCotizar;
                lPrice48.Estado = Datos.Estado;
                lPrice48.Aseguradora = Datos.Aseguradora;
                lPrice48.Cobertura = lPrice.Cobertura;
                lPrice48.PorcentajeComision = lPrice.PorcentajeComision;
                lPrice48.Comision = (lPrice48.PrecioTotal * lPrice48.PorcentajeComision);
                lPrice48.CAT = lPrice.CAT;
                lPrice48.PagoMensual = 0;
                lPrice48.Anualidad = 0;
                ValorResidual = (PrecioUnidad / (GetParametro("IVA") + 1)) * PorcentajeValorResidual;
                //lPrice48.Mensualidad = aExcel.WorksheetFunction.Pmt(TasaPura, lPrice48.Plazo, -lPrice48.PrecioTotal, ValorResidual);
                lPrice48.Mensualidad = PMT(TasaPura * 12, lPrice48.Plazo, lPrice48.PrecioTotal) - VF(TasaPura * 12, lPrice48.Plazo, ValorResidual);
                lPrice48.Cobertura = Datos.Cobertura;
                lPrice48.Movil = Datos.Telefono;
                lPrice48.Nombre = Datos.Nombre;
                lPrice48.Apellido = Datos.Apellido;
                lPrice48.Enganche = lPrice48.DepositoGarantia * lPrice48.Mensualidad * (GetParametro("IVA") + 1);

                lsPrice.Add(lPrice48);

            }
            catch (Exception ex)
            {
                msg += "GetPriceArrendamientoPuro(); >> " + ex.Message + "  >> " + ex.InnerException.ToString();
                status = 400;
                throw ex;
            }
            return lsPrice;
        }
    }
}