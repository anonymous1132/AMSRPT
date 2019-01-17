using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AMS.CIM.Caojin.RPTWebApp.Models
{
    public class ReqRpt020TBodyEntity
    {
        public List<ReqRpt020TableRowEntity> RowEntities
        {
            get;
            set;
        } = new List<ReqRpt020TableRowEntity>();

        public ReqRpt020TableRowEntity TotalRow
        {
            get;
            set;
        } = new ReqRpt020TableRowEntity();
    }
}