using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AMS.CIM.Caojin.RPTLibrary.Models;
using Caojin.Common;

namespace AMS.CIM.Caojin.RPTWebApp.Models
{
    public class ReqRpt048RemarkHandler
    {
        public ReqRpt048RemarkHandler(ReqRpt048RemarkHandlePostModel postModel)
        {
            LotID = postModel.LotID;
            ModulePD = postModel.ModulePD;
            OpeNo = postModel.OpeNo;
            Remark = postModel.Remark;
            Initialize();
        }

        private string LotID { get; set; }

        private string ModulePD { get; set; }

        private string OpeNo { get; set; }

        private string Remark { get; set; }

        public string Msg { get; set; }

        DB2DataCatcher<Rpt_SHL_Forecast_Remark> RmkCatcher { get; set; } = new DB2DataCatcher<Rpt_SHL_Forecast_Remark>("ISTRPT.Rpt_SHL_Forecast_Remark");

        private void Initialize()
        {
            RmkCatcher.Conditions =string.Format( "where Lot_ID='{0}' and ModulePD_ID='{1}' and Ope_No='{2}'",LotID,ModulePD,OpeNo);
            RmkCatcher.GetEntities();
            var entity = RmkCatcher.entities.EntityList;
            if (entity.Any())
            {
                if (entity.First().Remark == Remark) { Msg = "数据没有变化"; return; }

                string sql = string.Format("update ISTRPT.Rpt_SHL_Forecast_Remark set Remark='{0}',Update_Time=current timestamp {1} ", Remark, RmkCatcher.Conditions);
                new DB2Helper().GetSomeData(sql);
                Msg = "成功更新一条Remark";
            }
            else
            {
                string sql = string.Format("insert into ISTRPT.Rpt_SHL_Forecast_Remark (Lot_ID,ModulePD_ID,Ope_No,Remark) values ('{0}','{1}','{2}','{3}') ", LotID, ModulePD,OpeNo,Remark);
                new DB2Helper().GetSomeData(sql);
                Msg = "成功新增一条Remark";
            }
        }
    }
}