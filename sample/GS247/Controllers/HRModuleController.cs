using System;
using GS247.Utilities;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GS247.Models;
using GS247.Controllers.Connection;
using GS247.Models.Courses;
using GS247.Models.HRModule;
using GS247.Models.Teachers;
namespace GS247.Controllers
{
    public class HRModuleController : Controller
    {
        Connectivity Connect = new Connectivity();
        // GET: HRModule
        public ActionResult CreateLeaveType()
        {
            LeaveTypeModel obj = new LeaveTypeModel();
            obj.CategoryInfo = new List<CheckBoxDetails>();
            DataSet ds = new DataSet();
            obj.Genderinfo = new List<CheckBoxDetails>();
            DataSet ds1 = new DataSet();
            using (SqlConnection con = new SqlConnection("data source=43.255.152.25;initial catalog=GS247;persist security info=True;user id=sowndharya;password=gq$1aA95;MultipleActiveResultSets=True;"))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("dbo.Categorylist", con))
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
                        obj.CategoryInfo.Add(temp);
                        //objuser.CoursesInfo.Add("Course2");
                    }
                    con.Close();
                }
                con.Open();
                using (SqlCommand cmd = new SqlCommand("dbo.Genderlist", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@status", "GET");
                    cmd.Parameters.AddWithValue("@Id", 0);
                    cmd.Parameters.AddWithValue("@Name", "");
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds1);
                    for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
                    {
                        CheckBoxDetails temp = new CheckBoxDetails();
                        temp.Id = Convert.ToInt32(ds1.Tables[0].Rows[i].ItemArray[0]);
                        temp.Text = ds1.Tables[0].Rows[i].ItemArray[1].ToString();
                        temp.IsChecked = false;
                        obj.Genderinfo.Add(temp);
                        //objuser.CoursesInfo.Add("Course2");
                    }
                    con.Close();
                }
            }

            return View(obj);
        }
        [HttpPost]
        public ActionResult CreateLeaveType(GS247.Models.HRModule.LeaveTypeModel Details)
        {

            using (SqlConnection con = new SqlConnection("data source=43.255.152.25;initial catalog=GS247;persist security info=True;user id=sowndharya;password=gq$1aA95;MultipleActiveResultSets=True;"))
            {
                con.Open();
                DataSet ds = new DataSet();
                using (SqlCommand cmd = new SqlCommand("dbo.Leavecrudoperations", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@status", "INSERT");
                    cmd.Parameters.AddWithValue("@StaffLeaveTypeId", 0);
                    cmd.Parameters.AddWithValue("@LeaveTypeName", Details.LeaveTypeName.ToString());
                    cmd.Parameters.AddWithValue("@Description", Details.Description.ToString());
                    cmd.Parameters.AddWithValue("@CategoryId", Convert.ToInt32(Details.CategoryInfo[0].Id));
                    cmd.Parameters.AddWithValue("@Gender", Convert.ToInt32(Details.Genderinfo[0].Id));
                    cmd.Parameters.AddWithValue("@Count", Details.Count.ToString());
                    cmd.Parameters.AddWithValue("@CreatedBy", "User-1");
                    cmd.Parameters.AddWithValue("@CreatedDate", DateTime.Now);
                    cmd.Parameters.AddWithValue("@UpdatedBy", "User-1");
                    cmd.Parameters.AddWithValue("@UpdatedDate", DateTime.Now);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);

                }
                con.Close();
            }

            return RedirectToAction("CreateLeaveType", "HRModule");
        }
        public ActionResult Index()
        {
            LeaveTypeModel objuser = new LeaveTypeModel();
            DataSet ds = new DataSet();
            using (SqlConnection con = new SqlConnection("data source=43.255.152.25;initial catalog=GS247;persist security info=True;user id=sowndharya;password=gq$1aA95;MultipleActiveResultSets=True;"))
            {              
                using (SqlCommand cmd = new SqlCommand("dbo.Leavecrudoperations", con))
                {   
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@status", "GET");
                    cmd.Parameters.AddWithValue("@StaffLeaveTypeId", 0);
                    cmd.Parameters.AddWithValue("@LeaveTypeName", "");
                    cmd.Parameters.AddWithValue("@Description", "");
                    cmd.Parameters.AddWithValue("@CategoryId", "");
                    cmd.Parameters.AddWithValue("@Gender", "");
                    cmd.Parameters.AddWithValue("@Count", "");
                    cmd.Parameters.AddWithValue("@CreatedBy", "");
                    cmd.Parameters.AddWithValue("@CreatedDate", "");
                    cmd.Parameters.AddWithValue("@UpdatedBy", "");
                    cmd.Parameters.AddWithValue("@UpdatedDate", "");
                    con.Open();

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);
                    List<LeaveTypeModel> userlist = new List<LeaveTypeModel>();
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        LeaveTypeModel uobj = new LeaveTypeModel();
                        uobj.StaffLeaveTypeId = Convert.ToInt32(ds.Tables[0].Rows[i]["StaffLeaveTypeId"].ToString());
                        uobj.LeaveTypeName = ds.Tables[0].Rows[i]["LeaveTypeName"].ToString();                                            
                        uobj.CategoryId = Convert.ToInt32(ds.Tables[0].Rows[i]["CategoryId"].ToString());
                        uobj.Gender = Convert.ToInt32(ds.Tables[0].Rows[i]["Gender"].ToString());
                        uobj.Count = ds.Tables[0].Rows[i]["Count"].ToString();
                        userlist.Add(uobj);
                    }
                    objuser.LeaveInfo = userlist;
                }
                con.Close();
            }
            return View(objuser);
        }

        public ActionResult ViewLeave(int Id)
        {
            LeaveTypeModel objuser = new LeaveTypeModel();
            using (SqlConnection con = new SqlConnection("data source=43.255.152.25;initial catalog=GS247;persist security info=True;user id=sowndharya;password=gq$1aA95;MultipleActiveResultSets=True;"))
            {
                DataSet ds = new DataSet();
                using (SqlCommand cmd = new SqlCommand("dbo.Leavecrudoperations", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@status", "GETID");
                    cmd.Parameters.AddWithValue("@StaffLeaveTypeId", Id);
                    cmd.Parameters.AddWithValue("@LeaveTypeName", "");
                    cmd.Parameters.AddWithValue("@Description", "");
                    cmd.Parameters.AddWithValue("@CategoryId", "");
                    cmd.Parameters.AddWithValue("@Gender", "");
                    cmd.Parameters.AddWithValue("@Count", "");
                    cmd.Parameters.AddWithValue("@CreatedBy", "");
                    cmd.Parameters.AddWithValue("@CreatedDate", "");
                    cmd.Parameters.AddWithValue("@UpdatedBy", "");
                    cmd.Parameters.AddWithValue("@UpdatedDate", "");
                    con.Open();

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);                 
                    List<LeaveTypeModel> userlist = new List<LeaveTypeModel>();
                    objuser.StaffLeaveTypeId = Convert.ToInt32(ds.Tables[0].Rows[0]["StaffLeaveTypeId"].ToString());
                    objuser.LeaveTypeName = ds.Tables[0].Rows[0]["LeaveTypeName"].ToString();
                    objuser.Description = ds.Tables[0].Rows[0]["Description"].ToString();
                    objuser.CategoryId = Convert.ToInt32(ds.Tables[0].Rows[0]["CategoryId"].ToString());
                    objuser.Gender = Convert.ToInt32(ds.Tables[0].Rows[0]["Gender"].ToString());
                    objuser.Count = ds.Tables[0].Rows[0]["Count"].ToString();
                }           
                con.Close();
            }
            return View(objuser);

        }
        [HttpGet]
        public ActionResult UpdateLeave(int Id)
        {
            LeaveTypeModel objuser = new LeaveTypeModel();
            using (SqlConnection con = new SqlConnection("data source=43.255.152.25;initial catalog=GS247;persist security info=True;user id=sowndharya;password=gq$1aA95;MultipleActiveResultSets=True;"))
            {
                con.Open();
                DataSet ds2 = new DataSet();
                using (SqlCommand cmd = new SqlCommand("dbo.Leavecrudoperations", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@status", "GETID");
                    cmd.Parameters.AddWithValue("@StaffLeaveTypeId",Id);
                    cmd.Parameters.AddWithValue("@LeaveTypeName","");
                    cmd.Parameters.AddWithValue("@Description","");
                    cmd.Parameters.AddWithValue("@CategoryId", "");
                    cmd.Parameters.AddWithValue("@Gender","");
                    cmd.Parameters.AddWithValue("@Count", "");
                    cmd.Parameters.AddWithValue("@CreatedBy", "");
                    cmd.Parameters.AddWithValue("@CreatedDate", "");
                    cmd.Parameters.AddWithValue("@UpdatedBy", "");
                    cmd.Parameters.AddWithValue("@UpdatedDate", "");

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds2);
                    List<LeaveTypeModel> userlist = new List<LeaveTypeModel>();
                    objuser.StaffLeaveTypeId = Convert.ToInt32(ds2.Tables[0].Rows[0]["StaffLeaveTypeId"].ToString());
                    objuser.LeaveTypeName = ds2.Tables[0].Rows[0]["LeaveTypeName"].ToString();
                    objuser.Description = ds2.Tables[0].Rows[0]["Description"].ToString();
                    objuser.CategoryId = Convert.ToInt32(ds2.Tables[0].Rows[0]["CategoryId"].ToString());
                    objuser.Gender = Convert.ToInt32(ds2.Tables[0].Rows[0]["Gender"].ToString());
                    objuser.Count = ds2.Tables[0].Rows[0]["Count"].ToString();
                    userlist.Add(objuser);
                }

                con.Close();
            }
            objuser.CategoryInfo = new List<CheckBoxDetails>();
            DataSet ds = new DataSet();
            objuser.Genderinfo = new List<CheckBoxDetails>();
            DataSet ds1 = new DataSet();
            using (SqlConnection con = new SqlConnection("data source=43.255.152.25;initial catalog=GS247;persist security info=True;user id=sowndharya;password=gq$1aA95;MultipleActiveResultSets=True;"))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("dbo.Categorylist", con))
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
                        objuser.CategoryInfo.Add(temp);
                        //objuser.CoursesInfo.Add("Course2");
                    }
                    con.Close();
                }
                con.Open();
                using (SqlCommand cmd = new SqlCommand("dbo.Genderlist", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@status", "GET");
                    cmd.Parameters.AddWithValue("@Id", 0);
                    cmd.Parameters.AddWithValue("@Name", "");
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds1);
                    for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
                    {
                        CheckBoxDetails temp = new CheckBoxDetails();
                        temp.Id = Convert.ToInt32(ds1.Tables[0].Rows[i].ItemArray[0]);
                        temp.Text = ds1.Tables[0].Rows[i].ItemArray[1].ToString();
                        temp.IsChecked = false;
                        objuser.Genderinfo.Add(temp);
                        //objuser.CoursesInfo.Add("Course2");
                    }
                    con.Close();
                }
            }

            return View(objuser);
        }
        [HttpPost]
        public ActionResult UpdateLeave(GS247.Models.HRModule.LeaveTypeModel Details)
        {
            LeaveTypeModel objuser = new LeaveTypeModel();
            using (SqlConnection con = new SqlConnection("data source=43.255.152.25;initial catalog=GS247;persist security info=True;user id=sowndharya;password=gq$1aA95;MultipleActiveResultSets=True;"))
            {
                con.Open();
                DataSet ds = new DataSet();
                using (SqlCommand cmd = new SqlCommand("dbo.Leavecrudoperations", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@status", "UPDATE");
                    cmd.Parameters.AddWithValue("@StaffLeaveTypeId",Details.StaffLeaveTypeId);
                    cmd.Parameters.AddWithValue("@LeaveTypeName",Details.LeaveTypeName);
                    cmd.Parameters.AddWithValue("@Description",Details.Description);
                    cmd.Parameters.AddWithValue("@CategoryId", Convert.ToInt32(Details.CategoryInfo[0].Id));
                    cmd.Parameters.AddWithValue("@Gender", Convert.ToInt32(Details.Genderinfo[0].Id));
                    cmd.Parameters.AddWithValue("@Count",Details.Count);
                    cmd.Parameters.AddWithValue("@CreatedBy","User-1");
                    cmd.Parameters.AddWithValue("@CreatedDate",DateTime.Now);
                    cmd.Parameters.AddWithValue("@UpdatedBy", "User-1");
                    cmd.Parameters.AddWithValue("@UpdatedDate",DateTime.Now);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);                    
                }                
                con.Close();
            }
            return RedirectToAction("Index", "HRModule");
        }
        [HttpGet]
        public int cmDeleteLeave(int Id)
        {
            int ret = 1;
            using (SqlConnection con = new SqlConnection("data source=43.255.152.25;initial catalog=GS247;persist security info=True;user id=sowndharya;password=gq$1aA95;MultipleActiveResultSets=True;"))
            {
                con.Open();
                DataSet ds = new DataSet();
                using (SqlCommand cmd = new SqlCommand("dbo.Leavecrudoperations", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@status", "DELETE");
                    cmd.Parameters.AddWithValue("@StaffLeaveTypeId", Id);
                    cmd.Parameters.AddWithValue("@LeaveTypeName", "");
                    cmd.Parameters.AddWithValue("@Description", "");
                    cmd.Parameters.AddWithValue("@CategoryId", "");
                    cmd.Parameters.AddWithValue("@Gender", "");
                    cmd.Parameters.AddWithValue("@Count", "");
                    cmd.Parameters.AddWithValue("@CreatedBy", "");
                    cmd.Parameters.AddWithValue("@CreatedDate", "");
                    cmd.Parameters.AddWithValue("@UpdatedBy", "");
                    cmd.Parameters.AddWithValue("@UpdatedDate", "");
                    con.Close();
                }
            }
            return ret;
        }


        

        public ActionResult DeleteLeave(int Id)
        {
            using (SqlConnection con = new SqlConnection("data source=43.255.152.25;initial catalog=GS247;persist security info=True;user id=sowndharya;password=gq$1aA95;MultipleActiveResultSets=True;"))
            {
                con.Open();
                DataSet ds = new DataSet();
                using (SqlCommand cmd = new SqlCommand("dbo.Leavecrudoperations", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@status", "DELETE");
                    cmd.Parameters.AddWithValue("@StaffLeaveTypeId", Id);
                    cmd.Parameters.AddWithValue("@LeaveTypeName", "");
                    cmd.Parameters.AddWithValue("@Description", "");
                    cmd.Parameters.AddWithValue("@CategoryId", "");
                    cmd.Parameters.AddWithValue("@Gender", "");
                    cmd.Parameters.AddWithValue("@Count", "");
                    cmd.Parameters.AddWithValue("@CreatedBy", "");
                    cmd.Parameters.AddWithValue("@CreatedDate", "");
                    cmd.Parameters.AddWithValue("@UpdatedBy", "");
                    cmd.Parameters.AddWithValue("@UpdatedDate", "");
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);
                }
                con.Close();
            }
            return RedirectToAction("Index", "HRModule");
        }
        public ActionResult Leaverequest()
        {
            LeaveRequest objuser = new LeaveRequest();
            DataSet ds = new DataSet();
            using (SqlConnection con = new SqlConnection("data source=43.255.152.25;initial catalog=GS247;persist security info=True;user id=sowndharya;password=gq$1aA95;MultipleActiveResultSets=True;"))
            {
                using (SqlCommand cmd = new SqlCommand("dbo.Leaverequest", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@action", "GET");
                    cmd.Parameters.AddWithValue("@Id", 0);
                    cmd.Parameters.AddWithValue("@LeaveType", "");
                    cmd.Parameters.AddWithValue("@RequestedBy", "");
                    cmd.Parameters.AddWithValue("@FromDate", "");
                    cmd.Parameters.AddWithValue("@ToDate", "");
                    cmd.Parameters.AddWithValue("@Days", "");
                    cmd.Parameters.AddWithValue("@Reason", "");
                    cmd.Parameters.AddWithValue("@HalfDay", "");
                    cmd.Parameters.AddWithValue("@Status", "");
                    con.Open();

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);
                    List<LeaveRequest> userlist = new List<LeaveRequest>();
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        LeaveRequest uobj = new LeaveRequest();
                        uobj.Id = Convert.ToInt32(ds.Tables[0].Rows[i]["Id"].ToString());
                        uobj.LeaveType= ds.Tables[0].Rows[i]["LeaveType"].ToString();
                        uobj.RequestedBy = ds.Tables[0].Rows[i]["RequestedBy"].ToString();
                        uobj.FromDate = Convert.ToDateTime(ds.Tables[0].Rows[i]["FromDate"].ToString());
                        uobj.ToDate = Convert.ToDateTime(ds.Tables[0].Rows[i]["ToDate"].ToString());
                        uobj.Days = ds.Tables[0].Rows[i]["Days"].ToString();
                        userlist.Add(uobj);
                    }
                    objuser.LeaverequestInfo = userlist;
                }
                con.Close();
            }
            return View(objuser);
        }
        public ActionResult ApprovedLeave()
        {
            LeaveRequest objuser = new LeaveRequest();
            DataSet ds = new DataSet();
            using (SqlConnection con = new SqlConnection("data source=43.255.152.25;initial catalog=GS247;persist security info=True;user id=sowndharya;password=gq$1aA95;MultipleActiveResultSets=True;"))
            {
                using (SqlCommand cmd = new SqlCommand("dbo.Leaverequest", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@action", "APPROVEGET");
                    cmd.Parameters.AddWithValue("@Id", 0);
                    cmd.Parameters.AddWithValue("@LeaveType", "");
                    cmd.Parameters.AddWithValue("@RequestedBy", "");
                    cmd.Parameters.AddWithValue("@FromDate", "");
                    cmd.Parameters.AddWithValue("@ToDate", "");
                    cmd.Parameters.AddWithValue("@Days", "");
                    cmd.Parameters.AddWithValue("@Reason", "");
                    cmd.Parameters.AddWithValue("@HalfDay", "");
                    cmd.Parameters.AddWithValue("@Status", "");
                    con.Open();

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);
                    List<LeaveRequest> userlist = new List<LeaveRequest>();
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        LeaveRequest uobj = new LeaveRequest();
                        uobj.Id = Convert.ToInt32(ds.Tables[0].Rows[i]["Id"].ToString());
                        uobj.LeaveType = ds.Tables[0].Rows[i]["LeaveType"].ToString();
                        uobj.RequestedBy = ds.Tables[0].Rows[i]["RequestedBy"].ToString();
                        uobj.FromDate = Convert.ToDateTime(ds.Tables[0].Rows[i]["FromDate"].ToString());
                        uobj.ToDate = Convert.ToDateTime(ds.Tables[0].Rows[i]["ToDate"].ToString());
                        //uobj.ApprovedBy = ds.Tables[0].Rows[i]["ApprovedBy"].ToString();
                        userlist.Add(uobj);
                    }
                    objuser.LeaverequestInfo = userlist;
                }
                con.Close();
            }
            return View(objuser);
        }
        public ActionResult CancelledLeave()
        {
            LeaveRequest objuser = new LeaveRequest();
            DataSet ds = new DataSet();
            using (SqlConnection con = new SqlConnection("data source=43.255.152.25;initial catalog=GS247;persist security info=True;user id=sowndharya;password=gq$1aA95;MultipleActiveResultSets=True;"))
            {
                using (SqlCommand cmd = new SqlCommand("dbo.Leaverequest", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@action", "REJECTGET");
                    cmd.Parameters.AddWithValue("@Id", 0);
                    cmd.Parameters.AddWithValue("@LeaveType", "");
                    cmd.Parameters.AddWithValue("@RequestedBy", "");
                    cmd.Parameters.AddWithValue("@FromDate", "");
                    cmd.Parameters.AddWithValue("@ToDate", "");
                    cmd.Parameters.AddWithValue("@Days", "");
                    cmd.Parameters.AddWithValue("@Reason", "");
                    cmd.Parameters.AddWithValue("@HalfDay", "");
                    cmd.Parameters.AddWithValue("@Status", "");
                    con.Open();

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);
                    List<LeaveRequest> userlist = new List<LeaveRequest>();
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        LeaveRequest uobj = new LeaveRequest();
                        uobj.Id = Convert.ToInt32(ds.Tables[0].Rows[i]["Id"].ToString());
                        uobj.LeaveType = ds.Tables[0].Rows[i]["LeaveType"].ToString();
                        uobj.RequestedBy = ds.Tables[0].Rows[i]["RequestedBy"].ToString();
                        uobj.FromDate = Convert.ToDateTime(ds.Tables[0].Rows[i]["FromDate"].ToString());
                        uobj.ToDate = Convert.ToDateTime(ds.Tables[0].Rows[i]["ToDate"].ToString());
                        //uobj.ApprovedBy = ds.Tables[0].Rows[i]["ApprovedBy"].ToString();
                        userlist.Add(uobj);
                    }
                    objuser.LeaverequestInfo = userlist;
                }
                con.Close();
            }
            return View(objuser);
        }
        public ActionResult ViewLeaverequest(int Id)
        {
            LeaveRequest objuser = new LeaveRequest();
            using (SqlConnection con = new SqlConnection("data source=43.255.152.25;initial catalog=GS247;persist security info=True;user id=sowndharya;password=gq$1aA95;MultipleActiveResultSets=True;"))
            {
                DataSet ds = new DataSet();
                using (SqlCommand cmd = new SqlCommand("dbo.Leaverequest", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@action", "GETID");
                    cmd.Parameters.AddWithValue("@Id", Id);
                    cmd.Parameters.AddWithValue("@LeaveType", "");
                    cmd.Parameters.AddWithValue("@RequestedBy", "");
                    cmd.Parameters.AddWithValue("@FromDate", "");
                    cmd.Parameters.AddWithValue("@ToDate", "");
                    cmd.Parameters.AddWithValue("@Days", "");
                    cmd.Parameters.AddWithValue("@Reason", "");
                    cmd.Parameters.AddWithValue("@HalfDay", "");
                    cmd.Parameters.AddWithValue("@Status", "");
                    con.Open();

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);
                    List<LeaveRequest> userlist = new List<LeaveRequest>();                    
                    objuser.Id = Convert.ToInt32(ds.Tables[0].Rows[0]["Id"].ToString());
                    objuser.LeaveType = ds.Tables[0].Rows[0]["LeaveType"].ToString();
                    objuser.RequestedBy = ds.Tables[0].Rows[0]["RequestedBy"].ToString();
                    objuser.FromDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["FromDate"].ToString());
                    objuser.ToDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["ToDate"].ToString());
                    objuser.Days = ds.Tables[0].Rows[0]["Days"].ToString();
                    objuser.Reason = ds.Tables[0].Rows[0]["Reason"].ToString();
                    objuser.HalfDay = ds.Tables[0].Rows[0]["HalfDay"].ToString();
                    objuser.Status = ds.Tables[0].Rows[0]["Status"].ToString();            
                }
                con.Close();
            }
            return View(objuser);

        }
        [HttpPost]
        public ActionResult UpdateLeaveRequest(int Id)
        {
            LeaveRequest objuser = new LeaveRequest();
            using (SqlConnection con = new SqlConnection("data source=43.255.152.25;initial catalog=GS247;persist security info=True;user id=sowndharya;password=gq$1aA95;MultipleActiveResultSets=True;"))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("update staffLeaverequest set Status='Approve' where Id='" + Id + "'", con);
                cmd.ExecuteNonQuery();
                
            }
            return RedirectToAction("Leaverequest", "HRModule");
        }
        [HttpPost]
        public ActionResult RejectLeaveRequest(int Id)
        {
            LeaveRequest objuser = new LeaveRequest();
            using (SqlConnection con = new SqlConnection("data source=43.255.152.25;initial catalog=GS247;persist security info=True;user id=sowndharya;password=gq$1aA95;MultipleActiveResultSets=True;"))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("update staffLeaverequest set Status='Reject' where Id='" + Id + "'", con);
                cmd.ExecuteNonQuery();

            }
            return RedirectToAction("Leaverequest", "HRModule");
        }

        public ActionResult Payslip()
        {
            return View();
        }
        public ActionResult Reports()
        {
            return View();
        }
        [HttpPost]
        public string GetSalary_Details(string flag)
        {
            string res = string.Empty;
            DataAccess obj = new DataAccess();
            List<Params> paramList = new List<Params>();
            paramList.Add(new Params("@Flag", flag));
            DataTable dt = obj.Execute("GetSalaryDetails", CommandType.StoredProcedure, paramList);
            res = JsonConvert.SerializeObject(dt, Formatting.Indented);
            return res;

        }

        [HttpPost]
        public string UpdateSalary_Details(string flag, string BasicPay, string TDS, string ESI, string EPF, string staff_Id)
        {
            string res = string.Empty;
            DataAccess obj = new DataAccess();
            List<Params> paramList = new List<Params>();
            paramList.Add(new Params("@StaffSysId", Convert.ToInt32(staff_Id)));
            paramList.Add(new Params("@BasicPay", BasicPay));
            paramList.Add(new Params("@EPF", EPF));
            paramList.Add(new Params("@ESI", ESI));
            paramList.Add(new Params("@TDS", TDS));
            paramList.Add(new Params("@Flag", flag));
            DataTable dt = obj.Execute("GetSalaryDetails", CommandType.StoredProcedure, paramList);
            res = JsonConvert.SerializeObject(dt, Formatting.Indented);
            return res;

        }


        [HttpPost]
        public string PaySlipInsert(string flag, string BasicPay, string TDS, string ESI, string EPF, string Empno, string SalaryDate, string Incentive, string overtime, string hike, string TotalEarn, string Lop, string Loan, string Bonus, string TotalDeduction, string NetSalary, string Notes)
        {
            string res = string.Empty;
            DataAccess obj = new DataAccess();
            List<Params> paramList = new List<Params>();
            paramList.Add(new Params("@SalaryDate", Convert.ToDateTime(SalaryDate)));
            paramList.Add(new Params("@Incentive", Incentive));
            paramList.Add(new Params("@Overtime", overtime));
            paramList.Add(new Params("@Hike", hike));
            paramList.Add(new Params("@TotalEarn", TotalEarn));
            paramList.Add(new Params("@Lop", Lop));
            paramList.Add(new Params("@Loan", Loan));
            paramList.Add(new Params("@Bonus", Bonus));
            paramList.Add(new Params("@TotalDeduction", TotalDeduction));
            paramList.Add(new Params("@NetSalary", NetSalary));
            paramList.Add(new Params("@Notes", Notes));
            paramList.Add(new Params("@StaffSysId", Convert.ToInt32(Empno)));
            paramList.Add(new Params("@BasicPay", BasicPay));
            paramList.Add(new Params("@EPF", EPF));
            paramList.Add(new Params("@ESI", ESI));
            paramList.Add(new Params("@TDS", TDS));
            paramList.Add(new Params("@Flag", flag));
            DataTable dt = obj.Execute("GetSalaryDetails", CommandType.StoredProcedure, paramList);
            res = JsonConvert.SerializeObject(dt, Formatting.Indented);
            return res;

        }
        [HttpGet]
        public ActionResult ListTeachers(String OrderByColumn, int Desc, String Where)
        {
            using (SqlConnection con = new SqlConnection("data source=43.255.152.25;initial catalog=GS247;persist security info=True;user id=sowndharya;password=gq$1aA95;MultipleActiveResultSets=True;"))
            {
                String OrderBy = "asc";
                if (Desc == 1)
                {
                    OrderBy = "desc";
                }
                String SQL = "select TeacherId, TeacherNumber, FirstName, LastName, Gender, DepartmentId from(select ROW_NUMBER() OVER(Order by " + OrderByColumn + " " + OrderBy + " ) As RowNum, * FROM dbo.StaffDetails " + Where + " ) As RowConstraintResult where RowNum>= 1 and RowNum<= 10 Order by RowNum";
                TeachersList obj = new TeachersList();
                obj.Teachers = new List<Teacher>();
                con.Open();
                SqlDataReader Exec = Connect.QueryExecute(con, SQL);
                while (Exec.Read())
                {
                    Teacher sub = new Teacher();
                    sub.Id = Convert.ToInt32(Exec.GetValue(0));
                    sub.Number = Exec.GetValue(1).ToString();
                    sub.Name = Exec.GetValue(2).ToString() + " " + Exec.GetValue(3).ToString();
                    sub.Gender = "Male";
                    if (Convert.ToInt32(Exec.GetValue(4)) == 1)
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
                Genders.Add(new SelectListItem() { Text = "Male", Value = "0" });
                Genders.Add(new SelectListItem() { Text = "Female", Value = "1" });
                ViewBag.Genders = new SelectList(Genders, "Value", "Text");

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
        }

        [HttpPost]
        public ActionResult SearchListTeachers(String Name, String TeacherNumber, String Batch, String Cat, String Pos, String Grd, String Gender, String Marital, String BloodGroup, String Nationality, String DOBOpt, String DOB, String JoiningOpt, String joining)
        {
            String Where = "";
            if (Name != "")
            {
                Where = "Where LastName like '%" + Name + "%'";
            }
            else if (TeacherNumber != "") { }
            return RedirectToAction("ListTeachers", "Teachers", new { OrderByColumn = "FirstName", Desc = 0, Where = Where });
        }
    }
}