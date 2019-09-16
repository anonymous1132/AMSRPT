using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonUtilsLibrary.Models;
using System.Data;
using System.Dynamic;

namespace CommonUtilsLibrary.Utils
{
    public class DB2DynamicDataCatcher:OracleDynamicDataCatcher
    {
        public DB2DynamicDataCatcher(string tableName,OracleConPara para,string sql) : base(tableName,para,sql)
        {

        }

        public DB2DynamicDataCatcher(string tableName,List<string>columns,OracleConPara para) : base(tableName, columns,para)
        {

        }

        protected override void DBOper()
        {
            DB2Util util = new DB2Util(Para);
             util.GetSomeData(sql);
            DataTable dt = util.dt;
            foreach (DataRow dr in dt.Rows)
            {
                dynamic obj = new ExpandoObject();
                var dict = (IDictionary<string, object>)obj;
                foreach (DataColumn column in dt.Columns)
                {
                    dict[column.ColumnName] = dr[column.ColumnName];
                }
                entities.EntityList.Add(obj);
            }
        }
    }
}
