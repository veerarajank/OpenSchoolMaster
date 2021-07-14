using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Adm_features : System.Web.UI.Page
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
        DataTable dt_data = ies.Fn_ExecutiveSql_Datatable("select id,name,isnull(status,'0') as status from tbl_features", 0, 0);
        if (dt_data != null && dt_data.Rows.Count > 0)
        {
            string str_modulecontent = @"<table style='width:100%' border='0' cellspacing='0' cellpadding='0'>
                                    <tbody><tr>";

            for (int irow = 0; irow <= dt_data.Rows.Count - 1; irow++)
            {
               
                string name = Convert.ToString(dt_data.Rows[irow]["name"]);
                str_modulecontent += "<td><label for='Configurations_" + name + "'>" + name + "</label></td>";
            }
            str_modulecontent = str_modulecontent + "</tr>";
            str_modulecontent = str_modulecontent + "<tr>";

            for (int irow = 0; irow <= dt_data.Rows.Count - 1; irow++)
            {
                string id = Convert.ToString(dt_data.Rows[irow]["id"]);
                string name = Convert.ToString(dt_data.Rows[irow]["name"]);
                string status = "";
                string status_link = "EnableModule(\"" + id + "\")";

                if (Convert.ToString(dt_data.Rows[irow]["status"]) == "1")
                {
                    status = "checked='checked'";
                    status_link = "DisableModule(\"" + id + "\")";
                }
                str_modulecontent += "<td><input type='checkbox' " + status + "  onclick='" + status_link + "'></td>";
            }
            div_module_list.InnerHtml = str_modulecontent + "</tr></tbody></table>";


        }
    }
    protected void btn_del_ServerClick(object sender, EventArgs e)
    {
        IES_Generic_Function ies = new IES_Generic_Function();
        ies.ies_parameters.Clear();
        ies.ies_parameters.Add("@id", Convert.ToString(hd_id.Value));
        string str_qry = "Update tbl_features set status='0' where id=@id";
        ies.Fn_ExecutiveSql(str_qry, 0, 1);
        Response.Redirect("Adm_features.aspx");
    }
    protected void btn_enable_ServerClick(object sender, EventArgs e)
    {
        IES_Generic_Function ies = new IES_Generic_Function();
        ies.ies_parameters.Clear();
        ies.ies_parameters.Add("@id", Convert.ToString(hd_id.Value));
        string str_qry = "Update tbl_features set status='1' where id=@id";
        ies.Fn_ExecutiveSql(str_qry, 0, 1);
        Response.Redirect("Adm_features.aspx");
    }
}