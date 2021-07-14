using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Adm_TaxMaster : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["Usrid"] == null)
        {
            Response.Redirect("login.aspx");
        }
        if (!IsPostBack)
        {
            IES_Generic_Function ies = new IES_Generic_Function();
            if (Convert.ToBoolean(Session["Access_Edit"]) == false)
            {
                btn_new.Visible = false;
            }

            search_section.Visible = false;
            view_section.Visible = false;
            add_section.Visible = false;
            update_section.Visible = false;


            string mode = Convert.ToString(Request.QueryString["mode"]);
            if (mode != null && mode == "search")
            {
                search_section.Visible = true;
                BindListView();
            }
            else if (mode != null && mode == "add")
            {
                add_section.Visible = true;
            }
            else if (mode != null && mode == "edit")
            {
                update_section.Visible = true;
                BindDisplay();
            }
            else if (mode != null && mode == "view")
            {
                view_section.Visible = true;
                BindDisplay();
            }
            else if (mode == null)
            {
                search_section.Visible = true;
                BindListView();
            }




        }

    }

    protected void lnk_edit_Command(object sender, CommandEventArgs e)
    {
        string id = string.Empty;
        id = Convert.ToString(e.CommandArgument);
        if (e.CommandName == "edit")
        {
            if (Convert.ToBoolean(Session["Access_Edit"]) == true)
            {

                Response.Redirect("Adm_TaxMaster.aspx?mode=edit&id=" + id);
            }
        }
    }
    protected void lnk_search_Command(object sender, CommandEventArgs e)
    {
        string id = string.Empty;
        id = Convert.ToString(e.CommandArgument);
        if (e.CommandName == "view")
        {
            if (Convert.ToBoolean(Session["Access_View"]) == true)
            {

                Response.Redirect("Adm_TaxMaster.aspx?mode=view&id=" + id);
            }
        }
    }
    protected void btn_new_ServerClick(object sender, EventArgs e)
    {
        Response.Redirect("Adm_TaxMaster.aspx?mode=add");
    }
    protected void BindListView()
    {
        IES_Generic_Function ies = new IES_Generic_Function();
        ies.ies_parameters.Clear();

        ies.ies_parameters.Add("@mode", "search");
        DataTable dt_data = ies.Fn_ExecutiveSql_Datatable("UDP_Tax_Master", 1, 1);
        if (dt_data != null)
        {
            lbl_cnt.InnerHtml = "&nbsp;Displaying " + Convert.ToString(dt_data.Rows.Count) + " result(s).";
            lv_listview.DataSource = dt_data;
            lv_listview.DataBind();
        }
    }
    protected void BindDisplay()
    {
        string id = Convert.ToString(Request.QueryString["id"]);
        IES_Generic_Function ies = new IES_Generic_Function();
        ies.ies_parameters.Clear();
        ies.ies_parameters.Add("@id", Convert.ToString(id));
        ies.ies_parameters.Add("@mode", "search");
        DataTable dt_data = ies.Fn_ExecutiveSql_Datatable("UDP_Tax_Master", 1, 1);
        if (dt_data != null && dt_data.Rows.Count > 0)
        {

            //Edit Section
            tbx_edit_name.Text = Convert.ToString(dt_data.Rows[0]["name"]);
            tbx_edit_value.Text = Convert.ToString(dt_data.Rows[0]["taxpercent"]);

            rd_editno.Checked = true;
            rd_edityes.Checked = false;
            if (Convert.ToString(dt_data.Rows[0]["status"]) == "1")
            {
                rd_edityes.Checked = true;
                rd_editno.Checked = false;
            }


           
            //View Section

            lbl_name.InnerText = Convert.ToString(dt_data.Rows[0]["name"]);
            lbl_value.InnerText = Convert.ToString(dt_data.Rows[0]["taxpercent"]);

            if (Convert.ToString(dt_data.Rows[0]["status"]) == "1")
            {
                lbl_status.InnerText = "Active";
                lbl_status.Attributes.Add("class", "label label-success");
            }
            else
            {
                lbl_status.InnerText = "Inactive";
                lbl_status.Attributes.Add("class", "label label-warning");
            }



        }
    }

    protected bool ValidInput()
    {
        bool iserror = false;
        if (tbx_name.Text.Trim() == "") iserror = true;
        if (tbx_value.Text.Trim() == "") iserror = true;


        return iserror;

    }
    protected bool EditValidInput()
    {
        bool iserror = false;
        if (tbx_edit_name.Text.Trim() == "") iserror = true;
        if (tbx_edit_value.Text.Trim() == "") iserror = true;


        return iserror;

    }

    protected void btn_edit_ServerClick(object sender, EventArgs e)
    {
        string id = Convert.ToString(Request.QueryString["id"]);
        Response.Redirect("Adm_TaxMaster.aspx?mode=edit&id=" + id);
    }
    protected void btn_manage_ServerClick(object sender, EventArgs e)
    {
        Response.Redirect("Adm_TaxMaster.aspx?mode=search");
    }
    protected void btn_update_ServerClick(object sender, EventArgs e)
    {
        string id = Convert.ToString(Request.QueryString["id"]);
        if (EditValidInput() == true) { edit_field_mandary.Attributes.Add("style", "color:red"); return; }


        IES_Generic_Function ies = new IES_Generic_Function();
        ies.ies_parameters.Clear();

        string name = Convert.ToString(tbx_edit_name.Text);
        string taxpercent = Convert.ToString(tbx_edit_value.Text);

        string status = "0";
        if (rd_edityes.Checked == true) status = "1";            
            

        ies.ies_parameters.Add("@id", Convert.ToString(id));
        ies.ies_parameters.Add("@name", Convert.ToString(name));
        ies.ies_parameters.Add("@taxpercent", Convert.ToString(taxpercent));
        ies.ies_parameters.Add("@status", Convert.ToString(status));

        ies.ies_parameters.Add("@mode", "update");
        DataTable dt_result = ies.Fn_ExecutiveSql_Datatable("UDP_Tax_Master", 1, 1);
        if (dt_result != null && dt_result.Rows.Count > 0)
        {
            if (Convert.ToString(dt_result.Rows[0]["Error"]) == "0")
            {
                Response.Redirect("Adm_TaxMaster.aspx");
            }
        }
    }
    protected void btn_del_ServerClick(object sender, EventArgs e)
    {

        if (Convert.ToBoolean(Session["Access_Trash"]) == true)
        {
            IES_Generic_Function ies = new IES_Generic_Function();
            ies.ies_parameters.Clear();
            ies.ies_parameters.Add("@id", Convert.ToString(hd_id.Value));
            ies.ies_parameters.Add("@mode", "del");
            ies.Fn_ExecutiveSql("UDP_Tax_Master", 1, 1);
            Response.Redirect("Adm_TaxMaster.aspx");
        }
    }
    protected void btn_createnew_ServerClick(object sender, EventArgs e)
    {
        if (ValidInput() == true) { field_mandary.Attributes.Add("style", "color:red"); return; }

        IES_Generic_Function ies = new IES_Generic_Function();
        ies.ies_parameters.Clear();

        string name = Convert.ToString(tbx_name.Text);
        string taxpercent = Convert.ToString(tbx_value.Text);
        string status = "0";
        if (rd_isactiveyes.Checked == true) status = "1";    


        ies.ies_parameters.Add("@name", Convert.ToString(name));
        ies.ies_parameters.Add("@taxpercent", Convert.ToString(taxpercent));
        ies.ies_parameters.Add("@status", Convert.ToString(status));


        ies.ies_parameters.Add("@mode", "add");
        DataTable dt_result = ies.Fn_ExecutiveSql_Datatable("UDP_Tax_Master", 1, 1);
        if (dt_result != null && dt_result.Rows.Count > 0)
        {
            if (Convert.ToString(dt_result.Rows[0]["Error"]) == "0")
            {
                Response.Redirect("Adm_TaxMaster.aspx");
            }
            else if (Convert.ToString(dt_result.Rows[0]["Error"]) == "1")
            {
                lbl_error.InnerHtml = Convert.ToString(dt_result.Rows[0]["Msg"]);
            }

        }
    }

    protected void lv_listview_PagePropertiesChanging(object sender, PagePropertiesChangingEventArgs e)
    {

    }

   
}