using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using static Dapper.SqlMapper;

namespace DataAccessLayer.Dapper
{
    public class DapperCore
    {
        private readonly string connectionString;
        public DapperCore()
        {
            connectionString = "Data Source=<Data-Source>;Initial Catalog=<Database_Name>;User ID=<User_Id>;Password=<pwd>"; 
            //connectionString = System.Environment.GetEnvironmentVariable("connectionString");
        }
        public IDbConnection Connection
        {
            get { return new SqlConnection(connectionString); }
        }
        public IDbConnection ConnectionMaster
        {
            get { return new SqlConnection(connectionString); }
        }        
        public List<T> GetList<T>(DynamicParameters Parm, string SPName)
        {
            List<T> dp = null;
            try
            {
                using (IDbConnection dbConnection = Connection)
                {
                    dp = dbConnection.Query<T>(SPName, Parm, commandType: CommandType.StoredProcedure).ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                Connection.Dispose();
            }
            return dp;
        }
        public List<T> GetList<T>(string SPName)
        {
            List<T> dp = null;
            try
            {
                using (IDbConnection dbConnection = Connection)
                {
                    dp = dbConnection.Query<T>(SPName, commandType: CommandType.StoredProcedure).ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                Connection.Dispose();
            }
            return dp;
        }
        public T GetObject<T>(DynamicParameters Parm, string SPName)
        {
            T dp;
            try
            {
                using (IDbConnection dbConnection = Connection)
                {
                    dp = dbConnection.Query<T>(SPName, Parm, commandType: CommandType.StoredProcedure).FirstOrDefault();
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                Connection.Dispose();
            }
            return dp;
        }
        public T GetObjectMaster<T>(DynamicParameters Parm, string SPName)
        {
            T dp;
            try
            {
                using (IDbConnection dbConnection = ConnectionMaster)
                {
                    dp = dbConnection.Query<T>(SPName, Parm, commandType: CommandType.StoredProcedure).FirstOrDefault();
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                ConnectionMaster.Dispose();
            }
            return dp;
        }
        public T InsertUpdateDelete<T>(DynamicParameters Parm, string SPName)
        {
            T dp;
            try
            {
                using (IDbConnection dbConnection = Connection)
                {
                    dp = dbConnection.ExecuteScalar<T>(SPName, Parm, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                Connection.Dispose();
            }
            return dp;
        }
        public int InsertUpdateDeleteByQuery(string SqlQuery)
        {
            int i = 0;
            try
            {
                using (IDbConnection dbConnection = Connection)
                {
                    i = dbConnection.Execute(SqlQuery);
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                Connection.Dispose();
            }
            return i;
        }        
        public List<T> GetListByQuery<T>(string SqlQuery)
        {
            List<T> dp = null;
            try
            {
                using (IDbConnection dbConnection = Connection)
                {
                    dp = dbConnection.Query<T>(SqlQuery).ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                Connection.Dispose();
            }
            return dp;
        }
        public T GetObjectByQuery<T>(string SqlQuery)
        {
            T dp;
            try
            {
                using (IDbConnection dbConnection = Connection)
                {
                    dp = dbConnection.Query<T>(SqlQuery).FirstOrDefault();
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                Connection.Dispose();
            }
            return dp;
        }        
        public int Execute(string storedProcName, object parameters)
        {
            try
            {
                int i = Connection.Execute(storedProcName, parameters, commandType: CommandType.StoredProcedure);
                return 1;
            }
            catch (Exception ex) 
            {
                throw;
            }
            finally
            {
                Connection.Dispose();
            }
        }
        public DataSet GetDataSetExecute(string storedProcName, object parameters)
        {            
            try
            {
                DataSet dataset = new DataSet();
                var list = Connection.ExecuteReader(storedProcName, parameters, commandType: CommandType.StoredProcedure);
                if(list != null)
                {
                    int i = 0;
                    while (!list.IsClosed)
                    {
                        dataset.Tables.Add("Table" + (i + 1));
                        dataset.EnforceConstraints = false;
                        dataset.Tables[i].Load(list);
                        i++;
                    }
                }
                return dataset;                
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                Connection.Dispose();
            }
        }
        public T ExecuteScalar<T>(string storedProcName, object parameters)
        {
            T dp;
            try
            {
                using (IDbConnection dbConnection = Connection)
                {
                    dp = dbConnection.ExecuteScalar<T>(storedProcName, parameters, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                Connection.Dispose();
            }
            return dp;
        }

        //private DataSet ConvertDataReaderToDataSet(IDataReader list)
        //{
        //    DataSet ds = new DataSet();
        //    int i = 0;
        //    while (!list.IsClosed)
        //    {
        //        ds.Tables.Add("Table" + (i + 1));
        //        ds.EnforceConstraints = false;
        //        ds.Tables[i].Load(list);
        //        i++;
        //    }
        //    return ds;
        //}

        public DataTable GetDataTable(string spName, List<SqlParameter> sp = null)
        {
            try
            {
                SqlCommand SqlC = new SqlCommand();
                SqlC.Connection = (SqlConnection)Connection;
                SqlC.CommandType = CommandType.StoredProcedure;
                SqlC.CommandText = spName;
                if (sp != null)
                {
                    SqlC.Parameters.AddRange(sp.ToArray());
                }

                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(SqlC);
                da.Fill(dt);
                return dt;

            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                Connection.Dispose();
            }
        }
        public DataSet GetDataSet(string spName, List<SqlParameter> sp = null)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlCommand SqlC = new SqlCommand();
                SqlC.Connection = (SqlConnection)Connection;
                SqlC.CommandType = CommandType.StoredProcedure;
                SqlC.CommandText = spName;
                if (sp != null)
                {
                    SqlC.Parameters.AddRange(sp.ToArray());
                }

                SqlDataAdapter da = new SqlDataAdapter(SqlC);
                da.Fill(ds);
                return ds;

            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                Connection.Dispose();
            }
        }
        //public DataSet GetDataSetMaster(string spName, List<SqlParameter> sp = null)
        //{
        //    DataSet ds = new DataSet();
        //    try
        //    {
        //        SqlCommand SqlC = new SqlCommand();
        //        SqlC.Connection = (SqlConnection)ConnectionMaster;
        //        SqlC.CommandType = CommandType.StoredProcedure;
        //        SqlC.CommandText = spName;
        //        if (sp != null)
        //        {
        //            SqlC.Parameters.AddRange(sp.ToArray());
        //        }

        //        SqlDataAdapter da = new SqlDataAdapter(SqlC);
        //        da.Fill(ds);
        //        return ds;

        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //    finally
        //    {
        //        Connection.Dispose();
        //    }
        //}

        public Tuple<IEnumerable<T1>, IEnumerable<T2>> ExecuteQueryMultiple<T1, T2>(string spName, object parameters,
                                        Func<GridReader, IEnumerable<T1>> func1,
                                        Func<GridReader, IEnumerable<T2>> func2)
        {
            var objs = this.GetMultiple(spName, parameters, func1, func2);
            return Tuple.Create(objs[0] as IEnumerable<T1>, objs[1] as IEnumerable<T2>);
        }

        public Tuple<IEnumerable<T1>, IEnumerable<T2>, IEnumerable<T3>> ExecuteQueryMultiple<T1, T2, T3>(string spName, object parameters,
                                        Func<GridReader, IEnumerable<T1>> func1,
                                        Func<GridReader, IEnumerable<T2>> func2,
                                        Func<GridReader, IEnumerable<T3>> func3)
        {
            var objs = this.GetMultiple(spName, parameters, func1, func2, func3);
            return Tuple.Create(objs[0] as IEnumerable<T1>, objs[1] as IEnumerable<T2>, objs[2] as IEnumerable<T3>);
        }

        private List<object> GetMultiple(string spName, object parameters, params Func<GridReader, object>[] readerFuncs)
        {
            var returnResults = new List<object>();

            using (IDbConnection dbConnection = Connection)
            {
                var gridReader = dbConnection.QueryMultiple(spName, parameters, commandType: CommandType.StoredProcedure);
                foreach (var readerFunc in readerFuncs)
                {
                    var obj = readerFunc(gridReader);
                    returnResults.Add(obj);
                }
            }
            return returnResults;
        }

    }
}
