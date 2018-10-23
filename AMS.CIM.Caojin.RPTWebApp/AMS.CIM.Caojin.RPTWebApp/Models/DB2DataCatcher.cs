using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using Caojin.Common;
using System.Reflection;

namespace AMS.CIM.Caojin.RPTWebApp.Models
{
    public class DB2DataCatcher<T> where T:new()
    {
        /// <summary>
        /// DB2实体集合构造函数
        /// </summary>
        /// <param name="columnName">设置时间列名，用于指定某列作为最后一条记录的依据</param>
        /// <param name="tableName">指定表名</param>
        public DB2DataCatcher(string columnName,string tableName)
        {
            entities = new DB2Entities<T>(columnName,tableName);
            GetData();
        }

        /// <summary>
        /// DB2实体集合构造函数，不设置ColumnName，每次全更新
        /// </summary>
        /// <param name="tableName">指定表名</param>
        public DB2DataCatcher(string tableName,bool autoUpdate=false)
        {
            entities = new DB2Entities<T>("",tableName);
            AutoUpdate = autoUpdate;
            GetData();
        }

        DB2Entities<T> entities;

        public string sql
        {
            get
            {
                string temp = "";
                if (entities.CompareTimeColumnName != "") temp = string.Format("select * from {0} where {1} > '{2}'", entities.TableName, entities.CompareTimeColumnName, LastRecordTime.ToString("yyyy-MM-dd-HH.mm.ss.ffffff"));
                else
                {

                 entities.EntityList.Clear(); temp = string.Format("select * from {0}", entities.TableName);

                }
                return temp;
            }
        }

        public void GetData()
        {
            if (NeedUpdate())
            {
                UpdateTime = DateTime.Now;
                lock(entities.EntityList)
                {
                    DB2Oper();
                }
            }
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

            //部分更新的表需要设定最后的一条记录时间
            if (entities.CompareTimeColumnName != "" &&entities.EntityList.Count>0)
            {
                T t =entities.EntityList.OrderBy(o => o.GetType().GetProperty(entities.CompareTimeColumnName)).LastOrDefault();
                LastRecordTime = (DateTime)t.GetType().GetProperty(entities.CompareTimeColumnName).GetValue(t, null);
            }
            
        }

        private DateTime LastRecordTime = DateTime.MinValue;
   
        private DateTime UpdateTime = DateTime.MinValue;

        private int RecordCount
        {
            get { return entities.EntityList.Count; }
        }

        public bool NeedUpdate()
        {
            if (DateTime.Now - UpdateTime < TimeSpan.FromMinutes(5)) return false;
            if (entities.CompareTimeColumnName != "") return true;
            else if (AutoUpdate) return true;
            else
            {
                DB2Helper dB2 = new DB2Helper();
                dB2.GetSomeData("select count(*) from " + entities.TableName);
                return RecordCount != (int)dB2.dt.DefaultView[0][0];
            }
        }

        private bool AutoUpdate
        {
            get;
            set;
        }

        public DB2Entities<T> GetEntities()
        {
            GetData();
            return entities;
        }
    }
}