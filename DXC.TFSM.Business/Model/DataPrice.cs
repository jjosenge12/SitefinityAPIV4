using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DXC.TFSM.Business.Model
{
    public class DataPrice
    {
        public string Marca { get; set; }
        public string Modelo { get; set; }
        public string Vesion { get; set; }
        public string Anio { get; set; }
        public string TipoPersona { get; set; }
        public string Estado { get; set; }
        public string Aseguradora { get; set; }
        public string Cobertura { get; set; }

        public string TipoUso { get; set; }
        public string PlanCotizar { get; set; }

        public string EngancheDeposito { get; set; }
        public int CantidadDepositosGarantia { get; set; }
        public string Plazo { get; set; }

        public string Telefono { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
    }
}
