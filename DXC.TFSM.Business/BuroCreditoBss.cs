using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using DXC.TFSM.DataAccess;

namespace DXC.TFSM.Business
{
    public class BuroCreditoBss
    {
        TFSM_PortalCotizadorEntities TFSM_PortalCotizador = new TFSM_PortalCotizadorEntities();
        public async Task<DXC.TFSM.Business.Model.ResponseBuroData> BuroWS(DXC.TFSM.Business.Model.BuroData data_) {
            string resume = string.Empty;
            bool esAprobado = false;
            DXC.TFSM.Business.Model.ResponseBuroData res = new DXC.TFSM.Business.Model.ResponseBuroData();

            try {
                //Consumir el servicio de buro de credito de mr. antel:               
                wsBuroCredito.ServiceBuroCreditoClient wsBuroCliente = new wsBuroCredito.ServiceBuroCreditoClient();
                wsBuroCredito.ConsultaTransUnionRequest wsRquest = new wsBuroCredito.ConsultaTransUnionRequest();
                wsBuroCliente.ClientCredentials.UserName.UserName = WebConfigurationManager.AppSettings["BC01"].ToString();
                wsBuroCliente.ClientCredentials.UserName.Password = WebConfigurationManager.AppSettings["BC02"].ToString();

                //-----------CONSULTA OBJECT--------------//
                wsBuroCredito.ConsultaBuroCredito consulta = new wsBuroCredito.ConsultaBuroCredito()
                {
                    origen = wsBuroCredito.Origen.WEB,
                    producto = wsBuroCredito.ProductosTransUnion._107,
                    usuario = "WebUser",
                    idCotizacion = "0",
                    idDealer = 0,
                };
                wsRquest.consulta = consulta;

                //-----------PERSONA OBJECT---------------//
                //--Domicilio object--//
                wsBuroCredito.Domicilio domicilio = new wsBuroCredito.Domicilio()
                {
                    ciudad = data_.Ciudad,
                    codigoPais = data_.Pais,
                    coloniaPoblacion = data_.Colonia,
                    cp = data_.CP,
                    delegacionMpo = data_.Delegacion,
                    direccion1 = data_.Ciudad,
                    direccion2 = data_.Calle,
                    estado = data_.Estado
                };
                //--Nombre object--//
                wsBuroCredito.Nombre nombre = new wsBuroCredito.Nombre()
                {
                    apellidoPaterno = data_.ApellidoP,
                    apellidoMaterno = data_.ApellidoM,
                    primerNombre = data_.PrimerNombre,
                    segundoNombre = data_.SegundoNombre,
                    fechaNacimiento = data_.FechaNacimiento,
                    nacionalidad = data_.Nacionadlidad,
                    rfc = data_.RFC,
                };
                wsBuroCredito.Persona persona = new wsBuroCredito.Persona()
                {
                    catTipoRol = wsBuroCredito.APS_CatTipoRol.TITULAR,
                    tipoPersona = wsBuroCredito.TipoPersona.PF,
                    errorBC = false,
                    folioBuro = "0",
                    idDealer = "0",
                    idPersona = 0,
                    idTipoRol = 0,
                    domicilio = domicilio,
                    nombre = nombre,
                };
                //-----------------------------------------//
                wsRquest.consulta.p = persona;

                //---------AUNTENTICA OBJECT--------------//
                wsBuroCredito.Autentica autentica = new wsBuroCredito.Autentica();
                autentica.autenticar = false;

                autentica.isCreditoAutomotriz = data_.CreditoAutomotriz == "Si" ? true: false;
                autentica.isCreditoHipotecario = data_.CreditoHipotecario == "Si" ? true: false;
                autentica.isTarjetaCredito = data_.TarjetaCredito == "Si" ? true: false;
                autentica.ultimosCuatroDigitosTC = data_.TarjetaCredito == "Si" ? data_.NumeroTarjeta:"";

                wsRquest.consulta.autentica = autentica;

                var Respuesta = wsBuroCliente.ConsultaTransUnion(wsRquest);
                esAprobado = Respuesta.ConsultaTransUnionResult.scoresBC.Select( x=> x.ProspectorAprobado).FirstOrDefault();
                res.EsAprobado = esAprobado;

                var descripcion = string.Empty;
                if (Respuesta.ConsultaTransUnionResult.scoresBC.Length > 0) {
                    descripcion += "||scoresBC >> CodigoError: " + Respuesta.ConsultaTransUnionResult.scoresBC.Select(x => x.CodigoError).FirstOrDefault() + "\n";
                    descripcion += " >> CodigoErrorDescripcion: " + Respuesta.ConsultaTransUnionResult.scoresBC.Select(x => x.CodigoErrorDescripcion).FirstOrDefault() + "\n";
                }
                foreach (var error in Respuesta.ConsultaTransUnionResult.errores) {
                    descripcion += "Error:" + error.Descripcion + " >> cod:" + error.Codigo + "\n";
                }
                foreach (var error in Respuesta.ConsultaTransUnionResult.erroresAR) {
                    descripcion += "\n" + "\n" + "||erroresAR:" + error.ErrorSistemaBC + "\n"
                        + " >> ClaveOPasswordErroneo: " + error.ClaveOPasswordErroneo + "\n"
                        + " >> FaltaCampoRequerido: " + error.FaltaCampoRequerido + "\n"
                        + " >> SujetoNoAutenticado: " + error.SujetoNoAutenticado + "\n"
                        + " >> ReferenciaOperador: " + error.ReferenciaOperador + "\n" + "\n" + "\n";
                }
                foreach (var error in Respuesta.ConsultaTransUnionResult.erroresUR) {
                    descripcion += "||erroresUR:" + error.ErrorSistemaBuroCredito + "\n"
                        + " >> ErrorReporteBloqueado: " + error.ErrorReporteBloqueado + "\n"
                         + " >> EtiquetaSegmentoErronea: " + error.EtiquetaSegmentoErronea + "\n"
                          + " >> FaltaCampoRequerido: " + error.FaltaCampoRequerido + "\n"
                           + " >> InformacionErroneaParaConsulta: " + error.InformacionErroneaParaConsulta + "\n"
                            + " >> NumeroErroneoSegmentos: " + error.NumeroErroneoSegmentos + "\n"
                             + " >> ProductoSolicitadoErroneo: " + error.ProductoSolicitadoErroneo + "\n"
                              + " >> NumeroReferenciaOperador: " + error.NumeroReferenciaOperador + "\n"
                               + " >> SegmentoRequeridoNoProporcionado: " + error.SegmentoRequeridoNoProporcionado + "\n"
                                + " >> UltimaInformacionValidaCliente: " + error.UltimaInformacionValidaCliente;
                }

                res.Description = descripcion;
                
            }
            catch (Exception ex) {
                res.Description = res.Description + " || Error: " + ex.InnerException.ToString();
                throw ex;
            }
            await Task.Delay(2000);

            return res;
        }

        public List<tbl_cotizador_paises> GetCountries() => TFSM_PortalCotizador.tbl_cotizador_paises.ToList();

        public List<tbl_cotizador_estados> GetTbl_Cotizador_states(int id) => TFSM_PortalCotizador.tbl_cotizador_estados.Where(x => x.idPais == id).ToList();

        //Funcion que regrese un archivo PDF
        public void updateDataHTML(string nombre) {
            try {

                StringBuilder sb = new StringBuilder();
                sb.AppendLine("<html>");
                sb.AppendLine("<head>");
                sb.AppendLine("<title>" + nombre + "</title>");
                sb.AppendLine("</head>");
                sb.AppendLine("<body>");
                
                sb.AppendLine("<h1>" + nombre + "</h1>");

                sb.AppendLine("</body>");
                sb.AppendLine("</html>");

                string startupPath = HttpContext.Current.Server.MapPath("~/Views/PDF/");

                if (!Directory.Exists(startupPath)) {
                    Directory.CreateDirectory(startupPath);
                    string filePath = Path.Combine(startupPath + "Response.html");
                }

                using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"\Response.html")) {
                    file.WriteLine(sb.ToString()); // "sb" is the StringBuilder
                }

            }
            catch (Exception ex) {

                throw ex;
            }
        }
    }
}