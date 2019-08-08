using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMS.CIM.Caojin.RPTLibrary.Models
{
    public class Report64_SPCModel
    {
        public int Gno { get; set; }

        public int Cno { get; set; }

        public int Ctype { get; set; }

        public string Gname { get; set; }

        public string Dept_ID { get; set; }

        public string Eqp_ID { get; set; }

        public string Product_ID { get; set; }

        public string Ctitle { get; set; }
        
        public double? Usl_Value { get; set; }

        public double? Ucl_Value { get; set; }

        public double? Target_Value { get; set; }

        public double? Lsl_Value { get; set; }

        public double? Lcl_Value { get; set; }

        public double? Mean_Value { get; set; }

        public string Value_List { get; set; }

        public string GetUsl()
        {
            return GetDouleToString(Usl_Value);
        }

        public string GetUcl()
        {

            string temp= GetDouleToString(Ucl_Value);
            if (temp == "") return "";
            return  ((Mean_Value ?? 0)+3*(GetSigma()??0)).ToString("0.000");
        }

        public string GetTarget()
        {
            return GetDouleToString(Target_Value);
        }

        public string GetLsl()
        {
            return GetDouleToString(Lsl_Value);

        }

        public string GetLcl()
        {
            string temp = GetDouleToString(Lcl_Value);
            if (temp == "") return "";
            return ((Mean_Value ?? 0) - 3 * (GetSigma() ?? 0)).ToString("0.000");
        }

        public string GetMean()
        {
          return  GetDouleToString(Mean_Value);
        }

        public float? GetSigma()
        {
            if (!Mean_Value.HasValue) return null;
            var fList = Value_List.Split('|').Select(s => Convert.ToSingle(s)).ToArray();
            return  StDev(fList);
        }

        public double? GetCa()
        {
            if (GetUsl() == "" || GetLsl() == "" ||GetTarget()=="") return null;
            return (Mean_Value - Target_Value) * 2 / (Usl_Value - Lsl_Value);
        }

        public double? GetCp()
        {
            if (GetUsl() == "" || GetLsl() == "" ) return null;
            return (Usl_Value - Lsl_Value) / 6 * GetSigma();
        }

        public double? GetCpk()
        {
            var usl = Usl_Value == 1E-37 ? null : Usl_Value;
            var lsl = Lsl_Value == 1E-37 ? null : Lsl_Value;
            var temp1 = (usl - Mean_Value) / (3 * GetSigma());
            var temp2 = (Mean_Value - lsl) / (3 * GetSigma());
            if (temp1.HasValue && temp2.HasValue) return temp1.Value > temp2.Value ? temp2.Value : temp1.Value;
            if (temp1.HasValue) return temp1.Value;
            if (temp2.HasValue) return temp2.Value;
            return null;
        }

        public string GetStrSigma()
        {
            return GetDouleToString(GetSigma());
        }

        public string GetStrCp()
        {
            return GetDouleToString(GetCp());
        }

        public string GetStrCpk()
        {
            return GetDouleToString(GetCpk());
        }

        public string GetStrCa()
        {
            return GetDouleToString(GetCa());
        }


        //保留2位小数
        private string GetDouleToString(double? dValue)
        {
            if (!Mean_Value.HasValue) return "";
            if (dValue.HasValue)
            {
                if (dValue.Value == 1E-37)
                {
                    return "";
                }
                else
                {
                    return (dValue??0).ToString("0.000");
                }
            }
            return "";
        }
        //计算标准偏差
        private float StDev(float[] arrData)
        {
            float xSum = 0F;
            float xAvg = 0F;
            float sSum = 0F;
            float tmpStDev = 0F;
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
            tmpStDev = Convert.ToSingle(Math.Sqrt((sSum / (arrNum - 1))).ToString());
            return tmpStDev;
        }

    }
}
