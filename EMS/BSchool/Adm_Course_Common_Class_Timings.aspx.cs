using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Adm_Course_Common_Class_Timings : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["Usrid"] == null)
        {
            Response.Redirect("login.aspx");
        }
        if (!IsPostBack)
        {

            BindListView();

            search_section.Visible = false;
            view_section.Visible = false;
            add_section.Visible = false;
            update_section.Visible = false;
            delete_section.Visible = false;

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
            else if (mode != null && mode == "del")
            {
                delete_section.Visible = true;
            }
            else if (mode == null)
            {
                search_section.Visible = true;
                BindListView();
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
            ies.Fn_ExecutiveSql("UDP_Common_Class_Timing", 1, 1);
            Response.Redirect("Adm_Course_Common_Class_Timings.aspx?mode=search");
        }
    }
    protected void BindListView()
    {


        IES_Generic_Function ies = new IES_Generic_Function();
        ies.ies_parameters.Clear();
        ies.ies_parameters.Add("@mode", "search");        
        DataTable dt_data = ies.Fn_ExecutiveSql_Datatable("UDP_Common_Class_Timing", 1, 1);
        if (dt_data != null)
        {
            lv_details.DataSource = dt_data;
            lv_details.DataBind();
        }
    }
    protected void lnk_search_Command(object sender, CommandEventArgs e)
    {
        string course_id = Convert.ToString(Request.QueryString["courseid"]);
        string id = string.Empty;
        id = Convert.ToString(e.CommandArgument);
        if (e.CommandName == "view")
        {
            if (Convert.ToBoolean(Session["Access_Edit"]) == true)
            {
                Response.Redirect("Adm_Course_Common_Class_Timings.aspx?mode=view&id=" + id);
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

                Response.Redirect("Adm_Course_Common_Class_Timings.aspx?mode=edit&id=" + id);
            }
        }

    }
    
    protected void btn_update_ServerClick(object sender, EventArgs e)
    {
        string id = Convert.ToString(Request.QueryString["id"]);
        if (EditValidInput() == true) { edit_field_mandary.Attributes.Add("style", "color:red"); return; }


        IES_Generic_Function ies = new IES_Generic_Function();
        ies.ies_parameters.Clear();


        string is_active = "1";

        string name = Convert.ToString(tbx_edit_name.Text); 

        string start_time = "";
        if (tbx_edit_start_time.Text.Trim() != "")
        {
            DateTime dtstart_time = DateTime.ParseExact(tbx_edit_start_time.Text.Trim(), "hh:mm tt", null);
            start_time = dtstart_time.ToString("HH:mm:ss");
        }

        string end_time = "";
        if (tbx_edit_end_time.Text.Trim() != "")
        {
            DateTime dtend_time = DateTime.ParseExact(tbx_edit_end_time.Text.Trim(), "hh:mm tt", null);
            end_time = dtend_time.ToString("HH:mm:ss");
        }

        string is_break = "0";
        if (chk_edit_break.Checked == true) is_break = "1";



         ies.ies_parameters.Add("@id", Convert.ToString(id).Trim());
         ies.ies_parameters.Add("@name", Convert.ToString(name).Trim());
        ies.ies_parameters.Add("@start_time", Convert.ToString(start_time).Trim());
        ies.ies_parameters.Add("@end_time", Convert.ToString(end_time).Trim());
        ies.ies_parameters.Add("@is_status", Convert.ToString(is_active).Trim());
        ies.ies_parameters.Add("@is_break", Convert.ToString(is_break).Trim());


        ies.ies_parameters.Add("@modified_by", Convert.ToString(Session["Usrid"]));
        ies.ies_parameters.Add("@mode", "update");
        DataTable dt_result = ies.Fn_ExecutiveSql_Datatable("UDP_Common_Class_Timing", 1, 1);
        if (dt_result != null && dt_result.Rows.Count > 0)
        {
            if (Convert.ToString(dt_result.Rows[0]["Error"]) == "0")
            {
                Response.Redirect("Adm_Course_Common_Class_Timings.aspx?mode=search");
            }
            else if (Convert.ToString(dt_result.Rows[0]["Error"]) == "1")
            {
                lbl_error.InnerHtml = Convert.ToString(dt_result.Rows[0]["Msg"]);
            }

        }
    }
    protected void btn_edit_ServerClick(object sender, EventArgs e)
    {
        string id = Convert.ToString(Request.QueryString["id"]);
        Response.Redirect("Adm_Course_Common_Class_Timings.aspx?mode=edit&id=" + id);
    }
    protected void btn_manage_ServerClick(object sender, EventArgs e)
    {
        Response.Redirect("Adm_Course_Common_Class_Timings.aspx?mode=search");
    }
    protected void BindDisplay()
    {
        string id = Convert.ToString(Request.QueryString["id"]);
        IES_Generic_Function ies = new IES_Generic_Function();
        ies.ies_parameters.Clear();
        ies.ies_parameters.Add("@id", Convert.ToString(id));
        ies.ies_parameters.Add("@mode", "search");
        DataTable dt_data = ies.Fn_ExecutiveSql_Datatable("UDP_Common_Class_Timing", 1, 1);
        if (dt_data != null && dt_data.Rows.Count > 0)
        {
            lbl_name.InnerHtml = Convert.ToString(dt_data.Rows[0]["name"]);
            lbl_starttime.InnerHtml = Convert.ToString(Convert.ToDateTime(dt_data.Rows[0]["start_time"]).ToString("hh:mm tt"));
            lbl_endtime.InnerHtml = Convert.ToString(Convert.ToDateTime(dt_data.Rows[0]["end_time"]).ToString("hh:mm tt"));
            lbl_breaksts.Attributes.Add("style", "font-size:12px");
            if (Convert.ToString(dt_data.Rows[0]["is_break"]) == "1")
            {
                lbl_breaksts.InnerHtml = "<i class='fa fa-check'></i>";
                lbl_breaksts.Attributes.Add("class", "label label-success");
            }
            else
            {
                lbl_breaksts.InnerHtml = "<i class='fa fa-remove'></i>";
                lbl_breaksts.Attributes.Add("class", "label label-warning");
            }
            //Edit Section
            tbx_edit_name.Text = Convert.ToString(dt_data.Rows[0]["name"]);
            tbx_edit_start_time.Text = Convert.ToString(Convert.ToDateTime(dt_data.Rows[0]["start_time"]).ToString("hh:mm tt"));
            tbx_edit_end_time.Text = Convert.ToString(Convert.ToDateTime(dt_data.Rows[0]["end_time"]).ToString("hh:mm tt"));

            chk_edit_break.Checked = false;
            if (Convert.ToString(dt_data.Rows[0]["is_break"]) == "1")
            {
                chk_edit_break.Checked = true;
            }

            

        }
    }
    
    protected bool ValidInput()
    {
        bool iserror = false;
        if (tbx_name.Text.Trim() == "") iserror = true;
        if (tbx_start_time.Text.Trim() == "") iserror = true;
        if (tbx_end_time.Text.Trim() == "") iserror = true;


        return iserror;

    }
    protected bool EditValidInput()
    {
        bool iserror = false;
        if (tbx_edit_name.Text.Trim() == "") iserror = true;
        if (tbx_edit_start_time.Text.Trim() == "") iserror = true;
        if (tbx_edit_end_time.Text.Trim() == "") iserror = true;
        return iserror;
    }
    
   

    protected void btn_manage_classtiming_Click(object sender, EventArgs e)
    {
        Response.Redirect("Adm_Course_Common_Class_Timings.aspx?mode=search");
    }
    protected void btn_newtiming_ServerClick(object sender, EventArgs e)
    {
        if (ValidInput() == true) { field_mandary.Attributes.Add("style", "color:red"); return; }


        IES_Generic_Function ies = new IES_Generic_Function();
        ies.ies_parameters.Clear();

        string is_active = "1";

        string name = Convert.ToString(tbx_name.Text);

        string start_time = "";
        if (tbx_start_time.Text.Trim() != "")
        {
            DateTime dtstart_time = DateTime.ParseExact(tbx_start_time.Text.Trim(), "hh:mm tt", null);
            start_time = dtstart_time.ToString("HH:mm:ss");
        }

        string end_time = "";
        if (tbx_end_time.Text.Trim() != "")
        {
            DateTime dtend_time = DateTime.ParseExact(tbx_end_time.Text.Trim(), "hh:mm tt", null);
            end_time = dtend_time.ToString("HH:mm:ss");
        }

        string is_break = "0";
        if (chk_break.Checked == true) is_break = "1";



     
        ies.ies_parameters.Add("@name", Convert.ToString(name).Trim());
        ies.ies_parameters.Add("@start_time", Convert.ToString(start_time).Trim());
        ies.ies_parameters.Add("@end_time", Convert.ToString(end_time).Trim());
        ies.ies_parameters.Add("@is_status", Convert.ToString(is_active).Trim());
        ies.ies_parameters.Add("@is_break", Convert.ToString(is_break).Trim());
        ies.ies_parameters.Add("@created_by", Convert.ToString(Session["Usrid"]));



        ies.ies_parameters.Add("@mode", "add");
        DataTable dt_result = ies.Fn_ExecutiveSql_Datatable("UDP_Common_Class_Timing", 1, 1);
        if (dt_result != null && dt_result.Rows.Count > 0)
        {
            if (Convert.ToString(dt_result.Rows[0]["Error"]) == "0")
            {

                Response.Redirect("Adm_Course_Common_Class_Timings.aspx?mode=search");
            }
            else if (Convert.ToString(dt_result.Rows[0]["Error"]) == "1")
            {
                lbl_error.InnerHtml = Convert.ToString(dt_result.Rows[0]["Msg"]);
            }

        }
    }
    protected void btn_addclasstiming_up_screen_Click(object sender, EventArgs e)
    {
        Response.Redirect("Adm_Course_Common_Class_Timings.aspx?mode=add");
    }
    protected void btn_manageclasstiming_up_screen_Click(object sender, EventArgs e)
    {
        Response.Redirect("Adm_Course_Common_Class_Timings.aspx?mode=search");
    }

    protected void btn_new_ServerClick(object sender, EventArgs e)
    {
        Response.Redirect("Adm_Course_Common_Class_Timings.aspx?mode=add");
    }
    
}