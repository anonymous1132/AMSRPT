using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AMS.CIM.Caojin.RPTLibrary.Models;
using AMS.CIM.Caojin.RPTWebApp.Models;
using Caojin.Common;

namespace AMS.CIM.Caojin.RPTWebApp.Models
{
    public class ReqRpt047MainTableDataBuilder
    {
        public ReqRpt047MainTableDataBuilder(List<string> Eqps)
        {
            EqpList = Eqps;
            Initialize();
        }

        List<string> EqpList { get; set; }

        public List<ReqRpt047TableRowEntity> RowEntities { get; set; } = new List<ReqRpt047TableRowEntity>();

        DB2DataCatcher<Report47_Proc> ProcCatcher { get; set; } = new DB2DataCatcher<Report47_Proc>("ISTRPT.Report47_Proc");

        DB2DataCatcher<Rpt_Non_Lot_Edc_Plan> EdcCatcher { get; set; } = new DB2DataCatcher<Rpt_Non_Lot_Edc_Plan>("ISTRPT.Rpt_Non_Lot_Edc_Plan");

        void Initialize()
        {
            string strEqps = string.Join("','", EqpList);

            EdcCatcher.Conditions = string.Format("where eqp_id in ('{0}')",strEqps);
            var edcList = EdcCatcher.GetEntities().EntityList;
            if (!edcList.Any()) throw new Exception("DB中没有找到相关EDC Plan");
            ProcCatcher.Conditions = string.Format("where proc_eqp_id in ('{0}')", strEqps);
            var procList= ProcCatcher.GetEntities().EntityList;
            foreach (var eqp in EqpList)
            {
                var row = new ReqRpt047TableRowEntity() { EqpID = eqp };
                var edc_row = edcList.Where(w => w.Eqp_ID == eqp);
                foreach (var edc in edc_row)
                {
                    var entity = new ReqRpt047TableEntity { EdcPlan = edc.Edc_Plan, Period = edc.Period, PeriodType = edc.Period_Type };
                    var proc = procList.Where(w => w.Proc_Eqp_ID == eqp && w.DCItem_Name == edc.Edc_Plan);
                    if (proc.Any())
                    {
                        var proc_first = proc.First();
                        var periodStartTime = entity.GetPeriodStartTime();
                        if (periodStartTime > proc_first.Proc_Time) { entity.TestTime = ""; entity.SpecResult = false; entity.Count = 0; }
                        else
                        {
                            entity.TestTime = proc_first.Proc_Time.ToString("yyyy/MM/dd HH:mm:ss");
                            entity.SpecResult = proc_first.Spec_Result == 0 ? false : true;
                            entity.Count = GetCount(eqp,periodStartTime.ToString("yyyy-MM-dd HH:mm:ss"));
                        }
                    }
                    else
                    {
                        entity.TestTime = "";
                        entity.SpecResult = false;
                        entity.Count = 0;
                    }
                    row.EdcEntities.Add(entity);
                }
                if (row.EdcEntities.Count > 0)
                {
                    RowEntities.Add(row);
                }
            }
        }

        int GetCount(string eqp, string startTime)
        {
            DB2Helper db2 = new DB2Helper();
            db2.GetSomeData(string.Format( "select distinct proc_time from mmview.fhcdatahs_lot where proc_eqp_id='{0}' and proc_time >'{1}'",eqp,startTime));
            return db2.dt.Rows.Count;
        }
    }
}