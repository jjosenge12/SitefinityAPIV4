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
    
    public partial class sf_pblshng_pp_sttngs_cntnt_lnk
    {
        public System.Guid id { get; set; }
        public int seq { get; set; }
        public Nullable<System.Guid> val { get; set; }
    
        public virtual sf_publishing_pipe_settings sf_publishing_pipe_settings { get; set; }
    }
}
