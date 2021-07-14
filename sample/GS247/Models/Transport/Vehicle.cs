using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GS247.Models.Transport
{
    public class Vehicle
    {
        public string VehicleNo { get; set; }
        public string VehicleCode { get; set; }
        public int NoOfSeats { get; set; }
        public int MaximumCapacity { get; set; }
        public string DriverName { get; set; }
        public string RouteName { get; set; }
        public string VehicleType { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Phone { get; set; }
        public string Insurance { get; set; }
        public string TaxRemitted { get; set; }
        public string Permit { get; set; }
    }
       
}