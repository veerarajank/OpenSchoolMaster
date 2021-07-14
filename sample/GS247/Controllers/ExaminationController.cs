using GS247.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace GS247.Controllers
{
    public class ExaminationController : Controller
    {

        #region "INDEX"
        public ActionResult Index()
        {
            if (sessionValidate())
            {
                return View();
            }
            else
                return RedirectToAction("Index", "Login");
        }

        #endregion

        #region "EXAMS"

        public ActionResult OnlineExams()
        {
            if (sessionValidate())
            {
                using (var context = new GS247Entities8())
                {
                    var FieldsList = context.Examinations.Where(x => x.ExamType == 1);
                    int totalCount = FieldsList.Count();
                    var totalPages = (int)Math.Ceiling((decimal)totalCount / (decimal)10);
                    ViewBag.PageCount = totalPages;
                    ViewBag.CurrentPageIndex = 1;
                    var examList = FieldsList.OrderByDescending(x => x.CreatedDate).Take(10).ToList();

                    examList.ForEach(data =>
                    {
                        data.SubjectFlag = 0;
                        var questionMark = context.ExamQuestionPapers.Where(x => x.ExamId == data.ExamId).ToList();
                        if(questionMark.Count > 0)
                        {
                            if(data.TotalMarks == questionMark.Sum(x => x.Mark))
                                data.SubjectFlag = 1;                 
                        }                       
                        var batch = context.batches.Find(data.BatchId);
                        data.BatchName = batch != null ? batch.Name : string.Empty;
                        var course = context.Courses.Find(data.CourseId);
                        data.CourseName = course != null ? course.Name : string.Empty;
                    });
                    ViewBag.ExaminationsList = examList;
                }
                return View();
            }
            else
                return RedirectToAction("Index", "Login");
        }

        public ActionResult PagingOnlineExams(int CurrentPage)
        {
            if (sessionValidate())
            {
                using (var context = new GS247Entities8())
                {
                    var FieldsList = context.Examinations.Where(x => x.ExamType == 1);
                    int totalCount = FieldsList.Count();
                    var totalPages = (int)Math.Ceiling((decimal)totalCount / (decimal)10);
                    ViewBag.PageCount = totalPages;
                    ViewBag.CurrentPageIndex = CurrentPage;
                    var examList = FieldsList.OrderByDescending(x => x.CreatedDate).Skip((CurrentPage - 1) * 10).Take(10).ToList();

                    examList.ForEach(data =>
                    {
                        data.SubjectFlag = 0;
                        var questionMark = context.ExamQuestionPapers.Where(x => x.ExamId == data.ExamId).ToList();
                        if (questionMark.Count > 0)
                        {
                            if (Convert.ToDecimal(data.TotalMarks) == questionMark.Sum(x => x.Mark))
                                data.SubjectFlag = 1;
                        } 
                        var batch = context.batches.Find(data.BatchId);
                        data.BatchName = batch != null ? batch.Name : string.Empty;

                        var course = context.Courses.Find(data.CourseId);
                        data.CourseName = course != null ? course.Name : string.Empty;

                    });
                    ViewBag.ExaminationsList = examList;
                }
                return View("OnlineExams");
            }
            else
                return RedirectToAction("Index", "Login");
        }

        public ActionResult ExamResult(int ExamId)
        {
            if (sessionValidate())
            {
                using (var context = new GS247Entities8())
                {
                    var StudentExamDetailCOList = new List<StudentExamDetailCO>();

                    var exam = context.StudentExamDetails.Where(x => x.ExamId == ExamId);
                    int totalCount = exam.Count();
                    var totalPages = (int)Math.Ceiling((decimal)totalCount / (decimal)10);
                    ViewBag.PageCount = totalPages;
                    ViewBag.CurrentPageIndex = 1;
                    var examList = exam.OrderByDescending(x => x.CreatedDate).Take(10).ToList();

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

                        var examData = context.Examinations.Find(ExamId);
                        if (examData != null)
                        {
                            ViewBag.ExamName = examData.Name;
                            var student = context.StudentDetails.Find(data.StudentDetailsId);
                            if (student != null)
                            {
                                result.StudentName = student.FirstName + " " + student.LastName;
                                result.AdmissionNo = student.AdmissionNo;
                                var batch = context.batches.Find(student.Batch);
                                if (batch != null)
                                {
                                    var course = (from c in context.Courses
                                                  join cbl in context.BatchCourseLinks on c.Id equals cbl.CourseId
                                                  join b in context.batches on cbl.BatchId equals b.Id
                                                  where b.Id == batch.Id
                                                  select c).FirstOrDefault();

                                    result.CourseName = course.Name;
                                    result.BatchName = batch.Name;
                                }
                            }

                            if (examData.MarksType == 1)
                            {
                                result.TotalMark = data.TotalMark;
                            }
                            else if (examData.MarksType == 2)
                            {
                                int? mark = Convert.ToInt32(data.TotalMark);
                                var grade = context.ExamGradeLevels.Where(x => x.Score >= mark).OrderBy(y => y.Score).FirstOrDefault();
                                result.TotalMark = grade != null ? grade.Name : data.TotalMark;
                            }
                            else
                            {
                                int? mark = Convert.ToInt32(data.TotalMark);
                                var grade = context.ExamGradeLevels.Where(x => x.Score >= mark).OrderBy(y => y.Score).FirstOrDefault();
                                result.TotalMark = grade != null ? grade.Name + " - " + data.TotalMark : data.TotalMark;
                            }
                        }

                        StudentExamDetailCOList.Add(result);
                    });

                    ViewBag.StudentExamList = StudentExamDetailCOList;

                    ViewBag.ExamId = ExamId;
                }
                return View();
            }
            else
                return RedirectToAction("Index", "Login");
        }

        public ActionResult SearchExamResultList(SearchSFields SearchSFields)
        {
            if (sessionValidate())
            {
                var StudentExamDetailCOList = new List<StudentExamDetailCO>();
                using (var context = new GS247Entities8())
                {
                    var exam = context.StudentExamDetails.Where(x => x.ExamId == SearchSFields.ExamId);
                    int totalCount = exam.Count();
                    var totalPages = (int)Math.Ceiling((decimal)totalCount / (decimal)10);
                    ViewBag.PageCount = totalPages;
                    ViewBag.CurrentPageIndex = Convert.ToInt16(SearchSFields.CurrentPage);
                    var examList = exam.OrderByDescending(x => x.CreatedDate).Skip((Convert.ToInt16(SearchSFields.CurrentPage) - 1) * 10).Take(10).ToList();

                    examList.ForEach(data =>
                    {
                        var result = new StudentExamDetailCO();
                        result.StudentExamDetailsId = data.StudentExamDetailsId;
                        result.ExamId = data.ExamId;
                        result.StudentDetailsId = data.StudentDetailsId;
                        result.StartTime = data.StartTime;
                        result.EndTime = data.EndTime;
                        result.LoginId = data.LoginId;
                        result.StatusFlag = data.StatusFlag;
                        result.TotalMark = data.TotalMark;
                        result.CreatedBy = data.CreatedBy;
                        result.CreatedDate = data.CreatedDate;

                        var examData = context.Examinations.Find(SearchSFields.ExamId);
                        if (examData != null)
                        {
                            ViewBag.ExamName = examData.Name;
                            var student = context.StudentDetails.Find(data.StudentDetailsId);
                            if (student != null)
                            {
                                result.StudentName = student.FirstName + " " + student.LastName;
                                result.AdmissionNo = student.AdmissionNo;
                                var batch = context.batches.Find(student.Batch);
                                if (batch != null)
                                {
                                    var course = (from c in context.Courses
                                                  join cbl in context.BatchCourseLinks on c.Id equals cbl.CourseId
                                                  join b in context.batches on cbl.BatchId equals b.Id
                                                  where b.Id == batch.Id
                                                  select c).FirstOrDefault();

                                    result.CourseName = course.Name;
                                    result.BatchName = batch.Name;
                                }
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
                        StudentExamDetailCOList.Add(result);
                    });

                    ViewBag.StudentExamList = StudentExamDetailCOList;
                    ViewBag.ExamId = SearchSFields.ExamId;
                }
                return RedirectToAction("ExamResult", new RouteValueDictionary(
                         new { controller = "Examination", action = "ExamResult", ExamId = SearchSFields.ExamId }));
            }
            else
                return RedirectToAction("Index", "Login");
        }

        public class SearchSFields
        {
            public int ExamId { get; set; }

            public string CurrentPage { get; set; }
        }

        public ActionResult ViewQuestionPaper(int ExamId)
        {
            if (sessionValidate())
            {
                using (var context = new GS247Entities8())
                {
                    var examData = context.Examinations.Find(ExamId);
                    var questionPaperList = context.ExamQuestionPapers.Where(x => x.ExamId == ExamId).ToList();
                    var newList = new List<ExamQuestionPaperCO>();
                    questionPaperList.ForEach(data =>
                    {
                        var examQuestionPaperCO = new ExamQuestionPaperCO();
                        examQuestionPaperCO.ExamQuestionId = data.ExamQuestionId;
                        examQuestionPaperCO.ExamId = data.ExamId;
                        examQuestionPaperCO.QuestionName = data.QuestionName;
                        examQuestionPaperCO.QuestionType = data.QuestionType;
                        examQuestionPaperCO.Answer = data.Answer;
                        examQuestionPaperCO.Mark = data.Mark;
                        examQuestionPaperCO.CreatedDate = data.CreatedDate;
                        examQuestionPaperCO.Ststus = examData.Ststus;

                        if (data.QuestionType == 1)
                            examQuestionPaperCO.OptionValue = context.ExamQuestionTypeValues.Where(x => x.ExamQuestionId == data.ExamQuestionId).Select(x => x.Value).ToList();
                        newList.Add(examQuestionPaperCO);
                    });

                    var examList = new List<Examination>();
                    examList.Add(examData);
                    ViewBag.ExaminationsData = examList;
                    ViewBag.QuestionPaperList = newList;
                }
                return View();
            }
            else
                return RedirectToAction("Index", "Login");
        }

        public ActionResult CreateNewOnlineExams()
        {
            if (sessionValidate())
            {
                using (var context = new GS247Entities8())
                {
                    ViewBag.CoursesList = context.Courses.Where(x => x.IsEnable == true).ToList();
                    ViewBag.BatchList = context.batches.ToList();
                }

                return View();
            }
            else
                return RedirectToAction("Index", "Login");
        }

        [HttpPost]
        public ActionResult CreateNewOnlineExams(Examination collection)
        {
            if (sessionValidate())
            {
                using (var context = new GS247Entities8())
                {
                    collection.CreatedBy = Session["UserName"].ToString();
                    collection.CreatedDate = DateTime.Now;
                    collection.Ststus = 0;
                    collection.ExamType = 1;
                    collection.StaffId = Convert.ToInt32(Session["StaffId"].ToString());
                    context.Examinations.Add(collection);
                    context.SaveChanges();

                    return RedirectToAction("AddQuestion", new RouteValueDictionary(
                        new { controller = "Examination", action = "AddQuestion", ExamId = collection.ExamId }));
                }
            }
            else
                return RedirectToAction("Index", "Login");
        }

        [HttpPost]
        public ActionResult CreateNewCommonExams(Examination collection, string BatchIds)
        {
            if (sessionValidate())
            {
                using (var context = new GS247Entities8())
                {
                    collection.CreatedBy = Session["UserName"].ToString();
                    collection.CreatedDate = DateTime.Now;
                    collection.Ststus = 0;
                    collection.ExamType = 2;
                    collection.StaffId = Convert.ToInt32(Session["StaffId"].ToString());
                    context.Examinations.Add(collection);
                    context.SaveChanges();

                    if (collection.ExamType == 2)
                    {
                        List<int> list = BatchIds.Split(',').Select(int.Parse).ToList();                   

                        list.ForEach(data =>
                        {
                            var courseId = (from cbl in context.BatchCourseLinks
                                            join b in context.batches on cbl.BatchId equals b.Id
                                            where cbl.BatchId == data
                                            select cbl).FirstOrDefault();

                            var examCommonTypeCourse = new ExamCommonTypeCourse();
                            examCommonTypeCourse.ExamId = collection.ExamId;
                            if (courseId != null)
                                examCommonTypeCourse.CourseId = courseId.CourseId;
                            else
                                examCommonTypeCourse.CourseId = null;
                            examCommonTypeCourse.BatchId = data;
                            examCommonTypeCourse.SubjectId = collection.SubjectId;
                            examCommonTypeCourse.CreatedBy = Session["UserName"].ToString();
                            examCommonTypeCourse.CreatedDate = DateTime.Now;
                            context.ExamCommonTypeCourses.Add(examCommonTypeCourse);
                            context.SaveChanges();
                        });
                    }
                    return RedirectToAction("AddQuestion", new RouteValueDictionary(
                        new { controller = "Examination", action = "AddQuestion", ExamId = collection.ExamId }));
                }
            }
            else
                return RedirectToAction("Index", "Login");
        }

        public ActionResult CommonExams()
        {
            if (sessionValidate())
            {
                using (var context = new GS247Entities8())
                {
                    var FieldsList = context.Examinations.Where(x => x.ExamType == 2);
                    int totalCount = FieldsList.Count();
                    var totalPages = (int)Math.Ceiling((decimal)totalCount / (decimal)10);
                    ViewBag.PageCount = totalPages;
                    ViewBag.CurrentPageIndex = 1;
                    var examList = FieldsList.OrderByDescending(x => x.CreatedDate).Take(10).ToList();
                    examList.ForEach(data =>
                    {
                        data.SubjectFlag = 0;
                        var questionMark = context.ExamQuestionPapers.Where(x => x.ExamId == data.ExamId).ToList();
                        if (questionMark.Count > 0)
                        {
                            if (data.TotalMarks == questionMark.Sum(x => x.Mark))
                                data.SubjectFlag = 1;
                        }                       
                    });
                    ViewBag.ExaminationsList = examList;
                }
                return View();
            }
            else
                return RedirectToAction("Index", "Login");
        }

        public ActionResult PagingCommonExams(int CurrentPage)
        {
            if (sessionValidate())
            {
                using (var context = new GS247Entities8())
                {
                    var FieldsList = context.Examinations.Where(x => x.ExamType == 1);
                    int totalCount = FieldsList.Count();
                    var totalPages = (int)Math.Ceiling((decimal)totalCount / (decimal)10);
                    ViewBag.PageCount = totalPages;
                    ViewBag.CurrentPageIndex = CurrentPage;
                    var examList = FieldsList.OrderByDescending(x => x.CreatedDate).Skip((CurrentPage - 1) * 10).Take(10).ToList();
                    examList.ForEach(data =>
                    {
                        data.SubjectFlag = 0;
                        var questionMark = context.ExamQuestionPapers.Where(x => x.ExamId == data.ExamId).ToList();
                        if (questionMark.Count > 0)
                        {
                            if (data.TotalMarks == questionMark.Sum(x => x.Mark))
                                data.SubjectFlag = 1;
                        }
                    });
                    ViewBag.ExaminationsList = examList;
                }
                return View("CommonExams");
            }
            else
                return RedirectToAction("Index", "Login");
        }

        public ActionResult CreateCommonExams()
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

        public ActionResult AddExamSubject(int ExamId)
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

        public ActionResult ExamStartNew(int ExamId)
        {
            if (sessionValidate())
            {
                using (var context = new GS247Entities8())
                {
                    var newList = new List<ExamQuestionPaperCO>();
                    var examData = context.Examinations.Where(y => y.ExamId == ExamId).FirstOrDefault();
                    var questionPaperList = context.ExamQuestionPapers.Where(x => x.ExamId == ExamId).ToList();
                    questionPaperList.ForEach(data =>
                    {
                        var examQuestionPaperCO = new ExamQuestionPaperCO();
                        examQuestionPaperCO.ExamQuestionId = data.ExamQuestionId;
                        examQuestionPaperCO.ExamId = data.ExamId;
                        examQuestionPaperCO.QuestionName = data.QuestionName;
                        examQuestionPaperCO.QuestionType = data.QuestionType;
                        examQuestionPaperCO.Answer = data.Answer;
                        examQuestionPaperCO.Mark = data.Mark;
                        examQuestionPaperCO.CreatedDate = data.CreatedDate;
                        if (data.QuestionType == 1)
                            examQuestionPaperCO.OptionValue = context.ExamQuestionTypeValues.Where(x => x.ExamQuestionId == data.ExamQuestionId).Select(x => x.Value).ToList();
                        newList.Add(examQuestionPaperCO);

                    });


                    var examList = new List<Examination>();
                    examList.Add(examData);
                    ViewBag.ExaminationsData = examList;
                    ViewBag.QuestionPaperList = newList;

                    var studentExamDetail = new StudentExamDetail();
                    studentExamDetail.StudentDetailsId = 2;
                    studentExamDetail.LoginId = "1";
                    studentExamDetail.ExamId = ExamId;
                    studentExamDetail.StartTime = DateTime.Now;
                    studentExamDetail.StatusFlag = 0;
                    studentExamDetail.CreatedBy = Session["UserName"].ToString();
                    studentExamDetail.CreatedDate = DateTime.Now;
                    context.StudentExamDetails.Add(studentExamDetail);
                    context.SaveChanges();

                    ViewBag.StudentExamDetailsId = studentExamDetail.StudentExamDetailsId;
                }
                return View();
            }
            else
                return RedirectToAction("Index", "Login");
        }

        public ActionResult ExamStart(int ExamId)
        {
            if (sessionValidate())
            {
                var studentId = Session["UserName"].ToString();
                var studentDetails = new StudentDetail();
                using (var context = new GS247Entities8())
                {
                    var examData = context.Examinations.Find(ExamId);

                    if (examData != null)
                    {
                        if (examData.ExamType == 1)
                        {
                            studentDetails = (from a in context.StudentDetails
                                                  where a.StudentId == studentId && a.Batch == examData.BatchId && a.Course == examData.CourseId
                                                  select a).FirstOrDefault();
                        }
                        else
                        {
                            studentDetails = (from a in context.StudentDetails
                                              join b in context.ExamCommonTypeCourses on a.Batch equals b.BatchId
                                              where a.StudentId == studentId && a.Batch == b.BatchId && a.Course == b.CourseId
                                              && b.ExamId == ExamId
                                              select a).FirstOrDefault();
                        }

                        if(studentDetails != null)
                        {
                            var studentExamDetail = new StudentExamDetail();
                            studentExamDetail.StudentDetailsId = studentDetails.StudentDetailsId;
                            studentExamDetail.LoginId = "1";
                            studentExamDetail.ExamId = examData.ExamId;
                            studentExamDetail.StatusFlag = 0;
                            studentExamDetail.StartTime = DateTime.Now;
                            studentExamDetail.CreatedBy = Session["UserName"].ToString();
                            studentExamDetail.CreatedDate = DateTime.Now;
                            context.StudentExamDetails.Add(studentExamDetail);
                            context.SaveChanges();
                            ViewBag.StudentExamDetailsId = studentExamDetail.StudentExamDetailsId;
                            return View(examData);
                        }
                        else
                            return RedirectToAction("Index", "Login");
                    }
                    else
                        return View();
                }
            }
            else
                return RedirectToAction("Index", "Login");
        }

        [HttpPost]
        public JsonResult ExamStartList(int ExamId)
        {
            List<ExamQuestionPaperCO> QuestionPaperList = new List<ExamQuestionPaperCO>();
            var studentId = Session["UserName"].ToString();
            using (var context = new GS247Entities8())
            {
                var studentDetails = (from a in context.StudentDetails
                                      where a.StudentId == studentId
                                      select a).FirstOrDefault();

                if (studentDetails != null)
                {
                    var newList = new List<ExamQuestionPaperCO>();
                    var examData = context.Examinations.Find(ExamId);
                    if (examData != null)
                    {
                        var cnt = context.ExamQuestionPapers.Where(x => x.ExamId == examData.ExamId).Count();
                        var QuestionPaper = context.ExamQuestionPapers.Where(x => x.ExamId == examData.ExamId).OrderBy(x => x.ExamQuestionId).Take(1).ToList();
                        QuestionPaper.ForEach(data =>
                        {
                            var examQuestionPaperCO = new ExamQuestionPaperCO();
                            examQuestionPaperCO.ExamQuestionId = data.ExamQuestionId;
                            examQuestionPaperCO.ExamId = data.ExamId;
                            examQuestionPaperCO.QuestionName = data.QuestionName;
                            examQuestionPaperCO.QuestionType = data.QuestionType;
                            examQuestionPaperCO.Answer = data.Answer;
                            examQuestionPaperCO.Mark = data.Mark;
                            examQuestionPaperCO.CreatedDate = data.CreatedDate;
                            if (data.QuestionType == 1)
                                examQuestionPaperCO.OptionValue = context.ExamQuestionTypeValues.Where(x => x.ExamQuestionId == data.ExamQuestionId).Select(x => x.Value).ToList();
                            if (cnt == 1)
                                examQuestionPaperCO.StatusFlagDesc = "3";
                            QuestionPaperList.Add(examQuestionPaperCO);
                        });
                    }
                }
            }
            return Json(new { QuestionPaperList = QuestionPaperList }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ExamStartListNext(ExamStudentAnswer examAnswerSheet, int Count, int ExamId)
        {
            List<ExamQuestionPaperCO> QuestionPaperList = new List<ExamQuestionPaperCO>();
            var studentId = Session["UserName"].ToString();

            StudentOnlineExamAnswerSheet(examAnswerSheet);

            using (var context = new GS247Entities8())
            {

                var cnt = context.ExamQuestionPapers.Where(x => x.ExamId == ExamId).Count();
                var questionPaper = context.ExamQuestionPapers.Where(x => x.ExamId == ExamId).OrderBy(x => x.ExamQuestionId).Skip(Count - 1).Take(1).ToList();
                questionPaper.ForEach(data =>
                {
                    var examQuestionPaperCO = new ExamQuestionPaperCO();
                    examQuestionPaperCO.ExamQuestionId = data.ExamQuestionId;
                    examQuestionPaperCO.ExamId = data.ExamId;
                    examQuestionPaperCO.QuestionName = data.QuestionName;
                    examQuestionPaperCO.QuestionType = data.QuestionType;
                    examQuestionPaperCO.Answer = data.Answer;
                    examQuestionPaperCO.Mark = data.Mark;
                    examQuestionPaperCO.CreatedDate = data.CreatedDate;

                    var studentExamAnswerDetails = context.StudentExamAnswerDetails.Where(x => x.StudentExamDetailsId == examAnswerSheet.StudentExamDetailsId
                 && x.ExamQuestionId == data.ExamQuestionId).FirstOrDefault();
                    if (studentExamAnswerDetails != null)
                        examQuestionPaperCO.Answer = studentExamAnswerDetails.Answer;
                    else
                        examQuestionPaperCO.Answer = null;

                    if (data.QuestionType == 1)
                        examQuestionPaperCO.OptionValue = context.ExamQuestionTypeValues.Where(x => x.ExamQuestionId == data.ExamQuestionId).Select(x => x.Value).ToList();
                    if (cnt == Count)
                        examQuestionPaperCO.StatusFlagDesc = "2";
                    else if (Count == 1)
                        examQuestionPaperCO.StatusFlagDesc = "1";
                    else
                        examQuestionPaperCO.StatusFlagDesc = "0";

                    QuestionPaperList.Add(examQuestionPaperCO);
                });

            }

            return Json(new { QuestionPaperList = QuestionPaperList }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ExamStartListBack(int Count, int ExamId, int StudentExamDetailsId)
        {
            List<ExamQuestionPaperCO> QuestionPaperList = new List<ExamQuestionPaperCO>();
            var studentId = Session["UserName"].ToString();
            using (var context = new GS247Entities8())
            {
                var cnt = context.ExamQuestionPapers.Where(x => x.ExamId == ExamId).Count();
                var questionPaper = context.ExamQuestionPapers.Where(x => x.ExamId == ExamId).OrderBy(x => x.ExamQuestionId).Skip(Count - 1).Take(1).ToList();
                questionPaper.ForEach(data =>
                {
                    var examQuestionPaperCO = new ExamQuestionPaperCO();
                    examQuestionPaperCO.ExamQuestionId = data.ExamQuestionId;
                    examQuestionPaperCO.ExamId = data.ExamId;
                    examQuestionPaperCO.QuestionName = data.QuestionName;
                    examQuestionPaperCO.QuestionType = data.QuestionType;
                    examQuestionPaperCO.Answer = data.Answer;
                    examQuestionPaperCO.Mark = data.Mark;
                    examQuestionPaperCO.QuestionNo = data.QuestionNo;
                    examQuestionPaperCO.CreatedDate = data.CreatedDate;

                    var studentExamAnswerDetails = context.StudentExamAnswerDetails.Where(x => x.StudentExamDetailsId == StudentExamDetailsId
                 && x.ExamQuestionId == data.ExamQuestionId).FirstOrDefault();
                    if (studentExamAnswerDetails != null)
                        examQuestionPaperCO.Answer = studentExamAnswerDetails.Answer;
                    else
                        examQuestionPaperCO.Answer = null;

                    if (data.QuestionType == 1)
                        examQuestionPaperCO.OptionValue = context.ExamQuestionTypeValues.Where(x => x.ExamQuestionId == data.ExamQuestionId).Select(x => x.Value).ToList();
                    if (cnt == Count)
                        examQuestionPaperCO.StatusFlagDesc = "2";
                    else if (Count == 1)
                        examQuestionPaperCO.StatusFlagDesc = "1";
                    else
                        examQuestionPaperCO.StatusFlagDesc = "0";

                    QuestionPaperList.Add(examQuestionPaperCO);

                });

            }

            return Json(new { QuestionPaperList = QuestionPaperList }, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> StudentOnlineExamAnswerSheet(ExamStudentAnswer examAnswerSheet)
        {
            if (sessionValidate())
            {
                using (var context = new GS247Entities8())
                {
                    var data = context.StudentExamAnswerDetails.Where(x => x.StudentExamDetailsId == examAnswerSheet.StudentExamDetailsId && x.ExamQuestionId == examAnswerSheet.ExamQuestionId).FirstOrDefault();
                    if (data != null)
                    {
                        if (examAnswerSheet.Answer != null)
                            data.Answer = examAnswerSheet.Answer;
                        if (!string.IsNullOrEmpty(examAnswerSheet.Answer))
                        {
                            var examQuesPaper = context.ExamQuestionPapers.Find(examAnswerSheet.ExamQuestionId);
                            if (examQuesPaper != null)
                            {
                                if (examQuesPaper.Answer.ToLower() == examAnswerSheet.Answer.ToLower())
                                {
                                    data.AnswerCorrectOrWrong = 1;
                                    data.Mark = examQuesPaper.Mark.ToString();
                                }
                                else
                                {
                                    data.AnswerCorrectOrWrong = 0;
                                    data.Mark = "0";
                                }
                            }
                        }
                        else
                        {
                            data.AnswerCorrectOrWrong = 0;
                            data.Mark = "0";
                        }
                        context.Entry(data).State = EntityState.Modified;
                        context.SaveChanges();
                    }
                    else
                    {
                        var studentExamDetail = new StudentExamAnswerDetail();
                        studentExamDetail.StudentExamDetailsId = examAnswerSheet.StudentExamDetailsId;
                        studentExamDetail.ExamQuestionId = examAnswerSheet.ExamQuestionId;
                        studentExamDetail.Answer = examAnswerSheet.Answer;

                        if (!string.IsNullOrEmpty(examAnswerSheet.Answer))
                        {
                            var examQuesPaper = context.ExamQuestionPapers.Find(examAnswerSheet.ExamQuestionId);
                            if (examQuesPaper != null)
                            {
                                if (examQuesPaper.Answer.ToLower() == examAnswerSheet.Answer.ToLower())
                                {
                                    studentExamDetail.AnswerCorrectOrWrong = 1;
                                    studentExamDetail.Mark = examQuesPaper.Mark.ToString();
                                }
                                else
                                {
                                    studentExamDetail.AnswerCorrectOrWrong = 0;
                                    studentExamDetail.Mark = "0";
                                }
                            }
                        }
                        else
                        {
                            studentExamDetail.AnswerCorrectOrWrong = 0;
                            studentExamDetail.Mark = "0";
                        }
                        studentExamDetail.CreatedBy = Session["UserName"].ToString();
                        studentExamDetail.CreatedDate = DateTime.Now;
                        context.StudentExamAnswerDetails.Add(studentExamDetail);
                        context.SaveChanges();
                    }
                    var examStudentDetails = context.StudentExamDetails.Find(examAnswerSheet.StudentExamDetailsId);

                    if (examStudentDetails != null)
                    {
                        var totalMark = context.StudentExamAnswerDetails.Where(x => x.StudentExamDetailsId == examAnswerSheet.StudentExamDetailsId).ToList().Sum(x => Convert.ToDouble(x.Mark));
                        examStudentDetails.TotalMark = totalMark.ToString();
                        examStudentDetails.EndTime = DateTime.Now;
                        examStudentDetails.UpdatedDate = DateTime.Now;
                        examStudentDetails.UpdatedBy = Session["UserName"].ToString();
                        context.Entry(examStudentDetails).State = EntityState.Modified;
                        context.SaveChanges();
                    }
                    await context.SaveChangesAsync();
                }
                return new EmptyResult();
            }
            else
                return RedirectToAction("Index", "Login");
        }

        [HttpPost]
        public async Task<ActionResult> StudentOnlineExamAnswer(ExamStudentAnswer examAnswerSheet)
        {
            if (sessionValidate())
            {
                using (var context = new GS247Entities8())
                {
                    //var studentExamDetail = new StudentExamAnswerDetail();
                    //studentExamDetail.StudentExamDetailsId = examAnswerSheet.StudentExamDetailsId;
                    //studentExamDetail.ExamQuestionId = examAnswerSheet.ExamQuestionId;
                    //studentExamDetail.Answer = examAnswerSheet.Answer;

                    //if (!string.IsNullOrEmpty(examAnswerSheet.Answer))
                    //{
                    //    var examQuesPaper = context.ExamQuestionPapers.Find(examAnswerSheet.ExamQuestionId);
                    //    if (examQuesPaper != null)
                    //    {
                    //        if (examQuesPaper.Answer.ToLower() == examAnswerSheet.Answer.ToLower())
                    //        {
                    //            studentExamDetail.AnswerCorrectOrWrong = 1;
                    //            studentExamDetail.Mark = examQuesPaper.Mark.ToString();
                    //        }
                    //        else
                    //        {
                    //            studentExamDetail.AnswerCorrectOrWrong = 0;
                    //            studentExamDetail.Mark = "0";
                    //        }
                    //    }
                    //}
                    //else
                    //{
                    //    studentExamDetail.AnswerCorrectOrWrong = 0;
                    //    studentExamDetail.Mark = "0";
                    //}
                    //studentExamDetail.CreatedBy = Session["UserName"].ToString();
                    //studentExamDetail.CreatedDate = DateTime.Now;
                    //context.StudentExamAnswerDetails.Add(studentExamDetail);
                    //context.SaveChanges();

                    //var examStudentDetails = context.StudentExamDetails.Find(examAnswerSheet.StudentExamDetailsId);

                    //if (examStudentDetails != null)
                    //{
                    //    var totalMark = context.StudentExamAnswerDetails.Where(x => x.StudentExamDetailsId == examAnswerSheet.StudentExamDetailsId).ToList().Sum(x => Convert.ToDouble(x.Mark));
                    //    examStudentDetails.TotalMark = totalMark.ToString();
                    //    examStudentDetails.EndTime = DateTime.Now;
                    //    examStudentDetails.UpdatedDate = DateTime.Now;
                    //    examStudentDetails.UpdatedBy = Session["UserName"].ToString();
                    //    context.Entry(examStudentDetails).State = EntityState.Modified;
                    //    context.SaveChanges();
                    //}

                    var data = context.StudentExamAnswerDetails.Where(x => x.StudentExamDetailsId == examAnswerSheet.StudentExamDetailsId && x.ExamQuestionId == examAnswerSheet.ExamQuestionId).FirstOrDefault();
                    if (data != null)
                    {
                        if (examAnswerSheet.Answer != null)
                            data.Answer = examAnswerSheet.Answer;
                        if (!string.IsNullOrEmpty(examAnswerSheet.Answer))
                        {
                            var examQuesPaper = context.ExamQuestionPapers.Find(examAnswerSheet.ExamQuestionId);
                            if (examQuesPaper != null)
                            {
                                if (examQuesPaper.Answer.ToLower() == examAnswerSheet.Answer.ToLower())
                                {
                                    data.AnswerCorrectOrWrong = 1;
                                    data.Mark = examQuesPaper.Mark.ToString();
                                }
                                else
                                {
                                    data.AnswerCorrectOrWrong = 0;
                                    data.Mark = "0";
                                }
                            }
                        }
                        else
                        {
                            data.AnswerCorrectOrWrong = 0;
                            data.Mark = "0";
                        }
                        context.Entry(data).State = EntityState.Modified;
                        context.SaveChanges();
                    }
                    else
                    {
                        var studentExamDetail = new StudentExamAnswerDetail();
                        studentExamDetail.StudentExamDetailsId = examAnswerSheet.StudentExamDetailsId;
                        studentExamDetail.ExamQuestionId = examAnswerSheet.ExamQuestionId;
                        studentExamDetail.Answer = examAnswerSheet.Answer;

                        if (!string.IsNullOrEmpty(examAnswerSheet.Answer))
                        {
                            var examQuesPaper = context.ExamQuestionPapers.Find(examAnswerSheet.ExamQuestionId);
                            if (examQuesPaper != null)
                            {
                                if (examQuesPaper.Answer.ToLower() == examAnswerSheet.Answer.ToLower())
                                {
                                    studentExamDetail.AnswerCorrectOrWrong = 1;
                                    studentExamDetail.Mark = examQuesPaper.Mark.ToString();
                                }
                                else
                                {
                                    studentExamDetail.AnswerCorrectOrWrong = 0;
                                    studentExamDetail.Mark = "0";
                                }
                            }
                        }
                        else
                        {
                            studentExamDetail.AnswerCorrectOrWrong = 0;
                            studentExamDetail.Mark = "0";
                        }
                        studentExamDetail.CreatedBy = Session["UserName"].ToString();
                        studentExamDetail.CreatedDate = DateTime.Now;
                        context.StudentExamAnswerDetails.Add(studentExamDetail);
                        context.SaveChanges();
                    }
                    var examStudentDetails = context.StudentExamDetails.Find(examAnswerSheet.StudentExamDetailsId);

                    if (examStudentDetails != null)
                    {
                        var totalMark = context.StudentExamAnswerDetails.Where(x => x.StudentExamDetailsId == examAnswerSheet.StudentExamDetailsId).ToList().Sum(x => Convert.ToDouble(x.Mark));
                        examStudentDetails.TotalMark = totalMark.ToString();
                        examStudentDetails.EndTime = DateTime.Now;
                        examStudentDetails.UpdatedDate = DateTime.Now;
                        examStudentDetails.UpdatedBy = Session["UserName"].ToString();
                        context.Entry(examStudentDetails).State = EntityState.Modified;
                        context.SaveChanges();
                    }

                    await context.SaveChangesAsync();
                }
                return new EmptyResult();
            }
            else
                return RedirectToAction("Index", "Login");
        }

        [HttpPost]
        public async Task<ActionResult> StudentOnlineExamAnswerOld(ExamAnswerSheet examAnswerSheet)
        {
            if (sessionValidate())
            {
                using (var context = new GS247Entities8())
                {
                    if (examAnswerSheet.ExamStudentAnswer.Count > 0)
                    {
                        decimal? mark = 0;
                        examAnswerSheet.ExamStudentAnswer.ForEach(data =>
                        {

                            var studentExamDetail = new StudentExamAnswerDetail();
                            studentExamDetail.StudentExamDetailsId = examAnswerSheet.StudentExamDetailsId;
                            studentExamDetail.ExamQuestionId = data.ExamQuestionId;
                            studentExamDetail.Answer = data.Answer;

                            if (!string.IsNullOrEmpty(data.Answer))
                            {
                                var examQuesPaper = context.ExamQuestionPapers.Find(data.ExamQuestionId);
                                if (examQuesPaper != null)
                                {
                                    if (examQuesPaper.Answer.ToLower() == data.Answer.ToLower())
                                    {
                                        studentExamDetail.AnswerCorrectOrWrong = 1;
                                        studentExamDetail.Mark = examQuesPaper.Mark.ToString();
                                        mark += examQuesPaper.Mark;
                                    }
                                    else
                                    {
                                        studentExamDetail.AnswerCorrectOrWrong = 0;
                                        studentExamDetail.Mark = "0";
                                    }
                                }
                            }
                            else
                            {
                                studentExamDetail.AnswerCorrectOrWrong = 0;
                                studentExamDetail.Mark = "0";
                            }

                            studentExamDetail.CreatedBy = Session["UserName"].ToString();
                            studentExamDetail.CreatedDate = DateTime.Now;
                            context.StudentExamAnswerDetails.Add(studentExamDetail);
                            context.SaveChanges();

                        });

                        var examStudentDetails = context.StudentExamDetails.Find(examAnswerSheet.StudentExamDetailsId);

                        if (examStudentDetails != null)
                        {
                            examStudentDetails.TotalMark = mark.ToString();
                            examStudentDetails.EndTime = DateTime.Now;
                            examStudentDetails.UpdatedDate = DateTime.Now;
                            examStudentDetails.UpdatedBy = Session["UserName"].ToString();
                            context.Entry(examStudentDetails).State = EntityState.Modified;
                            context.SaveChanges();
                        }

                        await context.SaveChangesAsync();
                    }

                    return new EmptyResult();
                }
            }
            else
                return RedirectToAction("Index", "Login");
        }

        [HttpPost]
        public ActionResult updateStatus(int ExamId, int Status)
        {
            if (sessionValidate())
            {
                using (var context = new GS247Entities8())
                {
                    var data = context.Examinations.Find(ExamId);
                    if (data != null)
                    {
                        data.Ststus = Status;
                        data.UpdatedDate = DateTime.Now;
                        data.UpdatedBy = Session["UserName"].ToString();
                        context.Entry(data).State = EntityState.Modified;
                        context.SaveChanges();
                    }
                }
                return new EmptyResult();
            }
            else
                return RedirectToAction("Index", "Login");

        }

        public ActionResult AnswerSheetList(int ExamId)
        {
            if (sessionValidate())
            {
                var StudentExamDetailCOList = new List<StudentExamDetailCO>();
                using (var context = new GS247Entities8())
                {
                    var exam = context.StudentExamDetails.Where(x => x.ExamId == ExamId);
                    int totalCount = exam.Count();
                    var totalPages = (int)Math.Ceiling((decimal)totalCount / (decimal)10);
                    ViewBag.PageCount = totalPages;
                    ViewBag.CurrentPageIndex = 1;
                    var examList = exam.OrderByDescending(x => x.CreatedDate).Take(10).ToList();

                    examList.ForEach(data =>
                    {
                        var result = new StudentExamDetailCO();
                        result.StudentExamDetailsId = data.StudentExamDetailsId;
                        result.ExamId = data.ExamId;
                        result.StudentDetailsId = data.StudentDetailsId;
                        result.StartTime = data.StartTime;
                        result.EndTime = data.EndTime;
                        result.LoginId = data.LoginId;
                        result.StatusFlag = data.StatusFlag;
                        result.TotalMark = data.TotalMark;
                        result.CreatedBy = data.CreatedBy;
                        result.CreatedDate = data.CreatedDate;

                        var examData = context.Examinations.Find(ExamId);
                        if (examData != null)
                        {
                            ViewBag.ExamName = examData.Name;
                            var student = context.StudentDetails.Find(data.StudentDetailsId);
                            if (student != null)
                            {
                                result.StudentName = student.FirstName + " " + student.LastName;
                                result.AdmissionNo = student.AdmissionNo;
                                var batch = context.batches.Find(student.Batch);
                                if (batch != null)
                                {
                                    var course = (from c in context.Courses
                                                  join cbl in context.BatchCourseLinks on c.Id equals cbl.CourseId
                                                  join b in context.batches on cbl.BatchId equals b.Id
                                                  where b.Id == batch.Id
                                                  select c).FirstOrDefault();

                                    result.CourseName = course.Name;
                                    result.BatchName = batch.Name;
                                }
                            }


                            if (examData.SubjectFlag == 1)
                            {
                                var Subject = context.Subjects.Find(examData.SubjectId);
                                result.SubjectName = Subject != null ? Subject.Name : string.Empty;
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

                        StudentExamDetailCOList.Add(result);

                    });

                    ViewBag.StudentExamList = StudentExamDetailCOList;

                    ViewBag.ExamId = ExamId;
                }
                return View();
            }
            else
                return RedirectToAction("Index", "Login");
        }

        public ActionResult SearchAnswerSheetList(SearchSFields SearchSFields)
        {
            if (sessionValidate())
            {
                var StudentExamDetailCOList = new List<StudentExamDetailCO>();
                using (var context = new GS247Entities8())
                {
                    var exam = context.StudentExamDetails.Where(x => x.ExamId == SearchSFields.ExamId);
                    int totalCount = exam.Count();
                    var totalPages = (int)Math.Ceiling((decimal)totalCount / (decimal)10);
                    ViewBag.PageCount = totalPages;
                    ViewBag.CurrentPageIndex = Convert.ToInt16(SearchSFields.CurrentPage);
                    var examList = exam.OrderByDescending(x => x.CreatedDate).Skip((Convert.ToInt16(SearchSFields.CurrentPage) - 1) * 10).Take(10).ToList();

                    examList.ForEach(data =>
                    {
                        var result = new StudentExamDetailCO();
                        result.StudentExamDetailsId = data.StudentExamDetailsId;
                        result.ExamId = data.ExamId;
                        result.StudentDetailsId = data.StudentDetailsId;
                        result.StartTime = data.StartTime;
                        result.EndTime = data.EndTime;
                        result.LoginId = data.LoginId;
                        result.StatusFlag = data.StatusFlag;
                        result.TotalMark = data.TotalMark;
                        result.CreatedBy = data.CreatedBy;
                        result.CreatedDate = data.CreatedDate;

                        var examData = context.Examinations.Find(SearchSFields.ExamId);
                        if (examData != null)
                        {
                            ViewBag.ExamName = examData.Name;
                            var student = context.StudentDetails.Find(data.StudentDetailsId);
                            if (student != null)
                            {
                                result.StudentName = student.FirstName + " " + student.LastName;
                                result.AdmissionNo = student.AdmissionNo;
                                var batch = context.batches.Find(student.Batch);
                                if (batch != null)
                                {
                                    var course = (from c in context.Courses
                                                  join cbl in context.BatchCourseLinks on c.Id equals cbl.CourseId
                                                  join b in context.batches on cbl.BatchId equals b.Id
                                                  where b.Id == batch.Id
                                                  select c).FirstOrDefault();

                                    result.CourseName = course.Name;
                                    result.BatchName = batch.Name;
                                }
                            }


                            if (examData.SubjectFlag == 1)
                            {
                                var Subject = context.Subjects.Find(examData.SubjectId);
                                result.SubjectName = Subject != null ? Subject.Name : string.Empty;
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

                        StudentExamDetailCOList.Add(result);
                    });

                    ViewBag.StudentExamList = StudentExamDetailCOList;
                    ViewBag.ExamId = SearchSFields.ExamId;
                }
                return RedirectToAction("AnswerSheetList", new RouteValueDictionary(
                         new { controller = "Examination", action = "AnswerSheetList", ExamId = SearchSFields.ExamId }));
            }
            else
                return RedirectToAction("Index", "Login");
        }

        public ActionResult QuestionPaperCorrection(int StudentExamDetailsId)
        {
            if (sessionValidate())
            {
                using (var context = new GS247Entities8())
                {
                    var studentData = (from x in context.StudentExamAnswerDetails
                                       join y in context.StudentExamDetails on x.StudentExamDetailsId equals y.StudentExamDetailsId
                                       join z in context.Examinations on y.ExamId equals z.ExamId
                                       join a in context.ExamQuestionPapers on z.ExamId equals a.ExamId
                                       where y.StudentExamDetailsId == StudentExamDetailsId && a.QuestionType > 2
                                       && x.ExamQuestionId == a.ExamQuestionId && y.ExamId == a.ExamId
                                       select new StudentExamAnswerDetailsCO
                                       {
                                           StudentExamAnswerDetailsId = x.StudentExamAnswerDetailsId,
                                           StudentExamDetailsId = y.StudentExamDetailsId,
                                           ExamQuestionId = x.ExamQuestionId,
                                           Answer = x.Answer,
                                           QuestionAnswer = a.Answer,
                                           QuestionMark = a.Mark,
                                           QuestionName = a.QuestionName,
                                           QuestionType = a.QuestionType,
                                           QuestionNo = a.QuestionNo

                                       }).ToList();

                    var studentAnswerList = studentData.GroupBy(x => x.ExamQuestionId).Select(data => data.First()).ToList();

                    var Data = context.StudentExamDetails.Find(StudentExamDetailsId);
                    if (Data != null)
                    {
                        var student = context.StudentDetails.Find(Data.StudentDetailsId);
                        ViewBag.StudentName = student != null ? student.FirstName + " " + student.LastName : string.Empty;
                        var examination = context.Examinations.Find(Data.ExamId);
                        ViewBag.ExamId = Data.ExamId;
                        ViewBag.ExamName = examination != null ? examination.Name : "";                         
                        if (student != null)
                        {
                            var course = context.Courses.Find(student.Course);
                            ViewBag.CourseName = course != null ? course.Name : string.Empty;
                            var batch = context.batches.Find(student.Batch);
                            ViewBag.BatchName = batch != null ? batch.Name : string.Empty;
                        }
                    }
                    ViewBag.StudentAnswerSheetList = studentAnswerList;
                    ViewBag.StudentExamDetailsId = StudentExamDetailsId;
                }
                return View();
            }
            else
                return RedirectToAction("Index", "Login");
        }

        public ActionResult StudentAnswerSheet()
        {
            if (sessionValidate())
            {
                var studentId = Session["UserName"].ToString();
                using (var context = new GS247Entities8())
                {
                    var studentData = context.StudentDetails.Where(x => x.StudentId == studentId).FirstOrDefault();
                    if (studentData != null)
                    {
                        var examData = context.Examinations.Where(y => y.BatchId == studentData.Batch && y.Ststus == 1).FirstOrDefault();
                        if (examData != null)
                        {
                            var SchoolCO1 = new SchoolCO();
                            var questionPaper = context.ExamQuestionPapers.Where(x => x.ExamId == examData.ExamId);
                            var studentExamDetail = new StudentExamDetail();
                            studentExamDetail.StudentDetailsId = studentData.StudentDetailsId;
                            studentExamDetail.LoginId = "1";
                            studentExamDetail.ExamId = examData.ExamId;
                            studentExamDetail.StatusFlag = 0;
                            studentExamDetail.StartTime = DateTime.Now;
                            studentExamDetail.CreatedBy = Session["UserName"].ToString();
                            studentExamDetail.CreatedDate = DateTime.Now;
                            context.StudentExamDetails.Add(studentExamDetail);
                            context.SaveChanges();
                            ViewBag.StudentExamDetailsId = studentExamDetail.StudentExamDetailsId;
                        }
                        return View(examData);
                    }
                    else
                        return View();
                }
            }
            else
                return RedirectToAction("Index", "Login");
        }

        [HttpPost]
        public async Task<ActionResult> StudentOnlineExamAnswerEvaluate(ExamAnswerSheet examAnswerSheet)
        {
            if (sessionValidate())
            {
                using (var context = new GS247Entities8())
                {
                    examAnswerSheet.StudentExamAnswerDetailsCO.ForEach(data =>
                    {
                        var answerData = context.StudentExamAnswerDetails.Find(data.StudentExamAnswerDetailsId);
                        if (answerData != null)
                        {
                            if (data.Mark == "" || data.Mark == null)
                                data.Mark = "0";
                            answerData.Mark = data.Mark;
                            answerData.UpdatedDate = DateTime.Now;
                            answerData.UpdatedBy = Session["UserName"].ToString();
                            context.Entry(answerData).State = EntityState.Modified;
                            context.SaveChanges();
                        }

                    });

                    var examStudentDetails = context.StudentExamDetails.Find(examAnswerSheet.StudentExamDetailsId);

                    if (examStudentDetails != null)
                    {
                        var totalMark = context.StudentExamAnswerDetails.Where(x => x.StudentExamDetailsId == examAnswerSheet.StudentExamDetailsId).ToList().Sum(x => Convert.ToDouble(x.Mark));
                        examStudentDetails.TotalMark = totalMark.ToString();
                        examStudentDetails.EndTime = DateTime.Now;
                        examStudentDetails.StatusFlag = 1;
                        examStudentDetails.UpdatedDate = DateTime.Now;
                        examStudentDetails.UpdatedBy = Session["UserName"].ToString();
                        context.Entry(examStudentDetails).State = EntityState.Modified;
                        context.SaveChanges();
                    }
                    await context.SaveChangesAsync();

                    return new EmptyResult();
                }
            }
            else
                return RedirectToAction("Index", "Login");
        }

        [HttpPost]
        public JsonResult SubjectListLoad(int? Id)
        {
            List<Subject> SubjectList = new List<Subject>();
            using (var context = new GS247Entities8())
            {
                SubjectList = (from cbl in context.BatchSubjectsLinks
                               join b in context.Subjects on cbl.SubjectsId equals b.Id
                               where cbl.BatchId == Id
                               select b).ToList();
            }
            return Json(new { SubjectList = SubjectList }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult SubjectList()
        {
            List<Subject> SubjectList = new List<Subject>();
            using (var context = new GS247Entities8())
            {
                SubjectList = (from cbl in context.Subjects
                               select cbl).ToList();
            }
            return Json(new { SubjectList = SubjectList }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SubjectCourse(int? id)
        {
            List<Cours> CourseList = new List<Cours>();
            using (var context = new GS247Entities8())
            {
                CourseList = (from c in context.Courses
                              join cbl in context.BatchCourseLinks on c.Id equals cbl.CourseId
                              join b in context.batches on cbl.BatchId equals b.Id
                              join x in context.BatchSubjectsLinks on b.Id equals x.BatchId
                              where x.SubjectsId == id
                              select c).ToList();
            }
            return Json(new { CourseList = CourseList }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SubjectCourseBatch(int? CourseId, int? SubjectId)
        {
            List<batch> BatchList = new List<batch>();
            using (var context = new GS247Entities8())
            {
                BatchList = (from c in context.Courses
                             join cbl in context.BatchCourseLinks on c.Id equals cbl.CourseId
                             join b in context.batches on cbl.BatchId equals b.Id
                             join x in context.BatchSubjectsLinks on b.Id equals x.BatchId
                             where x.SubjectsId == SubjectId && cbl.CourseId == CourseId
                             select b).ToList();
            }
            return Json(new { BatchList = BatchList }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region "CBSE EXAM SETTINGS"

        public ActionResult CreateCBSEExamSetting()
        {
            if (sessionValidate())
            {
                return View();
            }
            else
                return RedirectToAction("Index", "Login");
        }

        [HttpPost]
        public ActionResult CreateCBSEExamSetting(CBSEExamSetting collection)
        {
            if (sessionValidate())
            {
                int id = Convert.ToInt32(Session["UserId"].ToString());
                using (var context = new GS247Entities8())
                {
                    collection.CreatedBy = Session["UserName"].ToString();
                    collection.CreatedDate = DateTime.Now;
                    collection.UserId = id;
                    context.CBSEExamSettings.Add(collection);
                    context.SaveChanges();
                }
                return RedirectToRoute(new { controller = "Examination", action = "CBSEExamSetting" });
            }
            else
                return RedirectToAction("Index", "Login");
        }

        [HttpPost]
        public ActionResult UpdateCBSEExamSetting(CBSEExamSetting collection)
        {
            if (sessionValidate())
            {
                using (var context = new GS247Entities8())
                {
                    var _templist = context.CBSEExamSettings.Find(collection.CBSEExamSettingId);
                    if (_templist != null)
                    {
                        _templist.FA1Weightage = collection.FA1Weightage;
                        _templist.FA2Weightage = collection.FA2Weightage;
                        _templist.SA1Weightage = collection.SA1Weightage;
                        _templist.SA2Weightage = collection.SA2Weightage;
                        _templist.UpdatedDate = DateTime.Now;
                        _templist.UpdatedBy = Session["UserName"].ToString();
                        context.Entry(_templist).State = EntityState.Modified;
                        context.SaveChanges();
                    }
                }
                return RedirectToRoute(new { controller = "Examination", action = "CBSEExamSetting" });
            }
            else
                return RedirectToAction("Index", "Login");
        }

        [HttpGet]
        public ActionResult CreateCBSEExamSetting(int? CBSEExamSettingId)
        {
            if (sessionValidate())
            {
                CBSEExamSetting _templist = new CBSEExamSetting();
                using (var context = new GS247Entities8())
                {
                    _templist = context.CBSEExamSettings.Find(CBSEExamSettingId);
                }
                return View(_templist);
            }
            else
                return RedirectToAction("Index", "Login");
        }

        public ActionResult CBSEExamSetting()
        {
            if (sessionValidate())
            {
                CBSEExamSetting _templist = new CBSEExamSetting();
                using (var context = new GS247Entities8())
                {
                    _templist = context.CBSEExamSettings.FirstOrDefault();
                }
                return View(_templist);
            }
            else
                return RedirectToAction("Index", "Login");
        }

        #endregion

        #region "GRADE LEVEL"

        public ActionResult SetGradeList()
        {
            if (sessionValidate())
            {
                using (var context = new GS247Entities8())
                {
                    ViewBag.GradeList = context.ExamGradeLevels.ToList();
                }
                return View();
            }
            else
                return RedirectToAction("Index", "Login");
        }

        [HttpPost]
        public ActionResult SaveGradeLevel(ExamGradeLevel examGradeLevel)
        {
            if (sessionValidate())
            {
                using (var context = new GS247Entities8())
                {
                    var data = context.ExamGradeLevels.Find(examGradeLevel.ExamGradeLevelId);
                    if (data == null)
                    {
                        examGradeLevel.CreatedBy = Session["UserName"].ToString();
                        examGradeLevel.CreatedDate = DateTime.Now;
                        context.ExamGradeLevels.Add(examGradeLevel);
                        context.SaveChanges();
                    }
                    else
                    {
                        data.Name = examGradeLevel.Name;
                        data.Score = examGradeLevel.Score;
                        data.UpdatedDate = DateTime.Now;
                        data.UpdatedBy = Session["UserName"].ToString();
                        context.Entry(data).State = EntityState.Modified;
                        context.SaveChanges();
                    }

                }
                return new EmptyResult();
            }
            else
                return RedirectToAction("Index", "Login");
        }

        [HttpPost]
        public ActionResult DeleteGradeLevel(int ExamGradeLevelId)
        {
            using (var context = new GS247Entities8())
            {
                var data = context.ExamGradeLevels.Find(ExamGradeLevelId);
                if (data != null)
                {
                    context.ExamGradeLevels.Remove(data);
                    context.SaveChanges();
                }
            }
            return new EmptyResult();
        }

        #endregion

        #region "GRADE BOOK"
        public ActionResult CBSEGradeBook()
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
        public ActionResult GradeBook()
        {
            if (sessionValidate())
            {
                using (var context = new GS247Entities8())
                {
                    ViewBag.CoursesList = context.Courses.Where(x => x.IsEnable == true).ToList();
                }

                var studentList = new List<StudentExamDetail>();
                ViewBag.studentList = studentList;
                return View();
            }
            else
                return RedirectToAction("Index", "Login");
        }

        [HttpPost]
        public JsonResult GradeBookList(SearchGradeBook SearchSFields)
        {
            using (var context = new GS247Entities8())
            {
                int bid = Convert.ToInt32(SearchSFields.BatchId);
                int cid = Convert.ToInt32(SearchSFields.CourseId);
                var List = new List<StudentExamDetailCO>();

                var FieldsList = (from ex in context.Examinations
                                  join ed in context.StudentExamDetails on ex.ExamId equals ed.ExamId
                                  where ex.CourseId == cid && ed.StatusFlag == 1
                                  && bid == 0 ? ex.BatchId > 0 : ex.BatchId == bid && ex.ExamType == 1
                                  select ed).ToList();

                var FieldsList1 = (from ex in context.Examinations
                                   join ed in context.StudentExamDetails on ex.ExamId equals ed.ExamId
                                   join ct in context.ExamCommonTypeCourses on ex.ExamId equals ct.ExamId
                                   where ct.CourseId == cid && ed.StatusFlag == 1
                                  && bid == 0 ? ct.BatchId > 0 : ex.BatchId == bid && ex.ExamType == 2
                                   select ed).ToList();

                FieldsList.AddRange(FieldsList1);

                int totalCount = FieldsList.Count;
                var totalPages = (int)Math.Ceiling((decimal)totalCount / (decimal)10);
                ViewBag.PageCount = totalPages;
                ViewBag.CurrentPageIndex = Convert.ToInt16(SearchSFields.CurrentPage);
                var studentList = FieldsList.OrderByDescending(x => x.CreatedDate).Skip((Convert.ToInt16(SearchSFields.CurrentPage) - 1) * 10).Take(10).ToList();

                studentList.ForEach(data =>
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
                    result.ExamName = context.Examinations.Find(data.ExamId).Name;
                    var studentData = context.StudentDetails.Find(data.StudentDetailsId);
                    result.StudentName = studentData != null ? studentData.FirstName + " " + studentData.LastName : "";
                    result.AdmissionNo = studentData != null ? studentData.AdmissionNo : "";
                    List.Add(result);
                });

                return Json(new { StudentList = List, TotalPages = totalPages }, JsonRequestBehavior.AllowGet);
            }

        }

        #endregion

        #region "ADD QUESTION"

        public ActionResult AddQuestion(int? ExamId)
        {

            if (sessionValidate())
            {
                using (var context = new GS247Entities8())
                {
                    ViewBag.ExamId = ExamId;
                    ViewBag.Name = context.Examinations.Find(ExamId).Name;
                }
                return View();
            }
            else
                return RedirectToAction("Index", "Login");
        }

        [HttpPost]
        public ActionResult AddQuestion(QuestionPaper questionPaper)
        {
            if (sessionValidate())
            {
                int? Type = 1;
                var Message = string.Empty;
                using (var context = new GS247Entities8())
                {
                    var examData = context.Examinations.Find(questionPaper.ExamId);
                    Type = examData.ExamType;
                    var questionPaperList = context.ExamQuestionPapers.Where(x => x.ExamId == questionPaper.ExamId).ToList();

                    decimal? queationMarks = 0;
                    if (questionPaperList.Count > 0)
                        queationMarks = questionPaperList.Sum(x => x.Mark) + questionPaper.Mark;
                    else
                        queationMarks = questionPaper.Mark;

                    if (examData.TotalMarks >= queationMarks)
                    {
                        var QuestionPaper = new ExamQuestionPaper();
                        QuestionPaper.ExamId = questionPaper.ExamId;
                        QuestionPaper.QuestionName = questionPaper.QuestionName;
                        QuestionPaper.Answer = questionPaper.Answer;
                        QuestionPaper.Mark = questionPaper.Mark;
                        QuestionPaper.QuestionType = questionPaper.QuestionType;
                        QuestionPaper.CreatedBy = Session["UserName"].ToString();
                        QuestionPaper.CreatedDate = DateTime.Now;
                        QuestionPaper.QuestionNo = context.ExamQuestionPapers.Where(x => x.ExamId == questionPaper.ExamId).Count() + 1;
                        context.ExamQuestionPapers.Add(QuestionPaper);
                        context.SaveChanges();

                        if (QuestionPaper.QuestionType == 1)
                        {
                            questionPaper.OptionValue.ForEach(data =>
                            {
                                var QuestionTypeValue = new ExamQuestionTypeValue();
                                QuestionTypeValue.ExamId = questionPaper.ExamId;
                                QuestionTypeValue.ExamQuestionId = QuestionPaper.ExamQuestionId;
                                QuestionTypeValue.QuestionType = questionPaper.QuestionType;
                                QuestionTypeValue.Value = data;
                                QuestionTypeValue.CreatedBy = Session["UserName"].ToString();
                                QuestionTypeValue.CreatedDate = DateTime.Now;
                                context.ExamQuestionTypeValues.Add(QuestionTypeValue);
                                context.SaveChanges();
                            });
                        }
                    }
                    else
                    {
                        Message = "Total Question marks exceed total marks ";
                    }
                }
                return Json(new { Message = Message, Type = Type });
            }
            else
                return RedirectToAction("Index", "Login");
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

        [HttpPost]
        public ActionResult DeleteQuestion(int ExamQuestionId)
        {
            if (sessionValidate())
            {
                using (var context = new GS247Entities8())
                {
                    var data = context.ExamQuestionPapers.Find(ExamQuestionId);
                    if (data != null)
                    {
                        context.ExamQuestionPapers.Remove(data);
                        context.SaveChanges();
                    }
                }

                return new EmptyResult();
            }
            else
                return RedirectToAction("Index", "Login");
        }

        public ActionResult UpdateQuestion(int ExamQuestionId)
        {
            if (sessionValidate())
            {
                var examQuestionPaperCO = new ExamQuestionPaperCO();
                using (var context = new GS247Entities8())
                {
                    var data = context.ExamQuestionPapers.Find(ExamQuestionId);
                    examQuestionPaperCO.ExamQuestionId = data.ExamQuestionId;
                    examQuestionPaperCO.ExamId = data.ExamId;
                    examQuestionPaperCO.QuestionName = data.QuestionName;
                    examQuestionPaperCO.QuestionType = data.QuestionType;
                    examQuestionPaperCO.Answer = data.Answer;
                    examQuestionPaperCO.Mark = data.Mark;
                    examQuestionPaperCO.QuestionNo = data.QuestionNo;
                    examQuestionPaperCO.CreatedDate = data.CreatedDate;

                    if (data.QuestionType == 1)
                        examQuestionPaperCO.OptionValue = context.ExamQuestionTypeValues.Where(x => x.ExamQuestionId == data.ExamQuestionId).Select(x => x.Value).ToList();

                    ViewBag.Name = context.Examinations.Find(examQuestionPaperCO.ExamId).Name;
                }
                return View(examQuestionPaperCO);
            }
            else
                return RedirectToAction("Index", "Login");
        }

        [HttpPost]
        public ActionResult UpdateQuestion(QuestionPaper questionPaper, int ExamQuestionId)
        {
            if (sessionValidate())
            {                
                var Message = string.Empty;
                using (var context = new GS247Entities8())
                {
                    var questionPaperList = context.ExamQuestionPapers.Where(x => x.ExamId == questionPaper.ExamId && x.ExamQuestionId != ExamQuestionId).ToList();
                    var examData = context.Examinations.Find(questionPaper.ExamId);
                    decimal? queationMarks = 0;
                    if (questionPaperList.Count > 0)
                        queationMarks = questionPaperList.Sum(x => x.Mark) + questionPaper.Mark;
                    else
                        queationMarks = questionPaper.Mark;

                    if (examData.TotalMarks >= queationMarks)
                    {
                        var _templist = context.ExamQuestionPapers.Find(ExamQuestionId);
                        if (_templist != null)
                        {
                            _templist.QuestionName = questionPaper.QuestionName;
                            _templist.QuestionType = questionPaper.QuestionType;
                            _templist.Answer = questionPaper.Answer;
                            _templist.Mark = questionPaper.Mark;
                            _templist.UpdatedDate = DateTime.Now;
                            _templist.UpdatedBy = Session["UserName"].ToString();
                            context.Entry(_templist).State = EntityState.Modified;
                            context.SaveChanges();
                        }

                        var ExamQuestionTypeValues = context.ExamQuestionTypeValues.Where(x => x.ExamQuestionId == ExamQuestionId).ToList();

                        ExamQuestionTypeValues.ForEach(item =>
                        {
                            context.ExamQuestionTypeValues.Remove(item);
                            context.SaveChanges();
                        });

                        if (questionPaper.QuestionType == 1)
                        {
                            questionPaper.OptionValue.ForEach(data =>
                            {
                                var QuestionTypeValue = new ExamQuestionTypeValue();
                                QuestionTypeValue.ExamId = questionPaper.ExamId;
                                QuestionTypeValue.ExamQuestionId = ExamQuestionId;
                                QuestionTypeValue.QuestionType = questionPaper.QuestionType;
                                QuestionTypeValue.Value = data;
                                QuestionTypeValue.CreatedBy = Session["UserName"].ToString();
                                QuestionTypeValue.CreatedDate = DateTime.Now;
                                context.ExamQuestionTypeValues.Add(QuestionTypeValue);
                                context.SaveChanges();
                            });
                        }
                    }
                    else
                    {
                        Message = "Total Question marks exceed total marks ";
                    }
                }
                return Json(new { Message = Message});
            }
            else
                return RedirectToAction("Index", "Login");
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

        #endregion

        #region "COMMON Class"

        public class SearchGradeBook
        {
            public string BatchId { get; set; }

            public string CourseId { get; set; }

            public string CurrentPage { get; set; }

        }

        public class ExamAnswerSheet
        {
            public int StudentExamDetailsId { get; set; }

            public List<ExamStudentAnswer> ExamStudentAnswer { get; set; }

            public List<StudentExamAnswerDetailsCO> StudentExamAnswerDetailsCO { get; set; }

        }

        public class ExamStudentAnswer
        {
            public int ExamQuestionId { get; set; }

            public string Answer { get; set; }

            public int? StudentExamDetailsId { get; set; }

            public string Mark { get; set; }

        }

        public class QuestionPaper
        {
            public int ExamId { get; set; }
            public string QuestionName { get; set; }

            public int QuestionType { get; set; }

            public string Answer { get; set; }

            public decimal Mark { get; set; }

            public List<string> OptionValue { get; set; }

        }

        public class StudentExamAnswerDetailsCO
        {
            public int StudentExamAnswerDetailsId { get; set; }
            public Nullable<int> StudentExamDetailsId { get; set; }
            public Nullable<int> ExamQuestionId { get; set; }
            public string Answer { get; set; }
            public Nullable<int> AnswerCorrectOrWrong { get; set; }
            public string Mark { get; set; }
            public decimal? QuestionMark { get; set; }
            public string QuestionAnswer { get; set; }
            public int? QuestionNo { get; set; }

            public int? QuestionType { get; set; }

            public string QuestionName { get; set; }
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