using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AMS.CIM.Caojin.RPTWebApp.Models
{
    public class ReqRpt064DetailModel
    {
        public ReqRpt064DetailCalcEntity ProcessModel { get; set; } = new ReqRpt064DetailCalcEntity();

        public ReqRpt064DetailCalcEntity StaticModel { get; set; } = new ReqRpt064DetailCalcEntity();

        public List<string> TimeList { get; set; }

        public List<string> ValueList { get; set; }

        public ReqRpt064DetailSpcSetEntity SetModel { get; set; } = new ReqRpt064DetailSpcSetEntity();

        public void SetProcessCalcProperty()
        {
            //sigma
            if (ProcessModel.Lcl.HasValue && ProcessModel.Ucl.HasValue) ProcessModel.Sigma = (ProcessModel.Ucl.Value - ProcessModel.Lcl.Value) / 6;
            else if (ProcessModel.Lcl.HasValue) ProcessModel.Sigma = (ProcessModel.Target - ProcessModel.Lcl) / 3;  //如果Target为null会怎样
            else ProcessModel.Sigma = (ProcessModel.Ucl - ProcessModel.Target) / 3;
            //ca
            ProcessModel.Ca = (ProcessModel.Mean - ProcessModel.Target) * 2 / (ProcessModel.Usl - ProcessModel.Lsl);
            //cpk
            var cpk1 = (ProcessModel.Usl - ProcessModel.Mean) / (3 * ProcessModel.Sigma);
            var cpk2 = (ProcessModel.Mean - ProcessModel.Lsl) / (3 * ProcessModel.Sigma);
            ProcessModel.Cpk = cpk1 < cpk2 ? cpk1 : cpk2;
            var cp = (ProcessModel.Usl - ProcessModel.Lsl) / (6 * ProcessModel.Sigma);
            ProcessModel.Cp = cp.HasValue ? cp : ProcessModel.Cpk;
            ProcessModel.Cpkv= ProcessModel.Ca.HasValue ? (1 - Math.Abs(ProcessModel.Ca.Value)) * ProcessModel.Cp : null;
        }

        public void SetStaticCalcProperty()
        {
            StaticModel.Usl = ProcessModel.Usl;
            StaticModel.Lsl = ProcessModel.Lsl;
            StaticModel.Target = ProcessModel.Target;
            StaticModel.Mean = ProcessModel.Mean;
            StaticModel.Sigma =StaticModel.Mean.HasValue? StDev(ValueList.Select(s => Convert.ToDouble(s)).ToArray()):StaticModel.Mean;
            StaticModel.Ucl = ProcessModel.Ucl.HasValue ?StaticModel.Mean+(3*StaticModel.Sigma): ProcessModel.Ucl;
            StaticModel.Lcl = ProcessModel.Lcl.HasValue ? StaticModel.Mean - (3 * StaticModel.Sigma) : ProcessModel.Lcl;
            StaticModel.Ca = (StaticModel.Mean - StaticModel.Target) * 2 / (StaticModel.Usl - StaticModel.Lsl);
            var cpk1 = (StaticModel.Usl - StaticModel.Mean) / (3 * StaticModel.Sigma);
            var cpk2 = (StaticModel.Mean - StaticModel.Lsl) / (3 * StaticModel.Sigma);
            StaticModel.Cpk = cpk1 < cpk2 ? cpk1 : cpk2;
            var cp = (StaticModel.Usl - StaticModel.Lsl) / (6 * StaticModel.Sigma);
            StaticModel.Cp = cp.HasValue ? cp : StaticModel.Cpk;
            StaticModel.Cpkv = StaticModel.Ca.HasValue ? (1 - Math.Abs(StaticModel.Ca.Value)) * StaticModel.Cp : null;
        }

        //计算标准偏差
        private double StDev(double[] arrData)
        {
            double xSum = 0F;
            double xAvg = 0F;
            double sSum = 0F;
            double tmpStDev = 0F;
            int arrNum = arrData.Length;
            for (int i = 0; i < arrNum; i++)
            {
                xSum += arrData[i];
            }
            xAvg = xSum / arrNum;
            for (int j = 0; j < arrNum; j++)
            {
                sSum += ((arrData[j] - xAvg) * (arrData[j] - xAvg));
            }
            tmpStDev = Convert.ToDouble(Math.Sqrt((sSum / (arrNum - 1))).ToString());
            return tmpStDev;
        }
    }
}