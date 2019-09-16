using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Reflection;
using System.Threading.Tasks;
using CommonUtilsLibrary.Models;

namespace CommonUtilsLibrary.Utils
{
    public class OracleDataCatcher<T> where T : new()
    {
        public OracleDataCatcher(string tableName,OracleConPara para,string sql="")
        {
            entities = new DBEntities<T>(tableName);
            Para = para;
            _sql = sql;
        }

        public DBEntities<T> entities;

        private OracleConPara Para;

        private string _sql { get; set; }
        private string sql
        {
            get
            {
                if (!string.IsNullOrEmpty(_sql)) return _sql;
                entities.EntityList.Clear();
                T t = new T();
                string columns = string.Join(",", t.GetType().GetProperties().Where(w => w.CanWrite).Select(s => s.Name).ToList());
                return string.Format("select {1} from {0} {2}", entities.TableName, columns, Conditions);
            }
        }

        public string Conditions
        {
            get;
            set;
        }

        private void DBOper()
        {
            OracleUtil util = new OracleUtil(Para);
            DataTable dt= util.ExecuteDataTable(sql);
            foreach (DataRow dr in dt.Rows)
            {
                T t = new T();
                // 获得此模型的公共属性      
                PropertyInfo[] propertys = t.GetType().GetProperties();
                foreach (PropertyInfo pi in propertys)
                {
                    if (dt.Columns.Contains(pi.Name))
                    {
                        if (!pi.CanWrite) continue;
                        object value = dr[pi.Name];
                        if (value != DBNull.Value)
                        {
                            pi.SetValue(t, value, null);
                        }
                    }
                }
                entities.EntityList.Add(t);
            }
        }

        public DBEntities<T> GetEntities()
        {
            DBOper();
            return entities;
        }


    }
}
