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
    
    public partial class sf_prs_criterion
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public sf_prs_criterion()
        {
            this.sf_prs_crtr_grp_sf_prs_crtrion = new HashSet<sf_prs_crtr_grp_sf_prs_crtrion>();
        }
    
        public int sf_prs_criterion_id { get; set; }
        public byte is_negated { get; set; }
        public string criterion_value { get; set; }
        public string criterion_title { get; set; }
        public string criterion_name { get; set; }
        public string criterion_display_value { get; set; }
        public short voa_version { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<sf_prs_crtr_grp_sf_prs_crtrion> sf_prs_crtr_grp_sf_prs_crtrion { get; set; }
    }
}
