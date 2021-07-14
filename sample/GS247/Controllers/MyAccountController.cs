using GS247.Utilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace GS247.Controllers
{
    public class MyAccountController : Controller
    {
        // GET: MyAccount
        public ActionResult Index()
        {
            int UserId = Convert.ToInt32(Session["UserId"]);
            if (UserId == 0)
            {
                return RedirectToAction("Index", "Login");
            }
            return View();
        }

        public ActionResult News()
        {
            int UserId = Convert.ToInt32(Session["UserId"]);
            if (UserId == 0)
            {
                return RedirectToAction("Index", "Login");
            }
            return View();
        }
        public ActionResult CreateNews()
        {
            int UserId = Convert.ToInt32(Session["UserId"]);
            if (UserId == 0)
            {
                return RedirectToAction("Index", "Login");
            }
            return View();
        }

        public ActionResult PublishNews()
        {
            int UserId = Convert.ToInt32(Session["UserId"]);
            if (UserId == 0)
            {
                return RedirectToAction("Index", "Login");
            }
            return View();
        }

        public ActionResult EventTypes()
        {
            int UserId = Convert.ToInt32(Session["UserId"]);
            if (UserId == 0)
            {
                return RedirectToAction("Index", "Login");
            }
            return View();
        }
        public ActionResult Events()
        {
            int UserId = Convert.ToInt32(Session["UserId"]);
            if (UserId == 0)
            {
                return RedirectToAction("Index", "Login");
            }
            return View();
        }
        public ActionResult Category()
        {
            int UserId = Convert.ToInt32(Session["UserId"]);
            if (UserId == 0)
            {
                return RedirectToAction("Index", "Login");
            }
            return View();
        }
        public ActionResult Calendar()
        {
            int UserId = Convert.ToInt32(Session["UserId"]);
            if (UserId == 0)
            {
                return RedirectToAction("Index", "Login");
            }
            return View();
        }
        public ActionResult ComplaintRegister()
        {
            int UserId = Convert.ToInt32(Session["UserId"]);
            if (UserId == 0)
            {
                return RedirectToAction("Index", "Login");
            }
            return View();
        }
        public ActionResult ComplaintList()
        {
            int UserId = Convert.ToInt32(Session["UserId"]);
            if (UserId == 0)
            {
                return RedirectToAction("Index", "Login");
            }
            return View();
        }
        public ActionResult DocumentList()
        {
            int UserId = Convert.ToInt32(Session["UserId"]);
            if (UserId == 0)
            {
                return RedirectToAction("Index", "Login");
            }


            



            return View();
        }
        
        [HttpPost]
        public string GetNewsFeed(string flag)
        {
            string res = string.Empty;
            DataAccess obj = new DataAccess();
            List<Params> paramList = new List<Params>();
            paramList.Add(new Params("@Flag", flag));
            DataTable dt = obj.Execute("MyAccountDetails", CommandType.StoredProcedure, paramList);
            res = JsonConvert.SerializeObject(dt, Formatting.Indented);
            return res;

        }

        [HttpPost]
        public string ActivityFeedDetails(string flag, string TableName, string OperationType,string StartDate, string EndDate)
        {
            string res = string.Empty;
            DataAccess obj = new DataAccess();
            List<Params> paramList = new List<Params>();
            paramList.Add(new Params("@TableName", TableName));
            paramList.Add(new Params("@OperationType", OperationType));
            paramList.Add(new Params("@StartDate", StartDate));
            paramList.Add(new Params("@EndDate", EndDate));
            paramList.Add(new Params("@Flag", flag));
            DataTable dt = obj.Execute("MyAccountDetails", CommandType.StoredProcedure, paramList);
            res = JsonConvert.SerializeObject(dt, Formatting.Indented);
            return res;

        }

        [HttpPost]
        public string ActivityFeedInsert(string flag, string TableName, string OperationType, string CreatedDate, string CreatedBy, string userId, string IPAddress)
        {
            string res = string.Empty;
            DataAccess obj = new DataAccess();
            List<Params> paramList = new List<Params>();
            paramList.Add(new Params("@TableName", TableName));
            paramList.Add(new Params("@OperationType", OperationType));
            paramList.Add(new Params("@CreatedDate", CreatedDate));
            paramList.Add(new Params("@CreatedBy", CreatedBy));
            paramList.Add(new Params("@userId", Session["UserId"]));
            paramList.Add(new Params("@IPAddress", IPAddress));
            paramList.Add(new Params("@Flag", flag));
            DataTable dt = obj.Execute("MyAccountDetails", CommandType.StoredProcedure, paramList);
            res = JsonConvert.SerializeObject(dt, Formatting.Indented);
            return res;

        }

        [HttpPost]
        public string NewsFeedDetails(string Id,string flag, string NewsTopic, string NewsDescription, string userId,string IsPublish)
        {
            string res = string.Empty;
            DataAccess obj = new DataAccess();
            List<Params> paramList = new List<Params>();
            paramList.Add(new Params("@Id", Id));
            paramList.Add(new Params("@NewsTopic", NewsTopic));
            paramList.Add(new Params("@NewsDescription", NewsDescription));
            paramList.Add(new Params("@userId", Session["UserId"]));
            paramList.Add(new Params("@IsPublish", IsPublish));
            paramList.Add(new Params("@Flag", flag));
            DataTable dt = obj.Execute("MyAccountDetails", CommandType.StoredProcedure, paramList);
            res = JsonConvert.SerializeObject(dt, Formatting.Indented);
            return res;

        }


        [HttpPost]
        public string EventDetails(string Id, string flag, string Eventname, string ColorCode)
        {
            string res = string.Empty;
            DataAccess obj = new DataAccess();
            List<Params> paramList = new List<Params>();
            paramList.Add(new Params("@Id", Id));
            paramList.Add(new Params("@EventType", Eventname));
            paramList.Add(new Params("@ColorCode", ColorCode));
            paramList.Add(new Params("@Flag", flag));
            DataTable dt = obj.Execute("MyAccountDetails", CommandType.StoredProcedure, paramList);
            res = JsonConvert.SerializeObject(dt, Formatting.Indented);
            return res;

        }

        [HttpPost]
        public string CategoryDetails(string Id, string flag, string CategoryName)
        {
            string res = string.Empty;
            DataAccess obj = new DataAccess();
            List<Params> paramList = new List<Params>();
            paramList.Add(new Params("@Id", Id));
            paramList.Add(new Params("@CategoryName", CategoryName));
            paramList.Add(new Params("@Flag", flag));
            DataTable dt = obj.Execute("MyAccountDetails", CommandType.StoredProcedure, paramList);
            res = JsonConvert.SerializeObject(dt, Formatting.Indented);
            return res;

        }
       
        [HttpPost]
        public string CalenderDetails(string Id, string EventName, string flag, string EventDescription, string EventFromDate, string EventToDate, string EventFromTime, string EventToTime, string EventType, string Organizer, string Allday, string Editable)
        {
            string res = string.Empty;
            DataAccess obj = new DataAccess();
            List<Params> paramList = new List<Params>();
            paramList.Add(new Params("@Organizer", Organizer));
            paramList.Add(new Params("@Allday", Allday));
            paramList.Add(new Params("@Editable", Editable));            
            paramList.Add(new Params("@Id", Convert.ToInt32(Id)));
            paramList.Add(new Params("@EventName", EventName));
            paramList.Add(new Params("@EventDescription", EventDescription));
            paramList.Add(new Params("@EventFromDate",DateTime.ParseExact(EventFromDate, "dd-MM-yyyy", null)));
             paramList.Add(new Params("@EventToDate", DateTime.ParseExact(EventToDate, "dd-MM-yyyy", null)));
            paramList.Add(new Params("@EventFromTime", EventFromTime));
            paramList.Add(new Params("@EventToTime", EventToTime));
             paramList.Add(new Params("@EventType", EventType));
            paramList.Add(new Params("@Flag", flag));
            DataTable dt = obj.Execute("MyAccountDetails", CommandType.StoredProcedure, paramList);
            res = JsonConvert.SerializeObject(dt, Formatting.Indented);
            return res;

        }
        
        [HttpPost]
        public string ComplaintDetails(string Id, string flag, string Category, string subject, string complaint, string status, string UserType, string userId)
        {
            string res = string.Empty;
            DataAccess obj = new DataAccess();
            List<Params> paramList = new List<Params>();
            paramList.Add(new Params("@Id", Convert.ToInt32(Id)));
            paramList.Add(new Params("@ComplaintCategory", Category));
            paramList.Add(new Params("@ComplaintSubject", subject));
            paramList.Add(new Params("@Complaint", complaint));
            paramList.Add(new Params("@ComplaintStatus", status));
            paramList.Add(new Params("@Flag", flag));
            paramList.Add(new Params("@ComplaintUserType", UserType));
            paramList.Add(new Params("@userId", Session["UserId"]));
            DataTable dt = obj.Execute("MyAccountDetails", CommandType.StoredProcedure, paramList);
            res = JsonConvert.SerializeObject(dt, Formatting.Indented);
            return res;

        }


        [HttpPost]
        public string DisApprovedDetails(string Id, string flag, string Category, string subject, string complaint, string status, string UserType, string userId)
        {
            string res = string.Empty;
            DataAccess obj = new DataAccess();
            List<Params> paramList = new List<Params>();
            paramList.Add(new Params("@Id", Convert.ToInt32(Id)));
            paramList.Add(new Params("@ComplaintCategory", Category));
            paramList.Add(new Params("@ComplaintSubject", subject));
            paramList.Add(new Params("@Complaint", complaint));
            paramList.Add(new Params("@ComplaintStatus", status));
            paramList.Add(new Params("@Flag", flag));
            paramList.Add(new Params("@ComplaintUserType", UserType));
            paramList.Add(new Params("@userId", Session["UserId"]));
            DataTable dt = obj.Execute("MyAccountDetails", CommandType.StoredProcedure, paramList);
            res = JsonConvert.SerializeObject(dt, Formatting.Indented);
            //if (res == "Success")
            //{
            //    string uploadModulefolder = Path.Combine(Category);
            //    string ArchiveFolder = Path.Combine(uploadModulefolder, "Archive");
            //    if (Directory.Exists(ArchiveFolder) == false)
            //    {
            //        Directory.CreateDirectory(uploadModulefolder);

            //    }
            //    System.IO.File.Move(uploadModulefolder, ArchiveFolder);
            //}


            //string uploadModulefolder = Path.Combine(@"C:\\Users\\SIP010404\\Downloads\\New folder (5)\\samplefile.pdf");
            //string ArchiveFolder = Path.Combine(@"C:\\Users\\SIP010404\\Downloads\\New folder (5)", "Archive");            
            //if (Directory.Exists(ArchiveFolder) == false)
            //{
            //    Directory.CreateDirectory(ArchiveFolder);

            //}


            // System.IO.File.Move(uploadModulefolder, ArchiveFolder);

            //System.IO.File.Copy(uploadModulefolder, ArchiveFolder, true);
            //System.IO.File.Move(uploadModulefolder, ArchiveFolder);


            return res;

        }


        public FileResult DownloadFile(string filePath)
        {
            var FileVirtualPath = filePath;
            //var FileVirtualPath = Path.Combine(Server.MapPath("~/UploadFiles"),filename);
            return File(FileVirtualPath, "application/force-download", Path.GetFileName(FileVirtualPath));

        }

       


    }
}