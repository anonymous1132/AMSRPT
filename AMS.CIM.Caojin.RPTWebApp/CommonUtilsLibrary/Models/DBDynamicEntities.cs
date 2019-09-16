using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonUtilsLibrary.Models
{
    public class DBDynamicEntities
    {
        public DBDynamicEntities(string tableName,List<string>columns,string conditions="")
        {
            TableName = tableName;
            Columns = columns;
            Conditions = conditions;
        }

        public string TableName
        {
            get;
            private set;
        }

        public List<string> Columns { get; set; }

        public string Conditions { get; set; }

        public List<dynamic> EntityList { get; set; } = new List<dynamic>();

    }
}
