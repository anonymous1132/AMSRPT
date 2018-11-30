using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AMS.CIM.Caojin.RPTWebApp.Models;

namespace AMS.CIM.Caojin.RPTWebApp.Controllers
{
    public class ReqRpt030Controller : Controller
    {
        // GET: ReqRpt030
        public ActionResult Index()
        {
            return View(new ReqRpt030MainViewModel() );
        }

        public ActionResult ReSetMySpecialLot(ReqRpt030SetSpecialLotPostModel postModel)
        {
            try
            {
                ReqRpt030SetSpecialLotResultModel resaultModel = new ReqRpt030SetSpecialLotResultModel(postModel);
                return Json(new { resaultModel.AllIn, resaultModel.ErrorID,resaultModel.RepeatID }, JsonRequestBehavior.DenyGet);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public ActionResult GetAllSpecialLot()
        {
            try
            {
                ReqRpt030MainViewModel mainViewModel = new ReqRpt030MainViewModel();
                return Json(new { mainViewModel.Lot_ID}, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public ActionResult DeleteSpecialLotID(string LotID)
        {
            try
            {
                ReqRpt030DeleteSpecialLotResultModel model = new ReqRpt030DeleteSpecialLotResultModel(LotID);
                return Json(new {message="success" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { message = "failed:"+e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult GetDetailByLotID(string LotID)
        {
            try
            {
                ReqRpt030GetDetailModel model = new ReqRpt030GetDetailModel(LotID);
                var TableModel = new { total=model.EntityList.Count,rows=model.EntityList};
                return Json(TableModel, JsonRequestBehavior.DenyGet);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public ActionResult UpdateRemarkPurpose(ReqRpt030UpdateRemarkPurposePostModel postModel)
        {
            try
            {
                ReqRpt030UpdateRemarkPurposeResultModel model = new ReqRpt030UpdateRemarkPurposeResultModel(postModel);
                return Json(new { success=true},JsonRequestBehavior.DenyGet);
            }
            catch (Exception e)
            {
                return Json(new { success = false,message=e.Message },JsonRequestBehavior.DenyGet);
            }
            
        }
    }
}