using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AMS.CIM.Caojin.RPTLibrary.Models;

namespace AMS.CIM.Caojin.RPTWebApp.Models
{
    public class ReqRpt209DataBuilder
    {
        public ReqRpt209DataBuilder()
        {

        }

        public List<FRProdModel> GetProdList()
        {
            string sql = "select prodspec_id, prod_category_id from mmview.frprodspec where prod_category_id in ('Production','Dummy','Process Monitor','Equipment Monitor') order by prodspec_id";
            DB2DataCatcher<FRProdModel> ProdCatcher = new DB2DataCatcher<FRProdModel>("",sql);
            return ProdCatcher.GetEntities().EntityList.ToList();
        }

        public string LotStr { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public List<ReqRpt209RowEntity> RowEntities { get; set; } = new List<ReqRpt209RowEntity>();

        public void GetTableDataByManual(string lot,string startDate,string endDate)
        {
            LotStr = lot;
            StartDate = startDate;
            EndDate = endDate;
            var lots = lot.Split(',');
            var lots_single = lots.Where(w => !w.Contains("*"));
            var lots_like = lots.Except(lots_single).Select(s=>s.Replace('*','%'));
            List<string> ThkHead = new List<string>() { "THK","TK"};
            List<string> CdHead = new List<string>() { "CD"};
            string condition = string.Format("where  (meas_type='Site'or Item_Type = 'Derived') and claim_time between '{0}' and '{1} 23:59:59' ",startDate,endDate);
            condition = string.Format("{0} and ( meas_lot_id in ('{1}') or meas_lot_id like '{2}' )",condition, string.Join("','", lots_single),string.Join("' or meas_lot_id like '",lots_like));
            
            DB2DataCatcher<EDA_Inline_Data> InlineCatcher = new DB2DataCatcher<EDA_Inline_Data>("ISTRPT.EDA_Inline_Data") { Conditions=condition};
            var list = InlineCatcher.GetEntities().EntityList;
            if (!list.Where(w=>w.Item_Type=="Raw").Any()) throw new Exception("没有数据");
            var group = list.GroupBy(g => new { g.Meas_Lot_ID, g.Meas_MainPD_ID, g.Meas_Ope_No, g.Meas_Pass_Count, g.Claim_Time,});
            var site_max = list.Where(w=>!string.IsNullOrEmpty(w.Site_Position)).Max(m =>Convert.ToInt16(m.Site_Position));
            foreach (var gp in group)
            {
                var raw = gp.Where(w => w.Item_Type == "Raw");
                var dr = gp.Where(w => w.Item_Type == "Derived");
                var g = raw.GroupBy(a => new { a.DcItem_Name,a.Wafer_ID });

                foreach (var l in g)
                {
                    var entity = new ReqRpt209RowEntity
                    {
                        FoupID = l.First().Cast_ID,
                        ProdID=l.First().Meas_ProdSpec_ID,
                        EqpID = l.First().Meas_Eqp_ID,
                        EqpType = l.First().Eqp_Type,
                        LC = l.First().Cntl_Lower_Limit,
                        LotID = l.First().Meas_Lot_ID,
                        LS = l.First().Spec_Lower_Limit,
                        MeasItem = l.Key.DcItem_Name,
                        OpeNo = l.First().Meas_Ope_No,
                        RouteID = l.First().Meas_MainPD_ID,
                        OperID = l.First().Meas_PD_ID,
                        Qty = l.First().Cur_Wafer_Qty,
                        RecipeID = l.First().Recipe_ID,
                        SpecItem = l.First().Meas_DcSpec_ID,
                        Target = l.First().DcItem_Target,
                        UC = l.First().Cntl_Upper_Limit,
                        US = l.First().Spec_Upper_Limit,
                        WaferID =l.First().Wafer_ID,
                        OpeTime=l.First().Claim_Time.ToString("yyyy-MM-dd HH:mm:ss")
                    };
                    for (var i = 0; i < site_max; i++)
                    {
                        var site = l.Where(w => w.Site_Position == (i + 1).ToString());
                        if (site.Any())
                        {
                            entity.SiteValue.Add(Math.Round( Convert.ToDouble(site.First().DcItem_Value),3));
                        } else
                        {
                            entity.SiteValue.Add(null);
                        }
                    }
                    entity.WaferMean =Math.Round( entity.SiteValue.Average()??0,3);
                    if (ThkHead.Any(a => entity.MeasItem.ToUpper().IndexOf(a) == 0)|| entity.MeasItem.ToUpper().Contains("$THK"))
                    {
                        //thk
                        var range = dr.Select(s => new { item = s.DcItem_Name.ToUpper(), s.DcItem_Value,s.RangeUC }).Where(w => w.item.Contains("T") && w.item.Contains("K") && w.item.Contains("RANGE"));
                        if (range.Any()) { entity.MeanRange =Math.Round( Convert.ToDouble(range.First().DcItem_Value),3); entity.RangeUC = range.First().RangeUC; }
                        var mean = dr.Select(s => new { item = s.DcItem_Name.ToUpper(), s.DcItem_Value, s.Cntl_Lower_Limit, s.Cntl_Upper_Limit }).Where(w =>  w.item.Contains("MEAN"));
                        if (mean.Any()) { entity.LC = mean.First().Cntl_Lower_Limit;entity.UC = mean.First().Cntl_Upper_Limit; }
                    }
                    else if (CdHead.Any(a => entity.MeasItem.ToUpper().IndexOf(a) == 0)) 
                    {
                        //cd
                        var range = dr.Select(s => new { item = s.DcItem_Name.ToUpper(), s.DcItem_Value,s.RangeUC }).Where(w => w.item.Contains("CD") && w.item.Contains("RANGE"));
                        if (range.Any()) { entity.MeanRange =Math.Round( Convert.ToDouble(range.First().DcItem_Value),3);entity.RangeUC = range.First().RangeUC; }
                     
                    }
                    RowEntities.Add(entity);
                }
             

            }

        }
    }
}