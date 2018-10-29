using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AMS.CIM.Caojin.RPTLibrary.Models
{
    public class DB2Entities<T> where T:new()
    {
        public DB2Entities(string tableName)
        {
            TableName = tableName;
        }

        public IList<T> EntityList { get; set; } = new List<T>();

        public string TableName
        {
            get;
            private set;
        }



    }
}