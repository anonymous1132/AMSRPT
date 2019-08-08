using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AMS.CIM.Caojin.RPTWebApp.Models;

namespace AMS.CIM.Caojin.RPTWebApp.Controllers
{
    public class ReqRpt212Controller : Controller
    {
        // GET: ReqRpt212
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetTableData(string from,string to)
        {
            try
            {
                var model = new ReqRpt212DataBuilder();
                model.GetTableData(from, to);
                var response = new { success = true, model.RowEntities,StartDate=from,EndDate=to };
                return Json(response);
            }
            catch (Exception ex)
            {
                return Json(new { success=false,msg=ex.Message});
            }
        }

        //public JsonResult GetProdList()
        //{
        //    try
        //    {
        //        var model = new ReqRpt209DataBuilder();
        //        var data = model.GetProdList();
        //        var response = new { success = true, prodList = data };
        //        return Json(response);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new { success = false, msg = ex.Message });
        //    }
        //}
    }
}