using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AMS.CIM.Caojin.RPTLibrary.Models;
using AMS.CIM.Caojin.RPTLibrary;

namespace AMS.CIM.Caojin.RPTWebApp.Models
{
    public class ReqRpt018MainViewModel
    {
        public ReqRpt018MainViewModel()
        {
            GetData();
        }

        ~ReqRpt018MainViewModel()
        {
            db.Dispose();
        }

        public RPTContext db = new RPTContext();
        public List<string> Modules
        { get; set; }
        public List<string> EqpType
        { get; set; }
        public List<string> EqpID
        { get; set; }

        private void GetData()
        {
            Modules = db.EQPType_Department_Mapping.Select(s => s.Department).Distinct().ToList();
            Modules.Insert(0,"ALL");

            EqpType = db.EQPType_Department_Mapping.Select(s => s.EqpType).Distinct().ToList();
            EqpType.Insert(0,"ALL");

            EqpID = db.EQP_UPm_018.Select(s => s.EqpID).Distinct().ToList();
            EqpID.Sort();
        }
    }
}