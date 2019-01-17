using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caojin.Common;

namespace AMS.CIM.Caojin.RPTLibrary.Models
{
    public class DB2OperDataPusher<T> where T : new()
    {
        /// <summary>
        /// DB2实体集合构造函数
        /// </summary>
        /// <param name="tableName">指定表名</param>
        public DB2OperDataPusher(string tableName,DB2Oper db2)
        {
            entities = new DB2Entities<T>(tableName);
            DB2 = db2;
        }

        public DB2Entities<T> entities;

        private DB2Oper DB2 { get; set; }

        private List<string> Columns { get { return new T().GetType().GetProperties().Where(w => w.CanWrite).Select(s => s.Name).ToList(); } }

        private List<string> GetValues()
        {
            List<string> Val = new List<string>();
            if (entities.EntityList.Count == 0) { throw new Exception("DB2DataPubsher.GetValues():实体数据为空"); }
            foreach (T t in entities.EntityList)
            {
                List<string> list = t.GetType().GetProperties().Where(w => w.CanWrite).Select(s => FormatElement(s.GetValue(t))).ToList();
                Val.Add(string.Join(",", list));
            }
            return Val;
        }

        private string FormatElement(object obj)
        {
            if (obj is string) return string.Format("'{0}'", obj.ToString());
            if (double.TryParse(obj.ToString(), out double res)) return obj.ToString();
            if (obj is DateTime) return string.Format("'{0}'", ((DateTime)obj).ToString("yyyy-MM-dd HH:mm:ss.ffffff"));
            if (obj is null || obj == DBNull.Value) return "Null";
            else
                throw new Exception("DB2DataPubsher.FormatElement()：意外的类型");
        }

        private string sql
        {
            get
            {
                T t = new T();
                string columns = string.Join(",", t.GetType().GetProperties().Where(w => w.CanWrite).Select(s => s.Name).ToList());
                return string.Format("insert into {0} ({1}) values ({2})", entities.TableName, columns, string.Join("),(", GetValues()));
            }
        }

        public void PushEntities()
        {
            DB2.GetSomeData(sql);
        }

    }
}

