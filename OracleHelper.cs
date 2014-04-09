using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.OracleClient;
using System.IO;
using System.Configuration;

namespace WEB_Backgroundservice
{
    public abstract class DbHelperOra
    {
        //数据库连接对像   
        private OracleConnection conn = null;
        //数据库命令对像   
        private OracleCommand cmd = new OracleCommand();
        //DataAdapter对像   
        private OracleDataAdapter adapter = new OracleDataAdapter();

        private OracleParameter parameter = new OracleParameter();

        private OracleDataReader reader = null;

        private OracleTransaction trans = null;

        /// <summary>   
        /// 该实例使用的数据库连接字符串   
        /// </summary>   
        private static string connectionString = ConfigurationManager.ConnectionStrings["OraConnString"].ConnectionString;


        /// <summary>   
        /// 数据库类型   
        /// </summary>   
        public System.Data.DbType DatabaseType
        {
            get
            {
                return new System.Data.DbType();
            }
        }

        /// <summary>   
        /// 数据库DataReader对像   
        /// </summary>   
        public System.Data.IDataReader DataReader
        {
            get
            {
                return this.reader;
            }
            set
            {
                reader = (OracleDataReader)value;
            }
        }

        public System.Data.IDbConnection DbConnection
        {
            get
            {
                if (conn == null)
                {
                    conn = new OracleConnection(connectionString);
                }
                return conn;
            }
        }

        public System.Data.IDbCommand DataCommand
        {
            get
            {
                return this.cmd;
            }
            set
            {
                cmd = (OracleCommand)value;
            }
        }

        /// <summary>   
        /// OracleDataAdapter   
        /// </summary>   
        public System.Data.IDbDataAdapter DataAdapter
        {
            get
            {
                if (adapter != null)
                {
                    return adapter;
                }
                else
                {
                    adapter = new OracleDataAdapter();
                    return adapter;
                }
            }
        }

        /// <summary>   
        /// 打开数据库连接   
        /// </summary>   
        public void Open()
        {
            if (connectionString == "")
            {
                string UserId = "cyy";
                string Password = "cyy";
                string DataSource = "orcl";
                connectionString = "Data Source = " + DataSource + ";User ID=" + UserId + ";Password=" + Password;
            }

            if (conn == null)
            {
                conn = new OracleConnection(connectionString);
                conn.Open();
            }
            else
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();
            }
        }

        /// <summary>   
        /// 关闭数据库连接   
        /// </summary>   
        public void Close()
        {
            if (conn != null && conn.State == ConnectionState.Open)
            {
                conn.Close();
            }
        }

        /// <summary>   
        /// 开始执行数据库事务   
        /// </summary>   
        /// <returns></returns>   
        public System.Data.IDbTransaction BeginTransaction()
        {
            Open();
            trans = conn.BeginTransaction();
            return trans;
        }

        /// <summary>   
        /// 开始数据库连接   
        /// </summary>   
        /// <param name="isolationLevel"></param>   
        /// <returns></returns>   
        public System.Data.IDbTransaction BeginTransaction(IsolationLevel isolationLevel)
        {
            Open();

            trans = conn.BeginTransaction(isolationLevel);

            return trans;
        }

        /// <summary>   
        /// 执行无返回值的操作   
        /// </summary>   
        /// <param name="commandText">数据操作字符串</param>   
        /// <returns>返回影响的行数</returns>   
        public int ExecuteNonQuery(string commandText)
        {
            Open();
            int rValue = 0;
            cmd.Connection = conn;
            try
            {
                cmd.CommandText = commandText;
                cmd.CommandType = CommandType.Text;
                rValue = cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                this.Close();
                throw e;
            }
            return rValue;
        }

        private static void PrepareCommand(OracleCommand cmd, OracleConnection conn, OracleTransaction trans, string cmdText, OracleParameter[] cmdParms)
        {
            if (conn.State != ConnectionState.Open)
                conn.Open();
            cmd.Connection = conn;
            cmd.CommandText = cmdText;
            if (trans != null)
                cmd.Transaction = trans;
            cmd.CommandType = CommandType.Text;//cmdType;
            if (cmdParms != null)
            {
                foreach (OracleParameter parm in cmdParms)
                    cmd.Parameters.Add(parm);
            }
        }

        /// <summary>   
        /// 执行无返回值的数据操作命令   
        /// </summary>   
        /// <returns>返回影响的行数</returns>   
        public int ExecuteNonQuery()
        {
            Open();
            int rValue = 0;
            cmd.Connection = conn;
            try
            {
                rValue = cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                this.Close();
                throw e;
            }
            return rValue;
        }

        /// <summary>   
        /// 执行数据库操作命令   
        /// </summary>   
        /// <param name="commandText">数数据库接操命令</param>   
        /// <param name="trans">事务对像</param>   
        /// <returns>受影响的行数</returns>   
        public int ExecuteNonQuery(string commandText, System.Data.IDbTransaction trans)
        {
            int rValue = 0;
            cmd.Connection = conn;
            cmd.Transaction = (OracleTransaction)trans;
            try
            {
                cmd.CommandText = commandText;
                cmd.CommandType = CommandType.Text;
                rValue = cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                trans.Rollback();
                this.Close();
                throw e;
            }
            return rValue;
        }

        /// <summary>   
        /// 更新整个DataTable   
        /// </summary>   
        /// <param name="table">要更新的DataTable</param>   
        /// <param name="tableName">更新的表名</param>   
        /// <returns></returns>   
        public int UpdateDataTable(DataTable table, string tableName)
        {
            int rValue = 0;
            try
            {
                this.Open();
                cmd.Connection = this.conn;
                cmd.CommandText = "select * from " + tableName;
                OracleDataAdapter da = new OracleDataAdapter(cmd);
                OracleCommandBuilder cmdBuilder = new OracleCommandBuilder(da);
                da.UpdateCommand = cmdBuilder.GetUpdateCommand();

                rValue = da.Update(table);
                da.Dispose();
            }
            catch (OracleException ex)
            {
                this.Close();
                throw ex;
            }

            return rValue;
        }

        /// <summary>   
        /// 插入整个DataTable   
        /// </summary>   
        /// <param name="table">要插入的DataTable</param>   
        /// <param name="tableName">插入的表名</param>   
        /// <returns></returns>   
        public int InsertDataTable(DataTable table, string tableName)
        {
            int rValue = 0;
            try
            {
                this.Open();
                cmd.Connection = this.conn;
                cmd.CommandText = "select * from " + tableName;
                OracleDataAdapter da = new OracleDataAdapter(cmd);
                OracleCommandBuilder cmdBuilder = new OracleCommandBuilder(da);
                da.InsertCommand = cmdBuilder.GetInsertCommand();
                rValue = da.Update(table);
                da.Dispose();
            }
            catch (OracleException ex)
            {
                this.Close();
                throw ex;
            }

            return rValue;
        }

        /// <summary>   
        /// 执着行数据库操作命令   
        /// </summary>   
        /// <returns>返回单个操作结果</returns>   
        public object ExecuteScalar()
        {
            try
            {
                this.Open();
                cmd.Connection = conn;
                return cmd.ExecuteScalar();
            }
            catch (OracleException ex)
            {
                this.Close();
                throw ex;
            }
        }

        /// <summary>   
        /// 执着行数据库操作命令   
        /// </summary>   
        /// <param name="commandText">据库操作命令字符串</param>   
        /// <returns>返回单个操作结果</returns>   
        public object ExecuteScalar(string commandText)
        {
            OracleCommand command = new OracleCommand(commandText);
            object obj = null;
            try
            {
                this.Open();
                command.Connection = this.conn;
                obj = command.ExecuteScalar();
            }
            catch (OracleException ex)
            {
                this.Close();
                throw ex;
            }
            finally
            {
                command.Dispose();
            }

            return obj;
        }

        /// <summary>   
        /// 执行数据库操作命令   
        /// </summary>   
        /// <param name="commandText">据库操作命令字符串</param>   
        /// <param name="aTableName">填充数据集中的表名</param>   
        /// <returns>执行结果数据集</returns>   
        public System.Data.DataSet ExecuteDataSet(string commandText, string aTableName)
        {
            Open();
            DataSet ds = new DataSet();
            cmd.CommandText = commandText;
            adapter.SelectCommand = cmd;
            adapter.SelectCommand.Connection = conn;
            adapter.Fill(ds, aTableName);
            return ds;
        }

        /// <summary>   
        /// 根据DataCommand执行命令   
        /// 调用之前需将DataCommand赋值   
        /// </summary>   
        /// <param name="aTableName"></param>   
        /// <returns>成功时返回DataSet</returns>   
        public System.Data.DataSet ExecuteDataSet(string aTableName)
        {
            DataSet ds = new DataSet();

            this.Open();

            adapter.SelectCommand = cmd;
            adapter.SelectCommand.Connection = conn;
            adapter.Fill(ds, aTableName);

            return ds;
        }

        /// <summary>   
        /// 执行数据库操作命令   
        /// </summary>   
        /// <param name="commandText">据库操作命令字符串</param>   
        /// <returns>返回数据读取器DataReader</returns>   
        public System.Data.IDataReader ExecuteReader(string commandText)
        {
            Open();
            cmd.Connection = this.conn;
            cmd.CommandText = commandText;
            reader = cmd.ExecuteReader();
            return reader;
        }

        /// <summary>   
        /// 返回指定sql语句的DataTable   
        /// </summary>   
        /// <param name="commandText"></param>   
        /// <returns></returns>   
        public static DataSet Query(string commandText, params OracleParameter[] cmdParms)
        {
            using (OracleConnection connection = new OracleConnection(connectionString))
            {
                OracleCommand cmd = new OracleCommand();
                PrepareCommand(cmd, connection, null, commandText, cmdParms);
                using (OracleDataAdapter da = new OracleDataAdapter(cmd))
                {
                    DataSet ds = new DataSet();
                    try
                    {
                        da.Fill(ds, "ds");
                        cmd.Parameters.Clear();
                    }
                    catch (OracleException ex)
                    {
                        throw new Exception(ex.Message);
                    }
                    finally
                    {
                        cmd.Dispose();
                        connection.Close();
                    }
                    return ds;
                }
            }
        }


        /// <summary>   
        /// 关闭数据读取器    
        /// </summary>   
        public void ReaderClose()
        {
            reader.Close();
        }

        /// <summary>   
        /// 回滚事务   
        /// </summary>   
        public void RollBack()
        {
            this.trans.Rollback();
        }


        /// <summary>   
        /// 写入Blob字段   
        /// </summary>   
        /// <param name="commandText">sql语句，执行结果为BLOB数据</param>   
        /// <param name="DocumentAddress">本地文档的路径</param>   
        public void WriteBlob(string commandText, string DocumentAddress)
        {
            try
            {
                Open();
                cmd.Connection = conn;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = commandText;
                // 利用事务处理（必须）   
                OracleTransaction transaction = cmd.Connection.BeginTransaction();
                cmd.Transaction = transaction;

                reader = cmd.ExecuteReader();
                using (reader)
                {
                    //Obtain the first row of data.   
                    reader.Read();
                    OracleLob BLOB = reader.GetOracleLob(0);
                    //Perform any desired operations on the LOB, (read, position, and so on).   
                    //...   
                    //Example - Writing binary data (directly to the backend).   
                    //To write, you can use any of the stream classes, or write raw binary data using    
                    //the OracleLob write method. Writing character vs. binary is the same;   
                    //however note that character is always in terms of Unicode byte counts   
                    FileStream DataStream = new FileStream(DocumentAddress, FileMode.Open);
                    BLOB.BeginBatch(OracleLobOpenMode.ReadWrite);
                    int length = 30485760;
                    byte[] Buffer = new byte[length];
                    int i;
                    while ((i = DataStream.Read(Buffer, 0, length)) > 0)
                    {
                        BLOB.Write(Buffer, 0, i);
                    }
                    DataStream.Close();
                    BLOB.EndBatch();

                    //Commit the transaction now that everything succeeded.   
                    //Note: On error, Transaction.Dispose is called (from the using statement)   
                    //and will automatically roll-back the pending transaction.   
                    cmd.Transaction.Commit();
                }
            }
            catch (OracleException e)
            {
                cmd.Transaction.Rollback();
                throw e;
            }
            finally
            {
                Close();
            }
        }


        /// <summary>   
        /// 读出Blob字段   
        /// </summary>   
        /// <param name="commandText">sql语句，执行结果为BLOB数据</param>   
        /// <param name="DocumentAddress">将要把BLOB数据保存为的文档的路径</param>   
        public void ReadBlob(string commandText, string DocumentAddress)
        {
            try
            {
                Open();
                cmd.Connection = conn;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = commandText;

                // 利用事务处理（必须）   
                OracleTransaction transaction = cmd.Connection.BeginTransaction();
                cmd.Transaction = transaction;

                reader = cmd.ExecuteReader();
                reader.Read();
                OracleLob BLOB = reader.GetOracleLob(0);
                reader.Close();

                FileStream DataStream = new FileStream(DocumentAddress, FileMode.Create);
                int length = 30485760;
                byte[] Buffer = new byte[length];
                int i;
                while ((i = BLOB.Read(Buffer, 0, length)) > 0)
                {
                    DataStream.Write(Buffer, 0, i);
                }
                DataStream.Close();
                BLOB.Clone();

                cmd.Transaction.Commit();
            }
            catch (OracleException e)
            {
                cmd.Transaction.Rollback();
                throw e;
            }
            finally
            {
                Close();
            }
        }
    }
}