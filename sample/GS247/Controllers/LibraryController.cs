using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GS247.Models;
using System.Data.Entity;
using System.Web.WebPages.Html;
using System.Web.UI.WebControls;
using GS247.Models.Library;

namespace GS247.Controllers
{
    public class LibraryController : Controller
    {
        // GET: Library
     
        public ActionResult Index()
        {
            int UserId = Convert.ToInt32(Session["UserId"]);
            if (UserId == 0)
            {
                return RedirectToAction("Index", "Login");
            }
            return View();
        }
        public ActionResult ManageAuthors()
        {
            int UserId = Convert.ToInt32(Session["UserId"]);
            if (UserId == 0)
            {
                return RedirectToAction("Index", "Login");
            }
            using (GS247Entities8 objEntities = new GS247Entities8())
            {
                return View(objEntities.LibraryAuthors.ToList());
            }

        }

        public ActionResult ManageBookCategories()
        {
            int UserId = Convert.ToInt32(Session["UserId"]);
            if (UserId == 0)
            {
                return RedirectToAction("Index", "Login");
            }
            using (GS247Entities8 objEntities = new GS247Entities8())
            {
                return View(objEntities.LibraryCategories.ToList());
            }

        }
     
        public ActionResult ManageBooks()
        {
            int UserId = Convert.ToInt32(Session["UserId"]);
            if (UserId == 0)
            {
                return RedirectToAction("Index", "Login");
            }
            using (GS247Entities8 objEntities = new GS247Entities8())
            {
                var objBooks = from a in objEntities.LibraryAuthors
                               join b in objEntities.LibraryBooks on a.Id equals b.AuthorId
                               join s in objEntities.Subjects on b.SubjectId equals s.Id
                               select new ViewBooks
                               {
                                   Id=b.Id,
                                   AuthorId=b.AuthorId,
                                   SubjectName=s.Name,
                                   ISBN=b.ISBN,
                                   Title=b.Title,
                                   AuthorName=a.AuthorName,
                                   Edition=b.Edition,
                                   Publisher=b.Publisher,
                                   CopiesAvailable=b.CopiesAvailable,
                                   BookPosition=b.BookPosition,
                                   ShelfNo=b.ShelfNo,
                                   Copy=b.Copy
                               };

                return View(objBooks.ToList());
            }

        }
        public ActionResult ViewBooks(int id)
        {
            int UserId = Convert.ToInt32(Session["UserId"]);
            if (UserId == 0)
            {
                return RedirectToAction("Index", "Login");
            }
            using (GS247Entities8 objEntities = new GS247Entities8())
            {
                var lstBooks = from a in objEntities.LibraryAuthors
                               join b in objEntities.LibraryBooks on a.Id equals b.AuthorId where b.AuthorId==id
                               select new ViewBooks
                               {
                                   ISBN = b.ISBN,
                                   Title = b.Title,
                                   AuthorName = a.AuthorName,
                                   Edition = b.Edition,
                                   Publisher = b.Publisher,
                                   CopiesAvailable = b.CopiesAvailable,
                                   BookPosition = b.BookPosition,
                                   ShelfNo = b.ShelfNo,
                                   Copy = b.Copy
                               };
                return View(lstBooks.ToList());
            }
        }
       

        public ActionResult SearchBooks(string ddlSearchBy, string searchItem)
        {
            int UserId = Convert.ToInt32(Session["UserId"]);
            if (UserId == 0)
            {
                return RedirectToAction("Index", "Login");
            }
            using (GS247Entities8 objEntities = new GS247Entities8())
            {
                var objsearch = from a in objEntities.LibraryAuthors
                               join b in objEntities.LibraryBooks on a.Id equals b.AuthorId
                               join s in objEntities.Subjects on b.SubjectId equals s.Id
                               select new ViewBooks
                               {
                                   AuthorId = b.AuthorId,
                                   SubjectName = s.Name,
                                   ISBN = b.ISBN,
                                   Title = b.Title,
                                   AuthorName = a.AuthorName,
                                   CopiesAvailable = b.CopiesAvailable,
                                   BookPosition = b.BookPosition,
                                   ShelfNo = b.ShelfNo,
                                   Copy = b.Copy
                               };
               
                if (ddlSearchBy=="1" && !string.IsNullOrWhiteSpace(searchItem))
                {
                    objsearch = from a in objEntities.LibraryAuthors
                                join b in objEntities.LibraryBooks on a.Id equals b.AuthorId
                                join s in objEntities.Subjects on b.SubjectId equals s.Id
                                where b.Title.Contains(searchItem)
                                select new ViewBooks
                                {
                                    AuthorId = b.AuthorId,
                                    SubjectName = s.Name,
                                    ISBN = b.ISBN,
                                    Title = b.Title,
                                    AuthorName = a.AuthorName,
                                    CopiesAvailable = b.CopiesAvailable,
                                    BookPosition = b.BookPosition,
                                    ShelfNo = b.ShelfNo,
                                    Copy = b.Copy
                                };

                }
                if(ddlSearchBy=="2" && !string.IsNullOrWhiteSpace(searchItem))
                {
                    objsearch = from a in objEntities.LibraryAuthors
                                join b in objEntities.LibraryBooks on a.Id equals b.AuthorId
                                join s in objEntities.Subjects on b.SubjectId equals s.Id
                                where a.AuthorName.Contains(searchItem)
                                select new ViewBooks
                                {
                                    AuthorId = b.AuthorId,
                                    SubjectName = s.Name,
                                    ISBN = b.ISBN,
                                    Title = b.Title,
                                    AuthorName = a.AuthorName,
                                    CopiesAvailable = b.CopiesAvailable,
                                    BookPosition = b.BookPosition,
                                    ShelfNo = b.ShelfNo,
                                    Copy = b.Copy
                                };
                }
                if (ddlSearchBy == "3" && !string.IsNullOrWhiteSpace(searchItem))
                {
                    objsearch = from a in objEntities.LibraryAuthors
                                join b in objEntities.LibraryBooks on a.Id equals b.AuthorId
                                join s in objEntities.Subjects on b.SubjectId equals s.Id
                                where b.ISBN.Contains(searchItem)
                                select new ViewBooks
                                {
                                    AuthorId = b.AuthorId,
                                    SubjectName = s.Name,
                                    ISBN = b.ISBN,
                                    Title = b.Title,
                                    AuthorName = a.AuthorName,
                                    CopiesAvailable = b.CopiesAvailable,
                                    BookPosition = b.BookPosition,
                                    ShelfNo = b.ShelfNo,
                                    Copy = b.Copy
                                };
                }
              
                return View(objsearch.ToList());
            }
        }

        public ActionResult SearchReturnBooks(string stdName)
        {
            int UserId = Convert.ToInt32(Session["UserId"]);
            if (UserId == 0)
            {
                return RedirectToAction("Index", "Login");
            }
            using (GS247Entities8 objEntities = new GS247Entities8())
            {
               
                var objretbook = from a in objEntities.LibraryAuthors
                                 join b in objEntities.LibraryBooks on a.Id equals b.AuthorId
                                 join bi in objEntities.LibraryBooksIssues on b.Id equals bi.BookId
                                 join s in objEntities.StudentDetails on bi.StudentId equals s.StudentDetailsId
                                 where (s.FirstName + " " + s.MiddleName + " " + s.LastName).Contains(stdName)
                                 select new ReturnBookDetails
                                 {
                                     BookId = bi.BookId,
                                     StudentName = s.FirstName + " " + s.MiddleName + " " + s.LastName,
                                     ISBN = b.ISBN,
                                     Title = b.Title,
                                     AuthorName = a.AuthorName,
                                     IssueDate = bi.IssueDate,
                                     DueDate = bi.DueDate,
                                     isReturned = bi.isReturned
                                 };


                return View(objretbook.ToList());
            }

        }

       

        public ActionResult ViewBookDetails(string ddlBookTitles)
        {
            int UserId = Convert.ToInt32(Session["UserId"]);
            if (UserId == 0)
            {
                return RedirectToAction("Index", "Login");
            }
            using (GS247Entities8 objEntities = new GS247Entities8())
            {
                List<System.Web.Mvc.SelectListItem> ObjBookList = new List<System.Web.Mvc.SelectListItem>();

                ObjBookList = (from k in objEntities.LibraryBooks
                           select new System.Web.Mvc.SelectListItem()
                           {
                               Text = k.Title,
                               Value = k.Id.ToString()
                           }).ToList();

                ViewBag.BookTitles = ObjBookList;

                List<System.Web.Mvc.SelectListItem> ObjSubNameList = new List<System.Web.Mvc.SelectListItem>();

                ObjSubNameList = (from s in objEntities.Subjects
                              select new System.Web.Mvc.SelectListItem()
                              {
                                  Text = s.Name,
                                  Value = s.Id.ToString()
                              }).ToList();
                ViewBag.SubNames = ObjSubNameList;

                var objborrowedbooks = from a in objEntities.LibraryAuthors
                                       join b in objEntities.LibraryBooks on a.Id equals b.AuthorId
                                       join bi in objEntities.LibraryBooksIssues on b.Id equals bi.BookId
                                       join s in objEntities.StudentDetails on bi.StudentId equals s.StudentDetailsId
                                       where bi.BookId.ToString() == ddlBookTitles
                                       select new ViewBorrowedBooks
                                       {
                                           StudentName = s.FirstName + " " + s.MiddleName + " " + s.LastName,
                                           ISBN = b.ISBN,
                                           Title = b.Title,
                                           AuthorName = a.AuthorName,
                                           Edition = b.Edition,
                                           Publisher = b.Publisher,
                                           CopiesAvailable = b.CopiesAvailable,
                                           Copy = b.Copy,
                                       };
                return View(objborrowedbooks.ToList());
            }
        }

        public ActionResult DueDates(string ddlsearchitem)
        {
            int UserId = Convert.ToInt32(Session["UserId"]);
            if (UserId == 0)
            {
                return RedirectToAction("Index", "Login");
            }
            using (GS247Entities8 objEntities = new GS247Entities8())
            {
                List<System.Web.Mvc.SelectListItem> ObjBookList = new List<System.Web.Mvc.SelectListItem>();

                var objduebooks = from a in objEntities.LibraryAuthors
                                       join b in objEntities.LibraryBooks on a.Id equals b.AuthorId
                                       join bi in objEntities.LibraryBooksIssues on b.Id equals bi.BookId
                                       join s in objEntities.StudentDetails on bi.StudentId equals s.StudentDetailsId
                                  where bi.isReturned == 0 || bi.isReturned == 2
                                  select new ReturnBookDetails
                                       {
                                           StudentName = s.FirstName + " " + s.MiddleName + " " + s.LastName,
                                           ISBN = b.ISBN,
                                           Title = b.Title,
                                           AuthorName = a.AuthorName,
                                           IssueDate = bi.IssueDate,
                                           DueDate = bi.DueDate,
                                           isReturned = bi.isReturned
                                       };
                if (ddlsearchitem == "2")
                {
                    objduebooks = from a in objEntities.LibraryAuthors
                                  join b in objEntities.LibraryBooks on a.Id equals b.AuthorId
                                  join bi in objEntities.LibraryBooksIssues on b.Id equals bi.BookId
                                  join s in objEntities.StudentDetails on bi.StudentId equals s.StudentDetailsId
                                  where bi.isReturned == 2
                                  select new ReturnBookDetails
                                  {
                                      StudentName = s.FirstName + " " + s.MiddleName + " " + s.LastName,
                                      ISBN = b.ISBN,
                                      Title = b.Title,
                                      AuthorName = a.AuthorName,
                                      IssueDate = bi.IssueDate,
                                      DueDate = bi.DueDate,
                                      isReturned = bi.isReturned
                                  };
                }
                return View(objduebooks.ToList());
            }
        }
        // GET: Library/Details/5
        public ActionResult Details(int id)
        {
            int UserId = Convert.ToInt32(Session["UserId"]);
            if (UserId == 0)
            {
                return RedirectToAction("Index", "Login");
            }
            return View();
        }
        // GET: Library/Details/5
        public ActionResult AuthorDetails(int id)
        {
            int UserId = Convert.ToInt32(Session["UserId"]);
            if (UserId == 0)
            {
                return RedirectToAction("Index", "Login");
            }
            using (GS247Entities8 objEntities = new GS247Entities8())
            {
                var objauthDetails = from a in objEntities.LibraryAuthors
                            join b in objEntities.LibraryBooks on a.Id equals b.AuthorId
                            join s in objEntities.Subjects on b.SubjectId equals s.Id
                                     where b.AuthorId==id
                            select new ViewBooks
                            {
                                AuthorId = b.AuthorId,
                                SubjectName = s.Name,
                                ISBN = b.ISBN,
                                Title = b.Title,
                                AuthorName = a.AuthorName,
                                Publisher=b.Publisher
                            };
                return View(objauthDetails.ToList());
            }
        }
        // GET: Library/Create
        public ActionResult CreateAuthor()
        {
            int UserId = Convert.ToInt32(Session["UserId"]);
            if (UserId == 0)
            {
                return RedirectToAction("Index", "Login");
            }
            return View();
        }

        // POST: Library/Create
        [HttpPost]
        public ActionResult CreateAuthor(LibraryAuthor author)
        {
            try
            {
                int UserId = Convert.ToInt32(Session["UserId"]);
                if (UserId == 0)
                {
                    return RedirectToAction("Index", "Login");
                }
                // TODO: Add insert logic here
                using (GS247Entities8 objEntities = new GS247Entities8())
                {
                    objEntities.LibraryAuthors.Add(author);
                    objEntities.SaveChanges();
                }

                return RedirectToAction("ManageAuthors");
            }
            catch
            {
                return View();
            }
        }

        // GET: Library/Edit/5
        public ActionResult EditAuthor(int id)
        {
            int UserId = Convert.ToInt32(Session["UserId"]);
            if (UserId == 0)
            {
                return RedirectToAction("Index", "Login");
            }
            using (GS247Entities8 objEntities = new GS247Entities8())
            {
                return View(objEntities.LibraryAuthors.Where(l => l.Id == id).FirstOrDefault());
            }
        }

        // POST: Library/Edit/5
        [HttpPost]
        public ActionResult EditAuthor(int id, LibraryAuthor author)
        {
            try
            {
                int UserId = Convert.ToInt32(Session["UserId"]);
                if (UserId == 0)
                {
                    return RedirectToAction("Index", "Login");
                }
                // TODO: Add update logic here
                using (GS247Entities8 objEntities = new GS247Entities8())
                {
                    objEntities.Entry(author).State = EntityState.Modified;
                    objEntities.SaveChanges();
                }
                return RedirectToAction("ManageAuthors");
            }
            catch
            {
                return View();
            }
        }

        // GET: Library/Delete/5
        public ActionResult DeleteAuthor(int id)
        {
            int UserId = Convert.ToInt32(Session["UserId"]);
            if (UserId == 0)
            {
                return RedirectToAction("Index", "Login");
            }
            using (GS247Entities8 objEntities = new GS247Entities8())
            {
                return View(objEntities.LibraryAuthors.Where(l => l.Id == id).FirstOrDefault());
            }
        }

        // POST: Library/Delete/5
        [HttpPost]
        public ActionResult DeleteAuthor(int id, LibraryAuthor author)
        {
            try
            {
                int UserId = Convert.ToInt32(Session["UserId"]);
                if (UserId == 0)
                {
                    return RedirectToAction("Index", "Login");
                }
                // TODO: Add delete logic here
                using (GS247Entities8 objEntities = new GS247Entities8())
                {
                    author= objEntities.LibraryAuthors.Where(l => l.Id == id).FirstOrDefault();
                    objEntities.LibraryAuthors.Remove(author);
                    objEntities.SaveChanges();

                }
                return RedirectToAction("ManageAuthors");
            }
            catch
            {
                return View();
            }
        }
        public ActionResult CreateCategory()
        {
            int UserId = Convert.ToInt32(Session["UserId"]);
            if (UserId == 0)
            {
                return RedirectToAction("Index", "Login");
            }
            return View();
        }

        // POST: Library/Create
        [HttpPost]
        public ActionResult CreateCategory(LibraryCategory category)
        {
            try
            {
                int UserId = Convert.ToInt32(Session["UserId"]);
                if (UserId == 0)
                {
                    return RedirectToAction("Index", "Login");
                }
                // TODO: Add insert logic here
                using (GS247Entities8 objEntities = new GS247Entities8())
                {
                    var _templist = objEntities.LibraryCategories.Where(x => x.CategoryName.ToUpper().Equals(category.CategoryName.ToUpper()) && x.Id != category.Id).FirstOrDefault();
                    if (_templist != null)
                    {
                        ViewBag.Message = "Book category already available";
                        return View();
                    }  
                    else
                    {
                        objEntities.LibraryCategories.Add(category);
                        objEntities.SaveChanges();
                    }                    
                }

                return RedirectToAction("ManageBookCategories");
            }
            catch
            {
                return View();
            }
        }
        public ActionResult EditCategory(int id)
        {
            int UserId = Convert.ToInt32(Session["UserId"]);
            if (UserId == 0)
            {
                return RedirectToAction("Index", "Login");
            }
            using (GS247Entities8 objEntities = new GS247Entities8())
            {
                return View(objEntities.LibraryCategories.Where(l => l.Id == id).FirstOrDefault());
            }
        }

        // POST: Library/Edit/5
        [HttpPost]
        public ActionResult EditCategory(int id, LibraryCategory category)
        {
            try
            {
                int UserId = Convert.ToInt32(Session["UserId"]);
                if (UserId == 0)
                {
                    return RedirectToAction("Index", "Login");
                }
                // TODO: Add update logic here
                using (GS247Entities8 objEntities = new GS247Entities8())
                {
                    var _templist = objEntities.LibraryCategories.Where(x => x.CategoryName.ToUpper().Equals(category.CategoryName.ToUpper()) && x.Id != category.Id).FirstOrDefault();
                    if (_templist != null)
                    {
                        ViewBag.Message = "Book category already available";
                        return View();
                    }  
                    else
                    {
                        objEntities.Entry(category).State = EntityState.Modified;
                        objEntities.SaveChanges();
                    }                  
                }
                return RedirectToAction("ManageBookCategories");
            }
            catch
            {
                return View();
            }
        }

        // GET: Library/Delete/5
        public ActionResult DeleteCategory(int id)
        {
            int UserId = Convert.ToInt32(Session["UserId"]);
            if (UserId == 0)
            {
                return RedirectToAction("Index", "Login");
            }
            using (GS247Entities8 objEntities = new GS247Entities8())
            {
                return View(objEntities.LibraryCategories.Where(l => l.Id == id).FirstOrDefault());
            }
        }

        // POST: Library/Delete/5
        [HttpPost]
        public ActionResult DeleteCategory(int id, LibraryCategory category)
        {
            try
            {
                int UserId = Convert.ToInt32(Session["UserId"]);
                if (UserId == 0)
                {
                    return RedirectToAction("Index", "Login");
                }
                // TODO: Add delete logic here
                using (GS247Entities8 objEntities = new GS247Entities8())
                {
                    category = objEntities.LibraryCategories.Where(l => l.Id == id).FirstOrDefault();
                    objEntities.LibraryCategories.Remove(category);
                    objEntities.SaveChanges();

                }
                return RedirectToAction("ManageBookCategories");
            }
            catch
            {
                return View();
            }
        }

        [HttpPost]
        public JsonResult AutoFill(string Prefix)
        {
            
            //Note : you can bind same list from database  
            using (GS247Entities8 objEntities = new GS247Entities8())
            {

                var subjectlist = (from k in objEntities.Subjects
                                   where k.Name.StartsWith(Prefix)
                                   select new
                                   {
                                       label = k.Name,
                                       val = k.Id
                                   }).ToList();


                return Json(subjectlist);
            }
        }
        [HttpPost]
        public JsonResult AutoFillAuthor(string Prefix)
        {
          
            //Note : you can bind same list from database  
            using (GS247Entities8 objEntities = new GS247Entities8())
            {

                var authorlist = (from k in objEntities.LibraryAuthors
                                   where k.AuthorName.StartsWith(Prefix)
                                   select new
                                   {
                                       label = k.AuthorName,
                                       val = k.Id
                                   }).ToList();


                return Json(authorlist);
            }
        }
        [HttpPost]
        public JsonResult AutoFillStudent(string Prefix)
        {   
            //Note : you can bind same list from database  
            using (GS247Entities8 objEntities = new GS247Entities8())
            {

                var studentlist = (from s in objEntities.StudentDetails
                                  where s.FirstName.StartsWith(Prefix)
                                  && s.DeleteFlag == 0 && s.OnlineApplicationStatus == 1
                                  select new
                                  {
                                      label = s.FirstName+" "+s.MiddleName+" "+s.LastName,
                                      val = s.StudentDetailsId
                                  }).ToList();


                return Json(studentlist);
            }
        }
        [HttpPost]
        public JsonResult AutoFillPhoneNumber1(string Prefix)
        {
           
            //Note : you can bind same list from database  
            using (GS247Entities8 objEntities = new GS247Entities8())
            {

                var phnumberlist = (from s in objEntities.StudentDetails
                                   where s.PhoneNumber1.StartsWith(Prefix)
                                   select new
                                   {
                                       label = s.PhoneNumber1,
                                       val = s.StudentDetailsId
                                   }).ToList();


                return Json(phnumberlist);
            }
        }
        public ActionResult CreateBook()
        {
            int UserId = Convert.ToInt32(Session["UserId"]);
            if (UserId == 0)
            {
                return RedirectToAction("Index", "Login");
            }
            using (GS247Entities8 objEntities = new GS247Entities8())
            {  
                var ObjList = (from k in objEntities.LibraryCategories
                           select new System.Web.Mvc.SelectListItem()
                           {
                               Text = k.CategoryName,
                               Value = k.Id.ToString()
                           }).ToList();
               
                 //var ObjList1 = (from k in objEntities.Subjects
                 //           select new System.Web.Mvc.SelectListItem()
                 //           {
                 //               Text = k.Name,
                 //               Value = k.Id.ToString()
                 //           }).ToList();

                 var ObjList2 = (from k in objEntities.LibraryAuthors
                             select new System.Web.Mvc.SelectListItem()
                             {
                                 Text = k.AuthorName,
                                 Value = k.Id.ToString()
                             }).ToList();

                 ViewBag.Categories = ObjList;
                 //ViewBag.Subjects = ObjList1;
                 ViewBag.Authors = ObjList2;        
            }
            return View();
        }
        
        // POST: Library/Create
        [HttpPost]
        public ActionResult CreateBook(LibraryBook lib_book)
        {
            try
            {
                int UserId = Convert.ToInt32(Session["UserId"]);
                if (UserId == 0)
                {
                    return RedirectToAction("Index", "Login");
                }
                // TODO: Add insert logic here
                using (GS247Entities8 objEntities = new GS247Entities8())
                {
                    if (lib_book.SubjectId == null)
                    {
                        ViewBag.Categories = objEntities.LibraryCategories.Distinct().Select(i => new System.Web.Mvc.SelectListItem() { Text = i.CategoryName, Value = i.Id.ToString(), Selected = true }).ToList();

                        ViewBag.Authors = objEntities.LibraryAuthors.Distinct().Select(i => new System.Web.Mvc.SelectListItem() { Text = i.AuthorName, Value = i.Id.ToString(), Selected = true }).ToList();

                        ViewBag.Message = "Please Select Subject";
                        return View();
                    }
                    else
                    {
                        lib_book.CopiesAvailable = lib_book.Copy;
                        objEntities.LibraryBooks.Add(lib_book);
                        objEntities.SaveChanges();
                    }                 
                }
                return RedirectToAction("ManageBooks");
            }
            catch
            {
                return View();
            }
        }
        public ActionResult EditBook(int id)
        {
            int UserId = Convert.ToInt32(Session["UserId"]);
            if (UserId == 0)
            {
                return RedirectToAction("Index", "Login");
            }
            using (GS247Entities8 objEntities = new GS247Entities8())
            {

                var objbook = objEntities.LibraryBooks.Find(id);

                
                ViewBag.CategoryList=objEntities.LibraryCategories.Distinct().Select(i => new System.Web.Mvc.SelectListItem() { Text = i.CategoryName, Value = i.Id.ToString(), Selected=true }).ToList();

                //ViewBag.Subjects = objEntities.Subjects.Distinct().Select(i => new System.Web.Mvc.SelectListItem() { Text = i.Name, Value = i.Id.ToString(), Selected = true }).ToList();

                ViewBag.Authors = objEntities.LibraryAuthors.Distinct().Select(i => new System.Web.Mvc.SelectListItem() { Text = i.AuthorName, Value = i.Id.ToString(), Selected = true }).ToList();

                var subjectName = (from s in objEntities.Subjects where s.Id == objbook.SubjectId select s.Name).First();
                ViewBag.SubjectName = subjectName;

                return View(objbook);
            }
        }

        // POST: Library/Edit/5
        [HttpPost]
        public ActionResult EditBook(int id, LibraryBook lib_book)
        {
            try
            {
                int UserId = Convert.ToInt32(Session["UserId"]);
                if (UserId == 0)
                {
                    return RedirectToAction("Index", "Login");
                }
                // TODO: Add update logic here
                using (GS247Entities8 objEntities = new GS247Entities8())
                {
                    if (lib_book.SubjectId == null || lib_book.SubjectId == 0)
                    {
                        ViewBag.CategoryList = objEntities.LibraryCategories.Distinct().Select(i => new System.Web.Mvc.SelectListItem() { Text = i.CategoryName, Value = i.Id.ToString(), Selected = true }).ToList();

                        ViewBag.Authors = objEntities.LibraryAuthors.Distinct().Select(i => new System.Web.Mvc.SelectListItem() { Text = i.AuthorName, Value = i.Id.ToString(), Selected = true }).ToList();

                        ViewBag.Message = "Please Select Subject";
                        return View();
                    }
                    else
                    {
                        lib_book.CopiesAvailable = lib_book.Copy;
                        objEntities.Entry(lib_book).State = EntityState.Modified;
                        objEntities.SaveChanges();
                    }
                }
                return RedirectToAction("ManageBooks");
            }
            catch
            {
                return View();
            }
        }

        // GET: Library/Delete/5
        public ActionResult DeleteBook(int id)
        {
            int UserId = Convert.ToInt32(Session["UserId"]);
            if (UserId == 0)
            {
                return RedirectToAction("Index", "Login");
            }
            using (GS247Entities8 objEntities = new GS247Entities8())
            {

                var lstBook = objEntities.LibraryBooks.Find(id);
                var catName = (from c in objEntities.LibraryCategories where c.Id == lstBook.CategoryId select c.CategoryName).First();
                ViewBag.CategoryName = catName;

                var authName = (from a in objEntities.LibraryAuthors where a.Id == lstBook.AuthorId select a.AuthorName).First();
                ViewBag.AuthorName = authName;

                var subjectName = (from s in objEntities.Subjects where s.Id == lstBook.SubjectId select s.Name).First();
                ViewBag.SubjectName = subjectName;
                return View(objEntities.LibraryBooks.Where(l => l.Id == id).FirstOrDefault());
            }
        }

        // POST: Library/Delete/5
        [HttpPost]
        public ActionResult DeleteBook(int id, LibraryBook lib_book)
        {
            try
            {
                int UserId = Convert.ToInt32(Session["UserId"]);
                if (UserId == 0)
                {
                    return RedirectToAction("Index", "Login");
                }
                // TODO: Add delete logic here
                using (GS247Entities8 objEntities = new GS247Entities8())
                {
                    lib_book = objEntities.LibraryBooks.Where(l => l.Id == id).FirstOrDefault();
                    objEntities.LibraryBooks.Remove(lib_book);
                    objEntities.SaveChanges();

                }
                return RedirectToAction("ManageBooks");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult CreateBorrowBook()
        {
            int UserId = Convert.ToInt32(Session["UserId"]);
            if (UserId == 0)
            {
                return RedirectToAction("Index", "Login");
            }
            using (GS247Entities8 objEntities = new GS247Entities8())
            {
                List<System.Web.Mvc.SelectListItem> ObjList = new List<System.Web.Mvc.SelectListItem>();

                ObjList = (from k in objEntities.LibraryBooks
                           select new System.Web.Mvc.SelectListItem()
                           {
                               Text = k.Title,
                               Value = k.Id.ToString()
                           }).ToList();

                ViewBag.BookNames = ObjList;

                List<System.Web.Mvc.SelectListItem> ObjSubList = new List<System.Web.Mvc.SelectListItem>();

                ObjSubList = (from s in objEntities.Subjects
                           select new System.Web.Mvc.SelectListItem()
                           {
                               Text = s.Name,
                               Value = s.Id.ToString()
                           }).ToList();
                ViewBag.Subjects = ObjSubList;

              
            }
            return View();
        }

        // POST: Library/Create
        [HttpPost]
        public ActionResult CreateBorrowBook(LibraryBooksIssue bookIssue)
        {
            try
            {
                int UserId = Convert.ToInt32(Session["UserId"]);
                if (UserId == 0)
                {
                    return RedirectToAction("Index", "Login");
                }
                int copyavail;
                // TODO: Add insert logic here
                using (GS247Entities8 objEntities = new GS247Entities8())
                {
                    LibraryBook lb = objEntities.LibraryBooks.Where(j => j.Id == bookIssue.BookId).FirstOrDefault();
                    copyavail = Convert.ToInt32(lb.CopiesAvailable) - 1;
                    lb.CopiesAvailable = copyavail.ToString();
                    objEntities.Entry(lb).State = EntityState.Modified;
                    objEntities.SaveChanges();

                    bookIssue.isReturned = 0;
                    objEntities.LibraryBooksIssues.Add(bookIssue);
                    objEntities.SaveChanges();
                }

                return RedirectToAction("SearchReturnBooks");
            }
            catch
            {
                return View();
            }
        }
        public ActionResult ReturnBook(int id)
        {
            int UserId = Convert.ToInt32(Session["UserId"]);
            if (UserId == 0)
            {
                return RedirectToAction("Index", "Login");
            }
            using (GS247Entities8 objEntities = new GS247Entities8())
            {

                var returnbookDetails = from b in objEntities.LibraryBooks
                                        join bi in objEntities.LibraryBooksIssues on b.Id equals bi.BookId
                                        join s in objEntities.StudentDetails on bi.StudentId equals s.StudentDetailsId
                                        where bi.BookId == id
                                        select new ReturnBookDetails
                                        {
                                            BookId = bi.BookId,
                                            StudentName = s.FirstName + " " + s.MiddleName + " " + s.LastName,
                                            ISBN = b.ISBN,
                                            Title = b.Title,
                                            IssueDate = bi.IssueDate,
                                            DueDate = bi.DueDate,
                                            isReturned = bi.isReturned
                                        };
               
                return View(returnbookDetails.FirstOrDefault());
            }

        }
        [HttpPost]
        public ActionResult ReturnBook(int id, string ReturnDate)
        {
            try
            {
                int UserId = Convert.ToInt32(Session["UserId"]);
                if (UserId == 0)
                {
                    return RedirectToAction("Index", "Login");
                }
                int bookavail;
                // TODO: Add update logic here
                using (GS247Entities8 objEntities = new GS247Entities8())
                {
                    LibraryBooksIssue issuedbook = objEntities.LibraryBooksIssues.Where(l => l.BookId == id).FirstOrDefault();
                    if(Convert.ToDateTime(ReturnDate) > Convert.ToDateTime(issuedbook.DueDate))
                    {
                        issuedbook.isReturned = 2;
                    }
                    if (Convert.ToDateTime(ReturnDate) <= Convert.ToDateTime(issuedbook.DueDate))
                    {
                        issuedbook.isReturned = 1;
                        LibraryBook libbook = objEntities.LibraryBooks.Where(j => j.Id == id).FirstOrDefault();
                        bookavail = Convert.ToInt32(libbook.CopiesAvailable) + 1;
                        libbook.CopiesAvailable = bookavail.ToString();
                        objEntities.Entry(libbook).State = EntityState.Modified;
                        objEntities.SaveChanges();
                    }

                    issuedbook.ReturnDate = ReturnDate;
                    objEntities.Entry(issuedbook).State = EntityState.Modified;
                    objEntities.SaveChanges();
                }
                return RedirectToAction("SearchReturnBooks");
            }
            catch
            {
                return View();
            }
        }

        [HttpPost]
        public JsonResult GetStudentList(string currentPageIndex, string alphaValue, StudentAdvanceSearch studentAdvanceSearch)
        {
            List<StudentDetail> studentList = new List<StudentDetail>();
            List<StudentDetailCO> studentCOList = new List<StudentDetailCO>();

            var totalPages = 0;
            using (var context = new GS247Entities8())
            {                
                if (alphaValue != "All")
                {
                    var studentData = (from sd in context.StudentDetails
                                       where (sd.DeleteFlag == 0 || sd.DeleteFlag == null) && (sd.OnlineStatus == 0 || sd.OnlineApplicationStatus == 1)                                       
                                       select sd).ToList();

                    var Data = studentData.Where(m => m.FirstName[0] == Convert.ToChar(alphaValue)).ToList();
                    int totalCount = Data.Count;
                    totalPages = (int)Math.Ceiling((decimal)totalCount / (decimal)10);
                    studentList = Data.OrderByDescending(x => x.CreatedDate).Skip((Convert.ToInt16(currentPageIndex) - 1) * 10).Take(10).ToList();
                }
                else
                {
                    var studentData = (from sd in context.StudentDetails
                                       where (sd.DeleteFlag == 0 || sd.DeleteFlag == null) && (sd.OnlineStatus == 0 || sd.OnlineApplicationStatus == 1)                                       
                                       select sd);

                    if (!string.IsNullOrEmpty(studentAdvanceSearch.NameTxt))
                        studentData = studentData.Where(x => x.FirstName.Contains(studentAdvanceSearch.NameTxt) || x.LastName.Contains(studentAdvanceSearch.NameTxt));
                    if (!string.IsNullOrEmpty(studentAdvanceSearch.AdmissionNumberTxt))
                        studentData = studentData.Where(x => x.AdmissionNo.Contains(studentAdvanceSearch.AdmissionNumberTxt));

                    int totalCount = studentData.Count();
                    totalPages = (int)Math.Ceiling((decimal)totalCount / (decimal)10);
                    studentList = studentData.OrderByDescending(x => x.CreatedDate).Skip((Convert.ToInt16(currentPageIndex) - 1) * 10).Take(10).ToList();
                }

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
    }
}
