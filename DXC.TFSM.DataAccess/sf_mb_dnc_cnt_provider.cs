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
    
    public partial class sf_mb_dnc_cnt_provider
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public sf_mb_dnc_cnt_provider()
        {
            this.sf_permissions = new HashSet<sf_permissions>();
        }
    
        public string parent_secured_object_type { get; set; }
        public string parent_secured_object_title { get; set; }
        public Nullable<System.Guid> parent_secured_object_id { get; set; }
        public string nme { get; set; }
        public System.DateTime last_modified { get; set; }
        public byte inherits_permissions { get; set; }
        public System.Guid id { get; set; }
        public byte can_inherit_permissions { get; set; }
        public string application_name { get; set; }
        public short voa_version { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<sf_permissions> sf_permissions { get; set; }
    }
}
