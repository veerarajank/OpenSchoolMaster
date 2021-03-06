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
    
    public partial class TransportBusLog
    {
        public int Id { get; set; }
        public Nullable<int> VehicleId { get; set; }
        public string ReadingStart { get; set; }
        public string ReadingEnd { get; set; }
        public Nullable<double> FuelConsumed { get; set; }
        public Nullable<decimal> Amount { get; set; }
        public Nullable<System.DateTime> EntryDate { get; set; }
    
        public virtual TransportVehicle TransportVehicle { get; set; }
    }
}
