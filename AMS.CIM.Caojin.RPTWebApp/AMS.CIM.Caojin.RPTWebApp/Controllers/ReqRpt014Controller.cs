using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AMS.CIM.Caojin.RPTWebApp.Models;

namespace AMS.CIM.Caojin.RPTWebApp.Controllers
{
    public class ReqRpt014Controller : Controller
    {
        // GET: ReqRpt014
        /// <summary>
        /// 测试
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            try
            {
                return View(new ReqRpt014MainViewModel());
            }
            catch (Exception)
            {
                Response.Redirect("~/ErrorPage/Index");
                return View();
            }
        }

        public ActionResult Top3()
        {
            try
            {
                return View(new ReqRpt014MainViewModel());
            }
            catch (Exception)
            {
                Response.Redirect("~/ErrorPage/Index");
                return View();
            }
        }


        public PartialViewResult GetTableView(ReqRpt014PostModel postModel)
        {
            try
            {
                ReqRpt014TableViewModel tableViewModel = new ReqRpt014TableViewModel(postModel);
                return PartialView(tableViewModel);
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}