using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AMS.CIM.Caojin.RPTWebApp.Models;
using AMS.CIM.Caojin.RPTLibrary;


namespace AMS.CIM.Caojin.RPTWebApp.Controllers
{
    public class ReqRpt024Controller : Controller
    {

        private ReqRpt024MainViewModel MainViewModel = new ReqRpt024MainViewModel();
        // GET: ReqRpt024
        public ActionResult Index()
        {
            return View(MainViewModel);
        }

        private ReqRpt024TableViewModel tableViewModel;
        public PartialViewResult GetTableByDepartment(ReqRpt024PostViewModel viewModel)
        {
            try
            {
                tableViewModel = new ReqRpt024TableViewModel(viewModel);
                return PartialView(tableViewModel);
            }
            catch (Exception)
            {
                return null;
            }
        }

        private ReqRpt024LotDetailTableViewModel lotDetailTableViewModel;
        public ActionResult GetLotDetail(ReqRpt024LotDetailQueryPostViewModel viewModel)
        {
            try
            {
                lotDetailTableViewModel = new ReqRpt024LotDetailTableViewModel(viewModel);
                var rows = lotDetailTableViewModel.entities.Select(s => new { s.Lot_ID, s.Cast_ID, s.Status,s.Location,s.Ope_Category,strClaim_Time= s.Claim_Time.ToString("yyyy-MM-dd HH:mm:ss"), s.Cur_Wafer_Qty,s.Ope_No,s.ModulePD_ID,s.PD_Name, s.Claim_User_ID, s.Eqp_ID,s.Recipe_ID,s.Reticle_ID,s.Reason_Code,s.Reason_Description,s.ProdSpec_ID });
                int total = rows.Count();
                return Json(new { total, rows }, JsonRequestBehavior.DenyGet);
            }
            catch (Exception)
            {
                return null;
            }
        }

        //原Html方式作废
        //public PartialViewResult GetLotDetailBak(ReqRpt024LotDetailQueryPostViewModel viewModel)
        //{
        //    try
        //    {
        //        lotDetailTableViewModel = new ReqRpt024LotDetailTableViewModel(viewModel);
        //        return PartialView(lotDetailTableViewModel);
        //    }
        //    catch (Exception)
        //    {
        //        return null;
        //    }
        //}
    }
}