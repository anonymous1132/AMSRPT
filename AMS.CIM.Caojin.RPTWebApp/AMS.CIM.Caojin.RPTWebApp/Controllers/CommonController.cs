using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Caojin.Common;
using AMS.CIM.Caojin.RPTLibrary.Models;

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

        /// <summary>
        /// 获取所有Product
        /// </summary>
        /// <param name="type"> 
        /// 0:Dummy
        /// 1:Equipment Monitor
        /// 2:Process Monitor
        /// 3:Production
        /// 4:Raw
        /// 5:Recycle
        /// 6:Production without SL
        /// Default(>6):All
        /// </param>
        /// <returns></returns>
        public JsonResult GetAllProduct(int type)
        {
            try
            {
                string sql = "select prodspec_id from mmview.fvprodspec ";
                switch (type)
                {
                    case 0:
                        sql += "where prodcat_id='Dummy'";
                        break;
                    case 1:
                        sql += "where prodcat_id='Equipment Monitor'";
                        break;
                    case 2:
                        sql += "where prodcat_id='Process Monitor'";
                        break;
                    case 3:
                        sql += "where prodcat_id='Production'";
                        break;
                    case 4:
                        sql += "where prodcat_id='Raw'";
                        break;
                    case 5:
                        sql += "where prodcat_id='Recycle'";
                        break;
                    case 6:
                        sql += "where prodcat_id='Production' and prodspec_id not like 'SL%'";
                        break;
                    default:
                        break;
                }
                DB2Helper dB2 = new DB2Helper();
                dB2.GetSomeData(sql);
                List<string> Prods = new List<string>();
                for (var i = 0; i < dB2.dt.Rows.Count; i++)
                {
                    Prods.Add(dB2.dt.Rows[i][0].ToString());
                }
                return Json(new { success = true, prods = Prods });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, msg=ex.Message});
            }
        }

        public static double GetCount(string title)
        {
            DB2DataCatcher<RPTFuncUsage> CountCatcher = new DB2DataCatcher<RPTFuncUsage>("ISTRPT.RPTFuncUsage") { Conditions =string.Format( "where privilegeid='{0}'",title) };
            var list = CountCatcher.GetEntities().EntityList;
            double count = list.Any() ? list.First().Usage_Counter : 0;
            return count;
        }

        public JsonResult GetClickCount(string title)
        {
            try
            {
                double res = CommonController.GetCount(title);
                return Json(new { success = true, count = res });
            }
            catch (Exception ex)
            {
                return Json(new { success=false,msg=ex.Message});
            }
        }
    }
}