using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GS247.Models;
using GS247.Models.Teachers;
using GS247.Controllers;
using GS247.Controllers.Connection;
using System.Configuration;
using static GS247.Controllers.Connection.Connectivity;
using DocumentFormat.OpenXml.Spreadsheet;
//using DocumentFormat.OpenXml.Bibliography;

namespace GS247.Controllers
{
    public class TeachersController : Controller
    {
        public SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["GS247"].ConnectionString);
        Connectivity Connect = new Connectivity();
        List<ParamValues> param = new List<ParamValues>();
        // GET: Teachers
        public ActionResult Index()
        {
            return View();
        }
        //Category Views
        public ActionResult ManageCategory()
        {
            CategoryList obj = new CategoryList();
            obj.Categories = new List<Models.Teachers.Category>();
            con.Open();
            SqlDataReader ExecReader = null;
            ExecReader = Connect.QueryExecute(con,"Select Id,Name,Prefix from dbo.TeachersManageCategories");
            while(ExecReader.Read())
            {
                Models.Teachers.Category subobj = new Models.Teachers.Category();
                subobj.Id = Convert.ToInt32(ExecReader.GetValue(0));
                subobj.Name = ExecReader.GetValue(1).ToString();
                subobj.Prefix = ExecReader.GetValue(2).ToString();
                obj.Categories.Add(subobj);
            }
            con.Close();
            return View(obj);
        }
        [HttpPost]
        public ActionResult InsertCategory(GS247.Models.Teachers.Category data)
        {
            
                con.Open();
                DataSet ds = new DataSet();
                using (SqlCommand cmd = new SqlCommand("dbo.TeachersSettings", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    String str = "InsertCategory";
                    int val = 0;
                    if (data.Id > 0)
                    {
                        str = "UpdateCategory";
                        val = data.Id;
                    }
                    cmd.Parameters.AddWithValue("@status", str);
                    cmd.Parameters.AddWithValue("@Id", val);
                    cmd.Parameters.AddWithValue("@Name", data.Name);
                    cmd.Parameters.AddWithValue("@Code", data.Prefix);
                    cmd.Parameters.AddWithValue("@AdditionalId", 0);
                    cmd.Parameters.AddWithValue("@AddText", "");
                    cmd.Parameters.AddWithValue("@MaxDay", 0);
                    cmd.Parameters.AddWithValue("@MaxWeek", 0);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);

                }
                con.Close();
            return RedirectToAction("ManageCategory", "Teachers");
        }


        public ActionResult DeleteCategory(int Id)
        {
            using (SqlConnection con = new SqlConnection("data source=43.255.152.25;initial catalog=GS247;persist security info=True;user id=_system;password=Gl$rv247;MultipleActiveResultSets=True;"))
            {
                con.Open();
                DataSet ds = new DataSet();
                using (SqlCommand cmd = new SqlCommand("dbo.TeachersSettings", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@status", "DeleteCategory");
                    cmd.Parameters.AddWithValue("@Id", Id);
                    cmd.Parameters.AddWithValue("@Name", "");
                    cmd.Parameters.AddWithValue("@Code", "");
                    cmd.Parameters.AddWithValue("@AdditionalId", 0);
                    cmd.Parameters.AddWithValue("@AddText", "");
                    cmd.Parameters.AddWithValue("@MaxDay", 0);
                    cmd.Parameters.AddWithValue("@MaxWeek", 0);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);

                }
                con.Close();
            }

            return RedirectToAction("ManageCategory", "Teachers");
        }
        public ActionResult AddCategory()
        {
            Models.Teachers.Category obj = new Models.Teachers.Category();
            return View(obj);
        }
        public ActionResult UpdateCategory(int Id)
        {
            Models.Teachers.Category obj = new Models.Teachers.Category();
            using (SqlConnection con = new SqlConnection("data source=43.255.152.25;initial catalog=GS247;persist security info=True;user id=_system;password=Gl$rv247;MultipleActiveResultSets=True;"))
            {
                con.Open();
                DataSet ds = new DataSet();
                using (SqlCommand cmd = new SqlCommand("dbo.TeachersSettings", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@status", "GetCategory");
                    cmd.Parameters.AddWithValue("@Id", Id);
                    cmd.Parameters.AddWithValue("@Name", "");
                    cmd.Parameters.AddWithValue("@Code", "");
                    cmd.Parameters.AddWithValue("@AdditionalId", 0);
                    cmd.Parameters.AddWithValue("@AddText", "");
                    cmd.Parameters.AddWithValue("@MaxDay", 0);
                    cmd.Parameters.AddWithValue("@MaxWeek", 0);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);
                    if (ds.Tables.Count > 0)
                    {
                        obj.Id = Id;
                        obj.Name = ds.Tables[0].Rows[0].ItemArray[1].ToString();
                        obj.Prefix = ds.Tables[0].Rows[0].ItemArray[2].ToString();
                    }

                }
                con.Close();
            }
            return View(obj);
        }

        //Department Views
        public ActionResult ManageDepartment()
        {
            DepartmentList obj = new DepartmentList();
            obj.Departments = new List<Department>();
            using (SqlConnection con = new SqlConnection("data source=43.255.152.25;initial catalog=GS247;persist security info=True;user id=_system;password=Gl$rv247;MultipleActiveResultSets=True;"))
            {
                con.Open();
                DataSet ds = new DataSet();
                using (SqlCommand cmd = new SqlCommand("dbo.TeachersSettings", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@status", "ViewDepartment");
                    cmd.Parameters.AddWithValue("@Id", 0);
                    cmd.Parameters.AddWithValue("@Name", "");
                    cmd.Parameters.AddWithValue("@Code", "");
                    cmd.Parameters.AddWithValue("@AdditionalId", 0);
                    cmd.Parameters.AddWithValue("@AddText", "");
                    cmd.Parameters.AddWithValue("@MaxDay", 0);
                    cmd.Parameters.AddWithValue("@MaxWeek", 0);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);
                    if (ds.Tables.Count > 0)
                    {
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            Department subobj = new Department();
                            subobj.Id = Convert.ToInt32(ds.Tables[0].Rows[i].ItemArray[0]);
                            subobj.Name = ds.Tables[0].Rows[i].ItemArray[1].ToString();
                            subobj.Code = ds.Tables[0].Rows[i].ItemArray[2].ToString();
                            obj.Departments.Add(subobj);
                        }
                    }
                }
                con.Close();
            }
            return View(obj);
        }
        [HttpPost]
        public ActionResult InsertDepartment(GS247.Models.Teachers.Department data)
        {
            using (SqlConnection con = new SqlConnection("data source=43.255.152.25;initial catalog=GS247;persist security info=True;user id=_system;password=Gl$rv247;MultipleActiveResultSets=True;"))
            {
                con.Open();
                DataSet ds = new DataSet();
                using (SqlCommand cmd = new SqlCommand("dbo.TeachersSettings", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    String str = "InsertDepartment";
                    int val = 0;
                    if (data.Id > 0)
                    {
                        str = "UpdateDepartment";
                        val = data.Id;
                    }
                    cmd.Parameters.AddWithValue("@status", str);
                    cmd.Parameters.AddWithValue("@Id", val);
                    cmd.Parameters.AddWithValue("@Name", data.Name);
                    cmd.Parameters.AddWithValue("@Code", data.Code);
                    cmd.Parameters.AddWithValue("@AdditionalId", 0);
                    cmd.Parameters.AddWithValue("@AddText", "");
                    cmd.Parameters.AddWithValue("@MaxDay", 0);
                    cmd.Parameters.AddWithValue("@MaxWeek", 0);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);

                }
                con.Close();
            }

            return RedirectToAction("ManageDepartment", "Teachers");
        }


        public ActionResult DeleteDepartment(int Id)
        {
            using (SqlConnection con = new SqlConnection("data source=43.255.152.25;initial catalog=GS247;persist security info=True;user id=_system;password=Gl$rv247;MultipleActiveResultSets=True;"))
            {
                con.Open();
                DataSet ds = new DataSet();
                using (SqlCommand cmd = new SqlCommand("dbo.TeachersSettings", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@status", "DeleteDepartment");
                    cmd.Parameters.AddWithValue("@Id", Id);
                    cmd.Parameters.AddWithValue("@Name", "");
                    cmd.Parameters.AddWithValue("@Code", "");
                    cmd.Parameters.AddWithValue("@AdditionalId", 0);
                    cmd.Parameters.AddWithValue("@AddText", "");
                    cmd.Parameters.AddWithValue("@MaxDay", 0);
                    cmd.Parameters.AddWithValue("@MaxWeek", 0);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);

                }
                con.Close();
            }

            return RedirectToAction("ManageDepartment", "Teachers");
        }
        public ActionResult AddDepartment()
        {
            Department obj = new Department();
            return View(obj);
        }
        public ActionResult UpdateDepartment(int Id)
        {
            Department obj = new Department();
            using (SqlConnection con = new SqlConnection("data source=43.255.152.25;initial catalog=GS247;persist security info=True;user id=_system;password=Gl$rv247;MultipleActiveResultSets=True;"))
            {
                con.Open();
                DataSet ds = new DataSet();
                using (SqlCommand cmd = new SqlCommand("dbo.TeachersSettings", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@status", "GetDepartment");
                    cmd.Parameters.AddWithValue("@Id", Id);
                    cmd.Parameters.AddWithValue("@Name", "");
                    cmd.Parameters.AddWithValue("@Code", "");
                    cmd.Parameters.AddWithValue("@AdditionalId", 0);
                    cmd.Parameters.AddWithValue("@AddText", "");
                    cmd.Parameters.AddWithValue("@MaxDay", 0);
                    cmd.Parameters.AddWithValue("@MaxWeek", 0);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);
                    if (ds.Tables.Count > 0)
                    {
                        obj.Id = Id;
                        obj.Name = ds.Tables[0].Rows[0].ItemArray[1].ToString();
                        obj.Code = ds.Tables[0].Rows[0].ItemArray[2].ToString();
                    }

                }
                con.Close();
            }
            return View(obj);
        }

        //Grade Views
        public ActionResult ManageGrade()
        {
            GradeList obj = new GradeList();
            obj.Grades = new List<Grade>();
            using (SqlConnection con = new SqlConnection("data source=43.255.152.25;initial catalog=GS247;persist security info=True;user id=_system;password=Gl$rv247;MultipleActiveResultSets=True;"))
            {
                con.Open();
                DataSet ds = new DataSet();
                using (SqlCommand cmd = new SqlCommand("dbo.TeachersSettings", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@status", "ViewGrade");
                    cmd.Parameters.AddWithValue("@Id", 0);
                    cmd.Parameters.AddWithValue("@Name", "");
                    cmd.Parameters.AddWithValue("@Code", "");
                    cmd.Parameters.AddWithValue("@AdditionalId", 0);
                    cmd.Parameters.AddWithValue("@AddText", "");
                    cmd.Parameters.AddWithValue("@MaxDay", 0);
                    cmd.Parameters.AddWithValue("@MaxWeek", 0);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);
                    if (ds.Tables.Count > 0)
                    {
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            Grade subobj = new Grade();
                            subobj.Id = Convert.ToInt32(ds.Tables[0].Rows[i].ItemArray[0]);
                            subobj.Name = ds.Tables[0].Rows[i].ItemArray[1].ToString();
                            subobj.MaxHoursDay = Convert.ToInt32(ds.Tables[0].Rows[i].ItemArray[4]);
                            subobj.MaxHoursWeek = Convert.ToInt32(ds.Tables[0].Rows[i].ItemArray[5]);
                            obj.Grades.Add(subobj);
                        }
                    }
                }
                con.Close();
            }
            return View(obj);
        }
        [HttpPost]
        public ActionResult InsertGrade(GS247.Models.Teachers.Grade data)
        {
            using (SqlConnection con = new SqlConnection("data source=43.255.152.25;initial catalog=GS247;persist security info=True;user id=_system;password=Gl$rv247;MultipleActiveResultSets=True;"))
            {
                con.Open();
                DataSet ds = new DataSet();
                using (SqlCommand cmd = new SqlCommand("dbo.TeachersSettings", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    String str = "InsertGrade";
                    int val = 0;
                    if (data.Id > 0)
                    {
                        str = "UpdateGrade";
                        val = data.Id;
                    }
                    cmd.Parameters.AddWithValue("@status", str);
                    cmd.Parameters.AddWithValue("@Id", val);
                    cmd.Parameters.AddWithValue("@Name", data.Name);
                    cmd.Parameters.AddWithValue("@Code", data.Priority);
                    cmd.Parameters.AddWithValue("@AdditionalId", 0);
                    cmd.Parameters.AddWithValue("@AddText", data.Status);
                    cmd.Parameters.AddWithValue("@MaxDay", Convert.ToInt32(data.MaxHoursDay));
                    cmd.Parameters.AddWithValue("@MaxWeek", Convert.ToInt32(data.MaxHoursWeek));
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);

                }
                con.Close();
            }

            return RedirectToAction("ManageGrade", "Teachers");
        }


        public ActionResult DeleteGrade(int Id)
        {
            using (SqlConnection con = new SqlConnection("data source=43.255.152.25;initial catalog=GS247;persist security info=True;user id=_system;password=Gl$rv247;MultipleActiveResultSets=True;"))
            {
                con.Open();
                DataSet ds = new DataSet();
                using (SqlCommand cmd = new SqlCommand("dbo.TeachersSettings", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@status", "DeleteGrade");
                    cmd.Parameters.AddWithValue("@Id", Id);
                    cmd.Parameters.AddWithValue("@Name", "");
                    cmd.Parameters.AddWithValue("@Code", "");
                    cmd.Parameters.AddWithValue("@AdditionalId", 0);
                    cmd.Parameters.AddWithValue("@AddText", "");
                    cmd.Parameters.AddWithValue("@MaxDay", 0);
                    cmd.Parameters.AddWithValue("@MaxWeek", 0);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);

                }
                con.Close();
            }

            return RedirectToAction("ManageGrade", "Teachers");
        }
        public ActionResult AddGrade()
        {
            Grade obj = new Grade();
            obj.Priority = Priority.Select;
            obj.Status = Status.Select;
            return View(obj);
        }
        public ActionResult UpdateGrade(int Id)
        {
            Grade obj = new Grade();
            using (SqlConnection con = new SqlConnection("data source=43.255.152.25;initial catalog=GS247;persist security info=True;user id=_system;password=Gl$rv247;MultipleActiveResultSets=True;"))
            {
                con.Open();
                DataSet ds = new DataSet();
                using (SqlCommand cmd = new SqlCommand("dbo.TeachersSettings", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@status", "GetGrade");
                    cmd.Parameters.AddWithValue("@Id", Id);
                    cmd.Parameters.AddWithValue("@Name", "");
                    cmd.Parameters.AddWithValue("@Code", "");
                    cmd.Parameters.AddWithValue("@AdditionalId", 0);
                    cmd.Parameters.AddWithValue("@AddText", "");
                    cmd.Parameters.AddWithValue("@MaxDay", 0);
                    cmd.Parameters.AddWithValue("@MaxWeek", 0);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);
                    if (ds.Tables.Count > 0)
                    {
                        obj.Id = Id;
                        obj.Name = ds.Tables[0].Rows[0].ItemArray[1].ToString();
                        int Prio = Convert.ToInt32(ds.Tables[0].Rows[0].ItemArray[2]);
                        if (Prio == 1) { obj.Priority = Priority.Low; }
                        if (Prio == 2) { obj.Priority = Priority.Medium; }
                        if (Prio == 3) { obj.Priority = Priority.High; }
                        Prio = Convert.ToInt32(ds.Tables[0].Rows[0].ItemArray[3]);
                        if (Prio == 1) { obj.Status = Status.Active; }
                        if (Prio == 2) { obj.Status = Status.Inactive; }
                        obj.MaxHoursDay = Convert.ToInt32(ds.Tables[0].Rows[0].ItemArray[4]);
                        obj.MaxHoursWeek = Convert.ToInt32(ds.Tables[0].Rows[0].ItemArray[5]);
                    }

                }
                con.Close();
            }
            return View(obj);
        }
        //Position Views
        public ActionResult ManagePosition()
        {
            PositionList obj = new PositionList();
            obj.Positions = new List<Position>();
            obj.Categories = new List<SelectListItem>();
            using (SqlConnection con = new SqlConnection("data source=43.255.152.25;initial catalog=GS247;persist security info=True;user id=_system;password=Gl$rv247;MultipleActiveResultSets=True;"))
            {
                con.Open();
                DataSet ds = new DataSet();
                using (SqlCommand cmd = new SqlCommand("dbo.TeachersSettings", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@status", "ViewPosition");
                    cmd.Parameters.AddWithValue("@Id", 0);
                    cmd.Parameters.AddWithValue("@Name", "");
                    cmd.Parameters.AddWithValue("@Code", "");
                    cmd.Parameters.AddWithValue("@AdditionalId", 0);
                    cmd.Parameters.AddWithValue("@AddText", "");
                    cmd.Parameters.AddWithValue("@MaxDay", 0);
                    cmd.Parameters.AddWithValue("@MaxWeek", 0);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);
                    if (ds.Tables.Count > 0)
                    {
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            Position subobj = new Position();
                            subobj.Id = Convert.ToInt32(ds.Tables[0].Rows[i].ItemArray[0]);
                            subobj.Name = ds.Tables[0].Rows[i].ItemArray[1].ToString();
                            subobj.Category = ds.Tables[0].Rows[i].ItemArray[2].ToString();
                            obj.Positions.Add(subobj);
                        }
                    }
                }
                ds = new DataSet();
                using (SqlCommand cmd = new SqlCommand("dbo.TeachersSettings", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@status", "ViewCategory");
                    cmd.Parameters.AddWithValue("@Id", 0);
                    cmd.Parameters.AddWithValue("@Name", "");
                    cmd.Parameters.AddWithValue("@Code", "");
                    cmd.Parameters.AddWithValue("@AdditionalId", 0);
                    cmd.Parameters.AddWithValue("@AddText", "");
                    cmd.Parameters.AddWithValue("@MaxDay", 0);
                    cmd.Parameters.AddWithValue("@MaxWeek", 0);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);
                    if (ds.Tables.Count > 0)
                    {
                        string value;
                        string text;
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            value = ds.Tables[0].Rows[i].ItemArray[0].ToString();
                            text = ds.Tables[0].Rows[i].ItemArray[1].ToString();
                            obj.Categories.Add(new SelectListItem() { Text = text, Value = value });
                        }
                    }
                }

                con.Close();
            }
            return View(obj);
        }
        [HttpPost]
        public ActionResult InsertPosition(GS247.Models.Teachers.MPosition data)
        {
            using (SqlConnection con = new SqlConnection("data source=43.255.152.25;initial catalog=GS247;persist security info=True;user id=_system;password=Gl$rv247;MultipleActiveResultSets=True;"))
            {
                con.Open();
                DataSet ds = new DataSet();
                using (SqlCommand cmd = new SqlCommand("dbo.TeachersSettings", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    String str = "InsertPosition";
                    int val = 0;
                    if (data.Id > 0)
                    {
                        str = "UpdatePosition";
                        val = data.Id;
                    }
                    cmd.Parameters.AddWithValue("@status", str);
                    cmd.Parameters.AddWithValue("@Id", val);
                    cmd.Parameters.AddWithValue("@Name", data.Name);
                    cmd.Parameters.AddWithValue("@Code", "");
                    cmd.Parameters.AddWithValue("@AdditionalId", data.SelectedCategory);
                    cmd.Parameters.AddWithValue("@AddText", "");
                    cmd.Parameters.AddWithValue("@MaxDay", 0);
                    cmd.Parameters.AddWithValue("@MaxWeek", 0);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);

                }
                con.Close();
            }

            return RedirectToAction("ManagePosition", "Teachers");
        }


        public ActionResult DeletePosition(int Id)
        {
            using (SqlConnection con = new SqlConnection("data source=43.255.152.25;initial catalog=GS247;persist security info=True;user id=_system;password=Gl$rv247;MultipleActiveResultSets=True;"))
            {
                con.Open();
                DataSet ds = new DataSet();
                using (SqlCommand cmd = new SqlCommand("dbo.TeachersSettings", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@status", "DeletePosition");
                    cmd.Parameters.AddWithValue("@Id", Id);
                    cmd.Parameters.AddWithValue("@Name", "");
                    cmd.Parameters.AddWithValue("@Code", "");
                    cmd.Parameters.AddWithValue("@AdditionalId", 0);
                    cmd.Parameters.AddWithValue("@AddText", "");
                    cmd.Parameters.AddWithValue("@MaxDay", 0);
                    cmd.Parameters.AddWithValue("@MaxWeek", 0);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);

                }
                con.Close();
            }

            return RedirectToAction("ManagePosition", "Teachers");
        }
        public ActionResult AddPosition()
        {
            MPosition obj = new MPosition();
            obj.Categories = new List<SelectListItem>();
            using (SqlConnection con = new SqlConnection("data source=43.255.152.25;initial catalog=GS247;persist security info=True;user id=_system;password=Gl$rv247;MultipleActiveResultSets=True;"))
            {
                con.Open();
                DataSet ds = new DataSet();
                using (SqlCommand cmd = new SqlCommand("dbo.TeachersSettings", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@status", "ViewCategory");
                    cmd.Parameters.AddWithValue("@Id", 0);
                    cmd.Parameters.AddWithValue("@Name", "");
                    cmd.Parameters.AddWithValue("@Code", "");
                    cmd.Parameters.AddWithValue("@AdditionalId", 0);
                    cmd.Parameters.AddWithValue("@AddText", "");
                    cmd.Parameters.AddWithValue("@MaxDay", 0);
                    cmd.Parameters.AddWithValue("@MaxWeek", 0);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);
                    if (ds.Tables.Count > 0)
                    {
                        string value;
                        string text;
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            value = ds.Tables[0].Rows[i].ItemArray[0].ToString();
                            text = ds.Tables[0].Rows[i].ItemArray[1].ToString();
                            obj.Categories.Add(new SelectListItem() { Text = text, Value = value });
                        }
                    }
                }

                con.Close();
            }
            return View(obj);
        }
        public ActionResult UpdatePosition(int Id)
        {
            MPosition obj = new MPosition();
            obj.Categories = new List<SelectListItem>();
            using (SqlConnection con = new SqlConnection("data source=43.255.152.25;initial catalog=GS247;persist security info=True;user id=_system;password=Gl$rv247;MultipleActiveResultSets=True;"))
            {
                con.Open();
                DataSet ds = new DataSet();
                using (SqlCommand cmd = new SqlCommand("dbo.TeachersSettings", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@status", "GetPosition");
                    cmd.Parameters.AddWithValue("@Id", Id);
                    cmd.Parameters.AddWithValue("@Name", "");
                    cmd.Parameters.AddWithValue("@Code", "");
                    cmd.Parameters.AddWithValue("@AdditionalId", 0);
                    cmd.Parameters.AddWithValue("@AddText", "");
                    cmd.Parameters.AddWithValue("@MaxDay", 0);
                    cmd.Parameters.AddWithValue("@MaxWeek", 0);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);
                    if (ds.Tables.Count > 0)
                    {
                        obj.Id = Id;
                        obj.Name = ds.Tables[0].Rows[0].ItemArray[1].ToString();
                        obj.SelectedCategory = ds.Tables[0].Rows[0].ItemArray[2].ToString();
                    }

                }
                ds = new DataSet();
                using (SqlCommand cmd = new SqlCommand("dbo.TeachersSettings", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@status", "ViewCategory");
                    cmd.Parameters.AddWithValue("@Id", 0);
                    cmd.Parameters.AddWithValue("@Name", "");
                    cmd.Parameters.AddWithValue("@Code", "");
                    cmd.Parameters.AddWithValue("@AdditionalId", 0);
                    cmd.Parameters.AddWithValue("@AddText", "");
                    cmd.Parameters.AddWithValue("@MaxDay", 0);
                    cmd.Parameters.AddWithValue("@MaxWeek", 0);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);
                    if (ds.Tables.Count > 0)
                    {
                        string value;
                        string text;
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            value = ds.Tables[0].Rows[i].ItemArray[0].ToString();
                            text = ds.Tables[0].Rows[i].ItemArray[1].ToString();
                            obj.Categories.Add(new SelectListItem() { Text = text, Value = value });
                        }
                    }
                }

                con.Close();
            }
            return View(obj);
        }
        public ActionResult AddStaff()
        {
            Connectivity Connect = new Connectivity();
            ViewBag.CountryList = Connect.CountryList();
            StaffDetails obj = new StaffDetails();
            con.Open();

            int StaffNumber = 1;
            SqlDataReader Execreader = Connect.QueryExecute(con, "Select funcValue from dbo.LockTable where funcName='StaffId'");
            if (Execreader.Read())
            {
                StaffNumber = Convert.ToInt32(Execreader.GetValue(0)) + 1;
            }

            List<ParamValues> param = new List<ParamValues>();
            ParamValues sub = new ParamValues();
            sub.Code = "@TeacherNumber";
            sub.Value = StaffNumber.ToString();
            param.Add(sub);
            Connect.QueryExecute(con, "UPDATE dbo.LockTable SET funcValue=@TeacherNumber WHERE funcName='StaffId'", param);
            obj.StaffNumber = StaffNumber.ToString();
            ViewBag.RoleList = Connect.TableData(con, "select Id, Name from dbo.SettingsUserRole", "Id", "Name");
            ViewBag.Departments = Connect.TableData(con, "select Id,Name from dbo.TeachersManageDepartments", "Id", "Name");
            ViewBag.Categories = Connect.TableData(con, "select Id,Name from dbo.TeachersManageCategories", "Id", "Name");
            ViewBag.Grades = Connect.TableData(con, "select Id,Name from dbo.TeachersManageGrades", "Id", "Name");
            ViewBag.Positions = Connect.TableData(con, "select Id,Name from dbo.TeachersManagePositions", "Id", "Name");
            List<SelectListItem> list = new List<SelectListItem>();
            list.Add(new SelectListItem() { Text = "Male", Value = "1" });
            list.Add(new SelectListItem() { Text = "Female", Value = "2" });
            ViewBag.Genders = new SelectList(list, "Value", "Text");
            list = new List<SelectListItem>();
            list.Add(new SelectListItem() { Text = "Single", Value = "1" });
            list.Add(new SelectListItem() { Text = "Married", Value = "2" });
            list.Add(new SelectListItem() { Text = "Divorced", Value = "3" });
            ViewBag.MaritalStatus = new SelectList(list, "Value", "Text");
            list = new List<SelectListItem>();
            list.Add(new SelectListItem() { Text = "APositive", Value = "1" });
            list.Add(new SelectListItem() { Text = "ANegative", Value = "2" });
            list.Add(new SelectListItem() { Text = "BPositive", Value = "3" });
            list.Add(new SelectListItem() { Text = "BNegative", Value = "4" });
            list.Add(new SelectListItem() { Text = "OPositive", Value = "5" });
            list.Add(new SelectListItem() { Text = "ONegative", Value = "6" });
            list.Add(new SelectListItem() { Text = "ABPositive", Value = "7" });
            list.Add(new SelectListItem() { Text = "ABNegative", Value = "8" });
            ViewBag.BloodGroup = new SelectList(list, "Value", "Text");
            list = new List<SelectListItem>();
            list.Add(new SelectListItem() { Text = "Indian", Value = "1" });
            list.Add(new SelectListItem() { Text = "Others", Value = "2" });
            ViewBag.Nationality = new SelectList(list, "Value", "Text");
            list = new List<SelectListItem>();
            list.Add(new SelectListItem() { Text = "Years", Value = "" });
            for (int i = 1; i <= 40; i++)
            {
                list.Add(new SelectListItem() { Text = i.ToString(), Value = i.ToString() });
            }
            ViewBag.TotalExpYearsList = new SelectList(list, "Value", "Text");

            list = new List<SelectListItem>();
            list.Add(new SelectListItem() { Text = "Month", Value = "" });
            for (int i = 1; i <= 11; i++)
            {
                list.Add(new SelectListItem() { Text = i.ToString(), Value = i.ToString() });
            }
            ViewBag.TotalExpMonthsList = new SelectList(list, "Value", "Text");
            con.Close();
            return View(obj);
        }


        [HttpPost]
        public ActionResult InsertStaff(StaffDetails obj)
        {
            Connectivity Connect = new Connectivity();
            con.Open();
            List<ParamValues> param = new List<ParamValues>();
            String tmpValue = "";
            param.Add(Connect.Params("@TeacherId", obj.StaffId.ToString()));
            param.Add(Connect.Params("@TeacherNumber", obj.StaffNumber.ToString()));
            param.Add(Connect.Params("@JoiningDate", obj.JoiningDate));
            param.Add(Connect.Params("@FirstName", obj.FirstName));
            tmpValue = "";
            if (obj.MiddleName != null) { tmpValue = obj.MiddleName; }
            param.Add(Connect.Params("@MiddleName", tmpValue));
            param.Add(Connect.Params("@LastName", obj.LastName));
            param.Add(Connect.Params("@Gender", obj.GenderId.ToString()));
            param.Add(Connect.Params("@DateOfBirth", obj.DateOfBirth));
            param.Add(Connect.Params("@DepartmentId", obj.DepartmentId.ToString()));
            tmpValue = "";
            if (obj.CategoryId > 0) { tmpValue = obj.CategoryId.ToString(); }
            param.Add(Connect.Params("@CategoryId", tmpValue));
            tmpValue = "";
            if (obj.PositionId > 0) { tmpValue = obj.PositionId.ToString(); }
            param.Add(Connect.Params("@PositionId", tmpValue));
            //param.Add(Connect.Params("@CategoryId", obj.CategoryId.ToString()));
            tmpValue = "";
            if (obj.GradeId > 0) { tmpValue = obj.GradeId.ToString(); }
            param.Add(Connect.Params("@GradeId", tmpValue));
            tmpValue = "";
            if (obj.JobTitle != null) { tmpValue = obj.JobTitle; }
            param.Add(Connect.Params("@JobTitle", tmpValue));
            param.Add(Connect.Params("@Qualification", obj.Qualification));
            param.Add(Connect.Params("@TotalExpYears", obj.TotalExpYears.ToString()));
            param.Add(Connect.Params("@TotalExpMonths", obj.TotalExpMonths.ToString()));
            tmpValue = "";
            if (obj.ExpDetails != null) { tmpValue = obj.ExpDetails; }
            param.Add(Connect.Params("@ExperienceDetails", tmpValue));
            param.Add(Connect.Params("@MaritalStatus", obj.isMarried.ToString()));
            tmpValue = "";
            if (obj.ChildrenCount > 0) { tmpValue = obj.ChildrenCount.ToString(); }
            param.Add(Connect.Params("@ChildrenCount", tmpValue));
            tmpValue = "";
            if (obj.FatherName != null) { tmpValue = obj.FatherName; }
            param.Add(Connect.Params("@FatherName", tmpValue));
            tmpValue = "";
            if (obj.MotherName != null) { tmpValue = obj.MotherName; }
            param.Add(Connect.Params("@MotherName", tmpValue));
            tmpValue = "";
            if (obj.SpouseName != null) { tmpValue = obj.SpouseName; }
            param.Add(Connect.Params("@SpouseName", tmpValue));
            tmpValue = "";
            if (obj.BloodGroupId >= 0) { tmpValue = obj.BloodGroupId.ToString(); }
            param.Add(Connect.Params("@BloodGroup", tmpValue));
            tmpValue = "";
            if (obj.Nationality == Nationality.Indian) { tmpValue = "0"; }
            else if (obj.Nationality == Nationality.Others) { tmpValue = "1"; }
            param.Add(Connect.Params("@Nationality", tmpValue));
            param.Add(Connect.Params("@Email", obj.Email));
            param.Add(Connect.Params("@PhotoPath", ""));
            param.Add(Connect.Params("@HomeAddressLine1", obj.HomeAddressLine1));
            tmpValue = "";
            if (obj.HomeAddressLine2 != null) { tmpValue = obj.HomeAddressLine2; }
            param.Add(Connect.Params("@HomeAddressLine2", tmpValue));
            param.Add(Connect.Params("@HomeCity", obj.HomeCity));
            param.Add(Connect.Params("@HomeState", obj.HomeState));
            param.Add(Connect.Params("@HomeCountry", obj.HomeCountry));
            param.Add(Connect.Params("@HomePinCode", obj.HomePincode));
            tmpValue = "";
            if (obj.OfficeAddressLine1 != null) { tmpValue = obj.OfficeAddressLine1; }
            param.Add(Connect.Params("@OfficeAddressLine1", tmpValue));
            tmpValue = "";
            if (obj.OfficeAddressLine2 != null) { tmpValue = obj.OfficeAddressLine2; }
            param.Add(Connect.Params("@OfficeAddressLine2", tmpValue));
            tmpValue = "";
            if (obj.OfficeCity != null) { tmpValue = obj.OfficeCity; }
            param.Add(Connect.Params("@OfficeCity", tmpValue));
            tmpValue = "";
            if (obj.OfficeState != null) { tmpValue = obj.OfficeState; }
            param.Add(Connect.Params("@OfficeState", tmpValue));
            tmpValue = "";
            if (obj.OfficeCountry != null) { tmpValue = obj.OfficeCountry; }
            param.Add(Connect.Params("@OfficeCountry", tmpValue));
            tmpValue = "";
            if (obj.OfficePincode != null) { tmpValue = obj.OfficePincode; }
            param.Add(Connect.Params("@OfficePinCode", tmpValue));
            tmpValue = "";
            if (obj.OfficePhone1 != null) { tmpValue = obj.OfficePhone1; }
            param.Add(Connect.Params("@OficePhone1", tmpValue));
            tmpValue = "";
            if (obj.OfficePhone2 != null) { tmpValue = obj.OfficePhone2; }
            param.Add(Connect.Params("@OfficePhone2", tmpValue));
            param.Add(Connect.Params("@MobileNumber", obj.MobileNumber));
            tmpValue = "";
            if (obj.HomePhone != null) { tmpValue = obj.HomePhone; }
            param.Add(Connect.Params("@HomePhone", tmpValue));
            tmpValue = "";
            if (obj.Fax != null) { tmpValue = obj.Fax; }
            param.Add(Connect.Params("@Fax", tmpValue));
            param.Add(Connect.Params("@BasicPay", obj.BasicPay.ToString()));
            tmpValue = "";
            if (obj.EPF != null) { tmpValue = obj.EPF.ToString(); }
            param.Add(Connect.Params("@EPF", tmpValue));
            tmpValue = "";
            if (obj.ESI != null) { tmpValue = obj.ESI.ToString(); }
            param.Add(Connect.Params("@ESI", tmpValue));
            param.Add(Connect.Params("@BankName", obj.BankName));
            param.Add(Connect.Params("@BankAccountNo", obj.BankAccountNumber));
            param.Add(Connect.Params("@IFSCCode", obj.IfscCode));
            tmpValue = "";
            if (obj.PassportNumber != null) { tmpValue = obj.PassportNumber; }
            param.Add(Connect.Params("@PassportNo", tmpValue));
            tmpValue = "";
            if (obj.PassportExpiry != null) { tmpValue = obj.PassportExpiry; }
            param.Add(Connect.Params("@PassportExpiryDate", tmpValue));
            param.Add(Connect.Params("@PANCardNo", obj.PANCardNo));
            param.Add(Connect.Params("@AadhaarNo", obj.AadhaarNo));
            param.Add(Connect.Params("@StaffRoleId", obj.StaffRoleId.ToString()));
            if (obj.StaffId > 0)
            {

                Connect.QueryExecute(con, "UPDATE dbo.StaffDetails SET TeacherNumber=@TeacherNumber,JoiningDate=@JoiningDate,FirstName=@FirstName,MiddleName=@MiddleName,LastName=@LastName,Gender=@Gender,DateOfBirth=@DateOfBirth,DepartmentId=@DepartmentId,CategoryId=@CategoryId,PositionId=@PositionId,GradeId=@GradeId,JobTitle=@JobTitle,Qualification=@Qualification,TotalExpYears=@TotalExpYears,TotalExpMonths=@TotalExpMonths,ExperienceDetails=@ExperienceDetails,MaritalStatus=@MaritalStatus,ChildrenCount=@ChildrenCount,FatherName=@FatherName,MotherName=@MotherName,SpouseName=@SpouseName,BloodGroup=@BloodGroup,Nationality=@Nationality,Email=@Email,PhotoPath=@PhotoPath,HomeAddressLine1=@HomeAddressLine1,HomeAddressLine2=@HomeAddressLine2,HomeCity=@HomeCity,HomeState=@HomeState,HomeCountry=@HomeCountry,HomePinCode=@HomePinCode,OfficeAddressLine1=@OfficeAddressLine1,OfficeAddressLine2=@OfficeAddressLine2,OfficeCity=@OfficeCity,OfficeState=@OfficeState,OfficeCountry=@OfficeCountry,OfficePinCode=@OfficePinCode,OficePhone1=@OficePhone1,OfficePhone2=@OfficePhone2,MobileNumber=@MobileNumber,HomePhone=@HomePhone,Fax=@Fax,StaffRoleId=@StaffRoleId WHERE TeacherId=@TeacherId", param);
                Connect.QueryExecute(con, "UPDATE dbo.StaffSalaryDetails SET BasicPay=@BasicPay,EPF=@EPF,ESI=@ESI,BankName=@BankName,BankAccountNo=@BankAccountNo,IFSCCode=@IFSCCode,PassportNo=@PassportNo,PassportExpiryDate=@PassportExpiryDate,PANCardNo=@PANCardNo,AadhaarNo=@AadhaarNo WHERE StaffId=@TeacherId", param);
            }
            else
            {
                Connect.QueryExecute(con, "INSERT INTO dbo.StaffDetails(TeacherNumber,JoiningDate,FirstName,MiddleName,LastName,Gender,DateOfBirth,DepartmentId,CategoryId,PositionId,GradeId,JobTitle,Qualification,TotalExpYears,TotalExpMonths,ExperienceDetails,MaritalStatus,ChildrenCount,FatherName,MotherName,SpouseName,BloodGroup,Nationality,Email,PhotoPath,HomeAddressLine1,HomeAddressLine2,HomeCity,HomeState,HomeCountry,HomePinCode,OfficeAddressLine1,OfficeAddressLine2,OfficeCity,OfficeState,OfficeCountry,OfficePinCode,OficePhone1,OfficePhone2,MobileNumber,HomePhone,Fax,StaffRoleId)  Values(@TeacherNumber,@JoiningDate,@FirstName,@MiddleName,@LastName,@Gender,@DateOfBirth,@DepartmentId,@CategoryId,@PositionId,@GradeId,@JobTitle,@Qualification,@TotalExpYears,@TotalExpMonths,@ExperienceDetails,@MaritalStatus,@ChildrenCount,@FatherName,@MotherName,@SpouseName,@BloodGroup,@Nationality,@Email,@PhotoPath,@HomeAddressLine1,@HomeAddressLine2,@HomeCity,@HomeState,@HomeCountry,@HomePinCode,@OfficeAddressLine1,@OfficeAddressLine2,@OfficeCity,@OfficeState,@OfficeCountry,@OfficePinCode,@OficePhone1,@OfficePhone2,@MobileNumber,@HomePhone,@Fax,@StaffRoleId)", param);
                SqlDataReader ExecReader = Connect.QueryExecute(con, "SELECT TeacherId from dbo.StaffDetails where TeacherNumber=@TeacherNumber", param);
                if (ExecReader.Read())
                {
                    int StaffId = Convert.ToInt32(ExecReader.GetValue(0));
                    if (StaffId > 0)
                    {
                        param.Add(Connect.Params("@StaffId", StaffId.ToString()));
                        Connect.QueryExecute(con, "INSERT INTO dbo.StaffSalaryDetails(StaffId,BasicPay,EPF,ESI,BankName,BankAccountNo,IFSCCode,PassportNo,PassportExpiryDate,PANCardNo,AadhaarNo) VALUES(@StaffId,@BasicPay,@EPF,@ESI,@BankName,@BankAccountNo,@IFSCCode,@PassportNo,@PassportExpiryDate,@PANCardNo,@AadhaarNo)", param);
                    }

                }
            }
            con.Close();
            CreateUser(obj);
            return RedirectToAction("AddStaff", "Teachers");

        }
        public void CreateUser(StaffDetails obj)
        {
            Connectivity Connect = new Connectivity();
            con.Open();
            List<ParamValues> param = new List<ParamValues>();
            param.Add(Connect.Params("@UserName", obj.StaffNumber.ToString()));
            param.Add(Connect.Params("@Password", RandomGen(6)));
            param.Add(Connect.Params("@Email", obj.Email));
            param.Add(Connect.Params("@MobileNumber", obj.MobileNumber));
            param.Add(Connect.Params("@SuperUser", obj.StaffRoleId.ToString()));
            param.Add(Connect.Params("@Status", "1"));
            param.Add(Connect.Params("@FirstName", obj.FirstName));
            param.Add(Connect.Params("@LastName", obj.LastName));
            if (obj.StaffId > 0)
            {
                Connect.QueryExecute(con, "UPDATE dbo.Users Set SuperUser=@SuperUser,Email=@Email,MobileNumber=@MobileNumber,FirstName=@FirstName,LastName=@LastName WHERE UserName=@UserName", param);
            }
            else
            {
                Connect.QueryExecute(con, "INSERT INTO dbo.Users (UserName,Password,Email,MobileNumber,SuperUser,Status,FirstName,LastName) VALUES(@UserName,@Password,@Email,@MobileNumber,@SuperUser,@Status,@FirstName,@LastName)", param);
            }
            con.Close();
        }
       public void commonCreateUser(String UserName,String Email,int RoleId,String MobileNumber,String FirstName,String LastName,int Status)
        {
            String pwd = RandomGen(6);
            SqlDataReader Exec=Connect.QueryExecute(con, "select count(id) from dbo.Users WHERE UserName=" + UserName, param);
            if ((Exec.Read())&&(Convert.ToInt32(Exec.GetValue(0))>0))
            {
                Connect.QueryExecute(con, "UPDATE dbo.Users Set SuperUser=" + RoleId + " , Email='" + Email + "' ,MobileNumber='" + MobileNumber + "' ,FirstName='" + FirstName + "' ,LastName='" + LastName + "',Status="+Status+" WHERE UserName='" + UserName+"'");
            }
            else
            {
                Connect.QueryExecute(con, "INSERT INTO dbo.Users (UserName,Password,Email,MobileNumber,SuperUser,Status,FirstName,LastName) VALUES('"+UserName+"','"+ pwd+"','"+Email+"','"+MobileNumber+"',"+RoleId+","+Status+",'"+FirstName+"','"+LastName+"')");
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
        public ActionResult EditStaff(int Id)
        {
            Connectivity Connect = new Connectivity();
            ViewBag.CountryList = Connect.CountryList();
            StaffDetails obj = new StaffDetails();
            con.Open();

            List<ParamValues> param = new List<ParamValues>();
            ParamValues sub = new ParamValues();
            sub.Code = "@Id";
            sub.Value = Id.ToString();
            param.Add(sub);
            obj.IsChanged = 0;
            SqlDataReader Execreader = Connect.QueryExecute(con, "Select * from dbo.StaffDetails where TeacherId=@Id", param);
            if (Execreader.Read())
            {
                obj.StaffId = Convert.ToInt32(Execreader.GetValue(0));
                obj.StaffNumber = Execreader.GetValue(1).ToString();
                obj.JoiningDate = Execreader.GetValue(2).ToString();
                obj.FirstName = Execreader.GetValue(3).ToString();
                obj.MiddleName = Execreader.GetValue(4).ToString();
                obj.LastName = Execreader.GetValue(5).ToString();
                obj.GenderId = Convert.ToInt32(Execreader.GetValue(6));
                obj.DateOfBirth = Execreader.GetValue(7).ToString();
                obj.DepartmentId = Convert.ToInt32(Execreader.GetValue(8));
                obj.CategoryId = Convert.ToInt32(Execreader.GetValue(9));
                obj.PositionId = Convert.ToInt32(Execreader.GetValue(10));
                obj.GradeId = Convert.ToInt32(Execreader.GetValue(11));
                obj.JobTitle = Execreader.GetValue(12).ToString();
                obj.Qualification = Execreader.GetValue(13).ToString();
                obj.TotalExpYears = Convert.ToInt32(Execreader.GetValue(14));
                obj.TotalExpMonths = Convert.ToInt32(Execreader.GetValue(15));
                obj.ExpDetails = Execreader.GetValue(16).ToString();
                obj.isMarried = Convert.ToInt32(Execreader.GetValue(17));
                obj.ChildrenCount = Convert.ToInt32(Execreader.GetValue(18));
                obj.FatherName = Execreader.GetValue(19).ToString();
                obj.MotherName = Execreader.GetValue(20).ToString();
                obj.SpouseName = Execreader.GetValue(21).ToString();
                obj.BloodGroupId = Convert.ToInt32(Execreader.GetValue(22));
                int tmpValue = Convert.ToInt32(Execreader.GetValue(23));
                if (tmpValue == 0)
                {
                    obj.Nationality = Nationality.Indian;
                }
                else if (tmpValue == 1)
                {
                    obj.Nationality = Nationality.Others;
                }
                obj.Email = Execreader.GetValue(24).ToString();
                obj.HomeAddressLine1 = Execreader.GetValue(26).ToString();
                obj.HomeAddressLine2 = Execreader.GetValue(27).ToString();
                obj.HomeCity = Execreader.GetValue(28).ToString();
                obj.HomeState = Execreader.GetValue(29).ToString();
                obj.HomeCountry = Execreader.GetValue(30).ToString();
                obj.HomePincode = Execreader.GetValue(31).ToString();
                obj.OfficeAddressLine1 = Execreader.GetValue(32).ToString();
                obj.OfficeAddressLine2 = Execreader.GetValue(33).ToString();
                obj.OfficeCity = Execreader.GetValue(34).ToString();
                obj.OfficeState = Execreader.GetValue(35).ToString();
                obj.OfficeCountry = Execreader.GetValue(36).ToString();
                obj.OfficePincode = Execreader.GetValue(37).ToString();
                obj.OfficePhone1 = Execreader.GetValue(38).ToString();
                obj.OfficePhone2 = Execreader.GetValue(39).ToString();
                obj.MobileNumber = Execreader.GetValue(40).ToString();
                obj.HomePhone = Execreader.GetValue(41).ToString();
                obj.Fax = Execreader.GetValue(42).ToString();
                obj.StaffRoleId = Convert.ToInt32(Execreader.GetValue(43));

            }
            Execreader = Connect.QueryExecute(con, "Select * from dbo.StaffSalaryDetails where StaffId=@Id", param);
            if (Execreader.Read())
            {
                obj.BasicPay = Execreader.GetValue(2).ToString();
                obj.EPF = Execreader.GetValue(3).ToString();
                obj.ESI = Execreader.GetValue(4).ToString();
                obj.BankName = Execreader.GetValue(6).ToString();
                obj.BankAccountNumber = Execreader.GetValue(7).ToString();
                obj.IfscCode = Execreader.GetValue(8).ToString();
                obj.PassportNumber = Execreader.GetValue(9).ToString();
                obj.PassportExpiry = Execreader.GetValue(10).ToString();
                obj.PANCardNo = Execreader.GetValue(11).ToString();
                obj.AadhaarNo = Execreader.GetValue(12).ToString();
            }

            ViewBag.RoleList = Connect.TableData(con, "select Id, Name from dbo.SettingsUserRole", "Id", "Name");
            ViewBag.Departments = Connect.TableData(con, "select Id,Name from dbo.TeachersManageDepartments", "Id", "Name");
            ViewBag.Categories = Connect.TableData(con, "select Id,Name from dbo.TeachersManageCategories", "Id", "Name");
            ViewBag.Grades = Connect.TableData(con, "select Id,Name from dbo.TeachersManageGrades", "Id", "Name");
            ViewBag.Positions = Connect.TableData(con, "select Id,Name from dbo.TeachersManagePositions", "Id", "Name");
            List<SelectListItem> list = new List<SelectListItem>();
            list.Add(new SelectListItem() { Text = "Years", Value = "" });
            for (int i = 1; i <= 40; i++)
            {
                list.Add(new SelectListItem() { Text = i.ToString(), Value = i.ToString() });
            }
            ViewBag.TotalExpYearsList = new SelectList(list, "Value", "Text");

            list = new List<SelectListItem>();
            list.Add(new SelectListItem() { Text = "Month", Value = "" });
            for (int i = 1; i <= 11; i++)
            {
                list.Add(new SelectListItem() { Text = i.ToString(), Value = i.ToString() });
            }
            ViewBag.TotalExpMonthsList = new SelectList(list, "Value", "Text");
            con.Close();
            return View(obj);
        }
        public ActionResult ViewStaff(int Id)
        {
            Connectivity Connect = new Connectivity();
            ViewStaffDetails obj = new ViewStaffDetails();
            con.Open();

            List<ParamValues> param = new List<ParamValues>();
            ParamValues sub = new ParamValues();
            sub.Code = "@Id";
            sub.Value = Id.ToString();
            param.Add(sub);
            obj.IsChanged = 0;
            SqlDataReader Execreader = Connect.QueryExecute(con, "Select * from dbo.StaffDetails where TeacherId=@Id", param);
            if (Execreader.Read())
            {
                obj.StaffId = Convert.ToInt32(Execreader.GetValue(0));
                obj.StaffNumber = Execreader.GetValue(1).ToString();
                obj.JoiningDate = Execreader.GetValue(2).ToString();
                obj.FirstName = Execreader.GetValue(3).ToString();
                obj.MiddleName = Execreader.GetValue(4).ToString();
                obj.LastName = Execreader.GetValue(5).ToString();
                int tmpId = Convert.ToInt32(Execreader.GetValue(6));
                if(tmpId== 0)
                {
                    obj.GenderName = Genders.Male.ToString();
                }
                if (tmpId == 1)
                {
                    obj.GenderName = Genders.Female.ToString();
                }
                obj.DateOfBirth = Execreader.GetValue(7).ToString();
                tmpId= Convert.ToInt32(Execreader.GetValue(8));
                sub = new ParamValues();
                sub.Code = "@DepartmentId";
                sub.Value = tmpId.ToString();
                param.Add(sub);
                obj.DepatmentName = "";
                SqlDataReader Execreader1 = null;
                Execreader1 = Connect.QueryExecute(con, "select Name from dbo.TeachersManageDepartments where Id=@DepartmentId", param);
                if(Execreader1.Read())
                {
                    obj.DepatmentName = Execreader1.GetValue(0).ToString();
                }
                tmpId = Convert.ToInt32(Execreader.GetValue(9));
                sub = new ParamValues();
                sub.Code = "@CategoryId";
                sub.Value = tmpId.ToString();
                param.Add(sub);
                obj.CategoryName = "";
                Execreader1 = Connect.QueryExecute(con, "select Name from dbo.TeachersManageCategories where Id=@CategoryId", param);
                if (Execreader1.Read())
                {
                    obj.CategoryName = Execreader1.GetValue(0).ToString();
                }
                tmpId = Convert.ToInt32(Execreader.GetValue(10));
                sub = new ParamValues();
                sub.Code = "@PositionId";
                sub.Value = tmpId.ToString();
                param.Add(sub);
                obj.PositionName = "";
                Execreader1 = Connect.QueryExecute(con, "select Name from dbo.TeachersManagePositions where Id=@PositionId", param);
                if (Execreader1.Read())
                {
                    obj.PositionName = Execreader1.GetValue(0).ToString();
                }
                tmpId = Convert.ToInt32(Execreader.GetValue(11));
                sub = new ParamValues();
                sub.Code = "@GradeId";
                sub.Value = tmpId.ToString();
                param.Add(sub);
                obj.GradeName = "";
                Execreader1 = Connect.QueryExecute(con, "select Name from dbo.TeachersManageGrades where Id=@GradeId", param);
                if (Execreader1.Read())
                {
                    obj.GradeName = Execreader1.GetValue(0).ToString();
                }
                obj.JobTitle = Execreader.GetValue(12).ToString();
                obj.Qualification = Execreader.GetValue(13).ToString();
                obj.TotalExpYears = Convert.ToInt32(Execreader.GetValue(14));
                obj.TotalExpMonths = Convert.ToInt32(Execreader.GetValue(15));
                obj.ExpDetails = Execreader.GetValue(16).ToString();
                tmpId= Convert.ToInt32(Execreader.GetValue(17));
                if(tmpId==0)
                {
                    obj.isMarried = MaritalStatus.Single.ToString();
                }
                else if(tmpId==1)
                {
                    obj.isMarried = MaritalStatus.Married.ToString();
                }
                else if (tmpId == 2)
                {
                    obj.isMarried = MaritalStatus.Divorced.ToString();
                }
                obj.ChildrenCount = Convert.ToInt32(Execreader.GetValue(18));
                obj.FatherName = Execreader.GetValue(19).ToString();
                obj.MotherName = Execreader.GetValue(20).ToString();
                obj.SpouseName = Execreader.GetValue(21).ToString();
                tmpId= Convert.ToInt32(Execreader.GetValue(22));
                if(tmpId==0)
                {
                    obj.BloodGroupName = BloodGroup.APositive.ToString();
                }
                else if (tmpId == 1)
                {
                    obj.BloodGroupName = BloodGroup.ANegative.ToString();
                }
                else if (tmpId == 2)
                {
                    obj.BloodGroupName = BloodGroup.BPositive.ToString();
                }
                else if (tmpId == 3)
                {
                    obj.BloodGroupName = BloodGroup.BNegative.ToString();
                }
                else if (tmpId == 4)
                {
                    obj.BloodGroupName = BloodGroup.OPositive.ToString();
                }
                else if (tmpId == 5)
                {
                    obj.BloodGroupName = BloodGroup.ONegative.ToString();
                }
                else if (tmpId == 6)
                {
                    obj.BloodGroupName = BloodGroup.ABPositive.ToString();
                }
                else if (tmpId == 7)
                {
                    obj.BloodGroupName = BloodGroup.ABNegative.ToString();
                }
                tmpId = Convert.ToInt32(Execreader.GetValue(23));
                if (tmpId == 0)
                {
                    obj.Nationality = Nationality.Indian.ToString();
                }
                else if (tmpId == 1)
                {
                    obj.Nationality = Nationality.Others.ToString();
                }
                obj.Email = Execreader.GetValue(24).ToString();
                obj.HomeAddressLine1 = Execreader.GetValue(26).ToString();
                obj.HomeAddressLine2 = Execreader.GetValue(27).ToString();
                obj.HomeCity = Execreader.GetValue(28).ToString();
                obj.HomeState = Execreader.GetValue(29).ToString();
                obj.HomeCountry = Execreader.GetValue(30).ToString();
                obj.HomePincode = Execreader.GetValue(31).ToString();
                obj.OfficeAddressLine1 = Execreader.GetValue(32).ToString();
                obj.OfficeAddressLine2 = Execreader.GetValue(33).ToString();
                obj.OfficeCity = Execreader.GetValue(34).ToString();
                obj.OfficeState = Execreader.GetValue(35).ToString();
                obj.OfficeCountry = Execreader.GetValue(36).ToString();
                obj.OfficePincode = Execreader.GetValue(37).ToString();
                obj.OfficePhone1 = Execreader.GetValue(38).ToString();
                obj.OfficePhone2 = Execreader.GetValue(39).ToString();
                obj.MobileNumber = Execreader.GetValue(40).ToString();
                obj.HomePhone = Execreader.GetValue(41).ToString();
                obj.Fax = Execreader.GetValue(42).ToString();
                tmpId = Convert.ToInt32(Execreader.GetValue(43));
                sub = new ParamValues();
                sub.Code = "@RoleId";
                sub.Value = tmpId.ToString();
                param.Add(sub);
                obj.StaffRoleName = "";
                Execreader1 = Connect.QueryExecute(con, "select Name from dbo.SettingsUserRole where Id=@RoleId", param);
                if (Execreader1.Read())
                {
                    obj.StaffRoleName = Execreader1.GetValue(0).ToString();
                }

            }
            Execreader = Connect.QueryExecute(con, "Select * from dbo.StaffSalaryDetails where StaffId=@Id", param);
            if (Execreader.Read())
            {
                obj.BasicPay = Execreader.GetValue(2).ToString();
                obj.EPF = Execreader.GetValue(3).ToString();
                obj.ESI = Execreader.GetValue(4).ToString();
                obj.BankName = Execreader.GetValue(6).ToString();
                obj.BankAccountNumber = Execreader.GetValue(7).ToString();
                obj.IfscCode = Execreader.GetValue(8).ToString();
                obj.PassportNumber = Execreader.GetValue(9).ToString();
                obj.PassportExpiry = Execreader.GetValue(10).ToString();
                obj.PANCardNo = Execreader.GetValue(11).ToString();
                obj.AadhaarNo = Execreader.GetValue(12).ToString();
            }
            con.Close();
            return View(obj);
        }

    /** Teachers Log Category **/
        public ActionResult ManageLogCategory()
        {
            Connectivity Connect = new Connectivity();
            LogCategories obj = new LogCategories();
            obj.Categories = new List<LogCategory>();
            con.Open();
            SqlDataReader Execreader = Connect.QueryExecute(con, "Select Id,CategoryName from dbo.TeachersLogCategory");
            while (Execreader.Read())
            {
                LogCategory subobj = new LogCategory();
                subobj.Id = Convert.ToInt32(Execreader.GetValue(0));
                subobj.Name = Execreader.GetValue(1).ToString();
                obj.Categories.Add(subobj);
            }
            con.Close();
            return View(obj);
        }
        public ActionResult AddLogCategory()
        {
            LogCategory obj = new LogCategory();
            return View(obj);
        }
        [HttpPost]
        public ActionResult InsertLogCategory(LogCategory obj)
        {
            Connectivity Connect = new Connectivity();
            List<ParamValues> param = new List<ParamValues>();
            ParamValues sub = new ParamValues();
            sub.Code = "@Id";
            sub.Value = obj.Id.ToString();
            param.Add(sub);
            sub = new ParamValues();
            sub.Code = "@Name";
            sub.Value = obj.Name.ToString();
            param.Add(sub);
            con.Open();
            if(obj.Id>0)
            {
                Connect.QueryExecute(con, "Update dbo.TeachersLogCategory Set CategoryName=@Name where Id=@Id", param);
            }
            else
            {
                Connect.QueryExecute(con, "Insert dbo.TeachersLogCategory(CategoryName) VALUES (@Name)", param);
            }
            con.Close();
            return RedirectToAction("ManageLogCategory", "Teachers");
        }
        public ActionResult EditLogCategory(int Id)
        {
            Connectivity Connect = new Connectivity();
            LogCategory obj = new LogCategory();
            List<ParamValues> param = new List<ParamValues>();
            ParamValues sub = new ParamValues();
            sub.Code = "@Id";
            sub.Value = Id.ToString();
            param.Add(sub);
            con.Open();
            SqlDataReader Execreader = Connect.QueryExecute(con, "Select Id,CategoryName from dbo.TeachersLogCategory where Id=@Id", param);
            if (Execreader.Read())
            {
                obj.Id = Convert.ToInt32(Execreader.GetValue(0));
                obj.Name = Execreader.GetValue(1).ToString();
            }
            con.Close();
            return View(obj);
        }
        public ActionResult DeleteLogCategory(int Id)
        {
            Connectivity Connect = new Connectivity();
            LogCategory obj = new LogCategory();
            List<ParamValues> param = new List<ParamValues>();
            ParamValues sub = new ParamValues();
            sub.Code = "@Id";
            sub.Value = Id.ToString();
            param.Add(sub);
            con.Open();
            Connect.QueryExecute(con, "Delete from dbo.TeachersLogCategory where Id=@Id", param);
            con.Close();
            return RedirectToAction("ManageLogCategory", "Teachers");
        }
        /** Teachers Log Category **/
        [HttpGet]
        public JsonResult GetAllAchievements(int Id)
        {
            List<Achievement> obj = new List<Achievement>();
            Connectivity Connect = new Connectivity();
            
            List<ParamValues> param = new List<ParamValues>();
            ParamValues sub = new ParamValues();
            sub.Code = "@Id";
            sub.Value = Id.ToString();
            param.Add(sub);
            con.Open();
            SqlDataReader Execreader = Connect.QueryExecute(con, "select Id,AchievementTitle,Description,DocumentName from dbo.TeacherAchievements where TeacherId=@Id", param);
            while (Execreader.Read())
            {
                Achievement subobj = new Achievement();
                subobj.Id= Convert.ToInt32(Execreader.GetValue(0));
                subobj.Title = Execreader.GetValue(1).ToString();
                subobj.Description = Execreader.GetValue(2).ToString();
                subobj.DocumentName = Execreader.GetValue(3).ToString();
                obj.Add(subobj);
            }
            con.Close();
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult DeleteAchievement(int Id)
        {
            Achievement obj = new Achievement();
            Connectivity Connect = new Connectivity();
            List<ParamValues> param = new List<ParamValues>();
            ParamValues sub = new ParamValues();
            sub.Code = "@Id";
            sub.Value = Id.ToString();
            param.Add(sub);
            con.Open();
            Connect.QueryExecute(con, "Delete from dbo.TeacherAchievements where Id=@Id", param);
            con.Close();
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult InsertAchievement(int TeacherId,int Id,String title,String desc,String docName)
        {
            Connectivity Connect = new Connectivity();
            Achievement obj = new Achievement();
            List<ParamValues> param = new List<ParamValues>();
            ParamValues sub = new ParamValues();
            sub.Code = "@Id";
            sub.Value = Id.ToString();
            param.Add(sub);
            sub = new ParamValues();
            sub.Code = "@TeacherId";
            sub.Value = TeacherId.ToString();
            param.Add(sub);
            sub = new ParamValues();
            sub.Code = "@Title";
            sub.Value = title;
            param.Add(sub);
            sub = new ParamValues();
            sub.Code = "@Desc";
            sub.Value = desc;
            param.Add(sub);
            sub = new ParamValues();
            sub.Code = "@DocName";
            sub.Value = docName;
            param.Add(sub);
            sub = new ParamValues();
            sub.Code = "@DocPath";
            sub.Value = "test";
            param.Add(sub);
            con.Open();
            if (Id>0)
            {
                Connect.QueryExecute(con, "Update dbo.TeacherAchievements Set AchievementTitle=@Title,Description=@Desc,DocumentName=@DocName,DocumentPath=@DocPath Where TeacherId=@TeacherId and Id=@Id", param);
            }
            else
            {
                Connect.QueryExecute(con, "Insert into dbo.TeacherAchievements (AchievementTitle,Description,DocumentName,DocumentPath,TeacherId) VALUES(@Title,@Desc,@DocName,@DocPath,@TeacherId)", param);
            }
            con.Close();
            obj.Id = 1;
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetAllLogs(int Id)
        {
            List<Log> obj = new List<Log>();
            Connectivity Connect = new Connectivity();

            ParamValues sub = new ParamValues();
            sub.Code = "@Id";
            sub.Value = Id.ToString();
            param.Add(sub);
            con.Open();
            SqlDataReader Execreader = Connect.QueryExecute(con, "select A.Id,B.CategoryName,A.Logdetails,C.UserName,D.Name,A.LoggedDateTime from dbo.TeacherLog A, dbo.TeachersLogCategory B,dbo.Users C,dbo.SettingsUserRole D where A.TeacherId=@Id and B.Id=A.CategoryId and C.Id=A.UserId and D.Id=C.SuperUser", param);
            while (Execreader.Read())
            {
                Log subobj = new Log();
                subobj.Id = Convert.ToInt32(Execreader.GetValue(0));
                subobj.CategoryName = Execreader.GetValue(1).ToString();
                subobj.LogDetails = Execreader.GetValue(2).ToString();
                subobj.UserName = Execreader.GetValue(3).ToString();
                subobj.UserRole = Execreader.GetValue(4).ToString();
                subobj.LoggedDateTime = Execreader.GetValue(5).ToString();
                obj.Add(subobj);
            }
            con.Close();
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetAllDocuments(int Id)
        {
            List<Document> obj = new List<Document>();
            Connectivity Connect = new Connectivity();

            ParamValues sub = new ParamValues();
            sub.Code = "@Id";
            sub.Value = Id.ToString();
            param.Add(sub);
            con.Open();
            SqlDataReader Execreader = Connect.QueryExecute(con, "select Id,DocumentName from dbo.TeacherDocuments where TeacherId=@Id", param);
            while (Execreader.Read())
            {
                Document subobj = new Document();
                subobj.Id = Convert.ToInt32(Execreader.GetValue(0));
                subobj.DocumentName = Execreader.GetValue(1).ToString();
                obj.Add(subobj);
            }
            con.Close();
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult InsertDocument(String DocumentName,int TeacherId)
        {
            Dummy obj = new Dummy();
            Connectivity Connect = new Connectivity();
            ParamValues sub = new ParamValues();
            sub.Code = "@DocumentName";
            sub.Value = DocumentName.ToString();
            param.Add(sub);
            sub = new ParamValues();
            sub.Code = "@TeacherId";
            sub.Value = TeacherId.ToString();
            param.Add(sub);
            con.Open();
            Connect.QueryExecute(con, "insert into dbo.TeacherDocuments(DocumentName,TeacherId) VALUES(@DocumentName,@TeacherId)", param);
            con.Close();
            obj.id = 1;
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult DeleteDocument(int Id)
        {
            Dummy obj = new Dummy();
            Connectivity Connect = new Connectivity();
            ParamValues sub = new ParamValues();
            sub.Code = "@Id";
            sub.Value = Id.ToString();
            param.Add(sub);
            con.Open();
            Connect.QueryExecute(con, "delete from dbo.TeacherDocuments where Id=@Id", param);
            con.Close();
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetLogCategories()
        {
            List<LogCategory> obj = new List<LogCategory>();
            con.Open();
            SqlDataReader Execreader = Connect.QueryExecute(con, "select Id,CategoryName from dbo.TeachersLogCategory");
            while(Execreader.Read())
            {
                LogCategory subobj = new LogCategory();
                subobj.Id = Convert.ToInt32(Execreader.GetValue(0));
                subobj.Name = Execreader.GetValue(1).ToString();
                obj.Add(subobj);
            }
            con.Close();
            return Json(obj, JsonRequestBehavior.AllowGet);

        }
        [HttpGet]
        public JsonResult GetLog(int Id)
        {
            Log obj = new Log();
            ParamValues sub = new ParamValues();
            sub.Code = "@Id";
            sub.Value = Id.ToString();
            param.Add(sub);
            con.Open();
            SqlDataReader Execreader = Connect.QueryExecute(con, "select Id,CategoryId,Logdetails,UserId,LoggedDateTime from dbo.TeacherLog where Id=@Id", param);
            if (Execreader.Read())
            {
                obj.Id = Convert.ToInt32(Execreader.GetValue(0));
                int CategoryId= Convert.ToInt32(Execreader.GetValue(1));
                //obj.CategoryName = Connect.SingleColumn("select CategoryName from dbo.TeachersLogCategory where Id="+ CategoryId);
                obj.CategoryName = CategoryId.ToString();
                obj.LogDetails = Execreader.GetValue(2).ToString();
                /*int UserId = Convert.ToInt32(Execreader.GetValue(3));
                obj.UserName = Connect.SingleColumn("select UserName from dbo.Users where Id=" + UserId);
                String UserRole= Connect.SingleColumn("select SuperUser from dbo.Users where Id=" + UserId);
                obj.UserRole = Connect.SingleColumn("select Name from dbo.SettingsUserRole where Id=" + UserRole);
                obj.LoggedDateTime = Execreader.GetValue(4).ToString();*/
            }
            con.Close();
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult DeleteLog(int Id)
        {
            Achievement obj = new Achievement();
            Connectivity Connect = new Connectivity();
            List<ParamValues> param = new List<ParamValues>();
            ParamValues sub = new ParamValues();
            sub.Code = "@Id";
            sub.Value = Id.ToString();
            param.Add(sub);
            con.Open();
            Connect.QueryExecute(con, "Delete from dbo.TeacherLog where Id=@Id", param);
            con.Close();
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult InsertLog(int TeacherId, int Id, int CategoryId, String LogDetails)
        {
            int UserId = Convert.ToInt32(Session["UserId"]);
            Connectivity Connect = new Connectivity();
            Log obj = new Log();
            List<ParamValues> param = new List<ParamValues>();
            ParamValues sub = new ParamValues();
            sub.Code = "@Id";
            sub.Value = Id.ToString();
            param.Add(sub);
            sub = new ParamValues();
            sub.Code = "@TeacherId";
            sub.Value = TeacherId.ToString();
            param.Add(sub);
            sub = new ParamValues();
            sub.Code = "@CategoryId";
            sub.Value = CategoryId.ToString();
            param.Add(sub);
            sub = new ParamValues();
            sub.Code = "@LogDetails";
            sub.Value = LogDetails;
            param.Add(sub);
            sub = new ParamValues();
            sub.Code = "@UserId";
            sub.Value = UserId.ToString();
            param.Add(sub);
            con.Open();
            if (Id > 0)
            {
                Connect.QueryExecute(con, "Update dbo.TeacherLog Set CategoryId=@CategoryId,LogDetails=@LogDetails,UserId=@UserId,LoggedDateTime=GETDATE() Where TeacherId=@TeacherId and Id=@Id", param);
            }
            else
            {
                Connect.QueryExecute(con, "Insert into dbo.TeacherLog (CategoryId,LogDetails,UserId,TeacherId,LoggedDateTime) VALUES(@CategoryId,@LogDetails,@UserId,@TeacherId,GETDATE())", param);
            }
            con.Close();
            obj.Id = 1;
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SubjectAssociation()
        {

            return View();
        }

        [HttpGet]
        public JsonResult AllCourses()
        {
            List<DummyObj> obj = new List<DummyObj>();
            con.Open();
            SqlDataReader Execreader = Connect.QueryExecute(con, "select Id,Name from dbo.Courses");
            while(Execreader.Read())
            {
                DummyObj subobj = new DummyObj();
                subobj.Id = Convert.ToInt32(Execreader.GetValue(0));
                subobj.Name = Execreader.GetValue(1).ToString();
                obj.Add(subobj);
            }
            con.Close();
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult AllBatches(int CourseId)
        {
            List<DummyObj> obj = new List<DummyObj>();
            /*ParamValues sub = new ParamValues();
            sub.Code = "@CourseId";
            sub.Value = CourseId.ToString();
            param.Add(sub);*/
            con.Open();
            SqlDataReader Execreader = Connect.QueryExecute(con, "select Id,Name from dbo.Batch where CourseId="+CourseId);
            while (Execreader.Read())
            {
                DummyObj subobj = new DummyObj();
                subobj.Id = Convert.ToInt32(Execreader.GetValue(0));
                subobj.Name = Execreader.GetValue(1).ToString();
                obj.Add(subobj);
            }
            con.Close();
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult AllSubjects(int CourseId)
        {
            List<DummyObj> obj = new List<DummyObj>();
            /*ParamValues sub = new ParamValues();
            sub.Code = "@CourseId";
            sub.Value = CourseId.ToString();
            param.Add(sub);*/
            con.Open();
            SqlDataReader Execreader = Connect.QueryExecute(con, "select Id,Name from dbo.Subjects where CourseId="+CourseId);
            while (Execreader.Read())
            {
                DummyObj subobj = new DummyObj();
                subobj.Id = Convert.ToInt32(Execreader.GetValue(0));
                subobj.Name = Execreader.GetValue(1).ToString();
                obj.Add(subobj);
            }
            con.Close();
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult CurrentlyAssigned(int CourseId,int BatchId,int SubjectId)
        {
            List<CurrentlyAssigned> obj = new List<CurrentlyAssigned>();
            /*ParamValues sub = new ParamValues();
            sub.Code = "@CourseId";
            sub.Value = CourseId.ToString();
            param.Add(sub);
            sub = new ParamValues();
            sub.Code = "@BatchId";
            sub.Value = BatchId.ToString();
            param.Add(sub);
            sub = new ParamValues();
            sub.Code = "@SubjectId";
            sub.Value = SubjectId.ToString();
            param.Add(sub);*/
            con.Open();
            SqlDataReader Execreader = Connect.QueryExecute(con, "select A.Id,B.FirstName,B.LastName,C.Name from dbo.TeacherSubjectAssociation A,dbo.StaffDetails B,dbo.TeachersManageDepartments C where A.CourseId="+ CourseId +" and A.BatchId="+BatchId+" and A.SubjectId="+ SubjectId + " and B.TeacherId=A.TeacherId and C.Id=A.DepartmentId");
            while (Execreader.Read())
            {
                CurrentlyAssigned subobj = new CurrentlyAssigned();
                subobj.Id = Convert.ToInt32(Execreader.GetValue(0));
                subobj.FirstName = Execreader.GetValue(1).ToString();
                subobj.LastName = Execreader.GetValue(2).ToString();
                subobj.Department = Execreader.GetValue(3).ToString();
                obj.Add(subobj);
            }
            con.Close();
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult AllDepartments()
        {
            List<DummyObj> obj = new List<DummyObj>();
            con.Open();
            SqlDataReader Execreader = Connect.QueryExecute(con, "select Id,Name from dbo.TeachersManageDepartments");
            while (Execreader.Read())
            {
                DummyObj subobj = new DummyObj();
                subobj.Id = Convert.ToInt32(Execreader.GetValue(0));
                subobj.Name = Execreader.GetValue(1).ToString();
                obj.Add(subobj);
            }
            con.Close();
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult DepartmentTeachers(int DepartmentId)
        {
            List<DummyObj> obj = new List<DummyObj>();
            /*ParamValues sub = new ParamValues();
            sub.Code = "@DepartmentId";
            sub.Value = DepartmentId.ToString();
            param.Add(sub);*/
            con.Open();
            SqlDataReader Execreader1 = null;
            int TeacherId = 0;
            SqlDataReader Execreader = Connect.QueryExecute(con, "select TeacherId,FirstName,LastName from dbo.StaffDetails where DepartmentId="+ DepartmentId);
            while (Execreader.Read())
            {
                DummyObj subobj = new DummyObj();
                TeacherId = Convert.ToInt32(Execreader.GetValue(0));
                Execreader1 = Connect.QueryExecute(con, "select count(TeacherId) from dbo.TeacherSubjectAssociation where TeacherId=" + TeacherId);
                if(Execreader1.Read())
                {
                    if (Convert.ToInt32(Execreader1.GetValue(0)) > 0) continue;
                }
                subobj.Id = TeacherId;
                subobj.Name = Execreader.GetValue(1).ToString();
                obj.Add(subobj);
            }
            con.Close();
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult DeleteAssigned(int Id)
        {
            Dummy obj = new Dummy();
            con.Open();
            Connect.QueryExecute(con, "Delete from dbo.TeacherSubjectAssociation where Id=" + Id);
            obj.id = Id;
            con.Close();
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult Assigned(int CourseId,int BatchId,int SubjectId,int TeacherId,int DepartmentId)
        {
            Dummy obj = new Dummy();
            con.Open();
            Connect.QueryExecute(con, "Insert dbo.TeacherSubjectAssociation(CourseId,BatchId,SubjectId,TeacherId,DepartmentId) VALUES ("+CourseId+","+BatchId+","+SubjectId+","+TeacherId+","+DepartmentId+")");
            obj.id = 1;
            con.Close();
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult ListTeachers(String OrderByColumn,int Desc,String Where,int pageSize,int pageNo)
        {
            ViewBag.OrderByColumn = OrderByColumn;
            ViewBag.Desc = Desc;
            ViewBag.Where = Where;
            ViewBag.CurrentPage = pageNo;
            ViewBag.PreviousPage = pageNo - 1;
            ViewBag.NextPage = pageNo + 1;
            if (pageNo <= 0)
            {
                ViewBag.PreviousPage = 0;
            }
            int StartRowNum = (pageSize*pageNo)+1;
            int EndRowNum = (pageSize * pageNo) + pageSize;
            String OrderBy = "asc";
            if (Desc==1)
            {
                OrderBy = "desc";
            }
            String SQL = "select TeacherId, TeacherNumber, FirstName, LastName, Gender, DepartmentId from(select ROW_NUMBER() OVER(Order by "+OrderByColumn +" "+ OrderBy+" ) As RowNum, * FROM dbo.StaffDetails "+Where+" ) As RowConstraintResult where RowNum>="+StartRowNum+" and RowNum<="+ EndRowNum+" Order by RowNum";
            TeachersList obj = new TeachersList();
            obj.Teachers = new List<Teacher>();
            con.Open();
            int cnt = Convert.ToInt32(Connect.SingleColumn(con,"select count(TeacherId) from dbo.StaffDetails"));
            ViewBag.TotalCount = cnt;
            ViewBag.TotalPages = (cnt /pageSize)-1;
            ViewBag.StartRecord = StartRowNum;
            ViewBag.EndRecord = EndRowNum;
            if(EndRowNum>cnt)
            {
                ViewBag.EndRecord = cnt;
            }
            if (pageNo>= ViewBag.TotalPages)
            {
                ViewBag.NextPage = pageNo;
            }
            SqlDataReader Exec=Connect.QueryExecute(con, SQL);
            while(Exec.Read())
            {
                Teacher sub = new Teacher();
                sub.Id = Convert.ToInt32(Exec.GetValue(0));
                sub.Number = Exec.GetValue(1).ToString();
                sub.Name = Exec.GetValue(2).ToString() + " " + Exec.GetValue(3).ToString();
                sub.Gender = "Male";
                if(Convert.ToInt32(Exec.GetValue(4))==1)
                {
                    sub.Gender = "Female";
                }
                sub.Department = Connect.SingleColumn(con, "select Name from dbo.TeachersManageDepartments where Id=" + Convert.ToInt32(Exec.GetValue(5)));
                obj.Teachers.Add(sub);

            }
            ViewBag.Departments = Connect.TableData(con, "select Id,Name from dbo.TeachersManageDepartments", "Id", "Name");
            ViewBag.Categories = Connect.TableData(con, "select Id,Name from dbo.TeachersManageCategories", "Id", "Name");
            ViewBag.Grades = Connect.TableData(con, "select Id,Name from dbo.TeachersManageGrades", "Id", "Name");
            ViewBag.Positions = Connect.TableData(con, "select Id,Name from dbo.TeachersManagePositions", "Id", "Name");

            List<SelectListItem> Genders = new List<SelectListItem>();
            Genders.Add(new SelectListItem() { Text ="Male" , Value ="0"  });
            Genders.Add(new SelectListItem() { Text = "Female", Value = "1" });
            ViewBag.Genders=new SelectList(Genders, "Value", "Text");

            List<SelectListItem> Marital = new List<SelectListItem>();
            Marital.Add(new SelectListItem() { Text = "Single", Value = "0" });
            Marital.Add(new SelectListItem() { Text = "Married", Value = "1" });
            Marital.Add(new SelectListItem() { Text = "Divorced", Value = "2" });
            ViewBag.Marital = new SelectList(Marital, "Value", "Text");

            List<SelectListItem> BloodGroup = new List<SelectListItem>();
            BloodGroup.Add(new SelectListItem() { Text = "A+", Value = "0" });
            BloodGroup.Add(new SelectListItem() { Text = "A-", Value = "1" });
            BloodGroup.Add(new SelectListItem() { Text = "B+", Value = "2" });
            BloodGroup.Add(new SelectListItem() { Text = "B-", Value = "3" });
            BloodGroup.Add(new SelectListItem() { Text = "O+", Value = "4" });
            BloodGroup.Add(new SelectListItem() { Text = "O-", Value = "5" });
            BloodGroup.Add(new SelectListItem() { Text = "AB+", Value = "6" });
            BloodGroup.Add(new SelectListItem() { Text = "AB-", Value = "7" });
            ViewBag.BloodGroup = new SelectList(BloodGroup, "Value", "Text");

            List<SelectListItem> Nationality = new List<SelectListItem>();
            Nationality.Add(new SelectListItem() { Text = "Indian", Value = "1" });
            Nationality.Add(new SelectListItem() { Text = "Others", Value = "2" });
            ViewBag.Nationality = new SelectList(Nationality, "Value", "Text");
            con.Close();
            return View(obj);
        }

        [HttpPost]
        public ActionResult SearchListTeachers(String Name,String TeacherNumber,String Batch,String Cat,String Pos,String Grd,String Gender,String Marital,String BloodGroup,String Nationality,String DOBOpt,String DOB,String JoiningOpt,String joining)
        {
            String Where = "";
            if (Name != "") {
                Where = "Where LastName like '%" + Name + "%'";
            }
            else if (TeacherNumber != "") { }
            return RedirectToAction("ListTeachers", "Teachers", new { OrderByColumn = "FirstName", Desc = 0, Where = Where });
        }
    }



}