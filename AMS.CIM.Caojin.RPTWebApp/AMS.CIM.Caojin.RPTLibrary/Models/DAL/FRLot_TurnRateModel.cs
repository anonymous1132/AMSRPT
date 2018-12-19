using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMS.CIM.Caojin.RPTLibrary.Models
{

    /// <summary>
    /// TurnRate WIP数据计算使用。
    /// 第一次供ReqRpt025Translator文件调用。（condition 必须有lot_type='Production'）
    /// </summary>
    public class FRLot_TurnRateModel
    {
        public string Lot_ID { get; set; }

        public string ProdSpec_ID { get; set; }

        public DateTime Created_Time { get; set; }

        public DateTime Completion_Time { get; set; }
    }
}
