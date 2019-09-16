using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace AMS.CIM.Caojin.RPTLibrary.Models
{
    public class ReqRpt018GroupModelTest:ReqRpt018GroupModel
    {
        public EQP_UPm_018Entity Entity { get; set; } = new EQP_UPm_018Entity();

        public void TestGetData()
        {
            GetStatusList();
            GetModelList();
            foreach (var model in ReqRpt018Models)
            {
                if (model.EqpID == "MT-TPD-01")
                {
                    Entity = new EQP_UPm_018Entity()
                    {
                        EqpID = model.EqpID,
                        EqpType = ReqRpt018EqpStatusEntities.Where(w => w.EQP_ID == model.EqpID).FirstOrDefault().EQP_Type,
                        Date = model.SomeDay,
                        PRDMin = model.PRDTimeSpan,
                        SBYMin = model.SBYTimeSpan,
                        SDTMin = model.SDTTimeSpan,
                        UDTMin = model.UDTTimeSpan,
                        ENGMin = model.ENGTimeSpan,
                        NSTMin = model.NSTTimeSpan,
                        PRDTestMin = model.PRDTestTimeSpan,
                        PMMin = model.PMTimeSpan
                    };

                }
            }
        }

        private TimeSpan SplitTimeOfDay
        { get { return TimeSpan.Parse("8:00:00"); } }

        private DateTime lastRecordTime = DateTime.Parse("2019-8-12 08:00:00");

        private bool IsFirstRun = false;

        private void GetStatusList()
        {
            ShareDataEntity.GetSingleEntity().FHESCHSCatcher.Conditions = "where End_Time > '" + lastRecordTime.ToString("yyyy-MM-dd-HH.mm.ss.ffffff") + "'";
            ShareDataEntity.GetSingleEntity().FHOPEHSCatcher.Conditions = "where ope_category='OperationComplete' and Claim_Time >'" + lastRecordTime.ToString("yyyy-MM-dd-HH.mm.ss.ffffff") + "'";
            var HistoryEqpList = ShareDataEntity.GetSingleEntity().FHESCHSCatcher.GetEntities().EntityList;
            var CurEqpList = ShareDataEntity.GetSingleEntity().FREQPCatcher.GetEntities().EntityList;
            var list = HistoryEqpList.Where(w => !string.IsNullOrEmpty(w.EQP_ID)).GroupJoin(CurEqpList, h => h.EQP_ID, c => c.EQP_ID, (h, c) => new { h, c }).Select(o => new ReqRpt018EqpStatusEntity
            {
                EQP_ID = o.h.EQP_ID,
                EQP_Type = o.c.FirstOrDefault().Eqp_Type,
                E10_State = o.h.E10_State,
                Eqp_State = o.h.Eqp_State,
                Start_Time = o.h.Start_Time,
                End_Time = o.h.End_Time,
                Owner_ID = o.c.FirstOrDefault().Owner_ID
            }
              );

            var list2 = CurEqpList.Select(p => new ReqRpt018EqpStatusEntity
            {
                EQP_ID = p.EQP_ID,
                EQP_Type = p.Eqp_Type,
                E10_State = p.E10_State,
                Eqp_State = p.Cur_State_ID,
                Start_Time = p.State_History_Time,
                End_Time = DateTime.Now,
                Owner_ID = p.Owner_ID
            });

            ReqRpt018EqpStatusEntities = list.Union(list2).ToList();
        }

        private void GetModelList()
        {
            DateTime firstTime = IsFirstRun ? ReqRpt018EqpStatusEntities.Min(p => p.Start_Time) : lastRecordTime;
            if (firstTime.TimeOfDay < SplitTimeOfDay)
            {
                firstTime = firstTime.Date.AddDays(-1) + SplitTimeOfDay;
            }
            else
            {
                firstTime = firstTime.Date + SplitTimeOfDay;
            }

            DateTime lastTime = firstTime.AddDays(1).AddHours(1);
            if (lastTime.TimeOfDay < SplitTimeOfDay)
            {
                lastTime = lastTime.Date + SplitTimeOfDay;
            }
            else
            {
                lastTime = lastTime.Date.AddDays(1) + SplitTimeOfDay;
            }
            int CountDay = (lastTime - firstTime).Days;
            var OpeList = ShareDataEntity.GetSingleEntity().FHOPEHSCatcher.GetEntities().EntityList;
            for (int i = 0; i < CountDay; i++)
            {
                var list = ReqRpt018EqpStatusEntities.Where(p => !(p.Start_Time > firstTime.AddDays(i + 1) || p.End_Time <= firstTime.AddDays(i)));

                var list2 = list.Select(s => new { s.EQP_ID, s.E10_State, DeltaTime = ((s.End_Time < firstTime.AddDays(i + 1) ? s.End_Time : firstTime.AddDays(i + 1)) - (s.Start_Time > firstTime.AddDays(i) ? s.Start_Time : firstTime.AddDays(i))).TotalMinutes }).GroupBy(a => new { a.EQP_ID, a.E10_State }).Select(g => new { Data = g.Key, AllSpan = g.Sum(item => item.DeltaTime) });
                var list3 = list.Where(w => w.E10_State == "SDT");
                var eqps = list2.Select(s => s.Data.EQP_ID).Distinct();
                var list_ope = OpeList.Where(w => w.Claim_Time > firstTime.AddDays(i) && w.Claim_Time <= firstTime.AddDays(i + 1));
                foreach (string str in eqps)
                {
                    ReqRpt018Model model = new ReqRpt018Model();
                    List<string> pmCodes = new List<string>() { "4100", "4110", "4300", "4310" };
                    model.EqpID = str;
                    model.PRDTimeSpan = list2.Any(p => p.Data.EQP_ID == str && p.Data.E10_State == "PRD") ? list2.Where(p => p.Data.EQP_ID == str && p.Data.E10_State == "PRD").FirstOrDefault().AllSpan : 0;
                    model.SBYTimeSpan = list2.Any(p => p.Data.EQP_ID == str && p.Data.E10_State == "SBY") ? list2.Where(p => p.Data.EQP_ID == str && p.Data.E10_State == "SBY").FirstOrDefault().AllSpan : 0;
                    model.NSTTimeSpan = list2.Any(p => p.Data.EQP_ID == str && p.Data.E10_State == "NST") ? list2.Where(p => p.Data.EQP_ID == str && p.Data.E10_State == "NST").FirstOrDefault().AllSpan : 0;
                    model.SDTTimeSpan = list2.Any(p => p.Data.EQP_ID == str && p.Data.E10_State == "SDT") ? list2.Where(p => p.Data.EQP_ID == str && p.Data.E10_State == "SDT").FirstOrDefault().AllSpan : 0;
                    model.ENGTimeSpan = list2.Any(p => p.Data.EQP_ID == str && p.Data.E10_State == "ENG") ? list2.Where(p => p.Data.EQP_ID == str && p.Data.E10_State == "ENG").FirstOrDefault().AllSpan : 0;
                    model.UDTTimeSpan = list2.Any(p => p.Data.EQP_ID == str && p.Data.E10_State == "UDT") ? list2.Where(p => p.Data.EQP_ID == str && p.Data.E10_State == "UDT").FirstOrDefault().AllSpan : 0;
                    model.SomeDay = firstTime.AddDays(i);
                    model.PRDTestTimeSpan = list3.Any(p => p.EQP_ID == str && p.Eqp_State == "4600") ? list3.Where(p => p.EQP_ID == str && p.Eqp_State == "4600").Select(s => ((s.End_Time < firstTime.AddDays(i + 1) ? s.End_Time : firstTime.AddDays(i + 1)) - (s.Start_Time > firstTime.AddDays(i) ? s.Start_Time : firstTime.AddDays(i))).TotalMinutes).Sum() : 0;
                    model.PMTimeSpan = list3.Any(p => p.EQP_ID == str && pmCodes.Contains(p.Eqp_State)) ? list3.Where(p => p.EQP_ID == str && pmCodes.Contains(p.Eqp_State)).Select(s => ((s.End_Time < firstTime.AddDays(i + 1) ? s.End_Time : firstTime.AddDays(i + 1)) - (s.Start_Time > firstTime.AddDays(i) ? s.Start_Time : firstTime.AddDays(i))).TotalMinutes).Sum() : 0;
                    ReqRpt018Models.Add(model);
                    ReqRpt020Model model020 = new ReqRpt020Model();
                    model020.EqpID = str;
                    model020.PassQty = list_ope.Any(a => a.Eqp_ID == str) ? list_ope.Where(w => w.Eqp_ID == str).Sum(s => s.Cur_Wafer_QTY) : 0;
                    model020.ReworkQty = list_ope.Any(a => a.Eqp_ID == str && a.Ope_Pass_Count > 1) ? list_ope.Where(w => w.Eqp_ID == str && w.Ope_Pass_Count > 1).Sum(s => s.Cur_Wafer_QTY) : 0;
                    model020.EffMove = list_ope.Any(a => a.Eqp_ID == str && a.Ope_Pass_Count == 1) ? list_ope.Where(w => w.Eqp_ID == str && w.Ope_Pass_Count == 1).Sum(s => s.Cur_Wafer_QTY) : 0;
                    model020.SomeDay = firstTime.AddDays(i);
                    ReqRpt020Models.Add(model020);
                }

            }

        }

    }
}
