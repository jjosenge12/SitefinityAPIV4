using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DXC.TFSM.DataAccess;
using DXC.TFSM.Business.Model;

namespace DXC.TFSM.Business
{

    public  class BssProfiler
    {
        private struct SumaPlan
        {
            public string Plan;
            public decimal Suma;
        }
        readonly TFSM_PortalCotizadorEntities tFSM_PortalEntities = new TFSM_PortalCotizadorEntities();
        public List<Profiler> GetProfiler() {
            List<tbl_perfilador_C_Preguntas> Respuesta = new List<tbl_perfilador_C_Preguntas>();
            var Consulta = tFSM_PortalEntities.tbl_perfilador_C_Preguntas.Where(x => x.Activa == true).ToList();
            List<Profiler> lst = new List<Profiler>();

            foreach (var preguntaBD in Consulta) {
                Profiler newP = new Profiler();
                newP.lstRespuestas = new List<tbl_perfilador_Respuestas>();
                newP.IdPregunta = preguntaBD.Id;
                newP.Clave = preguntaBD.Clave;
                newP.Titulo = preguntaBD.Titulo;
                newP.Subtitulo = preguntaBD.Subtitulo;
                newP.Pregunta = preguntaBD.Pregunta;
                newP.EstaActiva = preguntaBD.Activa;
                newP.IdPregunta = preguntaBD.Id;
                newP.lstRespuestas = GetLstAnswers(preguntaBD.Id);
                lst.Add(newP);
            }

            return lst;
        }

        private List<tbl_perfilador_Respuestas> GetLstAnswers(int idQuestion) {
            return tFSM_PortalEntities.tbl_perfilador_Respuestas.Where(r => r.IdPregunta == idQuestion).OrderBy(o=> o.Orden).ToList();
        }

        public tbl_perfilador_Destinos GetProfilerFinal(List<string> Clave)
        {
            tbl_perfilador_Destinos Respuesta = new tbl_perfilador_Destinos();
            List<SumaPlan> SumaResultado = new List<SumaPlan>();
            decimal[,] Resultado= new decimal[1,6];

            SumaResultado.Add(new SumaPlan { Suma = GetSuma(Clave, "A"), Plan = "A001" });
            SumaResultado.Add(new SumaPlan { Suma = GetSuma(Clave, "B"), Plan = "B001" });
            SumaResultado.Add(new SumaPlan { Suma = GetSuma(Clave, "C"), Plan = "C001" });
            SumaResultado.Add(new SumaPlan { Suma = GetSuma(Clave, "D"), Plan = "D001" });
            SumaResultado.Add(new SumaPlan { Suma = GetSuma(Clave, "E"), Plan = "E001" });
            SumaResultado.Add(new SumaPlan { Suma = GetSuma(Clave, "F"), Plan = "F001" });

            SumaPlan RenglonFinal  = SumaResultado.OrderByDescending(o => o.Suma).FirstOrDefault();
            
            return tFSM_PortalEntities.tbl_perfilador_Destinos.Where(x => x.Activa == true && x.Clave == RenglonFinal.Plan).FirstOrDefault();
        }



        decimal GetSuma(List<string> Clave, string TipoDato)
        {
            decimal Suma = 0;
            foreach (string Renglon in Clave.Where(x => x.Contains(TipoDato)).ToList()){
                Suma = Suma + tFSM_PortalEntities.tbl_perfilador_Respuestas.Where(x => x.Activa == true && x.Clave == Renglon).FirstOrDefault().Valor;
            }
            return Suma;
        }
    }
}
