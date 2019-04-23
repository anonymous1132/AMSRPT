using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AMS.CIM.Caojin.RPTLibrary.Models;

namespace AMS.CIM.Caojin.RPTWebApp.Models
{
    public class ReqRpt011TableDataBuilder
    {
        public ReqRpt011TableDataBuilder(string month,List<string>prods)
        {
            Month = month;
            Prods = prods;
            Initialize();
        }

        string Month { get; set; }
        List<string> Prods { get; set; }

        DB2DataCatcher<Report11_WaferOut_LotDetail> LotCatcher { get; set; } = new DB2DataCatcher<Report11_WaferOut_LotDetail>("ISTRPT.Report11_WaferOut_LotDetail");

        DB2DataCatcher<RPT_WaferPlan_Out> PlanCatcher { get; set; } = new DB2DataCatcher<RPT_WaferPlan_Out>("ISTRPT.RPT_WaferPlan_Out");

        public List<ReqRpt011ProductEntity> ShipEntities { get; set; } = new List<ReqRpt011ProductEntity>();

        public List<ReqRpt011ProductEntity> WipEntities { get; set; } = new List<ReqRpt011ProductEntity>();

        public List<string> Items { get; set; } = new List<string>();
        List<string> _items { get; set; } = new List<string>();

        void Initialize()
        {
            GetDatas();
            FormProducts();
        }

        void GetDatas()
        {
            int year =Convert.ToInt16( Month.Split('-')[0]);
            string strMonth = Month.Split('-')[1];
            int month = Convert.ToInt16(strMonth);
            int days = DateTime.DaysInMonth(year,month);
            LotCatcher.Conditions = string.Format("where Completion_Time between '{0}-01' and '{0}-{1}'",Month,days);
            LotCatcher.GetEntities();
            PlanCatcher.Conditions = string.Format("where Plan_Date like '{0}-%'",Month);
            PlanCatcher.GetEntities();

            //生成Items
            for (int i = 1; i <= days; i++)
            {
                Items.Add(string.Format("{0}/{1}",month,i));
                _items.Add(string.Format("{0}-{1}",Month , FixNumber(i)));
            }
            Items.Add(DateTime.Parse(Month+"-01").ToString("MMM", new System.Globalization.CultureInfo("en-us")));

        }

        string FixNumber(int num)
        {
            return num < 10 ? "0" + num : num.ToString();
        }

        void FormProducts()
        {
            var lot_list = LotCatcher.entities.EntityList;
            var plan_list = PlanCatcher.entities.EntityList;
            foreach (string prod in Prods)
            {
                var lot_prod = lot_list.Where(w=>w.ProdSpec_ID==prod).Select(s=>new {s.Lot_ID,s.Out_Type,s.ProdSpec_ID,s.Qty,Date= s.Completion_Time.ToString("yyyy-MM-dd") });
                var plan_prod = plan_list.Where(w=>w.ProdSpec_ID==prod);
                ReqRpt011ProductEntity shipEntity = new ReqRpt011ProductEntity() { ProductID=prod};
                ReqRpt011ProductEntity wipEntity = new ReqRpt011ProductEntity() { ProductID = prod };
                for (int i = 0; i < _items.Count; i++)
                {
                    var lot_i = lot_prod.Where(w => w.Date == _items[i]);
                    var lot_ship = lot_i.Where(w=>w.Out_Type=="Ship");
                    var lot_wip = lot_i.Where(w=>w.Out_Type=="Wip");
                    var plan_i = plan_prod.Where(w=>w.Plan_Date==_items[i]);
                    var plan_wip = new ReqRpt011PlanEntity()
                    {
                        Actual = lot_wip.Sum(s => s.Qty),
                        Target = plan_i.Sum(s => s.Plan_Wip_Pcs)
                    };
                    var plan_ship = new ReqRpt011PlanEntity
                    {
                        Actual = lot_ship.Sum(s => s.Qty),
                        Target = plan_i.Sum(s => s.Plan_Ship_Pcs)
                    };
                    plan_ship.Lots = plan_ship.Actual > 0 ? lot_ship.Select(s => s.Lot_ID).ToList():new List<string>();
                    plan_wip.Lots = plan_wip.Actual > 0 ? lot_wip.Select(s => s.Lot_ID).ToList() : new List<string>();
                    shipEntity.Plans.Add(plan_ship);
                    wipEntity.Plans.Add(plan_wip);
                }
                shipEntity.Plans.Add(new ReqRpt011PlanEntity() { Actual=shipEntity.Plans.Sum(s=>s.Actual),Target=shipEntity.Plans.Sum(s=>s.Target)});
                wipEntity.Plans.Add(new ReqRpt011PlanEntity() { Actual = wipEntity.Plans.Sum(s => s.Actual), Target = wipEntity.Plans.Sum(s => s.Target) });
                var lot_ship_prod = lot_prod.Where(w => w.Out_Type == "Ship");
                var lot_wip_prod= lot_prod.Where(w => w.Out_Type == "Wip");
                shipEntity.Plans.Last().Lots = lot_ship_prod.Any() ? lot_ship_prod.Select(s => s.Lot_ID).ToList() : new List<string>();
                wipEntity.Plans.Last().Lots = lot_wip_prod.Any() ? lot_wip_prod.Select(s => s.Lot_ID).ToList() : new List<string>();
                ShipEntities.Add(shipEntity);
                WipEntities.Add(wipEntity);
            }
        }
    }
}