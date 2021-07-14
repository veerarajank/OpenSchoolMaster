using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace GS247.Models
{
    public class SchoolCO
    {
        public List<FeeParticular> FeeParticulars { get; set; }
             
        public int? SubscriptionFlag { get; set; }

        public List<FeeAccess> FeeAccess { get; set; }
        
        public string RouteName { get; set; }
        
        public string StopName { get; set; }
        
        public string FareAmount { get; set; }
        
        public string Name { get; set; }

        public string TransactionDateStr { get; set; }

        public string TransactionTypeName { get; set; }
        
        public string ParticularDesc { get; set; }

        public string Discount { get; set; }

        public string UnitAmount { get; set; }

        public string InvoicePaidAmount { get; set; }

        public string ParticularName { get; set; }
        
        public string FieldValue { get; set; }

        public string FeeCategoryName { get; set; }

        public string StatusFlagDesc { get; set; }

        public string LastPaymentDate { get; set; }

        public List<StudentInvoicePayment> StudentInvoicePayment { get; set; }

        public string InvoiceBalanceAmount { get; set; }
        
        public string DocumentName { get; set; }
        
        public string StudentName { get; set; }

        public string SubjectName { get; set; }

        public string StudentCategoryName { get; set; }
                
        public string GenderName { get; set; }

        public string RelationName { get; set; }

        public string CourseBatchName { get; set; }

        public string CountryName1 { get; set; }

        public string CountryName { get; set; }

        public string BatchName { get; set; }

        public int? NumberOfStudents { get; set; }
        
        public string CourseName { get; set; }
                
        public string TotalMark { get; set; }

        public List<string> OptionValue { get; set; }

        public List<int> BatchIds { get; set; }

        public string AdmissionNo { get; set; }

        public int? Quantity { get; set; }

        public int? FloorNo { get; set; }

        public string RoomNo { get; set; }

        public string Beds { get; set; }

        public int? NumberOfBets { get; set; }

        public int? Availabilty { get; set; }

        public int? HostelRoomId { get; set; }

        public string FoodPreference { get; set; }

        public int? StudentExamDetailsId { get; set; }

        public int? Id { get; set; }
        public string Tax { get; internal set; }
    }
}