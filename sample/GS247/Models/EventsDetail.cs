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
    
    public partial class EventsDetail
    {
        public int Id { get; set; }
        public string EventName { get; set; }
        public string EventDescription { get; set; }
        public Nullable<System.DateTime> EventFromDate { get; set; }
        public Nullable<System.DateTime> EventToDate { get; set; }
        public Nullable<System.TimeSpan> EventFromTime { get; set; }
        public Nullable<System.TimeSpan> EventToTime { get; set; }
        public Nullable<System.DateTime> CreatedDateTime { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDateTime { get; set; }
        public string ModifiedBy { get; set; }
        public string EventType { get; set; }
        public string Organizer { get; set; }
        public string Allday { get; set; }
        public string Editable { get; set; }
    }
}
