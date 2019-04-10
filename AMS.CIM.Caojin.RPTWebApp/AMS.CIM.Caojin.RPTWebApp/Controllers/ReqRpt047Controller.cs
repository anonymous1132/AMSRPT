using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AMS.CIM.Caojin.RPTLibrary.Models;
using AMS.CIM.Caojin.RPTWebApp.Models;
using Caojin.Common;

namespace AMS.CIM.Caojin.RPTWebApp.Controllers
{
    public class ReqRpt047Controller : Controller
    {
        // GET: ReqRpt047
        public ActionResult Index()
        {
            DB2DataCatcher<RPTFuncUsage> CountCatcher = new DB2DataCatcher<RPTFuncUsage>("ISTRPT.RPTFuncUsage") { Conditions = "where privilegeid='RPT000047'" };
            var list = CountCatcher.GetEntities().EntityList;
            double count = list.Any() ? list.First().Usage_Counter : 0;
            ViewBag.ClickCount = count;
            return View();
        }

        public JsonResult GetSelectOptions()
        {
            var model = new ReqRpt047GetSelectOptionsModel();
            var response = new {success=true, model.DepartmentEntities,model.EqpTypeEntities};
            return Json(response,JsonRequestBehavior.DenyGet);
        }

        public JsonResult GetTableViewModel(List<string>Eqps)
        {
            try
            {
                var model = new ReqRpt047MainTableDataBuilder(Eqps);
                var response = new { success = true, model.RowEntities };
                return Json(response, JsonRequestBehavior.DenyGet);
            }
            catch (Exception e)
            {
                return Json(new { success = false, msg = e.Message });
            }
        }

        public JsonResult MaintainEDCTable(ReqRpt047MaintainEdcPostModel postModel)
        {
            var model = new ReqRpt047MaintainEDCHandler(postModel);
            var response = new { success = model.Success,msg=model.Msg,newID=DateTime.Now.GetHashCode() };
            return Json(response);
        }

        public JsonResult GetDcitemList()
        {
            string sql = "select distinct(dcitem_name) from ISTRPT.Report47_Proc";
            DB2Helper db2 = new DB2Helper();
            db2.GetSomeData(sql);
            List<string> list = new List<string>();
            for (var i = 0; i < db2.dt.Rows.Count; i++)
            {
                list.Add(db2.dt.Rows[i][0].ToString());
            }
            return Json(new {success=true, DcItems=list});
        }

        public JsonResult GetEDCTable()
        {
            DB2DataCatcher<Rpt_Non_Lot_Edc_Plan> catcher = new DB2DataCatcher<Rpt_Non_Lot_Edc_Plan>("ISTRPT.Rpt_Non_Lot_Edc_Plan");
            List<Rpt_Non_Lot_Edc_Plan> list = new List<Rpt_Non_Lot_Edc_Plan>();
            catcher.GetEntities();
            list = catcher.entities.EntityList.Any() ? catcher.entities.EntityList.ToList() : list;
            var response = list.Select(s => new {EqpID= s.Eqp_ID,EdcPlan= s.Edc_Plan, s.Period,PeriodType= s.Period_Type, id = s.GetHashCode() });
            return Json(new { success = true,EdcTable=response });
        }
    }
}