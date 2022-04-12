using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DXC.TFSM.DataAccess;

namespace DXC.TFSM.Services.Models.Responses
{
    public class DealersByStateResponse
    {
        public int EstadoId { get; set; }
        public string Descripcion { get; set; }
        public string CVE { get; set; }
        public List<tbl_portal_Distribuidores> Distribuidores { get; set; }
    }
}