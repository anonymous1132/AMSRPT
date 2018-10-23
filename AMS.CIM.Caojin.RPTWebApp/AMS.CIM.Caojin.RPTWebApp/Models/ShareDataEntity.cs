using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AMS.CIM.Caojin.RPTWebApp.Models
{
    public class ShareDataEntity
    {
        private static ShareDataEntity _singleton;

        public static ShareDataEntity GetSingleEntity()
        {
            if (_singleton == null)
            {
                _singleton = new ShareDataEntity();
            }
            return _singleton;
        }

        public ShareDataEntity()
        {
            FREQPCatcher = new DB2DataCatcher<FREQPModel>("MMVIEW.FREQP",true);
            FHESCHSCatcher = new DB2DataCatcher<FHESCHSModel>("End_Time", "MMVIEW.FHESCHS");
            FRCodeCatcher = new DB2DataCatcher<FRCodeModel>("MMVIEW.FRCODE");
            FRUserCatcher = new DB2DataCatcher<FRUserModel>("MMVIEW.FRUSER");
        }

        public DB2DataCatcher<FREQPModel> FREQPCatcher;

        public DB2DataCatcher<FHESCHSModel> FHESCHSCatcher;

        public DB2DataCatcher<FRCodeModel> FRCodeCatcher;

        public DB2DataCatcher<FRUserModel> FRUserCatcher;

        public TimeSpan SplitTimeOfDay
        {
            get;
            set;
        } = TimeSpan.Parse("8:00");

        public ReqRpt018ViewModel Rpt018=new ReqRpt018ViewModel();
    }
}