using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DXC.TFSM.DataAccess;

namespace DXC.TFSM.Business
{
    public class BssCoverages
    {
        readonly TFSM_PortalCotizadorEntities tFSM_PortalEntities = new TFSM_PortalCotizadorEntities();

        public List<tbl_portal_C_Coberturas> GetAll(int id)
        {
            //*Business Logic here:
            //
            //*
            return tFSM_PortalEntities.tbl_portal_C_Coberturas.Where(c=> c.id_aseguradora == id).ToList();
        }
    }
}
