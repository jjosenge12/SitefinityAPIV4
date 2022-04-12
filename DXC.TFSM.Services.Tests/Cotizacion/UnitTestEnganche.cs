using Microsoft.VisualStudio.TestTools.UnitTesting;
using DXC.TFSM.Services.Controllers;
using System.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DXC.TFSM.Services.Tests.Cotizacion
{
    [TestClass]
    public class UnitTestEnganche
    {
        [TestMethod]
        public void TestInicial()
        {
            // Disponer
            CotizadorController controller = new CotizadorController();

            Business.Model.DataPrice Datos = new Business.Model.DataPrice();
            Datos.Marca = "C-HR";
            Datos.Modelo = "2019";
            Datos.Vesion = "CVT"; ;


            Datos.Estado = "DF";
            Datos.Aseguradora = "QualitasT";
            Datos.Cobertura = "AMPLIA";
            Datos.EngancheDeposito = "130000";


            Datos.PlanCotizar = "Tradicional";

            Datos.Apellido = "Apellido";
            Datos.Nombre = "Nombre";
            Datos.Plazo = "Plazo";
            Datos.Telefono = "Telefono";
            Datos.TipoPersona = "TipoPersona";
            Datos.TipoUso = "TipoUso";

            // Actuar
            var result = controller.GetEnganche(Datos);

            // Declarar
            Assert.IsNotNull(result);
        }
    }
}
