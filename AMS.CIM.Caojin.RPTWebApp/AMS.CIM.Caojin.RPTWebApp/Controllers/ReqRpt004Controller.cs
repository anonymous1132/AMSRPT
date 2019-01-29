using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AMS.CIM.Caojin.RPTWebApp.Models;

namespace AMS.CIM.Caojin.RPTWebApp.Controllers
{
    public class ReqRpt004Controller : Controller
    {
        // GET: ReqRpt004
        public ActionResult Index()
        {
            try
            {
                return View(new ReqRpt004MainViewModel());
            }
            catch (Exception)
            {
                return null;
            }

        }

        public PartialViewResult GetTableView(string Products,string TargetDate)
        {
            try
            {
                return PartialView(new ReqRpt004TableViewModel(Products, TargetDate));
            }
            catch (Exception)
            {
                return null;
            }
        }

        public JsonResult GetJson(string Products,string TargetDate)
        {
            var viewModel = new ReqRpt004TableViewModel(Products, TargetDate);
            var barData = viewModel.ChartList;
            var tableData =new { viewModel.TableRowEntities,viewModel.DateList,viewModel.DailyTotalTurn };
            //var responseData = new {tableData,viewModel.FormatTargetDate};
            var responseData = new { barData,tableData,viewModel.FormatTargetDate};
            return Json(responseData,JsonRequestBehavior.DenyGet);

        }
    }
}