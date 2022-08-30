using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DXC.TFSM.Services.Models.Forms
{
    public class LoginForm
    {
        public string idCliente_email { get; set; }
        public string password { get; set; }
        public string esCliente { get; set; }
        public string rfc { get; set; }
    }
}