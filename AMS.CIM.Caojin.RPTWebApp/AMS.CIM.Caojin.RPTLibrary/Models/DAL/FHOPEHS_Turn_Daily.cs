using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMS.CIM.Caojin.RPTLibrary.Models
{
    /// <summary>
    /// 用于reqrpt004 Turn Rate Effective计算使用
    ///  第一次供ReqRpt025Translator文件调用。（condition 必须有lot_type='Production' and Ope_Category='OperationCompletion'）
    /// </summary>
    public class FHOPEHS_Turn_Daily
    {
        public string ProdSpec_ID { get; set; }

        public DateTime Claim_Time{get;set;}

        //public string PD_Type { get; set; }
    }
}
