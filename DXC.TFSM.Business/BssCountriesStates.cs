using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DXC.TFSM.DataAccess;

namespace DXC.TFSM.Business
{
    public class BssCountriesStates
    {
        readonly TFSM_PortalCotizadorEntities tFSM_PortalEntities = new TFSM_PortalCotizadorEntities();

        public List<tbl_portal_C_Estados> GetAll()
        {
            return tFSM_PortalEntities.tbl_portal_C_Estados.OrderBy(a => a.descripcion).ToList();
        }

        public async Task<tbl_portal_C_Estados> GetCountryID(string adress)
        {
            var c = tFSM_PortalEntities.tbl_portal_C_Estados.Where(e => e.descripcion.Contains(adress)).ToList().FirstOrDefault();
            await Task.Delay(2000);
            return c;
        }
    }
}
