using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Adm_studentdocumenttype : System.Web.UI.Page
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

                Response.Redirect("Adm_studentdocumenttype.aspx?mode=edit&id=" + id);
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

                Response.Redirect("Adm_studentdocumenttype.aspx?mode=view&id=" + id);
            }
        }
    }
    protected void btn_new_ServerClick(object sender, EventArgs e)
    {
        Response.Redirect("Adm_studentdocumenttype.aspx?mode=add");
    }
    protected void BindListView()
    {
        IES_Generic_Function ies = new IES_Generic_Function();
        ies.ies_parameters.Clear();

        ies.ies_parameters.Add("@mode", "search");
        DataTable dt_data = ies.Fn_ExecutiveSql_Datatable("UDP_Student_Dcoument", 1, 1);
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
        DataTable dt_data = ies.Fn_ExecutiveSql_Datatable("UDP_Student_Dcoument", 1, 1);
        if (dt_data != null && dt_data.Rows.Count > 0)
        {

            //Edit Section

            chk_editmandary.Checked = false;

            tbx_edit_documentname.Text = Convert.ToString(dt_data.Rows[0]["name"]);
            lbl_mandatory.InnerHtml = "<span class='bg-red'>&nbsp;No&nbsp;</span>";
            if (Convert.ToString(dt_data.Rows[0]["mandatory"]) == "1")
            {
                chk_editmandary.Checked = true;
                lbl_mandatory.InnerHtml = "<span class='bg-green'>&nbsp;Yes&nbsp;</span>";
            }

            rdo_edit_isrequiredfalse.Checked = true;
            rdo_edit_isrequiredtrue.Checked = false;
            lbl_required.InnerHtml = "<span class='bg-red'>&nbsp;No&nbsp;</span>";
            if (Convert.ToString(dt_data.Rows[0]["is_required"]) == "1")
            {
                rdo_edit_isrequiredtrue.Checked = true;
                rdo_edit_isrequiredfalse.Checked = false;
                lbl_required.InnerHtml = "<span class='bg-green'>&nbsp;Yes&nbsp;</span>";
            }

            chk_edit_Document_ShowOnline.Checked = false;
            if (Convert.ToString(dt_data.Rows[0]["is_showonline"]) == "1")
            {
                chk_edit_Document_ShowOnline.Checked = true;
            }


            lbl_documentname.InnerHtml = Convert.ToString(dt_data.Rows[0]["name"]);
          


        }
    }

    protected bool ValidInput()
    {
        bool iserror = false;
        if (tbx_documentname.Text.Trim() == "") iserror = true;


        return iserror;

    }
    protected bool EditValidInput()
    {
        bool iserror = false;
        if (tbx_edit_documentname.Text.Trim() == "") iserror = true;


        return iserror;

    }

    protected void btn_edit_ServerClick(object sender, EventArgs e)
    {
        string id = Convert.ToString(Request.QueryString["id"]);
        Response.Redirect("Adm_studentdocumenttype.aspx?mode=edit&id=" + id);
    }
    protected void btn_manage_ServerClick(object sender, EventArgs e)
    {
        Response.Redirect("Adm_studentdocumenttype.aspx?mode=search");
    }
    protected void btn_update_ServerClick(object sender, EventArgs e)
    {
        string id = Convert.ToString(Request.QueryString["id"]);
        if (EditValidInput() == true) { edit_field_mandary.Attributes.Add("style", "color:red"); return; }


        IES_Generic_Function ies = new IES_Generic_Function();
        ies.ies_parameters.Clear();

        string name = Convert.ToString(tbx_edit_documentname.Text);

        int is_showonline = 0;
        if (chk_edit_Document_ShowOnline.Checked) is_showonline = 1;

        int mandatory = 0;
        if (chk_editmandary.Checked) mandatory = 1;

        int is_required = 0;
        if (rdo_edit_isrequiredtrue.Checked) is_required = 1;

        ies.ies_parameters.Add("@id", Convert.ToString(id));
        ies.ies_parameters.Add("@name", Convert.ToString(name));

        ies.ies_parameters.Add("@mandatory", Convert.ToString(mandatory));
        ies.ies_parameters.Add("@is_required", Convert.ToString(is_required));
        ies.ies_parameters.Add("@is_showonline", Convert.ToString(is_showonline));


        ies.ies_parameters.Add("@mode", "update");
        DataTable dt_result = ies.Fn_ExecutiveSql_Datatable("UDP_Student_Dcoument", 1, 1);
        if (dt_result != null && dt_result.Rows.Count > 0)
        {
            if (Convert.ToString(dt_result.Rows[0]["Error"]) == "0")
            {
                Response.Redirect("Adm_studentdocumenttype.aspx");
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
            ies.Fn_ExecutiveSql("UDP_Student_Dcoument", 1, 1);
            Response.Redirect("Adm_studentdocumenttype.aspx");
        }
    }
    protected void btn_createeventtype_ServerClick(object sender, EventArgs e)
    {
        if (ValidInput() == true) { field_mandary.Attributes.Add("style", "color:red"); return; }

        IES_Generic_Function ies = new IES_Generic_Function();
        ies.ies_parameters.Clear();

        string name = Convert.ToString(tbx_documentname.Text);

        int is_showonline = 0;
        if (chk_showonline.Checked) is_showonline = 1;

        int mandatory = 0;
        if (chk_mandary.Checked) mandatory = 1;

        int is_required = 0;
        if (rdo_isrequiredtrue.Checked) is_required = 1;



        ies.ies_parameters.Add("@name", Convert.ToString(name));

        ies.ies_parameters.Add("@mandatory", Convert.ToString(mandatory));
        ies.ies_parameters.Add("@is_required", Convert.ToString(is_required));
        ies.ies_parameters.Add("@is_showonline", Convert.ToString(is_showonline));


        ies.ies_parameters.Add("@mode", "add");
        DataTable dt_result = ies.Fn_ExecutiveSql_Datatable("UDP_Student_Dcoument", 1, 1);
        if (dt_result != null && dt_result.Rows.Count > 0)
        {
            if (Convert.ToString(dt_result.Rows[0]["Error"]) == "0")
            {
                Response.Redirect("Adm_studentdocumenttype.aspx");
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