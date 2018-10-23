using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AMS.CIM.Caojin.RPTWebApp.Models;
using System.Threading;
using System.Web.Services;

namespace AMS.CIM.Caojin.RPTWebApp.Controllers
{
    public class RptController : Controller
    {
        // GET: Rpt
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ReqRpt018()
        {
   
            try
            {
                ViewBag.Modules = ShareDataEntity.GetSingleEntity().Rpt018.Modules;
                ViewBag.EqpTypes = ShareDataEntity.GetSingleEntity().Rpt018.EqpType;
                ViewBag.EqpID = ShareDataEntity.GetSingleEntity().Rpt018.EqpID;
                ViewBag.QueryContent = ShareDataEntity.GetSingleEntity().Rpt018.SelectedContent;
                return View();
            }
            catch (Exception)
            {
                return null;
            }
        }

        private ReqRpt018TableViewModel TableViewModel;
        //直接返回布局页的方式：HTML
        public PartialViewResult GetTableByAllConditions(ReqRpt018PostViewModel viewModel)
        {
            try
            {
                TableViewModel = new ReqRpt018TableViewModel(viewModel);

                return PartialView(TableViewModel);
            }
            catch (Exception)
            {
                return null;
            }
        }

        private List<ReqRpt018EqpTypeListViewModel> EqpTypeListViewModel;
        public PartialViewResult GetSelectListByModule(string module)
        {
            try
            {
                var test = ShareDataEntity.GetSingleEntity().FRUserCatcher.GetEntities().EntityList;
                var ownerlist = ShareDataEntity.GetSingleEntity().FRUserCatcher.GetEntities().EntityList.Where(w => w.Department == module).Select(s => s.Owner_ID).Distinct().ToList();
                var eqptypelist = ShareDataEntity.GetSingleEntity().Rpt018.Rpt018GroupModel.ReqRpt018EqpStatusEntities.Where(w => !string.IsNullOrEmpty(w.EQP_Type) && ownerlist.Contains(w.Owner_ID)).Select(s => s.EQP_Type).Distinct().ToList();
                EqpTypeListViewModel = eqptypelist.Select(f => new ReqRpt018EqpTypeListViewModel { EqpTypeValue = f, EpqType = f }).ToList();

                return PartialView(EqpTypeListViewModel);
            }
            catch (Exception)
            { return null; }
        }
    }


}