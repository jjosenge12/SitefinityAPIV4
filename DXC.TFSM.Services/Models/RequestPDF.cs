using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DXC.TFSM.Services.Models
{
    public class RequestPDF
    {
        public Business.Model.ResultPrice[] DatosCotizar { get; set; }
        public string Plazo { get; set; }
        public string ImagenAuto { get; set; }
        public string ImagenModelo { get; set; }
        public double PrecioAuto { get; set; }
        public string FechaCotizacion { get; set; }
    }
}