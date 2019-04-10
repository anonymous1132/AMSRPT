using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AMS.CIM.Caojin.RPTLibrary.Models;

namespace AMS.CIM.Caojin.RPTWebApp.Models
{
    public class ReqRpt062MainTableBuilder
    {
        public ReqRpt062MainTableBuilder(string userid)
        {
            UserID = userid;
            Initialize();
        }
        string UserID { get; set; }

        public List<Report62_Lot> LotEntities { get; set; } 

        public List<Report_FoupOwner> CastEntities { get; set; } 

        DB2DataCatcher<Report62_Lot> LotCatcher { get; set; } = new DB2DataCatcher<Report62_Lot>("ISTRPT.Report62_Lot");

        DB2DataCatcher<Report_FoupOwner> CastCatcher { get; set; } = new DB2DataCatcher<Report_FoupOwner>("ISTRPT.Report_FoupOwner");

        void Initialize()
        {
            LotCatcher.Conditions = string.Format("where Lot_Owner_ID='{0}' and Lot_State='ACTIVE'",UserID);
            var lots = LotCatcher.GetEntities().EntityList;
            LotEntities = lots.Any() ? lots.ToList() : new List<Report62_Lot>();

            CastCatcher.Conditions = string.Format("where foup_owner ='{0}' and drbl_state !='SCRAPPED'",UserID);
           var casts = CastCatcher.GetEntities().EntityList;
            CastEntities = casts.Any() ? casts.ToList() : new List<Report_FoupOwner>();

        }
    }
}