using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using AMS.CIM.Caojin.RPTWebApp.Models;
using Caojin.Common;
using Newtonsoft.Json;

namespace AMS.CIM.Caojin.RPTWebApp.Controllers
{
    public class ReqRpt216Controller : Controller
    {
        // GET: ReqRpt213
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult checkrecipeisexist()
        {
            try
            {
                string RecipeName = Request["RecipeName"].ToString();
                //首先检查该RecipeName是否有坐标存在
                string sql = string.Format("select count(1) as num from ISTRPT.RPT_WAT_Recipe_coordinate where RecipeName='{0}'", RecipeName);
                DB2Helper db2 = new DB2Helper();
                db2.GetSomeData(sql);
                DataTable dt = db2.dt;
                if (Convert.ToInt32(dt.Rows[0][0]) > 0)
                {
                    return Json("exist");
                }
                return Json("notexist");
            }
            catch (Exception)
            {
                return Json("error");
            }
        }
        public JsonResult savecoordinate(List<ReqRpt216coordinate> savelist)
        {
            try
            {
                //首先检查该RecipeName是否有坐标存在
                string sql = string.Format("select count(1) as num from ISTRPT.RPT_WAT_Recipe_coordinate where RecipeName='{0}'", savelist.First().RECIPENAME);
                DB2Helper db2 = new DB2Helper();
                db2.GetSomeData(sql);
                DataTable dt = db2.dt;
                if(Convert.ToInt32(dt.Rows[0][0])>0)
                {
                    return Json("exist");
                }


                List<String> list = new List<string>();
                String sql2 = "";
                foreach (ReqRpt216coordinate o in savelist)
                {
                    sql2 = string.Format("insert into ISTRPT.RPT_WAT_Recipe_coordinate(RECIPENAME,SITENAME,COORDINATE,CREATETIME,OWNER) values('{0}','{1}','{2}','{3}','{4}') ;", o.RECIPENAME, o.SITENAME, o.COORDINATEX + ',' + o.COORDINATEY, o.CREATETIME, o.OWNER);
                    list.Add(sql2);
                }
                db2.UpdateBatchCommand(list);
                return Json("success");
            }
            catch (Exception)
            {
                return Json("error");
            }
        }
        //querycoordinate
        public JsonResult querycoordinate()
        {
            string RecipeName = Request["RecipeName"].ToString();
            string sql =string.Format( "select RecipeName,SiteName,Coordinate,CreateTime,Owner from ISTRPT.RPT_WAT_Recipe_coordinate where RecipeName='{0}'", RecipeName);
            DB2Helper db2 = new DB2Helper();
            db2.GetSomeData(sql);
            DataTable dt = db2.dt;
            dt.Columns.Add("COORDINATEX", Type.GetType("System.String"));
            dt.Columns.Add("COORDINATEY", Type.GetType("System.String"));
            if (dt.Rows.Count !=0 )
            {
                JsonSerializerSettings setting = new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                };
                //JavaScriptSerializer serialize = new JavaScriptSerializer();
                string str= JsonConvert.SerializeObject(dt, setting);
                return Json(str, JsonRequestBehavior.AllowGet);
            }
            else
            {
                //return Json("该RecipeName没有Coordinate，请先添加再查询");
                return Json("nodata");
            }

        }

        public JsonResult changecoordinate(List<ReqRpt216coordinate> changelist)
        {
            try
            {
                List<String> list = new List<string>();
                String sql = "";
                foreach (ReqRpt216coordinate o in changelist)
                {
                    if(o.COORDINATE==""|| o.COORDINATE ==null)
                    {
                        sql = string.Format("insert into ISTRPT.RPT_WAT_Recipe_coordinate(RECIPENAME,SITENAME,COORDINATE,CREATETIME,OWNER) values('{0}','{1}','{2}','{3}','{4}') ;", o.RECIPENAME, o.SITENAME, o.COORDINATEX + ',' + o.COORDINATEY, o.CREATETIME, o.OWNER);
                        list.Add(sql);
                    }
                    else
                    {
                        sql = string.Format("update ISTRPT.RPT_WAT_Recipe_coordinate set Coordinate='{0}',CreateTime='{1}',Owner='{2}' where RecipeName='{3}' and SiteName='{4}';", o.COORDINATEX + ',' + o.COORDINATEY, o.CREATETIME, o.OWNER, o.RECIPENAME, o.SITENAME);
                        list.Add(sql);
                    }


                   
                }
                DB2Helper db2 = new DB2Helper();
                db2.UpdateBatchCommand(list);
                return Json("success");
            }
            catch (Exception)
            {
                return Json("error");
            }


        }
        public JsonResult deletecoordinate()
        {
            try
            {
                List<String> list = new List<string>();
                string RECIPENAME = Request["RECIPENAME"].ToString();
                string SITENAME = Request["SITENAME"].ToString();

                string sql = string.Format("delete from  ISTRPT.RPT_WAT_Recipe_coordinate where RECIPENAME='{0}' and SITENAME='{1}';", RECIPENAME, SITENAME);
                list.Add(sql);
                DB2Helper db2 = new DB2Helper();
                db2.UpdateBatchCommand(list);
                return Json("success");
            }
            catch (Exception)
            {
                return Json("error");
            }


        }
    }
}