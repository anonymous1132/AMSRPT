using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AMS.CIM.Caojin.RPTLibrary.Models;

namespace AMS.CIM.Caojin.RPTWebApp.Models
{
    public class ReqRpt002WipQuerier
    {
        /// <summary>
        /// 查询某时间点的WIP
        /// </summary>
        /// <param name="datetime">eg:2019-05-10 08:00:00</param>
        public ReqRpt002WipQuerier(string datetime)
        {
            DateTime = datetime;
            DaDateTime= System.DateTime.Parse(DateTime);
            Initialize();
        }

        private string DateTime { get; set; }
        private DateTime DaDateTime { get; set; }

        public List<ReqRpt002WipEntity> WipEntities { get; set; } = new List<ReqRpt002WipEntity>();

        private DB2DataCatcher<RPT__WIP_ByDepartmentModel> WipCatcher { get; set; }

      

        private void Initialize()
        {
            GetDBDatas();
            var list = WipCatcher.entities.EntityList;
            if (!list.Any()) return;
            List<RPT__WIP_ByDepartmentModel> list_nrpt = new List<RPT__WIP_ByDepartmentModel>();
            list_nrpt.Add(list[0]);
            //list去重
            for (int i = 1;i<list.Count();i++)
            {
                if (list[i].Lot_ID == list_nrpt.Last().Lot_ID)
                {
                    list_nrpt.RemoveAt(list_nrpt.Count-1);
                }
                list_nrpt.Add(list[i]);
            }
            var list_outsource = list_nrpt.Where(w => w.Ope_Name == "CL BANK INOUT");
            var list_insource = list_nrpt.Where(w => w.Ope_Name != "CL BANK INOUT");
            var list_bank = list_insource.Where(w =>  w.Bank_ID != "");
            var list_deptlots = list_insource.Where(w => w.Bank_ID == ""&&w.Department!="");
            var list_dept = list_deptlots.Select(s => s.Department).Distinct();
            foreach (var dept in list_dept)
            {
                var list_wip = list_deptlots.Where(w => w.Department == dept);
                var list_hold = list_wip.Where(w => w.Hold_State == "ONHOLD");
                var wip = new ReqRpt002WipEntity
                {
                    Department = dept,
                    HoldWafers = list_hold.Sum(s => s.Cur_Wafer_Qty),
                    HoldLots = list_hold.Count(),
                    Lots = list_wip.Count(),
                    Wafers = list_wip.Sum(s => s.Cur_Wafer_Qty),
                    HoldLotOverTime = list_wip.Where(w => (DaDateTime - w.Hold_Time).TotalDays > 1).Count()
                };
                WipEntities.Add(wip);
            }

            WipEntities.Add(new ReqRpt002WipEntity() {
                Department = "OutSource",
                HoldWafers = list_outsource.Where(w => w.Hold_State == "ONHOLD").Sum(s => s.Cur_Wafer_Qty),
                Lots = list_outsource.Count(),
                Wafers = list_outsource.Sum(s => s.Cur_Wafer_Qty),
                HoldLots = list_outsource.Where(w => w.Hold_State == "ONHOLD").Count(),
                HoldLotOverTime =list_outsource.Where(w=>(DaDateTime-w.Hold_Time).TotalDays>1).Count()
            });

            WipEntities.Add(new ReqRpt002WipEntity() {
                Department = "Bank",
                HoldWafers = list_bank.Where(w => w.Hold_State == "ONHOLD").Sum(s => s.Cur_Wafer_Qty),
                Lots = list_bank.Count(),
                Wafers = list_bank.Sum(s => s.Cur_Wafer_Qty),
                HoldLots = list_bank.Where(w => w.Hold_State == "ONHOLD").Count(),
                HoldLotOverTime=list_bank.Where(w => (DaDateTime - w.Hold_Time).TotalDays > 1).Count()
            });
        }


        private void GetDBDatas()
        {
            string sql = string.Format(@"with t1 as(
select lot_id,pd_id,ope_category,bank_id,claim_time,hold_state,hold_time,cur_wafer_qty,pd_name from mmview.fhopehs where lot_id in
(
select lot_id from mmview.frlot where created_time <'{0}' and (lot_state='ACTIVE' or completion_time >'{0}')
)
and claim_time <'{0}'
and lot_type='Production'
and pd_id!=''
),t2 as (
select lot_id,max(claim_time) claim_time from t1 group by lot_id),
t3 as (
select lot_id,max(claim_time) claim_time from t1 where hold_state!='ONHOLD' group by lot_id
),
t4 as (
select lot_id,min(claim_time) hold_time from
(
select t1.lot_id,t1.claim_time from t1,t3 where t1.lot_id=t3.lot_id and t1.claim_time>t3.claim_Time
) group by lot_id)
select t2.lot_id,t2.claim_time,t1.pd_id,t1.ope_category,t1.hold_state ,t4.hold_time,t1.cur_wafer_qty,t1.bank_id,c.description department,t1.pd_name ope_name
from t2
left join t1 
on t1.lot_id=t2.lot_id
and t1.claim_time=t2.claim_time
left join t4
on t2.lot_id=t4.lot_id
left join mmview.frpd pd
on t1.pd_id=pd.pd_id
and pd.pd_level='Operation'
left join mmview.frcode c
on c.category_id='Department'
and c.code_id=pd.department
order by lot_id", DateTime);
            WipCatcher = new DB2DataCatcher<RPT__WIP_ByDepartmentModel>("", sql);
            WipCatcher.GetEntities();
        }
    }
}