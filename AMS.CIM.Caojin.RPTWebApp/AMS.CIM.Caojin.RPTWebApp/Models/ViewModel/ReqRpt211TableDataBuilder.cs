using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AMS.CIM.Caojin.RPTLibrary.Models;

namespace AMS.CIM.Caojin.RPTWebApp.Models
{
    public class ReqRpt211TableDataBuilder
    {
        public static List<EDA_Prod_Lot_MappingModel> GetLotAndProdMappingList()
        {
            DB2DataCatcher<EDA_Prod_Lot_MappingModel> prodCatcher = new DB2DataCatcher<EDA_Prod_Lot_MappingModel>("ISTRPT.EDA_Prod_Lot_Mapping");
            var list = prodCatcher.GetEntities().EntityList;
            return list.Any() ? list.Select(s=>new EDA_Prod_Lot_MappingModel {ProdSpec_ID=s.ProdSpec_ID,Prod_Category_ID=s.Prod_Category_ID,Lot_ID=s.Lot_ID.Split('.')[0]+"*" }).ToList() : new List<EDA_Prod_Lot_MappingModel>();
        }

        public static List<ReqRpt211RowEntity> GetRowEntities(string lot)
        {
            var lots = lot.Split(',');
            var lots_single = lots.Where(w => !w.Contains("*"));
            var lots_like = lots.Except(lots_single).Select(s => s.Replace('*', '%'));
            string condition = string.Format("where lot_id in ('{0}') or lot_id like '{1}' order by claim_time", string.Join("','", lots_single), string.Join("' or lot_id like '", lots_like));
            DB2DataCatcher<EDA_Lot_Wafer_HistModel> histCatcher = new DB2DataCatcher<EDA_Lot_Wafer_HistModel>("ISTRPT.EDA_Lot_Wafer_Hist") { Conditions=condition};
            var list = histCatcher.GetEntities().EntityList;
            var rowEntities = new List<ReqRpt211RowEntity>();
            foreach (var l in list)
            {
                var entity = new ReqRpt211RowEntity()
                {
                    LotID = l.Lot_ID,
                    FoupID = l.Cast_ID,
                    OperID=l.PD_ID,
                    OperNo=l.Ope_No,
                    Qty=l.Cur_Wafer_Qty,
                    OperCategory=l.Ope_Category,
                    ClaimMemo=l.Claim_Memo,
                    OperName=l.PD_Name,
                    OperTime=l.Claim_Time.ToString("yyyy-MM-dd HH:mm:ss"),
                    RouteID=l.MainPD_ID,
                    UserDept=l.Dept,
                    UserFullName=l.User_Full_Name,
                    UserID=l.Claim_User_ID
                };
                if (l.WafeList != null)
                {
                    var wl = l.WafeList.Split(',');
                    foreach (var w in wl)
                    {
                        var w_sp = w.Split('.');

                        if (w_sp.Length == 3 && int.TryParse(w_sp[2], out int temp) && temp < 26)
                        {
                            entity.WaferList[temp - 1] = true;
                        }
                    }
                }
                rowEntities.Add(entity);
            }
            return rowEntities;
        }
    }
}