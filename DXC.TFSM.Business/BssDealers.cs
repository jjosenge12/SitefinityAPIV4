using DXC.TFSM.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DXC.TFSM.Business
{
   public class BssDealers
    {
        readonly TFSM_PortalCotizadorEntities tFSM_PortalEntities = new TFSM_PortalCotizadorEntities();

        public async Task<List<tbl_portal_Distribuidores>> GetDealers(int idEstado)
        {
            if (idEstado == 1000) {
                var c = tFSM_PortalEntities.tbl_portal_Distribuidores.ToList();
                await Task.Delay(2000);
                return c;
            }
            else {
                var c = tFSM_PortalEntities.tbl_portal_Distribuidores.Where(e => e.IdState == idEstado).ToList();
                await Task.Delay(2000);
                return c;
            }
        }
        public async Task<List<tbl_portal_Distribuidores>> GetDealers()
        {
            var c = tFSM_PortalEntities.tbl_portal_Distribuidores.ToList();
            await Task.Delay(2000);
            return c;
        }

        public void SetDealersPostalCode(List<DealerPostalCode> dealers)
        {
            foreach (DealerPostalCode item in dealers)
            {
                tbl_portal_Distribuidores d = tFSM_PortalEntities.tbl_portal_Distribuidores.Where(x => x.IdDealer == item.DealerId).FirstOrDefault();
                d.PostalCode = item.PostalCode;
                tFSM_PortalEntities.SaveChanges();
            }
        }
    }

    public class DealerPostalCode
    {
        public int DealerId { get; set; }
        public string PostalCode { get; set; }
    }
}
