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
    
    public partial class StudentWaitingList
    {
        public int StudentWaitingId { get; set; }
        public Nullable<int> StudentDetailsId { get; set; }
        public Nullable<int> BatchId { get; set; }
        public Nullable<int> StarusFlag { get; set; }
        public Nullable<int> Priority { get; set; }
        public string CreatedBy { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
    }
}
