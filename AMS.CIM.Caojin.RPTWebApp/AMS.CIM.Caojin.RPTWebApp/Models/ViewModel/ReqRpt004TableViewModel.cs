using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AMS.CIM.Caojin.RPTLibrary.Models;
using Caojin.Common;
using System.Data;
using System.Collections;

namespace AMS.CIM.Caojin.RPTWebApp.Models
{
    public class ReqRpt004TableViewModel
    {
        public ReqRpt004TableViewModel(string Products,string TargetDate)
        {
            this.Products = Products;
            this.TargetDate = TargetDate;
            Initialize();
        }

        private string Products { get; set; }

        public string TargetDate { get; set; }

        public string FormatTargetDate { get { return string.Join("", TargetDate.Split('-')); } }

        private DateTime DTargetDate { get { return DateTime.Parse(TargetDate); } }
        private DateTime DFromDate { get { return DTargetDate.AddDays(-30); } }
        private string StrStartTime { get { return DFromDate.ToString("yyyy/MM/dd HH:mm:ss"); } }
        private string StrEndTime { get { return DTargetDate.AddHours(32).ToString("yyyy/MM/dd HH:mm:ss"); } }

        private List<string> ProductList { get { return Products.Split(',').ToList(); } }

        public List<ReqRpt004TableGroupEntity> TableRowEntities { get; set; } = new List<ReqRpt004TableGroupEntity>();

        DB2DataCatcher<RPT_Turn_Daily> TurnCatcher = new DB2DataCatcher<RPT_Turn_Daily>("ISTRPT.RPT_Turn_Daily");

        public List<string> DateList { get { return TableRowEntities.FirstOrDefault().TableEntities.Select(s => s.StrDate).ToList(); } }

        public List<string> DailyTotalTurn { get; set; } = new List<string>();

        public List<ReqRpt004ChartDataEntity> ChartList
        {
            get
            {
                var list = new List<ReqRpt004ChartDataEntity>();
                foreach (var item in TableRowEntities)
                {
                    List<ArrayList> slist = new List<ArrayList>();
                    foreach (var i in item.TableEntities)
                    {
                        ArrayList array = new ArrayList();
                        array.Add(TimeHelper.ConvertDateTimeToInt(i.Date.ToLocalTime()));
                        array.Add(Math.Round(i.TurnRate,2));
                        slist.Add(array);
                    }
                    list.Add(new ReqRpt004ChartDataEntity() { label = item.ProductID, data = slist });
                }
                return list;
            }
        }

        public void Initialize()
        {
            string sql = string.Format("select product,count(*) from (select t1.product,pd.pd_type from ( select product,pd_syskey from istrpt.report51_list where product in ('{0}'))t1 left join mmview.frpd pd on t1.pd_syskey=pd.d_thesystemkey where pd_type not in ('Dummy','Measurement')) group by product",string.Join("','",ProductList));
            DB2Helper db2 = new DB2Helper();
            db2.GetSomeData(sql);
            foreach (DataRow dr in db2.dt.Rows)
            {
                TableRowEntities.Add(new ReqRpt004TableGroupEntity() { ProductID=dr[0].ToString(),EffectiveSteps=(int)dr[1]});
            }
            TurnCatcher.Conditions = string.Format("where start_time between '{0}' and '{1}' and Product_ID in ('{2}')",StrStartTime,StrEndTime, string.Join("','", ProductList));
            var list = TurnCatcher.GetEntities().EntityList;
            foreach (var item in TableRowEntities)
            {
                var rawList = list.Where(w=>w.Product_ID==item.ProductID);
                DateTime dt = DFromDate;
                while (dt <= DTargetDate)
                {
                    if (!rawList.Any() || !rawList.Where(w => w.Start_Time.Date == dt).Any())
                    {
                        item.TableEntities.Add(new ReqRpt004TableEntity()
                        {
                            Date = dt,
                            Move = 0,
                            WIP = 0
                        });
                        dt = dt.AddDays(1);
                        continue;
                    }
                    var itemList = rawList.Where(w => w.Start_Time.Date == dt);
                    var dayWIP = itemList.Where(w => w.Start_Time.Hour == 8).Any()? itemList.Where(w => w.Start_Time.Hour == 8).First():new RPT_Turn_Daily();
                    var nightWIP= itemList.Where(w => w.Start_Time.Hour == 20).Any()? itemList.Where(w => w.Start_Time.Hour == 20).First():new RPT_Turn_Daily();
                    var nextWIP = rawList.Where(w => w.Start_Time.Date == dt.AddDays(1) && w.Start_Time.Hour == 8).Any()? rawList.Where(w => w.Start_Time.Date == dt.AddDays(1) && w.Start_Time.Hour == 8).First():new RPT_Turn_Daily();
                    item.TableEntities.Add(new ReqRpt004TableEntity() {
                        Date = dt,
                        Move=itemList.Sum(s=>s.MoveQty),
                        WIP=(dayWIP.WIP+2*nightWIP.WIP+nextWIP.WIP)/4.0
                    });
                    dt = dt.AddDays(1);
                }
            }
            //Set DailyTotalTurn
            int totalMove = 0;
            double totalWip = 0;
            for (int i = 0; i < 31; i++)
            {
                int move = 0;
                double wip = 0;
                foreach (var item in TableRowEntities)
                {
                    move += item.TableEntities[i].Move;
                    wip += item.TableEntities[i].WIP;
                }
                DailyTotalTurn.Add(wip==0?"0.00":(move/wip).ToString("0.00"));
                totalMove += move;
                totalWip += wip;
            }
            DailyTotalTurn.Add(totalWip == 0 ? "0.00" : (totalMove / totalWip).ToString("0.00"));
        }
    }
}