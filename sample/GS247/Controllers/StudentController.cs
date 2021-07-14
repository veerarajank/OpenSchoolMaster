using GS247.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace GS247.Controllers
{    
    public class StudentController : Controller
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["GS247"].ConnectionString);
        Connection.Connectivity Connect = new Connection.Connectivity();

        #region "STUDENT"
        public ActionResult Index()
        {
            if (sessionValidate())
            {
                var List = new List<StudentDetailCO>();
                using (var context = new GS247Entities8())
                {
                    var data = context.StudentDetails.Where(x => (x.DeleteFlag == 0 || x.DeleteFlag == null) && (x.OnlineStatus == 0 || x.OnlineApplicationStatus == 1));

                    var studentList = data.OrderByDescending(x => x.CreatedDate).Take(10).ToList();

                    studentList.ForEach(itr =>
                    {
                        var newData = new StudentDetailCO();
                        newData.StudentDetailsId = itr.StudentDetailsId;
                        newData.AdmissionNo = itr.AdmissionNo;
                        newData.AdmissionDate = itr.AdmissionDate;
                        newData.FirstName = itr.FirstName;
                        newData.LastName = itr.LastName;
                        newData.MiddleName = itr.MiddleName;
                        newData.StudentId = itr.StudentId;
                        newData.Batch = itr.Batch;
                        newData.DOB = itr.DOB;
                        newData.Gender = itr.Gender;
                        newData.Blood = itr.Blood;
                        newData.BirthPlace = itr.BirthPlace;
                        newData.Language = itr.Language;
                        newData.Nationality = itr.Nationality;
                        newData.Religion = itr.Religion;
                        newData.StudentCategory = itr.StudentCategory;
                        newData.PromitionalFlag = itr.PromitionalFlag;
                        newData.AddressLine1 = itr.AddressLine1;
                        newData.AddressLine2 = itr.AddressLine2;
                        newData.City = itr.City;
                        newData.State = itr.State;
                        newData.PinCode = itr.PinCode;
                        newData.PhoneNumber1 = itr.PhoneNumber1;
                        newData.PhoneNumber2 = itr.PhoneNumber2;
                        newData.Email = itr.Email;
                        newData.FileName = itr.FileName;
                        newData.Data = itr.Data;
                        newData.CreatedBy = itr.CreatedBy;
                        newData.CreatedDate = itr.CreatedDate;
                        newData.UpdatedBy = itr.UpdatedBy;
                        newData.UpdatedDate = itr.UpdatedDate;
                        newData.Country = itr.Country;
                        newData.DeleteFlag = itr.DeleteFlag;
                        newData.RollNumber = itr.RollNumber;
                        newData.Semester = itr.Semester;
                        newData.AcademicYear = itr.AcademicYear;
                        newData.AcademicYear = itr.AcademicYear;
                        newData.AcademicStatus = itr.AcademicStatus;
                        newData.Status = itr.Status;
                        newData.OnlineStatus = itr.OnlineStatus;
                        newData.OnlineApplicationStatus = itr.OnlineApplicationStatus;

                        var SchoolCO = new SchoolCO();
                        SchoolCO.GenderName = itr.Gender == 1 ? "Male" : itr.Gender == 2 ? "Female" : "Unknown";
                        var batch = context.batches.Find(itr.Batch);
                        if (batch != null)
                        {
                            var course = (from c in context.Courses
                                          join cbl in context.BatchCourseLinks on c.Id equals cbl.CourseId
                                          join b in context.batches on cbl.BatchId equals b.Id
                                          where b.Id == batch.Id
                                          select c).FirstOrDefault();

                            SchoolCO.CourseBatchName = course != null ? course.Name + " / " + batch.Name : "";
                        }
                        else
                            SchoolCO.CourseBatchName = "";

                        newData.SchoolCO = SchoolCO;
                        List.Add(newData);

                    });
                    ViewBag.TotalStudents = data.Count();
                    ViewBag.NewAdmissionStudents = data.Where(x => x.CreatedDate.Year == DateTime.Now.Year && x.CreatedDate.Month == DateTime.Now.Month).Count();
                    ViewBag.StudentList = List;
                }
                return View();
            }
            else
                return RedirectToAction("Index", "Login");
        }

        public ActionResult ManageStudentList()
        {
            if (sessionValidate())
            {
                using (var context = new GS247Entities8())
                {
                    ViewBag.CoursesList = context.Courses.Where(x => x.IsEnable == true).ToList();
                    ViewBag.CountryList = context.countries.ToList();
                }
                return View();
            }
            else
                return RedirectToAction("Index", "Login");
        }

        public ActionResult ManageLog()
        {
            if (sessionValidate())
            {
                var manageFieldsList = new List<StudentFieldSetting>();
                int id = Convert.ToInt32(Session["UserId"].ToString());
                using (var context = new GS247Entities8())
                {
                    var FieldsList = context.StudentLogCategories.Where(x => x.UserId > 0).ToList();
                    var totalPages = (int)Math.Ceiling((decimal)FieldsList.Count / (decimal)10);
                    ViewBag.PageCount = totalPages;
                    ViewBag.StudentLogCategoriesList = FieldsList.Take(10).ToList();
                    ViewBag.CurrentPageIndex = 1;
                }
                return View();
            }
            else
                return RedirectToAction("Index", "Login");
        }

        public ActionResult CreateLog()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateLog(StudentLogCategory collection)
        {
            if (sessionValidate())
            {
                using (var context = new GS247Entities8())
                {
                    if(!context.StudentLogCategories.Where(x => x.Name.ToUpper().Equals(collection.Name.ToUpper())).Any())
                    {
                        int id = Convert.ToInt32(Session["UserId"].ToString());
                        collection.CreatedBy = Session["UserName"].ToString();
                        collection.CreatedDate = DateTime.Now;
                        collection.UserId = id;
                        context.StudentLogCategories.Add(collection);
                        context.SaveChanges();
                    }
                    else
                    {
                        ViewBag.Message = "Already name available";
                        return View();
                    }                    
                }
                return RedirectToAction("ManageLog", "Student");
            }
            else
                return RedirectToAction("Index", "Login");
        }

        [HttpGet]
        public ActionResult EditStudentLog(int StudentLogCategoryID)
        {
            if (sessionValidate())
            {
                int id = Convert.ToInt32(Session["UserId"].ToString());
                using (var context = new GS247Entities8())
                {
                    var _templist = context.StudentLogCategories.Find(StudentLogCategoryID);
                    return View("CreateLog", _templist);
                }
            }
            else
                return RedirectToAction("Index", "Login");
        }

        [HttpPost]
        public ActionResult UpdateStudentLog(StudentLogCategory collection)
        {
            if (sessionValidate())
            {
                using (var context = new GS247Entities8())
                {
                    if (!context.StudentLogCategories.Where(x => x.Name.ToUpper().Equals(collection.Name.ToUpper())).Any())
                    {
                        var _templist = context.StudentLogCategories.Find(collection.StudentLogCategoryID);
                        if (_templist != null)
                        {
                            _templist.Name = collection.Name;
                            _templist.AllowStatusFlag = collection.AllowStatusFlag;
                            _templist.UpdatedDate = DateTime.Now;
                            _templist.UpdatedBy = Session["UserName"].ToString();
                            context.Entry(_templist).State = EntityState.Modified;
                            context.SaveChanges();
                        }
                    }
                    else
                    {
                        ViewBag.Message = "Already name available";
                        var _templist = context.StudentLogCategories.Find(collection.StudentLogCategoryID);
                        return View("CreateLog",_templist);
                    }
                }
                return RedirectToAction("ManageLog", "Student");
            }
            else
                return RedirectToAction("Index", "Login");
        }

        [HttpPost]
        public ActionResult DeleteStudentLog(int StudentLogCategoryID)
        {
            if (sessionValidate())
            {
                using (var context = new GS247Entities8())
                {
                    var subscriptions = context.StudentLogCategories.Find(StudentLogCategoryID);
                    if (subscriptions != null)
                    {
                        context.StudentLogCategories.Remove(subscriptions);
                        context.SaveChanges();
                    }
                }
                return new EmptyResult();
            }
            else
                return RedirectToAction("Index", "Login");
        }

        public ActionResult ManageStudentCategory()
        {
            if (sessionValidate())
            {
                var List = new List<StudentCategoryCO>();

                int id = Convert.ToInt32(Session["UserId"].ToString());
                using (var context = new GS247Entities8())
                {
                    var FieldsList = context.StudentCategories.Where(x => x.UserId > 0).ToList();
                    var totalPages = (int)Math.Ceiling((decimal)FieldsList.Count / (decimal)10);
                    ViewBag.PageCount = totalPages;

                    var studentCategoryList = FieldsList.Take(10).ToList();

                    studentCategoryList.ForEach(data =>
                    {
                        var StudentCategory = new StudentCategoryCO();
                        StudentCategory.StudentCategoryID = data.StudentCategoryID;
                        StudentCategory.UserId = data.UserId;
                        StudentCategory.Name = data.Name;
                        StudentCategory.CreatedBy = data.CreatedBy;
                        StudentCategory.CreatedDate = data.CreatedDate;
                        StudentCategory.UpdatedBy = data.UpdatedBy;
                        StudentCategory.UpdatedDate = data.UpdatedDate;

                        var SchoolCO = new SchoolCO();

                        SchoolCO.NumberOfStudents = context.StudentDetails.Where(x => x.StudentCategory == data.StudentCategoryID).Count();
                        StudentCategory.SchoolCO = SchoolCO;
                        List.Add(StudentCategory);
                    });

                    ViewBag.StudentCategoriesList = List;
                    ViewBag.CurrentPageIndex = 1;
                }
                return View();
            }
            else
                return RedirectToAction("Index", "Login");
        }

        [HttpPost]
        public ActionResult CreateStudentCategory(string Name, int StudentCategoryID)
        {
            if (sessionValidate())
            {
                var message = "";
                using (var context = new GS247Entities8())
                {
                    if (!context.StudentCategories.Where(x => x.Name.ToUpper().Equals(Name.ToUpper()) && x.StudentCategoryID != StudentCategoryID).Any())
                    {
                        int id = Convert.ToInt32(Session["UserId"].ToString());
                        var collection = context.StudentCategories.Find(StudentCategoryID);
                        if (collection == null)
                        {
                            var newData = new StudentCategory();
                            newData.Name = Name;
                            newData.CreatedBy = Session["UserName"].ToString();
                            newData.CreatedDate = DateTime.Now;
                            newData.UserId = id;
                            context.StudentCategories.Add(newData);
                            context.SaveChanges();
                        }
                        else
                        {
                            collection.Name = Name;
                            collection.UpdatedDate = DateTime.Now;
                            collection.UpdatedBy = Session["UserName"].ToString();
                            context.Entry(collection).State = EntityState.Modified;
                            context.SaveChanges();
                        }
                    }
                    else
                        message = "Already name available";                    
                }
                return Json(new { Message = message });
            }
            else
                return RedirectToAction("Index", "Login");
        }

        [HttpGet]
        public JsonResult getStudentDetails(int StudentCategoryID)
        {
            List<StudentDetail> studentList = new List<StudentDetail>();
            using (var context = new GS247Entities8())
            {
                studentList = context.StudentDetails.Where(x => x.StudentCategory == StudentCategoryID && (x.DeleteFlag == 0 || x.DeleteFlag == null)).ToList();
            }
            return Json(new { StudentList = studentList }, JsonRequestBehavior.AllowGet);
        }

        public class StudentName
        {
            public string Name { get; set; }
        }

        [HttpPost]
        public ActionResult DeleteStudentCategory(int StudentCategoryID)
        {
            if (sessionValidate())
            {
                var message = string.Empty;
                using (var context = new GS247Entities8())
                {
                    if (!context.StudentDetails.Where(x => x.StudentCategory == StudentCategoryID).Any())
                    {
                        var subscriptions = context.StudentCategories.Find(StudentCategoryID);
                        if (subscriptions != null)
                        {
                            context.StudentCategories.Remove(subscriptions);
                            context.SaveChanges();
                        }
                    }
                    else
                        message = "Student category refrenced in other modules can't be delete";
                }
                return Json(new { Message = message });
            }
            else
                return RedirectToAction("Index", "Login");
        }

        public ActionResult ListGuardian()
        {
            if (sessionValidate())
            {
                var guardianCOList = new List<GuardianDetailCO>();
                int id = Convert.ToInt32(Session["UserId"].ToString());
                using (var context = new GS247Entities8())
                {
                    var FieldsList = (from a in context.GuardianDetails
                                      join b in context.StudentDetails on a.StudentDetailsId equals b.StudentDetailsId
                                      where a.DeleteFlag == 0 && b.DeleteFlag == 0 && (b.OnlineStatus == 0 || b.OnlineApplicationStatus == 1)
                                      select a);  

                    var totalPages = (int)Math.Ceiling((decimal)FieldsList.Count() / (decimal)10);
                    ViewBag.PageCount = totalPages;
                    var guardianList = FieldsList.Take(10).ToList();

                    guardianList.ForEach(itr =>
                    {
                        var GuardianDetails = new GuardianDetailCO();
                        GuardianDetails.GuardianDetailsId = itr.GuardianDetailsId;
                        GuardianDetails.StudentDetailsId = itr.StudentDetailsId;
                        GuardianDetails.FirstName = itr.FirstName;
                        GuardianDetails.LastName = itr.LastName;
                        GuardianDetails.Relation = itr.Relation;
                        GuardianDetails.DOB = itr.DOB;
                        GuardianDetails.Education = itr.Education;
                        GuardianDetails.Occupation = itr.Occupation;
                        GuardianDetails.Income = itr.Income;
                        GuardianDetails.AddressLine1 = itr.AddressLine1;
                        GuardianDetails.AddressLine2 = itr.AddressLine2;
                        GuardianDetails.City = itr.City;
                        GuardianDetails.State = itr.State;
                        GuardianDetails.PinCode = itr.PinCode;
                        GuardianDetails.MobilePhone = itr.MobilePhone;
                        GuardianDetails.OfficePhone1 = itr.OfficePhone1;
                        GuardianDetails.OfficePhone2 = itr.OfficePhone2;
                        GuardianDetails.Email = itr.Email;
                        GuardianDetails.Country = itr.Country;
                        GuardianDetails.ParentUserFlag = itr.ParentUserFlag;
                        GuardianDetails.CreatedBy = itr.CreatedBy;
                        GuardianDetails.CreatedDate = itr.CreatedDate;
                        GuardianDetails.UpdatedBy = itr.UpdatedBy;
                        GuardianDetails.UpdatedDate = itr.UpdatedDate;
                        GuardianDetails.DeleteFlag = itr.DeleteFlag;

                        var SchoolCO = new SchoolCO();
                        var studentData = context.StudentDetails.Find(itr.StudentDetailsId);
                        SchoolCO.StudentName = studentData != null ? studentData.FirstName + " " + studentData.LastName : "";
                        GuardianDetails.SchoolCO = SchoolCO;
                        guardianCOList.Add(GuardianDetails);
                    });
                    ViewBag.GuardianList = guardianCOList;
                    ViewBag.CurrentPageIndex = 1;
                }
                return View();
            }
            else
                return RedirectToAction("Index", "Login");
        }

        #endregion

        #region "LEAVE TYPE"
        public ActionResult AddLeaveType()
        {
            if (sessionValidate())
            {
                int id = Convert.ToInt32(Session["UserId"].ToString());
                using (var context = new GS247Entities8())
                {
                    var FieldsList = context.StudentLeaveTypes.Where(x => x.userId > 0).ToList();
                    ViewBag.activeList = FieldsList.Where(x => x.StatusFlag == 1).ToList();
                    ViewBag.inActiveList = FieldsList.Where(x => x.StatusFlag == 2).ToList();
                }
                return View();
            }
            else
                return RedirectToAction("Index", "Login");
        }

        [HttpPost]
        public ActionResult CreateLeaveType(StudentLeaveType collection)
        {
            if (sessionValidate())
            {
                var message = string.Empty;
                using (var context = new GS247Entities8())
                {
                    if (!context.StudentLeaveTypes.Where(x => x.StudentLeaveTypeID != collection.StudentLeaveTypeID && x.Name.ToUpper().Equals(collection.Name.ToUpper())).Any())
                    {
                        var _templist = context.StudentLeaveTypes.Find(collection.StudentLeaveTypeID);
                        if (_templist == null)
                        {
                            int id = Convert.ToInt32(Session["UserId"].ToString());
                            collection.CreatedBy = Session["UserName"].ToString();
                            collection.CreatedDate = DateTime.Now;
                            collection.userId = id;
                            context.StudentLeaveTypes.Add(collection);
                            context.SaveChanges();
                        }
                        else
                        {
                            _templist.Name = collection.Name;
                            _templist.Code = collection.Code;
                            _templist.Label = collection.Label;
                            _templist.ColorCode = collection.ColorCode;
                            _templist.StatusFlag = collection.StatusFlag;
                            _templist.AttendanceFlag = collection.AttendanceFlag;
                            _templist.UpdatedDate = DateTime.Now;
                            _templist.UpdatedBy = Session["UserName"].ToString();
                            context.Entry(_templist).State = EntityState.Modified;
                            context.SaveChanges();
                        }
                    }
                    else
                        message = "Already name available";
                }
                return Json(new { Message = message });
            }
            else
                return RedirectToAction("Index", "Login");
        }

        [HttpGet]
        public JsonResult getStudentLeaveType(int StudentLeaveTypeID)
        {
            StudentLeaveType StudentLeaveType = new StudentLeaveType();
            using (var context = new GS247Entities8())
            {
                StudentLeaveType = context.StudentLeaveTypes.Find(StudentLeaveTypeID);
            }
            return Json(new { StudentLeaveType = StudentLeaveType }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult DeleteStudentLeaveType(int StudentLeaveTypeID)
        {
            if (sessionValidate())
            {
                using (var context = new GS247Entities8())
                {
                    var subscriptions = context.StudentLeaveTypes.Find(StudentLeaveTypeID);
                    if (subscriptions != null)
                    {
                        context.StudentLeaveTypes.Remove(subscriptions);
                        context.SaveChanges();
                    }
                }
                return new EmptyResult();
            }
            else
                return RedirectToAction("Index", "Login");
        }

        #endregion

        #region "STUDENT FIELD SETTINGS"
        public ActionResult StudentFieldSettings()
        {
            if (sessionValidate())
                return View();
            else
                return RedirectToAction("Index", "Login");
        }

        [HttpPost]
        public ActionResult StudentFieldSettings(StudentFieldSetting collection)
        {
            if (sessionValidate())
            {
                using (var context = new GS247Entities8())
                {
                    int id = Convert.ToInt32(Session["UserId"].ToString());
                    collection.CreatedBy = Session["UserName"].ToString();
                    collection.CreatedDate = DateTime.Now;
                    collection.UserID = id;
                    collection.StatusFlag = 2;
                    collection.Category = "Students";
                    context.StudentFieldSettings.Add(collection);
                    context.SaveChanges();
                }
                return RedirectToAction("ManageStudentFields");
            }
            else
                return RedirectToAction("Index", "Login");
        }

        public void addNewColumn(StudentFieldSetting collection)
        {
            //String pwd = RandomGen(6);
            //con.Open();
            //Connect.QueryExecute(con, "INSERT INTO dbo.Users (UserName,Password,Email,MobileNumber,SuperUser,Status,FirstName,LastName) VALUES('" + UserName + "','" + pwd + "','" + Email + "','" + MobileNumber + "'," + RoleId + "," + Status + ",'" + FirstName + "','" + LastName + "')");
            //con.Close();
        }

        public ActionResult ManageStudentFields()
        {
            if (sessionValidate())
            {
                var manageFieldsList = new List<StudentFieldSetting>();
                int id = Convert.ToInt32(Session["UserId"].ToString());
                using (var context = new GS247Entities8())
                {
                    var FieldsList = context.StudentFieldSettings.Where(x => x.UserID > 0).ToList();
                    var totalPages = (int)Math.Ceiling((decimal)FieldsList.Count / (decimal)10);
                    ViewBag.PageCount = totalPages;
                    ViewBag.manageFieldsList = FieldsList.Take(10).ToList();
                    ViewBag.CurrentPageIndex = 1;
                }
                return View();
            }
            else
                return RedirectToAction("Index", "Login");
        }

        [HttpGet]
        public ActionResult EditStudentFields(int StudentFieldSettingID)
        {
            if (sessionValidate())
            {
                using (var context = new GS247Entities8())
                {
                    var _templist = context.StudentFieldSettings.Where(x => x.StudentFieldSettingID == StudentFieldSettingID).FirstOrDefault();
                    return View("EditStudentFields", _templist);
                }
            }
            else
                return RedirectToAction("Index", "Login");
        }

        [HttpPost]
        public ActionResult UpdateStudentFields(StudentFieldSetting collection)
        {
            if (sessionValidate())
            {
                using (var context = new GS247Entities8())
                {
                    var _templist = context.StudentFieldSettings.Where(x => x.StudentFieldSettingID == collection.StudentFieldSettingID).FirstOrDefault();
                    if (_templist != null)
                    {
                        _templist.Title = collection.Title;
                        _templist.Required = collection.Required;
                        _templist.FieldTypeFlag = collection.FieldTypeFlag;
                        _templist.CheckAllFlag = collection.CheckAllFlag != null ? collection.CheckAllFlag : 0;
                        _templist.AdminStudentRegistrationFormFlag = collection.AdminStudentRegistrationFormFlag != null ? collection.AdminStudentRegistrationFormFlag : 0;
                        _templist.OnlineAdmissionFormFlag = collection.OnlineAdmissionFormFlag != null ? collection.OnlineAdmissionFormFlag : 0;
                        _templist.StudentPortalFlag = collection.StudentPortalFlag != null ? collection.StudentPortalFlag : 0;
                        _templist.StudentProfileFlag = collection.StudentProfileFlag != null ? collection.StudentProfileFlag : 0;
                        _templist.StudentProfilePDFFlag = collection.StudentProfilePDFFlag != null ? collection.StudentProfilePDFFlag : 0;
                        _templist.ParentPortalFlag = collection.ParentPortalFlag != null ? collection.ParentPortalFlag : 0;
                        _templist.TeacherPortalFlag = collection.TeacherPortalFlag != null ? collection.TeacherPortalFlag : 0;
                        _templist.TabSelection = collection.TabSelection != null ? collection.TabSelection : 0;
                        _templist.TabSubSelection = collection.TabSubSelection != null ? collection.TabSubSelection : 0;
                        _templist.UpdatedDate = DateTime.Now;
                        _templist.UpdatedBy = Session["UserName"].ToString();
                        context.Entry(_templist).State = EntityState.Modified;
                        context.SaveChanges();
                    }
                }
                return RedirectToAction("ManageStudentFields");
            }
            else
                return RedirectToAction("Index", "Login");
        }

        public ActionResult SearchFieldSettings(SearchSettingFields SearchSettingFields)
        {
            if (sessionValidate())
            {
                var manageFieldsList = new List<StudentFieldSetting>();
                int id = Convert.ToInt32(Session["UserId"].ToString());
                using (var context = new GS247Entities8())
                {
                    var FieldsList = context.StudentFieldSettings.Where(x => x.UserID > 0).ToList();
                    if (!string.IsNullOrEmpty(SearchSettingFields.Title))
                        FieldsList = FieldsList.Where(x => x.Title.Contains(SearchSettingFields.Title)).ToList();
                    if (SearchSettingFields.Flag != "0")
                        FieldsList = FieldsList.Where(x => x.FieldTypeFlag == Convert.ToInt16(SearchSettingFields.Flag)).ToList();

                    var totalPages = (int)Math.Ceiling((decimal)FieldsList.Count / (decimal)10);
                    ViewBag.PageCount = totalPages;
                    ViewBag.CurrentPageIndex = Convert.ToInt16(SearchSettingFields.CurrentPage);
                    ViewBag.manageFieldsList = FieldsList.Skip((Convert.ToInt16(SearchSettingFields.CurrentPage) - 1) * 10).Take(10).ToList();
                }
                return View("ManageStudentFields");
            }
            else
                return RedirectToAction("Index", "Login");
        }

        [HttpPost]
        public ActionResult PagenationFieldSettings(SearchSettingFields SearchSettingFields)
        {
            if (sessionValidate())
            {
                var manageFieldsList = new List<StudentFieldSetting>();
                int id = Convert.ToInt32(Session["UserId"].ToString());
                using (var context = new GS247Entities8())
                {
                    var FieldsList = context.StudentFieldSettings.Where(x => x.UserID > 0).ToList();
                    if (!string.IsNullOrEmpty(SearchSettingFields.Title))
                        FieldsList = FieldsList.Where(x => x.Title.Contains(SearchSettingFields.Title)).ToList();
                    if (SearchSettingFields.Flag != "0")
                        FieldsList = FieldsList.Where(x => x.FieldTypeFlag == Convert.ToInt16(SearchSettingFields.Flag)).ToList();

                    var totalPages = (int)Math.Ceiling((decimal)FieldsList.Count / (decimal)10);
                    ViewBag.PageCount = totalPages;
                    ViewBag.CurrentPageIndex = Convert.ToInt16(SearchSettingFields.CurrentPage);
                    ViewBag.manageFieldsList = FieldsList.Skip((Convert.ToInt16(SearchSettingFields.CurrentPage) - 1) * 10).Take(10).ToList();
                }
                return View("ManageStudentFields", ViewBag.manageFieldsList);
            }
            else
                return RedirectToAction("Index", "Login");
        }

        public JsonResult updateManageStudentFields(string flag, string checkValue, string StudentFieldSettingID)
        {
            using (var context = new GS247Entities8())
            {
                int id = Convert.ToInt32(StudentFieldSettingID);
                var _templist = context.StudentFieldSettings.Where(x => x.StudentFieldSettingID == id).FirstOrDefault();
                if (_templist != null)
                {
                    if (flag == "AdminStudentRegistrationFormFlag")
                        _templist.AdminStudentRegistrationFormFlag = Convert.ToInt32(checkValue);
                    else if (flag == "OnlineAdmissionFormFlag")
                        _templist.OnlineAdmissionFormFlag = Convert.ToInt32(checkValue);
                    else if (flag == "StudentPortalFlag")
                        _templist.StudentPortalFlag = Convert.ToInt32(checkValue);
                    else if (flag == "StudentProfileFlag")
                        _templist.StudentProfileFlag = Convert.ToInt32(checkValue);
                    else if (flag == "StudentProfilePDFFlag")
                        _templist.StudentProfilePDFFlag = Convert.ToInt32(checkValue);
                    else if (flag == "ParentPortalFlag")
                        _templist.ParentPortalFlag = Convert.ToInt32(checkValue);
                    else if (flag == "TeacherPortalFlag")
                        _templist.TeacherPortalFlag = Convert.ToInt32(checkValue);
                    _templist.UpdatedDate = DateTime.Now;
                    _templist.UpdatedBy = Session["UserName"].ToString();
                    context.Entry(_templist).State = EntityState.Modified;
                    context.SaveChanges();
                }
            }
            return Json(new { StudentFieldSettingID = StudentFieldSettingID });
        }

        public class SearchSettingFields
        {
            public string Title { get; set; }
            public string Flag { get; set; }
            public string CurrentPage { get; set; }
        }

        #endregion

        #region "STUDENT DETAILS"

        [HttpPost]
        public JsonResult GetStudentList(string listsize, string currentPageIndex, string alphaValue, StudentAdvanceSearch studentAdvanceSearch)
        {
            List<StudentDetail> studentList = new List<StudentDetail>();
            List<StudentDetailCO> studentCOList = new List<StudentDetailCO>();
            var totalPages = 0;
            using (var context = new GS247Entities8())
            {                
                studentList = context.StudentDetails.Where(x => (x.DeleteFlag == 0 || x.DeleteFlag == null) && (x.OnlineStatus == 0 || x.OnlineApplicationStatus == 1)).ToList();
                if (alphaValue != "All")
                {
                    var query = studentList.AsEnumerable();
                    studentList = query.Where(m => m.FirstName[0] == Convert.ToChar(alphaValue)).ToList();
                }

                if (!string.IsNullOrEmpty(studentAdvanceSearch.NameTxt))
                    studentList = studentList.Where(x => x.FirstName.Contains(studentAdvanceSearch.NameTxt) || x.LastName.Contains(studentAdvanceSearch.NameTxt)).ToList();
                if (!string.IsNullOrEmpty(studentAdvanceSearch.AdmissionNumberTxt))
                    studentList = studentList.Where(x => x.AdmissionNo.Contains(studentAdvanceSearch.AdmissionNumberTxt)).ToList();
                if (!string.IsNullOrEmpty(studentAdvanceSearch.GenderTxt))
                    studentList = studentList.Where(x => x.Gender == Convert.ToInt32(studentAdvanceSearch.GenderTxt)).ToList();
                if (!string.IsNullOrEmpty(studentAdvanceSearch.BloodGroupTxt))
                    studentList = studentList.Where(x => x.Blood == Convert.ToInt32(studentAdvanceSearch.BloodGroupTxt)).ToList();
                if (!string.IsNullOrEmpty(studentAdvanceSearch.CountryTxt))
                    studentList = studentList.Where(x => x.Country == Convert.ToInt32(studentAdvanceSearch.CountryTxt)).ToList();
                if (!string.IsNullOrEmpty(studentAdvanceSearch.DOBTxt))
                    studentList = studentList.Where(x => x.DOB.GetValueOrDefault().Date == Convert.ToDateTime(studentAdvanceSearch.DOBTxt).Date).ToList();
                if (!string.IsNullOrEmpty(studentAdvanceSearch.AdmissionDateTxt))
                    studentList = studentList.Where(x => x.AdmissionDate.GetValueOrDefault().Date == Convert.ToDateTime(studentAdvanceSearch.AdmissionDateTxt).Date).ToList();
                if (!string.IsNullOrEmpty(studentAdvanceSearch.CourseId))
                {
                    if (!string.IsNullOrEmpty(studentAdvanceSearch.BatchId))
                    {
                        if (studentAdvanceSearch.BatchId != "0")
                        {
                            studentList = studentList.Where(x => x.Batch == Convert.ToInt32(studentAdvanceSearch.BatchId)).ToList();
                        }
                        else
                        {
                            studentList = (from sm in studentList
                                           join cbl in context.BatchCourseLinks on sm.Batch equals cbl.BatchId
                                           where cbl.CourseId == Convert.ToInt32(studentAdvanceSearch.CourseId)
                                           select sm).ToList();
                        }
                    }
                }

                int totalCount = studentList.Count();
                totalPages = (int)Math.Ceiling((decimal)totalCount / (decimal)Convert.ToInt32(listsize));
                studentList = studentList.OrderByDescending(x => x.CreatedDate).Skip((Convert.ToInt16(currentPageIndex) - 1) * Convert.ToInt32(listsize)).Take(Convert.ToInt32(listsize)).ToList();

                studentList.ForEach(itr =>
                {
                    var newData = new StudentDetailCO();
                    newData.StudentDetailsId = itr.StudentDetailsId;
                    newData.AdmissionNo = itr.AdmissionNo;
                    newData.AdmissionDate = itr.AdmissionDate;
                    newData.FirstName = itr.FirstName;
                    newData.LastName = itr.LastName;
                    newData.MiddleName = itr.MiddleName;
                    newData.StudentId = itr.StudentId;
                    newData.Batch = itr.Batch;
                    newData.DOB = itr.DOB;
                    newData.Gender = itr.Gender;
                    newData.Blood = itr.Blood;
                    newData.BirthPlace = itr.BirthPlace;
                    newData.Language = itr.Language;
                    newData.Nationality = itr.Nationality;
                    newData.Religion = itr.Religion;
                    newData.StudentCategory = itr.StudentCategory;
                    newData.PromitionalFlag = itr.PromitionalFlag;
                    newData.AddressLine1 = itr.AddressLine1;
                    newData.AddressLine2 = itr.AddressLine2;
                    newData.City = itr.City;
                    newData.State = itr.State;
                    newData.PinCode = itr.PinCode;
                    newData.PhoneNumber1 = itr.PhoneNumber1;
                    newData.PhoneNumber2 = itr.PhoneNumber2;
                    newData.Email = itr.Email;
                    newData.FileName = itr.FileName;
                    newData.Data = itr.Data;
                    newData.CreatedBy = itr.CreatedBy;
                    newData.CreatedDate = itr.CreatedDate;
                    newData.UpdatedBy = itr.UpdatedBy;
                    newData.UpdatedDate = itr.UpdatedDate;
                    newData.Country = itr.Country;
                    newData.DeleteFlag = itr.DeleteFlag;
                    newData.RollNumber = itr.RollNumber;
                    newData.Semester = itr.Semester;
                    newData.AcademicYear = itr.AcademicYear;
                    newData.AcademicYear = itr.AcademicYear;
                    newData.AcademicStatus = itr.AcademicStatus;
                    newData.Status = itr.Status;
                    newData.OnlineStatus = itr.OnlineStatus;
                    newData.OnlineApplicationStatus = itr.OnlineApplicationStatus;


                    var SchoolCO = new SchoolCO();
                    SchoolCO.GenderName = itr.Gender == 1 ? "Male" : itr.Gender == 2 ? "Female" : "Unknown";
                    var batch = context.batches.Find(itr.Batch);
                    if (batch != null)
                    {
                        var course = (from c in context.Courses
                                      join cbl in context.BatchCourseLinks on c.Id equals cbl.CourseId
                                      join b in context.batches on cbl.BatchId equals b.Id
                                      where b.Id == batch.Id
                                      select c).FirstOrDefault();

                        SchoolCO.CourseBatchName = course != null ? course.Name + " / " + batch.Name : "";
                    }
                    else
                        SchoolCO.CourseBatchName = "";

                    newData.SchoolCO = SchoolCO;
                    studentCOList.Add(newData);
                });
            }
            return Json(new { StudentList = studentCOList, TotalPages = totalPages }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SaveStudentFilter(List<StudentSearchFilterData> StudentSearchFilter)
        {
            if (sessionValidate())
            {
                using (var context = new GS247Entities8())
                {
                    StudentSearchFilter.ForEach(item =>
                    {

                        var studentSearchFilter = new StudentSearchFilter();
                        studentSearchFilter.Category = "Students";
                        studentSearchFilter.SearchFilterdName = item.SearchFilterdName;
                        studentSearchFilter.FileldName = item.FileldName;
                        studentSearchFilter.SearchValue = item.SearchValue;
                        studentSearchFilter.OptionValue = item.OptionValue;
                        studentSearchFilter.CreatedBy = Session["UserName"].ToString();
                        studentSearchFilter.CreatedDate = DateTime.Now;
                        context.StudentSearchFilters.Add(studentSearchFilter);
                        context.SaveChanges();
                    });
                }
                return new EmptyResult();
            }
            else
                return RedirectToAction("Index", "Login");
        }

        [HttpPost]
        public JsonResult LoadStudentFilter(string category)
        {
            List<StudentSearchFilter> studentSearchFilter = new List<StudentSearchFilter>();
            using (var context = new GS247Entities8())
            {
                studentSearchFilter = context.StudentSearchFilters.Where(x => x.Category == category).ToList().GroupBy(y => y.SearchFilterdName).Select(it => it.First()).ToList();
            }
            return Json(new { StudentSearchFilter = studentSearchFilter }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult LoadStudentSelectFilter(string SearchFilterdName, string category)
        {
            List<StudentSearchFilter> studentSearchFilter = new List<StudentSearchFilter>();
            using (var context = new GS247Entities8())
            {
                studentSearchFilter = context.StudentSearchFilters.Where(x => x.Category == category && x.SearchFilterdName == SearchFilterdName).ToList();
            }

            return Json(new { StudentSearchFilter = studentSearchFilter }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult DeleteStudentSelectFilter(int SearchFilterdId)
        {
            if (sessionValidate())
            {
                using (var context = new GS247Entities8())
                {
                    var subscriptions = context.StudentSearchFilters.Find(SearchFilterdId);
                    if (subscriptions != null)
                    {
                        context.StudentSearchFilters.Remove(subscriptions);
                        context.SaveChanges();
                    }
                }
                return new EmptyResult();
            }
            else
                return RedirectToAction("Index", "Login");
        }

        public class StudentAdvanceSearch
        {
            public string NameTxt { get; set; }

            public string RollnumberTxt { get; set; }

            public string AdmissionNumberTxt { get; set; }

            public string GenderTxt { get; set; }

            public string BloodGroupTxt { get; set; }

            public string CountryTxt { get; set; }

            public string DOBTxt { get; set; }

            public string AdmissionDateTxt { get; set; }

            public string StatusTxt { get; set; }

            public string AcademicYearTxt { get; set; }

            public string AdmissionDateRange { get; set; }

            public string DOBDateRange { get; set; }

            public string CourseId { get; set; }

            public string BatchId { get; set; }
        }

        public class StudentSearchFilterData
        {
            public string SearchFilterdName { get; set; }

            public string FileldName { get; set; }

            public string SearchValue { get; set; }

            public string OptionValue { get; set; }
        }

        [HttpPost]
        public JsonResult BatchListLoad(int Id)
        {
            List<batch> BatchList = new List<batch>();
            using (var context = new GS247Entities8())
            {
                BatchList = (from cbl in context.BatchCourseLinks
                             join b in context.batches on cbl.BatchId equals b.Id
                             where cbl.CourseId == Id
                             select b).ToList();
            }
            return Json(new { BatchList = BatchList }, JsonRequestBehavior.AllowGet);
        }
                
        public ActionResult CreateStudent()
        {
            if (sessionValidate())
            {
                var manageFieldsList = new List<StudentFieldSetting>();
                int id = Convert.ToInt32(Session["UserId"].ToString());
                using (var context = new GS247Entities8())
                {
                    var FieldsList = context.StudentFieldSettings.Where(x => x.UserID > 0).ToList();
                    ViewBag.managePersonalFieldsList = FieldsList.Where(x => x.TabSelection == 1 && x.TabSubSelection == 1).ToList();
                    ViewBag.manageContactFieldsList = FieldsList.Where(x => x.TabSelection == 1 && x.TabSubSelection == 2).ToList();
                    ViewBag.AdmissionNo = GetMaxId("AdmissionNo","StudentDetails");
                    ViewBag.StudentId = GetMaxId("StudentID", "StudentDetails");
                    ViewBag.AdmissionDate = DateTime.Now;
                    ViewBag.CourseList = context.Courses.ToList();
                    ViewBag.StudentCategoryList = context.StudentCategories.ToList();
                    ViewBag.CountryList = context.countries.ToList();
                }
                return View();
            }
            else
                return RedirectToAction("Index", "Login");
        }

        public string GetMaxId(string ColumnName,string TableName)
        {
            string maxId = string.Empty;
            using (var context = new GS247Entities8())
            {
                var MaxGenerateId = context.GenerateSequenceMasters.Where(x => x.ColumnName == ColumnName && x.TableName == TableName).FirstOrDefault();
                if (MaxGenerateId != null)
                {
                    if (MaxGenerateId.SeqRunningLength != null)
                    {
                        string output = MaxGenerateId.MaxGenerateId.ToString().PadLeft(MaxGenerateId.SeqRunningLength.GetValueOrDefault(), '0');
                        maxId = MaxGenerateId.SeqPrefix + output;
                    }
                    else
                    {
                        string output = MaxGenerateId.MaxGenerateId.ToString();
                        maxId = MaxGenerateId.SeqPrefix + output;
                    }
                }
                else
                {
                    maxId = "1";
                }  
            }

            return maxId;
        }

        public void SaveSeqTable(string ColumnName, string TableName)
        {
            using (var context = new GS247Entities8())
            {
                var SequenceData = context.GenerateSequenceMasters.Where(x => x.ColumnName == ColumnName && x.TableName == TableName).FirstOrDefault();
                SequenceData.UpdatedDate = DateTime.Now;
                SequenceData.UpdatedBy = Session["UserName"].ToString();
                SequenceData.MaxGenerateId = (Convert.ToInt32(SequenceData.MaxGenerateId) + 1).ToString();
                context.Entry(SequenceData).State = EntityState.Modified;
                context.SaveChanges();
            }
        }

        public void CreateUser(StudentDetail collection)
        {   
            using (var context = new GS247Entities8())
            {
                //var user = new User();
                //user.UserName = collection.StudentId;
                //user.Password = RandomGen(6); //Guid.NewGuid().ToString("d").Substring(1, 8);
                //user.Status = 1;
                var data = context.SettingsUserRoles.Where(x => x.Name == "Student").FirstOrDefault();
                int RoleId = data != null ? data.Id : 0;
                //user.Email = collection.Email;
                //user.FirstName = collection.FirstName;
                //user.LastName = collection.LastName;
                //user.MobileNumber = collection.PhoneNumber1;
                //context.Users.Add(user);
                //context.SaveChanges();

                commonCreateUser(collection.StudentId, collection.Email, RoleId, collection.PhoneNumber1, collection.FirstName, collection.LastName, 1);
            }
        }

        public void commonCreateUser(String UserName, String Email, int RoleId, String MobileNumber, String FirstName, String LastName, int Status)
        {
            String pwd = RandomGen(6);
            con.Open();
            SqlDataReader Exec = Connect.QueryExecute(con, "select count(id) from dbo.Users WHERE UserName='" + UserName +"'");           

            if ((Exec.Read()) && (Convert.ToInt32(Exec.GetValue(0)) > 0))
            {
                con.Close();
                con.Open();
                Connect.QueryExecute(con, "UPDATE dbo.Users Set SuperUser=" + RoleId + " , Email='" + Email + "' ,MobileNumber='" + MobileNumber + "' ,FirstName='" + FirstName + "' ,LastName='" + LastName + "',Status=" + Status + " WHERE UserName='" + UserName + "'");
            }
            else
            {
                con.Close();
                con.Open();
                Connect.QueryExecute(con, "INSERT INTO dbo.Users (UserName,Password,Email,MobileNumber,SuperUser,Status,FirstName,LastName) VALUES('" + UserName + "','" + pwd + "','" + Email + "','" + MobileNumber + "'," + RoleId + "," + Status + ",'" + FirstName + "','" + LastName + "')");
            }
            con.Close();
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

        public void CreateGuardianUser(GuardianDetail collection)
        {
            using (var context = new GS247Entities8())
            {
                //var user = new User();
                //user.UserName = collection.ParentId;
                //user.Password = RandomGen(6);  // Guid.NewGuid().ToString("d").Substring(1, 8);
                //user.Status = 1;
                var data = context.SettingsUserRoles.Where(x => x.Name == "Parent").FirstOrDefault();
                int RoleId = data != null ? data.Id : 0;
                //user.SuperUser = data != null ? data.Id : 0;
                //user.Email = collection.Email;
                //user.FirstName = collection.FirstName;
                //user.LastName = collection.LastName;
                //user.MobileNumber = collection.MobilePhone;
                //context.Users.Add(user);
                //context.SaveChanges();

                commonCreateUser(collection.ParentId, collection.Email, RoleId, collection.MobilePhone, collection.FirstName, collection.LastName, 1);
            }
        }

        [HttpPost]
        public ActionResult CreateStudent(StudentDetail collection)
        {
            if (sessionValidate())
            {
                HttpPostedFileBase file = Request.Files["DataString"];
                string pathName = string.Empty;
                using (var context = new GS247Entities8())
                {
                    collection.CreatedBy = Session["UserName"].ToString();
                    collection.CreatedDate = DateTime.Now;
                    collection.DeleteFlag = 0;
                    collection.Status = 1;
                    collection.OnlineStatus = 0;
                    collection.OnlineApplicationStatus = 0;
                    if (file != null)
                    {
                        string strExtension = Path.GetExtension(file.FileName);
                        pathName = "Profile_" + collection.AdmissionNo + "_" + DateTime.Now.ToString("MMddyyyyHHmmss") + strExtension;
                    }
                    collection.Data = pathName;
                    context.StudentDetails.Add(collection);
                    context.SaveChanges();

                    if (file != null)
                        saveImagesUpload(file, pathName);
                }

                SaveSeqTable("AdmissionNo", "StudentDetails");
                SaveSeqTable("StudentID", "StudentDetails");
                CreateUser(collection);

                return RedirectToAction("CreateGuardian", new RouteValueDictionary(
                        new { controller = "Student", action = "CreateGuardian", StudentDetailsId = collection.StudentDetailsId }));
            }
            else
                return RedirectToAction("Index", "Login");
        }

        private async Task<ActionResult> saveImagesUpload(HttpPostedFileBase file, string pathName)
        {
            string nameAndLocation = "~/UploadedFiles/" + pathName;
            file.SaveAs(Server.MapPath(nameAndLocation));
            return new EmptyResult();
        }

        public ActionResult CreateGuardian(int? StudentDetailsId)
        {
            if (sessionValidate())
            {
                var manageFieldsList = new List<StudentFieldSetting>();
                int id = Convert.ToInt32(Session["UserId"].ToString());
                using (var context = new GS247Entities8())
                {
                    var FieldsList = context.StudentFieldSettings.Where(x => x.UserID > 0).ToList();
                    ViewBag.managePersonalFieldsList = FieldsList.Where(x => x.TabSelection == 2 && x.TabSubSelection == 1).ToList();
                    ViewBag.manageContactFieldsList = FieldsList.Where(x => x.TabSelection == 2 && x.TabSubSelection == 2).ToList();
                    var GuardiansList = context.GuardianDetails.Where(x => x.StudentDetailsId == StudentDetailsId && (x.DeleteFlag == 0 || x.DeleteFlag == null)).ToList();
                    ViewBag.GuardiansList = GuardiansList;
                    ViewBag.StudentDetailsId = StudentDetailsId;
                    ViewBag.CountryList = context.countries.ToList();                    
                }
                return View();
            }
            else
                return RedirectToAction("Index", "Login");
        }

        [HttpPost]
        public ActionResult CreateGuardian(GuardianDetail collection, int? StudentDetailsId, string AddFlag)
        {
            if (sessionValidate())
            {
                using (var context = new GS247Entities8())
                {
                    collection.ParentId = GetMaxId("ParentID", "GuardianDetails");
                    collection.CreatedBy = Session["UserName"].ToString();
                    collection.CreatedDate = DateTime.Now;
                    collection.DeleteFlag = 0;
                    context.GuardianDetails.Add(collection);
                    context.SaveChanges();
                    SaveSeqTable("ParentID", "GuardianDetails");
                    if (collection.ParentUserFlag == 1)
                        CreateGuardianUser(collection);
                    ViewBag.StudentDetailsId = collection.StudentDetailsId;
                }

                if (AddFlag == "1")
                {
                    return RedirectToAction("CreateGuardian", new RouteValueDictionary(
                            new { controller = "Student", action = "CreateGuardian", StudentDetailsId = collection.StudentDetailsId }));
                }
                else
                {
                    return RedirectToAction("CreatePreviousDetail", new RouteValueDictionary(
                            new { controller = "Student", action = "CreatePreviousDetail", StudentDetailsId = collection.StudentDetailsId }));
                }
            }
            else
                return RedirectToAction("Index", "Login");
        }

        [HttpGet]
        public ActionResult UpdateGuardian(int? GuardianDetailsId)
        {
            if (sessionValidate())
            {
                var List = new List<StudentFieldSettingCO>();
                var List1 = new List<StudentFieldSettingCO>();
                int id = Convert.ToInt32(Session["UserId"].ToString());
                using (var context = new GS247Entities8())
                {
                    var FieldsList = context.StudentFieldSettings.Where(x => x.UserID > 0).ToList();
                    var _templist = context.GuardianDetails.Find(GuardianDetailsId);

                    var managePersonalFieldsList = FieldsList.Where(x => x.TabSelection == 2 && x.TabSubSelection == 1).ToList();

                    managePersonalFieldsList.ForEach(data =>
                    {
                        var newData = new StudentFieldSettingCO();
                        newData.StudentFieldSettingID = data.StudentFieldSettingID;
                        newData.Title = data.Title;
                        newData.Required = data.Required;
                        newData.FieldTypeFlag = data.FieldTypeFlag;
                        newData.CheckAllFlag = data.CheckAllFlag;
                        newData.AdminStudentRegistrationFormFlag = data.AdminStudentRegistrationFormFlag;
                        newData.OnlineAdmissionFormFlag = data.OnlineAdmissionFormFlag;
                        newData.ParentPortalFlag = data.ParentPortalFlag;
                        newData.StudentProfileFlag = data.StudentProfileFlag;
                        newData.StudentProfilePDFFlag = data.StudentProfilePDFFlag;
                        newData.StudentPortalFlag = data.StudentPortalFlag;
                        newData.TeacherPortalFlag = data.TeacherPortalFlag;
                        newData.TabSelection = data.TabSelection;
                        newData.TabSubSelection = data.TabSubSelection;
                        newData.UserID = data.UserID;
                        newData.Category = data.Category;
                        newData.StatusFlag = data.StatusFlag;
                        newData.OrderNumber = data.OrderNumber;
                        newData.ColumnMapping = data.ColumnMapping;

                        var SchoolCO = new SchoolCO();
                        if (data.ColumnMapping == "FirstName")
                            SchoolCO.FieldValue = _templist.FirstName;
                        else if (data.ColumnMapping == "LastName")
                            SchoolCO.FieldValue = _templist.LastName;
                        else if (data.ColumnMapping == "Education")
                            SchoolCO.FieldValue = _templist.Education;
                        else if (data.ColumnMapping == "Occupation")
                            SchoolCO.FieldValue = _templist.Occupation;
                        else if (data.ColumnMapping == "Income")
                            SchoolCO.FieldValue = _templist.Income;
                        else if (data.ColumnMapping == "Relation")
                            SchoolCO.FieldValue = _templist.Relation != null ? _templist.Relation.ToString() : "0";
                        else if (data.ColumnMapping == "DOB")
                            SchoolCO.FieldValue = _templist.DOB != null ? _templist.DOB.GetValueOrDefault().ToString("dd MMM yyyy") : "null";

                        newData.SchoolCO = SchoolCO;
                        List.Add(newData);
                    });

                    ViewBag.managePersonalFieldsList = List;

                    var manageContactFieldsList = FieldsList.Where(x => x.TabSelection == 2 && x.TabSubSelection == 2).ToList();

                    manageContactFieldsList.ForEach(data =>
                    {
                        var newData = new StudentFieldSettingCO();
                        newData.StudentFieldSettingID = data.StudentFieldSettingID;
                        newData.Title = data.Title;
                        newData.Required = data.Required;
                        newData.FieldTypeFlag = data.FieldTypeFlag;
                        newData.CheckAllFlag = data.CheckAllFlag;
                        newData.AdminStudentRegistrationFormFlag = data.AdminStudentRegistrationFormFlag;
                        newData.OnlineAdmissionFormFlag = data.OnlineAdmissionFormFlag;
                        newData.ParentPortalFlag = data.ParentPortalFlag;
                        newData.StudentProfileFlag = data.StudentProfileFlag;
                        newData.StudentProfilePDFFlag = data.StudentProfilePDFFlag;
                        newData.StudentPortalFlag = data.StudentPortalFlag;
                        newData.TeacherPortalFlag = data.TeacherPortalFlag;
                        newData.TabSelection = data.TabSelection;
                        newData.TabSubSelection = data.TabSubSelection;
                        newData.UserID = data.UserID;
                        newData.Category = data.Category;
                        newData.StatusFlag = data.StatusFlag;
                        newData.OrderNumber = data.OrderNumber;
                        newData.ColumnMapping = data.ColumnMapping;


                        var SchoolCO = new SchoolCO();
                        if (data.ColumnMapping == "AddressLine1")
                            SchoolCO.FieldValue = _templist.AddressLine1;
                        else if (data.ColumnMapping == "AddressLine2")
                            SchoolCO.FieldValue = _templist.AddressLine2;
                        else if (data.ColumnMapping == "City")
                            SchoolCO.FieldValue = _templist.City;
                        else if (data.ColumnMapping == "State")
                            SchoolCO.FieldValue = _templist.State;
                        else if (data.ColumnMapping == "PinCode")
                            SchoolCO.FieldValue = _templist.PinCode;
                        else if (data.ColumnMapping == "MobilePhone")
                            SchoolCO.FieldValue = _templist.MobilePhone;
                        else if (data.ColumnMapping == "OfficePhone1")
                            SchoolCO.FieldValue = _templist.OfficePhone1;
                        else if (data.ColumnMapping == "OfficePhone2")
                            SchoolCO.FieldValue = _templist.OfficePhone2;
                        else if (data.ColumnMapping == "Email")
                            SchoolCO.FieldValue = _templist.Email;
                        else if (data.ColumnMapping == "Country")
                            SchoolCO.FieldValue = _templist.Country != null ? _templist.Country.ToString() : "0";
                        else if (data.ColumnMapping == "ParentUserFlag")
                            SchoolCO.FieldValue = _templist.Country != null ? _templist.Country.ToString() : "0";

                        newData.SchoolCO = SchoolCO;
                        List1.Add(newData);
                    });

                    ViewBag.manageContactFieldsList = List1;
                    ViewBag.GuardianDetailsId = _templist.GuardianDetailsId;
                    ViewBag.CountryList = context.countries.ToList();
                    return View("UpdateGuardian", _templist);
                }
            }
            else
                return RedirectToAction("Index", "Login");
        }

        [HttpPost]
        public ActionResult UpdateGuardian(GuardianDetail collection, int? GuardianDetailsId)
        {
            if (sessionValidate())
            {
                using (var context = new GS247Entities8())
                {
                    var _templist = context.GuardianDetails.Find(GuardianDetailsId);
                    if (_templist != null)
                    {
                        _templist.FirstName = collection.FirstName;
                        _templist.LastName = collection.LastName;
                        _templist.Relation = collection.Relation;
                        _templist.DOB = collection.DOB;
                        _templist.Education = collection.Education;
                        _templist.Occupation = collection.Occupation;
                        _templist.Income = collection.Income;
                        _templist.AddressLine1 = collection.AddressLine1;
                        _templist.AddressLine2 = collection.AddressLine2;
                        _templist.City = collection.City;
                        _templist.State = collection.State;
                        _templist.PinCode = collection.PinCode;
                        _templist.MobilePhone = collection.MobilePhone;
                        _templist.OfficePhone1 = collection.OfficePhone1;
                        _templist.OfficePhone2 = collection.OfficePhone2;
                        _templist.Email = collection.Email;
                        _templist.Country = collection.Country;
                        _templist.ParentUserFlag = collection.ParentUserFlag;
                        _templist.DeleteFlag = 0;
                        _templist.UpdatedDate = DateTime.Now;
                        _templist.UpdatedBy = Session["UserName"].ToString();
                        context.Entry(_templist).State = EntityState.Modified;
                        context.SaveChanges();
                        collection.StudentDetailsId = _templist.StudentDetailsId;
                    }
                }
                return RedirectToAction("CreateGuardian", new RouteValueDictionary(
                           new { controller = "Student", action = "CreateGuardian", StudentDetailsId = collection.StudentDetailsId }));
            }
            else
                return RedirectToAction("Index", "Login");
        }

        [HttpPost]
        public ActionResult DeleteGuardian(int GuardianDetailsId)
        {
            if (sessionValidate())
            {
                using (var context = new GS247Entities8())
                {
                    var _templist = context.GuardianDetails.Find(GuardianDetailsId);
                    if (_templist != null)
                    {
                        _templist.DeleteFlag = 1;
                        _templist.UpdatedDate = DateTime.Now;
                        _templist.UpdatedBy = Session["UserName"].ToString();
                        context.Entry(_templist).State = EntityState.Modified;
                        context.SaveChanges();
                    }
                }
                return new EmptyResult();
            }
            else
                return RedirectToAction("Index", "Login");
        }

        public ActionResult CreatePreviousDetail(int? StudentDetailsId)
        {
            if (sessionValidate())
            {
                int id = Convert.ToInt32(Session["UserId"].ToString());
                using (var context = new GS247Entities8())
                {
                    var FieldsList = context.StudentFieldSettings.Where(x => x.UserID > 0).ToList();
                    ViewBag.managInstutionFieldsList = FieldsList.Where(x => x.TabSelection == 3 && x.TabSubSelection == 1).ToList();
                    var StudentInstutionDetailsList = context.StudentInstutionDetails.Where(x => x.StudentDetailsId == StudentDetailsId).ToList();
                    ViewBag.StudentInstutionDetailsList = StudentInstutionDetailsList;
                    ViewBag.StudentDetailsId = StudentDetailsId;
                }
                return View();
            }
            else
                return RedirectToAction("Index", "Login");
        }

        [HttpPost]
        public ActionResult CreatePreviousDetail(StudentInstutionDetail collection, int? StudentDetailsId, string AddFlag)
        {
            if (sessionValidate())
            {
                using (var context = new GS247Entities8())
                {
                    var _templist = context.StudentInstutionDetails.Find(collection.InstutionDetailsId);
                    if (_templist != null)
                    {
                        _templist.Instution = collection.Instution;
                        _templist.Year = collection.Year;
                        _templist.Course = collection.Course;
                        _templist.TotalMark = collection.TotalMark;
                        _templist.UpdatedDate = DateTime.Now;
                        _templist.UpdatedBy = Session["UserName"].ToString();
                        context.Entry(_templist).State = EntityState.Modified;
                        context.SaveChanges();
                        collection.StudentDetailsId = _templist.StudentDetailsId;

                        return RedirectToAction("CreatePreviousDetail", new RouteValueDictionary(
                        new { controller = "Student", action = "CreatePreviousDetail", StudentDetailsId = collection.StudentDetailsId }));

                    }
                    else
                    {
                        collection.CreatedBy = Session["UserName"].ToString();
                        collection.CreatedDate = DateTime.Now;
                        context.StudentInstutionDetails.Add(collection);
                        context.SaveChanges();
                        ViewBag.StudentDetailsId = collection.StudentDetailsId;

                        if (AddFlag == "1")
                        {
                            return RedirectToAction("CreatePreviousDetail", new RouteValueDictionary(
                                     new { controller = "Student", action = "CreatePreviousDetail", StudentDetailsId = collection.StudentDetailsId }));
                        }
                        else
                        {
                            return RedirectToAction("CreateStudentDocument", new RouteValueDictionary(
                          new { controller = "Student", action = "CreateStudentDocument", StudentDetailsId = collection.StudentDetailsId }));
                        }
                    }
                }
            }
            else
                return RedirectToAction("Index", "Login");
        }

        [HttpPost]
        public ActionResult UpdatePreviousDetail(StudentInstutionDetail collection, int? StudentDetailsId)
        {
            if (sessionValidate())
            {
                using (var context = new GS247Entities8())
                {
                    var _templist = context.StudentInstutionDetails.Find(collection.InstutionDetailsId);
                    if (_templist != null)
                    {
                        _templist.Instution = collection.Instution;
                        _templist.Year = collection.Year;
                        _templist.Course = collection.Course;
                        _templist.TotalMark = collection.TotalMark;
                        _templist.UpdatedDate = DateTime.Now;
                        _templist.UpdatedBy = Session["UserName"].ToString();
                        context.Entry(_templist).State = EntityState.Modified;
                        context.SaveChanges();
                        collection.StudentDetailsId = _templist.StudentDetailsId;
                    }
                }
                return RedirectToAction("CreatePreviousDetail", new RouteValueDictionary(
                            new { controller = "Student", action = "CreatePreviousDetail", StudentDetailsId = collection.StudentDetailsId }));
            }
            else
                return RedirectToAction("Index", "Login");
        }

        [HttpPost]
        public ActionResult DeleteStudentDocument(int StudentDocumentsId)
        {
            if (sessionValidate())
            {
                using (var context = new GS247Entities8())
                {
                    var subscriptions = context.StudentDocuments.Find(StudentDocumentsId);
                    if (subscriptions != null)
                    {
                        context.StudentDocuments.Remove(subscriptions);
                        context.SaveChanges();
                    }
                }
                return new EmptyResult();
            }
            else
                return RedirectToAction("Index", "Login");
        }

        [HttpPost]
        public ActionResult DeleteStudentInstitution(int InstutionDetailsId)
        {
            if (sessionValidate())
            {
                using (var context = new GS247Entities8())
                {
                    var subscriptions = context.StudentInstutionDetails.Find(InstutionDetailsId);
                    if (subscriptions != null)
                    {
                        context.StudentInstutionDetails.Remove(subscriptions);
                        context.SaveChanges();
                    }
                }
                return new EmptyResult();
            }
            else
                return RedirectToAction("Index", "Login");
        }

        public ActionResult CreateStudentDocument(int? StudentDetailsId)
        {
            if (sessionValidate())
            {
                var List = new List<StudentDocumentCO>();
                using (var context = new GS247Entities8())
                {
                    var FieldsList = context.StudentDocuments.Where(x => x.StudentDetailsId == StudentDetailsId).ToList();

                    var DocumentList = (from sdt in context.SettingsDocumentTypes.ToList()
                                        where !FieldsList.Any(x => x.DocumentTypeId == sdt.Id)
                                        select sdt).Distinct().ToList();

                    ViewBag.DocumentsList = DocumentList;

                    FieldsList.ForEach(data =>
                    {
                        var StudentDocument = new StudentDocumentCO();
                        StudentDocument.StudentDocumentsId = data.StudentDocumentsId;
                        StudentDocument.StudentDetailsId = data.StudentDetailsId;
                        StudentDocument.DocumentTypeId = data.DocumentTypeId;
                        StudentDocument.FileName = data.FileName;
                        StudentDocument.Data = data.Data;
                        StudentDocument.CreatedBy = data.CreatedBy;
                        StudentDocument.CreatedDate = data.CreatedDate;
                        StudentDocument.UpdatedBy = data.UpdatedBy;
                        StudentDocument.UpdatedDate = data.UpdatedDate;
                        StudentDocument.StatusFlag = data.StatusFlag;

                        var SchoolCO = new SchoolCO();
                        SchoolCO.DocumentName = context.SettingsDocumentTypes.Where(x => x.Id == data.DocumentTypeId).FirstOrDefault().DocumentName;
                        StudentDocument.SchoolCO = SchoolCO;
                        List.Add(StudentDocument);
                    });
                    ViewBag.StudentDocumentsList = List;
                    ViewBag.StudentDetailsId = StudentDetailsId;
                }
                return View();
            }
            else
                return RedirectToAction("Index", "Login");
        }

        [HttpPost]
        public ActionResult CreateStudentDocument(StudentDocument collection, int? StudentDetailsId, string AddFlag)
        {
            if (sessionValidate())
            {
                HttpPostedFileBase file = Request.Files["DataString"];
                string pathName = string.Empty;

                using (var context = new GS247Entities8())
                {
                    collection.CreatedBy = Session["UserName"].ToString();
                    collection.CreatedDate = DateTime.Now;
                    collection.StatusFlag = 0;

                    if (file != null)
                    {
                        string strExtension = Path.GetExtension(file.FileName);
                        pathName = "Student_Document_" + StudentDetailsId + "_" + DateTime.Now.ToString("MMddyyyyHHmmss") + strExtension;
                    }
                    collection.Data = pathName;
                    context.StudentDocuments.Add(collection);
                    context.SaveChanges();
                    ViewBag.StudentDetailsId = collection.StudentDetailsId;
                    if (file != null)
                        saveImagesUpload(file, pathName);
                }

                if (AddFlag == "1")
                {
                    return RedirectToAction("ViewProfile", new RouteValueDictionary(
                             new { controller = "Student", action = "ViewProfile", StudentDetailsId = collection.StudentDetailsId }));
                }
                else
                {
                    return RedirectToAction("CreateStudentDocument", new RouteValueDictionary(
                  new { controller = "Student", action = "CreateStudentDocument", StudentDetailsId = collection.StudentDetailsId }));
                }
            }
            else
                return RedirectToAction("Index", "Login");
        }

        [HttpPost]
        public ActionResult StudentDocumentStatusUpdate(int StudentDocumentsId,int Status)
        {
            if (sessionValidate())
            {
                using (var context = new GS247Entities8())
                {
                    var StudentDocuments = context.StudentDocuments.Find(StudentDocumentsId);
                    if (StudentDocuments != null)
                    {
                        StudentDocuments.StatusFlag = Status;
                        StudentDocuments.UpdatedDate = DateTime.Now;
                        StudentDocuments.UpdatedBy = Session["UserName"].ToString();
                        context.Entry(StudentDocuments).State = EntityState.Modified;
                        context.SaveChanges();
                    }
                }
                return new EmptyResult();
            }
            else
                return RedirectToAction("Index", "Login");
        }

        public FileResult Download(string ImageName)
        {
            var FileVirtualPath = "~/UploadedFiles/" + ImageName;
            return File(FileVirtualPath, "application/force-download", Path.GetFileName(FileVirtualPath));
        }

        [HttpGet]
        public ActionResult UpdateStudent(int? StudentDetailsId)
        {
            if (sessionValidate())
            {
                var managePersonalFieldsCOList = new List<StudentFieldSettingCO>();
                var manageContactFieldsCOList = new List<StudentFieldSettingCO>();
                int id = Convert.ToInt32(Session["UserId"].ToString());
                using (var context = new GS247Entities8())
                {
                    var FieldsList = context.StudentFieldSettings.Where(x => x.UserID > 0).ToList();
                    var _templist = context.StudentDetails.Find(StudentDetailsId);

                    var managePersonalFieldsList = FieldsList.Where(x => x.TabSelection == 1 && x.TabSubSelection == 1).ToList();

                    managePersonalFieldsList.ForEach(data =>
                    {
                        var newData = new StudentFieldSettingCO();
                        newData.StudentFieldSettingID = data.StudentFieldSettingID;
                        newData.Title = data.Title;
                        newData.Required = data.Required;
                        newData.FieldTypeFlag = data.FieldTypeFlag;
                        newData.CheckAllFlag = data.CheckAllFlag;
                        newData.AdminStudentRegistrationFormFlag = data.AdminStudentRegistrationFormFlag;
                        newData.OnlineAdmissionFormFlag = data.OnlineAdmissionFormFlag;
                        newData.ParentPortalFlag = data.ParentPortalFlag;
                        newData.StudentProfileFlag = data.StudentProfileFlag;
                        newData.StudentProfilePDFFlag = data.StudentProfilePDFFlag;
                        newData.StudentPortalFlag = data.StudentPortalFlag;
                        newData.TeacherPortalFlag = data.TeacherPortalFlag;
                        newData.TabSelection = data.TabSelection;
                        newData.TabSubSelection = data.TabSubSelection;
                        newData.UserID = data.UserID;
                        newData.Category = data.Category;
                        newData.StatusFlag = data.StatusFlag;
                        newData.OrderNumber = data.OrderNumber;
                        newData.ColumnMapping = data.ColumnMapping;

                        var SchoolCO = new SchoolCO();
                        if (data.ColumnMapping == "AdmissionNo")
                            SchoolCO.FieldValue = _templist.AdmissionNo;
                        else if (data.ColumnMapping == "AdmissionDate")
                            SchoolCO.FieldValue = _templist.AdmissionDate.ToString();
                        else if (data.ColumnMapping == "FirstName")
                            SchoolCO.FieldValue = _templist.FirstName;
                        else if (data.ColumnMapping == "LastName")
                            SchoolCO.FieldValue = _templist.LastName;
                        else if (data.ColumnMapping == "MiddleName")
                            SchoolCO.FieldValue = _templist.MiddleName;
                        else if (data.ColumnMapping == "StudentId")
                            SchoolCO.FieldValue = _templist.StudentId;
                        else if (data.ColumnMapping == "Course")
                            SchoolCO.FieldValue = _templist.Course != null ? _templist.Course.ToString() : "0";
                        else if (data.ColumnMapping == "Batch")
                            SchoolCO.FieldValue = _templist.Batch != null ? _templist.Batch.ToString() : "0";
                        else if (data.ColumnMapping == "DOB")
                        {
                            var dob = _templist.DOB.GetValueOrDefault().ToString("dd MMM yyyy");
                            SchoolCO.FieldValue = dob;
                        }
                        else if (data.ColumnMapping == "Gender")
                            SchoolCO.FieldValue = _templist.Gender.ToString();
                        else if (data.ColumnMapping == "Blood")
                            SchoolCO.FieldValue = _templist.Blood != null ? _templist.Blood.ToString() : "0";
                        else if (data.ColumnMapping == "BirthPlace")
                            SchoolCO.FieldValue = _templist.BirthPlace;
                        else if (data.ColumnMapping == "Language")
                            SchoolCO.FieldValue = _templist.Language;
                        else if (data.ColumnMapping == "Nationality")
                            SchoolCO.FieldValue = _templist.Nationality != null ? _templist.Nationality.ToString() : "0";
                        else if (data.ColumnMapping == "Religion")
                            SchoolCO.FieldValue = _templist.Religion;
                        else if (data.ColumnMapping == "StudentCategory")
                            SchoolCO.FieldValue = _templist.StudentCategory != null ? _templist.StudentCategory.ToString() : "0";
                        else if (data.ColumnMapping == "PromitionalFlag")
                            SchoolCO.FieldValue = _templist.PromitionalFlag.ToString();

                        newData.SchoolCO = SchoolCO;
                        managePersonalFieldsCOList.Add(newData);
                    });

                    ViewBag.managePersonalFieldsList = managePersonalFieldsCOList;

                    var manageContactFieldsList = FieldsList.Where(x => x.TabSelection == 1 && x.TabSubSelection == 2).ToList();

                    manageContactFieldsList.ForEach(data =>
                    {
                        var newData = new StudentFieldSettingCO();
                        newData.StudentFieldSettingID = data.StudentFieldSettingID;
                        newData.Title = data.Title;
                        newData.Required = data.Required;
                        newData.FieldTypeFlag = data.FieldTypeFlag;
                        newData.CheckAllFlag = data.CheckAllFlag;
                        newData.AdminStudentRegistrationFormFlag = data.AdminStudentRegistrationFormFlag;
                        newData.OnlineAdmissionFormFlag = data.OnlineAdmissionFormFlag;
                        newData.ParentPortalFlag = data.ParentPortalFlag;
                        newData.StudentProfileFlag = data.StudentProfileFlag;
                        newData.StudentProfilePDFFlag = data.StudentProfilePDFFlag;
                        newData.StudentPortalFlag = data.StudentPortalFlag;
                        newData.TeacherPortalFlag = data.TeacherPortalFlag;
                        newData.TabSelection = data.TabSelection;
                        newData.TabSubSelection = data.TabSubSelection;
                        newData.UserID = data.UserID;
                        newData.Category = data.Category;
                        newData.StatusFlag = data.StatusFlag;
                        newData.OrderNumber = data.OrderNumber;
                        newData.ColumnMapping = data.ColumnMapping;

                        var SchoolCO = new SchoolCO();
                        if (data.ColumnMapping == "AddressLine1")
                            SchoolCO.FieldValue = _templist.AddressLine1;
                        else if (data.ColumnMapping == "AddressLine2")
                            SchoolCO.FieldValue = _templist.AddressLine2;
                        else if (data.ColumnMapping == "City")
                            SchoolCO.FieldValue = _templist.City;
                        else if (data.ColumnMapping == "State")
                            SchoolCO.FieldValue = _templist.State;
                        else if (data.ColumnMapping == "PinCode")
                            SchoolCO.FieldValue = _templist.PinCode;
                        else if (data.ColumnMapping == "PhoneNumber1")
                            SchoolCO.FieldValue = _templist.PhoneNumber1;
                        else if (data.ColumnMapping == "PhoneNumber2")
                            SchoolCO.FieldValue = _templist.PhoneNumber2;
                        else if (data.ColumnMapping == "Email")
                            SchoolCO.FieldValue = _templist.Email;

                        newData.SchoolCO = SchoolCO;
                        manageContactFieldsCOList.Add(newData);
                    });

                    ViewBag.manageContactFieldsList = manageContactFieldsCOList;
                    ViewBag.FileName = _templist.FileName;
                    ViewBag.AdmissionNo = _templist.AdmissionNo;
                    ViewBag.AdmissionDate = _templist.AdmissionDate;
                    ViewBag.StudentDetailsId = _templist.StudentDetailsId;
                    ViewBag.CourseList = context.Courses.ToList();
                    ViewBag.BatchList = (from cbl in context.BatchCourseLinks
                                         join b in context.batches on cbl.BatchId equals b.Id
                                         where cbl.CourseId == _templist.Course
                                         select b).ToList();    
                    ViewBag.StudentCategoryList = context.StudentCategories.ToList();
                    ViewBag.CountryList = context.countries.ToList();
                    return View("UpdateStudent", _templist);
                }
            }
            else
                return RedirectToAction("Index", "Login");
        }

        [HttpPost]
        public ActionResult UpdateStudent(StudentDetail collection)
        {
            if (sessionValidate())
            {
                using (var context = new GS247Entities8())
                {
                    var _templist = context.StudentDetails.Find(collection.StudentDetailsId);
                    if (_templist != null)
                    {
                        _templist.FirstName = collection.FirstName;
                        _templist.MiddleName = collection.MiddleName;
                        _templist.LastName = collection.LastName;
                        _templist.StudentId = collection.StudentId;
                        _templist.AdmissionDate = collection.AdmissionDate;
                        _templist.AdmissionNo = collection.AdmissionNo;
                        _templist.Batch = collection.Batch;
                        _templist.Course = collection.Course;
                        _templist.DOB = collection.DOB;
                        _templist.Gender = collection.Gender;
                        _templist.Blood = collection.Blood;
                        _templist.BirthPlace = collection.BirthPlace;
                        _templist.Language = collection.Language;
                        _templist.Nationality = collection.Nationality;
                        _templist.Religion = collection.Religion;
                        _templist.StudentCategory = collection.StudentCategory;
                        _templist.PromitionalFlag = collection.PromitionalFlag;
                        _templist.AddressLine1 = collection.AddressLine1;
                        _templist.AddressLine2 = collection.AddressLine2;
                        _templist.City = collection.City;
                        _templist.State = collection.State;
                        _templist.PinCode = collection.PinCode;
                        _templist.PhoneNumber1 = collection.PhoneNumber1;
                        _templist.PhoneNumber2 = collection.PhoneNumber2;
                        _templist.Email = collection.Email;
                        _templist.DeleteFlag = 0;
                        if (!string.IsNullOrEmpty(collection.Data))
                        {
                            _templist.Data = collection.Data;
                        }
                        else if (collection.FileName == _templist.FileName)
                            _templist.Data = _templist.Data;
                        else
                            _templist.Data = collection.Data;
                        _templist.FileName = collection.FileName;
                        _templist.UpdatedDate = DateTime.Now;
                        _templist.UpdatedBy = Session["UserName"].ToString();
                        context.Entry(_templist).State = EntityState.Modified;
                        context.SaveChanges();
                    }
                }
                return RedirectToAction("CreateGuardian", new RouteValueDictionary(
                       new { controller = "Student", action = "CreateGuardian", StudentDetailsId = collection.StudentDetailsId }));
            }
            else
                return RedirectToAction("Index", "Login");
        }

        [HttpPost]
        public ActionResult DeleteStudent(int StudentDetailsId)
        {
            if (sessionValidate())
            {
                var message = "";
                using (var context = new GS247Entities8())
                {
                    var validateFlag = context.GuardianDetails.Where(x => x.StudentDetailsId == StudentDetailsId && x.DeleteFlag == 0).Any();

                    if (!validateFlag)
                    {
                        validateFlag = context.StudentFeesInvoices.Where(x => x.StudentDetailsId == StudentDetailsId).Any();
                        if (!validateFlag)
                        {
                            validateFlag = context.StudentFeesInvoices.Where(x => x.StudentDetailsId == StudentDetailsId).Any();
                            if (!validateFlag)
                            {
                                validateFlag = context.StudentExamDetails.Where(x => x.StudentDetailsId == StudentDetailsId).Any();
                                if (!validateFlag)
                                {
                                    validateFlag = context.StudentRoomDetails.Where(x => x.StudentDetailsId == StudentDetailsId).Any();
                                    if (!validateFlag)
                                    {
                                        validateFlag = context.StudentRoomDetails.Where(x => x.StudentDetailsId == StudentDetailsId).Any();
                                    }
                                }
                            }
                        }
                    }

                    if (!validateFlag)
                    {
                        var StudentDetails = context.StudentDetails.Find(StudentDetailsId);

                        if (StudentDetails != null)
                        {
                            StudentDetails.DeleteFlag = 1;
                            StudentDetails.UpdatedDate = DateTime.Now;
                            StudentDetails.UpdatedBy = Session["UserName"].ToString();
                            context.Entry(StudentDetails).State = EntityState.Modified;
                            context.SaveChanges();
                        }

                        var waitingData = context.StudentWaitingLists.Where(x => x.StudentDetailsId == StudentDetailsId && x.StarusFlag == 0).FirstOrDefault();
                        if (waitingData != null)
                        {
                            context.StudentWaitingLists.Remove(waitingData);
                            context.SaveChanges();
                        }
                    }
                    else
                        message = "Stident refreenced in other module can't be deleted for some students";
                }
                return Json(new { Message = message });
            }
            else
                return RedirectToAction("Index", "Login");
        }

        public ActionResult ManageStudentArchive()
        {
            if (sessionValidate())
            {
                using (var context = new GS247Entities8())
                {
                    var FieldsList = context.StudentDetails.Where(x => (x.DeleteFlag == 1)).ToList();
                    var totalPages = (int)Math.Ceiling((decimal)FieldsList.Count / (decimal)10);
                    ViewBag.PageCount = totalPages;
                    ViewBag.StudentList = FieldsList.Take(10).ToList();
                    ViewBag.CurrentPageIndex = 1;
                }
                return View();
            }
            else
                return RedirectToAction("Index", "Login");
        }

        public ActionResult SearchStudentArchiveList(SearchSFields SearchSFields)
        {
            if (sessionValidate())
            {
                using (var context = new GS247Entities8())
                {
                    var FieldsList = context.StudentDetails.Where(x => (x.DeleteFlag == 1)).ToList();
                    if (!string.IsNullOrEmpty(SearchSFields.alphaValue) && SearchSFields.alphaValue != "All" )
                    {
                        var query = FieldsList.AsEnumerable();
                        FieldsList = query.Where(m => m.FirstName[0] == Convert.ToChar(SearchSFields.alphaValue)).ToList();
                    }

                    int totalCount = FieldsList.Count;
                    var totalPages = (int)Math.Ceiling((decimal)totalCount / (decimal)10);
                    ViewBag.PageCount = totalPages;
                    ViewBag.CurrentPageIndex = Convert.ToInt16(SearchSFields.CurrentPage);
                    var StudentList = FieldsList.OrderByDescending(x => x.CreatedDate).Skip((Convert.ToInt16(SearchSFields.CurrentPage) - 1) * 10).Take(10).ToList();
                    ViewBag.StudentList = StudentList;
                }
                return View("ManageStudentArchive");
            }
            else
                return RedirectToAction("Index", "Login");
        }

        [HttpPost]
        public ActionResult UpdateStudentEmail(StudentDetail collection)
        {
            if (sessionValidate())
            {
                using (var context = new GS247Entities8())
                {
                    var _templist = context.StudentDetails.Find(collection.StudentDetailsId);
                    if (_templist != null)
                    {
                        _templist.PhoneNumber1 = collection.PhoneNumber1;
                        _templist.Email = collection.Email;
                        _templist.UpdatedDate = DateTime.Now;
                        _templist.UpdatedBy = Session["UserName"].ToString();
                        context.Entry(_templist).State = EntityState.Modified;
                        context.SaveChanges();
                    }
                }
                return RedirectToAction("ManageStudentArchive");
            }
            else
                return RedirectToAction("Index", "Login");
        }

        [HttpPost]
        public ActionResult DeleteStudentArchive(int StudentDetailsId)
        {
            if (sessionValidate())
            {
                using (var context = new GS247Entities8())
                {
                    var subscriptions = context.StudentDetails.Find(StudentDetailsId);
                    if (subscriptions != null)
                    {
                        context.StudentDetails.Remove(subscriptions);
                        context.SaveChanges();
                    }
                }
                return new EmptyResult();
            }
            else
                return RedirectToAction("Index", "Login");
        }

        [HttpPost]
        public ActionResult RestoreStudentArchive(int StudentDetailsId)
        {
            if (sessionValidate())
            {
                using (var context = new GS247Entities8())
                {
                    var _templist = context.StudentDetails.Find(StudentDetailsId);
                    if (_templist != null)
                    {
                        _templist.DeleteFlag = 0;
                        _templist.UpdatedDate = DateTime.Now;
                        _templist.UpdatedBy = Session["UserName"].ToString();
                        context.Entry(_templist).State = EntityState.Modified;
                        context.SaveChanges();

                        var GuardianDetails = context.GuardianDetails.Where(x => x.StudentDetailsId == StudentDetailsId).ToList();

                        GuardianDetails.ForEach(data =>
                        {
                            data.DeleteFlag = 0;
                            context.Entry(data).State = EntityState.Modified;
                            context.SaveChanges();

                        });
                    }
                }
                return new EmptyResult();
            }
            else
                return RedirectToAction("Index", "Login");
        }

        [HttpPost]
        public ActionResult DeleteAndRestoreAllStudent(DeleteRestoreStudent deleteRestoreStudent)
        {
            if (sessionValidate())
            {
                using (var context = new GS247Entities8())
                {
                    if (deleteRestoreStudent.Flag == "0")
                    {
                        deleteRestoreStudent.StudentDetailsId.ForEach(data =>
                        {
                            var _templist = context.StudentDetails.Find(data);
                            if (_templist != null)
                            {
                                context.StudentDetails.Remove(_templist);
                                context.SaveChanges();

                                var GuardianDetails = context.GuardianDetails.Where(x => x.StudentDetailsId == _templist.StudentDetailsId).ToList();

                                GuardianDetails.ForEach(item =>
                                {
                                    context.GuardianDetails.Remove(item);
                                    context.SaveChanges();

                                });

                                var StudentInstutionDetails = context.StudentInstutionDetails.Where(x => x.StudentDetailsId == _templist.StudentDetailsId).ToList();

                                StudentInstutionDetails.ForEach(item =>
                                {
                                    context.StudentInstutionDetails.Remove(item);
                                    context.SaveChanges();
                                });

                                var StudentDocuments = context.StudentDocuments.Where(x => x.StudentDetailsId == _templist.StudentDetailsId).ToList();

                                StudentDocuments.ForEach(item =>
                                {
                                    context.StudentDocuments.Remove(item);
                                    context.SaveChanges();
                                });
                            }
                        });
                    }
                    else
                    {
                        deleteRestoreStudent.StudentDetailsId.ForEach(data =>
                        {

                            var _templist = context.StudentDetails.Find(data);
                            if (_templist != null)
                            {
                                _templist.DeleteFlag = 0;
                                _templist.UpdatedDate = DateTime.Now;
                                _templist.UpdatedBy = Session["UserName"].ToString();
                                context.Entry(_templist).State = EntityState.Modified;
                                context.SaveChanges();

                                var GuardianDetails = context.GuardianDetails.Where(x => x.StudentDetailsId == _templist.StudentDetailsId).ToList();

                                GuardianDetails.ForEach(item =>
                                {
                                    item.DeleteFlag = 0;
                                    context.Entry(item).State = EntityState.Modified;
                                    context.SaveChanges();

                                });
                            }
                        });
                    }
                }
                return new EmptyResult();
            }
            else
                return RedirectToAction("Index", "Login");
        }

        public class DeleteRestoreStudent
        {
            public List<int> StudentDetailsId { get; set; }
            public string Flag { get; set; }
        }

        public ActionResult ManageGuardianArchive()
        {
            if (sessionValidate())
            {
                using (var context = new GS247Entities8())
                {
                    var FieldsList = context.GuardianDetails.Where(x => (x.DeleteFlag == 1)).ToList();
                    var totalPages = (int)Math.Ceiling((decimal)FieldsList.Count / (decimal)10);
                    ViewBag.PageCount = totalPages;
                    ViewBag.GuardianList = FieldsList.Take(10).ToList();
                    ViewBag.CurrentPageIndex = 1;
                }
                return View();
            }
            else
                return RedirectToAction("Index", "Login");
        }

        public ActionResult SearchGuardianArchiveList(SearchSFields SearchSFields)
        {
            if (sessionValidate())
            {
                using (var context = new GS247Entities8())
                {
                    var FieldsList = context.GuardianDetails.Where(x => (x.DeleteFlag == 1)).ToList();
                    if (!string.IsNullOrEmpty(SearchSFields.alphaValue) && SearchSFields.alphaValue != "All")
                    {
                        var query = FieldsList.AsEnumerable();
                        FieldsList = query.Where(m => m.FirstName[0] == Convert.ToChar(SearchSFields.alphaValue)).ToList();
                    }

                    int totalCount = FieldsList.Count;
                    var totalPages = (int)Math.Ceiling((decimal)totalCount / (decimal)10);
                    ViewBag.PageCount = totalPages;
                    ViewBag.CurrentPageIndex = Convert.ToInt16(SearchSFields.CurrentPage);
                    var GuardianList = FieldsList.OrderByDescending(x => x.CreatedDate).Skip((Convert.ToInt16(SearchSFields.CurrentPage) - 1) * 10).Take(10).ToList();
                    ViewBag.GuardianList = GuardianList;
                }
                return View("ManageGuardianArchive");
            }
            else
                return RedirectToAction("Index", "Login");
        }

        public ActionResult SearchGuardianList(SearchSFields SearchSFields)
        {
            if (sessionValidate())
            {
                var guardianCOList = new List<GuardianDetailCO>();
                using (var context = new GS247Entities8())
                {
                    var FieldsList = (from a in context.GuardianDetails
                                     join b in context.StudentDetails on a.StudentDetailsId equals b.StudentDetailsId
                                     where a.DeleteFlag == 0 && b.DeleteFlag == 0 && (b.OnlineStatus ==0 || b.OnlineApplicationStatus == 1)
                                     select a);                                         

                    if (!string.IsNullOrEmpty(SearchSFields.StudentName))
                    {
                        FieldsList = from dd in FieldsList
                                     join sd in context.StudentDetails
                                     on dd.StudentDetailsId equals sd.StudentDetailsId
                                     where (dd.DeleteFlag == 0 || dd.DeleteFlag == null)
                                     && (sd.FirstName.Contains(SearchSFields.StudentName) || sd.LastName.Contains(SearchSFields.StudentName))
                                     select dd;
                    }
                    if (!string.IsNullOrEmpty(SearchSFields.Name))
                    {
                        FieldsList = FieldsList.Where(x => (x.FirstName.Contains(SearchSFields.Name) || x.LastName.Contains(SearchSFields.Name)));
                    }
                    if (!string.IsNullOrEmpty(SearchSFields.Email))
                    {
                        FieldsList = FieldsList.Where(x => (x.Email.Contains(SearchSFields.Email)));
                    }

                    int totalCount = FieldsList.Count();
                    var totalPages = (int)Math.Ceiling((decimal)totalCount / (decimal)10);
                    ViewBag.PageCount = totalPages;
                    ViewBag.CurrentPageIndex = Convert.ToInt16(SearchSFields.CurrentPage);
                    var guardianList = FieldsList.OrderByDescending(x => x.CreatedDate).Skip((Convert.ToInt16(SearchSFields.CurrentPage) - 1) * 10).Take(10).ToList();

                    guardianList.ForEach(itr =>
                    {
                        var GuardianDetails = new GuardianDetailCO();
                        GuardianDetails.GuardianDetailsId = itr.GuardianDetailsId;
                        GuardianDetails.StudentDetailsId = itr.StudentDetailsId;
                        GuardianDetails.FirstName = itr.FirstName;
                        GuardianDetails.LastName = itr.LastName;
                        GuardianDetails.Relation = itr.Relation;
                        GuardianDetails.DOB = itr.DOB;
                        GuardianDetails.Education = itr.Education;
                        GuardianDetails.Occupation = itr.Occupation;
                        GuardianDetails.Income = itr.Income;
                        GuardianDetails.AddressLine1 = itr.AddressLine1;
                        GuardianDetails.AddressLine2 = itr.AddressLine2;
                        GuardianDetails.City = itr.City;
                        GuardianDetails.State = itr.State;
                        GuardianDetails.PinCode = itr.PinCode;
                        GuardianDetails.MobilePhone = itr.MobilePhone;
                        GuardianDetails.OfficePhone1 = itr.OfficePhone1;
                        GuardianDetails.OfficePhone2 = itr.OfficePhone2;
                        GuardianDetails.Email = itr.Email;
                        GuardianDetails.Country = itr.Country;
                        GuardianDetails.ParentUserFlag = itr.ParentUserFlag;
                        GuardianDetails.CreatedBy = itr.CreatedBy;
                        GuardianDetails.CreatedDate = itr.CreatedDate;
                        GuardianDetails.UpdatedBy = itr.UpdatedBy;
                        GuardianDetails.UpdatedDate = itr.UpdatedDate;
                        GuardianDetails.DeleteFlag = itr.DeleteFlag;

                        var SchoolCO = new SchoolCO();
                        var studentData = context.StudentDetails.Find(itr.StudentDetailsId);
                        SchoolCO.StudentName = studentData != null ? studentData.FirstName + " " + studentData.LastName : "";

                        GuardianDetails.SchoolCO = SchoolCO;
                        guardianCOList.Add(GuardianDetails);
                    });

                    ViewBag.guardianList = guardianCOList;
                }
                return View("ListGuardian");
            }
            else
                return RedirectToAction("Index", "Login");
        }

        [HttpPost]
        public ActionResult UpdateGuardianEmail(GuardianDetail collection)
        {
            if (sessionValidate())
            {
                using (var context = new GS247Entities8())
                {
                    var _templist = context.GuardianDetails.Find(collection.GuardianDetailsId);
                    if (_templist != null)
                    {
                        _templist.MobilePhone = collection.MobilePhone;
                        _templist.Email = collection.Email;
                        _templist.UpdatedDate = DateTime.Now;
                        _templist.UpdatedBy = Session["UserName"].ToString();
                        context.Entry(_templist).State = EntityState.Modified;
                        context.SaveChanges();
                    }
                }
                return RedirectToAction("ManageGuardianArchive");
            }
            else
                return RedirectToAction("Index", "Login");
        }

        [HttpPost]
        public ActionResult DeleteGuardianArchive(int GuardianDetailsId)
        {
            if (sessionValidate())
            {
                using (var context = new GS247Entities8())
                {
                    var subscriptions = context.GuardianDetails.Find(GuardianDetailsId);
                    if (subscriptions != null)
                    {
                        context.GuardianDetails.Remove(subscriptions);
                        context.SaveChanges();
                    }
                }
                return new EmptyResult();
            }
            else
                return RedirectToAction("Index", "Login");
        }

        [HttpPost]
        public ActionResult RestoreGuardianArchive(int GuardianDetailsId)
        {
            if (sessionValidate())
            {
                using (var context = new GS247Entities8())
                {
                    var _templist = context.GuardianDetails.Find(GuardianDetailsId);
                    if (_templist != null)
                    {
                        _templist.DeleteFlag = 0;
                        _templist.UpdatedDate = DateTime.Now;
                        _templist.UpdatedBy = Session["UserName"].ToString();
                        context.Entry(_templist).State = EntityState.Modified;
                        context.SaveChanges();
                    }
                }
                return new EmptyResult();
            }
            else
                return RedirectToAction("Index", "Login");
        }
                

        [HttpPost]
        public JsonResult LoadExistingGuardian(string SibilingName, string Parentname, string ParentEmail, int StudentDetailsId)
        {
            List<GuardianDetail> GuardianDetailList = new List<GuardianDetail>();
            List<GuardianDetailCO> GuardianDetailCOList = new List<GuardianDetailCO>();
            
            using (var context = new GS247Entities8())
            {
                if (!string.IsNullOrEmpty(ParentEmail))
                    GuardianDetailList = context.GuardianDetails.Where(x => (x.DeleteFlag == 0 || x.DeleteFlag == null) && x.StudentDetailsId != StudentDetailsId && x.Email.Equals(ParentEmail)).ToList();
                else if (!string.IsNullOrEmpty(Parentname))
                    GuardianDetailList = context.GuardianDetails.Where(x => (x.DeleteFlag == 0 || x.DeleteFlag == null) && x.StudentDetailsId != StudentDetailsId && x.FirstName.Equals(Parentname)).ToList();
                else if (!string.IsNullOrEmpty(SibilingName))
                {
                    GuardianDetailList = (from gd in context.GuardianDetails
                                          join sd in context.StudentDetails
                                          on gd.StudentDetailsId equals sd.StudentDetailsId
                                          where (gd.DeleteFlag == 0 || gd.DeleteFlag == null)
                                          && (sd.DeleteFlag == 0 || sd.DeleteFlag == null)
                                          && sd.FirstName.Equals(SibilingName)
                                          && gd.StudentDetailsId != StudentDetailsId
                                          select gd).ToList();
                }

                GuardianDetailList.ForEach(itr =>
                {
                    var GuardianDetails = new GuardianDetailCO();
                    GuardianDetails.GuardianDetailsId = itr.GuardianDetailsId;
                    GuardianDetails.StudentDetailsId = itr.StudentDetailsId;
                    GuardianDetails.FirstName = itr.FirstName;
                    GuardianDetails.LastName = itr.LastName;
                    GuardianDetails.Relation = itr.Relation;
                    GuardianDetails.DOB = itr.DOB;
                    GuardianDetails.Education = itr.Education;
                    GuardianDetails.Occupation = itr.Occupation;
                    GuardianDetails.Income = itr.Income;
                    GuardianDetails.AddressLine1 = itr.AddressLine1;
                    GuardianDetails.AddressLine2 = itr.AddressLine2;
                    GuardianDetails.City = itr.City;
                    GuardianDetails.State = itr.State;
                    GuardianDetails.PinCode = itr.PinCode;
                    GuardianDetails.MobilePhone = itr.MobilePhone;
                    GuardianDetails.OfficePhone1 = itr.OfficePhone1;
                    GuardianDetails.OfficePhone2 = itr.OfficePhone2;
                    GuardianDetails.Email = itr.Email;
                    GuardianDetails.Country = itr.Country;
                    GuardianDetails.ParentUserFlag = itr.ParentUserFlag;
                    GuardianDetails.CreatedBy = itr.CreatedBy;
                    GuardianDetails.CreatedDate = itr.CreatedDate;
                    GuardianDetails.UpdatedBy = itr.UpdatedBy;
                    GuardianDetails.UpdatedDate = itr.UpdatedDate;
                    GuardianDetails.DeleteFlag = itr.DeleteFlag;

                    var SchoolCO = new SchoolCO();
                    SchoolCO.RelationName = itr.Relation == 1 ? "Father" : itr.Relation == 2 ? "Mother" : "Others";
                    GuardianDetails.SchoolCO = SchoolCO;
                    GuardianDetailCOList.Add(GuardianDetails);
                });
            }
            return Json(new { GuardianDetailList = GuardianDetailCOList }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SaveExistingGuardian(int StudentDetailsId, int GuardianDetailsId)
        {
            using (var context = new GS247Entities8())
            {
                var GuardianDetailList = context.GuardianDetails.Where(x => x.GuardianDetailsId == GuardianDetailsId).FirstOrDefault();
                if (GuardianDetailList != null)
                {
                    var guardianData = new GuardianDetail();
                    guardianData.StudentDetailsId = StudentDetailsId;
                    guardianData.FirstName = GuardianDetailList.FirstName;
                    guardianData.LastName = GuardianDetailList.LastName;
                    guardianData.Relation = GuardianDetailList.Relation;
                    guardianData.DOB = GuardianDetailList.DOB;
                    guardianData.Education = GuardianDetailList.Education;
                    guardianData.Occupation = GuardianDetailList.Occupation;
                    guardianData.Income = GuardianDetailList.Income;
                    guardianData.AddressLine1 = GuardianDetailList.AddressLine1;
                    guardianData.AddressLine2 = GuardianDetailList.AddressLine2;
                    guardianData.City = GuardianDetailList.City;
                    guardianData.State = GuardianDetailList.State;
                    guardianData.PinCode = GuardianDetailList.PinCode;
                    guardianData.MobilePhone = GuardianDetailList.MobilePhone;
                    guardianData.OfficePhone1 = GuardianDetailList.OfficePhone1;
                    guardianData.OfficePhone2 = GuardianDetailList.OfficePhone2;
                    guardianData.Email = GuardianDetailList.Email;
                    guardianData.Country = GuardianDetailList.Country;
                    guardianData.ParentUserFlag = GuardianDetailList.ParentUserFlag;
                    guardianData.DeleteFlag = GuardianDetailList.DeleteFlag;
                    guardianData.CreatedBy = Session["UserName"].ToString();
                    guardianData.CreatedDate = DateTime.Now;
                    context.GuardianDetails.Add(guardianData);
                    context.SaveChanges();
                }
            }
            return Json(null, JsonRequestBehavior.AllowGet);
        }

        public class SearchSFields
        {
            public string StudentName { get; set; }
            public string Name { get; set; }

            public string Email { get; set; }

            public string CurrentPage { get; set; }

            public string alphaValue {get;set;}
        }

        public ActionResult ViewProfile(int StudentDetailsId)
        {
            if (sessionValidate())
            {
                var StudentCOList = new List<StudentDetailCO>();
                var GuardianCOList = new List<GuardianDetailCO>();
                using (var context = new GS247Entities8())
                {
                    ViewBag.StudentDetailsId = StudentDetailsId;
                    var StudentList = context.StudentDetails.Where(x => (x.DeleteFlag == 0) && x.StudentDetailsId == StudentDetailsId).ToList();
                    StudentList.ForEach(itr =>
                    {
                        var newData = new StudentDetailCO();
                        newData.StudentDetailsId = itr.StudentDetailsId;
                        newData.AdmissionNo = itr.AdmissionNo;
                        newData.AdmissionDate = itr.AdmissionDate;
                        newData.FirstName = itr.FirstName;
                        newData.LastName = itr.LastName;
                        newData.MiddleName = itr.MiddleName;
                        newData.StudentId = itr.StudentId;
                        newData.Batch = itr.Batch;
                        newData.DOB = itr.DOB;
                        newData.Gender = itr.Gender;
                        newData.Blood = itr.Blood;
                        newData.BirthPlace = itr.BirthPlace;
                        newData.Language = itr.Language;
                        newData.Nationality = itr.Nationality;
                        newData.Religion = itr.Religion;
                        newData.StudentCategory = itr.StudentCategory;
                        newData.PromitionalFlag = itr.PromitionalFlag;
                        newData.AddressLine1 = itr.AddressLine1;
                        newData.AddressLine2 = itr.AddressLine2;
                        newData.City = itr.City;
                        newData.State = itr.State;
                        newData.PinCode = itr.PinCode;
                        newData.PhoneNumber1 = itr.PhoneNumber1;
                        newData.PhoneNumber2 = itr.PhoneNumber2;
                        newData.Email = itr.Email;
                        newData.FileName = itr.FileName;
                        newData.Data = itr.Data;
                        newData.CreatedBy = itr.CreatedBy;
                        newData.CreatedDate = itr.CreatedDate;
                        newData.UpdatedBy = itr.UpdatedBy;
                        newData.UpdatedDate = itr.UpdatedDate;
                        newData.Country = itr.Country;
                        newData.DeleteFlag = itr.DeleteFlag;
                        newData.RollNumber = itr.RollNumber;
                        newData.Semester = itr.Semester;
                        newData.AcademicYear = itr.AcademicYear;
                        newData.AcademicYear = itr.AcademicYear;
                        newData.AcademicStatus = itr.AcademicStatus;
                        newData.Status = itr.Status;
                        newData.OnlineStatus = itr.OnlineStatus;
                        newData.OnlineApplicationStatus = itr.OnlineApplicationStatus;

                        var SchoolCO = new SchoolCO();
                        var studentCategory = context.StudentCategories.Where(x => x.StudentCategoryID == itr.StudentCategory).FirstOrDefault();
                        SchoolCO.StudentCategoryName = studentCategory != null ? studentCategory.Name : "";

                        var countries = context.countries.Where(x => x.id == itr.Nationality).FirstOrDefault();

                        SchoolCO.CountryName = countries != null ? countries.country_name : "";

                        var countries1 = context.countries.Where(x => x.id == itr.Country).FirstOrDefault();
                        SchoolCO.CountryName1 = countries1 != null ? countries1.country_name : "";

                        newData.SchoolCO = SchoolCO;
                        StudentCOList.Add(newData);
                    });
                    ViewBag.StudentList = StudentCOList;
                    var GuardianList = context.GuardianDetails.Where(x => (x.DeleteFlag == 0) && x.StudentDetailsId == StudentDetailsId).ToList();

                    GuardianList.ForEach(itr =>
                    {
                        var GuardianDetails = new GuardianDetailCO();
                        GuardianDetails.GuardianDetailsId = itr.GuardianDetailsId;
                        GuardianDetails.StudentDetailsId = itr.StudentDetailsId;
                        GuardianDetails.FirstName = itr.FirstName;
                        GuardianDetails.LastName = itr.LastName;
                        GuardianDetails.Relation = itr.Relation;
                        GuardianDetails.DOB = itr.DOB;
                        GuardianDetails.Education = itr.Education;
                        GuardianDetails.Occupation = itr.Occupation;
                        GuardianDetails.Income = itr.Income;
                        GuardianDetails.AddressLine1 = itr.AddressLine1;
                        GuardianDetails.AddressLine2 = itr.AddressLine2;
                        GuardianDetails.City = itr.City;
                        GuardianDetails.State = itr.State;
                        GuardianDetails.PinCode = itr.PinCode;
                        GuardianDetails.MobilePhone = itr.MobilePhone;
                        GuardianDetails.OfficePhone1 = itr.OfficePhone1;
                        GuardianDetails.OfficePhone2 = itr.OfficePhone2;
                        GuardianDetails.Email = itr.Email;
                        GuardianDetails.Country = itr.Country;
                        GuardianDetails.ParentUserFlag = itr.ParentUserFlag;
                        GuardianDetails.CreatedBy = itr.CreatedBy;
                        GuardianDetails.CreatedDate = itr.CreatedDate;
                        GuardianDetails.UpdatedBy = itr.UpdatedBy;
                        GuardianDetails.UpdatedDate = itr.UpdatedDate;
                        GuardianDetails.DeleteFlag = itr.DeleteFlag;


                        var SchoolCO = new SchoolCO();
                        SchoolCO.RelationName = itr.Relation == 1 ? "Father" : itr.Relation == 2 ? "Mother" : "Other";
                        var countries1 = context.countries.Where(x => x.id == itr.Country).FirstOrDefault();                        
                        SchoolCO.CountryName = countries1 != null ? countries1.country_name : "";
                        GuardianDetails.SchoolCO = SchoolCO;
                        GuardianCOList.Add(GuardianDetails);
                    });

                    ViewBag.GuardianList = GuardianCOList;
                    ViewBag.InstitutionList = context.StudentInstutionDetails.Where(x => x.StudentDetailsId == StudentDetailsId).ToList();
                }
                return View();
            }
            else
                return RedirectToAction("Index", "Login");
        }

        public ActionResult ViewCourses(int StudentDetailsId)
        {
            if (sessionValidate())
            {
                var StudentCOList = new List<StudentDetailCO>();
                using (var context = new GS247Entities8())
                {
                    ViewBag.StudentDetailsId = StudentDetailsId;
                    var StudentList = context.StudentDetails.Where(x => (x.DeleteFlag == 0) && x.StudentDetailsId == StudentDetailsId).ToList();
                    StudentList.ForEach(itr =>
                    {
                        var newData = new StudentDetailCO();
                        newData.StudentDetailsId = itr.StudentDetailsId;
                        newData.AdmissionNo = itr.AdmissionNo;
                        newData.AdmissionDate = itr.AdmissionDate;
                        newData.FirstName = itr.FirstName;
                        newData.LastName = itr.LastName;
                        newData.MiddleName = itr.MiddleName;
                        newData.StudentId = itr.StudentId;
                        newData.Batch = itr.Batch;
                        newData.DOB = itr.DOB;
                        newData.Gender = itr.Gender;
                        newData.Blood = itr.Blood;
                        newData.BirthPlace = itr.BirthPlace;
                        newData.Language = itr.Language;
                        newData.Nationality = itr.Nationality;
                        newData.Religion = itr.Religion;
                        newData.StudentCategory = itr.StudentCategory;
                        newData.PromitionalFlag = itr.PromitionalFlag;
                        newData.AddressLine1 = itr.AddressLine1;
                        newData.AddressLine2 = itr.AddressLine2;
                        newData.City = itr.City;
                        newData.State = itr.State;
                        newData.PinCode = itr.PinCode;
                        newData.PhoneNumber1 = itr.PhoneNumber1;
                        newData.PhoneNumber2 = itr.PhoneNumber2;
                        newData.Email = itr.Email;
                        newData.FileName = itr.FileName;
                        newData.Data = itr.Data;
                        newData.CreatedBy = itr.CreatedBy;
                        newData.CreatedDate = itr.CreatedDate;
                        newData.UpdatedBy = itr.UpdatedBy;
                        newData.UpdatedDate = itr.UpdatedDate;
                        newData.Country = itr.Country;
                        newData.DeleteFlag = itr.DeleteFlag;
                        newData.RollNumber = itr.RollNumber;
                        newData.Semester = itr.Semester;
                        newData.AcademicYear = itr.AcademicYear;
                        newData.AcademicYear = itr.AcademicYear;
                        newData.AcademicStatus = itr.AcademicStatus;
                        newData.Status = itr.Status;
                        newData.OnlineStatus = itr.OnlineStatus;
                        newData.OnlineApplicationStatus = itr.OnlineApplicationStatus;
                        var SchoolCO = new SchoolCO();
                        var batch = context.batches.Find(itr.Batch);
                        if (batch != null)
                        {
                            var course = (from c in context.Courses
                                          join cbl in context.BatchCourseLinks on c.Id equals cbl.CourseId
                                          join b in context.batches on cbl.BatchId equals b.Id
                                          where b.Id == batch.Id
                                          select c).FirstOrDefault();

                            if (course != null)
                            {
                                SchoolCO.CourseBatchName = course.Name + " / " + batch.Name;

                                //var Semester = (from c in context.batches
                                //                join cbl in context.semesters on c.Id equals cbl.Id
                                //                join b in context.batches on cbl.BatchId equals b.Id
                                //                where b.Id == batch.Id
                                //                select c).FirstOrDefault();
                            }
                            else
                                SchoolCO.CourseBatchName = "";
                        }

                        newData.SchoolCO = SchoolCO;
                        StudentCOList.Add(newData);
                    });

                    ViewBag.StudentList = StudentCOList;
                }
                return View();
            }
            else
                return RedirectToAction("Index", "Login");
        }

        public ActionResult ViewAssessments(int StudentDetailsId)
        {
            if (sessionValidate())
            {
                var StudentCOList = new List<StudentDetailCO>();
                using (var context = new GS247Entities8())
                {
                    ViewBag.StudentDetailsId = StudentDetailsId;
                    var StudentList = context.StudentDetails.Where(x => (x.DeleteFlag == 0) && x.StudentDetailsId == StudentDetailsId).ToList();
                    StudentList.ForEach(itr =>
                    {
                        var newData = new StudentDetailCO();
                        newData.StudentDetailsId = itr.StudentDetailsId;
                        newData.AdmissionNo = itr.AdmissionNo;
                        newData.AdmissionDate = itr.AdmissionDate;
                        newData.FirstName = itr.FirstName;
                        newData.LastName = itr.LastName;
                        newData.MiddleName = itr.MiddleName;
                        newData.StudentId = itr.StudentId;
                        newData.Batch = itr.Batch;
                        newData.DOB = itr.DOB;
                        newData.Gender = itr.Gender;
                        newData.Blood = itr.Blood;
                        newData.BirthPlace = itr.BirthPlace;
                        newData.Language = itr.Language;
                        newData.Nationality = itr.Nationality;
                        newData.Religion = itr.Religion;
                        newData.StudentCategory = itr.StudentCategory;
                        newData.PromitionalFlag = itr.PromitionalFlag;
                        newData.AddressLine1 = itr.AddressLine1;
                        newData.AddressLine2 = itr.AddressLine2;
                        newData.City = itr.City;
                        newData.State = itr.State;
                        newData.PinCode = itr.PinCode;
                        newData.PhoneNumber1 = itr.PhoneNumber1;
                        newData.PhoneNumber2 = itr.PhoneNumber2;
                        newData.Email = itr.Email;
                        newData.FileName = itr.FileName;
                        newData.Data = itr.Data;
                        newData.CreatedBy = itr.CreatedBy;
                        newData.CreatedDate = itr.CreatedDate;
                        newData.UpdatedBy = itr.UpdatedBy;
                        newData.UpdatedDate = itr.UpdatedDate;
                        newData.Country = itr.Country;
                        newData.DeleteFlag = itr.DeleteFlag;
                        newData.RollNumber = itr.RollNumber;
                        newData.Semester = itr.Semester;
                        newData.AcademicYear = itr.AcademicYear;
                        newData.AcademicYear = itr.AcademicYear;
                        newData.AcademicStatus = itr.AcademicStatus;
                        newData.Status = itr.Status;
                        newData.OnlineStatus = itr.OnlineStatus;
                        newData.OnlineApplicationStatus = itr.OnlineApplicationStatus;
                        var SchoolCO = new SchoolCO();
                        var batch = context.batches.Find(itr.Batch);
                        if (batch != null)
                        {
                            var course = (from c in context.Courses
                                          join cbl in context.BatchCourseLinks on c.Id equals cbl.CourseId
                                          join b in context.batches on cbl.BatchId equals b.Id
                                          where b.Id == batch.Id
                                          select c).FirstOrDefault();

                            if (course != null)
                            {
                                SchoolCO.CourseBatchName = course.Name + " / " + batch.Name;
                            }
                            else
                                SchoolCO.CourseBatchName = "";
                        }

                        newData.SchoolCO = SchoolCO;
                        StudentCOList.Add(newData);
                    });

                    ViewBag.StudentList = StudentCOList;

                    var StudentExamDetailCOList = new List<StudentExamDetailCO>();
                    var examList = (from a in context.StudentExamDetails
                                   join b in context.Examinations on a.ExamId equals b.ExamId    
                                   where a.StudentDetailsId == StudentDetailsId && b.Ststus == 3
                                   select a).OrderByDescending(x => x.StartTime).ToList();         
          
                    examList.ForEach(data =>
                    {
                        var result = new StudentExamDetailCO();
                        result.StudentExamDetailsId = data.StudentExamDetailsId;
                        result.ExamId = data.ExamId;
                        result.StudentDetailsId = data.StudentDetailsId;
                        result.StartTime = data.StartTime;
                        result.EndTime = data.EndTime;
                        result.LoginId = data.LoginId;
                        result.TotalMark = data.TotalMark;
                        result.StatusFlag = data.StatusFlag;
                        result.CreatedBy = data.CreatedBy;
                        result.CreatedDate = data.CreatedDate;

                        var examData = context.Examinations.Find(data.ExamId);
                        if (examData != null)
                        {
                            if (examData.SubjectFlag == 1)
                            {
                                var Subject = context.Subjects.Find(examData.SubjectId);
                                result.SubjectName = Subject != null ? Subject.Name : string.Empty;
                            }

                            var student = context.StudentDetails.Find(data.StudentDetailsId);
                            if (student != null)
                            {  
                                var batch = context.batches.Find(student.Batch);
                                var courses = context.Courses.Find(student.Course);
                                result.CourseName = courses != null ? courses.Name : string.Empty;
                                result.BatchName = batch != null ? batch.Name : string.Empty; 
                            }

                            if (examData.MarksType == 1)
                            {
                                result.TotalMark = data.TotalMark;
                            }
                            else if (examData.MarksType == 2)
                            {
                                int? mark = Convert.ToInt32(data.TotalMark);
                                var grade = context.ExamGradeLevels.Where(x => x.Score <= mark).FirstOrDefault();
                                result.TotalMark = grade != null ? grade.Name : data.TotalMark;
                            }
                            else
                            {
                                int? mark = Convert.ToInt32(data.TotalMark);
                                var grade = context.ExamGradeLevels.Where(x => x.Score >= mark).FirstOrDefault();
                                result.TotalMark = grade != null ? grade.Name + " - " + data.TotalMark : data.TotalMark;
                            }
                        }
                        result.ExamName = examData.Name;
                        StudentExamDetailCOList.Add(result);
                    });

                    ViewBag.StudentExamDetailCOList = StudentExamDetailCOList;
                }
                return View();
            }
            else
                return RedirectToAction("Index", "Login");
        }

        public ActionResult ViewAttendance(int StudentDetailsId)
        {
            if (sessionValidate())
            {
                using (var context = new GS247Entities8())
                {
                    var StudentList = context.StudentDetails.Where(x => (x.DeleteFlag == 0) && x.StudentDetailsId == StudentDetailsId).ToList();
                    ViewBag.StudentDetailsId = StudentDetailsId;
                    ViewBag.StudentList = StudentList;
                }
                return View();
            }
            else
                return RedirectToAction("Index", "Login");
            
        }

        public ActionResult ViewDocuments(int StudentDetailsId)
        {
            if (sessionValidate())
            {
                var list = new List<StudentDocumentCO>();
                using (var context = new GS247Entities8())
                {
                    var FieldsList = context.StudentDocuments.Where(x => x.StudentDetailsId == StudentDetailsId).ToList();

                    var DocumentList = (from sdt in context.SettingsDocumentTypes.ToList()
                                        where !FieldsList.Any(x => x.DocumentTypeId == sdt.Id)
                                        select sdt).Distinct().ToList();

                    ViewBag.DocumentsList = DocumentList;

                    FieldsList.ForEach(data =>
                    {
                        var StudentDocument = new StudentDocumentCO();
                        StudentDocument.StudentDocumentsId = data.StudentDocumentsId;
                        StudentDocument.StudentDetailsId = data.StudentDetailsId;
                        StudentDocument.DocumentTypeId = data.DocumentTypeId;
                        StudentDocument.FileName = data.FileName;
                        StudentDocument.Data = data.Data;
                        StudentDocument.CreatedBy = data.CreatedBy;
                        StudentDocument.CreatedDate = data.CreatedDate;
                        StudentDocument.UpdatedBy = data.UpdatedBy;
                        StudentDocument.UpdatedDate = data.UpdatedDate;
                        StudentDocument.StatusFlag = data.StatusFlag;
                        var SchoolCO = new SchoolCO();
                        SchoolCO.DocumentName = context.SettingsDocumentTypes.Where(x => x.Id == data.DocumentTypeId).FirstOrDefault().DocumentName;

                        StudentDocument.SchoolCO = SchoolCO;

                        list.Add(StudentDocument);

                    });

                    var StudentList = context.StudentDetails.Where(x => (x.DeleteFlag == 0) && x.StudentDetailsId == StudentDetailsId).ToList();
                    
                    ViewBag.StudentList = StudentList;
                    ViewBag.StudentDocumentsList = list;
                    ViewBag.StudentDetailsId = StudentDetailsId;
                }
                return View();
            }
            else
                return RedirectToAction("Index", "Login");
        }

        public ActionResult ViewElectives(int StudentDetailsId)
        {
            if (sessionValidate())
            {
                var list = new List<StudentDetailCO>();
                using (var context = new GS247Entities8())
                {
                    ViewBag.StudentDetailsId = StudentDetailsId;
                    var StudentList = context.StudentDetails.Where(x => (x.DeleteFlag == 0) && x.StudentDetailsId == StudentDetailsId).ToList();
                    StudentList.ForEach(itr =>
                    {
                        var newData = new StudentDetailCO();
                        newData.StudentDetailsId = itr.StudentDetailsId;
                        newData.AdmissionNo = itr.AdmissionNo;
                        newData.AdmissionDate = itr.AdmissionDate;
                        newData.FirstName = itr.FirstName;
                        newData.LastName = itr.LastName;
                        newData.MiddleName = itr.MiddleName;
                        newData.StudentId = itr.StudentId;
                        newData.Batch = itr.Batch;
                        newData.DOB = itr.DOB;
                        newData.Gender = itr.Gender;
                        newData.Blood = itr.Blood;
                        newData.BirthPlace = itr.BirthPlace;
                        newData.Language = itr.Language;
                        newData.Nationality = itr.Nationality;
                        newData.Religion = itr.Religion;
                        newData.StudentCategory = itr.StudentCategory;
                        newData.PromitionalFlag = itr.PromitionalFlag;
                        newData.AddressLine1 = itr.AddressLine1;
                        newData.AddressLine2 = itr.AddressLine2;
                        newData.City = itr.City;
                        newData.State = itr.State;
                        newData.PinCode = itr.PinCode;
                        newData.PhoneNumber1 = itr.PhoneNumber1;
                        newData.PhoneNumber2 = itr.PhoneNumber2;
                        newData.Email = itr.Email;
                        newData.FileName = itr.FileName;
                        newData.Data = itr.Data;
                        newData.CreatedBy = itr.CreatedBy;
                        newData.CreatedDate = itr.CreatedDate;
                        newData.UpdatedBy = itr.UpdatedBy;
                        newData.UpdatedDate = itr.UpdatedDate;
                        newData.Country = itr.Country;
                        newData.DeleteFlag = itr.DeleteFlag;
                        newData.RollNumber = itr.RollNumber;
                        newData.Semester = itr.Semester;
                        newData.AcademicYear = itr.AcademicYear;
                        newData.AcademicYear = itr.AcademicYear;
                        newData.AcademicStatus = itr.AcademicStatus;
                        newData.Status = itr.Status;
                        newData.OnlineStatus = itr.OnlineStatus;
                        newData.OnlineApplicationStatus = itr.OnlineApplicationStatus;

                        var SchoolCO = new SchoolCO();
                        var batch = context.batches.Find(itr.Batch);
                        if (batch != null)
                        {
                            SchoolCO.BatchName = batch.Name;
                            var course = (from c in context.Courses
                                          join cbl in context.BatchCourseLinks on c.Id equals cbl.CourseId
                                          join b in context.batches on cbl.BatchId equals b.Id
                                          where b.Id == batch.Id
                                          select c).FirstOrDefault();

                            if (course != null)
                            {
                                SchoolCO.CourseBatchName = course.Name;

                                var subject = (from c in context.Courses
                                               join csl in context.CoursesSubjectsLinks on c.Id equals csl.SubjectId
                                               join s in context.Subjects on csl.SubjectId equals s.Id
                                               where c.Id == course.Id
                                               select s).FirstOrDefault();
                                SchoolCO.SubjectName = subject != null ? subject.Name : "";
                            }
                            else
                                SchoolCO.CourseBatchName = "";
                        }
                        else
                            SchoolCO.BatchName = "";

                        newData.SchoolCO = SchoolCO;
                        list.Add(newData);
                    });

                    ViewBag.StudentList = list;
                }
                return View();
            }
            else
                return RedirectToAction("Index", "Login");
        }

        public ActionResult ViewAchievements(int StudentDetailsId)
        {
            if (sessionValidate())
            {
                using (var context = new GS247Entities8())
                {
                    var StudentList1 = context.StudentAchievements.Where(x => x.StudentDetailsId == StudentDetailsId).ToList();
                    ViewBag.StudentList1 = StudentList1;
                    var StudentList = context.StudentDetails.Where(x => (x.DeleteFlag == 0) && x.StudentDetailsId == StudentDetailsId).ToList();
                    ViewBag.StudentList = StudentList;
                    ViewBag.StudentDetailsId = StudentDetailsId;
                }
                return View();
            }
            else
                return RedirectToAction("Index", "Login");
        }

        public ActionResult ViewLog(int StudentDetailsId)
        {
            if (sessionValidate())
            {
                var StudentCOList1 = new List<StudentLogCO>();

                using (var context = new GS247Entities8())
                {
                    var StudentList1 = context.StudentLogs.Where(x => x.StudentDetailsId == StudentDetailsId).ToList();
                    StudentList1.ForEach(data =>
                    {
                        var newData = new StudentLogCO();
                        newData.StudentLogID = data.StudentLogID;
                        newData.StudentDetailsId = data.StudentDetailsId;
                        newData.StudentLogCategoryID = data.StudentLogCategoryID;
                        newData.Description = data.Description;
                        newData.NotificationToStudent = data.NotificationToStudent;
                        newData.NotificationToParent = data.NotificationToParent;
                        newData.VisibleToStudents = data.VisibleToStudents;
                        newData.VisibleToParents = data.VisibleToParents;
                        newData.VisibleToTeachers = data.VisibleToTeachers;
                        newData.CreatedBy = data.CreatedBy;
                        newData.CreatedDate = data.CreatedDate;
                        newData.UpdatedBy = data.UpdatedBy;
                        newData.UpdatedDate = data.UpdatedDate;

                        var SchoolCO = new SchoolCO();
                        var Name = context.StudentLogCategories.Find(data.StudentLogCategoryID);
                        SchoolCO.Name = Name != null ? Name.Name : "";
                        newData.SchoolCO = SchoolCO;
                        StudentCOList1.Add(newData);
                    });
                    ViewBag.StudentList1 = StudentCOList1;

                    var StudentList = context.StudentDetails.Where(x => (x.DeleteFlag == 0) && x.StudentDetailsId == StudentDetailsId).ToList();
                    ViewBag.StudentList = StudentList;

                    ViewBag.StudentLogList = context.StudentLogCategories.ToList();
                    ViewBag.StudentDetailsId = StudentDetailsId;
                }
                return View();
            }
            else
                return RedirectToAction("Index", "Login");
        }

        public ActionResult ViewAppraisal(int StudentDetailsId)
        {
            if (sessionValidate())
            {
                using (var context = new GS247Entities8())
                {
                    var StudentList = context.StudentDetails.Where(x => (x.DeleteFlag == 0) && x.StudentDetailsId == StudentDetailsId).ToList();
                    ViewBag.StudentList = StudentList;
                    ViewBag.StudentDetailsId = StudentDetailsId;
                    
                }
                return View();
            }
            else
                return RedirectToAction("Index", "Login");
        }

        [HttpPost]
        public JsonResult StudentValidation(string Email, string phone)
        {
            string Message = "";
            using (var context = new GS247Entities8())
            {
                if (context.StudentDetails.Where(x => x.Email == Email).Any())
                    Message = "Email already available";
                if (context.StudentDetails.Where(x => x.PhoneNumber1 == phone || x.PhoneNumber2 == phone).Any())
                    Message += "<br/> Phone no already available";
            }

            return Json(new { Message = Message }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult StudentUpdateValidation(string Email, string phone, int StudentDetailsId)
        {
            string Message = "";
            using (var context = new GS247Entities8())
            {
                if (context.StudentDetails.Where(x => x.Email == Email && x.StudentDetailsId != StudentDetailsId).Any())
                    Message = "Email already available";
                if (context.StudentDetails.Where(x => x.PhoneNumber1 == phone || x.PhoneNumber2 == phone && x.StudentDetailsId != StudentDetailsId).Any())
                    Message += "<br/> Phone no already available";
            }
            return Json(new { Message = Message }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult UpdateStudentStatus(int StudentDetailsId, int status)
        {
            using (var context = new GS247Entities8())
            {
                var _templist = context.StudentDetails.Find(StudentDetailsId);
                if (_templist != null)
                {
                    _templist.Status = status;
                    _templist.UpdatedDate = DateTime.Now;
                    _templist.UpdatedBy = Session["UserName"].ToString();
                    context.Entry(_templist).State = EntityState.Modified;
                    context.SaveChanges();
                }
            }
            return new EmptyResult();
        }

        [HttpPost]
        public ActionResult DeleteStudentStatus(int StudentDetailsId)
        {
            using (var context = new GS247Entities8())
            {
                var _templist = context.StudentDetails.Find(StudentDetailsId);
                if (_templist != null)
                {
                    _templist.Status = 0;
                    _templist.Batch = null;
                    _templist.UpdatedDate = DateTime.Now;
                    _templist.UpdatedBy = Session["UserName"].ToString();
                    context.Entry(_templist).State = EntityState.Modified;
                    context.SaveChanges();
                }
            }
            return new EmptyResult();
        }

        [HttpPost]
        public ActionResult SaveStudentsLog(int StudentDetailsId, StudentLog StudentLogFields)
        {
            using (var context = new GS247Entities8())
            {
                var _templist = context.StudentLogs.Find(StudentLogFields.StudentLogID);
                if (_templist != null)
                {
                    _templist.StudentLogCategoryID = StudentLogFields.StudentLogCategoryID;
                    _templist.Description = StudentLogFields.Description;
                    _templist.NotificationToParent = StudentLogFields.NotificationToParent;
                    _templist.NotificationToStudent = StudentLogFields.NotificationToStudent;
                    _templist.VisibleToParents = StudentLogFields.VisibleToParents;
                    _templist.VisibleToStudents = StudentLogFields.VisibleToStudents;
                    _templist.VisibleToTeachers = StudentLogFields.VisibleToTeachers;
                    _templist.UpdatedDate = DateTime.Now;
                    _templist.UpdatedBy = Session["UserName"].ToString();
                    context.Entry(_templist).State = EntityState.Modified;
                    context.SaveChanges();
                }
                else
                {
                    StudentLogFields.NotificationToParent = StudentLogFields.NotificationToParent == null ? 0 : StudentLogFields.NotificationToParent;
                    StudentLogFields.NotificationToStudent = StudentLogFields.NotificationToStudent == null ? 0 : StudentLogFields.NotificationToStudent;
                    StudentLogFields.VisibleToParents = StudentLogFields.VisibleToParents == null ? 0 : StudentLogFields.VisibleToParents;
                    StudentLogFields.VisibleToTeachers = StudentLogFields.VisibleToTeachers == null ? 0 : StudentLogFields.VisibleToTeachers;
                    StudentLogFields.VisibleToStudents = StudentLogFields.VisibleToStudents == null ? 0 : StudentLogFields.VisibleToStudents;
                    StudentLogFields.CreatedBy = Session["UserName"].ToString();
                    StudentLogFields.CreatedDate = DateTime.Now;
                    context.StudentLogs.Add(StudentLogFields);
                    context.SaveChanges();
                }
            }
            return RedirectToAction("ViewLog", new RouteValueDictionary(
                   new { controller = "Student", action = "ViewLog", StudentDetailsId = StudentDetailsId }));
        }

        public class StudentLogFields
        {
            public int StudentLogID { get; set; }
            public int? StudentLogCategoryID { get; set; }
            public string Description { get; set; }
            public int? NotificationToStudent { get; set; }
            public int? NotificationToParent { get; set; }
            public int? VisibleToStudents { get; set; }
            public int? VisibleToParents { get; set; }
            public int? VisibleToTeachers { get; set; }
        }

        [HttpPost]
        public ActionResult DeleteStudentsLog(int StudentLogID)
        {
            using (var context = new GS247Entities8())
            {
                var _templist = context.StudentLogs.Find(StudentLogID);
                if (_templist != null)
                {
                    context.StudentLogs.Remove(_templist);
                    context.SaveChanges();
                }
            }
            return new EmptyResult();
        }

        [HttpPost]
        public ActionResult SaveStudentAchievements(int StudentDetailsId, StudentAchievement StudentAchievementFields)
        {
            using (var context = new GS247Entities8())
            {
                HttpPostedFileBase file = Request.Files["DataString"];
                string pathName = string.Empty;

                var _templist = context.StudentAchievements.Find(StudentAchievementFields.AchievementID);
                if (_templist != null)
                {
                    _templist.Name = StudentAchievementFields.Name;
                    _templist.Description = StudentAchievementFields.Description;
                    _templist.DocumentName = StudentAchievementFields.DocumentName;
                    _templist.FileName = StudentAchievementFields.FileName;
                    if (file != null)
                    {
                        string strExtension = Path.GetExtension(file.FileName);
                        pathName = "Student_Achievement" + _templist.DocumentName + "_" + DateTime.Now.ToString("MMddyyyyHHmmss") + strExtension;
                        _templist.Data = pathName;
                    }
                    else
                        _templist.Data = StudentAchievementFields.Data;
                    _templist.StudentDetailsId = StudentDetailsId;
                    _templist.UpdatedDate = DateTime.Now;
                    _templist.UpdatedBy = Session["UserName"].ToString();
                    context.Entry(_templist).State = EntityState.Modified;
                    context.SaveChanges();

                    if (file != null)
                        saveImagesUpload(file, pathName);
                }
                else
                {
                    StudentAchievementFields.StudentDetailsId = StudentDetailsId;
                    StudentAchievementFields.CreatedBy = Session["UserName"].ToString();
                    if (file != null)
                    {
                        string strExtension = Path.GetExtension(file.FileName);
                        pathName = "Student_Achievement" + StudentAchievementFields.DocumentName + "_" + DateTime.Now.ToString("MMddyyyyHHmmss") + strExtension;
                    }

                    StudentAchievementFields.Data = pathName;
                    StudentAchievementFields.CreatedDate = DateTime.Now;
                    context.StudentAchievements.Add(StudentAchievementFields);
                    context.SaveChanges();

                    if (file != null)
                        saveImagesUpload(file, pathName);
                }
            }
            return RedirectToAction("ViewAchievements", new RouteValueDictionary(
                   new { controller = "Student", action = "ViewAchievements", StudentDetailsId = StudentDetailsId }));
        }

        public class StudentAchievementFields
        {
            public int AchievementID { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public string DocumentName { get; set; }
            public string FileName { get; set; }
            public string Data { get; set; }
        }

        [HttpPost]
        public ActionResult DeleteStudentAchievements(int AchievementID)
        {
            using (var context = new GS247Entities8())
            {
                var _templist = context.StudentAchievements.Find(AchievementID);
                if (_templist != null)
                {
                    context.StudentAchievements.Remove(_templist);
                    context.SaveChanges();
                }
            }
            return new EmptyResult();
        }

        public ActionResult studentLogDetail()
        {
            if (sessionValidate())
            {
                var StudentCOList1 = new List<StudentLogCO>();
                using (var context = new GS247Entities8())
                {
                    var StudentList1 = context.StudentLogs.Where(x => x.VisibleToStudents == 1).ToList();
                    StudentList1.ForEach(data =>
                    {
                        var newData = new StudentLogCO();
                        newData.StudentLogID = data.StudentLogID;
                        newData.StudentDetailsId = data.StudentDetailsId;
                        newData.StudentLogCategoryID = data.StudentLogCategoryID;
                        newData.Description = data.Description;
                        newData.NotificationToStudent = data.NotificationToStudent;
                        newData.NotificationToParent = data.NotificationToParent;
                        newData.VisibleToStudents = data.VisibleToStudents;
                        newData.VisibleToParents = data.VisibleToParents;
                        newData.VisibleToTeachers = data.VisibleToTeachers;
                        newData.CreatedBy = data.CreatedBy;
                        newData.CreatedDate = data.CreatedDate;
                        newData.UpdatedBy = data.UpdatedBy;
                        newData.UpdatedDate = data.UpdatedDate;

                        var SchoolCO = new SchoolCO();
                        var Name = context.StudentLogCategories.Find(data.StudentLogCategoryID);
                        SchoolCO.Name = Name != null ? Name.Name : "";

                        newData.SchoolCO = SchoolCO;
                        StudentCOList1.Add(newData);
                    });
                    ViewBag.StudentList1 = StudentCOList1;
                }
                return View();
            }
            else
                return RedirectToAction("Index", "Login");
        }

        #endregion

        #region "ONLINE APPLICATION"

        public ActionResult OnlineApplicant()
        {
            if (sessionValidate())
            {
                var StudentCOList = new List<StudentDetailCO>();
                using (var context = new GS247Entities8())
                {                    
                    var FieldsList = context.StudentDetails.Where(x => (x.DeleteFlag == 0) && x.OnlineStatus == 1);
                    int totalCount = FieldsList.Count();
                    var totalPages = (int)Math.Ceiling((decimal)totalCount / (decimal)10);
                    ViewBag.PageCount = totalPages;
                    ViewBag.CurrentPageIndex = 1;
                    var StudentList = FieldsList.OrderByDescending(x => x.CreatedDate).Take(10).ToList();
                    StudentList.ForEach(itr =>
                    {
                        var newData = new StudentDetailCO();
                        newData.StudentDetailsId = itr.StudentDetailsId;
                        newData.AdmissionNo = itr.AdmissionNo;
                        newData.AdmissionDate = itr.AdmissionDate;
                        newData.FirstName = itr.FirstName;
                        newData.LastName = itr.LastName;
                        newData.MiddleName = itr.MiddleName;
                        newData.StudentId = itr.StudentId;
                        newData.Batch = itr.Batch;
                        newData.DOB = itr.DOB;
                        newData.Gender = itr.Gender;
                        newData.Blood = itr.Blood;
                        newData.BirthPlace = itr.BirthPlace;
                        newData.Language = itr.Language;
                        newData.Nationality = itr.Nationality;
                        newData.Religion = itr.Religion;
                        newData.StudentCategory = itr.StudentCategory;
                        newData.PromitionalFlag = itr.PromitionalFlag;
                        newData.AddressLine1 = itr.AddressLine1;
                        newData.AddressLine2 = itr.AddressLine2;
                        newData.City = itr.City;
                        newData.State = itr.State;
                        newData.PinCode = itr.PinCode;
                        newData.PhoneNumber1 = itr.PhoneNumber1;
                        newData.PhoneNumber2 = itr.PhoneNumber2;
                        newData.Email = itr.Email;
                        newData.FileName = itr.FileName;
                        newData.Data = itr.Data;
                        newData.CreatedBy = itr.CreatedBy;
                        newData.CreatedDate = itr.CreatedDate;
                        newData.UpdatedBy = itr.UpdatedBy;
                        newData.UpdatedDate = itr.UpdatedDate;
                        newData.Country = itr.Country;
                        newData.DeleteFlag = itr.DeleteFlag;
                        newData.RollNumber = itr.RollNumber;
                        newData.Semester = itr.Semester;
                        newData.AcademicYear = itr.AcademicYear;
                        newData.AcademicYear = itr.AcademicYear;
                        newData.AcademicStatus = itr.AcademicStatus;
                        newData.Status = itr.Status;
                        newData.OnlineStatus = itr.OnlineStatus;
                        newData.OnlineApplicationStatus = itr.OnlineApplicationStatus;

                        var SchoolCO = new SchoolCO();
                        var batch = context.batches.Find(itr.Batch);
                        if (batch != null)
                        {
                            var course = (from c in context.Courses
                                          join cbl in context.BatchCourseLinks on c.Id equals cbl.CourseId
                                          join b in context.batches on cbl.BatchId equals b.Id
                                          where b.Id == batch.Id
                                          select c).FirstOrDefault();

                            if (course != null)
                            {
                                SchoolCO.CourseBatchName = course.Name + " / " + batch.Name;
                            }
                            else
                                SchoolCO.CourseBatchName = "";
                        }

                        newData.SchoolCO = SchoolCO;
                        StudentCOList.Add(newData);
                    });

                    ViewBag.StudentList = StudentCOList;

                    var courseBatchList = context.BatchCourseLinks.ToList();
                    var batchList = new List<batch>();
                    courseBatchList.ForEach(dd =>
                    {
                        var item = new batch();
                        item.Id = dd.BatchId;
                        var batch = context.batches.Find(dd.BatchId);
                        var course = context.Courses.Find(dd.CourseId);
                        item.Name = (course != null && batch != null) ? course.Name + " / " + batch.Name : string.Empty;
                        batchList.Add(item);
                    });

                    ViewBag.BatchList = batchList;
                }
                return View();
            }
            else
                return RedirectToAction("Index", "Login");
        }

        public ActionResult OnlineApplicantSearch(string alphaValue, string AdmissionId, string CurrentPage)
        {
            if (sessionValidate())
            {
                var StudentCOList = new List<StudentDetailCO>();

                using (var context = new GS247Entities8())
                {
                    var FieldsList = context.StudentDetails.Where(x => (x.DeleteFlag == 0) && x.OnlineStatus == 1).ToList();

                    if (alphaValue != "All")
                    {
                        var query = FieldsList.AsEnumerable();
                        FieldsList = query.Where(m => m.FirstName[0] == Convert.ToChar(alphaValue)).ToList();
                    }

                    int totalCount = FieldsList.Count();
                    var totalPages = (int)Math.Ceiling((decimal)totalCount / (decimal)10);
                    ViewBag.PageCount = totalPages;
                    ViewBag.CurrentPageIndex = Convert.ToInt16(CurrentPage);
                    var StudentList = FieldsList.OrderByDescending(x => x.CreatedDate).Skip((Convert.ToInt16(CurrentPage) - 1) * 10).Take(10).ToList();

                    StudentList.ForEach(itr =>
                    {
                        var newData = new StudentDetailCO();
                        newData.StudentDetailsId = itr.StudentDetailsId;
                        newData.AdmissionNo = itr.AdmissionNo;
                        newData.AdmissionDate = itr.AdmissionDate;
                        newData.FirstName = itr.FirstName;
                        newData.LastName = itr.LastName;
                        newData.MiddleName = itr.MiddleName;
                        newData.StudentId = itr.StudentId;
                        newData.Batch = itr.Batch;
                        newData.DOB = itr.DOB;
                        newData.Gender = itr.Gender;
                        newData.Blood = itr.Blood;
                        newData.BirthPlace = itr.BirthPlace;
                        newData.Language = itr.Language;
                        newData.Nationality = itr.Nationality;
                        newData.Religion = itr.Religion;
                        newData.StudentCategory = itr.StudentCategory;
                        newData.PromitionalFlag = itr.PromitionalFlag;
                        newData.AddressLine1 = itr.AddressLine1;
                        newData.AddressLine2 = itr.AddressLine2;
                        newData.City = itr.City;
                        newData.State = itr.State;
                        newData.PinCode = itr.PinCode;
                        newData.PhoneNumber1 = itr.PhoneNumber1;
                        newData.PhoneNumber2 = itr.PhoneNumber2;
                        newData.Email = itr.Email;
                        newData.FileName = itr.FileName;
                        newData.Data = itr.Data;
                        newData.CreatedBy = itr.CreatedBy;
                        newData.CreatedDate = itr.CreatedDate;
                        newData.UpdatedBy = itr.UpdatedBy;
                        newData.UpdatedDate = itr.UpdatedDate;
                        newData.Country = itr.Country;
                        newData.DeleteFlag = itr.DeleteFlag;
                        newData.RollNumber = itr.RollNumber;
                        newData.Semester = itr.Semester;
                        newData.AcademicYear = itr.AcademicYear;
                        newData.AcademicYear = itr.AcademicYear;
                        newData.AcademicStatus = itr.AcademicStatus;
                        newData.Status = itr.Status;
                        newData.OnlineStatus = itr.OnlineStatus;
                        newData.OnlineApplicationStatus = itr.OnlineApplicationStatus;

                        var SchoolCO = new SchoolCO();
                        var batch = context.batches.Find(itr.Batch);
                        if (batch != null)
                        {
                            var course = (from c in context.Courses
                                          join cbl in context.BatchCourseLinks on c.Id equals cbl.CourseId
                                          join b in context.batches on cbl.BatchId equals b.Id
                                          where b.Id == batch.Id
                                          select c).FirstOrDefault();

                            if (course != null)
                            {
                                SchoolCO.CourseBatchName = course.Name + " / " + batch.Name;
                            }
                            else
                                SchoolCO.CourseBatchName = "";
                        }

                        newData.SchoolCO = SchoolCO;
                        StudentCOList.Add(newData);
                    });

                    ViewBag.StudentList = StudentCOList;

                    var courseBatchList = context.BatchCourseLinks.ToList();
                    var batchList = new List<batch>();
                    courseBatchList.ForEach(dd =>
                    {
                        var item = new batch();
                        item.Id = dd.BatchId;
                        var batch = context.batches.Find(dd.BatchId);
                        var course = context.Courses.Find(dd.CourseId);
                        item.Name = (course != null && batch != null) ? course.Name + " / " + batch.Name : string.Empty;
                        batchList.Add(item);
                    });

                    ViewBag.BatchList = batchList;
                }
                return View("OnlineApplicant");
            }
            else
                return RedirectToAction("Index", "Login");
        }

        public ActionResult OnlineViewProfile(int StudentDetailsId)
        {
            if (sessionValidate())
            {
                var StudentCOList = new List<StudentDetailCO>();
                var GuardianDetailCOList = new List<GuardianDetailCO>();
                using (var context = new GS247Entities8())
                {
                    ViewBag.StudentDetailsId = StudentDetailsId;
                    var StudentList = context.StudentDetails.Where(x => (x.DeleteFlag == 0) && x.StudentDetailsId == StudentDetailsId).ToList();
                    StudentList.ForEach(itr =>
                    {
                        var newData = new StudentDetailCO();
                        newData.StudentDetailsId = itr.StudentDetailsId;
                        newData.AdmissionNo = itr.AdmissionNo;
                        newData.AdmissionDate = itr.AdmissionDate;
                        newData.FirstName = itr.FirstName;
                        newData.LastName = itr.LastName;
                        newData.MiddleName = itr.MiddleName;
                        newData.StudentId = itr.StudentId;
                        newData.Batch = itr.Batch;
                        newData.DOB = itr.DOB;
                        newData.Gender = itr.Gender;
                        newData.Blood = itr.Blood;
                        newData.BirthPlace = itr.BirthPlace;
                        newData.Language = itr.Language;
                        newData.Nationality = itr.Nationality;
                        newData.Religion = itr.Religion;
                        newData.StudentCategory = itr.StudentCategory;
                        newData.PromitionalFlag = itr.PromitionalFlag;
                        newData.AddressLine1 = itr.AddressLine1;
                        newData.AddressLine2 = itr.AddressLine2;
                        newData.City = itr.City;
                        newData.State = itr.State;
                        newData.PinCode = itr.PinCode;
                        newData.PhoneNumber1 = itr.PhoneNumber1;
                        newData.PhoneNumber2 = itr.PhoneNumber2;
                        newData.Email = itr.Email;
                        newData.FileName = itr.FileName;
                        newData.Data = itr.Data;
                        newData.CreatedBy = itr.CreatedBy;
                        newData.CreatedDate = itr.CreatedDate;
                        newData.UpdatedBy = itr.UpdatedBy;
                        newData.UpdatedDate = itr.UpdatedDate;
                        newData.Country = itr.Country;
                        newData.DeleteFlag = itr.DeleteFlag;
                        newData.RollNumber = itr.RollNumber;
                        newData.Semester = itr.Semester;
                        newData.AcademicYear = itr.AcademicYear;
                        newData.AcademicYear = itr.AcademicYear;
                        newData.AcademicStatus = itr.AcademicStatus;
                        newData.Status = itr.Status;
                        newData.OnlineStatus = itr.OnlineStatus;
                        newData.OnlineApplicationStatus = itr.OnlineApplicationStatus;

                        var SchoolCO = new SchoolCO();
                        var studentCategory = context.StudentCategories.Where(x => x.StudentCategoryID == itr.StudentCategory).FirstOrDefault();
                        SchoolCO.StudentCategoryName = studentCategory != null ? studentCategory.Name : "";

                        var countries = context.countries.Where(x => x.id == itr.Nationality).FirstOrDefault();

                        SchoolCO.CountryName = countries != null ? countries.country_name : "";

                        var countries1 = context.countries.Where(x => x.id == itr.Country).FirstOrDefault();
                        SchoolCO.CountryName1 = countries1 != null ? countries1.country_name : "";

                        newData.SchoolCO = SchoolCO;
                        StudentCOList.Add(newData);

                    });
                    ViewBag.StudentList = StudentCOList;
                    var GuardianList = context.GuardianDetails.Where(x => (x.DeleteFlag == 0) && x.StudentDetailsId == StudentDetailsId).ToList();

                    GuardianList.ForEach(itr =>
                    {
                        var GuardianDetails = new GuardianDetailCO();
                        GuardianDetails.GuardianDetailsId = itr.GuardianDetailsId;
                        GuardianDetails.StudentDetailsId = itr.StudentDetailsId;
                        GuardianDetails.FirstName = itr.FirstName;
                        GuardianDetails.LastName = itr.LastName;
                        GuardianDetails.Relation = itr.Relation;
                        GuardianDetails.DOB = itr.DOB;
                        GuardianDetails.Education = itr.Education;
                        GuardianDetails.Occupation = itr.Occupation;
                        GuardianDetails.Income = itr.Income;
                        GuardianDetails.AddressLine1 = itr.AddressLine1;
                        GuardianDetails.AddressLine2 = itr.AddressLine2;
                        GuardianDetails.City = itr.City;
                        GuardianDetails.State = itr.State;
                        GuardianDetails.PinCode = itr.PinCode;
                        GuardianDetails.MobilePhone = itr.MobilePhone;
                        GuardianDetails.OfficePhone1 = itr.OfficePhone1;
                        GuardianDetails.OfficePhone2 = itr.OfficePhone2;
                        GuardianDetails.Email = itr.Email;
                        GuardianDetails.Country = itr.Country;
                        GuardianDetails.ParentUserFlag = itr.ParentUserFlag;
                        GuardianDetails.CreatedBy = itr.CreatedBy;
                        GuardianDetails.CreatedDate = itr.CreatedDate;
                        GuardianDetails.UpdatedBy = itr.UpdatedBy;
                        GuardianDetails.UpdatedDate = itr.UpdatedDate;
                        GuardianDetails.DeleteFlag = itr.DeleteFlag;

                        var SchoolCO = new SchoolCO();
                        SchoolCO.RelationName = itr.Relation == 1 ? "Father" : itr.Relation == 2 ? "Mother" : "Other";
                        var countries1 = context.countries.Where(x => x.id == itr.Country).FirstOrDefault();
                        SchoolCO.CountryName = countries1 != null ? countries1.country_name : "";
                        GuardianDetails.SchoolCO = SchoolCO;
                        GuardianDetailCOList.Add(GuardianDetails);

                    });

                    ViewBag.GuardianList = GuardianDetailCOList;
                    ViewBag.InstitutionList = context.StudentInstutionDetails.Where(x => x.StudentDetailsId == StudentDetailsId).ToList();

                    var courseBatchList = context.BatchCourseLinks.ToList();
                    var batchList = new List<batch>();
                    courseBatchList.ForEach(dd => {
                        var item = new batch();
                        item.Id = dd.BatchId;
                        var batch = context.batches.Find(dd.BatchId);
                        var course = context.Courses.Find(dd.CourseId);
                        item.Name = (course != null && batch != null) ? course.Name + " / " + batch.Name : string.Empty;
                        batchList.Add(item);
                    });

                    ViewBag.BatchList = batchList;
                }
                return View();
            }
            else
                return RedirectToAction("Index", "Login");
        }
        
        public ActionResult StudentApproval()
        {
            if (sessionValidate())
            {
                var StudentCOList = new List<StudentDetailCO>();
                using (var context = new GS247Entities8())
                {
                    var StudentList = context.StudentDetails.Where(x => (x.DeleteFlag == 0) && x.OnlineStatus == 1 && x.Status == 0).ToList();
                    StudentList.ForEach(itr =>
                    {
                        var newData = new StudentDetailCO();
                        newData.StudentDetailsId = itr.StudentDetailsId;
                        newData.AdmissionNo = itr.AdmissionNo;
                        newData.AdmissionDate = itr.AdmissionDate;
                        newData.FirstName = itr.FirstName;
                        newData.LastName = itr.LastName;
                        newData.MiddleName = itr.MiddleName;
                        newData.StudentId = itr.StudentId;
                        newData.Batch = itr.Batch;
                        newData.DOB = itr.DOB;
                        newData.Gender = itr.Gender;
                        newData.Blood = itr.Blood;
                        newData.BirthPlace = itr.BirthPlace;
                        newData.Language = itr.Language;
                        newData.Nationality = itr.Nationality;
                        newData.Religion = itr.Religion;
                        newData.StudentCategory = itr.StudentCategory;
                        newData.PromitionalFlag = itr.PromitionalFlag;
                        newData.AddressLine1 = itr.AddressLine1;
                        newData.AddressLine2 = itr.AddressLine2;
                        newData.City = itr.City;
                        newData.State = itr.State;
                        newData.PinCode = itr.PinCode;
                        newData.PhoneNumber1 = itr.PhoneNumber1;
                        newData.PhoneNumber2 = itr.PhoneNumber2;
                        newData.Email = itr.Email;
                        newData.FileName = itr.FileName;
                        newData.Data = itr.Data;
                        newData.CreatedBy = itr.CreatedBy;
                        newData.CreatedDate = itr.CreatedDate;
                        newData.UpdatedBy = itr.UpdatedBy;
                        newData.UpdatedDate = itr.UpdatedDate;
                        newData.Country = itr.Country;
                        newData.DeleteFlag = itr.DeleteFlag;
                        newData.RollNumber = itr.RollNumber;
                        newData.Semester = itr.Semester;
                        newData.AcademicYear = itr.AcademicYear;
                        newData.AcademicYear = itr.AcademicYear;
                        newData.AcademicStatus = itr.AcademicStatus;
                        newData.Status = itr.Status;
                        newData.OnlineStatus = itr.OnlineStatus;
                        newData.OnlineApplicationStatus = itr.OnlineApplicationStatus;

                        var SchoolCO = new SchoolCO();
                        var batch = context.batches.Find(itr.Batch);
                        if (batch != null)
                        {
                            var course = (from c in context.Courses
                                          join cbl in context.BatchCourseLinks on c.Id equals cbl.CourseId
                                          join b in context.batches on cbl.BatchId equals b.Id
                                          where b.Id == batch.Id
                                          select c).FirstOrDefault();

                            if (course != null)
                            {
                                SchoolCO.CourseBatchName = course.Name + " / " + batch.Name;
                            }
                            else
                                SchoolCO.CourseBatchName = "";
                        }

                        newData.SchoolCO = SchoolCO;
                        StudentCOList.Add(newData);
                    });

                    ViewBag.StudentList = StudentCOList;
                    var courseBatchList = context.BatchCourseLinks.ToList();
                    var batchList = new List<batch>();
                    courseBatchList.ForEach(dd =>
                    {
                        var item = new batch();
                        item.Id = dd.BatchId;
                        var batch = context.batches.Find(dd.BatchId);
                        var course = context.Courses.Find(dd.CourseId);
                        item.Name = (course != null && batch != null) ? course.Name + " / " + batch.Name : string.Empty;
                        batchList.Add(item);
                    });

                    ViewBag.BatchList = batchList;
                }
                return View();
            }
            else
                return RedirectToAction("Index", "Login");
        }

        public ActionResult WaitingList()
        {
            if (sessionValidate())
            {
                var waitingCOList = new List<StudentWaitingListCO>();
                using (var context = new GS247Entities8())
                {
                    var waitingList = context.StudentWaitingLists.Where(x => x.StarusFlag == 0).ToList(); 
                    waitingList.ForEach(data =>
                    {

                        var newData = new StudentWaitingListCO();
                        newData.StudentWaitingId = data.StudentWaitingId;
                        newData.StudentDetailsId = data.StudentDetailsId;
                        newData.BatchId = data.BatchId;
                        newData.StarusFlag = data.StarusFlag;
                        newData.Priority = data.Priority;
                        newData.CreatedBy = data.CreatedBy;
                        newData.CreatedDate = data.CreatedDate;
                        newData.UpdatedBy = data.UpdatedBy;
                        newData.UpdatedDate = data.UpdatedDate;

                        var StudentData = context.StudentDetails.Where(x => x.StudentDetailsId == data.StudentDetailsId).FirstOrDefault();
                        if(StudentData != null)
                        {
                            var SchoolCO = new SchoolCO();
                            var batch = context.batches.Find(StudentData.Batch);
                            if (batch != null)
                            {                                
                                var course = (from c in context.Courses
                                              join cbl in context.BatchCourseLinks on c.Id equals cbl.CourseId
                                              join b in context.batches on cbl.BatchId equals b.Id
                                              where b.Id == batch.Id
                                              select c).FirstOrDefault();

                                if (course != null)
                                {
                                    SchoolCO.CourseBatchName = course.Name + " / " + batch.Name;
                                }
                                else
                                    SchoolCO.CourseBatchName = "";
                            }
                            else
                            {
                                SchoolCO.CourseBatchName = "";
                                SchoolCO.Id = null;
                            }
                            SchoolCO.TransactionDateStr = StudentData.AdmissionDate.GetValueOrDefault().ToString("dd MMM yyyy");
                            SchoolCO.StudentName = StudentData.FirstName + " " + StudentData.LastName;
                            SchoolCO.AdmissionNo = StudentData.AdmissionNo;
                            SchoolCO.Id = StudentData.Batch;
                            newData.SchoolCO = SchoolCO;
                        }
                        waitingCOList.Add(newData);               
                    });

                    ViewBag.StudentList = waitingCOList;

                    var courseBatchList = context.BatchCourseLinks.ToList();
                    var batchList = new List<batch>();
                    courseBatchList.ForEach(dd =>
                    {
                        var item = new batch();
                        item.Id = dd.BatchId;
                        var batch = context.batches.Find(dd.BatchId);
                        var course = context.Courses.Find(dd.CourseId);
                        item.Name = (course != null && batch != null) ? course.Name + " / " + batch.Name : string.Empty;
                        batchList.Add(item);
                    });

                    ViewBag.BatchList = batchList;
                }
                return View();
            }
            else
                return RedirectToAction("Index", "Login");
        }

        public ActionResult IncompleteRegistration()
        {
            if (sessionValidate())
            {
                var StudentCOList = new List<StudentDetailCO>();
                using (var context = new GS247Entities8())
                {
                    var StudentList = context.StudentDetails.Where(x => (x.DeleteFlag == 0) && x.OnlineStatus == 1 && x.Status == 3).ToList();
                    StudentList.ForEach(itr =>
                    {
                        var newData = new StudentDetailCO();
                        newData.StudentDetailsId = itr.StudentDetailsId;
                        newData.AdmissionNo = itr.AdmissionNo;
                        newData.AdmissionDate = itr.AdmissionDate;
                        newData.FirstName = itr.FirstName;
                        newData.LastName = itr.LastName;
                        newData.MiddleName = itr.MiddleName;
                        newData.StudentId = itr.StudentId;
                        newData.Batch = itr.Batch;
                        newData.DOB = itr.DOB;
                        newData.Gender = itr.Gender;
                        newData.Blood = itr.Blood;
                        newData.BirthPlace = itr.BirthPlace;
                        newData.Language = itr.Language;
                        newData.Nationality = itr.Nationality;
                        newData.Religion = itr.Religion;
                        newData.StudentCategory = itr.StudentCategory;
                        newData.PromitionalFlag = itr.PromitionalFlag;
                        newData.AddressLine1 = itr.AddressLine1;
                        newData.AddressLine2 = itr.AddressLine2;
                        newData.City = itr.City;
                        newData.State = itr.State;
                        newData.PinCode = itr.PinCode;
                        newData.PhoneNumber1 = itr.PhoneNumber1;
                        newData.PhoneNumber2 = itr.PhoneNumber2;
                        newData.Email = itr.Email;
                        newData.FileName = itr.FileName;
                        newData.Data = itr.Data;
                        newData.CreatedBy = itr.CreatedBy;
                        newData.CreatedDate = itr.CreatedDate;
                        newData.UpdatedBy = itr.UpdatedBy;
                        newData.UpdatedDate = itr.UpdatedDate;
                        newData.Country = itr.Country;
                        newData.DeleteFlag = itr.DeleteFlag;
                        newData.RollNumber = itr.RollNumber;
                        newData.Semester = itr.Semester;
                        newData.AcademicYear = itr.AcademicYear;
                        newData.AcademicYear = itr.AcademicYear;
                        newData.AcademicStatus = itr.AcademicStatus;
                        newData.Status = itr.Status;
                        newData.OnlineStatus = itr.OnlineStatus;
                        newData.OnlineApplicationStatus = itr.OnlineApplicationStatus;

                        var SchoolCO = new SchoolCO();
                        var batch = context.batches.Find(itr.Batch);
                        if (batch != null)
                        {
                            var course = (from c in context.Courses
                                          join cbl in context.BatchCourseLinks on c.Id equals cbl.CourseId
                                          join b in context.batches on cbl.BatchId equals b.Id
                                          where b.Id == batch.Id
                                          select c).FirstOrDefault();

                            if (course != null)
                            {
                                SchoolCO.CourseBatchName = course.Name + " / " + batch.Name;
                            }
                            else
                                SchoolCO.CourseBatchName = "";
                        }
                        newData.SchoolCO = SchoolCO;
                        StudentCOList.Add(newData);
                    });

                    ViewBag.StudentList = StudentCOList;

                    var courseBatchList = context.BatchCourseLinks.ToList();
                    var batchList = new List<batch>();
                    courseBatchList.ForEach(dd =>
                    {
                        var item = new batch();
                        item.Id = dd.BatchId;
                        var batch = context.batches.Find(dd.BatchId);
                        var course = context.Courses.Find(dd.CourseId);
                        item.Name = (course != null && batch != null) ? course.Name + " / " + batch.Name : string.Empty;
                        batchList.Add(item);
                    });

                    ViewBag.BatchList = batchList;
                }
                return View();
            }
            else
                return RedirectToAction("Index", "Login");
        }

        public ActionResult OnlineCreateStudent()
        {
            if (sessionValidate())
            {
                var manageFieldsList = new List<StudentFieldSetting>();
                int id = Convert.ToInt32(Session["UserId"].ToString());
                using (var context = new GS247Entities8())
                {
                    var FieldsList = context.StudentFieldSettings.Where(x => x.UserID > 0).ToList();
                    ViewBag.managePersonalFieldsList = FieldsList.Where(x => x.TabSelection == 1 && x.TabSubSelection == 1).ToList();
                    ViewBag.manageContactFieldsList = FieldsList.Where(x => x.TabSelection == 1 && x.TabSubSelection == 2).ToList();
                    ViewBag.AdmissionNo = GetMaxId("OnlineAdmissionNo", "StudentDetails");                                    
                    ViewBag.AdmissionDate = DateTime.Now;                    
                    ViewBag.CourseList = context.Courses.ToList();
                    ViewBag.StudentCategoryList = context.StudentCategories.ToList();
                    ViewBag.CountryList = context.countries.ToList();
                }
                return View();
            }
            else
                return RedirectToAction("Index", "Login");
        }

        [HttpPost]
        public ActionResult OnlineCreateStudent(StudentDetail collection)
        {
            HttpPostedFileBase file = Request.Files["DataString"];
            string pathName = string.Empty;
            using (var context = new GS247Entities8())
            {
                collection.CreatedBy = Session["UserName"].ToString();
                collection.CreatedDate = DateTime.Now;
                collection.DeleteFlag = 0;
                collection.Status = 0;
                collection.OnlineStatus = 1;
                collection.OnlineApplicationStatus = 0;
                if (file != null)
                {
                    string strExtension = Path.GetExtension(file.FileName);
                    pathName = "Profile_" + collection.AdmissionNo + "_" + DateTime.Now.ToString("MMddyyyyHHmmss") + strExtension;
                }
                collection.Data = pathName;
                context.StudentDetails.Add(collection);
                context.SaveChanges();

                SaveSeqTable("OnlineAdmissionNo", "StudentDetails");                

                if (file != null)
                    saveImagesUpload(file, pathName);
            }
            return RedirectToAction("OnlineCreateGuardian", new RouteValueDictionary(
                    new { controller = "Student", action = "OnlineCreateGuardian", StudentDetailsId = collection.StudentDetailsId }));
        }

        public ActionResult OnlineCreatePreviousDetail(int? StudentDetailsId)
        {
            if (sessionValidate())
            {
                int id = Convert.ToInt32(Session["UserId"].ToString());
                using (var context = new GS247Entities8())
                {
                    var FieldsList = context.StudentFieldSettings.Where(x => x.UserID > 0).ToList();
                    ViewBag.managInstutionFieldsList = FieldsList.Where(x => x.TabSelection == 3 && x.TabSubSelection == 1).ToList();
                    var StudentInstutionDetailsList = context.StudentInstutionDetails.Where(x => x.StudentDetailsId == StudentDetailsId).ToList();
                    ViewBag.StudentInstutionDetailsList = StudentInstutionDetailsList;
                    ViewBag.StudentDetailsId = StudentDetailsId;
                }
                return View();
            }
            else
                return RedirectToAction("Index", "Login");
        }

        [HttpPost]
        public ActionResult OnlineCreatePreviousDetail(StudentInstutionDetail collection, int? StudentDetailsId, string AddFlag)
        {
            using (var context = new GS247Entities8())
            {
                collection.CreatedBy = Session["UserName"].ToString();
                collection.CreatedDate = DateTime.Now;
                context.StudentInstutionDetails.Add(collection);
                context.SaveChanges();
                ViewBag.StudentDetailsId = collection.StudentDetailsId;

                return RedirectToAction("OnlineCreateStudentDocument", new RouteValueDictionary(
                   new { controller = "Student", action = "OnlineCreateStudentDocument", StudentDetailsId = collection.StudentDetailsId }));
            }
        }

        [HttpPost]
        public ActionResult OnlineDeleteStudentDocument(int StudentDocumentsId)
        {
            using (var context = new GS247Entities8())
            {
                var subscriptions = context.StudentDocuments.Find(StudentDocumentsId);
                if (subscriptions != null)
                {
                    context.StudentDocuments.Remove(subscriptions);
                    context.SaveChanges();
                }
            }
            return new EmptyResult();
        }

        public ActionResult OnlineCreateStudentDocument(int? StudentDetailsId)
        {
            if (sessionValidate())
            {
                var List = new List<StudentDocumentCO>();
                using (var context = new GS247Entities8())
                {
                    var FieldsList = context.StudentDocuments.Where(x => x.StudentDetailsId == StudentDetailsId).ToList();

                    var DocumentList = (from sdt in context.SettingsDocumentTypes.ToList()
                                        where !FieldsList.Any(x => x.DocumentTypeId == sdt.Id)
                                        select sdt).Distinct().ToList();

                    ViewBag.DocumentsList = DocumentList;

                    FieldsList.ForEach(data =>
                    {
                        var StudentDocument = new StudentDocumentCO();
                        StudentDocument.StudentDocumentsId = data.StudentDocumentsId;
                        StudentDocument.StudentDetailsId = data.StudentDetailsId;
                        StudentDocument.DocumentTypeId = data.DocumentTypeId;
                        StudentDocument.FileName = data.FileName;
                        StudentDocument.Data = data.Data;
                        StudentDocument.CreatedBy = data.CreatedBy;
                        StudentDocument.CreatedDate = data.CreatedDate;
                        StudentDocument.UpdatedBy = data.UpdatedBy;
                        StudentDocument.UpdatedDate = data.UpdatedDate;
                        StudentDocument.StatusFlag = data.StatusFlag;
                        var SchoolCO = new SchoolCO();
                        SchoolCO.DocumentName = context.SettingsDocumentTypes.Where(x => x.Id == data.DocumentTypeId).FirstOrDefault().DocumentName;
                        StudentDocument.SchoolCO = SchoolCO;
                        List.Add(StudentDocument);
                    });
                    ViewBag.StudentDocumentsList = List;
                    ViewBag.StudentDetailsId = StudentDetailsId;
                }
                return View();
            }
            else
                return RedirectToAction("Index", "Login");
        }

        [HttpPost]
        public ActionResult ProfileCreateStudentDocument(StudentDocument collection, int? StudentDetailsId, string AddFlag)
        {
            HttpPostedFileBase file = Request.Files["DataString"];
            string pathName = string.Empty;

            using (var context = new GS247Entities8())
            {
                collection.CreatedBy = Session["UserName"].ToString();
                collection.CreatedDate = DateTime.Now;

                if (file != null)
                {
                    string strExtension = Path.GetExtension(file.FileName);
                    pathName = "Student_Document_" + StudentDetailsId + "_" + DateTime.Now.ToString("MMddyyyyHHmmss") + strExtension;
                }
                collection.Data = pathName;
                context.StudentDocuments.Add(collection);
                context.SaveChanges();
                ViewBag.StudentDetailsId = collection.StudentDetailsId;
                if (file != null)
                    saveImagesUpload(file, pathName);
            }

            return RedirectToAction("ViewDocuments", new RouteValueDictionary(
              new { controller = "Student", action = "ViewDocuments", StudentDetailsId = collection.StudentDetailsId }));
        }

        [HttpPost]
        public ActionResult OnlineCreateStudentDocument(StudentDocument collection, int? StudentDetailsId, string AddFlag)
        {
            HttpPostedFileBase file = Request.Files["DataString"];
            string pathName = string.Empty;

            using (var context = new GS247Entities8())
            {
                collection.CreatedBy = Session["UserName"].ToString();
                collection.CreatedDate = DateTime.Now;

                if (file != null)
                {
                    string strExtension = Path.GetExtension(file.FileName);
                    pathName = "Student_Document_" + StudentDetailsId + "_" + DateTime.Now.ToString("MMddyyyyHHmmss") + strExtension;
                }
                collection.Data = pathName;
                context.StudentDocuments.Add(collection);
                context.SaveChanges();
                ViewBag.StudentDetailsId = collection.StudentDetailsId;
                if (file != null)
                    saveImagesUpload(file, pathName);
            }

            if (AddFlag == "1")
            {
                return RedirectToAction("OnlineApplicant", "Student");
            }
            else
            {
                return RedirectToAction("OnlineCreateStudentDocument", new RouteValueDictionary(
              new { controller = "Student", action = "OnlineCreateStudentDocument", StudentDetailsId = collection.StudentDetailsId }));
            }
        }

        public ActionResult OnlineCreateGuardian(int? StudentDetailsId)
        {
            if (sessionValidate())
            {
                var manageFieldsList = new List<StudentFieldSetting>();
                int id = Convert.ToInt32(Session["UserId"].ToString());
                using (var context = new GS247Entities8())
                {
                    var FieldsList = context.StudentFieldSettings.Where(x => x.UserID > 0).ToList();
                    ViewBag.managePersonalFieldsList = FieldsList.Where(x => x.TabSelection == 2 && x.TabSubSelection == 1).ToList();
                    ViewBag.manageContactFieldsList = FieldsList.Where(x => x.TabSelection == 2 && x.TabSubSelection == 2).ToList();
                    var GuardiansList = context.GuardianDetails.Where(x => x.StudentDetailsId == StudentDetailsId && (x.DeleteFlag == 0 || x.DeleteFlag == null)).ToList();
                    ViewBag.GuardiansList = GuardiansList;
                    ViewBag.StudentDetailsId = StudentDetailsId;
                    ViewBag.CountryList = context.countries.ToList();
                }
                return View();
            }
            else
                return RedirectToAction("Index", "Login");
        }

        [HttpPost]
        public ActionResult OnlineCreateGuardian(GuardianDetail collection, int? StudentDetailsId, string AddFlag)
        {
            using (var context = new GS247Entities8())
            {
                collection.CreatedBy = Session["UserName"].ToString();
                collection.CreatedDate = DateTime.Now;
                collection.DeleteFlag = 0;
                context.GuardianDetails.Add(collection);
                context.SaveChanges();
                ViewBag.StudentDetailsId = collection.StudentDetailsId;
            }

            return RedirectToAction("OnlineCreatePreviousDetail", new RouteValueDictionary(
                       new { controller = "Student", action = "OnlineCreatePreviousDetail", StudentDetailsId = collection.StudentDetailsId }));
        }

        [HttpPost]
        public ActionResult OnlineUpdateStudentStatus(int StudentDetailsId, int status,int? BatchId)
        {
            using (var context = new GS247Entities8())
            {
                var _templist = context.StudentDetails.Find(StudentDetailsId);
                if (_templist != null)
                {
                    if (status == 1)
                    {
                        _templist.Batch = BatchId;
                        _templist.OnlineApplicationStatus = 1;
                        var waitingData = context.StudentWaitingLists.Where(x => x.StudentDetailsId == StudentDetailsId && x.StarusFlag == 0).FirstOrDefault();
                        if (waitingData != null)
                        {
                            waitingData.StarusFlag = 1;
                            waitingData.UpdatedDate = DateTime.Now;
                            waitingData.UpdatedBy = Session["UserName"].ToString();
                            context.Entry(waitingData).State = EntityState.Modified;
                            context.SaveChanges();
                        }
                        _templist.StudentId = GetMaxId("StudentID", "StudentDetails");                        
                        SaveSeqTable("StudentID", "StudentDetails");
                        CreateUser(_templist);

                        var guardianDetails = context.GuardianDetails.Where(x => x.StudentDetailsId == _templist.StudentDetailsId).ToList();
                        guardianDetails.ForEach(item => {

                            item.ParentId = GetMaxId("ParentID", "GuardianDetails");
                            item.UpdatedDate = DateTime.Now;
                            item.UpdatedBy = Session["UserName"].ToString();
                            context.Entry(item).State = EntityState.Modified;
                            context.SaveChanges();
                            SaveSeqTable("ParentID", "GuardianDetails");
                            if(item.ParentUserFlag == 1)
                                CreateGuardianUser(item);
                        });
                    }                        
                    _templist.Status = status;
                    _templist.UpdatedDate = DateTime.Now;
                    _templist.UpdatedBy = Session["UserName"].ToString();
                    context.Entry(_templist).State = EntityState.Modified;
                    context.SaveChanges();                    
                }
            }
            return new EmptyResult();
        }

        [HttpGet]
        public ActionResult OnlineUpdateStudent(int? StudentDetailsId)
        {
            if (sessionValidate())
            {
                var managePersonalFieldsCOList = new List<StudentFieldSettingCO>();
                var manageContactCOFieldsList = new List<StudentFieldSettingCO>();

                int id = Convert.ToInt32(Session["UserId"].ToString());
                using (var context = new GS247Entities8())
                {
                    var FieldsList = context.StudentFieldSettings.Where(x => x.UserID > 0).ToList();
                    var _templist = context.StudentDetails.Find(StudentDetailsId);

                    var managePersonalFieldsList = FieldsList.Where(x => x.TabSelection == 1 && x.TabSubSelection == 1).ToList();

                    managePersonalFieldsList.ForEach(data =>
                    {
                        var newData = new StudentFieldSettingCO();
                        newData.StudentFieldSettingID = data.StudentFieldSettingID;
                        newData.Title = data.Title;
                        newData.Required = data.Required;
                        newData.FieldTypeFlag = data.FieldTypeFlag;
                        newData.CheckAllFlag = data.CheckAllFlag;
                        newData.AdminStudentRegistrationFormFlag = data.AdminStudentRegistrationFormFlag;
                        newData.OnlineAdmissionFormFlag = data.OnlineAdmissionFormFlag;
                        newData.ParentPortalFlag = data.ParentPortalFlag;
                        newData.StudentProfileFlag = data.StudentProfileFlag;
                        newData.StudentProfilePDFFlag = data.StudentProfilePDFFlag;
                        newData.StudentPortalFlag = data.StudentPortalFlag;
                        newData.TeacherPortalFlag = data.TeacherPortalFlag;
                        newData.TabSelection = data.TabSelection;
                        newData.TabSubSelection = data.TabSubSelection;
                        newData.UserID = data.UserID;
                        newData.Category = data.Category;
                        newData.StatusFlag = data.StatusFlag;
                        newData.OrderNumber = data.OrderNumber;
                        newData.ColumnMapping = data.ColumnMapping;

                        var SchoolCO = new SchoolCO();
                        if (data.ColumnMapping == "AdmissionNo")
                            SchoolCO.FieldValue = _templist.AdmissionNo;
                        else if (data.ColumnMapping == "AdmissionDate")
                            SchoolCO.FieldValue = _templist.AdmissionDate.ToString();
                        else if (data.ColumnMapping == "FirstName")
                            SchoolCO.FieldValue = _templist.FirstName;
                        else if (data.ColumnMapping == "LastName")
                            SchoolCO.FieldValue = _templist.LastName;
                        else if (data.ColumnMapping == "MiddleName")
                            SchoolCO.FieldValue = _templist.MiddleName;
                        else if (data.ColumnMapping == "StudentId")
                            SchoolCO.FieldValue = _templist.StudentId;
                        else if (data.ColumnMapping == "Course")
                            SchoolCO.FieldValue = _templist.Course != null ? _templist.Course.ToString() : "0";
                        else if (data.ColumnMapping == "Batch")
                            SchoolCO.FieldValue = _templist.Batch != null ? _templist.Batch.ToString() : "0";
                        else if (data.ColumnMapping == "DOB")
                        {
                            var dob = _templist.DOB.GetValueOrDefault().ToString("dd MMM yyyy");
                            SchoolCO.FieldValue = dob;
                        }
                        else if (data.ColumnMapping == "Gender")
                            SchoolCO.FieldValue = _templist.Gender.ToString();
                        else if (data.ColumnMapping == "Blood")
                            SchoolCO.FieldValue = _templist.Blood != null ? _templist.Blood.ToString() : "0";
                        else if (data.ColumnMapping == "BirthPlace")
                            SchoolCO.FieldValue = _templist.BirthPlace;
                        else if (data.ColumnMapping == "Language")
                            SchoolCO.FieldValue = _templist.Language;
                        else if (data.ColumnMapping == "Nationality")
                            SchoolCO.FieldValue = _templist.Nationality != null ? _templist.Nationality.ToString() : "0";
                        else if (data.ColumnMapping == "Religion")
                            SchoolCO.FieldValue = _templist.Religion;
                        else if (data.ColumnMapping == "StudentCategory")
                            SchoolCO.FieldValue = _templist.StudentCategory != null ? _templist.StudentCategory.ToString() : "0";
                        else if (data.ColumnMapping == "PromitionalFlag")
                            SchoolCO.FieldValue = _templist.PromitionalFlag.ToString();

                        newData.SchoolCO = SchoolCO;
                        managePersonalFieldsCOList.Add(newData);
                    });

                    ViewBag.managePersonalFieldsList = managePersonalFieldsCOList;

                    var manageContactFieldsList = FieldsList.Where(x => x.TabSelection == 1 && x.TabSubSelection == 2).ToList();

                    manageContactFieldsList.ForEach(data =>
                    {
                        var newData = new StudentFieldSettingCO();
                        newData.StudentFieldSettingID = data.StudentFieldSettingID;
                        newData.Title = data.Title;
                        newData.Required = data.Required;
                        newData.FieldTypeFlag = data.FieldTypeFlag;
                        newData.CheckAllFlag = data.CheckAllFlag;
                        newData.AdminStudentRegistrationFormFlag = data.AdminStudentRegistrationFormFlag;
                        newData.OnlineAdmissionFormFlag = data.OnlineAdmissionFormFlag;
                        newData.ParentPortalFlag = data.ParentPortalFlag;
                        newData.StudentProfileFlag = data.StudentProfileFlag;
                        newData.StudentProfilePDFFlag = data.StudentProfilePDFFlag;
                        newData.StudentPortalFlag = data.StudentPortalFlag;
                        newData.TeacherPortalFlag = data.TeacherPortalFlag;
                        newData.TabSelection = data.TabSelection;
                        newData.TabSubSelection = data.TabSubSelection;
                        newData.UserID = data.UserID;
                        newData.Category = data.Category;
                        newData.StatusFlag = data.StatusFlag;
                        newData.OrderNumber = data.OrderNumber;
                        newData.ColumnMapping = data.ColumnMapping;

                        var SchoolCO = new SchoolCO();
                        if (data.ColumnMapping == "AddressLine1")
                            SchoolCO.FieldValue = _templist.AddressLine1;
                        else if (data.ColumnMapping == "AddressLine2")
                            SchoolCO.FieldValue = _templist.AddressLine2;
                        else if (data.ColumnMapping == "City")
                            SchoolCO.FieldValue = _templist.City;
                        else if (data.ColumnMapping == "State")
                            SchoolCO.FieldValue = _templist.State;
                        else if (data.ColumnMapping == "PinCode")
                            SchoolCO.FieldValue = _templist.PinCode;
                        else if (data.ColumnMapping == "PhoneNumber1")
                            SchoolCO.FieldValue = _templist.PhoneNumber1;
                        else if (data.ColumnMapping == "PhoneNumber2")
                            SchoolCO.FieldValue = _templist.PhoneNumber2;
                        else if (data.ColumnMapping == "Email")
                            SchoolCO.FieldValue = _templist.Email;

                        newData.SchoolCO = SchoolCO;
                        manageContactCOFieldsList.Add(newData);

                    });

                    ViewBag.manageContactFieldsList = manageContactCOFieldsList;
                    ViewBag.FileName = _templist.FileName;
                    ViewBag.AdmissionNo = _templist.AdmissionNo;
                    ViewBag.AdmissionDate = _templist.AdmissionDate;
                    ViewBag.StudentDetailsId = _templist.StudentDetailsId;
                    ViewBag.CourseList = context.Courses.ToList();
                    ViewBag.BatchList = (from cbl in context.BatchCourseLinks
                                 join b in context.batches on cbl.BatchId equals b.Id
                                         where cbl.CourseId == _templist.Course
                                         select b).ToList();                    
                    ViewBag.StudentCategoryList = context.StudentCategories.ToList();
                    ViewBag.CountryList = context.countries.ToList();
                    return View("OnlineUpdateStudent", _templist);
                }
            }
            else
                return RedirectToAction("Index", "Login");
        }

        [HttpPost]
        public ActionResult OnlineUpdateStudent(StudentDetail collection)
        {
            if (sessionValidate())
            {
                using (var context = new GS247Entities8())
                {
                    var _templist = context.StudentDetails.Find(collection.StudentDetailsId);
                    if (_templist != null)
                    {
                        _templist.FirstName = collection.FirstName;
                        _templist.MiddleName = collection.MiddleName;
                        _templist.LastName = collection.LastName;
                        _templist.StudentId = collection.StudentId;
                        _templist.AdmissionDate = collection.AdmissionDate;
                        _templist.AdmissionNo = collection.AdmissionNo;
                        _templist.Batch = collection.Batch;
                        _templist.Course = collection.Course;
                        _templist.DOB = collection.DOB;
                        _templist.Gender = collection.Gender;
                        _templist.Blood = collection.Blood;
                        _templist.BirthPlace = collection.BirthPlace;
                        _templist.Language = collection.Language;
                        _templist.Nationality = collection.Nationality;
                        _templist.Religion = collection.Religion;
                        _templist.StudentCategory = collection.StudentCategory;
                        _templist.PromitionalFlag = collection.PromitionalFlag;
                        _templist.AddressLine1 = collection.AddressLine1;
                        _templist.AddressLine2 = collection.AddressLine2;
                        _templist.City = collection.City;
                        _templist.State = collection.State;
                        _templist.PinCode = collection.PinCode;
                        _templist.PhoneNumber1 = collection.PhoneNumber1;
                        _templist.PhoneNumber2 = collection.PhoneNumber2;
                        _templist.Email = collection.Email;
                        _templist.DeleteFlag = 0;
                        if (!string.IsNullOrEmpty(collection.Data))
                        {
                            _templist.Data = collection.Data;
                        }
                        else if (collection.FileName == _templist.FileName)
                            _templist.Data = _templist.Data;
                        else
                            _templist.Data = collection.Data;
                        _templist.FileName = collection.FileName;
                        _templist.UpdatedDate = DateTime.Now;
                        _templist.UpdatedBy = Session["UserName"].ToString();
                        context.Entry(_templist).State = EntityState.Modified;
                        context.SaveChanges();
                    }
                }
                return RedirectToAction("OnlineCreateGuardian", new RouteValueDictionary(
                       new { controller = "Student", action = "OnlineCreateGuardian", StudentDetailsId = collection.StudentDetailsId }));
            }
            else
                return RedirectToAction("Index", "Login");
        }

        [HttpPost]
        public ActionResult OnlineCreateStudentWaiting(StudentWaitingList collection)
        {
            if (sessionValidate())
            {
                using (var context = new GS247Entities8())
                {
                    var templist = context.StudentWaitingLists.Where(x => x.StudentDetailsId == collection.StudentDetailsId).FirstOrDefault();
                    if (templist == null)
                    {
                        collection.CreatedBy = Session["UserName"].ToString();
                        collection.CreatedDate = DateTime.Now;
                        collection.StarusFlag = 0;
                        context.StudentWaitingLists.Add(collection);
                        context.SaveChanges();                        
                    }
                    else
                    {
                        templist.Priority = collection.Priority;
                        templist.BatchId = collection.BatchId;
                        templist.UpdatedDate = DateTime.Now;
                        templist.UpdatedBy = Session["UserName"].ToString();
                        context.Entry(templist).State = EntityState.Modified;
                        context.SaveChanges();
                    }

                    var _templist = context.StudentDetails.Find(collection.StudentDetailsId);
                    if (_templist != null)
                    {
                        _templist.Batch = collection.BatchId;
                        _templist.OnlineApplicationStatus = 0;
                        _templist.Status = 2;
                        _templist.UpdatedDate = DateTime.Now;
                        _templist.UpdatedBy = Session["UserName"].ToString();
                        context.Entry(_templist).State = EntityState.Modified;
                        context.SaveChanges();
                    }
                }
                return new EmptyResult();
            }
            else
                return RedirectToAction("Index", "Login");
        }

        [HttpPost]
        public JsonResult EditStudentWaiting(int StudentDetailsId)
        {
            var waitingData = new StudentWaitingList();
            using (var context = new GS247Entities8())
            {
                waitingData = context.StudentWaitingLists.Where(x => x.StudentDetailsId == StudentDetailsId && x.StarusFlag == 0).FirstOrDefault();
            }
            return Json(new { WaitingData = waitingData }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region "COMMON"

        public bool sessionValidate()
        {
            if (Session["UserId"] != null)
            {
                if (Convert.ToInt32(Session["UserId"]) != 0)
                    return true;
                else
                    return false;
            }
            else
                return false;
        }

        public partial class StudentDetailCO
        {  
            public int StudentDetailsId { get; set; }
            public string AdmissionNo { get; set; }
            public Nullable<System.DateTime> AdmissionDate { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string MiddleName { get; set; }
            public string StudentId { get; set; }
            public Nullable<int> Batch { get; set; }
            public Nullable<System.DateTime> DOB { get; set; }
            public Nullable<int> Gender { get; set; }
            public Nullable<int> Blood { get; set; }
            public string BirthPlace { get; set; }
            public string Language { get; set; }
            public Nullable<int> Nationality { get; set; }
            public string Religion { get; set; }
            public Nullable<int> StudentCategory { get; set; }
            public Nullable<int> PromitionalFlag { get; set; }
            public string AddressLine1 { get; set; }
            public string AddressLine2 { get; set; }
            public string City { get; set; }
            public string State { get; set; }
            public string PinCode { get; set; }
            public string PhoneNumber1 { get; set; }
            public string PhoneNumber2 { get; set; }
            public string Email { get; set; }
            public string FileName { get; set; }
            public string Data { get; set; }
            public string CreatedBy { get; set; }
            public System.DateTime CreatedDate { get; set; }
            public string UpdatedBy { get; set; }
            public Nullable<System.DateTime> UpdatedDate { get; set; }
            public Nullable<int> Country { get; set; }
            public Nullable<int> DeleteFlag { get; set; }
            public string RollNumber { get; set; }
            public string Semester { get; set; }
            public string AcademicYear { get; set; }
            public Nullable<int> AcademicStatus { get; set; }
            public Nullable<int> Status { get; set; }
            public Nullable<int> OnlineStatus { get; set; }
            public Nullable<int> OnlineApplicationStatus { get; set; }

            public virtual SchoolCO SchoolCO { get; set; }
        }
        
        public partial class StudentCategoryCO
        {
            public int StudentCategoryID { get; set; }
            public Nullable<int> UserId { get; set; }
            public string Name { get; set; }
            public string CreatedBy { get; set; }
            public System.DateTime CreatedDate { get; set; }
            public string UpdatedBy { get; set; }
            public Nullable<System.DateTime> UpdatedDate { get; set; }
            public virtual SchoolCO SchoolCO { get; set; }
        }

        public partial class GuardianDetailCO
        {
            public int GuardianDetailsId { get; set; }
            public Nullable<int> StudentDetailsId { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public Nullable<int> Relation { get; set; }
            public Nullable<System.DateTime> DOB { get; set; }
            public string Education { get; set; }
            public string Occupation { get; set; }
            public string Income { get; set; }
            public string AddressLine1 { get; set; }
            public string AddressLine2 { get; set; }
            public string City { get; set; }
            public string State { get; set; }
            public string PinCode { get; set; }
            public string MobilePhone { get; set; }
            public string OfficePhone1 { get; set; }
            public string OfficePhone2 { get; set; }
            public string Email { get; set; }
            public Nullable<int> Country { get; set; }
            public Nullable<int> ParentUserFlag { get; set; }
            public string CreatedBy { get; set; }
            public System.DateTime CreatedDate { get; set; }
            public string UpdatedBy { get; set; }
            public Nullable<System.DateTime> UpdatedDate { get; set; }
            public Nullable<int> DeleteFlag { get; set; }

            public virtual SchoolCO SchoolCO { get; set; }
        }

        public partial class StudentWaitingListCO
        {
            public int StudentWaitingId { get; set; }
            public Nullable<int> StudentDetailsId { get; set; }
            public Nullable<int> BatchId { get; set; }
            public Nullable<int> StarusFlag { get; set; }
            public Nullable<int> Priority { get; set; }
            public string CreatedBy { get; set; }
            public System.DateTime CreatedDate { get; set; }
            public string UpdatedBy { get; set; }
            public Nullable<System.DateTime> UpdatedDate { get; set; }
            public virtual SchoolCO SchoolCO { get; set; }
        }

        public partial class StudentDocumentCO
        {
            public int StudentDocumentsId { get; set; }
            public Nullable<int> StudentDetailsId { get; set; }
            public Nullable<int> DocumentTypeId { get; set; }
            public string FileName { get; set; }
            public string Data { get; set; }
            public string CreatedBy { get; set; }
            public System.DateTime CreatedDate { get; set; }
            public string UpdatedBy { get; set; }
            public Nullable<System.DateTime> UpdatedDate { get; set; }
            public Nullable<int> StatusFlag { get; set; }
            public virtual SchoolCO SchoolCO { get; set; }
        }

        public partial class StudentFieldSettingCO
        {
            public int StudentFieldSettingID { get; set; }
            public string Title { get; set; }
            public Nullable<int> Required { get; set; }
            public Nullable<int> FieldTypeFlag { get; set; }
            public Nullable<int> CheckAllFlag { get; set; }
            public Nullable<int> AdminStudentRegistrationFormFlag { get; set; }
            public Nullable<int> OnlineAdmissionFormFlag { get; set; }
            public Nullable<int> ParentPortalFlag { get; set; }
            public Nullable<int> StudentProfileFlag { get; set; }
            public Nullable<int> StudentProfilePDFFlag { get; set; }
            public Nullable<int> StudentPortalFlag { get; set; }
            public Nullable<int> TeacherPortalFlag { get; set; }
            public Nullable<int> TabSelection { get; set; }
            public Nullable<int> TabSubSelection { get; set; }
            public string CreatedBy { get; set; }
            public System.DateTime CreatedDate { get; set; }
            public string UpdatedBy { get; set; }
            public Nullable<System.DateTime> UpdatedDate { get; set; }
            public Nullable<int> UserID { get; set; }
            public string Category { get; set; }
            public Nullable<int> StatusFlag { get; set; }
            public Nullable<int> OrderNumber { get; set; }
            public string ColumnMapping { get; set; }
            public virtual SchoolCO SchoolCO { get; set; }
        }

        public partial class StudentLogCO
        {
            public int StudentLogID { get; set; }
            public Nullable<int> StudentDetailsId { get; set; }
            public Nullable<int> StudentLogCategoryID { get; set; }
            public string Description { get; set; }
            public Nullable<int> NotificationToStudent { get; set; }
            public Nullable<int> NotificationToParent { get; set; }
            public Nullable<int> VisibleToStudents { get; set; }
            public Nullable<int> VisibleToParents { get; set; }
            public Nullable<int> VisibleToTeachers { get; set; }
            public string CreatedBy { get; set; }
            public System.DateTime CreatedDate { get; set; }
            public string UpdatedBy { get; set; }
            public Nullable<System.DateTime> UpdatedDate { get; set; }

            public virtual SchoolCO SchoolCO { get; set; }
        }

        public partial class StudentExamDetailCO
        {
            public int StudentExamDetailsId { get; set; }
            public Nullable<int> ExamId { get; set; }
            public Nullable<int> StudentDetailsId { get; set; }
            public Nullable<System.DateTime> StartTime { get; set; }
            public Nullable<System.DateTime> EndTime { get; set; }
            public string LoginId { get; set; }
            public string TotalMark { get; set; }
            public string CreatedBy { get; set; }
            public System.DateTime CreatedDate { get; set; }
            public string UpdatedBy { get; set; }
            public Nullable<System.DateTime> UpdatedDate { get; set; }
            public string Grade { get; set; }
            public Nullable<int> StatusFlag { get; set; }
            public Nullable<int> MarkType { get; set; }
            public string StudentName { get; set; }
            public string AdmissionNo { get; set; }

            public string CourseName { get; set; }

            public string BatchName { get; set; }

            public string SubjectName { get; set; }

            public string ExamName { get; set; }
        }

        #endregion
    }
}