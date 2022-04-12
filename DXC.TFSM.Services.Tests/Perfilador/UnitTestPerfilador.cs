using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net.Http;
using System.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using Microsoft.VisualBasic;
using DXC.TFSM.Services.Controllers;

namespace DXC.TFSM.Services.Tests.Perfilador
{
    [TestClass]
    public class UnitTestPerfilador
    {
        [TestMethod]
        public void GetQuestion()
        {
            CotizadorController controller = new CotizadorController();

            // Actuar
            var result = controller.GetQuestion();

            // Declarar
            Assert.IsNotNull(result);
        }


        [TestMethod]
        public void GetProfilerFinal()
        {
            CotizadorController controller = new CotizadorController();

            List<string> Datos = new List<string>();
            Datos.Add("R1A");
            Datos.Add("R2B");
            Datos.Add("R3C");
            Datos.Add("R4D");
            Datos.Add("R5E");

            // Actuar
            //var result = controller.GetProfile(Datos);

            // Declarar
           // Assert.IsNotNull(result);
        }
    }
}