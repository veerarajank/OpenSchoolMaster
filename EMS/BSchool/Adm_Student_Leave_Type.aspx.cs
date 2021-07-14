using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Adm_Student_Leave_Type : System.Web.UI.Page
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
            //ies.Fn_PageLevel_Access("Brand", Convert.ToString(Session["Usrid"]));
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
        string leave_id = string.Empty;
        leave_id = Convert.ToString(e.CommandArgument);
        if (e.CommandName == "edit")
        {
            if (Convert.ToBoolean(Session["Access_Edit"]) == true)
            {

                Response.Redirect("Adm_Student_Leave_Type.aspx?mode=edit&id=" + leave_id);
            }
        }
    }
    protected void lnk_search_Command(object sender, CommandEventArgs e)
    {
        string leave_id = string.Empty;
        leave_id = Convert.ToString(e.CommandArgument);
        if (e.CommandName == "view")
        {
            if (Convert.ToBoolean(Session["Access_View"]) == true)
            {

                Response.Redirect("Adm_Student_Leave_Type.aspx?mode=view&id=" + leave_id);
            }
        }
    }
    protected void btn_new_ServerClick(object sender, EventArgs e)
    {
        Response.Redirect("Adm_Student_Leave_Type.aspx?mode=add");
    }
    protected void BindListView()
    {
        IES_Generic_Function ies = new IES_Generic_Function();
        ies.ies_parameters.Clear();

        ies.ies_parameters.Add("@mode", "search");
        DataTable dt_data = ies.Fn_ExecutiveSql_Datatable("UDP_Leave_Type", 1, 1);
        if (dt_data != null)
        {
            lbl_cnt.InnerHtml = "&nbsp;Displaying " + Convert.ToString(dt_data.Rows.Count) + " result(s).";
            lv_leavetype.DataSource = dt_data;
            lv_leavetype.DataBind();
        }

        ies = new IES_Generic_Function();
        ies.ies_parameters.Clear();
        ies.ies_parameters.Add("@mode", "searchinactive");
        dt_data = ies.Fn_ExecutiveSql_Datatable("UDP_Leave_Type", 1, 1);
        if (dt_data != null)
        {
            lbl_cnt2.InnerHtml = "&nbsp;Displaying " + Convert.ToString(dt_data.Rows.Count) + " result(s).";
            lv_inactiveleavetype.DataSource = dt_data;
            lv_inactiveleavetype.DataBind();
        }
    }
    protected void BindDisplay()
    {
        string leave_id = Convert.ToString(Request.QueryString["id"]);
        IES_Generic_Function ies = new IES_Generic_Function();
        ies.ies_parameters.Clear();
        ies.ies_parameters.Add("@leave_id", Convert.ToString(leave_id));
        ies.ies_parameters.Add("@mode", "normalsearch");
        DataTable dt_data = ies.Fn_ExecutiveSql_Datatable("UDP_Leave_Type", 1, 1);
        if (dt_data != null && dt_data.Rows.Count > 0)
        {

            //Edit Section
            tbx_edit_leave_type.Text = Convert.ToString(dt_data.Rows[0]["leave_name"]);
            tbx_edit_Event_colortype.Text = Convert.ToString(dt_data.Rows[0]["leave_colour_code"]);

            tbx_edit_leavecode.Text = Convert.ToString(dt_data.Rows[0]["leave_code"]);
            tbx_edit_leavelable.Text = Convert.ToString(dt_data.Rows[0]["leave_label"]);

            if (ddl_edit_leave_status.Items.FindByValue(Convert.ToString(dt_data.Rows[0]["leave_status"])) != null)
            {
                ddl_edit_leave_status.SelectedValue = Convert.ToString(dt_data.Rows[0]["leave_status"]);
            }
            chk_edit_exclude.Checked = false;
            if (Convert.ToString(dt_data.Rows[0]["Is_exclude"]) == "1")
            {
                chk_edit_exclude.Checked = true;
            }


            lbl_eventname.InnerText = Convert.ToString(dt_data.Rows[0]["leave_name"]);
            lbl_colorcode.InnerText = Convert.ToString(dt_data.Rows[0]["leave_colour_code"]);

            lbl_code.InnerText = Convert.ToString(dt_data.Rows[0]["leave_code"]);
            lbl_label.InnerText = Convert.ToString(dt_data.Rows[0]["leave_label"]);

            lbl_viewcolorcode.Attributes.Add("style", "height: 22px; width: 22px; background: none center center no-repeat " + Convert.ToString(dt_data.Rows[0]["leave_colour_code"]) + "; vertical-align: middle; margin: 0px 0.25em; display: inline-block; color: rgb(255, 255, 255);");

            if (Convert.ToString(dt_data.Rows[0]["leave_status"]) == "1")
            {
                lbl_eventsts.InnerText = "Active";
                lbl_eventsts.Attributes.Add("class", "label label-success");
            }
            else
            {
                lbl_eventsts.InnerText = "Inactive";
                lbl_eventsts.Attributes.Add("class", "label label-warning");
            }

            if (Convert.ToString(dt_data.Rows[0]["Is_exclude"]) == "1")
            {
                lbl_exclude.InnerText = "Yes";
                lbl_exclude.Attributes.Add("class", "label label-success");
            }
            else
            {
                lbl_exclude.InnerText = "No";
                lbl_exclude.Attributes.Add("class", "label label-warning");
            }

        }
    }

    protected bool ValidInput()
    {
        bool iserror = false;
        if (tbx_leave_type.Text.Trim() == "") iserror = true;
        if (tbx_Event_colortype.Text.Trim() == "") iserror = true;
        if (tbx_leavecode.Text.Trim() == "") iserror = true;
        if (tbx_leavelable.Text.Trim() == "") iserror = true;
        


        return iserror;

    }
    protected bool EditValidInput()
    {
        bool iserror = false;
        if (tbx_edit_leave_type.Text.Trim() == "") iserror = true;
        if (tbx_edit_Event_colortype.Text.Trim() == "") iserror = true;
        if (tbx_edit_leavecode.Text.Trim() == "") iserror = true;
        if (tbx_edit_leavelable.Text.Trim() == "") iserror = true;
        

        return iserror;

    }

    protected void btn_edit_ServerClick(object sender, EventArgs e)
    {
        string leave_id = Convert.ToString(Request.QueryString["id"]);
        Response.Redirect("Adm_Student_Leave_Type.aspx?mode=edit&id=" + leave_id);
    }
    protected void btn_manage_ServerClick(object sender, EventArgs e)
    {
        Response.Redirect("Adm_Student_Leave_Type.aspx?mode=search");
    }
    protected void btn_update_ServerClick(object sender, EventArgs e)
    {
        string leave_id = Convert.ToString(Request.QueryString["id"]);
        if (EditValidInput() == true) { edit_field_mandary.Attributes.Add("style", "color:red"); return; }


        IES_Generic_Function ies = new IES_Generic_Function();
        ies.ies_parameters.Clear();

        string leave_name = Convert.ToString(tbx_edit_leave_type.Text);
        string leave_colour_code = Convert.ToString(tbx_edit_Event_colortype.Text);
        string leave_status = Convert.ToString(ddl_edit_leave_status.SelectedItem.Value);

        string leave_code = Convert.ToString(tbx_edit_leavecode.Text);
        string leave_label = Convert.ToString(tbx_edit_leavelable.Text);

        int Is_exclude = 0;
        if (chk_edit_exclude.Checked == true) Is_exclude = 1;
        

        ies.ies_parameters.Add("@leave_code", Convert.ToString(leave_code).Trim());
        ies.ies_parameters.Add("@leave_label", Convert.ToString(leave_label).Trim());
        ies.ies_parameters.Add("@Is_exclude", Convert.ToString(Is_exclude).Trim());


        ies.ies_parameters.Add("@leave_id", Convert.ToString(leave_id));
        ies.ies_parameters.Add("@leave_name", Convert.ToString(leave_name));
        ies.ies_parameters.Add("@leave_colour_code", Convert.ToString(leave_colour_code).Trim());
        ies.ies_parameters.Add("@leave_status", Convert.ToString(leave_status).Trim());
        ies.ies_parameters.Add("@modified_by", Convert.ToString(Session["Usrid"]));
        ies.ies_parameters.Add("@mode", "update");
        DataTable dt_result = ies.Fn_ExecutiveSql_Datatable("UDP_Leave_Type", 1, 1);
        if (dt_result != null && dt_result.Rows.Count > 0)
        {
            if (Convert.ToString(dt_result.Rows[0]["Error"]) == "0")
            {
                Response.Redirect("Adm_Student_Leave_Type.aspx");
            }
        }
    }
    protected void btn_del_ServerClick(object sender, EventArgs e)
    {

        if (Convert.ToBoolean(Session["Access_Trash"]) == true)
        {
            IES_Generic_Function ies = new IES_Generic_Function();
            ies.ies_parameters.Clear();
            ies.ies_parameters.Add("@leave_id", Convert.ToString(hd_id.Value));
            ies.ies_parameters.Add("@mode", "del");
            ies.Fn_ExecutiveSql("UDP_Leave_Type", 1, 1);
            Response.Redirect("Adm_Student_Leave_Type.aspx");
        }
    }
  
   
    protected void lv_leavetype_PagePropertiesChanging(object sender, PagePropertiesChangingEventArgs e)
    {

    }
    protected void lv_inactiveleavetype_PagePropertiesChanging(object sender, PagePropertiesChangingEventArgs e)
    {

    }
    protected void btn_leavetype_ServerClick(object sender, EventArgs e)
    {
        if (ValidInput() == true) { field_mandary.Attributes.Add("style", "color:red"); return; }

        IES_Generic_Function ies = new IES_Generic_Function();
        ies.ies_parameters.Clear();

        string leave_name = Convert.ToString(tbx_leave_type.Text);
        string leave_colour_code = Convert.ToString(tbx_Event_colortype.Text);
        string leave_status = Convert.ToString(ddl_leave_status.SelectedItem.Value);
        string leave_code = Convert.ToString(tbx_leavecode.Text);
        string leave_label = Convert.ToString(tbx_leavelable.Text);

        int Is_exclude = 0;
        if (chk_exclude.Checked == true) Is_exclude = 1;
        

        ies.ies_parameters.Add("@leave_name", Convert.ToString(leave_name));
        ies.ies_parameters.Add("@leave_colour_code", Convert.ToString(leave_colour_code).Trim());
        ies.ies_parameters.Add("@leave_code", Convert.ToString(leave_code).Trim());
        ies.ies_parameters.Add("@leave_label", Convert.ToString(leave_label).Trim());
        ies.ies_parameters.Add("@Is_exclude", Convert.ToString(Is_exclude).Trim());

        ies.ies_parameters.Add("@leave_status", Convert.ToString(leave_status).Trim());
        ies.ies_parameters.Add("@created_by", Convert.ToString(Session["Usrid"]));
        ies.ies_parameters.Add("@mode", "add");
        DataTable dt_result = ies.Fn_ExecutiveSql_Datatable("UDP_Leave_Type", 1, 1);
        if (dt_result != null && dt_result.Rows.Count > 0)
        {
            if (Convert.ToString(dt_result.Rows[0]["Error"]) == "0")
            {
                Response.Redirect("Adm_Student_Leave_Type.aspx");
            }
            else if (Convert.ToString(dt_result.Rows[0]["Error"]) == "1")
            {
                lbl_error.InnerHtml = Convert.ToString(dt_result.Rows[0]["Msg"]);
            }

        }
    }
}