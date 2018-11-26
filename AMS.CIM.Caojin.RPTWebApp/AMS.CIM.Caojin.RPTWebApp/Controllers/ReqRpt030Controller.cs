using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AMS.CIM.Caojin.RPTWebApp.Models;

namespace AMS.CIM.Caojin.RPTWebApp.Controllers
{
    public class ReqRpt030Controller : Controller
    {
        // GET: ReqRpt030
        public ActionResult Index()
        {
            return View(new ReqRpt030MainViewModel() );
        }
    }
}