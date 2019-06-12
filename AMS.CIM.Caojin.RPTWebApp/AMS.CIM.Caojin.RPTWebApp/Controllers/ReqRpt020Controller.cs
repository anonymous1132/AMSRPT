using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AMS.CIM.Caojin.RPTWebApp.Models;

namespace AMS.CIM.Caojin.RPTWebApp.Controllers
{
    public class ReqRpt020Controller : Controller
    {
        // GET: ReqRpt020
        public ActionResult Index()
        {
            return View(new ReqRpt020MainViewModel());
        }

        public ActionResult Test()
        {
            return View(new ReqRpt020MainViewModel());
        }

        public JsonResult SetTargetValue(List<ReqRpt020SetTargetPostModel> postModel)
        {
            try
            {
                var viewModel = new ReqRpt020SetTargetViewModel(postModel);
                return Json(new { message = viewModel.Message }, JsonRequestBehavior.DenyGet);
            }
            catch (Exception e)
            {
                return Json(new { message = e.Message }, JsonRequestBehavior.DenyGet);
            }
        }

        public PartialViewResult GetEqpTypeByModule(string module)
        {
            try
            {
                var model = new ReqRpt020EqpTypeSelectViewModel(module);
                return PartialView(model);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public JsonResult GetTableView(ReqRpt020TablePostModel postModel)
        {
            var model = new ReqRpt020TableViewModel(postModel);
            var responsData = new { model.Conditions,model.StartDate,model.EndDate,model.TbodyEntities};
            return Json(responsData,JsonRequestBehavior.DenyGet);
        }
    }
}