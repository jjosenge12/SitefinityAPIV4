using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DXC.TFSM.Services.Models.Forms
{
    public class PlanForm
    {
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
        public int DealerId { get; set; }
        public string Dealer { get; set; } 
        public string Phone { get; set; }
        public string Plan { get; set; }
        public string Vehicle { get; set; }
    }
}