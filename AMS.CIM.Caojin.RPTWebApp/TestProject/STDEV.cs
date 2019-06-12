using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProject
{
   public class STDEV
    {
        public static double CalculateStdDev(IEnumerable<double> values)
        {
            double ret = 0;
            if (values.Count() > 0)
            {
                //  计算平均数   
                double avg = values.Average();
                Console.WriteLine("AVG:{0}",avg);
                //  计算各数值与平均数的差值的平方，然后求和 
                double sum = values.Sum(d => Math.Pow(d - avg, 2));
                Console.WriteLine("SUM:{0}",sum);
                //  除以数量，然后开方
                ret = Math.Sqrt(sum / values.Count());
            }
            return ret;
        }


        public static float StDev(float[] arrData) //计算标准偏差
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
