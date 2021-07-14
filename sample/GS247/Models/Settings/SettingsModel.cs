using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GS247.Models.Settings
{
    public class SettingsModel
    {
    }
    public class AcademicYearList
    {
        public List<AcademicYear> Years { get; set; }
    }
    public class AcademicYear
    {
        public int Id { get; set; }
        public String Name { get; set; }
        public String StartsOn { get; set; }
        public String EndsOn { get; set; }
        public String Description { get; set; }
        public String SelectStatus { get; set; }
    }
    public class MAcademicYear
    {
        public int Id { get; set; }
        public String Name { get; set; }
        public String StartsOn { get; set; }
        public String EndsOn { get; set; }
        public String Description { get; set; }
        public status OptStatus { get; set; }
    }
    public enum status
    {
        Inactive, Active
    }
    public class UsersList
    {
        public List<NewUser> NewUsers { get; set; }
    }
    public class NewUser
    {
        public int Id { get; set; }
        public String UserName { get; set; }
        public String Password { get; set; }
        public String Email { get; set; }
        public String Mobile { get; set; }
        public YesNo SuperUser { get; set; }
        public String SuperUserStr { get; set; }
        public status Status { get; set; }
        public String FirstName { get; set; }
        public String LastName { get; set; }
        public String LastVisit { get; set; }
    }
    public enum YesNo
    {
            No,Yes
    }
    public class NewEventList
    {
        public List<NewEvent> NewEvents { get; set; }
    }
    public class NewEvent
    {
        public int Id { get; set; }
        public String EventType { get; set; }
        public String ColorCode { get; set; }
    }
    public class DashboardList
    {
        public List<Dashboard> Dashboards { get; set; }
    }
    public class Dashboard
    {
        public int Id { get; set; }
        public String Name { get; set; }
        public int BlockOrder { get; set; }
        public Enable isEnable { get; set; }
    }
    public enum Enable
    {
        Enable,Disable
    }
    public class SchoolSetup
    {
        public int Id { get; set; }
        public String Name { get; set; }
        public String RegistrationId { get; set; }
        public String FoundedOn { get; set; }
        public String Curriculum { get; set; }
        public String Address { get; set; }
        public String Pincode { get; set; }
        public String Phone { get; set; }
        public String AlternatePhone { get; set; }
        public String Email { get; set; }
        public String Fax { get; set; }

        public String PrincipalName { get; set; }
        public String PrincipalEmail { get; set; }
        public String PrincipalPhone { get; set; }
        public String PrincipalMobile { get; set; }

        public List<AcademicYear> AcademicYears { get; set; }
        public String FinanceYearStart { get; set; }
        public String FinanceYearEnd { get; set; }

   }
    public class DefaultWeekdaysList
    {
        public List<DefaultWeekdays> Weekdays { get; set; }
    }
   public class DefaultWeekdays
    {
        public int Id { get; set; }
        public String Name { get; set; }
        public bool IsEnable { get; set; }
    }
    public class Feature
    {
        public int Id { get; set; }
        public bool IsAchievements { get; set; }
        public bool IsComplaints { get; set; }
    }
    public class ClassTimingsList
    {
        public List<ClassTimingDiff> ClassTimings { get; set; }
    }
    public class ClassTiming
    {
        public int Id { get; set; }
        public String Name { get; set; }
        public String StartTime { get; set; }
        public String EndTime { get; set; }
        public bool IsBreak { get; set; }
        public bool IsAllBatch { get; set; }
        public List<DefaultWeekdays> Weekdays { get; set; }
    }
    public class ClassTimingDiff
    {
        public int Id { get; set; }
        public String Name { get; set; }
        public String StartTime { get; set; }
        public String EndTime { get; set; }
        public Break IsBreak { get; set; }
        public bool IsAllBatch { get; set; }
        public List<DefaultWeekdays> Weekdays { get; set; }
    }
    public enum Break
    {
        No,Yes
    }
    public class DocumentTypeList
    {
        public List<DocumentType> DocumentTypes { get; set; }
    }
    public class DocumentType
    {
        public int Id { get; set; }
        public String Name { get; set; }
        public Mandatory Mandatory { get; set; }
        public Break IsRequired { get; set; }
    }
    public class DocumentTypeAdd
    {
        public int Id { get; set; }
        public String Name { get; set; }
        public int Mandatory { get; set; }
        public bool IsRequired { get; set; }

    }
    public enum Mandatory
    {
        No,
        [Display(Name="Yes,Can be skipped during Registration")]
        YesCan,
        [Display(Name = "Yes,Cannot be skipped during Registration")]
        YesCannot
    }
    public class ManageRegistration
    {
        public int Id { get; set; }
        public int AcademicYearId { get; set; }
        public String AcademicYear { get; set; }
        public bool AllowOnlineAdmission { get; set; }
        public bool ShowLink { get; set; }
    }
    public class DefaultGradingList
    {
        public List<DefaultGrading> Grades { get; set; }
    }
    public class DefaultGrading
    {
        public int Id { get; set; }
        public String Name { get; set; }
        public int MinScore { get; set; }
    }
    public class ExamFormat
    {
        public int Id { get; set; }
        public bool SchoolLevel { get; set; }
        public bool IsDefault { get; set; }
        public bool IsCBSE { get; set; }
        public bool CourseLevel { get; set; }
        public bool BatchLevel { get; set; }
        public int IsLevel { get; set; }
        public int IsFormat { get; set; }
    }
    public class SMTP
    {
        public int Id { get; set; }
        public String Host { get; set; }
        public String UserName { get; set; }
        public String Password { get; set; }
        public String Port { get; set; }
        public int ConnectionSecurity { get; set; }
        public int EnableSMTP { get; set; }
    }
    public enum Security
    {
        TLS,SSL
    }
    public class NotificationList
    {
        public List<Notification> Notifications { get; set; }
    }
    public class Notification
    {
        public int Id { get; set; }
        public String Name { get; set; }
        public bool IsSMS { get; set; }
        public bool IsEMail { get; set; }
        public bool IsMessage { get; set; }
        public bool IsStudent { get; set; }
        public bool IsGuardian { get; set; }
        public bool IsTeacher { get; set; }
    }
    public class SMSCounter
    {
        public int TotalCount { get; set; }
        public int DayCount { get; set; }
        public int MonthCount { get; set; }
        public List<DailySMS> DailySMSList { get; set; }

    }
    public class DailySMS
    {
        public String SMSDate { get; set; }
        public int SMSCount { get; set; }
    }
}