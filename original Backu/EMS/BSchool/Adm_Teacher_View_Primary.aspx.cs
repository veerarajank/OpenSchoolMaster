using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Adm_Teacher_View_Primary : System.Web.UI.Page
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
            lbl_teacherno.InnerHtml = "<strong>&nbsp;" + Convert.ToString(dt_data.Rows[0]["employee_number"]) + "</strong>";
            lbl_teacherjoindate.InnerHtml = Convert.ToString(dt_data.Rows[0]["joining_date_ddmmyy"]);
            lbl_teachergender.InnerHtml = Convert.ToString(dt_data.Rows[0]["gender_txt"]);
            lbl_teacherdob.InnerHtml = Convert.ToString(dt_data.Rows[0]["date_of_birth_ddmmyy"]);
            lbl_teacherdept.InnerHtml = Convert.ToString(dt_data.Rows[0]["Department"]);
            lbl_teachername.InnerHtml = Convert.ToString(dt_data.Rows[0]["first_name"]) + " " + Convert.ToString(dt_data.Rows[0]["middle_name"]) + " " + Convert.ToString(dt_data.Rows[0]["last_name"]);

            lbl_teacherposition.InnerHtml = Convert.ToString(dt_data.Rows[0]["position"]);
            lbl_teachercategory.InnerHtml = Convert.ToString(dt_data.Rows[0]["categoryname"]);
            lbl_teachergrade.InnerHtml = Convert.ToString(dt_data.Rows[0]["gradename"]);
            lbl_teachertitle.InnerHtml = Convert.ToString(dt_data.Rows[0]["job_title"]);
            lbl_teacherqualification.InnerHtml = Convert.ToString(dt_data.Rows[0]["qualification"]);
            lbl_teachertotalexperience.InnerHtml = Convert.ToString(dt_data.Rows[0]["experience_year"]) + " Years " + Convert.ToString(dt_data.Rows[0]["experience_month"]) + " Months(s).";
            lbl_teacherexpereincedetails.InnerHtml = Convert.ToString(dt_data.Rows[0]["experience_detail"]);
            lbl_teachermarital_sts.InnerHtml = Convert.ToString(dt_data.Rows[0]["marital_stsname"]);
            lbl_teacherchildcnt.InnerHtml = Convert.ToString(dt_data.Rows[0]["children_count"]);
            lbl_teacher_fathername.InnerHtml = Convert.ToString(dt_data.Rows[0]["father_name"]);
            lbl_teacher_mothername.InnerHtml = Convert.ToString(dt_data.Rows[0]["mother_name"]);
            lbl_teacher_spousename.InnerHtml = Convert.ToString(dt_data.Rows[0]["spouse_name"]);

            lbl_teacher_birthplace.InnerHtml = Convert.ToString(dt_data.Rows[0]["birth_place"]);
            lbl_teachernationality.InnerHtml = Convert.ToString(dt_data.Rows[0]["nationality"]);
            lbl_lang.InnerHtml = Convert.ToString(dt_data.Rows[0]["lang"]);
            lbl_religion.InnerHtml = Convert.ToString(dt_data.Rows[0]["religion"]);

            lbl_teacher_addr1.InnerHtml = Convert.ToString(dt_data.Rows[0]["address_line1"]);
            lbl_teacher_addr2.InnerHtml = Convert.ToString(dt_data.Rows[0]["address_line2"]);
            lbl_teacher_city.InnerHtml = Convert.ToString(dt_data.Rows[0]["city"]);
            lbl_teacher_state.InnerHtml = Convert.ToString(dt_data.Rows[0]["state"]);
            lbl_teacher_country.InnerHtml = Convert.ToString(dt_data.Rows[0]["country"]);
            lbl_teacher_pincode.InnerHtml = Convert.ToString(dt_data.Rows[0]["pin_code"]);
            lbl_teacher_phone1.InnerHtml = Convert.ToString(dt_data.Rows[0]["phone1"]);
            lbl_teacher_phone2.InnerHtml = Convert.ToString(dt_data.Rows[0]["phone2"]);
            lbl_teacher_emailid.InnerHtml = Convert.ToString(dt_data.Rows[0]["email"]);


            lbl_teachermailid.InnerHtml = Convert.ToString(dt_data.Rows[0]["email"]);



        }
    }
}