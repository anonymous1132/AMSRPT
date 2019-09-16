using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonUtilsLibrary.Models
{
    public class DBEntities<T> where T:new()
    {
        public DBEntities(string tableName)
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
