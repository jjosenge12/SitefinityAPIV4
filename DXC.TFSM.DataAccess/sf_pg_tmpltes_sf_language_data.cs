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
    
    public partial class sf_pg_tmpltes_sf_language_data
    {
        public System.Guid id { get; set; }
        public int seq { get; set; }
        public Nullable<System.Guid> id2 { get; set; }
    
        public virtual sf_language_data sf_language_data { get; set; }
        public virtual sf_page_templates sf_page_templates { get; set; }
    }
}
