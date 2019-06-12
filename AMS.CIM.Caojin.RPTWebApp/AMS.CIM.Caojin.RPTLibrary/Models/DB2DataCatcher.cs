using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using Caojin.Common;
using System.Reflection;

namespace AMS.CIM.Caojin.RPTLibrary.Models
{
    public class DB2DataCatcher<T> where T:new()
    {
        /// <summary>
        /// DB2实体集合构造函数
        /// </summary>
        /// <param name="tableName">指定表名</param>
        public DB2DataCatcher(string tableName,string sql="")
        {
            entities = new DB2Entities<T>(tableName);
            _sql = sql;
        }

        public DB2Entities<T> entities;

        private string _sql;
        private string sql
        {
            get
            {
                if (!string.IsNullOrEmpty(_sql)) return _sql;
                entities.EntityList.Clear();
                T t = new T();
                string columns =string.Join(",", t.GetType().GetProperties().Where(w=>w.CanWrite).Select(s => s.Name).ToList()) ;
                return string.Format("select {1} from {0} {2}", entities.TableName,columns,Conditions);
            }
        }

        public string Conditions
        {
            get;
            set;
        }


        private void DB2Oper()
        {
            DB2Helper dB2 = new DB2Helper();
            dB2.GetSomeData(sql);
            foreach (DataRow dr in dB2.dt.Rows)
            {
                T t= new T();
                // 获得此模型的公共属性      
                PropertyInfo[] propertys = t.GetType().GetProperties();
                foreach (PropertyInfo pi in propertys)
                {
                    if (dB2.dt.Columns.Contains(pi.Name))
                    {
                        if (!pi.CanWrite) continue;
                        object value = dr[pi.Name];
                        if (value != DBNull.Value)
                        {
                            pi.SetValue(t,value,null);
                        }
                    }
                }
                entities.EntityList.Add(t);
            }            
        }


        public DB2Entities<T> GetEntities()
        {
            DB2Oper();
            return entities;
        }
    }
}