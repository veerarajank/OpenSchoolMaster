using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Adm_StudentInfo_Log : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["Usrid"] == null)
        {
            Response.Redirect("login.aspx");
        }
        if (!IsPostBack)
        {
            string id = Convert.ToString(Request.QueryString["id"]);
            img_student.ImageUrl = "ImgHandler.ashx?imgid=" + id;

            IES_Generic_Function ies = new IES_Generic_Function();
            ies.Fn_Executive_ListWithSelect_WithTitle(drp_category, "--Select Log Category--", "select log_id, log_name from tbl_students_log	order by log_name", "log_id", "log_name");

            BindDisplay();

            BindListView();

        }
    }
    protected void BindDisplay()
    {
        string id = Convert.ToString(Request.QueryString["id"]);
        IES_Generic_Function ies = new IES_Generic_Function();
        ies.ies_parameters.Clear();
        ies.ies_parameters.Add("@id", Convert.ToString(id));
        ies.ies_parameters.Add("@mode", "search");
        DataTable dt_data = ies.Fn_ExecutiveSql_Datatable("UDP_Create_Student", 1, 1);
        if (dt_data != null && dt_data.Rows.Count > 0)
        {
            lbl_studentaddmissionno.InnerHtml = "<strong>Admission No&nbsp;:</strong>&nbsp;" + Convert.ToString(dt_data.Rows[0]["admission_no"]);
            lbl_studentbatch.InnerHtml = "<strong>Batch&nbsp;:</strong>&nbsp;" + Convert.ToString(dt_data.Rows[0]["batchname"]);
            lbl_studentcourse.InnerHtml = "<strong>Course&nbsp;:</strong>&nbsp;" + Convert.ToString(dt_data.Rows[0]["course_name"]);

            lbl_studentname.InnerHtml = (Convert.ToString(dt_data.Rows[0]["first_name"]) + " " + Convert.ToString(dt_data.Rows[0]["middle_name"]) + " " + Convert.ToString(dt_data.Rows[0]["last_name"]));


            lbl_studentmailid.InnerHtml = Convert.ToString(dt_data.Rows[0]["email"]);



        }
    }
    protected void btn_save_Click(object sender, EventArgs e)
    {
        string log_category_id = Convert.ToString(drp_category.SelectedItem.Value);
        string log_comment = Convert.ToString(tbx_description.Text);
        
        string notice_student = "0";
        if (chk_notice_student.Checked == true) notice_student = "1";

        string visible_student = "0";
        if (chk_visible_student.Checked == true) visible_student = "1";




        string notice_parent = "0";
        if (chk_notice_parent.Checked == true) notice_parent = "1";

        string visible_parent = "0";
        if (chk_visible_parent.Checked == true) visible_parent = "1";


        string notice_teacher = "0";
        if (chk_notice_teacher.Checked == true) notice_teacher = "1";

        string visible_teacher = "0";
        if (chk_visible_teacher.Checked == true) visible_teacher = "1";
        
        string student_id = Convert.ToString(Request.QueryString["id"]);
        IES_Generic_Function ies = new IES_Generic_Function();
        ies.ies_parameters.Clear();
        ies.ies_parameters.Add("@student_id", Convert.ToString(student_id));
        ies.ies_parameters.Add("@log_comment", Convert.ToString(log_comment).Trim());
        ies.ies_parameters.Add("@log_category_id", Convert.ToString(log_category_id).Trim());
        ies.ies_parameters.Add("@notice_student", Convert.ToString(notice_student).Trim());
        ies.ies_parameters.Add("@notice_parent", Convert.ToString(notice_parent).Trim());
        ies.ies_parameters.Add("@notice_teacher", Convert.ToString(notice_teacher).Trim());

        ies.ies_parameters.Add("@visible_parent", Convert.ToString(visible_parent).Trim());
        ies.ies_parameters.Add("@visible_teacher", Convert.ToString(visible_teacher).Trim());
        ies.ies_parameters.Add("@visible_student", Convert.ToString(visible_teacher).Trim());


     

        
        if (hd_logid.Value == "")
        {
            ies.ies_parameters.Add("@created_by", Convert.ToString(Session["Usrid"]));
            ies.ies_parameters.Add("@mode", "add");
        }
        else
        {
            ies.ies_parameters.Add("@updated_by", Convert.ToString(Session["Usrid"]));
            ies.ies_parameters.Add("@id", Convert.ToString(hd_logid.Value));
            ies.ies_parameters.Add("@mode", "update");
        }
        DataTable dt_result = ies.Fn_ExecutiveSql_Datatable("UDP_Student_Log_List", 1, 1);
        if (dt_result != null && dt_result.Rows.Count > 0)
        {
            if (Convert.ToString(dt_result.Rows[0]["Error"]) == "0")
            {
                hd_logid.Value = "";
                btn_save.Text = "Save";
                BindListView();

                string id = Convert.ToString(Request.QueryString["id"]);
                Response.Redirect("Adm_StudentInfo_Log.aspx?id=" + id);
            }
        }

    }

    protected void BindListView()
    {
        string id = Convert.ToString(Request.QueryString["id"]);

        IES_Generic_Function ies = new IES_Generic_Function();
        ies.ies_parameters.Clear();
        ies.ies_parameters.Add("@student_id", id);
        ies.ies_parameters.Add("@mode", "search");
        DataTable dt_data = ies.Fn_ExecutiveSql_Datatable("UDP_Student_Log_List", 1, 1);
        if (dt_data != null)
        {

            lv_details.DataSource = dt_data;
            lv_details.DataBind();
        }
    }
    protected void lnk_student_profile_ServerClick(object sender, EventArgs e)
    {
        string id = Convert.ToString(Request.QueryString["id"]);
        Response.Redirect("Adm_StudentInfo.aspx?id=" + id);
    }
    protected void lnk_student_course_ServerClick(object sender, EventArgs e)
    {
        string id = Convert.ToString(Request.QueryString["id"]);
        Response.Redirect("Adm_StudentInfo_Course.aspx?id=" + id);
    }
    protected void lnk_student_assessment_ServerClick(object sender, EventArgs e)
    {
        string id = Convert.ToString(Request.QueryString["id"]);
        Response.Redirect("Adm_StudentInfo_Assesment.aspx?id=" + id);
    }
    protected void lnk_student_attendance_ServerClick(object sender, EventArgs e)
    {
        string id = Convert.ToString(Request.QueryString["id"]);
        Response.Redirect("Adm_StudentInfo_Attendance.aspx?id=" + id);
    }
    protected void lnk_student_document_ServerClick(object sender, EventArgs e)
    {
        string id = Convert.ToString(Request.QueryString["id"]);
        Response.Redirect("Adm_StudentInfo_Document.aspx?id=" + id);
    }
    protected void lnk_student_elective_ServerClick(object sender, EventArgs e)
    {
        string id = Convert.ToString(Request.QueryString["id"]);
        Response.Redirect("Adm_StudentInfo_Elective.aspx?id=" + id);
    }
    protected void lnk_student_achievement_ServerClick(object sender, EventArgs e)
    {
        string id = Convert.ToString(Request.QueryString["id"]);
        Response.Redirect("Adm_StudentInfo_Achievements.aspx?id=" + id);
    }
    protected void lnk_student_log_ServerClick(object sender, EventArgs e)
    {
        string id = Convert.ToString(Request.QueryString["id"]);
        Response.Redirect("Adm_StudentInfo_Log.aspx?id=" + id);
    }
    protected void ln_edit_Command(object sender, CommandEventArgs e)
    {
        string id = string.Empty;
        id = Convert.ToString(e.CommandArgument);
        if (e.CommandName == "editvalue")
        {
            IES_Generic_Function ies = new IES_Generic_Function();
            ies.ies_parameters.Clear();
            ies.ies_parameters.Add("@id", id);
            ies.ies_parameters.Add("@mode", "edit");
            DataTable dt_result = ies.Fn_ExecutiveSql_Datatable("UDP_Student_Log_List", 1, 1);
            if (dt_result != null && dt_result.Rows.Count > 0)
            {
                drp_category.SelectedValue = Convert.ToString(dt_result.Rows[0]["log_category_id"]);
                tbx_description.Text = Convert.ToString(dt_result.Rows[0]["log_comment"]);
                hd_logid.Value = Convert.ToString(dt_result.Rows[0]["id"]);
                btn_save.Text = "Update";
                chk_notice_student.Checked = Convert.ToBoolean(dt_result.Rows[0]["notice_student"]);
                chk_notice_parent.Checked = Convert.ToBoolean(dt_result.Rows[0]["notice_parent"]);
                chk_notice_teacher.Checked = Convert.ToBoolean(dt_result.Rows[0]["notice_teacher"]);
                chk_visible_student.Checked = Convert.ToBoolean(dt_result.Rows[0]["visible_student"]);
                chk_visible_parent.Checked = Convert.ToBoolean(dt_result.Rows[0]["visible_parent"]);
                chk_visible_teacher.Checked = Convert.ToBoolean(dt_result.Rows[0]["visible_teacher"]);

            }
        }
    }
    protected void ln_deleted_Command(object sender, CommandEventArgs e)
    {
        string id = string.Empty;
        id = Convert.ToString(e.CommandArgument);
        if (e.CommandName == "delete")
        {
            IES_Generic_Function ies = new IES_Generic_Function();
            ies.ies_parameters.Clear();
            ies.ies_parameters.Add("@id", id);
            ies.ies_parameters.Add("@mode", "delete");
            ies.Fn_ExecutiveSql("UDP_Student_Log_List", 1, 1);
        }
        BindListView();
    }
    protected void lnk_student_edit_ServerClick(object sender, EventArgs e)
    {
        string id = Convert.ToString(Request.QueryString["id"]);
        Response.Redirect("Adm_StudentCreation_Edit.aspx?mode=edit&id=" + id);
    }
    protected void lnk_student_search_ServerClick(object sender, EventArgs e)
    {
        Response.Redirect("Adm_Student_List.aspx");
    }
}