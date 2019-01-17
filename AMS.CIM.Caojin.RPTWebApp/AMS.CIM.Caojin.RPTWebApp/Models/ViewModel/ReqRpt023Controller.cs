using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AMS.CIM.Caojin.RPTWebApp.Models;

namespace AMS.CIM.Caojin.RPTWebApp.Controllers
{
    public class ReqRpt023Controller : Controller
    {

        // GET: ReqRpt023
        public ActionResult Index()
        {
            return View(new ReqRpt023MainViewModel());
        }

        public PartialViewResult GetTableByAllCondition(ReqRpt023PostViewModel postViewModel)
        {
            try { return PartialView(new ReqRpt023TableViewModel(postViewModel)); }
            catch (Exception)
            {
                return null;
            }
        }

        public PartialViewResult GetLotDetail(ReqRpt023LotDetailQueryPostModel postModel)
        {
            try
            {
                return PartialView(new ReqRpt023LotDetailTableViewModel(postModel));
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}