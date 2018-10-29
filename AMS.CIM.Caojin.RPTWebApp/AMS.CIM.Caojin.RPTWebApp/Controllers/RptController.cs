using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AMS.CIM.Caojin.RPTLibrary.Models;
using AMS.CIM.Caojin.RPTWebApp.Models;

namespace AMS.CIM.Caojin.RPTWebApp.Controllers
{
    public class RptController : Controller
    {
        // GET: Rpt
        public ActionResult Index()
        {
            return View();
        }

        private ReqRpt018MainViewModel Rpt018MainViewModel=new ReqRpt018MainViewModel();
        public ActionResult ReqRpt018()
        {
            try
            {
                ViewBag.Modules =Rpt018MainViewModel.Modules;
                ViewBag.EqpTypes = Rpt018MainViewModel.EqpType;
                ViewBag.EqpID = Rpt018MainViewModel.EqpID;
                return View(ViewBag);
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
                if (module.ToUpper() == "ALL")
                {
                    EqpTypeListViewModel = Rpt018MainViewModel.EqpType.Select(s => new ReqRpt018EqpTypeListViewModel() { EpqType = s, EqpTypeValue = s }).ToList();
                }
                else
                {
                    EqpTypeListViewModel = Rpt018MainViewModel.db.EQPType_Department_Mapping.Where(w => w.Department == module).Select(s=>new ReqRpt018EqpTypeListViewModel() { EpqType=s.EqpType,EqpTypeValue=s.EqpType}).Distinct().ToList();
                    EqpTypeListViewModel.Insert(0, new ReqRpt018EqpTypeListViewModel() { EpqType = "ALL", EqpTypeValue = "ALL" });
                }
                return PartialView(EqpTypeListViewModel);
            }
            catch (Exception)
            { return null; }
        }

        private List<ReqRpt018EqpIDListViewModel> EqpIDListViewModel;
        public PartialViewResult GetSelectListByEqpType(string eqpType,string module)
        {
            try
            {
                if (eqpType.ToUpper() == "ALL")
                {
                    if (module.ToUpper() == "ALL") EqpIDListViewModel = Rpt018MainViewModel.EqpID.Select(s => new ReqRpt018EqpIDListViewModel() { EqpID = s, EqpIDValue = s }).ToList();
                    else
                    {
                        var eqptypelist = Rpt018MainViewModel.db.EQPType_Department_Mapping.Where(w => w.Department == module).Select(s => s.EqpType).Distinct().ToList();
                        EqpIDListViewModel = Rpt018MainViewModel.db.EQP_UPm_018.Where(w => eqptypelist.Contains(w.EqpType)).Select(s => new ReqRpt018EqpIDListViewModel() { EqpID = s.EqpID, EqpIDValue = s.EqpID }).Distinct().ToList();
                    } 
                }
                else
                {

                    EqpIDListViewModel = Rpt018MainViewModel.db.EQP_UPm_018.Where(w => w.EqpType == eqpType).Select(s => new ReqRpt018EqpIDListViewModel() { EqpID = s.EqpID, EqpIDValue = s.EqpID }).Distinct().ToList();
                }
                return PartialView(EqpIDListViewModel);
            }
            catch (Exception)
            { return null; }
        }

    }


}