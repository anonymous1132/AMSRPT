using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Caojin.Common;
using AMS.CIM.Caojin.RPTLibrary.Models;

namespace AMS.CIM.Caojin.RPTWebApp.Models
{
    public class ReqRpt001PlanSetHandler
    {
        DB2Helper DB2 = new DB2Helper();

        readonly string tableName = "istrpt.rpt_waferplan";

        DB2DataCatcher<RPT_WaferPlan> PlanCatcher { get; set; } = new DB2DataCatcher<RPT_WaferPlan>("istrpt.rpt_waferplan");
        
        /// <summary>
        /// 设置某月每天WaferStart都是同一数据
        /// </summary>
        /// <param name="prod">产品</param>
        /// <param name="year">年</param>
        /// <param name="month">月</param>
        /// <param name="value">计划值</param>
        public void SetValueByMonth(string prod, int year,int month,int value)
        {
            string strMonth = month < 10 ? "0" + month : month.ToString();
            PlanCatcher.Conditions =string.Format("where prodspec_id ='{0}' and plan_date like '{1}-{2}%'", prod, year,strMonth );
            var list = PlanCatcher.GetEntities().EntityList;
            int days = DateTime.DaysInMonth(year, month) ;
            List<string> sqls = new List<string>();
            for (int i = 1; i <= days; i++)
            {
                string d = i<10?"0"+i:i.ToString();
                string date = year + "-" + strMonth + "-" +d;
                if (!list.Any(a => a.Plan_Date == date))
                {
                    sqls.Add(string.Format("insert into {0} values ('{1}','{2}',0,0)",tableName,prod,date));
                }
                sqls.Add(string.Format( "update {0} set plan_start_pcs ={1} where prodspec_id='{2}' and plan_date like '{3}-{4}%'",tableName,value,prod,year,strMonth));
            }
            int res= DB2.UpdateBatchCommand(sqls);
        }

        /// <summary>
        /// 设置某一范围内都是同一数据,最大支持100天
        /// </summary>
        /// <param name="prod">产品</param>
        /// <param name="fromDate">yyyy-MM-dd</param>
        /// <param name="toDate">yyyy-MM-dd</param>
        /// <param name="value">pcs</param>
        public void SetValueByDateRange(string prod,string fromDate,string toDate,int value)
        {
            DateTime startDate = DateTime.Parse(fromDate);
            DateTime endDate = DateTime.Parse(toDate);
            if (startDate > endDate) throw new Exception("起始日期大于结束日期");
            PlanCatcher.Conditions = string.Format("where prodspec_id ='{0}' and plan_date between '{1}' and '{2}'", prod,fromDate,toDate);
            var list = PlanCatcher.GetEntities().EntityList;
            List<string> sqls = new List<string>();
            DateTime tempDate = startDate;
            int num = 0;
            while (endDate >= tempDate && num<100)
            {
                string date = tempDate.ToString("yyyy-MM-dd");
                if (!list.Any(a => a.Plan_Date == date))
                {
                    sqls.Add(string.Format("insert into {0} values ('{1}','{2}',{3},0)", tableName, prod, date, value));
                }
                else
                {
                    sqls.Add(string.Format("update {0} set plan_start_pcs ={1} where prodspec_id='{2}' and plan_date = '{3}'", tableName, value, prod, date));
                }
                tempDate= tempDate.AddDays(1);
                num++;
            }
            int res = DB2.UpdateBatchCommand(sqls);

        }


        /// <summary>
        /// 设定某日WaferStart计划值
        /// </summary>
        /// <param name="prod">产品</param>
        /// <param name="date">日期</param>
        /// <param name="value">计划值</param>
        public void SetValueBySingleDay(string prod, string date,int value)
        {
            PlanCatcher.Conditions = string.Format("where prodspec_id ='{0}' and plan_date = '{1}'", prod, date);
            var list = PlanCatcher.GetEntities().EntityList;
            List<string> sqls = new List<string>();
            if (list.Any())
            {
                sqls.Add(string.Format("update {0} set plan_start_pcs ={1} where prodspec_id='{2}' and plan_date='{3}'", tableName, value,prod, date));
            }
            else
            {
                sqls.Add(string.Format("insert into {0} values ('{1}','{2}',{3},0)", tableName, prod, date,value));
            }
            int res = DB2.UpdateBatchCommand(sqls);
        }

        public List<string> SetSingleProdValueByArray(string prod,List<string>dates,List<int>values)
        {
            int c = dates.Count;
            if (c != values.Count) throw new Exception("数据错误");
            List<string> res = new List<string>();
            for (int i = 0; i < c; i++)
            {
                try
                {
                    SetValueBySingleDay(prod, dates[i], values[i]);
                }
                catch (Exception)
                {
                    res.Add(string.Format("prod:{0},date:{1},plan_value:{2}",prod,dates[i],values[i]));
                }
            }
            if (res.Any()) { res.Insert(0, "以下数据错误："); }
            else { res.Add("所有数据请求成功"); }
            return res;
        }

        public List<string> SetProdsValueByArray(List<string> prods, List<string> dates, List<int> values)
        {
            int c = dates.Count;
            if (c != values.Count) throw new Exception("数据错误");
            if(c!=prods.Count) throw new Exception("数据错误");
            List<string> res = new List<string>();
            for (int i = 0; i < c; i++)
            {
                try
                {
                    SetValueBySingleDay(prods[i], dates[i], values[i]);
                }
                catch (Exception)
                {
                    res.Add(string.Format("prod:{0},date:{1},plan_value:{2}", prods[i], dates[i], values[i]));
                }
            }
            if (res.Any()) { res.Insert(0, "以下数据错误："); }
            else { res.Add("所有数据请求成功"); }
            return res;
        }
    }
}