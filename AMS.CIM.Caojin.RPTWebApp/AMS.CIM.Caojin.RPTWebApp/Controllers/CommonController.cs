using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Caojin.Common;
using AMS.CIM.Caojin.RPTLibrary.Models;
using AMS.CIM.Caojin.RPTWebApp.Models;

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
                var querier = new ReqRptCommonProductQuerier(type);
                return Json(new { success = true, prods = querier.Prods });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, msg=ex.Message});
            }
        }

        public JsonResult GetAllLotType()
        {
            try
            {
                string sql = "select lot_type from mmview.frlottype";
                DB2Helper db2 = new DB2Helper();
                db2.GetSomeData(sql);
                var dt = db2.dt;
                List<string> list = new List<string>();
                for (var i = 0; i < dt.Rows.Count; i++)
                {
                    list.Add(dt.DefaultView[i][0].ToString());
                }
                return Json(new { success = true, values = list });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, msg = ex.Message });
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

        /// <summary>
        /// 获取所有部门
        /// </summary>
        /// <param name="type">
        /// 0：所有
        /// 1：不包含IT
        /// 2:不包含IT,WD
        /// </param>
        /// <returns></returns>
        public JsonResult GetAllDepartment(int type)
        {
            try
            {
                //string sql = "select code_id,description from mmview.frcode where category_id='Department'";
                DB2DataCatcher<FRCodeModel> CodeCatcher = new DB2DataCatcher<FRCodeModel>("MMVIEW.FRCODE") { Conditions= "where category_id='Department'" };
                switch (type)
                {
                    case 0:
                        break;
                    case 1:
                        CodeCatcher.Conditions += "and description !='IT'";
                        break;
                    case 2:
                        CodeCatcher.Conditions += "and code_id not in ('IT','WD')";
                        break;
                    default:
                        //default不处理
                        break;
                }
                var list = CodeCatcher.GetEntities().EntityList;
                var response = list.Select(s => new { s.Code_ID, s.Description });
                return Json(response);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, msg = ex.Message });
            }
        }

        public JsonResult GetAllModule(int type)
        {
            try
            {
                //string sql = "select code_id,description from mmview.frcode where category_id='Department'";
                DB2DataCatcher<FRCodeModel> CodeCatcher = new DB2DataCatcher<FRCodeModel>("MMVIEW.FRCODE") { Conditions = "where category_id='Department'" };
                switch (type)
                {
                    case 0:
                        break;
                    case 1:
                        CodeCatcher.Conditions += "and description !='IT'";
                        break;
                    case 2:
                        CodeCatcher.Conditions += "and code_id not in ('IT','WD')";
                        break;
                    default:
                        //default不处理
                        break;
                }
                var list = CodeCatcher.GetEntities().EntityList;
                var modules = list.Select(s => new { s.Code_ID, s.Description });
                return Json(new { success=true,modules});
            }
            catch (Exception ex)
            {
                return Json(new { success = false, msg = ex.Message });
            }
        }
        /// <summary>
        ///验证口令
        /// </summary>
        /// <returns></returns>
        public JsonResult CheckKey(string project,string key)
        {
            ReqKeyOperateModel model = new ReqKeyOperateModel();
            bool res = model.CheckKey(project, key);
            return Json(new { success = res });
        }
        /// <summary>
        /// 修改口令
        /// </summary>
        /// <returns></returns>
        public JsonResult UpdateKey(string project,string newKey,string oldKey)
        {
            ReqKeyOperateModel model = new ReqKeyOperateModel();
            string msg = "";
            bool successed = model.CheckKey(project, oldKey);
            if (successed)
            {
                successed = model.UpdateKey(project, newKey);
                msg = successed ? "口令已更新！" : "口令更新出现未知错误，请联系开发人员！";
            }
            else
            {
                msg = "口令认证失败！";
            }
            return Json(new { success=successed, msg }, JsonRequestBehavior.DenyGet);
        }
    }
}