using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GS247.Models.Transport
{
    public class TransportFee
    {
        public int StudentDetailsId { get; set; }
        public string StudentName { get; set; }
        public string RollNumber { get; set; }
        public string Batch { get; set; }
        public string Course { get; set; }
        public string RouteName { get; set; }
        public string StopName { get; set; }
        public double Fare { get; set; }
        public string Status { get; set; }
        public string PaidDate { get; set; }
        public int StudentId { get; set; }

    }
       
}