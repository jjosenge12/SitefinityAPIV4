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
    
    public partial class sf_subscriber
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public sf_subscriber()
        {
            this.sf_lst = new HashSet<sf_lst>();
        }
    
        public string short_id { get; set; }
        public string last_name { get; set; }
        public System.DateTime last_modified { get; set; }
        public byte is_suspended { get; set; }
        public System.Guid id { get; set; }
        public string first_name { get; set; }
        public string email { get; set; }
        public System.DateTime date_created { get; set; }
        public string application_name { get; set; }
        public short voa_version { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<sf_lst> sf_lst { get; set; }
    }
}
