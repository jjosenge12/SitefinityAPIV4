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
    
    public partial class sf_security_roots
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public sf_security_roots()
        {
            this.sf_scrty_rts_prmssnst_bjct_ttl = new HashSet<sf_scrty_rts_prmssnst_bjct_ttl>();
            this.sf_scrty_rts_spprtd_prmssn_sts = new HashSet<sf_scrty_rts_spprtd_prmssn_sts>();
            this.sf_permissions = new HashSet<sf_permissions>();
        }
    
        public Nullable<System.DateTime> last_modified { get; set; }
        public string ky { get; set; }
        public System.Guid id { get; set; }
        public byte can_inherit_permissions { get; set; }
        public string app_name { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<sf_scrty_rts_prmssnst_bjct_ttl> sf_scrty_rts_prmssnst_bjct_ttl { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<sf_scrty_rts_spprtd_prmssn_sts> sf_scrty_rts_spprtd_prmssn_sts { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<sf_permissions> sf_permissions { get; set; }
    }
}
