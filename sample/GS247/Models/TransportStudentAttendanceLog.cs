//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace GS247.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class TransportStudentAttendanceLog
    {
        public long Id { get; set; }
        public Nullable<int> StudentDetailId { get; set; }
        public Nullable<int> RouteId { get; set; }
        public Nullable<System.DateTime> AttendanceDate { get; set; }
        public Nullable<int> StopId { get; set; }
        public string MorngStartTime { get; set; }
        public string MorngEndTime { get; set; }
        public string EvngStartTime { get; set; }
        public string EvngEndTime { get; set; }
    
        public virtual StudentDetail StudentDetail { get; set; }
    }
}
