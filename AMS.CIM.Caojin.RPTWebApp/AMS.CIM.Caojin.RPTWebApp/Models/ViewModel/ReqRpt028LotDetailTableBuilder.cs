using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AMS.CIM.Caojin.RPTLibrary.Models;

namespace AMS.CIM.Caojin.RPTWebApp.Models
{
    public class ReqRpt028LotDetailTableBuilder : DB2OperateBase
    {
        public ReqRpt028LotDetailTableBuilder(ReqRpt028LotDetailQueryPostModel postModel)
        {
            PostModel = postModel;

            GetDB();

            DataHandle();

            GetPreLay();
        }

        ReqRpt028LotDetailQueryPostModel PostModel { get; set; }

        DB2OperDataCatcher<Report28_LotDetail> LotCatcher { get; set; }

        DB2OperDataCatcher<Report28_LotDetail> SplitCaseCatcher { get; set; }


        IList<Report28_LotDetail> LotInfoList { get; set; } = new List<Report28_LotDetail>();
        IList<Report28_LotDetail> SplitInfoList { get; set; }= new List<Report28_LotDetail>();

        IEnumerable<string> Products { get { return LotInfoEntities.Select(s => s.ProductID).Distinct(); } }
        public List<ReqRpt028LotInfoEntity> LotInfoEntities { get; set; } = new List<ReqRpt028LotInfoEntity>();
        public override void Initial()
        {
           
        }

        void GetDB()
        {
            string x = PostModel.OpeNo;
            LotCatcher = new DB2OperDataCatcher<Report28_LotDetail>("ISTRPT.Report28_LotDetail", DB2) { Conditions =string.Format( "where lot_id in ('{0}') and Ope_No='{1}'",string.Join("','",PostModel.LotList),PostModel.OpeNo) };
            LotInfoList = LotCatcher.GetEntities().EntityList;
            var lotList_1 = LotInfoList.Select(s => s.Lot_ID).Distinct();
            if (PostModel.LotList.Count() > lotList_1.Count())
            {
                var splitLots = PostModel.LotList.Except(lotList_1);
                SplitCaseCatcher = new DB2OperDataCatcher<Report28_LotDetail>("ISTRPT.Report28_SplitLotDetail", DB2) { Conditions = string.Format("where lot_id in ('{0}') and ope_no='{1}'", string.Join("','", splitLots),PostModel.OpeNo) };
                SplitInfoList = SplitCaseCatcher.GetEntities().EntityList;
            }
        }

        void DataHandle()
        {
            foreach (var lot in PostModel.LotList)
            {
                Report28_LotDetail detail = new Report28_LotDetail();
                var list = LotInfoList.Where(w => w.Lot_ID == lot);
                if (!list.Any())
                {
                    list = SplitInfoList.Where(w => w.Lot_ID == lot && w.Ope_No == PostModel.OpeNo);
                    if (!list.Any()) continue;
                }
                detail = list.Last();
                var entity = new ReqRpt028LotInfoEntity
                {
                    LotID = lot,
                    OpeNo =  PostModel.OpeNo,
                    ChgUserID = detail.Claim_User_ID,
                    ChgUserName = detail.User_Name,
                    OpeName = detail.PD_Name,
                    Foup = detail.Cast_ID,
                    Location = detail.Location,
                    Status = detail.State,
                    OpeStartTime = detail.Claim_Time is null?"": detail.Claim_Time.Split('.')[0],
                    //WaitTime =Math.Round( (DateTime.Now - detail.Claim_Time).TotalHours,2,MidpointRounding.AwayFromZero),
                    //StatusTime = 0,
                    CustomerDate="",
                    HoldReason=detail.Hold_Reason_Code,
                    HoldReasonDesc=detail.Hold_Reason_Desc,
                    LotProcStatus=detail.Lot_Process_State,
                    LotHoldStatus=detail.Lot_Hold_State,
                    Priority=detail.Priority_Class,
                    ProductID=detail.ProdSpec_ID,
                    Qty=detail.Cur_Wafer_Qty
                };
                LotInfoEntities.Add(entity);
            }
        }

        void GetPreLay()
        {
            string sql = string.Format("select eqp_type  from mmview.rpt_flow_sum where prodspec_id='{0}' and ope_no='{1}'", Products.First(), PostModel.OpeNo);
            DB2.GetSomeData(sql);
           // Stage = DB2.dt.DefaultView[0][0].ToString();
            string EqpType = DB2.dt.DefaultView[0][0].ToString();
            DB2OperDataCatcher<FHOPEHS_LotEqp> eqpCatcher = new DB2OperDataCatcher<FHOPEHS_LotEqp>("MMVIEW.FHOPEHS", DB2) { Conditions = string.Format("where  lot_id in ('{0}') and ope_category = 'OperationStart' and ope_no < '{1}' ", string.Join("','", PostModel.LotList), PostModel.OpeNo) };
            var eqpList = eqpCatcher.GetEntities().EntityList;
            foreach (var lot in LotInfoEntities)
            {
                lot.EqpType = EqpType;
                var list = eqpList.Where(w => w.Lot_ID == lot.LotID);
                lot.PreLayer = list.Any() ? list.Last().Eqp_ID : "";
            }
        }
    }
}