using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AMS.CIM.Caojin.RPTLibrary.Models;

namespace AMS.CIM.Caojin.RPTWebApp.Models
{
    public class ReqRpt002MoveQuerier
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="date">eg:2019-05-08</param>
        public ReqRpt002MoveQuerier(string date)
        {
            Date = DateTime.Parse(date);
            Initialize();
        }

        private DateTime Date { get; set; }

        public List<ReqRpt002MoveEntity> MoveEntities { get; set; } = new List<ReqRpt002MoveEntity>();

        private DB2DataCatcher<RPT_Move_ByDepartmentModel> MoveCatcher { get; set; }

        private DB2DataCatcher<Rpt_Move_Target> TargetCatcher { get; set; }

        private void Initialize()
        {
            string sql = string.Format(@"select lot_id,cur_wafer_qty,prev_pd_id pd_id,prev_pd_name pd_name,c.description department
from mmview.fhopehs hs
left join
istrpt.rpt_prod_flow fl
on hs.pd_id=fl.pd_id
left join mmview.frcode c
on c.category_id='Department'
and c.code_id=fl.department
where ope_category ='OperationComplete'
 and claim_time between '{0} 08:00:00' and '{1} 08:00:00'
 and lot_type='Production'", Date.ToString("yyyy-MM-dd"),Date.AddDays(1).ToString("yyyy-MM-dd"));
            MoveCatcher = new DB2DataCatcher<RPT_Move_ByDepartmentModel>("",sql);
            var list = MoveCatcher.GetEntities().EntityList;
           // if (!list.Any()) return;
            sql = string.Format("select c.description department,m.target_value from    ISTRPT.RPT_Move_Target m left join mmview.frcode c on c.category_id='Department' and c.code_id=m.dept_code where m.plan_date ='{0}'",Date.ToString("yyyy-MM-dd"));
            TargetCatcher = new DB2DataCatcher<Rpt_Move_Target>("",sql);
            var list_target = TargetCatcher.GetEntities().EntityList;
            var list_dept = new ReqRptCommonDeptQuerier().DeptList;
            var wipQuier1 = new ReqRpt002WipQuerier(Date.AddHours(8).ToString("yyyy-MM-dd HH:mm:ss"));
            var wipQuier2 = new ReqRpt002WipQuerier(Date.AddHours(20).ToString("yyyy-MM-dd HH:mm:ss"));
            var wipQuier3 = new ReqRpt002WipQuerier(Date.AddHours(32).ToString("yyyy-MM-dd HH:mm:ss"));
            foreach (var dept in list_dept)
            {
                var list_target_dept = list_target.Where(w => w.Department == dept.Description);
                var move = new ReqRpt002MoveEntity();
                move.Department = dept.Description;
                move.MoveValue = list.Where(w => w.Department == dept.Description).Sum(s => s.Cur_Wafer_Qty);
                move.MoveTarget = list_target_dept.Any() ? list_target_dept.First().Target_Value : 0;
                //判断查询时间是否为今天
                if (DateTime.Now.Date> Date.AddHours(8) && DateTime.Now.Date<Date.AddHours(32))
                {
                    var target= move.MoveTarget * (DateTime.Now - Date.AddHours(8)).TotalHours / 24;
                    move.Percentage = target == 0 ? -1 : move.MoveValue * 1.0 / target;
                }
                else
                {
                    move.Percentage = move.MoveTarget == 0 ? -1 : move.MoveValue*1.0 / move.MoveTarget;
                }
                int wip1 = GetWipOfDepartment(wipQuier1,dept.Description);
                int wip2 = GetWipOfDepartment(wipQuier2, dept.Description);
                int wip3 = GetWipOfDepartment(wipQuier3,dept.Description);
                double wip = (wip1 + 2 * wip2 + wip3) / 4.0;
                move.AvaWip = wip;
                move.TurnRate =wip==0?-1: move.MoveValue / wip;
                MoveEntities.Add(move);
            }

        }

        private int GetWipOfDepartment(ReqRpt002WipQuerier wipQuerier, string dept)
        {
            var wip = wipQuerier.WipEntities.Where(w => w.Department == dept);
            return wip.Any() ? wip.First().Wafers:0;
        }
    }
}