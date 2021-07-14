using GS247.Models;
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
    public class LoginController : Controller
    {
        public SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["GS247"].ConnectionString);
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Authorize(User user)
        {
            int UserId = 0, SuperUser = 0;
            con.Open();
            SqlDataReader reader = null;
            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandType = CommandType.Text;
            sqlCmd.CommandText = "select Id,SuperUser from dbo.users where UserName=@User and Password=@Password";
            sqlCmd.Connection = con;
            sqlCmd.Parameters.AddWithValue("@User",user.UserName);
            sqlCmd.Parameters.AddWithValue("@Password", user.Password);
            reader = sqlCmd.ExecuteReader();
            if(reader.Read())
            {
                UserId = Convert.ToInt32(reader.GetValue(0));
                SuperUser = Convert.ToInt32(reader.GetValue(1));
            }
            con.Close();
            Session["UserId"] = UserId;
            Session["SuperUser"] = SuperUser;
            Session["UserName"] = user.UserName;
            Session["StudentDetailsId"] = 17438;
            Session["StaffId"] = 1;
            if(UserId>0)
            { return RedirectToAction("Route", "Home"); }
            else
            { return RedirectToAction("Index", "Login"); }
            
        }
        // GET: Login/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Login/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Login/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Login/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Login/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Login/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Login/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
