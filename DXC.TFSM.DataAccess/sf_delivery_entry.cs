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
    
    public partial class sf_delivery_entry
    {
        public int sf_delivery_entry_id { get; set; }
        public Nullable<System.Guid> subscriber_id { get; set; }
        public int delivery_status { get; set; }
        public System.DateTime delivery_date { get; set; }
        public Nullable<System.Guid> campaign_id { get; set; }
    }
}
