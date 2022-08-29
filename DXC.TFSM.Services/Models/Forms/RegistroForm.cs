using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DXC.TFSM.Services.Models.Forms
{
    public class RegistroForm
    {
        public string rfcFull { get; set; }

        public string name { get; set; }

        public string lastName { get; set; }

        public string email { get; set; }

        public string idCliente { get; set; }

        public string idUsuario { get; set; }

        public string url { get; set; }
    }
}