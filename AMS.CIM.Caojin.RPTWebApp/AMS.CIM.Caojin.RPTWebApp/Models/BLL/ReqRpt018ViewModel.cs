using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AMS.CIM.Caojin.RPTWebApp.Models
{
    public class ReqRpt018ViewModel
    {
       public ReqRpt018GroupModel Rpt018GroupModel = new ReqRpt018GroupModel();
        public List<string> Modules = new List<string>();
        public List<string> EqpType = new List<string>();
        public List<string> EqpID = new List<string>();
        public string SelectedContent = "";
        public void GetData()
        {
            Rpt018GroupModel.GetData();
            var owners = Rpt018GroupModel.ReqRpt018EqpStatusEntities.Select(s => s.Owner_ID);
            Modules = ShareDataEntity.GetSingleEntity().FRUserCatcher.GetEntities().EntityList.Where(w => owners.Contains(w.User_ID)).Select(s => s.Department).Distinct().ToList();
            EqpType = Rpt018GroupModel.ReqRpt018EqpStatusEntities.Where(w=>!string.IsNullOrEmpty(w.EQP_Type)).Select(s => s.EQP_Type).Distinct().ToList();
            EqpID = Rpt018GroupModel.ReqRpt018Models.Select(s => s.EqpID).Distinct().ToList();
        }

    }
}