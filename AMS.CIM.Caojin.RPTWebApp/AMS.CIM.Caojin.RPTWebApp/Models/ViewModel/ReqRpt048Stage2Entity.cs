using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AMS.CIM.Caojin.RPTWebApp.Models
{
    public class ReqRpt048Stage2Entity
    {
        public string OpeNO { get; set; }

        public string Department { get; set; }

        public string EqpType { get; set; }

        public List<ReqRpt048EqpEntity> EqpList { get; set; } = new List<ReqRpt048EqpEntity>();

        public string ModulePD { get; set; }
        public string OpeName { get; set; }
        public string Recipe { get; set; }

        public double PRTime { get; set; }

        public string PRSecond { get { return (PRTime * 60).ToString("0.0"); } }
        //min
        public double CT { get; set; }

        public string CTSecond { get { return (CT * 60).ToString("0.0"); } }

        public DateTime Plan { get; set; }

        public string strPlan { get { return Plan.ToString("yyyy/MM/dd HH:mm:ss"); } }

        public DateTime? Forecast { get; set; } = null;

        public string strForecast { get { return Forecast == null ? "" : Forecast.Value.ToString("yyyy/MM/dd HH:mm:ss"); } }

        public DateTime? WFIn { get; set; } = null;

        public string strWFIn { get { return WFIn == null ? "" : WFIn.Value.ToString("yyyy/MM/dd HH:mm:ss"); } }

        public DateTime? StepComplete { get; set; } = null;

        public string strStepComplete { get { return StepComplete == null ? "" : StepComplete.Value.ToString("yyyy/MM/dd HH:mm:ss"); } }

        public double? StepGap { get { return (WFIn == null || StepComplete == null) ? null : (double?)(CT - (StepComplete - WFIn).Value.TotalMinutes); } }

        public string strStepGap { get { return StepGap == null ? "" : (StepGap.Value * 60).ToString("0.0"); } }

        public double? Qtime { get; set; }

        public string strQtime { get; set; } = "";

        public string Remark { get; set; }

        public string QtimeType { get; set; }

        public List<ReqRpt048Stage2GapModel> GapModels { get; set; } = new List<ReqRpt048Stage2GapModel>();

        public double? GapWithOutHold { get { return StepGap == null ? null : (double?)(StepGap.Value + GapModels.Sum(s =>s.HoldGap)); } }

    }
}