using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Adm_StudentInfo_Assesment : System.Web.UI.Page
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
            BindDisplay();



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