using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AMS.CIM.Caojin.RPTLibrary.Models;

namespace AMS.CIM.Caojin.RPTWebApp.Models
{
    public class ReqRpt029TableViewModel
    {
        public ReqRpt029TableViewModel()
        {
            Initialize();
        }
        public List<ReqRpt029TableEntity> Entities { get; set; } = new List<ReqRpt029TableEntity>();
        DB2DataCatcher<ReqRpt029_Lot> LotCatcher { get; set; } = new DB2DataCatcher<ReqRpt029_Lot>("ISTRPT.Report29_Lot") { Conditions="where from_ope_no != null"};
        DB2DataCatcher<ReqRpt029_Flow> FlowCatcher { get; set; } = new DB2DataCatcher<ReqRpt029_Flow>("ISTRPT.Report29_Flow");
        DB2DataCatcher<FHOPEHS_Qtime> HsCatcher { get; set; } = new DB2DataCatcher<FHOPEHS_Qtime>("MMVIEW.FHOPEHS");

        public void Initialize()
        {
            var lotList = LotCatcher.GetEntities().EntityList;
            if (!lotList.Any()) throw new Exception("没有触发Qtime的Lot");
            FlowCatcher.Conditions = string.Format("where mainpd_id in '{0}'",string.Join("','",lotList.Select(s=>s.MainPD_ID).Distinct()));
            FlowCatcher.GetEntities();
          //  HsCatcher.Conditions = string.Format("where lot_family_id",string.Join("','",lotList.Select(s=>s.Lot_ID)));
            foreach (var lot in lotList)
            {
                ReqRpt029TableEntity entity = new ReqRpt029TableEntity() {
                    LotID=lot.Lot_ID,
                    OpeNo=lot.Ope_No,
                    Priority=lot.Priority_Class,
                    Qty=lot.Qty,
                    LotHoldState=lot.Lot_Hold_State,
                    LotProcessState=lot.Lot_Process_State,
                    ToOpeNo=lot.To_Ope_No,
                    Qtime=lot.Qtime,
                    FoupID=lot.Cast_ID,
                    Location=lot.Location,
                    Status=lot.Xfer_State,
                    HoldCode=lot.ReasonCode_ID,
                    HoldComment=lot.Hold_Claim_Memo
                };
                var MainPDFlows = FlowCatcher.entities.EntityList.Where(w => w.MainPD_ID == lot.MainPD_ID);
                var flow =MainPDFlows.Where(w=>w.Ope_No==lot.Ope_No);
                if (flow.Any())
                {
                    entity.Department = flow.First().Description;
                    entity.EqpType = flow.First().Eqp_Type;
                    entity.Step = flow.First().PD_ID;
                }
                var toFlow = MainPDFlows.Where(w => w.Ope_No == lot.To_Ope_No);
                if (toFlow.Any())
                {
                    entity.ToDepartment = toFlow.First().Description;
                    entity.ToEqpType = toFlow.First().Eqp_Type;
                    entity.ToStep = toFlow.First().PD_ID;
                }
                flow = MainPDFlows.Where(w=>w.Ope_No.CompareTo(lot.Ope_No)>0&&w.Ope_No.CompareTo(lot.To_Ope_No)<=0).OrderBy(o=>o.Ope_No);
                //剩余站点ct
                double ct = flow.Where(w => w.PD_Std_Proc_Time_Min > 0).Sum(s => s.PD_Std_Proc_Time_Min);
                double m = 2;
                if (lot.Priority_Class == 1) m = 1.3;
                else if (lot.Priority_Class == 2) m = 1.6;
                ct = ct * m;
                var curFlow = MainPDFlows.Where(w => w.Ope_No == lot.Ope_No&&w.PD_Std_Proc_Time_Min>0);
                //当前站点process time
                double pt = curFlow.Any() ? curFlow.First().PD_Std_Proc_Time_Min : 0;
                entity.RemainCt = pt + ct;
                bool isOrignLot = lot.Lot_ID.Substring(lot.Lot_ID.Length-3)==".00";
                if (isOrignLot)
                {
                    //如果不是分批的情况
                    HsCatcher.Conditions = string.Format("where lot_id='{0}' and ope_category in ('OperationComplete','STB') order by Claim_Time", lot.Lot_ID);
                    HsCatcher.GetEntities();
                    var hsList = HsCatcher.entities.EntityList;
                    entity.FirstStepInTime = hsList.Where(w => w.MainPD_ID == lot.MainPD_ID && w.Ope_No == lot.From_Ope_No).First().Claim_Time;
                }
                else
                {
                    //如果是分批的情况
                    entity.FirstStepInTime = GetTargetStepInTime(lot.Lot_ID,lot.MainPD_ID,lot.From_Ope_No);
                }

            }
        }

        private DateTime GetTargetStepInTime(string lot,string mainPD,string openo)
        {
            DB2DataCatcher<FRLot_SplitLot> splitCatcher = new DB2DataCatcher<FRLot_SplitLot>("MMVIEW.FRLOT");
            splitCatcher.Conditions = string.Format("where lot_id like '{0}.%'", lot.Split('.')[0]);
            var list = splitCatcher.GetEntities().EntityList;
            List<string> lotList = new List<string>();
            string templot = lot;
            lotList.Add(templot);
            while (lot.Split('.')[1] != "00")
            {
                var temList = list.Where(w => w.Lot_ID == templot);
                if (!temList.Any()) break;
                templot = temList.First().Split_Lot_ID;
                lotList.Add(templot);
            }
            HsCatcher.Conditions = string.Format("where lot_id in '{0}' and ope_category in ('OperationComplete','STB','Split') order by claim_time",string.Join("','",lotList) );
            HsCatcher.GetEntities();
            var hsList = HsCatcher.entities.EntityList.TakeWhile(t => t.Lot_ID != lot).Union(HsCatcher.entities.EntityList.Where(w => w.Lot_ID == lot));
           return  hsList.Where(w => w.MainPD_ID == mainPD && w.Ope_No ==openo).First().Claim_Time;
        }
    }
}