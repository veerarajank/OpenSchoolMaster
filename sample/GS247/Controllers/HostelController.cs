using GS247.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace GS247.Controllers
{
    public class HostelController : Controller
    {
        #region "DASHBOARD"

        public ActionResult Index()
        {
            if (sessionValidate())
            {
                List<HostelDetailCO> HostelDetailsList = new List<HostelDetailCO>();
                using (var context = new GS247Entities8())
                {
                    var OccupiedList = context.HostelRoomBedDetails.Where(x => x.AllocateFlag == 0).Distinct();

                    var dataList = (from hd in context.HostelDetails
                                    join hf in context.HostelFloorDetails on hd.HostelDetailsId equals hf.HostelDetailsId
                                    join hrd in context.HostelRoomDetails on hf.HostelFloorId equals hrd.HostelFloorId
                                    where OccupiedList.Any(x => x.HostelRoomId == hrd.HostelRoomId)
                                    select new
                                    {
                                        HostelDetailsId = hd.HostelDetailsId,
                                        Name = hd.Name,
                                        FloorNo = hf.FloorNo,
                                        RoomNo = hrd.RoomNo,
                                        HostelRoomId = hrd.HostelRoomId,
                                        NumberOfBeds = hrd.NumberOfBeds,
                                        CreatedDate = hrd.CreatedDate
                                    }).OrderByDescending(x => x.CreatedDate).Take(20).ToList();

                    dataList.ForEach(data =>
                    {
                        var SchoolCO = new SchoolCO();
                        var HostelDetails = new HostelDetailCO();
                        HostelDetails.HostelDetailsId = data.HostelDetailsId;
                        HostelDetails.Name = data.Name;
                        SchoolCO.FloorNo = data.FloorNo;
                        SchoolCO.NumberOfBets = data.NumberOfBeds;
                        SchoolCO.HostelRoomId = data.HostelRoomId;
                        SchoolCO.RoomNo = data.RoomNo;
                        int j = 1;
                        for (int c = 97; c <= 122; ++c)
                        {
                            if (j <= data.NumberOfBeds)
                            {
                                var name = (char)c;
                                SchoolCO.Beds += "    " + name.ToString();
                                j++;
                            }
                        }
                        SchoolCO.Availabilty = context.HostelRoomBedDetails.Where(x => x.AllocateFlag == 0 && x.HostelRoomId == data.HostelRoomId).Count();
                        HostelDetails.SchoolCO = SchoolCO;
                        HostelDetailsList.Add(HostelDetails);
                    });

                    ViewBag.MessDuesCount = context.StudentRoomDetails.Where(x => x.PaidStatusFlag == 0 && (x.StatusFlag == 0 || x.StatusFlag == null)).Count();
                    ViewBag.RequestCount = context.StudentHostelRoomChangelNotifications.Where(x => x.StatusFlag == 0).Count();
                    ViewBag.Count = context.HostelRoomBedDetails.Where(x => x.AllocateFlag == 0).Count();
                }
                ViewBag.HostelDetailsList = HostelDetailsList;
                return View();
            }
            else
                return RedirectToAction("Index", "Login");
        }

        #endregion

        #region "HOSTEL DETAILS"

        public ActionResult HostelDetails()
        {
            if (sessionValidate())
            {
                using (var context = new GS247Entities8())
                {
                    ViewBag.HostelList = context.HostelDetails.ToList();
                }
                return View();
            }
            else
                return RedirectToAction("Index", "Login");
        }

        public ActionResult CreateHostelDetails()
        {
            if (sessionValidate())
            {
                return View();
            }
            else
                return RedirectToAction("Index", "Login");
        }

        [HttpPost]
        public ActionResult CreateHostelDetails(HostelDetail collection)
        {
            if (sessionValidate())
            {
                using (var context = new GS247Entities8())
                {
                    var _templist = context.HostelDetails.Where(x => x.Name.ToUpper().Equals(collection.Name.ToUpper()) && x.HostelDetailsId != collection.HostelDetailsId).FirstOrDefault();
                    if (_templist != null)
                    {
                        ViewBag.Message = "Hostel Name already available";
                        if (collection.HostelDetailsId == 0)
                            return View();
                        else
                        {
                            var _templist1 = context.HostelDetails.Find(collection.HostelDetailsId);
                            return View("CreateHostelDetails", _templist1);
                        }
                    }
                    else
                    {
                        var _templist1 = context.HostelDetails.Find(collection.HostelDetailsId);
                        if (_templist1 == null)
                        {
                            collection.CreatedBy = Session["UserName"].ToString();
                            collection.CreatedDate = DateTime.Now;
                            context.HostelDetails.Add(collection);
                            context.SaveChanges();
                        }
                        else
                        {
                            _templist1.Name = collection.Name;
                            _templist1.Address = collection.Address;
                            _templist1.UpdatedDate = DateTime.Now;
                            _templist1.UpdatedBy = Session["UserName"].ToString();
                            context.Entry(_templist1).State = EntityState.Modified;
                            context.SaveChanges();
                        }
                    }
                }

                return RedirectToAction("HostelDetails", new RouteValueDictionary(
                       new { controller = "Hostel", action = "HostelDetails" }));
            }
            else
                return RedirectToAction("Index", "Login");
        }

        [HttpGet]
        public ActionResult EditHostelDetails(int HostelDetailsId)
        {
            if (sessionValidate())
            {
                using (var context = new GS247Entities8())
                {
                    var _templist = context.HostelDetails.Find(HostelDetailsId);
                    return View("CreateHostelDetails", _templist);
                }
            }
            else
                return RedirectToAction("Index", "Login");
        }

        [HttpPost]
        public ActionResult DeleteHostelDetails(int HostelDetailsId)
        {
            if (sessionValidate())
            {
                var message = "";
                using (var context = new GS247Entities8())
                {
                    var validationFlag = true;
                    var data = context.HostelFloorDetails.Where(x => x.HostelDetailsId == HostelDetailsId).Any();
                    if (data)
                        validationFlag = false;
                    else
                    {
                        data = context.HostelRoomDetails.Where(x => x.HostelDetailsId == HostelDetailsId).Any();
                        if (data)
                            validationFlag = false;
                        else
                        {
                            data = context.StudentRoomDetails.Where(x => x.HostelDetailsId == HostelDetailsId).Any();
                            if (data)
                                validationFlag = false;
                        }
                    }
                    if (validationFlag)
                    {
                        var HostelDetails = context.HostelDetails.Find(HostelDetailsId);
                        if (HostelDetails != null)
                        {
                            context.HostelDetails.Remove(HostelDetails);
                            context.SaveChanges();
                        }
                    }
                    else
                        message = "Hostel refrenced in other module can't be deleted";
                }
                return Json(new { Message = message });
            }
            else
                return RedirectToAction("Index", "Login");
        }

        public ActionResult ViewHostelDetails()
        {
            return View();
        }

        #endregion

        #region "MESS DETAILS"

        public ActionResult ManageMessDetails()
        {
            if (sessionValidate())
            {
                using (var context = new GS247Entities8())
                {
                    ViewBag.HostelMessDetails = context.HostelMessDetails.ToList();
                }
                return View();
            }
            else
                return RedirectToAction("Index", "Login");
        }

        public ActionResult CreateMessDetails()
        {
            if (sessionValidate())
            {
                return View();
            }
            else
                return RedirectToAction("Index", "Login");
        }

        [HttpPost]
        public ActionResult CreateMessDetails(HostelMessDetail collection)
        {
            if (sessionValidate())
            {
                using (var context = new GS247Entities8())
                {
                    var _templist = context.HostelMessDetails.Find(collection.HostelMessId);
                    if (_templist == null)
                    {
                        collection.CreatedBy = Session["UserName"].ToString();
                        collection.CreatedDate = DateTime.Now;
                        context.HostelMessDetails.Add(collection);
                        context.SaveChanges();
                    }
                    else
                    {
                        _templist.FoodPreference = collection.FoodPreference;
                        _templist.Amount = collection.Amount;
                        _templist.UpdatedDate = DateTime.Now;
                        _templist.UpdatedBy = Session["UserName"].ToString();
                        context.Entry(_templist).State = EntityState.Modified;
                        context.SaveChanges();
                    }

                }

                return RedirectToAction("ManageMessDetails", new RouteValueDictionary(
                       new { controller = "Hostel", action = "ManageMessDetails" }));
            }
            else
                return RedirectToAction("Index", "Login");
        }

        [HttpGet]
        public ActionResult EditMessDetails(int HostelMessId)
        {
            if (sessionValidate())
            {
                using (var context = new GS247Entities8())
                {
                    var _templist = context.HostelMessDetails.Find(HostelMessId);
                    return View("CreateMessDetails", _templist);
                }
            }
            else
                return RedirectToAction("Index", "Login");
        }

        [HttpPost]
        public ActionResult DeleteMessDetails(int HostelMessId)
        {
            if (sessionValidate())
            {
                var message = "";
                using (var context = new GS247Entities8())
                {
                    var validationFlag = true;
                    var data = context.StudentRoomDetails.Where(x => x.HostelDetailsId == HostelMessId).Any();
                    if (data)
                        validationFlag = false;
                    if (validationFlag)
                    {
                        var HostelMessDetails = context.HostelMessDetails.Find(HostelMessId);
                        if (HostelMessDetails != null)
                        {
                            context.HostelMessDetails.Remove(HostelMessDetails);
                            context.SaveChanges();
                        }
                    }
                    else
                        message = "Hostel mess refrenced in other module can't be deleted";
                }
                return Json(new { Message = message });
            }
            else
                return RedirectToAction("Index", "Login");
        }

        public ActionResult MessFeePaidDetails()
        {
            return View();
        }

        public ActionResult MessDetails()
        {
            if (sessionValidate())
            {
                List<StudentRoomDetailCO> StudentRoomDetailsList = new List<StudentRoomDetailCO>();
                using (var context = new GS247Entities8())
                {
                    var FieldsList = (from a in context.StudentRoomDetails
                                      join b in context.StudentDetails on a.StudentDetailsId equals b.StudentDetailsId
                                      join c in context.HostelDetails on a.HostelDetailsId equals c.HostelDetailsId
                                      where a.StatusFlag == 0
                                      select a);

                    var totalPages = (int)Math.Ceiling((decimal)FieldsList.Count() / (decimal)10);
                    ViewBag.PageCount = totalPages;
                    var studentList = FieldsList.Take(10).ToList();

                    studentList.ForEach(itr =>
                    {
                        var SchoolCO = new SchoolCO();

                        var StudentRoomDetails = new StudentRoomDetailCO();
                        StudentRoomDetails.StudentRoomDetailsId = itr.StudentRoomDetailsId;
                        StudentRoomDetails.HostelDetailsId = itr.HostelDetailsId;
                        StudentRoomDetails.HostelFloorId = itr.HostelFloorId;
                        StudentRoomDetails.StudentDetailsId = itr.StudentDetailsId;
                        StudentRoomDetails.HostelMessId = itr.HostelMessId;
                        StudentRoomDetails.Description = itr.Description;
                        StudentRoomDetails.Amount = itr.Amount;
                        StudentRoomDetails.HostelRoomId = itr.HostelRoomId;
                        StudentRoomDetails.RoomNo = itr.RoomNo;
                        StudentRoomDetails.HostelRoomBedId = itr.HostelRoomBedId;
                        StudentRoomDetails.BedName = itr.BedName;
                        StudentRoomDetails.AdmissionDate = itr.AdmissionDate;
                        StudentRoomDetails.VacateDate = itr.VacateDate;
                        StudentRoomDetails.CreatedBy = itr.CreatedBy;
                        StudentRoomDetails.CreatedDate = itr.CreatedDate;
                        StudentRoomDetails.UpdatedBy = itr.UpdatedBy;
                        StudentRoomDetails.UpdatedDate = itr.UpdatedDate;
                        StudentRoomDetails.VacateFlag = itr.VacateFlag;
                        StudentRoomDetails.PaidStatusFlag = itr.PaidStatusFlag;
                        StudentRoomDetails.StatusFlag = itr.StatusFlag;

                        var hostelData = context.HostelDetails.Find(itr.HostelDetailsId);
                        SchoolCO.Name = hostelData != null ? hostelData.Name : "";
                        var studentData = context.StudentDetails.Find(itr.StudentDetailsId);
                        SchoolCO.StudentName = studentData != null ? studentData.FirstName + " " + studentData.LastName : "";
                        var hostelMessData = context.HostelMessDetails.Find(itr.HostelMessId);
                        SchoolCO.FoodPreference = hostelMessData != null ? hostelMessData.FoodPreference : "";
                        var FloorDetails = context.HostelFloorDetails.Find(itr.HostelFloorId);
                        SchoolCO.FloorNo = FloorDetails != null ? FloorDetails.FloorNo : 0;
                        StudentRoomDetails.SchoolCO = SchoolCO;
                        StudentRoomDetailsList.Add(StudentRoomDetails);
                    });

                    ViewBag.StudentList = StudentRoomDetailsList;
                    ViewBag.CurrentPageIndex = 1;
                }
                return View();
            }
            else
                return RedirectToAction("Index", "Login");
        }

        [HttpPost]
        public ActionResult MessManageSearch(int? CurrentPageIndex)
        {
            if (sessionValidate())
            {
                List<StudentRoomDetailCO> StudentRoomDetailsList = new List<StudentRoomDetailCO>();

                using (var context = new GS247Entities8())
                {
                    var FieldsList = context.StudentRoomDetails.Where(x => x.StatusFlag == 0);
                    var totalPages = (int)Math.Ceiling((decimal)FieldsList.Count() / (decimal)10);
                    ViewBag.PageCount = totalPages;
                    var studentList = FieldsList.OrderByDescending(x => x.CreatedDate).Skip((Convert.ToInt16(CurrentPageIndex) - 1) * 10).Take(10).ToList();

                    studentList.ForEach(itr =>
                    {
                        var StudentRoomDetails = new StudentRoomDetailCO();
                        StudentRoomDetails.StudentRoomDetailsId = itr.StudentRoomDetailsId;
                        StudentRoomDetails.HostelDetailsId = itr.HostelDetailsId;
                        StudentRoomDetails.HostelFloorId = itr.HostelFloorId;
                        StudentRoomDetails.StudentDetailsId = itr.StudentDetailsId;
                        StudentRoomDetails.HostelMessId = itr.HostelMessId;
                        StudentRoomDetails.Description = itr.Description;
                        StudentRoomDetails.Amount = itr.Amount;
                        StudentRoomDetails.HostelRoomId = itr.HostelRoomId;
                        StudentRoomDetails.RoomNo = itr.RoomNo;
                        StudentRoomDetails.HostelRoomBedId = itr.HostelRoomBedId;
                        StudentRoomDetails.BedName = itr.BedName;
                        StudentRoomDetails.AdmissionDate = itr.AdmissionDate;
                        StudentRoomDetails.VacateDate = itr.VacateDate;
                        StudentRoomDetails.CreatedBy = itr.CreatedBy;
                        StudentRoomDetails.CreatedDate = itr.CreatedDate;
                        StudentRoomDetails.UpdatedBy = itr.UpdatedBy;
                        StudentRoomDetails.UpdatedDate = itr.UpdatedDate;
                        StudentRoomDetails.VacateFlag = itr.VacateFlag;
                        StudentRoomDetails.PaidStatusFlag = itr.PaidStatusFlag;
                        StudentRoomDetails.StatusFlag = itr.StatusFlag;

                        var SchoolCO = new SchoolCO();
                        var hostelData = context.HostelDetails.Find(itr.HostelDetailsId);
                        SchoolCO.Name = hostelData != null ? hostelData.Name : "";
                        var studentData = context.StudentDetails.Find(itr.StudentDetailsId);
                        SchoolCO.StudentName = studentData != null ? studentData.FirstName + " " + studentData.LastName : "";
                        var hostelMessData = context.HostelMessDetails.Find(itr.HostelMessId);
                        SchoolCO.FoodPreference = hostelMessData != null ? hostelMessData.FoodPreference : "";
                        var FloorDetails = context.HostelFloorDetails.Find(itr.HostelFloorId);
                        SchoolCO.FloorNo = FloorDetails != null ? FloorDetails.FloorNo : 0;
                        StudentRoomDetails.SchoolCO = SchoolCO;
                        StudentRoomDetailsList.Add(StudentRoomDetails);
                    });
                    ViewBag.StudentList = StudentRoomDetailsList;
                    ViewBag.CurrentPageIndex = CurrentPageIndex;
                }
                return View("MessDetails");
            }
            else
                return RedirectToAction("Index", "Login");
        }

        public ActionResult ViewMessDetails()
        {
            return View();
        }

        public ActionResult ChangeFood()
        {
            if (sessionValidate())
            {
                using (var context = new GS247Entities8())
                {
                    ViewBag.HostelList = context.HostelDetails.ToList();
                    ViewBag.HostelMessDetails = context.HostelMessDetails.ToList();
                }
                return View();
            }
            else
                return RedirectToAction("Index", "Login");
        }

        [HttpPost]
        public ActionResult ChangeFood(int StudentRoomDetailsId, int HostelMessId)
        {
            if (sessionValidate())
            {
                using (var context = new GS247Entities8())
                {
                    var StudentRoomDetails = context.StudentRoomDetails.Find(StudentRoomDetailsId);
                    if (StudentRoomDetails != null)
                    {
                        StudentRoomDetails.HostelMessId = HostelMessId;
                        context.Entry(StudentRoomDetails).State = EntityState.Modified;
                        context.SaveChanges();
                    }
                }
                return new EmptyResult();
            }
            else
                return RedirectToAction("Index", "Login");
        }

        #endregion

        #region "ROOM DETAILS"

        public ActionResult ManageRoomDetails()
        {
            if (sessionValidate())
            {
                return View();
            }
            else
                return RedirectToAction("Index", "Login");
        }

        [HttpPost]
        public JsonResult ManageRoomList(int RoomType, int? currentPage)
        {
            List<HostelDetailCO> HostelDetailsList = new List<HostelDetailCO>();
            var totalPages = 0;
            using (var context = new GS247Entities8())
            {
                if (RoomType < 2)
                {
                    var dataList = (from hd in context.HostelDetails
                                    join hf in context.HostelFloorDetails on hd.HostelDetailsId equals hf.HostelDetailsId
                                    join hrd in context.HostelRoomDetails on hf.HostelFloorId equals hrd.HostelFloorId
                                    select new
                                    {
                                        HostelDetailsId = hd.HostelDetailsId,
                                        Name = hd.Name,
                                        FloorNo = hf.FloorNo,
                                        RoomNo = hrd.RoomNo,
                                        HostelRoomId = hrd.HostelRoomId,
                                        NumberOfBeds = hrd.NumberOfBeds
                                    }).ToList();

                    dataList.ForEach(data =>
                    {
                        var SchoolCO = new SchoolCO();
                        var HostelDetails = new HostelDetailCO();
                        HostelDetails.HostelDetailsId = data.HostelDetailsId;
                        HostelDetails.Name = data.Name;
                        SchoolCO.FloorNo = data.FloorNo;
                        SchoolCO.NumberOfBets = data.NumberOfBeds;
                        SchoolCO.HostelRoomId = data.HostelRoomId;
                        SchoolCO.RoomNo = data.RoomNo;
                        int j = 1;
                        for (int c = 97; c <= 122; ++c)
                        {
                            if (j <= data.NumberOfBeds)
                            {
                                var name = (char)c;
                                SchoolCO.Beds += "    " + name.ToString();
                                j++;
                            }
                        }
                        SchoolCO.Availabilty = context.HostelRoomBedDetails.Where(x => x.AllocateFlag == 0 && x.HostelRoomId == data.HostelRoomId).Count();
                        HostelDetails.SchoolCO = SchoolCO;
                        HostelDetailsList.Add(HostelDetails);
                    });
                }
                else if (RoomType == 2)
                {
                    var OccupiedList = context.HostelRoomBedDetails.Where(x => x.AllocateFlag == 1).Distinct();

                    var dataList = (from hd in context.HostelDetails
                                    join hf in context.HostelFloorDetails on hd.HostelDetailsId equals hf.HostelDetailsId
                                    join hrd in context.HostelRoomDetails on hf.HostelFloorId equals hrd.HostelFloorId
                                    where OccupiedList.Any(x => x.HostelRoomId == hrd.HostelRoomId)
                                    select new
                                    {
                                        HostelDetailsId = hd.HostelDetailsId,
                                        Name = hd.Name,
                                        FloorNo = hf.FloorNo,
                                        RoomNo = hrd.RoomNo,
                                        HostelRoomId = hrd.HostelRoomId,
                                        NumberOfBeds = hrd.NumberOfBeds
                                    }).ToList();

                    dataList.ForEach(data =>
                    {
                        var SchoolCO = new SchoolCO();
                        var HostelDetails = new HostelDetailCO();
                        HostelDetails.HostelDetailsId = data.HostelDetailsId;
                        HostelDetails.Name = data.Name;
                        SchoolCO.FloorNo = data.FloorNo;
                        SchoolCO.NumberOfBets = data.NumberOfBeds;
                        SchoolCO.HostelRoomId = data.HostelRoomId;
                        SchoolCO.RoomNo = data.RoomNo;
                        int j = 1;
                        for (int c = 97; c <= 122; ++c)
                        {
                            if (j <= data.NumberOfBeds)
                            {
                                var name = (char)c;
                                SchoolCO.Beds += "    " + name.ToString();
                                j++;
                            }
                        }
                        SchoolCO.Availabilty = context.HostelRoomBedDetails.Where(x => x.AllocateFlag == 1 && x.HostelRoomId == data.HostelRoomId).Count();
                        HostelDetails.SchoolCO = SchoolCO;
                        HostelDetailsList.Add(HostelDetails);
                    });
                }
                else if (RoomType == 3)
                {
                    var OccupiedList = context.HostelRoomBedDetails.Where(x => x.AllocateFlag == 0).Distinct();

                    var dataList = (from hd in context.HostelDetails
                                    join hf in context.HostelFloorDetails on hd.HostelDetailsId equals hf.HostelDetailsId
                                    join hrd in context.HostelRoomDetails on hf.HostelFloorId equals hrd.HostelFloorId
                                    where OccupiedList.Any(x => x.HostelRoomId == hrd.HostelRoomId)
                                    select new
                                    {
                                        HostelDetailsId = hd.HostelDetailsId,
                                        Name = hd.Name,
                                        FloorNo = hf.FloorNo,
                                        RoomNo = hrd.RoomNo,
                                        HostelRoomId = hrd.HostelRoomId,
                                        NumberOfBeds = hrd.NumberOfBeds
                                    }).ToList();

                    dataList.ForEach(data =>
                    {
                        var SchoolCO = new SchoolCO();
                        var HostelDetails = new HostelDetailCO();
                        HostelDetails.HostelDetailsId = data.HostelDetailsId;
                        HostelDetails.Name = data.Name;
                        SchoolCO.FloorNo = data.FloorNo;
                        SchoolCO.NumberOfBets = data.NumberOfBeds;
                        SchoolCO.HostelRoomId = data.HostelRoomId;
                        SchoolCO.RoomNo = data.RoomNo;
                        int j = 1;
                        for (int c = 97; c <= 122; ++c)
                        {
                            if (j <= data.NumberOfBeds)
                            {
                                var name = (char)c;
                                SchoolCO.Beds += "    " + name.ToString();
                                j++;
                            }
                        }
                        SchoolCO.Availabilty = context.HostelRoomBedDetails.Where(x => x.AllocateFlag == 0 && x.HostelRoomId == data.HostelRoomId).Count();
                        HostelDetails.SchoolCO = SchoolCO;
                        HostelDetailsList.Add(HostelDetails);
                    });
                }
            }

            int totalCount = HostelDetailsList.Count();
            totalPages = (int)Math.Ceiling((decimal)totalCount / (decimal)10);
            HostelDetailsList = HostelDetailsList.Skip((Convert.ToInt16(currentPage) - 1) * 10).Take(10).ToList();
            return Json(new { HostelDetailsList = HostelDetailsList, TotalPages = totalPages }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AddRoomDetails()
        {
            if (sessionValidate())
            {
                using (var context = new GS247Entities8())
                {
                    ViewBag.HostelList = context.HostelDetails.ToList();
                }
                return View();
            }
            else
                return RedirectToAction("Index", "Login");
        }

        [HttpPost]
        public ActionResult AddRoomDetails(HostelFloorDetail collection)
        {
            if (sessionValidate())
            {
                var message = "";
                using (var context = new GS247Entities8())
                {
                    if (context.HostelFloorDetails.Where(x => x.FloorNo == collection.FloorNo && x.HostelDetailsId == collection.HostelDetailsId).Any())
                    {
                        message = "Floor already available";                        
                    }
                    else
                    {
                        var _templist = context.HostelFloorDetails.Find(collection.HostelFloorId);
                        if (_templist == null)
                        {
                            collection.CreatedBy = Session["UserName"].ToString();
                            collection.CreatedDate = DateTime.Now;
                            context.HostelFloorDetails.Add(collection);
                            context.SaveChanges();
                        }
                        else
                        {
                            _templist.HostelDetailsId = collection.HostelDetailsId;
                            _templist.FloorNo = collection.FloorNo;
                            _templist.NumberOfRooms = collection.NumberOfRooms;
                            _templist.UpdatedDate = DateTime.Now;
                            _templist.UpdatedBy = Session["UserName"].ToString();
                            context.Entry(_templist).State = EntityState.Modified;
                            context.SaveChanges();
                        }
                    }
                }

                return Json(new { Message = message, HostelFloorId = collection.HostelFloorId });
                //return RedirectToAction("CreateRoom", new RouteValueDictionary(
                //       new { controller = "Hostel", action = "CreateRoom", HostelFloorId = collection.HostelFloorId }));
            }
            else
                return RedirectToAction("Index", "Login");
        }

        public ActionResult CreateRoom(int? HostelFloorId)
        {
            if (sessionValidate())
            {
                using (var context = new GS247Entities8())
                {
                    var floor = context.HostelFloorDetails.Find(HostelFloorId);
                    if (floor != null)
                    {
                        ViewBag.FloorNo = floor.FloorNo;
                        ViewBag.HostelName = context.HostelDetails.Find(floor.HostelDetailsId).Name;
                        ViewBag.NumberOfRooms = floor.NumberOfRooms;
                        ViewBag.HostelFloorId = floor.HostelFloorId;
                        ViewBag.HostelDetailsId = floor.HostelDetailsId;
                    }
                }
                return View();
            }
            else
                return RedirectToAction("Index", "Login");
        }

        [HttpPost]
        public ActionResult CreateRoom(List<HostelRoomDetail> collection)
        {
            if (sessionValidate())
            {
                using (var context = new GS247Entities8())
                {
                    collection.ForEach(data =>
                    {

                        data.CreatedBy = Session["UserName"].ToString();
                        data.CreatedDate = DateTime.Now;
                        context.HostelRoomDetails.Add(data);
                        context.SaveChanges();

                        int j = 1;
                        for (int c = 97; c <= 122; ++c)
                        {
                            var name = (char)c;
                            if (j <= data.NumberOfBeds)
                            {
                                var hostelRoomBedDetails = new HostelRoomBedDetail();
                                hostelRoomBedDetails.HostelRoomId = data.HostelRoomId;
                                hostelRoomBedDetails.BedName = name.ToString();
                                hostelRoomBedDetails.AllocateFlag = 0;
                                hostelRoomBedDetails.CreatedBy = Session["UserName"].ToString();
                                hostelRoomBedDetails.CreatedDate = DateTime.Now;
                                context.HostelRoomBedDetails.Add(hostelRoomBedDetails);
                                context.SaveChanges();
                                j++;
                            }
                        }
                    });
                }

                return new EmptyResult();
            }
            else
                return RedirectToAction("Index", "Login");
        }

        public ActionResult UpdateRoom(int HostelRoomId)
        {
            if (sessionValidate())
            {
                using (var context = new GS247Entities8())
                {
                    ViewBag.HostelRoomId = HostelRoomId;
                    var RoomDetails = context.HostelRoomDetails.Find(HostelRoomId);
                    if (RoomDetails != null)
                    {
                        ViewBag.RoomNo = RoomDetails.RoomNo;
                        ViewBag.NumberOfBeds = RoomDetails.NumberOfBeds;
                        ViewBag.HostelDetailsId = RoomDetails.HostelDetailsId;
                        var FloorDetails = context.HostelFloorDetails.Find(RoomDetails.HostelFloorId);
                        ViewBag.FloorNo = FloorDetails != null ? FloorDetails.FloorNo : 0;
                    }
                }
                return View();
            }
            else
                return RedirectToAction("Index", "Login");
        }

        [HttpPost]
        public JsonResult UpdateRoom(int HostelRoomId, int? NumberOfBeds)
        {
            var msg = string.Empty;
            if (sessionValidate())
            {
                using (var context = new GS247Entities8())
                {
                    var count = context.HostelRoomBedDetails.Where(x => x.AllocateFlag == 1 && x.HostelRoomId == HostelRoomId).Count();
                    if (count <= NumberOfBeds)
                    {
                        var _templist = context.HostelRoomDetails.Find(HostelRoomId);

                        if (_templist != null)
                        {
                            _templist.NumberOfBeds = NumberOfBeds;
                            _templist.UpdatedDate = DateTime.Now;
                            _templist.UpdatedBy = Session["UserName"].ToString();
                            context.Entry(_templist).State = EntityState.Modified;
                            context.SaveChanges();

                            var bedList = context.HostelRoomBedDetails.Where(x => x.AllocateFlag == 0 && x.HostelRoomId == HostelRoomId).ToList();
                            bedList.ForEach(it =>
                            {
                                context.HostelRoomBedDetails.Remove(it);
                                context.SaveChanges();
                            });

                            int totalCOunt = NumberOfBeds.GetValueOrDefault() - count;

                            int j = 1;
                            for (int c = 97; c <= 122; ++c)
                            {
                                var name = (char)c;
                                if (j <= totalCOunt)
                                {
                                    var hostelRoomBedDetails = new HostelRoomBedDetail();
                                    hostelRoomBedDetails.HostelRoomId = _templist.HostelRoomId;
                                    hostelRoomBedDetails.BedName = name.ToString();
                                    hostelRoomBedDetails.AllocateFlag = 0;
                                    hostelRoomBedDetails.CreatedBy = Session["UserName"].ToString();
                                    hostelRoomBedDetails.CreatedDate = DateTime.Now;
                                    context.HostelRoomBedDetails.Add(hostelRoomBedDetails);
                                    context.SaveChanges();
                                    j++;
                                }
                            }
                        }
                    }
                    else
                        msg = "Room bed(s) already allocated cannot modified.";
                }
            }
            return Json(new { Message = msg }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult DeleteHostelRoomDetails(int HostelRoomId)
        {
            if (sessionValidate())
            {
                var message = "";
                using (var context = new GS247Entities8())
                {
                    var validationFlag = true;
                    var data = context.StudentRoomDetails.Where(x => x.HostelDetailsId == HostelRoomId).Any();
                    if (data)
                        validationFlag = false;

                    if (validationFlag)
                    {
                        var HostelBedDetails = context.HostelRoomBedDetails.Where(x => x.HostelRoomId == HostelRoomId).ToList();
                        HostelBedDetails.ForEach(it =>
                        {
                            context.HostelRoomBedDetails.Remove(it);
                            context.SaveChanges();

                        });

                        var HostelDetails = context.HostelRoomDetails.Find(HostelRoomId);
                        if (HostelDetails != null)
                        {
                            context.HostelRoomDetails.Remove(HostelDetails);
                            context.SaveChanges();
                        }
                    }
                    else
                        message = "Hostel room refrenced in other module can't be deleted";
                }
                return Json(new { Message = message });
            }
            else
                return RedirectToAction("Index", "Login");
        }

        #endregion

        #region "ROOM ALLOTMENT

        public ActionResult AllotRoom(int? StudentDetailsId)
        {
            if (sessionValidate())
            {
                using (var context = new GS247Entities8())
                {
                    ViewBag.HostelList = context.HostelDetails.ToList();
                    ViewBag.HostelMessDetails = context.HostelMessDetails.ToList();
                    if (StudentDetailsId != null)
                    {
                        var studentData = context.StudentDetails.Find(StudentDetailsId);
                        ViewBag.StudentName = studentData != null ? studentData.FirstName + " " + studentData.LastName : "";
                        ViewBag.StudentDetailsId = StudentDetailsId;
                    }
                    else
                    {
                        ViewBag.StudentDetailsId = null;
                        ViewBag.StudentName = "";
                    }
                }
                return View();
            }
            else
                return RedirectToAction("Index", "Login");
        }


        [HttpPost]
        public ActionResult AllotRoom(StudentRoomDetail collection)
        {
            if (sessionValidate())
            {
                using (var context = new GS247Entities8())
                {
                    collection.CreatedBy = Session["UserName"].ToString();
                    collection.CreatedDate = DateTime.Now;
                    collection.StatusFlag = 0;
                    collection.VacateFlag = 0;
                    collection.PaidStatusFlag = 0;
                    var messData = context.HostelMessDetails.Find(collection.HostelMessId);
                    collection.Amount = messData != null ? messData.Amount : 0;
                    context.StudentRoomDetails.Add(collection);
                    context.SaveChanges();

                    var data = context.StudentHostelRoomChangelNotifications.Where(x => x.StudentDetailsId == collection.StudentDetailsId && x.StatusFlag == 3).FirstOrDefault();
                    if (data != null)
                    {
                        data.StatusFlag = 1;
                        data.UpdatedDate = DateTime.Now;
                        data.UpdatedBy = Session["UserName"].ToString();
                        context.Entry(data).State = EntityState.Modified;
                        context.SaveChanges();
                    }
                }
                return RedirectToAction("StudentRoomAllotment", new RouteValueDictionary(
                       new { controller = "Hostel", action = "StudentRoomAllotment", StudentRoomDetailsId = collection.StudentRoomDetailsId }));
            }
            else
                return RedirectToAction("Index", "Login");
        }

        public ActionResult StudentRoomAllotment(int? StudentRoomDetailsId)
        {
            if (sessionValidate())
            {
                List<HostelRoomBedDetailCO> OccupiedList = new List<HostelRoomBedDetailCO>();
                using (var context = new GS247Entities8())
                {
                    var itr = context.StudentRoomDetails.Find(StudentRoomDetailsId);
                    var StudentRoomDetails = new StudentRoomDetailCO();
                    if (itr != null)
                    {
                        StudentRoomDetails.StudentRoomDetailsId = itr.StudentRoomDetailsId;
                        StudentRoomDetails.HostelDetailsId = itr.HostelDetailsId;
                        StudentRoomDetails.HostelFloorId = itr.HostelFloorId;
                        StudentRoomDetails.StudentDetailsId = itr.StudentDetailsId;
                        StudentRoomDetails.HostelMessId = itr.HostelMessId;
                        StudentRoomDetails.Description = itr.Description;
                        StudentRoomDetails.Amount = itr.Amount;
                        StudentRoomDetails.HostelRoomId = itr.HostelRoomId;
                        StudentRoomDetails.RoomNo = itr.RoomNo;
                        StudentRoomDetails.HostelRoomBedId = itr.HostelRoomBedId;
                        StudentRoomDetails.BedName = itr.BedName;
                        StudentRoomDetails.AdmissionDate = itr.AdmissionDate;
                        StudentRoomDetails.VacateDate = itr.VacateDate;
                        StudentRoomDetails.CreatedBy = itr.CreatedBy;
                        StudentRoomDetails.CreatedDate = itr.CreatedDate;
                        StudentRoomDetails.UpdatedBy = itr.UpdatedBy;
                        StudentRoomDetails.UpdatedDate = itr.UpdatedDate;
                        StudentRoomDetails.VacateFlag = itr.VacateFlag;
                        StudentRoomDetails.PaidStatusFlag = itr.PaidStatusFlag;
                        StudentRoomDetails.StatusFlag = itr.StatusFlag;

                        var SchoolCO = new SchoolCO();
                        var hostelData = context.HostelDetails.Find(itr.HostelDetailsId);
                        SchoolCO.Name = hostelData != null ? hostelData.Name : "";
                        var studentData = context.StudentDetails.Find(itr.StudentDetailsId);
                        SchoolCO.StudentName = studentData != null ? studentData.FirstName + " " + studentData.LastName : "";
                        var messData = context.HostelMessDetails.Find(itr.HostelMessId);
                        SchoolCO.FoodPreference = messData != null ? messData.FoodPreference : "";
                        StudentRoomDetails.SchoolCO = SchoolCO;

                        var daatList = (from rbd in context.HostelRoomBedDetails
                                        join rd in context.HostelRoomDetails on rbd.HostelRoomId equals rd.HostelRoomId
                                        join fd in context.HostelFloorDetails on rd.HostelFloorId equals fd.HostelFloorId
                                        where rbd.AllocateFlag == 0 && fd.HostelFloorId == itr.HostelFloorId && rd.HostelDetailsId == itr.HostelDetailsId
                                        select new
                                        {
                                            FloorNo = fd.FloorNo,
                                            RoomNo = rd.RoomNo,
                                            BedName = rbd.BedName,
                                            HostelRoomBedId = rbd.HostelRoomBedId
                                        }).ToList();

                        daatList.ForEach(item =>
                        {
                            var itemData = new HostelRoomBedDetailCO();
                            itemData.FloorNo = item.FloorNo;
                            itemData.RoomNo = item.RoomNo;
                            itemData.BedName = item.BedName;
                            itemData.HostelRoomBedId = item.HostelRoomBedId;
                            OccupiedList.Add(itemData);
                        });
                    }
                    ViewBag.StudentRoomDetails = StudentRoomDetails;
                    ViewBag.OccupiedList = OccupiedList;
                }
                return View();
            }
            else
                return RedirectToAction("Index", "Login");
        }

        [HttpPost]
        public ActionResult StudentRoomAllotcate(int StudentRoomDetailsId, int HostelRoomBedId)
        {
            if (sessionValidate())
            {
                using (var context = new GS247Entities8())
                {
                    var data = context.StudentRoomDetails.Find(StudentRoomDetailsId);
                    if (data != null)
                    {

                        if (data.HostelRoomBedId != null)
                        {
                            var hostelBedData = context.HostelRoomBedDetails.Find(data.HostelRoomBedId);
                            if (hostelBedData != null)
                            {
                                hostelBedData.AllocateFlag = 0;
                                context.Entry(hostelBedData).State = EntityState.Modified;
                                context.SaveChanges();
                            }
                        }
                        data.HostelRoomBedId = HostelRoomBedId;
                        var hostelBed = context.HostelRoomBedDetails.Find(HostelRoomBedId);
                        if (hostelBed != null)
                        {
                            data.HostelRoomId = hostelBed.HostelRoomId;
                            data.BedName = hostelBed.BedName;
                            var hostelRoom = context.HostelRoomDetails.Find(hostelBed.HostelRoomId);
                            data.RoomNo = hostelRoom != null ? hostelRoom.RoomNo : "";
                            hostelBed.AllocateFlag = 1;
                            context.Entry(hostelBed).State = EntityState.Modified;
                            context.SaveChanges();
                        }
                        data.AdmissionDate = DateTime.Now;
                        context.Entry(data).State = EntityState.Modified;
                        context.SaveChanges();

                        //var hostelRoomBed = context.HostelRoomBedDetails.Find(data.HostelRoomBedId);
                        //if (hostelRoomBed != null)
                        //{
                        //    hostelRoomBed.AllocateFlag = 0;
                        //    context.Entry(hostelRoomBed).State = EntityState.Modified;
                        //    context.SaveChanges();
                        //}
                    }
                }
                return new EmptyResult();
            }
            else
                return RedirectToAction("Index", "Login");
        }

        [HttpPost]
        public JsonResult GetStudentList(string currentPageIndex, string alphaValue, StudentAdvanceSearch studentAdvanceSearch)
        {
            List<StudentDetail> studentList = new List<StudentDetail>();
            List<StudentDetailCO> studentCOList = new List<StudentDetailCO>();

            var totalPages = 0;
            using (var context = new GS247Entities8())
            {
                var exists = context.StudentRoomDetails;
                if (alphaValue != "All")
                {
                    var studentData = (from sd in context.StudentDetails
                                       where (sd.DeleteFlag == 0 || sd.DeleteFlag == null) && (sd.OnlineStatus == 0 || sd.OnlineApplicationStatus == 1)
                                       && !exists.Any(x => x.StudentDetailsId == sd.StudentDetailsId)
                                       select sd).ToList();

                    var Data = studentData.Where(m => m.FirstName[0] == Convert.ToChar(alphaValue)).ToList();
                    int totalCount = Data.Count;
                    totalPages = (int)Math.Ceiling((decimal)totalCount / (decimal)10);
                    studentList = Data.OrderByDescending(x => x.CreatedDate).Skip((Convert.ToInt16(currentPageIndex) - 1) * 10).Take(10).ToList();
                }
                else
                {
                    var studentData = (from sd in context.StudentDetails
                                       where (sd.DeleteFlag == 0 || sd.DeleteFlag == null) && (sd.OnlineStatus == 0 || sd.OnlineApplicationStatus == 1)
                                       && !exists.Any(x => x.StudentDetailsId == sd.StudentDetailsId)
                                       select sd);

                    if (!string.IsNullOrEmpty(studentAdvanceSearch.NameTxt))
                        studentData = studentData.Where(x => x.FirstName.Contains(studentAdvanceSearch.NameTxt) || x.LastName.Contains(studentAdvanceSearch.NameTxt));
                    if (!string.IsNullOrEmpty(studentAdvanceSearch.AdmissionNumberTxt))
                        studentData = studentData.Where(x => x.AdmissionNo.Contains(studentAdvanceSearch.AdmissionNumberTxt));

                    int totalCount = studentData.Count();
                    totalPages = (int)Math.Ceiling((decimal)totalCount / (decimal)10);
                    studentList = studentData.OrderByDescending(x => x.CreatedDate).Skip((Convert.ToInt16(currentPageIndex) - 1) * 10).Take(10).ToList();
                }

                studentList.ForEach(itr =>
                {
                    var newData = new StudentDetailCO();
                    newData.StudentDetailsId = itr.StudentDetailsId;
                    newData.AdmissionNo = itr.AdmissionNo;
                    newData.AdmissionDate = itr.AdmissionDate;
                    newData.FirstName = itr.FirstName;
                    newData.LastName = itr.LastName;
                    newData.MiddleName = itr.MiddleName;
                    newData.StudentId = itr.StudentId;
                    newData.Batch = itr.Batch;
                    newData.DOB = itr.DOB;
                    newData.Gender = itr.Gender;
                    newData.Blood = itr.Blood;
                    newData.BirthPlace = itr.BirthPlace;
                    newData.Language = itr.Language;
                    newData.Nationality = itr.Nationality;
                    newData.Religion = itr.Religion;
                    newData.StudentCategory = itr.StudentCategory;
                    newData.PromitionalFlag = itr.PromitionalFlag;
                    newData.AddressLine1 = itr.AddressLine1;
                    newData.AddressLine2 = itr.AddressLine2;
                    newData.City = itr.City;
                    newData.State = itr.State;
                    newData.PinCode = itr.PinCode;
                    newData.PhoneNumber1 = itr.PhoneNumber1;
                    newData.PhoneNumber2 = itr.PhoneNumber2;
                    newData.Email = itr.Email;
                    newData.FileName = itr.FileName;
                    newData.Data = itr.Data;
                    newData.CreatedBy = itr.CreatedBy;
                    newData.CreatedDate = itr.CreatedDate;
                    newData.UpdatedBy = itr.UpdatedBy;
                    newData.UpdatedDate = itr.UpdatedDate;
                    newData.Country = itr.Country;
                    newData.DeleteFlag = itr.DeleteFlag;
                    newData.RollNumber = itr.RollNumber;
                    newData.Semester = itr.Semester;
                    newData.AcademicYear = itr.AcademicYear;
                    newData.AcademicYear = itr.AcademicYear;
                    newData.AcademicStatus = itr.AcademicStatus;
                    newData.Status = itr.Status;
                    newData.OnlineStatus = itr.OnlineStatus;
                    newData.OnlineApplicationStatus = itr.OnlineApplicationStatus;

                    var SchoolCO = new SchoolCO();
                    var batch = context.batches.Find(itr.Batch);
                    if (batch != null)
                    {
                        var course = (from c in context.Courses
                                      join cbl in context.BatchCourseLinks on c.Id equals cbl.CourseId
                                      join b in context.batches on cbl.BatchId equals b.Id
                                      where b.Id == batch.Id
                                      select c).FirstOrDefault();

                        SchoolCO.CourseBatchName = course != null ? course.Name + " / " + batch.Name : "";
                    }
                    else
                        SchoolCO.CourseBatchName = "";

                    newData.SchoolCO = SchoolCO;
                    studentCOList.Add(newData);
                });
            }
            return Json(new { StudentList = studentCOList, TotalPages = totalPages }, JsonRequestBehavior.AllowGet);
        }

        public class StudentAdvanceSearch
        {
            public string NameTxt { get; set; }

            public string RollnumberTxt { get; set; }

            public string AdmissionNumberTxt { get; set; }

            public string GenderTxt { get; set; }

            public string BloodGroupTxt { get; set; }

            public string CountryTxt { get; set; }

            public string DOBTxt { get; set; }

            public string AdmissionDateTxt { get; set; }

            public string StatusTxt { get; set; }

            public string AcademicYearTxt { get; set; }

            public string AdmissionDateRange { get; set; }

            public string DOBDateRange { get; set; }

            public string CourseId { get; set; }

            public string BatchId { get; set; }
        }

        [HttpPost]
        public JsonResult HostelFloorLoad(int HostelDetailsId)
        {
            List<HostelFloorDetail> HostelFloorList = new List<HostelFloorDetail>();
            using (var context = new GS247Entities8())
            {
                HostelFloorList = context.HostelFloorDetails.Where(x => x.HostelDetailsId == HostelDetailsId).ToList();
            }
            return Json(new { BatchList = HostelFloorList }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ViewDetails(int StudentRoomDetailsId)
        {
            if (sessionValidate())
            {
                List<HostelRoomBedDetail> OccupiedList = new List<HostelRoomBedDetail>();

                var StudentRoomDetails = new StudentRoomDetailCO();

                using (var context = new GS247Entities8())
                {
                    var itr = context.StudentRoomDetails.Find(StudentRoomDetailsId);
                    if (itr != null)
                    {
                        StudentRoomDetails.StudentRoomDetailsId = itr.StudentRoomDetailsId;
                        StudentRoomDetails.HostelDetailsId = itr.HostelDetailsId;
                        StudentRoomDetails.HostelFloorId = itr.HostelFloorId;
                        StudentRoomDetails.StudentDetailsId = itr.StudentDetailsId;
                        StudentRoomDetails.HostelMessId = itr.HostelMessId;
                        StudentRoomDetails.Description = itr.Description;
                        StudentRoomDetails.Amount = itr.Amount;
                        StudentRoomDetails.HostelRoomId = itr.HostelRoomId;
                        StudentRoomDetails.RoomNo = itr.RoomNo;
                        StudentRoomDetails.HostelRoomBedId = itr.HostelRoomBedId;
                        StudentRoomDetails.BedName = itr.BedName;
                        StudentRoomDetails.AdmissionDate = itr.AdmissionDate;
                        StudentRoomDetails.VacateDate = itr.VacateDate;
                        StudentRoomDetails.CreatedBy = itr.CreatedBy;
                        StudentRoomDetails.CreatedDate = itr.CreatedDate;
                        StudentRoomDetails.UpdatedBy = itr.UpdatedBy;
                        StudentRoomDetails.UpdatedDate = itr.UpdatedDate;
                        StudentRoomDetails.VacateFlag = itr.VacateFlag;
                        StudentRoomDetails.PaidStatusFlag = itr.PaidStatusFlag;
                        StudentRoomDetails.StatusFlag = itr.StatusFlag;

                        var SchoolCO = new SchoolCO();
                        var hostelData = context.HostelDetails.Find(itr.HostelDetailsId);
                        SchoolCO.Name = hostelData != null ? hostelData.Name : "";
                        var studentData = context.StudentDetails.Find(itr.StudentDetailsId);
                        SchoolCO.StudentName = studentData != null ? studentData.FirstName + " " + studentData.LastName : "";
                        var FloorDetails = context.HostelFloorDetails.Find(itr.HostelFloorId);
                        SchoolCO.FloorNo = FloorDetails != null ? FloorDetails.FloorNo : 0;
                        StudentRoomDetails.SchoolCO = SchoolCO;
                    }
                    ViewBag.StudentRoomDetails = StudentRoomDetails;
                }
                return View();
            }
            else
                return RedirectToAction("Index", "Login");
        }

        public ActionResult ViewRoomAllotment()
        {
            return View();
        }

        #endregion

        #region "CHANGE ROOM"

        public ActionResult ChangeRoom()
        {
            if (sessionValidate())
            {
                return View();
            }
            else
                return RedirectToAction("Index", "Login");
        }

        [HttpPost]
        public JsonResult GetHostelStudentList(string currentPageIndex, string alphaValue, StudentAdvanceSearch studentAdvanceSearch)
        {
            List<StudentDetail> studentList = new List<StudentDetail>();
            List<StudentDetailCO> studentCOList = new List<StudentDetailCO>();

            var totalPages = 0;
            using (var context = new GS247Entities8())
            {
                studentList = (from sd in context.StudentDetails
                               from shd in context.StudentRoomDetails
                               where (sd.DeleteFlag == 0 || sd.DeleteFlag == null) && (sd.OnlineStatus == 0 || sd.OnlineApplicationStatus == 1)
                               && sd.StudentDetailsId == shd.StudentDetailsId
                               select sd).ToList();

                if (alphaValue != "All")
                {
                    var query = studentList.AsEnumerable();
                    studentList = query.Where(m => m.FirstName[0] == Convert.ToChar(alphaValue)).ToList();
                }
                if (!string.IsNullOrEmpty(studentAdvanceSearch.NameTxt))
                    studentList = studentList.Where(x => x.FirstName.Contains(studentAdvanceSearch.NameTxt) || x.LastName.Contains(studentAdvanceSearch.NameTxt)).ToList();
                if (!string.IsNullOrEmpty(studentAdvanceSearch.AdmissionNumberTxt))
                    studentList = studentList.Where(x => x.AdmissionNo.Contains(studentAdvanceSearch.AdmissionNumberTxt)).ToList();

                int totalCount = studentList.Count();
                totalPages = (int)Math.Ceiling((decimal)totalCount / (decimal)10);
                studentList = studentList.OrderByDescending(x => x.CreatedDate).Skip((Convert.ToInt16(currentPageIndex) - 1) * 10).Take(10).ToList();

                studentList.ForEach(itr =>
                {
                    var newData = new StudentDetailCO();
                    newData.StudentDetailsId = itr.StudentDetailsId;
                    newData.AdmissionNo = itr.AdmissionNo;
                    newData.AdmissionDate = itr.AdmissionDate;
                    newData.FirstName = itr.FirstName;
                    newData.LastName = itr.LastName;
                    newData.MiddleName = itr.MiddleName;
                    newData.StudentId = itr.StudentId;
                    newData.Batch = itr.Batch;
                    newData.DOB = itr.DOB;
                    newData.Gender = itr.Gender;
                    newData.Blood = itr.Blood;
                    newData.BirthPlace = itr.BirthPlace;
                    newData.Language = itr.Language;
                    newData.Nationality = itr.Nationality;
                    newData.Religion = itr.Religion;
                    newData.StudentCategory = itr.StudentCategory;
                    newData.PromitionalFlag = itr.PromitionalFlag;
                    newData.AddressLine1 = itr.AddressLine1;
                    newData.AddressLine2 = itr.AddressLine2;
                    newData.City = itr.City;
                    newData.State = itr.State;
                    newData.PinCode = itr.PinCode;
                    newData.PhoneNumber1 = itr.PhoneNumber1;
                    newData.PhoneNumber2 = itr.PhoneNumber2;
                    newData.Email = itr.Email;
                    newData.FileName = itr.FileName;
                    newData.Data = itr.Data;
                    newData.CreatedBy = itr.CreatedBy;
                    newData.CreatedDate = itr.CreatedDate;
                    newData.UpdatedBy = itr.UpdatedBy;
                    newData.UpdatedDate = itr.UpdatedDate;
                    newData.Country = itr.Country;
                    newData.DeleteFlag = itr.DeleteFlag;
                    newData.RollNumber = itr.RollNumber;
                    newData.Semester = itr.Semester;
                    newData.AcademicYear = itr.AcademicYear;
                    newData.AcademicYear = itr.AcademicYear;
                    newData.AcademicStatus = itr.AcademicStatus;
                    newData.Status = itr.Status;
                    newData.OnlineStatus = itr.OnlineStatus;
                    newData.OnlineApplicationStatus = itr.OnlineApplicationStatus;

                    var SchoolCO = new SchoolCO();
                    var batch = context.batches.Find(itr.Batch);
                    if (batch != null)
                    {
                        var course = (from c in context.Courses
                                      join cbl in context.BatchCourseLinks on c.Id equals cbl.CourseId
                                      join b in context.batches on cbl.BatchId equals b.Id
                                      where b.Id == batch.Id
                                      select c).FirstOrDefault();

                        SchoolCO.CourseBatchName = course != null ? course.Name + " / " + batch.Name : "";
                    }
                    else
                        SchoolCO.CourseBatchName = "";

                    newData.SchoolCO = SchoolCO;
                    studentCOList.Add(newData);
                });
            }
            return Json(new { StudentList = studentCOList, TotalPages = totalPages }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ChangeAllotRoom(int StudentRoomDetailsId)
        {
            if (sessionValidate())
            {
                using (var context = new GS247Entities8())
                {
                    ViewBag.HostelList = context.HostelDetails.ToList();
                }
                ViewBag.StudentRoomDetailsId = StudentRoomDetailsId;
                return View();
            }
            else
                return RedirectToAction("Index", "Login");
        }

        [HttpPost]
        public JsonResult GetPendingRoomList(int HostelDetailsId, int HostelFloorId)
        {
            List<HostelRoomBedDetailCO> HostelRoomBedList = new List<HostelRoomBedDetailCO>();
            var totalPages = 0;
            using (var context = new GS247Entities8())
            {
                var daatList = (from rbd in context.HostelRoomBedDetails
                                join rd in context.HostelRoomDetails on rbd.HostelRoomId equals rd.HostelRoomId
                                join fd in context.HostelFloorDetails on rd.HostelFloorId equals fd.HostelFloorId
                                join hd in context.HostelDetails on rd.HostelDetailsId equals hd.HostelDetailsId
                                where rbd.AllocateFlag == 0 && fd.HostelFloorId == HostelFloorId && rd.HostelDetailsId == HostelDetailsId
                                select new
                                {
                                    Name = hd.Name,
                                    FloorNo = fd.FloorNo,
                                    RoomNo = rd.RoomNo,
                                    BedName = rbd.BedName,
                                    HostelRoomBedId = rbd.HostelRoomBedId
                                }).ToList();

                daatList.ForEach(item =>
                {
                    var itemData = new HostelRoomBedDetailCO();
                    itemData.FloorNo = item.FloorNo;
                    itemData.RoomNo = item.RoomNo;
                    itemData.BedName = item.BedName;
                    itemData.HostelRoomBedId = item.HostelRoomBedId;
                    itemData.Name = item.Name;
                    HostelRoomBedList.Add(itemData);
                });
            }

            return Json(new { HostelRoomBedList = HostelRoomBedList, TotalPages = totalPages }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult StudentChangeRoom()
        {
            if (sessionValidate())
            {
                var studentId = Session["UserName"].ToString();
                using (var context = new GS247Entities8())
                {
                    var studentDetails = (from a in context.StudentRoomDetails
                                          join b in context.StudentDetails on a.StudentDetailsId equals b.StudentDetailsId
                                          where b.StudentId == studentId
                                          select a).Any();

                    if (studentDetails)
                        ViewBag.RequestFlag = true;
                    else
                        ViewBag.RequestFlag = false;

                    ViewBag.HostelList = context.HostelDetails.ToList();
                }
                return View();
            }
            else
                return RedirectToAction("Index", "Login");
        }

        [HttpPost]
        public ActionResult StudentChangeRequest(int StudentDetailsId, int HostelDetailsId, int HostelFloorId, int HostelRoomBedId)
        {
            if (sessionValidate())
            {
                var studentId = Session["UserName"].ToString();
                using (var context = new GS247Entities8())
                {
                    var studentDetails = context.StudentDetails.Where(x => x.StudentId == studentId).FirstOrDefault();
                    if (studentDetails != null)
                    {
                        var collection = new StudentHostelRoomChangelNotification();
                        collection.StudentDetailsId = studentDetails.StudentDetailsId;
                        collection.HostelDetailsId = HostelDetailsId;
                        collection.HostelFloorId = HostelFloorId;
                        collection.HostelRoomBedId = HostelRoomBedId;
                        collection.SendDate = DateTime.Now;
                        collection.StatusFlag = 0;
                        collection.CreatedBy = Session["UserName"].ToString();
                        collection.CreatedDate = DateTime.Now;
                        context.StudentHostelRoomChangelNotifications.Add(collection);
                        context.SaveChanges();
                    }
                }
                return new EmptyResult();
            }
            else
                return RedirectToAction("Index", "Login");
        }

        [HttpPost]
        public ActionResult StudentRoomRequest()
        {
            if (sessionValidate())
            {
                var studentId = Session["UserName"].ToString();
                using (var context = new GS247Entities8())
                {
                    var studentDetails = context.StudentDetails.Where(x => x.StudentId == studentId).FirstOrDefault();
                    if (studentDetails != null)
                    {
                        if (!context.StudentHostelRoomChangelNotifications.Where(x => x.StudentDetailsId == studentDetails.StudentDetailsId).Any())
                        {
                            var collection = new StudentHostelRoomChangelNotification();
                            collection.StudentDetailsId = studentDetails.StudentDetailsId;
                            collection.SendDate = DateTime.Now;
                            collection.StatusFlag = 3;
                            collection.CreatedBy = Session["UserName"].ToString();
                            collection.CreatedDate = DateTime.Now;
                            context.StudentHostelRoomChangelNotifications.Add(collection);
                            context.SaveChanges();
                        }
                    }
                }
                return new EmptyResult();
            }
            else
                return RedirectToAction("Index", "Login");
        }
        #endregion

        #region "VACATE"

        public ActionResult Vacate()
        {
            if (sessionValidate())
            {
                return View();
            }
            else
                return RedirectToAction("Index", "Login");
        }

        [HttpPost]
        public ActionResult VacateStudent(int StudentRoomDetailsId)
        {
            if (sessionValidate())
            {
                using (var context = new GS247Entities8())
                {
                    var StudentRoomDetails = context.StudentRoomDetails.Find(StudentRoomDetailsId);
                    if (StudentRoomDetails != null)
                    {
                        StudentRoomDetails.VacateFlag = 1;
                        context.Entry(StudentRoomDetails).State = EntityState.Modified;
                        context.SaveChanges();

                        var hostelRoomBed = context.HostelRoomBedDetails.Find(StudentRoomDetails.HostelRoomBedId);
                        if (hostelRoomBed != null)
                        {
                            hostelRoomBed.AllocateFlag = 0;
                            context.Entry(hostelRoomBed).State = EntityState.Modified;
                            context.SaveChanges();
                        }
                    }
                }
                return new EmptyResult();
            }
            else
                return RedirectToAction("Index", "Login");
        }
        #endregion

        #region "NOTIFICATIONS"

        public ActionResult Notifications()
        {
            if (sessionValidate())
            {
                var list = new List<StudentHostelRoomChangelNotificationCO>();
                using (var context = new GS247Entities8())
                {
                    var data = context.StudentHostelRoomChangelNotifications.Where(x => x.StatusFlag == 0 || x.StatusFlag == 3).ToList();

                    data.ForEach(item =>
                    {

                        var StudentHostelRoomChangelNotifications = new StudentHostelRoomChangelNotificationCO();
                        StudentHostelRoomChangelNotifications.RoomChangeNotificationId = item.RoomChangeNotificationId;
                        StudentHostelRoomChangelNotifications.StudentDetailsId = item.StudentDetailsId;
                        StudentHostelRoomChangelNotifications.SendDate = item.SendDate;
                        StudentHostelRoomChangelNotifications.CreatedBy = item.CreatedBy;
                        StudentHostelRoomChangelNotifications.CreatedDate = item.CreatedDate;
                        StudentHostelRoomChangelNotifications.UpdatedBy = item.UpdatedBy;
                        StudentHostelRoomChangelNotifications.UpdatedDate = item.UpdatedDate;
                        StudentHostelRoomChangelNotifications.HostelDetailsId = item.HostelDetailsId;
                        StudentHostelRoomChangelNotifications.HostelFloorId = item.HostelFloorId;
                        StudentHostelRoomChangelNotifications.HostelRoomBedId = item.HostelRoomBedId;
                        StudentHostelRoomChangelNotifications.StatusFlag = item.StatusFlag;

                        var SchoolCO = new SchoolCO();
                        var studentData = context.StudentDetails.Find(item.StudentDetailsId);
                        SchoolCO.StudentName = studentData != null ? studentData.FirstName + " " + studentData.LastName : "";

                        var hostelData = context.HostelRoomBedDetails.Find(item.HostelRoomBedId);
                        if (hostelData != null)
                        {
                            SchoolCO.Beds = hostelData.BedName;
                            var hostelRoomData = context.HostelRoomDetails.Find(hostelData.HostelRoomId);
                            SchoolCO.RoomNo = hostelRoomData != null ? hostelRoomData.RoomNo : "";
                        }
                        StudentHostelRoomChangelNotifications.SchoolCO = SchoolCO;

                        list.Add(StudentHostelRoomChangelNotifications);
                    });

                    ViewBag.NotificationsList = list;
                }
                return View();
            }
            else
                return RedirectToAction("Index", "Login");
        }

        [HttpPost]
        public ActionResult StudentRoomAllotcates(int RoomChangeNotificationId, int Flag)
        {
            if (sessionValidate())
            {
                using (var context = new GS247Entities8())
                {
                    var data = context.StudentHostelRoomChangelNotifications.Find(RoomChangeNotificationId);
                    if (data != null)
                    {
                        if (Flag == 1)
                        {
                            var studentRoomData = context.StudentRoomDetails.Where(x => x.StatusFlag == 0
                                && x.VacateFlag == 0 && x.StudentDetailsId == data.StudentDetailsId).FirstOrDefault();

                            if (studentRoomData != null)
                            {
                                studentRoomData.HostelRoomBedId = data.HostelRoomBedId;
                                var hostelBed = context.HostelRoomBedDetails.Find(data.HostelRoomBedId);
                                if (hostelBed != null)
                                {
                                    studentRoomData.HostelRoomId = hostelBed.HostelRoomId;
                                    studentRoomData.BedName = hostelBed.BedName;
                                    var hostelRoom = context.HostelRoomDetails.Find(hostelBed.HostelRoomId);
                                    studentRoomData.RoomNo = hostelRoom != null ? hostelRoom.RoomNo : "";
                                    hostelBed.AllocateFlag = 1;
                                    context.Entry(hostelBed).State = EntityState.Modified;
                                    context.SaveChanges();
                                }
                                studentRoomData.HostelDetailsId = data.HostelDetailsId;
                                studentRoomData.HostelFloorId = data.HostelFloorId;
                                studentRoomData.UpdatedDate = DateTime.Now;
                                studentRoomData.UpdatedBy = Session["UserName"].ToString();
                                context.Entry(studentRoomData).State = EntityState.Modified;
                                context.SaveChanges();

                                var hostelRoomBed = context.HostelRoomBedDetails.Find(studentRoomData.HostelRoomBedId);
                                if (hostelRoomBed != null)
                                {
                                    hostelRoomBed.AllocateFlag = 0;
                                    context.Entry(hostelRoomBed).State = EntityState.Modified;
                                    context.SaveChanges();
                                }
                            }
                        }
                        data.StatusFlag = Flag;
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

        #endregion

        #region "MESS DUES"

        public ActionResult ViewMessDues()
        {
            if (sessionValidate())
            {
                var list = new List<StudentRoomDetailCO>();

                using (var context = new GS247Entities8())
                {
                    var data = context.StudentRoomDetails.Where(x => x.PaidStatusFlag == 0 && (x.StatusFlag == 0 || x.StatusFlag == null));

                    var totalPages = (int)Math.Ceiling((decimal)data.Count() / (decimal)10);
                    ViewBag.PageCount = totalPages;
                    var studentList = data.OrderByDescending(x => x.CreatedDate).Take(10).ToList();

                    studentList.ForEach(itr =>
                    {
                        var StudentRoomDetails = new StudentRoomDetailCO();
                        StudentRoomDetails.StudentRoomDetailsId = itr.StudentRoomDetailsId;
                        StudentRoomDetails.HostelDetailsId = itr.HostelDetailsId;
                        StudentRoomDetails.HostelFloorId = itr.HostelFloorId;
                        StudentRoomDetails.StudentDetailsId = itr.StudentDetailsId;
                        StudentRoomDetails.HostelMessId = itr.HostelMessId;
                        StudentRoomDetails.Description = itr.Description;
                        StudentRoomDetails.Amount = itr.Amount;
                        StudentRoomDetails.HostelRoomId = itr.HostelRoomId;
                        StudentRoomDetails.RoomNo = itr.RoomNo;
                        StudentRoomDetails.HostelRoomBedId = itr.HostelRoomBedId;
                        StudentRoomDetails.BedName = itr.BedName;
                        StudentRoomDetails.AdmissionDate = itr.AdmissionDate;
                        StudentRoomDetails.VacateDate = itr.VacateDate;
                        StudentRoomDetails.CreatedBy = itr.CreatedBy;
                        StudentRoomDetails.CreatedDate = itr.CreatedDate;
                        StudentRoomDetails.UpdatedBy = itr.UpdatedBy;
                        StudentRoomDetails.UpdatedDate = itr.UpdatedDate;
                        StudentRoomDetails.VacateFlag = itr.VacateFlag;
                        StudentRoomDetails.PaidStatusFlag = itr.PaidStatusFlag;
                        StudentRoomDetails.StatusFlag = itr.StatusFlag;

                        var SchoolCO = new SchoolCO();
                        var hostelData = context.HostelDetails.Find(itr.HostelDetailsId);
                        SchoolCO.Name = hostelData != null ? hostelData.Name : "";
                        var studentData = context.StudentDetails.Find(itr.StudentDetailsId);
                        SchoolCO.StudentName = studentData != null ? studentData.FirstName + " " + studentData.LastName : "";
                        var hostelMessData = context.HostelMessDetails.Find(itr.HostelMessId);
                        SchoolCO.FoodPreference = hostelMessData != null ? hostelMessData.FoodPreference : "";
                        var FloorDetails = context.HostelFloorDetails.Find(itr.HostelFloorId);
                        SchoolCO.FloorNo = FloorDetails != null ? FloorDetails.FloorNo : 0;
                        StudentRoomDetails.SchoolCO = SchoolCO;
                        list.Add(StudentRoomDetails);
                    });

                    ViewBag.StudentList = list;
                    ViewBag.CurrentPageIndex = 1;
                }
                return View();
            }
            else
                return RedirectToAction("Index", "Login");
        }

        [HttpPost]
        public ActionResult ViewMessDues(int? CurrentPageIndex)
        {
            if (sessionValidate())
            {
                var list = new List<StudentRoomDetailCO>();

                using (var context = new GS247Entities8())
                {
                    var data = context.StudentRoomDetails.Where(x => x.PaidStatusFlag == 0 && (x.StatusFlag == 0 || x.StatusFlag == null));

                    var totalPages = (int)Math.Ceiling((decimal)data.Count() / (decimal)10);
                    ViewBag.PageCount = totalPages;
                    var studentList = data.OrderByDescending(x => x.CreatedDate).Skip((Convert.ToInt16(CurrentPageIndex) - 1) * 10).Take(10).ToList();

                    studentList.ForEach(itr =>
                    {
                        var StudentRoomDetails = new StudentRoomDetailCO();
                        StudentRoomDetails.StudentRoomDetailsId = itr.StudentRoomDetailsId;
                        StudentRoomDetails.HostelDetailsId = itr.HostelDetailsId;
                        StudentRoomDetails.HostelFloorId = itr.HostelFloorId;
                        StudentRoomDetails.StudentDetailsId = itr.StudentDetailsId;
                        StudentRoomDetails.HostelMessId = itr.HostelMessId;
                        StudentRoomDetails.Description = itr.Description;
                        StudentRoomDetails.Amount = itr.Amount;
                        StudentRoomDetails.HostelRoomId = itr.HostelRoomId;
                        StudentRoomDetails.RoomNo = itr.RoomNo;
                        StudentRoomDetails.HostelRoomBedId = itr.HostelRoomBedId;
                        StudentRoomDetails.BedName = itr.BedName;
                        StudentRoomDetails.AdmissionDate = itr.AdmissionDate;
                        StudentRoomDetails.VacateDate = itr.VacateDate;
                        StudentRoomDetails.CreatedBy = itr.CreatedBy;
                        StudentRoomDetails.CreatedDate = itr.CreatedDate;
                        StudentRoomDetails.UpdatedBy = itr.UpdatedBy;
                        StudentRoomDetails.UpdatedDate = itr.UpdatedDate;
                        StudentRoomDetails.VacateFlag = itr.VacateFlag;
                        StudentRoomDetails.PaidStatusFlag = itr.PaidStatusFlag;
                        StudentRoomDetails.StatusFlag = itr.StatusFlag;

                        var SchoolCO = new SchoolCO();
                        var hostelData = context.HostelDetails.Find(itr.HostelDetailsId);
                        SchoolCO.Name = hostelData != null ? hostelData.Name : "";
                        var studentData = context.StudentDetails.Find(itr.StudentDetailsId);
                        SchoolCO.StudentName = studentData != null ? studentData.FirstName + " " + studentData.LastName : "";
                        var hostelMessData = context.HostelMessDetails.Find(itr.HostelMessId);
                        SchoolCO.FoodPreference = hostelMessData != null ? hostelMessData.FoodPreference : "";
                        var FloorDetails = context.HostelFloorDetails.Find(itr.HostelFloorId);
                        SchoolCO.FloorNo = FloorDetails != null ? FloorDetails.FloorNo : 0;
                        StudentRoomDetails.SchoolCO = SchoolCO;
                        list.Add(StudentRoomDetails);
                    });
                    ViewBag.StudentList = list;
                    ViewBag.CurrentPageIndex = CurrentPageIndex;
                }
                return View();
            }
            else
                return RedirectToAction("Index", "Login");
        }

        [HttpPost]
        public JsonResult HostelMessFees(int StudentRoomDetailsId)
        {
            var message = string.Empty;
            if (sessionValidate())
            {
                using (var context = new GS247Entities8())
                {
                    var data = context.StudentRoomDetails.Find(StudentRoomDetailsId);
                    if (data != null)
                    {
                        data.PaidStatusFlag = 2;
                        context.Entry(data).State = EntityState.Modified;
                        context.SaveChanges();

                        var fee = new Fee();
                        fee.Name = "Hostel Mess Fee - " + DateTime.Now.ToString("dd MMM yyyyy");
                        fee.Description = "Hostel Mess Fee";
                        fee.UserId = Convert.ToInt16(Session["UserId"]);
                        fee.CreatedBy = Session["UserName"].ToString();
                        fee.CreatedDate = DateTime.Now;
                        fee.PaymentTypeFlag = 2;
                        context.Fees.Add(fee);
                        context.SaveChanges();

                        var feeParticular = new FeeParticular();
                        feeParticular.FeeID = fee.FeeID;
                        feeParticular.Name = "Hostel Mess Fee";
                        feeParticular.Description = "Hostel Mess Fee";
                        feeParticular.Tax = null;
                        feeParticular.Discount = "0.00";
                        feeParticular.Type = null;
                        feeParticular.CreatedBy = Session["UserName"].ToString();
                        feeParticular.CreatedDate = DateTime.Now;
                        context.FeeParticulars.Add(feeParticular);
                        context.SaveChanges();

                        var feeAccess = new FeeAccess();
                        feeAccess.FeeParticularID = feeParticular.FeeParticularID;
                        feeAccess.Type = null;
                        feeAccess.AdmissionNumbers = null;
                        feeAccess.CourseID = null;
                        feeAccess.BatchID = null;
                        feeAccess.CategoryID = null;
                        feeAccess.Amount = data.Amount.GetValueOrDefault().ToString();
                        feeAccess.FeeID = fee.FeeID;
                        feeAccess.CreatedBy = Session["UserName"].ToString();
                        feeAccess.CreatedDate = DateTime.Now;
                        context.FeeAccesses.Add(feeAccess);
                        context.SaveChanges();

                        var StudentFeesInvoice1 = new StudentFeesInvoice();
                        StudentFeesInvoice1.StudentDetailsId = data.StudentDetailsId;
                        StudentFeesInvoice1.FeeId = fee.FeeID;
                        StudentFeesInvoice1.TotalAmount = data.Amount.GetValueOrDefault().ToString();
                        StudentFeesInvoice1.DiscountAmount = "0.00";
                        StudentFeesInvoice1.InvoiceAmount = data.Amount.GetValueOrDefault().ToString();
                        StudentFeesInvoice1.PaidStatusFlag = 0;

                        StudentFeesInvoice1.DueDate = DateTime.Now;
                        StudentFeesInvoice1.CreatedDate = DateTime.Now;
                        StudentFeesInvoice1.CreatedBy = Session["UserName"].ToString();
                        StudentFeesInvoice1.FeeAccessID = feeAccess.FeeAccessID;
                        StudentFeesInvoice1.FeeParticularID = feeParticular.FeeParticularID;
                        StudentFeesInvoice1.CancelFlag = 0;
                        StudentFeesInvoice1.CourseId = null;
                        StudentFeesInvoice1.BatchId = null;
                        context.StudentFeesInvoices.Add(StudentFeesInvoice1);
                        context.SaveChanges();

                        var studentInvoicePayment = new StudentInvoicePayment();
                        studentInvoicePayment.InvoiceId = StudentFeesInvoice1.InvoiceID;
                        studentInvoicePayment.FeeId = fee.FeeID;
                        studentInvoicePayment.FeeParticularID = feeParticular.FeeParticularID;
                        studentInvoicePayment.FeeAccessID = feeAccess.FeeAccessID;
                        studentInvoicePayment.InvoicePaymentAmount = data.Amount.GetValueOrDefault().ToString();
                        studentInvoicePayment.CreatedDate = DateTime.Now;
                        studentInvoicePayment.CreatedBy = Session["UserName"].ToString();
                        context.StudentInvoicePayments.Add(studentInvoicePayment);
                        context.SaveChanges();
                        message = "Success";
                    }
                }
            }
            return Json(new { Message = message });
        }

        #endregion

        #region "VIEW STUDENT DETAILS"

        public ActionResult ViewStudentDetails()
        {
            if (sessionValidate())
            {
                using (var context = new GS247Entities8())
                {
                    ViewBag.HostelList = context.HostelDetails.ToList();
                }
                return View();
            }
            else
                return RedirectToAction("Index", "Login");
        }

        [HttpPost]
        public JsonResult ViewStudentDetails(int? StudentDetailsId, int? HostelDetailsId)
        {
            var StudentRoomList = new List<StudentRoomDetail>();

            var list = new List<StudentRoomDetailCO>();

            if (sessionValidate())
            {
                using (var context = new GS247Entities8())
                {
                    if (HostelDetailsId != null && StudentDetailsId != null)
                    {
                        StudentRoomList = context.StudentRoomDetails.Where(x => x.StudentDetailsId == StudentDetailsId && x.HostelDetailsId == HostelDetailsId).ToList();
                    }
                    else if (StudentDetailsId != null)
                    {
                        StudentRoomList = context.StudentRoomDetails.Where(x => x.StudentDetailsId == StudentDetailsId).ToList();
                    }
                    else if (HostelDetailsId != null)
                    {
                        if (HostelDetailsId == 0)
                            StudentRoomList = context.StudentRoomDetails.Where(x => x.HostelDetailsId > 0).ToList();
                        else
                            StudentRoomList = context.StudentRoomDetails.Where(x => x.HostelDetailsId == HostelDetailsId).ToList();
                    }

                    StudentRoomList.ForEach(itr =>
                    {
                        var StudentRoomDetails = new StudentRoomDetailCO();
                        StudentRoomDetails.StudentRoomDetailsId = itr.StudentRoomDetailsId;
                        StudentRoomDetails.HostelDetailsId = itr.HostelDetailsId;
                        StudentRoomDetails.HostelFloorId = itr.HostelFloorId;
                        StudentRoomDetails.StudentDetailsId = itr.StudentDetailsId;
                        StudentRoomDetails.HostelMessId = itr.HostelMessId;
                        StudentRoomDetails.Description = itr.Description;
                        StudentRoomDetails.Amount = itr.Amount;
                        StudentRoomDetails.HostelRoomId = itr.HostelRoomId;
                        StudentRoomDetails.RoomNo = itr.RoomNo;
                        StudentRoomDetails.HostelRoomBedId = itr.HostelRoomBedId;
                        StudentRoomDetails.BedName = itr.BedName;
                        StudentRoomDetails.AdmissionDate = itr.AdmissionDate;
                        StudentRoomDetails.VacateDate = itr.VacateDate;
                        StudentRoomDetails.CreatedBy = itr.CreatedBy;
                        StudentRoomDetails.CreatedDate = itr.CreatedDate;
                        StudentRoomDetails.UpdatedBy = itr.UpdatedBy;
                        StudentRoomDetails.UpdatedDate = itr.UpdatedDate;
                        StudentRoomDetails.VacateFlag = itr.VacateFlag;
                        StudentRoomDetails.PaidStatusFlag = itr.PaidStatusFlag;
                        StudentRoomDetails.StatusFlag = itr.StatusFlag;

                        var SchoolCO = new SchoolCO();
                        var hostelData = context.HostelDetails.Find(itr.HostelDetailsId);
                        SchoolCO.Name = hostelData != null ? hostelData.Name : "";
                        var studentData = context.StudentDetails.Find(itr.StudentDetailsId);
                        SchoolCO.StudentName = studentData != null ? studentData.FirstName + " " + studentData.LastName : "";
                        var FloorDetails = context.HostelFloorDetails.Find(itr.HostelFloorId);
                        SchoolCO.FloorNo = FloorDetails != null ? FloorDetails.FloorNo : 0;
                        var fees = context.StudentHostelFeesPaidDetails.Where(x => x.StudentRoomDetailsId == itr.StudentRoomDetailsId).FirstOrDefault();
                        SchoolCO.StatusFlagDesc = fees != null ? "1" : "0";
                        StudentRoomDetails.SchoolCO = SchoolCO;
                        list.Add(StudentRoomDetails);

                    });
                }
            }
            return Json(new { StudentRoomList = list }, JsonRequestBehavior.AllowGet);
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

        public partial class HostelDetailCO
        {
            public int HostelDetailsId { get; set; }
            public string Name { get; set; }
            public string Address { get; set; }
            public string CreatedBy { get; set; }
            public System.DateTime CreatedDate { get; set; }
            public string UpdatedBy { get; set; }
            public Nullable<System.DateTime> UpdatedDate { get; set; }

            [System.ComponentModel.DataAnnotations.Schema.NotMapped]

            public virtual SchoolCO SchoolCO { get; set; }
        }

        public partial class StudentRoomDetailCO
        {
            public int StudentRoomDetailsId { get; set; }
            public Nullable<int> HostelDetailsId { get; set; }
            public Nullable<int> HostelFloorId { get; set; }
            public Nullable<int> StudentDetailsId { get; set; }
            public Nullable<int> HostelMessId { get; set; }
            public string Description { get; set; }
            public Nullable<decimal> Amount { get; set; }
            public Nullable<int> HostelRoomId { get; set; }
            public string RoomNo { get; set; }
            public Nullable<int> HostelRoomBedId { get; set; }
            public string BedName { get; set; }
            public Nullable<System.DateTime> AdmissionDate { get; set; }
            public Nullable<System.DateTime> VacateDate { get; set; }
            public string CreatedBy { get; set; }
            public System.DateTime CreatedDate { get; set; }
            public string UpdatedBy { get; set; }
            public Nullable<System.DateTime> UpdatedDate { get; set; }
            public Nullable<int> VacateFlag { get; set; }
            public Nullable<int> PaidStatusFlag { get; set; }
            public Nullable<int> StatusFlag { get; set; }

            [System.ComponentModel.DataAnnotations.Schema.NotMapped]

            public virtual SchoolCO SchoolCO { get; set; }
        }

        public partial class StudentHostelRoomChangelNotificationCO
        {
            public int RoomChangeNotificationId { get; set; }
            public Nullable<int> StudentDetailsId { get; set; }
            public Nullable<System.DateTime> SendDate { get; set; }
            public string CreatedBy { get; set; }
            public System.DateTime CreatedDate { get; set; }
            public string UpdatedBy { get; set; }
            public Nullable<System.DateTime> UpdatedDate { get; set; }
            public Nullable<int> HostelDetailsId { get; set; }
            public Nullable<int> HostelFloorId { get; set; }
            public Nullable<int> HostelRoomBedId { get; set; }
            public Nullable<int> StatusFlag { get; set; }

            [System.ComponentModel.DataAnnotations.Schema.NotMapped]

            public virtual SchoolCO SchoolCO { get; set; }
        }

        public partial class HostelRoomBedDetailCO
        {
            public int HostelRoomBedId { get; set; }
            public Nullable<int> HostelRoomId { get; set; }
            public string BedName { get; set; }
            public Nullable<int> AllocateFlag { get; set; }
            public string CreatedBy { get; set; }
            public System.DateTime CreatedDate { get; set; }
            public string UpdatedBy { get; set; }
            public Nullable<System.DateTime> UpdatedDate { get; set; }

            public virtual SchoolCO SchoolCO { get; set; }

            public virtual int? FloorNo { get; set; }

            public virtual string RoomNo { get; set; }

            public virtual string Name { get; set; }
        }

        public partial class StudentDetailCO
        {
            public int StudentDetailsId { get; set; }
            public string AdmissionNo { get; set; }
            public Nullable<System.DateTime> AdmissionDate { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string MiddleName { get; set; }
            public string StudentId { get; set; }
            public Nullable<int> Batch { get; set; }
            public Nullable<System.DateTime> DOB { get; set; }
            public Nullable<int> Gender { get; set; }
            public Nullable<int> Blood { get; set; }
            public string BirthPlace { get; set; }
            public string Language { get; set; }
            public Nullable<int> Nationality { get; set; }
            public string Religion { get; set; }
            public Nullable<int> StudentCategory { get; set; }
            public Nullable<int> PromitionalFlag { get; set; }
            public string AddressLine1 { get; set; }
            public string AddressLine2 { get; set; }
            public string City { get; set; }
            public string State { get; set; }
            public string PinCode { get; set; }
            public string PhoneNumber1 { get; set; }
            public string PhoneNumber2 { get; set; }
            public string Email { get; set; }
            public string FileName { get; set; }
            public string Data { get; set; }
            public string CreatedBy { get; set; }
            public System.DateTime CreatedDate { get; set; }
            public string UpdatedBy { get; set; }
            public Nullable<System.DateTime> UpdatedDate { get; set; }
            public Nullable<int> Country { get; set; }
            public Nullable<int> DeleteFlag { get; set; }
            public string RollNumber { get; set; }
            public string Semester { get; set; }
            public string AcademicYear { get; set; }
            public Nullable<int> AcademicStatus { get; set; }
            public Nullable<int> Status { get; set; }
            public Nullable<int> OnlineStatus { get; set; }
            public Nullable<int> OnlineApplicationStatus { get; set; }
            public virtual SchoolCO SchoolCO { get; set; }
        }

        #endregion
    }
}