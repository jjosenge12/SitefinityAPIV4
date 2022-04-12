using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DXC.TFSM.DataAccess;

namespace DXC.TFSM.Business
{
    public class BssPlan
    {
        readonly TFSM_PortalCotizadorEntities tFSM_PortalEntities = new TFSM_PortalCotizadorEntities();

        public List<tbl_portal_C_plan> GetPlans(int idTipoPersona)
        {
            if (idTipoPersona == 1)
                return tFSM_PortalEntities.tbl_portal_C_plan.Where(t => t.id_plan < 4).ToList();
            else 
                return tFSM_PortalEntities.tbl_portal_C_plan.ToList();
        }

        public List<tbl_portal_C_TipoUso> GetCatTipoUso(int idAuto)     {
            try
            {
                
                return tFSM_PortalEntities.tbl_portal_C_TipoUso.ToList();
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
            finally {

            }
        }
    }
}
