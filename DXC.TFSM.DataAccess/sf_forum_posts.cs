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
    
    public partial class sf_forum_posts
    {
        public string title { get; set; }
        public System.Guid thread_id { get; set; }
        public Nullable<System.Guid> reply_to_post_id { get; set; }
        public Nullable<System.Guid> ownr { get; set; }
        public Nullable<System.Guid> last_modified_by { get; set; }
        public System.DateTime last_modified { get; set; }
        public byte is_published { get; set; }
        public byte is_marked_spam { get; set; }
        public byte is_featured { get; set; }
        public System.Guid id { get; set; }
        public System.DateTime date_created { get; set; }
        public string content { get; set; }
        public string app_name { get; set; }
    }
}
