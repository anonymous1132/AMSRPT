using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AMS.CIM.Caojin.RPTWebApp.Models
{
    public class ReqRpt029TableEntity
    {
        public string LotID { get; set; }
        public string FoupID { get; set; }
        public string Location { get; set; }
        public string Status { get; set; }
        public double Qtime { get; set; }
      //  public double StayHr { get { return (DateTime.Now - CurrentStepInTime).TotalHours; } }
        public string Department { get; set; }
        public string OpeNo { get; set; }
        public string Step { get; set; }
        public int Priority { get; set; }
        public int Qty { get; set; }
        public string EqpType { get; set; }
        public string LotHoldState { get; set; }
        public string LotProcessState { get; set; }
        public string HoldCode { get; set; }
        public string HoldComment { get; set; }
        public string ToDepartment { get; set; }
        public string ToOpeNo { get; set; }
        public string ToStep { get; set; }
        public string ToEqpType { get; set; }
        public string LotState { get { return string.Format("{0}/{1}", LotHoldState, LotProcessState); } }
        public double RemainQt { get { return Qtime - (DateTime.Now - FirstStepInTime).TotalMinutes; } }
        public double RemainCt { get; set; }
        public DateTime FirstStepInTime { get; set; }
      //  public DateTime CurrentStepInTime { get; set; }
        public double FlowFactor { get { return RemainQt / RemainCt; } }
        public string StrFlowFactor { get { return string.Format("{0}/{1}",RemainQt.ToString("0.0"),RemainCt.ToString("0.0")); } }

    }
}