using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace GS247.Utilities
{
    public class DataAccess
    {

        private string _connectionString = ConfigurationManager.ConnectionStrings["DBName"].ConnectionString.ToString();
        public string connectionString
        {
            get
            {
                if (string.IsNullOrEmpty(_connectionString))
                {
                    _connectionString = ConfigurationManager.ConnectionStrings["DBName"].ConnectionString.ToString();
                }

                return _connectionString;
            }
            set
            {
                _connectionString = value;
            }
        }

        public DataSet ExecuteSet(string Query,CommandType commandType,List<Params> paramList)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                
                SqlCommand cmd = new SqlCommand(Query, conn);
                cmd.CommandType = commandType;
                if(paramList!=null && paramList.Count > 0)
                {
                    foreach (Params p in paramList)
                    {
                        cmd.Parameters.AddWithValue(p.Name,p.Value);
                    }
                }

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                conn.Open();
                DataSet ds = new DataSet();
                da.Fill(ds);
                conn.Close();
                return ds;
            }
        }

        public DataSet ExecuteSet(string Query,CommandType commandType)
        {
            return ExecuteSet(Query, commandType, null);
        }

        public DataTable Execute(string Query,CommandType commandType, List<Params> paramList)
        {
            return ExecuteSet(Query, commandType, paramList).Tables[0];
        }

        public DataTable Execute(string Query, CommandType commandType)
        {
            return ExecuteSet(Query, commandType, null).Tables[0];
        }


    }

    public class Params
    {
        public string Name;
        public object Value;

        public Params(string name,object value)
        {
            this.Name = name;
            this.Value = value;
        }

    }
}