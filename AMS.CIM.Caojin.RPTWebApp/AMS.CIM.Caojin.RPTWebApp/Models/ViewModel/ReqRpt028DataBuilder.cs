using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AMS.CIM.Caojin.RPTLibrary.Models;

namespace AMS.CIM.Caojin.RPTWebApp.Models
{
    public class ReqRpt028DataBuilder:DB2OperateBase
    {

        DB2OperDataCatcher<Report28_Lot> LotCatcher { get; set; }

        DB2OperDataCatcher<Report28_Flow> FlowCatcher { get; set; }

        DB2OperDataCatcher<RPT_WipChart_TargetOut> OutCatcher { get; set; }

       // DB2OperDataCatcher<FBProd> ProdCatcher { get; set; }

        IList<Report28_Lot> lotList { get; set; }

        IEnumerable<Report28_Lot> lotList_Prod_Cur { get; set; }
        IEnumerable<Report28_Lot> lotList_Prod_Ystd { get; set; }
        IEnumerable<Report28_Lot> lotList_Other_Cur { get; set; }
        IEnumerable<Report28_Lot> lotList_Other_Ystd { get; set; }

        IList<Report28_Flow> flowList { get; set; }

        IList<RPT_WipChart_TargetOut> outList { get; set; }

        public List<ReqRpt028ChartModel> CurModels { get; set; } = new List<ReqRpt028ChartModel>();

        public List<ReqRpt028ChartModel> YstdModels { get; set; } = new List<ReqRpt028ChartModel>();

        public List<ReqRpt028TableEntity> CurTableEntities { get; set; } = new List<ReqRpt028TableEntity>();

        public List<ReqRpt028TableEntity> YstdTableEntities { get; set; } = new List<ReqRpt028TableEntity>();

		public List<ReqRpt028TableEntity> ProdList { get; set; } = new List<ReqRpt028TableEntity>();

		//public ReqRpt028ChartModel CurOtherModel;

		//public ReqRpt028ChartModel YSTDOtherModel;

		public override void Initial()
        {
            GetDBData();

            DataHandle();
        }
        //获取DB2中的数据
        public void GetDBData()
        {
            // ProdCatcher = new DB2OperDataCatcher<FBProd>("SMVIEW.FBPROD_M", DB2) { Conditions="where prodcat_ident='Production' and identifier not like 'SL%'"};
            //ProdList = ProdCatcher.GetEntities().EntityList.Select(s=>s.Identifier).ToList();
            //v2 LotCatcher = new DB2OperDataCatcher<Report28_Lot>("ISTRPT.Report28_Lot", DB2) { Conditions=string.Format("where prodspec_id not like 'SL%'")};
            //v3 LotCatcher = new DB2OperDataCatcher<Report28_Lot>("ISTRPT.Report28_LotV2", DB2);//v2
            LotCatcher = new DB2OperDataCatcher<Report28_Lot>("ISTRPT.Report28_LotV2", DB2) { Conditions= "where lot_inv_state in ('OnFloor','NonProBank')" };
            lotList = LotCatcher.GetEntities().EntityList;
            var curList = lotList.Where(w => w.Source == "Cur");
            var ystdList = lotList.Where(w => w.Source == "Ystd");
            lotList_Prod_Cur =curList .Where(w => w.Lot_Type == "Production");
            lotList_Prod_Ystd = ystdList.Where(w => w.Lot_Type == "Production");
            lotList_Other_Cur = curList.Where(w=>w.Lot_Type!="Production");
            lotList_Other_Ystd=ystdList.Where(w => w.Lot_Type != "Production");
            //   if (lotList.Count() == 0) throw new Exception("没有Active的Production Lot");
            var cond = string.Join("','", lotList.Select(s=>s.ProdSpec_ID).Distinct());
            FlowCatcher = new DB2OperDataCatcher<Report28_Flow>("ISTRPT.Report28_Flow", DB2) { Conditions = string.Format("where prodspec_id in ('{0}') order by mainpd_id,ope_no", cond) };
            flowList = FlowCatcher.GetEntities().EntityList;
            OutCatcher = new DB2OperDataCatcher<RPT_WipChart_TargetOut>("ISTRPT.RPT_WIPCHART_TARGETOUT",DB2);
            outList=  OutCatcher.GetEntities().EntityList;
        }
        //处理数据
        public void DataHandle()
        {
            var prods = lotList.Where(w => w.Lot_Type == "Production").Select(s => s.ProdSpec_ID).Distinct();
            foreach (var f in prods)
            {
                var cur = GetChartModelByProd(f, lotList_Prod_Cur);
                var ystd = GetChartModelByProd(f, lotList_Prod_Ystd);
                if (ystd.ChartEntities.Any())
                {
                    CurModels.Add(cur);
                    YstdModels.Add(ystd);
                }
            }
            GetTableEntities(prods);
            var curOtherModels= new List<ReqRpt028ChartModel>();
            var ystdOtherModels = new List<ReqRpt028ChartModel>();
            var otherProds = lotList.Where(w => w.Lot_Type != "Production").Select(s => s.ProdSpec_ID).Distinct().Except(prods);
            foreach (var prod in otherProds)
            {
                var cur = GetChartModelByProd(prod, lotList_Other_Cur);
                var ystd = GetChartModelByProd(prod, lotList_Other_Ystd);
                if (ystd.ChartEntities.Any())
                {
                    curOtherModels.Add(cur);
                    ystdOtherModels.Add(ystd);
                }
            }
            //计算OtherModel
            if (ystdOtherModels.Any())
            {
                var CurOtherModel = GetOtherModelByOtherModels(curOtherModels);
                var YSTDOtherModel = GetOtherModelByOtherModels(ystdOtherModels);
                CurModels.Add(CurOtherModel);
                YstdModels.Add(YSTDOtherModel);
            }

        }


        private ReqRpt028ChartModel GetChartModelByProd(string prod,IEnumerable<Report28_Lot>lots)
        {
            var model = new ReqRpt028ChartModel() { Product = prod };
            foreach (var flow in flowList.Where(w => w.ProdSpec_ID == prod).OrderByDescending(o => o.Ope_No))
            {
                var entity = new ReqRpt028ChartEntity()
                {
                    Stage = flow.ModulePD_Name,
                    Step = flow.PD_ID,
                    OpeNo = flow.Ope_No,
                    RemainCT = flow.PD_Std_Cycle_Time_Min + (model.ChartEntities.Any() ? model.ChartEntities.Last().RemainCT : 0)
                };
                var lot_flow = lots.Where(w => w.MainPD_ID == flow.MainPD_ID && w.Ope_No == flow.Ope_No);
                foreach (var lot in lot_flow)
                {
                    entity.LotEntities.Add(new ReqRpt028LotEntity { LotID = lot.Lot_ID, HoldState = lot.Lot_Hold_State, Priority = lot.Priority_Class, Qty = lot.Qty, InBank = lot.Lot_Inv_State == "NonProBank" });
                }
                entity.Wip = entity.GetCurWip();
                model.ChartEntities.Add(entity);
            }
            model.ChartEntities.Reverse();
            model.ChartEntities.ForEach(el => el.RemainCT = FixRemianCT(el.RemainCT / (24 * 60)));
            return model;
        }

        private void GetTableEntities(IEnumerable<string>prods)
        {
            //TableEntities
            foreach(var f in prods) {
                double keyCt = 0;
                double MonthCt = 0;
                var outs = outList.Where(w => w.ProdSpec_ID == f);
                var curModel = CurModels.Where(w => w.Product == f).First();
                var ystdModel = YstdModels.Where(w => w.Product == f).First();
                var curTable = new ReqRpt028TableEntity { Product = f, AlreadyOut = outs.Any() ? outs.First().CurOut : 0, TargetOut = outs.Any() ? outs.First().TargetOut : 0 };
                var ystdTable = new ReqRpt028TableEntity { Product = f, AlreadyOut = outs.Any() ? outs.First().YstdOut : 0, TargetOut = curTable.TargetOut };
                double remainDays = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month) - DateTime.Now.Day + 1;
                //curTable.ForecastOut = curModel.ChartEntities.Where(w => w.RemainCT <= remainDays).First().AccWip;
                var belowCurKeys = curModel.ChartEntities.Where(w => w.RemainCT <= remainDays);
                var aboveCurKeys = curModel.ChartEntities.Where(w => w.RemainCT >= remainDays);
                curTable.ForecastOut = belowCurKeys.Sum(s => s.Wip);
                curTable.KeyStage = aboveCurKeys.Any() ? aboveCurKeys.Last().Stage : curModel.ChartEntities.First().Stage;
                keyCt = aboveCurKeys.Any() ? aboveCurKeys.Last().RemainCT : curModel.ChartEntities.First().RemainCT;
                //目标剩余量
                var remainCurTaget = curTable.AlreadyOut - curTable.TargetOut;
                if (remainCurTaget >= 0) { curTable.MotherStage = "wafer out"; MonthCt = 0; }
                else
                {
                    var acc = 0;
                    for (var i = curModel.ChartEntities.Count() - 1; i >= 0; i--)
                    {
                        acc += curModel.ChartEntities[i].Wip;
                        if (remainCurTaget + acc >= 0)
                        {
                            curTable.MotherStage = curModel.ChartEntities[i].Stage;
                            MonthCt = curModel.ChartEntities[i].RemainCT;
                            break;
                        }
                    }
                    if (string.IsNullOrEmpty(curTable.MotherStage))
                    {
                        curTable.MotherStage = "wafer start";
                        MonthCt = curModel.ChartEntities.First().RemainCT;
                    }
                }
                curTable.Day = keyCt - MonthCt;
                keyCt = 0;
                MonthCt = 0;
                //ystdTable.ForecastOut=ystdModel.ChartEntities.Where(w => w.RemainCT <= remainDays).First().AccWip;
                var belowYstKeys = ystdModel.ChartEntities.Where(w => w.RemainCT <= remainDays);
                var aboveYstKeys = ystdModel.ChartEntities.Where(w => w.RemainCT >= remainDays);
                ystdTable.ForecastOut = belowYstKeys.Sum(s => s.Wip);
                ystdTable.KeyStage = aboveYstKeys.Any() ? aboveYstKeys.Last().Stage : ystdModel.ChartEntities.First().Stage;
                keyCt = aboveYstKeys.Any() ? aboveYstKeys.Last().RemainCT : ystdModel.ChartEntities.First().RemainCT;
                //目标剩余量
                var remainYstTaget = ystdTable.AlreadyOut - ystdTable.TargetOut;
                if (remainYstTaget >= 0) { ystdTable.MotherStage = "wafer out"; MonthCt = 0; }
                else
                {
                    var acc = 0;
                    for (var i = ystdModel.ChartEntities.Count() - 1; i >= 0; i--)
                    {
                        acc += ystdModel.ChartEntities[i].Wip;
                        if (remainYstTaget + acc >= 0)
                        {
                            ystdTable.MotherStage = ystdModel.ChartEntities[i].Stage;
                            MonthCt = ystdModel.ChartEntities[i].RemainCT;
                            break;
                        }
                    }
                    if (string.IsNullOrEmpty(ystdTable.MotherStage))
                    {
                        ystdTable.MotherStage = "wafer start";
                        MonthCt = ystdModel.ChartEntities.First().RemainCT;
                    }
                }
                ystdTable.Day = keyCt - MonthCt;
                CurTableEntities.Add(curTable);
                YstdTableEntities.Add(ystdTable);
            }
        }

        private ReqRpt028ChartModel GetOtherModelByOtherModels(List<ReqRpt028ChartModel>models)
        {
            var model= new ReqRpt028ChartModel() { Product = "Others" };
            foreach (var m in models)
            {
                model.ChartEntities.InsertRange(0, m.ChartEntities);
            }
            model.ChartEntities.OrderByDescending(o => o.RemainCT).ThenBy(t=>t.OpeNo);
            return model;
        }
        private double FixRemianCT(double ct)
        {
            //return Math.Round(ct,1,MidpointRounding.AwayFromZero);
            return Math.Round(ct,1);
        }
    }
}