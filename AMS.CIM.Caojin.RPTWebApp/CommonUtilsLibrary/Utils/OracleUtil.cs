using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using CommonUtilsLibrary.Models;

namespace CommonUtilsLibrary.Utils
{
    public class OracleUtil
    {
        public OracleUtil(OracleConPara para)
        {
            connStr = string.Format("User Id={0};Password={1};Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST={2})(PORT={3})))(CONNECT_DATA=(SERVICE_NAME={4})))", para.UserID,para.Password,para.HostName,para.Port,para.ServiceName);
        }

        private string connStr { get; set; }

        #region 执行SQL语句,返回受影响行数
        public int ExecuteNonQuery(string sql, params OracleParameter[] parameters)
        {
            using (OracleConnection conn = new OracleConnection(connStr))
            {
                conn.Open();
                using (OracleCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = sql;
                    cmd.Parameters.AddRange(parameters);
                    return cmd.ExecuteNonQuery();
                }
            }
        }
        #endregion

        public int ExecuteNonQuery(string sql)
        {
            return ExecuteNonQuery(sql, new OracleParameter());
        }


        #region 执行SQL语句,返回DataTable;只用来执行查询结果比较少的情况
        public  DataTable ExecuteDataTable(string sql, params OracleParameter[] parameters)
        {
            using (OracleConnection conn = new OracleConnection(connStr))
            {
                conn.Open();
                using (OracleCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = sql;
                    cmd.Parameters.AddRange(parameters);
                    OracleDataAdapter adapter = new OracleDataAdapter(cmd);
                    DataTable datatable = new DataTable();
                    adapter.Fill(datatable);
                    return datatable;
                }
            }
        }
        #endregion

        public  DataTable ExecuteDataTable(string sql)
        {
            return ExecuteDataTable(sql, new OracleParameter());
        }

        public int UpdateBatchCommand(List<string> commandStringList)
        {
            using (OracleConnection conn = new OracleConnection(connStr))
            {
                conn.Open();
                OracleTransaction m_OraTrans = conn.BeginTransaction();
                using (OracleCommand cmd = conn.CreateCommand())
                {
                    string tmpStr = "";
                    int influenceRowCount = 0;
                    try
                    {
                        foreach (string commandString in commandStringList)
                        {
                            tmpStr = commandString;
                            cmd.CommandText = tmpStr;
                            influenceRowCount += cmd.ExecuteNonQuery();
                        }
                        m_OraTrans.Commit();
                        return influenceRowCount;
                    }
                    catch (OracleException ex)
                    {
                        m_OraTrans.Rollback();
                        throw ex;
                    }
                }
                   
            }
        }

    }
}
