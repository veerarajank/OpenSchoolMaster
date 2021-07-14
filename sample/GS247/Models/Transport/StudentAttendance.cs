using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GS247.Models.Transport
{
    public class TransportStudentAttendance
    {
        public int RouteId { get; set; }
        public int StopId { get; set; }
        public string Session { get; set; }
       public List<StudentDetail> Students { get; set; }
    }
       
}