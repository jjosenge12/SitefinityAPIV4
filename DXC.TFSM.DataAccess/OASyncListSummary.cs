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
    
    public partial class OASyncListSummary
    {
        public int Id { get; set; }
        public string ListName { get; set; }
        public int ImportStatus { get; set; }
        public System.DateTimeOffset ImportStart { get; set; }
        public Nullable<System.DateTimeOffset> ImportEnd { get; set; }
        public int SourceDeletes { get; set; }
        public int SourceInserts { get; set; }
        public int SourceReads { get; set; }
        public int SourceUpdates { get; set; }
        public int DestinationDeletes { get; set; }
        public int DestinationInserts { get; set; }
        public int DestinationReads { get; set; }
        public int DestinationUpdates { get; set; }
        public int BaseDeletes { get; set; }
        public int BaseInserts { get; set; }
        public int BaseReads { get; set; }
        public int BaseUpdates { get; set; }
        public int ImportSummary { get; set; }
    }
}
