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
    
    public partial class sf_content_items_sf_commnt
    {
        public System.Guid content_id { get; set; }
        public int seq { get; set; }
        public Nullable<System.Guid> content_id2 { get; set; }
    
        public virtual sf_commnt sf_commnt { get; set; }
        public virtual sf_content_items sf_content_items { get; set; }
    }
}
