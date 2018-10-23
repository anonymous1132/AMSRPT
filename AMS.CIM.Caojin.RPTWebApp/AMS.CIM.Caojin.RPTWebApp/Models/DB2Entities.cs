using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AMS.CIM.Caojin.RPTWebApp.Models
{
    public class DB2Entities<T> where T:new()
    {
        public DB2Entities(string columnName,string tableName)
        {
            CompareTimeColumnName = columnName;
            TableName = tableName;
        }

        public IList<T> EntityList { get; set; } = new List<T>();

        public string CompareTimeColumnName
        {
            get;
            private set;
        }

        public string TableName
        {
            get;
            private set;
        }



    }
}