using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GS247.Models.Transport
{
    public class Attendance
    {
        public int RouteId { get; set; }
        public int DriverId { get; set; }
       public bool MorngStart { get; set; }
        public bool MorngEnd { get; set; }
        public bool EvngStart { get; set; }
        public bool EvngEnd { get; set; }
    }
       
}