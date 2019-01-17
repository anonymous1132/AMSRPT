using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace Caojin.Common
{
    public class CsvHelper
    {
        /// <summary>
        /// 读取CSV文件通过文本格式
        /// </summary>
        /// <param name="strpath"></param>
        /// <returns></returns>
        public DataTable readCsvTxt(string strpath)
        {
            int intColCount = 0;
            bool blnFlag = true;
            DataTable mydt = new DataTable();

            DataColumn mydc;
            DataRow mydr;

            string strline;
            string[] aryline;

            System.IO.StreamReader mysr = new System.IO.StreamReader(strpath);

            while ((strline = mysr.ReadLine()) != null)
            {
                aryline = strline.Split(',');

                if (blnFlag)
                {
                    blnFlag = false;
                    intColCount = aryline.Length;
                    for (int i = 0; i < aryline.Length; i++)
                    {
                        mydc = new DataColumn(aryline[i]);
                        mydt.Columns.Add(mydc);
                    }
                }

                mydr = mydt.NewRow();
                for (int i = 0; i < intColCount; i++)
                {
                    mydr[i] = aryline[i];
                }
                mydt.Rows.Add(mydr);
            }
            mysr.Close();
            return mydt;
        }

        public DataTable readCsvTxtWithColumnName(string strpath)
        {
            int intColCount = 0;
            string filename =strpath.Substring(strpath.LastIndexOf("\\")+1);
            DataTable mydt = new DataTable(filename);

            DataColumn mydc;
            DataRow mydr;

            string strline;
            string[] aryline;

            System.IO.StreamReader mysr = new System.IO.StreamReader(strpath);
            string line = mysr.ReadLine();
            aryline = line.Split(',');
            intColCount = aryline.Length;

            if (aryline[0] != "Collection Date Time")
            {
                mysr.Close();
                throw new Exception(filename + ":\t格式错误，不包含列Collection Date Time");
            }

            if (aryline.Length < 17)
            {
                mysr.Close();
                throw new Exception(filename + ":\t格式错误，列总数小于17");
            }

            //datacolumn赋值
            for (int i = 0; i < intColCount; i++)
            {
                mydc = new DataColumn(aryline[i]);
                mydt.Columns.Add(mydc);
            }

            while ((strline = mysr.ReadLine()) != null)
            {
                aryline = strline.Split(',');
                if (aryline.Length != intColCount) continue;
                mydr = mydt.NewRow();
                for (int i = 0; i < intColCount; i++)
                {
                    mydr[i] = aryline[i];
                }
                mydt.Rows.Add(mydr);
            }
            mysr.Close();
            return mydt;
        }
    }
}
