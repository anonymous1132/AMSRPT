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
    public class RPTCycleTimeTranslator
    {
        public RPTCycleTimeTranslator()
        {
            StartTime = GetLastUpdateTimeFromFile();
            EndTime = CurrentDate;
            Initialize();
        }

        public RPTCycleTimeTranslator(DateTime startTime, DateTime endTime)
        {
            StartTime = startTime;
            EndTime = endTime;
            Initialize();
        }

        string DirPath = System.IO.Path.GetDirectoryName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);
        string Db2ConfigFileName = "\\App\\db2Config.json";
        string LastTimeKeepFileName = "\\App\\cycleTimeRunningConfig.json";

        private DateTime StartTime { get; set; }
        private string sqlStartTime { get { return StartTime.ToString("yyyy-MM-dd HH:mm:ss.ffffff"); } }
        private DateTime EndTime { get; set; }
        private string sqlEndTime { get { return EndTime.ToString("yyyy-MM-dd HH:mm:ss.ffffff"); } }
        private DateTime CurrentDate { get { return DateTime.Now.Date; } }
        private DateTime GetLastUpdateTimeFromFile()
        {
            DateTime dt = DateTime.Now;
            string json = System.IO.File.ReadAllText(DirPath + LastTimeKeepFileName);
            DateTime.TryParseExact(JsonConvert.DeserializeObject<CycleTimeRunningConfig>(json).LastUpdateTime, "yyyy-MM-dd HH.mm.ss.ffffff", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out dt);
            return dt;
        }
        private void RewriteLastUpdateTimeToFile()
        {
            string json = JsonConvert.SerializeObject(new { LastRunTime = EndTime.ToString("yyyy-MM-dd HH.mm.ss.ffffff") });
            System.IO.File.WriteAllText(DirPath + LastTimeKeepFileName,json);
        }
        #region Sql Define
        string DelCurrentDateContentFromTimeFlowTableSql = "DELETE FROM MMVIEW.RPT_STD_TIME_FLOW WHERE FLOW_RECORD_DATE=CURRENT DATE ";
        string DelOldDataFromTimeFlowTableSql = "DELETE FROM MMVIEW.RPT_STD_TIME_FLOW WHERE FLOW_RECORD_DATE < CURRENT DATE ";
        string InsertCurrentDateContentToTimeFlowTableSql = @"INSERT INTO MMVIEW.RPT_STD_TIME_FLOW (
  FLOW_RECORD_DATE, PRODSPEC_ID, MAINPD_ID, OPE_NO, PD_ID,
PD_STD_PROC_TIME_MIN, PD_STD_PROC_TIME_TYPE,
  PD_STD_WAIT_TIME_MIN, PD_STD_WAIT_TIME_TYPE,
PD_STD_CYCLE_TIME_MIN, PD_STD_CYCLE_TIME_TYPE
)
SELECT
CURRENT DATE AS FLOW_RECORD_DATE,
F.PRODSPEC_ID,
F.MAINPD_ID,
  F.OPE_NO,
  F.PD_ID,
CASE
WHEN U.STD_PROC_TIME IS NULL
THEN -1
ELSE
U.STD_PROC_TIME
END AS PD_PROC_TIME,
0 AS PD_PROC_TIME_TYPE,
CASE
  WHEN U.STD_WAIT_TIME IS NULL
     THEN -1
  ELSE
     U.STD_WAIT_TIME
END AS PD_WAIT_TIME,
0 AS PD_WAIT_TIME_TYPE,
CASE
  WHEN U.STD_CYCLE_TIME IS NULL
     THEN -1
  ELSE
     U.STD_CYCLE_TIME
END AS PD_CYCLE_TIME,
0 AS PD_CYCLE_TIME_TYPE
FROM MMVIEW.RPT_FLOW_SUM F
LEFT JOIN (
SELECT PRODSPEC_ID, U.MAINPD_ID, OPE_NO,PD_ID,
USER_STD_PROCTIME_MIN AS STD_PROC_TIME,
USER_STD_PROCTIME_MIN*(FACTOR) AS STD_WAIT_TIME,
USER_STD_PROCTIME_MIN*(1+ FACTOR) AS STD_CYCLE_TIME
FROM MMVIEW.RPTU_DEF_STDTIME U, MMVIEW.RPTU_WAITTIME_FACTOR
WHERE
  description= 'Standard') U
ON
          U.PRODSPEC_ID=F.PRODSPEC_ID
AND U.MAINPD_ID=F.MAINPD_ID
AND U.OPE_NO=F.OPE_NO
AND U.PD_ID=F.PD_ID
WHERE
      F.PD_TYPE IN('Production')
ORDER BY F.PRODSPEC_ID, F.MAINPD_ID, F.OPE_NO";

        string DeleteContentFromTimeFlowTSql = "DELETE FROM MMVIEW.RPT_STD_TIME_FLOW_T";

        string MergeFormTimeFlowSql = @"MERGE INTO MMVIEW.RPT_STD_TIME_FLOW F
USING (
SELECT PRODSPEC_ID, MAINPD_ID, OPE_NO, PROC_TIME_MIN,
WAIT_TIME_MIN, CYCLE_TIME_MIN
FROM (
SELECT PRODSPEC_ID, MAINPD_ID, OPE_NO,
DOUBLE(SUM(PD_PROC_TIME_SEC))/60 AS PROC_TIME_MIN,
DOUBLE(SUM(PD_WAIT_TIME_SEC))/60 AS WAIT_TIME_MIN,
DOUBLE(SUM(PD_PROC_TIME_SEC+PD_WAIT_TIME_SEC))/60 AS CYCLE_TIME_MIN
FROM MMVIEW.RPT_STD_TIME_FLOW_T
GROUP BY PRODSPEC_ID, MAINPD_ID, OPE_NO, PD_PROC_TIME_SEC )) U
on
F.FLOW_RECORD_DATE=CURRENT DATE
AND F.PRODSPEC_ID=U.PRODSPEC_ID
AND F.MAINPD_ID=U.MAINPD_ID
AND F.OPE_NO=U.OPE_NO
WHEN MATCHED THEN
  UPDATE SET
PD_STD_PROC_TIME_MIN=U.PROC_TIME_MIN,
PD_STD_PROC_TIME_TYPE=1,
PD_STD_WAIT_TIME_MIN=U.WAIT_TIME_MIN,
PD_STD_WAIT_TIME_TYPE=1,
PD_STD_CYCLE_TIME_MIN=U.CYCLE_TIME_MIN,
PD_STD_CYCLE_TIME_TYPE=1";

        string MergeFormTimeTableSql = @"MERGE INTO MMVIEW.RPT_STD_TIME M
USING (
    SELECT F.PRODSPEC_ID, F.MAINPD_ID
FROM MMVIEW.RPT_STD_TIME_FLOW F
WHERE FLOW_RECORD_DATE=CURRENT DATE
GROUP BY PRODSPEC_ID, MAINPD_ID
) F
ON
          M.PRODSPEC_ID=F.PRODSPEC_ID
AND M.MAINPD_ID=F.MAINPD_ID
WHEN MATCHED THEN
  UPDATE SET
FLOW_RECORD_DATE=CURRENT DATE
WHEN NOT MATCHED THEN
    INSERT ( PRODSPEC_ID, MAINPD_ID, FLOW_RECORD_DATE,
CYCLETIME_RECORD_DATE )
              VALUES( F.PRODSPEC_ID, F.MAINPD_ID, CURRENT DATE,
'1900-01-01-00.00.00.000000' )";

        string DelCurrentDateContentFromCycleTimeSql = @"DELETE FROM MMVIEW.RPT_STD_TIME_CYCLETIME
WHERE CYCLETIME_RECORD_DATE=CURRENT DATE";

        string InsertIntoCycleTimeSql = @"INSERT INTO MMVIEW.RPT_STD_TIME_CYCLETIME
( CYCLETIME_RECORD_DATE, PRODSPEC_ID, MAINPD_ID,
          FLOW_PROC_TIME_min, FLOW_PROC_TIME_TYPE,
      FLOW_CYCLE_TIME_min, FLOW_CYCLE_TIME_TYPE,
FLOW_WAITTIME_FACTOR_min, FLOW_WAITTIME_FACTOR_TYPE)
   SELECT
CURRENT DATE, PRODSPEC_ID, MAINPD_ID,
SUM(PD_STD_PROC_TIME_MIN) AS FLOW_PROC_TIME_MIN, 1,
SUM(PD_STD_CYCLE_TIME_MIN) AS FLOW_CYCLE_TIME_MIN, 0,
SUM(PD_STD_CYCLE_TIME_MIN) / SUM(PD_STD_PROC_TIME_MIN) AS FLOW_WAITTIME_FACTOR_MIN, 0
       FROM MMVIEW.RPT_STD_TIME_FLOW
       WHERE FLOW_RECORD_DATE=CURRENT DATE
       GROUP BY PRODSPEC_ID, MAINPD_ID ";



        string MergeUpdateCycleTimeSql ="";

        string MergeUpdateTimeTable = @"MERGE INTO MMVIEW.RPT_STD_TIME M
USING (
    SELECT PRODSPEC_ID, MAINPD_ID
FROM MMVIEW.RPT_STD_TIME_CYCLETIME
WHERE CYCLETIME_RECORD_DATE=CURRENT DATE
) C
ON
M.PRODSPEC_ID=C.PRODSPEC_ID
AND M.MAINPD_ID=C.MAINPD_ID
WHEN MATCHED THEN
UPDATE SET
CYCLETIME_RECORD_DATE=CURRENT DATE";
        #endregion
        #region DB About
        private DB2Oper Db2 { get; set; }

        private DB2OperDataCatcher<FRPD_PD_ID> PDCatcher { get; set; }

        private DB2OperDataCatcher<RPTH_LOT_PROC_TIME> ProcTimeCatcher { get; set; }

        private DB2OperDataPusher<RPT_STD_TIME_FLOW_T> TimeFlowTPusher { get; set; }

        #endregion

        //MainLogic
        private void Initialize()
        {
            // GetLastUpdateTimeFromFile();
            if (StartTime >= EndTime) return;
            string jsonObj = System.IO.File.ReadAllText(DirPath + Db2ConfigFileName);
            Db2ConnObj conn = JsonConvert.DeserializeObject<Db2ConnObj>(jsonObj);
            Db2 = new DB2Oper(conn);
            HandleDBData();
        }

        private void HandleDBData()
        {
            Db2.GetSomeData(DelCurrentDateContentFromTimeFlowTableSql);

            Db2.GetSomeData(InsertCurrentDateContentToTimeFlowTableSql);

            Db2.GetSomeData(DeleteContentFromTimeFlowTSql);

            Db2.GetSomeData(DelOldDataFromTimeFlowTableSql);

            PDCatcher = new DB2OperDataCatcher<FRPD_PD_ID>("MMVIEW.FRPD", Db2) { Conditions= "WHERE PD_LEVEL='Main' AND PD_TYPE IN ('Production')" };

            ProcTimeCatcher = new DB2OperDataCatcher<RPTH_LOT_PROC_TIME>("MMVIEW.RPTH_LOT_PROC_TIME",Db2);

            TimeFlowTPusher = new DB2OperDataPusher<RPT_STD_TIME_FLOW_T>("MMVIEW.RPT_STD_TIME_FLOW_T", Db2);

            var pd_list = PDCatcher.GetEntities().EntityList.Select(s=>s.PD_ID);
            string pdCondition = string.Join("','", pd_list);

            ProcTimeCatcher.Conditions =string.Format(@"WHERE
OPE_START_TIME BETWEEN '{0}' AND '{1}'
AND OPE_COMP_USER_ID ='TCS'
AND LAST_OPE_CATEGORY IN ('OperationComplete')
AND OPE_PASS_COUNT=1
AND QTY=25
AND MAINPD_ID IN ('{2}' )
ORDER BY LOT_ID, OPE_START_TIME",sqlStartTime,sqlEndTime, pdCondition);

            var proc_list = ProcTimeCatcher.GetEntities().EntityList;
            if (!proc_list.Any()) return;
            string lotIDCondition = string.Join("','",proc_list.Select(s => s.Lot_ID).Distinct());

            ProcTimeCatcher = new DB2OperDataCatcher<RPTH_LOT_PROC_TIME>("MMVIEW.RPTH_LOT_PROC_TIME", Db2) { Conditions=string.Format( "where LAST_OPE_CATEGORY IN ('OperationComplete', 'STB') and AND MAINPD_ID IN '{0}' and lot_id in ('{1}') ",pdCondition,lotIDCondition) };
            var allProcList = ProcTimeCatcher.GetEntities().EntityList;
            if (!allProcList.Any()) return;
            foreach (var proc in proc_list)
            {
                var p = allProcList.Where(w => w.Lot_ID == proc.Lot_ID && w.Ope_Start_Time < proc.Ope_Start_Time);
                if(p.Any())
                {
                    var entity = p.OrderBy(o => o.Ope_Start_Time).Last();

                    RPT_STD_TIME_FLOW_T fLOW_T = new RPT_STD_TIME_FLOW_T() { Lot_ID=proc.Lot_ID,ProdSpec_ID=proc.ProdSpec_ID,MainPD_ID=proc.MainPD_ID,Ope_No=proc.Ope_No};

                    bool isComplete = entity.Last_Ope_Category == "OperationComplete";
                    fLOW_T.PD_Proc_Time_Sec = isComplete? entity.Process_Duration_Sec:0;
                    fLOW_T.PD_Wait_Time_Sec =isComplete? Convert.ToInt16 ( (proc.Process_Start_Time - entity.Process_End_Time).TotalSeconds): Convert.ToInt16((proc.Process_Start_Time - entity.Ope_Start_Time).TotalSeconds);

                }
            }

            Db2.GetSomeData(MergeFormTimeFlowSql);

            Db2.GetSomeData(MergeFormTimeTableSql);

            Db2.GetSomeData(DeleteContentFromTimeFlowTSql);

            Db2.GetSomeData(DelCurrentDateContentFromCycleTimeSql);

            Db2.GetSomeData(InsertIntoCycleTimeSql);

            MergeUpdateCycleTimeSql = string.Format(@"MERGE INTO MMVIEW.RPT_STD_TIME_CYCLETIME M
USING (
    SELECT PRODSPEC_ID, MAINPD_ID,
      AVG(DOUBLE(TIMESTAMPDIFF(2, COMPLETION_TIME - RELEASED_TIME))/60) AS FLOW_CYCLE_TIME_MIN
FROM MMVIEW.FRLOT
WHERE
     LOT_STATE='FINSIHED' AND LOT_FINISHED_STATE='COMPLETED'
       AND LOT_TYPE='Production'
       AND COMPLETION_TIME BETWEEN '{0}' AND '{1}'
       AND QTY=25
GROUP BY PRODSPEC_ID, MAINPD_ID ) C
ON
          M.CYCLETIME_RECORD_DATE=CURRENT DATE
AND M.PRODSPEC_ID=C.PRODSPEC_ID
AND M.MAINPD_ID=C.MAINPD_ID
WHEN MATCHED THEN
UPDATE SET
FLOW_CYCLE_TIME_MIN=C. FLOW_CYCLE_TIME_MIN,
FLOW_CYCLE_TIME_TYPE=1,
FLOW_WAITTIME_FACTOR_MIN= C.FLOW_CYCLE_TIME_MIN/ FLOW_PROC_TIME_MIN,
FLOW_WAITTIME_FACTOR_TYPE=1",sqlStartTime,sqlEndTime);

            Db2.GetSomeData(MergeUpdateCycleTimeSql);

            Db2.GetSomeData(MergeUpdateTimeTable);

            RewriteLastUpdateTimeToFile();
        }

    }
}
