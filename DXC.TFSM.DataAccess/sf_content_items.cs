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
    
    public partial class sf_content_items
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public sf_content_items()
        {
            this.sf_cntnt_tems_sf_language_data = new HashSet<sf_cntnt_tems_sf_language_data>();
            this.sf_cntnt_tms_pblshd_trnsltions = new HashSet<sf_cntnt_tms_pblshd_trnsltions>();
            this.sf_content_items_category = new HashSet<sf_content_items_category>();
            this.sf_content_items_sf_commnt = new HashSet<sf_content_items_sf_commnt>();
            this.sf_content_items_tags = new HashSet<sf_content_items_tags>();
            this.sf_permissions = new HashSet<sf_permissions>();
        }
    
        public decimal votes_sum { get; set; }
        public long votes_count { get; set; }
        public byte visible { get; set; }
        public int views_count { get; set; }
        public int vrsion { get; set; }
        public string url_name_ { get; set; }
        public string title_ { get; set; }
        public int status { get; set; }
        public string source_key { get; set; }
        public System.DateTime publication_date { get; set; }
        public int post_rights { get; set; }
        public Nullable<System.Guid> ownr { get; set; }
        public Nullable<System.Guid> original_content_id { get; set; }
        public Nullable<System.Guid> last_modified_by { get; set; }
        public Nullable<System.DateTime> last_modified { get; set; }
        public Nullable<byte> include_in_sitemap { get; set; }
        public System.Guid content_id { get; set; }
        public Nullable<System.DateTime> expiration_date { get; set; }
        public Nullable<byte> email_author { get; set; }
        public string draft_culture { get; set; }
        public string description_ { get; set; }
        public Nullable<System.Guid> default_page_id { get; set; }
        public Nullable<System.DateTime> date_created { get; set; }
        public string content_state { get; set; }
        public Nullable<byte> approve_comments { get; set; }
        public string app_name { get; set; }
        public Nullable<byte> allow_track_backs { get; set; }
        public Nullable<byte> allow_comments { get; set; }
        public string nme { get; set; }
        public byte inherits_permissions { get; set; }
        public string content_ { get; set; }
        public byte can_inherit_permissions { get; set; }
        public string author { get; set; }
        public short voa_version { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<sf_cntnt_tems_sf_language_data> sf_cntnt_tems_sf_language_data { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<sf_cntnt_tms_pblshd_trnsltions> sf_cntnt_tms_pblshd_trnsltions { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<sf_content_items_category> sf_content_items_category { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<sf_content_items_sf_commnt> sf_content_items_sf_commnt { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<sf_content_items_tags> sf_content_items_tags { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<sf_permissions> sf_permissions { get; set; }
    }
}
