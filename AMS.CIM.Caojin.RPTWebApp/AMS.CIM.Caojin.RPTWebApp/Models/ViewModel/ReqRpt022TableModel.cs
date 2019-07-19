using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AMS.CIM.Caojin.RPTLibrary.Models;

namespace AMS.CIM.Caojin.RPTWebApp.Models
{
    public class ReqRpt022TableModel
    {
        /// <summary>
        /// ReqRpt022TableModel
        /// </summary>
        /// <param name="startTime">eg:2019-06-06 00:00:00</param>
        /// <param name="endTime">eg:2019-06-06 23:59:59</param>
        public ReqRpt022TableModel(string startTime,string endTime)
        {
            ScrapCatcher.Conditions = string.Format("where scrap_time between '{0}' and '{1}'",startTime,endTime);

            Initialize();
        }
        DB2DataCatcher<Report22_Scrap_List> ScrapCatcher { get; set; } = new DB2DataCatcher<Report22_Scrap_List>("ISTRPT.Report22_Scrap_List");
        public List<ReqRpt022TableRowEntity> RowEntities { get; set; } = new List<ReqRpt022TableRowEntity>();

        void Initialize()
        {
          var list=  ScrapCatcher.GetEntities().EntityList;
            foreach (var l in list)
            {
                var entity = new ReqRpt022TableRowEntity
                {
                    LotID = l.Lot_ID,
                    LotType = l.Lot_Type,
                    ClaimMemo = l.Claim_Memo,
                    User = l.User_Name,
                    Owner = l.Owner_Name,
                    Code=l.Reason_Code,
                    CodeDesc=l.Reason_Description,
                    EqpType=l.Eqp_Type,
                    EventTime=l.Event_Time.HasValue?l.Event_Time.Value.ToString("yyyy-MM-dd HH:mm:ss"):"",
                    MainPD=l.Reason_MainPD_ID,
                    ModulePD=l.ModulePD_ID,
                    OpeNo=l.Reason_Ope_No,
                    Qty=l.Qty,
                    ScrapTime=l.Scrap_Time.ToString("yyyy-MM-dd HH:mm:ss"),
                    ScrapType="WaferScrap",
                    Module=l.Dept
                };
                RowEntities.Add(entity);
            }
        }
    }
}