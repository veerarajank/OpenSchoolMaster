using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Adm_Course_Batches_Semester_List : System.Web.UI.Page
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
            ies.Fn_Executive_ListWithSelect(drp_academic_year, "select academic_id as academic_id,academic_name from tbl_academic_years where academic_status=1 order by academic_id", "academic_id", "academic_name");


            ies = new IES_Generic_Function();
            ies.Fn_Executive_ListWithSelect(drp_semester, "select id as id,name from tbl_semester where status=1 order by id", "id", "name");


            string course_id = Convert.ToString(Request.QueryString["courseid"]);
            

            string academic_id = "0";
            ies = new IES_Generic_Function();
            ies.ies_parameters.Clear();
            DataTable dt_data = ies.Fn_ExecutiveSql_Datatable("select * from tbl_setcurrentacademicyear_settings where id='1'", 0, 0);
            if (dt_data != null && dt_data.Rows.Count > 0)
            {
                academic_id = Convert.ToString(dt_data.Rows[0]["academic_id"]);

                if (drp_academic_year.Items.FindByValue(Convert.ToString(dt_data.Rows[0]["academic_id"])) != null)
                {
                    drp_academic_year.SelectedValue = Convert.ToString(dt_data.Rows[0]["academic_id"]);
                }
            }

            string acid = Convert.ToString(Request.QueryString["acid"]);
            if (drp_academic_year.Items.FindByValue(acid) != null)
            {
                drp_academic_year.SelectedValue = Convert.ToString(acid);
                academic_id = acid;
            }

            BindListView(academic_id, course_id);

            search_section.Visible = false;
            view_section.Visible = false;
            add_section.Visible = false;
            update_section.Visible = false;
            delete_section.Visible = false;

            string mode = Convert.ToString(Request.QueryString["mode"]);
            if (mode != null && mode == "search")
            {
                search_section.Visible = true;
                BindListView(academic_id, course_id);
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
                BindListView(academic_id, course_id);
            }




        }


    }
    protected void drp_academic_year_SelectedIndexChanged(object sender, EventArgs e)
    {
       string course_id = Convert.ToString(Request.QueryString["courseid"]);       
        string mode = Convert.ToString(Request.QueryString["mode"]);
        if (mode == "" || mode == "search")
        {

            string academic_id = "0";
            academic_id = Convert.ToString(drp_academic_year.SelectedItem.Value);
            BindListView(academic_id, course_id);
        }
        else
        {
            Response.Redirect("Adm_Course_Batches_Semester_List.aspx?mode=search&courseid=" + course_id);
        }
    }
    protected void btn_new_Click(object sender, EventArgs e)
    {
        string course_id = Convert.ToString(Request.QueryString["courseid"]);        
        string academic_id = "0";
        academic_id = Convert.ToString(drp_academic_year.SelectedItem.Value);
        if (academic_id != "0")
        {
            Response.Redirect("Adm_Course_Batches_Semester_List.aspx?mode=add&courseid=" + course_id + "&acid=" + academic_id);
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
            ies.Fn_ExecutiveSql("UDP_Course_Semester_Assign", 1, 1);
            Bind_Redirect_Page(2, Convert.ToString(hd_id.Value));
        }
    }
    protected void BindListView(string academic_id, string course_id)
    {


        IES_Generic_Function ies = new IES_Generic_Function();
        ies.ies_parameters.Clear();
        ies.ies_parameters.Add("@mode", "academicsemestersearch");
        ies.ies_parameters.Add("@academic_id", academic_id);
        ies.ies_parameters.Add("@course_id", course_id);        
        DataTable dt_data = ies.Fn_ExecutiveSql_Datatable("UDP_Course_Semester_Assign", 1, 1);
        if (dt_data != null)
        {
            if (dt_data.Rows.Count > 0)
            {
             
                lbl_displaycourse.Text = Convert.ToString(dt_data.Rows[0]["Course"]);
            }
            else
            {
                ies = new IES_Generic_Function();
                ies.ies_parameters.Clear();
                ies.ies_parameters.Add("@mode", "search");
                ies.ies_parameters.Add("@course_id", course_id);                
                DataTable dt_data_result = ies.Fn_ExecutiveSql_Datatable("UDP_Batches", 1, 1);
                if (dt_data_result != null)
                {
                    if (dt_data_result.Rows.Count > 0)
                    {
                      
                        lbl_displaycourse.Text = Convert.ToString(dt_data_result.Rows[0]["course_name"]);

                    }
                }
            }
            lv_semesterdetails.DataSource = dt_data;
            lv_semesterdetails.DataBind();
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
                
                Response.Redirect("Adm_Course_Batches_Semester_List.aspx?mode=view&courseid=" + course_id + "&id=" + id);
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
                string course_id = Convert.ToString(Request.QueryString["courseid"]);
                Response.Redirect("Adm_Course_Batches_Semester_List.aspx?mode=edit&courseid=" + course_id + "&id=" + id);
            }
        }

    }
    protected void btn_newsemester_ServerClick(object sender, EventArgs e)
    {
        if (ValidInput() == true) { field_mandary.Attributes.Add("style", "color:red"); return; }


        IES_Generic_Function ies = new IES_Generic_Function();
        ies.ies_parameters.Clear();

        string course_id = Convert.ToString(Request.QueryString["courseid"]);        
        string academic_id = Convert.ToString(Request.QueryString["acid"]);

        string is_active = Convert.ToString(ddl_semester_status.SelectedItem.Value);

        string semester_id = Convert.ToString(drp_semester.SelectedItem.Value);


        string semester_start_date = "";
        if (tbx_semester_start.Text.Trim() != "")
        {
            DateTime dtsemester_start_date = DateTime.ParseExact(tbx_semester_start.Text.Trim(), "dd-MM-yyyy", null);
            semester_start_date = dtsemester_start_date.ToString("yyyy-MM-dd");
        }

        string semester_end_date = "";
        if (tbx_semester_end.Text.Trim() != "")
        {
            DateTime dtsemester_end_date = DateTime.ParseExact(tbx_semester_end.Text.Trim(), "dd-MM-yyyy", null);
            semester_end_date = dtsemester_end_date.ToString("yyyy-MM-dd");
        }

       
        ies.ies_parameters.Add("@course_id", Convert.ToString(course_id));
        ies.ies_parameters.Add("@academic_id", Convert.ToString(academic_id));

        ies.ies_parameters.Add("@semester_id", Convert.ToString(semester_id));
        ies.ies_parameters.Add("@semester_start_date", Convert.ToString(semester_start_date).Trim());
        ies.ies_parameters.Add("@semester_end_date", Convert.ToString(semester_end_date).Trim());
        ies.ies_parameters.Add("@is_active", Convert.ToString(is_active).Trim());



        ies.ies_parameters.Add("@mode", "add");
        DataTable dt_result = ies.Fn_ExecutiveSql_Datatable("UDP_Course_Semester_Assign", 1, 1);
        if (dt_result != null && dt_result.Rows.Count > 0)
        {
            if (Convert.ToString(dt_result.Rows[0]["Error"]) == "0")
            {

                Response.Redirect("Adm_Course_Batches_Semester_List.aspx?mode=search&courseid=" + course_id + "&acid=" + academic_id);
            }
            else if (Convert.ToString(dt_result.Rows[0]["Error"]) == "1")
            {
                lbl_error.InnerHtml = Convert.ToString(dt_result.Rows[0]["Msg"]);
            }

        }
    }
    protected void btn_update_ServerClick(object sender, EventArgs e)
    {
        string id = Convert.ToString(Request.QueryString["id"]);
        if (EditValidInput() == true) { edit_field_mandary.Attributes.Add("style", "color:red"); return; }


        IES_Generic_Function ies = new IES_Generic_Function();
        ies.ies_parameters.Clear();


        string is_active = Convert.ToString(ddl_edit_semester_status.SelectedItem.Value);

        string semester_start_date = "";
        if (tbx_edit_semester_start.Text.Trim() != "")
        {
            DateTime dtsemester_start_date = DateTime.ParseExact(tbx_edit_semester_start.Text.Trim(), "dd-MM-yyyy", null);
            semester_start_date = dtsemester_start_date.ToString("yyyy-MM-dd");
        }

        string semester_end_date = "";
        if (tbx_edit_semester_end.Text.Trim() != "")
        {
            DateTime dtsemester_end_date = DateTime.ParseExact(tbx_edit_semester_end.Text.Trim(), "dd-MM-yyyy", null);
            semester_end_date = dtsemester_end_date.ToString("yyyy-MM-dd");
        }



        ies.ies_parameters.Add("@id", Convert.ToString(id).Trim());
        ies.ies_parameters.Add("@semester_start_date", Convert.ToString(semester_start_date).Trim());
        ies.ies_parameters.Add("@semester_end_date", Convert.ToString(semester_end_date).Trim());
        ies.ies_parameters.Add("@is_active", Convert.ToString(is_active).Trim());



        ies.ies_parameters.Add("@mode", "update");
        DataTable dt_result = ies.Fn_ExecutiveSql_Datatable("UDP_Course_Semester_Assign", 1, 1);
        if (dt_result != null && dt_result.Rows.Count > 0)
        {
            if (Convert.ToString(dt_result.Rows[0]["Error"]) == "0")
            {
                string course_id = Convert.ToString(dt_result.Rows[0]["course_id"]);               
                string academic_id = Convert.ToString(dt_result.Rows[0]["academic_id"]);

                Response.Redirect("Adm_Course_Batches_Semester_List.aspx?mode=search&courseid=" + course_id + "&acid=" + academic_id);
            }
            else if (Convert.ToString(dt_result.Rows[0]["Error"]) == "1")
            {
                lbl_error.InnerHtml = Convert.ToString(dt_result.Rows[0]["Msg"]);
            }

        }
    }
   
   
    protected void BindDisplay()
    {
        string id = Convert.ToString(Request.QueryString["id"]);
        IES_Generic_Function ies = new IES_Generic_Function();
        ies.ies_parameters.Clear();
        ies.ies_parameters.Add("@id", Convert.ToString(id));
        ies.ies_parameters.Add("@mode", "search");
        DataTable dt_data = ies.Fn_ExecutiveSql_Datatable("UDP_Course_Semester_Assign", 1, 1);
        if (dt_data != null && dt_data.Rows.Count > 0)
        {
            lbl_semestername.InnerHtml = Convert.ToString(dt_data.Rows[0]["Semester"]);
            lbl_semestertart.InnerHtml = Convert.ToString(dt_data.Rows[0]["semester_start_date"]);
            lbl_semesterend.InnerHtml = Convert.ToString(dt_data.Rows[0]["semester_end_date"]);
            lbl_semestersts.Attributes.Add("style", "font-size:12px");
            if (Convert.ToString(dt_data.Rows[0]["is_active"]) == "1")
            {
                lbl_semestersts.InnerText = "Active";
                lbl_semestersts.Attributes.Add("class", "label label-success");
            }
            else
            {
                lbl_semestersts.InnerText = "Inactive";
                lbl_semestersts.Attributes.Add("class", "label label-warning");
            }
            //Edit Section
            tbx_edit_semester_name.Text = Convert.ToString(dt_data.Rows[0]["Semester"]);
            tbx_edit_semester_start.Text = Convert.ToString(dt_data.Rows[0]["semester_start_date"]);
            tbx_edit_semester_end.Text = Convert.ToString(dt_data.Rows[0]["semester_end_date"]);


            if (ddl_edit_semester_status.Items.FindByValue(Convert.ToString(dt_data.Rows[0]["is_active"])) != null)
            {
                ddl_edit_semester_status.SelectedValue = Convert.ToString(dt_data.Rows[0]["is_active"]);
            }

        }
    }
   
    protected bool ValidInput()
    {
        bool iserror = false;
        if (drp_semester.SelectedValue == "0") iserror = true;
        if (tbx_semester_start.Text.Trim() == "") iserror = true;
        if (tbx_semester_end.Text.Trim() == "") iserror = true;


        return iserror;

    }
    protected bool EditValidInput()
    {
        bool iserror = false;
        if (tbx_edit_semester_start.Text.Trim() == "") iserror = true;
        if (tbx_edit_semester_end.Text.Trim() == "") iserror = true;
        return iserror;
    }
   
    protected void Bind_Redirect_Page(int mode, string id)
    {

        IES_Generic_Function ies = new IES_Generic_Function();
        ies.ies_parameters.Clear();
        ies.ies_parameters.Add("@id", Convert.ToString(id));
        ies.ies_parameters.Add("@mode", "edit");
        DataTable dt_data = ies.Fn_ExecutiveSql_Datatable("UDP_Course_Semester_Assign", 1, 1);
        if (dt_data != null && dt_data.Rows.Count > 0)
        {
            string course_id = Convert.ToString(dt_data.Rows[0]["course_id"]);          
            string academic_id = Convert.ToString(dt_data.Rows[0]["academic_id"]);

            if (mode == 1)
            {
                Response.Redirect("Adm_Course_Batches_Semester_List.aspx?mode=add&courseid=" + course_id  + "&acid=" + academic_id);
            }
            else if (mode == 2)
            {
                Response.Redirect("Adm_Course_Batches_Semester_List.aspx?mode=search&courseid=" + course_id  + "&acid=" + academic_id);
            }

        }
    }

    protected void btn_cancel_ServerClick(object sender, EventArgs e)
    {
        string course_id = Convert.ToString(Request.QueryString["courseid"]);
        Response.Redirect("Adm_Course_Batches_Semester_List.aspx?mode=search&courseid=" + course_id);
        
    }
    protected void lnk_subject_Command(object sender, CommandEventArgs e)
    {
        string id = string.Empty;
        id = Convert.ToString(e.CommandArgument);
        if (e.CommandName == "subject")
        {
            if (Convert.ToBoolean(Session["Access_Edit"]) == true)
            {
                string course_id = Convert.ToString(Request.QueryString["courseid"]);
                Response.Redirect("Adm_Course_Batches_Semester_SubjectList.aspx?id=" + id);
            }
        }
    }
}