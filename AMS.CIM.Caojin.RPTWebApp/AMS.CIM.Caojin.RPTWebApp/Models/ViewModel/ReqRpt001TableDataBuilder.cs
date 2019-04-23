using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AMS.CIM.Caojin.RPTLibrary.Models;


namespace AMS.CIM.Caojin.RPTWebApp.Models
{
    public class ReqRpt001TableDataBuilder
    {
        public ReqRpt001TableDataBuilder(ReqRpt001QueryTablePostModel postModel)
        {
            CheckDate(postModel);
            Initialize();
        }

        string from;
        string to;
        string type;
        List<string> prodList;
        DateTime fromDate;
        DateTime toDate;
        readonly int maxItem = 100;

        public List<string> Items { get; set; } = new List<string>();

        public List<ReqRpt001ProductEntity> ProductEntities { get; set; } = new List<ReqRpt001ProductEntity>();

        public string Title { get; set; }

        public bool ShowTarget { get; set; } = false;

        DB2DataCatcher<FHOPEHS_STB> StbCatcher { get; set; } = new DB2DataCatcher<FHOPEHS_STB>("MMVIEW.FHOPEHS");

        DB2DataCatcher<RPT_WaferPlan> PlanCatcher { get; set; } = new DB2DataCatcher<RPT_WaferPlan>("ISTRPT.RPT_WaferPlan");

        void Initialize()
        {
            string prodStr = string.Join("','", prodList);

            StbCatcher.Conditions = string.Format("where ope_category='STB' and prodspec_id in ('{0}')",prodStr);

            PlanCatcher.Conditions = string.Format("where prodspec_id in ('{0}') and plan_date between '{1}' and '{2}' and plan_start_pcs>0",prodStr,from,to);

            StbCatcher.GetEntities();
            PlanCatcher.GetEntities();

            FillItems();

            HandleData();
        }

        void CheckDate(ReqRpt001QueryTablePostModel postModel)
        {
            prodList = postModel.ProdList;
            type = postModel.DateType;
            if (postModel.DateType == "month")
            {
                from = postModel.DateFromValue + "-01";
                fromDate = DateTime.Parse(from);
                var rawList = postModel.DateToValue.Split('-');
                int days = DateTime.DaysInMonth(Convert.ToInt16(rawList[0]), Convert.ToInt16(rawList[1]));
                to = postModel.DateToValue +"-"+ (days>10?days.ToString():"0"+days);
                toDate = DateTime.Parse(to);
            }
            else
            {
                from = postModel.DateFromValue.Split('-')[0] + "-01-01";
                fromDate = DateTime.Parse(from);
                to = postModel.DateToValue.Split('-')[0] + "-12-31";
                toDate = DateTime.Parse(to);
            }

            if (fromDate > toDate) throw new Exception("开始日期不能大于结束日期");
        }

        void FillItems()
        {
            var idate = fromDate;
            var i = maxItem;
            if (type == "month")
            {
                while (toDate >= idate || i<0)
                {
                    Items.Add(idate.ToString("yyyy-MM-dd"));
                    idate = idate.AddDays(1);
                    i--;
                }
                idate = idate.AddDays(-1);
                if (fromDate.Year == idate.Year && fromDate.Month == idate.Month) Title = fromDate.ToString("yyyy年MM月");
                else Title = string.Format("{0}~{1}",fromDate.ToString("yyyy年MM月"),idate.ToString("yyyy年MM月"));
            }
            else
            {
                while (toDate > idate || i<0)
                {
                    Items.Add(idate.ToString("yyyy-MM"));
                    idate = idate.AddMonths(1);
                    i--;
                }
                idate = idate.AddMonths(-1);
                if (fromDate.Year == idate.Year) Title = fromDate.ToString("yyyy年");
                else Title = string.Format("{0}~{1}", fromDate.ToString("yyyy年"), idate.ToString("yyyy年"));
            }
            if (Items.Contains(DateTime.Now.ToString("yyyy-MM-dd"))) { ShowTarget = true; }
        }

        void HandleData()
        {
            var stbs = StbCatcher.entities.EntityList.Select(s=>new {s.Lot_ID,s.ProdSpec_ID,Claim_Date= s.Claim_Time.ToString("yyyy-MM-dd"),s.Cur_Wafer_Qty });
            var plans = PlanCatcher.entities.EntityList;
            foreach (string prod in prodList)
            {
                var entity = new ReqRpt001ProductEntity() { ProductID=prod};
                var prod_stb = stbs.Where(w => w.ProdSpec_ID==prod);
                var prod_plan = plans.Where(w=>w.ProdSpec_ID==prod);
                foreach (var item in Items)
                {
                    int act = prod_stb.Where(w => w.Claim_Date.IndexOf(item)==0).Sum(s=>s.Cur_Wafer_Qty);
                    int plan = prod_plan.Where(w => w.Plan_Date.IndexOf(item) == 0).Sum(s=>s.Plan_Start_Pcs);
                    entity.Plans.Add(new ReqRpt001PlanEntity() { Act=act,Plan=plan});
                }
                var plan_total = entity.Plans.Sum(s => s.Plan);
                var act_total = entity.Plans.Sum(s => s.Act);
                if (ShowTarget)
                {
                    // int days = entity.Plans.Where(w => w.Plan > 0).Count();
                    int days = entity.Plans.Count();
                    entity.OriginalTarget = days > 0 ? entity.Plans.Sum(s => s.Plan)/days:0;
                    int leftDay = Items.Where(w => w.CompareTo(DateTime.Now.ToString("yyyy-MM-dd")) >= 0).Count();
                    entity.CurrentTarget =Math.Round (leftDay == 0 || plan_total < act_total ? 0 : (plan_total - act_total )/(leftDay*1.0),2);
                }
                entity.Plans.Add(new ReqRpt001PlanEntity { Act=act_total,Plan=plan_total});
                ProductEntities.Add(entity);
            }

        }
    }
}