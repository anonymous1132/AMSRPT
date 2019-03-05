using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AMS.CIM.Caojin.RPTLibrary.Models;
using AMS.CIM.Caojin.RPTWebApp.Models;

namespace AMS.CIM.Caojin.RPTWebApp.Controllers
{
    public class ReqRpt048Controller : Controller
    {
        // GET: ReqRpt048
        public ActionResult Index()
        {
            DB2DataCatcher<RPTFuncUsage> CountCatcher = new DB2DataCatcher<RPTFuncUsage>("ISTRPT.RPTFuncUsage") { Conditions="where privilegeid='RPT000048'"};
            var list = CountCatcher.GetEntities().EntityList;
            double count = list.Any() ? list.First().Usage_Counter : 0;
            ViewBag.ClickCount = count;
            return View();
        }


        public JsonResult GetMainTableView(string Priority)
        {
            try
            {
                ReqRpt048SHLTableViewModel viewModel = new ReqRpt048SHLTableViewModel(Priority);
                var Rows = viewModel.FilterdSHLEntities.Select(s => new { s.Department, s.Description, s.FoupID, s.Gap, s.Location, s.LotID, s.LotStates, s.ProcessStates, s.PriChgStage, s.Priority, s.Project, s.Purpose, s.Qty, s.QuotaOwner, s.QuotaType, s.Remark, s.Status, s.strTargetWFOut, s.strWaferStart, s.strWAT, s.strWFOut, s.YSDT, EditState = false, s.ProductID, s.QuotaDept, s.OpeNo });
                var ProductQuota = new { Rows = viewModel.ProductQuotaEntities };
                var DepartmentQuota = new { Rows = viewModel.DepartmentQuotaEntities };
               // var ProjectQuota = new { Rows = viewModel.ProjectQuotaEntities };
                var Quotas = viewModel.ProjectQuotaEntities.Select(s => new { id = s.GetHashCode(), s.Department, s.Project, s.Purpose, s.QuotaType, s.QuotaHL, s.QuotaSHL, s.UsedHL, s.UsedSHL, s.RemnantHL, s.RemnantSHL, EditState = false });
                var ProjectQuota = new { Rows = Quotas.Where(w => w.QuotaType == 1) };
                var NormalQuota = new { Rows = Quotas.Where(w => w.QuotaType == 0) };
                var response = new { successed = true, Conditions = viewModel.QueryConditions, Rows, ProductQuota, DepartmentQuota, ProjectQuota, NormalQuota };
                return Json(response, JsonRequestBehavior.DenyGet);
            }
            catch (Exception ex)
            {
                return Json(new { successed = false, msg = ex.Message }, JsonRequestBehavior.DenyGet);
            }
        }

        //验证口令
        public JsonResult CheckKey(string key)
        {
            ReqKeyOperateModel model = new ReqKeyOperateModel();
            bool res= model.CheckKey("ReqRpt00048", key);
            return Json(new { successed = res });
        }
        //更改口令
        public JsonResult UpdateKey(string newKey,string oldKey)
        {
            ReqKeyOperateModel model = new ReqKeyOperateModel();
            string msg = "";
            bool successed = model.CheckKey("ReqRpt00048", oldKey);
            if (successed)
            {
                successed = model.UpdateKey("ReqRpt00048",newKey);
                msg = successed ? "口令已更新！" : "口令更新出现未知错误，请联系开发人员！";
            }
            else
            {
                msg = "口令认证失败！";
            }
            return Json(new { successed,msg},JsonRequestBehavior.DenyGet);
        }

        //操作QuotaDefine
        public JsonResult QuotaHandle(Rpt_Quota_Define define)
        {
            try
            {
                var handle = new ReqRpt048QuotaDefineHandler(define);
                return Json(new { handle.Success,handle.Msg,newID=DateTime.Now.GetHashCode()});
            }
            catch (Exception e)
            {
                return Json(new { Success=false,Msg=e.Message});
            }
        }

        //操作QuotaProjectLotMapping
        public JsonResult QuotaProjectLotMappingHandle(string lotid,string project)
        {
            try
            {
                var handle = new ReqRpt048LotQuotaMappingHandler(lotid, project);
                return Json(new { handle.Success });
            }
            catch (Exception ex)
            {
                return Json(new { Success = false, Msg = ex.Message });
            }
                
        }

        //stage2，查询forecast&history
        public JsonResult Stage2Query(string lot,string product)
        {
            var model = new ReqRpt048SHLStage2ViewModel(lot,product);

            return Json(new {success=true,model.Entities } );
        }

        //操作Remark
        public JsonResult RemarkHandle(ReqRpt048RemarkHandlePostModel postModel)
        {
            try
            {
                ReqRpt048RemarkHandler handler = new ReqRpt048RemarkHandler(postModel);
                return Json(new { success=true,handler.Msg},JsonRequestBehavior.DenyGet);
            }
            catch (Exception ex)
            {
                return Json(new { success=false,Msg="操作失败",Error=ex.Message},JsonRequestBehavior.DenyGet);
            }
        }
    }
}