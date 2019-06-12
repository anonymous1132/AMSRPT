using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AMS.CIM.Caojin.RPTWebApp.Models;

namespace AMS.CIM.Caojin.RPTWebApp.Controllers
{
    /// <summary>
    ///  All in one
    /// </summary>
    public class ReqRpt002Controller : Controller
    {
        // GET: ReqRpt002
        public ActionResult Index()
        {
           // ViewBag.ClickCount = CommonController.GetCount("RPT000002");
            return View();
        }

        public JsonResult GetAllDatas(string date)
        {
            try
            {
                var builder = new ReqRpt002TableDataBuilder(date);
                var tableData = new { moduleEntities = builder.DeptTableModuleRowEntities, testEntities = builder.DeptTableTestRowEntities, bankEntities = builder.DeptTableBankRowEntities, moduleTotal = builder.ModuleTotal, bankTotal = builder.BankTotal, testTotal = builder.TestTotal, fabTotal = builder.FabTotal, waferOutEntities = builder.WaferOutEntities, waferOutTotalEntity = builder.WaferOutTotalEntity };
                var response = new { success = true, tableData };
                return Json(response);
            }
            catch (Exception ex)
            {
                return Json(new { success=false,msg=ex.Message});
            }
        }

        public JsonResult GetCurWipAndHoldAndMoveDatas(string lastUpdateTime)
        {
            try
            {
                DateTime dt = DateTime.Parse(lastUpdateTime);
                if (dt < DateTime.Now.Date.AddHours(8) && DateTime.Now.Hour > 8)
                {
                    //需要全部更新
                    var builder = new ReqRpt002TableDataBuilder(DateTime.Now.ToString("yyyy-MM-dd"));
                    var tableData = new { moduleEntities = builder.DeptTableModuleRowEntities, testEntities = builder.DeptTableTestRowEntities, bankEntities = builder.DeptTableBankRowEntities, moduleTotal = builder.ModuleTotal, bankTotal = builder.BankTotal, testTotal = builder.TestTotal, fabTotal = builder.FabTotal, waferOutEntities = builder.WaferOutEntities, waferOutTotalEntity = builder.WaferOutTotalEntity };
                    var response = new { success = true, type = "all", tableData };
                    return Json(response);
                }
                else
                {
                    var updater = new ReqRpt002TableDataUpdater();
                    var tableData = new { moduleEntities = updater.DeptTableModuleRowEntities, bankEntities = updater.DeptTableBankRowEntities, testEntities = updater.DeptTableTestRowEntities, moduleTotal = updater.ModuleTotal, testTotal = updater.TestTotal, bankTotal = updater.BankTotal, fabTotal = updater.FabTotal };
                    var response = new { success = true, type = "cur", tableData };
                    return Json(response);
                }
            }
            catch (Exception ex)
            {
                return Json(new { success=false,msg=ex.Message});
            }
        }

        public JsonResult GetMoveTargetByMonth(string month)
        {
            try
            {
                var model = new ReqRpt002MoveTargetOperator();
                model.GetTargetListByMonth(month);
                var response = new { success = true, model.Days, model.Month, model.TargetList };
                return Json(response);
            }
            catch (Exception ex)
            {
                return Json(new { success=false,msg=ex.Message});
            }
        }

        public JsonResult SetMoveTarget(ReqRpt002SetMoveTargetPostModel postModel)
        {
            try
            {
                var oper = new ReqRpt002MoveTargetOperator();
                oper.SetTargetListByMonth(postModel);
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success=false,msg=ex.Message});
            }
        }


    }
}