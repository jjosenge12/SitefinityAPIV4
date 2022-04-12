using DXC.TFSM.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DXC.TFSM.Business.Model
{
   public class Profiler
    {
        public string Clave { get; set; }
        public string Titulo { get; set; }
        public string Subtitulo { get; set; }
        public string Pregunta { get; set; }
        public bool EstaActiva { get; set; }
        public int IdPregunta { get; set; }
        public List<tbl_perfilador_Respuestas> lstRespuestas { get; set; }
    }

    public class Answers {
        public int IdRespuesta { get; set; }
        public string Clave { get; set; }
        public int Orden { get; set; }
        public string Respuesta { get; set; }
        public string Valor { get; set; }
        public bool EstaActiva { get; set; }
        public int IdPregunta { get; set; }
    }
}
