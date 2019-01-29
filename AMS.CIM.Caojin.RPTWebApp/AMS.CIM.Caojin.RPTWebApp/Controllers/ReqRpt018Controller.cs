using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AMS.CIM.Caojin.RPTWebApp.Models;
using Caojin.Common;

namespace AMS.CIM.Caojin.RPTWebApp.Controllers
{
    public class ReqRpt018Controller : Controller
    {
        // GET: ReqRpt018
        public ActionResult Index()
        {
            ReqRpt018MainViewModel Rpt018MainViewModel = new ReqRpt018MainViewModel();
            try
            {
                ViewBag.Modules = Rpt018MainViewModel.Modules;
                ViewBag.EqpTypes = Rpt018MainViewModel.EqpType;
                ViewBag.EqpID = Rpt018MainViewModel.EqpID;
                ViewBag.ClickCount = Rpt018MainViewModel.StrClickCount;
                return View(ViewBag);
            }
            catch (Exception)
            {
                return null;
            }
        }

        private ReqRpt018TableViewModel TableViewModel;
        //直接返回布局页的方式：HTML
        public JsonResult GetTableByAllConditions(ReqRpt018PostViewModel viewModel)
        {
            try
            {
                TableViewModel = new ReqRpt018TableViewModel(viewModel);

                return Json(TableViewModel,JsonRequestBehavior.DenyGet);
            }
            catch (Exception)
            {
                return null;
            }
        }

        private string fixFigure(int s)
        {
            return s > 0 ? s.ToString() : "0" + s.ToString();
        }

        public JsonResult GetMonthlyRptData(string EqpID)
        {
            try
            {
                DateTime Now = DateTime.Now;
                DateTime LastWeek = Now.Date.AddDays(-7);
                DateTime FourWeeksAgo = Now.Date.AddDays(-4 * 7);
                int week_to_year = LastWeek.Year;
                int week_to_week = TimeHelper.GetWeekOfYear(LastWeek);
                int week_from_year = FourWeeksAgo.Year;
                int week_from_week = TimeHelper.GetWeekOfYear(FourWeeksAgo);

                var postModel = new ReqRpt018PostViewModel() { type = "week", frame = "", from = string.Format("{0}-W{1}", week_from_year, fixFigure(week_from_week)), to = string.Format("{0}-W{1}", week_to_year, fixFigure(week_to_week)), selectedeqpid = EqpID };

                TableViewModel = new ReqRpt018TableViewModel(postModel);
                var data = TableViewModel.entities[0].Datas.Select(s => new { s.Date, s.SD, s.UD, EN = s.ENGHour / s.TotalHour, s.UUm, s.UPm });
                var request = data;
                return Json(request, JsonRequestBehavior.DenyGet);
            }
            catch (Exception)
            { return null; }
        }


        private List<ReqRpt018EqpTypeListViewModel> EqpTypeListViewModel;
        public PartialViewResult GetSelectListByModule(string module)
        {
            ReqRpt018MainViewModel Rpt018MainViewModel = new ReqRpt018MainViewModel();
            try
            {
                if (module.ToUpper() == "ALL")
                {
                    EqpTypeListViewModel = Rpt018MainViewModel.EqpType.Select(s => new ReqRpt018EqpTypeListViewModel() { EpqType = s, EqpTypeValue = s }).ToList();
                }
                else
                {
                    EqpTypeListViewModel = Rpt018MainViewModel.db.EQPType_Department_Mapping.Where(w => w.Department == module).Select(s => new ReqRpt018EqpTypeListViewModel() { EpqType = s.EqpType, EqpTypeValue = s.EqpType }).Distinct().ToList();
                    EqpTypeListViewModel.Insert(0, new ReqRpt018EqpTypeListViewModel() { EpqType = "ALL", EqpTypeValue = "ALL" });
                }
                return PartialView(EqpTypeListViewModel);
            }
            catch (Exception)
            { return null; }
        }

        private List<ReqRpt018EqpIDListViewModel> EqpIDListViewModel;
        public PartialViewResult GetSelectListByEqpType(string eqpType, string module)
        {
            ReqRpt018MainViewModel Rpt018MainViewModel = new ReqRpt018MainViewModel();
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

        public JsonResult GetChart1Excel(ReqRpt018Chart1PostModel postModel)
        {
            string fileName = string.Format("UPmUUmStackChart{0}_{1}.xlsx",postModel.Date, DateTime.Now.ToString("yyyyMMddHHmmssffffff"));
            string root ="~/App_Data";
            string path = System.IO.Path.Combine(root,fileName);
            string sPath = Server.MapPath(path);
            //ReqRpt018Chart1ExcelWorkSpace excelWorkSpace=new ReqRpt018Chart1ExcelWorkSpace(sPath)
            //    {
            //        PostModel = postModel
            //    };
            
            try
            {
                ReqRpt018Chart1ExcelWorkSpace excelWorkSpace = new ReqRpt018Chart1ExcelWorkSpace(sPath)
                {
                    PostModel = postModel
                };
                excelWorkSpace.DoWork();
                if (System.IO.File.Exists(sPath)) { return Json(new { success = true, fileName }); }
                else { return Json(new { success = false, error = "没有正确产生文件！" }); }
                
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error=ex.Message });
            }
        }

        public JsonResult GetChart2Excel(ReqRpt018Chart2PostModel postModel) {
            string fileName = string.Format("UPmUUmMonthlyChart{0}_{1}.xlsx", postModel.EqpID, DateTime.Now.ToString("yyyyMMddHHmmssffffff"));
            string root = "~/App_Data";
            string path = System.IO.Path.Combine(root, fileName);
            string sPath = Server.MapPath(path);
            try
            {
                ReqRpt018Chart2ExcelWorkSpace excelWorkSpace = new ReqRpt018Chart2ExcelWorkSpace(sPath)
                {
                    PostModel = postModel
                };
                excelWorkSpace.DoWork();
                if (System.IO.File.Exists(sPath)) { return Json(new { success = true, fileName }); }
                else { return Json(new { success = false, error = "没有正确产生文件！" }); }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = ex.Message });
            }
        }

    }
}