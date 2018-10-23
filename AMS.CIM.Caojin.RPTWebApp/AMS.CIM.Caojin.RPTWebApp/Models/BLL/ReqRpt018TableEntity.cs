using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AMS.CIM.Caojin.RPTWebApp.Models
{
    public class ReqRpt018TableEntity
    {
        public List<ReqRpt018DataBase> Datas
        {
            get;
            set;
        } = new List<ReqRpt018DataBase>();

        public string EqpID
        {
            get;
            set;
        }
    }
}