using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AMS.CIM.Caojin.RPTLibrary.Models;
using Caojin.Common;

namespace AMS.CIM.Caojin.RPTWebApp.Models
{
    public class ReqRpt048SHLStage2ViewModel
    {
        public ReqRpt048SHLStage2ViewModel(string lot, string prod)
        {
            LotID = lot;
            ProductID = prod;
            Initialize();
        }
        private string LotID { get; set; }

        private string ProductID { get; set; }

        private int Priority { get; set; }

        public List<ReqRpt048Stage2Entity> Entities { get; set; } = new List<ReqRpt048Stage2Entity>();

        private ReqRpt048Stage2Entity curHs { get; set; }

        private Report48_Forecast curFlow { get; set; }

        #region DB2
        DB2DataCatcher<FREQPModel_CurState> CurStateCatcher { get; set; } = new DB2DataCatcher<FREQPModel_CurState>("MMVIEW.FREQP");
        //history
        DB2DataCatcher<FHOPEHS_SHL_Stage2> HSCatcher { get; set; } = new DB2DataCatcher<FHOPEHS_SHL_Stage2>("MMVIEW.FHOPEHS");
        //remark table
        DB2DataCatcher<Rpt_SHL_Forecast_Remark> RMKCatcher { get; set; } = new DB2DataCatcher<Rpt_SHL_Forecast_Remark>("ISTRPT.Rpt_SHL_Forecast_Remark");
        //main view
        DB2DataCatcher<Report48_Forecast> ForecastCatcher { get; set; } = new DB2DataCatcher<Report48_Forecast>("ISTRPT.Report48_Forecast");
        //qt
        DB2DataCatcher<Report48_SM_Qt> QtCatcher { get; set; } = new DB2DataCatcher<Report48_SM_Qt>("ISTRPT.Report48_SM_Qt");
        #endregion


        private void Initialize()
        {
            GetDb2Datas();
            SetHistEntities();
            SetForcastEntities();
            SetQtValue();
        }

        private void GetDb2Datas()
        {
            CurStateCatcher.GetEntities();
            HSCatcher.Conditions = string.Format("where lot_id {0} and Ope_Category in ('STB','OperationComplete','Split')", LotID.Split('.').Length > 1 ? "like '"+ LotID.Split('.')[0] + ".%'" :"='"+LotID+"'");
            HSCatcher.GetEntities();
            RMKCatcher.Conditions = string.Format("where lot_id='{0}'", LotID);
            RMKCatcher.GetEntities();
            ForecastCatcher.Conditions = string.Format("where prodspec_id='{0}'", ProductID);
            ForecastCatcher.GetEntities();
        }

        private void SetHistEntities()
        {
            var famHists = HSCatcher.entities.EntityList;
            var lotHists= famHists.Where(w => w.Lot_ID == LotID).OrderBy(o=>o.Claim_Time);
            var firstIn = lotHists.First();
            var lotInHists = lotHists.Where(w => w.Ope_Category == "STB" || w.Ope_Category == "OperationComplete" ||w.Claim_Time==firstIn.Claim_Time);
            Report48_Forecast flow = new Report48_Forecast();
            //每天历史记录生成一条entity,去掉过程中的Split记录
            foreach (var hist in lotInHists)
            {
                ReqRpt048Stage2Entity entity = new ReqRpt048Stage2Entity();
                flow = ForecastCatcher.entities.EntityList.Where(w => w.Ope_NO == hist.Ope_NO && w.MainPD_ID == hist.MainPD_ID).First();
                entity.OpeNO = hist.Ope_NO;
                entity.Department = flow.Department;
                entity.EqpType = flow.Eqp_Type;
                //EqpList
                var eqps = flow.Eqp_List.Split('|').ToList();
                foreach (var eqp in eqps)
                {
                    entity.EqpList.Add(new ReqRpt048EqpEntity() {
                        EqpID=eqp
                    });
                }
                entity.ModulePD = flow.ModulePD_ID;
                entity.Recipe = flow.LRecipe;
                //PRTime
                double m = 1;
                if (hist.Priority_Class == 1) m = 0.3;
                else if (hist.Priority_Class == 2) m = 0.6;
                entity.PRTime = flow.PT*m;
                entity.CT = flow.CT*m;
                double addCt = 0;
                addCt = ForecastCatcher.entities.EntityList.SkipWhile(s =>!( s.MainPD_ID == firstIn.MainPD_ID && s.Ope_NO == firstIn.Ope_NO)).TakeWhile(t => !(t.MainPD_ID == hist.MainPD_ID && t.Ope_NO == hist.Ope_NO)).Sum(s => s.CT)+flow.CT;
                entity.Plan = firstIn.Claim_Time.AddMinutes(addCt);
                entity.Forecast = hist.Claim_Time.AddMinutes(flow.CT);
                entity.WFIn =hist.Ope_Category=="Split"? famHists.Where(w=>w.MainPD_ID==hist.MainPD_ID&&w.Ope_NO==hist.Ope_NO).TakeWhile(t=>t.Lot_ID!=LotID).Last().Claim_Time: hist.Claim_Time;
                //stepcomplete
                if (Entities.Any()) { Entities.Last().StepComplete = hist.Claim_Time; }
                //remark
                var rmk = RMKCatcher.entities.EntityList.Where(w => w.ModulePD_ID == flow.ModulePD_ID && w.Ope_NO == flow.Ope_NO);
                entity.Remark = rmk.Any() ? rmk.First().Remark : "";
                Entities.Add(entity);
            }
            Priority = lotInHists.Last().Priority_Class;
            curFlow = flow;
            curHs = Entities.Last();
        }

        private void SetForcastEntities()
        {
            var ForecastList = ForecastCatcher.entities.EntityList.Where(w => w.Ope_NO.CompareTo(curFlow.Ope_NO) > 0);
            foreach (var cast in ForecastList)
            {
                ReqRpt048Stage2Entity entity = new ReqRpt048Stage2Entity();
                entity.OpeNO = cast.Ope_NO;
                entity.Department = cast.Department;
                entity.EqpType = cast.Eqp_Type;
                //EqpList
                var eqps = cast.Eqp_List.Split('|').ToList();
                foreach (var eqp in eqps)
                {
                    var state = CurStateCatcher.entities.EntityList.Where(w => w.Eqp_ID == eqp);
                    entity.EqpList.Add(new ReqRpt048EqpEntity()
                    {
                        EqpID = eqp,
                        E10Status = state.Any() ? state.First().E10_State:"",
                        StateID=state.Any()?state.First().Cur_State_ID:"",
                        EqpStateChgTime=state.Any()?state.First().State_History_Time:DateTime.MinValue
                    });
                }
                entity.ModulePD = cast.ModulePD_ID;
                entity.Recipe = cast.LRecipe;
                double m = Priority == 1 ? 0.3 : 0.6;
                entity.PRTime = cast.PT * m;
                entity.CT = cast.CT * m;
                entity.Plan = Entities.Last().Plan.AddMinutes(entity.CT);
                entity.Forecast = Entities.Last().Forecast.Value.AddMinutes(entity.CT);
                var rmk = RMKCatcher.entities.EntityList.Where(w => w.ModulePD_ID == cast.ModulePD_ID && w.Ope_NO == cast.Ope_NO);
                entity.Remark = rmk.Any() ? rmk.First().Remark : "";
                Entities.Add(entity);
            }
        }

        private void SetQtValue()
        {
            //最后站点填入状态
            foreach (var eqp in curHs.EqpList)
            {
                var state = CurStateCatcher.entities.EntityList.Where(w => w.Eqp_ID == eqp.EqpID);
                eqp.E10Status = state.Any() ? state.First().E10_State : "";
                eqp.StateID = state.Any() ? state.First().Cur_State_ID : "";
                eqp.EqpStateChgTime = state.Any() ? state.First().State_History_Time:DateTime.MinValue;
            }
            //Qt region
            QtCatcher.Conditions = string.Format("where D_Thesystemkey in ('{0}','{1}')", curFlow.MainPD_ID, curFlow.ModulePD_ID);
            QtCatcher.GetEntities();
            var qtList = QtCatcher.entities.EntityList.Where(w => w.Oper_NO.CompareTo(curFlow.Ope_NO) <= 0 && w.Target.CompareTo(curFlow.Ope_NO) >= 0);
            
            if (qtList.Any())
            {
                //暂且取第一个吧。。。
                var qt = qtList.First();
                double remainQt = qt.Duration - (DateTime.Now - curHs.WFIn.Value).TotalMinutes;
                var lastQtStep = Entities.Where(w => w.OpeNO == qt.Target).Last().Forecast;
                double remainCt = (lastQtStep.Value - DateTime.Now).TotalMinutes;
                curHs.Qtime = remainQt / remainCt;
            }
        }
    }
}