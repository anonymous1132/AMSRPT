using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AMS.CIM.Caojin.RPTLibrary.Models;

namespace AMS.CIM.Caojin.RPTWebApp.Models
{
    public class ReqRpt011LotDetailBuilder
    {
        public ReqRpt011LotDetailBuilder(List<string>lots,string type)
        {
            LotList = lots;
            Type = type;
            Initialize();
        }

        readonly string Type = "";
        readonly List<string> LotList;
        public List<ReqRpt011LotDetailEntity> LotEntities { get; set; } = new List<ReqRpt011LotDetailEntity>();
        DB2DataCatcher<Report11_LotDetail> LotCatcher { get; set; } = new DB2DataCatcher<Report11_LotDetail>("ISTRPT.Report11_LotDetail");

        void Initialize()
        {
            if (Type == "Ship") GetDataCaseShip();
            if (Type == "Wip") GetDataCaseWip();
            HandleData();
        }

        void GetDataCaseWip()
        {
            LotCatcher.Conditions = string.Format("where type='Wip' and lot_id in ('{0}')",string.Join("','",LotList));
            LotCatcher.GetEntities();
        }

        void GetDataCaseShip()
        {
            LotCatcher.Conditions= string.Format("where type='Ship' and lot_id in ('{0}')", string.Join("','", LotList));
            LotCatcher.GetEntities();
        }

        void HandleData()
        {
            var list = LotCatcher.entities.EntityList;
            foreach (var lot in list)
            {
                ReqRpt011LotDetailEntity entity = new ReqRpt011LotDetailEntity
                {
                    LotID = lot.Lot_ID,
                    Pri = lot.Priority_Class,
                    ProductID=lot.ProdSpec_ID,
                    ShipTime=lot.Complete_Time.ToString("yyyy/MM/dd HH:mm:ss"),
                    Status=lot.Lot_Inv_State,
                    Qty=lot.Qty,
                    StayDays=Math.Round( (lot.Next_Time-lot.Complete_Time).TotalDays,2),
                    Comment=lot.Bank_ID==""?"":lot.Bank_ID+":"+ lot.Comment
                };
                LotEntities.Add(entity);
            }
        }
    }
}