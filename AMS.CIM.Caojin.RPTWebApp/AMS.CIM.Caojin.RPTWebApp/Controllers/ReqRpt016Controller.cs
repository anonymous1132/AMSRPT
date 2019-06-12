using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AMS.CIM.Caojin.RPTWebApp.Models;

namespace AMS.CIM.Caojin.RPTWebApp.Controllers
{
    public class ReqRpt016Controller : Controller
    {
        // GET: ReqRpt016
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetModuleAndReasonList()
        {
            var model = new ReqRpt016DataBuilder();
            model.GetModulesAndCodes();
            var response = new { success = true, model.ModuleList,model.HoldReasonCodeList};
            return Json(response);
        }

        public JsonResult GetLotDetail(string startTime,string endTime)
        {
            var model = new ReqRpt016DataBuilder();
            model.GetLotHoldDetail(startTime,endTime);

            var response = new { success = true,Items= model.Items.Select(s=>s.ToString("yyyyMMdd")),model.NonProdSummaryEntities,model.ProdSummaryEntities,model.LotHoldList };
            return Json(response);
        }
    }
}