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
        private readonly string strConn = "Provider=IBMDADB2;Data Source=AMRPTDB;UID=rptprod;PWD=rptprod;";
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

        #region//事务操作数据库
        /// <summary>
        /// 提交一组（多条）SQL语句操作数据库
        /// </summary>
        /// <param name="commandStringList">SQL列表</param>
        /// <returns>执行结果</returns>
        public int UpdateBatchCommand(List<string> commandStringList)
        {
            using (OleDbConnection conn = new OleDbConnection(strConn))
            {
                OleDbCommand cmd = new OleDbCommand
                {
                    Connection = conn
                };
                int influenceRowCount = 0;
                string tmpStr = "";
                conn.Open();
                OleDbTransaction m_Trans = conn.BeginTransaction();//创建事务对象
                try
                {
                    foreach (string commandString in commandStringList)
                    {
                        tmpStr = commandString;
                        cmd.CommandText = tmpStr;
                        cmd.Transaction = m_Trans;
                        influenceRowCount += cmd.ExecuteNonQuery();
                    }
                    m_Trans.Commit();
                    return influenceRowCount;
                }
                catch (OleDbException ex)
                {
                    m_Trans.Rollback();
                    throw ex;
                }
            }
        }
        #endregion
    }
}
