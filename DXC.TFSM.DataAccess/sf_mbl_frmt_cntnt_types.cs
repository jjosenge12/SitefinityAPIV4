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
    
    public partial class sf_mbl_frmt_cntnt_types
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public sf_mbl_frmt_cntnt_types()
        {
            this.sf_mbl_frmt_dfntns = new HashSet<sf_mbl_frmt_dfntns>();
        }
    
        public Nullable<System.Guid> siteId { get; set; }
        public string name { get; set; }
        public System.DateTime last_modified { get; set; }
        public System.Guid id { get; set; }
        public string fullName { get; set; }
        public string displayName { get; set; }
        public string app_name { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<sf_mbl_frmt_dfntns> sf_mbl_frmt_dfntns { get; set; }
    }
}
