using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caojin.Common;
using AMS.CIM.Caojin.RPTLibrary.Models;
using Newtonsoft.Json;

namespace AMS.CIM.Caojin.RPTLibrary.Models
{
    public class RPTFlowSumTranslator
    {
        public RPTFlowSumTranslator()
        {
            Initialize();
        }

        string DirPath =System.IO.Path.GetDirectoryName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);
        string Db2ConfigFileName = "\\App\\db2Config.json";
       // string LastTimeKeepFileName = "\\App\\cycleTimeRunningConfig.json";
        //private DateTime StartTime { get; set; }
        //private DateTime LastUpdateTime { get; set; }

        #region Sql Def
        public string DelNotExistOpeStepAtSMSql =@"MERGE INTO MMVIEW.RPT_FLOW_SUM S
USING(
SELECT PRODSPEC_ID, MAINPD_ID, OPE_NO, PD_ID FROM MMVIEW.RPT_FLOW_SUM
EXCEPT
SELECT PROD.D_THESYSTEMKEY AS PRODSPEC_ID,
  PO.D_THESYSTEMKEY AS MAINPD_ID,
  PO.OPER_NO AS OPE_NO,
  PO.PD_IDENT AS PD_ID
FROM SMVIEW.FBMNPD_M MN, SMVIEW.FBMNPD_PO_M PO, SMVIEW.FBPROD_M PROD
WHERE
    PO.D_THESYSTEMKEY= MN.D_THESYSTEMKEY
AND PROD.MAINPD_IDENT= MN.D_THESYSTEMKEY) T
ON
    S.PRODSPEC_ID = T.PRODSPEC_ID
AND S.MAINPD_ID = T.MAINPD_ID
AND S.OPE_NO = T.OPE_NO
AND S.PD_ID = T.PD_ID
WHEN MATCHED
    THEN DELETE ;";

        public string CmpSmOpeWithRptFlowSql = @"MERGE INTO MMVIEW.RPT_FLOW_SUM S
USING (
SELECT
  PROD.D_THESYSTEMKEY AS PRODSPEC_ID,
  PO.D_THESYSTEMKEY AS MAINPD_ID,
  PO.OPER_NO AS OPE_NO,
  PO.MODPD_IDENT AS MODULEPD_ID,
  PO.PD_IDENT AS PD_ID,
  PD.PD_NAME,
  MN.MAINPD_TYPE_ID AS PD_TYPE,
  MN.PROC_FLOW_TYPE AS FLOW_TYPE,
  PO.STAGE_IDENT AS STAGE_ID,
  PD.DFLT_LRCP_IDENT as LRecipe
FROM SMVIEW.FBMNPD_M MN, SMVIEW.FBMNPD_PO_M PO,
SMVIEW.FBPROD_M PROD, SMVIEW.FBPD_M PD
WHERE
    PO.D_THESYSTEMKEY=MN.D_THESYSTEMKEY
AND PD.D_THESYSTEMKEY=PO.PD_IDENT
AND PROD.MAINPD_IDENT=MN.D_THESYSTEMKEY
ORDER BY PROD.D_THESYSTEMKEY, PO.D_THESYSTEMKEY, PO.OPER_NO ) U
ON
    S.PRODSPEC_ID = U.PRODSPEC_ID
AND S.MAINPD_ID = U.MAINPD_ID
AND S.OPE_NO = U.OPE_NO
AND S.PD_ID = U.PD_ID
WHEN MATCHED AND 
 (S.MODULEPD_ID<>U.MODULEPD_ID OR S.PD_NAME<>U.PD_NAME OR S.PD_TYPE<>U.PD_TYPE
  OR S.FLOW_TYPE<>U.FLOW_TYPE OR S.STAGE_ID<>U.STAGE_ID) THEN
  UPDATE SET (S.MODULEPD_ID, S.PD_NAME, S.PD_TYPE, S.FLOW_TYPE, S.STAGE_ID,
              S.LAST_UPDATE, S.LAST_UPDATE_TIME,S.LRECIPE)=
             (U.MODULEPD_ID, U.PD_NAME, U.PD_TYPE, U.FLOW_TYPE, U.STAGE_ID,
              'Update', CURRENT TIMESTAMP,U.LRecipe)
WHEN NOT MATCHED THEN
  INSERT (PRODSPEC_ID, MAINPD_ID, OPE_NO, MODULEPD_ID, PD_ID, PD_NAME,
     PD_TYPE, FLOW_TYPE, STAGE_ID, LRECIPE, MRECIPE_LIST, EQP_LIST, EQP_TYPE,
     LAST_UPDATE, LAST_UPDATE_TIME)
  VALUES (U.PRODSPEC_ID, U.MAINPD_ID, U.OPE_NO, U.MODULEPD_ID, U.PD_ID, U.PD_NAME,
     U.PD_TYPE, U.FLOW_TYPE, U.STAGE_ID, U.LRecipe, '', '', '', 'New', CURRENT TIMESTAMP) ;";
        #endregion

        #region DB About
        private DB2Oper Db2 { get; set; }

        private DB2OperDataCatcher<RPT_FLOW_SUM> FlowSumCatcher { get; set; }

        private DB2OperDataCatcher<RPT_FLOW_SUM_EQPBASE> FlowEqpCatcher { get; set; }

        private DB2OperDataCatcher<RPT_FLOW_SUM_LRCP> LRcpCatcher { get; set; }
        //用于get mrecipe by lrecipe
        private DB2OperDataCatcher<FBLRCP_SSET_M> SETCatcher { get; set; }

        private DB2OperDataCatcher<RPT_FLOW_SUM_MR_LR_MAPPING> MR_LRCatcher { get; set; }

        private DB2OperDataCatcher<RPT_FLOW_SUM_EQPT_LR_MAPPING> EQPType_LRCatcher { get; set; }

        #endregion

        //MainLogic
        private void Initialize()
        {
           // GetLastUpdateTimeFromFile();
            string jsonObj = System.IO.File.ReadAllText(DirPath+Db2ConfigFileName);
            Db2ConnObj conn = JsonConvert.DeserializeObject<Db2ConnObj>(jsonObj);
            Db2 = new DB2Oper(conn);
            HandleDBData();
        }

        //private void GetLastUpdateTimeFromFile()
        //{
        //    DateTime dt = DateTime.Now.Date;
        //    string json = System.IO.File.ReadAllText(DirPath + LastTimeKeepFileName);
        //    DateTime.TryParseExact(JsonConvert.DeserializeObject<CycleTimeRunningConfig>(json).LastUpdateTime, "yyyy-MM-dd-HH.mm.ss.ffffff", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out dt);
        //    StartTime = dt;
        //}

        private void HandleDBData()
        {
            Db2.GetSomeData(DelNotExistOpeStepAtSMSql);
            Db2.GetSomeData(CmpSmOpeWithRptFlowSql);
            FlowSumCatcher = new DB2OperDataCatcher<RPT_FLOW_SUM>("MMVIEW.RPT_FLOW_SUM",Db2);
            FlowEqpCatcher = new DB2OperDataCatcher<RPT_FLOW_SUM_EQPBASE>("ISTRPT.RPT_FLOW_SUM_EQPBASE", Db2);
           // LRcpCatcher = new DB2OperDataCatcher<RPT_FLOW_SUM_LRCP>("ISTRPT.RPT_FLOW_SUM_LRCP",Db2);
            SETCatcher = new DB2OperDataCatcher<FBLRCP_SSET_M>("SMVIEW.FBLRCP_SSET_M", Db2);
            MR_LRCatcher = new DB2OperDataCatcher<RPT_FLOW_SUM_MR_LR_MAPPING>("ISTRPT.RPT_FLOW_SUM_MR_LR_MAPPING", Db2);
            EQPType_LRCatcher = new DB2OperDataCatcher<RPT_FLOW_SUM_EQPT_LR_MAPPING>("ISTRPT.RPT_FLOW_SUM_EQPT_LR_MAPPING",Db2);
            var FlowSumList = FlowSumCatcher.GetEntities().EntityList;
            var FlowEqpList = FlowEqpCatcher.GetEntities().EntityList;
            //var LRcpList = LRcpCatcher.GetEntities().EntityList;
            //var SetList = SETCatcher.GetEntities().EntityList;
            IList<FBLRCP_SSET_M> SetList=null;
            //var MR_LRList = MR_LRCatcher.GetEntities().EntityList;
            IList<RPT_FLOW_SUM_MR_LR_MAPPING> MR_LRList = null;
            //var EqpType_LRList = EQPType_LRCatcher.GetEntities().EntityList;
            IList<RPT_FLOW_SUM_EQPT_LR_MAPPING> EqpType_LRList = null;
            //重置待更新的字段,并在该遍历中获取EqpType、EqpList、Logic Recipe
            var newFlowList = FlowSumList.Select(s=>new RPT_FLOW_SUM() { ProdSpec_ID=s.ProdSpec_ID,MainPD_ID=s.MainPD_ID,Ope_No=s.Ope_No,PD_ID=s.PD_ID,LRecipe= s.LRecipe});
            List<RPT_FLOW_SUM> UpdateList = new List<RPT_FLOW_SUM>();
            foreach (var item in newFlowList)
            {
                try
                {
                    SetEqpInfo(item, FlowEqpList);
                    // SetLRcpInfo(item, LRcpList);
                    bool hasGet = SetMRcpInfoBySset(item, SetList);
                    if (!hasGet) { SetMRcpInfoByDset(item, MR_LRList); }
                    if (item.Eqp_Type == "") SetEqpType(item, EqpType_LRList);

                    var rawFlow = FlowSumList.Where(w => w.ProdSpec_ID == item.ProdSpec_ID && w.MainPD_ID == item.MainPD_ID && w.Ope_No == item.Ope_No).First();
                    if (!(rawFlow.Eqp_Type == item.Eqp_Type && rawFlow.LRecipe == item.LRecipe && rawFlow.MRecipe_List == item.MRecipe_List && rawFlow.Eqp_List == item.Eqp_List)) UpdateList.Add(item);

                }
                catch (Exception e)
                {
                    LogHelper.ErrorLog(string.Format("RPTFlowSumTranslator>HandleDBData>foreach模块,ProdSpec_ID={0},MainPD_ID={1},Ope_NO={2}",item.ProdSpec_ID,item.MainPD_ID,item.Ope_No),e);
                }
            }

            List<string> sqlList = UpdateList.Select(s => string.Format("UPDATE MMVIEW.RPT_FLOW_SUM SET EQP_TYPE ='{0}',EQP_LIST='{1}',MRECIPE_LIST='{3}',LAST_UPDATE_TIME=CURRENT TIMESTAMP WHERE PRODSPEC_ID='{4}' AND MAINPD_ID='{5}' AND OPE_NO='{6}'",s.Eqp_Type,s.Eqp_List,s.LRecipe,s.MRecipe_List,s.ProdSpec_ID,s.MainPD_ID,s.Ope_No)).ToList();

            Db2.UpdateBatchCommand(sqlList);
        }

        private void SetEqpInfo(RPT_FLOW_SUM flow,IList<RPT_FLOW_SUM_EQPBASE>FlowEqpList)
        {
            //test
            if (flow.PD_ID =="APAAPD0180.00")
            {
                string test = "";
            }
            var list = FlowEqpList.Where(w =>( w.ProdSpec_ID == flow.ProdSpec_ID || w.Origin == "4") && w.PD_ID == flow.PD_ID);
            if(!list.Any())return ;
            var firstItem = list.First();
            flow.Eqp_Type = firstItem.Eqp_Type;
             flow.Eqp_List = FormatListToStringWithinMaxLenth (list.Where(w => w.Origin == firstItem.Origin).Select(s => s.Eqp_ID).Distinct().OrderBy(o=>o).ToList(),255);
            //flow.Eqp_List = FormatListToStringWithinMaxLenth(list.Select(s => s.Eqp_ID).Distinct().OrderBy(o => o).ToList(), 255);
        }

        private void SetLRcpInfo(RPT_FLOW_SUM flow,IList<RPT_FLOW_SUM_LRCP>LRcpList)
        {
            var list = LRcpList.Where(w => w.ProdSpec_ID == flow.ProdSpec_ID && w.PD_ID == flow.PD_ID);
            if (!list.Any()) return;
            var firstItem = list.First();
            flow.LRecipe = firstItem.LRcp_Ident;
        }

        private bool SetMRcpInfoBySset(RPT_FLOW_SUM flow, IList<FBLRCP_SSET_M> SetList)
        {
            if (SetList == null) SetList = SETCatcher.GetEntities().EntityList;
            var list = SetList.Where(w => w.D_TheSystemKey == flow.LRecipe);
            if (!list.Any()) return false;
            if (flow.Eqp_Type == "")
            {
                flow.Eqp_List = FormatListToStringWithinMaxLenth(list.Select(s => s.EQP_IDENT).Distinct().OrderBy(o => o).ToList(),255);
                flow.MRecipe_List =FormatListToStringWithinMaxLenth(list.Select(s => s.RCP_IDENT).Distinct().OrderBy(o => o).ToList(),255);
            }
            else
            {
                list = list.Where(w => flow.Eqp_List.Contains(w.EQP_IDENT));
                flow.MRecipe_List = list.Any() ? FormatListToStringWithinMaxLenth( list.Select(s => s.RCP_IDENT).OrderBy(o => o).ToList(),255) : "";
            }
            return true;
        }

        private void SetMRcpInfoByDset(RPT_FLOW_SUM flow, IList<RPT_FLOW_SUM_MR_LR_MAPPING> MR_LRList)
        {
            if (MR_LRList == null) MR_LRList = MR_LRCatcher.GetEntities().EntityList;
            var list = MR_LRList.Where(w => w.LRecipe == flow.LRecipe);
            if (!list.Any()) return;
            if (flow.Eqp_Type == "")
            {
                flow.Eqp_List =FormatListToStringWithinMaxLenth(list.Select(s => s.EQP_ID).Distinct().OrderBy(o => o).ToList(),255);
                flow.MRecipe_List = FormatListToStringWithinMaxLenth(list.Select(s => s.MRecipe).Distinct().OrderBy(o => o).ToList(),255);
            }
            else
            {
                list = list.Where(w => flow.Eqp_List.Contains(w.EQP_ID));
                flow.MRecipe_List = list.Any() ? FormatListToStringWithinMaxLenth(list.Select(s => s.MRecipe).Distinct().OrderBy(o => o).ToList(),255) : "";
            }
        }

        private void SetEqpType(RPT_FLOW_SUM flow, IList<RPT_FLOW_SUM_EQPT_LR_MAPPING> EqpType_LRList)
        {
            if (EqpType_LRList == null) EqpType_LRList = EQPType_LRCatcher.GetEntities().EntityList;
            var list = EqpType_LRList.Where(w => w.LRecipe == flow.LRecipe);
            if (!list.Any()) return;
            flow.Eqp_Type = list.First().EQP_Type;
        }

        string FormatListToStringWithinMaxLenth(List<string>list,int maxLength)
        {
            string str = "";
            for (var i = 0; i < list.Count; i++)
            {
                string temp = str + list[i] ;
                if (temp.Length > maxLength)
                {
                    return str.TrimEnd('|');
                }
                str = temp + "|";
            }
            return str.TrimEnd('|');
        }
        
    }

    class CycleTimeRunningConfig
    {
        public string LastRunTime { get; set; }
    }
}
