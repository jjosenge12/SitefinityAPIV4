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
    
    public partial class sf_forum_threads
    {
        public int views_count { get; set; }
        public string url_name_ { get; set; }
        public string title { get; set; }
        public int threadType { get; set; }
        public Nullable<System.Guid> subscr_lst_id { get; set; }
        public int posts_count { get; set; }
        public Nullable<System.Guid> ownr { get; set; }
        public string last_post_user_name { get; set; }
        public Nullable<System.Guid> last_post_user_id { get; set; }
        public Nullable<System.Guid> last_post_id { get; set; }
        public Nullable<System.DateTime> last_post_date { get; set; }
        public System.DateTime last_modified { get; set; }
        public string item_default_url_ { get; set; }
        public byte is_published { get; set; }
        public byte is_marked_spam { get; set; }
        public byte is_locked { get; set; }
        public System.Guid id { get; set; }
        public System.Guid forum_id { get; set; }
        public Nullable<System.Guid> first_post_id { get; set; }
        public System.DateTime date_created { get; set; }
        public byte block_crawlers { get; set; }
        public string app_name { get; set; }
    }
}
