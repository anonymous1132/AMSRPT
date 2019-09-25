using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using AMS.CIM.Caojin.RPTWebApp.Models;
using Caojin.Common;
using Newtonsoft.Json;

namespace AMS.CIM.Caojin.RPTWebApp.Controllers
{
    public class ReqRpt217Controller : Controller
    {
        // GET: ReqRpt213
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult getlotnamelist()
        {
            try
            {
                string path = @"Z:\WAT";
                DirectoryInfo root = new DirectoryInfo(path);
                FileInfo[] filenames = root.GetFiles();
                var filenamewithdatlist = filenames.Where(t => t.Extension == ".dat");
                ArrayList Lotlist = new ArrayList();
                foreach (var name in filenamewithdatlist)
                {
                    Lotlist.Add(Path.GetFileNameWithoutExtension(name.FullName));
                }
                return Json(Lotlist);
            }
            catch (Exception)
            {
                return Json("error");
            }
        }
    }
}