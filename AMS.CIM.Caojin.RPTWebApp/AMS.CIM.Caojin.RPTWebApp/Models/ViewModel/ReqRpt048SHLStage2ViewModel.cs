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

        public List<ReqRpt048ChartGapModel> ChartModels { get; set; } = new List<ReqRpt048ChartGapModel>();

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
        DB2DataCatcher<Report48_SM_Qt> QtCatcher { get; set; } = new DB2DataCatcher<Report48_SM_Qt>("ISTRPT.Report48_SM_Qt_filter");
        //holdhs
        DB2DataCatcher<FHOPEHS_SHL_Stage2> HoldHsCatcher { get; set; } = new DB2DataCatcher<FHOPEHS_SHL_Stage2>("MMVIEW.FHOPEHS");
        DB2DataCatcher<FRCodeModel> CodeCatcher { get; set; } = new DB2DataCatcher<FRCodeModel>("MMVIEW.FRCODE") { Conditions= "where category_id='Department'" };
        DB2DataCatcher<PMSTaskContentAct> PmsCatcher { get; set; } = new DB2DataCatcher<PMSTaskContentAct>("PMSVIEW.Task_Content_Act");

        DB2DataCatcher<SHLFRPDModel> PDCatcher { get; set; } = new DB2DataCatcher<SHLFRPDModel>("MMVIEW.FRPD");

        #endregion


        private void Initialize()
        {
            string sql =string.Format( "select Priority_Class from mmview.fhopehs where lot_id='{0}' order by Claim_Time desc fetch first 1 rows only",LotID);
            var db2 = new DB2Helper();
            db2.GetSomeData(sql);
            Priority = (int)db2.dt.DefaultView[0][0];
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
            ForecastCatcher.Conditions = string.Format("where prodspec_id='{0}' order by ope_no", ProductID);
            ForecastCatcher.GetEntities();
            HoldHsCatcher.Conditions = string.Format("where lot_id ='{0}' and Ope_Category like '%Hold%'",LotID);
            HoldHsCatcher.GetEntities();
            CodeCatcher.GetEntities();
            PmsCatcher.Conditions = " where next_late_date >= to_char( current Date,'yyyy-MM-dd')";
            PmsCatcher.GetEntities();
            PDCatcher.GetEntities();
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
                var pd = PDCatcher.entities.EntityList.Where(w => w.PD_ID == hist.PD_ID);
                entity.OpeName = pd.Any()?pd.First().Ope_Name:"";
                entity.Recipe = flow.LRecipe;
                //PRTime
                double m = 2;
                if (hist.Priority_Class == 1) m = 1.3;
                else if (hist.Priority_Class == 2) m = 1.6;
                entity.PRTime = flow.PT;
                entity.CT = flow.PT*m;
                entity.WFIn =hist.Ope_Category=="Split"? famHists.Where(w=>w.MainPD_ID==hist.MainPD_ID&&w.Ope_NO==hist.Ope_NO).TakeWhile(t=>t.Lot_ID!=LotID).Last().Claim_Time: hist.Claim_Time;
                //stepcomplete
                if (Entities.Any()) { Entities.Last().StepComplete = hist.Claim_Time; entity.Plan = Entities.Last().Plan.AddMinutes(Entities.Last().CT); }
                else { entity.Plan = entity.WFIn.Value; }
                //remark
                var rmk = RMKCatcher.entities.EntityList.Where(w => w.ModulePD_ID == flow.ModulePD_ID && w.Ope_NO == flow.Ope_NO);
                entity.Remark = rmk.Any() ? rmk.First().Remark : "";
                Entities.Add(entity);
            }
            //Priority = lotHists.Last().Priority_Class;
            curFlow = flow;
            curHs = Entities.Last();
            //设置gapmodel
            foreach (var entity in Entities)
            {
                if (entity.WFIn == null || entity.StepComplete == null) continue;
                var holdList = HoldHsCatcher.entities.EntityList.Where(w=>w.Ope_NO==entity.OpeNO&&w.Claim_Time<=entity.StepComplete&&w.Claim_Time>=entity.WFIn);
                if (holdList.Any())
                {
                    ReqRpt048GapComputer computer = new ReqRpt048GapComputer();
                    var holds = holdList.Where(w => w.Ope_Category != "LotHoldRelease");
                    var release = holdList.Where(w => w.Ope_Category == "LotHoldRelease").ToList();
                    foreach (var hold in holds)
                    {
                        ReqRpt048GapComputerEntity computerEntity = new ReqRpt048GapComputerEntity();
                        computerEntity.ReasonCode = hold.Hold_Reason_Code;
                        computerEntity.StartTime = hold.Claim_Time;
                        var firstRelease = release.Where(w => w.Hold_Reason_Code == hold.Hold_Reason_Code).First();
                        computerEntity.EndTime =firstRelease.Claim_Time;
                        release.Remove(firstRelease);
                        computer.GapComputerEntities.Add(computerEntity);
                    }
                    entity.GapModels= computer.GetResault();
                    foreach (var model in entity.GapModels)
                    {
                        model.Department = GetDepartmentByCode(model.Department);
                    }
                }
            }
            foreach (var dept in CodeCatcher.entities.EntityList)
            {
                var item = new ReqRpt048ChartGapModel();
                item.Department = dept.Description;
                double rawGap = Entities.Sum(s => s.GapWithOutHold == null ? 0 : s.GapWithOutHold.Value);
                double delta = Entities.Sum(s => s.GapModels.Where(q => q.Department == dept.Description).Sum(p=>p.HoldGap));
            }
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
                entity.ModulePD = cast.ModulePD_ID;
                var pd = PDCatcher.entities.EntityList.Where(w=>w.PD_ID==cast.PD_ID);
                entity.OpeName = pd.Any() ? pd.First().Ope_Name : "";
                entity.Recipe = cast.LRecipe;
                double m = Priority == 1 ? 1.3 : 1.6;
                entity.PRTime = cast.PT ;
                entity.CT = cast.PT * m;
                var lastCT = Entities.Last().CT;
                var lastPT= Entities.Last().PRTime;
                entity.Plan = Entities.Last().Plan.AddMinutes(lastPT*m);
                if (Entities.Last().Forecast == null)
                {
                    var forecastDate = Entities.Last().WFIn.Value.AddMinutes(lastCT);
                    entity.Forecast = forecastDate>DateTime.Now? Entities.Last().WFIn.Value.AddMinutes(lastPT*m) : DateTime.Now.AddMinutes(lastPT*m);
                }
                else
                {
                    entity.Forecast = Entities.Last().Forecast.Value.AddMinutes(lastPT * m);
                }
                var rmk = RMKCatcher.entities.EntityList.Where(w => w.ModulePD_ID == cast.ModulePD_ID && w.Ope_NO == cast.Ope_NO);
                entity.Remark = rmk.Any() ? rmk.First().Remark : "";
                //EqpList
                var eqps = cast.Eqp_List.Split('|').ToList();
                foreach (var eqp in eqps)
                {
                    var state = CurStateCatcher.entities.EntityList.Where(w => w.Eqp_ID == eqp);
                    var eqpEntity = new ReqRpt048EqpEntity()
                    {
                        EqpID = eqp,
                        E10Status = state.Any() ? state.First().E10_State : "",
                        StateID = state.Any() ? state.First().Cur_State_ID : "",
                        EqpStateChgTime = state.Any() ? state.First().State_History_Time : DateTime.MinValue
                    };
                    var pms = PmsCatcher.entities.EntityList.Where(w => w.Eqp_ID == eqp &&w.Next_Early_Date.CompareTo(entity.Forecast.Value.ToString("yyyy-MM-dd")) <=0 &&w.Next_Late_Date.CompareTo(entity.Forecast.Value.ToString("yyyy-MM-dd"))>=0);
                    if (pms.Any())
                    {
                        var p = pms.OrderBy(o=>o.Next_Early_Date).First();
                        eqpEntity.PMS_Early_Time = p.Next_Early_Date;
                        eqpEntity.PMS_Late_Time = p.Next_Late_Date;
                        eqpEntity.PMS_Time = p.Next_PM_Date;
                        eqpEntity.Description = p.Description;
                    }
                    entity.EqpList.Add(eqpEntity);
                }
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
            var list2 = QtCatcher.entities;
            foreach (var qt in QtCatcher.entities.EntityList)
            {
                //var qtList = QtCatcher.entities.EntityList.Where(w => w.Oper_NO.CompareTo(entity.OpeNO) <= 0 && w.Target.CompareTo(entity.OpeNO) >= 0);
                var list= Entities.SkipWhile(s => s.OpeNO != qt.Oper_NO).TakeWhile(t => t.OpeNO.CompareTo(qt.Target) <= 0);
                if (!list.Any()) continue;
                string type = "";
                double remainQt = 0;
                double remainCt = 1;
                //case History
                if (qt.Target.CompareTo(curFlow.Ope_NO) < 0)
                {
                    remainQt = qt.Duration;
                    remainCt= (list.Last().StepComplete.Value - list.First().WFIn.Value).TotalMinutes;
                    type = "h";//history
                }
                //case Future
                else if (qt.Oper_NO.CompareTo(curFlow.Ope_NO)> 0)
                {
                    remainQt = qt.Duration;
                    remainCt = list.Sum(s => s.CT);
                    type = "f";//future
                }
                //case current
                else
                {
                    double totalUsed = (DateTime.Now - list.First().WFIn.Value).TotalMinutes;
                    double totalQt = qt.Duration;
                    remainCt = list.SkipWhile(s => s.OpeNO != curFlow.Ope_NO).Sum(s => s.CT);
                    remainQt = totalQt - totalUsed;
                    type = "c";//current
                }

                foreach (var l in list)
                {
                    l.Qtime =remainCt==0?0: remainQt/remainCt;
                    l.QtimeType = type;
                    l.strQtime = string.Format("{0}/{1}",remainQt,remainCt);
                }
         
            }
 
        }

        private string GetDepartmentByCode(string code)
        {
            return CodeCatcher.entities.EntityList.Where(w => w.Code_ID == code).First().Description;
        }
    }
}