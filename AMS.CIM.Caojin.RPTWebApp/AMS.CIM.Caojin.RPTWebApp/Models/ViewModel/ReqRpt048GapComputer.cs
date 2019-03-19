using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AMS.CIM.Caojin.RPTWebApp.Models
{
    public class ReqRpt048GapComputer
    {
        public List<ReqRpt048GapComputerEntity> GapComputerEntities { get; set; } = new List<ReqRpt048GapComputerEntity>();

        private List<ReqRpt048GapSplitTimeEntity> SplitedGapEntities { get; set; } = new List<ReqRpt048GapSplitTimeEntity>();

        public List<ReqRpt048Stage2GapModel> GetResault()
        {
            var list = new List<ReqRpt048Stage2GapModel>();
            if (!GapComputerEntities.Any()) return list;
            GetSplitGapEntities();
            foreach (var split in SplitedGapEntities)
            {
                split.Departments = GapComputerEntities.Where(w => w.StartTime <= split.StartTime && w.EndTime >= split.EndTime).Select(s => GetDeptCodeByReasonCode(s.ReasonCode)).ToList();
                foreach (var de in split.Departments)
                {
                    //更新list
                    if (string.IsNullOrEmpty(de)) continue;
                    var l = list.Where(w => w.Department == de);
                    if (l.Any()) l.First().HoldGap += split.UnitDuration;
                    else list.Add(new ReqRpt048Stage2GapModel() { Department=de,HoldGap=split.UnitDuration});
                }
            }
            return list;
        }

        private void GetSplitGapEntities()
        {

            var timespans = GapComputerEntities.Select(s => s.StartTime).Union(GapComputerEntities.Select(s => s.EndTime)).Distinct().OrderBy(o=>o).ToList();
            for (var i = 0; i < timespans.Count() - 1; i++)
            {
                SplitedGapEntities.Add(new ReqRpt048GapSplitTimeEntity() { StartTime=timespans[i],EndTime=timespans[i+1]});
            }
        }

        private string GetDeptCodeByReasonCode(string ReasonCode)
        {
            if (ReasonCode.Length == 5) { return ReasonCode.Substring(0, 1); }
            else { return ""; }
        }
    }
}