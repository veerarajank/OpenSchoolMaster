using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GS247.Models;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Net.Mail;
using System.Collections;

namespace GS247.Controllers
{
    public class FeesController : Controller
    {

        #region "INDEX"

        public ActionResult Index()
        {
            if (sessionValidate())
            {
                var feeList = new List<Fee>();
                var feeCOList = new List<FeeCO>();
                int id = Convert.ToInt32(Session["UserId"].ToString());
                using (var context = new GS247Entities8())
                {
                    feeList = context.Fees.Where(x => x.UserId > 0 && x.PaymentTypeFlag == 0).ToList();

                    feeList.ForEach(data =>
                    {
                        var newData = new FeeCO();
                        newData.FeeID = data.FeeID;
                        newData.Name = data.Name;
                        newData.Description = data.Description;
                        newData.UserId = data.UserId;
                        newData.CreatedBy = data.CreatedBy;
                        newData.CreatedDate = data.CreatedDate;
                        newData.UpdatedBy = data.UpdatedBy;
                        newData.UpdatedDate = data.UpdatedDate;
                        newData.InvoiceStatusFlag = data.InvoiceStatusFlag;
                        newData.PaymentTypeFlag = data.PaymentTypeFlag;

                        var SchoolCO = new SchoolCO();
                        SchoolCO.SubscriptionFlag = context.FeeSubscriptions.Where(x => x.FeeID == data.FeeID).Count();
                        newData.SchoolCO = SchoolCO;
                        feeCOList.Add(newData);
                    });

                    ViewBag.feesDetails = feeCOList;
                    ViewBag.feesCount = feeList.Count;
                    ViewBag.InvoiceGenerateCount = feeList.Where(x => x.InvoiceStatusFlag == 1).Count();
                }
                return View();
            }
            else
                return RedirectToAction("Index", "Login");
        }

        #endregion

        #region "FEES"

        public ActionResult CreateFees()
        {
            if (sessionValidate())
            {
                var taxList = new List<Tax>();
                int id = Convert.ToInt32(Session["UserId"].ToString());
                using (var context = new GS247Entities8())
                {
                    taxList = context.Taxes.Where(x => x.UserId == id).ToList();
                    var configuration = context.FeesConfigurations.Where(x => x.UserId > 0).FirstOrDefault();
                    ViewBag.taxdetails = taxList;
                    ViewBag.feeConfiguration = configuration;
                    ViewBag.CoursesList = context.Courses.Where(x => x.IsEnable == true).ToList();
                    ViewBag.CategoryList = context.StudentCategories.ToList();
                    ViewBag.BatchList = context.batches.ToList();
                }
                return View();
            }
            else
                return RedirectToAction("Index", "Login");
        }

        public ActionResult FeesParticular(int? FeeID)
        {
            if (sessionValidate())
            {
                var feeParList = new List<FeeParticular>();
                var feeParCOList = new List<FeeParticularCO>();

                using (var context = new GS247Entities8())
                {
                    feeParList = context.FeeParticulars.Where(x => x.FeeID == FeeID).ToList();
                    feeParList.ForEach(data =>
                    {
                        var newData = new FeeParticularCO();
                        newData.FeeParticularID = data.FeeParticularID;
                        newData.Name = data.Name;
                        newData.Description = data.Description;
                        newData.Tax = data.Tax;
                        var taxAmount = context.Taxes.Find(data.Tax);
                        if (taxAmount != null && taxAmount.StatusFlag == 1)
                            newData.TaxName = taxAmount.TaxName;
                        newData.Discount = data.Discount;
                        newData.Type = data.Type;
                        newData.FeeID = data.FeeID;
                        newData.CreatedBy = data.CreatedBy;
                        newData.CreatedDate = data.CreatedDate;
                        newData.UpdatedBy = data.UpdatedBy;
                        newData.UpdatedDate = data.UpdatedDate;

                        var SchoolCO = new SchoolCO();
                        SchoolCO.FeeAccess = context.FeeAccesses.Where(x => x.FeeParticularID == data.FeeParticularID).ToList();
                        SchoolCO.FeeAccess.ForEach(itr =>
                        {
                            var SchoolCO1 = new SchoolCO();
                            if (itr.Type == 1)
                            {
                                var batch = context.batches.Find(itr.BatchID);
                                SchoolCO1.BatchName = batch != null ? "Batch : " + batch.Name : "Batch : All Batches";
                                var course = context.Courses.Find(itr.CourseID);
                                SchoolCO1.CourseName = course != null ? "Course : " + course.Name : "Course : All Courses";
                                var category = context.Categories.Find(itr.CategoryID);
                                SchoolCO1.FeeCategoryName = category != null ? "Student Category  : " + category.CategoryName : "Student Category : All Categories";
                            }
                            itr.SchoolCO = SchoolCO1;
                        });

                        newData.SchoolCO = SchoolCO;
                        feeParCOList.Add(newData);

                    });
                    ViewBag.FeesParticular = feeParCOList;
                }
                return View();
            }
            else
                return RedirectToAction("Index", "Login");
        }

        [HttpPost]
        public ActionResult DeleteFees(int FeeID)
        {
            using (var context = new GS247Entities8())
            {
                var subscriptions = context.FeeSubscriptions.Where(x => x.FeeID == FeeID).FirstOrDefault();

                if (subscriptions != null)
                {
                    context.FeeSubscriptions.Remove(subscriptions);
                    context.SaveChanges();
                }

                var FeeAccesses = context.FeeAccesses.Where(x => x.FeeID == FeeID).ToList();

                FeeAccesses.ForEach(data =>
                {

                    context.FeeAccesses.Remove(data);
                    context.SaveChanges();

                });

                var FeeParticulars = context.FeeParticulars.Where(x => x.FeeID == FeeID).ToList();

                FeeParticulars.ForEach(data =>
                {

                    context.FeeParticulars.Remove(data);
                    context.SaveChanges();

                });


                var _templist = context.Fees.Find(FeeID);
                if (_templist != null)
                {
                    context.Fees.Remove(_templist);
                    context.SaveChanges();
                }
            }
            return new EmptyResult();
        }

        [HttpGet]
        public ActionResult EditFees(int FeeID)
        {
            if (sessionValidate())
            {
                int id = Convert.ToInt32(Session["UserId"].ToString());
                using (var context = new GS247Entities8())
                {
                    var _templist = context.Fees.Find(FeeID);

                    var SchoolCO1 = new SchoolCO();
                    SchoolCO1.FeeParticulars = context.FeeParticulars.Where(x => x.FeeID == _templist.FeeID).ToList();

                    _templist.SchoolCO = SchoolCO1;

                    _templist.SchoolCO.FeeParticulars.ForEach(item =>
                    {
                        var SchoolCO = new SchoolCO();
                        SchoolCO.FeeAccess = context.FeeAccesses.Where(x => x.FeeParticularID == item.FeeParticularID).ToList();
                        item.SchoolCO = SchoolCO;
                    });

                    var taxList = context.Taxes.Where(x => x.UserId > 0).ToList();
                    ViewBag.taxdetails = taxList;
                    var configuration = context.FeesConfigurations.Where(x => x.UserId > 0).FirstOrDefault();
                    ViewBag.feeConfiguration = configuration;
                    ViewBag.CoursesList = context.Courses.Where(x => x.IsEnable == true).ToList();
                    ViewBag.CategoryList = context.StudentCategories.ToList();
                    ViewBag.BatchList = context.batches.ToList();
                    return View("EditFees", _templist);
                }
            }
            else
                return RedirectToAction("Index", "Login");
        }

        [HttpPost]
        public ActionResult UpdateFees(Fee fee)
        {
            using (var context = new GS247Entities8())
            {
                var _templist = context.Fees.Find(fee.FeeID);
                if (_templist != null)
                {
                    _templist.Name = fee.Name;
                    _templist.Description = fee.Description;
                    _templist.UpdatedDate = DateTime.Now;
                    _templist.UpdatedBy = Session["UserName"].ToString();
                    context.Entry(_templist).State = EntityState.Modified;
                    context.SaveChanges();
                    TempData["Fees"] = _templist;
                }
            }
            return RedirectToAction("CreateSubscription", fee.FeeID);
        }

        public JsonResult CreateFeesTemplate(List<feeParticularList> feeList, string Name, string Description, int FeeID)
        {
            if (FeeID == 0)
            {
                using (var context = new GS247Entities8())
                {
                    var fee = new Fee();
                    fee.Name = Name;
                    fee.Description = Description;
                    fee.UserId = Convert.ToInt16(Session["UserId"]);
                    fee.CreatedBy = Session["UserName"].ToString();
                    fee.CreatedDate = DateTime.Now;
                    fee.PaymentTypeFlag = 0;
                    context.Fees.Add(fee);
                    context.SaveChanges();
                    FeeID = fee.FeeID;
                    feeList.ForEach(data =>
                    {
                        var feeParticular = new FeeParticular();
                        feeParticular.FeeID = fee.FeeID;
                        feeParticular.Name = data.Name;
                        feeParticular.Description = data.Description;
                        feeParticular.Tax = (!(string.IsNullOrEmpty(data.Tax)) ? Convert.ToInt32(data.Tax) : (int?)null);
                        feeParticular.Discount = (data.Discount != "" ? data.Discount : "0.00");
                        feeParticular.Type = (!(string.IsNullOrEmpty(data.Type)) ? Convert.ToInt32(data.Type) : (int?)null);
                        feeParticular.CreatedBy = Session["UserName"].ToString();
                        feeParticular.CreatedDate = DateTime.Now;
                        context.FeeParticulars.Add(feeParticular);
                        context.SaveChanges();

                        data.feeAccessList.ForEach(item =>
                        {
                            var feeAccess = new FeeAccess();
                            feeAccess.FeeParticularID = feeParticular.FeeParticularID;
                            feeAccess.Type = (!(string.IsNullOrEmpty(item.Type)) ? Convert.ToInt32(item.Type) : (int?)null);
                            feeAccess.AdmissionNumbers = item.AdmissionNumbers;
                            feeAccess.CourseID = (!(string.IsNullOrEmpty(item.CourseID)) ? Convert.ToInt32(item.CourseID) : (int?)null);
                            feeAccess.BatchID = (!(string.IsNullOrEmpty(item.BatchID)) ? Convert.ToInt32(item.BatchID) : (int?)null);
                            feeAccess.CategoryID = (!(string.IsNullOrEmpty(item.CategoryID)) ? Convert.ToInt32(item.CategoryID) : (int?)null);
                            feeAccess.Amount = (item.Amount != "" ? item.Amount : "0.00");
                            feeAccess.FeeID = fee.FeeID;
                            feeAccess.CreatedBy = Session["UserName"].ToString();
                            feeAccess.CreatedDate = DateTime.Now;
                            context.FeeAccesses.Add(feeAccess);
                            context.SaveChanges();

                        });

                    });

                    TempData["Fees"] = fee;
                }
            }
            else
            {
                using (var context = new GS247Entities8())
                {
                    var _templist = context.Fees.Find(FeeID);
                    if (_templist != null)
                    {
                        _templist.Name = Name;
                        _templist.Description = Description;
                        _templist.UpdatedDate = DateTime.Now;
                        _templist.UpdatedBy = Session["UserName"].ToString();
                        context.Entry(_templist).State = EntityState.Modified;
                        context.SaveChanges();
                        TempData["Fees"] = _templist;
                    }

                    var FeeAccesses = context.FeeAccesses.Where(x => x.FeeID == FeeID).ToList();

                    FeeAccesses.ForEach(data =>
                    {
                        context.FeeAccesses.Remove(data);
                        context.SaveChanges();
                    });

                    var FeeParticulars = context.FeeParticulars.Where(x => x.FeeID == FeeID).ToList();

                    FeeParticulars.ForEach(data =>
                    {
                        context.FeeParticulars.Remove(data);
                        context.SaveChanges();

                    });

                    feeList.ForEach(data =>
                    {
                        var feeParticular = new FeeParticular();
                        feeParticular.FeeID = FeeID;
                        feeParticular.Name = data.Name;
                        feeParticular.Description = data.Description;
                        feeParticular.Tax = (!(string.IsNullOrEmpty(data.Tax)) ? Convert.ToInt32(data.Tax) : (int?)null);
                        feeParticular.Discount = (data.Discount != "" ? data.Discount : "0.00");
                        feeParticular.Type = (!(string.IsNullOrEmpty(data.Type)) ? Convert.ToInt32(data.Type) : (int?)null);
                        feeParticular.CreatedBy = Session["UserName"].ToString();
                        feeParticular.CreatedDate = DateTime.Now;
                        context.FeeParticulars.Add(feeParticular);
                        context.SaveChanges();

                        data.feeAccessList.ForEach(item =>
                        {

                            var feeAccess = new FeeAccess();
                            feeAccess.FeeParticularID = feeParticular.FeeParticularID;
                            feeAccess.Type = (!(string.IsNullOrEmpty(item.Type)) ? Convert.ToInt32(item.Type) : (int?)null);
                            feeAccess.AdmissionNumbers = item.AdmissionNumbers;
                            feeAccess.CourseID = (!(string.IsNullOrEmpty(item.CourseID)) ? Convert.ToInt32(item.CourseID) : (int?)null);
                            feeAccess.BatchID = (!(string.IsNullOrEmpty(item.BatchID)) ? Convert.ToInt32(item.BatchID) : (int?)null);
                            feeAccess.CategoryID = (!(string.IsNullOrEmpty(item.CategoryID)) ? Convert.ToInt32(item.CategoryID) : (int?)null);
                            feeAccess.Amount = (item.Amount != "" ? item.Amount : "0.00");
                            feeAccess.FeeID = FeeID;
                            feeAccess.CreatedBy = Session["UserName"].ToString();
                            feeAccess.CreatedDate = DateTime.Now;
                            context.FeeAccesses.Add(feeAccess);
                            context.SaveChanges();

                        });

                    });
                }
            }

            return Json(new { feeID = FeeID });
        }

        public class feeParticularList
        {
            public string Name { get; set; }
            public string Description { get; set; }
            public string Tax { get; set; }
            public string Discount { get; set; }
            public string Type { get; set; }

            public List<feeAccessList> feeAccessList { get; set; }
        }

        public class feeAccessList
        {
            public string Type { get; set; }
            public string AdmissionNumbers { get; set; }
            public string CourseID { get; set; }
            public string BatchID { get; set; }
            public string CategoryID { get; set; }
            public string Amount { get; set; }

        }

        #endregion

        #region "INVOICE"

        public ActionResult ManageInvoices(int? FeeID)
        {
            if (sessionValidate())
            {
                using (var context = new GS247Entities8())
                {
                    ViewBag.TotalInvoices = context.StudentFeesInvoices.Count();
                    ViewBag.UnpaidInvoices = context.StudentFeesInvoices.Where(x => x.PaidStatusFlag == 0).Count();
                    ViewBag.PaidInvoices = context.StudentFeesInvoices.Where(x => x.PaidStatusFlag == 1).Count();
                    ViewBag.CancelledInvoices = context.StudentFeesInvoices.Where(x => x.PaidStatusFlag == 2).Count();
                    ViewBag.CoursesList = context.Courses.Where(x => x.IsEnable == true).ToList();
                    ViewBag.BatchList = context.batches.ToList();
                    ViewBag.FeesList = context.Fees.ToList();
                    ViewBag.FeeID = FeeID;
                }
                return View();
            }
            else
                return RedirectToAction("Index", "Login");
        }

        [HttpPost]
        public JsonResult GetStudentList(string currentPageIndex, string FeeID, StudentAdvanceSearch studentAdvanceSearch)
        {
            List<StudentFeesInvoice> studentInvoiceList = new List<StudentFeesInvoice>();
            List<StudentFeesInvoiceCO> studentInvoiceCOList = new List<StudentFeesInvoiceCO>();


            var totalPages = 0;
            using (var context = new GS247Entities8())
            {
                IQueryable<StudentFeesInvoice> studentInvoiceData = context.StudentFeesInvoices;

                if (!string.IsNullOrEmpty(FeeID))
                {
                    int feeid = Convert.ToInt32(FeeID);
                    studentInvoiceData = context.StudentFeesInvoices.Where(z => z.FeeId == feeid);
                }
                if (!string.IsNullOrEmpty(studentAdvanceSearch.FeeId))
                {
                    int feeid = Convert.ToInt32(studentAdvanceSearch.FeeId);
                    studentInvoiceData = studentInvoiceData.Where(z => z.FeeId == feeid);
                }
                if (!string.IsNullOrEmpty(studentAdvanceSearch.CourseId))
                {
                    int CourseId = Convert.ToInt32(studentAdvanceSearch.CourseId);
                    studentInvoiceData = studentInvoiceData.Where(z => z.CourseId == CourseId);
                }
                if (!string.IsNullOrEmpty(studentAdvanceSearch.BatchId))
                {
                    int BatchId = Convert.ToInt32(studentAdvanceSearch.BatchId);
                    studentInvoiceData = studentInvoiceData.Where(z => z.BatchId == BatchId);
                }
                if (!string.IsNullOrEmpty(studentAdvanceSearch.InvoiceId))
                {
                    int InvoiceId = Convert.ToInt32(studentAdvanceSearch.InvoiceId);
                    studentInvoiceData = studentInvoiceData.Where(z => z.InvoiceID == InvoiceId);
                }
                if (!string.IsNullOrEmpty(studentAdvanceSearch.StudentName))
                {
                    studentInvoiceData = (from si in studentInvoiceData
                                          join sm in context.StudentDetails on si.StudentDetailsId equals sm.StudentDetailsId
                                          where sm.FirstName.Contains(studentAdvanceSearch.StudentName) || sm.LastName.Contains(studentAdvanceSearch.StudentName)
                                          select si);
                }
                if (!string.IsNullOrEmpty(studentAdvanceSearch.Status))
                {
                    int Status = Convert.ToInt32(studentAdvanceSearch.Status);
                    studentInvoiceData = studentInvoiceData.Where(z => z.PaidStatusFlag == Status);
                }
                if (!string.IsNullOrEmpty(studentAdvanceSearch.InvoiceDate))
                {
                    DateTime InvoiceDate = Convert.ToDateTime(studentAdvanceSearch.InvoiceDate).Date;
                    studentInvoiceData = studentInvoiceData.Where(z => z.CreatedDate.Date == InvoiceDate);
                }

                int totalCount = studentInvoiceData.Count();
                totalPages = (int)Math.Ceiling((decimal)totalCount / (decimal)20);
                studentInvoiceList = studentInvoiceData.OrderByDescending(x => x.CreatedDate).Skip((Convert.ToInt16(currentPageIndex) - 1) * 20).Take(20).ToList();

                studentInvoiceList.ForEach(data =>
                {
                    var newData = new StudentFeesInvoiceCO();
                    newData.InvoiceID = data.InvoiceID;
                    newData.StudentDetailsId = data.StudentDetailsId;
                    newData.FeeId = data.FeeId;
                    newData.InvoiceAmount = data.InvoiceAmount;
                    newData.PaidStatusFlag = data.PaidStatusFlag;
                    newData.DueDate = data.DueDate;
                    newData.PaymentDate = data.PaymentDate;
                    newData.CreatedBy = data.CreatedBy;
                    newData.CreatedDate = data.CreatedDate;
                    newData.FeeAccessID = data.FeeAccessID;
                    newData.FeeParticularID = data.FeeParticularID;
                    newData.CancelFlag = data.CancelFlag;
                    newData.CourseId = data.CourseId;
                    newData.BatchId = data.BatchId;
                    newData.TotalAmount = data.TotalAmount;
                    newData.DiscountAmount = data.DiscountAmount;

                    var SchoolCO = new SchoolCO();
                    var student = context.StudentDetails.Find(data.StudentDetailsId);
                    SchoolCO.StudentName = student != null ? student.FirstName + " " + student.LastName : "";

                    var Fees = context.Fees.Find(data.FeeId);
                    SchoolCO.FeeCategoryName = Fees != null ? Fees.Name : "";

                    SchoolCO.StatusFlagDesc = data.PaidStatusFlag == 0 ? "Unpaid" : data.PaidStatusFlag == 1 ? "Paid" : "Cancelled";

                    SchoolCO.TransactionDateStr = data.DueDate != null ? data.DueDate.GetValueOrDefault().ToString("dd MMM yyyy") : "";

                    newData.SchoolCO = SchoolCO;

                    studentInvoiceCOList.Add(newData);

                });
            }
            return Json(new { studentInvoiceList = studentInvoiceCOList, TotalPages = totalPages }, JsonRequestBehavior.AllowGet);
        }

        public class StudentAdvanceSearch
        {
            public string FeeId { get; set; }

            public string InvoiceId { get; set; }

            public string StudentName { get; set; }

            public string InvoiceDate { get; set; }

            public string Status { get; set; }

            public string CourseId { get; set; }

            public string BatchId { get; set; }
        }

        [HttpPost]
        public async Task<ActionResult> GenerateInvoice(int FeeID)
        {
            var data1 = new List<StudentInvoiceData>();
            var data2 = new List<StudentInvoiceData>();
            var data3 = new List<StudentInvoiceData>();
            var invoiceList = new List<StudentFeesInvoice>();
            var invoiceList1 = new List<StudentFeesInvoice>();
            using (var context = new GS247Entities8())
            {
                var subscription = context.FeeSubscriptions.Where(x => x.FeeID == FeeID).FirstOrDefault();
                var _templist = context.Fees.Find(FeeID);
                if (_templist != null)
                {
                    _templist.InvoiceStatusFlag = 1;
                    _templist.UpdatedDate = DateTime.Now;
                    _templist.UpdatedBy = Session["UserName"].ToString();
                    context.Entry(_templist).State = EntityState.Modified;
                    context.SaveChanges();
                }

                var FeeParticularsList = context.FeeParticulars.Where(x => x.FeeID == FeeID).ToList();
                FeeParticularsList.ForEach(data =>
                {
                    var feeSccessList = context.FeeAccesses.Where(x => x.FeeParticularID == data.FeeParticularID).ToList();

                    data1 = new List<StudentInvoiceData>();
                    data2 = new List<StudentInvoiceData>();
                    data3 = new List<StudentInvoiceData>();

                    feeSccessList.ForEach(item =>
                    {
                        if (item.Type == 2)
                        {
                            var studentData = context.StudentDetails.Where(x => x.AdmissionNo == item.AdmissionNumbers).FirstOrDefault();
                            if (studentData != null)
                            {
                                var StudentInvoiceData = new StudentInvoiceData();
                                StudentInvoiceData.StudentDetailsId = studentData.StudentDetailsId;
                                StudentInvoiceData.Flag = 1;
                                StudentInvoiceData.InvoiceAmount = item.Amount;
                                StudentInvoiceData.Type = data.Type;
                                StudentInvoiceData.FeeAccessID = item.FeeAccessID;
                                StudentInvoiceData.FeeParticularID = data.FeeParticularID;
                                StudentInvoiceData.FeeID = FeeID;
                                StudentInvoiceData.Discount = data.Discount;
                                StudentInvoiceData.BatchId = studentData.Batch;
                                var cId = context.BatchCourseLinks.Where(x => x.BatchId == studentData.Batch).FirstOrDefault();
                                if (cId != null)
                                    StudentInvoiceData.CourseId = cId.CourseId;
                                else
                                    StudentInvoiceData.CourseId = null;
                                StudentInvoiceData.TaxId = data.Tax;
                                data1.Add(StudentInvoiceData);
                            }
                        }
                        else if (item.Type == 1 && item.CourseID == null && item.BatchID == null && item.CategoryID == null)
                        {
                            var course = (from c in context.Courses
                                          join cbl in context.BatchCourseLinks on c.Id equals cbl.CourseId
                                          join b in context.batches on cbl.BatchId equals b.Id
                                          join sm in context.StudentDetails on b.Id equals sm.Batch
                                          where sm.DeleteFlag == 0 && (sm.OnlineStatus == 0 || sm.OnlineApplicationStatus == 1)
                                          select new StudentInvoiceData
                                          {
                                              StudentDetailsId = sm.StudentDetailsId,
                                              InvoiceAmount = item.Amount,
                                              FeeID = FeeID,
                                              FeeAccessID = item.FeeAccessID,
                                              FeeParticularID = data.FeeParticularID,
                                              Type = data.Type,
                                              Flag = 3,
                                              Discount = data.Discount,
                                              BatchId = sm.Batch,
                                              CourseId = c.Id,
                                              TaxId = data.Tax
                                          }).ToList();
                            data2.AddRange(course);
                        }
                        else
                        {
                            var course = (from c in context.Courses
                                          join cbl in context.BatchCourseLinks on c.Id equals cbl.CourseId
                                          join b in context.batches on cbl.BatchId equals b.Id
                                          join sm in context.StudentDetails on b.Id equals sm.Batch
                                          join stc in context.StudentCategories on sm.StudentCategory equals stc.StudentCategoryID
                                          where sm.DeleteFlag == 0 && (item.CourseID == null ? c.Id != 0 : c.Id == item.CourseID)
                                          && (item.BatchID == null ? b.Id != 0 : b.Id == item.BatchID)
                                          && (item.CategoryID == null ? stc.StudentCategoryID != 0 : stc.StudentCategoryID == sm.StudentCategory)
                                          && (sm.OnlineStatus == 0 || sm.OnlineApplicationStatus == 1)
                                          select new StudentInvoiceData
                                          {
                                              StudentDetailsId = sm.StudentDetailsId,
                                              InvoiceAmount = item.Amount,
                                              FeeID = FeeID,
                                              FeeAccessID = item.FeeAccessID,
                                              FeeParticularID = data.FeeParticularID,
                                              Type = data.Type,
                                              Flag = 3,
                                              Discount = data.Discount,
                                              BatchId = sm.Batch,
                                              CourseId = c.Id,
                                              TaxId = data.Tax
                                          }).ToList();

                            data3.AddRange(course);
                        }
                    });

                    data1.AddRange(data2);
                    data1.AddRange(data3);
                    var dataList = data1.OrderBy(z => z.Flag).GroupBy(x => x.StudentDetailsId).Select(x => x.First()).ToList();

                    if (subscription.SubscriptionType == 1)
                    {
                        dataList.ForEach(it =>
                        {
                            var StudentFeesInvoice1 = new StudentFeesInvoice();
                            StudentFeesInvoice1.StudentDetailsId = it.StudentDetailsId;
                            StudentFeesInvoice1.FeeId = it.FeeID;
                            StudentFeesInvoice1.TotalAmount = it.InvoiceAmount;

                            var taxAmount = context.Taxes.Find(it.TaxId);
                            if (taxAmount != null && taxAmount.StatusFlag == 1)
                            {
                                StudentFeesInvoice1.TaxAmount = it.Type == 1 ? (Math.Round(((Convert.ToDecimal(taxAmount.Value) / 100) * Convert.ToDecimal(it.InvoiceAmount)), 2)).ToString("N2") : Convert.ToDecimal(taxAmount.Value).ToString("N2");
                            }
                            else
                            {
                                StudentFeesInvoice1.TaxAmount = "0.00";
                            }
                            StudentFeesInvoice1.DiscountAmount = it.Type == 1 ? (Math.Round(((Convert.ToDecimal(it.Discount) / 100) * Convert.ToDecimal(it.InvoiceAmount)), 2)).ToString("N2") : Convert.ToDecimal(it.Discount).ToString("N2");
                            StudentFeesInvoice1.InvoiceAmount = ((Convert.ToDecimal(it.InvoiceAmount) + Convert.ToDecimal(StudentFeesInvoice1.TaxAmount)) - Math.Round((Convert.ToDecimal(StudentFeesInvoice1.DiscountAmount)), 2)).ToString("N2");
                            StudentFeesInvoice1.PaidStatusFlag = 0;
                            StudentFeesInvoice1.DueDate = DateTime.Now;
                            StudentFeesInvoice1.CreatedDate = DateTime.Now;
                            StudentFeesInvoice1.CreatedBy = Session["UserName"].ToString();
                            StudentFeesInvoice1.FeeAccessID = it.FeeAccessID;
                            StudentFeesInvoice1.FeeParticularID = it.FeeParticularID;
                            StudentFeesInvoice1.CancelFlag = 0;
                            StudentFeesInvoice1.CourseId = it.CourseId;
                            StudentFeesInvoice1.BatchId = it.BatchId;
                            StudentFeesInvoice1.DueDate = subscription.DueDate1;
                            invoiceList.Add(StudentFeesInvoice1);
                        });
                    }
                    else
                    {
                        int m = 2;

                        if (subscription.RecurringInterval == 1)
                            m = 2;
                        else if (subscription.RecurringInterval == 2)
                            m = 4;
                        else if (subscription.RecurringInterval == 3)
                        {
                            m = (12 * (subscription.EndDate.Year - subscription.StartDate.Year)) + subscription.EndDate.Month - subscription.StartDate.Month;
                        }

                        dataList.ForEach(it =>
                        {
                            var StudentFeesInvoice1 = new StudentFeesInvoice();
                            StudentFeesInvoice1.StudentDetailsId = it.StudentDetailsId;
                            StudentFeesInvoice1.FeeId = it.FeeID;

                            var tempInvoiceAmount = it.InvoiceAmount; ;
                            if (subscription.NumberOfSubscription != null)
                            {
                                tempInvoiceAmount = (Convert.ToDecimal(tempInvoiceAmount) / m).ToString();
                            }
                            StudentFeesInvoice1.TotalAmount = tempInvoiceAmount;
                            var taxAmount = context.Taxes.Find(it.TaxId);
                            if (taxAmount != null && taxAmount.StatusFlag == 1)
                            {
                                StudentFeesInvoice1.TaxAmount = it.Type == 1 ? (Math.Round(((Convert.ToDecimal(taxAmount.Value) / 100) * Convert.ToDecimal(tempInvoiceAmount)), 2)).ToString("N2") : Convert.ToDecimal(taxAmount.Value).ToString("N2");
                            }
                            else
                            {
                                StudentFeesInvoice1.TaxAmount = "0.00";
                            }
                            StudentFeesInvoice1.DiscountAmount = it.Type == 1 ? (Math.Round(((Convert.ToDecimal(it.Discount) / 100) * Convert.ToDecimal(tempInvoiceAmount)), 2)).ToString("N2") : Convert.ToDecimal(it.Discount).ToString("N2");
                            StudentFeesInvoice1.InvoiceAmount = ((Convert.ToDecimal(tempInvoiceAmount) + Convert.ToDecimal(StudentFeesInvoice1.TaxAmount)) - Math.Round((Convert.ToDecimal(StudentFeesInvoice1.DiscountAmount)), 2)).ToString("N2");
                            StudentFeesInvoice1.PaidStatusFlag = 0;
                            StudentFeesInvoice1.DueDate = DateTime.Now;
                            StudentFeesInvoice1.CreatedDate = DateTime.Now;
                            StudentFeesInvoice1.CreatedBy = Session["UserName"].ToString();
                            StudentFeesInvoice1.FeeAccessID = it.FeeAccessID;
                            StudentFeesInvoice1.FeeParticularID = it.FeeParticularID;
                            StudentFeesInvoice1.CancelFlag = 0;
                            StudentFeesInvoice1.CourseId = it.CourseId;
                            StudentFeesInvoice1.BatchId = it.BatchId;
                            if (subscription.RecurringInterval == 3)
                            {
                                var date = new DateTime(subscription.StartDate.Year, subscription.StartDate.AddMonths(1).Month, subscription.DayOfMonth.GetValueOrDefault());
                                StudentFeesInvoice1.DueDate = date;
                            }                               
                            else
                                StudentFeesInvoice1.DueDate = subscription.DueDate2;                           
                            invoiceList.Add(StudentFeesInvoice1);
                        });
                    }
                });

                if(subscription.SubscriptionType == 1)
                {
                    var resultList = invoiceList.GroupBy(x => new { x.StudentDetailsId, x.FeeId }).Select(x => x.First()).ToList();

                    context.StudentFeesInvoices.AddRange(resultList);
                    await context.SaveChangesAsync();

                    var queryList = context.StudentFeesInvoices.Where(x => x.FeeId == FeeID).ToList();

                    queryList.ForEach(it =>
                    {
                        var query = invoiceList.Where(x => x.FeeId == it.FeeId && x.StudentDetailsId == it.StudentDetailsId).ToList();

                        if (query.Count > 0)
                        {
                            decimal invoiceamount = 0;
                            decimal totalamount = 0;
                            decimal discountamount = 0;
                            decimal taxamount = 0;

                            query.ForEach(itt =>
                            {
                                var studentInvoicePayment = new StudentInvoicePayment();
                                studentInvoicePayment.InvoiceId = it.InvoiceID;
                                studentInvoicePayment.FeeId = it.FeeId;
                                studentInvoicePayment.FeeParticularID = itt.FeeParticularID;
                                studentInvoicePayment.FeeAccessID = itt.FeeAccessID;
                                studentInvoicePayment.InvoicePaymentAmount = itt.InvoiceAmount;
                                studentInvoicePayment.TotalAmount = itt.TotalAmount;
                                studentInvoicePayment.TaxAmount = itt.TaxAmount;
                                studentInvoicePayment.DiscountAmount = itt.DiscountAmount;
                                studentInvoicePayment.CreatedDate = DateTime.Now;
                                studentInvoicePayment.CreatedBy = Session["UserName"].ToString();
                                context.StudentInvoicePayments.Add(studentInvoicePayment);
                                context.SaveChanges();
                                invoiceamount = invoiceamount + Convert.ToDecimal(itt.InvoiceAmount);
                                totalamount = totalamount + Convert.ToDecimal(itt.TotalAmount);
                                discountamount = discountamount + Convert.ToDecimal(itt.DiscountAmount);
                                taxamount = taxamount + Convert.ToDecimal(itt.TaxAmount);
                            });

                            it.InvoiceAmount = invoiceamount.ToString("N2");
                            it.TotalAmount = totalamount.ToString("N2");
                            it.DiscountAmount = discountamount.ToString("N2");
                            it.TaxAmount = taxamount.ToString("N2");
                            it.UpdatedDate = DateTime.Now;
                            it.UpdatedBy = Session["UserName"].ToString();
                            it.DueDate = subscription.DueDate1;
                            context.Entry(it).State = EntityState.Modified;
                            context.SaveChanges();
                        }
                    });
                }
                else
                {
                    var resultList = invoiceList.GroupBy(x => new { x.StudentDetailsId, x.FeeId }).Select(x => x.First()).ToList();
                    context.StudentFeesInvoices.AddRange(resultList);
                    await context.SaveChangesAsync(); 
                  
                    var queryList = context.StudentFeesInvoices.Where(x => x.FeeId == FeeID).ToList();

                    queryList.ForEach(it =>
                    {
                        int m = 2;
                        if (subscription.RecurringInterval == 1)
                            m = 2;
                        else if (subscription.RecurringInterval == 2)
                            m = 4;
                        else if (subscription.RecurringInterval == 3)
                        {
                            m = (12 * (subscription.EndDate.Year - subscription.StartDate.Year)) + subscription.EndDate.Month - subscription.StartDate.Month;
                        }

                        for(int i = 0; i < m - 1; i++)
                        {
                            var StudentFeesInvoice1 = new StudentFeesInvoice();
                            StudentFeesInvoice1.StudentDetailsId = it.StudentDetailsId;
                            StudentFeesInvoice1.FeeId = FeeID;
                            StudentFeesInvoice1.TotalAmount = it.TotalAmount;
                            StudentFeesInvoice1.TaxAmount = it.TaxAmount;
                            StudentFeesInvoice1.DiscountAmount = it.DiscountAmount;
                            StudentFeesInvoice1.InvoiceAmount = it.InvoiceAmount;
                            StudentFeesInvoice1.PaidStatusFlag = 0;
                            StudentFeesInvoice1.DueDate = DateTime.Now;
                            StudentFeesInvoice1.CreatedDate = DateTime.Now;
                            StudentFeesInvoice1.CreatedBy = Session["UserName"].ToString();
                            StudentFeesInvoice1.FeeAccessID = it.FeeAccessID;
                            StudentFeesInvoice1.FeeParticularID = it.FeeParticularID;
                            StudentFeesInvoice1.CancelFlag = 0;
                            StudentFeesInvoice1.CourseId = it.CourseId;
                            StudentFeesInvoice1.BatchId = it.BatchId;
                            if (subscription.RecurringInterval == 3)
                            {
                                var date = new DateTime(subscription.StartDate.Year, subscription.StartDate.AddMonths(i + 2).Month, subscription.DayOfMonth.GetValueOrDefault());
                                StudentFeesInvoice1.DueDate = date;
                            }
                            else
                            {
                                if (i == 0)
                                    StudentFeesInvoice1.DueDate = subscription.DueDate3;
                                else if (i == 1)
                                    StudentFeesInvoice1.DueDate = subscription.DueDate4;
                                else if (i == 2)
                                    StudentFeesInvoice1.DueDate = subscription.DueDate3;
                            }                           
                            invoiceList1.Add(StudentFeesInvoice1);
                        }                        
                    });

                    context.StudentFeesInvoices.AddRange(invoiceList1);
                    await context.SaveChangesAsync();

                    queryList = context.StudentFeesInvoices.Where(x => x.FeeId == FeeID).ToList();

                    queryList.ForEach(it =>
                    {
                        var query = invoiceList.Where(x => x.FeeId == it.FeeId && x.StudentDetailsId == it.StudentDetailsId).ToList();

                        query.ForEach(itt =>
                        {
                            var studentInvoicePayment = new StudentInvoicePayment();
                            studentInvoicePayment.InvoiceId = it.InvoiceID;
                            studentInvoicePayment.FeeId = it.FeeId;
                            studentInvoicePayment.FeeParticularID = itt.FeeParticularID;
                            studentInvoicePayment.FeeAccessID = itt.FeeAccessID;                           
                            studentInvoicePayment.InvoicePaymentAmount = itt.InvoiceAmount;
                            studentInvoicePayment.TotalAmount = itt.TotalAmount;
                            studentInvoicePayment.TaxAmount = itt.TaxAmount;
                            studentInvoicePayment.DiscountAmount = itt.DiscountAmount;
                            studentInvoicePayment.CreatedDate = DateTime.Now;
                            studentInvoicePayment.CreatedBy = Session["UserName"].ToString();
                            context.StudentInvoicePayments.Add(studentInvoicePayment);
                            context.SaveChanges();
                        });
                    });

                    queryList.ForEach(it =>
                    {
                        var query = context.StudentInvoicePayments.Where(x => x.InvoiceId == it.InvoiceID).ToList();
                        if(query.Count > 0)
                        {
                            it.InvoiceAmount = query.Sum(x => Convert.ToDecimal(x.InvoicePaymentAmount)).ToString("N2");
                            it.TotalAmount = query.Sum(x => Convert.ToDecimal(x.TotalAmount)).ToString("N2");
                            it.TaxAmount = query.Sum(x => Convert.ToDecimal(x.TaxAmount)).ToString("N2");
                            it.DiscountAmount = query.Sum(x => Convert.ToDecimal(x.DiscountAmount)).ToString("N2");
                        }                            
                       
                        it.UpdatedDate = DateTime.Now;
                        it.UpdatedBy = Session["UserName"].ToString();                        
                        context.Entry(it).State = EntityState.Modified;
                        context.SaveChanges();
                    });
                }
            }
            return new EmptyResult();
        }

        private void SemdEmail()
        {
            MailMessage mail = new MailMessage();
            SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

            mail.From = new MailAddress("");
            mail.To.Add("");
            mail.Subject = "Test Mail";
            mail.Body = "This is for testing SMTP mail from GMAIL";

            SmtpServer.Port = 587;
            SmtpServer.Credentials = new System.Net.NetworkCredential("", "");
            SmtpServer.EnableSsl = true;
            SmtpServer.UseDefaultCredentials = true;
            SmtpServer.Send(mail);
        }

        public class StudentInvoiceData
        {
            public int StudentDetailsId { get; set; }

            public int FeeID { get; set; }

            public int FeeParticularID { get; set; }

            public int FeeAccessID { get; set; }

            public int? Type { get; set; }

            public int Flag { get; set; }

            public string InvoiceAmount { get; set; }

            public string Discount { get; set; }

            public int? CourseId { get; set; }

            public int? BatchId { get; set; }

            public int? TaxId { get; set; }
        }

        public ActionResult ViewInvoice(int? InvoiceID)
        {
            if (sessionValidate())
            {
                StudentFeesInvoice _templist = new StudentFeesInvoice();
                using (var context = new GS247Entities8())
                {
                    _templist = context.StudentFeesInvoices.Find(InvoiceID);
                    var student = context.StudentDetails.Find(_templist.StudentDetailsId);
                    var SchoolCO = new SchoolCO();
                    SchoolCO.StudentName = student != null ? student.FirstName + " " + student.LastName : "";
                    var Fees = context.Fees.Find(_templist.FeeId);
                    SchoolCO.FeeCategoryName = Fees != null ? Fees.Name : "";
                    SchoolCO.StatusFlagDesc = _templist.PaidStatusFlag == 0 ? "Unpaid" : "Paid";

                    var data = context.StudentInvoicePayments.Where(x => x.InvoiceId == InvoiceID).ToList();
                    data.ForEach(it =>
                    {
                        var SchoolCO1 = new SchoolCO();
                        var feeParticular = context.FeeParticulars.Find(it.FeeParticularID);
                        if (feeParticular != null)
                        {
                            SchoolCO1.ParticularName = feeParticular.Name;
                            SchoolCO1.ParticularDesc = feeParticular.Description;
                            SchoolCO1.Discount = feeParticular.Discount + " " + (feeParticular.Type == 1 ? "%" : "INR");
                            var taxAmount = context.Taxes.Find(feeParticular.Tax);
                            if (taxAmount != null && taxAmount.StatusFlag == 1)
                            {
                                SchoolCO1.Tax = taxAmount.Value + " " + (feeParticular.Type == 1 ? "%" : "INR");
                            }
                            else
                                SchoolCO1.Tax = "0.00";
                            var FeeAccesses = context.FeeAccesses.Find(it.FeeAccessID);
                            SchoolCO1.UnitAmount = Convert.ToDecimal(it.TotalAmount).ToString("N2");
                        }

                        it.SchoolCO = SchoolCO1;
                    });

                    var transactionDate = context.StudentInvoiceTransactions.Where(x => x.InvoiceId == _templist.InvoiceID && x.StatusFlag == 0).ToList();
                    SchoolCO.InvoicePaidAmount = transactionDate.Count > 0 ? Math.Round(transactionDate.Sum(x => Convert.ToDecimal(x.InvoicePaymentAmount)), 2).ToString() : "0.00";

                    SchoolCO.InvoiceBalanceAmount = Math.Round((Convert.ToDecimal(_templist.InvoiceAmount) - Convert.ToDecimal(SchoolCO.InvoicePaidAmount)), 2).ToString();

                    SchoolCO.LastPaymentDate = transactionDate.Count > 0 ? transactionDate.Max(x => x.CreatedDate).ToString("dd MMM yyyy") : "";
                    SchoolCO.StudentInvoicePayment = data;

                    _templist.SchoolCO = SchoolCO;
                }
                return View(_templist);
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }

        [HttpPost]
        public JsonResult LoadTransaction(string InvoiceID)
        {
            List<StudentInvoiceTransaction> TransactionList = new List<StudentInvoiceTransaction>();
            List<StudentInvoiceTransactionCO> TransactionCOList = new List<StudentInvoiceTransactionCO>();
            using (var context = new GS247Entities8())
            {
                int id = Convert.ToInt32(InvoiceID);
                TransactionList = context.StudentInvoiceTransactions.Where(x => x.InvoiceId == id).ToList();

                TransactionList.ForEach(data =>
                {
                    var newData = new StudentInvoiceTransactionCO();
                    newData.TransactionID = data.TransactionID;
                    newData.InvoiceId = data.InvoiceId;
                    newData.TransactionDate = data.TransactionDate;
                    newData.TransactionType = data.TransactionType;
                    newData.TransactionNumber = data.TransactionNumber;
                    newData.Description = data.Description;
                    newData.InvoicePaymentAmount = data.InvoicePaymentAmount;
                    newData.StatusFlag = data.StatusFlag;
                    newData.proof = data.proof;
                    newData.FileName = data.FileName;
                    newData.CreatedBy = data.CreatedBy;
                    newData.CreatedDate = data.CreatedDate;

                    var SchoolCO = new SchoolCO();
                    data.Description = data.Description != null ? data.Description : string.Empty;
                    data.TransactionNumber = data.TransactionNumber != null ? data.TransactionNumber : string.Empty;
                    SchoolCO.TransactionDateStr = data.TransactionDate.GetValueOrDefault().ToString("dd MMM yyyy");
                    SchoolCO.TransactionTypeName = data.TransactionType == 1 ? "Cash" : data.TransactionType == 2 ? "Cheque" : "Credit Card";
                    newData.SchoolCO = SchoolCO;

                    TransactionCOList.Add(newData);
                });
            }
            return Json(new { TransactionList = TransactionCOList }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SaveTransactionData(StudentInvoiceTransaction StudentInvoiceTransaction)
        {
            using (var context = new GS247Entities8())
            {
                StudentInvoiceTransaction.StatusFlag = 0;
                StudentInvoiceTransaction.CreatedBy = Session["UserName"].ToString();
                StudentInvoiceTransaction.CreatedDate = DateTime.Now;
                context.StudentInvoiceTransactions.Add(StudentInvoiceTransaction);
                context.SaveChanges();

                var transactionData = context.StudentInvoiceTransactions.Where(x => x.InvoiceId == StudentInvoiceTransaction.InvoiceId && x.StatusFlag == 0).ToList();
                var InvoicePaidAmount = Math.Round(transactionData.Sum(x => Convert.ToDecimal(x.InvoicePaymentAmount)), 2);
                var StudentFeesInvoice = context.StudentFeesInvoices.Find(StudentInvoiceTransaction.InvoiceId);
                if (InvoicePaidAmount == Convert.ToDecimal(StudentFeesInvoice.InvoiceAmount))
                    StudentFeesInvoice.PaidStatusFlag = 1;
                StudentFeesInvoice.PaymentDate = DateTime.Now;
                StudentFeesInvoice.UpdatedDate = DateTime.Now;
                StudentFeesInvoice.UpdatedBy = Session["UserName"].ToString();
                context.Entry(StudentFeesInvoice).State = EntityState.Modified;
                context.SaveChanges();

                var fees = context.Fees.Find(StudentFeesInvoice.FeeId);
                if (fees != null)
                {
                    if (fees.PaymentTypeFlag == 2)
                    {
                        var data = context.StudentRoomDetails.Where(x => x.StudentDetailsId == StudentFeesInvoice.StudentDetailsId).FirstOrDefault();
                        if (data != null)
                        {
                            data.PaidStatusFlag = 1;
                            context.Entry(data).State = EntityState.Modified;
                            context.SaveChanges();
                        }
                    }
                }
            }
            return new EmptyResult();
        }

        [HttpPost]
        public ActionResult RemoveTransactionData(int TransactionID)
        {
            using (var context = new GS247Entities8())
            {
                var StudentInvoiceTransaction = context.StudentInvoiceTransactions.Find(TransactionID);
                if (StudentInvoiceTransaction != null)
                {
                    StudentInvoiceTransaction.StatusFlag = 1;
                    StudentInvoiceTransaction.UpdatedDate = DateTime.Now;
                    StudentInvoiceTransaction.UpdatedBy = Session["UserName"].ToString();
                    context.Entry(StudentInvoiceTransaction).State = EntityState.Modified;
                    context.SaveChanges();

                    var StudentFeesInvoice = context.StudentFeesInvoices.Find(StudentInvoiceTransaction.InvoiceId);
                    StudentFeesInvoice.PaidStatusFlag = 0;
                    StudentFeesInvoice.PaymentDate = DateTime.Now;
                    StudentFeesInvoice.UpdatedDate = DateTime.Now;
                    StudentFeesInvoice.UpdatedBy = Session["UserName"].ToString();
                    context.Entry(StudentFeesInvoice).State = EntityState.Modified;
                    context.SaveChanges();

                    var fees = context.Fees.Find(StudentFeesInvoice.FeeId);
                    if (fees != null)
                    {
                        if (fees.PaymentTypeFlag == 2)
                        {
                            var data = context.StudentRoomDetails.Where(x => x.StudentDetailsId == StudentFeesInvoice.StudentDetailsId).FirstOrDefault();
                            if (data != null)
                            {
                                data.PaidStatusFlag = 2;
                                context.Entry(data).State = EntityState.Modified;
                                context.SaveChanges();
                            }
                        }
                    }
                }
            }
            return new EmptyResult();
        }

        [HttpPost]
        public ActionResult CancelInvoiceTransactionData(int InvoiceId)
        {
            using (var context = new GS247Entities8())
            {
                var StudentFeesInvoice = context.StudentFeesInvoices.Find(InvoiceId);
                if (StudentFeesInvoice != null)
                {
                    StudentFeesInvoice.PaidStatusFlag = 2;
                    StudentFeesInvoice.PaymentDate = DateTime.Now;
                    StudentFeesInvoice.UpdatedDate = DateTime.Now;
                    StudentFeesInvoice.UpdatedBy = Session["UserName"].ToString();
                    context.Entry(StudentFeesInvoice).State = EntityState.Modified;
                    context.SaveChanges();
                }
            }
            return new EmptyResult();
        }

        [HttpPost]
        public ActionResult fileDownload(int TransactionID)
        {

            using (var context = new GS247Entities8())
            {
                var StudentInvoiceTransaction = context.StudentInvoiceTransactions.Find(TransactionID);
                if (StudentInvoiceTransaction != null)
                {
                    char[] charArr = StudentInvoiceTransaction.proof.ToCharArray();
                    byte[] bytes = new byte[charArr.Length];
                    for (int i = 0; i < charArr.Length; i++)
                    {
                        byte current = Convert.ToByte(charArr[i]);
                        bytes[i] = current;
                    }

                    Response.Clear();
                    Response.AddHeader("Cache-Control", "no-cache, must-revalidate, post-check=0, pre-check=0");
                    Response.AddHeader("Pragma", "no-cache");
                    Response.AddHeader("Content-Description", "File Download");
                    Response.AddHeader("Content-Type", "application/force-download");
                    Response.AddHeader("Content-Transfer-Encoding", "binary\n");
                    Response.AddHeader("content-disposition", "attachment;filename=" + StudentInvoiceTransaction.FileName);
                    Response.BinaryWrite(bytes);
                    Response.Flush();
                    Response.End();
                }
            }

            return new EmptyResult();
        }

        #endregion

        #region "TRANSPORATION"

        public ActionResult TransportationFee()
        {
            if (sessionValidate())
            {
                var studentCOData = new List<TransportStudentRouteLinkCO>();
                using (var context = new GS247Entities8())
                {
                    var studentData = context.TransportStudentRouteLinks.ToList();
                    studentData.ForEach(data =>
                    {

                        var newData = new TransportStudentRouteLinkCO();
                        newData.RouteId = data.RouteId;
                        newData.StopId = data.StopId;
                        newData.StudentId = data.StudentId;

                        var SchoolCO = new SchoolCO();

                        var student = context.StudentDetails.Find(data.StudentId);
                        SchoolCO.StudentName = student != null ? student.FirstName + " " + student.LastName : string.Empty;

                        var route = context.TransportRoutes.Find(data.RouteId);
                        SchoolCO.RouteName = route != null ? route.RouteName : string.Empty;

                        var stop = context.TransportStops.Find(data.StopId);
                        SchoolCO.StopName = stop != null ? stop.StopName : string.Empty;
                        SchoolCO.FareAmount = stop != null ? stop.Fare.GetValueOrDefault().ToString("N2") : string.Empty;

                        newData.SchoolCO = SchoolCO;
                        studentCOData.Add(newData);
                    });

                    ViewBag.StudentList = studentCOData;
                }
                return View();
            }
            else
                return RedirectToAction("Index", "Login");
        }

        #endregion

        #region "INVOICE"

        public ActionResult GenerateTransportInvoice(int StudentDetailsId)
        {
            if (sessionValidate())
            {
                ViewBag.StudentDetailsId = StudentDetailsId;
                List<int> year = new List<int>();
                for (int i = 1900; i < 300; i++)
                {
                    year.Add(i);
                }
                ViewBag.Years = year;
                return View();
            }
            else
                return RedirectToAction("Index", "Login");
        }

        public ActionResult GenerateInvoiceAll()
        {
            if (sessionValidate())
            {
                List<int> year = new List<int>();
                for (int i = 1900; i < 3000; i++)
                {
                    year.Add(i);
                }
                ViewBag.Years = year;
                return View();
            }
            else
                return RedirectToAction("Index", "Login");
        }

        [HttpPost]
        public JsonResult TransporationFees(int month, int year, int StudentDetailsId)
        {
            var message = string.Empty;
            if (sessionValidate())
            {
                using (var context = new GS247Entities8())
                {
                    var date = new DateTime(year, month, DateTime.DaysInMonth(year, month));

                    var StudentFeesInvoice = (from fees in context.Fees
                                              join sfi in context.StudentFeesInvoices on fees.FeeID equals sfi.FeeId
                                              where fees.PaymentTypeFlag == 1 && sfi.StudentDetailsId == StudentDetailsId
                                              select sfi).ToList();
                    var flag = true;

                    if (StudentFeesInvoice.Count > 0)
                    {
                        flag = StudentFeesInvoice.Where(x => x.DueDate.GetValueOrDefault().Date == date.Date).Any();
                    }

                    if (!flag)
                    {
                        var studentData = context.TransportStudentRouteLinks.Find(StudentDetailsId);
                        double? FareAmount = 0;
                        if (studentData != null)
                        {
                            var stop = context.TransportStops.Find(studentData.StopId);
                            FareAmount = stop.Fare;

                            var fee = new Fee();
                            fee.Name = "Transportation Fee - " + getMonth(month) + " " + year;
                            fee.Description = "Transportation Fee";
                            fee.UserId = Convert.ToInt16(Session["UserId"]);
                            fee.CreatedBy = Session["UserName"].ToString();
                            fee.CreatedDate = DateTime.Now;
                            fee.PaymentTypeFlag = 1;
                            context.Fees.Add(fee);
                            context.SaveChanges();

                            var feeParticular = new FeeParticular();
                            feeParticular.FeeID = fee.FeeID;
                            feeParticular.Name = "Transportation Fee";
                            feeParticular.Description = "Transportation Fee";
                            feeParticular.Tax = null;
                            feeParticular.Discount = "0.00";
                            feeParticular.Type = null;
                            feeParticular.CreatedBy = Session["UserName"].ToString();
                            feeParticular.CreatedDate = DateTime.Now;
                            context.FeeParticulars.Add(feeParticular);
                            context.SaveChanges();

                            var feeAccess = new FeeAccess();
                            feeAccess.FeeParticularID = feeParticular.FeeParticularID;
                            feeAccess.Type = null;
                            feeAccess.AdmissionNumbers = null;
                            feeAccess.CourseID = null;
                            feeAccess.BatchID = null;
                            feeAccess.CategoryID = null;
                            feeAccess.Amount = FareAmount.GetValueOrDefault().ToString("N2");
                            feeAccess.FeeID = fee.FeeID;
                            feeAccess.CreatedBy = Session["UserName"].ToString();
                            feeAccess.CreatedDate = DateTime.Now;
                            context.FeeAccesses.Add(feeAccess);
                            context.SaveChanges();

                            var StudentFeesInvoice1 = new StudentFeesInvoice();
                            StudentFeesInvoice1.StudentDetailsId = StudentDetailsId;
                            StudentFeesInvoice1.FeeId = fee.FeeID;
                            StudentFeesInvoice1.TotalAmount = FareAmount.GetValueOrDefault().ToString("N2");
                            StudentFeesInvoice1.DiscountAmount = "0.00";
                            StudentFeesInvoice1.InvoiceAmount = FareAmount.GetValueOrDefault().ToString("N2");
                            StudentFeesInvoice1.PaidStatusFlag = 0;

                            StudentFeesInvoice1.DueDate = date;
                            StudentFeesInvoice1.CreatedDate = DateTime.Now;
                            StudentFeesInvoice1.CreatedBy = Session["UserName"].ToString();
                            StudentFeesInvoice1.FeeAccessID = feeAccess.FeeAccessID;
                            StudentFeesInvoice1.FeeParticularID = feeParticular.FeeParticularID;
                            StudentFeesInvoice1.CancelFlag = 0;
                            StudentFeesInvoice1.CourseId = null;
                            StudentFeesInvoice1.BatchId = null;
                            context.StudentFeesInvoices.Add(StudentFeesInvoice1);
                            context.SaveChanges();

                            var studentInvoicePayment = new StudentInvoicePayment();
                            studentInvoicePayment.InvoiceId = StudentFeesInvoice1.InvoiceID;
                            studentInvoicePayment.FeeId = fee.FeeID;
                            studentInvoicePayment.FeeParticularID = feeParticular.FeeParticularID;
                            studentInvoicePayment.FeeAccessID = feeAccess.FeeAccessID;
                            studentInvoicePayment.InvoicePaymentAmount = FareAmount.GetValueOrDefault().ToString("N2");
                            studentInvoicePayment.CreatedDate = DateTime.Now;
                            studentInvoicePayment.CreatedBy = Session["UserName"].ToString();
                            context.StudentInvoicePayments.Add(studentInvoicePayment);
                            context.SaveChanges();

                            message = "Success";
                        }
                    }
                    else
                    {
                        message = "Failed.";
                    }
                }
            }

            return Json(new { Message = message });
        }

        [HttpPost]
        public async Task<ActionResult> GenerateIAllnvoice(int month, int year)
        {
            var date = new DateTime(year, month, DateTime.DaysInMonth(year, month));

            using (var context = new GS247Entities8())
            {

                var studentData = context.TransportStudentRouteLinks.ToList();

                var StudentFeesInvoice = (from fees in context.Fees
                                          join sfi in context.StudentFeesInvoices on fees.FeeID equals sfi.FeeId
                                          where fees.PaymentTypeFlag == 1
                                          select sfi).ToList();

                var result = StudentFeesInvoice.Where(x => x.DueDate.GetValueOrDefault().Date == date.Date).GroupBy(x => x.StudentDetailsId)
                                                            .Select(dd => dd.First()).ToList();

                var dataList = (from ss in studentData
                                where !result.Any(x => x.StudentDetailsId == ss.StudentId)
                                select ss).Distinct().ToList();

                dataList.ForEach(data =>
                {

                    double? FareAmount = 0;

                    var stop = context.TransportStops.Find(data.StopId);
                    FareAmount = stop.Fare;

                    var fee = new Fee();
                    fee.Name = "Transportation Fee - " + getMonth(month) + " " + year;
                    fee.Description = "Transportation Fee";
                    fee.UserId = Convert.ToInt16(Session["UserId"]);
                    fee.CreatedBy = Session["UserName"].ToString();
                    fee.CreatedDate = DateTime.Now;
                    fee.PaymentTypeFlag = 1;
                    context.Fees.Add(fee);
                    context.SaveChanges();

                    var feeParticular = new FeeParticular();
                    feeParticular.FeeID = fee.FeeID;
                    feeParticular.Name = "Transportation Fee";
                    feeParticular.Description = "Transportation Fee";
                    feeParticular.Tax = null;
                    feeParticular.Discount = "0.00";
                    feeParticular.Type = null;
                    feeParticular.CreatedBy = Session["UserName"].ToString();
                    feeParticular.CreatedDate = DateTime.Now;
                    context.FeeParticulars.Add(feeParticular);
                    context.SaveChanges();

                    var feeAccess = new FeeAccess();
                    feeAccess.FeeParticularID = feeParticular.FeeParticularID;
                    feeAccess.Type = null;
                    feeAccess.AdmissionNumbers = null;
                    feeAccess.CourseID = null;
                    feeAccess.BatchID = null;
                    feeAccess.CategoryID = null;
                    feeAccess.Amount = FareAmount.GetValueOrDefault().ToString("N2");
                    feeAccess.FeeID = fee.FeeID;
                    feeAccess.CreatedBy = Session["UserName"].ToString();
                    feeAccess.CreatedDate = DateTime.Now;
                    context.FeeAccesses.Add(feeAccess);
                    context.SaveChanges();

                    var StudentFeesInvoice1 = new StudentFeesInvoice();
                    StudentFeesInvoice1.StudentDetailsId = data.StudentId;
                    StudentFeesInvoice1.FeeId = fee.FeeID;
                    StudentFeesInvoice1.TotalAmount = FareAmount.GetValueOrDefault().ToString("N2");
                    StudentFeesInvoice1.DiscountAmount = "0.00";
                    StudentFeesInvoice1.InvoiceAmount = FareAmount.GetValueOrDefault().ToString("N2");
                    StudentFeesInvoice1.PaidStatusFlag = 0;

                    StudentFeesInvoice1.DueDate = date;
                    StudentFeesInvoice1.CreatedDate = DateTime.Now;
                    StudentFeesInvoice1.CreatedBy = Session["UserName"].ToString();
                    StudentFeesInvoice1.FeeAccessID = feeAccess.FeeAccessID;
                    StudentFeesInvoice1.FeeParticularID = feeParticular.FeeParticularID;
                    StudentFeesInvoice1.CancelFlag = 0;
                    StudentFeesInvoice1.CourseId = null;
                    StudentFeesInvoice1.BatchId = null;
                    context.StudentFeesInvoices.Add(StudentFeesInvoice1);
                    context.SaveChanges();

                    var studentInvoicePayment = new StudentInvoicePayment();
                    studentInvoicePayment.InvoiceId = StudentFeesInvoice1.InvoiceID;
                    studentInvoicePayment.FeeId = fee.FeeID;
                    studentInvoicePayment.FeeParticularID = feeParticular.FeeParticularID;
                    studentInvoicePayment.FeeAccessID = feeAccess.FeeAccessID;
                    studentInvoicePayment.InvoicePaymentAmount = FareAmount.GetValueOrDefault().ToString("N2");
                    studentInvoicePayment.CreatedDate = DateTime.Now;
                    studentInvoicePayment.CreatedBy = Session["UserName"].ToString();
                    context.StudentInvoicePayments.Add(studentInvoicePayment);
                    context.SaveChanges();

                });

                await context.SaveChangesAsync();

            }
            return new EmptyResult();
        }

        #endregion

        #region "SUBSCRIPTION"

        public ActionResult CreateSubscription()
        {
            if (sessionValidate())
            {

                Fee fee = TempData["Fees"] as Fee;
                ViewBag.FeeID = fee.FeeID;
                ViewBag.Name = fee.Name;
                ViewBag.CreatedDate = fee.CreatedDate;
                ViewBag.Description = fee.Description;

                List<SelectListItem> items = new List<SelectListItem>();
                items.Add(new SelectListItem() { Text = "Half Yearly", Value = "1" });
                items.Add(new SelectListItem() { Text = "Quarterly", Value = "2" });
                items.Add(new SelectListItem() { Text = "Monthly", Value = "3" });
                items.Add(new SelectListItem() { Text = "Weekly", Value = "4" });

                List<SelectListItem> itemsDay = new List<SelectListItem>();
                for (int i = 1; i <= 28; i++)
                {
                    itemsDay.Add(new SelectListItem() { Text = i.ToString(), Value = i.ToString() });
                }

                List<SelectListItem> itemsWeek = new List<SelectListItem>();
                itemsWeek.Add(new SelectListItem() { Text = "Sunday", Value = "1" });
                itemsWeek.Add(new SelectListItem() { Text = "Monday", Value = "2" });
                itemsWeek.Add(new SelectListItem() { Text = "Tuesday", Value = "3" });
                itemsWeek.Add(new SelectListItem() { Text = "Wednesday", Value = "4" });
                itemsWeek.Add(new SelectListItem() { Text = "Thursday", Value = "5" });
                itemsWeek.Add(new SelectListItem() { Text = "Friday", Value = "6" });
                itemsWeek.Add(new SelectListItem() { Text = "Saturday", Value = "7" });


                ViewBag.RecurringList = items.ToList();
                ViewBag.DayofMonth = itemsDay.ToList();
                ViewBag.DayofWeek = itemsWeek.ToList();

                FeeSubscription _templist = new FeeSubscription();
                using (var context = new GS247Entities8())
                {
                    _templist = context.FeeSubscriptions.Where(x => x.FeeID == fee.FeeID).FirstOrDefault();
                }
                return View(_templist);
            }
            else
                return RedirectToAction("Index", "Login");
        }

        [HttpGet]
        public ActionResult CreateSubscription(int? FeeID)
        {
            Fee fees = TempData["Fees"] as Fee;
            if (fees != null)
            {
                ViewBag.FeeID = fees.FeeID;
                ViewBag.Name = fees.Name;
                ViewBag.CreatedDate = fees.CreatedDate;
                ViewBag.Description = fees.Description;
            }
            else if (FeeID != null)
            {
                using (var context = new GS247Entities8())
                {
                    Fee fee = context.Fees.Find(FeeID);
                    ViewBag.FeeID = fee.FeeID;
                    ViewBag.Name = fee.Name;
                    ViewBag.CreatedDate = fee.CreatedDate;
                    ViewBag.Description = fee.Description;
                }
            }

            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem() { Text = "Half Yearly", Value = "1" });
            items.Add(new SelectListItem() { Text = "Quarterly", Value = "2" });
            items.Add(new SelectListItem() { Text = "Monthly", Value = "3" });
            items.Add(new SelectListItem() { Text = "Weekly", Value = "4" });

            List<SelectListItem> itemsDay = new List<SelectListItem>();
            for (int i = 1; i <= 28; i++)
            {
                itemsDay.Add(new SelectListItem() { Text = i.ToString(), Value = i.ToString() });
            }

            List<SelectListItem> itemsWeek = new List<SelectListItem>();
            itemsWeek.Add(new SelectListItem() { Text = "Sunday", Value = "1" });
            itemsWeek.Add(new SelectListItem() { Text = "Monday", Value = "2" });
            itemsWeek.Add(new SelectListItem() { Text = "Tuesday", Value = "3" });
            itemsWeek.Add(new SelectListItem() { Text = "Wednesday", Value = "4" });
            itemsWeek.Add(new SelectListItem() { Text = "Thursday", Value = "5" });
            itemsWeek.Add(new SelectListItem() { Text = "Friday", Value = "6" });
            itemsWeek.Add(new SelectListItem() { Text = "Saturday", Value = "7" });

            ViewBag.RecurringList = items.ToList();
            ViewBag.DayofMonth = itemsDay.ToList();
            ViewBag.DayofWeek = itemsWeek.ToList();

            FeeSubscription _templist = new FeeSubscription();
            using (var context = new GS247Entities8())
            {
                _templist = context.FeeSubscriptions.Where(x => x.FeeID == FeeID).FirstOrDefault();
            }

            if (_templist != null)
                return View(_templist);
            else
            {
                return View("CreateSubscription");
            }
        }

        [HttpPost]
        public ActionResult CreateSubscription(FeeSubscription feeSubscription)
        {
            using (var context = new GS247Entities8())
            {
                var _templist = context.FeeSubscriptions.Where(x => x.FeeID == feeSubscription.FeeID).FirstOrDefault();
                if (_templist == null)
                {
                    if (feeSubscription.SubscriptionType == 1)
                    {
                        feeSubscription.DayOfMonth = null;
                        feeSubscription.DayOfWeek = null;
                        feeSubscription.DueDate2 = null;
                        feeSubscription.DueDate3 = null;
                        feeSubscription.DueDate4 = null;
                        feeSubscription.RecurringInterval = null;
                    }
                    else
                    {
                        feeSubscription.DueDate1 = null;
                        if (feeSubscription.RecurringInterval == 1)
                        {
                            feeSubscription.DueDate4 = null;
                            feeSubscription.DayOfMonth = null;
                            feeSubscription.DayOfWeek = null;
                        }
                        else if (feeSubscription.RecurringInterval == 2)
                        {
                            feeSubscription.DayOfMonth = null;
                            feeSubscription.DayOfWeek = null;
                        }
                        else if (feeSubscription.RecurringInterval == 3)
                        {
                            feeSubscription.DueDate2 = null;
                            feeSubscription.DueDate3 = null;
                            feeSubscription.DueDate4 = null;
                            feeSubscription.DayOfWeek = null;
                        }
                        else if (feeSubscription.RecurringInterval == 4)
                        {
                            feeSubscription.DueDate2 = null;
                            feeSubscription.DueDate3 = null;
                            feeSubscription.DueDate4 = null;
                            feeSubscription.DayOfMonth = null;
                        }
                    }
                    feeSubscription.CreatedBy = Session["UserName"].ToString();
                    feeSubscription.CreatedDate = DateTime.Now;
                    context.FeeSubscriptions.Add(feeSubscription);
                    context.SaveChanges();
                }
                else
                {

                    if (_templist != null)
                    {
                        if (feeSubscription.SubscriptionType == 1)
                        {
                            feeSubscription.DayOfMonth = null;
                            feeSubscription.DayOfWeek = null;
                            feeSubscription.DueDate2 = null;
                            feeSubscription.DueDate3 = null;
                            feeSubscription.DueDate4 = null;
                            feeSubscription.RecurringInterval = null;
                        }
                        else
                        {
                            feeSubscription.DueDate1 = null;
                            if (feeSubscription.RecurringInterval == 1)
                            {
                                feeSubscription.DueDate4 = null;
                                feeSubscription.DayOfMonth = null;
                                feeSubscription.DayOfWeek = null;
                            }
                            else if (feeSubscription.RecurringInterval == 2)
                            {
                                feeSubscription.DayOfMonth = null;
                                feeSubscription.DayOfWeek = null;
                            }
                            else if (feeSubscription.RecurringInterval == 3)
                            {
                                feeSubscription.DueDate2 = null;
                                feeSubscription.DueDate3 = null;
                                feeSubscription.DueDate4 = null;
                                feeSubscription.DayOfWeek = null;
                            }
                            else if (feeSubscription.RecurringInterval == 4)
                            {
                                feeSubscription.DueDate2 = null;
                                feeSubscription.DueDate3 = null;
                                feeSubscription.DueDate4 = null;
                                feeSubscription.DayOfMonth = null;
                            }
                        }

                        _templist.StartDate = feeSubscription.StartDate;
                        _templist.EndDate = feeSubscription.EndDate;
                        _templist.DueDate1 = feeSubscription.DueDate1;
                        _templist.DueDate2 = feeSubscription.DueDate2;
                        _templist.DueDate3 = feeSubscription.DueDate3;
                        _templist.DueDate4 = feeSubscription.DueDate4;
                        _templist.DueDate5 = feeSubscription.DueDate5;
                        _templist.DayOfMonth = feeSubscription.DayOfMonth;
                        _templist.DayOfWeek = feeSubscription.DayOfWeek;
                        _templist.SubscriptionType = feeSubscription.SubscriptionType;
                        _templist.RecurringInterval = feeSubscription.RecurringInterval;
                        _templist.NumberOfSubscription = feeSubscription.NumberOfSubscription;
                        _templist.UpdatedDate = DateTime.Now;
                        _templist.UpdatedBy = Session["UserName"].ToString();
                        context.Entry(_templist).State = EntityState.Modified;
                        context.SaveChanges();
                    }
                }
            }
            return RedirectToRoute(new { controller = "Fees", action = "Index" });
        }

        [HttpPost]
        public ActionResult UpdateSubscription(FeeSubscription feeSubscription)
        {
            using (var context = new GS247Entities8())
            {
                var _templist = context.FeeSubscriptions.Where(x => x.FeeID == feeSubscription.FeeID).FirstOrDefault();
                if (_templist != null)
                {
                    if (feeSubscription.SubscriptionType == 1)
                    {
                        feeSubscription.DayOfMonth = null;
                        feeSubscription.DayOfWeek = null;
                        feeSubscription.DueDate2 = null;
                        feeSubscription.DueDate3 = null;
                        feeSubscription.DueDate4 = null;
                        feeSubscription.DueDate5 = null;
                        feeSubscription.RecurringInterval = null;
                    }
                    else
                    {
                        feeSubscription.DueDate1 = null;
                        if (feeSubscription.RecurringInterval == 1)
                        {
                            feeSubscription.DueDate4 = null;
                            feeSubscription.DueDate5 = null;
                            feeSubscription.DayOfMonth = null;
                            feeSubscription.DayOfWeek = null;
                        }
                        else if (feeSubscription.RecurringInterval == 2)
                        {
                            feeSubscription.DayOfMonth = null;
                            feeSubscription.DayOfWeek = null;
                        }
                        else if (feeSubscription.RecurringInterval == 3)
                        {
                            feeSubscription.DueDate2 = null;
                            feeSubscription.DueDate3 = null;
                            feeSubscription.DueDate4 = null;
                            feeSubscription.DueDate5 = null;
                            feeSubscription.DayOfWeek = null;
                        }
                        else if (feeSubscription.RecurringInterval == 4)
                        {
                            feeSubscription.DueDate2 = null;
                            feeSubscription.DueDate3 = null;
                            feeSubscription.DueDate4 = null;
                            feeSubscription.DueDate5 = null;
                            feeSubscription.DayOfMonth = null;
                        }
                    }

                    _templist.StartDate = feeSubscription.StartDate;
                    _templist.EndDate = feeSubscription.EndDate;
                    _templist.DueDate1 = feeSubscription.DueDate1;
                    _templist.DueDate2 = feeSubscription.DueDate2;
                    _templist.DueDate3 = feeSubscription.DueDate3;
                    _templist.DueDate4 = feeSubscription.DueDate4;
                    _templist.DueDate5 = feeSubscription.DueDate5;
                    _templist.DayOfMonth = feeSubscription.DayOfMonth;
                    _templist.DayOfWeek = feeSubscription.DayOfWeek;
                    _templist.SubscriptionType = feeSubscription.SubscriptionType;
                    _templist.RecurringInterval = feeSubscription.RecurringInterval;
                    _templist.NumberOfSubscription = feeSubscription.NumberOfSubscription;
                    _templist.UpdatedDate = DateTime.Now;
                    _templist.UpdatedBy = Session["UserName"].ToString();
                    context.Entry(_templist).State = EntityState.Modified;
                    context.SaveChanges();
                }
            }
            return RedirectToAction("Index");
        }

        #endregion

        #region "REPORTS"
        public ActionResult DailyReport()
        {
            return View();
        }

        [HttpPost]
        public JsonResult DailyReportList(DateTime fromDate, DateTime toDate, string currentPageIndex)
        {
            List<StudentInvoiceTransactionCO> TransactionDetailsList = new List<StudentInvoiceTransactionCO>();
            var totalPages = 0;
            var totalAmount = "";
            using (var context = new GS247Entities8())
            {
                var list = context.StudentInvoiceTransactions.Where(x => x.TransactionDate >= fromDate && x.TransactionDate <= toDate);
                int totalCount = list.Count();
                totalPages = (int)Math.Ceiling((decimal)totalCount / (decimal)10);
                var TransactionList = list.OrderByDescending(x => x.CreatedDate).Skip((Convert.ToInt16(currentPageIndex) - 1) * 10).Take(10).ToList();

                TransactionList.ForEach(data =>
                {
                    var newData = new StudentInvoiceTransactionCO();
                    newData.TransactionID = data.TransactionID;
                    newData.InvoiceId = data.InvoiceId;
                    newData.TransactionDate = data.TransactionDate;
                    newData.TransactionType = data.TransactionType;
                    newData.TransactionNumber = string.IsNullOrEmpty(data.TransactionNumber) ? "" : data.TransactionNumber;
                    newData.Description = string.IsNullOrEmpty(data.Description) ? "" : data.Description;
                    newData.InvoicePaymentAmount = Convert.ToDecimal(data.InvoicePaymentAmount).ToString("N2");
                    newData.StatusFlag = data.StatusFlag;
                    newData.proof = data.proof;
                    newData.FileName = data.FileName;
                    newData.CreatedBy = data.CreatedBy;
                    newData.CreatedDate = data.CreatedDate;

                    newData.Description = data.Description != null ? data.Description : string.Empty;
                    newData.TransactionNumber = data.TransactionNumber != null ? data.TransactionNumber : string.Empty;
                    newData.TransactionDateStr = data.TransactionDate.GetValueOrDefault().ToString("dd MMM yyyy");
                    newData.TransactionTypeName = data.TransactionType == 1 ? "Cash" : data.TransactionType == 2 ? "Cheque" : "Credit Card";
                    var fees = (from a in context.Fees
                                join b in context.StudentFeesInvoices on a.FeeID equals b.FeeId
                                where b.InvoiceID == data.InvoiceId
                                select a).FirstOrDefault();
                    newData.FeeCategoryName = fees != null ? fees.Name : "";                    
                    TransactionDetailsList.Add(newData);
                });
            }

            if (TransactionDetailsList.Count > 0)
                totalAmount = TransactionDetailsList.Sum(x => Convert.ToDecimal(x.InvoicePaymentAmount)).ToString("N2");
            return Json(new { TransactionDetailsList = TransactionDetailsList, TotalPages = totalPages,TotalAmount = totalAmount }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DueReport()
        {
            if (sessionValidate())
            {  
                using (var context = new GS247Entities8())
                {   
                    ViewBag.CoursesList = context.Courses.Where(x => x.IsEnable == true).ToList();                            
                }
                return View();
            }
            else
                return RedirectToAction("Index", "Login");
        }

        [HttpPost]
        public JsonResult DueReportList(int courseId, int batchId, string currentPageIndex)
        {
            List<StudentFeesInvoiceCO> TransactionDetailsList = new List<StudentFeesInvoiceCO>();
            var totalPages = 0;            
            using (var context = new GS247Entities8())
            {
                var list = context.StudentFeesInvoices.Where(x => x.PaidStatusFlag == 0 && x.CourseId == courseId);
                if (batchId > 0)
                    list = list.Where(x => x.BatchId == batchId);
                int totalCount = list.Count();
                totalPages = (int)Math.Ceiling((decimal)totalCount / (decimal)10);
                var TransactionList = list.OrderByDescending(x => x.CreatedDate).Skip((Convert.ToInt16(currentPageIndex) - 1) * 10).Take(10).ToList();

                TransactionList.ForEach(data =>
                {
                    var newData = new StudentFeesInvoiceCO();
                    newData.InvoiceID = data.InvoiceID;
                    newData.StudentDetailsId = data.StudentDetailsId;
                    newData.FeeId = data.FeeId;
                    newData.InvoiceAmount = data.InvoiceAmount;
                    newData.PaidStatusFlag = data.PaidStatusFlag;
                    newData.DueDate = data.DueDate;
                    newData.PaymentDate = data.PaymentDate;
                    newData.CreatedBy = data.CreatedBy;
                    newData.CreatedDate = data.CreatedDate;
                    newData.FeeAccessID = data.FeeAccessID;
                    newData.FeeParticularID = data.FeeParticularID;
                    newData.CancelFlag = data.CancelFlag;
                    newData.CourseId = data.CourseId;
                    newData.BatchId = data.BatchId;
                    newData.TotalAmount = data.TotalAmount;
                    newData.DiscountAmount = data.DiscountAmount;

                    var studentDetails = context.StudentDetails.Find(data.StudentDetailsId);
                    newData.StudentName = studentDetails != null ? studentDetails.FirstName + " " + studentDetails.LastName : "";
                    newData.AdmissionNo = studentDetails != null ? studentDetails.AdmissionNo : "";
                    var fees = context.Fees.Find(data.FeeId);                                
                    newData.FeeCategoryName = fees != null ? fees.Name : "";
                    newData.TransactionDateStr = data.DueDate.GetValueOrDefault().ToString("dd MMM yyyy");

                    var transactionDate = context.StudentInvoiceTransactions.Where(x => x.InvoiceId == newData.InvoiceID && x.StatusFlag == 0).ToList();
                    var InvoicePaidAmount = transactionDate.Count > 0 ? Math.Round(transactionDate.Sum(x => Convert.ToDecimal(x.InvoicePaymentAmount)), 2).ToString() : "0.00";

                    newData.InvoiceBalanceAmount = Math.Round((Convert.ToDecimal(newData.InvoiceAmount) - Convert.ToDecimal(InvoicePaidAmount)), 2).ToString();

                    TransactionDetailsList.Add(newData);
                });
            }

            return Json(new { TransactionDetailsList = TransactionDetailsList, TotalPages = totalPages}, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region "CONFIGURATIONS"

        public ActionResult Configurations()
        {
            if (sessionValidate())
            {
                FeesConfiguration _templist = new FeesConfiguration();
                int id = Convert.ToInt32(Session["UserId"].ToString());

                using (var context = new GS247Entities8())
                {
                    _templist = context.FeesConfigurations.Where(x => x.UserId > 0).FirstOrDefault();
                }
                return View(_templist);
            }
            else
                return RedirectToAction("Index", "Login");
        }

        [HttpPost]
        public ActionResult Configurations(FeesConfiguration collection)
        {
            using (var context = new GS247Entities8())
            {
                int id = Convert.ToInt32(Session["UserId"].ToString());
                var _templist = context.FeesConfigurations.Where(x => x.UserId > 0).FirstOrDefault();
                if (_templist == null)
                {
                    collection.CreatedBy = Session["UserName"].ToString();
                    collection.CreatedDate = DateTime.Now;
                    collection.UserId = id;
                    context.FeesConfigurations.Add(collection);
                    context.SaveChanges();
                }
                else
                {
                    _templist.TaxFlag = collection.TaxFlag;
                    _templist.DiscountFlag = collection.DiscountFlag;
                    _templist.InvoiceDiscountShow = collection.InvoiceDiscountShow;
                    _templist.InvoiceTemplate = collection.InvoiceTemplate;
                    _templist.UpdatedDate = DateTime.Now;
                    _templist.UpdatedBy = Session["UserName"].ToString();
                    context.Entry(_templist).State = EntityState.Modified;
                    context.SaveChanges();
                }
            }
            return RedirectToAction("Configurations");
        }

        #endregion

        #region "PAYMENT"

        public ActionResult CreatePayment()
        {
            return View();
        }

        public ActionResult ManagePayment()
        {
            if (sessionValidate())
            {
                var paymentList = new List<PaymentType>();
                int id = Convert.ToInt32(Session["UserId"].ToString());
                using (var context = new GS247Entities8())
                {
                    paymentList = context.PaymentTypes.Where(x => x.UserId > 0).ToList();
                    ViewBag.paymentdetails = paymentList;
                }
                return View();
            }
            else
                return RedirectToAction("Index", "Login");
        }

        [HttpPost]
        public ActionResult CreatePayment(PaymentType paymentType)
        {
            using (var context = new GS247Entities8())
            {
                var _templist = context.PaymentTypes.Where(x => x.PaymentTypeID == paymentType.PaymentTypeID).FirstOrDefault();
                if (_templist == null)
                {
                    paymentType.UserId = Convert.ToInt16(Session["UserId"]);
                    paymentType.CreatedBy = Session["UserName"].ToString();
                    paymentType.CreatedDate = DateTime.Now;
                    context.PaymentTypes.Add(paymentType);
                    context.SaveChanges();
                }
            }
            return RedirectToAction("ManagePayment");
        }

        [HttpPost]
        public ActionResult UpdatePayment(PaymentType paymentType)
        {
            using (var context = new GS247Entities8())
            {
                var _templist = context.PaymentTypes.Where(x => x.PaymentTypeID == paymentType.PaymentTypeID).FirstOrDefault();
                if (_templist != null)
                {
                    _templist.Type = paymentType.Type;
                    _templist.StatusFlag = paymentType.StatusFlag;
                    _templist.UpdatedDate = DateTime.Now;
                    _templist.UpdatedBy = Session["UserName"].ToString();
                    context.Entry(_templist).State = EntityState.Modified;
                    context.SaveChanges();
                }
            }
            return RedirectToAction("ManagePayment");
        }

        [HttpGet]
        public ActionResult EditPayment(int PaymentTypeID)
        {
            using (var context = new GS247Entities8())
            {
                var _templist = context.PaymentTypes.Where(x => x.PaymentTypeID == PaymentTypeID).FirstOrDefault();
                return View("CreatePayment", _templist);
            }
        }

        [HttpPost]
        public ActionResult DeletePayment(int PaymentTypeID)
        {
            using (var context = new GS247Entities8())
            {
                var _templist = context.PaymentTypes.Where(x => x.PaymentTypeID == PaymentTypeID).FirstOrDefault();
                if (_templist != null)
                {
                    context.PaymentTypes.Remove(_templist);
                    context.SaveChanges();
                }
            }
            return new EmptyResult();
        }

        #endregion

        #region "TAX"

        public ActionResult CreateTax()
        {
            return View();
        }

        public ActionResult ManageTax()
        {
            if (sessionValidate())
            {
                var taxList = new List<Tax>();
                int id = Convert.ToInt32(Session["UserId"].ToString());
                using (var context = new GS247Entities8())
                {
                    taxList = context.Taxes.Where(x => x.UserId > 0).ToList();
                    ViewBag.taxdetails = taxList;
                }
                return View();
            }
            else
                return RedirectToAction("Index", "Login");
        }

        [HttpPost]
        public ActionResult CreateTax(Tax tax)
        {
            using (var context = new GS247Entities8())
            {
                var _templist = context.Taxes.Where(x => x.TaxID == tax.TaxID).FirstOrDefault();
                if (_templist == null)
                {
                    tax.UserId = Convert.ToInt16(Session["UserId"]);
                    tax.CreatedBy = Session["UserName"].ToString();
                    tax.Value = tax.Value;
                    tax.CreatedDate = DateTime.Now;
                    context.Taxes.Add(tax);
                    context.SaveChanges();
                }
            }
            return RedirectToAction("ManageTax");
        }

        [HttpPost]
        public ActionResult UpdateTax(Tax tax)
        {
            using (var context = new GS247Entities8())
            {
                var _templist = context.Taxes.Where(x => x.TaxID == tax.TaxID).FirstOrDefault();
                if (_templist != null)
                {
                    _templist.TaxName = tax.TaxName;
                    _templist.StatusFlag = tax.StatusFlag;
                    _templist.Value = tax.Value;
                    _templist.UpdatedDate = DateTime.Now;
                    _templist.UpdatedBy = Session["UserName"].ToString();
                    context.Entry(_templist).State = EntityState.Modified;
                    context.SaveChanges();
                }
            }
            return RedirectToAction("ManageTax");
        }

        [HttpGet]
        public ActionResult EditTax(int taxID)
        {
            using (var context = new GS247Entities8())
            {
                var _templist = context.Taxes.Where(x => x.TaxID == taxID).FirstOrDefault();
                return View("CreateTax", _templist);
            }
        }

        [HttpPost]
        public ActionResult DeleteTax(int taxID)
        {
            using (var context = new GS247Entities8())
            {
                var _templist = context.Taxes.Where(x => x.TaxID == taxID).FirstOrDefault();
                if (_templist != null)
                {
                    context.Taxes.Remove(_templist);
                    context.SaveChanges();
                }
            }
            return new EmptyResult();
        }

        #endregion

        #region "GATEWAY SETTINGS"

        public ActionResult Gateway()
        {
            if (sessionValidate())
            {
                GatewaySetting _templist = new GatewaySetting();
                int id = Convert.ToInt32(Session["UserId"].ToString());

                using (var context = new GS247Entities8())
                {
                    _templist = context.GatewaySettings.Where(x => x.UserId > 0).FirstOrDefault();
                    var currencyList = context.Currencies.ToList();
                    ViewBag.currencyDetails = currencyList;
                }
                return View(_templist);
            }
            else
                return RedirectToAction("Index", "Login");
        }

        [HttpPost]
        public ActionResult Gateway(GatewaySetting gateway)
        {
            using (var context = new GS247Entities8())
            {
                int id = Convert.ToInt32(Session["UserId"].ToString());
                var _templist = context.GatewaySettings.Where(x => x.UserId > 0).FirstOrDefault();
                if (_templist == null)
                {
                    gateway.UserId = Convert.ToInt16(Session["UserId"]);
                    gateway.CreatedBy = Session["UserName"].ToString();
                    gateway.CreatedDate = DateTime.Now;
                    context.GatewaySettings.Add(gateway);
                    context.SaveChanges();
                }
                else
                {
                    _templist.APIUserName = gateway.APIUserName;
                    _templist.APIPassword = gateway.APIPassword;
                    _templist.APISignature = gateway.APISignature;
                    _templist.Currency = gateway.Currency;
                    _templist.UpdatedDate = DateTime.Now;
                    _templist.UpdatedBy = Session["UserName"].ToString();
                    context.Entry(_templist).State = EntityState.Modified;
                    context.SaveChanges();
                }
            }
            return RedirectToAction("Gateway");
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

        public string getMonth(int month)
        {
            var monthName = "";

            if (month == 1)
                monthName = "Jan";
            else if (month == 2)
                monthName = "Feb";
            else if (month == 3)
                monthName = "Mar";
            else if (month == 4)
                monthName = "Apr";
            else if (month == 5)
                monthName = "May";
            else if (month == 6)
                monthName = "Jun";
            else if (month == 7)
                monthName = "Jul";
            else if (month == 8)
                monthName = "Aug";
            else if (month == 9)
                monthName = "Sep";
            else if (month == 10)
                monthName = "Oct";
            else if (month == 11)
                monthName = "Nov";
            else if (month == 12)
                monthName = "Dec";

            return monthName;
        }

        public partial class FeeCO
        {
            public int FeeID { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public Nullable<int> UserId { get; set; }
            public string CreatedBy { get; set; }
            public System.DateTime CreatedDate { get; set; }
            public string UpdatedBy { get; set; }
            public Nullable<System.DateTime> UpdatedDate { get; set; }
            public Nullable<int> InvoiceStatusFlag { get; set; }
            public Nullable<int> PaymentTypeFlag { get; set; }

            public virtual SchoolCO SchoolCO { get; set; }
        }

        public partial class FeeParticularCO
        {
            public int FeeParticularID { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public Nullable<int> Tax { get; set; }
            public string Discount { get; set; }
            public Nullable<int> Type { get; set; }
            public Nullable<int> FeeID { get; set; }
            public string CreatedBy { get; set; }
            public System.DateTime CreatedDate { get; set; }
            public string UpdatedBy { get; set; }
            public Nullable<System.DateTime> UpdatedDate { get; set; }

            public string TaxName { get; set; }

            public virtual SchoolCO SchoolCO { get; set; }
        }

        public partial class StudentFeesInvoiceCO
        {
            public int InvoiceID { get; set; }
            public Nullable<int> StudentDetailsId { get; set; }
            public Nullable<int> FeeId { get; set; }
            public string InvoiceAmount { get; set; }
            public Nullable<int> PaidStatusFlag { get; set; }
            public Nullable<System.DateTime> DueDate { get; set; }
            public Nullable<System.DateTime> PaymentDate { get; set; }
            public string CreatedBy { get; set; }
            public System.DateTime CreatedDate { get; set; }
            public string UpdatedBy { get; set; }
            public Nullable<System.DateTime> UpdatedDate { get; set; }
            public Nullable<int> FeeAccessID { get; set; }
            public Nullable<int> FeeParticularID { get; set; }
            public Nullable<int> CancelFlag { get; set; }
            public Nullable<int> CourseId { get; set; }
            public Nullable<int> BatchId { get; set; }
            public string TotalAmount { get; set; }
            public string DiscountAmount { get; set; }

            public string StudentName { get; set; }

            public string AdmissionNo { get; set; }

            public string FeeCategoryName { get; set; }

            public string TransactionDateStr { get; set; }

            public string InvoiceBalanceAmount { get; set; }           

            public virtual SchoolCO SchoolCO { get; set; }
        }

        public partial class StudentInvoiceTransactionCO
        {
            public int TransactionID { get; set; }
            public Nullable<int> InvoiceId { get; set; }
            public Nullable<System.DateTime> TransactionDate { get; set; }
            public Nullable<int> TransactionType { get; set; }
            public string TransactionNumber { get; set; }
            public string Description { get; set; }
            public string InvoicePaymentAmount { get; set; }
            public Nullable<int> StatusFlag { get; set; }
            public string proof { get; set; }
            public string FileName { get; set; }
            public string CreatedBy { get; set; }
            public System.DateTime CreatedDate { get; set; }
            public string UpdatedBy { get; set; }
            public Nullable<System.DateTime> UpdatedDate { get; set; }

            public string TransactionDateStr { get; set; }

            public string FeeCategoryName { get; set; }

            public string TransactionTypeName { get; set; }
            public virtual SchoolCO SchoolCO { get; set; }
        }

        public partial class TransportStudentRouteLinkCO
        {
            public int StudentId { get; set; }
            public int RouteId { get; set; }
            public int StopId { get; set; }
            public virtual SchoolCO SchoolCO { get; set; }
        }

        #endregion
    }
}