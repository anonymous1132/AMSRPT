using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AMS.CIM.Caojin.RPTWebApp.Models;

namespace AMS.CIM.Caojin.RPTWebApp.Controllers
{
    /// <summary>
    /// scrap list
    /// </summary>
    public class ReqRpt022Controller : Controller
    {
        // GET: ReqRpt022
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetTableData(string startTime,string endTime)
        {
            var model = new ReqRpt022TableModel(startTime,endTime);
            var response = new { success = true, model.RowEntities };
            return Json(response);
        }
    }
}