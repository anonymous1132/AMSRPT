using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AMS.CIM.Caojin.RPTLibrary.Models;
using Caojin.Common;
using AMS.CIM.Caojin.RPTWebApp.Models;

namespace AMS.CIM.Caojin.RPTWebApp.Controllers
{
    public class ReqRpt062Controller : Controller
    {
        // GET: ReqRpt062
        public ActionResult Index()
        {
            DB2DataCatcher<RPTFuncUsage> CountCatcher = new DB2DataCatcher<RPTFuncUsage>("ISTRPT.RPTFuncUsage") { Conditions = "where privilegeid='RPT000062'" };
            var list = CountCatcher.GetEntities().EntityList;
            double count = list.Any() ? list.First().Usage_Counter : 0;
            ViewBag.ClickCount = count;
            return View();
        }

        public JsonResult GetUserInfoByUserID(string userid)
        {
            try
            {
                string sql = string.Format(@"select user.fullname,code.description   from  mmview.fvuser user , mmview.frcode code where
user.Department = code.code_id
and code.category_id = 'Department' and user.user_id='{0}'", userid);
                DB2Helper db2 = new DB2Helper();
                db2.GetSomeData(sql);
                if (db2.dt.DefaultView.Count > 0)
                {
                    return Json(new { success = true, UserName = db2.dt.DefaultView[0][0].ToString(), Department = db2.dt.DefaultView[0][1].ToString() }, JsonRequestBehavior.DenyGet);
                }
                else
                {
                    return Json(new { success = false, msg = string.Format("数据库中没有UserID:{0}", userid) }, JsonRequestBehavior.DenyGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { success=false,msg=ex.Message});
            }
        }

        public JsonResult GetTableEntities(string userid)
        {
            try
            {
                var model = new ReqRpt062MainTableBuilder(userid);
                return Json(new {success=true,model.LotEntities,model.CastEntities },JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success=false,msg=ex.Message});
            }
        }
    }
}