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

namespace DXC.TFSM.Services.Tests.Salesforce
{

    [TestClass]
    public class UnitTestPost
    {
        //public string DoFormPost(string Target, string PostData)
        //{
        //    //Make a request            
        //    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Target);
        //    request.Method = "POST";
        //    request.ContentType = "application/x-www-form-urlencoded";
        //    request.AllowAutoRedirect = false;

        //    //Put the post data into the request
        //    byte[] data = (new ASCIIEncoding()).GetBytes(PostData);
        //    request.ContentLength = data.Length;
        //    Stream reqStream = request.GetRequestStream();
        //    reqStream.Write(data, 0, data.Length);
        //    reqStream.Close();

        //    //Get response
        //    HttpWebResponse response = (HttpWebResponse)request.GetResponse();

        //    //Output response to a string            
        //    String result = "";
        //    using (Stream responseStream = response.GetResponseStream())
        //    {
        //        using (StreamReader reader = new StreamReader(responseStream, Encoding.UTF8))
        //        {
        //            result = reader.ReadToEnd();
        //            reader.Close();
        //        }
        //        return result;
        //    }
        //}
        
        [TestMethod]
        public void POST_A001_DXC()
        {

            var client = new HttpClient(new HttpClientHandler() { AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip });
            var request = new HttpRequestMessage()
            {
                RequestUri = new Uri("https://test.salesforce.com"),
                Method = HttpMethod.Post
            };

   
            request.Content = new StringContent("oid=00D3D000000AJDJ&retURL=https%3A%2F%2Ftoyotacredito.com.mx%2F&email=POST_A001_DXC@dxc.com&state=CDMX&lead_source=Trafico+Piso&submit=Enviar", Encoding.UTF8, "text/xml");

            request.Headers.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/xml"));
            request.Content.Headers.ContentType = new MediaTypeHeaderValue("text/xml");
            request.Headers.Add("URL","/servlet/servlet.WebToLead");

            HttpResponseMessage response = client.SendAsync(request).Result;
            Assert.IsTrue(response.StatusCode == System.Net.HttpStatusCode.OK);
        }

        [TestMethod]
        public void POST_A002_DXC()
        {
            var client = new HttpClient(new HttpClientHandler() );
            var request = new HttpRequestMessage()
            {
                RequestUri = new Uri("https://test.salesforce.com/servlet/servlet.WebToLead?encoding=UTF-8"),
                Method = HttpMethod.Post
            };

            request.Content = new StringContent("oid=00D3D000000AJDJ&retURL=https%3A%2F%2Ftoyotacredito.com.mx%2F&email=POST_A002_DXC%dxc.com&state=CDMX&lead_source=Cotizador+Web&submit=Enviar+consulta", Encoding.UTF8, "text/xml");

            request.Headers.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/xml"));
            request.Content.Headers.ContentType = new MediaTypeHeaderValue("text/xml");
            request.Headers.Add("SOAPAction", "https://test.salesforce.com/servlet/servlet.WebToLead?encoding=UTF-8");

            HttpResponseMessage response = client.SendAsync(request).Result;
            Assert.IsTrue(response.StatusCode == System.Net.HttpStatusCode.OK);


        }


        [TestMethod]
        public void POST_A003_DXC()
        {
            var client = new HttpClient(new HttpClientHandler());
            var request = new HttpRequestMessage()
            {
                RequestUri = new Uri("https://test.salesforce.com"),
                Method = HttpMethod.Post
            };

            request.Content = new StringContent("oid=00D3D000000AJDJ&retURL=https%3A%2F%2Ftoyotacredito.com.mx%2F&email=POST_A003_DXC%dxc.com&state=CDMX&lead_source=Cotizador+Web&submit=Enviar+consulta", Encoding.UTF8, "text/xml");

            request.Headers.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/xml"));
            request.Content.Headers.ContentType = new MediaTypeHeaderValue("text/xml");
            request.Headers.Add("SOAPAction", "/servlet/servlet.WebToLead?encoding=UTF-8");

            HttpResponseMessage response = client.SendAsync(request).Result;
            Assert.IsTrue(response.StatusCode == System.Net.HttpStatusCode.OK);


        }




        [TestMethod]
        public void POST_A004_DXC()
        {
            var client = new HttpClient(new HttpClientHandler());
            var request = new HttpRequestMessage()
            {
                RequestUri = new Uri("https://test.salesforce.com"),
                Method = HttpMethod.Post
            };

            request.Content = new StringContent("oid=00D3D000000AJDJ&retURL=https%3A%2F%2Ftoyotacredito.com.mx%2F&email=POST_A004_DXC%dxc.com&state=CDMX&lead_source=Cotizador+Web&submit=Enviar+consulta", Encoding.UTF8, "text/xml");

            request.Headers.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/xml"));
            request.Content.Headers.ContentType = new MediaTypeHeaderValue("text/xml");
            request.Headers.Add("SOAPAction", "/servlet/servlet.WebToLead");

            HttpResponseMessage response = client.SendAsync(request).Result;
            Assert.IsTrue(response.StatusCode == System.Net.HttpStatusCode.OK);


        }


        [TestMethod]
        public void POST_A005_DXC()
        {
            var client = new HttpClient(new HttpClientHandler());
            var request = new HttpRequestMessage()
            {
                RequestUri = new Uri("https://test.salesforce.com"),
                Method = HttpMethod.Post
            };

            request.Content = new StringContent("oid=00D3D000000AJDJ&retURL=https%3A%2F%2Ftoyotacredito.com.mx%2F&email=POST_A005_DXC%dxc.com&state=CDMX&lead_source=Cotizador+Web&submit=Enviar+consulta", Encoding.UTF8, "text/xml");

            request.Headers.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/xml;charset=UTF-8"));
            request.Content.Headers.ContentType = new MediaTypeHeaderValue("text/xml");
            request.Headers.Add("SOAPAction", "/servlet/servlet.WebToLead?encoding=UTF-8");

            HttpResponseMessage response = client.SendAsync(request).Result;
            Assert.IsTrue(response.StatusCode == System.Net.HttpStatusCode.OK);


        }




        [TestMethod]
        public void POST_A006_DXC()
        {
            var client = new HttpClient(new HttpClientHandler());
            var request = new HttpRequestMessage()
            {
                RequestUri = new Uri("https://test.salesforce.com"),
                Method = HttpMethod.Post
            };

            request.Content = new StringContent("oid=00D3D000000AJDJ&retURL=https%3A%2F%2Ftoyotacredito.com.mx%2F&email=POST_A006_DXC%dxc.com&state=CDMX&lead_source=Cotizador+Web&submit=Enviar+consulta", Encoding.UTF8, "text/xml");

            request.Headers.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/xml;charset=UTF-8"));
            request.Content.Headers.ContentType = new MediaTypeHeaderValue("text/xml");
            request.Headers.Add("SOAPAction", "/servlet/servlet.WebToLead");

            HttpResponseMessage response = client.SendAsync(request).Result;
            Assert.IsTrue(response.StatusCode == System.Net.HttpStatusCode.OK);


        }




        [TestMethod]
        public void POST_A007_DXC()
        {
            var client = new HttpClient(new HttpClientHandler());
            var request = new HttpRequestMessage()
            {
                RequestUri = new Uri("https://test.salesforce.com"),
                Method = HttpMethod.Post
            };

            request.Content = new StringContent("oid=00D3D000000AJDJ&retURL=https%3A%2F%2Ftoyotacredito.com.mx%2F&email=POST_A007_DXC%dxc.com&state=CDMX&lead_source=Cotizador+Web&submit=Enviar+consulta", Encoding.UTF8, "text/xml");

            request.Headers.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/xml;charset=UTF-8"));
            request.Content.Headers.ContentType = new MediaTypeHeaderValue("text/xml;charset=UTF-8");
            request.Headers.Add("SOAPAction", "/servlet/servlet.WebToLead?encoding=UTF-8");

            HttpResponseMessage response = client.SendAsync(request).Result;
            Assert.IsTrue(response.StatusCode == System.Net.HttpStatusCode.OK);


        }




        [TestMethod]
        public void POST_A008_DXC()
        {
            var client = new HttpClient(new HttpClientHandler());
            var request = new HttpRequestMessage()
            {
                RequestUri = new Uri("https://test.salesforce.com"),
                Method = HttpMethod.Post
            };

            request.Content = new StringContent("oid=00D3D000000AJDJ&retURL=https%3A%2F%2Ftoyotacredito.com.mx%2F&email=POST_A008_DXC%dxc.com&state=CDMX&lead_source=Cotizador+Web&submit=Enviar+consulta", Encoding.UTF8, "text/xml");

            request.Headers.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/xml;charset=UTF-8"));
            request.Content.Headers.ContentType = new MediaTypeHeaderValue("text/xml;charset=UTF-8");
            request.Headers.Add("SOAPAction", "/servlet/servlet.WebToLead");

            HttpResponseMessage response = client.SendAsync(request).Result;
            Assert.IsTrue(response.StatusCode == System.Net.HttpStatusCode.OK);


        }



        [TestMethod]
        public void POST_A009_DXC()
        {
            var client = new HttpClient(new HttpClientHandler());
            var request = new HttpRequestMessage()
            {
                RequestUri = new Uri("https://test.salesforce.com"),
                Method = HttpMethod.Post
            };

            request.Content = new StringContent("oid=00D3D000000AJDJ&retURL=https%3A%2F%2Ftoyotacredito.com.mx%2F&email=POST_A009_DXC%dxc.com&state=CDMX&lead_source=Cotizador+Web&submit=Enviar+consulta", Encoding.UTF8, "text/xml");

            request.Headers.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/xml;charset=UTF-8"));
            request.Content.Headers.ContentType = new MediaTypeHeaderValue("text/xml");
            request.Headers.Add("SOAPAction", "/servlet/servlet.WebToLead?encoding=UTF-8");

            HttpResponseMessage response = client.SendAsync(request).Result;
            Assert.IsTrue(response.StatusCode == System.Net.HttpStatusCode.OK);


        }




        [TestMethod]
        public void POST_A010_DXC()
        {
            var client = new HttpClient(new HttpClientHandler());
            var request = new HttpRequestMessage()
            {
                RequestUri = new Uri("https://test.salesforce.com"),
                Method = HttpMethod.Post
            };

            request.Content = new StringContent("oid=00D3D000000AJDJ&retURL=https%3A%2F%2Ftoyotacredito.com.mx%2F&email=POST_A010_DXC%dxc.com&state=CDMX&lead_source=Cotizador+Web&submit=Enviar+consulta", Encoding.UTF8, "text/xml");

            request.Headers.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/xml"));
            request.Content.Headers.ContentType = new MediaTypeHeaderValue("text/xml;charset=UTF-8");
            request.Headers.Add("SOAPAction", "/servlet/servlet.WebToLead");

            HttpResponseMessage response = client.SendAsync(request).Result;
            Assert.IsTrue(response.StatusCode == System.Net.HttpStatusCode.OK);


        }
    }
}
