using GS247.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using ClosedXML.Excel;
using GS247.Models.Import;
using System.Data.OleDb;
//using Microsoft.Office.Interop.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using System.Web.WebPages.Html;

namespace GS247.Controllers
{
    public class ImportController : Controller
    {
       public GS247Entities8 db=new GS247Entities8();
        SqlConnection conn=new SqlConnection(ConfigurationManager.ConnectionStrings["GS247"].ConnectionString);
        SqlCommand cmd;
        public OleDbDataAdapter dta;
        public string fileLocation;
        //public OLEDBConnection oledbCon;
        public DataSet ds=new DataSet();
        public SqlDataAdapter da;
        public SqlDataReader dr;
        public System.Data.DataTable dt = new System.Data.DataTable("Sheet1");
       
        string excelConnectionString = string.Empty;
        string fileExtension = string.Empty;
        public ActionResult Import()
        {
            int UserId = Convert.ToInt32(Session["UserId"]);
            if (UserId == 0)
            {
                return RedirectToAction("Index", "Login");
            }
            return View(new ImportClass());
        }
        [HttpPost]
        public ActionResult Import(HttpPostedFileBase FileUpload,string modelName,string action)
        {
            int UserId = Convert.ToInt32(Session["UserId"]);
            if (UserId == 0)
            {
                return RedirectToAction("Index", "Login");
            }
            if (FileUpload==null || modelName==null||action==null)
            {
                TempData["Import"] = "Mandatory Fields are missing";
                return RedirectToAction("Import");
            }
            
            string fileLocation = "";
                if (Request.Files["FileUpload"].ContentLength > 0)
                {
                    fileExtension =
                                         Path.GetExtension(Request.Files["FileUpload"].FileName);

                    if (fileExtension == ".xls" || fileExtension == ".xlsx")
                    {
                        fileLocation = Server.MapPath("~/Content/UploadFile")+fileExtension;
                    
                        if (System.IO.File.Exists(fileLocation))
                        {
                            System.IO.File.Delete(fileLocation);
                        }
                        Request.Files["FileUpload"].SaveAs(fileLocation);
                        
                        if (fileExtension == ".xls")
                        {
                            excelConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" +
                            fileLocation + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=2\"";
                        }
                        else if (fileExtension == ".xlsx")
                        {
                            excelConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + fileLocation + ";Extended Properties=Excel 12.0;";
                        }
                        try
                        {
                            dta = new OleDbDataAdapter("Select * From [Sheet1$]", excelConnectionString);
                            dta.Fill(dt);
                        int rowsCount = dt.Rows.Count;
                        if (rowsCount > 0)
                        {
                            System.Data.DataTable dtCopy = ImportData(modelName, action, dt);
                            if (dtCopy.Rows.Count > 0)
                            {
                                TempData["Import"] = rowsCount - dtCopy.Rows.Count + " of " + rowsCount + " Rows Imported Successfully and " + dtCopy.Rows.Count + " Rows Not Imported. Edit those Rows and Import Again";
                                using (XLWorkbook wb = new XLWorkbook())
                                {
                                    wb.Worksheets.Add(dtCopy);
                                    using (MemoryStream stream = new MemoryStream())
                                    {
                                        wb.SaveAs(stream);
                                        return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", modelName + "_UnFilledRows.xlsx");
                                    }
                                }
                            }

                            else
                                TempData["Import"] = rowsCount - dtCopy.Rows.Count + " of " + rowsCount + " Rows Imported Successfully";
                            return RedirectToAction("Import");
                        }
                        else
                        {
                            TempData["Import"] = "No data found to import";
                        }
                        ImportClass import = new ImportClass
                        {
                            Action = action,
                            ModelName = modelName
                        };
                        return View("Import", import);
                    }
                        catch (Exception ex)
                        {
                            throw;
                        }
                       
                    }
                    else
                        TempData["Import"] = "Unsupported File";
                }
            TempData["Import"] = "No data found to Import";
            return RedirectToAction("Import");
        }
       
        public System.Data.DataTable ImportData(string ModelName,string Action, System.Data.DataTable dt1)
        {
            int k=0;
            
            System.Data.DataTable dtCopy = dt1.Clone();
            first:
            try
            {
                if (ModelName == "Student")
                {
                    for (int j = 0; j < dt1.Rows.Count; j++)
                    {
                        k = j;
                        
                        string AdmissionNo = dt1.Rows[j]["AdmissionNo"].ToString() != null ? dt1.Rows[j]["AdmissionNo"].ToString().Trim() : "";
                        string FirstName = dt1.Rows[j]["FirstName"].ToString() != null ? dt1.Rows[j]["FirstName"].ToString().Trim() : "";
                        DateTime AdmissionDate = Convert.ToDateTime(dt1.Rows[j]["AdmissionDate"].ToString().Trim());
                        DateTime DOB = Convert.ToDateTime(dt1.Rows[j]["DOB"].ToString().Trim());
                        string gender = dt1.Rows[j]["Gender"].ToString() != null ? dt1.Rows[j]["Gender"].ToString().Trim() : "";
                        string AddressLine1 = dt1.Rows[j]["AddressLine1"].ToString() != null ? dt1.Rows[j]["AddressLine1"].ToString().Trim() : "";
                        string City = dt1.Rows[j]["City"].ToString() != null ? dt1.Rows[j]["City"].ToString().Trim() : "";
                        string Pincode = dt1.Rows[j]["Pincode"].ToString() != null ? dt1.Rows[j]["Pincode"].ToString().Trim() : "";
                        string State = dt1.Rows[j]["State"].ToString() != null ? dt1.Rows[j]["State"].ToString().Trim() : "";
                        string Phone1 = dt1.Rows[j]["Phone1"].ToString() != null ? dt1.Rows[j]["Phone1"].ToString().Trim() : "";
                        string Batch = dt1.Rows[j]["Batch"].ToString() != null ? dt1.Rows[j]["Batch"].ToString().Trim() : "";
                        string Course = dt1.Rows[j]["Course"].ToString() != null ? dt1.Rows[j]["Course"].ToString().Trim() : "";
                        string Country = dt1.Rows[j]["Country"].ToString() != null ? dt1.Rows[j]["Country"].ToString().Trim() : "";
                        string Nationality = dt1.Rows[j]["Nationality"].ToString() != null ? dt1.Rows[j]["Nationality"].ToString().Trim() : "";
                        string blood = dt1.Rows[j]["Blood"].ToString() != null ? dt1.Rows[j]["Blood"].ToString().Trim() : "";
                        string RollNumber = dt1.Rows[j]["RollNumber"].ToString() != null ? dt1.Rows[j]["RollNumber"].ToString().Trim() : "";
                        string Category = dt1.Rows[j]["Category"].ToString() != null ? dt1.Rows[j]["Category"].ToString().Trim() : "";

                        if (AdmissionNo != "" && AdmissionDate != null && FirstName != "" && DOB != null && gender != "" && AddressLine1 != "" && City != "" && Pincode != "" && State != "" && Phone1 != "" && Batch != "" && Course != ""&&Nationality!="")
                        {
                                var studentDetails = db.StudentDetails.Where(x => x.RollNumber == RollNumber).FirstOrDefault();
                            bool getOut = false;
                            string status = "";
                            if ((RollNumber != "" && studentDetails != null) )
                            {
                                status += "Roll Number already exist,";
                                getOut = true;
                            }
                            studentDetails = db.StudentDetails.Where(x => x.AdmissionNo == AdmissionNo).FirstOrDefault();
                            if (AdmissionNo != "" && studentDetails != null)
                            {
                                status += "Admission Number already exist,";
                                getOut = true;
                            }

                            //Check Batch,Course,Category

                            if (db.batches.Where(x => x.Name == Batch).FirstOrDefault() == null)
                            {
                                status += "Invalid Batch,";
                                getOut = true;
                            }
                            if (db.Courses.Where(x => x.Name == Course).FirstOrDefault() == null)
                            {
                                status += "Invalid Course,";
                                getOut = true;
                            }
                            var StudentCategory = db.StudentCategories.Where(x => x.Name == Category).FirstOrDefault();
                            if (StudentCategory == null&&Category!="")
                            {
                                status+="Invalid Category,";
                                getOut = true;
                            }
                            var StudentCountry = db.countries.Where(x => x.country_name == Country).FirstOrDefault();
                            if (StudentCountry == null&&Country!="")
                            {
                                status += "Invalid Country,";
                                getOut = true;
                            }
                            if (db.countries.Where(x => x.country_name == Nationality).FirstOrDefault() == null)
                            {
                                status += "Invalid Nationality,";
                                getOut = true;
                            }
                            if (Gender(gender)==null)
                            {
                                status += "Invalid Gender";
                                getOut = true;
                            }
                            if (getOut == true)
                            {
                                dt1.Rows[k]["Status"] = status;
                                dtCopy.ImportRow(dt1.Rows[k]);
                                dt1.Rows[j].Delete();
                                dt1.AcceptChanges();
                                j = -1;
                                continue;
                            }

                            //Getting Country Id, Student Category Id
                            int studentCountry = 0;
                            if (StudentCountry != null) studentCountry = StudentCountry.id;

                            int studentCategory = 0;
                            if (StudentCategory != null) studentCategory = StudentCategory.StudentCategoryID;

                            if (Action == "Add")
                            {
                                string studentId = "";
                                var uidCheck = db.StudentDetails.FirstOrDefault();
                                if (uidCheck == null) studentId = "S000001";
                                else
                                {
                                    studentId = (from cs in db.StudentDetails
                                                 select cs.StudentId.Substring(1)).Max();
                                    long curValue = Convert.ToInt64(studentId) + 1;
                                    studentId = "S" + curValue.ToString("D6");
                                }
                                var acadamicYear = db.Years.Where(x => x.Status == 1).FirstOrDefault();
                                string AcadamicYear = "";
                                if (acadamicYear != null) AcadamicYear = acadamicYear.Id.ToString();


                                StudentDetail studentDetail = new StudentDetail
                                {
                                    AddressLine1 = AddressLine1,
                                    AddressLine2 = dt1.Rows[j]["AddressLine2"].ToString(),
                                    AdmissionDate = AdmissionDate,
                                    AdmissionNo = AdmissionNo,
                                    Batch = db.batches.Where(x => x.Name == Batch).FirstOrDefault().Id,
                                    BirthPlace = dt1.Rows[0]["BirthPlace"].ToString(),
                                    Blood = Convert.ToInt32(Blood(blood)),
                                    Country = studentCountry,
                                    Course = db.Courses.Where(x => x.Name == Course).FirstOrDefault().Id,
                                    City = City,
                                    CreatedBy = Session["UserName"].ToString(),
                                    CreatedDate = DateTime.Today,
                                    DOB = DOB,
                                    Email = dt1.Rows[j]["Email"].ToString(),
                                    FirstName = FirstName,
                                    Gender = Convert.ToInt32(Gender(gender)),
                                    Language = dt1.Rows[j]["Language"].ToString(),
                                    LastName = dt1.Rows[j]["LastName"].ToString(),
                                    MiddleName = dt1.Rows[j]["MiddleName"].ToString(),
                                    Nationality = db.countries.Where(x => x.country_name == Nationality).FirstOrDefault().id,
                                    PhoneNumber1 = Phone1,
                                    PhoneNumber2 = dt1.Rows[j]["Phone2"].ToString(),
                                    PinCode = Pincode,
                                    Religion = dt1.Rows[j]["Religion"].ToString(),
                                    RollNumber = RollNumber,
                                    Semester = dt1.Rows[j]["Semester"].ToString(),
                                    State = dt1.Rows[j]["State"].ToString(),
                                    StudentCategory = studentCategory,
                                    StudentId = studentId,
                                    DeleteFlag=0,
                                     AcademicYear= AcadamicYear
                                };
                                db.StudentDetails.Add(studentDetail);
                                db.SaveChanges();
                            }
                            else if (Action == "Edit")
                            {
                                string StudentId = dt1.Rows[j]["StudentId"].ToString() != null ? dt1.Rows[j]["StudentId"].ToString().Trim() : "";
                                var studentDetail = db.StudentDetails.Where(x => x.StudentId == StudentId).FirstOrDefault();
                                if (studentDetail != null)
                                {
                                    studentDetail.AddressLine1 = AddressLine1;
                                    studentDetail.AddressLine2 = dt1.Rows[j]["AddressLine2"].ToString();
                                    studentDetail.AdmissionDate = AdmissionDate;
                                    studentDetail.AdmissionNo = AdmissionNo;
                                    studentDetail.Batch = db.batches.Where(x => x.Name == Batch).FirstOrDefault().Id;
                                    studentDetail.BirthPlace = dt1.Rows[0]["BirthPlace"].ToString();
                                    studentDetail.Blood = Convert.ToInt32(Blood(blood));
                                    studentDetail.Country = studentCountry;
                                    studentDetail.Course = db.Courses.Where(x => x.Name == Course).FirstOrDefault().Id;
                                    studentDetail.City = City;
                                    studentDetail.UpdatedBy = Session["UserName"].ToString();
                                    studentDetail.UpdatedDate = DateTime.Today;
                                    studentDetail.DOB = DOB;
                                    studentDetail.Email = dt1.Rows[j]["Email"].ToString();
                                    studentDetail.FirstName = FirstName;
                                    studentDetail.Gender = Convert.ToInt32(Gender(gender));
                                    studentDetail.Language = dt1.Rows[j]["Language"].ToString();
                                    studentDetail.LastName = dt1.Rows[j]["LastName"].ToString();
                                    studentDetail.MiddleName = dt1.Rows[j]["MiddleName"].ToString();
                                    studentDetail.Nationality = db.countries.Where(x => x.country_name == Nationality).FirstOrDefault().id;
                                    studentDetail.PhoneNumber1 = Phone1;
                                    studentDetail.PhoneNumber2 = dt1.Rows[j]["Phone2"].ToString();
                                    studentDetail.PinCode = Pincode;
                                    studentDetail.Religion = dt1.Rows[j]["MiddleName"].ToString();
                                    studentDetail.RollNumber = RollNumber;
                                    studentDetail.Semester = dt1.Rows[j]["Semester"].ToString();
                                    studentDetail.State = dt1.Rows[j]["State"].ToString();
                                    studentDetail.StudentCategory = studentCategory;
                                    studentDetail.DeleteFlag = 0;

                                    db.Entry(studentDetail).State = System.Data.Entity.EntityState.Modified;
                                    db.SaveChanges();
                                }
                                else
                                {
                                    dt1.Rows[k]["Status"] = "Student # is missing";
                                    dtCopy.ImportRow(dt1.Rows[k]);
                                    dt1.Rows[j].Delete();
                                    dt1.AcceptChanges();
                                    j = -1;
                                }
                            }
                           

                        }
                        else
                        {
                            dt1.Rows[k]["Status"] = "Mandatory values missing";
                            dtCopy.ImportRow(dt1.Rows[k]);
                            dt1.Rows[j].Delete();
                            dt1.AcceptChanges();
                            j = -1;
                        }
                    }
                }
               else if(ModelName=="Teacher")
                {
                    for (int j = 0; j < dt1.Rows.Count; j++)
                    {
                        k = j;
                        string Category = dt1.Rows[j]["Category"].ToString() != null ? dt1.Rows[j]["Category"].ToString().Trim() : "";
                        string Qualification = dt1.Rows[j]["Qualification"].ToString() != null ? dt1.Rows[j]["Qualification"].ToString().Trim() : "";
                        string TotalExperienceInYears = dt1.Rows[j]["TotalExperienceInYears"].ToString() != null ? dt1.Rows[j]["TotalExperienceInYears"].ToString().Trim() : "";
                        string FirstName = dt1.Rows[j]["FirstName"].ToString() != null ? dt1.Rows[j]["FirstName"].ToString().Trim() : "";
                        DateTime JoiningDate = Convert.ToDateTime(dt1.Rows[j]["JoiningDate"].ToString().Trim());
                        string JobTitle = dt1.Rows[j]["JobTitle"].ToString() != null ? dt1.Rows[j]["JobTitle"].ToString().Trim() : "";
                        DateTime DOB = Convert.ToDateTime(dt1.Rows[j]["DOB"].ToString().Trim());
                        string gender = dt1.Rows[j]["Gender"].ToString() != null ? dt1.Rows[j]["Gender"].ToString().Trim() : "";
                        string HomeAddressLine1 = dt1.Rows[j]["HomeAddressLine1"].ToString() != null ? dt1.Rows[j]["HomeAddressLine1"].ToString().Trim() : "";
                        string Email = dt1.Rows[j]["Email"].ToString() != null ? dt1.Rows[j]["Email"].ToString().Trim() : "";
                        string MobilePhone = dt1.Rows[j]["MobilePhone"].ToString() != null ? dt1.Rows[j]["MobilePhone"].ToString().Trim() : "";
                        string Department = dt1.Rows[j]["Department"].ToString() != null ? dt1.Rows[j]["Department"].ToString().Trim() : "";
                        string Position = dt1.Rows[j]["Position"].ToString() != null ? dt1.Rows[j]["Position"].ToString().Trim() : "";
                        string HomeCountry = dt1.Rows[j]["HomeCountry"].ToString() != null ? dt1.Rows[j]["HomeCountry"].ToString().Trim() : "";
                        string OfficeCountry = dt1.Rows[j]["OfficeCountry"].ToString() != null ? dt1.Rows[j]["OfficeCountry"].ToString().Trim() : "";
                        string Nationality = dt1.Rows[j]["Nationality"].ToString() != null ? dt1.Rows[j]["Nationality"].ToString().Trim() : "";
                        string BloodGroup = dt1.Rows[j]["BloodGroup"].ToString() != null ? dt1.Rows[j]["BloodGroup"].ToString().Trim() : "";
                        string Grade = dt1.Rows[j]["Grade"].ToString() != null ? dt1.Rows[j]["Grade"].ToString().Trim() : "";
                        int ChildrenCount =dt1.Rows[j]["ChildrenCount"].ToString()!="" ?Convert.ToInt32( dt1.Rows[j]["ChildrenCount"]) : 0;

                        if (Category != "" && Qualification != null && TotalExperienceInYears != "" &&FirstName!=""&&JoiningDate!=null&&JobTitle!=""&& DOB != null && gender != "" && HomeAddressLine1 != "" && MobilePhone != "" && Department != "" && Position != "")
                        {
                           
                            bool getOut = false;
                            string status = "";
                            
                            //Check Category,Department,Position,Grade,Country,Grade

                            if (db.TeachersManageCategories.Where(x => x.Name == Category).FirstOrDefault() == null)
                            {
                                status += "Invalid Category,";
                                getOut = true;
                            }
                            if (db.TeachersManageDepartments.Where(x => x.Name == Department).FirstOrDefault() == null)
                            {
                                status += "Invalid Department,";
                                getOut = true;
                            }
                            var TeacherGrade = db.TeachersManageGrades.Where(x => x.Name == Grade).FirstOrDefault();
                            if (TeacherGrade == null&&Grade!="")
                            {
                                status += "Invalid Grade,";
                                getOut = true;
                            }
                            if (db.TeachersManagePositions.Where(x => x.Name == Position).FirstOrDefault() == null)
                            {
                                status += "Invalid Position,";
                                getOut = true;
                            }
                            var TeacherNationality = db.countries.Where(x => x.country_name == Nationality).FirstOrDefault();
                            if (TeacherNationality == null && Nationality != "")
                            {
                                status += "Invalid Nationality,";
                                getOut = true;
                            }
                            if (db.Genders.Where(x => x.Name == gender).FirstOrDefault() == null)
                            {
                                status += "Invalid Gender,";
                                getOut = true;
                            }
                            if (getOut == true)
                            {
                                dt1.Rows[k]["Status"] = status;
                                dtCopy.ImportRow(dt1.Rows[k]);
                                dt1.Rows[j].Delete();
                                dt1.AcceptChanges();
                                j = -1;
                                continue;
                            }

                            //Getting Country Id
                            
                            int nationality = 0;
                            if (TeacherNationality != null) nationality = TeacherNationality.id;

                            int gradeId = 0;
                            if (TeacherGrade != null) gradeId = TeacherGrade.Id;

                            //Getting Marital Status
                            int MaritalStatus = 0;
                            if (dt1.Rows[j]["MaritalStatus"].ToString() == "Married") MaritalStatus = 1;

                            if (Action == "Add")
                            {
                                string teacherNum = "";
                                var uidCheck = db.StaffDetails.FirstOrDefault();
                                if (uidCheck == null) teacherNum = "E001";
                                else
                                {
                                    teacherNum = (from cs in db.StaffDetails
                                                  select cs.TeacherNumber.Substring(1)).Max();
                                    long curValue = Convert.ToInt64(teacherNum) + 1;
                                    teacherNum = "E" + curValue.ToString("D3");
                                }
                                StaffDetail staffDetail = new StaffDetail
                                {
                                    HomeAddressLine1 = HomeAddressLine1,
                                    HomeAddressLine2 = dt1.Rows[j]["HomeAddressLine2"].ToString(),
                                    HomeCity = dt1.Rows[j]["HomeCity"].ToString(),
                                    BloodGroup = Convert.ToInt32(Blood(dt1.Rows[j]["BloodGroup"].ToString())),
                                    DateOfBirth = DOB.ToString().Remove(10,12),
                                    Email = Email,
                                    FirstName = FirstName,
                                    LastName = dt1.Rows[j]["LastName"].ToString(),
                                    MiddleName = dt1.Rows[j]["MiddleName"].ToString(),                                    
                                    MobileNumber = MobilePhone,
                                    HomePhone = dt1.Rows[j]["HomePhone"].ToString(),
                                    TeacherNumber = teacherNum,
                                     CategoryId=db.TeachersManageCategories.Where(x=>x.Name==Category).FirstOrDefault().Id,
                                      ChildrenCount=ChildrenCount,
                                       DepartmentId=db.TeachersManageDepartments.Where(x=>x.Name==Department).FirstOrDefault().Id,
                                        ExperienceDetails= dt1.Rows[j]["ExperienceDetails"].ToString(),
                                         FatherName = dt1.Rows[j]["FatherName"].ToString(),
                                          Fax = dt1.Rows[j]["Fax"].ToString(),
                                           HomePinCode= dt1.Rows[j]["HomePinCode"].ToString(),
                                            HomeState= dt1.Rows[j]["HomeState"].ToString(),
                                             JobTitle=JobTitle,
                                              JoiningDate=JoiningDate.ToString().Remove(10,12),
                                               MaritalStatus= MaritalStatus,
                                                OfficeAddressLine1= dt1.Rows[j]["OfficeAddressLine1"].ToString(),
                                                 OfficeAddressLine2= dt1.Rows[j]["OfficeAddressLine2"].ToString(),
                                                  MotherName= dt1.Rows[j]["MotherName"].ToString(),
                                                   OfficeCity= dt1.Rows[j]["OfficeCity"].ToString(),
                                                    OfficePhone2= dt1.Rows[j]["OfficePhone2"].ToString(),
                                                     OfficePinCode= dt1.Rows[j]["OfficePinCode"].ToString(),
                                                      OfficeState = dt1.Rows[j]["OfficeState"].ToString(),
                                                       OficePhone1 = dt1.Rows[j]["OfficePhone1"].ToString(),
                                                        PositionId=db.TeachersManagePositions.Where(x=>x.Name==Position).FirstOrDefault().Id,
                                                         Qualification=Qualification,
                                                          SpouseName= dt1.Rows[j]["SpouseName"].ToString(),
                                                           TotalExpMonths=dt1.Rows[j]["TotalExperienceInMonths"].ToString(),
                                                            TotalExpYears=TotalExperienceInYears,
                                                            Gender=db.Genders.Where(x=>x.Name==gender).FirstOrDefault().id.ToString(),
                                                             HomeCountry=HomeCountry,
                                                              Nationality= nationality,
                                                               OfficeCountry= OfficeCountry,
                                                                GradeId=gradeId,
                                                                StaffRoleId=2
                                };

                                db.StaffDetails.Add(staffDetail);
                                db.SaveChanges();
                            }
                            else if (Action == "Edit")
                            {
                                string TeacherNumber = dt1.Rows[j]["TeacherNumber"].ToString() != null ? dt1.Rows[j]["TeacherNumber"].ToString().Trim() : "";
                                var staffDetail = db.StaffDetails.Where(x => x.TeacherNumber == TeacherNumber).FirstOrDefault();
                                if (staffDetail != null)
                                {
                                    staffDetail.HomeAddressLine1 = HomeAddressLine1;
                                    staffDetail.HomeAddressLine2 = dt1.Rows[j]["HomeAddressLine2"].ToString();
                                    staffDetail.JoiningDate = JoiningDate.ToString().Remove(10, 12);
                                    staffDetail.HomeCity = dt1.Rows[j]["HomeCity"].ToString();
                                    staffDetail.BloodGroup = Convert.ToInt32(Blood(dt1.Rows[j]["BloodGroup"].ToString()));
                                    staffDetail.DateOfBirth = DOB.ToString().Remove(10,12);
                                    staffDetail.Email = Email;
                                    staffDetail.FirstName = FirstName;
                                    staffDetail.LastName = dt1.Rows[j]["LastName"].ToString();
                                    staffDetail.MiddleName = dt1.Rows[j]["MiddleName"].ToString();
                                    staffDetail.MobileNumber = MobilePhone;
                                    staffDetail.HomePhone = dt1.Rows[j]["HomePhone"].ToString();
                                    staffDetail.CategoryId = db.TeachersManageCategories.Where(x => x.Name == Category).FirstOrDefault().Id;
                                    staffDetail.ChildrenCount = ChildrenCount;
                                    staffDetail.DepartmentId = db.TeachersManageDepartments.Where(x => x.Name == Department).FirstOrDefault().Id;
                                    staffDetail.ExperienceDetails = dt1.Rows[j]["ExperienceDetails"].ToString();
                                    staffDetail.FatherName = dt1.Rows[j]["FatherName"].ToString();
                                    staffDetail.Fax = dt1.Rows[j]["Fax"].ToString();
                                    staffDetail.HomePinCode = dt1.Rows[j]["HomePinCode"].ToString();
                                    staffDetail.HomeState = dt1.Rows[j]["HomeState"].ToString();
                                    staffDetail.JobTitle = JobTitle;
                                    staffDetail.MaritalStatus = MaritalStatus;
                                    staffDetail.OfficeAddressLine1 = dt1.Rows[j]["OfficeAddressLine1"].ToString();
                                    staffDetail.OfficeAddressLine2 = dt1.Rows[j]["OfficeAddressLine2"].ToString();
                                    staffDetail.MotherName = dt1.Rows[j]["MotherName"].ToString();
                                    staffDetail.OfficeCity = dt1.Rows[j]["OfficeCity"].ToString();
                                    staffDetail.OficePhone1 = dt1.Rows[j]["OfficePhone1"].ToString();
                                    staffDetail.OfficePhone2 = dt1.Rows[j]["OfficePhone2"].ToString();
                                    staffDetail.OfficePinCode = dt1.Rows[j]["OfficePinCode"].ToString();
                                    staffDetail.OfficeState = dt1.Rows[j]["OfficeState"].ToString();
                                    staffDetail.PositionId = db.TeachersManagePositions.Where(x => x.Name == Position).FirstOrDefault().Id;
                                    staffDetail.Qualification = Qualification;
                                    staffDetail.SpouseName = dt1.Rows[j]["SpouseName"].ToString();
                                    staffDetail.TotalExpMonths = dt1.Rows[j]["TotalExperienceInMonths"].ToString();
                                    staffDetail.TotalExpYears = TotalExperienceInYears;
                                    staffDetail.Gender = db.Genders.Where(x => x.Name == gender).FirstOrDefault().id.ToString();
                                    staffDetail.HomeCountry = HomeCountry;
                                    staffDetail.Nationality = nationality;
                                    staffDetail.OfficeCountry = OfficeCountry;
                                    staffDetail.GradeId = gradeId;

                                    db.Entry(staffDetail).State = System.Data.Entity.EntityState.Modified;
                                    db.SaveChanges();
                                }
                                else
                                {
                                    dt1.Rows[k]["Status"] = "Teacher Number is missing";
                                    dtCopy.ImportRow(dt1.Rows[k]);
                                    dt1.Rows[j].Delete();
                                    dt1.AcceptChanges();
                                    j = -1;
                                }
                            }
                           
                        }
                        else
                        {
                            dt1.Rows[k]["Status"] = "Mandatory values missing";
                            dtCopy.ImportRow(dt1.Rows[k]);
                            dt1.Rows[j].Delete();
                            dt1.AcceptChanges();
                            j = -1;
                        }
                    }
                }
                else if(ModelName=="Guardian")
                {
                    for (int j = 0; j < dt1.Rows.Count; j++)
                    {
                        k = j;
                        string StudentId = dt1.Rows[j]["StudentId"].ToString() != null ? dt1.Rows[j]["StudentId"].ToString().Trim() : "";
                        string GuardianFirstName = dt1.Rows[j]["GuardianFirstName"].ToString() != null ? dt1.Rows[j]["GuardianFirstName"].ToString().Trim() : "";
                        string Relationship = dt1.Rows[j]["Relationship"].ToString() != null ? dt1.Rows[j]["Relationship"].ToString().Trim() : "";
                        string IncomePerYear = dt1.Rows[j]["IncomePerYear"].ToString() != null ? dt1.Rows[j]["IncomePerYear"].ToString().Trim() : "";
                        string AddressLine1 = dt1.Rows[j]["AddressLine1"].ToString() != null ? dt1.Rows[j]["AddressLine1"].ToString().Trim() : "";
                        string MobileNumber = dt1.Rows[j]["MobileNumber"].ToString() != null ? dt1.Rows[j]["MobileNumber"].ToString().Trim() : "";
                        string Country = dt1.Rows[j]["Country"].ToString() != null ? dt1.Rows[j]["Country"].ToString().Trim() : "";
                        string Occupation = dt1.Rows[j]["Occupation"].ToString() != null ? dt1.Rows[j]["Occupation"].ToString().Trim() : "";
                        DateTime DOB=Convert.ToDateTime(dt1.Rows[j]["DOB"].ToString());
                        
                        if (StudentId != "" && GuardianFirstName != "" && DOB!=null&&Occupation!=""&&Relationship != "" && IncomePerYear != "" && AddressLine1 != null && MobileNumber != "")
                        {
                            bool getOut = false;
                            string status = "";
                            var ParentCountry = db.countries.Where(x => x.country_name == Country).FirstOrDefault();
                            if ( ParentCountry== null && Country != "")
                            {
                                status += "Invalid Country,";
                                getOut = true;
                            }
                            if (db.StudentDetails.Where(x => x.StudentId == StudentId).FirstOrDefault() == null && StudentId != "")
                            {
                                status += "Invalid Student Id,";
                                getOut = true;
                            }
                            if (Relation(Relationship) == null)
                            {
                                status += "Invalid Relationship";
                                getOut = true;
                            }
                            if (getOut == true)
                            {
                                dt1.Rows[k]["Status"] = status;
                                dtCopy.ImportRow(dt1.Rows[k]);
                                dt1.Rows[j].Delete();
                                dt1.AcceptChanges();
                                j = -1;
                                continue;
                            }

                            //Check Country
                            int parentCountry = 0;
                            if (ParentCountry != null) parentCountry = ParentCountry.id;

                            if (Action == "Add")
                            {
                                string guardianId = "";
                                var uidCheck = db.GuardianDetails.FirstOrDefault();
                                if (uidCheck == null) guardianId = "P000001";
                                else
                                {
                                    guardianId = (from cs in db.GuardianDetails
                                                  select cs.ParentId.Substring(1)).Max();
                                    long curValue = Convert.ToInt64(guardianId) + 1;
                                    guardianId = "P" + curValue.ToString("D6");
                                }
                                GuardianDetail guardianDetail = new GuardianDetail
                                {
                                    AddressLine1=AddressLine1,
                                    AddressLine2 =dt1.Rows[j]["AddressLine2"].ToString(),
                                     PinCode= dt1.Rows[j]["PinCode"].ToString(),
                                      Relation=Convert.ToInt32(Relation(Relationship)),
                                       State= dt1.Rows[j]["State"].ToString(),
                                        CreatedBy=Session["UserName"].ToString(),
                                         CreatedDate=DateTime.Today,
                                          City= dt1.Rows[j]["City"].ToString(),
                                           Country=parentCountry,
                                            DeleteFlag=0,
                                             DOB=DOB,
                                              Education= dt1.Rows[j]["Education"].ToString(),
                                               Email= dt1.Rows[j]["Email"].ToString(),
                                                FirstName=GuardianFirstName,
                                                 Income=IncomePerYear,
                                                  LastName= dt1.Rows[j]["GuardianLastName"].ToString(),
                                                   MobilePhone=MobileNumber,
                                                    Occupation= Occupation,
                                                     OfficePhone1= dt1.Rows[j]["OfficePhone1"].ToString(),
                                                      OfficePhone2= dt1.Rows[j]["OfficePhone2"].ToString(),
                                                       StudentDetailsId=db.StudentDetails.Where(x=>x.StudentId==StudentId).FirstOrDefault().StudentDetailsId,
                                                        ParentId=guardianId
                                };

                                db.GuardianDetails.Add(guardianDetail);
                                db.SaveChanges();
                            }
                            else if (Action == "Edit")
                            {
                                string ParentId = dt1.Rows[j]["ParentId"].ToString() != null ? dt1.Rows[j]["ParentId"].ToString().Trim() : "";
                                var guardianDetail = db.GuardianDetails.Where(x => x.ParentId == ParentId).FirstOrDefault();
                                if (guardianDetail != null)
                                {
                                    guardianDetail.AddressLine1 = AddressLine1;
                                    guardianDetail.AddressLine2 = dt1.Rows[j]["AddressLine2"].ToString();
                                    guardianDetail.PinCode = dt1.Rows[j]["PinCode"].ToString();
                                    guardianDetail.Relation = Convert.ToInt32(Relation(Relationship));
                                    guardianDetail.State = dt1.Rows[j]["State"].ToString();
                                    guardianDetail.CreatedBy = Session["UserName"].ToString();
                                    guardianDetail.CreatedDate = DateTime.Today;
                                    guardianDetail.City = dt1.Rows[j]["City"].ToString();
                                    guardianDetail.Country = parentCountry;
                                    guardianDetail.DeleteFlag = 0;
                                    guardianDetail.DOB = DOB;
                                    guardianDetail.Education = dt1.Rows[j]["Education"].ToString();
                                    guardianDetail.Email = dt1.Rows[j]["Email"].ToString();
                                    guardianDetail.FirstName = GuardianFirstName;
                                    guardianDetail.Income = IncomePerYear;
                                    guardianDetail.LastName = dt1.Rows[j]["GuardianLastName"].ToString();
                                    guardianDetail.MobilePhone = MobileNumber;
                                    guardianDetail.Occupation = Occupation;
                                    guardianDetail.OfficePhone1 = dt1.Rows[j]["OfficePhone1"].ToString();
                                    guardianDetail.OfficePhone2 = dt1.Rows[j]["OfficePhone2"].ToString();
                                    guardianDetail.StudentDetailsId = db.StudentDetails.Where(x => x.StudentId == StudentId).FirstOrDefault().StudentDetailsId;
                                                        
                                    db.Entry(guardianDetail).State = System.Data.Entity.EntityState.Modified;
                                    db.SaveChanges();
                                }
                                else
                                {
                                    dt1.Rows[k]["Status"] = "Parent # is missing";
                                    dtCopy.ImportRow(dt1.Rows[k]);
                                    dt1.Rows[j].Delete();
                                    dt1.AcceptChanges();
                                    j = -1;
                                }
                            }
                            

                        }
                        else
                        {
                            dt1.Rows[k]["Status"] = "Mandatory values missing";
                            dtCopy.ImportRow(dt1.Rows[k]);
                            dt1.Rows[j].Delete();
                            dt1.AcceptChanges();
                            j = -1;
                        }
                    }
                }
                return dtCopy;
            }
            catch(Exception e)
            {
                dt1.Rows[k]["Status"] = "Mandatory fields missing or data not in correct format";
                dtCopy.ImportRow(dt1.Rows[k]);
                dt1.Rows[k].Delete();
                dt1.AcceptChanges();
                goto first;
            }

        }
        public ActionResult SampleFile(string ExcelFormat)
        {
            int UserId = Convert.ToInt32(Session["UserId"]);
            if (UserId == 0)
            {
                return RedirectToAction("Index", "Login");
            }
            try
            {
                if (ExcelFormat == "Student")
                {
                    byte[] fileBytes = System.IO.File.ReadAllBytes(Server.MapPath("~/Content/UploadFiles/StudentDetails.xlsx"));
                    return File(fileBytes, "application/force-download", "StudentDetails.xlsx");
                }
                else if (ExcelFormat == "Teacher")
                {
                    byte[] fileBytes = System.IO.File.ReadAllBytes(Server.MapPath("~/Content/UploadFiles/TeacherDetails.xlsx"));
                    return File(fileBytes, "application/force-download", "TeacherDetails.xlsx");
                }
                else if (ExcelFormat == "Guardian")
                {
                    byte[] fileBytes = System.IO.File.ReadAllBytes(Server.MapPath("~/Content/UploadFiles/GuardianDetails.xlsx"));
                    return File(fileBytes, "application/force-download", "GuardianDetails.xlsx");
                }
                return RedirectToAction("FileUpload_Index");
            }
            catch (Exception e)
            {
                TempData["FileUpload_Index"] = "Unable to Download";
                return RedirectToAction("FileUpload_Index");
            }
        }
        public ActionResult CreateUser()
        {
            return View(new List<UserCreation>());
        }
        public ActionResult GetUsers(string UserRole)
        { 
            var currentUsers = db.Users.Select(x=>x.UserName).Distinct();
            List<UserCreation> userCreations = new List<UserCreation>();
            if (UserRole == "Student")
            {
                //Getting Students
                var students = db.StudentDetails.Select(x => x.StudentId).Distinct();
                var users = students.Except(currentUsers);
                foreach (var item in users)
                {
                    var student = db.StudentDetails.Where(x => x.StudentId == item).FirstOrDefault();
                    if (student != null)
                    {
                        UserCreation userCreation = new UserCreation
                        {
                            Email = student.Email,
                            IsSelected = false,
                            MobileNumber = student.PhoneNumber1,
                            UserId = student.StudentId,
                            FirstName = student.FirstName,
                            LastName = student.MiddleName + " " + student.LastName
                        };
                        var role = db.SettingsUserRoles.Where(x => x.Name == "Student").FirstOrDefault();
                        if (role != null) userCreation.RoleId = role.Id;
                        userCreations.Add(userCreation);
                    }
                }
                if (userCreations.Count== 0) TempData["CreateUser"] = "No Users found to create account";
                return View("CreateUser", userCreations);
            }
            else if (UserRole == "Teacher")
            {
                //Getting Teachers
                var teachers = db.StaffDetails.Select(x => x.TeacherNumber).Distinct();
                var users = teachers.Except(currentUsers);
                foreach (var item in users)
                {
                    var teacher = db.StaffDetails.Where(x => x.TeacherNumber == item).FirstOrDefault();
                    if (teacher != null)
                    {
                        UserCreation userCreation = new UserCreation
                        {
                            Email = teacher.Email,
                            IsSelected = false,
                            MobileNumber = teacher.MobileNumber,
                            UserId = teacher.TeacherNumber,
                            FirstName = teacher.FirstName,
                            LastName = teacher.MiddleName + " " + teacher.LastName,
                        };
                        var role = db.SettingsUserRoles.Where(x => x.Name == "Teacher").FirstOrDefault();
                        if (role != null) userCreation.RoleId = role.Id;
                        userCreations.Add(userCreation);
                    }
                }
                if (userCreations.Count == 0) TempData["CreateUser"] = "No Users found to create account";
                return View("CreateUser", userCreations);
            }
            else if (UserRole == "Guardian")
            {
                //Getting Guardians
                var guardians = db.GuardianDetails.Select(x => x.ParentId).Distinct();
                var users = guardians.Except(currentUsers);
                foreach (var item in users)
                {
                    var guardian = db.GuardianDetails.Where(x => x.ParentId == item).FirstOrDefault();
                    if (guardian != null)
                    {
                        UserCreation userCreation = new UserCreation
                        {
                            Email = guardian.Email,
                            IsSelected = false,
                            MobileNumber = guardian.MobilePhone,
                            UserId = guardian.ParentId,
                            FirstName = guardian.FirstName,
                            LastName = guardian.LastName
                        };
                        var role = db.SettingsUserRoles.Where(x => x.Name == "Guardian").FirstOrDefault();
                        if (role != null) userCreation.RoleId = role.Id;
                        userCreations.Add(userCreation);
                    }
                }
                if (userCreations.Count == 0) TempData["CreateUser"] = "No Users found to create account";
                return View("CreateUser", userCreations);
            }
            return View("CreateUser", new List<UserCreation>());
        }
        [HttpPost]
        public ActionResult CreateUser(List<UserCreation> Users)
        {
            foreach(var item in Users)
            {
                commonCreateUser(item.UserId, item.Email, item.RoleId, item.MobileNumber, item.FirstName, item.LastName, 1);
            }
            if (Users.Count > 0)
                TempData["CreateUser"] = Users.Count+" User(s) Created Successfully";
            return View(Users);
        }
        public void commonCreateUser(String UserName, String Email, int RoleId, String MobileNumber, String FirstName, String LastName, int Status)
        {
            String pwd = RandomGen(6);
            var user = db.Users.Where(x => x.UserName == UserName).FirstOrDefault();
            if(user!=null)
            {
                user.Email = Email;
                user.FirstName = FirstName;
                user.LastName = LastName;
                user.MobileNumber = MobileNumber;
                user.SuperUser = RoleId;
                db.Entry(user).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
            else
            {
                using (conn=new SqlConnection(ConfigurationManager.ConnectionStrings["GS247"].ConnectionString))
                {
                    if (conn.State != ConnectionState.Open)
                        conn.Open();
                    cmd = new SqlCommand("Insert into Users(UserName,Password,Email,MobileNumber,SuperUser,Status,FirstName,LastName) values('"+
                        UserName+"','"+pwd+"','"+Email+"','"+MobileNumber+"','"+RoleId+"','1','"+FirstName+"','"+LastName+"')",conn);
                    cmd.ExecuteNonQuery();
                }
                
            }
        }
        public String RandomGen(int length)
        {
            string allowedChars = "";
            allowedChars = "a,b,c,d,e,f,g,h,i,j,k,l,m,n,o,p,q,r,s,t,u,v,w,x,y,z,";
            allowedChars += "A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z,";
            allowedChars += "1,2,3,4,5,6,7,8,9,0,!,@,#,$,%,&,?";
            char[] sep = { ',' };
            string[] arr = allowedChars.Split(sep);
            string passwordString = "";
            string temp = "";
            Random rand = new Random();
            for (int i = 0; i < length; i++)
            {
                temp = arr[rand.Next(0, arr.Length)];
                passwordString += temp;
            }
            return passwordString;
        }
        public string Gender(string genderText)
        {
            List<System.Web.Mvc.SelectListItem> Gender = new List<System.Web.Mvc.SelectListItem> {
                new System.Web.Mvc.SelectListItem { Text = "Male", Value = "1" },
            new System.Web.Mvc.SelectListItem { Text = "Female", Value = "2" },
            new System.Web.Mvc.SelectListItem { Text = "Unknown", Value = "3" }};
            string gender = Gender.Where(x => x.Text == genderText).Select(x => x.Value).SingleOrDefault();
            return gender;
        }
        public string Relation(string relation)
        {
            List<System.Web.Mvc.SelectListItem> Relation = new List<System.Web.Mvc.SelectListItem> {
                new System.Web.Mvc.SelectListItem { Text = "Father", Value = "1" },
            new System.Web.Mvc.SelectListItem { Text = "Mother", Value = "2" },
            new System.Web.Mvc.SelectListItem { Text = "Other", Value = "3" }};
            string relationshipId = Relation.Where(x => x.Text == relation).Select(x => x.Value).SingleOrDefault();
            return relationshipId;
        }
        public string Blood(string bloodText)
            {
            List<System.Web.Mvc.SelectListItem> Blood = new List<System.Web.Mvc.SelectListItem> {
                new System.Web.Mvc.SelectListItem { Text = "Unknown", Value = "0" },
            new System.Web.Mvc.SelectListItem { Text = "A+", Value = "1" },
            new System.Web.Mvc.SelectListItem { Text = "A-", Value = "2" },
            new System.Web.Mvc.SelectListItem { Text = "B+", Value = "3" },
            new System.Web.Mvc.SelectListItem { Text = "B-", Value = "4" },
            new System.Web.Mvc.SelectListItem { Text = "O+", Value = "5" },
            new System.Web.Mvc.SelectListItem { Text = "O-", Value = "6" },
            new System.Web.Mvc.SelectListItem { Text = "AB+", Value = "7" },
            new System.Web.Mvc.SelectListItem { Text = "AB-", Value = "8" },
            };
            string blood = Blood.Where(x => x.Text == bloodText).Select(x => x.Value).SingleOrDefault();
            return blood;
        }

    }
}