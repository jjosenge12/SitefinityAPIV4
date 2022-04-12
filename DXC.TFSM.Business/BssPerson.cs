using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DXC.TFSM.DataAccess;

namespace DXC.TFSM.Business
{
    public class BssPerson
    {
        readonly TFSM_PortalCotizadorEntities tFSM_PortalEntities = new TFSM_PortalCotizadorEntities();

        public List<tbl_portal_C_tipo_persona> Get_C_Tipo_Personas() {
            var Persons = tFSM_PortalEntities.tbl_portal_C_tipo_persona.ToList();
            return Persons;
        }
    }
}
