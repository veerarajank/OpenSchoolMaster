using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GS247.Utilities;
using Newtonsoft.Json;
using System.Data;

namespace GS247.Controllers
{
    public class DashboardController : Controller
    {
        // GET: Dashboard
        public ActionResult Index()
        {
            int UserId = Convert.ToInt32(Session["UserId"]);
            if(UserId ==0)
            {
                return RedirectToAction("Index", "Login");
            }
            return View();
        }

        [HttpPost]
        public string GetNewsFeed(string flag)
        {
            string res = string.Empty;
            DataAccess obj = new DataAccess();
            List<Params> paramList = new List<Params>();
            paramList.Add(new Params("@Flag", flag));
            DataTable dt = obj.Execute("GetDashboardDetails", CommandType.StoredProcedure, paramList);
            res = JsonConvert.SerializeObject(dt, Formatting.Indented);
            return res;

        }

        [HttpPost]
        public string GetDashBoard(string flag,string Id)
        {
            string res = string.Empty;
            DataAccess obj = new DataAccess();
            List<Params> paramList = new List<Params>();
            paramList.Add(new Params("@Flag", flag));
            paramList.Add(new Params("@Id", Id));
            DataTable dt = obj.Execute("GetDashboardDetails", CommandType.StoredProcedure, paramList);
            res = JsonConvert.SerializeObject(dt, Formatting.Indented);
            return res;

        }

    }
}