using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AMS.CIM.Caojin.RPTWebApp.Models;

namespace AMS.CIM.Caojin.RPTWebApp.Controllers
{
    public class ReqRpt206Controller : Controller
    {
        // GET: ReqRpt206
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetProdList()
        {
            try
            {
                var model = new ReqRpt209DataBuilder();
                var data = model.GetProdList();
                var response = new { success = true, prodList = data };
                return Json(response);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, msg = ex.Message });
            }
        }

        public JsonResult ManualQuery(string lot, string startDate, string endDate)
        {
            //try
            //{
                var model = new ReqRpt206DataBuilder();
                model.GetTableDataByManual(lot, startDate, endDate);
                var data = model.RowEntities;
                var response = new { success = true, RowEntities = data, model.LotStr, model.StartDate, model.EndDate };
                return Json(response);
            //}
            //catch (Exception ex)
            //{
            //    return Json(new {success=false,msg=ex.Message });
            //}
        }
    }
}