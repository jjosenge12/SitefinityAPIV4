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
    
    public partial class sf_events_category
    {
        public System.Guid content_id { get; set; }
        public int seq { get; set; }
        public Nullable<System.Guid> val { get; set; }
    
        public virtual sf_events sf_events { get; set; }
    }
}
