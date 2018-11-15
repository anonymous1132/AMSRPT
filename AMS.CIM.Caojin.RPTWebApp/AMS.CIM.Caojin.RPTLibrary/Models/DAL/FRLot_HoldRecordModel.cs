using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMS.CIM.Caojin.RPTLibrary.Models
{
    public class FRLot_HoldRecordModel
    {
        public string D_TheSystemKey { get; set; }

        public string Hold_Type { get; set; }

        public string Hold_Reason_ID { get; set; }

        public string Hold_Time { get; set; }

        public DateTime dtHoldTime { get { return DateTime.ParseExact(Hold_Time, "yyyy-MM-dd-HH.mm.ss.ffff", System.Globalization.CultureInfo.CurrentCulture); } }
    }
}
