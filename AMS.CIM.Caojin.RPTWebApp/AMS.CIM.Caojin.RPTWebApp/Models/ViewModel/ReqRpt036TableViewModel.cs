using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AMS.CIM.Caojin.RPTLibrary.Models;

namespace AMS.CIM.Caojin.RPTWebApp.Models
{
    public class ReqRpt036TableViewModel
    {
        public ReqRpt036TableViewModel(ReqRpt036PostModel postModel)
        {
            StartTime = DateTime.Parse(postModel.StartTime).AddHours(8);
            EndTime = postModel.IsToNow ? DateTime.Now : DateTime.Parse(postModel.EndTime).AddHours(8);
            if (EndTime > DateTime.Now) { throw new Exception("EndTime比服务器当前时间大"); }
            HasChamebr = postModel.HasChamebr;
            var list = postModel.EqpTypes.Split(',');
            foreach (string str in list)
            {
                EqpTypeList.Add(str);
            }
            Initialize();
        }

        #region PostModelData
        private DateTime StartTime { get; set; }

        private DateTime EndTime { get; set; }

        private string sqlStartTime { get { return StartTime.ToString("yyyy-MM-dd HH:mm:ss"); } }

        private string sqlEndTime { get { return EndTime.ToString("yyyy-MM-dd HH:mm:ss"); } }

        private List<string> EqpTypeList { get; set; } = new List<string>();

        private bool HasChamebr { get; set; } = false;

        private bool HasLoadPort { get; set; } = false;
        #endregion

        #region DB2Catchers
        private DB2DataCatcher<Report36_EQP> EQPCatcher { get; set; } = new DB2DataCatcher<Report36_EQP>("ISTRPT.Report36_EQP");

        private DB2DataCatcher<Report36_Chamber> ChamberCatcher { get; set; } = new DB2DataCatcher<Report36_Chamber>("ISTRPT.Report36_Chamber");

        private DB2DataCatcher<Report36_LoadPort> LoadPortCatcher { get; set; } = new DB2DataCatcher<Report36_LoadPort>("ISTRPT.Report36_LoadPort");

        private DB2DataCatcher<FREQP_CurStatus> CurStatusCatcher { get; set; } = new DB2DataCatcher<FREQP_CurStatus>("MMVIEW.FREQP");

        private DB2DataCatcher<FREQPST> EqpStateCatcher { get; set; } = new DB2DataCatcher<FREQPST>("MMVIEW.FREQPST");

        private DB2DataCatcher<Report36_ChamberCurrentState> ChamberCurStatus { get; set; } = new DB2DataCatcher<Report36_ChamberCurrentState>("ISTRPT.Report36_Chamber_Cur_State");
        #endregion

        #region Use For Keep Data
        private string sqlEqpListStr { get; set; }

        private string GetLoadPortCondition()
        {
            string BetweenCondition = string.Format("where eqp_id in ('{0}') and start_time between '{1}' and '{2}'", sqlEqpListStr, sqlStartTime, sqlEndTime);

            string PreCondition = string.Format("select b.eqp_id,a.eqp_state,b.start_time from (select eqp_id,max(start_time) as start_time from ISTRPT.Report36_LoadPort where  owner_eqp_id in ('{0}') and start_time <'{1}' group by eqp_id) b left join ISTRPT.Report36_LoadPort a on a.eqp_id=b.eqp_id and a.start_time=b.start_time", sqlEqpListStr, sqlStartTime);

            string NextCondition = string.Format("select b.eqp_id,a.eqp_state,b.start_time from (select eqp_id,min(start_time) as start_time from ISTRPT.Report36_LoadPort where  owner_eqp_id in ('{0}') and start_time >'{1}' group by eqp_id) b left join ISTRPT.Report36_LoadPort a on a.eqp_id=b.eqp_id and a.start_time=b.start_time", sqlEqpListStr, sqlEndTime);

            return string.Format("{0} union {1} union {2}", BetweenCondition, PreCondition, NextCondition);
        }


        #endregion

        #region 对外exports部分
        public List<ReqRpt036EqpEntity> RowEntities { get; set; } = new List<ReqRpt036EqpEntity>();

        public double TotalSecondDuration { get { return (EndTime - StartTime).TotalSeconds; } }

        public DateTime GetStartTime { get { return StartTime; } }

        public DateTime GetEndTime { get { return EndTime; } }

        public string QueryConditions { get { return string.Format("EqpTypes:{0};From:{1} To {2};{3}",string.Join(",",EqpTypeList),sqlStartTime,sqlEndTime,HasChamebr?"Include Chamber;":""); } }

        #endregion

        private void Initialize()
        {
            //获取查询Type对应的Eqp CurrentStatus
            CurStatusCatcher.Conditions =EqpTypeList.Count==0? "where eqp_id in (select eqp_id from mmview.freqp where claim_time >'2017-01-01')" : string.Format("where eqp_id in (select eqp_id from mmview.freqp where claim_time >'2017-01-01') and eqp_type in ('{0}')", string.Join("','",EqpTypeList));
            var EqpsList = CurStatusCatcher.GetEntities().EntityList;
            if (EqpsList.Count() == 0) throw new Exception("所选EqpType没有找到对应实际Run的机台");
            //sql语句中eqp条件
            sqlEqpListStr = string.Join("','", EqpsList.Select(s => s.Eqp_ID));
            string queryEqpCondition = string.Format("where eqp_id in ('{0}') and (not end_time < '{1}') and (not start_time > '{2}')",sqlEqpListStr,sqlStartTime,sqlEndTime);
            string queryChamberCondition = string.Format("where owner_eqp_id in ('{0}') and (not end_time < '{1}') and (not start_time > '{2}')", sqlEqpListStr, sqlStartTime, sqlEndTime);
            EQPCatcher.Conditions = queryEqpCondition;
            ChamberCatcher.Conditions = queryChamberCondition;
           // LoadPortCatcher.Conditions = GetLoadPortCondition();

            //DB查询
            var EqpEntities = EQPCatcher.GetEntities().EntityList.ToList();
            var ChamberEntities =HasChamebr? ChamberCatcher.GetEntities().EntityList.ToList():new List<Report36_Chamber>();
            var EqpStateList = EqpStateCatcher.GetEntities().EntityList;
            //  var LoadPortEntities = HasLoadPort ? LoadPortCatcher.GetEntities().EntityList.ToList() : new List<Report36_LoadPort>();
            var ChamberCurList = HasChamebr ? ChamberCurStatus.GetEntities().EntityList : null;

            //赋值
            foreach (var eqp in EqpsList)
            {
                ReqRpt036EqpEntity eqpEntity = new ReqRpt036EqpEntity();
                eqpEntity.CurState = eqp.E10_State;
                eqpEntity.EqpID = eqp.Eqp_ID;
                var list = EqpEntities.Where(w => w.EQP_ID == eqp.Eqp_ID).Select(s => new ReqRpt036EqpHistoryEntity() { StartTime = s.Start_Time, E10State = s.E10_State, EqpState = s.EQP_State, EndTime = s.End_Time, Claim_Memo = s.Claim_Memo, Claim_User_ID = s.Claim_User_ID, Description = s.Description });
                eqpEntity.HistoryEntities = MergedHistoryEntities(list.ToList());

               // var latestEntity = list.OrderBy(o=>o.EndTime).LastOrDefault();



                SetDic(eqpEntity,EqpStateList);

                RowEntities.Add(eqpEntity);

                var chamberHistoryList = ChamberEntities.Where(w => w.EQP_ID.Contains(eqp.Eqp_ID));
                if (chamberHistoryList.Any())
                {
                    var chamberIDList = chamberHistoryList.Select(s => s.EQP_ID).Distinct();
                    foreach (var chamber in chamberIDList)
                    {
                        ReqRpt036EqpEntity chamberEntity = new ReqRpt036EqpEntity();
                        chamberEntity.EqpID = chamber;
                        chamberEntity.CurState = ChamberCurList.Where(w => w.Eqp_ID == chamber).Select(s => s.New_E10_State).First();
                        var entity = chamberHistoryList.Where(w => w.EQP_ID == chamber);
                        var clist = entity.Select(s => new ReqRpt036EqpHistoryEntity() { StartTime = s.Start_Time, E10State = s.E10_State, EqpState = s.EQP_State, EndTime = s.End_Time, Claim_Memo = s.Claim_Memo, Claim_User_ID = s.Claim_User_ID, Description = s.Description });
                        chamberEntity.HistoryEntities = clist.ToList();
                        SetDic(chamberEntity,EqpStateList);
                        RowEntities.Add(chamberEntity);
                    }
                }

            }



        }

        private void SetDic(ReqRpt036EqpEntity eqpEntity, IList<FREQPST> EqpStateList)
        {
            SetDuration(eqpEntity);
            foreach (var item in EqpStateList)
            {
                //var list = eqpEntity.HistoryEntities.Where(w => w.EqpState == item.EqpState_ID);
                //int c = list.Any() ? 0 : list.Sum(s => s.DurationSecond);
                //eqpEntity.Dic_DetailState.Add(item.EqpState_ID, c);
                eqpEntity.Dic_DetailState.Add(item.EqpState_ID, eqpEntity.HistoryEntities.Where(w => w.EqpState == item.EqpState_ID).Sum(s => s.DurationSecond));
            }
        }

        //连续相同的状态合并
        private List<ReqRpt036EqpHistoryEntity> MergedHistoryEntities(List<ReqRpt036EqpHistoryEntity>rawEntities)
        {
            rawEntities= rawEntities.OrderBy(o=>o.StartTime).ToList();
            for (int i = rawEntities.Count; i >1; i--)
            {
                if (rawEntities[i-1].EqpState == rawEntities[i - 2].EqpState)
                {
                    rawEntities[i - 2].EndTime = rawEntities[i-1].EndTime;
                    rawEntities.RemoveAt(i-1);
                }
            }
            //2019-5-23 caojin 新增以下内容，用以调整Change Comment、Change User对应的操作记录
            for (int i = rawEntities.Count; i > 1; i--)
            {
                rawEntities[i - 1].Claim_Memo = rawEntities[i - 2].Claim_Memo;
                rawEntities[i - 1].Claim_User_ID = rawEntities[i - 2].Claim_User_ID;
            }
            rawEntities[0].Claim_Memo = "未查询";
            rawEntities[0].Claim_User_ID = "未查询";
            return rawEntities.ToList();
        }

        private void SetDuration(ReqRpt036EqpEntity eqpEntity)
        {
            foreach (var item in eqpEntity.HistoryEntities)
            {
                item.SetDurationSecond(StartTime,EndTime);
            }
        }
    }
}