using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Adm_settingsDashboard : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["Usrid"] == null)
        {
            Response.Redirect("login.aspx");
        }
        if (!IsPostBack)
        {
            BindDisplay();
        }
    }
    protected void BindDisplay()
    {

        IES_Generic_Function ies = new IES_Generic_Function();
        ies.ies_parameters.Clear();
        DataTable dt_data = ies.Fn_ExecutiveSql_Datatable("select id,name,isnull(status,'0') as status,isnull(value,'0') as value from tbl_settings order by id", 0, 0);
        if (dt_data != null && dt_data.Rows.Count > 0)
        {
            string str_modulecontent = @"<table style='width:100%' border='0' cellspacing='0' cellpadding='0'>
                                    <tbody><tr class='tablebx_topbg'><td>#</td><td>Name</td><td>Block Order</td><td>Action</td></tr>";

            for (int irow = 0; irow <= dt_data.Rows.Count - 1; irow++)
            {
                string id = Convert.ToString(dt_data.Rows[irow]["id"]);
                string name = Convert.ToString(dt_data.Rows[irow]["name"]);
                string value = Convert.ToString(dt_data.Rows[irow]["value"]);
                string status = "Disable";
                string status_link = "DisableModule(\"" + id + "\")";

                if (Convert.ToString(dt_data.Rows[irow]["status"]) == "0")
                {
                    status = "Enable";
                    status_link = "EnableModule(\"" + id + "\")";
                }
                if ((irow % 2) == 0)
                {
                    str_modulecontent += "<tr class='even'><td>" + (irow + 1).ToString() + "</td><td>" + name + "</td><td>" + value + "</td><td><a onclick='" + status_link + "'>" + status + "</a></td></tr>";
                }
                else
                {
                    str_modulecontent += "<tr class='odd'><td>" + (irow + 1).ToString() + "</td><td>" + name + "</td><td>" + value + "</td><td><a onclick='" + status_link + "'>" + status + "</a></td></tr>";
                }
            }
            div_module_list.InnerHtml = str_modulecontent + "</tbody></table>";


        }
    }
    protected void btn_del_ServerClick(object sender, EventArgs e)
    {
        IES_Generic_Function ies = new IES_Generic_Function();
        ies.ies_parameters.Clear();
        ies.ies_parameters.Add("@id", Convert.ToString(hd_id.Value));
        string str_qry = "Update tbl_settings set status='0', value='0' where id=@id";
        ies.Fn_ExecutiveSql(str_qry, 0, 1);
        Update_All();
        Response.Redirect("Adm_settingsDashboard.aspx");
    }
    protected void btn_enable_ServerClick(object sender, EventArgs e)
    {
        IES_Generic_Function ies = new IES_Generic_Function();
        ies.ies_parameters.Clear();
        ies.ies_parameters.Add("@id", Convert.ToString(hd_id.Value));
        string str_qry = "Update tbl_settings set status='1' where id=@id";
        ies.Fn_ExecutiveSql(str_qry, 0, 1);
        Update_All();
        Response.Redirect("Adm_settingsDashboard.aspx");
    }
    protected void Update_All()
    {

        IES_Generic_Function ies = new IES_Generic_Function();
        ies.ies_parameters.Clear();
        DataTable dt_data = ies.Fn_ExecutiveSql_Datatable("select id,name,isnull(value,'0') as value from tbl_settings where status='1' order by id", 0, 0);
        if (dt_data != null && dt_data.Rows.Count > 0)
        {           

            for (int irow = 0; irow <= dt_data.Rows.Count - 1; irow++)
            {
                string id = Convert.ToString(dt_data.Rows[irow]["id"]);
                ies = new IES_Generic_Function();
                ies.ies_parameters.Clear();
                ies.ies_parameters.Add("@id", Convert.ToString(id));
                string str_qry = "Update tbl_settings set value='" + (irow+1).ToString() + "' where id=@id";
                ies.Fn_ExecutiveSql(str_qry, 0, 1);
            }
           


        }
    }
    protected void btn_reset_Click(object sender, EventArgs e)
    {
        IES_Generic_Function ies = new IES_Generic_Function();
        ies.ies_parameters.Clear();
        DataTable dt_data = ies.Fn_ExecutiveSql_Datatable("select id,name,isnull(value,'0') as value from tbl_settings order by id", 0, 0);
        if (dt_data != null && dt_data.Rows.Count > 0)
        {

            for (int irow = 0; irow <= dt_data.Rows.Count - 1; irow++)
            {
                string id = Convert.ToString(dt_data.Rows[irow]["id"]);
                ies = new IES_Generic_Function();
                ies.ies_parameters.Clear();
                ies.ies_parameters.Add("@id", Convert.ToString(id));
                string str_qry = "Update tbl_settings set value='" + (irow + 1).ToString() + "' where id=@id";
                ies.Fn_ExecutiveSql(str_qry, 0, 1);
            }
        }
        Response.Redirect("Adm_settingsDashboard.aspx");
    }
    
    protected void btn_disable_Click(object sender, EventArgs e)
    {

        IES_Generic_Function ies = new IES_Generic_Function();
        ies.ies_parameters.Clear();
        string str_qry = "Update tbl_settings set status='0',value='0'";
        ies.Fn_ExecutiveSql(str_qry, 0, 0);
        Response.Redirect("Adm_settingsDashboard.aspx");
    }
    protected void btn_enableall_Click(object sender, EventArgs e)
    {
        IES_Generic_Function ies = new IES_Generic_Function();
        ies.ies_parameters.Clear();
        DataTable dt_data = ies.Fn_ExecutiveSql_Datatable("select id,name,isnull(value,'0') as value from tbl_settings order by id", 0, 0);
        if (dt_data != null && dt_data.Rows.Count > 0)
        {

            for (int irow = 0; irow <= dt_data.Rows.Count - 1; irow++)
            {
                string id = Convert.ToString(dt_data.Rows[irow]["id"]);
                ies = new IES_Generic_Function();
                ies.ies_parameters.Clear();
                ies.ies_parameters.Add("@id", Convert.ToString(id));
                string str_qry = "Update tbl_settings set status='1', value='" + (irow + 1).ToString() + "' where id=@id";
                ies.Fn_ExecutiveSql(str_qry, 0, 1);
            }
        }
        Response.Redirect("Adm_settingsDashboard.aspx");
    }
}