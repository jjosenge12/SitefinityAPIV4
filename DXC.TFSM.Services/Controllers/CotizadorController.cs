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

namespace DXC.TFSM.Services.Controllers
{
    [RoutePrefix("api/tfsm")]
    public class CotizadorController : ApiController
    {
        #region Global Variables
        readonly BssCars BssAutos = new BssCars();
        readonly string UrlBase = "https://toyotafinancial.my.salesforce.com/";
        readonly string UrlLogin = "https://login.salesforce.com/";
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
                new KeyValuePair<string,string>("oid","00Dj0000000KZrJ"),
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
                new KeyValuePair<string,string>("Phone",DatosCotizar.Movil),
                new KeyValuePair<string,string>("00N3D000005eK3o",DatosCotizar.Plazo),
                new KeyValuePair<string,string>("email",DatosCotizar.Email),

                new KeyValuePair<string,string>("AceptoTerminosYCondiciones",DatosCotizar.AceptoTerminosYCondiciones),
                new KeyValuePair<string,string>("CodigoDistribuidor",DatosCotizar.CodigoDistribuidor),
                new KeyValuePair<string,string>("EstadoSeleccionado",DatosCotizar.EstadoSeleccionado),

                new KeyValuePair<string,string>("00Nf100000Ce1Le",DatosCotizar.CodigoDistribuidor),
                new KeyValuePair<string,string>("lead_source","Cotizador Web"),
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
                new KeyValuePair<string,string>("oid","00Dj0000000KZrJ"),
                new KeyValuePair<string,string>("retURL","https://toyotacredito.com.mx/."),

                new KeyValuePair<string,string>("FWY_Aseguradora__c",DatosCotizar.Aseguradora),
                new KeyValuePair<string,string>("Cobertura__c",DatosCotizar.Cobertura),
                new KeyValuePair<string,string>("state",DatosCotizar.Estado),
                new KeyValuePair<string,string>("FWY_Tipo_de_plan__c",DatosCotizar.Plan),
                new KeyValuePair<string,string>("FWY_Veh_culo__c",DatosCotizar.Marca),
                new KeyValuePair<string,string>("FWY_Versi_n__c",DatosCotizar.Modelo),
                new KeyValuePair<string,string>("FWY_Modelo__c",DatosCotizar.Vesion),
                new KeyValuePair<string,string>("FWY_Tipo_de_persona__c",DatosCotizar.TipoPersona),
                new KeyValuePair<string,string>("FWY_Enganche_Monto__c",DatosCotizar.Enganche),
                new KeyValuePair<string,string>("Depositos_Garantia__c",DatosCotizar.DepositoGarantia),
                new KeyValuePair<string,string>("FWY_Balloon__c",DatosCotizar.Ballon),
                new KeyValuePair<string,string>("Plazo__c",DatosCotizar.Plazo),
                new KeyValuePair<string,string>("Mensualidad__c",DatosCotizar.Mensualidad),
                new KeyValuePair<string,string>("first_name",DatosCotizar.Nombre),
                new KeyValuePair<string,string>("last_name",DatosCotizar.Apellido),
                new KeyValuePair<string,string>("email",DatosCotizar.Email),
                new KeyValuePair<string,string>("Precio_Auto__c",DatosCotizar.Precio),
                new KeyValuePair<string,string>("ImagenAuto__c",DatosCotizar.ImagenAuto),
                new KeyValuePair<string,string>("visibleTFSM__c", "1"),
                new KeyValuePair<string,string>("AceptoTerminosYCondiciones",DatosCotizar.AceptoTerminosYCondiciones),
                new KeyValuePair<string,string>("FWY_codigo_distribuidor__c",DatosCotizar.CodigoDistribuidor),
                new KeyValuePair<string,string>("lead_source","Cotizador Web paso 4"),
                new KeyValuePair<string,string>("submit","Enviar")
            });

            //Petición POST para generar el token
            var result = await client.PostAsync("/services/data/v50.0/sobjects/lead", content);///servlet/servlet.WebToLead?encoding=UTF-8
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
                new KeyValuePair<string,string>("oid","00Dj0000000KZrJ"),
                new KeyValuePair<string,string>("retURL","https://toyotacredito.com.mx/."),

                //new KeyValuePair<string,string>("state",DatosCotizar.Estado),
                new KeyValuePair<string,string>("00Nf100000Ce1La",DatosCotizar.Plan),
                new KeyValuePair<string,string>("FWY_Veh_culo__c",DatosCotizar.Marca),
                new KeyValuePair<string,string>("first_name",DatosCotizar.Nombre),
                new KeyValuePair<string,string>("last_name",DatosCotizar.Apellido),
                new KeyValuePair<string,string>("Phone",DatosCotizar.Movil),
                new KeyValuePair<string,string>("email",DatosCotizar.Email),
                new KeyValuePair<string,string>("FWY_Nombre_distribuidor__c",DatosCotizar.Aseguradora),
                new KeyValuePair<string,string>("Company",DatosCotizar.Aseguradora),
                new KeyValuePair<string,string>("visibleTFSM__c", "1"),
                new KeyValuePair<string,string>("AceptoTerminosYCondiciones",DatosCotizar.AceptoTerminosYCondiciones),
                new KeyValuePair<string,string>("00Nf100000Ce1Le",DatosCotizar.CodigoDistribuidor),
                new KeyValuePair<string,string>("lead_source","Formulario de " + DatosCotizar.Plan),
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
                new KeyValuePair<string,string>("oid","00Dj0000000KZrJ"),
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
                new KeyValuePair<string,string>("oid","00D3D000000AJDJ"),
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
                new KeyValuePair<string,string>("oid","00D3D000000AJDJ"),
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
                new KeyValuePair<string,string>("oid","00D3D000000AJDJ"),
                new KeyValuePair<string,string>("retURL","https://toyotacredito.com.mx/."),

                //new KeyValuePair<string,string>("CodigoDistribuidor",data.CodigoDistribuidor),
                new KeyValuePair<string,string>("first_name",data.Nombre),
                new KeyValuePair<string,string>("last_name",data.Apellido),
                new KeyValuePair<string,string>("Phone",data.Movil),
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

            client.BaseAddress = new Uri(UrlLogin);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/xml"));
            var body = new FormUrlEncodedContent(new[] {
                new KeyValuePair<string,string>("grant_type","password"),
                new KeyValuePair<string,string>("client_secret","FD754CE5B4C5478ECD6693F4BF0448E2F33431AC903AF3427F88ABA094E3786A"),
                new KeyValuePair<string,string>("username","srv_salesforce@toyota.com"),
                new KeyValuePair<string,string>("password","S4L3sF0Rs!#RTfgkLCGsxw43Ffb98lFah4c"),
                new KeyValuePair<string,string>("client_id","3MVG9fMtCkV6eLhcg6COLqeCqSL56UhEHwdJMyeGXWr.UkzQsjM89YD9YlZqHB4xFFtRi56V6Qf26RhTYb4Pv")
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

            client.BaseAddress = new Uri("https://toyotafinancial.my.salesforce.com");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var body = new Hashtable();
            body.Add("FirstName", form.Firstname);
            body.Add("LastName", form.Lastname);
            body.Add("FWY_codigo_distribuidor__c", form.DealerId.ToString());
            body.Add("FWY_Nombre_distribuidor__c", form.Dealer);
            body.Add("Company", form.Dealer);
            body.Add("FWY_Veh_culo__c", string.IsNullOrEmpty(form.Vehicle) ? "RAV4" : form.Vehicle);
            body.Add("Phone", form.Phone);
            body.Add("Email", form.Email);
            body.Add("FWY_Tipo_de_plan__c", form.Plan);

            var json = JsonConvert.SerializeObject(body);
            using (var content = new StringContent(json, Encoding.UTF8, "application/json"))
            {
                var result = await client.PostAsync("/services/data/v50.0/sobjects/lead", content);
                if (result.StatusCode == HttpStatusCode.OK || result.StatusCode == HttpStatusCode.Created)
                {
                    return Ok(result);
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
                        res = ServicePDF.getPlanTradicionalPdf(dataPdf);
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
                return Content(HttpStatusCode.BadRequest, e.Message);
            }
        }

        #endregion
    }
}
