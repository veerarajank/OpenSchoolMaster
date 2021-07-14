using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Adm_academicYears : System.Web.UI.Page
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
                BindPrevYear();
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
    protected void lv_academicyear_PagePropertiesChanging(object sender, PagePropertiesChangingEventArgs e)
    {

    }
    protected void lnk_edit_Command(object sender, CommandEventArgs e)
    {
        string academic_id = string.Empty;
        academic_id = Convert.ToString(e.CommandArgument);
        if (e.CommandName == "edit")
        {
            if (Convert.ToBoolean(Session["Access_Edit"]) == true)
            {

                Response.Redirect("Adm_academicYears.aspx?mode=edit&id=" + academic_id);
            }
        }
    }
    protected void lnk_search_Command(object sender, CommandEventArgs e)
    {
        string academic_id = string.Empty;
        academic_id = Convert.ToString(e.CommandArgument);
        if (e.CommandName == "view")
        {
            if (Convert.ToBoolean(Session["Access_View"]) == true)
            {
               
                Response.Redirect("Adm_academicYears.aspx?mode=view&id=" + academic_id);
            }
        }
    }
    protected void btn_new_ServerClick(object sender, EventArgs e)
    {
        Response.Redirect("Adm_academicYears.aspx?mode=add");
    }
    protected void BindListView()
    {
        IES_Generic_Function ies = new IES_Generic_Function();
        ies.ies_parameters.Clear();
       
        ies.ies_parameters.Add("@mode", "search");
        DataTable dt_data = ies.Fn_ExecutiveSql_Datatable("UDP_Academic_Years", 1, 1);
        if (dt_data != null)
        {
            lbl_cnt.InnerHtml = "&nbsp;Displaying " + Convert.ToString(dt_data.Rows.Count) + " result(s).";
            lv_academicyear.DataSource = dt_data;
            lv_academicyear.DataBind();
        }
    }
    protected void BindDisplay()
    {
        string academic_id = Convert.ToString(Request.QueryString["id"]);
        IES_Generic_Function ies = new IES_Generic_Function();
        ies.ies_parameters.Clear();
        ies.ies_parameters.Add("@academic_id", Convert.ToString(academic_id));
        ies.ies_parameters.Add("@mode", "search");
        DataTable dt_data = ies.Fn_ExecutiveSql_Datatable("UDP_Academic_Years", 1, 1);
        if (dt_data != null && dt_data.Rows.Count > 0)
        {
            lbl_academicyearname.InnerHtml = Convert.ToString(dt_data.Rows[0]["academic_name"]);
            lbl_academicyeardesc.InnerHtml = Convert.ToString(dt_data.Rows[0]["academic_description"]);
            lbl_academicyearstart.InnerHtml = Convert.ToString(dt_data.Rows[0]["academic_start"]);
            lbl_academicyearend.InnerHtml = Convert.ToString(dt_data.Rows[0]["academic_end"]);
            lbl_academicyearsts.Attributes.Add("style", "font-size:12px");
            if (Convert.ToString(dt_data.Rows[0]["academic_status"]) == "1")
            {
                lbl_academicyearsts.InnerText = "Active";
                lbl_academicyearsts.Attributes.Add("class", "label label-success");                
            }
            else
            {
                lbl_academicyearsts.InnerText = "Inactive";
                lbl_academicyearsts.Attributes.Add("class", "label label-warning");
            }
            //Edit Section
            tbx_edit_AcademicYears_name.Text = Convert.ToString(dt_data.Rows[0]["academic_name"]);
            tbx_edit_AcademicYears_start.Text = Convert.ToString(dt_data.Rows[0]["academic_start"]);
            tbx_edit_AcademicYears_end.Text = Convert.ToString(dt_data.Rows[0]["academic_end"]);
            tbx_edit_AcademicYears_description.Value = Convert.ToString(dt_data.Rows[0]["academic_description"]);

            if (ddl_edit_AcademicYears_status.Items.FindByValue(Convert.ToString(dt_data.Rows[0]["academic_status"])) != null)            
            {
                ddl_edit_AcademicYears_status.SelectedValue = Convert.ToString(dt_data.Rows[0]["academic_status"]);
            }

        }
    }
    protected void BindPrevYear()
    {
        IES_Generic_Function ies = new IES_Generic_Function();
        ies.ies_parameters.Clear();

        ies.ies_parameters.Add("@mode", "prevyear");
        DataTable dt_data = ies.Fn_ExecutiveSql_Datatable("UDP_Academic_Years", 1, 1);
        if (dt_data != null && dt_data.Rows.Count>0)
        {
            h3_current_year.InnerHtml = "Current Academic Year :" + Convert.ToString(dt_data.Rows[0]["academic_name"]);
            td_current_year_desc.InnerHtml = Convert.ToString(dt_data.Rows[0]["academic_description"]);
            td_current_year_start.InnerHtml = Convert.ToString(dt_data.Rows[0]["academic_start"]);
            td_current_year_end.InnerHtml = Convert.ToString(dt_data.Rows[0]["academic_end"]);

        }
    }
    protected bool ValidInput()
    {
        bool iserror = false;
        if (tbx_AcademicYears_name.Text.Trim() == "") iserror = true;
        if (tbx_AcademicYears_start.Text.Trim() == "") iserror = true;
        if (tbx_AcademicYears_end.Text.Trim() == "") iserror = true;
        if (tbx_AcademicYears_description.Value.Trim() == "") iserror = true;

        return iserror;

    }
    protected bool EditValidInput()
    {
        bool iserror = false;
        if (tbx_edit_AcademicYears_name.Text.Trim() == "") iserror = true;
        if (tbx_edit_AcademicYears_start.Text.Trim() == "") iserror = true;
        if (tbx_edit_AcademicYears_end.Text.Trim() == "") iserror = true;
        if (tbx_edit_AcademicYears_description.Value.Trim() == "") iserror = true;

        return iserror;

    }
    protected void btn_createyear_ServerClick(object sender, EventArgs e)
    {
        if (ValidInput() == true) { field_mandary.Attributes.Add("style", "color:red"); return; }


        IES_Generic_Function ies = new IES_Generic_Function();
        ies.ies_parameters.Clear();
        
        string AcademicYears_status = Convert.ToString(ddl_AcademicYears_status.SelectedItem.Value);
        string AcademicYears_description = Convert.ToString(tbx_AcademicYears_description.Value);
        string AcademicYears_name = Convert.ToString(tbx_AcademicYears_name.Text);


        string academic_start = "";
        if (tbx_AcademicYears_start.Text.Trim() != "")
        {
            DateTime dtacademic_start = DateTime.ParseExact(tbx_AcademicYears_start.Text.Trim(), "dd-MM-yyyy", null);
            academic_start = dtacademic_start.ToString("yyyy-MM-dd");
        }

        string academic_end = "";
        if (tbx_AcademicYears_end.Text.Trim() != "")
        {
            DateTime dtacademic_end = DateTime.ParseExact(tbx_AcademicYears_end.Text.Trim(), "dd-MM-yyyy", null);
            academic_end = dtacademic_end.ToString("yyyy-MM-dd");
        }

        ies.ies_parameters.Add("@academic_name", Convert.ToString(AcademicYears_name));
        ies.ies_parameters.Add("@academic_start", Convert.ToString(academic_start).Trim());
        ies.ies_parameters.Add("@academic_end", Convert.ToString(academic_end).Trim());
        ies.ies_parameters.Add("@academic_description", Convert.ToString(AcademicYears_description).Trim());
        ies.ies_parameters.Add("@academic_status", Convert.ToString(AcademicYears_status).Trim());
        ies.ies_parameters.Add("@created_by", Convert.ToString(Session["Usrid"]));
        ies.ies_parameters.Add("@mode", "add");
        DataTable dt_result=ies.Fn_ExecutiveSql_Datatable("UDP_Academic_Years", 1, 1);
        if(dt_result!=null && dt_result.Rows.Count>0)
        {
            if (Convert.ToString(dt_result.Rows[0]["Error"]) == "0")
            {
                Response.Redirect("Adm_academicYears.aspx");
            }
            else if (Convert.ToString(dt_result.Rows[0]["Error"]) == "1")
            {
                lbl_error.InnerHtml = Convert.ToString(dt_result.Rows[0]["Msg"]);
            }

        }

        
    }
    protected void btn_edit_ServerClick(object sender, EventArgs e)
    {
        string academic_id = Convert.ToString(Request.QueryString["id"]);
        Response.Redirect("Adm_academicYears.aspx?mode=edit&id=" + academic_id);
    }
    protected void btn_manage_ServerClick(object sender, EventArgs e)
    {
        Response.Redirect("Adm_academicYears.aspx?mode=search");
    }
    protected void btn_update_ServerClick(object sender, EventArgs e)
    {
        string academic_id = Convert.ToString(Request.QueryString["id"]);
        if (EditValidInput() == true) { edit_field_mandary.Attributes.Add("style", "color:red"); return; }


        IES_Generic_Function ies = new IES_Generic_Function();
        ies.ies_parameters.Clear();

        string AcademicYears_status = Convert.ToString(ddl_edit_AcademicYears_status.SelectedItem.Value);
        string AcademicYears_description = Convert.ToString(tbx_edit_AcademicYears_description.Value);
        string AcademicYears_name = Convert.ToString(tbx_edit_AcademicYears_name.Text);


        string academic_start = "";
        if (tbx_edit_AcademicYears_start.Text.Trim() != "")
        {
            DateTime dtacademic_start = DateTime.ParseExact(tbx_edit_AcademicYears_start.Text.Trim(), "dd-MM-yyyy", null);
            academic_start = dtacademic_start.ToString("yyyy-MM-dd");
        }

        string academic_end = "";
        if (tbx_edit_AcademicYears_end.Text.Trim() != "")
        {
            DateTime dtacademic_end = DateTime.ParseExact(tbx_edit_AcademicYears_end.Text.Trim(), "dd-MM-yyyy", null);
            academic_end = dtacademic_end.ToString("yyyy-MM-dd");
        }
        ies.ies_parameters.Add("@academic_id", Convert.ToString(academic_id));
        ies.ies_parameters.Add("@academic_name", Convert.ToString(AcademicYears_name));
        ies.ies_parameters.Add("@academic_start", Convert.ToString(academic_start).Trim());
        ies.ies_parameters.Add("@academic_end", Convert.ToString(academic_end).Trim());
        ies.ies_parameters.Add("@academic_description", Convert.ToString(AcademicYears_description).Trim());
        ies.ies_parameters.Add("@academic_status", Convert.ToString(AcademicYears_status).Trim());
        ies.ies_parameters.Add("@modified_by", Convert.ToString(Session["Usrid"]));
        ies.ies_parameters.Add("@mode", "update");
        DataTable dt_result = ies.Fn_ExecutiveSql_Datatable("UDP_Academic_Years", 1, 1);
        if (dt_result != null && dt_result.Rows.Count > 0)
        {
            if (Convert.ToString(dt_result.Rows[0]["Error"]) == "0")
            {
                Response.Redirect("Adm_academicYears.aspx");
            }
        }
    }
    protected void btn_del_ServerClick(object sender, EventArgs e)
    {
       
        if (Convert.ToBoolean(Session["Access_Trash"]) == true)
        {
            IES_Generic_Function ies = new IES_Generic_Function();
            ies.ies_parameters.Clear();
            ies.ies_parameters.Add("@academic_id", Convert.ToString(hd_id.Value));
            ies.ies_parameters.Add("@mode", "del");
            ies.Fn_ExecutiveSql("UDP_Academic_Years", 1, 1);
            Response.Redirect("Adm_academicYears.aspx");            
        }
    }
}