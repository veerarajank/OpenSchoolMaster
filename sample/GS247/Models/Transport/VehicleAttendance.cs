using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GS247.Models.Transport
{
    public class VehicleAttendance
    {
        public string RouteName { get; set; }
        public string DriverName { get; set; }
        public DateTime AttendanceDate { get; set; } 
       public string MorngStart { get; set; }
        public string MorngEnd { get; set; }
        public string EvngStart { get; set; }
        public string EvngEnd { get; set; }
    }
       
}