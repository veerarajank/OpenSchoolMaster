using GS247.Models;
using GS247.Models.Transport;
using System;
using System.Collections.Generic;
using System.Configuration;
//using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;

namespace GS247.Controllers
{
    public class TransportController : Controller
    {
       public GS247Entities8 db=new GS247Entities8();
        SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["GS247"].ConnectionString);
        SqlCommand cmd;
        public string fileLocation;
        
        public SqlDataAdapter da;
        public SqlDataReader dr;
        public ActionResult ListAllRoutes_Index()
        {
            if (Convert.ToInt32(Session["UserId"]) == 0) return RedirectToAction("Index", "Login");
            return View(db.TransportRoutes.ToList());
        }
       public ActionResult Create_Route()
        {
            if (Convert.ToInt32(Session["UserId"]) == 0) return RedirectToAction("Index", "Login");
            ViewBag.Title = "Create Route";
            ViewBag.VehicleCodes=new SelectList(db.TransportVehicles.Select(x=>x.VehicleCode).Distinct());
            return View(new TransportRoute());
        }
        [HttpPost]
        public ActionResult Create_Route(TransportRoute route)
        {
            if (Convert.ToInt32(Session["UserId"]) == 0) return RedirectToAction("Index", "Login");
            if (db.TransportRoutes.Where(x => x.RouteName == route.RouteName).FirstOrDefault() == null)
            {
                db.TransportRoutes.Add(route);
                db.SaveChanges();
                TempData["ListAllRoutes"] = "Added";
                return RedirectToAction("ListAllRoutes_Index");
            }
            ViewBag.Title = "Create Route";
            ViewBag.VehicleCodes = new SelectList(db.TransportVehicles.Select(x => x.VehicleCode).Distinct());
            TempData["Route"] = "Route Name Already Exist";
            return View(route);
        }
        public ActionResult UpdateRoute(int id)
        {
            if (Convert.ToInt32(Session["UserId"]) == 0) return RedirectToAction("Index", "Login");
            ViewBag.Title = "Update Route";
            ViewBag.VehicleCodes = new SelectList(db.TransportVehicles.Select(x => x.VehicleCode).Distinct());
            return View("Create_Route", db.TransportRoutes.Find(id));
        }
        [HttpPost]
        public ActionResult UpdateRoute(TransportRoute route)
        {
            if (Convert.ToInt32(Session["UserId"]) == 0) return RedirectToAction("Index", "Login");
            db.Entry(route).State = EntityState.Modified;
            db.SaveChanges();
                TempData["ListAllRoutes"] = "Updated";
                return RedirectToAction("ListAllRoutes_Index");
        }
        public ActionResult DeleteRoute(int id)
        {
            if (Convert.ToInt32(Session["UserId"]) == 0) return RedirectToAction("Index", "Login");
            db.TransportRoutes.Remove(db.TransportRoutes.Find(id));
            db.SaveChanges();
            TempData["ListAllRoutes"] = "Deleted";
            return RedirectToAction("ListAllRoutes_Index");
        }
        public ActionResult Create_Stops(int id)
        {
            if (Convert.ToInt32(Session["UserId"]) == 0) return RedirectToAction("Index", "Login");
            var route = db.TransportRoutes.Find(id);
            if (db.TransportStops.Where(x => x.RouteName == route.RouteName).ToList().Count < 1)
            {
                for (int i = 1; i <= route.NoOfStops; i++)
                {
                    TransportStop stop = new TransportStop
                    {
                        StopName = "Stop" + i.ToString(),
                        Fare = 0,
                        EveArrival = "0:00",
                        MonArrival = "0:00",
                        RouteName = route.RouteName
                    };
                    db.TransportStops.Add(stop);
                    db.SaveChanges();
                }
            }
            Session["RouteName"] = route.RouteName;
            return View(db.TransportStops.Where(x => x.RouteName == route.RouteName).ToList());
        }
        public ActionResult Edit_Stops(string routeName)
        {
            if (Convert.ToInt32(Session["UserId"]) == 0) return RedirectToAction("Index", "Login");
            return View(db.TransportStops.Where(x => x.RouteName == routeName).ToList());
        }
        [HttpPost]
        public ActionResult SubmitStops(List<TransportStop> stops)
        {
            if (Convert.ToInt32(Session["UserId"]) == 0) return RedirectToAction("Index", "Login");
            for (int i=0;i<stops.Count;i++)
            {
                db.Entry(stops[i]).State = EntityState.Modified;
                db.SaveChanges();
            }
            TempData["Stops"] = "Updated";
            string routeName = stops[0].RouteName;
            Session["RouteName"] = stops[0].RouteName;
            return View("Create_Stops", db.TransportStops.Where(x => x.RouteName == routeName).ToList());
        }
        public ActionResult Remove_All_Stops(string routeName)
        {
            if (Convert.ToInt32(Session["UserId"]) == 0) return RedirectToAction("Index", "Login");
            db.TransportStops.RemoveRange(db.TransportStops.Where(x => x.RouteName == routeName).ToList());
            db.SaveChanges();
            TempData["Stops"] = "Removed";
            Session["RouteName"] = routeName;
            return View("Create_Stops",db.TransportStops.Where(x => x.RouteName == routeName).ToList());
        }
        public ActionResult Create_Stop(string routeName)
        {
            if (Convert.ToInt32(Session["UserId"]) == 0) return RedirectToAction("Index", "Login");
            var stopCount = db.TransportStops.Where(x => x.RouteName == routeName).Count();
            if ( stopCount== db.TransportRoutes.Where(x => x.RouteName == routeName).FirstOrDefault().NoOfStops||stopCount==0)
            {
                TransportStop stop = new TransportStop
                {
                    RouteName = routeName
                };
                ViewBag.Title = "Create Stop";
                return View(stop);
            }
            TempData["Stops"] = "Required stops are added already";
            Session["RouteName"] = routeName;
            return View("Create_Stops", db.TransportStops.Where(x => x.RouteName == routeName).ToList());
        }
        [HttpPost]
        public ActionResult Create_Stop(TransportStop stop)
        {
            if (Convert.ToInt32(Session["UserId"]) == 0) return RedirectToAction("Index", "Login");
            if (db.TransportStops.Where(x=>x.RouteName==stop.RouteName&&x.StopName==stop.StopName).FirstOrDefault()==null)
            {
                db.TransportStops.Add(stop);
                db.SaveChanges();
                TempData["Stops"] = "Added";
                Session["RouteName"] = stop.RouteName;
                return View("Create_Stops", db.TransportStops.Where(x => x.RouteName == stop.RouteName).ToList());
            }
            TempData["Create_Stop"] = "This route having this stop already";
            ViewBag.Title = "Create Stop";
            return View(stop);
        }
        public ActionResult UpdateStop(int id)
        {
            if (Convert.ToInt32(Session["UserId"]) == 0) return RedirectToAction("Index", "Login");
            ViewBag.Title = "Update Stop";
            return View("Create_Stop",db.TransportStops.Find(id));
        }
        [HttpPost]
        public ActionResult UpdateStop(TransportStop stop)
        {
            if (Convert.ToInt32(Session["UserId"]) == 0) return RedirectToAction("Index", "Login");
            db.Entry(stop).State = EntityState.Modified;
            db.SaveChanges();
            TempData["Stops"] = "Updated";
            Session["RouteName"] = stop.RouteName;
            return View("Create_Stops", db.TransportStops.Where(x => x.RouteName == stop.RouteName).ToList());
        }
        public ActionResult DeleteStop(int id)
        {
            if (Convert.ToInt32(Session["UserId"]) == 0) return RedirectToAction("Index", "Login");
            var stop = db.TransportStops.Find(id);
            db.TransportStops.Remove(stop);
            db.SaveChanges();
            TempData["Stops"] = "Removed";
            Session["RouteName"] = stop.RouteName;
            return View("Create_Stops", db.TransportStops.Where(x => x.RouteName == stop.RouteName).ToList());
        }
        public ActionResult Vehicle_Index()
        {
            if (Convert.ToInt32(Session["UserId"]) == 0) return RedirectToAction("Index", "Login");
            List<Vehicle> vehicles = new List<Vehicle>();
            var data = db.TransportVehicles.ToList();
            foreach(var item in data)
            { 
                    Vehicle vehicle = new Vehicle
                    {
                        Address = item.Address,
                        VehicleType =item.VehicleType,
                        TaxRemitted = item.TaxRemitted,
                        Permit = item.Permit,
                        Insurance = item.Insurance,
                        Phone = item.Phone,
                        State = item.State,
                        City = item.City,
                        MaximumCapacity = Convert.ToInt32(item.MaximumCapacity),
                        NoOfSeats =Convert.ToInt32(item.NoOfSeats),
                        VehicleCode = item.VehicleCode,
                        VehicleNo =item.VehicleNo
                    };
                //Getting Route Name
                var route = (from r in db.TransportRoutes join v in db.TransportVehicles on r.VehicleCode equals v.VehicleCode where v.VehicleCode == item.VehicleCode select r).FirstOrDefault();
                if (route != null) vehicle.RouteName = route.RouteName;
                else vehicle.RouteName = "Not Assigned";

                //Getting Driver Name
                var driver = (from d in db.TransportDrivers join v in db.TransportDriverVehicleLinks on d.Id equals v.DriverId where v.VehicleId == item.Id select new { d.FirstName,d.LastName}).FirstOrDefault();
                if (driver != null) vehicle.DriverName = driver.FirstName + " " + driver.LastName;
                else vehicle.DriverName = "Not Assigned";
                    vehicles.Add(vehicle);
                }
            return View(vehicles);
        }
        public ActionResult Create_Vehicle()
        {
            if (Convert.ToInt32(Session["UserId"]) == 0) return RedirectToAction("Index", "Login");
            ViewBag.Title = "Create Vehicle";
            return View(new TransportVehicle());
        }
        [HttpPost]
        public ActionResult Create_Vehicle(TransportVehicle vehicle)
        {
            if (Convert.ToInt32(Session["UserId"]) == 0) return RedirectToAction("Index", "Login");
            if (db.TransportVehicles.Where(x=>x.VehicleNo==vehicle.VehicleNo).FirstOrDefault()==null)
            {
                db.TransportVehicles.Add(vehicle);
                db.SaveChanges();
                TempData["Vehicle_Index"] = "Added";
                return RedirectToAction("Vehicle_Index");
            }
            ViewBag.Title = "Create Vehicle";
            TempData["Vehicle"] = "Vehicle number Already Exist";
            return View(vehicle);
        }
        public ActionResult UpdateVehicle(string vehicleNo)
        {
            if (Convert.ToInt32(Session["UserId"]) == 0) return RedirectToAction("Index", "Login");
            ViewBag.Title = "Update Vehicle";
            return View("Create_Vehicle", db.TransportVehicles.Where(x=>x.VehicleNo==vehicleNo).FirstOrDefault());
        }
        [HttpPost]
        public ActionResult UpdateVehicle(TransportVehicle vehicle)
        {
            if (Convert.ToInt32(Session["UserId"]) == 0) return RedirectToAction("Index", "Login");
            db.Entry(vehicle).State = EntityState.Modified;
            db.SaveChanges();
            TempData["Vehicle_Index"] = "Updated";
            return RedirectToAction("Vehicle_Index");
        }
        public ActionResult DeleteVehicle(string vehicleNo)
        {
            if (Convert.ToInt32(Session["UserId"]) == 0) return RedirectToAction("Index", "Login");
            db.TransportVehicles.Remove(db.TransportVehicles.Where(x=>x.VehicleNo==vehicleNo).FirstOrDefault());
            db.SaveChanges();
            TempData["Vehicle_Index"] = "Deleted";
            return RedirectToAction("Vehicle_Index");
        }
        public ActionResult Driver_Index()
        {
            if (Convert.ToInt32(Session["UserId"]) == 0) return RedirectToAction("Index", "Login");
            return View(db.TransportDrivers.ToList());
        }
        public ActionResult Create_Driver()
        {
            if (Convert.ToInt32(Session["UserId"]) == 0) return RedirectToAction("Index", "Login");
            ViewBag.Title = "Create Driver";
            return View(new TransportDriver());
        }
        [HttpPost]
        public ActionResult Create_Driver(TransportDriver driver)
        {
            if (Convert.ToInt32(Session["UserId"]) == 0) return RedirectToAction("Index", "Login");
            if (db.TransportDrivers.Where(x => x.LicenseNo == driver.LicenseNo).ToList().Count() == 0)
            {
                db.TransportDrivers.Add(driver);
                db.SaveChanges();
                TempData["Driver_Index"] = "Added";
                return RedirectToAction("Driver_Index");
            }
            ViewBag.Title = "Create Driver";
            TempData["Driver"] = "Driver with same License # already exist";
            return View(driver);
        }
        public ActionResult UpdateDriver(int id)
        {
            if (Convert.ToInt32(Session["UserId"]) == 0) return RedirectToAction("Index", "Login");
            ViewBag.Title = "Update Driver";
            return View("Create_Driver", db.TransportDrivers.Find(id));
        }
        [HttpPost]
        public ActionResult UpdateDriver(TransportDriver driver)
        {
            if (Convert.ToInt32(Session["UserId"]) == 0) return RedirectToAction("Index", "Login");
            db.Entry(driver).State = EntityState.Modified;
            db.SaveChanges();
            TempData["Driver_Index"] = "Updated";
            return RedirectToAction("Driver_Index");
        }
        public ActionResult DeleteDriver(int id)
        {
            if (Convert.ToInt32(Session["UserId"]) == 0) return RedirectToAction("Index", "Login");
            db.TransportDrivers.Remove(db.TransportDrivers.Find(id));
            db.SaveChanges();
            TempData["Driver_Index"] = "Deleted";
            return RedirectToAction("Driver_Index");
        }
        public ActionResult Driver_Vehicle_Association_Index()
        {
            if (Convert.ToInt32(Session["UserId"]) == 0) return RedirectToAction("Index", "Login");
            ViewBag.Drivers = new SelectList(db.TransportDrivers.Select(x => new { DriverName = x.FirstName + " " + x.LastName, x.Id }).Distinct(), "Id", "DriverName");
            ViewBag.Vehicles = new SelectList(db.TransportVehicles.Select(x =>new { x.VehicleCode,x.Id}).Distinct(),"Id","VehicleCode");
            List<Vehicle_Driver> vehicle_Drivers = new List<Vehicle_Driver>();
            using (conn)
            {
                conn.Open();
                cmd = new SqlCommand("select dv.DriverId,d.FirstName,d.LastName,d.DOB,v.VehicleCode from TransportDriverVehicleLink dv left join TransportDrivers d on dv.DriverId=d.Id left join TransportVehicles v on dv.VehicleId=v.Id", conn);
                dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    Vehicle_Driver vehicle_Driver = new Vehicle_Driver
                    {
                        DOB = dr["DOB"].ToString(),
                        DriverId = Convert.ToInt32(dr["DriverId"].ToString()),
                        DriverName = dr["FirstName"].ToString() + " " + dr["LastName"].ToString(),
                        VehicleCode = dr["VehicleCode"].ToString()
                    };
                    string vehicleCode = dr["VehicleCode"].ToString();
                    var route = (from r in db.TransportRoutes
                                 join v in db.TransportVehicles
                                 on r.VehicleCode equals v.VehicleCode
                                 where v.VehicleCode == vehicleCode
                                 select r).FirstOrDefault();
                    if (route != null)
                        vehicle_Driver.RouteName = route.RouteName;
                    else
                        vehicle_Driver.RouteName = "Not Assigned";
                    vehicle_Drivers.Add(vehicle_Driver);
                }
            }
            Vehicle_Driver_Context context = new Vehicle_Driver_Context
            {
                 link=new TransportDriverVehicleLink(),
                 links=vehicle_Drivers
            };
            return View(context);
        }
        [HttpPost]
        public ActionResult Driver_Vehicle_Association_Index(Vehicle_Driver_Context context)
        {
            if (Convert.ToInt32(Session["UserId"]) == 0) return RedirectToAction("Index", "Login");
            if (db.TransportDriverVehicleLinks.Where(x => x.DriverId == context.link.DriverId && x.VehicleId == context.link.VehicleId).FirstOrDefault() == null)
            {
                db.TransportDriverVehicleLinks.Add(context.link);
                db.SaveChanges();
                TempData["Link_Index"] = "Assigned";
                    }
            else
            TempData["Link_Index"] = "Same driver assigned to Same Vehicle already";
            ViewBag.Drivers = new SelectList(db.TransportDrivers.Select(x => new { DriverName = x.FirstName + " " + x.LastName, x.Id }).Distinct(), "Id", "DriverName");
            ViewBag.Vehicles = new SelectList(db.TransportVehicles.Select(x => new { x.VehicleCode, x.Id }).Distinct(), "Id", "VehicleCode");
            return RedirectToAction("Driver_Vehicle_Association_Index");
        }
        [HttpPost]
        public ActionResult UpdateAllot(int VehicleId,int DriverId)
        {
            if (Convert.ToInt32(Session["UserId"]) == 0) return RedirectToAction("Index", "Login");
            var link = db.TransportDriverVehicleLinks.Where(x => x.DriverId == DriverId).FirstOrDefault();
            link.VehicleId = VehicleId;
            db.Entry(link).State = EntityState.Modified;
            db.SaveChanges();
            TempData["Link_Index"] = "Updated";
            return RedirectToAction("Driver_Vehicle_Association_Index");
        }
       public ActionResult DeleteAllot(int id)
        {
            if (Convert.ToInt32(Session["UserId"]) == 0) return RedirectToAction("Index", "Login");
            db.TransportDriverVehicleLinks.Remove(db.TransportDriverVehicleLinks.Where(x => x.DriverId == id).FirstOrDefault());
            db.SaveChanges();
            TempData["Link_Index"] = "Removed";
            return RedirectToAction("Driver_Vehicle_Association_Index");
        }
        public ActionResult SearchStudents(string BackTo)
        {
            if (Convert.ToInt32(Session["UserId"]) == 0) return RedirectToAction("Index", "Login");
            Session["BackTo"] = BackTo;
            List<Students> students = new List<Students>();
            var data = db.StudentDetails.ToList();
            foreach(var item in data)
            { 
                    Students student = new Students
                    {
                         AdmissionNo=item.AdmissionNo,
                          Batch=db.batches.Where(x=>x.Id==item.Batch).FirstOrDefault().Name,
                           Phone=item.PhoneNumber1,
                            RollNumber=item.RollNumber,
                               StudentId=item.StudentDetailsId.ToString(),
                                StudentName=item.FirstName+" "+item.MiddleName+" "+item.LastName
                    };
                var route = (from s in db.StudentDetails
                             join sr in db.TransportStudentRouteLinks on s.StudentDetailsId equals sr.StudentId 
                             join r in db.TransportRoutes on sr.RouteId equals r.RouteId
                             join ss in db.TransportStops on sr.StopId equals ss.Id
                             where s.StudentDetailsId == item.StudentDetailsId select new { r,ss}).FirstOrDefault();
                if (route != null) {
                    student.RouteName = route.r.RouteName;
                    student.StopName = route.ss.StopName;
                        }
                else
                {
                    student.RouteName = "Not Assigned";
                    student.StopName = "Not Assigned";
                }
                    students.Add(student);
                }
            return View(students);
        }
        public ActionResult Allotment_Index()
        {
            if (Convert.ToInt32(Session["UserId"]) == 0) return RedirectToAction("Index", "Login");
            List<Students> students = new List<Students>();
            var studentData = (from s in db.StudentDetails
                               join sr in db.TransportStudentRouteLinks
                               on s.StudentDetailsId equals sr.StudentId
                               select s).ToList();
            foreach (var item in studentData)
            {
                Students student = new Students
                {
                    AdmissionNo = item.AdmissionNo,
                    StudentId = item.StudentDetailsId.ToString(),
                    Batch = db.batches.Where(x => x.Id == item.Batch).FirstOrDefault().Name,
                    Phone = item.PhoneNumber1,
                    RollNumber = item.RollNumber,
                    StudentName = item.FirstName + " " + item.MiddleName + " " + item.LastName,
                };
                var data = (from s in db.StudentDetails
                            join sr in db.TransportStudentRouteLinks on s.StudentDetailsId equals sr.StudentId
                            join r in db.TransportRoutes on sr.RouteId equals r.RouteId
                            join ss in db.TransportStops on sr.StopId equals ss.Id
                            select new { r, ss }).FirstOrDefault();

                if (data.r != null) student.RouteName = data.r.RouteName;
                else student.RouteName = "Not Assigned";
                if (data.ss != null) student.StopName = data.ss.StopName;
                else student.StopName = "Not Assigned";
                students.Add(student);
            }
            ViewBag.Routes = new SelectList(db.TransportRoutes.Select(x => new { x.RouteId, x.RouteName }).Distinct(), "RouteId", "RouteName");
            ViewBag.Stops = new SelectList(db.TransportStops.Select(x => new { x.Id, x.StopName }).Distinct(), "Id", "StopName");
            return View(students);
        }
        public ActionResult Student_Allotment()
        {
            if (Convert.ToInt32(Session["UserId"]) == 0) return RedirectToAction("Index", "Login");
            ViewBag.Routes = new SelectList(db.TransportRoutes.Select(x => new { x.RouteId, x.RouteName }).Distinct(), "RouteId", "RouteName");
            ViewBag.Stops = new SelectList(db.TransportStops.Select(x => new { x.Id, x.StopName }).Distinct(), "Id", "StopName");
            return View(new TransportStudentRouteLink());
        }
        [HttpPost]
        public ActionResult Student_Allotment(TransportStudentRouteLink link)
        {
            if (Convert.ToInt32(Session["UserId"]) == 0) return RedirectToAction("Index", "Login");
            ViewBag.Routes = new SelectList(db.TransportRoutes.Select(x => new { x.RouteId, x.RouteName }).Distinct(), "RouteId", "RouteName");
            ViewBag.Stops = new SelectList(db.TransportStops.Select(x => new { x.Id, x.StopName }).Distinct(), "Id", "StopName");
            if (link.StudentId==0)
            {
                TempData["Allotment"] = "Select Student";
                return View(link);
            }
            if (db.TransportStudentRouteLinks.Where(x => x.StudentId == link.StudentId && x.RouteId == link.RouteId && x.StopId == link.StopId).FirstOrDefault() == null)
            {
                db.TransportStudentRouteLinks.Add(link);
                db.SaveChanges();

                //Updating Student count with Routes
                var route = db.TransportRoutes.Find(link.RouteId);
                route.NoOfStudents= db.TransportStudentRouteLinks.Where(x => x.RouteId == link.RouteId).Count();
                db.Entry(route).State = EntityState.Modified;
                db.SaveChanges();

                TempData["Allotment_Index"] = "Assigned";
                return RedirectToAction("Allotment_Index");
            }
            
            TempData["Allotment"] = "Student Already assigned to same Route and same Stop";
            return View(link);
        }
        public ActionResult Update_Student_Allotment(int id)
        {
            if (Convert.ToInt32(Session["UserId"]) == 0) return RedirectToAction("Index", "Login");
            var link = db.TransportStudentRouteLinks.Find(id);
            link.StudentDetail = db.StudentDetails.Find(link.StudentId);
            var route = db.TransportRoutes.Find(link.RouteId);
            ViewBag.Routes = new SelectList(db.TransportRoutes.Select(x => new { x.RouteId, x.RouteName }).Distinct(), "RouteId", "RouteName");
            ViewBag.Stops = new SelectList(db.TransportStops.Where(x=>x.RouteName==route.RouteName).Select(x => new { x.Id, x.StopName }).Distinct(), "Id", "StopName");
            
                return View(link);
        }
        [HttpPost]
        public ActionResult Update_Student_Allotment(TransportStudentRouteLink link)
        {
            if (Convert.ToInt32(Session["UserId"]) == 0) return RedirectToAction("Index", "Login");
            db.Entry(link).State = EntityState.Modified;
            db.SaveChanges();

            //Updating Student count with Routes
            var route = db.TransportRoutes.Find(link.RouteId);
            route.NoOfStudents = db.TransportStudentRouteLinks.Where(x => x.RouteId == link.RouteId).Count();
            db.Entry(route).State = EntityState.Modified;
            db.SaveChanges();

            TempData["Allotment_Index"] = "Updated";
            return RedirectToAction("Allotment_Index");
        }
        public ActionResult Delete_Student_Allotment(int id)
        {
            if (Convert.ToInt32(Session["UserId"]) == 0) return RedirectToAction("Index", "Login");
            db.TransportStudentRouteLinks.Remove(db.TransportStudentRouteLinks.Find(id));
            db.SaveChanges();
            TempData["Allotment_Index"] = "Removed";
            return RedirectToAction("Allotment_Index");
        }
        public JsonResult GetStopNames(int routeId)
        {
            var route = db.TransportRoutes.Find(routeId);
            return Json(new SelectList(db.TransportStops.Where(x => x.RouteName == route.RouteName).Distinct(),"Id","StopName"),JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetStudent(int id)
        {
            if (Convert.ToInt32(Session["UserId"]) == 0) return RedirectToAction("Index", "Login");
            ViewBag.Routes = new SelectList(db.TransportRoutes.Select(x => new { x.RouteId, x.RouteName }).Distinct(), "RouteId", "RouteName");
            ViewBag.Stops = new SelectList(db.TransportStops.Select(x => new { x.Id, x.StopName }).Distinct(), "Id", "StopName");
            var link = db.TransportStudentRouteLinks.Find(id);
            if (link != null)
            {
                link.StudentDetail = db.StudentDetails.Find(link.StudentId);
                return View("Update_Student_Allotment", link);
            }
            TransportStudentRouteLink transport = new TransportStudentRouteLink
            {
                StudentId = id,
                StudentDetail = db.StudentDetails.Find(id)
            };
            if(Session["BackTo"]!=null)
            return View(Session["BackTo"].ToString(), transport);
            return RedirectToAction("SearchStudents",new { BackTo=""});
        }
     
        public ActionResult BusLog_Index()
        {
            if (Convert.ToInt32(Session["UserId"]) == 0) return RedirectToAction("Index", "Login");
            List<TransportBusLog> transportBusLogs = new List<TransportBusLog>();
            var busLogs = db.TransportBusLogs.ToList();
            foreach(var item in busLogs)
            {
                TransportBusLog log = new TransportBusLog
                {
                     Amount=item.Amount,
                      EntryDate=item.EntryDate,
                       FuelConsumed=item.FuelConsumed,
                        Id=item.Id,
                         ReadingEnd=item.ReadingEnd,
                          ReadingStart=item.ReadingStart,
                           TransportVehicle=db.TransportVehicles.Find(item.VehicleId),
                            VehicleId=item.VehicleId
                };
                transportBusLogs.Add(log);
            }
            return View(transportBusLogs);
        }
        public ActionResult Create_BusLog()
        {
            if (Convert.ToInt32(Session["UserId"]) == 0) return RedirectToAction("Index", "Login");
            ViewBag.Title = "Create Bus Log";
            ViewBag.Vehicles = new SelectList(db.TransportVehicles.Select(x=>new { x.Id,x.VehicleCode}).Distinct(),"Id","VehicleCode");
            return View(new TransportBusLog());
        }
        [HttpPost]
        public ActionResult Create_BusLog(TransportBusLog busLog)
        {
            if (Convert.ToInt32(Session["UserId"]) == 0) return RedirectToAction("Index", "Login");
            if (db.TransportBusLogs.Where(x => x.VehicleId==busLog.VehicleId&&x.ReadingStart == busLog.ReadingStart&&x.ReadingEnd==busLog.ReadingEnd).FirstOrDefault() == null)
            {
                db.TransportBusLogs.Add(busLog);
                db.SaveChanges();
                TempData["BusLog_Index"] = "Added";
                return RedirectToAction("BusLog_Index");
            }
            ViewBag.Title = "Create Bus Log";
            ViewBag.Vehicles = new SelectList(db.TransportVehicles.Select(x => new { x.Id, x.VehicleCode }).Distinct(), "Id", "VehicleCode");
            TempData["Bus_Log"] = "Same Vehicle with same Reading already submitted";
            return View(busLog);
        }
        public ActionResult Update_BusLog(int id)
        {
            if (Convert.ToInt32(Session["UserId"]) == 0) return RedirectToAction("Index", "Login");
            ViewBag.Title = "Update Bus Log";
            ViewBag.Vehicles = new SelectList(db.TransportVehicles.Select(x => new { x.Id, x.VehicleCode }).Distinct(), "Id", "VehicleCode");
            return View("Create_BusLog", db.TransportBusLogs.Find(id));
        }
        [HttpPost]
        public ActionResult Update_BusLog(TransportBusLog busLog)
        {
            if (Convert.ToInt32(Session["UserId"]) == 0) return RedirectToAction("Index", "Login");
            db.Entry(busLog).State = EntityState.Modified;
            db.SaveChanges();
            TempData["BusLog_Index"] = "Updated";
            return RedirectToAction("BusLog_Index");
        }
        public ActionResult Delete_BusLog(int id)
        {
            if (Convert.ToInt32(Session["UserId"]) == 0) return RedirectToAction("Index", "Login");
            db.TransportBusLogs.Remove(db.TransportBusLogs.Find(id));
            db.SaveChanges();
            TempData["BusLog_Index"] = "Deleted";
            return RedirectToAction("BusLog_Index");
        }
        public ActionResult Vehicle_Attendance()
        {
            if (Convert.ToInt32(Session["UserId"]) == 0) return RedirectToAction("Index", "Login");
            var attendance = db.TransportAttendanceByRoutes.ToList();
            List<VehicleAttendance> vehicleAttendances = new List<VehicleAttendance>();
            foreach(var item in attendance)
            {
                VehicleAttendance trip = new VehicleAttendance
                {
                     
                     DriverName=db.TransportDrivers.Where(x=>x.Id==item.DriverId).Select(x=>x.FirstName+" "+x.LastName).SingleOrDefault(),
                      AttendanceDate=Convert.ToDateTime(item.AttendanceDate),
                       EvngEnd=item.EvngEndTime,
                        EvngStart=item.EvngStartTime,
                         MorngEnd=item.MorngEndTime,
                          MorngStart=item.morngStartTime 
                };
                var route = db.TransportRoutes.Find(item.RouteId);
                if (route != null) trip.RouteName = route.RouteName;
                vehicleAttendances.Add(trip);
            }
            return View(vehicleAttendances);
        }
        public ActionResult Add_Attendance()
        {
            if (Convert.ToInt32(Session["UserId"]) == 0) return RedirectToAction("Index", "Login");
            ViewBag.Routes = new SelectList(db.TransportRoutes.Distinct(),"RouteId","RouteName");
            ViewBag.Drivers = new SelectList(db.TransportDrivers.Select(x=>new { x.Id,DriverName=x.FirstName+" "+x.LastName}).Distinct(), "Id", "DriverName");
            return View();
        }
       
        public ActionResult GetStations(string session,int route,int driver,int act)
        {
            if (Convert.ToInt32(Session["UserId"]) == 0) return RedirectToAction("Index", "Login");
            
            if (act == 0)
            {
                    var trip = db.TransportAttendanceByRoutes.Where(x => x.RouteId == route && x.AttendanceDate == DateTime.Today && x.DriverId == driver).FirstOrDefault();
                if (trip == null)
                {
                    TransportAttendanceByRoute byRoute = new TransportAttendanceByRoute
                    {
                        DriverId = driver,
                        RouteId = route,
                        AttendanceDate = DateTime.Today
                    };

                    if (session == "Morning")
                        byRoute.morngStartTime = DateTime.Now.ToString("h:mm:ss tt");
                    if (session == "Evening")
                        byRoute.EvngStartTime = DateTime.Now.ToString("h:mm:ss tt");
                    db.TransportAttendanceByRoutes.Add(byRoute);
                    db.SaveChanges();

                  
                }
                else
                {
                    if (session == "Evening")
                        trip.EvngStartTime = DateTime.Now.ToString("h:mm:ss tt");
                    else if (session == "Morning")
                        trip.morngStartTime = DateTime.Now.ToString("h:mm:ss tt");
                    db.Entry(trip).State = EntityState.Modified;
                    db.SaveChanges();
                }

                //Update Start time of all students
                if (session == "Evening")
                {
                    var students = db.TransportStudentAttendanceLogs.Where(x => x.RouteId == route && x.AttendanceDate == DateTime.Today).ToList();
                    foreach (var item in students)
                    {
                        item.EvngStartTime = DateTime.Now.ToString("h:mm:ss tt");
                        db.Entry(item).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                }
                var routeData = db.TransportRoutes.Find(route);
                    ViewBag.Stations = new SelectList(db.TransportStops.Where(x => x.RouteName == routeData.RouteName).ToList(), "Id", "StopName");

                TransportStudentAttendance studentAttendance = new TransportStudentAttendance
                {
                        RouteId = route,
                        Students = (from s in db.StudentDetails
                                    join sr in db.TransportStudentRouteLinks
                                    on s.StudentDetailsId equals sr.StudentId
                                    where sr.RouteId == route
                                    select s).ToList()
                    };

                    if (session == "Morning") studentAttendance.Session = "Morning";
                    else if (session == "Evening") studentAttendance.Session = "Evening";
                    return View("Student_Attendance", studentAttendance);
            }
            else if(act==1)
            {
                var EvngAttendance = db.TransportAttendanceByRoutes.Where(x => x.RouteId == route && x.AttendanceDate == DateTime.Today && x.DriverId == driver).FirstOrDefault();
                if (EvngAttendance != null)
                {
                    if (session == "Morning")
                        EvngAttendance.MorngEndTime = DateTime.Now.ToString("h:mm:ss tt");
                    else if (session == "Evening")
                        EvngAttendance.EvngEndTime = DateTime.Now.ToString("h:mm:ss tt");
                    db.Entry(EvngAttendance).State = EntityState.Modified;
                    db.SaveChanges();

                    //Update Exit time of all students
                    if(session=="Morning")
                    {
                        var students = db.TransportStudentAttendanceLogs.Where(x => x.RouteId == route && x.AttendanceDate == DateTime.Today).ToList();
                        foreach (var item in students)
                        {
                            item.MorngEndTime = DateTime.Now.ToString("h:mm:ss tt");
                            db.Entry(item).State = EntityState.Modified;
                            db.SaveChanges();
                        }
                    }
                    TempData["Attendance"] = session + " session is completed";
                    return RedirectToAction("Add_Attendance");
                }
                TempData["Attendance"] = "Trip not started yet";
            }
            return RedirectToAction("Add_Attendance");
        }
        public JsonResult Present(int studentId,int routeId,int stopId,string session)
        {
            if (db.TransportStudentAttendanceLogs.Where(x=>x.StudentDetailId==studentId&&x.AttendanceDate==DateTime.Today).ToList().Count()==0)
            {
                TransportStudentAttendanceLog log = new TransportStudentAttendanceLog
                {
                     AttendanceDate=DateTime.Today,
                      RouteId=routeId,
                       StopId=stopId,
                        StudentDetailId=studentId
                };
                if (session == "Morning") log.MorngStartTime = DateTime.Now.ToString("h:mm:ss tt");
                if (session == "Evening")
                {
                    var transportByRoute = db.TransportAttendanceByRoutes.Where(x => x.RouteId == routeId && x.AttendanceDate == DateTime.Today).FirstOrDefault();
                    log.EvngStartTime = transportByRoute.EvngStartTime;
                    log.EvngEndTime = DateTime.Now.ToString("h:mm:ss tt");
                }

                db.TransportStudentAttendanceLogs.Add(log);
                db.SaveChanges();
            }
            else
            {
                if (session == "Morning") return Json("Already Present");
                else if (session == "Evening")
                {
                    var log = db.TransportStudentAttendanceLogs.Where(x => x.StudentDetailId == studentId && x.AttendanceDate == DateTime.Today).FirstOrDefault();
                    log.EvngEndTime = DateTime.Now.ToString("h:mm:ss tt");
                    db.Entry(log).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
            return Json("Present", JsonRequestBehavior.AllowGet);
        }
        public ActionResult AttendanceLog_Index()
        {
            if (Convert.ToInt32(Session["UserId"]) == 0) return RedirectToAction("Index", "Login");
            List<AttendanceLog> attendanceLogs = new List<AttendanceLog>();
            var logs = db.TransportStudentAttendanceLogs.ToList();
            foreach (var log in logs)
            {
                log.StudentDetail = db.StudentDetails.Find(log.StudentDetailId);
                string course = (from bc in db.BatchCourseLinks
                                 join c in db.Courses
                                 on bc.CourseId equals c.Id
                                 where bc.BatchId == log.StudentDetail.Batch
                                 select c.Name).SingleOrDefault();
                AttendanceLog attendanceLog = new AttendanceLog
                {
                    StudentId = log.StudentDetail.StudentDetailsId,
                    AttendanceDate = log.AttendanceDate.ToString(),
                    Batch = course + "/" + db.batches.Find(log.StudentDetail.Batch).Name,
                    EvngEndTime = log.EvngEndTime,
                    EvngStartTime = log.EvngStartTime,
                    MorngEndTime = log.MorngEndTime,
                    MorngStartTime = log.MorngStartTime,
                    RollNumber = log.StudentDetail.RollNumber,
                    Route = db.TransportRoutes.Find(log.RouteId).RouteName,
                    StudentName = log.StudentDetail.FirstName + " " + log.StudentDetail.MiddleName + " " + log.StudentDetail.LastName
                };
                attendanceLogs.Add(attendanceLog);
            }
            ViewBag.Routes = new SelectList(db.TransportRoutes.ToList(), "RouteId", "RouteName");
            AttendanceLogContext context = new AttendanceLogContext
            {
                AttendanceLogs = attendanceLogs
            };
            return View(context);
        }
        [HttpPost]
        public ActionResult GeneratePDF(AttendanceLogContext logContext)
        {
            if (Convert.ToInt32(Session["UserId"]) == 0) return RedirectToAction("Index", "Login");
            List<AttendanceLog> attendanceLogs = new List<AttendanceLog>();
            var routeId = Convert.ToInt32(logContext.RouteName);
            var logs = db.TransportStudentAttendanceLogs.Where(x => x.RouteId == routeId && x.AttendanceDate == logContext.AttendanceDate).ToList();
            foreach (var log in logs)
            {
                log.StudentDetail = db.StudentDetails.Find(log.StudentDetailId);
                string course = (from bc in db.BatchCourseLinks
                                 join c in db.Courses
                                 on bc.CourseId equals c.Id
                                 where bc.BatchId == log.StudentDetail.Batch
                                 select c.Name).SingleOrDefault();
                AttendanceLog attendanceLog = new AttendanceLog
                {
                    StudentId=log.StudentDetail.StudentDetailsId,
                    AttendanceDate = log.AttendanceDate.ToString(),
                    Batch = course+"/"+db.batches.Find(log.StudentDetail.Batch).Name,
                    EvngEndTime = log.EvngEndTime,
                    EvngStartTime = log.EvngStartTime,
                    MorngEndTime = log.MorngEndTime,
                    MorngStartTime = log.MorngStartTime,
                    RollNumber = log.StudentDetail.RollNumber,
                    Route = db.TransportRoutes.Find(log.RouteId).RouteName,
                    StudentName = log.StudentDetail.FirstName + " " + log.StudentDetail.MiddleName + " " + log.StudentDetail.LastName
                };
                attendanceLogs.Add(attendanceLog);
            }
            AttendanceLogContext context = new AttendanceLogContext
            {
                RouteName = db.TransportRoutes.Find(routeId).RouteName,
                AttendanceDate = logContext.AttendanceDate,
                AttendanceLogs = attendanceLogs
            };
            return View("Attendance_Report",context);
        }
        public ActionResult FilterAttendance(int Route, DateTime AttendanceDate)
        {
            if (Convert.ToInt32(Session["UserId"]) == 0) return RedirectToAction("Index", "Login");
            List<AttendanceLog> attendanceLogs = new List<AttendanceLog>();
            var logs = db.TransportStudentAttendanceLogs.Where(x => x.RouteId == Route && x.AttendanceDate == AttendanceDate).ToList();
            foreach (var log in logs)
            {
                log.StudentDetail = db.StudentDetails.Find(log.StudentDetailId);
                string course = (from bc in db.BatchCourseLinks
                                 join c in db.Courses
                                 on bc.CourseId equals c.Id
                                 where bc.BatchId == log.StudentDetail.Batch
                                 select c.Name).SingleOrDefault();
                AttendanceLog attendanceLog = new AttendanceLog
                {
                    StudentId = log.StudentDetail.StudentDetailsId,
                    AttendanceDate = log.AttendanceDate.ToString(),
                    Batch = course + "/" + db.batches.Find(log.StudentDetail.Batch).Name,
                    EvngEndTime = log.EvngEndTime,
                    EvngStartTime = log.EvngStartTime,
                    MorngEndTime = log.MorngEndTime,
                    MorngStartTime = log.MorngStartTime,
                    RollNumber = log.StudentDetail.RollNumber,
                    Route = db.TransportRoutes.Find(log.RouteId).RouteName,
                    StudentName = log.StudentDetail.FirstName + " " + log.StudentDetail.MiddleName + " " + log.StudentDetail.LastName
                };
                attendanceLogs.Add(attendanceLog);
            }
            ViewBag.Routes = new SelectList(db.TransportRoutes.ToList(), "RouteId", "RouteName");
            AttendanceLogContext context = new AttendanceLogContext
            {
                RouteName = db.TransportRoutes.Find(Route).RouteName,
                AttendanceDate = AttendanceDate,
                AttendanceLogs = attendanceLogs
            };
            if (attendanceLogs.Count() == 0)
                TempData["AttendanceLog"] = "No data found with this filtration";
            return View("AttendanceLog_Index", context);
        }
        public ActionResult Transport_Fee_Index()
        {
            if (Convert.ToInt32(Session["UserId"]) == 0) return RedirectToAction("Index", "Login");
            List<TransportFee> transportFees = new List<TransportFee>();
            using (conn)
            {
                conn.Open();
                cmd = new SqlCommand("select r.StudentId,r.RouteId,r.StopId,s.Fare,f.PaidStatusFlag,f.PaymentDate from TransportStudentRouteLink r join StudentFeesInvoice f on f.StudentDetailsId=r.StudentId join TransportStops s on r.StopId=s.Id left join Fee on Fee.FeeID=f.FeeId where Fee.Name='Transport Fee'", conn);
                dr = cmd.ExecuteReader();
                while(dr.Read())
                {
                    int studentId = Convert.ToInt32(dr["StudentId"].ToString());
                    var student = (from s in db.StudentDetails
                                   join bc in db.BatchCourseLinks
                                   on s.Batch equals bc.BatchId
                                   join b in db.batches
                                   on bc.BatchId equals b.Id
                                   join c in db.Courses
                                   on bc.CourseId equals c.Id
                                   where s.StudentDetailsId == studentId
                                   select new { StudentName = s.FirstName + " " + s.MiddleName + " " + s.LastName, s.StudentId, Course = c.Name, Batch = b.Name }).FirstOrDefault();
                    var route = (from tr in db.TransportStudentRouteLinks
                                 join r in db.TransportRoutes
                                 on tr.RouteId equals r.RouteId
                                 join s in db.TransportStops
                                 on tr.StopId equals s.Id
                                 where tr.StudentId ==studentId
                                 select new { r.RouteName, s.StopName }).FirstOrDefault();
                    TransportFee fee = new TransportFee
                    {
                        Batch = student.Batch,
                        Course=student.Course,
                        StudentDetailsId = Convert.ToInt32(dr["StudentId"].ToString()),
                        Fare = Convert.ToDouble(dr["Fare"].ToString()),
                        PaidDate=dr["PaymentDate"].ToString(),
                        StudentId = Convert.ToInt32(student.StudentId),
                        StudentName = student.StudentName
                    };
                    if (route != null)
                    {
                        fee.StopName = route.StopName;
                        fee.RouteName = route.RouteName;
                        }
                    if (Convert.ToInt32(dr["PaidStatusFlag"].ToString()) == 0) fee.Status = "Not Paid";
                    else if (Convert.ToInt32(dr["PaidStatusFlag"].ToString()) == 1) fee.Status = "Paid";
                    transportFees.Add(fee);
                }
            }
            return View(transportFees);
        }
        public ActionResult FeeReceipt(int studentId)
        {
            if (Convert.ToInt32(Session["UserId"]) == 0) return RedirectToAction("Index", "Login");
            TransportFee transportFee = new TransportFee();
            using (conn)
            {
                conn.Open();
                cmd = new SqlCommand("select r.StudentId,r.RouteId,r.StopId,s.Fare,f.PaidStatusFlag,f.PaymentDate from TransportStudentRouteLink r join StudentFeesInvoice f on f.StudentDetailsId=r.StudentId join TransportStops s on r.StopId=s.Id left join Fee on Fee.FeeID=f.FeeId where Fee.Name='Transport Fee' and r.StudentId='" + studentId+"'", conn);
                dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    var student = (from s in db.StudentDetails
                                   join bc in db.BatchCourseLinks
                                   on s.Batch equals bc.BatchId
                                   join b in db.batches
                                   on bc.BatchId equals b.Id
                                   join c in db.Courses
                                   on bc.CourseId equals c.Id
                                   where s.StudentDetailsId == studentId
                                   select new { StudentName = s.FirstName + " " + s.MiddleName + " " + s.LastName, s.StudentId, Course = c.Name, Batch = b.Name }).FirstOrDefault();
                    var route = (from tr in db.TransportStudentRouteLinks
                                 join r in db.TransportRoutes
                                 on tr.RouteId equals r.RouteId
                                 join s in db.TransportStops
                                 on tr.StopId equals s.Id
                                 where tr.StudentId == studentId
                                 select new { r.RouteName, s.StopName }).FirstOrDefault();
                    transportFee = new TransportFee
                    {
                        Batch = student.Batch,
                        Course = student.Course,
                        StopName = route.StopName,
                        RouteName = route.RouteName,
                        StudentDetailsId = Convert.ToInt32(dr["StudentId"].ToString()),
                        Fare = Convert.ToDouble(dr["Fare"].ToString()),
                        PaidDate = dr["PaymentDate"].ToString(),
                        StudentId = Convert.ToInt32(student.StudentId),
                        StudentName = student.StudentName
                    };
                    if (Convert.ToInt32(dr["PaidStatusFlag"].ToString()) == 0) transportFee.Status = "Not Paid";
                    else if (Convert.ToInt32(dr["PaidStatusFlag"].ToString()) == 1) transportFee.Status = "Paid";
                    
                }
            }
            return View(transportFee);
        }
    }
}