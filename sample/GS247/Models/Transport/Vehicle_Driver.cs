using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GS247.Models.Transport
{
    public class Vehicle_Driver_Context
    {
       public TransportDriverVehicleLink link { get; set; }
        public List<Vehicle_Driver> links { get; set; }
    }
    public class Vehicle_Driver
    {
        public int DriverId { get; set; }
        public string DriverName { get; set; }
        public string DOB { get; set; }
        public string VehicleCode { get; set; }
        public string RouteName { get; set; }
    }
}