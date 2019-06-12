using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AMS.CIM.Caojin.RPTWebApp.Models;

namespace AMS.CIM.Caojin.RPTWebApp.Controllers
{
    public class ReqRpt063Controller : Controller
    {
        // GET: ReqRpt063 ClickCount
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetCategoryList()
        {
            var model = new ReqRpt0063BrprivilegeDataBuilder();
            var response = new { success = true, model.BrprivilegeList };
            return Json(response);
        }

        public JsonResult GetTableDataOfMonth(List<string>privilegeIdList,int year,int month)
        {
            var model = new ReqRpt063TableDataBuilder();
            model.GetDataByMonth(privilegeIdList,year,month);
            var response = new { success = true, model.Items,model.ClickCountEntities };
            return Json(response);
        }

        public JsonResult GetTableDataOfYear(List<string> privilegeIdList)
        {
            var model = new ReqRpt063TableDataBuilder();
            model.GetDataByYear(privilegeIdList);
            var response = new { success = true, model.Items, model.ClickCountEntities };
            return Json(response);
        }
    }
}