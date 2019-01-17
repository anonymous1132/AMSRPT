using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AMS.CIM.Caojin.RPTWebApp.Models;
using Caojin.Common;

namespace AMS.CIM.Caojin.RPTWebApp.Controllers
{
    public class ReqRpt036Controller : Controller
    {
        // GET: ReqRpt036
        public ActionResult Index()
        {
            return View(new ReqRpt020MainViewModel());
        }

        public JsonResult GetTableView(ReqRpt036PostModel postModel)
        {
            //try
            //{
            var viewModel = new ReqRpt036TableViewModel(postModel);
          //  Dictionary<string, string> MapStateColor = new Dictionary<string, string> { { "PRD", "white" },{ "SBY","yellow"},{ "ENG","blue"},{"SDT","pink" },{ "UDT","red"},{ "NST","gray"}};
            var StartTime = TimeHelper.GetTimeStamp(viewModel.GetStartTime);
            var EndTime = TimeHelper.GetTimeStamp(viewModel.GetEndTime);
            var response = new
            {
                TotalSeconds = viewModel.TotalSecondDuration,
                Conditions = viewModel.QueryConditions,
                StartTime,
                EndTime,
                Rows = viewModel.RowEntities.Select(s => new
                {
                    s.EqpID,
                    s.CurState,
                    //CurColor=MapStateColor.Where(w=>w.Key==s.CurState).Select(d=>d.Value).FirstOrDefault(),
                    detailState = s.Dic_DetailState,
                    s.PR,
                    s.EN,
                    s.SB,
                    s.SD,
                    s.NS,
                    s.PM,
                    s.PT,
                    s.UD,
                    s.UP,
                    HistoryStates = s.HistoryEntities.Select(ss => new
                    {
                        StartTime = TimeHelper.GetTimeStamp(ss.StartTime),
                        EndTime = TimeHelper.GetTimeStamp(ss.EndTime > viewModel.GetEndTime ? viewModel.GetEndTime : ss.EndTime),
                        ss.E10State,
                        ss.EqpState,
                        ss.DurationSecond,
                        ss.Claim_Memo,
                        ss.Claim_User_ID
                    })
                }).OrderBy(o=>o.EqpID)
            };
            return Json(response, JsonRequestBehavior.DenyGet);
            //}
            //catch (Exception) { return null; }

        }
    }
}