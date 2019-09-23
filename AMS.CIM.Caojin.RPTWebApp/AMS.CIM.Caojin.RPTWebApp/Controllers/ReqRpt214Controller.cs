using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AMS.CIM.Caojin.RPTWebApp.Models;


namespace AMS.CIM.Caojin.RPTWebApp.Controllers
{
    public class ReqRpt214Controller : Controller
    {
        // GET: ReqRpt214
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult ManualQuery(string lot, string startDate, string endDate)
        {
            try
            {
                var model = new ReqRpt214DataBuilder();
                model.GetTableDataByManual(lot, startDate, endDate);
                var data = model.RowEntities;
                var response = new { success = true, RowEntities = data, model.LotStr, model.StartDate, model.EndDate };
                return Json(response);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, msg = ex.Message });
            }
        }
    }
}