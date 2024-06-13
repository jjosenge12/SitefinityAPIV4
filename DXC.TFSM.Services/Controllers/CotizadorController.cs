using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using DXC.TFSM.Business;
using ExpertPdf.HtmlToPdf;
using Newtonsoft.Json;
using System.Web.Http.Results;
using DXC.TFSM.DataAccess;
using DXC.TFSM.Services.Models.Responses;
using DXC.TFSM.Services.Models.Forms;
using System.Text;
using System.Collections;
using DXC.TFSM.Services.Models;
using Newtonsoft.Json.Linq;
using System.Security.Cryptography;


namespace DXC.TFSM.Services.Controllers
{
    [RoutePrefix("api/tfsm")]
    public class CotizadorController : ApiController
    {
        #region Global Variables
        readonly BssCars BssAutos = new BssCars();
        //readonly string UrlBase = "https://toyotafinancial.my.salesforce.com/"; //PROD
        //readonly string organizationID = "00Dj0000000KZrJ"; //PROD
        //readonly string UrlBase = "https://toyotafinancial--salt001.sandbox.my.salesforce.com/"; //SALT
        //readonly string organizationID = "00D630000004s6r"; //SALT
        readonly string UrlBase = Environment.GetEnvironmentVariable("SitefinityURL");
        readonly string organizationID = Environment.GetEnvironmentVariable("SitefinityOrganizationIDSalesforce");
        readonly BssInsurers BssInsurers = new BssInsurers();
        readonly BssCoverages BssCoverages = new BssCoverages();
        readonly BssCountriesStates BssCStates = new BssCountriesStates();
        readonly BssPerson BssPerson = new BssPerson();
        readonly BssPlan BssPlan = new BssPlan();
        readonly BssPrice BssPrice = new BssPrice();
        readonly BssProfiler BssProfiler = new BssProfiler();
        readonly BssDealers BssDealers = new BssDealers();
        readonly BssCypher Cypher = new BssCypher();
        readonly BuroCreditoBss buroFrm = new BuroCreditoBss();
        #endregion

        #region Get Methods
        [Route("GetCars")]
        [HttpGet]
        public async Task<IHttpActionResult> GetCatAutos()
        {
            var cat = await BssAutos.GetListCars();
            if (cat != null)
                return Ok(new { results = cat });
            else
                return NotFound();
        }

        [Route("GetVersion")]
        [HttpGet]
        public IHttpActionResult GetVersion(int id)
        {
            var versions = BssAutos.GetVersionById(id);
            if (versions != null)
                return Ok(new { versions });
            else
                return NotFound();
        }

        [Route("GetInsurers")]
        [HttpGet]
        public IHttpActionResult GetCatInsurers()
        {
            var cat = BssInsurers.GetAll();
            return Ok(new { results = cat });
        }

        [Route("GetCoverage")]
        [HttpGet]
        public IHttpActionResult GetCatCoverages(int id)
        {
            var cat = BssCoverages.GetAll(id);
            if (cat != null)
                return Ok(new { results = cat });
            else
                return NotFound();
        }

        [Route("GetCountryStates")]
        [HttpGet]
        public IHttpActionResult GetCountryStates()
        {
            var cat = BssCStates.GetAll();
            return Ok(new { results = cat });
        }

        [Route("GetTypePerson")]
        [HttpGet]
        public IHttpActionResult GetTypePerson()
        {
            var personsType = BssPerson.Get_C_Tipo_Personas();
            if (personsType != null)
                return Ok(new { personsType });
            else
                return NotFound();
        }

        /// <summary>
        /// Method that returns list of plans avalible.
        /// </summary>
        /// <returns>List of plans avalible in system</returns>
        [Route("GetTypePlan")]
        [HttpGet]
        public IHttpActionResult GetTypePlan(int idTipoPersona)
        {
            var response = BssPlan.GetPlans(idTipoPersona);
            if (response != null)
                return Ok(new { results = response });
            else
                return NotFound();
        }

        [Route("GetTypeUsePlan")]
        [HttpGet]
        public IHttpActionResult GetUsePlan(int id)
        {
            var response = BssPlan.GetCatTipoUso(id);
            if (response != null)
                return Ok(new { results = response });
            else
                return NotFound();
        }

        #endregion

        #region Cotizador
        [Route("PostCotizacion")]
        [HttpPost]
        public IHttpActionResult PostCotizacion(Business.Model.DataPrice DatosCotizar)
        {
            var v = string.Empty;
            try
            {
                var data = BssPrice.GetPrice(DatosCotizar);
                return Ok(new { data });
            }
            catch (Exception ex)
            {
                v = "Exception >> " + ex.Message + " >> " + ex.InnerException.ToString();
                return Ok(new { v });
                throw ex;
            }
        }

        [Route("GetEnganche")]
        [HttpPost]
        public IHttpActionResult GetEnganche(Business.Model.DataPrice DatosCotizar)
        {
            var PriceType = BssPrice.GetHitch(DatosCotizar);
            if (PriceType != null)
                return Ok(new { PriceType });
            else
                return NotFound();
        }
        #endregion

        #region Perfilador

        [Route("GetQuestion")]
        [HttpGet]
        public IHttpActionResult GetQuestion()
        {
            var result = BssProfiler.GetProfiler();
            if (result != null)
                return Ok(new { result });
            else
                return NotFound();
        }
        [Route("GetProfile")]
        [HttpGet]
        public IHttpActionResult GetProfile(string Claves)
        {
            if (Claves != null)
            {
                List<string> lst = Claves.Split(',').ToList();

                var ProfileType = BssProfiler.GetProfilerFinal(lst);
                if (ProfileType != null)
                    return Ok(new { ProfileType });
                else
                    return NotFound();
            }
            else
            {
                return NotFound();
            }

        }
        #endregion

        #region SalesForce
        [Route("SalesForce")]
        [HttpPost]
        public async Task<IHttpActionResult> SalesFocreCommit(Business.Model.DataSalesForce DatosCotizar)
        {
            HttpClient client = new HttpClient();

            client.BaseAddress = new Uri(UrlBase);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/xml"));
            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string,string>("oid",organizationID),
                new KeyValuePair<string,string>("retURL","https://toyotacredito.com.mx/."),
                new KeyValuePair<string,string>("00Nf100000Ce1Ld",DatosCotizar.Modelo),
                new KeyValuePair<string,string>("00Nf100000Ce1LV",DatosCotizar.Vesion),
                new KeyValuePair<string,string>("00Nf100000Ce1LZ",DatosCotizar.TipoPersona),
                new KeyValuePair<string,string>("00Nf100000Ce1LK",DatosCotizar.Aseguradora),
                new KeyValuePair<string,string>("00N3D000005eK3j",DatosCotizar.Cobertura),
                new KeyValuePair<string,string>("state",DatosCotizar.Estado),
                new KeyValuePair<string,string>("00Nf100000Ce1La",DatosCotizar.Plan),
                new KeyValuePair<string,string>("FWY_Veh_culo__c",DatosCotizar.Marca),
                new KeyValuePair<string,string>("00Nf100000Ce1LO",DatosCotizar.Enganche),
                new KeyValuePair<string,string>("00N3D000005eK3y",DatosCotizar.DepositoGarantia),
                new KeyValuePair<string,string>("00N3D000005eK43",DatosCotizar.Ballon),
                new KeyValuePair<string,string>("first_name",DatosCotizar.Nombre),
                new KeyValuePair<string,string>("last_name",DatosCotizar.Apellido),
                new KeyValuePair<string,string>("phone",DatosCotizar.Movil),
                new KeyValuePair<string,string>("00N3D000005eK3o",DatosCotizar.Plazo),
                new KeyValuePair<string,string>("email",DatosCotizar.Email),

                new KeyValuePair<string,string>("AceptoTerminosYCondiciones",DatosCotizar.AceptoTerminosYCondiciones),
                new KeyValuePair<string,string>("CodigoDistribuidor",DatosCotizar.CodigoDistribuidor),
                new KeyValuePair<string,string>("EstadoSeleccionado",DatosCotizar.EstadoSeleccionado),

                new KeyValuePair<string,string>("00Nf100000Ce1Le",DatosCotizar.CodigoDistribuidor),
                new KeyValuePair<string,string>("company",DatosCotizar.Distribuidor),
                new KeyValuePair<string,string>("lead_source","TFS - Cotizador web"),
                new KeyValuePair<string,string>("submit","Enviar")
            });

            //Petición POST para generar el token
            var result = await client.PostAsync("/servlet/servlet.WebToLead?encoding=UTF-8", content);
            return Ok(result);
        }

        [Route("SalesForceStep4")]
        [HttpPost]
        public async Task<IHttpActionResult> SalesFocreCommitStep4(Business.Model.DataSalesForce DatosCotizar)
        {


            HttpClient client = new HttpClient();

            client.BaseAddress = new Uri(UrlBase);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/xml"));
            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string,string>("oid",organizationID),
                new KeyValuePair<string,string>("retURL","https://toyotacredito.com.mx/."),

                new KeyValuePair<string,string>("00Nf100000Ce1LK",DatosCotizar.Aseguradora),
                new KeyValuePair<string,string>("00N3D000005eK3j",DatosCotizar.Cobertura),
                new KeyValuePair<string,string>("state",DatosCotizar.Estado),
                new KeyValuePair<string,string>("00Nf100000Ce1La",DatosCotizar.Plan),
                new KeyValuePair<string,string>("FWY_Veh_culo__c",DatosCotizar.Marca),
                new KeyValuePair<string,string>("00Nf100000Ce1Ld",DatosCotizar.Modelo),
                new KeyValuePair<string,string>("00Nf100000Ce1LV",DatosCotizar.Vesion),
                new KeyValuePair<string,string>("00Nf100000Ce1LZ",DatosCotizar.TipoPersona),
                new KeyValuePair<string,string>("00Nf100000Ce1LO",DatosCotizar.Enganche),
                new KeyValuePair<string,string>("00N3D000005eK3y",DatosCotizar.DepositoGarantia),
                new KeyValuePair<string,string>("00N3D000005eK43",DatosCotizar.Ballon),
                new KeyValuePair<string,string>("Plazo__c",DatosCotizar.Plazo),
                new KeyValuePair<string,string>("Mensualidad__c",DatosCotizar.Mensualidad),
                new KeyValuePair<string,string>("first_name",DatosCotizar.Nombre),
                new KeyValuePair<string,string>("last_name",DatosCotizar.Apellido),
                new KeyValuePair<string,string>("mobile",DatosCotizar.Movil),
                new KeyValuePair<string,string>("email",DatosCotizar.Email),
                new KeyValuePair<string,string>("visibleTFSM__c", "1"),
                new KeyValuePair<string,string>("AceptoTerminosYCondiciones",DatosCotizar.AceptoTerminosYCondiciones),
                new KeyValuePair<string,string>("00Nf100000Ce1Le",DatosCotizar.CodigoDistribuidor),
                new KeyValuePair<string,string>("lead_source","Cotizador Web paso 4"),
                new KeyValuePair<string,string>("submit","Enviar")
            });

            //Petición POST para generar el token
            var result = await client.PostAsync("/servlet/servlet.WebToLead?encoding=UTF-8", content);
            return Ok(result);
        }

        [Route("SalesForceCommitPlan")]
        [HttpPost]
        public async Task<IHttpActionResult> SalesForceCommitPlan(Business.Model.DataSalesForce DatosCotizar)
        {
            HttpClient client = new HttpClient();

            client.BaseAddress = new Uri(UrlBase);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/xml"));
            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string,string>("oid",organizationID),
                new KeyValuePair<string,string>("retURL","https://toyotacredito.com.mx/."),

                //new KeyValuePair<string,string>("state",DatosCotizar.Estado),
                new KeyValuePair<string,string>("00Nf100000Ce1La",DatosCotizar.Plan),
                new KeyValuePair<string,string>("FWY_Veh_culo__c",DatosCotizar.Marca),
                new KeyValuePair<string,string>("first_name",DatosCotizar.Nombre),
                new KeyValuePair<string,string>("last_name",DatosCotizar.Apellido),
                new KeyValuePair<string,string>("phone",DatosCotizar.Movil),
                new KeyValuePair<string,string>("email",DatosCotizar.Email),
                new KeyValuePair<string,string>("FWY_Nombre_distribuidor__c",DatosCotizar.Aseguradora),
                new KeyValuePair<string,string>("Company",DatosCotizar.Aseguradora),
                new KeyValuePair<string,string>("visibleTFSM__c", "1"),
                new KeyValuePair<string,string>("AceptoTerminosYCondiciones",DatosCotizar.AceptoTerminosYCondiciones),
                new KeyValuePair<string,string>("00Nf100000Ce1Le",DatosCotizar.CodigoDistribuidor),
                new KeyValuePair<string,string>("lead_source","TFS - " + DatosCotizar.Plan),
                new KeyValuePair<string,string>("submit","Enviar")
            });

            //Petición POST para generar el token
            var result = await client.PostAsync("/servlet/servlet.WebToLead?encoding=UTF-8", content);
            return Ok(result);
        }

        [Route("SalesforceNewsletter")]
        [HttpPost]
        public async Task<IHttpActionResult> SalesForceNewsletter(Business.Model.DataSalesForce DatosCotizar)
        {
            HttpClient client = new HttpClient();

            client.BaseAddress = new Uri(UrlBase);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/xml"));
            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string,string>("oid",organizationID),
                new KeyValuePair<string,string>("retURL","https://toyotacredito.com.mx/."),

                //new KeyValuePair<string,string>("state",DatosCotizar.Estado),
                new KeyValuePair<string,string>("email",DatosCotizar.Email),
                new KeyValuePair<string,string>("AceptoTerminosYCondiciones",DatosCotizar.AceptoTerminosYCondiciones),
                //new KeyValuePair<string,string>("00Nf100000Ce1Le","57900"),
                new KeyValuePair<string,string>("lead_source","Newsletter"),
                new KeyValuePair<string,string>("submit","Enviar")
            });

            //Petición POST para generar el token
            var result = await client.PostAsync("/servlet/servlet.WebToLead?encoding=UTF-8", content);
            return Ok(result);
        }

        [Route("SalesForceClienteNoCliente")]
        [HttpPost]
        public async Task<IHttpActionResult> SalesForceClienteNoCliente(Business.Model.DataSalesForce DatosCotizar)
        {
            HttpClient client = new HttpClient();

            client.BaseAddress = new Uri(UrlBase);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/xml"));
            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string,string>("oid",organizationID),
                new KeyValuePair<string,string>("retURL","https://toyotacredito.com.mx/."),
                new KeyValuePair<string,string>("state",DatosCotizar.Estado),
                //new KeyValuePair<string,string>("CodigoDistribuidor",DatosCotizar.CodigoDistribuidor),
                new KeyValuePair<string,string>("first_name",DatosCotizar.Nombre),
                new KeyValuePair<string,string>("mobile",DatosCotizar.Movil),
                new KeyValuePair<string,string>("email",DatosCotizar.Email),
                new KeyValuePair<string,string>("AceptoTerminosYCondiciones",DatosCotizar.AceptoTerminosYCondiciones),
                new KeyValuePair<string,string>("00Nf100000Ce1Le",DatosCotizar.CodigoDistribuidor),
                new KeyValuePair<string,string>("lead_source","Cotizador Web Cliente No Cliente"),
                new KeyValuePair<string,string>("submit","Enviar")
            });

            //Petición POST para generar el token
            var result = await client.PostAsync("/servlet/servlet.WebToLead?encoding=UTF-8", content);
            return Ok(result);
        }

        [Route("SalesForcePrecalificado")]
        [HttpPost]
        public async Task<IHttpActionResult> SalesForcePrecalificado(Business.Model.DataSalesForce DatosCotizar)
        {
            HttpClient client = new HttpClient();

            client.BaseAddress = new Uri(UrlBase);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/xml"));
            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string,string>("oid",organizationID),
                new KeyValuePair<string,string>("retURL","https://toyotacredito.com.mx/."),

                new KeyValuePair<string,string>("state",DatosCotizar.Estado),
                new KeyValuePair<string,string>("CodigoDistribuidor",DatosCotizar.CodigoDistribuidor),
                new KeyValuePair<string,string>("first_name",DatosCotizar.Nombre),
                new KeyValuePair<string,string>("mobile",DatosCotizar.Movil),
                new KeyValuePair<string,string>("email",DatosCotizar.Email),
                new KeyValuePair<string,string>("AceptoTerminosYCondiciones",DatosCotizar.AceptoTerminosYCondiciones),

                new KeyValuePair<string,string>("00Nf100000Ce1Le","57900"),
                new KeyValuePair<string,string>("lead_source","Cotizador Web Cliente No Cliente"),
                new KeyValuePair<string,string>("submit","Enviar")
            });

            //Petición POST para generar el token
            var result = await client.PostAsync("/servlet/servlet.WebToLead?encoding=UTF-8", content);
            return Ok(result);
        }

        [Route("SalesForceDistribuidores")]
        [HttpPost]
        public async Task<IHttpActionResult> LeadDistribuidores(Business.Model.DataSalesForce data)
        {
            HttpClient client = new HttpClient();

            client.BaseAddress = new Uri(UrlBase);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/xml"));
            var content = new FormUrlEncodedContent(new[] {
                new KeyValuePair<string,string>("oid",organizationID),
                new KeyValuePair<string,string>("retURL","https://toyotacredito.com.mx/."),

                //new KeyValuePair<string,string>("CodigoDistribuidor",data.CodigoDistribuidor),
                new KeyValuePair<string,string>("first_name",data.Nombre),
                new KeyValuePair<string,string>("last_name",data.Apellido),
                new KeyValuePair<string,string>("phone",data.Movil),
                new KeyValuePair<string,string>("email",data.Email),

                new KeyValuePair<string,string>("00Nf100000Ce1Le",data.CodigoDistribuidor),
                new KeyValuePair<string,string>("lead_source","Web Distribuidores"),
                new KeyValuePair<string,string>("submit","Enviar")
            });

            //Petición POST para generar el token
            var result = await client.PostAsync("/servlet/servlet.WebToLead?encoding=UTF-8", content);
            return Ok(result);
        }

        public async Task<string> GetAccessToken()
        {
            HttpClient client = new HttpClient();

            client.BaseAddress = new Uri(UrlBase);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/xml"));
            var body = new FormUrlEncodedContent(new[] {
                new KeyValuePair<string,string>("grant_type","password"),
                /*PRODUCCION
                new KeyValuePair<string,string>("client_secret","FD754CE5B4C5478ECD6693F4BF0448E2F33431AC903AF3427F88ABA094E3786A"),
                new KeyValuePair<string,string>("username","srv_salesforce@toyota.com"),
                new KeyValuePair<string,string>("password","S4L3sF0Rs!#RTfgkLCGsxw43Ffb98lFah4c"),
                new KeyValuePair<string,string>("client_id","3MVG9fMtCkV6eLhcg6COLqeCqSL56UhEHwdJMyeGXWr.UkzQsjM89YD9YlZqHB4xFFtRi56V6Qf26RhTYb4Pv")
                 */
                //*/
                //* SALT
                /*
                new KeyValuePair<string,string>("client_secret","2EF4ABE7DF7ECACA5460B6D45157703E58C6495093F77BEDDFDE9C1EB9B28290"),
                new KeyValuePair<string,string>("username","sebastian.marmol@virtualdreams.io.salt001"),
                new KeyValuePair<string,string>("password","Sebas22!nynCmKj7Om6s7szeT3ZxuUcwM"),
                new KeyValuePair<string,string>("client_id","3MVG9GnaLrwG9TQRGlnlDs0akv1Wv8tqkoxMJtQ6xuCLQtFMnsIRFyYN_chGD9s34HOwtIqT6_dGMGYqEonBo")
                //*/
                new KeyValuePair<string,string>("client_secret",Environment.GetEnvironmentVariable("SitefinityClient_secret")),
                new KeyValuePair<string,string>("username",Environment.GetEnvironmentVariable("SitefinityUsername")),
                new KeyValuePair<string,string>("password",Environment.GetEnvironmentVariable("SitefinityPassword")),
                new KeyValuePair<string,string>("client_id",Environment.GetEnvironmentVariable("SitefinityClient_id"))
            });

            //Petición POST para generar el token
            var response = await client.PostAsync("/services/oauth2/token", body);
            var contents = await response.Content.ReadAsStringAsync();
            var res = JsonConvert.DeserializeObject<GetAccessTokenResponse>(contents);

            return res.access_token;
        }

        [Route("GetAccessToken")]
        [HttpGet]
        public async Task<IHttpActionResult> SendToken()
        {
            string token = await GetAccessToken();
            if (token != null)
                return Ok(new { result = token });
            else
                return Content(HttpStatusCode.NotFound, "Failed to obtain the Access Token");
        }

        [Route("submit-lead")]
        [HttpPost]
        public async Task<IHttpActionResult> SubmitLead(PlanForm form)
        {
            if (form == null)
            {
                return BadRequest();
            }

            string token = await GetAccessToken();

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            client.BaseAddress = new Uri(UrlBase);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var body = new Hashtable();
            body.Add("state", form.state);
            body.Add("FWY_Aseguradora__c", form.FWY_Aseguradora__c);
            body.Add("Mensualidad__c", form.Mensualidad__c);
            body.Add("Cobertura__c", form.Cobertura__c);
            body.Add("FWY_Tipo_de_plan__c", form.FWY_Tipo_de_plan__c);
            body.Add("MobilePhone", form.MobilePhone);
            body.Add("Email", form.Email);
            body.Add("FirstName", form.FirstName);
            body.Add("LastName", form.LastName);
            body.Add("FWY_Veh_culo__c", form.FWY_Veh_culo__c);
            body.Add("FWY_Versi_n__c", form.FWY_Versi_n__c);
            body.Add("FWY_Modelo__c", form.FWY_Modelo__c);
            body.Add("FWY_Tipo_de_persona__c", form.FWY_Tipo_de_persona__c);
            body.Add("FWY_Enganche_Monto__c", form.FWY_Enganche_Monto__c);
            body.Add("Plazo__c", form.Plazo__c);
            body.Add("FWY_Balloon__c", form.FWY_Balloon__c);
            body.Add("Depositos_Garantia__c", form.Depositos_Garantia__c);
            body.Add("Precio_Auto__c", form.Precio_Auto__c);
            body.Add("ImagenAuto__c", form.ImagenAuto__c);
            body.Add("LeadSource", form.LeadSource);
            body.Add("Company", form.Company);


            var stringPayload = JsonConvert.SerializeObject(body);

            // Wrap our JSON inside a StringContent which then can be used by the HttpClient class
            var httpContent = new StringContent(stringPayload, Encoding.UTF8, "application/json");

            /*
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
*/
            // Do the actual request and await the response
            var httpResponse = new HttpResponseMessage();


            httpResponse = await client.PostAsync("/services/data/v55.0/sobjects/Lead/", httpContent);

            // If the response contains content we want to read it!
            if (httpResponse.Content != null)
            {
                var responseContent = await httpResponse.Content.ReadAsStringAsync();

                // From here on you could deserialize the ResponseContent back again to a concrete C# type using Json.Net

                return Ok(responseContent);
            }



            return BadRequest();
        }

        [Route("inicio_sesion")]
        [HttpPost]
        public async Task<IHttpActionResult> InicioSesion(LoginForm form)
        {
            if (form == null)
            {
                return BadRequest();
            }

            string token = await GetAccessToken();

            var body = new Hashtable();
            body.Add("idCliente_email", form.idCliente_email);
            body.Add("password", form.password);
            body.Add("esCliente", form.esCliente);


            var stringPayload = JsonConvert.SerializeObject(body);

            var httpContent = new StringContent(stringPayload, Encoding.UTF8, "application/json");

            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            httpClient.BaseAddress = new Uri(UrlBase);

            var httpResponse = new HttpResponseMessage();


            httpResponse = await httpClient.PostAsync("/services/apexrest/SitefinityLoginWS", httpContent);

            if (httpResponse.Content != null)
            {
                var responseContent = await httpResponse.Content.ReadAsStringAsync();


                return Ok(responseContent);
            }


            return BadRequest();
        }

        [Route("coti")]
        [HttpPost]
        public async Task<IHttpActionResult> coti(PlanForm form)
        {
            if (form == null)
            {
                return BadRequest();
            }

            string token = await GetAccessToken();

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            client.BaseAddress = new Uri(UrlBase);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var body = new Hashtable();
            body.Add("email", form.Email);

            var json = JsonConvert.SerializeObject(body);
            using (var content = new StringContent(json, Encoding.UTF8, "application/json"))
            {
                var result = await client.PostAsync("/services/apexrest/sitefinity", content);
                if (result.StatusCode == HttpStatusCode.OK || result.StatusCode == HttpStatusCode.Created)
                {
                    var contents = await result.Content.ReadAsStringAsync();
                    JObject o = JObject.Parse(contents);

                    return Ok(o);
                }
                else
                {
                    // Do something with the contents, like write the statuscode and
                    // contents to a log file
                    string resultContent = await result.Content.ReadAsStringAsync();
                    // ... write to log
                    return BadRequest();
                }
            }
        }

        [Route("RegistroPC")]
        [HttpPost]
        public async Task<IHttpActionResult> RegistroPC(RegisForm form)
        {
            if (form == null)
            {
                return BadRequest();
            }

            string token = await GetAccessToken();

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            client.BaseAddress = new Uri(UrlBase);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var body = new Hashtable();

            body.Add("email", form.email);
            body.Add("rfcFull", form.rfcFull);
            if (form.idCliente != null)
            {
                body.Add("idCliente", form.idCliente);
            }
            else
            {
                body.Add("name", form.name);
                body.Add("lastName", form.lastName);
            }

            var json = JsonConvert.SerializeObject(body);
            using (var content = new StringContent(json, Encoding.UTF8, "application/json"))
            {
                var result = new HttpResponseMessage();
                if (form.idCliente != null)
                {
                    result = await client.PostAsync("/services/apexrest/SitefinityRegisterClientWS", content);
                }
                else
                {
                    result = await client.PostAsync("/services/apexrest/SitefinityRegisterProspectoWS", content);
                }

                if (result.StatusCode == HttpStatusCode.OK || result.StatusCode == HttpStatusCode.Created)
                {
                    var contents = await result.Content.ReadAsStringAsync();
                    JObject o = JObject.Parse(contents);

                    return Ok(o);
                }
                else
                {
                    string resultContent = await result.Content.ReadAsStringAsync();
                    return BadRequest();
                }
            }
        }

        [Route("Put-mail")]
        [HttpPost]
        public async Task<IHttpActionResult> PutMail(RegistroForm form)
        {
            if (form == null)
            {
                return BadRequest();
            }

            string token = await GetAccessToken();

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            client.BaseAddress = new Uri(UrlBase);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var body = new Hashtable();

            body.Add("email", form.email);
            body.Add("idUsuario", form.idUsuario);
            body.Add("url", form.url);
            try
            {
                var json = JsonConvert.SerializeObject(body);
                using (var content = new StringContent(json, Encoding.UTF8, "application/json"))
                {
                    var result = await client.PutAsync("/services/apexrest/SitefinityRegisterClientWS", content);


                    if (result.StatusCode == HttpStatusCode.OK || result.StatusCode == HttpStatusCode.Created)
                    {
                        var contents = await result.Content.ReadAsStringAsync();
                        //JObject o = JObject.Parse(contents);

                        return Ok(contents);
                    }
                    else
                    {
                        string resultContent = await result.Content.ReadAsStringAsync();
                        return BadRequest();
                    }
                }
            }
            catch (Exception e)
            {

            }
            return Ok();
        }

        [Route("SetPassword")]
        [HttpPost]
        public async Task<IHttpActionResult> SetPassword(LoginForm form)
        {
            if (form == null)
            {
                return BadRequest();
            }

            string token = await GetAccessToken();

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            client.BaseAddress = new Uri(UrlBase);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var body = new Hashtable();

            body.Add("rfc", form.rfc);
            body.Add("password", form.password);


            var json = JsonConvert.SerializeObject(body);
            using (var content = new StringContent(json, Encoding.UTF8, "application/json"))
            {
                var result = new HttpResponseMessage();

                result = await client.PostAsync("services/apexrest/SetPasswordSitefinity", content);

                if (result.StatusCode == HttpStatusCode.OK || result.StatusCode == HttpStatusCode.Created)
                {
                    var contents = await result.Content.ReadAsStringAsync();
                    //JObject o = JObject.Parse(contents);

                    return Ok(contents);
                }
                else
                {
                    string resultContent = await result.Content.ReadAsStringAsync();
                    return BadRequest();
                }
            }
        }

        [Route("UpdatePassword")]
        [HttpPost]
        public async Task<IHttpActionResult> UpdatePassword(RegistroForm form)
        {
            if (form == null)
            {
                return BadRequest();
            }

            string token = await GetAccessToken();

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            client.BaseAddress = new Uri(UrlBase);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var body = new Hashtable();

            if (form.clientVisible != null)
            {
                body.Add("idCliente", form.clientVisible);
            }
            body.Add("rfcFull", form.rfcVisible);
            body.Add("email", form.email);
            body.Add("url", form.url);


            var json = JsonConvert.SerializeObject(body);
            using (var content = new StringContent(json, Encoding.UTF8, "application/json"))
            {
                var result = new HttpResponseMessage();

                result = await client.PostAsync("services/apexrest/sitefinityForgotPwdWS", content);

                if (result.StatusCode == HttpStatusCode.OK || result.StatusCode == HttpStatusCode.Created)
                {
                    var contents = await result.Content.ReadAsStringAsync();
                    //JObject o = JObject.Parse(contents);

                    return Ok(contents);
                }
                else
                {
                    string resultContent = await result.Content.ReadAsStringAsync();
                    return BadRequest();
                }
            }
        }

        [Route("GetURLExternalScript")]
        [HttpGet]
        public IHttpActionResult generateURLExternalScript(string Cotizacion, string Dealer, string Descripcion, string Fase, string FechadeCompra, string FechadeSolicitud, string Motor, string NumerodeContrato, string NumerodeSolicitud, string Proceso, string RFCdelCliente, string TipodeDocumento, string TipodePersona, string UsuarioqueCarga, string VIN)
        {
            DateTime fechaCom;
            DateTime fechaSoli;

            //  VARIABLES REQUERIDAS
            string _secret = "xPoZOLwQl4N1j8At2Czz1mh8pVRKyNTxECaJ2iX5NXI=";
            string _baseURL1 = "https://mex.review.recall.com/IM39961UT1/203AppNet//UnityForm.aspx?d1=Ab28NDSVSfdMHKr7bl0ZEPM7UjDaSreemotHv6KS1qbZB4aCTZPyJye0JaHScRlZpciciKT07AORDpJX2GFrbLXNkajrcN8iitT3nZqPw5AsMosB7kZpM1FKDOrPRAlsBojSY0zYQ6DMEXhZxSV2SFy9E%2f2QiLC6NPMABd27cATD";
            string _params = string.Empty;

            // VARIABLES PARA CALCULO DE HASH
            byte[] tokenBytes = Convert.FromBase64String(_secret);
            byte[] calculatedHash = null;
            //SOLICITUD DE VALORES PARA PARAMETROS
            //Cotizacion
            if (!string.IsNullOrEmpty(Cotizacion))
            {
                _params += "&ufprecot=" + System.Uri.EscapeDataString(Cotizacion);
            }
            //Dealer
            if (!string.IsNullOrEmpty(Dealer))
            {
                _params += "&ufpredeal=" + System.Uri.EscapeDataString(Dealer);
            }
            //Descripcion
            if (!string.IsNullOrEmpty(Descripcion))
            {
                _params += "&ufpredscr=" + System.Uri.EscapeDataString(Descripcion);
            }
            //Fase
            if (!string.IsNullOrEmpty(Fase))
            {
                _params += "&ufprefase=" + System.Uri.EscapeDataString(Fase);
            }
            //FechadeCompra
            if (!string.IsNullOrEmpty(FechadeCompra))
            {

                if (DateTime.TryParseExact(FechadeCompra, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out fechaCom) || DateTime.TryParseExact(FechadeCompra, "dd-MM-yyyy", null, System.Globalization.DateTimeStyles.None, out fechaCom))
                {
                    string fcom = fechaCom.ToString("yyyy-MM-dd");
                    _params += "&ufprefcom=" + System.Uri.EscapeDataString(fcom);
                }
            }
            //FechadeSolicitud
            if (!string.IsNullOrEmpty(FechadeSolicitud))
            {
                if (DateTime.TryParseExact(FechadeSolicitud, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out fechaSoli) || DateTime.TryParseExact(FechadeSolicitud, "dd-MM-yyyy", null, System.Globalization.DateTimeStyles.None, out fechaSoli))
                {
                    string fsol = fechaSoli.ToString("yyyy-MM-dd");
                    _params += "&ufprefsol=" + System.Uri.EscapeDataString(fsol);
                }
            }
            //Motor
            if (!string.IsNullOrEmpty(Motor))
            {
                _params += "&ufpremotr=" + System.Uri.EscapeDataString(Motor);
            }
            //NumerodeContrato
            if (!string.IsNullOrEmpty(NumerodeContrato))
            {
                _params += "&ufprencont=" + System.Uri.EscapeDataString(NumerodeContrato);
            }
            //NumerodeSolicitud
            if (!string.IsNullOrEmpty(NumerodeSolicitud))
            {
                _params += "&ufprensol=" + System.Uri.EscapeDataString(NumerodeSolicitud);
            }
            //Proceso
            if (!string.IsNullOrEmpty(Proceso))
            {
                _params += "&ufpreproc=" + System.Uri.EscapeDataString(Proceso);
            }
            //RFCdelCliente
            if (!string.IsNullOrEmpty(RFCdelCliente))
            {
                _params += "&ufprerfc=" + System.Uri.EscapeDataString(RFCdelCliente);
            }
            //TipodeDocumento
            if (!string.IsNullOrEmpty(TipodeDocumento))
            {
                _params += "&ufpretdoc=" + System.Uri.EscapeDataString(TipodeDocumento);
            }
            //TipodePersona
            if (!string.IsNullOrEmpty(TipodePersona))
            {
                _params += "&ufpretper=" + System.Uri.EscapeDataString(TipodePersona);
            }
            //UsuarioqueCarga
            if (!string.IsNullOrEmpty(UsuarioqueCarga))
            {
                _params += "&ufpreucrg=" + System.Uri.EscapeDataString(UsuarioqueCarga);
            }
            //VIN
            if (!string.IsNullOrEmpty(VIN))
            {
                _params += "&ufprevin=" + System.Uri.EscapeDataString(VIN);
            }



            // PROCESAMIENTO DE HASH

            using (HMACSHA256 hmac = new HMACSHA256(tokenBytes))
            {
                calculatedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(_params));
            }
            string hashString = System.Uri.EscapeDataString(Convert.ToBase64String(calculatedHash));

            // CONSTRUCCIÓN DE URL FINAL
            string url1 = _baseURL1 + _params + "&ufprehash=" + hashString;
            //url1 = "hola";

            return Ok(url1);

        }

        [Route("GetURLExternalScript2")]
        [HttpGet]
        public IHttpActionResult generateURLExternalScript2(string Cotizacion, string Dealer, string Descripcion, string Fase, string FechadeCompra, string FechadeSolicitud, string Motor, string NumerodeContrato, string NumerodeSolicitud, string Proceso, string RFCdelCliente, string TipodeDocumento, string TipodePersona, string UsuarioqueCarga, string VIN)
        {
            /*
            if(Cotizacion == null || Dealer == null || Descripcion == null || Fase == null || FechadeCompra == null || FechadeSolicitud == null || Motor == null || NumerodeContrato == null || NumerodeSolicitud == null || Proceso == null || RFCdelCliente == null || TipodeDocumento == null || TipodePersona == null ||  UsuarioqueCarga == null || VIN == null)
            {
                throw new ArgumentException("O array de parâmetros deve conter exatamente 15 valores.");
            }
            */
            if (Cotizacion == null)
                Cotizacion = "";
            if (Dealer == null)
                Dealer = "";
            if (Descripcion == null)
                Descripcion = "";
            if (Fase == null)
                Fase = "";
            if (FechadeCompra == null)
                FechadeCompra = "";
            if (FechadeSolicitud == null)
                FechadeSolicitud = "";
            if (Motor == null)
                Motor = "";
            if (NumerodeContrato == null)
                NumerodeContrato = "";
            if (NumerodeSolicitud == null)
                NumerodeSolicitud = "";
            if (Proceso == null)
                Proceso = "";
            if (RFCdelCliente == null)
                RFCdelCliente = "";
            if (TipodeDocumento == null)
                TipodeDocumento = "";
            if (TipodePersona == null)
                TipodePersona = "";
            if (UsuarioqueCarga == null)
                UsuarioqueCarga = "";
            if (VIN == null)
                VIN = "";

            //  VARIABLES REQUERIDAS
            string _secret = "xPoZOLwQl4N1j8At2Czz1mh8pVRKyNTxECaJ2iX5NXI=";
            string _baseURL1 = "https://mex.review.recall.com/IM39961UT1/203AppNet/UnityForm.aspx?d1=AWgri4Eie5LrkNB8NIssxdlRJJ8t60kogGE4Q%2fkBi8EGYYlZqb6tMiGJZD2KlXKLzQxS%2bgMlgVP5xgvCCAC9MQhbfNhhkbCD1HuXsOhy%2b3zBQQ2IQzdDDLqL%2f1RZo%2faE0fupstu9Q93ITWho%2fvhQpifceGy4bywbsmlpfNJkFqtp";
            string _params = string.Empty;

            // VARIABLES PARA CALCULO DE HASH
            byte[] tokenBytes = Convert.FromBase64String(_secret);
            byte[] calculatedHash = null;
            //SOLICITUD DE VALORES PARA PARAMETROS
            //Cotizacion
            _params += "&ufprecot=" + System.Uri.EscapeDataString(Cotizacion);
            //Dealer
            _params += "&ufpredeal=" + System.Uri.EscapeDataString(Dealer);
            //Descripcion
            _params += "&ufpredscr=" + System.Uri.EscapeDataString(Descripcion);
            //Fase
            _params += "&ufprefase=" + System.Uri.EscapeDataString(Fase);
            //FechadeCompra
            _params += "&ufprefcom=" + System.Uri.EscapeDataString(FechadeCompra);
            //FechadeSolicitud
            _params += "&ufprefsol=" + System.Uri.EscapeDataString(FechadeSolicitud);
            //Motor
            _params += "&ufpremotr=" + System.Uri.EscapeDataString(Motor);
            //NumerodeContrato
            _params += "&ufprencont=" + System.Uri.EscapeDataString(NumerodeContrato);
            //NumerodeSolicitud
            _params += "&ufprensol=" + System.Uri.EscapeDataString(NumerodeSolicitud);
            //Proceso
            _params += "&ufpreproc=" + System.Uri.EscapeDataString(Proceso);
            //RFCdelCliente
            _params += "&ufprerfc=" + System.Uri.EscapeDataString(RFCdelCliente);
            //TipodeDocumento
            _params += "&ufpretdoc=" + System.Uri.EscapeDataString(TipodeDocumento);
            //TipodePersona
            _params += "&ufpretper=" + System.Uri.EscapeDataString(TipodePersona);
            //UsuarioqueCarga
            _params += "&ufpreucrg=" + System.Uri.EscapeDataString(UsuarioqueCarga);
            //VIN
            _params += "&ufprevin=" + System.Uri.EscapeDataString(VIN);


            // PROCESAMIENTO DE HASH

            using (HMACSHA256 hmac = new HMACSHA256(tokenBytes))
            {
                calculatedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(_params));
            }
            string hashString = System.Uri.EscapeDataString(Convert.ToBase64String(calculatedHash));

            // CONSTRUCCIÓN DE URL FINAL
            string url1 = _baseURL1 + _params + "&ufprehash=" + hashString;

            return Ok(url1);

        }

        #endregion

        #region Disribuidores

        [Route("GetCountryId")]
        [HttpGet]
        public async Task<IHttpActionResult> GetCountryId(string address)
        {
            var cat = await BssCStates.GetCountryID(address);
            if (cat != null)
                return Ok(new { results = cat });
            else
                return NotFound();
        }

        [Route("GetDealersById")]
        [HttpGet]
        public async Task<IHttpActionResult> GetCatDealersById(int id)
        {
            var cat = await BssDealers.GetDealers(id);
            if (cat != null)
                return Ok(new { results = cat });
            else
                return NotFound();
        }

        [Route("GetDealers")]
        [HttpGet]
        public async Task<IHttpActionResult> GetDealers()
        {
            try
            {
                var cat = await BssDealers.GetDealers();
                if (cat != null)
                    return Ok(new { results = cat });
                else
                    return NotFound();
            }
            catch (Exception e)
            {
                return BadRequest();
            }
        }

        [Route("GetDealersByState")]
        public async Task<IHttpActionResult> GetDealersByState()
        {
            var dealers = await BssDealers.GetDealers();
            var states = BssCStates.GetAll();
            List<DealersByStateResponse> res = new List<DealersByStateResponse>();

            foreach (var state in states)
            {
                DealersByStateResponse x = new DealersByStateResponse();
                x.EstadoId = state.id_estado;
                x.Descripcion = state.descripcion;
                x.CVE = state.cve;
                x.Distribuidores = dealers.Where(y => y.IdState == state.id_estado).OrderBy(y => y.Dealer).ToList();
                res.Add(x);
            }

            if (dealers != null && states != null)
                return Ok(new { results = res });
            else
                return NotFound();
        }

        [Route("GetDealersByStateId")]
        public async Task<IHttpActionResult> GetDealersByStateId(int stateId)
        {
            var dealers = await BssDealers.GetDealers();
            List<tbl_portal_Distribuidores> res = dealers.Where(x => x.IdState == stateId).ToList();

            if (res != null)
                return Ok(new { results = res });
            else
                return NotFound();
        }

        [Route("GetDealersByPostalCode")]
        public async Task<IHttpActionResult> GetDealersByPostalCode(string pc)
        {
            var dealers = await BssDealers.GetDealers();
            List<tbl_portal_Distribuidores> res = dealers.Where(x => x.PostalCode != null && x.PostalCode.Contains(pc)).ToList();

            if (res != null)
                return Ok(new { results = res });
            else
                return NotFound();
        }

        [Route("GetDealersByStateName")]
        public async Task<IHttpActionResult> GetDealersByStateName(string stateName)
        {
            // Obtener la lista de distribuidores
            var dealers = await BssDealers.GetDealers();

            // Buscar el id del estado correspondiente al nombre dado
            var states = BssCStates.GetAll();
            List<tbl_portal_C_Estados> state = states.Where(s => s.descripcion.ToLowerInvariant().Replace("á", "a").Replace("é", "e").Replace("í", "i").Replace("ó", "o").Replace("ú", "u") == stateName.ToLowerInvariant()).ToList();

            if (state == null)
            {
                return NotFound();
            }

            // Encontrar los distribuidores correspondientes al estado dado
            var res = dealers.Where(x => x.IdState == state[0].id_estado).ToList();

            if (res.Count > 0)
            {
                return Ok(new { results = res });
            }
            else
            {
                return NotFound();
            }
        }

        [Route("SetDealersPostalCode")]
        [HttpPost]
        public IHttpActionResult SetDealersPostalCode(List<DealerPostalCode> data)
        {
            try
            {
                BssDealers.SetDealersPostalCode(data);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest();
            }
        }

        [Route("getApiKey")]
        [HttpGet]
        public async Task<IHttpActionResult> GetApiKey()
        {
            //return Ok(new { results = "AIzaSyAXcPfvcjvg30JnlXggadE6_jbjnsQvCTw" });
            // Accede a la variable de entorno
            string apiKey = Environment.GetEnvironmentVariable("SitefinityAPI_KEY");

            if (!string.IsNullOrEmpty(apiKey))
            {
                return Ok(new { results = apiKey });
            }
            else
            {
                return NotFound();
            }
        }

        [Route("recaptchaSiteKey")]
        // Endpoint para obtener la clave del sitio ReCaptcha
        [HttpGet]
        public async Task<IHttpActionResult> GetReCaptchaSiteKey()
        {
            // Aquí obtienes la clave del sitio ReCaptcha desde la variable de entorno
            string recaptchaSiteKey = Environment.GetEnvironmentVariable("SitefinityRECAPTCHA_SITE_KEY");
            return Ok(recaptchaSiteKey);
        }

        [Route("recaptchaSecretKey")]
        // Endpoint para obtener la clave secreta de ReCaptcha
        [HttpGet]
        public async Task<IHttpActionResult> GetReCaptchaSecretKey()
        {
            // Aquí obtienes la clave secreta de ReCaptcha desde la variable de entorno
            string recaptchaSecretKey = Environment.GetEnvironmentVariable("SitefinityRECAPTCHA_SECRET_KEY");
            return Ok(recaptchaSecretKey);
        }

        #endregion

        #region Crypto

        [Route("Decrypt")]
        [HttpGet]
        public async Task<IHttpActionResult> Decrypt(string cypherText)
        {
            var decryptText = await Cypher.DecryptStringAES(cypherText);
            if (decryptText != null)
                return Ok(decryptText);
            else
                return InternalServerError();
        }

        [Route("Decrypt-String")]
        [HttpGet]
        public IHttpActionResult DecryptString(byte[] cypherText, byte[] key, byte[] iv)
        {
            var decryptText = BssCypher.DecryptStringFromBytes(cypherText, key, iv);
            if (decryptText != null)
                return Ok(decryptText);
            else
                return InternalServerError();
        }

        //GetKey
        [Route("GetKeyDecrypt")]
        [HttpGet]
        public async Task<IHttpActionResult> GetKeyDecrypt()
        {
            var decryptText = await Cypher.GetKey();
            if (decryptText != null)
                return Ok(decryptText);
            else
                return Ok("8080808080808080");
        }
        #endregion

        #region Email
        [Route("email")]
        [HttpGet]
        public IHttpActionResult email(string email, string user)
        {
            if (SendEmail(email, user))
                return Ok();
            else
                return NotFound();
        }

        protected bool SendEmail(string email, string user)
        {
            try
            {
                //calling for creating the email body with html template   
                string body = this.createEmailBody("Felicidades tienes un crédito Precalificado", user);
                return SendHtmlFormattedEmail("Estás a un paso de obtener el toyota de tus sueños", body, email);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private string createEmailBody(string title, string user)
        {
            string body = string.Empty;
            //using streamreader for reading my htmltemplate   

            using (StreamReader reader = new StreamReader(System.Web.HttpContext.Current.Server.MapPath("~/Mail Templates/newsletter.html")))
            {
                body = reader.ReadToEnd();
            }

            body = body.Replace("{Title}", title);
            body = body.Replace("{User}", user);

            return body;
        }

        private bool SendHtmlFormattedEmail(string subject, string body, string email)
        {
            try
            {
                using (MailMessage mailMessage = new MailMessage())
                {
                    mailMessage.From = new MailAddress(ConfigurationManager.AppSettings["UserName"]);

                    mailMessage.Subject = subject;

                    mailMessage.Body = body;

                    mailMessage.IsBodyHtml = true;

                    mailMessage.To.Add(new MailAddress(email));

                    SmtpClient smtp = new SmtpClient();

                    smtp.Host = ConfigurationManager.AppSettings["Host"];

                    smtp.EnableSsl = Convert.ToBoolean(ConfigurationManager.AppSettings["EnableSsl"]);

                    System.Net.NetworkCredential NetworkCred = new System.Net.NetworkCredential();

                    NetworkCred.UserName = ConfigurationManager.AppSettings["UserName"]; //reading from web.config  

                    NetworkCred.Password = ConfigurationManager.AppSettings["Password"]; //reading from web.config  

                    smtp.UseDefaultCredentials = true;

                    smtp.Credentials = NetworkCred;

                    smtp.Port = int.Parse(ConfigurationManager.AppSettings["Port"]); //reading from web.config  

                    smtp.Send(mailMessage);
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
                //throw ex;
            }
        }

        #endregion

        #region BuroWS
        [Route("SendData")]
        [HttpPost]
        public async Task<IHttpActionResult> SendDataBuro()
        {

            try
            {
                if (Request.Content.IsMimeMultipartContent())
                {
                    BuroCreditoBss creditoBss = new BuroCreditoBss(); BssCypher cypher = new BssCypher();
                    var stringJson = await cypher.DecryptStringAES(HttpContext.Current.Request.Params["hfData"]);

                    if (stringJson != "")
                    {
                        //DesSerializarlo
                        var result = JsonConvert.DeserializeObject<DXC.TFSM.Business.Model.BuroData>(stringJson);
                        //Enviarlo al servicio de buró. Y procesar la respuesta del servicio externo de buró:
                        var dataruesult = await creditoBss.BuroWS(result);
                    }
                    else
                    {
                        return NotFound();
                    }
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception)
            {
                return InternalServerError();
            }
            return NotFound();
        }

        [Route("SendDataBuroTest")]
        [HttpPost]
        public async Task<IHttpActionResult> SendDataBuroTest(DXC.TFSM.Business.Model.BuroData xxx)
        {

            BuroCreditoBss creditoBss = new BuroCreditoBss();
            BssCypher cypher = new BssCypher();
            try
            {
                var stringJson = await cypher.DecryptStringAES(xxx.ApellidoP);
                if (stringJson != "")
                {
                    //DesSerializarlo
                    var decode = JsonConvert.DeserializeObject<DXC.TFSM.Business.Model.BuroData>(stringJson);
                    //Enviarlo al servicio de buró. Y procesar la respuesta del servicio externo de buró:
                    var result = await creditoBss.BuroWS(decode);
                    return Ok(result);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                return InternalServerError();
                throw ex;
            }
        }

        [Route("getFilePdf")]
        [HttpGet]
        public HttpResponseMessage GetFile(string nombre, bool aprobado)
        {
            BuroCreditoBss creditoBss = new BuroCreditoBss();
            creditoBss.updateDataHTML(nombre);
            string localFilePath;

            localFilePath = "C:/Users/QUALITY/Source/Workspaces/DXC/DXC.TFSM.Services/DXC.TFSM.Services/Views/PDF/Response.html";

            PdfConverter pdfConverter = new PdfConverter();
            pdfConverter.LicenseKey = "99zF18/XwcXXxsXZx9fExtnGxdnOzs7O";
            byte[] downloadBytes = pdfConverter.GetPdfFromUrlBytes(localFilePath);
            var dataStream = new MemoryStream(downloadBytes);
            HttpResponseMessage httpResponseMessage = Request.CreateResponse(HttpStatusCode.OK);
            httpResponseMessage.Content = new StreamContent(dataStream, downloadBytes.Length);
            httpResponseMessage.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
            httpResponseMessage.Content.Headers.ContentDisposition.FileName = "responsePDF.pdf";
            httpResponseMessage.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");

            return httpResponseMessage;
        }

        [Route("GetCountriesBuro")]
        [HttpGet]
        public IHttpActionResult GetCountriesBuro()
        {
            return Ok(new { results = buroFrm.GetCountries() }); ;
        }

        [Route("GetStatesBuro")]
        [HttpGet]
        public IHttpActionResult GetStatesBuro(int id)
        {
            return Ok(new { results = buroFrm.GetTbl_Cotizador_states(id) });
        }
        #endregion

        #region download_pdf

        [Route("DownloadPlanPdf")]
        [HttpPost]
        public IHttpActionResult DownloadPDF(RequestPDF dataPdf)
        {
            try
            {
                byte[] res = new byte[] { };
                switch (dataPdf.DatosCotizar[0].Plan)
                {
                    case "Tradicional":
                        res = ServicePDF.getPlanTradicionalPdf2(dataPdf);
                        break;
                    case "Balloon":
                        res = ServicePDF.getPlanBalloonPdf(dataPdf);
                        break;
                    case "Anualidades":
                        res = ServicePDF.getPlanAnualidadesPdf(dataPdf);
                        break;
                    case "Arrendamiento financiero":
                        res = ServicePDF.getFinancieroPdf(dataPdf);
                        break;
                    case "Arrendamiento puro":
                        res = ServicePDF.getPuroPdf(dataPdf);
                        break;
                    case "Test":
                        res = ServicePDF.asposePdf(dataPdf);
                        break;
                }

                if (res != null && res.Count() > 0)
                {
                    return Ok(new { result = res });
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception e)
            {
                String msjError = "e.Message:" + e.Message + "\n e.Source: " + e.Source + "\n e.StackTrace:" + e.StackTrace;
                return Content(HttpStatusCode.BadRequest, msjError);
            }
        }

        #endregion
    }
}
