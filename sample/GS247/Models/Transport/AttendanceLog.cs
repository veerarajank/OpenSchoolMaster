using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GS247.Models.Transport
{
    public class AttendanceLog
    {
        public string RollNumber { get; set; }
        public string StudentName { get; set; }
        public int StudentId { get; set; }
        public string Batch { get; set; }
        public string Route { get; set; }
        public string AttendanceDate { get; set; }
        public string MorngStartTime { get; set; }
        public string MorngEndTime { get; set; }
        public string EvngStartTime { get; set; }
        public string EvngEndTime { get; set; }
    }
       public class AttendanceLogContext
    {
        public List<AttendanceLog> AttendanceLogs { get; set; }
        public string RouteName { get; set; }
        public DateTime AttendanceDate { get; set; }
    }
}