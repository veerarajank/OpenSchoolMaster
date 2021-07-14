using GS247.Models.Settings;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GS247.Controllers
{
    public class SettingsController : Controller
    {
        public SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["GS247"].ConnectionString);
        // GET: Settings
        public ActionResult Index()
        {
            int UserId = Convert.ToInt32(Session["UserId"]);
            if (UserId == 0)
            {
                return RedirectToAction("Index", "Login");
            }
            return View();
        }

        public ActionResult Home()
        {
            int UserId = Convert.ToInt32(Session["UserId"]);
            if (UserId == 0)
            {
                return RedirectToAction("Index", "Login");
            }
            return View();
        }

        /**Start of Academic Years Settings**/
        //Academic Year Views
        public ActionResult ManageYears()
        {
            int UserId = Convert.ToInt32(Session["UserId"]);
            if (UserId == 0)
            {
                return RedirectToAction("Index", "Login");
            }
            AcademicYearList obj = new AcademicYearList();
            obj.Years = new List<AcademicYear>();
            
                con.Open();
                DataSet ds = new DataSet();
                using (SqlCommand cmd = new SqlCommand("dbo.CourseSettings", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@param", "ViewYear");
                    cmd.Parameters.AddWithValue("@Id", 0);
                    cmd.Parameters.AddWithValue("@Name", "");
                    cmd.Parameters.AddWithValue("@StartsOn", "");
                    cmd.Parameters.AddWithValue("@EndsOn", "");
                    cmd.Parameters.AddWithValue("@Description", "");
                    cmd.Parameters.AddWithValue("@Status", 0);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);
                    if (ds.Tables.Count > 0)
                    {
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            AcademicYear subobj = new AcademicYear();
                            subobj.Id = Convert.ToInt32(ds.Tables[0].Rows[i].ItemArray[0]);
                            subobj.Name = ds.Tables[0].Rows[i].ItemArray[1].ToString();
                            subobj.StartsOn = ds.Tables[0].Rows[i].ItemArray[2].ToString();
                            subobj.EndsOn = ds.Tables[0].Rows[i].ItemArray[3].ToString();
                            subobj.Description = ds.Tables[0].Rows[i].ItemArray[4].ToString();
                            int stat = Convert.ToInt32(ds.Tables[0].Rows[i].ItemArray[5]);
                            subobj.SelectStatus = "Inactive";
                            if (stat == 1)
                            {
                                subobj.SelectStatus = "Active";
                            }
                            obj.Years.Add(subobj);
                        }
                    }
                }
                con.Close();
            
            return View(obj);
        }
        [HttpPost]
        public ActionResult InsertYear(GS247.Models.Settings.MAcademicYear data)
        {
            int UserId = Convert.ToInt32(Session["UserId"]);
            if (UserId == 0)
            {
                return RedirectToAction("Index", "Login");
            }
            con.Open();
                DataSet ds = new DataSet();
                using (SqlCommand cmd = new SqlCommand("dbo.CourseSettings", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    String str = "AddYear";
                    int val = 0;
                    if (data.Id > 0)
                    {
                        str = "UpdateYear";
                        val = data.Id;
                    }
                    if(data.OptStatus==status.Active)
                    {
                        SqlDataReader reader = null;
                        SqlCommand sqlCmd = new SqlCommand();
                        sqlCmd.CommandType = CommandType.Text;
                        sqlCmd.CommandText = "update dbo.Years Set Status=0";
                        sqlCmd.Connection = con;
                        reader = sqlCmd.ExecuteReader();
                        reader.Read();
                    }
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@param", str);
                    cmd.Parameters.AddWithValue("@Id", data.Id);
                    cmd.Parameters.AddWithValue("@Name", data.Name);
                    cmd.Parameters.AddWithValue("@StartsOn", data.StartsOn);
                    cmd.Parameters.AddWithValue("@EndsOn", data.EndsOn);
                    cmd.Parameters.AddWithValue("@Description", data.Description);
                    cmd.Parameters.AddWithValue("@Status", data.OptStatus);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);

                }
                con.Close();
            

            return RedirectToAction("ManageYears", "Settings");
        }


        public ActionResult DeleteYear(int Id)
        {
            int UserId = Convert.ToInt32(Session["UserId"]);
            if (UserId == 0)
            {
                return RedirectToAction("Index", "Login");
            }
            con.Open();
                DataSet ds = new DataSet();
                using (SqlCommand cmd = new SqlCommand("dbo.CourseSettings", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@param", "DeleteYear");
                    cmd.Parameters.AddWithValue("@Id", Id);
                    cmd.Parameters.AddWithValue("@Name", "");
                    cmd.Parameters.AddWithValue("@StartsOn", "");
                    cmd.Parameters.AddWithValue("@EndsOn", "");
                    cmd.Parameters.AddWithValue("@Description", "");
                    cmd.Parameters.AddWithValue("@Status", 0);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);

                }
                con.Close();
            return RedirectToAction("ManageYears", "Settings");
        }
        public ActionResult AddYear()
        {
            int UserId = Convert.ToInt32(Session["UserId"]);
            if (UserId == 0)
            {
                return RedirectToAction("Index", "Login");
            }
            MAcademicYear obj = new MAcademicYear();
            return View(obj);
        }
        public ActionResult UpdateYear(int Id)
        {
            int UserId = Convert.ToInt32(Session["UserId"]);
            if (UserId == 0)
            {
                return RedirectToAction("Index", "Login");
            }
            MAcademicYear obj = new MAcademicYear();
           
                con.Open();
                DataSet ds = new DataSet();
                using (SqlCommand cmd = new SqlCommand("dbo.CourseSettings", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@param", "GetYear");
                    cmd.Parameters.AddWithValue("@Id", Id);
                    cmd.Parameters.AddWithValue("@Name", "");
                    cmd.Parameters.AddWithValue("@StartsOn", "");
                    cmd.Parameters.AddWithValue("@EndsOn", "");
                    cmd.Parameters.AddWithValue("@Description", "");
                    cmd.Parameters.AddWithValue("@Status", 0);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);
                    if (ds.Tables.Count > 0)
                    {
                        obj.Id = Id;
                        obj.Name = ds.Tables[0].Rows[0].ItemArray[1].ToString();
                        obj.StartsOn = ds.Tables[0].Rows[0].ItemArray[2].ToString();
                        obj.EndsOn = ds.Tables[0].Rows[0].ItemArray[3].ToString();
                        obj.Description = ds.Tables[0].Rows[0].ItemArray[4].ToString();
                        int value = Convert.ToInt32(ds.Tables[0].Rows[0].ItemArray[5]);
                        obj.OptStatus = status.Inactive;
                        if (value == 1)
                        {
                            obj.OptStatus = status.Active;
                        }
                    }

                }
                con.Close();
            return View(obj);
        }

        /**End of Academic Years Settings**/

        /**Start of User Settings**/
        public ActionResult ManageUsers()
        {
            int UserId = Convert.ToInt32(Session["UserId"]);
            if (UserId == 0)
            {
                return RedirectToAction("Index", "Login");
            }
            UsersList obj = new UsersList();
            obj.NewUsers = new List<NewUser>();
            
                con.Open();
                //DataSet ds = new DataSet();
                SqlDataReader reader = null;
                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.CommandType= CommandType.Text;
                sqlCmd.CommandText = "Select Id,FirstName,LastName,SuperUser,Email,LastVisit from dbo.Users";
                sqlCmd.Connection = con;
                reader = sqlCmd.ExecuteReader();
                while(reader.Read())
                {
                    NewUser subobj = new NewUser();
                    subobj.Id = Convert.ToInt32(reader.GetValue(0));
                    subobj.FirstName = reader.GetValue(1).ToString();
                    subobj.LastName = reader.GetValue(2).ToString();
                    int suser = Convert.ToInt32(reader.GetValue(3));
                    subobj.SuperUserStr = "NoRoles";
                    if (suser == 1)
                    {
                        subobj.SuperUserStr = "Admin";
                    }
                    subobj.Email = reader.GetValue(4).ToString();
                    subobj.LastVisit = reader.GetValue(5).ToString();
                    obj.NewUsers.Add(subobj);
                }
                con.Close();
            
            return View(obj);
        }
        [HttpPost]
        public ActionResult InsertUser(GS247.Models.Settings.NewUser data)
        {
            int UserId = Convert.ToInt32(Session["UserId"]);
            if (UserId == 0)
            {
                return RedirectToAction("Index", "Login");
            }
            con.Open();
                DataSet ds = new DataSet();
                using (SqlCommand cmd = new SqlCommand("dbo.settings", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    String str = "InsertUser";
                    int val = 0;
                    if (data.Id > 0)
                    {
                        str = "UpdateUser";
                        val = data.Id;
                    }
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@param", str);
                    cmd.Parameters.AddWithValue("@Id", data.Id);
                    cmd.Parameters.AddWithValue("@UserName", data.UserName);
                    cmd.Parameters.AddWithValue("@Password", data.Password);
                    cmd.Parameters.AddWithValue("@Email", data.Email);
                    cmd.Parameters.AddWithValue("@Mobile", data.Mobile);
                    cmd.Parameters.AddWithValue("@SuperUser", data.SuperUser);
                    cmd.Parameters.AddWithValue("@Status", data.Status);
                    cmd.Parameters.AddWithValue("@FirstName", data.FirstName);
                    cmd.Parameters.AddWithValue("@LastName", data.LastName);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);

                }
                con.Close();
            return RedirectToAction("ManageUsers", "Settings");
        }


        public ActionResult DeleteUser(int Id)
        {
            int UserId = Convert.ToInt32(Session["UserId"]);
            if (UserId == 0)
            {
                return RedirectToAction("Index", "Login");
            }
            con.Open();
                DataSet ds = new DataSet();
                using (SqlCommand cmd = new SqlCommand("dbo.settings", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@param", "DeleteUser");
                    cmd.Parameters.AddWithValue("@Id", Id);
                    cmd.Parameters.AddWithValue("@UserName", "");
                    cmd.Parameters.AddWithValue("@Password", "");
                    cmd.Parameters.AddWithValue("@Email", "");
                    cmd.Parameters.AddWithValue("@Mobile", "");
                    cmd.Parameters.AddWithValue("@SuperUser", "");
                    cmd.Parameters.AddWithValue("@Status", "");
                    cmd.Parameters.AddWithValue("@FirstName", "");
                    cmd.Parameters.AddWithValue("@LastName", "");
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);

                }
                con.Close();
            
            return RedirectToAction("ManageUsers", "Settings");
        }
        public ActionResult AddUser()
        {
            int UserId = Convert.ToInt32(Session["UserId"]);
            if (UserId == 0)
            {
                return RedirectToAction("Index", "Login");
            }
            NewUser obj = new NewUser();
            return View(obj);
        }
        public ActionResult UpdateUser(int Id)
        {
            int UserId = Convert.ToInt32(Session["UserId"]);
            if (UserId == 0)
            {
                return RedirectToAction("Index", "Login");
            }
            NewUser obj = new NewUser();
            
                con.Open();
                DataSet ds = new DataSet();
                using (SqlCommand cmd = new SqlCommand("dbo.settings", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@param", "GetUser");
                    cmd.Parameters.AddWithValue("@Id", Id);
                    cmd.Parameters.AddWithValue("@UserName", "");
                    cmd.Parameters.AddWithValue("@Password", "");
                    cmd.Parameters.AddWithValue("@Email", "");
                    cmd.Parameters.AddWithValue("@Mobile", "");
                    cmd.Parameters.AddWithValue("@SuperUser", "");
                    cmd.Parameters.AddWithValue("@Status", "");
                    cmd.Parameters.AddWithValue("@FirstName", "");
                    cmd.Parameters.AddWithValue("@LastName", "");
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);
                    if (ds.Tables.Count > 0)
                    {
                        obj.Id = Id;
                        obj.UserName = ds.Tables[0].Rows[0].ItemArray[1].ToString();
                        obj.Password = ds.Tables[0].Rows[0].ItemArray[2].ToString();
                        obj.Email = ds.Tables[0].Rows[0].ItemArray[3].ToString();
                        obj.Mobile = ds.Tables[0].Rows[0].ItemArray[4].ToString();
                        int suser = Convert.ToInt32(ds.Tables[0].Rows[0].ItemArray[5]);
                        obj.SuperUser = YesNo.No;
                        if (suser == 1)
                        { obj.SuperUser = YesNo.Yes; }
                        int value = Convert.ToInt32(ds.Tables[0].Rows[0].ItemArray[6]);
                        obj.Status = status.Inactive;
                        if (value == 1)
                        {
                            obj.Status = status.Active;
                        }
                        obj.FirstName = ds.Tables[0].Rows[0].ItemArray[7].ToString();
                        obj.LastName = ds.Tables[0].Rows[0].ItemArray[8].ToString();
                    }

                }
                con.Close();
            
            return View(obj);
        }
        /**End of User Settings**/
        /**Start of Event Types**/
        public ActionResult ManageEventTypes()
        {
            int UserId = Convert.ToInt32(Session["UserId"]);
            if (UserId == 0)
            {
                return RedirectToAction("Index", "Login");
            }
            NewEventList obj = new NewEventList();
            obj.NewEvents = new List<NewEvent>();
            
                con.Open();
                DataSet ds = new DataSet();
                using (SqlCommand cmd = new SqlCommand("dbo.settings", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@param", "ViewEvent");
                    cmd.Parameters.AddWithValue("@Id", 0);
                    cmd.Parameters.AddWithValue("@UserName", "");
                    cmd.Parameters.AddWithValue("@Password", "");
                    cmd.Parameters.AddWithValue("@Email", "");
                    cmd.Parameters.AddWithValue("@Mobile", "");
                    cmd.Parameters.AddWithValue("@SuperUser", "");
                    cmd.Parameters.AddWithValue("@Status", "");
                    cmd.Parameters.AddWithValue("@FirstName", "");
                    cmd.Parameters.AddWithValue("@LastName", "");
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);
                    if (ds.Tables.Count > 0)
                    {
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            NewEvent subobj = new NewEvent();
                            subobj.Id = Convert.ToInt32(ds.Tables[0].Rows[i].ItemArray[0]);
                            subobj.EventType = ds.Tables[0].Rows[i].ItemArray[1].ToString();
                            subobj.ColorCode = ds.Tables[0].Rows[i].ItemArray[2].ToString();
                            obj.NewEvents.Add(subobj);
                        }
                    }
                }
                con.Close();
            
            return View(obj);
        }
        [HttpPost]
        public ActionResult InsertEvent(GS247.Models.Settings.NewEvent data)
        {
            int UserId = Convert.ToInt32(Session["UserId"]);
            if (UserId == 0)
            {
                return RedirectToAction("Index", "Login");
            }
                con.Open();
                DataSet ds = new DataSet();
                using (SqlCommand cmd = new SqlCommand("dbo.settings", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    String str = "InsertEvent";
                    int val = 0;
                    if (data.Id > 0)
                    {
                        str = "UpdateEvent";
                        val = data.Id;
                    }
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@param", str);
                    cmd.Parameters.AddWithValue("@Id", data.Id);
                    cmd.Parameters.AddWithValue("@UserName", data.EventType);
                    cmd.Parameters.AddWithValue("@Password", data.ColorCode);
                    cmd.Parameters.AddWithValue("@Email", "");
                    cmd.Parameters.AddWithValue("@Mobile", "");
                    cmd.Parameters.AddWithValue("@SuperUser", "");
                    cmd.Parameters.AddWithValue("@Status", "");
                    cmd.Parameters.AddWithValue("@FirstName", "");
                    cmd.Parameters.AddWithValue("@LastName", "");
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);

                }
                con.Close();
            

            return RedirectToAction("ManageEventTypes", "Settings");
        }


        public ActionResult DeleteEvent(int Id)
        {
            int UserId = Convert.ToInt32(Session["UserId"]);
            if (UserId == 0)
            {
                return RedirectToAction("Index", "Login");
            }
           
                con.Open();
                DataSet ds = new DataSet();
                using (SqlCommand cmd = new SqlCommand("dbo.settings", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@param", "DeleteEvent");
                    cmd.Parameters.AddWithValue("@Id", Id);
                    cmd.Parameters.AddWithValue("@UserName", "");
                    cmd.Parameters.AddWithValue("@Password", "");
                    cmd.Parameters.AddWithValue("@Email", "");
                    cmd.Parameters.AddWithValue("@Mobile", "");
                    cmd.Parameters.AddWithValue("@SuperUser", "");
                    cmd.Parameters.AddWithValue("@Status", "");
                    cmd.Parameters.AddWithValue("@FirstName", "");
                    cmd.Parameters.AddWithValue("@LastName", "");
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);

                }
                con.Close();
            
            return RedirectToAction("ManageEventTypes", "Settings");
        }
        public ActionResult AddEvent()
        {
            int UserId = Convert.ToInt32(Session["UserId"]);
            if (UserId == 0)
            {
                return RedirectToAction("Index", "Login");
            }
            NewEvent obj = new NewEvent();
            return View(obj);
        }
        public ActionResult UpdateEvent(int Id)
        {
            int UserId = Convert.ToInt32(Session["UserId"]);
            if (UserId == 0)
            {
                return RedirectToAction("Index", "Login");
            }
            NewEvent obj = new NewEvent();
            
                con.Open();
                DataSet ds = new DataSet();
                using (SqlCommand cmd = new SqlCommand("dbo.settings", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@param", "GetEvent");
                    cmd.Parameters.AddWithValue("@Id", Id);
                    cmd.Parameters.AddWithValue("@UserName", "");
                    cmd.Parameters.AddWithValue("@Password", "");
                    cmd.Parameters.AddWithValue("@Email", "");
                    cmd.Parameters.AddWithValue("@Mobile", "");
                    cmd.Parameters.AddWithValue("@SuperUser", "");
                    cmd.Parameters.AddWithValue("@Status", "");
                    cmd.Parameters.AddWithValue("@FirstName", "");
                    cmd.Parameters.AddWithValue("@LastName", "");
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);
                    if (ds.Tables.Count > 0)
                    {
                        obj.Id = Id;
                        obj.EventType = ds.Tables[0].Rows[0].ItemArray[1].ToString();
                        obj.ColorCode = ds.Tables[0].Rows[0].ItemArray[2].ToString();
                    }

                }
                con.Close();
            return View(obj);
        }
        /**End of Event Types**/
        /**Start of Dashboard Blocks**/
        public ActionResult ManageDashboard()
        {
            int UserId = Convert.ToInt32(Session["UserId"]);
            if (UserId == 0)
            {
                return RedirectToAction("Index", "Login");
            }
            DashboardList obj = new DashboardList();
            obj.Dashboards = new List<Dashboard>();
            
                con.Open();
                SqlDataReader reader = null;
                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.CommandType = CommandType.Text;
                sqlCmd.CommandText = "select Id,Name,BlockOrder,IsEnable from dbo.settingsdashboard order by Id asc";
                sqlCmd.Connection = con;
                reader = sqlCmd.ExecuteReader();
                Dashboard subobj=new Dashboard();
                int switchBut=0;
                while (reader.Read())
                {
                    subobj = new Dashboard();
                    subobj.Id = Convert.ToInt32(reader.GetValue(0));
                    subobj.Name = reader.GetValue(1).ToString(); ;
                    subobj.BlockOrder = Convert.ToInt32(reader.GetValue(2));
                    switchBut = Convert.ToInt32(reader.GetValue(3));
                    if (switchBut == 1)
                    {
                        subobj.isEnable = Enable.Disable;
                    }
                        obj.Dashboards.Add(subobj);
                }
                con.Close();
            return View(obj);
        }
        public ActionResult Switch(int Id, int val)
        {
            int UserId = Convert.ToInt32(Session["UserId"]);
            if (UserId == 0)
            {
                return RedirectToAction("Index", "Login");
            }
            con.Open();
                SqlDataReader reader = null;
                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.CommandType = CommandType.Text;
                sqlCmd.CommandText = "Update dbo.settingsdashboard Set IsEnable=@IsEnable where Id=@Id";
                sqlCmd.Connection = con;
                sqlCmd.Parameters.AddWithValue("@IsEnable", val);
                sqlCmd.Parameters.AddWithValue("@Id", Id);
                reader = sqlCmd.ExecuteReader();
                con.Close();
            

            return RedirectToAction("ManageDashboard", "Settings");
        }
        public ActionResult EnableAll(int val)
        {
            int UserId = Convert.ToInt32(Session["UserId"]);
            if (UserId == 0)
            {
                return RedirectToAction("Index", "Login");
            }
            con.Open();
                int value = 0;
                if (val == 1) { value = 1; }
                SqlDataReader reader = null;
                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.CommandType = CommandType.Text;
                if (val == 2)
                {
                    sqlCmd.CommandText = "Update dbo.settingsdashboard Set BlockOrder=Id";
                }
                else {
                    sqlCmd.CommandText = "Update dbo.settingsdashboard Set IsEnable=@IsEnable";
                }
                
                sqlCmd.Connection = con;
                sqlCmd.Parameters.AddWithValue("@IsEnable", value);
                reader = sqlCmd.ExecuteReader();
                con.Close();
            
            return RedirectToAction("ManageDashboard", "Settings");
        }
        /**End Of Dashboard**/
        /**Start Of School Setup**/
        public ActionResult SchoolSetup()
        {
            int UserId = Convert.ToInt32(Session["UserId"]);
            if (UserId == 0)
            {
                return RedirectToAction("Index", "Login");
            }
            SchoolSetup obj = new SchoolSetup();
            
                con.Open();
                DataSet ds = new DataSet();
                using (SqlCommand cmd = new SqlCommand("dbo.SettingsSchoolProc", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@param", "ViewSchoolSetup");
                    cmd.Parameters.AddWithValue("@Id", 0);
                    cmd.Parameters.AddWithValue("@Name", "");
                    cmd.Parameters.AddWithValue("@RegistrationId", "");
                    cmd.Parameters.AddWithValue("@FoundedOn", "");
                    cmd.Parameters.AddWithValue("@Curriculum", "");
                    cmd.Parameters.AddWithValue("@Address", "");
                    cmd.Parameters.AddWithValue("@Pincode", "");
                    cmd.Parameters.AddWithValue("@Phone", "");
                    cmd.Parameters.AddWithValue("@AlternatePhone", "");
                    cmd.Parameters.AddWithValue("@Email", "");
                    cmd.Parameters.AddWithValue("@Fax", "");
                    cmd.Parameters.AddWithValue("@PrincipalName", "");
                    cmd.Parameters.AddWithValue("@PrincipalEmail", "");
                    cmd.Parameters.AddWithValue("@PrincipalPhone", "");
                    cmd.Parameters.AddWithValue("@PrincipalMobile", "");
                    cmd.Parameters.AddWithValue("@CurrentAcademicYear", "");
                    cmd.Parameters.AddWithValue("@FinanceYearStart", "");
                    cmd.Parameters.AddWithValue("@FinanceYearEnd", "");
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        obj.Id = Convert.ToInt32(ds.Tables[0].Rows[0].ItemArray[0]);
                        obj.Name = ds.Tables[0].Rows[0].ItemArray[1].ToString();
                        obj.RegistrationId = ds.Tables[0].Rows[0].ItemArray[2].ToString();
                        obj.FoundedOn = ds.Tables[0].Rows[0].ItemArray[3].ToString();
                        obj.Curriculum = ds.Tables[0].Rows[0].ItemArray[4].ToString();
                        obj.Address = ds.Tables[0].Rows[0].ItemArray[5].ToString();
                        obj.Pincode = ds.Tables[0].Rows[0].ItemArray[6].ToString();
                        obj.Phone = ds.Tables[0].Rows[0].ItemArray[7].ToString();
                        obj.AlternatePhone = ds.Tables[0].Rows[0].ItemArray[8].ToString();
                        obj.Email = ds.Tables[0].Rows[0].ItemArray[9].ToString();
                        obj.Fax = ds.Tables[0].Rows[0].ItemArray[10].ToString();
                        obj.PrincipalName = ds.Tables[0].Rows[0].ItemArray[11].ToString();
                        obj.PrincipalEmail = ds.Tables[0].Rows[0].ItemArray[12].ToString();
                        obj.PrincipalPhone = ds.Tables[0].Rows[0].ItemArray[13].ToString();
                        obj.PrincipalMobile = ds.Tables[0].Rows[0].ItemArray[14].ToString();
                        int CurrentYear = 1;
                        obj.FinanceYearStart = ds.Tables[0].Rows[0].ItemArray[16].ToString();
                        obj.FinanceYearEnd = ds.Tables[0].Rows[0].ItemArray[17].ToString();

                    }
                }
                ds = new DataSet();
                using (SqlCommand cmd = new SqlCommand("dbo.CourseSettings", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@param", "ViewYear");
                    cmd.Parameters.AddWithValue("@Id", 0);
                    cmd.Parameters.AddWithValue("@Name", "");
                    cmd.Parameters.AddWithValue("@StartsOn", "");
                    cmd.Parameters.AddWithValue("@EndsOn", "");
                    cmd.Parameters.AddWithValue("@Description", "");
                    cmd.Parameters.AddWithValue("@Status", 0);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        obj.AcademicYears = new List<AcademicYear>();
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            AcademicYear subobj = new AcademicYear();
                            subobj.Id = Convert.ToInt32(ds.Tables[0].Rows[i].ItemArray[0]);
                            subobj.Name = ds.Tables[0].Rows[i].ItemArray[1].ToString();
                            int stat = Convert.ToInt32(ds.Tables[0].Rows[i].ItemArray[5]);
                            if (stat == 1)
                            {
                                obj.AcademicYears.Add(subobj);
                            }
                        }
                    }

                }
                con.Close();
            
            return View(obj);
        }
        [HttpPost]
        public ActionResult UpdateSetup(GS247.Models.Settings.SchoolSetup data)
        {
            int UserId = Convert.ToInt32(Session["UserId"]);
            if (UserId == 0)
            {
                return RedirectToAction("Index", "Login");
            }
            con.Open();
                DataSet ds = new DataSet();
                using (SqlCommand cmd = new SqlCommand("dbo.SettingsSchoolProc", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    String Str = "InsertSchoolSetup";
                    if (data.Id > 0)
                    { Str = "UpdateSchoolSetup"; }
                    cmd.Parameters.AddWithValue("@param", Str);
                    cmd.Parameters.AddWithValue("@Id", data.Id);
                    String Name = "";
                    if (data.Name != null)
                    { Name = data.Name; }
                    cmd.Parameters.AddWithValue("@Name", Name);
                    String RegId = "";
                    if (data.RegistrationId != null)
                    { RegId = data.RegistrationId; }
                    cmd.Parameters.AddWithValue("@RegistrationId", RegId);
                    String Founded = "";
                    if (data.FoundedOn != null)
                    { Founded = data.FoundedOn; }
                    cmd.Parameters.AddWithValue("@FoundedOn", Founded);
                    String Curriculam = "";
                    if (data.Curriculum != null)
                    { Curriculam = data.Curriculum; }
                    cmd.Parameters.AddWithValue("@Curriculum", Curriculam);
                    String Address = "";
                    if (data.Address != null)
                    { Address = data.Address; }
                    cmd.Parameters.AddWithValue("@Address", Address);
                    String Pincode = "";
                    if (data.Pincode != null)
                    { Pincode = data.Pincode; }
                    cmd.Parameters.AddWithValue("@Pincode", Pincode);
                    String Phone = "";
                    if (data.Phone != null)
                    { Phone = data.Phone; }
                    cmd.Parameters.AddWithValue("@Phone", Phone);
                    String AltPhone = "";
                    if (data.AlternatePhone != null)
                    { AltPhone = data.AlternatePhone; }
                    cmd.Parameters.AddWithValue("@AlternatePhone", AltPhone);
                    String Email = "";
                    if (data.Email != null)
                    { Email = data.Email; }
                    cmd.Parameters.AddWithValue("@Email", Email);
                    String Fax = "";
                    if (data.Fax != null)
                    { Fax = data.Fax; }
                    cmd.Parameters.AddWithValue("@Fax", Fax);
                    String PrincName = "";
                    if (data.PrincipalName != null)
                    { PrincName = data.PrincipalName; }
                    cmd.Parameters.AddWithValue("@PrincipalName", PrincName);
                    String PrincEmail = "";
                    if (data.PrincipalEmail != null)
                    { PrincEmail = data.PrincipalEmail; }
                    cmd.Parameters.AddWithValue("@PrincipalEmail", PrincEmail);
                    String PrincPhone = "";
                    if (data.PrincipalPhone != null)
                    { PrincPhone = data.PrincipalPhone; }
                    cmd.Parameters.AddWithValue("@PrincipalPhone", PrincPhone);
                    String PrincMobile = "";
                    if (data.PrincipalMobile != null)
                    { PrincMobile = data.PrincipalMobile; }
                    cmd.Parameters.AddWithValue("@PrincipalMobile", PrincMobile);
                    cmd.Parameters.AddWithValue("@CurrentAcademicYear", 1);
                    String FinYearStart = "";
                    if (data.FinanceYearStart != null)
                    { FinYearStart = data.FinanceYearStart; }
                    cmd.Parameters.AddWithValue("@FinanceYearStart", FinYearStart);
                    String FinYearEnd = "";
                    if (data.FinanceYearEnd != null)
                    { FinYearEnd = data.FinanceYearEnd; }
                    cmd.Parameters.AddWithValue("@FinanceYearEnd", FinYearEnd);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);

                }
                con.Close();
            
            return RedirectToAction("SchoolSetup", "Settings");
        }


        /**End of School Setup**/
        /**Start of Weekdays**/
        public ActionResult WeekDaysSetup()
        {
            int UserId = Convert.ToInt32(Session["UserId"]);
            if (UserId == 0)
            {
                return RedirectToAction("Index", "Login");
            }
            DefaultWeekdaysList obj = new DefaultWeekdaysList();
            obj.Weekdays = new List<DefaultWeekdays>();

            con.Open();
            SqlDataReader reader = null;
            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandType = CommandType.Text;
            sqlCmd.CommandText = "select Id,WeekDay,IsEnable from dbo.SettingsDefaultWeekdays";
            sqlCmd.Connection = con;
            reader = sqlCmd.ExecuteReader();
            while(reader.Read())
            {
                DefaultWeekdays subobj = new DefaultWeekdays();
                subobj.Id = Convert.ToInt32(reader.GetValue(0));
                subobj.Name = reader.GetValue(1).ToString();
                int val = Convert.ToInt32(reader.GetValue(2));
                subobj.IsEnable = false;
                if (val == 1)
                {
                    subobj.IsEnable = true;
                }
                obj.Weekdays.Add(subobj);
            }
            con.Close();
            return View(obj);
        }
        [HttpPost]
        public ActionResult UpdateWeekDays(DefaultWeekdaysList Data)
        {
            int UserId = Convert.ToInt32(Session["UserId"]);
            if (UserId == 0)
            {
                return RedirectToAction("Index", "Login");
            }

            con.Open();
            SqlDataReader reader=null;
            SqlCommand sqlCmd = new SqlCommand();
            for (int i = 0; i < Data.Weekdays.Count; i++)
            {
                int IsEnable = 0;
                if (Data.Weekdays[i].IsEnable == true)
                {
                    IsEnable = 1;
                }
                reader = null;
                sqlCmd = new SqlCommand();
                sqlCmd.CommandType = CommandType.Text;
                sqlCmd.CommandText = "Update dbo.SettingsDefaultWeekdays Set IsEnable=@IsEnable where Id=@Id";
                sqlCmd.Connection = con;
                sqlCmd.Parameters.AddWithValue("@Id", Data.Weekdays[i].Id);
                sqlCmd.Parameters.AddWithValue("@IsEnable", IsEnable);
                reader = sqlCmd.ExecuteReader();
                
            }
            return RedirectToAction("WeekDaysSetup", "Settings");
        }
        /**End of Weekdays**/
        /**Start of Manage Features**/
        public ActionResult ManageFeatures()
        {
            int UserId = Convert.ToInt32(Session["UserId"]);
            if (UserId == 0)
            {
                return RedirectToAction("Index", "Login");
            }
            Feature obj = new Feature();
            
                con.Open();
                DataSet ds = new DataSet();
                SqlDataReader reader = null;
                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.CommandType = CommandType.Text;
                sqlCmd.CommandText = "select top 1 Id,IsAchievements,IsComplaints from dbo.SettingsFeatures order by Id asc";
                sqlCmd.Connection = con;
                reader = sqlCmd.ExecuteReader();
                if (reader.Read())
                {
                    obj.Id = Convert.ToInt32(reader.GetValue(0));
                    int IsAchievements = Convert.ToInt32(reader.GetValue(1));
                    int IsComplaints = Convert.ToInt32(reader.GetValue(2));
                    obj.IsAchievements = false;
                    if (IsAchievements == 1)
                    { obj.IsAchievements = true; }
                    obj.IsComplaints = false;
                    if (IsComplaints == 1)
                    { obj.IsComplaints = true; }
                }
                con.Close();
            
            return View(obj);
        }
        [HttpPost]
        public ActionResult UpdateFeatures(Feature data)
        {
            int UserId = Convert.ToInt32(Session["UserId"]);
            if (UserId == 0)
            {
                return RedirectToAction("Index", "Login");
            }
            con.Open();
                DataSet ds = new DataSet();
                SqlDataReader reader = null;
                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.CommandType = CommandType.Text;
                if (data.Id > 0)
                {
                    sqlCmd.CommandText = "UPDATE dbo.SettingsFeatures SET IsAchievements=@IsAchieve,IsComplaints=@IsComplaint Where Id=@Id ";
                }
                else
                {
                    sqlCmd.CommandText = "INSERT INTO dbo.SettingsFeatures(IsAchievements,IsComplaints) VALUES(@IsAchieve,@IsComplaint)";
                }

                int IsAchieve = 0, IsComplaint = 0;
                if(data.IsAchievements==true)
                { IsAchieve = 1; }
                if (data.IsComplaints == true)
                { IsComplaint = 1; }
                sqlCmd.Parameters.AddWithValue("@IsAchieve", IsAchieve);
                sqlCmd.Parameters.AddWithValue("@IsComplaint", IsComplaint);
                sqlCmd.Parameters.AddWithValue("@Id", data.Id);
                sqlCmd.Connection = con;
                reader = sqlCmd.ExecuteReader();               
                con.Close();
            
            return RedirectToAction("ManageFeatures","Settings");
        }
        /**End Of Manage Features**/
        /**Start of Manage Class Timings**/
        public ActionResult ManageClassTimings()
        {
            int UserId = Convert.ToInt32(Session["UserId"]);
            if (UserId == 0)
            {
                return RedirectToAction("Index", "Login");
            }
            ClassTimingsList obj = new ClassTimingsList();
            obj.ClassTimings = new List<ClassTimingDiff>();
            
                con.Open();
                SqlDataReader reader = null;
                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.CommandType = CommandType.Text;
                sqlCmd.CommandText = "Select Id,Name,StartTime,EndTime,IsBreak,IsAllBatch from dbo.settingsCommonclasstimings";
                sqlCmd.Connection = con;
                reader = sqlCmd.ExecuteReader();
                if (reader.Read())
                {
                    ClassTimingDiff subobj = new ClassTimingDiff();
                    subobj.Id = Convert.ToInt32(reader.GetValue(0));
                    subobj.Name = reader.GetValue(1).ToString();
                    subobj.StartTime = reader.GetValue(2).ToString();
                    subobj.EndTime = reader.GetValue(3).ToString();
                    subobj.IsBreak = Break.No;
                    int val = Convert.ToInt32(reader.GetValue(4));
                    if (val == 1)
                    {
                        subobj.IsBreak = Break.Yes;
                    }
                    subobj.IsAllBatch = false;
                    int val1 = Convert.ToInt32(reader.GetValue(5));
                    if (val1 == 1)
                    {
                        subobj.IsAllBatch = true;
                    }
                    obj.ClassTimings.Add(subobj);
                }
                con.Close();
            
            return View(obj);
        }

        [HttpPost]
        public ActionResult InsertClassTimings(ClassTiming Data)
        {
            int UserId = Convert.ToInt32(Session["UserId"]);
            if (UserId == 0)
            {
                return RedirectToAction("Index", "Login");
            }
            
                con.Open();
                SqlDataReader reader = null;
                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.CommandType = CommandType.Text;
                if (Data.Id > 0)
                {
                    sqlCmd.CommandText = "UPDATE dbo.settingsCommonclasstimings SET Name=@Name,StartTime=@StartTime,EndTime=@EndTime,IsBreak=@IsBreak,IsAllBatch=@IsAllBatch where Id=@Id";
                }
                else
                {
                    sqlCmd.CommandText = "Insert into dbo.settingsCommonclasstimings (Name,StartTime,EndTime,IsBreak,IsAllBatch) values (@Name,@StartTime,@EndTime,@IsBreak,@IsAllBatch)";
                }
                sqlCmd.Connection = con;
                sqlCmd.Parameters.AddWithValue("@Id", Data.Id);
                sqlCmd.Parameters.AddWithValue("@Name", Data.Name);
                sqlCmd.Parameters.AddWithValue("@StartTime", Data.StartTime);
                sqlCmd.Parameters.AddWithValue("@EndTime", Data.EndTime);
                sqlCmd.Parameters.AddWithValue("@IsBreak", Data.IsBreak);
                sqlCmd.Parameters.AddWithValue("@IsAllBatch", Data.IsAllBatch);
                reader = sqlCmd.ExecuteReader();
                reader.Read();

                if(Data.Id>0)
                {
                    reader = null;
                    sqlCmd = new SqlCommand();
                    sqlCmd.CommandType = CommandType.Text;
                    sqlCmd.CommandText = "Delete from dbo.SettingsTimingsWeekdaysLink where ClassTimingId=@Id";
                    sqlCmd.Connection = con;
                    sqlCmd.Parameters.AddWithValue("@Id", Data.Id);
                    reader = sqlCmd.ExecuteReader();
                    reader.Read();
                }
                for (int i = 0; i < Data.Weekdays.Count; i++)
                {
                    if (Data.Weekdays[i].IsEnable == true)
                    {
                        reader = null;
                        sqlCmd = new SqlCommand();
                        sqlCmd.CommandType = CommandType.Text;
                        sqlCmd.CommandText = "INSERT INTO dbo.SettingsTimingsWeekdaysLink (ClassTimingId,WeekDaysId) VALUES (@ClassId,@WeekId)";
                        sqlCmd.Connection = con;
                        sqlCmd.Parameters.AddWithValue("@ClassId", Data.Id);
                        sqlCmd.Parameters.AddWithValue("@WeekId", Data.Weekdays[i].Id);
                        reader = sqlCmd.ExecuteReader();
                        reader.Read();
                    }
                }

                con.Close();
            
          return RedirectToAction("ManageClassTimings", "Settings");
        }

        public ActionResult AddClassTimings()
        {
            int UserId = Convert.ToInt32(Session["UserId"]);
            if (UserId == 0)
            {
                return RedirectToAction("Index", "Login");
            }
            ClassTiming obj = new ClassTiming();
            obj.Weekdays = new List<DefaultWeekdays>();
            
                con.Open();
                SqlDataReader reader = null;
                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.CommandType = CommandType.Text;
                sqlCmd.CommandText = "Select Id,WeekDay,IsEnable from dbo.settingsDefaultWeekdays where IsEnable=1";
                sqlCmd.Connection = con;
                reader = sqlCmd.ExecuteReader();
                while(reader.Read())
                {
                    DefaultWeekdays subobj = new DefaultWeekdays();
                    subobj.Id = Convert.ToInt32(reader.GetValue(0));
                    subobj.Name = reader.GetValue(1).ToString();
                    int val = Convert.ToInt32(reader.GetValue(2));
                    subobj.IsEnable = false;
                    obj.Weekdays.Add(subobj);
                }
            
            return View(obj);
        }

        public ActionResult UpdateClassTimings(int Id)
        {
            int UserId = Convert.ToInt32(Session["UserId"]);
            if (UserId == 0)
            {
                return RedirectToAction("Index", "Login");
            }
            ClassTiming obj = new ClassTiming();
            obj.Weekdays = new List<DefaultWeekdays>();
            
                con.Open();
                SqlDataReader reader = null;
                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.CommandType = CommandType.Text;
                sqlCmd.CommandText = "select Name,StartTime,EndTime,IsBreak,IsAllBatch from dbo.SettingsCommonClassTimings where Id=@Id";
                sqlCmd.Connection = con;
                sqlCmd.Parameters.AddWithValue("@Id", Id);
                reader = sqlCmd.ExecuteReader();
                if(reader.Read())
                {
                    obj.Id = Id;
                    obj.Name =reader.GetValue(0).ToString();
                    obj.StartTime = reader.GetValue(1).ToString();
                    obj.EndTime = reader.GetValue(2).ToString();
                    obj.IsBreak = false;
                    int val = Convert.ToInt32(reader.GetValue(3));
                    if (val == 1)
                    { obj.IsBreak = true; }
                    obj.IsAllBatch = false;
                    val = Convert.ToInt32(reader.GetValue(4));
                    if (val == 1)
                    { obj.IsAllBatch = true; }
                }

                reader = null;
                sqlCmd = new SqlCommand();
                sqlCmd.CommandType = CommandType.Text;
                sqlCmd.CommandText = "Select Id,WeekDay,IsEnable from dbo.settingsDefaultWeekdays where IsEnable=1";
                sqlCmd.Connection = con;
                reader = sqlCmd.ExecuteReader();
                while (reader.Read())
                {
                    DefaultWeekdays subobj = new DefaultWeekdays();
                    subobj.Id = Convert.ToInt32(reader.GetValue(0));
                    subobj.Name = reader.GetValue(1).ToString();
                    subobj.IsEnable = false;

                    SqlDataReader reader1 = null;
                    SqlCommand sqlCmd1 = new SqlCommand();
                    sqlCmd1.CommandType = CommandType.Text;
                    sqlCmd1.CommandText = "select count(ClassTimingId) from dbo.SettingsTimingsWeekdaysLink where ClassTimingId=@ClassId and WeekdaysId=@WeekId";
                    sqlCmd1.Connection = con;
                    sqlCmd1.Parameters.AddWithValue("@ClassId", Id);
                    sqlCmd1.Parameters.AddWithValue("@WeekId", subobj.Id);
                    reader1 = sqlCmd1.ExecuteReader();
                    if (reader1.Read())
                    {
                        int count = Convert.ToInt32(reader1.GetValue(0));
                        if (count > 0)
                        {
                            subobj.IsEnable = true;
                        }
                    }
                    obj.Weekdays.Add(subobj);
                }
                con.Close();
            
            
            return View(obj);
        }
        public ActionResult DeleteClassTimings(int Id)
        {
            int UserId = Convert.ToInt32(Session["UserId"]);
            if (UserId == 0)
            {
                return RedirectToAction("Index", "Login");
            }
            
                con.Open();
                SqlDataReader reader = null;
                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.CommandType = CommandType.Text;
                sqlCmd.CommandText = "Delete from dbo.SettingsCommonClassTimings where Id=@Id";
                sqlCmd.Connection = con;
                sqlCmd.Parameters.AddWithValue("@Id", Id);
                reader = sqlCmd.ExecuteReader();
                reader.Read();

                reader = null;
                sqlCmd = new SqlCommand();
                sqlCmd.CommandType = CommandType.Text;
                sqlCmd.CommandText = "Delete from dbo.SettingsTimingsWeekdaysLink where ClassTimingId=@Id";
                sqlCmd.Connection = con;
                sqlCmd.Parameters.AddWithValue("@Id", Id);
                reader = sqlCmd.ExecuteReader();
                reader.Read();

                con.Close();
            
            return RedirectToAction("ManageClassTimings", "Settings");
        }
        /**End of Manage Class Timings**/
        public static string GetConnectionStringByName(string name)
        {
            string returnValue = null;
            ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings[name];
            if (settings!=null)
            {
                returnValue = settings.ConnectionString;
            }
            return returnValue;
        }
        /**Start of Manage Student Document Types**/
        public ActionResult ManageDocumentType()
        {
            int UserId = Convert.ToInt32(Session["UserId"]);
            if (UserId == 0)
            {
                return RedirectToAction("Index", "Login");
            }
            DocumentTypeList obj = new DocumentTypeList();
            obj.DocumentTypes = new List<DocumentType>();
            
                con.Open();
                SqlDataReader reader = null;
                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.CommandType = CommandType.Text;
                sqlCmd.CommandText = "select Id,DocumentName,IsMandatory,IsRequired from dbo.SettingsDocumentType order by Id asc";
                sqlCmd.Connection = con;
                reader = sqlCmd.ExecuteReader();
                while (reader.Read())
                {
                    DocumentType subobj = new DocumentType();
                    subobj.Id = Convert.ToInt32(reader.GetValue(0));
                    subobj.Name = reader.GetValue(1).ToString();
                    int val = Convert.ToInt32(reader.GetValue(2));
                    if (val == 1)
                    {
                        subobj.Mandatory = Mandatory.No;
                    }
                    else if (val == 2)
                    {
                        subobj.Mandatory = Mandatory.YesCan;
                    }
                    else
                    {
                        subobj.Mandatory = Mandatory.YesCannot;
                    }
                    subobj.IsRequired = Break.No;
                    val = Convert.ToInt32(reader.GetValue(3));
                    if (val == 1)
                    {
                        subobj.IsRequired = Break.Yes;
                    }
                    obj.DocumentTypes.Add(subobj);
                }
                con.Close();
            
            return View(obj);
        }
        [HttpPost]
        public ActionResult InsertDocumentType(DocumentTypeAdd Data)
        {
            int UserId = Convert.ToInt32(Session["UserId"]);
            if (UserId == 0)
            {
                return RedirectToAction("Index", "Login");
            }
           
                con.Open();
                SqlDataReader reader = null;
                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.CommandType = CommandType.Text;
                if(Data.Id>0)
                {
                    sqlCmd.CommandText = "Update dbo.SettingsDocumentType Set DocumentName=@DocumentName,IsMandatory=@IsMandatory,IsRequired=@IsRequired Where Id=@Id";
                }
                else
                {
                    sqlCmd.CommandText = "Insert dbo.SettingsDocumentType (DocumentName,IsMandatory,IsRequired) Values (@DocumentName,@IsMandatory,@IsRequired)";
                }
                sqlCmd.Parameters.AddWithValue("@Id", Data.Id);
                sqlCmd.Parameters.AddWithValue("@DocumentName", Data.Name);
                sqlCmd.Parameters.AddWithValue("@IsMandatory", Data.Mandatory);
                int val = 0;
                if(Data.IsRequired==true)
                { val = 1; }
                sqlCmd.Parameters.AddWithValue("@IsRequired", val);
                sqlCmd.Connection = con;
                reader = sqlCmd.ExecuteReader();
                reader.Read();
                con.Close();
            
            return RedirectToAction("ManageDocumentType", "Settings");
        }
        public ActionResult AddDocumentType()
        {
            int UserId = Convert.ToInt32(Session["UserId"]);
            if (UserId == 0)
            {
                return RedirectToAction("Index", "Login");
            }
            DocumentTypeAdd obj = new DocumentTypeAdd();
            return View(obj);
        }
        public ActionResult UpdateDocumentType(int Id)
        {
            int UserId = Convert.ToInt32(Session["UserId"]);
            if (UserId == 0)
            {
                return RedirectToAction("Index", "Login");
            }
            DocumentTypeAdd obj = new DocumentTypeAdd();
            
                con.Open();
                SqlDataReader reader = null;
                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.CommandType = CommandType.Text;
                sqlCmd.CommandText = "select Id,DocumentName,IsMandatory,IsRequired from dbo.SettingsDocumentType where Id=@Id";
                sqlCmd.Connection = con;
                sqlCmd.Parameters.AddWithValue("@Id", Id);
                reader = sqlCmd.ExecuteReader();
                reader.Read();
                obj.Id = Convert.ToInt32(reader.GetValue(0));
                obj.Name = reader.GetValue(1).ToString();
                obj.Mandatory = Convert.ToInt32(reader.GetValue(2));
                obj.IsRequired = false;
                int val = Convert.ToInt32(reader.GetValue(3));
                if (val == 1)
                {
                    obj.IsRequired = true;
                }
                con.Close();
            
            return View(obj);
        }
        public ActionResult DeleteDocumentType(int Id)
        {
            int UserId = Convert.ToInt32(Session["UserId"]);
            if (UserId == 0)
            {
                return RedirectToAction("Index", "Login");
            }
            con.Open();
                SqlDataReader reader = null;
                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.CommandType = CommandType.Text;
                sqlCmd.CommandText = "delete from dbo.SettingsDocumentType where Id=@Id";
                sqlCmd.Parameters.AddWithValue("@Id", Id);
                sqlCmd.Connection = con;
                reader = sqlCmd.ExecuteReader();
                con.Close();
            
            return RedirectToAction("ManageDocumentType", "Settings");
        }
        /**End of Manage Document Types**/
        /**Start of Online Registration Settings**/
        public ActionResult ManageRegistration()
        {
            int UserId = Convert.ToInt32(Session["UserId"]);
            if (UserId == 0)
            {
                return RedirectToAction("Index", "Login");
            }
            ManageRegistration obj = new ManageRegistration();
            
                con.Open();
                SqlDataReader reader = null;
                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.CommandType = CommandType.Text;
                sqlCmd.CommandText = "select Id, Name from dbo.Years where Status = 1";
                sqlCmd.Connection = con;
                reader = sqlCmd.ExecuteReader();
                if(reader.Read())
                {
                    obj.AcademicYearId = Convert.ToInt32(reader.GetValue(0));
                    obj.AcademicYear = reader.GetValue(1).ToString();
                }
                
                 reader = null;
                 sqlCmd = new SqlCommand();
                sqlCmd.CommandType = CommandType.Text;
                sqlCmd.CommandText = "select Id,AcademicYearId,AllowOnlineAdmission,ShowLink from dbo.SettingsOnlineRegistration where Id=1";
                sqlCmd.Connection = con;
                reader = sqlCmd.ExecuteReader();
                if (reader.Read())
                {
                    obj.Id = Convert.ToInt32(reader.GetValue(0));
                    int val = Convert.ToInt32(reader.GetValue(2));
                    obj.AllowOnlineAdmission = false;
                    if (val == 1)
                    { obj.AllowOnlineAdmission = true; }
                    val = Convert.ToInt32(reader.GetValue(3));
                    obj.ShowLink = false;
                    if (val == 1)
                    { obj.ShowLink = true; }
                }
                con.Close();
            
            return View(obj);
        }
        [HttpPost]
        public ActionResult UpdateRegistration(ManageRegistration Data)
        {
            int UserId = Convert.ToInt32(Session["UserId"]);
            if (UserId == 0)
            {
                return RedirectToAction("Index", "Login");
            }
            
                con.Open();
                SqlDataReader reader = null;
                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.CommandType = CommandType.Text;
                sqlCmd.CommandText = "Update dbo.SettingsOnlineRegistration Set AcademicYearId=@YearId,AllowOnlineAdmission=@Allow,ShowLink=@Show where Id=1";
                sqlCmd.Connection = con;
                sqlCmd.Parameters.AddWithValue("@YearId", Data.AcademicYearId);
                int val = 0;
                if(Data.AllowOnlineAdmission==true)
                { val = 1; }
                sqlCmd.Parameters.AddWithValue("@Allow", val);
                val = 0;
                if (Data.ShowLink == true)
                { val = 1; }
                sqlCmd.Parameters.AddWithValue("@Show", val);
                reader = sqlCmd.ExecuteReader();
                reader.Read();
                con.Close();
            
            return RedirectToAction("ManageRegistration", "Settings");
        }
        /**End of Online Registration Settings**/
        /**Start of Default Grading Levels**/
        public ActionResult ManageGradingLevels()
        {
            int UserId = Convert.ToInt32(Session["UserId"]);
            if (UserId == 0)
            {
                return RedirectToAction("Index", "Login");
            }
            DefaultGradingList obj = new DefaultGradingList();
            obj.Grades = new List<DefaultGrading>();

            con.Open();
            SqlDataReader reader = null;
            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandType = CommandType.Text;
            sqlCmd.CommandText = "select Id,Name,MinScore from dbo.SettingsDefaultGrading";
            sqlCmd.Connection = con;
            reader = sqlCmd.ExecuteReader();
            while (reader.Read())
            {
                DefaultGrading subobj = new DefaultGrading();
                subobj.Id = Convert.ToInt32(reader.GetValue(0));
                subobj.Name = reader.GetValue(1).ToString();
                subobj.MinScore = Convert.ToInt32(reader.GetValue(2));
                obj.Grades.Add(subobj);
            }
            con.Close();

            return View(obj);
        }
        [HttpPost]
        public ActionResult InsertGrading(DefaultGrading Data)
        {
            int UserId = Convert.ToInt32(Session["UserId"]);
            if (UserId == 0)
            {
                return RedirectToAction("Index", "Login");
            }

            con.Open();
            SqlDataReader reader = null;
            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandType = CommandType.Text;
            if(Data.Id>0)
            {
                sqlCmd.CommandText = "Update dbo.SettingsDefaultGrading Set Name=@Name,MinScore=@MinScore where Id=@Id";
            }
            else
            {
                sqlCmd.CommandText = "Insert into dbo.SettingsDefaultGrading (Name,MinScore) Values (@Name,@MinScore)";
            }
            sqlCmd.Parameters.AddWithValue("@Id", Data.Id);
            sqlCmd.Parameters.AddWithValue("@Name", Data.Name);
            sqlCmd.Parameters.AddWithValue("@MinScore", Data.MinScore);
            sqlCmd.Connection = con;
            reader = sqlCmd.ExecuteReader();
            reader.Read();
            con.Close();            
            return RedirectToAction("ManageGradingLevels", "Settings");
        }
        [HttpPost]
        public ActionResult DeleteGrading(int Id)
        {
            int UserId = Convert.ToInt32(Session["UserId"]);
            if (UserId == 0)
            {
                return RedirectToAction("Index", "Login");
            }

            con.Open();
            SqlDataReader reader = null;
            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandType = CommandType.Text;
            sqlCmd.CommandText = "Delete from dbo.SettingsDefaultGrading where Id=@Id";
            sqlCmd.Parameters.AddWithValue("@Id", Id);
            sqlCmd.Connection = con;
            reader = sqlCmd.ExecuteReader();
            reader.Read();
            con.Close();

            return RedirectToAction("ManageGradingLevels", "Settings");
        }
        public ActionResult AddGrading()
        {
            int UserId = Convert.ToInt32(Session["UserId"]);
            if (UserId == 0)
            {
                return RedirectToAction("Index", "Login");
            }
            DefaultGrading obj = new DefaultGrading();

            return View(obj);
        }
        public ActionResult UpdateGrading(int Id)
        {
            int UserId = Convert.ToInt32(Session["UserId"]);
            if (UserId == 0)
            {
                return RedirectToAction("Index", "Login");
            }
            DefaultGrading obj = new DefaultGrading();

            con.Open();
            SqlDataReader reader = null;
            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandType = CommandType.Text;
            sqlCmd.CommandText = "Select Name,MinScore from dbo.SettingsDefaultGrading where Id=@Id";
            sqlCmd.Parameters.AddWithValue("@Id", Id);
            sqlCmd.Connection = con;
            reader = sqlCmd.ExecuteReader();
            if(reader.Read())
            {
                obj.Id = Id;
                obj.Name = reader.GetValue(0).ToString();
                obj.MinScore = Convert.ToInt32(reader.GetValue(1));
            }
            con.Close();
            return View(obj);
        }
        /**End of Online Registration Settings**/
        /**Start of Exam Format**/
        public ActionResult ManageExamFormat()
        {
            int UserId = Convert.ToInt32(Session["UserId"]);
            if (UserId == 0)
            {
                return RedirectToAction("Index", "Login");
            }
            ExamFormat obj = new ExamFormat();

            con.Open();
            SqlDataReader reader = null;
            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandType = CommandType.Text;
            sqlCmd.CommandText = "select Id,IsLevel,SchoolLevelType from dbo.SettingsExamFormat where Id=1";
            sqlCmd.Connection = con;
            reader = sqlCmd.ExecuteReader();
            if (reader.Read())
            {
                obj.Id = Convert.ToInt32(reader.GetValue(0));
                int val = Convert.ToInt32(reader.GetValue(1));
                obj.IsLevel = val;
                val = Convert.ToInt32(reader.GetValue(2));
                obj.IsFormat = val;

            }
            con.Close();

            return View(obj);
        }
        [HttpPost]
        public ActionResult UpdateExamFormat(ExamFormat Data)
        {
            int UserId = Convert.ToInt32(Session["UserId"]);
            if (UserId == 0)
            {
                return RedirectToAction("Index", "Login");
            }

            con.Open();
            SqlDataReader reader = null;
            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandType = CommandType.Text;
            sqlCmd.CommandText = "Update dbo.SettingsExamFormat Set IsLevel=@IsLevel,SchoolLevelType=@IsType where Id=1";
            sqlCmd.Connection = con;
            sqlCmd.Parameters.AddWithValue("@IsLevel", Data.IsLevel);
            sqlCmd.Parameters.AddWithValue("@IsType", Data.IsFormat);
            reader = sqlCmd.ExecuteReader();
            reader.Read();
            con.Close();            
            return RedirectToAction("ManageExamFormat", "Settings");
        }
        /**End of Exam Format**/
        /**Start of Manage Time Table**/
        public ActionResult ManageTimeTable()
        {
            int UserId = Convert.ToInt32(Session["UserId"]);
            if (UserId == 0)
            {
                return RedirectToAction("Index", "Login");
            }
            ExamFormat obj = new ExamFormat();

            con.Open();
            SqlDataReader reader = null;
            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandType = CommandType.Text;
            sqlCmd.CommandText = "select Id,IsLevel,IsLevelType from dbo.SettingsTimeTableFormat where Id=1";
            sqlCmd.Connection = con;
            reader = sqlCmd.ExecuteReader();
            if(reader.Read())
            {
                obj.Id = Convert.ToInt32(reader.GetValue(0));
                int val = Convert.ToInt32(reader.GetValue(1));
                obj.IsLevel = val;
                val = Convert.ToInt32(reader.GetValue(2));
                obj.IsFormat = val;
                
            }
            con.Close();

            return View(obj);
        }
        [HttpPost]
        public ActionResult UpdateTimeTable(ExamFormat Data)
        {
            int UserId = Convert.ToInt32(Session["UserId"]);
            if (UserId == 0)
            {
                return RedirectToAction("Index", "Login");
            }

            con.Open();
            SqlDataReader reader = null;
            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandType = CommandType.Text;
            sqlCmd.CommandText = "Update dbo.SettingsTimeTableFormat Set IsLevel=@IsLevel,IsLevelType=@IsLevelType where Id=1";
            sqlCmd.Parameters.AddWithValue("@IsLevel", Data.IsLevel);
            sqlCmd.Parameters.AddWithValue("@IsLevelType", Data.IsFormat);
            sqlCmd.Connection = con;
            reader = sqlCmd.ExecuteReader();
            reader.Read();
            return RedirectToAction("ManageTimeTable", "Settings");
        }
        /**End Of TimeTable Format**/

        /**Start of SMTP Settings**/
        public ActionResult ManageSMTP()
        {
            int UserId = Convert.ToInt32(Session["UserId"]);
            if (UserId == 0)
            {
                return RedirectToAction("Index", "Login");
            }
            SMTP obj = new SMTP();
            con.Open();
            SqlDataReader reader = null;
            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandType = CommandType.Text;
            sqlCmd.Connection = con;
            sqlCmd.CommandText = "Select Id, Host, UserName, Password, Port, ConnectionSecurity, EnableSMTP from dbo.SettingsSMTP where Id=1";
            reader = sqlCmd.ExecuteReader();
            if(reader.Read())
            {
                obj.Id = Convert.ToInt32(reader.GetValue(0));
                obj.Host = reader.GetValue(1).ToString();
                obj.UserName = reader.GetValue(2).ToString();
                obj.Password = reader.GetValue(3).ToString();
                obj.Port = reader.GetValue(4).ToString();
                int val = Convert.ToInt32(reader.GetValue(5));
                obj.ConnectionSecurity = val;
                /*if (val == 1)
                { obj.ConnectionSecurity = Security.SSL; }*/
                val = Convert.ToInt32(reader.GetValue(6));
                obj.EnableSMTP = val;
                /*if (val == 0)
                { obj.EnableSMTP = Break.No; }*/
            }
            con.Close();
            return View(obj);
        }
        [HttpPost]
        public ActionResult UpdateSMTP(SMTP Data)
        {
            int UserId = Convert.ToInt32(Session["UserId"]);
            if (UserId == 0)
            {
                return RedirectToAction("Index", "Login");
            }

            con.Open();
            SqlDataReader reader = null;
            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandType = CommandType.Text;
            sqlCmd.Connection = con;
            sqlCmd.CommandText = "Update dbo.SettingsSMTP Set Host=@Host, UserName=@UserName, Password=@Password, Port=@Port, ConnectionSecurity=@ConnectionSecurity, EnableSMTP=@EnableSMTP from dbo.SettingsSMTP where Id=1";
            sqlCmd.Parameters.AddWithValue("@Host", Data.Host);
            sqlCmd.Parameters.AddWithValue("@UserName", Data.UserName);
            sqlCmd.Parameters.AddWithValue("@Password", Data.Password);
            sqlCmd.Parameters.AddWithValue("@Port", Data.Port);
            sqlCmd.Parameters.AddWithValue("@ConnectionSecurity", Data.ConnectionSecurity);
            sqlCmd.Parameters.AddWithValue("@EnableSMTP", Data.EnableSMTP);
            reader = sqlCmd.ExecuteReader();
            reader.Read();
            con.Close();

            return RedirectToAction("ManageSMTP", "Settings");
        }
        /**End of SMTP Settings**/

        /** Start of Notification Settings**/
        public ActionResult ManageNotification()
        {
            int UserId = Convert.ToInt32(Session["UserId"]);
            if (UserId == 0)
            {
                return RedirectToAction("Index", "Login");
            }
            NotificationList obj = new NotificationList();
            obj.Notifications = new List<Notification>();

            con.Open();
            SqlDataReader reader = null;
            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandType = CommandType.Text;
            sqlCmd.Connection = con;
            sqlCmd.CommandText = "select Id,Name,IsSMS,IsEmail,IsMessage,IsStudent,IsGuardian,IsTeacher from dbo.SettingsNotification";
            reader = sqlCmd.ExecuteReader();
            while (reader.Read())
            {
                Notification subobj = new Notification();
                subobj.Id = Convert.ToInt32(reader.GetValue(0));
                subobj.Name = reader.GetValue(1).ToString();
                subobj.IsSMS = false;
                subobj.IsEMail = false;
                subobj.IsMessage = false;
                subobj.IsStudent = false;
                subobj.IsGuardian = false;
                subobj.IsTeacher = false;
                int val = Convert.ToInt32(reader.GetValue(2));
                if (val == 1)
                { subobj.IsSMS = true; }

                val = Convert.ToInt32(reader.GetValue(3));
                if (val == 1)
                { subobj.IsEMail = true; }

                val = Convert.ToInt32(reader.GetValue(4));
                if (val == 1)
                { subobj.IsMessage = true; }

                val = Convert.ToInt32(reader.GetValue(5));
                if (val == 1)
                { subobj.IsStudent = true; }

                val = Convert.ToInt32(reader.GetValue(6));
                if (val == 1)
                { subobj.IsGuardian = true; }

                val = Convert.ToInt32(reader.GetValue(7));
                if (val == 1)
                { subobj.IsTeacher = true; }

                obj.Notifications.Add(subobj);
            }
            con.Close();
            return View(obj);
        }
        [HttpPost]
        public ActionResult UpdateNotification(NotificationList Data)
        {
            int UserId = Convert.ToInt32(Session["UserId"]);
            if (UserId == 0)
            {
                return RedirectToAction("Index", "Login");
            }
            con.Open();
                
                int val;
            SqlDataReader reader = null;
            SqlCommand sqlCmd= new SqlCommand();
            for (int i = 0; i < Data.Notifications.Count; i++)
                {
                reader = null;
                sqlCmd = new SqlCommand();
                sqlCmd.CommandType = CommandType.Text;
                sqlCmd.Connection = con;
                sqlCmd.CommandText = "Update dbo.SettingsNotification Set IsSMS=@IsSMS,IsEmail=@IsEmail,IsMessage=@IsMessage,IsStudent=@IsStudent,IsGuardian=@IsGuardian,IsTeacher=@IsTeacher where Id=@Id";
                        sqlCmd.Parameters.AddWithValue("@Id", Data.Notifications[i].Id);
                        val = 0;
                        if (Data.Notifications[i].IsSMS == true)
                        { val = 1; }
                sqlCmd.Parameters.AddWithValue("@IsSMS", val);
                        val = 0;
                         if (Data.Notifications[i].IsEMail == true)
                        { val = 1; }
                sqlCmd.Parameters.AddWithValue("@IsEMail", val);
                        val = 0;
                        if (Data.Notifications[i].IsMessage == true)
                        { val = 1; }
                sqlCmd.Parameters.AddWithValue("@IsMessage", val);
                        val = 0;
                        if (Data.Notifications[i].IsStudent == true)
                        { val = 1; }
                sqlCmd.Parameters.AddWithValue("@IsStudent", val);
                        val = 0;
                        if (Data.Notifications[i].IsGuardian == true)
                        { val = 1; }
                sqlCmd.Parameters.AddWithValue("@IsGuardian", val);
                        val = 0;
                        if (Data.Notifications[i].IsTeacher == true)
                        { val = 1; }
                sqlCmd.Parameters.AddWithValue("@IsTeacher", val);
                        
                reader = sqlCmd.ExecuteReader();
                reader.Read();
                }
                con.Close();
            
            return RedirectToAction("ManageNotification", "Settings");
        }
        /**End of Notification Settings**/
        /**Start of SMS Counter**/
        public ActionResult ManageSMSCounter()
        {
            int UserId = Convert.ToInt32(Session["UserId"]);
            if (UserId == 0)
            {
                return RedirectToAction("Index", "Login");
            }
            SMSCounter obj = new SMSCounter();
            obj.DailySMSList = new List<DailySMS>();
            con.Open();
            SqlDataReader reader = null;
            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandType = CommandType.Text;
            sqlCmd.Connection = con;
            sqlCmd.CommandText = "SELECT count(id) FROM dbo.SettingsSMSCounter";
            reader = sqlCmd.ExecuteReader();
            if(reader.Read())
            {
                obj.TotalCount = Convert.ToInt32(reader.GetValue(0));
            }
            reader = null;
            sqlCmd = new SqlCommand();
            sqlCmd.CommandType = CommandType.Text;
            sqlCmd.Connection = con;
            sqlCmd.CommandText = "SELECT count(id) FROM dbo.SettingsSMSCounter WHERE SMSDate=CONVERT(DATE,GETDATE())";
            reader = sqlCmd.ExecuteReader();
            if (reader.Read())
            {
                obj.DayCount = Convert.ToInt32(reader.GetValue(0));
            }

            reader = null;
            sqlCmd = new SqlCommand();
            sqlCmd.CommandType = CommandType.Text;
            sqlCmd.Connection = con;
            sqlCmd.CommandText = "SELECT count(id) FROM dbo.SettingsSMSCounter WHERE SMSDate>=(DATEADD(MONTH,DATEDIFF(MONTH,0,GETDATE()),0)) AND SMSDate<=(DATEADD(SECOND,-1,DATEADD(MONTH,1,DATEADD(MONTH,DATEDIFF(MONTH,0,GETDATE()),0))))";
            reader = sqlCmd.ExecuteReader();
            if (reader.Read())
            {
                obj.MonthCount = Convert.ToInt32(reader.GetValue(0));
            }

            reader = null;
            sqlCmd = new SqlCommand();
            sqlCmd.CommandType = CommandType.Text;
            sqlCmd.Connection = con;
            sqlCmd.CommandText = "SELECT SMSDate,DayWiseCount FROM dbo.SettingsSMSCounter";
            reader = sqlCmd.ExecuteReader();
            while (reader.Read())
            {
                DailySMS subobj = new DailySMS();
                subobj.SMSDate = reader.GetValue(0).ToString();
                subobj.SMSCount = Convert.ToInt32(reader.GetValue(1));
                obj.DailySMSList.Add(subobj);
            }
            return View(obj);
        }
        /**End of SMS Counter**/
    }
}

