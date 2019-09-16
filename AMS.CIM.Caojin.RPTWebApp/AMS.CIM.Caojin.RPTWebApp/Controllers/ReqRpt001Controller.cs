using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AMS.CIM.Caojin.RPTWebApp.Models;

namespace AMS.CIM.Caojin.RPTWebApp.Controllers
{
    public class ReqRpt001Controller : Controller
    {
        // GET: ReqRpt001
        public ActionResult Index()
        {
            ViewBag.ClickCount = CommonController.GetCount("RPT000001");
            return View();
        }

        public JsonResult GetTableData(ReqRpt001QueryTablePostModel postModel)
        {
            try
            {
                var model = new ReqRpt001TableDataBuilder(postModel);
                return Json(new { success = true, model.Items, model.ProductEntities, model.ShowTarget, model.Title });
            }
            catch (Exception ex)
            {
                return Json(new { success=false,msg=ex.Message});
            }
        }

        public JsonResult HandlePlanTableByCmd(string prod,string month,int fromDate,int toDate,int value)
        {
            try
            {
                var arry = month.Split('-');
                int y = Convert.ToInt16(arry[0]);
                int m = Convert.ToInt16(arry[1]);
                int maxDay = DateTime.DaysInMonth(y, m);
                int toDay = toDate > maxDay ? maxDay : toDate;
                var handle = new ReqRpt001PlanSetHandler();
                if (fromDate == 1 && toDay == maxDay) { handle.SetValueByMonth(prod, y, m, value); }
                else
                {
                    string from = month + "-" + (fromDate >= 10 ? fromDate.ToString() : "0" + fromDate);
                    string to = month + "-" + (toDay >= 10 ? toDay.ToString() : "0" + toDay);
                    handle.SetValueByDateRange(prod, from, to, value);
                }
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success=false,msg=ex.Message});
            }
        }

        public JsonResult HandlePlanTableByMonth(string prod,int year,int month,int value)
        {
            var handle = new ReqRpt001PlanSetHandler();
            handle.SetValueByMonth(prod,year,month,value);
            return Json(new { success=true});
        }

        public JsonResult HandlePlanTableByList(List<string>prods,List<string>dates,List<int>values)
        {
            var handle = new ReqRpt001PlanSetHandler();
            var res= handle.SetProdsValueByArray(prods,dates,values);
            return Json(new { success = true,msg=res });
        }

        public JsonResult GetLotDetailOfProd(string prod)
        {
            try
            {
                var model = new ReqRpt001LotDetailDataBuilder(prod);
                if (!model.LotDetails.Any())
                {
                    return Json(new { success = false, msg = "DB中无相关数据" });
                }
                else
                {
                    var data = model.LotDetails.Select(s => new { ProdID = s.ProdSpec_ID, LotID = s.Lot_ID, FoupID = s.Foup_ID, s.Location, s.Status, CreateTime = s.Created_Time, s.Qty, SrcProdID = s.Vendor_ProdSpec_ID, SrcLotID = s.Vendor_Lot_ID, VendorCode = s.Vendor_Name, LotType = s.Lot_Type, LotOwner = s.Lot_Owner_ID });
                    return Json(new { success = true, data });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, msg=ex.Message });
            }

        }

        //验证口令
        public JsonResult CheckKey(string key)
        {
                ReqKeyOperateModel model = new ReqKeyOperateModel();
                bool res = model.CheckKey("ReqRpt00001", key);
                string msg = res ? "验证成功" : "口令验证失败";
                return Json(new { success = res, msg });
        }

        //更改口令
        public JsonResult UpdateKey(string newKey, string oldKey)
        {
            ReqKeyOperateModel model = new ReqKeyOperateModel();
            string msg = "";
            bool success = model.CheckKey("ReqRpt00001", oldKey);
            if (success)
            {
                success = model.UpdateKey("ReqRpt00001", newKey);
                msg = success ? "口令已更新！" : "口令更新出现未知错误，请联系开发人员！";
            }
            else
            {
                msg = "口令认证失败！";
            }
            return Json(new { success, msg }, JsonRequestBehavior.DenyGet);
        }

    }
}