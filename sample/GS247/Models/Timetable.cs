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
    
    public partial class Timetable
    {
        public int Id { get; set; }
        public Nullable<int> BatchId { get; set; }
        public string Weekday { get; set; }
        public Nullable<int> ClassTimingId { get; set; }
        public Nullable<int> SubjectId { get; set; }
        public Nullable<int> TeacherId { get; set; }
        public Nullable<int> DefaultClassTiming { get; set; }
        public Nullable<int> DefaultWeekday { get; set; }
    }
}
