using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.OleDb;

namespace Caojin.Common
{
    public class DB2Helper
    {
        private readonly string strConn = "Provider=IBMDADB2;Data Source=AMRPTDB;UID=istrpt;PWD=istrpt;";
        public DataTable dt ;

        public void GetSomeData(string strSql)
        {
            using (OleDbConnection conn = new OleDbConnection(strConn))
            {
                OleDbCommand cmd = new OleDbCommand(strSql, conn);
                dt = new DataTable();
                try
                {
                    conn.Open();
                    OleDbDataAdapter adp = new OleDbDataAdapter(cmd);
                    adp.Fill(dt);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }


    }
}
