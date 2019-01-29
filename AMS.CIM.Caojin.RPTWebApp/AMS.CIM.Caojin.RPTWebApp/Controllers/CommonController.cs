using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AMS.CIM.Caojin.RPTWebApp.Controllers
{
    public class CommonController : Controller
    {
        // GET: Common
        public ActionResult GetFile(string fileName)
        {
            string root = "~/App_Data";
            string path = System.IO.Path.Combine(root, fileName);
            string sPath = Server.MapPath(path);
            if (System.IO.File.Exists(sPath))
            {
                return File(sPath, "application/ms-excel", fileName);
            }
            else {
                Response.Redirect("~/ErrorPage/Index");
                return null;
            }
        }
    }
}