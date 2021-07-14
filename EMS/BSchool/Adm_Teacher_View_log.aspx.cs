using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Adm_Teacher_View_log : System.Web.UI.Page
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
            img_student.ImageUrl = "EmployeeHandler.ashx?imgid=" + id;

            BindDisplay();


        }
    }
    protected void btn_save_Click(object sender, EventArgs e)
    {

    }
    protected void lnk_teacher_profile_ServerClick(object sender, EventArgs e)
    {
        string teacher_id = Convert.ToString(Request.QueryString["id"]);
        Response.Redirect("Adm_Teacher_View_Primary.aspx?id=" + teacher_id);
    }
    protected void lnk_teacher_achievement_ServerClick(object sender, EventArgs e)
    {
        string teacher_id = Convert.ToString(Request.QueryString["id"]);
        Response.Redirect("Adm_Teacher_View_Achievement.aspx?id=" + teacher_id);
    }
    protected void lnk_teacher_log_ServerClick(object sender, EventArgs e)
    {
        string teacher_id = Convert.ToString(Request.QueryString["id"]);
        Response.Redirect("Adm_Teacher_View_log.aspx?id=" + teacher_id);
    }
    protected void lnk_teacher_document_ServerClick(object sender, EventArgs e)
    {
        string teacher_id = Convert.ToString(Request.QueryString["id"]);
        Response.Redirect("Adm_Teacher_View_Document.aspx?id=" + teacher_id);
    }
    protected void lnk_teacher_attendance_ServerClick(object sender, EventArgs e)
    {
        string teacher_id = Convert.ToString(Request.QueryString["id"]);
        Response.Redirect("Adm_Teacher_View_attendance.aspx?id=" + teacher_id);
    }
    protected void lnk_teacher_subject_ServerClick(object sender, EventArgs e)
    {
        string teacher_id = Convert.ToString(Request.QueryString["id"]);
        Response.Redirect("Adm_Teacher_View_subjectAssociation.aspx?id=" + teacher_id);
    }
    protected void BindDisplay()
    {
        string id = Convert.ToString(Request.QueryString["id"]);
        IES_Generic_Function ies = new IES_Generic_Function();
        ies.ies_parameters.Clear();
        ies.ies_parameters.Add("@id", Convert.ToString(id));
        ies.ies_parameters.Add("@mode", "search");
        DataTable dt_data = ies.Fn_ExecutiveSql_Datatable("UDP_Create_Teacher", 1, 1);
        if (dt_data != null && dt_data.Rows.Count > 0)
        {

            lbl_teachername.InnerHtml = Convert.ToString(dt_data.Rows[0]["first_name"]) + " " + Convert.ToString(dt_data.Rows[0]["middle_name"]) + " " + Convert.ToString(dt_data.Rows[0]["last_name"]);
            lbl_teachermailid.InnerHtml = Convert.ToString(dt_data.Rows[0]["email"]);
        }
    }
}