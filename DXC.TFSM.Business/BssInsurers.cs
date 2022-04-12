using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DXC.TFSM.DataAccess;

namespace DXC.TFSM.Business
{
   
    public class BssInsurers
    {
        private readonly TFSM_PortalCotizadorEntities tFSM_PortalEntities = new TFSM_PortalCotizadorEntities();

        public List<tbl_portal_C_Aseguradora> GetAll()
        {
            //Business Logic here:
            //*
            //
            return tFSM_PortalEntities.tbl_portal_C_Aseguradora.ToList();
        }
    }
}
