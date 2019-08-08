using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AMS.CIM.Caojin.RPTLibrary.Models;

namespace AMS.CIM.Caojin.RPTWebApp.Models
{
    public class ReqRpt206DataBuilder
    {
        public string LotStr { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public List<ReqRpt206RowEntity> RowEntities { get; set; } = new List<ReqRpt206RowEntity>();

        public void GetTableDataByManual(string lot, string startDate, string endDate)
        {
            LotStr = lot;
            StartDate = startDate;
            EndDate = endDate;
            var lots = lot.Split(',');
            var lots_single = lots.Where(w => !w.Contains("*"));
            var lots_like = lots.Except(lots_single).Select(s => s.Replace('*', '%'));
            string condition = string.Format("where ( meas_type='Wafer' or  item_type='Raw') and claim_time between '{0}' and '{1} 23:59:59' ", startDate, endDate);
            condition = string.Format("{0} and ( meas_lot_id in ('{1}') or meas_lot_id like '{2}' )", condition, string.Join("','", lots_single), string.Join("' or meas_lot_id like '", lots_like));
            DB2DataCatcher<EDA_Inline_Data> InlineCatcher = new DB2DataCatcher<EDA_Inline_Data>("ISTRPT.EDA_Inline_Data") { Conditions = condition };
            var list = InlineCatcher.GetEntities().EntityList;
            if (!list.Any()) throw new Exception("没有数据");
            //var group = list.GroupBy(g => new { g.Meas_Lot_ID, g.Meas_MainPD_ID, g.Meas_Ope_No, g.Meas_Pass_Count, g.Claim_Time,g.DcItem_Name });

            var group= list.GroupBy(g => new { g.Meas_Lot_ID, g.Meas_MainPD_ID, g.Meas_Ope_No, g.Meas_Pass_Count, g.Claim_Time });
            List<string> ThkHead = new List<string>() { "THK", "TK" };
            foreach (var gp in group)
            {
                var gp_site = gp.Where(w => w.Meas_Type == "Site");
                var gp_wafer = gp.Where(w => w.Meas_Type == "Wafer");
                var group2 = gp_wafer.GroupBy(g => new { g.DcItem_Name });
                foreach (var g in group2)
                {
                    var entity = new ReqRpt206RowEntity
                    {
                        LotID = gp.Key.Meas_Lot_ID,
                        RouteID = gp.Key.Meas_MainPD_ID,
                        OpeNo = gp.Key.Meas_Ope_No,
                        OperID = g.First().Meas_PD_ID,
                        MeasItem = g.Key.DcItem_Name,
                        SpecItem = g.First().Meas_DcSpec_ID,
                        LC = g.First().Cntl_Lower_Limit,
                        LS = g.First().Spec_Lower_Limit,
                        UC = g.First().Cntl_Upper_Limit,
                        US = g.First().Spec_Upper_Limit,
                        Target = g.First().DcItem_Target,
                        ProdID = g.First().Meas_ProdSpec_ID,
                        OpeTime = gp.Key.Claim_Time.ToString("yyyy-MM-dd HH:mm:ss")
                    };
                    var wafer_t = gp_wafer.Where(w => w.Wafer_ID == "*");

                    foreach (var w in wafer_t)
                    {
                        w.Wafer_ID = (gp_site.Where(wh => wh.Wafer_Position == w.Wafer_Position).FirstOrDefault()??new EDA_Inline_Data()).Wafer_ID;
                    }
                    for (var i = 0; i < 25; i++)
                    {
                        string waferNumber = i < 10 ? "0" + i.ToString() : i.ToString();
                        entity.WaferValue.Add(Str2DoubleNullable(g.Where(w =>w.Wafer_ID.Length>2&& w.Wafer_ID.Substring(w.Wafer_ID.Length - 2) == waferNumber).Select(s => s.DcItem_Value).FirstOrDefault()));
                    }
                    if ((entity.MeasItem.ToUpper().Contains("$THK")&& entity.MeasItem.ToUpper().Contains("MEAN")) || ThkHead.Any(a => entity.MeasItem.ToUpper().IndexOf(a) == 0))
                    {
                        entity.LS = gp_site.First().Spec_Lower_Limit;
                        entity.US = gp_site.First().Spec_Upper_Limit;
                    }
                        RowEntities.Add(entity);
                }

            }
        }

        private double? Str2DoubleNullable(string str)
        {
            if (str is null) return null;
            return Math.Round(Convert.ToDouble(str),3);
        }
    }

}