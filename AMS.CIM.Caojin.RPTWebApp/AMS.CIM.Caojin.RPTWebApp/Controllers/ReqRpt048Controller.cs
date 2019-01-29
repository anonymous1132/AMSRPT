using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AMS.CIM.Caojin.RPTLibrary.Models;

namespace AMS.CIM.Caojin.RPTWebApp.Controllers
{
    public class ReqRpt048Controller : Controller
    {
        // GET: ReqRpt048
        public ActionResult Index()
        {
            DB2DataCatcher<RPTFuncUsage> CountCatcher = new DB2DataCatcher<RPTFuncUsage>("ISTRPT.RPTFuncUsage") { Conditions="where privilegeid='RPT000048'"};
            var list = CountCatcher.GetEntities().EntityList;
            double count = list.Any() ? list.First().Usage_Counter : 0;
            ViewBag.ClickCount = count;
            return View();
        }
    }
}