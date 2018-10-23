using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AMS.CIM.Caojin.RPTWebApp.Models
{
    public class ReqRpt018GroupModel
    {
       public List<ReqRpt018EqpStatusEntity> ReqRpt018EqpStatusEntities = new List<ReqRpt018EqpStatusEntity>();
       public List<ReqRpt018Model> ReqRpt018Models = new List<ReqRpt018Model>();


        public void GetData()
        {
            GetStatusList();
            GetModelList();
        }

        private void GetStatusList()
        {
          var HistoryEqpList=  ShareDataEntity.GetSingleEntity().FHESCHSCatcher.GetEntities().EntityList;
          var CurEqpList = ShareDataEntity.GetSingleEntity().FREQPCatcher.GetEntities().EntityList;
          var list = HistoryEqpList.Where(w=>!string.IsNullOrEmpty(w.EQP_ID)).GroupJoin(CurEqpList, h => h.EQP_ID, c => c.EQP_ID, (h, c) => new { h,c}).Select(o=>new ReqRpt018EqpStatusEntity
            {
                EQP_ID=o.h.EQP_ID,
                EQP_Type=o.c.FirstOrDefault().Eqp_Type,
                E10_State=o.h.E10_State,
                Eqp_State=o.h.Eqp_State,
                Start_Time=o.h.Start_Time,
                End_Time=o.h.End_Time,
                Owner_ID=o.c.FirstOrDefault().Owner_ID
            }
            );

            var list2 = CurEqpList.Select(p => new ReqRpt018EqpStatusEntity
            {
                EQP_ID=p.EQP_ID,
                EQP_Type=p.Eqp_Type,
                E10_State=p.E10_State,
                Eqp_State=p.Cur_State_ID,
                Start_Time=p.State_History_Time,
                End_Time=DateTime.Now,
                Owner_ID=p.Owner_ID
            });

            ReqRpt018EqpStatusEntities = list.Union(list2).ToList();
        }

        private void GetModelList()
        {
            DateTime firstTime = ReqRpt018EqpStatusEntities.Min(p => p.Start_Time);
            if (firstTime.TimeOfDay < ShareDataEntity.GetSingleEntity().SplitTimeOfDay)
            {
                firstTime = firstTime.Date.AddDays(-1) + ShareDataEntity.GetSingleEntity().SplitTimeOfDay;
            }
            else
            {
                firstTime = firstTime.Date + ShareDataEntity.GetSingleEntity().SplitTimeOfDay;
            }

            DateTime lastTime = DateTime.Now;
            if (lastTime.TimeOfDay < ShareDataEntity.GetSingleEntity().SplitTimeOfDay)
            {
                lastTime = lastTime.Date + ShareDataEntity.GetSingleEntity().SplitTimeOfDay;
            }
            else
            {
                lastTime = lastTime.Date.AddDays(1) + ShareDataEntity.GetSingleEntity().SplitTimeOfDay;
            }
            int CountDay = (lastTime - firstTime).Days;
            for (int i = 0; i < CountDay; i++)
            {
                var list = ReqRpt018EqpStatusEntities.Where(p=>!(p.Start_Time>firstTime.AddDays(i+1)||p.End_Time<=firstTime.AddDays(i)));

                var list2 = list.Select(s => new { s.EQP_ID, s.E10_State, DeltaTime = ((s.End_Time < firstTime.AddDays(i + 1) ? s.End_Time : firstTime.AddDays(i + 1)) - (s.Start_Time > firstTime.AddDays(i) ? s.Start_Time : firstTime.AddDays(i))).TotalMinutes }).GroupBy(a => new { a.EQP_ID, a.E10_State }).Select(g => new { Data = g.Key, AllSpan = g.Sum(item => item.DeltaTime) });
                var eqps = list2.Select(s => s.Data.EQP_ID);

                foreach (string str in eqps)
                {
                    ReqRpt018Model model = new ReqRpt018Model();
                    model.EqpID = str;
                    model.PRDTimeSpan = list2.Any(p => p.Data.EQP_ID == str && p.Data.E10_State == "PRD")? list2.Where(p => p.Data.EQP_ID == str && p.Data.E10_State == "PRD").FirstOrDefault().AllSpan:0;
                    model.SBYTimeSpan = list2.Any(p => p.Data.EQP_ID == str && p.Data.E10_State == "SBY") ? list2.Where(p => p.Data.EQP_ID == str && p.Data.E10_State == "SBY").FirstOrDefault().AllSpan:0;
                    model.NSTTimeSpan= list2.Any(p => p.Data.EQP_ID == str && p.Data.E10_State == "NST") ? list2.Where(p => p.Data.EQP_ID == str && p.Data.E10_State == "NST").FirstOrDefault().AllSpan:0;
                    model.SDTTimeSpan= list2.Any(p => p.Data.EQP_ID == str && p.Data.E10_State == "SDT") ? list2.Where(p => p.Data.EQP_ID == str && p.Data.E10_State == "SDT").FirstOrDefault().AllSpan:0;
                    model.ENGTimeSpan = list2.Any(p => p.Data.EQP_ID == str && p.Data.E10_State == "ENG") ? list2.Where(p => p.Data.EQP_ID == str && p.Data.E10_State == "ENG").FirstOrDefault().AllSpan:0;
                    model.UDTTimeSpan = list2.Any(p => p.Data.EQP_ID == str && p.Data.E10_State == "UDT") ? list2.Where(p => p.Data.EQP_ID == str && p.Data.E10_State == "UDT").FirstOrDefault().AllSpan:0;
                    model.SomeDay = firstTime.AddDays(i);
                    ReqRpt018Models.Add(model);
                }
                
            }

        }



    }
}