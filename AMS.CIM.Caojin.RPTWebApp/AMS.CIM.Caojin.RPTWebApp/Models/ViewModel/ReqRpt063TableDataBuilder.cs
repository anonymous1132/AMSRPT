using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AMS.CIM.Caojin.RPTLibrary.Models;
namespace AMS.CIM.Caojin.RPTWebApp.Models
{
    public class ReqRpt063TableDataBuilder
    {

        public List<string> Items { get; set; } = new List<string>();

        public List<ReqRpt063ClickCountEntity> ClickCountEntities { get; set; } = new List<ReqRpt063ClickCountEntity>();

        DB2DataCatcher<RPT_Click_Count_History> ClkCatcher { get; set; }

        public void GetDataByYear(List<string> privilegeIdList)
        {
            int year = DateTime.Now.Year;
            string sql = string.Format(@"
select privilegeid,max(usage_counter) usage_counter,'Max' as Date from istrpt.RPT_Click_Count_History where date < '{0}-01' group by  privilegeid
union

select privilegeid,max(usage_counter), Date from 

(
select privilegeid,usage_counter,substr( Date,1,7) date from 

istrpt.RPT_Click_Count_History 
)

where date between '{0}-01' and '{0}-12'
group by privilegeid,date", year);
            ClkCatcher = new DB2DataCatcher<RPT_Click_Count_History>("", sql);
            var list = ClkCatcher.GetEntities().EntityList;
            for (int i = 1; i <= 12; i++)
            {
                string strI = i < 10 ? "0" + i : i.ToString();
                Items.Add(string.Format("{0}-{1}", year, strI));
            }
            foreach (var id in privilegeIdList)
            {
                var entity = new ReqRpt063ClickCountEntity() { PrivilegeID=id};
                var list_id = list.Where(w => w.PrivilegeID == id);
                if (!list_id.Any())
                {
                    for (int i = 1; i <= 12; i++)
                    {
                        entity.ClickCountValues.Add(0);
                    }
                    ClickCountEntities.Add(entity);
                    continue;
                }
                var max = list_id.Where(w => w.Date == "Max");
                var preValue = max.Any() ? max.First().Usage_Counter : 0;
                for (int i = 1; i <= 12; i++)
                {
                    string strMonth =year+"-"+ (i < 10 ? "0" + i : i.ToString());
                    var value_list = list_id.Where(w => w.Date == strMonth);
                    var value = value_list.Any() ? value_list.First().Usage_Counter : preValue;
                    entity.ClickCountValues.Add(value-preValue);
                    preValue = value;
                }
                ClickCountEntities.Add(entity);
            }

        }

        public void GetDataByMonth(List<string> privilegeIdList,int year,int month)
        {
            int days = DateTime.DaysInMonth(year,month);
            string strMonth = month < 10 ? "0" + month : month.ToString();
            string sql = string.Format(@"select privilegeid,max(usage_counter) usage_counter,'Max' as Date from istrpt.RPT_Click_Count_History where date < '{0}-{1}-01' group by  privilegeid
union

select privilegeid,usage_counter, Date from 

istrpt.RPT_Click_Count_History 


where date between '{0}-{1}-01' and '{0}-{1}-31'", year,strMonth);
            ClkCatcher = new DB2DataCatcher<RPT_Click_Count_History>("", sql);
            var list = ClkCatcher.GetEntities().EntityList;
            for (int i = 1; i <= days; i++)
            {
                Items.Add(string.Format("{0}-{1}", month,i));
            }
            foreach (var id in privilegeIdList)
            {
                var entity = new ReqRpt063ClickCountEntity() { PrivilegeID = id };
                var list_id = list.Where(w => w.PrivilegeID == id);
                if (!list_id.Any())
                {
                    for (int i = 1; i <= days; i++)
                    {
                        entity.ClickCountValues.Add(0);
                    }
                    ClickCountEntities.Add(entity);
                    continue;
                }
                var max = list_id.Where(w => w.Date == "Max");
                var preValue = max.Any() ? max.First().Usage_Counter : 0;
                for (int i = 1; i <= days; i++)
                {
                    string strDate = year +"-"+ strMonth+ "-"+(i < 10 ? "0" + i : i.ToString());
                    var value_list = list_id.Where(w => w.Date == strDate);
                    var value = value_list.Any() ? value_list.First().Usage_Counter : preValue;
                    entity.ClickCountValues.Add(value - preValue);
                    preValue = value;
                }
                ClickCountEntities.Add(entity);
            }
        }
    }
}