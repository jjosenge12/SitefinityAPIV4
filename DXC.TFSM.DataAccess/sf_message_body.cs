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
    
    public partial class sf_message_body
    {
        public Nullable<byte> was_template_copied { get; set; }
        public string plain_text_version { get; set; }
        public string nme { get; set; }
        public int message_body_type { get; set; }
        public System.DateTime last_modified { get; set; }
        public byte is_template { get; set; }
        public Nullable<System.Guid> internal_page_template_id { get; set; }
        public System.Guid id { get; set; }
        public Nullable<System.Guid> copied_template_id { get; set; }
        public string body_text { get; set; }
        public string application_name { get; set; }
        public short voa_version { get; set; }
    }
}
