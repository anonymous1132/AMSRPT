using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AMS.CIM.Caojin.RPTLibrary.Models;

namespace AMS.CIM.Caojin.RPTWebApp.Models
{
    public class ReqRpt030GetDetailModel
    {
        public ReqRpt030GetDetailModel(string LotID)
        {
            List<string> list = LotID.Split(',').ToList();
            dB2.Conditions =string.Format( "where Special_LOT_ID in ('{0}')",string.Join("','",list));
            EntityList= dB2.GetEntities().EntityList.ToList();
        }
        public DB2DataCatcher<Report_SpecialDetail> dB2 { get; set; } = new DB2DataCatcher<Report_SpecialDetail>("ISTRPT.REPORT30_SPECIAL_DETAIL");
        public List<Report_SpecialDetail> EntityList;
    }
}