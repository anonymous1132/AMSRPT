using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Caojin.Common;
using AMS.CIM.Caojin.RPTLibrary.Models;

namespace AMS.CIM.Caojin.RPTWebApp.Models
{
    public class ReqRpt020SetTargetViewModel
    {
        public ReqRpt020SetTargetViewModel(List<ReqRpt020SetTargetPostModel> postModel)
        {
            PostModel = postModel;
            Initialize();
        }

        public string Message {  get; private set; }

        private List<ReqRpt020SetTargetPostModel> PostModel { get; set; }

        private DB2DataCatcher<RPT_EQP_PERFM_TARGET_EQPTYPE> TargetCatcher { get; set; } = new DB2DataCatcher<RPT_EQP_PERFM_TARGET_EQPTYPE>("ISTRPT.RPT_EQP_PERFM_TARGET");

        DB2DataPusher<RPT_EQP_PERFM_TARGET> TargetPusher { get; set; } = new DB2DataPusher<RPT_EQP_PERFM_TARGET>("ISTRPT.RPT_EQP_PERFM_TARGET");
        //主工作进程
        private void Initialize()
        {
            //判断postmodel
            if (PostModel == null || PostModel.Count == 0) { Message = "上传的数据非预期合法数据！"; return; }
            var list_eqpType = PostModel.Where(w =>  IsNatural_Number(w.eqpType)).Select(s => s.eqpType.Trim()).Distinct();
            if(list_eqpType==null||list_eqpType.Count()==0) { Message = "上传的数据非预期合法数据！"; return; }

            var list_eqpType_db = TargetCatcher.GetEntities().EntityList;
            //如果db中的表非空
            if ((list_eqpType_db != null) && list_eqpType_db.Count() > 0)
            {
                //交集
                var list_eqpType_update = list_eqpType.Intersect(list_eqpType_db.Select(s => s.EQP_TYPE));
                var updateSql = list_eqpType_update.Select(s => PostModel.Where(w => w.eqpType == s).FirstOrDefault()).Select(s => string.Format("update ISTRPT.RPT_EQP_PERFM_TARGET set upm_target={0},uum_target={1},passqty_target={2},rework_target={3},eff_target={4},wph={5} where eqp_type='{6}'", s.upm, s.uum, s.passqty, s.rework, s.eff, s.throughput, s.eqpType));

                //差集
                var list_eqpType_insert = list_eqpType.Except(list_eqpType_db.Select(s => s.EQP_TYPE));
                InsertData(list_eqpType_insert);
                if(list_eqpType_insert.Count()>0) Message += string.Format("{0}数据插入成功<br />", string.Join("、", list_eqpType_insert));
                DB2Helper db2 = new DB2Helper();
                db2.UpdateBatchCommand(updateSql.ToList());
                if (list_eqpType_update.Count() > 0) Message += string.Format("{0}数据更新成功<br />", string.Join("、", list_eqpType_update));
            }
            else //db为空则直接插入
            {
                InsertData(list_eqpType);
                Message += string.Format("{0}数据插入成功<br />", string.Join("、", list_eqpType));
            }

            
        }

        private double FixStringToDouble(string s)
        {
            try
            {
                return s.Substring(s.Length - 1) == "%" ? double.Parse(s.TrimEnd('%')) / 100 : double.Parse(s);
            }
            catch (Exception)
            {
                throw new Exception("数据格式存在错误，Target数据必须为数字类型，百分号将会被转化为小数保存至db。");
            }
        }

        //判断是否为英文+数字
        private bool IsNatural_Number(string str)
        {
            if (string.IsNullOrEmpty(str)) return false;
            if (str.ToUpper() == "EQPTYPE") return false;
            System.Text.RegularExpressions.Regex reg1 = new System.Text.RegularExpressions.Regex(@"^[A-Za-z0-9\-_]+$");
            return reg1.IsMatch(str);
        }

        //InsertData
        private void InsertData(IEnumerable<string>list_eqptype)
        {
            foreach (var item in list_eqptype)
            {
                var model = PostModel.Where(w => w.eqpType == item).First();
                double upm = FixStringToDouble(model.upm);
                double uum = FixStringToDouble(model.uum);
                double passqty = double.Parse(model.passqty);
                double rework = FixStringToDouble(model.rework);
                double eff = double.Parse(model.eff);
                double wph = FixStringToDouble(model.throughput);
                TargetPusher.entities.EntityList.Add(new RPT_EQP_PERFM_TARGET() { Eqp_Type = item, Uum_Target = uum, Upm_Target = upm, PassQty_Target = passqty, Eff_Target = eff, Rework_Target = rework, Wph = wph });
                TargetPusher.PushEntities();
            }
        }

    }
}