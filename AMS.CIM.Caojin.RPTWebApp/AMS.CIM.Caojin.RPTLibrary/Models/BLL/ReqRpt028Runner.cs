using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caojin.Common;

namespace AMS.CIM.Caojin.RPTLibrary.Models
{
    public class ReqRpt028Runner
    {
        public ReqRpt028Runner(Db2ConnObj connObj)
        {
            LotCatcher = new DB2OperDataCatcher<FRLot_WipChart>("MMVIEW.FRLOT", new DB2Oper(connObj)) { Conditions = Condition };
            WipChartPusher = new DB2OperDataPusher<FRLot_WipChart>("ISTRPT.RPT_WIPCHART_YSTD", new DB2Oper(connObj));
            Initialize();
        }

        DB2OperDataCatcher<FRLot_WipChart> LotCatcher { get; set; }
        DB2OperDataPusher<FRLot_WipChart> WipChartPusher { get; set; }
        readonly string DelOldRecords = "delete from ISTRPT.RPT_WIPCHART_YSTD";
        string Condition { get { return "where lot_type='Production' and lot_state='ACTIVE'"; } }
        private void Initialize()
        {
            LotCatcher.DB2.GetSomeData(DelOldRecords);
            LotCatcher.GetEntities();
            var lotList = LotCatcher.entities.EntityList;
            if (lotList.Count() < 1) throw new Exception("没有YSTD Record");
            WipChartPusher.entities.EntityList = LotCatcher.entities.EntityList;
            WipChartPusher.PushEntities();
        }
    }
}
