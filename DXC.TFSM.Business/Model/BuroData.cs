using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DXC.TFSM.Business.Model
{
    public class BuroData
    {
        public string ApellidoP { get; set; }
        public string ApellidoM { get; set; }
        public string PrimerNombre { get; set; }
        public string SegundoNombre { get; set; }
        public string FechaNacimiento { get; set; }
        public string Nacionadlidad { get; set; }
        public string RFC { get; set; }
        public string CP { get; set; }
        public string Calle { get; set; }
        public string Colonia { get; set; }
        public string Delegacion { get; set; }
        public string Ciudad { get; set; }
        public string Estado { get; set; }
        public string Pais { get; set; }
        public string TarjetaCredito { get; set; }
        public string NumeroTarjeta { get; set; }
        public string CreditoHipotecario { get; set; }
        public string CreditoAutomotriz { get; set; }
        public string TerminosCondiciones{ get; set; }

    }
    public class ResponseBuroData {
        public bool EsAprobado { get; set; }
        public string Description { get; set; }
    }
}
