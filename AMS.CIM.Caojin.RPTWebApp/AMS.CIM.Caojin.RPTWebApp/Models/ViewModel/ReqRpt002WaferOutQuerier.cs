using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AMS.CIM.Caojin.RPTLibrary.Models;

namespace AMS.CIM.Caojin.RPTWebApp.Models
{
    public class ReqRpt002WaferOutQuerier
    {
        /// <summary>
        /// 计算WaferOut部分
        /// </summary>
        /// <param name="date">查询的参考日期,eg:2019-05-10</param>
        public ReqRpt002WaferOutQuerier(string date)
        {
            Date = DateTime.Parse(date).AddDays(-1);
            Initialize();
        }
        //前一天的日期
        private DateTime Date { get; set; }

        private List<string> Products { get; set; }

        public List<ReqRpt002WaferOutEntity> WaferOutEntities { get; set; } = new List<ReqRpt002WaferOutEntity>();
        public ReqRpt002WaferOutEntity WaferOutTotalEntity { get; set; } = new ReqRpt002WaferOutEntity();

        private void Initialize()
        {
            Products = new ReqRptCommonProductQuerier(6).Prods;
            string month = Date.ToString("yyyy-MM");
            var model = new ReqRpt011TableDataBuilder(month,Products);
            var lineYieldPostModel = new ReqRpt025PostModel() { FromCategory = "day", ToCategory = "day", FromDate = Date.ToString("yyyy-MM") + "-01", ToDate = Date.AddDays(1).ToString("yyyy-MM-dd") };
            var lineYiledDataModel = new ReqRpt025TableViewModel(lineYieldPostModel);
            foreach (var prod in Products)
            {
                var outSource = model.WipEntities.Where(w => w.ProductID == prod);
                var outEntity = new ReqRpt002WaferOutEntity() { Product=prod};
                outEntity.OutSourceAccActual = outSource.Any() ? outSource.First().Plans.Take(Date.Day).Sum(s => s.Actual) : 0;
                outEntity.OutSourceAccTarget= outSource.Any() ? outSource.First().Plans.Take(Date.Day).Sum(s => s.Target) : 0;
                var waferOut = model.ShipEntities.Where(w => w.ProductID == prod);
                outEntity.WFOutAccActual = waferOut.Any() ? waferOut.First().Plans.Take(Date.Day).Sum(s => s.Actual) : 0;
                outEntity.WFOutAccTarget = waferOut.Any() ? waferOut.First().Plans.Take(Date.Day).Sum(s => s.Target) : 0;
                var yieldFindList = lineYiledDataModel.TableEntities.Where(w => w.ProductID == prod);
                outEntity.WFOutYield = yieldFindList.Any() ? yieldFindList.First().Yield : 0;
                WaferOutEntities.Add(outEntity);
            }
            WaferOutTotalEntity.Product = "Total";
            WaferOutTotalEntity.OutSourceAccActual = WaferOutEntities.Sum(s => s.OutSourceAccActual);
            WaferOutTotalEntity.OutSourceAccTarget = WaferOutEntities.Sum(s => s.OutSourceAccTarget);
            WaferOutTotalEntity.WFOutAccActual = WaferOutEntities.Sum(s => s.WFOutAccActual);
            WaferOutTotalEntity.WFOutAccTarget = WaferOutEntities.Sum(s => s.WFOutAccTarget);
            var fab_pass = lineYiledDataModel.TableEntities.Sum(s => s.FAB_PassQty);
            var fab_scrap= lineYiledDataModel.TableEntities.Sum(s => s.FAB_ScrapQty);
            var wat_pass= lineYiledDataModel.TableEntities.Sum(s => s.WAT_PassQty);
            var wat_scrap= lineYiledDataModel.TableEntities.Sum(s => s.WAT_ScrapQty);
            var fab_yield = fab_pass + fab_scrap == 0 ? 0 : fab_pass*1.0 / (fab_pass + fab_scrap);
            var wat_yield = wat_pass + wat_scrap == 0 ? 0 : wat_pass*1.0 / (wat_pass + wat_scrap);
            WaferOutTotalEntity.WFOutYield = fab_yield * wat_yield;
        }
    }
}