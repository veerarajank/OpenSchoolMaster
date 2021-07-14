using GS247.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using System.Configuration;
using GS247.Models.Export;
using ClosedXML.Excel;
using System.Web.Services.Description;

namespace GS247.Controllers
{
    public class ExportController : Controller
    {
       public GS247Entities8 db=new GS247Entities8();
        SqlConnection conn=new SqlConnection(ConfigurationManager.ConnectionStrings["GS247"].ConnectionString);
        SqlCommand cmd;
        public string fileLocation;
        public DataSet ds;
        public SqlDataAdapter da;
        public SqlDataReader dr;
        public ActionResult Export_Database()
        {
            int UserId = Convert.ToInt32(Session["UserId"]);
            if (UserId == 0)
            {
                return RedirectToAction("Index", "Login");
            }
            ViewBag.Columns = new SelectList("");
            ViewBag.SelectedColumns = new SelectList("");
            ViewBagList();
            return View(new ExportClass());
        }
        public void ViewBagList()
        {
            ViewBag.Courses = new SelectList(db.Courses.ToList(),"Id","Name");
            ViewBag.Batches = new SelectList(db.batches.ToList(),"Id","Name");
        }
        public ActionResult GetFields(ExportClass export)
        {
            int UserId = Convert.ToInt32(Session["UserId"]);
            if (UserId == 0)
            {
                return RedirectToAction("Index", "Login");
            }
            List<SelectListItem> fields = new List<SelectListItem>();
            string query = "";
            if (export.Model == "Student") query = "select top 0 * from StudentView";
            else if (export.Model == "Teacher") query = "select top 0 * from TeacherView";
            else if (export.Model == "Guardian") query = "select top 0 * from GuardianView";
            else if (export.Model == "HR") query = "select top 0 * from HRView";
            using (conn)
            {
                conn.Open();
                
                cmd = new SqlCommand(query,conn);
                dr = cmd.ExecuteReader();
              
                for (int i = 0; i < dr.FieldCount; i++)
                {
                    if(dr.GetName(i)!="BatchId"&&dr.GetName(i)!="CourseId")
                    fields.Add(new SelectListItem { Text = dr.GetName(i), Value = dr.GetName(i) });
                }                
            }
            ViewBag.Columns = fields;
            ViewBag.SelectedColumns = new SelectList("");
            ViewBagList();
            return View("Export_Database",export);
        }
        [HttpPost]
      public ActionResult Export(ExportClass export)
        {
            int UserId = Convert.ToInt32(Session["UserId"]);
            if (UserId == 0)
            {
                return RedirectToAction("Index", "Login");
            }
            try
            {
                string query = "";
                conn.Open();
                string fields = "";
                if (export.Fields != null)
                    ViewBag.Columns = new SelectList(export.Fields);
                else
                    ViewBag.Columns = new SelectList("");
                ViewBag.SelectedColumns = new SelectList(export.SelectedFields);
                ViewBagList();
                if (export.SelectedFields!=null)
                { 
                if (export.SelectedFields.Count != 0)
                {
                    //Combining selected Fields to make query
                    foreach (string field in export.SelectedFields)
                    {
                        fields = fields+field + ",";
                    }

                    //Removing Last comma
                    fields=fields.Remove(fields.Count() - 1);

                        if (export.Model=="Student"&&export.Batch!=null&&export.Course!=null)
                            query = "Select " + fields + " from StudentView where BatchId='"+export.Batch+"' and CourseId='"+export.Course+"'";
                        else if (export.Model == "Student" && export.Batch != null && export.Course == null)
                            query = "Select " + fields + " from StudentView where BatchId='" + export.Batch + "'";
                        else if (export.Model == "Student" && export.Batch == null && export.Course == null)
                            query = "Select " + fields + " from StudentView";
                        else if (export.Model == "Guardian" && export.Batch != null && export.Course != null)
                            query = "Select " + fields + " from GuardianView where BatchId='" + export.Batch + "' and CourseId='" + export.Course + "'";
                        else if (export.Model == "Guardian" && export.Batch != null && export.Course == null)
                            query = "Select " + fields + " from GuardianView where BatchId='" + export.Batch + "'";
                        else if (export.Model == "Guardian" && export.Batch == null && export.Course == null)
                            query = "Select " + fields + " from GuardianView";
                        else if (export.Model == "Teacher" && export.Batch != null && export.Course != null)
                            query = "Select " + fields + " from TeacherView where BatchId='" + export.Batch + "' and CourseId='" + export.Course + "'";
                        else if (export.Model == "Teacher" && export.Batch != null && export.Course == null)
                            query = "Select " + fields + " from TeacherView where BatchId='" + export.Batch + "'";
                        else if (export.Model == "Teacher" && export.Batch == null && export.Course == null)
                            query = "Select " + fields + " from TeacherView";
                        else if (export.Model == "HR")
                            query = "Select " + fields + " from HRView";
                        System.Data.DataTable dt = new System.Data.DataTable("Grid");
                    
                    da = new SqlDataAdapter(query, conn);
                    da.Fill(dt);

if(export.Model=="Student")
                        {
                            for(int i=0;i<dt.Rows.Count;i++)
                            {
                                dt.Rows[i]["Blood"] =Convert.ToInt32(dt.Rows[i]["Blood"])!=0?Blood(dt.Rows[i]["Blood"].ToString()):"";
                                dt.Rows[i]["Gender"] = Convert.ToInt32(dt.Rows[i]["Gender"]) != 0 ? Gender(dt.Rows[i]["Gender"].ToString()) : "";
                            }
                        }
else if(export.Model=="Teacher"||export.Model=="HR")
                        {
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                if (Convert.ToInt32(dt.Rows[i]["MaritalStatus"]) == 1)
                                    dt.Rows[i]["MaritalStatus"] = "Yes";
                                else
                                    dt.Rows[i]["MaritalStatus"] = "No";
                            }
                        }
                        else if (export.Model == "Guardian")
                        {
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                dt.Rows[i]["Relation"] = dt.Rows[i]["Relation"].ToString() != null ? Relation(dt.Rows[i]["Relation"].ToString()) : "";
                            }
                        }
                        conn.Close();
                        if (dt.Rows.Count > 0)
                        {
                            using (XLWorkbook wb = new XLWorkbook())
                            {
                                wb.Worksheets.Add(dt);
                                using (MemoryStream stream = new MemoryStream())
                                {
                                    wb.SaveAs(stream);
                                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", export.Model + ".xlsx");
                                }
                            }
                        }
                        else
                            TempData["ExportDatabase"] = "No data found to export";
                }
            }
                else
                TempData["ExportDatabase"] = "No fields selected to Export";
                return View("Export_Database", export);
            }
            catch(Exception e)
            {
                return RedirectToAction("Export_Database");
            }
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
            string blood = Blood.Where(x => x.Value == bloodText).Select(x => x.Text).SingleOrDefault();
            return blood;
        }
        public string Gender(string genderText)
        {
            List<System.Web.Mvc.SelectListItem> Gender = new List<System.Web.Mvc.SelectListItem> {
                new System.Web.Mvc.SelectListItem { Text = "Male", Value = "1" },
            new System.Web.Mvc.SelectListItem { Text = "Female", Value = "2" },
            new System.Web.Mvc.SelectListItem { Text = "Unknown", Value = "3" }};
            string gender = Gender.Where(x => x.Value == genderText).Select(x => x.Text).SingleOrDefault();
            return gender;
        }
        public string Relation(string relation)
        {
            List<System.Web.Mvc.SelectListItem> Relation = new List<System.Web.Mvc.SelectListItem> {
                new System.Web.Mvc.SelectListItem { Text = "Father", Value = "1" },
            new System.Web.Mvc.SelectListItem { Text = "Mother", Value = "2" },
            new System.Web.Mvc.SelectListItem { Text = "Other", Value = "3" }};
            string relationshipId = Relation.Where(x => x.Value == relation).Select(x => x.Text).SingleOrDefault();
            return relationshipId;
        }
    }
}