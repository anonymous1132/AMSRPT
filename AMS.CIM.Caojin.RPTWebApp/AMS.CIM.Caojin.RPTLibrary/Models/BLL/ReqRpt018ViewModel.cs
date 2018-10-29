using System;
using System.Collections.Generic;
using System.Linq;


namespace AMS.CIM.Caojin.RPTLibrary.Models
{
    public class ReqRpt018ViewModel
    {
       public ReqRpt018GroupModel Rpt018GroupModel = new ReqRpt018GroupModel();
        public void GetData()
        {
            Rpt018GroupModel.GetData();
            var owners = Rpt018GroupModel.ReqRpt018EqpStatusEntities.Select(s =>new { s.Owner_ID,s.EQP_Type } ).Where(w => !string.IsNullOrEmpty(w.EQP_Type)).Distinct();
            foreach (var owner in owners)
            {
                var Modules = ShareDataEntity.GetSingleEntity().FRUserCatcher.GetEntities().EntityList.Where((w => owner.Owner_ID==w.User_ID)).Select(s => s.Department).Distinct().ToList();
                foreach (string m in Modules)
                {
                    if (!ShareDataEntity.GetSingleEntity().db.EQPType_Department_Mapping.Any(a => a.Department == m && a.EqpType == owner.Owner_ID))
                    {
                        ShareDataEntity.GetSingleEntity().db.EQPType_Department_Mapping.Add(new EQPType_Department_MappingEntity() { Department=m,EqpType=owner.EQP_Type});
                    }
                }
            }


            ShareDataEntity.GetSingleEntity().db.SaveChanges();

        }
    }
}