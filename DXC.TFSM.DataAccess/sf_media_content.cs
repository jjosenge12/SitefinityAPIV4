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
    
    public partial class sf_media_content
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public sf_media_content()
        {
            this.sf_md_cntnt_pblshd_trnslations = new HashSet<sf_md_cntnt_pblshd_trnslations>();
            this.sf_md_content_sf_language_data = new HashSet<sf_md_content_sf_language_data>();
            this.sf_media_content_category2 = new HashSet<sf_media_content_category2>();
            this.sf_media_content_category = new HashSet<sf_media_content_category>();
            this.sf_media_content_category3 = new HashSet<sf_media_content_category3>();
            this.sf_media_content_sf_commnt = new HashSet<sf_media_content_sf_commnt>();
            this.sf_media_content_tags = new HashSet<sf_media_content_tags>();
            this.sf_media_content_tags2 = new HashSet<sf_media_content_tags2>();
            this.sf_media_content_tags3 = new HashSet<sf_media_content_tags3>();
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
        public Nullable<byte> was_published { get; set; }
        public byte lgcy_tmb_strg { get; set; }
        public byte uploaded { get; set; }
        public int tmb_vrsn { get; set; }
        public Nullable<System.Guid> parent_id { get; set; }
        public float ordinal { get; set; }
        public byte tmb_regen { get; set; }
        public string media_file_url_name_ { get; set; }
        public string item_default_url_ { get; set; }
        public byte inherits_permissions { get; set; }
        public Nullable<System.Guid> folder_id { get; set; }
        public byte can_inherit_permissions { get; set; }
        public string blob_storage { get; set; }
        public string author_ { get; set; }
        public string approval_workflow_state_ { get; set; }
        public int voa_class { get; set; }
        public short voa_version { get; set; }
        public string parts_ { get; set; }
        public string alternative_text_ { get; set; }
        public string url_name_es_mx { get; set; }
        public string url_name_en { get; set; }
        public string title_es_mx { get; set; }
        public string title_en { get; set; }
        public string description_es_mx { get; set; }
        public string description_en { get; set; }
        public string media_file_url_name_es_mx { get; set; }
        public string media_file_url_name_en { get; set; }
        public string item_default_url_es_mx { get; set; }
        public string item_default_url_en { get; set; }
        public string author_es_mx { get; set; }
        public string author_en { get; set; }
        public string approval_workflow_state_es_mx { get; set; }
        public string approval_workflow_state_en { get; set; }
        public string parts_es_mx { get; set; }
        public string parts_en { get; set; }
        public string alternative_text_es_mx { get; set; }
        public string alternative_text_en { get; set; }
        public string url_link { get; set; }
        public string url_link_en { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<sf_md_cntnt_pblshd_trnslations> sf_md_cntnt_pblshd_trnslations { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<sf_md_content_sf_language_data> sf_md_content_sf_language_data { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<sf_media_content_category2> sf_media_content_category2 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<sf_media_content_category> sf_media_content_category { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<sf_media_content_category3> sf_media_content_category3 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<sf_media_content_sf_commnt> sf_media_content_sf_commnt { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<sf_media_content_tags> sf_media_content_tags { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<sf_media_content_tags2> sf_media_content_tags2 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<sf_media_content_tags3> sf_media_content_tags3 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<sf_permissions> sf_permissions { get; set; }
    }
}
