using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AMS.CIM.Caojin.RPTLibrary.Models
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
            FREQPCatcher = new DB2DataCatcher<FREQPModel>("MMVIEW.FREQP");
            FHESCHSCatcher = new DB2DataCatcher<FHESCHSModel>("MMVIEW.FHESCHS");
            FRCodeCatcher = new DB2DataCatcher<FRCodeModel>("MMVIEW.FRCODE");
            FRUserCatcher = new DB2DataCatcher<FRUserModel>("MMVIEW.FRUSER");
            FHOPEHSCatcher = new DB2DataCatcher<FHOPEHSModel>("MMVIEW.FHOPEHS");
        }

        public DB2DataCatcher<FREQPModel> FREQPCatcher;

        public DB2DataCatcher<FHESCHSModel> FHESCHSCatcher;

        public DB2DataCatcher<FRCodeModel> FRCodeCatcher;

        public DB2DataCatcher<FRUserModel> FRUserCatcher;

        public DB2DataCatcher<FHOPEHSModel> FHOPEHSCatcher;

        public ReqRpt018ViewModel Rpt018=new ReqRpt018ViewModel();

        public RPTContext db = new RPTContext();
    }
}