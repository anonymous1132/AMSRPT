using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AMS.CIM.Caojin.RPTWebApp.Models
{
    public class ReqRpt036EqpHistoryEntity
    {
        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public string EqpState { get; set; }

        public string E10State { get; set; }

        public string Description { get; set; }

        public string Claim_Memo { get; set; }

        public string Claim_User_ID { get; set; }

        public double DurationSecond { get; private set; }

        public void SetDurationSecond(DateTime start,DateTime end)
        {
            var s = StartTime > start ? StartTime : start;

            var e = EndTime < end ? EndTime : end;

            DurationSecond= (e - s).TotalSeconds;
        }
    }
}