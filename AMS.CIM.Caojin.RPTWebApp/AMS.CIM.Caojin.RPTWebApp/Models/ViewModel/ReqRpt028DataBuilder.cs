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

        DB2OperDataCatcher<FBProd> ProdCatcher { get; set; }

        IList<Report28_Lot> lotList { get; set; }

        IList<Report28_Flow> flowList { get; set; }

        IList<RPT_WipChart_TargetOut> outList { get; set; }

        public List<string> ProdList { get; set; }

        public List<ReqRpt028ChartModel> CurModels { get; set; } = new List<ReqRpt028ChartModel>();

        public List<ReqRpt028ChartModel> YstdModels { get; set; } = new List<ReqRpt028ChartModel>();

        public List<ReqRpt028TableEntity> CurTableEntities { get; set; } = new List<ReqRpt028TableEntity>();

        public List<ReqRpt028TableEntity> YstdTableEntities { get; set; } = new List<ReqRpt028TableEntity>();


        public override void Initial()
        {
            GetDBData();

            DataHandle();
        }
        //获取DB2中的数据
        public void GetDBData()
        {
            ProdCatcher = new DB2OperDataCatcher<FBProd>("SMVIEW.FBPROD_M", DB2) { Conditions="where prodcat_ident='Production' and identifier not like 'SL%'"};
            ProdList = ProdCatcher.GetEntities().EntityList.Select(s=>s.Identifier).ToList();
            LotCatcher = new DB2OperDataCatcher<Report28_Lot>("ISTRPT.Report28_Lot", DB2) { Conditions=string.Format("where prodspec_id not like 'SL%'")};
            lotList = LotCatcher.GetEntities().EntityList;
         //   if (lotList.Count() == 0) throw new Exception("没有Active的Production Lot");
            var cond = string.Join("','", ProdList);
            FlowCatcher = new DB2OperDataCatcher<Report28_Flow>("ISTRPT.Report28_Flow", DB2) { Conditions = string.Format("where prodspec_id in ('{0}') order by mainpd_id,ope_no", cond) };
            flowList = FlowCatcher.GetEntities().EntityList;
            OutCatcher = new DB2OperDataCatcher<RPT_WipChart_TargetOut>("ISTRPT.RPT_WIPCHART_TARGETOUT",DB2);
            outList=  OutCatcher.GetEntities().EntityList;
        }
        //处理数据
        public void DataHandle()
        {
            var curList = lotList.Where(w => w.Source == "Cur");

            var ystdList = lotList.Where(w => w.Source == "Ystd");

            ProdList.ForEach(f=> {
                var model1 = new ReqRpt028ChartModel() { Product = f };
                var model2 = new ReqRpt028ChartModel() { Product = f };
                var lot_cur = curList.Where(w => w.ProdSpec_ID == f);
                var lot_ystd = ystdList.Where(w => w.ProdSpec_ID == f);
                //CurModels&YstdModels
                foreach (var flow in flowList.Where(w=>w.ProdSpec_ID==f).OrderByDescending(o=>o.Ope_No))
                {
                    var entity = new ReqRpt028ChartEntity()
                    {
                        Stage = flow.ModulePD_Name,
                        Step = flow.PD_ID,
                        OpeNo = flow.Ope_No,
                        // RemainCT =FixRemianCT((flow.PD_Std_Cycle_Time_Min + (model1.ChartEntities.Any() ? model1.ChartEntities.Last().RemainCT : 0)) / (24 * 60))
                        RemainCT = flow.PD_Std_Cycle_Time_Min + (model1.ChartEntities.Any() ? model1.ChartEntities.Last().RemainCT : 0)
                    };
                    var lot_cur_flow = lot_cur.Where(w=>w.MainPD_ID==flow.MainPD_ID&&w.Ope_No==flow.Ope_No);
                    foreach (var lot in lot_cur_flow)
                    {
                        entity.LotEntities.Add(new ReqRpt028LotEntity { LotID=lot.Lot_ID,HoldState=lot.Lot_Hold_State,Priority=lot.Priority_Class,Qty=lot.Qty,InBank=lot.Lot_Inv_State=="InBank"});
                    }
                    //entity.AccWip = entity.GetCurWip() +( model1.ChartEntities.Any() ? model1.ChartEntities.Last().AccWip : 0);
                    entity.Wip = entity.GetCurWip();
                    model1.ChartEntities.Add(entity);
                    var entity2 = new ReqRpt028ChartEntity() {
                        Stage =entity.Stage,
                        Step =entity.Step,
                        OpeNo =entity.OpeNo,
                        //RemainCT =entity.RemainCT
                        RemainCT= flow.PD_Std_Cycle_Time_Min + (model2.ChartEntities.Any() ? model2.ChartEntities.Last().RemainCT : 0)
                    };
                    var lot_ystd_flow = lot_ystd.Where(w => w.MainPD_ID == flow.MainPD_ID && w.Ope_No == flow.Ope_No);
                    foreach (var lot in lot_ystd_flow)
                    {
                        entity2.LotEntities.Add(new ReqRpt028LotEntity { LotID = lot.Lot_ID, HoldState = lot.Lot_Hold_State, Priority = lot.Priority_Class, Qty = lot.Qty,InBank=lot.Lot_Inv_State=="InBank" });
                    }
                    //entity2.AccWip = entity2.GetCurWip() + (model2.ChartEntities.Any() ? model2.ChartEntities.Last().AccWip : 0);
                    entity2.Wip = entity2.GetCurWip();
                    model2.ChartEntities.Add(entity2);
                }
                model1.ChartEntities.Reverse();
                model2.ChartEntities.Reverse();
                if (model2.ChartEntities.Any()) {
                    model1.ChartEntities.ForEach(el=>el.RemainCT=FixRemianCT(el.RemainCT/(24*60)));
                    model2.ChartEntities.ForEach(el => el.RemainCT = FixRemianCT(el.RemainCT / (24 * 60)));
                    CurModels.Add(model1);
                    YstdModels.Add(model2);
                }
               
            });

            ProdList = CurModels.Select(s => s.Product).ToList();
            //TableEntities
            ProdList.ForEach(f=> {
                double keyCt = 0;
                double MonthCt = 0;
                var outs = outList.Where(w => w.ProdSpec_ID == f);
                var curModel = CurModels.Where(w => w.Product == f).First();
                var ystdModel = YstdModels.Where(w => w.Product == f).First();
                var curTable = new ReqRpt028TableEntity { Product = f, AlreadyOut = outs.Any() ? outs.First().CurOut : 0, TargetOut = outs.Any() ? outs.First().TargetOut : 0 };
                var ystdTable= new ReqRpt028TableEntity { Product = f, AlreadyOut = outs.Any() ? outs.First().YstdOut : 0, TargetOut = curTable.TargetOut };
                double remainDays = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month) - DateTime.Now.Day+1;
                //curTable.ForecastOut = curModel.ChartEntities.Where(w => w.RemainCT <= remainDays).First().AccWip;
                var belowCurKeys = curModel.ChartEntities.Where(w => w.RemainCT <= remainDays);
                var aboveCurKeys= curModel.ChartEntities.Where(w => w.RemainCT >= remainDays);
                curTable.ForecastOut = belowCurKeys.Sum(s=>s.Wip);
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
            });
        }

        private double FixRemianCT(double ct)
        {
            //return Math.Round(ct,1,MidpointRounding.AwayFromZero);
            return Math.Round(ct,1);
        }
    }
}