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
    
    public partial class StaffLeaveRequest
    {
        public int Id { get; set; }
        public string LeaveType { get; set; }
        public string RequestedBy { get; set; }
        public Nullable<System.DateTime> FromDate { get; set; }
        public Nullable<System.DateTime> ToDate { get; set; }
        public string Days { get; set; }
        public string Reason { get; set; }
        public string Status { get; set; }
        public string HalfDay { get; set; }
    }
}
