using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AMS.CIM.Caojin.RPTWebApp.Models;
using AMS.CIM.Caojin.RPTLibrary.Models;

namespace AMS.CIM.Caojin.RPTWebApp.Controllers
{
    public class ReqRpt028Controller : Controller
    {
        // GET: ReqRpt028
        public ActionResult Index()
        {
            DB2DataCatcher<RPTFuncUsage> CountCatcher = new DB2DataCatcher<RPTFuncUsage>("ISTRPT.RPTFuncUsage") { Conditions = "where privilegeid='RPT000028'" };
            var list = CountCatcher.GetEntities().EntityList;
            double count = list.Any() ? list.First().Usage_Counter : 0;
            ViewBag.ClickCount = count;
            return View();
        }

        public JsonResult GetChartData()
        {
            try
            {
                var model = new ReqRpt028DataBuilder();
                var YstdChart = model.YstdModels;
                var CurChart = model.CurModels;
                var YstdTable = model.YstdTableEntities;
                var CurTable = model.CurTableEntities;
                var Prods = model.ProdList;
                var resopnse = new { success = true, CurChart, CurTable, YstdChart, YstdTable, Prods };
                return Json(resopnse, JsonRequestBehavior.DenyGet);
            }
            catch (Exception ex) 
            {
                return Json(new {success=false,msg=ex.Message },JsonRequestBehavior.DenyGet);
            }
        }

        public JsonResult GetLotDetail(ReqRpt028LotDetailQueryPostModel postModel)
        {
            var model = new ReqRpt028LotDetailTableBuilder(postModel);
            return Json(new { success=true,lotInfoEntities=model.LotInfoEntities});
        }

    }
}