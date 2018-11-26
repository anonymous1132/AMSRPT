using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AMS.CIM.Caojin.RPTWebApp.Models;

namespace AMS.CIM.Caojin.RPTWebApp.Controllers
{
    public class ReqRpt023Controller : Controller
    {

        // GET: ReqRpt023
        public ActionResult Index()
        {
            return View(new ReqRpt023MainViewModel());
        }

        public PartialViewResult GetTableByAllCondition(ReqRpt023PostViewModel postViewModel)
        {
            try { return PartialView(new ReqRpt023TableViewModel(postViewModel)); }
            catch (Exception)
            {
                return null;
            }
        }

        public JsonResult GetLotDetail(ReqRpt023LotDetailQueryPostModel postModel)
        {
            try
            {
                ReqRpt023LotDetailTableViewModel ViewModel = new ReqRpt023LotDetailTableViewModel(postModel);
                var rows = ViewModel.entities.Select(s => new { s.Lot_ID, s.Cast_ID,  s.Ope_Category, strClaim_Time = s.Claim_Time.ToString("yyyy-MM-dd HH:mm:ss"), s.Cur_Wafer_Qty, s.Ope_No, s.ModulePD_ID, s.PD_Name, s.Claim_User_ID, s.Eqp_ID, s.Recipe_ID, s.Reticle_ID, s.Reason_Code, s.Reason_Description, s.ProdSpec_ID });
                int total = rows.Count();
                return Json(new { total, rows }, JsonRequestBehavior.DenyGet);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public PartialViewResult GetProdList(string lotType)
        {
            try
            {
                return PartialView(new ReqRpt023GetProdListViewModel(lotType));
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}