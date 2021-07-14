using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Adm_eventsType : System.Web.UI.Page
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
        string event_id = string.Empty;
        event_id = Convert.ToString(e.CommandArgument);
        if (e.CommandName == "edit")
        {
            if (Convert.ToBoolean(Session["Access_Edit"]) == true)
            {

                Response.Redirect("Adm_eventsType.aspx?mode=edit&id=" + event_id);
            }
        }
    }
    protected void lnk_search_Command(object sender, CommandEventArgs e)
    {
        string event_id = string.Empty;
        event_id = Convert.ToString(e.CommandArgument);
        if (e.CommandName == "view")
        {
            if (Convert.ToBoolean(Session["Access_View"]) == true)
            {

                Response.Redirect("Adm_eventsType.aspx?mode=view&id=" + event_id);
            }
        }
    }
    protected void btn_new_ServerClick(object sender, EventArgs e)
    {
        Response.Redirect("Adm_eventsType.aspx?mode=add");
    }
    protected void BindListView()
    {
        IES_Generic_Function ies = new IES_Generic_Function();
        ies.ies_parameters.Clear();

        ies.ies_parameters.Add("@mode", "search");
        DataTable dt_data = ies.Fn_ExecutiveSql_Datatable("UDP_Event_Type", 1, 1);
        if (dt_data != null)
        {
            lbl_cnt.InnerHtml = "&nbsp;Displaying " + Convert.ToString(dt_data.Rows.Count) + " result(s).";
            lv_eventtype.DataSource = dt_data;
            lv_eventtype.DataBind();
        }
    }
    protected void BindDisplay()
    {
        string event_id = Convert.ToString(Request.QueryString["id"]);
        IES_Generic_Function ies = new IES_Generic_Function();
        ies.ies_parameters.Clear();
        ies.ies_parameters.Add("@event_id", Convert.ToString(event_id));
        ies.ies_parameters.Add("@mode", "search");
        DataTable dt_data = ies.Fn_ExecutiveSql_Datatable("UDP_Event_Type", 1, 1);
        if (dt_data != null && dt_data.Rows.Count > 0)
        {
           
            //Edit Section
            tbx_edit_Event_type.Text = Convert.ToString(dt_data.Rows[0]["event_name"]);
            tbx_edit_Event_colortype.Text = Convert.ToString(dt_data.Rows[0]["event_colour_code"]);
            if (ddl_edit_Eventtype_status.Items.FindByValue(Convert.ToString(dt_data.Rows[0]["event_status"])) != null)
            {
                ddl_edit_Eventtype_status.SelectedValue = Convert.ToString(dt_data.Rows[0]["event_status"]);
            }

            lbl_eventname.InnerText = Convert.ToString(dt_data.Rows[0]["event_name"]);
            lbl_colorcode.InnerText =Convert.ToString(dt_data.Rows[0]["event_colour_code"]);
            lbl_viewcolorcode.Attributes.Add("style", "height: 22px; width: 22px; background: none center center no-repeat " + Convert.ToString(dt_data.Rows[0]["event_colour_code"]) + "; vertical-align: middle; margin: 0px 0.25em; display: inline-block; color: rgb(255, 255, 255);");

            if (Convert.ToString(dt_data.Rows[0]["event_status"]) == "1")
            {
                lbl_eventsts.InnerText = "Active";
                lbl_eventsts.Attributes.Add("class", "label label-success");
            }
            else
            {
                lbl_eventsts.InnerText = "Inactive";
                lbl_eventsts.Attributes.Add("class", "label label-warning");
            }

        }
    }
   
    protected bool ValidInput()
    {
        bool iserror = false;
        if (tbx_Event_type.Text.Trim() == "") iserror = true;
        if (tbx_Event_colortype.Text.Trim() == "") iserror = true;
       

        return iserror;

    }
    protected bool EditValidInput()
    {
        bool iserror = false;
        if (tbx_edit_Event_type.Text.Trim() == "") iserror = true;
        if (tbx_edit_Event_colortype.Text.Trim() == "") iserror = true;
       

        return iserror;

    }
    
    protected void btn_edit_ServerClick(object sender, EventArgs e)
    {
        string event_id = Convert.ToString(Request.QueryString["id"]);
        Response.Redirect("Adm_eventsType.aspx?mode=edit&id=" + event_id);
    }
    protected void btn_manage_ServerClick(object sender, EventArgs e)
    {
        Response.Redirect("Adm_eventsType.aspx?mode=search");
    }
    protected void btn_update_ServerClick(object sender, EventArgs e)
    {
        string event_id = Convert.ToString(Request.QueryString["id"]);
        if (EditValidInput() == true) { edit_field_mandary.Attributes.Add("style", "color:red"); return; }


        IES_Generic_Function ies = new IES_Generic_Function();
        ies.ies_parameters.Clear();

        string event_name = Convert.ToString(tbx_edit_Event_type.Text);
        string event_colour_code = Convert.ToString(tbx_edit_Event_colortype.Text);
        string event_status = Convert.ToString(ddl_edit_Eventtype_status.SelectedItem.Value);


        ies.ies_parameters.Add("@event_id", Convert.ToString(event_id));
        ies.ies_parameters.Add("@event_name", Convert.ToString(event_name));
        ies.ies_parameters.Add("@event_colour_code", Convert.ToString(event_colour_code).Trim());
        ies.ies_parameters.Add("@event_status", Convert.ToString(event_status).Trim());
        ies.ies_parameters.Add("@modified_by", Convert.ToString(Session["Usrid"]));
        ies.ies_parameters.Add("@mode", "update");
        DataTable dt_result = ies.Fn_ExecutiveSql_Datatable("UDP_Event_Type", 1, 1);
        if (dt_result != null && dt_result.Rows.Count > 0)
        {
            if (Convert.ToString(dt_result.Rows[0]["Error"]) == "0")
            {
                Response.Redirect("Adm_eventsType.aspx");
            }
        }
    }
    protected void btn_del_ServerClick(object sender, EventArgs e)
    {

        if (Convert.ToBoolean(Session["Access_Trash"]) == true)
        {
            IES_Generic_Function ies = new IES_Generic_Function();
            ies.ies_parameters.Clear();
            ies.ies_parameters.Add("@event_id", Convert.ToString(hd_id.Value));
            ies.ies_parameters.Add("@mode", "del");
            ies.Fn_ExecutiveSql("UDP_Event_Type", 1, 1);
            Response.Redirect("Adm_eventsType.aspx");
        }
    }
    protected void btn_createeventtype_ServerClick(object sender, EventArgs e)
    {
        if (ValidInput() == true) { field_mandary.Attributes.Add("style", "color:red"); return; }

        IES_Generic_Function ies = new IES_Generic_Function();
        ies.ies_parameters.Clear();
      
        string event_name = Convert.ToString(tbx_Event_type.Text);
        string event_colour_code = Convert.ToString(tbx_Event_colortype.Text);
        string event_status = Convert.ToString(ddl_Eventtype_status.SelectedItem.Value);


        ies.ies_parameters.Add("@event_name", Convert.ToString(event_name));
        ies.ies_parameters.Add("@event_colour_code", Convert.ToString(event_colour_code).Trim());
        ies.ies_parameters.Add("@event_status", Convert.ToString(event_status).Trim());
        ies.ies_parameters.Add("@created_by", Convert.ToString(Session["Usrid"]));
        ies.ies_parameters.Add("@mode", "add");
        DataTable dt_result = ies.Fn_ExecutiveSql_Datatable("UDP_Event_Type", 1, 1);
        if (dt_result != null && dt_result.Rows.Count > 0)
        {
            if (Convert.ToString(dt_result.Rows[0]["Error"]) == "0")
            {
                Response.Redirect("Adm_eventsType.aspx");
            }
            else if (Convert.ToString(dt_result.Rows[0]["Error"]) == "1")
            {
                lbl_error.InnerHtml = Convert.ToString(dt_result.Rows[0]["Msg"]);
            }

        }
    }
    protected void lv_eventtype_PagePropertiesChanging(object sender, PagePropertiesChangingEventArgs e)
    {

    }
}