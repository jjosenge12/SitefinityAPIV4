using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DXC.TFSM.Business.Model
{
    public class ResultPrice
    {
        public double Mensualidad { get; set; }
        public double PrecioTotal { get; set; }
        public double PagoMensual { get; set; }
        public double Enganche { get; set; }

        public string Marca { get; set; }
        public string Modelo { get; set; }
        public string Vesion { get; set; }
        public string Anio { get; set; }
        public string TipoPersona { get; set; }
        public string Estado { get; set; }
        public string Aseguradora { get; set; }
        public string Cobertura { get; set; }
        public double CAT { get; set; }

        public string Plan { get; set; }
        public double Ballon { get; set; }

        public double PBallon { get; set; }

        public string Movil { get; set; }
        public string Email { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }


        public double Comision { get; set; }
        public double PorcentajeComision { get; set; }

        public Int16 Plazo { get; set; }
        public double Anualidad { get; set; }

        public double DepositoGarantia { get; set; }

        public string MesAnualidad { get; set; }
        
    }

    public class DataSalesForce{
        public string Mensualidad { get; set; }
        public string PrecioTotal { get; set; }
        public string PagoMensual { get; set; }
        public string Enganche { get; set; }

        public string Marca { get; set; }
        public string Modelo { get; set; }
        public string Vesion { get; set; }
        public string TipoPersona { get; set; }
        public string Estado { get; set; }
        public string Aseguradora { get; set; }
        public string Cobertura { get; set; }
        public string CAT { get; set; }

        public string Plan { get; set; }
        public string Ballon { get; set; }

        public string Movil { get; set; }
        public string Email { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }


        public string Comision { get; set; }
        public string PorcentajeComision { get; set; }

        public string Plazo { get; set; }
        public string Anualidad { get; set; }

        public string DepositoGarantia { get; set; }

        public string AceptoTerminosYCondiciones { get; set; }
        public string CodigoDistribuidor { get; set; }
        public string EstadoSeleccionado { get; set; }
        public string Precio { get; set; }
        public string ImagenAuto { get; set; }
        

    }

    public class ResponseProcess {
        public int Status { get; set; }
        public string Message { get; set; }
        public List<ResultPrice> Prices { get; set; }
    }
}
