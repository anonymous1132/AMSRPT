using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AMS.CIM.Caojin.RPTWebApp.Models;

namespace AMS.CIM.Caojin.RPTWebApp.Controllers
{
    public class ReqRpt064Controller : Controller
    {
        // GET: ReqRpt064
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetMainTable(string startDate, string endDate)
        {
            try
            {
                var builder = new ReqRpt064SpcMainTableDataBuilder(startDate, endDate);
                var response = new { success = true, tableEntities = builder.TableEntities };
                return Json(response);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, msg = ex.Message });
            }
        }

        public JsonResult GetDetail(string gno, string cno, string ctype, string startDate, string endDate)
        {
            try
            {
                var builder = new ReqRpt064DetailDataBuilder(gno, cno, ctype, startDate, endDate);
                var ProcessModel = builder.DetailModel.ProcessModel;
                var StaticModel = builder.DetailModel.StaticModel;
                var response = new
                {
                    success = true,
                    detail = new
                    {
                        ProcessModel = new
                        {
                            Lcl = ProcessModel.Lcl.ToString(),
                            Lsl = ProcessModel.Lsl.ToString(),
                            Mean = ProcessModel.Mean.ToString(),
                            Target = ProcessModel.Target.ToString(),
                            Sigma = ProcessModel.Sigma.ToString(),
                            Ucl = ProcessModel.Ucl.ToString(),
                            Usl = ProcessModel.Usl.ToString(),
                            Ca = ProcessModel.Ca.ToString(),
                            Cp = ProcessModel.Cp.ToString(),
                            Cpk = ProcessModel.Cpk.ToString(),
                            Cpkv = ProcessModel.Cpkv.ToString()
                        },
                        StaticModel = new
                        {
                            Lcl = StaticModel.Lcl.ToString(),
                            Lsl = StaticModel.Lsl.ToString(),
                            Mean = StaticModel.Mean.ToString(),
                            Target = StaticModel.Target.ToString(),
                            Sigma = StaticModel.Sigma.ToString(),
                            Ucl = StaticModel.Ucl.ToString(),
                            Usl = StaticModel.Usl.ToString(),
                            Ca = StaticModel.Ca.ToString(),
                            Cp = StaticModel.Cp.ToString(),
                            Cpk = StaticModel.Cpk.ToString(),
                            Cpkv = StaticModel.Cpkv.ToString()

                        },
                        builder.DetailModel.SetModel,
                        builder.DetailModel.TimeList,
                        builder.DetailModel.ValueList
                    }
                };
                return Json(response);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, msg = ex.Message });
            }
        }
    }
}