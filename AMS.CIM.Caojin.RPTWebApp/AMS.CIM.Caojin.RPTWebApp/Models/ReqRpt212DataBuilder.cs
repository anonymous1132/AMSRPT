using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AMS.CIM.Caojin.RPTLibrary.Models;

namespace AMS.CIM.Caojin.RPTWebApp.Models
{
    public class ReqRpt212DataBuilder
    {
        public ReqRpt212DataBuilder()
        {
           
        }

        private string From { get; set; }

        private string To { get; set; }

        public List<ReqRpt212RowEntity> RowEntities { get; set; } = new List<ReqRpt212RowEntity>();

        private DB2DataCatcher<EDA_Compare_Process_ToolModel> EdaCatcher { get; set; }

        /// <summary>
        /// 获取TableData
        /// </summary>
        /// <param name="from">查询起始日期，eg：2019-07-31</param>
        /// <param name="to">查询结束日期，eg:2019-08-01</param>
        public void GetTableData(string from, string to)
        {
            From = from;
            To = to;
            #region sql
            string sql = string.Format(@"with t1 as(
select
lot_id,
wafer_id,
mainpd_id,
ope_no,
eqp_id,
procrsc_id,
ctrljob_id
from
MMVIEW.FHWCPHS
where proc_time between '{0} 00:00:00' and '{1} 23:59:59'
group by
lot_id,
wafer_id,
mainpd_id,
ope_no,
eqp_id,
procrsc_id,
ctrljob_id
)
select t1.*,h.pd_name,h.claim_time,h.prodspec_id,module.pd_id as modulepd_id from t1 left join mmview.fhopehs h
on t1.lot_id=h.lot_id
and t1.ctrljob_id=h.ctrl_job
and h.ope_category='OperationStart'
left join
(
select pf.mainpd_id,pf.pd_level,pd.module_no,pd.pd_id from mmview.frpf pf
inner join mmview.frpf_pdlist pd
on pf.d_thesystemkey=pd.d_thesystemkey
and pf.state=1
) module
on module.mainpd_id=t1.mainpd_id
and module.module_no=substr(t1.ope_no,1,locate('.',t1.ope_no,1)-1)", from,to);
            #endregion 
            EdaCatcher = new DB2DataCatcher<EDA_Compare_Process_ToolModel>("",sql);
            var list = EdaCatcher.GetEntities().EntityList;
            var gp = list.GroupBy(g => new { g.Lot_ID, g.Claim_Time });
            foreach (var g in gp)
            {
                var entity = new ReqRpt212RowEntity()
                {
                    EQP = g.First().EQP_ID,
                    LotID = g.Key.Lot_ID,
                    OpeName=g.First().PD_Name,
                    OpeNo=g.First().Ope_NO,
                    RouteID=g.First().MainPD_ID,
                    OperID=g.First().ModulePD_ID,
                    ProdID=g.First().ProdSpec_ID,
                    OperTime=g.Key.Claim_Time.ToString("yyyy-MM-dd HH:mm:ss")
                };
                var wafers=  g.Select(s =>new {wafer= s.Wafer_ID.Split('.').Last(),s.Procrsc_ID });
                for (int i = 1; i < 26; i++)
                {
                    string str = i < 10 ? '0' + i.ToString() : i.ToString();
                    var wafer = wafers.Where(w => w.wafer == str);
                    entity.ChamberArray[i - 1] = wafer.Count() > 0 ? wafer.First().Procrsc_ID : "";
                }
                RowEntities.Add(entity);
            }
        }
    }
}