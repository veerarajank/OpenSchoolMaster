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
    
    public partial class FeesConfiguration
    {
        public int FeesConfigurationID { get; set; }
        public int TaxFlag { get; set; }
        public int DiscountFlag { get; set; }
        public int InvoiceDiscountShow { get; set; }
        public int InvoiceTemplate { get; set; }
        public Nullable<int> UserId { get; set; }
        public string CreatedBy { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
    }
}
