using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caojin.Common;
using System.Data;
using System.Reflection;

namespace AMS.CIM.Caojin.RPTLibrary.Models
{
    /// <summary>
    /// 功能同DB2DataCatcher,但是设置了DB2的Conn
    /// </summary>
    public class DB2OperDataCatcher<T> where T : new()
    {
        /// <summary>
        /// DB2实体集合构造函数
        /// </summary>
        /// <param name="tableName">指定表名</param>
        public DB2OperDataCatcher(string tableName,DB2Oper dB2)
        {
            this.tableName = tableName;
            DB2 = dB2;
        }


        public DB2Entities<T> entities;

        private string sql
        {
            get
            {
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

        public DB2Oper DB2 { get; set; }

        private string tableName { get; set; }

        private void DB2Operate()
        {

            entities = new DB2Entities<T>(tableName);
            DB2.GetSomeData(sql);
            foreach (DataRow dr in DB2.dt.Rows)
            {
                T t = new T();
                // 获得此模型的公共属性      
                PropertyInfo[] propertys = t.GetType().GetProperties();
                foreach (PropertyInfo pi in propertys)
                {
                    if (DB2.dt.Columns.Contains(pi.Name))
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

        public DB2Entities<T> GetEntities()
        {
            DB2Operate();
            return entities;
        }


    }
}
