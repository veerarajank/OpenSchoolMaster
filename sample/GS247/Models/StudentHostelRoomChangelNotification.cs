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
    
    public partial class StudentHostelRoomChangelNotification
    {
        public int RoomChangeNotificationId { get; set; }
        public Nullable<int> StudentDetailsId { get; set; }
        public Nullable<System.DateTime> SendDate { get; set; }
        public string CreatedBy { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public Nullable<int> HostelDetailsId { get; set; }
        public Nullable<int> HostelFloorId { get; set; }
        public Nullable<int> HostelRoomBedId { get; set; }
        public Nullable<int> StatusFlag { get; set; }
    }
}
