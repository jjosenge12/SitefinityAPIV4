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
    
    public partial class SyncTaskExecution
    {
        public System.Guid Id { get; set; }
        public System.DateTimeOffset StartTime { get; set; }
        public Nullable<System.DateTimeOffset> EndTime { get; set; }
        public string ErrorMessage { get; set; }
        public int Status { get; set; }
        public byte UsesOwnAppdomain { get; set; }
        public Nullable<int> SyncStopAction { get; set; }
        public Nullable<System.DateTimeOffset> WatchdogUpdateTime { get; set; }
        public Nullable<System.Guid> TaskId { get; set; }
        public Nullable<int> ResultId { get; set; }
        public Nullable<System.Guid> SchedulerExecutionId { get; set; }
    }
}
