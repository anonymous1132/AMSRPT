using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AMS.CIM.Caojin.RPTWebApp.Models;

namespace AMS.CIM.Caojin.RPTWebApp.Controllers
{
    public class ReqRpt207Controller : Controller
    {
        // GET: ReqRpt207
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetTableData(string eqp,string startDate,string endDate)
        {
            try
            {
                var model = new ReqRpt207DataBuilder();
                model.GetTableData(eqp, startDate, endDate);
                var response = new { success = true, model.RowEntities, model.EqpStr, model.StartDate, model.EndDate };
                return Json(response);
            }
            catch (Exception ex)
            {
                return Json(new { success=false,msg=ex.Message});
            }
        }
    }
}