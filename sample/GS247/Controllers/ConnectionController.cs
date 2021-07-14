using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GS247.Controllers.Connection
{
    public class Connectivity
    {
        public SelectList TableData(SqlConnection con, String Query, String Param1, String Param2)
        {
            DataTable reader = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(Query, con);
            da.Fill(reader);
            return ToSelectList(reader, Param1, Param2);
        }
        public SelectList ToSelectList(DataTable table, String ValueField, String TextField)
        {
            List<SelectListItem> list = new List<SelectListItem>();
            foreach (DataRow row in table.Rows)
            {
                list.Add(new SelectListItem() { Text = row[TextField].ToString(), Value = row[ValueField].ToString() });
            }
            return new SelectList(list, "Value", "Text");
        }
        public SqlDataReader QueryExecute(SqlConnection con, string Query, List<ParamValues> Param)
        {
            SqlDataReader Execreader = null;
            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandType = CommandType.Text;
            sqlCmd.CommandText = Query;
            sqlCmd.Connection = con;
            for (int i = 0; i < Param.Count; i++)
            {
                sqlCmd.Parameters.AddWithValue(Param[i].Code, Param[i].Value);
            }
            Execreader = sqlCmd.ExecuteReader();
            return Execreader;
        }
        public SqlDataReader QueryExecute(SqlConnection con, string Query)
        {
            SqlDataReader Execreader = null;
            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandType = CommandType.Text;
            sqlCmd.CommandText = Query;
            sqlCmd.Connection = con;
            Execreader = sqlCmd.ExecuteReader();
            return Execreader;
        }
        public String SingleColumn(SqlConnection con, string Query)
        {
            String ret = "";
            SqlDataReader Execreader = null;
            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandType = CommandType.Text;
            sqlCmd.CommandText = Query;
            sqlCmd.Connection = con;
            Execreader = sqlCmd.ExecuteReader();
            if (Execreader.Read())
            {
                ret = Execreader.GetValue(0).ToString();
            }
            return ret;
        }
        public class ParamValues
        {
            public String Code;
            public String Value;
        }
        public List<ParamValues> Parameters { get; set; }
        public List<String> CountryList()
        {
            List<String> CountryList = new List<string>();
            CultureInfo[] CInfoList = CultureInfo.GetCultures(CultureTypes.SpecificCultures);
            foreach (CultureInfo CInfo in CInfoList)
            {
                RegionInfo R = new RegionInfo(CInfo.LCID);
                if (!(CountryList.Contains(R.EnglishName)))
                {
                    CountryList.Add(R.EnglishName);
                }
            }
            CountryList.Sort();
            return CountryList;
        }
        public ParamValues Params(String Code, String Value)
        {
            ParamValues sub = new ParamValues();
            sub.Code = Code;
            sub.Value = Value;
            return sub;
        }
    }


}