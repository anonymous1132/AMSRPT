using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMS.CIM.Caojin.RPTLibrary.Models
{
    /// <summary>
    /// 用于ReqRpt004
    /// </summary>
    public class RPT_Turn_Daily
    {
        /// <summary>
        /// 开始时间，仅有2个上班的时间点：8:00,20:00
        /// </summary>
        public DateTime Start_Time { get; set; }

        public string Product_ID { get; set; }
        //move量
        public int MoveQty { get; set; } = 0;
        //有效步数，除dummy、Measurement类型的PD数量（starttime~endtime之间）
        public int EffectiveSteps { get; set; }
        //WIP量，StartTime时间点，Product对应的LotID数量
        public int WIP { get; set; } = 0;
    }
}
