﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AMS.CIM.Caojin.RPTLibrary.Models;

namespace AMS.CIM.Caojin.RPTWebApp.Models
{
    public class ReqRpt213DataBuilder
    {
        public ReqRpt213DataBuilder()
        {
        }

        public string LotStr { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public List<ReqRpt213TableRowEntity> RowEntities { get; set; } = new List<ReqRpt213TableRowEntity>();

        DB2DataCatcher<EDAFuranceOPHSModel> HisCatcher { get; set; } = new DB2DataCatcher<EDAFuranceOPHSModel>("ISTRPT.EDA_Furance_OPHS");

        public void GetTableDataByManual(string lot,string startDate,string endDate)
        {
            LotStr = lot;
            StartDate = startDate;
            EndDate = endDate;
            var lots = lot.Split(',');
            var lots_single = lots.Where(w => !w.Contains("*"));
            var lots_like = lots.Except(lots_single).Select(s => s.Replace('*', '%'));
            string condition = string.Format("where  claim_time between '{0}' and '{1} 23:59:59' and ctrl_job !='' ", startDate, endDate);
            HisCatcher.Conditions = string.Format("{0} and ( lot_id in ('{1}') or lot_id like '{2}' )", condition, string.Join("','", lots_single), string.Join("' or lot_id like '", lots_like));
            var list = HisCatcher.GetEntities().EntityList;
            if (!list.Any()) throw new Exception("没有数据");
            var group = list.GroupBy(g =>  new { g.Lot_ID, g.Ctrl_Job } );
            foreach (var g in group)
            {
                var opeStart = g.Where(w => w.Ope_Category == "OperationStart").FirstOrDefault();
                if (opeStart is null) continue;
                var opeEnd = g.Where(w => w.Ope_Category == "OperationComplete" || w.Ope_Category == "ForceComp").FirstOrDefault();
                var entity = new ReqRpt213TableRowEntity()
                {
                    LotID = g.Key.Lot_ID,
                    FoupID = opeStart.Cast_ID,
                    Qty = opeStart.Cur_Wafer_Qty,
                    OperID = opeStart.ModulePD_ID,
                    EqpID = opeStart.Eqp_ID,
                    Position = opeStart.Pos,
                    OperName = opeStart.PD_Name,
                    OperNo = opeStart.Ope_NO,
                    OpeStartTime = opeStart.Claim_Time.ToString("yyyy-MM-dd HH:mm:ss"),
                    OpeCompleteTime = (opeEnd is null) ? "" : opeEnd.Claim_Time.ToString("yyyy-MM-dd HH:mm:ss"),
                    RecipeID = opeStart.Recipe_ID,
                    RouteID = opeStart.MainPD_ID,
                    RunHrs = (opeEnd is null) ? "On Running" : (opeEnd.Claim_Time - opeStart.Claim_Time).TotalHours.ToString("0.00"),
                    UserDept=opeStart.Dept,
                    UserFullName=opeStart.User_Full_Name,
                    UserID=opeStart.Claim_User_ID,
                    Prod=opeStart.ProdSpec_ID,
                };
                RowEntities.Add(entity);
            }
        }
    }
}