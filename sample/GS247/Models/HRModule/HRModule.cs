using GS247.Models.Courses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GS247.Models.HRModule
{
    public class LeaveTypeModel
    {
        public int StaffLeaveTypeId { get; set; }
        public string LeaveTypeName { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; }
        public int Gender { get; set; }
        public List<CheckBoxDetails> CategoryInfo { get; set; }
        public List<CheckBoxDetails> Genderinfo { get; set; }
        public string Count { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public List<LeaveTypeModel> LeaveInfo { get; set; }
    }
    public class LeaveRequest
    {
        public int Id { get; set; }
        public string LeaveType { get; set; }
        public string RequestedBy { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string Days { get; set; }
        public string Reason { get; set; }
        public string HalfDay { get; set; }
        public string Status { get; set; }
        public string ApprovedBy { get; set; }
        public string CancelledBy { get; set; }
        public List<LeaveRequest> LeaverequestInfo { get; set; }
    }
}