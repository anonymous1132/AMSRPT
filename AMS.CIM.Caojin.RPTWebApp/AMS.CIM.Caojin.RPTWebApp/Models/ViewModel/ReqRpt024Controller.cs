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
        public PartialViewResult GetLotDetail(ReqRpt024LotDetailQueryPostViewModel viewModel)
        {
            try
            {
                lotDetailTableViewModel = new ReqRpt024LotDetailTableViewModel(viewModel);
                return PartialView(lotDetailTableViewModel);
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}