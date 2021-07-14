using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using System.Net;
using System.Net.Cache;
using System.IO;
using System.Text;


/// <summary>
/// Summary description for IES_Generic_Function
/// </summary>
public class IES_Generic_Function
{
    SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conn"].ConnectionString);
    public Dictionary<string, string> ies_parameters = new Dictionary<string, string>();
    public IES_Generic_Function()
    {

        try
        {


            con = new SqlConnection(ConfigurationManager.ConnectionStrings["conn"].ConnectionString);

        }
        catch
        {
            HttpContext.Current.Response.Redirect("login.aspx");
        }
        //
        // TODO: Add constructor logic here
        //
    }
    //Only Feedback form
    public IES_Generic_Function(string mode)
    {

        try
        {

            con = new SqlConnection(ConfigurationManager.ConnectionStrings["conn"].ConnectionString);
        }
        catch
        {
            HttpContext.Current.Response.Redirect("login.aspx");
        }
        //
        // TODO: Add constructor logic here
        //
    }
    public string Get_connection()
    {
        try
        {


            return ConfigurationManager.ConnectionStrings["conn"].ConnectionString;

        }
        catch
        {
            return "";
        }
    }
    public void Fn_NonExecutiveSql(string sql_text, int is_type, int is_parameter)
    {
        try
        {
            if (con.State == ConnectionState.Closed) con.Open();
            SqlCommand cmd = new SqlCommand(sql_text, con);
            if (is_type == 1) cmd.CommandType = CommandType.StoredProcedure;
            else cmd.CommandType = CommandType.Text;
            if (is_parameter == 1)
            {
                foreach (var is_params in ies_parameters)
                {
                    cmd.Parameters.AddWithValue(is_params.Key, is_params.Value);
                }
            }
            cmd.ExecuteNonQuery();
            cmd.Connection.Close();
            con.Close();
        }
        catch
        {
        }
        finally
        {
            if (con != null && con.State == ConnectionState.Open) con.Close();
            if (con != null) con = null;
        }
    }


    public string Fn_GetScalarValue(string sql_text, int is_type, int is_parameter)
    {
        string str_getvalue = "";
        try
        {
            if (con.State == ConnectionState.Closed) con.Open();
            SqlCommand cmd = new SqlCommand(sql_text, con);
            if (is_type == 1) cmd.CommandType = CommandType.StoredProcedure;
            else cmd.CommandType = CommandType.Text;
            if (is_parameter == 1)
            {
                foreach (var is_params in ies_parameters)
                {
                    cmd.Parameters.AddWithValue(is_params.Key, is_params.Value);
                }
            }
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            if (dt != null)
            {
                if (dt.Rows.Count > 0) str_getvalue = Convert.ToString(dt.Rows[0][0]);
            }
            da.SelectCommand.Connection.Close();
            cmd.Connection.Close();

        }
        catch
        {
        }
        finally
        {
            if (con != null && con.State == ConnectionState.Open) con.Close();
            if (con != null) con = null;
        }
        return str_getvalue;
    }
    public DataTable Fn_ExecutiveSql_Datatable(string sql_text, int is_type, int is_parameter)
    {
        DataTable dt = new DataTable();
        try
        {
            if (con.State == ConnectionState.Closed) con.Open();
            SqlCommand cmd = new SqlCommand(sql_text, con);
            if (is_type == 1) cmd.CommandType = CommandType.StoredProcedure;
            else cmd.CommandType = CommandType.Text;

            if (is_parameter == 1)
            {
                foreach (var is_params in ies_parameters)
                {
                    cmd.Parameters.AddWithValue(is_params.Key, is_params.Value);
                }
            }

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            da.SelectCommand.Connection.Close();
            cmd.Connection.Close();


        }
        catch (Exception ex)
        {
        }
        finally
        {
            if (con != null && con.State == ConnectionState.Open) con.Close();
            if (con != null) con = null;
        }
        return dt;
    }
    public DataSet Fn_ExecuteDataSet(string sql_text, int is_type, int is_parameter)
    {
        DataSet dt = new DataSet();
        try
        {
            if (con.State == ConnectionState.Closed) con.Open();
            SqlCommand cmd = new SqlCommand(sql_text, con);
            if (is_type == 1) cmd.CommandType = CommandType.StoredProcedure;
            else cmd.CommandType = CommandType.Text;

            if (is_parameter == 1)
            {
                foreach (var is_params in ies_parameters)
                {
                    cmd.Parameters.AddWithValue(is_params.Key, is_params.Value);
                }
            }

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            da.SelectCommand.Connection.Close();
            cmd.Connection.Close();


        }
        catch
        {
        }
        finally
        {
            if (con != null && con.State == ConnectionState.Open) con.Close();
            if (con != null) con = null;
        }
        return dt;
    }


    public string Fn_ExecutiveSql(string sql_text, int is_type, int is_parameter)
    {
        string returnvalue = string.Empty;
        try
        {
            if (con.State == ConnectionState.Closed) con.Open();
            SqlCommand cmd = new SqlCommand(sql_text, con);

            if (is_type == 1) cmd.CommandType = CommandType.StoredProcedure;
            else cmd.CommandType = CommandType.Text;

            if (is_parameter == 1)
            {
                foreach (var is_params in ies_parameters)
                {
                    cmd.Parameters.AddWithValue(is_params.Key, is_params.Value);
                }
            }
            returnvalue = Convert.ToString(cmd.ExecuteScalar());
            cmd.Connection.Close();

        }
        catch (Exception ex)
        {
        }
        finally
        {
            if (con != null && con.State == ConnectionState.Open) con.Close();
            if (con != null) con = null;
        }
        return returnvalue;
    }

    public void Fn_Executive_List(DropDownList drp_list, string sql_text, string valuetype, string texttype)
    {
        string returnvalue = string.Empty;
        try
        {
            if (con.State == ConnectionState.Closed) con.Open();
            SqlCommand cmd = new SqlCommand(sql_text, con);
            DataTable dt_list = new DataTable();
            SqlDataAdapter da_list = new SqlDataAdapter(cmd);
            da_list.Fill(dt_list);
            drp_list.DataSource = dt_list;
            drp_list.DataTextField = texttype;
            drp_list.DataValueField = valuetype;
            drp_list.DataBind();
            cmd.Connection.Close();

        }
        catch
        {
        }
        finally
        {
            if (con != null && con.State == ConnectionState.Open) con.Close();
            if (con != null) con = null;
        }

    }
    public void Fn_Executive_Listbox(ListBox drp_list, string sql_text, string valuetype, string texttype)
    {
        string returnvalue = string.Empty;
        try
        {
            if (con.State == ConnectionState.Closed) con.Open();
            SqlCommand cmd = new SqlCommand(sql_text, con);
            DataTable dt_list = new DataTable();
            SqlDataAdapter da_list = new SqlDataAdapter(cmd);
            da_list.Fill(dt_list);
            drp_list.DataSource = dt_list;
            drp_list.DataTextField = texttype;
            drp_list.DataValueField = valuetype;
            drp_list.DataBind();
            cmd.Connection.Close();
        }
        catch
        {
        }
        finally
        {
            if (con != null && con.State == ConnectionState.Open) con.Close();
            if (con != null) con = null;
        }

    }

    public void Fn_Executive_ListWithSelect(DropDownList drp_list, string sql_text, string valuetype, string texttype)
    {
        string returnvalue = string.Empty;
        try
        {
            if (con.State == ConnectionState.Closed) con.Open();
            SqlCommand cmd = new SqlCommand(sql_text, con);
            DataTable dt_list = new DataTable();
            SqlDataAdapter da_list = new SqlDataAdapter(cmd);
            da_list.Fill(dt_list);

            DataRow newdata = dt_list.NewRow();
            newdata[valuetype] = "0";
            newdata[texttype] = "--Select--";
            dt_list.Rows.InsertAt(newdata, 0);

            drp_list.DataSource = dt_list;
            drp_list.DataTextField = texttype;
            drp_list.DataValueField = valuetype;
            drp_list.DataBind();
            cmd.Connection.Close();

        }
        catch
        {
        }
        finally
        {
            if (con != null && con.State == ConnectionState.Open) con.Close();

        }
    }
    public void Fn_Executive_ListWithSelect_WithTitle(DropDownList drp_list, string title, string sql_text, string valuetype, string texttype)
    {
        string returnvalue = string.Empty;
        try
        {
            if (con.State == ConnectionState.Closed) con.Open();
            SqlCommand cmd = new SqlCommand(sql_text, con);
            DataTable dt_list = new DataTable();
            SqlDataAdapter da_list = new SqlDataAdapter(cmd);
            da_list.Fill(dt_list);

            DataRow newdata = dt_list.NewRow();
            newdata[valuetype] = "0";
            newdata[texttype] = title;
            dt_list.Rows.InsertAt(newdata, 0);

            drp_list.DataSource = dt_list;
            drp_list.DataTextField = texttype;
            drp_list.DataValueField = valuetype;
            drp_list.DataBind();
            cmd.Connection.Close();

        }
        catch
        {
        }
        finally
        {
            if (con != null && con.State == ConnectionState.Open) con.Close();

        }
    }
    public void Fn_Executive_CheckListBox(CheckBoxList chkbox_list, string sql_text, string valuetype, string texttype)
    {
        string returnvalue = string.Empty;
        try
        {
            if (con.State == ConnectionState.Closed) con.Open();
            SqlCommand cmd = new SqlCommand(sql_text, con);
            DataTable dt_list = new DataTable();
            SqlDataAdapter da_list = new SqlDataAdapter(cmd);
            da_list.Fill(dt_list);



            chkbox_list.DataSource = dt_list;
            chkbox_list.DataTextField = texttype;
            chkbox_list.DataValueField = valuetype;
            chkbox_list.DataBind();
            cmd.Connection.Close();

        }
        catch
        {
        }
        finally
        {
            if (con != null && con.State == ConnectionState.Open) con.Close();

        }
    }
    public void Fn_Executive_checkListWithSelect(CheckBoxList drp_list, string sql_text, string valuetype, string texttype)
    {
        string returnvalue = string.Empty;
        try
        {
            if (con.State == ConnectionState.Closed) con.Open();
            SqlCommand cmd = new SqlCommand(sql_text, con);
            DataTable dt_list = new DataTable();
            SqlDataAdapter da_list = new SqlDataAdapter(cmd);
            da_list.Fill(dt_list);

            DataRow newdata = dt_list.NewRow();
            newdata[valuetype] = "0";
            newdata[texttype] = "--Select--";
            dt_list.Rows.InsertAt(newdata, 0);

            drp_list.DataSource = dt_list;
            drp_list.DataTextField = texttype;
            drp_list.DataValueField = valuetype;
            drp_list.DataBind();
            cmd.Connection.Close();

        }
        catch
        {
        }
        finally
        {
            if (con != null && con.State == ConnectionState.Open) con.Close();

        }
    }
    public void Fn_Executive_checkListWithoutSelect(CheckBoxList drp_list, string sql_text, string valuetype, string texttype)
    {
        string returnvalue = string.Empty;
        try
        {
            if (con.State == ConnectionState.Closed) con.Open();
            SqlCommand cmd = new SqlCommand(sql_text, con);
            DataTable dt_list = new DataTable();
            SqlDataAdapter da_list = new SqlDataAdapter(cmd);
            da_list.Fill(dt_list);

            drp_list.DataSource = dt_list;
            drp_list.DataTextField = texttype;
            drp_list.DataValueField = valuetype;
            drp_list.DataBind();
            cmd.Connection.Close();

        }
        catch
        {
        }
        finally
        {
            if (con != null && con.State == ConnectionState.Open) con.Close();

        }
    }
    public void Fn_Executive_ListWithSelect_WithOther(DropDownList drp_list, string sql_text, string valuetype, string texttype)
    {
        string returnvalue = string.Empty;
        try
        {
            if (con.State == ConnectionState.Closed) con.Open();
            SqlCommand cmd = new SqlCommand(sql_text, con);
            DataTable dt_list = new DataTable();
            SqlDataAdapter da_list = new SqlDataAdapter(cmd);
            da_list.Fill(dt_list);

            int totolcnt = 0;
            if (dt_list.Rows.Count > 0) totolcnt = dt_list.Rows.Count;


            DataRow newdata = dt_list.NewRow();
            newdata[valuetype] = "0";
            newdata[texttype] = "--Select--";
            dt_list.Rows.InsertAt(newdata, 0);

            newdata = dt_list.NewRow();
            newdata[valuetype] = "-999";
            newdata[texttype] = "Others";
            dt_list.Rows.InsertAt(newdata, totolcnt + 1);

            drp_list.DataSource = dt_list;
            drp_list.DataTextField = texttype;
            drp_list.DataValueField = valuetype;
            drp_list.DataBind();
            cmd.Connection.Close();

        }
        catch
        {
        }
        finally
        {
            if (con != null && con.State == ConnectionState.Open) con.Close();

        }
    }

    public void Fn_YearList_Select(DropDownList drp_list, string valuetype, string texttype)
    {
        string returnvalue = string.Empty;
        try
        {

            DataTable dt_list = new DataTable();
            dt_list.Columns.Add(valuetype);
            dt_list.Columns.Add(texttype);
            DataRow newdata = dt_list.NewRow();
            newdata[valuetype] = "0";
            newdata[texttype] = "--Select--";
            dt_list.Rows.InsertAt(newdata, 0);

            int startyear = DateTime.Now.Year;
            startyear = startyear - 10;


            for (int yearid = startyear; yearid <= startyear + 20; yearid++)
            {
                newdata = dt_list.NewRow();
                newdata[valuetype] = Convert.ToString(yearid);
                newdata[texttype] = Convert.ToString(yearid);

                int totolcnt = 0;
                if (dt_list.Rows.Count > 0) totolcnt = dt_list.Rows.Count;
                dt_list.Rows.InsertAt(newdata, totolcnt + 1);
            }

            drp_list.DataSource = dt_list;
            drp_list.DataTextField = texttype;
            drp_list.DataValueField = valuetype;
            drp_list.DataBind();


        }
        catch
        {
        }
    }
    public bool Fn_DropDown_Data_found(DropDownList drp_list)
    {

        try
        {
            if (drp_list != null)
            {
                if (drp_list.Items.Count > 0)
                {
                    return true;
                }
            }
            return false;
        }
        catch
        {
            return false;
        }
    }
    public string Fn_Dropdown_FindByValue(DropDownList drp_list, string default_value,string find_value)
    {
        
        try
        {
            if (drp_list != null)
            {
                if (drp_list.Items.Count > 0)
                {
                    if (drp_list.Items.FindByValue(find_value) != null)
                    {
                        default_value = find_value;
                    }
                    return default_value;
                }
            }
            return default_value;
        }
        catch
        {
            return default_value;
        }
    }

    public bool Fn_DropDown_Data_Mandatory(DropDownList drp_list)
    {
        bool iserror = false;
        try
        {
            if (Fn_DropDown_Data_found(drp_list) == true)
            {
                if (drp_list.SelectedValue == "0" || drp_list.SelectedValue == "-999")
                {
                    iserror = true;
                }
            }
            else
            {
                iserror = true;
            }
            return iserror;
        }
        catch
        {
            return iserror;
        }
    }
    public void Fn_Financial_Year(DropDownList drp_list)
    {
        string returnvalue = string.Empty;
        try
        {
            if (con.State == ConnectionState.Closed) con.Open();
            SqlCommand cmd = new SqlCommand("USP_GenerateFinancial_Year", con);
            cmd.CommandType = CommandType.StoredProcedure;
            DataTable dt_list = new DataTable();
            SqlDataAdapter da_list = new SqlDataAdapter(cmd);
            da_list.Fill(dt_list);

            DataRow newdata = dt_list.NewRow();
            newdata["yearpos"] = "0";
            newdata["yearvalue"] = "--Select--";
            dt_list.Rows.InsertAt(newdata, 0);

            drp_list.DataSource = dt_list;
            drp_list.DataTextField = "yearvalue";
            drp_list.DataValueField = "yearpos";
            drp_list.DataBind();
            cmd.Connection.Close();

        }
        catch
        {
        }
        finally
        {
            if (con != null && con.State == ConnectionState.Open) con.Close();

        }
    }

    public void Fn_PageLevel_Access(string page, string userid)
    {
        try
        {
            if (con.State == ConnectionState.Closed) con.Open();
            SqlCommand cmd = new SqlCommand("USP_PageLevelAccess", con);
            cmd.Parameters.AddWithValue("@pagename", page);
            cmd.Parameters.AddWithValue("@usr_id", userid);
            cmd.Parameters.AddWithValue("@mode", "userbasedsearch");
            cmd.CommandType = CommandType.StoredProcedure;
            DataTable dt_list = new DataTable();
            SqlDataAdapter da_list = new SqlDataAdapter(cmd);
            da_list.Fill(dt_list);
            if (dt_list != null)
            {
                if (dt_list.Rows.Count > 0)
                {
                    HttpContext.Current.Session["Access_Add"] = Convert.ToString(dt_list.Rows[0]["Access_Add"]);
                    HttpContext.Current.Session["Access_View"] = Convert.ToString(dt_list.Rows[0]["Access_View"]);
                    HttpContext.Current.Session["Access_Edit"] = Convert.ToString(dt_list.Rows[0]["Access_Edit"]);
                    HttpContext.Current.Session["Access_Trash"] = Convert.ToString(dt_list.Rows[0]["Access_Trash"]);
                }
                else
                {
                    HttpContext.Current.Session["Access_Add"] = Convert.ToString("False");
                    HttpContext.Current.Session["Access_View"] = Convert.ToString("False");
                    HttpContext.Current.Session["Access_Edit"] = Convert.ToString("False");
                    HttpContext.Current.Session["Access_Trash"] = Convert.ToString("False");
                }
            }
            else
            {
                HttpContext.Current.Session["Access_Add"] = Convert.ToString("False");
                HttpContext.Current.Session["Access_View"] = Convert.ToString("False");
                HttpContext.Current.Session["Access_Edit"] = Convert.ToString("False");
                HttpContext.Current.Session["Access_Trash"] = Convert.ToString("False");
            }


            cmd.Connection.Close();

        }
        catch
        {
        }
        finally
        {
            if (con != null && con.State == ConnectionState.Open) con.Close();

        }
    }





    public DataTable Fn_Menu_Access(string userid)
    {
        DataTable dt_list = new DataTable();
        try
        {
            if (con.State == ConnectionState.Closed) con.Open();
            SqlCommand cmd = new SqlCommand("USP_PageLevelAccess", con);
            cmd.Parameters.AddWithValue("@usr_id", userid);
            cmd.Parameters.AddWithValue("@mode", "userbasedaccess");
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da_list = new SqlDataAdapter(cmd);
            da_list.Fill(dt_list);
            cmd.Connection.Close();

        }
        catch
        {
        }
        finally
        {
            if (con != null && con.State == ConnectionState.Open) con.Close();

        }
        return dt_list;
    }
    public void Fn_YearList1_Select(System.Web.UI.HtmlControls.HtmlSelect drp_list, string valuetype, string texttype)
    {
        string returnvalue = string.Empty;
        try
        {

            DataTable dt_list = new DataTable();
            dt_list.Columns.Add(valuetype);
            dt_list.Columns.Add(texttype);
            DataRow newdata = dt_list.NewRow();
            newdata[valuetype] = "0";
            newdata[texttype] = "--Select--";
            dt_list.Rows.InsertAt(newdata, 0);

            int startyear = DateTime.Now.Year;
            startyear = startyear - 7;


            for (int yearid = startyear; yearid <= startyear + 20; yearid++)
            {
                newdata = dt_list.NewRow();
                newdata[valuetype] = Convert.ToString(yearid);
                newdata[texttype] = Convert.ToString(yearid);

                int totolcnt = 0;
                if (dt_list.Rows.Count > 0) totolcnt = dt_list.Rows.Count;
                dt_list.Rows.InsertAt(newdata, totolcnt + 1);
            }

            drp_list.DataSource = dt_list;
            drp_list.DataTextField = texttype;
            drp_list.DataValueField = valuetype;
            drp_list.DataBind();


        }
        catch
        {
        }
    }
    public void ftp_transfer(string filename, string filepath, string ftpfolder)
    {
        try
        {
            String ftpip = Convert.ToString(ConfigurationManager.AppSettings["ftpip"]);
            String ftpport = Convert.ToString(ConfigurationManager.AppSettings["ftpport"]);
            String ftpuser = Convert.ToString(ConfigurationManager.AppSettings["ftpuser"]);
            String ftppwd = Convert.ToString(ConfigurationManager.AppSettings["ftppwd"]);


            string requestpath = "ftp://" + ftpip + ":" + ftpport + "/" + ftpfolder + "/" + filename;


            // Get the object used to communicate with the server.  
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(requestpath);
            request.Method = WebRequestMethods.Ftp.UploadFile;

            // This example assumes the FTP site uses anonymous logon.  
            request.Credentials = new NetworkCredential(ftpuser, ftppwd);

            // Copy the contents of the file to the request stream.  


            byte[] fileContents = File.ReadAllBytes(filepath);
            request.ContentLength = fileContents.Length;
            Stream requestStream = request.GetRequestStream();
            requestStream.Write(fileContents, 0, fileContents.Length);
            requestStream.Close();

            FtpWebResponse response = (FtpWebResponse)request.GetResponse();

            Console.WriteLine("Upload File Complete, status {0}", response.StatusDescription);

            response.Close();

        }
        catch (WebException e)
        {
            Console.WriteLine(e.Message.ToString());
            String status = ((FtpWebResponse)e.Response).StatusDescription;
            Console.WriteLine(status);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message.ToString());
        }
    }
    public static string NumberToWords(int number)
    {
        if (number == 0)
            return "zero";

        if (number < 0)
            return "minus " + NumberToWords(Math.Abs(number));

        string words = "";

        if ((number / 1000000) > 0)
        {
            words += NumberToWords(number / 1000000) + " million ";
            number %= 1000000;
        }

        if ((number / 1000) > 0)
        {
            words += NumberToWords(number / 1000) + " thousand ";
            number %= 1000;
        }

        if ((number / 100) > 0)
        {
            words += NumberToWords(number / 100) + " hundred ";
            number %= 100;
        }

        if (number > 0)
        {
            if (words != "")
                words += "and ";

            var unitsMap = new[] { "zero", "first", "second", "third", "fourth", "fifth", "sixth", "seventh", "eighth", "nineth", "tenth", "eleventh", "twelveth", "thirteenth", "fourteenth", "fifteenth", "sixteenth", "seventeenth", "eighteenth", "nineteenth" };
            var tensMap = new[] { "zero", "tenth", "twenty", "thirty", "forty", "fifty", "sixty", "seventy", "eighty", "ninety" };

            if (number < 20)
                words += unitsMap[number];
            else
            {
                words += tensMap[number / 10];
                if ((number % 10) > 0)
                    words += "-" + unitsMap[number % 10];
            }
        }

        return words;
    }
    public bool EmptyValidator(string controlvalue, string alertmsg)
    {

        if (controlvalue.Trim() == "")
        {
            return false;
        }
        return true;
    }
}
