using GS247.Models;
using GS247.Models.Courses;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text.RegularExpressions;
using System.Configuration;

namespace GS247.Controllers
{
    public class CoursesController : Controller
    {
        public SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["GS247"].ConnectionString);
        // GET: Courses
        public ActionResult Index()
        {
            int UserId = Convert.ToInt32(Session["UserId"]);
            if (UserId == 0)
            {
                return RedirectToAction("Index", "Login");
            }
            return View();
        }

        

         /// <summary>
        /// Courses module
        /// </summary>
        /// <returns></returns>
        public ActionResult ManageSemester()
        {
            int UserId = Convert.ToInt32(Session["UserId"]);
            if (UserId==0)
            {
                return RedirectToAction("Index", "Login");
            }
            SemesterClass objuser = new SemesterClass();
            DataSet ds = new DataSet();
            
                using (SqlCommand cmd = new SqlCommand("dbo.Semestercrudoperations", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@status", "GET");
                    cmd.Parameters.AddWithValue("@Id", 0);
                    cmd.Parameters.AddWithValue("@Name", "");
                    cmd.Parameters.AddWithValue("@Description", "");
                    cmd.Parameters.AddWithValue("@StartDate", "");
                    cmd.Parameters.AddWithValue("@EndDate", "");
                    con.Open();

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);
                    List<SemesterClass> userlist = new List<SemesterClass>();
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        SemesterClass uobj = new SemesterClass();
                        uobj.Id = Convert.ToInt32(ds.Tables[0].Rows[i]["Id"].ToString());
                        uobj.Name = ds.Tables[0].Rows[i]["Name"].ToString();
                        uobj.Description = ds.Tables[0].Rows[i]["Description"].ToString();
                        uobj.StartDate = ds.Tables[0].Rows[i]["StartDate"].ToString();
                        uobj.EndDate = ds.Tables[0].Rows[i]["EndDate"].ToString();
                        userlist.Add(uobj);
                    }
                    objuser.SemesterInfo = userlist;
                }
                con.Close();
            
            return View(objuser);

        }
        public ActionResult CreateSemester()
        {
            int UserId = Convert.ToInt32(Session["UserId"]);
            if (UserId == 0)
            {
                return RedirectToAction("Index", "Login");
            }
            SemesterClass objuser = new SemesterClass();
            DataSet ds = new DataSet();
               using (SqlCommand cmd = new SqlCommand("dbo.Coursescrudoperations", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@status", "GET");
                    cmd.Parameters.AddWithValue("@Id", 0);
                    cmd.Parameters.AddWithValue("@Name", "");
                    cmd.Parameters.AddWithValue("@IsEnable", 0);
                    cmd.Parameters.AddWithValue("@ExamFormat", 0);

                    con.Open();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);
                    objuser.CoursesInfo = new List<CheckBoxDetails>();
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        //String test = ds.Tables[0].Rows[i].ItemArray[1].ToString();
                        //String test = ds.Tables[0].Rows[i]["Name"].ToString();
                        CheckBoxDetails temp = new CheckBoxDetails();
                        temp.Id = Convert.ToInt32(ds.Tables[0].Rows[i].ItemArray[0]);
                        temp.Text = ds.Tables[0].Rows[i].ItemArray[1].ToString();
                        temp.IsChecked = false;
                        objuser.CoursesInfo.Add(temp);
                        //objuser.CoursesInfo.Add("Course2");
                    }
                    con.Close();
                
                return View(objuser);
            }
        }
        [HttpGet]
        public int SaveSemester(int SemesterId,String SemesterName,String StartDate,String EndDate,String Desc)
        {
            int ret = 1;
           
                con.Open();
                DataSet ds = new DataSet();
                using (SqlCommand cmd = new SqlCommand("dbo.Semestercrudoperations", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    String str = "INSERT";
                    if (SemesterId > 0) str = "UPDATE";
                    cmd.Parameters.AddWithValue("@status", str);
                    cmd.Parameters.AddWithValue("@Id", SemesterId);
                    cmd.Parameters.AddWithValue("@Name", SemesterName);
                    cmd.Parameters.AddWithValue("@Description", Desc);
                    cmd.Parameters.AddWithValue("@StartDate", StartDate);
                    cmd.Parameters.AddWithValue("@EndDate", EndDate);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);
                }
            
           return ret;
        }
        [HttpGet]
        public JsonResult GetSemester(int SemesterId)
        {
            SemesterClass obj = new SemesterClass();
                con.Open();
                DataSet ds = new DataSet();
                using (SqlCommand cmd = new SqlCommand("dbo.Semestercrudoperations", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@status", "GET");
                    cmd.Parameters.AddWithValue("@Id", SemesterId);
                    cmd.Parameters.AddWithValue("@Name", "");
                    cmd.Parameters.AddWithValue("@Description", "");
                    cmd.Parameters.AddWithValue("@StartDate", "");
                    cmd.Parameters.AddWithValue("@EndDate", "");
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);
                    if (ds.Tables.Count > 0)
                    {
                        obj.Id = Convert.ToInt32(ds.Tables[0].Rows[0].ItemArray[0]);
                        obj.Name = ds.Tables[0].Rows[0].ItemArray[1].ToString();
                        obj.Description = ds.Tables[0].Rows[0].ItemArray[2].ToString();
                        obj.StartDate = ds.Tables[0].Rows[0].ItemArray[3].ToString();
                        obj.EndDate = ds.Tables[0].Rows[0].ItemArray[4].ToString();
                    }
                }
                con.Close();
             return Json(obj, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult InsertSemester(GS247.Models.Courses.SemesterClass Details)
        {

            
                con.Open();
                DataSet ds = new DataSet();
                using (SqlCommand cmd = new SqlCommand("dbo.Semestercrudoperations", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@status", "INSERT");
                    cmd.Parameters.AddWithValue("@Id", 0);
                    cmd.Parameters.AddWithValue("@Name", Details.Name.ToString());
                    string tempDesc = "";
                    if(Details.Description!=null)
                    {
                        tempDesc = Details.Description.ToString();
                    }
                    cmd.Parameters.AddWithValue("@Description", tempDesc);
                    cmd.Parameters.AddWithValue("@StartDate", Details.StartDate.ToString());
                    cmd.Parameters.AddWithValue("@EndDate", Details.EndDate.ToString());
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);

                }
                for (int i = 0; i < Details.CoursesInfo.Count; i++)
                {
                    if (Details.CoursesInfo[i].IsChecked == true)
                    {
                        ds = new DataSet();
                        using (SqlCommand cmd = new SqlCommand("dbo.CoursesSemesterLinkProc", con))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@status", "INSERT");
                            cmd.Parameters.AddWithValue("@SemesterId", Details.Name.ToString());
                            cmd.Parameters.AddWithValue("@CourseId", Convert.ToInt32(Details.CoursesInfo[i].Id));
                            SqlDataAdapter da = new SqlDataAdapter(cmd);
                            da.Fill(ds);
                        }
                    }
                }
                con.Close();
            

            return RedirectToAction("ManageSemester", "Courses");
        }
        [HttpPost]
        public ActionResult UpdateSemesterRecord(GS247.Models.Courses.SemesterClass Details)
        {

            
                con.Open();
                DataSet ds = new DataSet();
                using (SqlCommand cmd = new SqlCommand("dbo.Semestercrudoperations", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@status", "UPDATE");
                    cmd.Parameters.AddWithValue("@Id", Details.Id);
                    cmd.Parameters.AddWithValue("@Name", Details.Name.ToString());
                    cmd.Parameters.AddWithValue("@Description", Details.Description.ToString());
                    cmd.Parameters.AddWithValue("@StartDate", Details.StartDate.ToString());
                    cmd.Parameters.AddWithValue("@EndDate", Details.EndDate.ToString());
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);
                    con.Close();
                }
                for (int i = 0; i < Details.CoursesInfo.Count; i++)
                {
                    DataSet delds = new DataSet();
                    using (SqlCommand delcmd = new SqlCommand("dbo.CoursesSemesterLinkProc", con))
                    {
                        delcmd.CommandType = CommandType.StoredProcedure;
                        delcmd.Parameters.AddWithValue("@status", "DELETE");
                        delcmd.Parameters.AddWithValue("@SemesterId", Convert.ToInt32(Details.Id));
                        delcmd.Parameters.AddWithValue("@CourseId", Convert.ToInt32(Details.CoursesInfo[i].Id));
                        SqlDataAdapter delda = new SqlDataAdapter(delcmd);
                        delda.Fill(ds);
                        if (Details.CoursesInfo[i].IsChecked == true)
                        {
                            ds = new DataSet();
                            using (SqlCommand cmd = new SqlCommand("dbo.CoursesSemesterLinkProc", con))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.AddWithValue("@status", "INSERT");
                                cmd.Parameters.AddWithValue("@SemesterId", Details.Name.ToString());
                                cmd.Parameters.AddWithValue("@CourseId", Convert.ToInt32(Details.CoursesInfo[i].Id));
                                SqlDataAdapter da = new SqlDataAdapter(cmd);
                                da.Fill(ds);
                            }
                        }
                    }
                }
                con.Close();
            

            return RedirectToAction("ManageSemester", "Courses");
        }
        public ActionResult ViewSemester(int id)
        {
            int UserId = Convert.ToInt32(Session["UserId"]);
            if (UserId == 0)
            {
                return RedirectToAction("Index", "Login");
            }
            SemesterClass objuser = new SemesterClass();
            
                DataSet ds = new DataSet();
                using (SqlCommand cmd = new SqlCommand("dbo.Semestercrudoperations", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@status", "GETID");
                    cmd.Parameters.AddWithValue("@Id", id);
                    cmd.Parameters.AddWithValue("@Name", "");
                    cmd.Parameters.AddWithValue("@Description", "");
                    cmd.Parameters.AddWithValue("@StartDate", "");
                    cmd.Parameters.AddWithValue("@EndDate", "");
                    con.Open();

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);
                    List<SemesterClass> userlist = new List<SemesterClass>();

                    objuser.Id = Convert.ToInt32(ds.Tables[0].Rows[0]["Id"].ToString());
                    objuser.Name = ds.Tables[0].Rows[0]["Name"].ToString();
                    objuser.Description = ds.Tables[0].Rows[0]["Description"].ToString();
                    objuser.StartDate = ds.Tables[0].Rows[0]["StartDate"].ToString();
                    objuser.EndDate = ds.Tables[0].Rows[0]["EndDate"].ToString();
                    //userlist.Add(uobj);
                    //objuser.SemesterInfo = userlist;
                }
                ds = new DataSet();
                using (SqlCommand cmd = new SqlCommand("dbo.FetchCourses", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@SemesterId", objuser.Id);
                    cmd.Parameters.AddWithValue("@status", "GETLINKEDNAME");
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);
                    objuser.Courses = ds.Tables[0].Rows[0].ItemArray[0].ToString();
                    //userlist.Add(uobj);
                    //objuser.SemesterInfo = userlist;
                }
                con.Close();
            
            return View(objuser);

        }
        public ActionResult UpdateSemester(int Id)
        {
            int UserId = Convert.ToInt32(Session["UserId"]);
            if (UserId == 0)
            {
                return RedirectToAction("Index", "Login");
            }
            SemesterClass objuser = new SemesterClass();
           
                con.Open();
                DataSet ds = new DataSet();
                using (SqlCommand cmd = new SqlCommand("dbo.Semestercrudoperations", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@status", "GETID");
                    cmd.Parameters.AddWithValue("@Id", Id);
                    cmd.Parameters.AddWithValue("@Name", "");
                    cmd.Parameters.AddWithValue("@Description", "");
                    cmd.Parameters.AddWithValue("@StartDate", "");
                    cmd.Parameters.AddWithValue("@EndDate", "");

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);
                    List<SemesterClass> userlist = new List<SemesterClass>();

                    objuser.Id = Convert.ToInt32(ds.Tables[0].Rows[0]["Id"].ToString());
                    objuser.Name = ds.Tables[0].Rows[0]["Name"].ToString();
                    objuser.Description = ds.Tables[0].Rows[0]["Description"].ToString();
                    objuser.StartDate = ds.Tables[0].Rows[0]["StartDate"].ToString();
                    objuser.EndDate = ds.Tables[0].Rows[0]["EndDate"].ToString();
                    //userlist.Add(uobj);
                    //objuser.SemesterInfo = userlist;
                }
                ds = new DataSet();
                using (SqlCommand cmd = new SqlCommand("dbo.Coursescrudoperations", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@status", "GET");
                    cmd.Parameters.AddWithValue("@Id", 0);
                    cmd.Parameters.AddWithValue("@Name", "");
                    cmd.Parameters.AddWithValue("@IsEnable", 0);
                    cmd.Parameters.AddWithValue("@ExamFormat", 0);


                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);
                    objuser.CoursesInfo = new List<CheckBoxDetails>();
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        //String test = ds.Tables[0].Rows[i].ItemArray[1].ToString();
                        //String test = ds.Tables[0].Rows[i]["Name"].ToString();
                        CheckBoxDetails temp = new CheckBoxDetails();
                        temp.Id = Convert.ToInt32(ds.Tables[0].Rows[i].ItemArray[0]);
                        temp.Text = ds.Tables[0].Rows[i].ItemArray[1].ToString();
                        DataSet newds = new DataSet();
                        using (SqlCommand newcmd = new SqlCommand("dbo.CoursesSemesterLinkProc", con))
                        {
                            newcmd.CommandType = CommandType.StoredProcedure;
                            newcmd.Parameters.AddWithValue("@SemesterId", objuser.Id);
                            newcmd.Parameters.AddWithValue("@status", "GETCOUNT");
                            newcmd.Parameters.AddWithValue("@CourseId", temp.Id);
                            SqlDataAdapter newda = new SqlDataAdapter(newcmd);
                            newda.Fill(newds);
                            int count = Convert.ToInt32(newds.Tables[0].Rows[0].ItemArray[0]);
                            temp.IsChecked = false;
                            if (count > 0)
                            {
                                temp.IsChecked = true;
                            }
                            objuser.CoursesInfo.Add(temp);
                            //objuser.CoursesInfo.Add("Course2");
                        }
                    }

                }
                con.Close();
            
            return View(objuser);
        }
        [HttpGet]
        public int cmDeleteSemester(int Id)
        {
            int ret = 1;
            
                con.Open();
                DataSet ds = new DataSet();
                using (SqlCommand cmd = new SqlCommand("dbo.Semestercrudoperations", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@status", "DELETE");
                    cmd.Parameters.AddWithValue("@Id", Id);
                    cmd.Parameters.AddWithValue("@Name", "");
                    cmd.Parameters.AddWithValue("@Description", "");
                    cmd.Parameters.AddWithValue("@StartDate", "");
                    cmd.Parameters.AddWithValue("@EndDate", "");
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);
                }
                con.Close();
            
            return ret;
        }

        public ActionResult DeleteSemester(int Id)
        {
           
                con.Open();
                DataSet ds = new DataSet();
                using (SqlCommand cmd = new SqlCommand("dbo.Semestercrudoperations", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@status", "DELETE");
                    cmd.Parameters.AddWithValue("@Id", Id);
                    cmd.Parameters.AddWithValue("@Name", "");
                    cmd.Parameters.AddWithValue("@Description", "");
                    cmd.Parameters.AddWithValue("@StartDate", "");
                    cmd.Parameters.AddWithValue("@EndDate", "");
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);
                }
                con.Close();
            
            return RedirectToAction("ManageSemester", "Courses");
        }
        public ActionResult SubjectsList()
        {
            int UserId = Convert.ToInt32(Session["UserId"]);
            if (UserId == 0)
            {
                return RedirectToAction("Index", "Login");
            }
            CourseList objsubject = new CourseList();
            objsubject.Course = new List<CoursesModel>();
            
                con.Open();
                using (SqlCommand cmd = new SqlCommand("dbo.Coursescrudoperations", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@status", "GET");
                    cmd.Parameters.AddWithValue("@Id", 0);
                    cmd.Parameters.AddWithValue("@Name", "");
                    cmd.Parameters.AddWithValue("@ExamFormat", 0);
                    cmd.Parameters.AddWithValue("@IsEnable", 0);
                    DataSet ds = new DataSet();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        CoursesModel temp = new CoursesModel();
                        temp.Id = Convert.ToInt32(ds.Tables[0].Rows[i].ItemArray[0]);
                        temp.Name = ds.Tables[0].Rows[i].ItemArray[1].ToString();
                        objsubject.Course.Add(temp);
                    }
                }
                con.Close();
            

            return View(objsubject);
        }
        public ActionResult DeactivatedBatch()
        {
            return View();
        }
        public ActionResult CreateCourse()
        {
            int UserId = Convert.ToInt32(Session["UserId"]);
            if (UserId == 0)
            {
                return RedirectToAction("Index", "Login");
            }
            CoursesModel obj = new CoursesModel();
            obj.IsEnable = new CheckBoxDetails();
            obj.ExamFormat = new List<CheckBoxDetails>();
            obj.IsEnable.IsChecked = false;
            DataSet ds = new DataSet();
           
                con.Open();
                using (SqlCommand cmd = new SqlCommand("dbo.ExamFormatCrudOper", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@status", "GET");
                    cmd.Parameters.AddWithValue("@Id", 0);
                    cmd.Parameters.AddWithValue("@Name", "");
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        CheckBoxDetails temp = new CheckBoxDetails();
                        temp.Id = Convert.ToInt32(ds.Tables[0].Rows[i].ItemArray[0]);
                        temp.Text = ds.Tables[0].Rows[i].ItemArray[1].ToString();
                        temp.IsChecked = false;
                        obj.ExamFormat.Add(temp);
                        //objuser.CoursesInfo.Add("Course2");
                    }

                }
                con.Close();
            
            return View(obj);
        }
        public ActionResult EditCourse(int CourseId)
        {
            int UserId = Convert.ToInt32(Session["UserId"]);
            if (UserId == 0)
            {
                return RedirectToAction("Index", "Login");
            }
            CoursesModel obj = new CoursesModel();
            obj.IsEnable = new CheckBoxDetails();
            obj.ExamFormat = new List<CheckBoxDetails>();
            obj.IsEnable.IsChecked = false;
            DataSet ds = new DataSet();
                con.Open();
                using (SqlCommand cmd = new SqlCommand("dbo.ExamFormatCrudOper", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@status", "GET");
                    cmd.Parameters.AddWithValue("@Id", 0);
                    cmd.Parameters.AddWithValue("@Name", "");
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        CheckBoxDetails temp = new CheckBoxDetails();
                        temp.Id = Convert.ToInt32(ds.Tables[0].Rows[i].ItemArray[0]);
                        temp.Text = ds.Tables[0].Rows[i].ItemArray[1].ToString();
                        temp.IsChecked = false;
                        obj.ExamFormat.Add(temp);
                        //objuser.CoursesInfo.Add("Course2");
                    }

                }
                con.Close();
            return View(obj);
        }
        [HttpPost]
        public ActionResult InsertCourse(GS247.Models.Courses.CoursesModel Details)
        {

                con.Open();
                DataSet ds = new DataSet();
                using (SqlCommand cmd = new SqlCommand("dbo.Coursescrudoperations", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@status", "INSERT");
                    cmd.Parameters.AddWithValue("@Id", 0);
                    cmd.Parameters.AddWithValue("@Name", Details.Name.ToString());
                    if (Details.IsEnable.IsChecked == true)
                    { cmd.Parameters.AddWithValue("@IsEnable", 1); }
                    else { cmd.Parameters.AddWithValue("@IsEnable", 0); }
                    cmd.Parameters.AddWithValue("@ExamFormat", "");
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);

                }
                con.Close();
             return RedirectToAction("CreateCourse", "Courses");
        }
        [HttpGet]
        public int DeleteCourse(int CourseId)
        {
            int ret = 0;
           
                con.Open();
                DataSet ds = new DataSet();
                using (SqlCommand cmd = new SqlCommand("dbo.Coursescrudoperations", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@status", "DELETE");
                    cmd.Parameters.AddWithValue("@Id", CourseId);
                    cmd.Parameters.AddWithValue("@Name", "");
                    cmd.Parameters.AddWithValue("@IsEnable", 0);
                    cmd.Parameters.AddWithValue("@ExamFormat", "");
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);

                }
                con.Close();
            

            return ret;
        }
        public int UpdateCourse(int CourseId,String CourseName)
        {
            int ret = 1;
            
                con.Open();
                DataSet ds = new DataSet();
                using (SqlCommand cmd = new SqlCommand("dbo.Coursescrudoperations", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@status", "UPDATE");
                    cmd.Parameters.AddWithValue("@Id", CourseId);
                    cmd.Parameters.AddWithValue("@Name", CourseName);
                    cmd.Parameters.AddWithValue("@IsEnable", 0); 
                    cmd.Parameters.AddWithValue("@ExamFormat", "");
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);

                }
                con.Close();
            
            return ret;
        }
        public ActionResult ManageCourse()
        {
            int UserId = Convert.ToInt32(Session["UserId"]);
            if (UserId == 0)
            {
                return RedirectToAction("Index", "Login");
            }
            AllObjects obj = new AllObjects();
            obj.Items = new List<SubAllObjects>();
           
                con.Open();
                using (SqlCommand cmd = new SqlCommand("dbo.Coursescrudoperations", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@status", "GET");
                    cmd.Parameters.AddWithValue("@Id", 0);
                    cmd.Parameters.AddWithValue("@Name", "");
                    cmd.Parameters.AddWithValue("@IsEnable", 0);
                    cmd.Parameters.AddWithValue("@ExamFormat", 0);
                    DataSet ds = new DataSet();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        SubAllObjects sub = new SubAllObjects();
                        int CourseId = Convert.ToInt32(ds.Tables[0].Rows[i].ItemArray[0]);
                        sub.Course = new CoursesModel();
                        sub.Course.Id = CourseId;
                        sub.Course.Name = ds.Tables[0].Rows[i].ItemArray[1].ToString();
                        using (SqlCommand cmd1 = new SqlCommand("dbo.BatchCrudOper", con))
                        {
                            cmd1.CommandType = CommandType.StoredProcedure;
                            cmd1.Parameters.AddWithValue("@status", "GETBATCHFORCOURSE");
                            cmd1.Parameters.AddWithValue("@Id", 0);
                            cmd1.Parameters.AddWithValue("@Name", "");
                            cmd1.Parameters.AddWithValue("@Class", "");
                            cmd1.Parameters.AddWithValue("@StartDate", "");
                            cmd1.Parameters.AddWithValue("@EndDate", "");
                            cmd1.Parameters.AddWithValue("@TeacherId", 0);
                            cmd1.Parameters.AddWithValue("@SemesterId", 0);
                            cmd1.Parameters.AddWithValue("@CourseId", CourseId);
                            cmd1.Parameters.AddWithValue("@TimeTableFormatId", 0);
                            DataSet ds1 = new DataSet();
                            SqlDataAdapter da1 = new SqlDataAdapter(cmd1);
                            da1.Fill(ds1);
                            if (ds1.Tables.Count>0)
                            {
                                sub.Batch = new List<BatchClass>();
                                for (int j = 0; j < ds1.Tables[0].Rows.Count; j++)
                                {
                                    BatchClass Batch = new BatchClass();
                                    Batch.Id = Convert.ToInt32(ds1.Tables[0].Rows[j].ItemArray[0]); ;
                                    Batch.Name = ds1.Tables[0].Rows[j].ItemArray[1].ToString();
                                    Batch.StartDate = ds1.Tables[0].Rows[j].ItemArray[2].ToString();
                                    Batch.EndDate = ds1.Tables[0].Rows[j].ItemArray[3].ToString();
                                    /*Batch.Teacher = new TeacherModel();
                                    Batch.Teacher.Id = 
                                    Batch.Teacher.Name = "";*/
                                    sub.Batch.Add(Batch);
                                }
                            }
                        }
                        using (SqlCommand cmd1 = new SqlCommand("dbo.BatchCrudOper", con))
                        {
                            cmd1.CommandType = CommandType.StoredProcedure;
                            cmd1.Parameters.AddWithValue("@status", "GETSUBFORCOURSE");
                            cmd1.Parameters.AddWithValue("@Id", 0);
                            cmd1.Parameters.AddWithValue("@Name", "");
                            cmd1.Parameters.AddWithValue("@Class", "");
                            cmd1.Parameters.AddWithValue("@StartDate", "");
                            cmd1.Parameters.AddWithValue("@EndDate", "");
                            cmd1.Parameters.AddWithValue("@TeacherId", 0);
                            cmd1.Parameters.AddWithValue("@SemesterId", 0);
                            cmd1.Parameters.AddWithValue("@CourseId", CourseId);
                            cmd1.Parameters.AddWithValue("@TimeTableFormatId", 0);
                            DataSet ds1 = new DataSet();
                            SqlDataAdapter da1 = new SqlDataAdapter(cmd1);
                            da1.Fill(ds1);
                            sub.Subjects = new List<SubjectModel>();
                            if (ds1.Tables.Count>0)
                            {
                                for (int j = 0; j < ds1.Tables[0].Rows.Count; j++)
                                {
                                    SubjectModel Subject = new SubjectModel();
                                    Subject.Id = Convert.ToInt32(ds1.Tables[0].Rows[j].ItemArray[0]); ;
                                    Subject.Name = ds1.Tables[0].Rows[j].ItemArray[1].ToString();
                                    Subject.FirstSub = ds1.Tables[0].Rows[j].ItemArray[2].ToString();
                                    Subject.SecondSub = ds1.Tables[0].Rows[j].ItemArray[3].ToString();
                                    Subject.Weekly = Convert.ToInt32(ds1.Tables[0].Rows[j].ItemArray[4]);
                                    
                                    sub.Subjects.Add(Subject);
                                }
                            }

                        }
                        using (SqlCommand cmd1 = new SqlCommand("dbo.BatchCrudOper", con))
                        {
                            cmd1.CommandType = CommandType.StoredProcedure;
                            cmd1.Parameters.AddWithValue("@status", "GETSEMFORCOURSE");
                            cmd1.Parameters.AddWithValue("@Id", 0);
                            cmd1.Parameters.AddWithValue("@Name", "");
                            cmd1.Parameters.AddWithValue("@Class", "");
                            cmd1.Parameters.AddWithValue("@StartDate", "");
                            cmd1.Parameters.AddWithValue("@EndDate", "");
                            cmd1.Parameters.AddWithValue("@TeacherId", 0);
                            cmd1.Parameters.AddWithValue("@SemesterId", 0);
                            cmd1.Parameters.AddWithValue("@CourseId", CourseId);
                            cmd1.Parameters.AddWithValue("@TimeTableFormatId", 0);
                            DataSet ds1 = new DataSet();
                            SqlDataAdapter da1 = new SqlDataAdapter(cmd1);
                            da1.Fill(ds1);
                            if (ds1.Tables.Count>0)
                            {
                                sub.Semester = new List<SemesterClass>();
                                for (int j = 0; j < ds1.Tables[0].Rows.Count; j++)
                                {
                                    SemesterClass semester = new SemesterClass();
                                    semester.Id = Convert.ToInt32(ds1.Tables[0].Rows[j].ItemArray[0]); ;
                                    semester.Name = ds1.Tables[0].Rows[j].ItemArray[1].ToString();
                                    semester.Description = ds1.Tables[0].Rows[j].ItemArray[2].ToString();
                                    semester.StartDate = ds1.Tables[0].Rows[j].ItemArray[3].ToString();
                                    semester.EndDate = ds1.Tables[0].Rows[j].ItemArray[3].ToString();
                                    sub.Semester.Add(semester);
                                }
                            }

                        }
                        obj.Items.Add(sub);
                    }
                }
                con.Close();
            return View(obj);
        }
        [HttpGet]
        public JsonResult GetBatch(int BatchId)
        {
            BatchClass obj = new BatchClass();
            
                con.Open();
                using (SqlCommand cmd = new SqlCommand("dbo.BatchCrudOper", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Status", "GET");
                    cmd.Parameters.AddWithValue("@Id", BatchId);
                    cmd.Parameters.AddWithValue("@Name", "");
                    cmd.Parameters.AddWithValue("@Class", "");
                    cmd.Parameters.AddWithValue("@StartDate", "");
                    cmd.Parameters.AddWithValue("@EndDate", "");
                    cmd.Parameters.AddWithValue("@CourseId", 0);
                    cmd.Parameters.AddWithValue("@TeacherId", 0);
                    cmd.Parameters.AddWithValue("@SemesterId", 0);
                    cmd.Parameters.AddWithValue("@TimeTableFormatId", 0);
                    DataSet ds = new DataSet();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);
                    if (ds.Tables.Count > 0)
                    {
                       obj.Id = Convert.ToInt32(ds.Tables[0].Rows[0].ItemArray[0]);
                       obj.Name = ds.Tables[0].Rows[0].ItemArray[1].ToString();
                       obj.StartDate = ds.Tables[0].Rows[0].ItemArray[2].ToString();
                       obj.EndDate = ds.Tables[0].Rows[0].ItemArray[3].ToString();
                          /*obj.Teacher = new TeacherModel();
                            obj.Teacher.Id=*/
                    }
                }
                con.Close();
            
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public int DeleteBatch(int BatchId)
        {
            int ret = 1;
            
                con.Open();
                using (SqlCommand cmd = new SqlCommand("dbo.BatchCrudOper", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Status", "DELETE");
                    cmd.Parameters.AddWithValue("@Id", BatchId);
                    cmd.Parameters.AddWithValue("@Name", "");
                    cmd.Parameters.AddWithValue("@Class", "");
                    cmd.Parameters.AddWithValue("@StartDate", "");
                    cmd.Parameters.AddWithValue("@EndDate", "");
                    cmd.Parameters.AddWithValue("@CourseId", 0);
                    cmd.Parameters.AddWithValue("@TeacherId", 0);
                    cmd.Parameters.AddWithValue("@SemesterId", 0);
                    cmd.Parameters.AddWithValue("@TimeTableFormatId", 0);
                    DataSet ds = new DataSet();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);
                }
                con.Close();
            
            return ret;
        }
        [HttpGet]
        public int InsertBatch(int CourseId, String Name, String StartDate, String EndDate, int BatchId,int Teacher)
        {
            int ret = 1;
            
                con.Open();
                using (SqlCommand cmd = new SqlCommand("dbo.BatchCrudOper", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    String Status = "INSERT";
                    if (BatchId > 0)
                    {
                        Status = "UPDATE";
                    }
                    cmd.Parameters.AddWithValue("@Status", Status);
                    cmd.Parameters.AddWithValue("@Id", BatchId);
                    cmd.Parameters.AddWithValue("@Name", Name);
                    cmd.Parameters.AddWithValue("@Class", "");
                    cmd.Parameters.AddWithValue("@StartDate", StartDate);
                    cmd.Parameters.AddWithValue("@EndDate", EndDate);
                    cmd.Parameters.AddWithValue("@CourseId", CourseId);
                    cmd.Parameters.AddWithValue("@TeacherId", Teacher);
                    cmd.Parameters.AddWithValue("@SemesterId", 0);
                    cmd.Parameters.AddWithValue("@TimeTableFormatId", 0);
                    DataSet ds = new DataSet();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);
                }
                con.Close();
            
            return ret;
        }

        [HttpGet]
        public int InsertSubject(int CourseId,String Name,String FirstSub,String SecondSub,int weekly,int SubjectId)
        {
            int ret = 1;
            
                con.Open();
                using (SqlCommand cmd = new SqlCommand("dbo.SubjectsCrudProc", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    String Status = "INSERT";
                    if (SubjectId>0)
                    {
                        Status = "UPDATE";
                    }
                    cmd.Parameters.AddWithValue("@status", Status);
                    cmd.Parameters.AddWithValue("@Id", SubjectId);
                    cmd.Parameters.AddWithValue("@Name", Name);
                    cmd.Parameters.AddWithValue("@FirstSub", FirstSub);
                    cmd.Parameters.AddWithValue("@SecondSub", SecondSub);
                    cmd.Parameters.AddWithValue("@Weekly", weekly);
                    cmd.Parameters.AddWithValue("@CourseId", CourseId);
                    DataSet ds = new DataSet();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);
                }
                con.Close();
            
            return ret;
        }
        public ActionResult ManageAllCourses()
        {
            int UserId = Convert.ToInt32(Session["UserId"]);
            if (UserId == 0)
            {
                return RedirectToAction("Index", "Login");
            }
            return View();
        }
        [HttpGet]
        public JsonResult GetResults(int Id)
        {
            List<SubjectModel> obj = new List<SubjectModel>();
            
                con.Open();
                using (SqlCommand cmd = new SqlCommand("dbo.SubjectsCrudProc", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@status", "GETSUBJ");
                    cmd.Parameters.AddWithValue("@Id", 0);
                    cmd.Parameters.AddWithValue("@Name", "");
                    cmd.Parameters.AddWithValue("@FirstSub", "");
                    cmd.Parameters.AddWithValue("@SecondSub", "");
                    cmd.Parameters.AddWithValue("@Weekly", 0);
                    cmd.Parameters.AddWithValue("@CourseId", Id);
                    DataSet ds = new DataSet();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        SubjectModel json = new SubjectModel();
                        json.Id = Convert.ToInt32(ds.Tables[0].Rows[i].ItemArray[0]);
                        //json.Id = i + 1;
                        json.Name = ds.Tables[0].Rows[i].ItemArray[1].ToString();
                        json.FirstSub = ds.Tables[0].Rows[i].ItemArray[2].ToString();
                        json.SecondSub = ds.Tables[0].Rows[i].ItemArray[3].ToString();
                        json.Weekly = Convert.ToInt32(ds.Tables[0].Rows[i].ItemArray[4]);
                        obj.Add(json);
                    }
                }
                con.Close();
            
            return Json(obj,JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetAllCourses(int CourseId)
        {
            List<CoursesModel> obj = new List<CoursesModel>();
            
                con.Open();
                using (SqlCommand cmd = new SqlCommand("dbo.Coursescrudoperations", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    String StatusStr = "GET";
                    if(CourseId>0)
                    {
                        StatusStr = "GETID";
                    }
                    cmd.Parameters.AddWithValue("@status", StatusStr);
                    cmd.Parameters.AddWithValue("@Id", CourseId);
                    cmd.Parameters.AddWithValue("@Name", "");
                    cmd.Parameters.AddWithValue("@ExamFormat", 0);
                    cmd.Parameters.AddWithValue("@IsEnable", 0);
                    DataSet ds = new DataSet();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        CoursesModel temp = new CoursesModel();
                        temp.Id = Convert.ToInt32(ds.Tables[0].Rows[i].ItemArray[0]);
                        temp.Name = ds.Tables[0].Rows[i].ItemArray[1].ToString();
                        temp.IsEnable = new CheckBoxDetails();
                        obj.Add(temp);
                    }
                }
                con.Close();
            
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetCourseWithSubject(int SubjectId)
        {
            CourseSubject obj = new CourseSubject();
           
                con.Open();
                using (SqlCommand cmd = new SqlCommand("dbo.SubjectsCrudProc", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@status", "GETID");
                    cmd.Parameters.AddWithValue("@Id", SubjectId);
                    cmd.Parameters.AddWithValue("@Name", "");
                    cmd.Parameters.AddWithValue("@FirstSub", 0);
                    cmd.Parameters.AddWithValue("@SecondSub", 0);
                    cmd.Parameters.AddWithValue("@Weekly", 0);
                    cmd.Parameters.AddWithValue("@CourseId", 0);
                    DataSet ds = new DataSet();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);
                    obj.Course = new CoursesModel();
                    obj.Course.Id = Convert.ToInt32(ds.Tables[0].Rows[0].ItemArray[5]); ;
                    obj.Course.Name = ds.Tables[0].Rows[0].ItemArray[6].ToString();
                    obj.Subjects = new SubjectModel();
                    obj.Subjects.Id = SubjectId;
                    obj.Subjects.Name = ds.Tables[0].Rows[0].ItemArray[1].ToString();
                    obj.Subjects.FirstSub = ds.Tables[0].Rows[0].ItemArray[2].ToString();
                    obj.Subjects.SecondSub = ds.Tables[0].Rows[0].ItemArray[3].ToString();
                    obj.Subjects.Weekly = Convert.ToInt32(ds.Tables[0].Rows[0].ItemArray[4]);
                }
                con.Close();
            
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult DeleteSubject(int SubjectId)
        {
            
                con.Open();
                using (SqlCommand cmd = new SqlCommand("dbo.SubjectsCrudProc", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@status", "DELETE");
                    cmd.Parameters.AddWithValue("@Id", SubjectId);
                    cmd.Parameters.AddWithValue("@Name", "");
                    cmd.Parameters.AddWithValue("@FirstSub", 0);
                    cmd.Parameters.AddWithValue("@SecondSub", 0);
                    cmd.Parameters.AddWithValue("@Weekly", 0);
                    cmd.Parameters.AddWithValue("@CourseId", 0);
                    DataSet ds = new DataSet();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);
                 }
                con.Close();
            
            return RedirectToAction("SubjectsList", "Courses");
        }


    }
}