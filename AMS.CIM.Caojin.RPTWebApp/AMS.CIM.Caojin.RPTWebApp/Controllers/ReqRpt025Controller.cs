using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AMS.CIM.Caojin.RPTWebApp.Models;

namespace AMS.CIM.Caojin.RPTWebApp.Controllers
{
    public class ReqRpt025Controller : Controller
    {
        // GET: ReqRpt025
        public ActionResult Index()
        {
            return View();
        }

        public PartialViewResult GetTable(ReqRpt025PostModel postModel)
        {
            try
            {
                ReqRpt025TableViewModel tableViewModel = new ReqRpt025TableViewModel(postModel);
                return PartialView(tableViewModel);
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}