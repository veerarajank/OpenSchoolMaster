<%@ WebHandler Language="C#" Class="EmployeeHandler" %>

using System;
using System.Web;

public class EmployeeHandler : IHttpHandler {

    public void ProcessRequest(HttpContext context)
    {
        System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["conn"].ConnectionString);
        try
        {
            System.Data.SqlClient.SqlCommand selcmd = new System.Data.SqlClient.SqlCommand("select photo_data from tbl_employees where id=" + context.Request.QueryString["imgid"], con);
            con.Open();
            System.Data.SqlClient.SqlDataAdapter da = new System.Data.SqlClient.SqlDataAdapter(selcmd);
            System.Data.DataTable dt = new System.Data.DataTable();
            da.Fill(dt);

            if (dt != null && dt.Rows.Count > 0)
            {
                context.Response.ContentType = "image/jpg";
                context.Response.BinaryWrite((byte[])dt.Rows[0]["photo_data"]);
            }
            if (selcmd != null)
                selcmd.Connection.Close();
        }
        catch
        {
        }
        finally
        {
            if (con != null)
                con.Close();
        }
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}