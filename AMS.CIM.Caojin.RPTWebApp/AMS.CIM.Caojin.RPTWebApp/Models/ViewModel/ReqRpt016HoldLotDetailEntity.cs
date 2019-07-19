using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AMS.CIM.Caojin.RPTWebApp.Models
{
    public class ReqRpt016HoldLotDetailEntity
    {
        public string LotID { get; set; }

        public string LotType { get; set; }

        public string Prod { get; set; }

        public int  Qty { get; set; }

        public int CurQty { get; set; }

        public string MainPDID { get; set; }

        public string OpeNO { get; set; }

        public string PDID { get; set; }

        public string PDName { get; set; }

        public string EqpType { get; set; }

        public string HoldType { get; set; }

        //HoldPDDept必须在ReasonCode前赋值，否则FinalDept会出错
        public string HoldPDDept { get; set; }

        public string HoldUserID { get; set; }

        public string HoldUserName { get; set; }

        public string HoldUserDept { get; set; }

        public string ReleaseUserID { get; set; }

        public string ReleaseUserName { get; set; }

        public string ReleaseUserDept { get; set; }

        public string HoldTime { get; set; }

        public string ReleaseTime { get; set; }

        public double Duration { get; set; }

        public string HoldComment { get; set; }

        public string ReleaseComment { get; set; }

        public string ReasonDept { get; private set; }

        public string FinalDept { get; private set; }

        private string _reasonCode;
        public string ReasonCode
        {
            get { return _reasonCode; }
            set
            {
                _reasonCode = value;
                if (value.Length == 5)
                {
                    ReasonDept = value.Substring(0, 1);
                    FinalDept = ReasonDept;
                }
                else
                {
                    ReasonDept = "system";
                    FinalDept = HoldPDDept;
                }
            }

        }
    }
}