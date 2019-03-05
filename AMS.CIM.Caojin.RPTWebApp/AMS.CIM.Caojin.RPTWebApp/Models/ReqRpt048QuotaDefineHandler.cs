using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AMS.CIM.Caojin.RPTLibrary.Models;
using Caojin.Common;

namespace AMS.CIM.Caojin.RPTWebApp.Models
{
    public class ReqRpt048QuotaDefineHandler
    {
        public ReqRpt048QuotaDefineHandler(Rpt_Quota_Define quota)
        {
            Quota = quota;
            Initialize();
        }

        private Rpt_Quota_Define Quota { get; set; }
        private DB2DataCatcher<Rpt_Quota_Define> QuotaCatcher = new DB2DataCatcher<Rpt_Quota_Define>("ISTRPT.Rpt_Quota_Define");
        public bool Success { get; set; } = false;
        public string Msg { get; set; }
        private void Initialize()
        {
            if (string.IsNullOrEmpty(Quota.Department)) throw new Exception("Department不能为空");
            QuotaCatcher.Conditions = string.Format("where Project_Desc ='{0}'",Quota.Project_Desc);
            var quota_list = QuotaCatcher.GetEntities().EntityList;
            var quotas = quota_list.Where(w => w.Department == Quota.Department && w.Quota_Type == Quota.Quota_Type);
            //删除：定义Purpose为DEL_FLAG为删除信号
            if (Quota.Purpose == "DEL_FLAG")
            {
                string sql =string.Format( "delete from istrpt.rpt_quota_define where Department='{0}' and Project_Desc='{1}' and Quota_Type={2}",Quota.Department,Quota.Project_Desc,Quota.Quota_Type);
                new DB2Helper().GetSomeData(sql);
                Msg =string.Format( "删除{0}数据",quotas.Count());
                Success = true;
                return;
            }
            //修改
            if (quotas.Count() ==1)
            {
                if (quotas.First().Quota_SHL == Quota.Quota_SHL && quotas.First().Quota_HL == Quota.Quota_HL&& quotas.First().Purpose==Quota.Purpose) { Success = false;Msg = "数据没有变化";return; };
                string sql = string.Format("update istrpt.rpt_quota_define set quota_shl={0} ,quota_hl={1} where department='{2}' and project_desc='{3}' and quota_type={4}",Quota.Quota_SHL,Quota.Quota_HL,Quota.Department,Quota.Project_Desc,Quota.Quota_Type);
                new DB2Helper().GetSomeData(sql);
                Msg = "成功修改一条数据";
                Success = true;
            }
            //新增
            if (quotas.Count() == 0)
            {
                string sql = string.Format("insert into istrpt.rpt_quota_define (department,quota_type,project_desc,purpose,quota_shl,quota_hl) values ('{0}',{1},'{2}','{3}',{4},{5})", Quota.Department, Quota.Quota_Type, Quota.Project_Desc, Quota.Purpose, Quota.Quota_SHL, Quota.Quota_HL);
                new DB2Helper().GetSomeData(sql);
                Msg = "成功新增一条数据";
                Success = true;
            }
        }
    }
}