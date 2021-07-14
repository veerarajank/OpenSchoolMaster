using GS247.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Dynamic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace GS247.Controllers
{
    public class PurchaseController : Controller
    {

        #region "ITEMS"

        public ActionResult PurchaseItems()
        {
            if (sessionValidate())
            {
                int id = Convert.ToInt32(Session["UserId"].ToString());
                using (var context = new GS247Entities8())
                {
                    var FieldsList = context.PurchaseItems.Where(x => x.UserId > 0);
                    int totalCount = FieldsList.Count();
                    var totalPages = (int)Math.Ceiling((decimal)totalCount / (decimal)10);
                    ViewBag.PageCount = totalPages;
                    ViewBag.CurrentPageIndex = 1;
                    ViewBag.PurchaseItemsList = FieldsList.OrderByDescending(x => x.CreatedDate).Take(10).ToList();
                }
                return View();
            }
            else
                return RedirectToAction("Index", "Login");
        }

        [HttpPost]
        public ActionResult CreatePurchaseItems(PurchaseItem collection)
        {
            if (sessionValidate())
            {
                int id = Convert.ToInt32(Session["UserId"].ToString());
                using (var context = new GS247Entities8())
                {
                    var _templist = context.PurchaseItems.Where(x => x.ItemName.ToUpper().Equals(collection.ItemName.ToUpper()) && x.ItemId != collection.ItemId).FirstOrDefault();
                    if (_templist != null)
                    {
                        ViewBag.Message = "Item already available";
                        return View();
                    }                        
                    else
                    {

                        collection.CreatedBy = Session["UserName"].ToString();
                        collection.CreatedDate = DateTime.Now;
                        collection.UserId = id;
                        context.PurchaseItems.Add(collection);
                        context.SaveChanges();

                        return RedirectToAction("PurchaseItems", new RouteValueDictionary(
                      new { controller = "Purchase", action = "PurchaseItems" }));
                    }
                }
            }
            else
                return RedirectToAction("Index", "Login");
        }

        [HttpGet]
        public ActionResult CreatePurchaseItems(int? ItemId)
        {
            if (sessionValidate())
            {
                PurchaseItem _templist = new PurchaseItem();
                using (var context = new GS247Entities8())
                {
                    _templist = context.PurchaseItems.Find(ItemId);
                }
                return View(_templist);
            }
            else
                return RedirectToAction("Index", "Login");
        }

        [HttpPost]
        public ActionResult UpdatePurchaseItems(PurchaseItem collection)
        {
            if (sessionValidate())
            {
                using (var context = new GS247Entities8())
                {
                    var templist = context.PurchaseItems.Where(x => x.ItemName.ToUpper().Equals(collection.ItemName.ToUpper()) && x.ItemId != collection.ItemId).FirstOrDefault();
                    if (templist != null)
                    {
                        var _templist = context.PurchaseItems.Find(collection.ItemId);
                        ViewBag.Message = "Item already available";
                        return View("CreatePurchaseItems", _templist);
                    } 
                    else
                    {
                        var _templist = context.PurchaseItems.Find(collection.ItemId);
                        if (_templist != null)
                        {
                            _templist.ItemName = collection.ItemName;
                            _templist.UpdatedDate = DateTime.Now;
                            _templist.UpdatedBy = Session["UserName"].ToString();
                            context.Entry(_templist).State = EntityState.Modified;
                            context.SaveChanges();
                        }
                        return RedirectToRoute(new { controller = "Purchase", action = "PurchaseItems" });
                    }         
                }                
            }
            else
                return RedirectToAction("Index", "Login");
        }

        [HttpPost]
        public ActionResult DeletePurchaseItems(int ItemId)
        {
            using (var context = new GS247Entities8())
            {
                var _templist = context.PurchaseItems.Find(ItemId);
                if (_templist != null)
                {
                    context.PurchaseItems.Remove(_templist);
                    context.SaveChanges();
                }
            }
            return new EmptyResult();
        }

        public ActionResult SearchPurchaseItems(SearchSFields SearchSFields)
        {
            if (sessionValidate())
            {
                using (var context = new GS247Entities8())
                {
                    int id = Convert.ToInt32(Session["UserId"].ToString());
                    var FieldsList = context.PurchaseItems.Where(x => x.UserId > 0);
                    if (!string.IsNullOrEmpty(SearchSFields.ItemName))
                    {
                        FieldsList = FieldsList.Where(x => (x.ItemName.Contains(SearchSFields.ItemName)));
                    }

                    int totalCount = FieldsList.Count();
                    var totalPages = (int)Math.Ceiling((decimal)totalCount / (decimal)10);
                    ViewBag.PageCount = totalPages;
                    ViewBag.CurrentPageIndex = Convert.ToInt16(SearchSFields.CurrentPage);
                    ViewBag.PurchaseItemsList = FieldsList.OrderByDescending(x => x.CreatedDate).Skip((Convert.ToInt16(SearchSFields.CurrentPage) - 1) * 10).Take(10).ToList();
                }
                return View("PurchaseItems");
            }
            else
                return RedirectToAction("Index", "Login");
        }

        [HttpPost]
        public JsonResult ValidateItems(string Name, int? ItemId)
        {
            string Message = string.Empty;

            using (var context = new GS247Entities8())
            {
                var _templist = context.PurchaseItems.Where(x => x.ItemName.ToUpper().Equals(Name.ToUpper()) && x.ItemId != ItemId).FirstOrDefault();
                if (_templist != null)
                    Message = "Item already available";
            }

            return Json(new { Message = Message }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region "VENDOR DETAILS"

        public ActionResult VendorDetails()
        {
            if (sessionValidate())
            {
                int id = Convert.ToInt32(Session["UserId"].ToString());
                using (var context = new GS247Entities8())
                {
                    var FieldsList = context.PurchaseVendorDetails.Where(x => x.UserId > 0);
                    int totalCount = FieldsList.Count();
                    var totalPages = (int)Math.Ceiling((decimal)totalCount / (decimal)10);
                    ViewBag.PageCount = totalPages;
                    ViewBag.CurrentPageIndex = 1;
                    ViewBag.PurchaseVendorList = FieldsList.OrderByDescending(x => x.CreatedDate).Take(10).ToList();
                }
                return View();
            }
            else
                return RedirectToAction("Index", "Login");
        }

        public ActionResult SearchVendorDetails(SearchSFields SearchSFields)
        {
            if (sessionValidate())
            {
                using (var context = new GS247Entities8())
                {
                    int id = Convert.ToInt32(Session["UserId"].ToString());
                    var FieldsList = context.PurchaseVendorDetails.Where(x => x.UserId > 0);
                    int totalCount = FieldsList.Count();
                    var totalPages = (int)Math.Ceiling((decimal)totalCount / (decimal)10);
                    ViewBag.PageCount = totalPages;
                    ViewBag.CurrentPageIndex = Convert.ToInt16(SearchSFields.CurrentPage);
                    var VendorDetails = FieldsList.OrderByDescending(x => x.CreatedDate).Skip((Convert.ToInt16(SearchSFields.CurrentPage) - 1) * 10).Take(10).ToList();
                }
                return View("PurchaseItems");
            }
            else
                return RedirectToAction("Index", "Login");
        }

        public ActionResult CreateVendorDetails()
        {
            if (sessionValidate())
            {
                using (var context = new GS247Entities8())
                {
                    ViewBag.CurrencyList = context.Currencies.ToList();
                    ViewBag.CountryList = context.countries.ToList();
                }
                return View();
            }
            else
                return RedirectToAction("Index", "Login");
        }

        public ActionResult CreateProductDetails(int VendorDetailId)
        {
            if (sessionValidate())
            {
                var list = new List<PurchaseProductDetailCO>();

                using (var context = new GS247Entities8())
                {
                    var productDetails = context.PurchaseProductDetails.Where(x => x.VendorDetailId == VendorDetailId).ToList();
                    var PurchaseItemsList = (from sdt in context.PurchaseItems.ToList()
                                             where !productDetails.Any(x => x.ItemId == sdt.ItemId)
                                             select sdt).Distinct().ToList();

                    productDetails.ForEach(data =>
                    {
                        var PurchaseProductDetail = new PurchaseProductDetailCO();
                        PurchaseProductDetail.ProductId = data.ProductId;
                        PurchaseProductDetail.VendorDetailId = data.VendorDetailId;
                        PurchaseProductDetail.ItemId = data.ItemId;
                        PurchaseProductDetail.Price = data.Price;
                        PurchaseProductDetail.Description = data.Description;
                        PurchaseProductDetail.CreatedBy = data.CreatedBy;
                        PurchaseProductDetail.UpdatedBy = data.UpdatedBy;
                        PurchaseProductDetail.CreatedDate = data.CreatedDate;
                        PurchaseProductDetail.UpdatedDate = data.UpdatedDate;

                        var SchoolCO = new SchoolCO();
                        var items = context.PurchaseItems.Find(data.ItemId);
                        SchoolCO.Name = items != null ? items.ItemName : "";
                        PurchaseProductDetail.SchoolCO = SchoolCO;
                        list.Add(PurchaseProductDetail);
                    });
                    ViewBag.ProductList = list;
                    ViewBag.PurchaseItemsList = PurchaseItemsList;
                    ViewBag.VendorDetailId = VendorDetailId;
                }
                return View();
            }
            else
                return RedirectToAction("Index", "Login");
        }

        [HttpPost]
        public ActionResult CreateVendorDetails(PurchaseVendorDetail collection)
        {
            if (sessionValidate())
            {
                using (var context = new GS247Entities8())
                {
                    int id = Convert.ToInt32(Session["UserId"].ToString());
                    var _templist = context.PurchaseVendorDetails.Find(collection.VendorDetailId);
                    if (_templist == null)
                    {
                        collection.CreatedBy = Session["UserName"].ToString();
                        collection.CreatedDate = DateTime.Now;
                        collection.UserId = id;
                        context.PurchaseVendorDetails.Add(collection);
                        context.SaveChanges();
                    }
                    else
                    {
                        _templist.FirstName = collection.FirstName;
                        _templist.LastName = collection.LastName;
                        _templist.AddressLine1 = collection.AddressLine1;
                        _templist.AddressLine2 = collection.AddressLine2;
                        _templist.City = collection.City;
                        _templist.State = collection.State;
                        _templist.PhoneNumber = collection.PhoneNumber;
                        _templist.OfficeNumber = collection.OfficeNumber;
                        _templist.Email = collection.Email;
                        _templist.CurrencyType = collection.CurrencyType;
                        _templist.CompanyName = collection.CompanyName;
                        _templist.VATNumber = collection.VATNumber;
                        _templist.CSTNumber = collection.CSTNumber;
                        _templist.UpdatedDate = DateTime.Now;
                        _templist.UpdatedBy = Session["UserName"].ToString();
                        context.Entry(_templist).State = EntityState.Modified;
                        context.SaveChanges();
                    }

                }
                return RedirectToAction("CreateProductDetails", new RouteValueDictionary(
                             new { controller = "Purchase", action = "CreateProductDetails", VendorDetailId = collection.VendorDetailId }));
            }
            else
                return RedirectToAction("Index", "Login");
        }

        [HttpGet]
        public ActionResult UpdateVendorDetails(int? VendorDetailId)
        {
            if (sessionValidate())
            {
                PurchaseVendorDetail _templist = new PurchaseVendorDetail();
                using (var context = new GS247Entities8())
                {
                    ViewBag.CurrencyList = context.Currencies.ToList();
                    ViewBag.CountryList = context.countries.ToList();
                    _templist = context.PurchaseVendorDetails.Find(VendorDetailId);
                }
                return View(_templist);
            }
            else
                return RedirectToAction("Index", "Login");
        }

        [HttpPost]
        public ActionResult CreateProductDetails(PurchaseProductDetail collection, int VendorDetailId, string AddFlag)
        {
            if (sessionValidate())
            {
                using (var context = new GS247Entities8())
                {
                    var _templist = context.PurchaseProductDetails.Find(collection.ProductId);
                    if (_templist == null)
                    {
                        collection.CreatedBy = Session["UserName"].ToString();
                        collection.CreatedDate = DateTime.Now;
                        context.PurchaseProductDetails.Add(collection);
                        context.SaveChanges();
                    }
                    else
                    {
                        _templist.Price = collection.Price;
                        _templist.Description = collection.Description;
                        _templist.UpdatedDate = DateTime.Now;
                        _templist.UpdatedBy = Session["UserName"].ToString();
                        context.Entry(_templist).State = EntityState.Modified;
                        context.SaveChanges();
                    }
                }

                if (AddFlag == "1")
                {
                    return RedirectToAction("CreateProductDetails", new RouteValueDictionary(
                             new { controller = "Purchase", action = "CreateProductDetails", VendorDetailId = collection.VendorDetailId }));
                }
                else
                {
                    return RedirectToAction("VendorDetails", new RouteValueDictionary(
                  new { controller = "Purchase", action = "VendorDetails", VendorDetailId = collection.VendorDetailId }));
                }
            }
            else
                return RedirectToAction("Index", "Login");
        }

        [HttpGet]
        public ActionResult ViewVendorDetails(int VendorDetailId)
        {
            if (sessionValidate())
            {
                var list = new List<PurchaseProductDetailCO>();
                PurchaseVendorDetail _templist = new PurchaseVendorDetail();

                using (var context = new GS247Entities8())
                {
                    ViewBag.PurchaseVendorList = context.PurchaseVendorDetails.Where(x => x.VendorDetailId == VendorDetailId).ToList();
                    var productDetails = context.PurchaseProductDetails.Where(x => x.VendorDetailId == VendorDetailId).ToList();
                    productDetails.ForEach(data =>
                    {

                        var PurchaseProductDetail = new PurchaseProductDetailCO();
                        PurchaseProductDetail.ProductId = data.ProductId;
                        PurchaseProductDetail.VendorDetailId = data.VendorDetailId;
                        PurchaseProductDetail.ItemId = data.ItemId;
                        PurchaseProductDetail.Price = data.Price;
                        PurchaseProductDetail.Description = data.Description;
                        PurchaseProductDetail.CreatedBy = data.CreatedBy;
                        PurchaseProductDetail.UpdatedBy = data.UpdatedBy;
                        PurchaseProductDetail.CreatedDate = data.CreatedDate;
                        PurchaseProductDetail.UpdatedDate = data.UpdatedDate;

                        var SchoolCO = new SchoolCO();
                        var items = context.PurchaseItems.Find(data.ItemId);
                        SchoolCO.Name = items != null ? items.ItemName : "";
                        PurchaseProductDetail.SchoolCO = SchoolCO;
                        list.Add(PurchaseProductDetail);
                    });
                    ViewBag.ProductDetailsList = list;
                    ViewBag.VendorDetailId = VendorDetailId;
                }
                return View();
            }
            else
                return RedirectToAction("Index", "Login");
        }

        [HttpPost]
        public ActionResult DeleteProductDetails(int ProductId)
        {
            using (var context = new GS247Entities8())
            {
                var _templist = context.PurchaseProductDetails.Find(ProductId);
                if (_templist != null)
                {
                    context.PurchaseProductDetails.Remove(_templist);
                    context.SaveChanges();
                }
            }
            return new EmptyResult();
        }

        [HttpPost]
        public ActionResult DeleteVendorDetails(int VendorDetailId)
        {
            using (var context = new GS247Entities8())
            {
                var _templist = context.PurchaseVendorDetails.Find(VendorDetailId);
                if (_templist != null)
                {
                    context.PurchaseVendorDetails.Remove(_templist);
                    context.SaveChanges();
                }
            }
            return new EmptyResult();
        }

        [HttpPost]
        public JsonResult ValidateVendor(string CompanyName, int? VendorDetailId)
        {
            string Message = string.Empty;

            using (var context = new GS247Entities8())
            {
                var _templist = context.PurchaseVendorDetails.Where(x => x.CompanyName.ToUpper().Equals(CompanyName.ToUpper()) && x.VendorDetailId != VendorDetailId).FirstOrDefault();
                if (_templist != null)
                    Message = "Vendor already available";
            }

            return Json(new { Message = Message }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region "PURCHASE ORDER"

        public ActionResult PurchaseOrder()
        {
            if (sessionValidate())
            {
                var PurchaseOrderItemList = new List<PurchaseOrderItemCO>();
                using (var context = new GS247Entities8())
                {
                    var List = context.PurchaseOrderItems.ToList();

                    List.ForEach(data =>
                    {

                        var PurchaseOrderItem = new PurchaseOrderItemCO();
                        PurchaseOrderItem.OrderItemId = data.OrderItemId;
                        PurchaseOrderItem.VendorDetailId = data.VendorDetailId;
                        PurchaseOrderItem.ItemId = data.ItemId;
                        PurchaseOrderItem.Quantity = data.Quantity;
                        PurchaseOrderItem.TotalPrice = data.TotalPrice;
                        PurchaseOrderItem.CreatedBy = data.CreatedBy;
                        PurchaseOrderItem.StatusFlag = data.StatusFlag;
                        PurchaseOrderItem.CreatedDate = data.CreatedDate;
                        PurchaseOrderItem.UpdatedBy = data.UpdatedBy;
                        PurchaseOrderItem.UpdatedDate = data.UpdatedDate;

                        var SchoolCO = new SchoolCO();
                        var items = context.PurchaseItems.Find(data.ItemId);
                        SchoolCO.Name = items != null ? items.ItemName : "";
                        var vendor = context.PurchaseVendorDetails.Find(data.VendorDetailId);
                        SchoolCO.RelationName = vendor != null ? vendor.FirstName + " " + vendor.LastName : "";

                        var product = context.PurchaseProductDetails.Where(x => x.VendorDetailId == data.VendorDetailId && x.ItemId == data.ItemId).FirstOrDefault();
                        SchoolCO.FareAmount = product != null ? product.Price.GetValueOrDefault().ToString("N2") : "";

                        PurchaseOrderItem.SchoolCO = SchoolCO;

                        PurchaseOrderItemList.Add(PurchaseOrderItem);
                    });

                    ViewBag.PurchaseOrderItemsList = PurchaseOrderItemList;
                }
                return View();
            }
            else
                return RedirectToAction("Index", "Login");
        }

        public ActionResult CreatePurchaseSupply()
        {
            if (sessionValidate())
            {
                using (var context = new GS247Entities8())
                {
                    ViewBag.PurchaseItemsList = context.PurchaseItems.ToList();
                }
                return View();
            }
            else
                return RedirectToAction("Index", "Login");
        }

        [HttpPost]
        public ActionResult CreatePurchaseSupply(PurchaseOrderItem collection)
        {
            if (sessionValidate())
            {
                using (var context = new GS247Entities8())
                {
                    collection.CreatedBy = Session["UserName"].ToString();
                    collection.CreatedDate = DateTime.Now;
                    collection.StatusFlag = 0;
                    var product = context.PurchaseProductDetails.Where(x => x.VendorDetailId == collection.VendorDetailId && x.ItemId == collection.ItemId).FirstOrDefault();
                    collection.TotalPrice = product != null ? (product.Price * collection.Quantity) : 0;
                    context.PurchaseOrderItems.Add(collection);
                    context.SaveChanges();
                }
                return RedirectToAction("PurchaseOrder", new RouteValueDictionary(
                             new { controller = "Purchase", action = "PurchaseOrder" }));
            }
            else
                return RedirectToAction("Index", "Login");
        }

        [HttpPost]
        public JsonResult VendorListLoad(int Id)
        {
            List<PurchaseVendorDetail> VendorList = new List<PurchaseVendorDetail>();
            using (var context = new GS247Entities8())
            {
                VendorList = (from cbl in context.PurchaseVendorDetails
                              join b in context.PurchaseProductDetails on cbl.VendorDetailId equals b.VendorDetailId
                              where b.ItemId == Id
                              select cbl).ToList();
            }
            return Json(new { VendorList = VendorList }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult StatusUpdate(int OrderItemId, int StatusFlag)
        {
            using (var context = new GS247Entities8())
            {
                var _templist = context.PurchaseOrderItems.Find(OrderItemId);
                if (_templist != null)
                {
                    _templist.StatusFlag = StatusFlag;
                    _templist.UpdatedDate = DateTime.Now;
                    _templist.UpdatedBy = Session["UserName"].ToString();
                    context.Entry(_templist).State = EntityState.Modified;
                    context.SaveChanges();

                    if (StatusFlag == 1)
                    {
                        var collection = new PurchaseApplyOrder();
                        collection.CreatedBy = Session["UserName"].ToString();
                        collection.CreatedDate = DateTime.Now;
                        collection.StatusFlag = 0;
                        collection.VerifiedFlag = 0;
                        collection.OrderItemId = OrderItemId;
                        context.PurchaseApplyOrders.Add(collection);
                        context.SaveChanges();
                    }
                }
            }
            return new EmptyResult();
        }

        #endregion

        #region "PURCHASE APPLY"

        public ActionResult PurchaseSupply()
        {
            if (sessionValidate())
            {
                var PurchaseApplyOrderList = new List<PurchaseApplyOrderCO>();

                using (var context = new GS247Entities8())
                {
                    var List = context.PurchaseApplyOrders.ToList();

                    List.ForEach(data =>
                    {
                        var PurchaseOrderItem = new PurchaseApplyOrderCO();
                        PurchaseOrderItem.OrderItemId = data.OrderItemId;
                        PurchaseOrderItem.ApplyOrderId = data.ApplyOrderId;
                        PurchaseOrderItem.VerifiedFlag = data.VerifiedFlag;
                        PurchaseOrderItem.StatusFlag = data.StatusFlag;
                        PurchaseOrderItem.CreatedBy = data.CreatedBy;
                        PurchaseOrderItem.StatusFlag = data.StatusFlag;
                        PurchaseOrderItem.CreatedDate = data.CreatedDate;
                        PurchaseOrderItem.UpdatedBy = data.UpdatedBy;
                        PurchaseOrderItem.UpdatedDate = data.UpdatedDate;

                        var SchoolCO = new SchoolCO();
                        var orderItems = context.PurchaseOrderItems.Find(data.OrderItemId);
                        if (orderItems != null)
                        {
                            var items = context.PurchaseItems.Find(orderItems.ItemId);
                            SchoolCO.Name = items != null ? items.ItemName : "";
                            var vendor = context.PurchaseVendorDetails.Find(orderItems.VendorDetailId);
                            SchoolCO.RelationName = vendor != null ? vendor.FirstName + " " + vendor.LastName : "";

                            SchoolCO.Quantity = orderItems.Quantity;

                        }
                        PurchaseOrderItem.SchoolCO = SchoolCO;

                        PurchaseApplyOrderList.Add(PurchaseOrderItem);
                    });

                    ViewBag.PurchaseOrderItemsList = PurchaseApplyOrderList;
                }
                return View();
            }
            else
                return RedirectToAction("Index", "Login");
        }

        [HttpPost]
        public ActionResult StatusApplyUpdate(int ApplyOrderId, int StatusFlag, int Flag)
        {
            using (var context = new GS247Entities8())
            {
                var _templist = context.PurchaseApplyOrders.Find(ApplyOrderId);
                if (_templist != null)
                {
                    if (Flag == 1)
                        _templist.StatusFlag = StatusFlag;
                    else
                        _templist.VerifiedFlag = StatusFlag;
                    _templist.UpdatedDate = DateTime.Now;
                    _templist.UpdatedBy = Session["UserName"].ToString();
                    context.Entry(_templist).State = EntityState.Modified;
                    context.SaveChanges();
                    if (_templist.VerifiedFlag == 1)
                    {
                        var orderItems = context.PurchaseOrderItems.Find(_templist.OrderItemId);
                        if (orderItems != null)
                        {
                            var collection = new PurchaseItemsStockDetail();
                            collection.CreatedBy = Session["UserName"].ToString();
                            collection.CreatedDate = DateTime.Now;
                            collection.ItemId = orderItems.ItemId;
                            collection.VendorDetailId = orderItems.VendorDetailId;
                            collection.OrderItemId = _templist.OrderItemId;
                            collection.ItemAvailable = orderItems.Quantity;
                            collection.SaleItemQuantity = 0;
                            context.PurchaseItemsStockDetails.Add(collection);
                            context.SaveChanges();
                        }
                    }
                }
            }
            return new EmptyResult();
        }

        #endregion

        #region "MANAGE STOCK"

        public ActionResult StockList()
        {
            if (sessionValidate())
            {
                var PurchaseItemsStockDetailList = new List<PurchaseItemsStockDetailCO>();
                using (var context = new GS247Entities8())
                {
                    var List = context.PurchaseItemsStockDetails.ToList();

                    List.ForEach(data =>
                    {
                        var PurchaseOrderItems = new PurchaseItemsStockDetailCO();
                        PurchaseOrderItems.StockDetailsId = data.StockDetailsId;
                        PurchaseOrderItems.ItemId = data.ItemId;
                        PurchaseOrderItems.VendorDetailId = data.VendorDetailId;
                        PurchaseOrderItems.ItemAvailable = data.ItemAvailable;
                        PurchaseOrderItems.CreatedBy = data.CreatedBy;
                        PurchaseOrderItems.SaleItemQuantity = data.SaleItemQuantity;
                        PurchaseOrderItems.CreatedDate = data.CreatedDate;
                        PurchaseOrderItems.UpdatedBy = data.UpdatedBy;
                        PurchaseOrderItems.UpdatedDate = data.UpdatedDate;
                        PurchaseOrderItems.OrderItemId = data.OrderItemId;

                        var SchoolCO = new SchoolCO();
                        var items = context.PurchaseItems.Find(data.ItemId);
                        SchoolCO.Name = items != null ? items.ItemName : "";
                        PurchaseOrderItems.SchoolCO = SchoolCO;
                        PurchaseItemsStockDetailList.Add(PurchaseOrderItems);
                    });

                    ViewBag.PurchaseItemsStockDetailsList = PurchaseItemsStockDetailList;
                }
                return View();
            }
            else
                return RedirectToAction("Index", "Login");
        }

        public ActionResult IssueDetails(int ItemId)
        {
            if (sessionValidate())
            {
                var PurchaseSaleDetailCOList = new List<PurchaseSaleDetailCO>();
                using (var context = new GS247Entities8())
                {
                    var List = context.PurchaseSaleDetails.Where(x => x.ItemId == ItemId).ToList();

                    List.ForEach(data =>
                    {
                        var PurchaseSaleDetails = new PurchaseSaleDetailCO();
                        PurchaseSaleDetails.SaleDetailsId = data.SaleDetailsId;
                        PurchaseSaleDetails.ItemId = data.ItemId;
                        PurchaseSaleDetails.Quantity = data.Quantity;
                        PurchaseSaleDetails.PurchaserType = data.PurchaserType;
                        PurchaseSaleDetails.PurchasedBy = data.PurchasedBy;
                        PurchaseSaleDetails.StatsuFlag = data.StatsuFlag;
                        PurchaseSaleDetails.ReturnDate = data.ReturnDate;
                        PurchaseSaleDetails.ReturnReason = data.ReturnReason;
                        PurchaseSaleDetails.CreatedBy = data.CreatedBy;
                        PurchaseSaleDetails.CreatedDate = data.CreatedDate;
                        PurchaseSaleDetails.UpdatedBy = data.UpdatedBy;
                        PurchaseSaleDetails.UpdatedDate = data.UpdatedDate;
                        PurchaseSaleDetails.BatchId = data.BatchId;

                        var SchoolCO = new SchoolCO();
                        if (data.PurchaserType == 1)
                        {
                            var student = context.StudentDetails.Find(data.PurchasedBy);
                            SchoolCO.StudentName = student != null ? student.FirstName + " " + student.LastName : "";
                        }
                        else if (data.PurchaserType == 2)
                        {
                            var teacher = context.StaffDetails.Find(data.PurchasedBy);
                            if (teacher != null)
                            {
                                SchoolCO.StudentName = teacher.FirstName + " " + teacher.LastName;
                                var departement = context.TeachersManageDepartments.Find(teacher.DepartmentId);
                                SchoolCO.RelationName = departement != null ? departement.Code + " " + departement.Name : "";
                            }
                            else
                            {
                                SchoolCO.StudentName = string.Empty;
                                SchoolCO.RelationName = string.Empty;
                            }
                        }
                        PurchaseSaleDetails.SchoolCO = SchoolCO;
                        PurchaseSaleDetailCOList.Add(PurchaseSaleDetails);
                    });

                    var _templist = context.PurchaseItems.Find(ItemId);
                    ViewBag.ItemName = _templist != null ? _templist.ItemName : string.Empty;
                    ViewBag.RequestMaterialDetails = PurchaseSaleDetailCOList;
                }
                return View();
            }
            else
                return RedirectToAction("Index", "Login");
        }

        public ActionResult StockVendorDetails(int ItemId)
        {
            if (sessionValidate())
            {
                var PurchaseProductDetailCOList = new List<PurchaseProductDetailCO>();
                using (var context = new GS247Entities8())
                {
                    var List = context.PurchaseProductDetails.Where(x => x.ItemId == ItemId).ToList();

                    List.ForEach(data =>
                    {
                        var PurchaseProductDetail = new PurchaseProductDetailCO();
                        PurchaseProductDetail.ProductId = data.ProductId;
                        PurchaseProductDetail.VendorDetailId = data.VendorDetailId;
                        PurchaseProductDetail.ItemId = data.ItemId;
                        PurchaseProductDetail.Price = data.Price;
                        PurchaseProductDetail.Description = data.Description;
                        PurchaseProductDetail.CreatedBy = data.CreatedBy;
                        PurchaseProductDetail.UpdatedBy = data.UpdatedBy;
                        PurchaseProductDetail.CreatedDate = data.CreatedDate;
                        PurchaseProductDetail.UpdatedDate = data.UpdatedDate;

                        var SchoolCO = new SchoolCO();
                        var vendor = context.PurchaseVendorDetails.Find(data.VendorDetailId);
                        SchoolCO.StudentName = vendor != null ? vendor.FirstName + " " + vendor.LastName : "";
                        PurchaseProductDetail.SchoolCO = SchoolCO;

                        PurchaseProductDetailCOList.Add(PurchaseProductDetail);
                    });
                    var _templist = context.PurchaseItems.Find(ItemId);
                    ViewBag.ItemName = _templist != null ? _templist.ItemName : string.Empty;
                    ViewBag.PurchaseVendorList = PurchaseProductDetailCOList;
                }
                return View();
            }
            else
                return RedirectToAction("Index", "Login");
        }

        [HttpPost]
        public JsonResult ItemQuantityValidation(int ItemId, int Quantity)
        {
            string Message = "";
            using (var context = new GS247Entities8())
            {
                if (!context.PurchaseItemsStockDetails.Where(x => x.ItemId == ItemId && x.ItemAvailable >= Quantity).Any())
                    Message = "Stock not available";
            }

            return Json(new { Message = Message }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region "MANAGE SALE"

        public ActionResult SaleItemList()
        {
            if (sessionValidate())
            {
                return View();
            }
            else
                return RedirectToAction("Index", "Login");
        }

        [HttpPost]
        public JsonResult SaleItemList(int PurchaserType, int? currentPage)
        {
            List<PurchaseSaleDetailCO> PurchaseSaleDetailCOList = new List<PurchaseSaleDetailCO>();

            List<PurchaseSaleDetail> PurchaseSaleDetailsList = new List<PurchaseSaleDetail>();
            using (var context = new GS247Entities8())
            {
                if (PurchaserType == 0)
                    PurchaseSaleDetailsList = context.PurchaseSaleDetails.OrderByDescending(x => x.CreatedDate).ToList();
                else
                    PurchaseSaleDetailsList = context.PurchaseSaleDetails.Where(x => x.PurchaserType == PurchaserType).OrderByDescending(x => x.CreatedDate).ToList();

                PurchaseSaleDetailsList.ForEach(data =>
                {
                    var PurchaseSaleDetails = new PurchaseSaleDetailCO();
                    PurchaseSaleDetails.SaleDetailsId = data.SaleDetailsId;
                    PurchaseSaleDetails.ItemId = data.ItemId;
                    PurchaseSaleDetails.Quantity = data.Quantity;
                    PurchaseSaleDetails.PurchaserType = data.PurchaserType;
                    PurchaseSaleDetails.PurchasedBy = data.PurchasedBy;
                    PurchaseSaleDetails.StatsuFlag = data.StatsuFlag;
                    PurchaseSaleDetails.ReturnDate = data.ReturnDate;
                    PurchaseSaleDetails.ReturnReason = data.ReturnReason;
                    PurchaseSaleDetails.CreatedBy = data.CreatedBy;
                    PurchaseSaleDetails.CreatedDate = data.CreatedDate;
                    PurchaseSaleDetails.UpdatedBy = data.UpdatedBy;
                    PurchaseSaleDetails.UpdatedDate = data.UpdatedDate;
                    PurchaseSaleDetails.BatchId = data.BatchId;

                    var SchoolCO = new SchoolCO();
                    if (data.PurchaserType == 1)
                    {
                        var student = context.StudentDetails.Find(data.PurchasedBy);
                        SchoolCO.StudentName = student != null ? student.FirstName + " " + student.LastName : "";
                    }
                    else if (data.PurchaserType == 2)
                    {
                        var teacher = context.StaffDetails.Find(data.PurchasedBy);
                        if (teacher != null)
                        {
                            SchoolCO.StudentName = teacher.FirstName + " " + teacher.LastName;
                            //var Departments = context.TeachersManageDepartments.Find(teacher.DepartmentId);
                            //SchoolCO.RelationName = Departments != null ? Departments.Name + " " + Departments.Code : string.Empty;
                        }
                        else
                        {
                            SchoolCO.StudentName = "";
                            SchoolCO.RelationName = "";
                        }
                    }
                    else if (data.PurchaserType == 3)
                    {
                        var teacher = context.GuardianDetails.Find(data.PurchasedBy);
                        SchoolCO.StudentName = teacher != null ? teacher.FirstName + " " + teacher.LastName : "";
                    }

                    var items = context.PurchaseItems.Find(data.ItemId);
                    SchoolCO.Name = items != null ? items.ItemName : "";

                    SchoolCO.TransactionTypeName = data.PurchaserType == 1 ? "Student" : data.PurchaserType == 2 ? "Teacher" : "Parent";

                    PurchaseSaleDetails.SchoolCO = SchoolCO;

                    PurchaseSaleDetailCOList.Add(PurchaseSaleDetails);

                });

            }
            return Json(new { PurchaseSaleDetailsList = PurchaseSaleDetailCOList }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SaleStatusUpdate(int SaleDetailsId, string ReturnDate, string Reason)
        {
            using (var context = new GS247Entities8())
            {
                var _templist = context.PurchaseSaleDetails.Find(SaleDetailsId);
                if (_templist != null)
                {
                    _templist.ReturnDate = Convert.ToDateTime(ReturnDate);
                    _templist.ReturnReason = Reason;
                    _templist.StatsuFlag = 5;
                    _templist.UpdatedDate = DateTime.Now;
                    _templist.UpdatedBy = Session["UserName"].ToString();
                    context.Entry(_templist).State = EntityState.Modified;
                    context.SaveChanges();

                    var _templist1 = context.PurchaseItemsStockDetails.Where(x => x.ItemId == _templist.ItemId).FirstOrDefault();
                    if (_templist != null)
                    {
                        _templist1.ItemAvailable = _templist1.ItemAvailable + _templist.Quantity;
                        _templist1.SaleItemQuantity = _templist1.SaleItemQuantity - _templist.Quantity;
                        _templist1.UpdatedDate = DateTime.Now;
                        _templist.UpdatedBy = Session["UserName"].ToString();
                        context.Entry(_templist).State = EntityState.Modified;
                        context.SaveChanges();
                    }
                }
            }
            return new EmptyResult();
        }

        public ActionResult CreateSaleItems()
        {
            if (sessionValidate())
            {
                var PurchaseItemsStockDetailCOList = new List<PurchaseItemsStockDetailCO>();
                using (var context = new GS247Entities8())
                {
                    var List = context.PurchaseItemsStockDetails.ToList();
                    List.ForEach(data =>
                    {
                        var PurchaseOrderItems = new PurchaseItemsStockDetailCO();
                        PurchaseOrderItems.StockDetailsId = data.StockDetailsId;
                        PurchaseOrderItems.ItemId = data.ItemId;
                        PurchaseOrderItems.VendorDetailId = data.VendorDetailId;
                        PurchaseOrderItems.ItemAvailable = data.ItemAvailable;
                        PurchaseOrderItems.CreatedBy = data.CreatedBy;
                        PurchaseOrderItems.SaleItemQuantity = data.SaleItemQuantity;
                        PurchaseOrderItems.CreatedDate = data.CreatedDate;
                        PurchaseOrderItems.UpdatedBy = data.UpdatedBy;
                        PurchaseOrderItems.UpdatedDate = data.UpdatedDate;
                        PurchaseOrderItems.OrderItemId = data.OrderItemId;

                        var SchoolCO = new SchoolCO();
                        var items = context.PurchaseItems.Find(data.ItemId);
                        SchoolCO.Name = items != null ? items.ItemName : "";
                        PurchaseOrderItems.SchoolCO = SchoolCO;

                        PurchaseItemsStockDetailCOList.Add(PurchaseOrderItems);
                    });
                    ViewBag.PurchaseItemsList = PurchaseItemsStockDetailCOList;
                    ViewBag.BatchList = context.batches.ToList();
                }
                return View();
            }
            else
                return RedirectToAction("Index", "Login");
        }

        [HttpPost]
        public ActionResult CreateSaleItems(PurchaseSaleDetail collection)
        {
            if (sessionValidate())
            {
                int id = Convert.ToInt32(Session["UserId"].ToString());
                using (var context = new GS247Entities8())
                {
                    collection.CreatedBy = Session["UserName"].ToString();
                    collection.CreatedDate = DateTime.Now;
                    collection.StatsuFlag = 2;
                    context.PurchaseSaleDetails.Add(collection);
                    context.SaveChanges();

                    var _templist = context.PurchaseItemsStockDetails.Where(x => x.ItemId == collection.ItemId).FirstOrDefault();
                    if (_templist != null)
                    {
                        _templist.ItemAvailable = _templist.ItemAvailable - collection.Quantity;
                        _templist.SaleItemQuantity = _templist.SaleItemQuantity + collection.Quantity;
                        _templist.UpdatedDate = DateTime.Now;
                        _templist.UpdatedBy = Session["UserName"].ToString();
                        context.Entry(_templist).State = EntityState.Modified;
                        context.SaveChanges();
                    }
                }

                return RedirectToAction("SaleItemList", new RouteValueDictionary(
                       new { controller = "Purchase", action = "SaleItemList" }));
            }
            else
                return RedirectToAction("Index", "Login");
        }

        [HttpPost]
        public JsonResult PurchasedByStudent(int Type, int? BatchId)
        {
            List<StudentDetail> List = new List<StudentDetail>();
            using (var context = new GS247Entities8())
            {
                List = context.StudentDetails.Where(x => x.Batch == BatchId).ToList();
            }
            return Json(new { List = List }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult PurchasedByStaff(int Type)
        {
            List<StaffDetail> List = new List<StaffDetail>();
            using (var context = new GS247Entities8())
            {
                List = context.StaffDetails.ToList();
            }
            return Json(new { List = List }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult PurchasedByParent(int Type)
        {
            List<GuardianDetail> List = new List<GuardianDetail>();
            using (var context = new GS247Entities8())
            {
                List = (from sd in context.GuardianDetails
                        select sd).Distinct().ToList();
            }
            return Json(new { List = List }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region "STUDENT REQUEST MATERIAL"

        public ActionResult StudentRequestMaterial()
        {
            if (sessionValidate())
            {
                var PurchaseOrderItemsList = new List<PurchaseItemsStockDetailCO>();
                using (var context = new GS247Entities8())
                {
                    var List = context.PurchaseItemsStockDetails.ToList();
                    List.ForEach(data =>
                    {

                        var PurchaseOrderItems = new PurchaseItemsStockDetailCO();
                        PurchaseOrderItems.StockDetailsId = data.StockDetailsId;
                        PurchaseOrderItems.ItemId = data.ItemId;
                        PurchaseOrderItems.VendorDetailId = data.VendorDetailId;
                        PurchaseOrderItems.ItemAvailable = data.ItemAvailable;
                        PurchaseOrderItems.CreatedBy = data.CreatedBy;
                        PurchaseOrderItems.SaleItemQuantity = data.SaleItemQuantity;
                        PurchaseOrderItems.CreatedDate = data.CreatedDate;
                        PurchaseOrderItems.UpdatedBy = data.UpdatedBy;
                        PurchaseOrderItems.UpdatedDate = data.UpdatedDate;
                        PurchaseOrderItems.OrderItemId = data.OrderItemId;

                        var SchoolCO = new SchoolCO();
                        var items = context.PurchaseItems.Find(data.ItemId);
                        SchoolCO.Name = items != null ? items.ItemName : "";
                        PurchaseOrderItems.SchoolCO = SchoolCO;
                        PurchaseOrderItemsList.Add(PurchaseOrderItems);
                    });
                    ViewBag.PurchaseItemsList = PurchaseOrderItemsList;
                }
                return View();
            }
            else
                return RedirectToAction("Index", "Login");
        }

        public ActionResult StudentMaterialRequistion()
        {
            if (sessionValidate())
            {
                var RequestMaterialDetailList = new List<RequestMaterialDetailCO>();
                using (var context = new GS247Entities8())
                {
                    var List = context.RequestMaterialDetails.Where(x => x.RequestType == 1).ToList();

                    List.ForEach(data =>
                    {
                        var newData = new RequestMaterialDetailCO();
                        newData.MaterialId = data.MaterialId;
                        newData.RequestType = data.RequestType;
                        newData.ItemId = data.ItemId;
                        newData.Quantity = data.Quantity;
                        newData.DepartementId = data.DepartementId;
                        newData.IssueDate = data.IssueDate;
                        newData.StatusFlag = data.StatusFlag;
                        newData.TeacherApprovedFlag = data.TeacherApprovedFlag;
                        newData.CreatedBy = data.CreatedBy;
                        newData.CreatedDate = data.CreatedDate;
                        newData.ReferenceId = data.ReferenceId;
                        newData.ApprovedBy = data.ApprovedBy;

                        newData.MaterialId = data.MaterialId;

                        var SchoolCO = new SchoolCO();
                        var items = context.PurchaseItems.Find(data.ItemId);
                        SchoolCO.Name = items != null ? items.ItemName : "";
                        SchoolCO.StatusFlagDesc = data.StatusFlag == 0 ? "Pending" : data.StatusFlag == 1 ? "Approved" : data.StatusFlag == 2 ? "Issued" : data.StatusFlag == 3 ? "Rejected" : "Withdraw";
                        newData.SchoolCO = SchoolCO;
                        RequestMaterialDetailList.Add(newData);
                    });

                    ViewBag.RequestMaterialDetails = RequestMaterialDetailList;
                }
                return View();
            }
            else
                return RedirectToAction("Index", "Login");
        }

        [HttpPost]
        public ActionResult CreateStudentRequestMaterial(RequestMaterialDetail collection)
        {
            if (sessionValidate())
            {
                using (var context = new GS247Entities8())
                {
                    collection.CreatedBy = Session["UserName"].ToString();
                    collection.CreatedDate = DateTime.Now;
                    collection.RequestType = 1;
                    collection.ReferenceId = 17438;
                    collection.StatusFlag = 0;
                    collection.TeacherApprovedFlag = 0;
                    context.RequestMaterialDetails.Add(collection);
                    context.SaveChanges();
                }
                return RedirectToAction("StudentMaterialRequistion", new RouteValueDictionary(
                             new { controller = "Purchase", action = "StudentMaterialRequistion" }));
            }
            else
                return RedirectToAction("Index", "Login");
        }

        [HttpPost]
        public ActionResult StudentStatusMaterialUpdate(int MaterialId, int StatusFlag)
        {
            using (var context = new GS247Entities8())
            {
                var _templist = context.RequestMaterialDetails.Find(MaterialId);
                if (_templist != null)
                {
                    _templist.StatusFlag = StatusFlag;
                    _templist.UpdatedDate = DateTime.Now;
                    _templist.UpdatedBy = Session["UserName"].ToString();
                    context.Entry(_templist).State = EntityState.Modified;
                    context.SaveChanges();
                }
            }
            return new EmptyResult();
        }

        #endregion

        #region "TEACHER REQUEST MATERIAL"

        public ActionResult TeacherRequestMaterial()
        {
            if (sessionValidate())
            {
                var PurchaseOrderItemsList = new List<PurchaseItemsStockDetailCO>();

                using (var context = new GS247Entities8())
                {
                    var List = context.PurchaseItemsStockDetails.ToList();
                    List.ForEach(data =>
                    {
                        var PurchaseOrderItems = new PurchaseItemsStockDetailCO();
                        PurchaseOrderItems.StockDetailsId = data.StockDetailsId;
                        PurchaseOrderItems.ItemId = data.ItemId;
                        PurchaseOrderItems.VendorDetailId = data.VendorDetailId;
                        PurchaseOrderItems.ItemAvailable = data.ItemAvailable;
                        PurchaseOrderItems.CreatedBy = data.CreatedBy;
                        PurchaseOrderItems.SaleItemQuantity = data.SaleItemQuantity;
                        PurchaseOrderItems.CreatedDate = data.CreatedDate;
                        PurchaseOrderItems.UpdatedBy = data.UpdatedBy;
                        PurchaseOrderItems.UpdatedDate = data.UpdatedDate;
                        PurchaseOrderItems.OrderItemId = data.OrderItemId;

                        var SchoolCO = new SchoolCO();
                        var items = context.PurchaseItems.Find(data.ItemId);
                        SchoolCO.Name = items != null ? items.ItemName : "";
                        PurchaseOrderItems.SchoolCO = SchoolCO;
                        PurchaseOrderItemsList.Add(PurchaseOrderItems);
                    });
                    ViewBag.PurchaseItemsList = PurchaseOrderItemsList;
                    ViewBag.DepartmentList = context.TeachersManageDepartments.ToList();
                }
                return View();
            }
            else
                return RedirectToAction("Index", "Login");
        }

        public ActionResult TeacherMaterialRequistion()
        {
            if (sessionValidate())
            {
                var RequestMaterialDetailList = new List<RequestMaterialDetailCO>();

                using (var context = new GS247Entities8())
                {
                    var List = context.RequestMaterialDetails.Where(x => x.RequestType == 2).ToList();

                    List.ForEach(data =>
                    {
                        var newData = new RequestMaterialDetailCO();
                        newData.MaterialId = data.MaterialId;
                        newData.RequestType = data.RequestType;
                        newData.ItemId = data.ItemId;
                        newData.Quantity = data.Quantity;
                        newData.DepartementId = data.DepartementId;
                        newData.IssueDate = data.IssueDate;
                        newData.StatusFlag = data.StatusFlag;
                        newData.TeacherApprovedFlag = data.TeacherApprovedFlag;
                        newData.CreatedBy = data.CreatedBy;
                        newData.CreatedDate = data.CreatedDate;
                        newData.ReferenceId = data.ReferenceId;
                        newData.ApprovedBy = data.ApprovedBy;

                        var SchoolCO = new SchoolCO();
                        var items = context.PurchaseItems.Find(data.ItemId);
                        SchoolCO.Name = items != null ? items.ItemName : "";
                        SchoolCO.StatusFlagDesc = data.StatusFlag == 0 ? "Pending" : data.StatusFlag == 1 ? "Approved" : data.StatusFlag == 2 ? "Issued" : "Rejected";
                        var Departments = context.TeachersManageDepartments.Find(data.DepartementId);
                        SchoolCO.RelationName = Departments != null ? Departments.Name + " " + Departments.Code : string.Empty;
                        newData.SchoolCO = SchoolCO;

                        RequestMaterialDetailList.Add(newData);

                    });

                    ViewBag.RequestMaterialDetails = RequestMaterialDetailList;
                }
                return View();
            }
            else
                return RedirectToAction("Index", "Login");
        }

        [HttpPost]
        public ActionResult CreateTeacherRequestMaterial(RequestMaterialDetail collection)
        {
            if (sessionValidate())
            {
                using (var context = new GS247Entities8())
                {
                    collection.CreatedBy = Session["UserName"].ToString();
                    collection.CreatedDate = DateTime.Now;
                    collection.RequestType = 2;
                    collection.ReferenceId = 2;
                    collection.StatusFlag = 0;
                    collection.TeacherApprovedFlag = 1;
                    context.RequestMaterialDetails.Add(collection);
                    context.SaveChanges();
                }
                return RedirectToAction("TeacherMaterialRequistion", new RouteValueDictionary(
                             new { controller = "Purchase", action = "TeacherMaterialRequistion" }));
            }
            else
                return RedirectToAction("Index", "Login");
        }

        [HttpPost]
        public ActionResult TeacherStatusMaterialUpdate(int MaterialId, int StatusFlag)
        {
            using (var context = new GS247Entities8())
            {
                var _templist = context.RequestMaterialDetails.Find(MaterialId);
                if (_templist != null)
                {
                    _templist.StatusFlag = StatusFlag;
                    _templist.UpdatedDate = DateTime.Now;
                    _templist.UpdatedBy = Session["UserName"].ToString();
                    context.Entry(_templist).State = EntityState.Modified;
                    context.SaveChanges();
                }
            }
            return new EmptyResult();
        }

        #endregion

        #region "TEACHER APPROVE"

        public ActionResult TeacherApproval()
        {
            if (sessionValidate())
            {
                var RequestMaterialDetailList = new List<RequestMaterialDetailCO>();

                using (var context = new GS247Entities8())
                {
                    var List = context.RequestMaterialDetails.Where(x => x.RequestType == 1).ToList();

                    List.ForEach(data =>
                    {
                        var newData = new RequestMaterialDetailCO();
                        newData.MaterialId = data.MaterialId;
                        newData.RequestType = data.RequestType;
                        newData.ItemId = data.ItemId;
                        newData.Quantity = data.Quantity;
                        newData.DepartementId = data.DepartementId;
                        newData.IssueDate = data.IssueDate;
                        newData.StatusFlag = data.StatusFlag;
                        newData.TeacherApprovedFlag = data.TeacherApprovedFlag;
                        newData.CreatedBy = data.CreatedBy;
                        newData.CreatedDate = data.CreatedDate;
                        newData.ReferenceId = data.ReferenceId;
                        newData.ApprovedBy = data.ApprovedBy;


                        var SchoolCO = new SchoolCO();
                        var items = context.PurchaseItems.Find(data.ItemId);
                        SchoolCO.Name = items != null ? items.ItemName : "";
                        if (data.TeacherApprovedFlag == 0)
                            SchoolCO.StatusFlagDesc = data.StatusFlag == 0 ? "Pending" : data.StatusFlag == 1 ? "Approved" : data.StatusFlag == 2 ? "Issued" : data.StatusFlag == 3 ? "Rejected" : "Withdraw";
                        else
                            SchoolCO.StatusFlagDesc = "Approved";
                        var student = context.StudentDetails.Find(data.ReferenceId);
                        SchoolCO.StudentName = student != null ? student.FirstName + " " + student.LastName : string.Empty;
                        newData.SchoolCO = SchoolCO;

                        RequestMaterialDetailList.Add(newData);
                    });

                    ViewBag.RequestMaterialDetails = RequestMaterialDetailList;
                }
                return View();
            }
            else
                return RedirectToAction("Index", "Login");
        }

        [HttpPost]
        public ActionResult TeacherMaterialStatusUpdate(int MaterialId, int StatusFlag)
        {
            using (var context = new GS247Entities8())
            {
                var _templist = context.RequestMaterialDetails.Find(MaterialId);
                if (_templist != null)
                {
                    _templist.StatusFlag = StatusFlag;
                    if (StatusFlag == 0)
                        _templist.TeacherApprovedFlag = 1;
                    else
                        _templist.TeacherApprovedFlag = 0;
                    _templist.UpdatedDate = DateTime.Now;
                    _templist.UpdatedBy = Session["UserName"].ToString();
                    context.Entry(_templist).State = EntityState.Modified;
                    context.SaveChanges();
                }
            }
            return new EmptyResult();
        }


        #endregion

        #region "MATERIAL REQUISTION"

        public ActionResult MaterialRequistion()
        {
            if (sessionValidate())
            {
                var RequestMaterialDetailList = new List<RequestMaterialDetailCO>();
                using (var context = new GS247Entities8())
                {
                    var List = context.RequestMaterialDetails.Where(x => (x.StatusFlag == 0 || x.StatusFlag == 1 || x.StatusFlag == 3) && x.TeacherApprovedFlag == 1).OrderByDescending(x => x.CreatedDate).ToList();

                    List.ForEach(data =>
                    {
                        var newData = new RequestMaterialDetailCO();
                        newData.MaterialId = data.MaterialId;
                        newData.RequestType = data.RequestType;
                        newData.ItemId = data.ItemId;
                        newData.Quantity = data.Quantity;
                        newData.DepartementId = data.DepartementId;
                        newData.IssueDate = data.IssueDate;
                        newData.StatusFlag = data.StatusFlag;
                        newData.TeacherApprovedFlag = data.TeacherApprovedFlag;
                        newData.CreatedBy = data.CreatedBy;
                        newData.CreatedDate = data.CreatedDate;
                        newData.ReferenceId = data.ReferenceId;
                        newData.ApprovedBy = data.ApprovedBy;

                        var SchoolCO = new SchoolCO();
                        if (data.RequestType == 1)
                        {
                            var student = context.StudentDetails.Find(data.ReferenceId);
                            SchoolCO.StudentName = student != null ? student.FirstName + " " + student.LastName : "";
                        }
                        else if (data.RequestType == 2)
                        {
                            var teacher = context.StaffDetails.Find(data.ReferenceId);
                            SchoolCO.StudentName = teacher != null ? teacher.FirstName + " " + teacher.LastName : "";

                            var Departments = context.TeachersManageDepartments.Find(data.DepartementId);
                            SchoolCO.RelationName = Departments != null ? Departments.Name + " " + Departments.Code : string.Empty;
                        }

                        var items = context.PurchaseItems.Find(data.ItemId);
                        SchoolCO.Name = items != null ? items.ItemName : "";
                        SchoolCO.TransactionTypeName = data.RequestType == 1 ? "Student" : "Teacher";

                        newData.SchoolCO = SchoolCO;
                        RequestMaterialDetailList.Add(newData);
                    });

                    ViewBag.RequestMaterialDetails = RequestMaterialDetailList;
                }
                return View();
            }
            else
                return RedirectToAction("Index", "Login");
        }

        [HttpPost]
        public ActionResult MaterialStatusUpdate(int MaterialId, int StatusFlag)
        {
            using (var context = new GS247Entities8())
            {
                var _templist = context.RequestMaterialDetails.Find(MaterialId);
                if (_templist != null)
                {
                    _templist.StatusFlag = StatusFlag;
                    int id = Convert.ToInt32(Session["UserId"].ToString());
                    _templist.ApprovedBy = id;
                    _templist.UpdatedDate = DateTime.Now;
                    _templist.UpdatedBy = Session["UserName"].ToString();
                    context.Entry(_templist).State = EntityState.Modified;
                    context.SaveChanges();
                }
            }
            return new EmptyResult();
        }

        #endregion

        #region "ISSUE ITEM"

        public ActionResult MaterialIssueItems()
        {
            if (sessionValidate())
            {
                var RequestMaterialDetailList = new List<RequestMaterialDetailCO>();
                using (var context = new GS247Entities8())
                {
                    var List = context.RequestMaterialDetails.Where(x => (x.StatusFlag == 1 || x.StatusFlag == 2) && x.TeacherApprovedFlag == 1).OrderByDescending(x => x.CreatedDate).ToList();

                    List.ForEach(data =>
                    {
                        var newData = new RequestMaterialDetailCO();
                        newData.MaterialId = data.MaterialId;
                        newData.RequestType = data.RequestType;
                        newData.ItemId = data.ItemId;
                        newData.Quantity = data.Quantity;
                        newData.DepartementId = data.DepartementId;
                        newData.IssueDate = data.IssueDate;
                        newData.StatusFlag = data.StatusFlag;
                        newData.TeacherApprovedFlag = data.TeacherApprovedFlag;
                        newData.CreatedBy = data.CreatedBy;
                        newData.CreatedDate = data.CreatedDate;
                        newData.ReferenceId = data.ReferenceId;
                        newData.ApprovedBy = data.ApprovedBy;


                        var SchoolCO = new SchoolCO();
                        if (data.RequestType == 1)
                        {
                            var student = context.StudentDetails.Find(data.ReferenceId);
                            SchoolCO.StudentName = student != null ? student.FirstName + " " + student.LastName : "";
                        }
                        else if (data.RequestType == 2)
                        {
                            var teacher = context.StaffDetails.Find(data.ReferenceId);
                            SchoolCO.StudentName = teacher != null ? teacher.FirstName + " " + teacher.LastName : "";

                            var Departments = context.TeachersManageDepartments.Find(data.DepartementId);
                            SchoolCO.RelationName = Departments != null ? Departments.Name + " " + Departments.Code : string.Empty;
                        }

                        var items = context.PurchaseItems.Find(data.ItemId);
                        SchoolCO.Name = items != null ? items.ItemName : "";
                        SchoolCO.TransactionTypeName = data.RequestType == 1 ? "Student" : "Teacher";

                        newData.SchoolCO = SchoolCO;
                        RequestMaterialDetailList.Add(newData);
                    });

                    ViewBag.RequestMaterialDetails = RequestMaterialDetailList;
                }
                return View();
            }
            else
                return RedirectToAction("Index", "Login");
        }

        [HttpPost]
        public ActionResult MaterialStatusIssueUpdate(int MaterialId, int StatusFlag)
        {
            using (var context = new GS247Entities8())
            {
                var _templist = context.RequestMaterialDetails.Find(MaterialId);
                if (_templist != null)
                {
                    _templist.StatusFlag = StatusFlag;
                    _templist.IssueDate = DateTime.Now;
                    _templist.UpdatedDate = DateTime.Now;
                    _templist.UpdatedBy = Session["UserName"].ToString();
                    context.Entry(_templist).State = EntityState.Modified;
                    context.SaveChanges();

                    if (StatusFlag == 2)
                    {
                        var _templist1 = context.PurchaseItemsStockDetails.Where(x => x.ItemId == _templist.ItemId).FirstOrDefault();
                        if (_templist != null)
                        {
                            _templist1.ItemAvailable = _templist1.ItemAvailable - _templist.Quantity;
                            _templist1.SaleItemQuantity = _templist1.SaleItemQuantity + _templist.Quantity;
                            _templist1.UpdatedDate = DateTime.Now;
                            _templist.UpdatedBy = Session["UserName"].ToString();
                            context.Entry(_templist1).State = EntityState.Modified;
                            context.SaveChanges();
                        }
                    }
                }
            }
            return new EmptyResult();
        }

        #endregion

        #region "SALE REPORT"

        public ActionResult SaleReport()
        {
            if (sessionValidate())
            {
                return View();
            }
            else
                return RedirectToAction("Index", "Login");
        }

        [HttpPost]
        public JsonResult SaleReportList(SearchSaleReportS SearchSaleReportS)
        {
            List<PurchaseSaleDetail> PurchaseSaleDetailsList = new List<PurchaseSaleDetail>();

            List<PurchaseSaleDetailCO> PurchaseSaleDetailCOList = new List<PurchaseSaleDetailCO>();

            var totalPages = 0;
            int totalCount = 0;

            using (var context = new GS247Entities8())
            {
                var list = context.PurchaseSaleDetails.Where(x => x.StatsuFlag == 2);
                if (SearchSaleReportS.Mode == 2)
                {
                    var date = new DateTime(SearchSaleReportS.Year.GetValueOrDefault(), 1, DateTime.DaysInMonth(SearchSaleReportS.Year.GetValueOrDefault(), 1));
                    var list1 = list.Where(x => x.CreatedDate.Year == date.Year).ToList();
                    totalCount = list1.Count;
                    PurchaseSaleDetailsList.AddRange(list1);
                }
                else if (SearchSaleReportS.Mode == 3)
                {
                    var date = new DateTime(SearchSaleReportS.Year.GetValueOrDefault(), SearchSaleReportS.Month.GetValueOrDefault(), DateTime.DaysInMonth(SearchSaleReportS.Year.GetValueOrDefault(), SearchSaleReportS.Month.GetValueOrDefault()));
                    var list1 = list.Where(x => x.CreatedDate.Year == date.Year && x.CreatedDate.Month == date.Month).ToList();
                    totalCount = list1.Count;
                    PurchaseSaleDetailsList.AddRange(list1);
                }
                else if (SearchSaleReportS.Mode == 4)
                {
                    var date = Convert.ToDateTime(SearchSaleReportS.Date);
                    var list1 = list.Where(x => x.CreatedDate.Year == date.Year && x.CreatedDate.Month == date.Month && x.CreatedDate.Day == date.Day).ToList();
                    totalCount = list1.Count;
                    PurchaseSaleDetailsList.AddRange(list1);
                }
                else
                {
                    totalCount = list.Count();
                    PurchaseSaleDetailsList = list.OrderByDescending(x => x.CreatedDate).ToList();
                }

                totalPages = (int)Math.Ceiling((decimal)totalCount / (decimal)10);
                PurchaseSaleDetailsList = PurchaseSaleDetailsList.OrderByDescending(x => x.CreatedDate).Skip((Convert.ToInt16(SearchSaleReportS.CurrentPage) - 1) * 10).Take(10).ToList();
                PurchaseSaleDetailsList.ForEach(data =>
                {
                    var PurchaseSaleDetails = new PurchaseSaleDetailCO();
                    PurchaseSaleDetails.SaleDetailsId = data.SaleDetailsId;
                    PurchaseSaleDetails.ItemId = data.ItemId;
                    PurchaseSaleDetails.Quantity = data.Quantity;
                    PurchaseSaleDetails.PurchaserType = data.PurchaserType;
                    PurchaseSaleDetails.PurchasedBy = data.PurchasedBy;
                    PurchaseSaleDetails.StatsuFlag = data.StatsuFlag;
                    PurchaseSaleDetails.ReturnDate = data.ReturnDate;
                    PurchaseSaleDetails.ReturnReason = data.ReturnReason;
                    PurchaseSaleDetails.CreatedBy = data.CreatedBy;
                    PurchaseSaleDetails.CreatedDate = data.CreatedDate;
                    PurchaseSaleDetails.UpdatedBy = data.UpdatedBy;
                    PurchaseSaleDetails.UpdatedDate = data.UpdatedDate;
                    PurchaseSaleDetails.BatchId = data.BatchId;

                    var SchoolCO = new SchoolCO();
                    if (data.PurchaserType == 1)
                    {
                        var student = context.StudentDetails.Find(data.PurchasedBy);
                        SchoolCO.StudentName = student != null ? student.FirstName + " " + student.LastName : "";
                    }
                    else if (data.PurchaserType == 2)
                    {
                        var teacher = context.StaffDetails.Find(data.PurchasedBy);
                        SchoolCO.StudentName = teacher != null ? teacher.FirstName + " " + teacher.LastName : "";
                    }

                    var items = context.PurchaseItems.Find(data.ItemId);
                    SchoolCO.Name = items != null ? items.ItemName : "";

                    var products = context.PurchaseProductDetails.Find(data.ItemId);
                    SchoolCO.FareAmount = products != null ? Convert.ToDecimal(products.Price * data.Quantity).ToString("N2") : "0.00";

                    SchoolCO.TransactionDateStr = data.CreatedDate.ToString("dd MMM yyyy");

                    PurchaseSaleDetails.SchoolCO = SchoolCO;

                    PurchaseSaleDetailCOList.Add(PurchaseSaleDetails);
                });

            }
            return Json(new { PurchaseSaleDetailsList = PurchaseSaleDetailCOList, TotalPages = totalPages }, JsonRequestBehavior.AllowGet);
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

        #endregion

        #region "COMMON CLASS"

        public class SearchSFields
        {
            public string ItemName { get; set; }

            public string CurrentPage { get; set; }

        }

        public class SearchSaleReportS
        {
            public int Mode { get; set; }

            public int? Year { get; set; }

            public int? Month { get; set; }

            public DateTime? Date { get; set; }

            public string CurrentPage { get; set; }
        }

        public partial class PurchaseProductDetailCO
        {
            public int ProductId { get; set; }
            public Nullable<int> VendorDetailId { get; set; }
            public Nullable<int> ItemId { get; set; }
            public Nullable<decimal> Price { get; set; }
            public string Description { get; set; }
            public string CreatedBy { get; set; }
            public System.DateTime CreatedDate { get; set; }
            public string UpdatedBy { get; set; }
            public Nullable<System.DateTime> UpdatedDate { get; set; }

            public virtual SchoolCO SchoolCO { get; set; }

        }
        public partial class PurchaseOrderItemCO
        {
            public int OrderItemId { get; set; }
            public Nullable<int> VendorDetailId { get; set; }
            public Nullable<int> ItemId { get; set; }
            public Nullable<int> Quantity { get; set; }
            public Nullable<decimal> TotalPrice { get; set; }
            public Nullable<int> StatusFlag { get; set; }
            public string CreatedBy { get; set; }
            public System.DateTime CreatedDate { get; set; }
            public string UpdatedBy { get; set; }
            public Nullable<System.DateTime> UpdatedDate { get; set; }

            public virtual SchoolCO SchoolCO { get; set; }
        }

        public partial class PurchaseApplyOrderCO
        {
            public int ApplyOrderId { get; set; }
            public Nullable<int> OrderItemId { get; set; }
            public Nullable<int> StatusFlag { get; set; }
            public Nullable<int> VerifiedFlag { get; set; }
            public string CreatedBy { get; set; }
            public System.DateTime CreatedDate { get; set; }
            public string UpdatedBy { get; set; }
            public Nullable<System.DateTime> UpdatedDate { get; set; }

            public virtual SchoolCO SchoolCO { get; set; }
        }

        public partial class PurchaseItemsStockDetailCO
        {
            public int StockDetailsId { get; set; }
            public Nullable<int> ItemId { get; set; }
            public Nullable<int> VendorDetailId { get; set; }
            public Nullable<int> ItemAvailable { get; set; }
            public Nullable<int> SaleItemQuantity { get; set; }
            public string CreatedBy { get; set; }
            public System.DateTime CreatedDate { get; set; }
            public string UpdatedBy { get; set; }
            public Nullable<System.DateTime> UpdatedDate { get; set; }
            public Nullable<int> OrderItemId { get; set; }

            public virtual SchoolCO SchoolCO { get; set; }
        }

        public partial class PurchaseSaleDetailCO
        {
            public int SaleDetailsId { get; set; }
            public Nullable<int> ItemId { get; set; }
            public Nullable<int> Quantity { get; set; }
            public Nullable<int> PurchaserType { get; set; }
            public Nullable<int> PurchasedBy { get; set; }
            public Nullable<int> StatsuFlag { get; set; }
            public Nullable<System.DateTime> ReturnDate { get; set; }
            public string ReturnReason { get; set; }
            public string CreatedBy { get; set; }
            public System.DateTime CreatedDate { get; set; }
            public string UpdatedBy { get; set; }
            public Nullable<System.DateTime> UpdatedDate { get; set; }
            public Nullable<int> BatchId { get; set; }

            public virtual SchoolCO SchoolCO { get; set; }
        }

        public partial class RequestMaterialDetailCO
        {
            public int MaterialId { get; set; }
            public Nullable<int> RequestType { get; set; }
            public Nullable<int> ItemId { get; set; }
            public Nullable<int> Quantity { get; set; }
            public Nullable<int> DepartementId { get; set; }
            public Nullable<System.DateTime> IssueDate { get; set; }
            public Nullable<int> StatusFlag { get; set; }
            public Nullable<int> TeacherApprovedFlag { get; set; }
            public string CreatedBy { get; set; }
            public System.DateTime CreatedDate { get; set; }
            public string UpdatedBy { get; set; }
            public Nullable<System.DateTime> UpdatedDate { get; set; }
            public Nullable<int> ReferenceId { get; set; }
            public Nullable<int> ApprovedBy { get; set; }

            public virtual SchoolCO SchoolCO { get; set; }
        }

        #endregion
    }
}