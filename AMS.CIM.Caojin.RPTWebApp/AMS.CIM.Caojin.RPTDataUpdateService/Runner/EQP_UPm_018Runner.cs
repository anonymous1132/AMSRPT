using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AMS.CIM.Caojin.RPTLibrary.Models;

namespace AMS.CIM.Caojin.RPTDataUpdateService.Runner
{
    public class EQP_UPm_018Runner
    {
        public static void Run()
        {
            ShareDataEntity.GetSingleEntity();
            ShareDataEntity.GetSingleEntity().Rpt018 = new ReqRpt018ViewModel();
            ShareDataEntity.GetSingleEntity().Rpt018.GetData();
        }
    }
}
