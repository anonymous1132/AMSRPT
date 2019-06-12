using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AMS.CIM.Caojin.RPTLibrary.Models;
using Caojin.Common;

namespace AMS.CIM.Caojin.RPTWebApp.Models
{
    public class ReqRpt002MoveTargetOperator
    {
        public ReqRpt002MoveTargetOperator()
        {

        }

        public List<ReqRpt002MoveTargetEntity> TargetList { get; set; } = new List<ReqRpt002MoveTargetEntity>();

        DB2DataCatcher<Rpt_Move_Target> TargetCatcher { get; set; }

       

        public int Days { get; set; } = 0;

        public string Month { get; set; } = "";
        /// <summary>
        /// 获取各部门某月move target
        /// </summary>
        /// <param name="month">eg:2019-05</param>
        /// <returns></returns>
        public void GetTargetListByMonth(string month)
        {

            int year = Convert.ToInt16(month.Split('-')[0]);
            int mon = Convert.ToInt16(month.Split('-')[1]);
            string strMonth = string.Format("{0}-{1}",year,mon<10?"0"+mon:mon.ToString());
            string sql =string.Format(@"select 
t.Dept_code,plan_date,target_value,c.description department
from istrpt.Rpt_Move_Target t
left join 
mmview.frcode c
on
c.category_id='Department'
and c.code_id=t.dept_code where t.plan_date like '{0}-%'", strMonth);

            Days = DateTime.DaysInMonth(year, mon);
            Month = year.ToString() + "年" + mon.ToString() + "月";
            TargetCatcher = new DB2DataCatcher<Rpt_Move_Target>("",sql);
            var list = TargetCatcher.GetEntities().EntityList;
            if (!list.Any()) return;
            var deptList = list.Select(s => s.Dept_Code).Distinct();
            foreach (var dept in deptList)
            {
                var list_dept = list.Where(w => w.Dept_Code == dept);
                ReqRpt002MoveTargetEntity targetEntity = new ReqRpt002MoveTargetEntity() { DeptCode=dept};
                targetEntity.Department = list_dept.First().Department;
                for (var i = 1; i <= Days; i++)
                {
                    var date = i < 10 ? ("0" + i): i.ToString();
                    var val= list_dept.Where(w => w.Plan_Date == strMonth + "-" + date);
                    targetEntity.TargetList.Add(val.Any()?val.First().Target_Value:0);
                }
                TargetList.Add(targetEntity);
            }
            
        }

        public void SetTargetListByMonth(ReqRpt002SetMoveTargetPostModel postModel)
        {
            string month = postModel.date.ToString("yyyy-MM");
            var dayArray = postModel.dayArray.Select(s => month + "-" + (s < 10 ? "0" + s : s.ToString())).ToList();
            //获取deptList
            DB2DataCatcher<FRCodeModel> codeCatcher = new DB2DataCatcher<FRCodeModel>("MMVIEW.FRCODE") { Conditions="where category_id='Department'"};
            var deptList = codeCatcher.GetEntities().EntityList;
            var deptOpeList = postModel.deptData.Select(s => s.dept).Distinct();
            var deptCodeOpeList = deptList.Where(w => deptOpeList.Contains(w.Description)).Select(s => s.Code_ID);
            //执行删除旧数据的动作
            List<string> sqls = new List<string>();
            string delSql = string.Format("delete from istrpt.RPT_MOVE_TARGET where dept_code in ('{0}') and plan_date in ('{1}')",string.Join("','",deptCodeOpeList),string.Join("','",dayArray));
            sqls.Add(delSql);
            foreach (var dept in deptOpeList)
            {
                var code_list = deptList.Where(w => w.Description == dept);
                if (code_list.Any())
                {
                    var deptData = postModel.deptData.Where(w => w.dept == dept).First();
                    string code = code_list.First().Code_ID;
                    List<string> valueList = new List<string>();
                    for (int i = 0; i < dayArray.Count(); i++)
                    {
                        string value = string.Format("('{0}','{1}',{2})",code,dayArray[i],deptData.targetArray[i]);
                        valueList.Add(value);
                    }
                    string sql =string.Format( "insert into istrpt.rpt_move_target values {0}",string.Join(",",valueList));
                    sqls.Add(sql);
                }
            }
            DB2Helper db2 = new DB2Helper();
            db2.UpdateBatchCommand(sqls);
        }
    }
}