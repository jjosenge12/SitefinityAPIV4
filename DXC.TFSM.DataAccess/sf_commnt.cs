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
    
    public partial class sf_commnt
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public sf_commnt()
        {
            this.sf_blog_posts_sf_commnt = new HashSet<sf_blog_posts_sf_commnt>();
            this.sf_blogs_sf_commnt = new HashSet<sf_blogs_sf_commnt>();
            this.sf_commnt_parent_group_ids = new HashSet<sf_commnt_parent_group_ids>();
            this.sf_commnt_sf_commnt = new HashSet<sf_commnt_sf_commnt>();
            this.sf_commnt_sf_commnt1 = new HashSet<sf_commnt_sf_commnt>();
            this.sf_content_items_sf_commnt = new HashSet<sf_content_items_sf_commnt>();
            this.sf_form_description_sf_commnt = new HashSet<sf_form_description_sf_commnt>();
            this.sf_libraries_sf_commnt = new HashSet<sf_libraries_sf_commnt>();
            this.sf_list_items_sf_commnt = new HashSet<sf_list_items_sf_commnt>();
            this.sf_lists_sf_commnt = new HashSet<sf_lists_sf_commnt>();
            this.sf_media_content_sf_commnt = new HashSet<sf_media_content_sf_commnt>();
            this.sf_news_items_sf_commnt = new HashSet<sf_news_items_sf_commnt>();
            this.sf_events_sf_commnt = new HashSet<sf_events_sf_commnt>();
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
        public string website { get; set; }
        public string provider_name { get; set; }
        public string ip_address { get; set; }
        public string email { get; set; }
        public string content_ { get; set; }
        public string commented_item_type { get; set; }
        public System.Guid commented_item_i_d { get; set; }
        public int comment_status { get; set; }
        public string author_name_ { get; set; }
        public short voa_version { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<sf_blog_posts_sf_commnt> sf_blog_posts_sf_commnt { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<sf_blogs_sf_commnt> sf_blogs_sf_commnt { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<sf_commnt_parent_group_ids> sf_commnt_parent_group_ids { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<sf_commnt_sf_commnt> sf_commnt_sf_commnt { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<sf_commnt_sf_commnt> sf_commnt_sf_commnt1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<sf_content_items_sf_commnt> sf_content_items_sf_commnt { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<sf_form_description_sf_commnt> sf_form_description_sf_commnt { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<sf_libraries_sf_commnt> sf_libraries_sf_commnt { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<sf_list_items_sf_commnt> sf_list_items_sf_commnt { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<sf_lists_sf_commnt> sf_lists_sf_commnt { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<sf_media_content_sf_commnt> sf_media_content_sf_commnt { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<sf_news_items_sf_commnt> sf_news_items_sf_commnt { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<sf_events_sf_commnt> sf_events_sf_commnt { get; set; }
    }
}
