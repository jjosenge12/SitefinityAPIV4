//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DXC.TFSM.DataAccess
{
    using System;
    using System.Collections.Generic;
    
    public partial class sf_form_entry
    {
        public string lnguage { get; set; }
        public int status { get; set; }
        public string ip_address { get; set; }
        public System.DateTime started_on { get; set; }
        public Nullable<System.DateTime> submitted_on { get; set; }
        public Nullable<System.Guid> user_id { get; set; }
        public string user_provider { get; set; }
        public Nullable<int> voa_class { get; set; }
        public string referral_code { get; set; }
        public string app_name { get; set; }
        public System.DateTime date_created { get; set; }
        public Nullable<System.DateTime> expiration_date { get; set; }
        public System.Guid id { get; set; }
        public Nullable<System.DateTime> last_modified { get; set; }
        public Nullable<System.Guid> ownr { get; set; }
        public System.DateTime publication_date { get; set; }
        public byte visible { get; set; }
        public string source_key { get; set; }
        public Nullable<System.Guid> source_site_id { get; set; }
        public string source_site_name { get; set; }
        public string description_ { get; set; }
        public string title_ { get; set; }
        public short voa_version { get; set; }
    }
}
