using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AMS.CIM.Caojin.RPTLibrary.Models;

namespace AMS.CIM.Caojin.RPTWebApp.Models
{
    public class ReqRpt001LotDetailDataBuilder
    {
        public ReqRpt001LotDetailDataBuilder(string prod)
        {
            this.prod = prod;
            Initialize();
        }

        DB2DataCatcher<Report001_LotDetail> LotCatcher { get; set; } = new DB2DataCatcher<Report001_LotDetail>("ISTRPT.Report001_LotDetail");

        public List<Report001_LotDetail> LotDetails { get; set; }

        string prod;

        void Initialize()
        {
            string condition = string.Format("where prodspec_id ='{0}'",prod);
            LotCatcher.Conditions = condition;
            LotDetails= LotCatcher.GetEntities().EntityList.ToList();
        }
    }
}