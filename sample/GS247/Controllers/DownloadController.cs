using GS247.Models;
using GS247.Models.Courses;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GS247.Controllers
{
    public class DownloadController : Controller
    {
       public GS247Entities8 db=new GS247Entities8();
        public ActionResult Category_Index()
        {
            int UserId = Convert.ToInt32(Session["UserId"]);
            if (UserId == 0)
            {
                return RedirectToAction("Index", "Login");
            }
            return View(db.Categories.ToList());
        }
        public ActionResult AddCategory()
        {
            int UserId = Convert.ToInt32(Session["UserId"]);
            if (UserId == 0)
            {
                return RedirectToAction("Index", "Login");
            }
            ViewBag.Title = "Add Category";
            return View(new Category());
        }
        [HttpPost]
        public ActionResult AddCategory(Category category)
        {
            int UserId = Convert.ToInt32(Session["UserId"]);
            if (UserId == 0)
            {
                return RedirectToAction("Index", "Login");
            }
            if (db.Categories.Where(x => x.CategoryName == category.CategoryName).FirstOrDefault() == null)
            {
                db.Categories.Add(category);
                db.SaveChanges();
                TempData["Category_Index"] = "Category Added";
                return RedirectToAction("Category_Index");
            }
            ViewBag.Title = "Add Category";
            TempData["AddCategory"] = "Category Exist";
            return View(category);
        }
        public ActionResult UpdateCategory(int id)
        {
            int UserId = Convert.ToInt32(Session["UserId"]);
            if (UserId == 0)
            {
                return RedirectToAction("Index", "Login");
            }
            ViewBag.Title = "Update Category";
            return View("AddCategory",db.Categories.Find(id));
        }
        [HttpPost]
        public ActionResult UpdateCategory(Category category)
        {
            int UserId = Convert.ToInt32(Session["UserId"]);
            if (UserId == 0)
            {
                return RedirectToAction("Index", "Login");
            }
            if (db.Categories.Where(x => x.CategoryName == category.CategoryName).ToList().Count<2)
            {
                var category1 = db.Categories.Find(category.CategoryId);
                category1.CategoryName = category.CategoryName;
                db.Entry(category1).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                TempData["Category_Index"] = "Category Updated";
                return RedirectToAction("Category_Index");
            }
            ViewBag.Title = "Update Category";
            TempData["AddCategory"] = "Category Exist";
            return View(category);
        }
       
        public ActionResult DeleteCategory(int id)
        {
            int UserId = Convert.ToInt32(Session["UserId"]);
            if (UserId == 0)
            {
                return RedirectToAction("Index", "Login");
            }
            db.Categories.Remove(db.Categories.Find(id));
            db.SaveChanges();
            TempData["Category_Index"] = "Category Removed";
            return RedirectToAction("Category_Index");
        }
        public ActionResult FileUpload_Index()
        {
            int UserId = Convert.ToInt32(Session["UserId"]);
            if (UserId == 0)
            {
                return RedirectToAction("Index", "Login");
            }
            var fileUploads = db.FileUploads.ToList();
            foreach(var item in fileUploads)
            {
                var users = db.Users.Find(Convert.ToInt32(item.UploadedBy));
                if (users != null)
                    item.UploadedBy = users.FirstName + " " + users.LastName + "-" + users.UserName;
            }
            return View(fileUploads);
        }
        public ActionResult UploadFile()
        {
            int UserId = Convert.ToInt32(Session["UserId"]);
            if (UserId == 0)
            {
                return RedirectToAction("Index", "Login");
            }
            ViewBag.Categories = new SelectList(db.Categories.Select(x => x.CategoryName).Distinct());
            ViewBag.Placeholders = new SelectList(db.SettingsUserRoles.Select(x => x.Name).Distinct());
            ViewBag.Courses = new SelectList(db.Courses.Select(x => x.Name).Distinct());
            ViewBag.Batches = new SelectList(db.batches.Select(x => x.Name).Distinct());
            return View(new FileUpload());
        }
      
        [HttpPost]
        public ActionResult UploadFile(HttpPostedFileBase FileUpload,string title,string categoryName,string placeholder,string course,string batch)
        {
            int UserId = Convert.ToInt32(Session["UserId"]);
            if (UserId == 0)
            {
                return RedirectToAction("Index", "Login");
            }
            string fileLocation = "";
            if (Request.Files["FileUpload"].ContentLength > 0)
            {
                string filePath = Server.MapPath("~/Content/UploadFiles/");
                if (!System.IO.Directory.Exists(filePath))
                System.IO.Directory.CreateDirectory(filePath);
                fileLocation = filePath + Request.Files["FileUpload"].FileName;
                if (System.IO.File.Exists(fileLocation))
                {
                    System.IO.File.Delete(fileLocation);
                }
                Request.Files["FileUpload"].SaveAs(fileLocation);
            }
            var fileCheck = db.FileUploads.Where(x => x.CategoryName == categoryName && x.Placeholder == placeholder && x.Course == course && x.Batch == batch).FirstOrDefault();
            if(fileCheck==null)
            {
                FileUpload fileUpload = new FileUpload
                {
                    Title=title,
                     CategoryName= categoryName,
                      Placeholder=placeholder,
                      Course=course,
                      Batch=batch,
                       FilePath=Request.Files["FileUpload"].FileName,
                       FileType = Path.GetExtension(Request.Files["FileUpload"].FileName),
                       UploadedBy= Session["UserId"].ToString(),
                        UploadedOn=DateTime.Today
                };
                db.FileUploads.Add(fileUpload);
                db.SaveChanges();
            }
            else if(fileCheck!=null)
            {
                fileCheck.FilePath = Request.Files["FileUpload"].FileName;
                fileCheck.FileType = Path.GetExtension(Request.Files["FileUpload"].FileName);
                fileCheck.UploadedBy = Session["UserId"].ToString();
                fileCheck.UploadedOn = DateTime.Today;
                db.Entry(fileCheck).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
            return RedirectToAction("FileUpload_Index");
        }
        public ActionResult Update_FileUpload(long id)
        {
            int UserId = Convert.ToInt32(Session["UserId"]);
            if (UserId == 0)
            {
                return RedirectToAction("Index", "Login");
            }
            ViewBag.Categories = new SelectList(db.Categories.Select(x => x.CategoryName).Distinct());
            ViewBag.Placeholders = new SelectList(db.SettingsUserRoles.Select(x => x.Name).Distinct());
            ViewBag.Courses = new SelectList(db.Courses.Select(x => x.Name).Distinct());
            ViewBag.Batches = new SelectList(db.batches.Select(x=>x.Name).Distinct());
            return View("UploadFile", db.FileUploads.Find(id));
        }
        public ActionResult DownloadFile(long id)
        {
            int UserId = Convert.ToInt32(Session["UserId"]);
            if (UserId == 0)
            {
                return RedirectToAction("Index", "Login");
            }
            try
            {
                var fileUpload = db.FileUploads.Find(id);
                if (fileUpload.FilePath != null)
                {
                    byte[] fileBytes = System.IO.File.ReadAllBytes(Server.MapPath("~/Content/UploadFiles/"+fileUpload.FilePath));
                    return File(fileBytes, "application/force-download",fileUpload.FilePath);
                }
                TempData["FileUpload_Index"] = "No document found to download..!";
                return RedirectToAction("FileUpload_Index");
            }
            catch (Exception e)
            {
                TempData["FileUpload_Index"] = "Unable to Download";
                return RedirectToAction("FileUpload_Index");
            }
        }
        public ActionResult Delete_FileUpload(long id)
        {
            int UserId = Convert.ToInt32(Session["UserId"]);
            if (UserId == 0)
            {
                return RedirectToAction("Index", "Login");
            }
            db.FileUploads.Remove(db.FileUploads.Find(id));
            db.SaveChanges();
            return RedirectToAction("FileUpload_Index");
        }
    }
}