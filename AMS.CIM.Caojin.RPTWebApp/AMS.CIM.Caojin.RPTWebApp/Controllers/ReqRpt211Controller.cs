using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AMS.CIM.Caojin.RPTWebApp.Models;

namespace AMS.CIM.Caojin.RPTWebApp.Controllers
{
    public class ReqRpt211Controller : Controller
    {
        // GET: ReqRpt211
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetTableData(string lot)
        {
            var model = ReqRpt211TableDataBuilder.GetRowEntities(lot);
            var response = new { success=true,tableData=model};
            return Json(response);
        }

        public JsonResult GetLotProdMapping()
        {
            var model = ReqRpt211TableDataBuilder.GetLotAndProdMappingList();
            var response = new { success = true, prodLotMap = model };
            return Json(response);
        }
    }
}