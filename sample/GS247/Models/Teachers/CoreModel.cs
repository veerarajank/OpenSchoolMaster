using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GS247.Models.Teachers
{
    public class CoreModel
    {

    }
    public class CategoryList
    {
        public List<Category> Categories { get; set; }
    }
    public class DepartmentList
    {
        public List<Department> Departments { get; set; }
    }
    public class GradeList
    {
        public List<Grade> Grades { get; set; }
    }
    public class PositionList
    {
        public List<Position> Positions { get; set; }
        public List<SelectListItem> Categories { get; set; }
        public String SelectedCategory { get; set; }
    }
    public class Category
    {
        public int Id { get; set; }
        public String Name { get; set; }
        public String Prefix { get; set; }
    }
    public class Department
    {
        public int Id { get; set; }
        public String Code { get; set; }
        public String Name { get; set; }
    }
    public class Position
    {
        public int Id { get; set; }
        public String Name { get; set; }
        public String Category { get; set; }
    }
    public class MPosition
    {
        public int Id { get; set; }
        public String Name { get; set; }
        public String SelectedCategory { get; set; }
        public List<SelectListItem> Categories { get; set; }
    }
    public class Grade
    {
        public int Id { get; set; }
        public String Name { get; set; }
        public Priority Priority { get; set; }
        public Status Status { get; set; }
        public int MaxHoursDay { get; set; }
        public int MaxHoursWeek { get; set; }
    }
    public enum Priority
    {
        Select, Low, Medium, High
    }
    public enum Status
    {
        Select, Active, Inactive
    }
    public class StaffDetails
    {
        public int StaffId { get; set; }
        public String StaffNumber { get; set; }
        public String JoiningDate { get; set; }
        public String FirstName { get; set; }
        public String MiddleName { get; set; }
        public String LastName { get; set; }
        public int GenderId { get; set; }
        public String DateOfBirth { get; set; }
        public int DepartmentId { get; set; }
        public int CategoryId { get; set; }
        public int PositionId { get; set; }
        public int GradeId { get; set; }
        public String JobTitle { get; set; }
        public String Qualification { get; set; }
        public int TotalExpYears { get; set; }
        public int TotalExpMonths { get; set; }
        public String ExpDetails { get; set; }
        public int isMarried { get; set; }
        public int ChildrenCount { get; set; }
        public String FatherName { get; set; }
        public String MotherName { get; set; }
        public String SpouseName { get; set; }
        public int BloodGroupId { get; set; }
        public int StaffRoleId { get; set; }
        public int NationalityId { get; set; }
        public Nationality Nationality { get; set; }
        public String Email { get; set; }
        public String HomeAddressLine1 { get; set; }
        public String HomeAddressLine2 { get; set; }
        public String HomeCity { get; set; }
        public String HomeState { get; set; }
        public String HomeCountry { get; set; }
        public String HomePincode { get; set; }
        public String OfficeAddressLine1 { get; set; }
        public String OfficeAddressLine2 { get; set; }
        public String OfficeCity { get; set; }
        public String OfficeState { get; set; }
        public String OfficeCountry { get; set; }
        public String OfficePincode { get; set; }
        public String OfficePhone1 { get; set; }
        public String OfficePhone2 { get; set; }
        public String MobileNumber { get; set; }
        public String HomePhone { get; set; }
        public String Fax { get; set; }
        public String BasicPay { get; set; }
        public String EPF { get; set; }
        public String ESI { get; set; }
        public String BankName { get; set; }
        public String BankAccountNumber { get; set; }
        public String IfscCode { get; set; }
        public String PassportNumber { get; set; }
        public String PassportExpiry { get; set; }
        public String PANCardNo { get; set; }
        public String AadhaarNo { get; set; }
        public int IsChanged { get; set; }

    }
    public class ViewStaffDetails
    {
        public int StaffId { get; set; }
        public String StaffNumber { get; set; }
        public String JoiningDate { get; set; }
        public String FirstName { get; set; }
        public String MiddleName { get; set; }
        public String LastName { get; set; }
       public String GenderName { get; set; }
        public String DateOfBirth { get; set; }
       public String DepatmentName { get; set; }
       public String CategoryName { get; set; }
       public String PositionName { get; set; }
       public String GradeName { get; set; }
        public String JobTitle { get; set; }
        public String Qualification { get; set; }
        public int TotalExpYears { get; set; }
        public int TotalExpMonths { get; set; }
        public String ExpDetails { get; set; }
       public String isMarried { get; set; }
        public int ChildrenCount { get; set; }
        public String FatherName { get; set; }
        public String MotherName { get; set; }
        public String SpouseName { get; set; }
       public String BloodGroupName { get; set; }
       public String StaffRoleName { get; set; }
       public String Nationality { get; set; }
        public String Email { get; set; }
        public String HomeAddressLine1 { get; set; }
        public String HomeAddressLine2 { get; set; }
        public String HomeCity { get; set; }
        public String HomeState { get; set; }
        public String HomeCountry { get; set; }
        public String HomePincode { get; set; }
        public String OfficeAddressLine1 { get; set; }
        public String OfficeAddressLine2 { get; set; }
        public String OfficeCity { get; set; }
        public String OfficeState { get; set; }
        public String OfficeCountry { get; set; }
        public String OfficePincode { get; set; }
        public String OfficePhone1 { get; set; }
        public String OfficePhone2 { get; set; }
        public String MobileNumber { get; set; }
        public String HomePhone { get; set; }
        public String Fax { get; set; }
        public String BasicPay { get; set; }
        public String EPF { get; set; }
        public String ESI { get; set; }
        public String BankName { get; set; }
        public String BankAccountNumber { get; set; }
        public String IfscCode { get; set; }
        public String PassportNumber { get; set; }
        public String PassportExpiry { get; set; }
        public String PANCardNo { get; set; }
        public String AadhaarNo { get; set; }
        public int IsChanged { get; set; }

    }
    public enum MaritalStatus
    {
        Single,
        Married,
        Divorced
    }
    public enum BloodGroup
    {
        [Display(Name = "A+")]
        APositive,
        [Display(Name = "A-")]
        ANegative,
        [Display(Name = "B+")]
        BPositive,
        [Display(Name = "B-")]
        BNegative,
        [Display(Name = "O+")]
        OPositive,
        [Display(Name = "O-")]
        ONegative,
        [Display(Name = "AB+")]
        ABPositive,
        [Display(Name = "AB-")]
        ABNegative,

    }
    public enum Genders
    {
        Male, Female
    }
    public enum Nationality
    {
        Indian, Others
    }
    public class LogCategories
    {
        public List<LogCategory> Categories { get; set; }

      }
    public class LogCategory
    {
        public int Id { get; set; }
        public String Name { get; set; }
    }
    public class Achievement
    {
        public int Id { get; set; } 
        public String Title { get; set; }
        public String Description { get; set; }
        public String DocumentName { get; set; }
    }
    public class Log
    {
        public int Id { get; set; }
        public String CategoryName { get; set; }
        public String LogDetails { get; set; }
        public String UserName { get; set; }
        public String UserRole { get; set; }
        public String LoggedDateTime { get; set; }
    }
    public class Document
    {
        public int Id { get; set; }
        public String DocumentName { get; set; }
    }
    public class Dummy
    {
        public int id { get; set; }
    }
    public class SubjectAssociation
    {
        public int CourseId { get; set; }
        public int BatchId { get; set; }
        public int SubjectId { get; set; }
    }
    public class DummyObj
    {
        public int Id { get; set; }
        public String Name { get; set; }
    }
    public class CurrentlyAssigned
    {
        public int Id { get; set; }
        public String FirstName { get; set; }
        public String LastName { get; set; }
        public String Department { get; set; }
    }
    public class Teacher
    {
        public int Id { get; set; }
        public String Name { get; set; }
        public String Number { get; set; }
        public String Department { get; set; }
        public String Gender { get; set; }
    }
    public class TeachersList
    {
        public List<Teacher> Teachers { get; set; }
    }
}