using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AMS.CIM.Caojin.RPTLibrary.Models;
using AMS.CIM.Caojin.RPTWebApp.Models;


namespace AMS.CIM.Caojin.RPTWebApp.Controllers
{
    public class ReqRpt029Controller : Controller
    {
        // GET: ReqRpt029
        public ActionResult Index()
        {
            DB2DataCatcher<RPTFuncUsage> CountCatcher = new DB2DataCatcher<RPTFuncUsage>("ISTRPT.RPTFuncUsage") { Conditions = "where privilegeid='RPT000029'" };
            var list = CountCatcher.GetEntities().EntityList;
            double count = list.Any() ? list.First().Usage_Counter : 0;
            ViewBag.ClickCount = count;
            return View();
        }

        public JsonResult GetTableEntities()
        {
            try
            {
                var model = new ReqRpt029TableViewModel();
                var Entities = model.Entities.Select(s => new { s.LotID, s.OpeNo, s.FoupID, s.Location, s.Status, s.Qtime, s.RemainQt, s.FlowFactor, s.StrFlowFactor, Dept = s.Department, s.Step, s.Priority, s.Qty, s.EqpType, LotStates = s.LotState, s.HoldCode, s.HoldComment, ToDept = s.ToDepartment, s.ToStep, s.ToOpeNo, s.ToEqpType });

                return Json(new { success = true, Entities });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, Msg = ex.Message });
            }
        }

    }
}