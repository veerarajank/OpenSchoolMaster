using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Adm_CreateCourses : System.Web.UI.Page
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

                Response.Redirect("Adm_CreateCourses.aspx?mode=edit&id=" + id);
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

                Response.Redirect("Adm_CreateCourses.aspx?mode=view&id=" + id);
            }
        }
    }
    protected void btn_new_ServerClick(object sender, EventArgs e)
    {
        Response.Redirect("Adm_CreateCourses.aspx?mode=add");
    }
    protected void BindListView()
    {
        IES_Generic_Function ies = new IES_Generic_Function();
        ies.ies_parameters.Clear();

        ies.ies_parameters.Add("@mode", "search");
        DataTable dt_data = ies.Fn_ExecutiveSql_Datatable("UDP_Course", 1, 1);
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
        DataTable dt_data = ies.Fn_ExecutiveSql_Datatable("UDP_Course", 1, 1);
        if (dt_data != null && dt_data.Rows.Count > 0)
        {

            //Edit Section
            tbx_edit_name.Text = Convert.ToString(dt_data.Rows[0]["name"]);
            tbx_edit_code.Text = Convert.ToString(dt_data.Rows[0]["code"]);
            if (ddl_edit_status.Items.FindByValue(Convert.ToString(dt_data.Rows[0]["status"])) != null)
            {
                ddl_edit_status.SelectedValue = Convert.ToString(dt_data.Rows[0]["status"]);
            }

            if (Convert.ToString(dt_data.Rows[0]["semester_enabled"]) == "1")
            {
                chk_edit_enable_semester.Checked = true;
            }
            else
            {
                chk_edit_enable_semester.Checked = false;
            }

            //View Section

            lbl_name.InnerText = Convert.ToString(dt_data.Rows[0]["name"]);
            lbl_code.InnerText = Convert.ToString(dt_data.Rows[0]["code"]);

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

            if (Convert.ToString(dt_data.Rows[0]["semester_enabled"]) == "1")
            {
                view_chked.Visible = true;
                view_unchked.Visible = false;
            }
            else
            {
                view_unchked.Visible = true;
                view_chked.Visible = false;
            }



        }
    }

    protected bool ValidInput()
    {
        bool iserror = false;
        if (tbx_name.Text.Trim() == "") iserror = true;
        if (tbx_code.Text.Trim() == "") iserror = true;


        return iserror;

    }
    protected bool EditValidInput()
    {
        bool iserror = false;
        if (tbx_edit_name.Text.Trim() == "") iserror = true;
        if (tbx_edit_code.Text.Trim() == "") iserror = true;


        return iserror;

    }

    protected void btn_edit_ServerClick(object sender, EventArgs e)
    {
        string id = Convert.ToString(Request.QueryString["id"]);
        Response.Redirect("Adm_CreateCourses.aspx?mode=edit&id=" + id);
    }
    protected void btn_manage_ServerClick(object sender, EventArgs e)
    {
        Response.Redirect("Adm_CreateCourses.aspx?mode=search");
    }
    protected void btn_update_ServerClick(object sender, EventArgs e)
    {
        string id = Convert.ToString(Request.QueryString["id"]);
        if (EditValidInput() == true) { edit_field_mandary.Attributes.Add("style", "color:red"); return; }


        IES_Generic_Function ies = new IES_Generic_Function();
        ies.ies_parameters.Clear();

        string name = Convert.ToString(tbx_edit_name.Text);
        string code = Convert.ToString(tbx_edit_code.Text);
        string status = Convert.ToString(ddl_edit_status.SelectedItem.Value);

        ies.ies_parameters.Add("@id", Convert.ToString(id));
        ies.ies_parameters.Add("@name", Convert.ToString(name));
        ies.ies_parameters.Add("@code", Convert.ToString(code));

        int semester_enabled = 0;
        if (chk_edit_enable_semester.Checked == true) semester_enabled = 1;

        ies.ies_parameters.Add("@semester_enabled", Convert.ToString(semester_enabled));
        ies.ies_parameters.Add("@exam_format", Convert.ToString("1"));  //1 Default        

        ies.ies_parameters.Add("@status", Convert.ToString(status));

        ies.ies_parameters.Add("@mode", "update");
        DataTable dt_result = ies.Fn_ExecutiveSql_Datatable("UDP_Course", 1, 1);
        if (dt_result != null && dt_result.Rows.Count > 0)
        {
            if (Convert.ToString(dt_result.Rows[0]["Error"]) == "0")
            {
                Response.Redirect("Adm_CreateCourses.aspx");
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
            ies.Fn_ExecutiveSql("UDP_Course", 1, 1);
            Response.Redirect("Adm_CreateCourses.aspx");
        }
    }
    protected void btn_save_ServerClick(object sender, EventArgs e)
    {
        if (ValidInput() == true) { field_mandary.Attributes.Add("style", "color:red"); return; }

        IES_Generic_Function ies = new IES_Generic_Function();
        ies.ies_parameters.Clear();

        string name = Convert.ToString(tbx_name.Text);
        string code = Convert.ToString(tbx_code.Text);
        string status = Convert.ToString(ddl_status.SelectedItem.Value);


        ies.ies_parameters.Add("@name", Convert.ToString(name));
        ies.ies_parameters.Add("@code", Convert.ToString(code));
        ies.ies_parameters.Add("@status", Convert.ToString(status));

        int semester_enabled = 0;
        if (chk_enable_semester.Checked == true) semester_enabled = 1;

        ies.ies_parameters.Add("@semester_enabled", Convert.ToString(semester_enabled));
        ies.ies_parameters.Add("@exam_format", Convert.ToString("1"));  //1 Default        


        ies.ies_parameters.Add("@mode", "add");
        DataTable dt_result = ies.Fn_ExecutiveSql_Datatable("UDP_Course", 1, 1);
        if (dt_result != null && dt_result.Rows.Count > 0)
        {
            if (Convert.ToString(dt_result.Rows[0]["Error"]) == "0")
            {
                Response.Redirect("Adm_CreateCourses.aspx");
            }
            else if (Convert.ToString(dt_result.Rows[0]["Error"]) == "1")
            {
                lbl_error.InnerHtml = Convert.ToString(dt_result.Rows[0]["Msg"]);
            }

        }
    }

    protected void lv_listview_PagePropertiesChanging(object sender, PagePropertiesChangingEventArgs e)
    {
        (lv_listview.FindControl("DataPager1") as DataPager).SetPageProperties(e.StartRowIndex, e.MaximumRows, false);
        this.BindListView();
    }
}