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
    
    public partial class PurchaseVendorDetail
    {
        public int VendorDetailId { get; set; }
        public Nullable<int> UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PhoneNumber { get; set; }
        public string OfficeNumber { get; set; }
        public string Email { get; set; }
        public Nullable<int> CurrencyType { get; set; }
        public string CompanyName { get; set; }
        public string VATNumber { get; set; }
        public string CSTNumber { get; set; }
        public string CreatedBy { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public Nullable<int> country { get; set; }
    }
}
