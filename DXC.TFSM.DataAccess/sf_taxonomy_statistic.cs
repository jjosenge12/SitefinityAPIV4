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
    
    public partial class sf_taxonomy_statistic
    {
        public Nullable<System.Guid> taxonomy_id { get; set; }
        public Nullable<System.Guid> taxon_id { get; set; }
        public int statistic_type { get; set; }
        public long marked_items_count { get; set; }
        public string item_provider_name { get; set; }
        public System.Guid id { get; set; }
        public string data_item_type { get; set; }
        public string application_name { get; set; }
        public short voa_version { get; set; }
    }
}
