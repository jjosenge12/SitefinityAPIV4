using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net.Http;
using System.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace DXC.TFSM.Services.Tests.Salesforce
{
    [TestClass]
    public class UnitTestConsultaIDrecta
    {

        private HttpClient client = new HttpClient();

        [TestMethod]
        public async Task POST_002_DXC()
        {
            client.BaseAddress = new Uri("https://test.salesforce.com");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/xml"));
            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string,string>("oid","00D3D000000AJDJ"),
                new KeyValuePair<string,string>("retURL","https://toyotacredito.com.mx/."),
                new KeyValuePair<string,string>("email","002_DXC@dxc_test.com"),
                new KeyValuePair<string,string>("state", "PUEBLA"),
                new KeyValuePair<string,string>("lead_source", "Cotizador+Web"),
                new KeyValuePair<string,string>("submit", "Enviar+consulta")

            });

            CacheControlHeaderValue cacheControl = new CacheControlHeaderValue();
            cacheControl.Private = true;
            cacheControl.NoCache = true;
            client.DefaultRequestHeaders.CacheControl = cacheControl;
            //Petición POST para generar el token
            var result = await client.PostAsync("/servlet/servlet.WebToLead?encoding=UTF-8", content);
            var resultContent = await result.Content.ReadAsStringAsync();
            Assert.IsTrue(result.StatusCode == System.Net.HttpStatusCode.OK);
        }

      



    }
}
