using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Threading.Tasks;
using System.Reflection;
using System.Dynamic;
using CommonUtilsLibrary.Models;

namespace CommonUtilsLibrary.Utils
{
    public class OracleDynamicDataCatcher
    {
        public OracleDynamicDataCatcher(string tableName,List<string>columns, OracleConPara para)
        {
            entities = new DBDynamicEntities(tableName,columns);
            Para = para;
        }

        public OracleDynamicDataCatcher(string tableName, OracleConPara para,string sql)
        {
            entities = new DBDynamicEntities(tableName, new List<string>());
            Para = para;
            _sql = sql;
        }

        public DBDynamicEntities entities;

        protected readonly OracleConPara Para;

        private string _sql { get; set; }
        protected string sql
        {
            get
            {
                if (!string.IsNullOrEmpty(_sql)) return _sql;
                entities.EntityList.Clear();
                return string.Format("select {1} from {0} {2}", entities.TableName, string.Join("','", entities.Columns) , entities.Conditions);
            }
        }

        protected virtual void DBOper()
        {
            OracleUtil util = new OracleUtil(Para);
            DataTable dt = util.ExecuteDataTable(sql);
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


        public DBDynamicEntities GetEntities()
        {
            DBOper();
            return entities;
        }
    }
}
