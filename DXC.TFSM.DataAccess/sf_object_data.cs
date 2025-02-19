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
    
    public partial class sf_object_data
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public sf_object_data()
        {
            this.sf_permissions = new HashSet<sf_permissions>();
        }
    
        public int vrsn { get; set; }
        public short strategy { get; set; }
        public Nullable<System.Guid> parent_prop_id { get; set; }
        public Nullable<System.Guid> parent_id { get; set; }
        public string object_type { get; set; }
        public string multilingual_version { get; set; }
        public Nullable<System.DateTime> last_modified { get; set; }
        public byte is_backend_object { get; set; }
        public System.Guid id { get; set; }
        public string dictionary_key { get; set; }
        public int collection_index { get; set; }
        public string app_name { get; set; }
        public int voa_class { get; set; }
        public short voa_version { get; set; }
        public Nullable<System.Guid> sibling_id { get; set; }
        public Nullable<byte> shred { get; set; }
        public string place_holder { get; set; }
        public Nullable<System.Guid> personalization_segment_id { get; set; }
        public Nullable<System.Guid> ownr { get; set; }
        public Nullable<System.Guid> original_control_id { get; set; }
        public Nullable<byte> is_personalized { get; set; }
        public Nullable<byte> is_overrided_control { get; set; }
        public Nullable<byte> is_layout_control { get; set; }
        public Nullable<byte> is_data_source { get; set; }
        public Nullable<byte> inherits_permissions { get; set; }
        public Nullable<byte> editable { get; set; }
        public string description_ { get; set; }
        public Nullable<System.Guid> id2 { get; set; }
        public string caption_ { get; set; }
        public Nullable<byte> can_inherit_permissions { get; set; }
        public Nullable<System.Guid> base_control_id { get; set; }
        public Nullable<byte> allow_security { get; set; }
        public Nullable<System.Guid> personalization_master_id { get; set; }
        public Nullable<System.Guid> page_id { get; set; }
        public Nullable<byte> enable_override_for_control { get; set; }
        public Nullable<byte> enable_override_for_control2 { get; set; }
        public Nullable<byte> published { get; set; }
        public Nullable<System.Guid> content_id { get; set; }
        public Nullable<byte> published2 { get; set; }
        public Nullable<System.Guid> id3 { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<sf_permissions> sf_permissions { get; set; }
    }
}
