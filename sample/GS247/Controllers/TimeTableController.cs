using DocumentFormat.OpenXml.Drawing.Diagrams;
using DocumentFormat.OpenXml.EMMA;
using DocumentFormat.OpenXml.Office2010.Excel;
using GS247.Models;
using GS247.Models.TimeTable;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;

namespace GS247.Controllers
{
    public class TimeTableController : Controller
    {
        public GS247Entities8 db = new GS247Entities8();
        SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["GS247"].ConnectionString);
        SqlCommand cmd;
        public string fileLocation;
        
        public SqlDataAdapter da;
        public SqlDataReader dr;
        public ActionResult WeekDays_Index()
        {
            return View();
        }
       public ActionResult TimeTable_Index()
        {
            if (Convert.ToInt32(Session["UserId"]) == 0) return RedirectToAction("Index", "Login");

            Session["BackTo"] = "TimeTable_Index";

            var ClassTimings = new List<ClassTiming>();
            var Weekdays = new List<Weekdays>();

            ViewBag.Subjects = new SelectList(db.Subjects.ToList(), "Id", "Name");
            ViewBag.Teachers = new SelectList("");
            if (Session["BatchId"] == null)
            {
                Session["BatchId"] = 0;
                TimeTable_Context context = new TimeTable_Context
                {
                     ClassTimings=new List<ClassTiming>(),
                       TimeTable_Combined=new List<TimeTable_Combined>(),
                        Weekdays=new List<Weekdays>()
                };
                return View(context);
            }
            else if(Convert.ToInt32(Session["BatchId"])==0)
                {
                    TimeTable_Context context = new TimeTable_Context
                    {
                        ClassTimings = new List<ClassTiming>(),
                        TimeTable_Combined = new List<TimeTable_Combined>(),
                        Weekdays = new List<Weekdays>()
                    };
                    return View(context);
            }
            else
            {
                var batchId = Convert.ToInt32(Session["BatchId"]);
               

                var weekday = db.Timetable_Weekdays.Where(x => x.BatchId == batchId&&x.IsEnable==true).ToList();
                var classTimings = db.Timetable_ClassTimings.Where(x => x.BatchId == batchId).ToList();

                if (weekday.Count == 0)
                {
                    foreach(var item in db.SettingsDefaultWeekdays.Where(x=>x.IsEnable==1).ToList())
                    {
                        Weekdays weekDay = new Weekdays
                        {
                            Id=item.Id,
                              WeekdayName=item.WeekDay,
                               DefaultWeekdays=1
                        };
                        Weekdays.Add(weekDay);
                    }
                }
                else
                {
                    db.Timetables.RemoveRange(db.Timetables.Where(x=>x.BatchId==batchId&&x.DefaultWeekday==1).ToList());
                    db.SaveChanges();
                    foreach (var item in weekday)
                    {
                        Weekdays weekDay = new Weekdays
                        {
                            Id = item.Id,
                            WeekdayName = item.WeekdayName,
                             DefaultWeekdays=0
                        };
                        Weekdays.Add(weekDay);
                    }
                }
                if (classTimings.Count == 0)
                {
                    using (conn)
                    {
                        conn.Open();
                        cmd = new SqlCommand("select Id,CONVERT(varchar(15),CAST(StartTime AS TIME),100)as StartTime,CONVERT(varchar(15),CAST(EndTime AS TIME),100)as EndTime,IsBreak from SettingsCommonClassTimings", conn);
                        dr = cmd.ExecuteReader();
                        while (dr.Read())
                        {
                            ClassTiming classTiming = new ClassTiming
                            {
                                Id =Convert.ToInt32(dr["Id"]),
                                EndTime = dr["EndTime"].ToString(),
                                IsBreak = Convert.ToInt32(dr["IsBreak"].ToString()),
                                StartTime = dr["StartTime"].ToString(),
                                 DefaultClassTiming=1
                            };
                            ClassTimings.Add(classTiming);
                        }
                    }
                }
                else
                {
                    db.Timetables.RemoveRange(db.Timetables.Where(x => x.BatchId == batchId && x.DefaultClassTiming == 1).ToList());
                    db.SaveChanges();
                    using (conn)
                    {
                        conn.Open();
                        cmd = new SqlCommand("select Id,CONVERT(varchar(15),CAST(StartTime AS TIME),100)as StartTime,CONVERT(varchar(15),CAST(EndTime AS TIME),100)as EndTime,IsBreak from Timetable_ClassTimings where BatchId='"+batchId+"'", conn);
                        dr = cmd.ExecuteReader();
                        while (dr.Read())
                        {
                            ClassTiming classTiming = new ClassTiming
                            {
                                Id = Convert.ToInt32(dr["Id"]),
                                EndTime = dr["EndTime"].ToString(),
                                StartTime = dr["StartTime"].ToString(),
                                 DefaultClassTiming=0
                            };
                            classTiming.IsBreak = 0;
                            if (Convert.ToBoolean(dr["IsBreak"])) classTiming.IsBreak = 1;
                            ClassTimings.Add(classTiming);
                        }
                    }
                }
                    
                TimeTable_Context context = new TimeTable_Context
                {
                    ClassTimings = ClassTimings,
                    Weekdays = Weekdays
                };

                List<TimeTable_Combined> timeTable = new List<TimeTable_Combined>();

                var TimeTables = db.Timetables.Where(x => x.BatchId == batchId).ToList();
                if (TimeTables.Count() == 0)
                {
                    foreach(var day in Weekdays)
                    {
                        foreach(var timing in ClassTimings)
                        {
                            Timetable timetable = new Timetable
                            {
                                 BatchId=batchId,
                                  ClassTimingId=timing.Id,
                                   SubjectId=0,
                                    TeacherId=0,
                                     Weekday=day.WeekdayName,
                                      DefaultClassTiming=timing.DefaultClassTiming,
                                       DefaultWeekday=day.DefaultWeekdays
                            };
                            db.Timetables.Add(timetable);
                            db.SaveChanges();
                        }
                    }
                }
                    foreach(var table in db.Timetables.Where(x=>x.BatchId==batchId).ToList())
                {
                    var subject = db.Subjects.Find(table.SubjectId);
                    var teacher = db.StaffDetails.Find(table.TeacherId);
                    var classTiming = ClassTimings.Where(x => x.Id == table.ClassTimingId).FirstOrDefault();
                    TimeTable_Combined combined = new TimeTable_Combined
                    {
                        Id = table.Id,
                        WeekDay=table.Weekday,
                        IsBreak = classTiming.IsBreak,
                        SubjectId = Convert.ToInt32(table.SubjectId),
                        TeacherId = Convert.ToInt32(table.TeacherId)
                    };
                    if (subject != null) combined.SubjectName = subject.Name;
                    else combined.SubjectName = "";
                    if (teacher != null) combined.TeacherName = teacher.FirstName + " " + teacher.MiddleName + " " + teacher.LastName + "-" + teacher.TeacherNumber;
                    else combined.TeacherName = "";

                        timeTable.Add(combined);
                }
                    context.TimeTable_Combined = timeTable;
                
                return View(context);
            }
            

        }
        public ActionResult AssignSubject(string Subject,string Teacher,int TimeTableId)
        {
            if (Convert.ToInt32(Session["UserId"]) == 0) return RedirectToAction("Index", "Login");
            var timeTable = db.Timetables.Find(TimeTableId);
            if (timeTable != null)
            {
                  timeTable.SubjectId = Convert.ToInt32(Subject);
                timeTable.TeacherId = Convert.ToInt32(Teacher);
                db.Entry(timeTable).State = EntityState.Modified;
                db.SaveChanges();
            }
            return RedirectToAction("TimeTable_Index");
        }
        public ActionResult GetTeachers(int subject)
        {
            var teachers = (from t in db.TeacherSubjectAssociations
                            join s in db.StaffDetails
                            on t.TeacherId equals s.TeacherId
                            where t.SubjectId == subject
                            select new { TeacherName = s.FirstName + " " + s.MiddleName + " " + s.LastName+"-"+s.TeacherNumber, s.TeacherId }).ToList();
            return Json(new SelectList(teachers, "TeacherId", "TeacherName"), JsonRequestBehavior.AllowGet);
        }
        public ActionResult SearchCourseBatch()
        {
            if (Convert.ToInt32(Session["UserId"]) == 0) return RedirectToAction("Index", "Login");
            List<SearchBatchCourse> searchBatchCourses = new List<SearchBatchCourse>();
            var data = (from cb in db.BatchCourseLinks
                        join b in db.batches
                        on cb.BatchId equals b.Id
                        join c in db.Courses
                        on cb.CourseId equals c.Id
                        select new { BatchId = b.Id, BatchName = b.Name, CourseId = c.Id, CourseName = c.Name }).ToList();
            foreach(var item in data)
            {
                SearchBatchCourse batchCourse = new SearchBatchCourse
                {
                    BatchId = item.BatchId,
                    BatchName = item.BatchName,
                    CourseId = item.CourseId,
                    CourseName = item.CourseName
                };
                searchBatchCourses.Add(batchCourse);
            }
            return View(searchBatchCourses);
        } 
        public ActionResult GetBatch(int id)
        {
            if (Convert.ToInt32(Session["UserId"]) == 0) return RedirectToAction("Index", "Login");
            Session["BatchId"] = id;
            var courseBatch = db.BatchCourseLinks.Where(x => x.BatchId == id).FirstOrDefault();

            var batch = db.batches.Find(id);
            if (batch != null) Session["Batch"] = batch.Name;
            var course = db.Courses.Find(courseBatch.CourseId);
            if (course != null) Session["Course"] = course.Name;

            return RedirectToAction(Session["BackTo"].ToString());
        }
        public ActionResult Delete_Assign(int id)
        {
            if (Convert.ToInt32(Session["UserId"]) == 0) return RedirectToAction("Index", "Login");
            var table = db.Timetables.Find(id);
            if(table!=null)
            {
                db.Timetables.Remove(table);
                db.SaveChanges();
            }
            return RedirectToAction("TimeTable_Index");
        }
        public ActionResult FullTimetable_Index()
        {
            if (Convert.ToInt32(Session["UserId"]) == 0) return RedirectToAction("Index", "Login");
            ViewBag.AcadamicYears = new SelectList((from c in db.Courses
                                                   join y in db.Years
                                                   on c.AcadamicYrId equals y.Id
                                                   select new { YearId=y.Id,YearName=y.Name}).Distinct(),"YearId","YearName");
            ViewBag.Course = new SelectList(db.Courses.ToList(), "Id", "Name");
            ViewBag.Batch = new SelectList(db.batches.ToList(), "Id", "Name");
            
            ViewBag.Day = new SelectList(db.SettingsDefaultWeekdays.Select(x => x.WeekDay).Distinct());
            FullTime_Context context = new FullTime_Context
            {
                FullTimeTable = new FullTimeTable(),
                FullTimeTable_Contexts = new List<FullTimeTable_Context>()
        };
            return View(context);
            }
        [HttpPost]
        public ActionResult FullTimetable_Index(FullTime_Context context)
        {
            if (context.FullTimeTable.Mode == "Day" && context.FullTimeTable.Day == "0")
            {
                TempData["FullTimeTable_Index"] = "Select Day";
                return View(context);
            }
            if (Convert.ToInt32(Session["UserId"]) == 0) return RedirectToAction("Index", "Login");
            ViewBag.AcadamicYears = new SelectList((from c in db.Courses
                                                    join y in db.Years
                                                    on c.AcadamicYrId equals y.Id
                                                    select new { YearId = y.Id, YearName = y.Name }).ToList(), "YearId", "YearName",context.FullTimeTable.AcadamicYear);
            ViewBag.Course = new SelectList(db.Courses.ToList(), "Id", "Name",context.FullTimeTable.Course);
            ViewBag.Batch = new SelectList(db.batches.ToList(), "Id", "Name",context.FullTimeTable.Batch);
            
            ViewBag.Day = new SelectList(db.SettingsDefaultWeekdays.Select(x => x.WeekDay).Distinct(),context.FullTimeTable.Day);

            List<FullTimeTable_Context> fullTimeTable_Contexts=new List<FullTimeTable_Context>();
            FullTime_Context fullTime_Context = new FullTime_Context();

            var AcadamicYr = context.FullTimeTable.AcadamicYear;
            var Course = Convert.ToInt32(context.FullTimeTable.Course);
            var Batch= Convert.ToInt32(context.FullTimeTable.Batch);
            var Mode = context.FullTimeTable.Mode;
            var Day = context.FullTimeTable.Day;

            string weekdayQ1 = "";
            string weekdayQ2 = "";
            string timingQ1 = "";
            string timingQ2 = "";
            string timetableQ = "";
            
            string cbtQuery = "";

            if (AcadamicYr!=null&&Course!=0&&Batch==0&&Mode!="Week")
            {
                if (Day == null)
                {
                    TempData["FullTimeTable_Index"] = "Select Day";
                    return View(fullTime_Context);
                        }
                int courseId = Convert.ToInt32(Course);
                var batchIds = db.BatchCourseLinks.Where(x => x.CourseId == Course).Select(x => x.BatchId).Distinct();
                foreach(var batchId in batchIds)
                {
                    weekdayQ1 = "select * from Timetable_Weekdays where IsEnable=1 and BatchId='"+batchId+"' and WeekdayName='"+Day+"'";
                    weekdayQ2 = "select * from SettingsDefaultWeekdays where IsEnable=1 and Weekday='"+Day+"'";
                    timingQ1 = "select Id,CONVERT(varchar(15),CAST(StartTime AS TIME),100)as StartTime,CONVERT(varchar(15),CAST(EndTime AS TIME),100)as EndTime,IsBreak from Timetable_ClassTimings where BatchId='" + batchId + "'";
                    timingQ2 = "select Id,CONVERT(varchar(15),CAST(StartTime AS TIME),100)as StartTime,CONVERT(varchar(15),CAST(EndTime AS TIME),100)as EndTime,IsBreak from SettingsCommonClassTimings";
                    timetableQ = "select * from Timetable where BatchId='"+batchId+"' and Weekday='"+Day+"'";
                    cbtQuery = "Select c.Name as CourseName,b.Name as BatchName,(st.FirstName+''+st.MiddleName+''+st.LastName) as TeacherName " +
"from BatchCourseLink bc left join Courses c on bc.CourseId = c.Id left join batch b on bc.BatchId = b.Id left join BatchLink bl on bl.BatchId = b.Id left " +
"join StaffDetails st on bl.TeacherId = st.TeacherId where b.Id = '" + batchId + "'";
                    var fullTimeTable1 = filterData(weekdayQ1, weekdayQ2, timingQ1, timingQ2, timetableQ, cbtQuery);
                    fullTimeTable_Contexts.Add(fullTimeTable1);
                }
                fullTime_Context.FullTimeTable_Contexts = fullTimeTable_Contexts;
            }
            else if (AcadamicYr != null && Course != 0 && Batch==0&&Mode == "Week")
            {
                int courseId = Convert.ToInt32(Course);
                var batchIds = db.BatchCourseLinks.Where(x => x.CourseId == Course).Select(x => x.BatchId).Distinct();
                foreach (var batchId in batchIds)
                {
                    weekdayQ1 = "select * from Timetable_Weekdays where IsEnable=1 and BatchId='" + batchId + "'";
                    weekdayQ2 = "select * from SettingsDefaultWeekdays where IsEnable=1";
                    timingQ1 = "select Id,CONVERT(varchar(15),CAST(StartTime AS TIME),100)as StartTime,CONVERT(varchar(15),CAST(EndTime AS TIME),100)as EndTime,IsBreak from Timetable_ClassTimings where BatchId='" + batchId + "'";
                    timingQ2 = "select Id,CONVERT(varchar(15),CAST(StartTime AS TIME),100)as StartTime,CONVERT(varchar(15),CAST(EndTime AS TIME),100)as EndTime,IsBreak from SettingsCommonClassTimings";
                    timetableQ = "select * from Timetable where BatchId='" + batchId + "'";
                    cbtQuery = "Select c.Name as CourseName,b.Name as BatchName,(st.FirstName+''+st.MiddleName+''+st.LastName) as TeacherName " +
"from BatchCourseLink bc left join Courses c on bc.CourseId = c.Id left join batch b on bc.BatchId = b.Id left join BatchLink bl on bl.BatchId = b.Id left " +
"join StaffDetails st on bl.TeacherId = st.TeacherId where b.Id = '" + batchId + "'";
                    var fullTimeTable1 = filterData(weekdayQ1, weekdayQ2, timingQ1, timingQ2, timetableQ, cbtQuery);
                    fullTimeTable_Contexts.Add(fullTimeTable1);
                }
                fullTime_Context.FullTimeTable_Contexts = fullTimeTable_Contexts;
            }
            else if (AcadamicYr != null && Course != 0 && Batch != 0 && Mode == "Week")
            {
                    weekdayQ1 = "select * from Timetable_Weekdays where IsEnable=1 and BatchId='" + Batch + "'";
                    weekdayQ2 = "select * from SettingsDefaultWeekdays where IsEnable=1";
                    timingQ1 = "select Id,CONVERT(varchar(15),CAST(StartTime AS TIME),100)as StartTime,CONVERT(varchar(15),CAST(EndTime AS TIME),100)as EndTime,IsBreak from Timetable_ClassTimings where BatchId='"+Batch+"'";
                    timingQ2 = "select Id,CONVERT(varchar(15),CAST(StartTime AS TIME),100)as StartTime,CONVERT(varchar(15),CAST(EndTime AS TIME),100)as EndTime,IsBreak from SettingsCommonClassTimings";
                    timetableQ = "select * from Timetable where BatchId='" + Batch + "'";
                cbtQuery = "Select c.Name as CourseName,b.Name as BatchName,(st.FirstName+''+st.MiddleName+''+st.LastName) as TeacherName " +
"from BatchCourseLink bc left join Courses c on bc.CourseId = c.Id left join batch b on bc.BatchId = b.Id left join BatchLink bl on bl.BatchId = b.Id left " +
"join StaffDetails st on bl.TeacherId = st.TeacherId where b.Id = '" + Batch + "'";
                var fullTimeTable1 = filterData(weekdayQ1, weekdayQ2, timingQ1, timingQ2, timetableQ, cbtQuery);
                    fullTimeTable_Contexts.Add(fullTimeTable1);
                fullTime_Context.FullTimeTable_Contexts = fullTimeTable_Contexts;
            }
            else if (AcadamicYr != null && Course != 0 && Batch != 0 && Mode != "Week"&&Day!=null)
            {
                weekdayQ1 = "select * from Timetable_Weekdays where IsEnable=1 and BatchId='" + Batch + "' and WeekdayName='" + Day + "'";
                weekdayQ2 = "select * from SettingsDefaultWeekdays where IsEnable=1 and Weekday='" + Day + "'";
                timingQ1 = "select Id,CONVERT(varchar(15),CAST(StartTime AS TIME),100)as StartTime,CONVERT(varchar(15),CAST(EndTime AS TIME),100)as EndTime,IsBreak from Timetable_ClassTimings where BatchId='" + Batch + "'";
                timingQ2 = "select Id,CONVERT(varchar(15),CAST(StartTime AS TIME),100)as StartTime,CONVERT(varchar(15),CAST(EndTime AS TIME),100)as EndTime,IsBreak from SettingsCommonClassTimings";
                timetableQ = "select * from Timetable where BatchId='" + Batch + "' and Weekday='" + Day + "'";
                cbtQuery = "Select c.Name as CourseName,b.Name as BatchName,(st.FirstName+''+st.MiddleName+''+st.LastName) as TeacherName " +
"from BatchCourseLink bc left join Courses c on bc.CourseId = c.Id left join batch b on bc.BatchId = b.Id left join BatchLink bl on bl.BatchId = b.Id left " +
"join StaffDetails st on bl.TeacherId = st.TeacherId where b.Id = '" + Batch + "'";
                var fullTimeTable1 = filterData(weekdayQ1, weekdayQ2, timingQ1, timingQ2, timetableQ, cbtQuery);
                fullTimeTable_Contexts.Add(fullTimeTable1);
                fullTime_Context.FullTimeTable_Contexts = fullTimeTable_Contexts;
            }
            else if(AcadamicYr!=null&&Course==0&&Mode=="Week")
            {
                foreach (var course in db.Courses.ToList().Select(x => x.Id).Distinct())
                {
                    int courseId = Convert.ToInt32(course);
                    var batchIds = db.BatchCourseLinks.Where(x => x.CourseId == courseId).Select(x => x.BatchId).Distinct();
                    foreach (var batchId in batchIds)
                    {
                        weekdayQ1 = "select * from Timetable_Weekdays where IsEnable=1 and BatchId='" + batchId + "'";
                        weekdayQ2 = "select * from SettingsDefaultWeekdays where IsEnable=1";
                        timingQ1 = "select Id,CONVERT(varchar(15),CAST(StartTime AS TIME),100)as StartTime,CONVERT(varchar(15),CAST(EndTime AS TIME),100)as EndTime,IsBreak from Timetable_ClassTimings where BatchId='" + batchId + "'";
                        timingQ2 = "select Id,CONVERT(varchar(15),CAST(StartTime AS TIME),100)as StartTime,CONVERT(varchar(15),CAST(EndTime AS TIME),100)as EndTime,IsBreak from SettingsCommonClassTimings";
                        timetableQ = "select * from Timetable where BatchId='" + batchId + "'";
                        cbtQuery = "Select c.Name as CourseName,b.Name as BatchName,(st.FirstName+''+st.MiddleName+''+st.LastName) as TeacherName " +
 "from BatchCourseLink bc left join Courses c on bc.CourseId = c.Id left join batch b on bc.BatchId = b.Id left join BatchLink bl on bl.BatchId = b.Id left " +
 "join StaffDetails st on bl.TeacherId = st.TeacherId where b.Id = '" + batchId + "'";
                        var fullTimeTable1 = filterData(weekdayQ1, weekdayQ2, timingQ1, timingQ2, timetableQ, cbtQuery);
                        fullTimeTable_Contexts.Add(fullTimeTable1);
                    }
                }
                fullTime_Context.FullTimeTable_Contexts = fullTimeTable_Contexts;
            }
            else if (AcadamicYr != null && Course == 0 && Mode != "Week"&&Day!=null)
            {
                foreach (var course in db.Courses.ToList().Select(x => x.Id).Distinct())
                {
                    int courseId = Convert.ToInt32(course);
                    var batchIds = db.BatchCourseLinks.Where(x => x.CourseId == courseId).Select(x => x.BatchId).Distinct();
                    foreach (var batchId in batchIds)
                    {
                        weekdayQ1 = "select * from Timetable_Weekdays where IsEnable=1 and BatchId='" + batchId + "' and WeekdayName='" + Day + "'";
                        weekdayQ2 = "select * from SettingsDefaultWeekdays where IsEnable=1 and Weekday='" + Day + "'";
                        timingQ1 = "select Id,CONVERT(varchar(15),CAST(StartTime AS TIME),100)as StartTime,CONVERT(varchar(15),CAST(EndTime AS TIME),100)as EndTime,IsBreak from Timetable_ClassTimings where BatchId='" + batchId + "'";
                        timingQ2 = "select Id,CONVERT(varchar(15),CAST(StartTime AS TIME),100)as StartTime,CONVERT(varchar(15),CAST(EndTime AS TIME),100)as EndTime,IsBreak from SettingsCommonClassTimings";
                        timetableQ = "select * from Timetable where BatchId='" + batchId + "' and Weekday='" + Day + "'";
                        cbtQuery = "Select c.Name as CourseName,b.Name as BatchName,(st.FirstName+''+st.MiddleName+''+st.LastName) as TeacherName " +
 "from BatchCourseLink bc left join Courses c on bc.CourseId = c.Id left join batch b on bc.BatchId = b.Id left join BatchLink bl on bl.BatchId = b.Id left " +
 "join StaffDetails st on bl.TeacherId = st.TeacherId where b.Id = '" + batchId + "'";
                        var fullTimeTable1 = filterData(weekdayQ1, weekdayQ2, timingQ1, timingQ2, timetableQ, cbtQuery);
                        fullTimeTable_Contexts.Add(fullTimeTable1);
                    }
                }
                fullTime_Context.FullTimeTable_Contexts = fullTimeTable_Contexts;
            }
            FullTimeTable fullTimeTable = new FullTimeTable
            {
                 AcadamicYear=AcadamicYr,
                  Batch=Batch.ToString(),
                   Course=Course.ToString(),
                    Day=Day,
                     Mode=Mode
            };
            fullTime_Context.FullTimeTable = fullTimeTable;
            return View(fullTime_Context);
        }
        public ActionResult GetCourses(int AcadamicYrId)
        {
            var Courses = new SelectList(db.Courses.Where(x=>x.AcadamicYrId==AcadamicYrId).ToList(),"Id","Name");
            return Json(Courses,JsonRequestBehavior.AllowGet);
        }
        public FullTimeTable_Context filterData(string weekdayQuery1,string weekdayQuery2,string timingQuery1,string timingQuery2,string timeTableQuery,string cbtQuery)
        {
            List<Weekdays> weekdays = new List<Weekdays>();
            List<ClassTiming> classTimings = new List<ClassTiming>();
            List<TimeTable_Combined> combineds = new List<TimeTable_Combined>();
            CourseBatchTeacher courseBatchTeacher = new CourseBatchTeacher();
            using (conn=new SqlConnection(ConfigurationManager.ConnectionStrings["GS247"].ConnectionString))
            {
                if(conn!=null&&conn.State== System.Data.ConnectionState.Closed)
                conn.Open();
                //Getting Weekdays 
                cmd = new SqlCommand(weekdayQuery1, conn);
                dr = cmd.ExecuteReader();
                if(dr.HasRows)
                {
                    while(dr.Read())
                    { 
                        Weekdays weekdays1 = new Weekdays
                        {
                            Id = Convert.ToInt32(dr["Id"]),
                            WeekdayName =dr["WeekdayName"].ToString()
                        };
                        weekdays.Add(weekdays1);
                    }
                }
                else
                {
                    cmd = new SqlCommand(weekdayQuery2, conn);
                    dr = cmd.ExecuteReader();
                    while(dr.Read())
                    { 
                        Weekdays weekdays1 = new Weekdays
                        {
                            Id = Convert.ToInt32(dr["Id"]),
                            WeekdayName = dr["Weekday"].ToString()
                        };
                        weekdays.Add(weekdays1);
                    }
                }
                //Getting Class Timings(Header)
                        cmd = new SqlCommand(timingQuery1, conn);
                        dr = cmd.ExecuteReader();
                    if(dr.HasRows)
                { 
                    while (dr.Read())
                    {
                        ClassTiming classTiming = new ClassTiming
                        {
                            Id = Convert.ToInt32(dr["Id"]),
                            EndTime = dr["EndTime"].ToString(),
                            
                            StartTime = dr["StartTime"].ToString()
                        };
                        classTiming.IsBreak = 0;
                        if (Convert.ToBoolean(dr["IsBreak"])) classTiming.IsBreak = 1;
                        classTimings.Add(classTiming);
                    }
                }
                else
                {
                        cmd = new SqlCommand(timingQuery2, conn);
                        dr = cmd.ExecuteReader();
                        while (dr.Read())
                        {
                            ClassTiming classTiming = new ClassTiming
                            {
                                Id = Convert.ToInt32(dr["Id"]),
                                EndTime = dr["EndTime"].ToString(),
                                IsBreak = Convert.ToInt32(dr["IsBreak"].ToString()),
                                StartTime = dr["StartTime"].ToString()
                            };
                            classTimings.Add(classTiming);
                        }
                }
                    //Getting Timetable
                cmd = new SqlCommand(timeTableQuery, conn);
                dr = cmd.ExecuteReader();
                while(dr.Read())
                {
                    var subjectId = Convert.ToInt32(dr["SubjectId"]);
                    var teacherId = Convert.ToInt32(dr["TeacherId"]);
                        TimeTable_Combined combined = new TimeTable_Combined
                        {
                            Id = Convert.ToInt32(dr["Id"]),
                            SubjectId = Convert.ToInt32(dr["SubjectId"]),
                            TeacherId = Convert.ToInt32(dr["TeacherId"]),
                            WeekDay=dr["Weekday"].ToString()
                             
                        };

                    //Getting Subject Name and Teacher Name
                    var subject = db.Subjects.Find(subjectId);
                    if (subject != null) combined.SubjectName = subject.Name;
                    var teacher = db.StaffDetails.Where(x => x.TeacherId == teacherId).FirstOrDefault();
                    if (teacher != null) combined.TeacherName = teacher.FirstName + " " + teacher.MiddleName + " " + teacher.LastName + "-" + teacher.TeacherNumber;


                    //Checking IsBreak
                    var classTimingId = Convert.ToInt32(dr["ClassTimingId"]);
                    var settingsCommonClassTiming = db.SettingsCommonClassTimings.Find(classTimingId);
                    if(settingsCommonClassTiming!=null) combined.IsBreak = Convert.ToInt32(settingsCommonClassTiming.IsBreak);
                    if (Convert.ToInt32(dr["DefaultClassTiming"]) == 0)
                    {
                        var CommonClassTiming = db.Timetable_ClassTimings.Find(classTimingId);
                        if (CommonClassTiming != null) combined.IsBreak = Convert.ToInt32(CommonClassTiming.IsBreak);
                    }
                    combineds.Add(combined);
                }

                //Getting Course Batch Class Teacher details
                cmd = new SqlCommand(cbtQuery, conn);
                dr = cmd.ExecuteReader();
                while(dr.Read())
                {
                    courseBatchTeacher = new CourseBatchTeacher
                    {
                        Batch = dr["BatchName"].ToString(),
                        Course = dr["CourseName"].ToString(),
                        ClassTeacher = dr["TeacherName"].ToString()
                    };
                }
                
            }
            conn.Close();
            FullTimeTable_Context fullTimeTable1 = new FullTimeTable_Context();
            fullTimeTable1.ClassTimings = classTimings;
            fullTimeTable1.TimeTableCombined = combineds;
            fullTimeTable1.Weekdays = weekdays;
            fullTimeTable1.CourseBatchTeacher = courseBatchTeacher;
            return fullTimeTable1;
        }
        public JsonResult GetBatches(string course)
        {
            int courseId = Convert.ToInt32(course);
            var batches = (from bc in db.BatchCourseLinks
                           join b in db.batches
                           on bc.BatchId equals b.Id
                           where bc.CourseId == courseId
                           select new { BatchName = b.Name, BatchId = b.Id }).ToList();
            return Json(new SelectList(batches, "BatchId", "BatchName"), JsonRequestBehavior.AllowGet);
        }
        public ActionResult WeekDaysSetup()
        {
            if (Convert.ToInt32(Session["UserId"]) == 0) return RedirectToAction("Index", "Login");
          
            if (Session["BatchId"] == null) Session["BatchId"] = 0;
            Session["BackTo"] = "WeekDaysSetup";
            int batchId = Convert.ToInt32(Session["BatchId"]);
            var weekdays = db.Timetable_Weekdays.Where(x => x.BatchId == batchId).ToList();
            if(weekdays.Count==0&&batchId!=0)
            {
                var defaultWeekdays = db.SettingsDefaultWeekdays.ToList();
                foreach(var item in defaultWeekdays)
                {
                    Timetable_Weekdays day = new Timetable_Weekdays
                    {
                         BatchId=batchId,
                          IsEnable=false,
                           WeekdayName=item.WeekDay
                    };
                    db.Timetable_Weekdays.Add(day);
                    db.SaveChanges();
                }
            }
            List<TimetableWeekdays> timetableWeekdays = new List<TimetableWeekdays>();
            foreach(var item in db.Timetable_Weekdays.Where(x=>x.BatchId==batchId).ToList())
            {
                TimetableWeekdays weekdays1 = new TimetableWeekdays
                {
                    BatchId = batchId,
                    Id = item.Id,
                    IsEnable = Convert.ToBoolean(item.IsEnable),
                    Weekday = item.WeekdayName
                };
                timetableWeekdays.Add(weekdays1);
            }
            return View(timetableWeekdays);
        }
        [HttpPost]
        public ActionResult UpdateWeekDays(List<TimetableWeekdays> Data)
        {
            if (Convert.ToInt32(Session["UserId"]) == 0) return RedirectToAction("Index", "Login");
           
            foreach(var item in Data)
            {
                    var timetable = db.Timetable_Weekdays.Find(item.Id);
                if (timetable != null)
                {
                    timetable.IsEnable = item.IsEnable;
                    db.Entry(timetable).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
            TempData["WeekDaysSetup"] = "Submitted";
            return RedirectToAction("WeekDaysSetup");
        }
        /**End of Weekdays**/

        /**Start of Manage Class Timings**/
        public ActionResult ManageClassTimings()
        {
            if (Convert.ToInt32(Session["UserId"]) == 0) return RedirectToAction("Index", "Login");
           
            Session["BackTo"] = "ManageClassTimings";
            if (Session["BatchId"] == null) Session["BatchId"] = 0;
                int batchId = Convert.ToInt32(Session["BatchId"]);
            return View(db.Timetable_ClassTimings.Where(x => x.BatchId == batchId).ToList());
        }
        public ActionResult AddClassTimings()
        {
            if (Convert.ToInt32(Session["UserId"]) == 0) return RedirectToAction("Index", "Login");
          
            ViewBag.Title = "Create Class Timings";
            return View(new TimetableClassTiming());
        }
        [HttpPost]
        public ActionResult AddClassTimings(TimetableClassTiming Data)
        {
            if (Convert.ToInt32(Session["UserId"]) == 0) return RedirectToAction("Index", "Login");
           
            int batchId = Convert.ToInt32(Session["BatchId"]);
            if (db.Timetable_ClassTimings.Where(x => x.StartTime == Data.StartTime && x.EndTime == Data.EndTime && x.BatchId == batchId).FirstOrDefault() == null)
            {
                Timetable_ClassTimings timetable_ClassTimings = new Timetable_ClassTimings
                {
                     BatchId=batchId,
                      EndTime=Data.EndTime,
                       IsBreak=Data.IsBreak,
                        Name=Data.Name,
                         StartTime=Data.StartTime
                };
                db.Timetable_ClassTimings.Add(timetable_ClassTimings);
                db.SaveChanges();
                TempData["ClassTiming_Index"] = "Added";
            }
            else
                TempData["ClassTiming_Index"] = "Timing addeda already";
            return RedirectToAction("ManageClassTimings");
        }

       

        public ActionResult UpdateClassTimings(int Id)
        {
            if (Convert.ToInt32(Session["UserId"]) == 0) return RedirectToAction("Index", "Login");
           
            ViewBag.Title = "Update Class Timings";
            var timing = db.Timetable_ClassTimings.Find(Id);
            TimetableClassTiming timetableClassTiming = new TimetableClassTiming
            {
                  BatchId=Convert.ToInt32(timing.BatchId),
                   EndTime=timing.EndTime,
                    Id=timing.Id,
                     IsBreak=Convert.ToBoolean(timing.IsBreak),
                      Name=timing.Name,
                       StartTime=timing.StartTime
            };
            return View("AddClassTimings",timetableClassTiming);
        }
        [HttpPost]
        public ActionResult UpdateClassTimings(TimetableClassTiming Data)
        {
            if (Convert.ToInt32(Session["UserId"]) == 0) return RedirectToAction("Index", "Login");
           
            var timing = db.Timetable_ClassTimings.Find(Data.Id);
            if (timing != null)
            {
                timing.EndTime = Data.EndTime;
                timing.StartTime = Data.StartTime;
                timing.IsBreak = Data.IsBreak;
                timing.Name = Data.Name;
                db.Entry(timing).State = EntityState.Modified;
                db.SaveChanges();
                TempData["ClassTiming_Index"] = "Added";
            }
            return RedirectToAction("ManageClassTimings");
        }
        public ActionResult DeleteClassTimings(int Id)
        {
            if (Convert.ToInt32(Session["UserId"]) == 0) return RedirectToAction("Index", "Login");
           
            db.Timetable_ClassTimings.Remove(db.Timetable_ClassTimings.Find(Id));
            db.SaveChanges();
            TempData["ClassTiming_Index"] = "Removed";
            return RedirectToAction("ManageClassTimings");
        }
        public ActionResult TeachersTimetable_Index()
        {
            if (Convert.ToInt32(Session["UserId"]) == 0) return RedirectToAction("Index", "Login");
            ViewBag.Departments = new SelectList(db.TeachersManageDepartments.ToList(), "Id", "Name");
            ViewBag.Teachers = new SelectList((from t in db.StaffDetails
                                               join r in db.SettingsUserRoles
                                               on t.StaffRoleId equals r.Id
                                               where r.Name == "Teacher"
                                               select new { t.TeacherId, TeacherName = t.FirstName + " " + t.MiddleName + " " + t.LastName + "-" + t.TeacherNumber }).ToList(), "TeacherId", "TeacherName");
            ViewBag.Days = new SelectList(db.SettingsDefaultWeekdays.Select(x=>x.WeekDay).Distinct());
            
            return View(new List<Teacher_Timetable_Context>());
        }
        [HttpPost]
        public ActionResult TeachersTimetable_Index(int Department,int Teacher,string Day)
        {
            if (Convert.ToInt32(Session["UserId"]) == 0) return RedirectToAction("Index", "Login");
            ViewBag.Departments = new SelectList(db.TeachersManageDepartments.ToList(), "Id", "Name");
            ViewBag.Teachers = new SelectList(from t in db.StaffDetails
                                              join d in db.TeachersManageDepartments
                                              on t.DepartmentId equals d.Id
                                              where d.Id == Department
                                              select new { t.TeacherId, TeacherName = t.FirstName + " " + t.MiddleName + " " + t.LastName + "-" + t.TeacherNumber }, "TeacherId", "TeacherName",Teacher);
            ViewBag.Days = new SelectList(db.SettingsDefaultWeekdays.Select(x => x.WeekDay).Distinct());
            List<Teacher_Timetable_Context> context = new List<Teacher_Timetable_Context>();
            if (Department!=0&&Teacher==0&&Day=="")
            {
                context = Filter_Teacher_Timetable_ForAllTeachers_AndAllWeeks(Department);
            }
            else if(Department != 0 && Teacher != 0 && Day == "")
            {
                context = Filter_Teacher_Timetable__ForOneTeacher(Department,Teacher);
            }
            else if (Department != 0 && Teacher != 0 && Day != "")
            {
                context = Filter_Teacher_Timetable__ForOneTeacher_AndOneDay(Department, Teacher, Day);
            }
            return View(context);
        }
        public List<Teacher_Timetable_Context> Filter_Teacher_Timetable_ForAllTeachers_AndAllWeeks(int Department)
        {
            List<Teacher_Timetable_Context> context = new List<Teacher_Timetable_Context>();
            
            var teachers = db.StaffDetails.Where(x => x.DepartmentId.ToString() == Department.ToString()).Select(x => x.TeacherId).Distinct();
            foreach (var teacherId in teachers)
            {
                Teacher_Timetable_Context teacher_Timetable_Context = new Teacher_Timetable_Context { Teacher_Timetables = new List<Teacher_Timetables>() };
                Teacher_Timetables teacher_Timetables = new Teacher_Timetables();
                var days = db.Timetables.Where(x => x.TeacherId == teacherId).Select(x => x.Weekday).Distinct();
                foreach (var day in days)
                {
                    var timetables = db.Timetables.Where(x => x.TeacherId == teacherId && x.Weekday == day).ToList();
                    List<Teacher_TimeTable> teacher_TimeTables = new List<Teacher_TimeTable>();
                    foreach (var timeTable in timetables)
                    {
                        Teacher_TimeTable table = new Teacher_TimeTable();

                        //Getting Class Timing
                        using (conn = new SqlConnection(ConfigurationManager.ConnectionStrings["GS247"].ConnectionString))
                        {
                            if (conn != null && conn.State == System.Data.ConnectionState.Closed)
                                conn.Open();
                            string query = "";
                            if (timeTable.DefaultClassTiming == 1)
                                query = "select Id,CONVERT(varchar(15),CAST(StartTime AS TIME),100)as StartTime,CONVERT(varchar(15),CAST(EndTime AS TIME),100)as EndTime,IsBreak from SettingsCommonClassTimings where Id='" + timeTable.ClassTimingId + "'";
                            else
                                query = "select Id,CONVERT(varchar(15),CAST(StartTime AS TIME),100)as StartTime,CONVERT(varchar(15),CAST(EndTime AS TIME),100)as EndTime,IsBreak from Timetable_ClassTimings where Id='" + timeTable.ClassTimingId + "'";
                            cmd = new SqlCommand(query, conn);
                            dr = cmd.ExecuteReader();
                            while (dr.Read())
                            {
                                table.ClassTiming = dr["StartTime"].ToString() + "-" + dr["EndTime"].ToString();
                            }
                        }

                        //Getting Course Name & Batch Name
                        var data = (from cb in db.BatchCourseLinks
                                    join c in db.Courses
                                    on cb.CourseId equals c.Id
                                    join b in db.batches
                                    on cb.BatchId equals b.Id
                                    where cb.BatchId == timeTable.BatchId
                                    select new { BatchName = b.Name, CourseName = c.Name }).FirstOrDefault();
                        if (data != null)
                        {
                            table.CourseName = data.CourseName;
                            table.BatchName = data.BatchName;
                        }

                        //Getting Subject Name
                        var subject = db.Subjects.Find(timeTable.SubjectId);
                        if (subject != null)
                            table.Subject = subject.Name;
                        
                            teacher_TimeTables.Add(table);
                    }
                    teacher_Timetables.DayName = day;
                    teacher_Timetables.Teacher_TimeTables = teacher_TimeTables;
                    if (db.Timetables.Where(x => x.TeacherId == teacherId&&x.Weekday== day).ToList().Count > 0)
                        teacher_Timetable_Context.Teacher_Timetables.Add(teacher_Timetables);
                }
                var teacher = db.StaffDetails.Find(teacherId);
                if (teacher != null)
                    teacher_Timetable_Context.TeacherName = teacher.FirstName + " " + teacher.MiddleName + " " + teacher.LastName;
                teacher_Timetable_Context.TeacherNumber = Convert.ToInt64(teacher.TeacherNumber);
                if(db.Timetables.Where(x=>x.TeacherId== teacherId).ToList().Count>0)
                context.Add(teacher_Timetable_Context);
                
            }
            
            return context;
        }
        public List<Teacher_Timetable_Context> Filter_Teacher_Timetable__ForOneTeacher(int Department,int Teacher)
        {
            List<Teacher_Timetable_Context> context = new List<Teacher_Timetable_Context>();
            Teacher_Timetable_Context teacher_Timetable_Context = new Teacher_Timetable_Context
            {
                  Teacher_Timetables=new List<Teacher_Timetables>()
            };
            var teachers = db.StaffDetails.Where(x => x.DepartmentId.ToString() == Department.ToString()&&x.TeacherId==Teacher).Select(x => x.TeacherId).Distinct();
            foreach (var teacherId in teachers)
            {
                Teacher_Timetables teacher_Timetables = new Teacher_Timetables {  Teacher_TimeTables=new List<Teacher_TimeTable>()};
                var days = db.Timetables.Where(x => x.TeacherId == teacherId).Select(x => x.Weekday).Distinct();
                foreach (var day in days)
                {
                    var timetables = db.Timetables.Where(x => x.TeacherId == teacherId && x.Weekday == day).ToList();
                    List<Teacher_TimeTable> teacher_TimeTables = new List<Teacher_TimeTable>();
                    foreach (var timeTable in timetables)
                    {
                        Teacher_TimeTable table = new Teacher_TimeTable();

                        //Getting Class Timing
                        using (conn = new SqlConnection(ConfigurationManager.ConnectionStrings["GS247"].ConnectionString))
                        {
                            if (conn != null && conn.State == System.Data.ConnectionState.Closed)
                                conn.Open();
                            string query = "";
                            if (timeTable.DefaultClassTiming == 1)
                                query = "select Id,CONVERT(varchar(15),CAST(StartTime AS TIME),100)as StartTime,CONVERT(varchar(15),CAST(EndTime AS TIME),100)as EndTime,IsBreak from SettingsCommonClassTimings where Id='" + timeTable.ClassTimingId + "'";
                            else
                                query = "select Id,CONVERT(varchar(15),CAST(StartTime AS TIME),100)as StartTime,CONVERT(varchar(15),CAST(EndTime AS TIME),100)as EndTime,IsBreak from Timetable_ClassTimings where Id='" + timeTable.ClassTimingId + "'";
                            cmd = new SqlCommand(query, conn);
                            dr = cmd.ExecuteReader();
                            while (dr.Read())
                            {
                                table.ClassTiming = dr["StartTime"].ToString() + "-" + dr["EndTime"].ToString();
                            }
                        }

                        //Getting Course Name & Batch Name
                        var data = (from cb in db.BatchCourseLinks
                                    join c in db.Courses
                                    on cb.CourseId equals c.Id
                                    join b in db.batches
                                    on cb.BatchId equals b.Id
                                    where cb.BatchId == timeTable.BatchId
                                    select new { BatchName = b.Name, CourseName = c.Name }).FirstOrDefault();
                        if (data != null)
                        {
                            table.CourseName = data.CourseName;
                            table.BatchName = data.BatchName;
                        }

                        //Getting Subject Name
                        var subject = db.Subjects.Find(timeTable.SubjectId);
                        if (subject != null)
                            table.Subject = subject.Name;

                        teacher_TimeTables.Add(table);
                    }
                    teacher_Timetables.DayName = day;
                    teacher_Timetables.Teacher_TimeTables = teacher_TimeTables;
                    if (db.Timetables.Where(x => x.TeacherId == teacherId && x.Weekday == day).ToList().Count > 0)
                        teacher_Timetable_Context.Teacher_Timetables.Add(teacher_Timetables);
      
                }

                var teacher = db.StaffDetails.Find(teacherId);
                if (teacher != null)
                    teacher_Timetable_Context.TeacherName = teacher.FirstName + " " + teacher.MiddleName + " " + teacher.LastName;
                teacher_Timetable_Context.TeacherNumber = Convert.ToInt64(teacher.TeacherNumber);
                context.Add(teacher_Timetable_Context);
            }
            return context;
        }
        public List<Teacher_Timetable_Context> Filter_Teacher_Timetable__ForOneTeacher_AndOneDay(int Department, int Teacher,string Day)
        {
            
            List<Teacher_Timetable_Context> context = new List<Teacher_Timetable_Context>();
            Teacher_Timetable_Context teacher_Timetable_Context = new Teacher_Timetable_Context { Teacher_Timetables = new List<Teacher_Timetables>() };
            var teachers = db.StaffDetails.Where(x => x.DepartmentId.ToString() == Department.ToString() && x.TeacherId == Teacher).Select(x => x.TeacherId).Distinct();
            foreach (var teacherId in teachers)
            {
                Teacher_Timetables teacher_Timetables = new Teacher_Timetables();
                var weekday = db.SettingsDefaultWeekdays.Where(x=>x.WeekDay==Day).FirstOrDefault();
                var days = db.Timetables.Where(x => x.TeacherId == teacherId&&x.Weekday==weekday.WeekDay).Select(x => x.Weekday).Distinct();
                foreach (var day in days)
                {
                    var timetables = db.Timetables.Where(x => x.TeacherId == teacherId && x.Weekday == day).ToList();
                    List<Teacher_TimeTable> teacher_TimeTables = new List<Teacher_TimeTable>();
                    foreach (var timeTable in timetables)
                    {
                        Teacher_TimeTable table = new Teacher_TimeTable();

                        //Getting Class Timing
                        using (conn = new SqlConnection(ConfigurationManager.ConnectionStrings["GS247"].ConnectionString))
                        {
                            if (conn != null && conn.State == System.Data.ConnectionState.Closed)
                                conn.Open();
                            string query = "";
                            if (timeTable.DefaultClassTiming == 1)
                                query = "select Id,CONVERT(varchar(15),CAST(StartTime AS TIME),100)as StartTime,CONVERT(varchar(15),CAST(EndTime AS TIME),100)as EndTime,IsBreak from SettingsCommonClassTimings where Id='" + timeTable.ClassTimingId + "'";
                            else
                                query = "select Id,CONVERT(varchar(15),CAST(StartTime AS TIME),100)as StartTime,CONVERT(varchar(15),CAST(EndTime AS TIME),100)as EndTime,IsBreak from Timetable_ClassTimings where Id='" + timeTable.ClassTimingId + "'";
                            cmd = new SqlCommand(query, conn);
                            dr = cmd.ExecuteReader();
                            while (dr.Read())
                            {
                                table.ClassTiming = dr["StartTime"].ToString() + "-" + dr["EndTime"].ToString();
                            }
                        }

                        //Getting Course Name & Batch Name
                        var data = (from cb in db.BatchCourseLinks
                                    join c in db.Courses
                                    on cb.CourseId equals c.Id
                                    join b in db.batches
                                    on cb.BatchId equals b.Id
                                    where cb.BatchId == timeTable.BatchId
                                    select new { BatchName = b.Name, CourseName = c.Name }).FirstOrDefault();
                        if (data != null)
                        {
                            table.CourseName = data.CourseName;
                            table.BatchName = data.BatchName;
                        }

                        //Getting Subject Name
                        var subject = db.Subjects.Find(timeTable.SubjectId);
                        if (subject != null)
                            table.Subject = subject.Name;

                        teacher_TimeTables.Add(table);
                    }
                    teacher_Timetables.DayName=day;
                    teacher_Timetables.Teacher_TimeTables=teacher_TimeTables;
                    teacher_Timetable_Context.Teacher_Timetables.Add(teacher_Timetables);
                }
                var teacher = db.StaffDetails.Find(teacherId);
                if (teacher != null)
                {
                    teacher_Timetable_Context.TeacherName = teacher.FirstName + " " + teacher.MiddleName + " " + teacher.LastName;
                    teacher_Timetable_Context.TeacherNumber = Convert.ToInt64(teacher.TeacherNumber);
                }
                context.Add(teacher_Timetable_Context);
            }
           
            return context;
        }
        public ActionResult GetTeachersByDepartment(int department)
        {
            var teachers=new SelectList(from t in db.StaffDetails
                                        join d in db.TeachersManageDepartments
                                        on t.DepartmentId equals d.Id
                                        where d.Id==department
                                        select new { t.TeacherId,TeacherName=t.FirstName+" "+t.MiddleName+" "+t.LastName+"-"+t.TeacherNumber},"TeacherId","TeacherName");
            return Json(teachers,JsonRequestBehavior.AllowGet);
        }
    }
    }