using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DXC.TFSM.Services.Models.Forms
{
    public class PlanForm
    {

        public string state { get; set; }
        public string FWY_Aseguradora__c { get; set; }
        public float Mensualidad__c { get; set; }
        public string Cobertura__c { get; set; }
        public string FWY_Tipo_de_plan__c { get; set; }
        public string MobilePhone { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FWY_Veh_culo__c { get; set; }
        public string FWY_Versi_n__c { get; set; }
        public string FWY_Modelo__c { get; set; }
        public string FWY_Tipo_de_persona__c { get; set; }
        public float FWY_Enganche_Monto__c { get; set; }
        public string Plazo__c { get; set; }
        public string FWY_Balloon__c { get; set; }
        public float Depositos_Garantia__c { get; set; }
        public float Precio_Auto__c { get; set; }
        public string ImagenAuto__c { get; set; }
        public string LeadSource { get; set; }
        public string Company { get; set; }
        public string Idcoti { get; set; }
    }
}