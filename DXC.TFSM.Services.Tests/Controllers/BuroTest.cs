using Microsoft.VisualStudio.TestTools.UnitTesting;
using DXC.TFSM.Services.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DXC.TFSM.Services.Controllers.Tests
{
    [TestClass()]
    public class BuroTest
    {
        [TestMethod()]
        public void SendDataBuroTestTestBuro()
        {

            // Disponer
            CotizadorController controller = new CotizadorController();

            //>>>>> PRUEBA 1 >>>>>>> OK

            //Business.Model.BuroData buro = new Business.Model.BuroData()
            //{
            //    ApellidoP = "ARAUJO",
            //    ApellidoM = "OROZCO",
            //    PrimerNombre = "SERGIO",
            //    SegundoNombre = "ANTONIO",
            //    FechaNacimiento = "01111941",
            //    Nacionadlidad = "MX",
            //    RFC = "AAOS411109D34",
            //    CP = "01430",
            //    Calle = "CALZ DE LA ROMERIA 302",
            //    Colonia = "COLINAS DEL SUR",
            //    Delegacion = "ALVARO OBREGON",
            //    Ciudad = "CIUDAD DE MEXICO",
            //    Pais = "MX",
            //    Estado = "EM",
            //    TarjetaCredito = "true",
            //    NumeroTarjeta = "0697",
            //    CreditoHipotecario = "false",
            //    CreditoAutomotriz = "false",
            //    TerminosCondiciones = "true"
            //};


            //>>>>> PRUEBA 2 >>>>>>> OK

            Business.Model.BuroData buro = new Business.Model.BuroData()
            {
                ApellidoP = "GUTIERREZ",
                ApellidoM = "DE QUEVEDO",
                PrimerNombre = "MARIA",
                SegundoNombre = "DEL PILAR",
                FechaNacimiento = "23111978",
                Nacionadlidad = "MX",
                RFC = "GURM781123",
                CP = "04500",
                Calle = "CENOTE 8",
                Colonia = "JARDS DEL PEDREGAL DE SN ANGEL",
                Delegacion = "COYOACAN",
                Ciudad = "CIUDAD DE MEXICO",
                Pais = "MX",
                Estado = "EM",
                TarjetaCredito = "No",
                NumeroTarjeta = "0",
                CreditoHipotecario = "No",
                CreditoAutomotriz = "No",
                TerminosCondiciones = "Si"
            };



            //>>>>> PRUEBA PERSONALIZADA >>>>>>> ERR

            //Business.Model.BuroData buro = new Business.Model.BuroData()
            //{
            //    ApellidoP = "CASASOLA",
            //    ApellidoM = "GARCIA",
            //    PrimerNombre = "CARLOS",
            //    SegundoNombre = "",
            //    FechaNacimiento = "09051987",
            //    Nacionadlidad = "MX",
            //    RFC = "CAGC870509NM1",
            //    CP = "55630",
            //    Calle = "CDA CENTAURO DEL NORTE 3",
            //    Colonia = "SAN BARTOLO CUAUTLALPAN",
            //    Delegacion = "ZUMPANGO",
            //    Ciudad = "ESTADO DE MEXICO",
            //    Pais = "MX",
            //    Estado = "EM",
            //    TarjetaCredito = "No",
            //    NumeroTarjeta = "0",
            //    CreditoHipotecario = "Mo",
            //    CreditoAutomotriz = "Si",
            //    TerminosCondiciones = "Si"
            //};



            // Actuar
            var result = controller.SendDataBuroTest(buro);

            // Declarar
            Assert.IsNotNull(result);
          
        }
    }
}