using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonUtilsLibrary.Models
{
    public class OracleConPara
    {
        public string UserID { get; set; }

        public string Password { get; set; }

        public string HostName { get; set; }

        public int Port { get; set; } = 1521;

        public string ServiceName { get; set; }
    }
}
