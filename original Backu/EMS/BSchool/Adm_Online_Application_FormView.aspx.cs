using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Adm_Online_Application_FormView : System.Web.UI.Page
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
            ies.ies_parameters.Clear();
            DataTable dt_data = ies.Fn_ExecutiveSql_Datatable("select (select academic_name from tbl_academic_years sub where sub.academic_id=pm.academic_id) as academic_name,pm.academic_id,replace(convert(varchar(10),getdate(),103),'/','-') as admissiondate from tbl_onlineregistration_settings pm where id='1'", 0, 0);
            if (dt_data != null && dt_data.Rows.Count > 0)
            {
                lbl_acyear.InnerHtml = "Academic Year - " + Convert.ToString(dt_data.Rows[0]["academic_name"]);
                hd_acyear.Value = Convert.ToString(dt_data.Rows[0]["academic_id"]);
            }
            Bind_Details(Convert.ToString(hd_acyear.Value));
        }
    }
    protected void Bind_Details(string academic_yr)
    {

        string online_id = Convert.ToString(Request.QueryString["id"]);

        string str_qry = @"select *,dbo.UDF_DateFormat(created_at,1,1) as created_atddmmyyy,dbo.UDF_DateFormat(dob,1,0) as dobmmddyyy
                        from tbl_student_online where academic_yr=@academic_yr and online_id=@online_id";

       

        IES_Generic_Function ies = new IES_Generic_Function();
        ies.ies_parameters.Clear();
        ies.ies_parameters.Add("@academic_yr", Convert.ToString(academic_yr));
        ies.ies_parameters.Add("@online_id", Convert.ToString(online_id));
        DataTable dt_data = ies.Fn_ExecutiveSql_Datatable(str_qry, 0, 1);
        if (dt_data != null)
        {
            if (dt_data.Rows.Count > 0)
            {
                tbx_admissionno.Text = Convert.ToString(dt_data.Rows[0]["admission_no"]);
                tbx_admissiondate.Text = Convert.ToString(dt_data.Rows[0]["created_atddmmyyy"]);

                tbx_name.InnerText = Convert.ToString(dt_data.Rows[0]["name"]);
                tbx_fathername.InnerText = Convert.ToString(dt_data.Rows[0]["father_name"]);
                tbx_course.InnerText = Convert.ToString(dt_data.Rows[0]["course_name"]);
                tbx_gender.InnerText = Convert.ToString(dt_data.Rows[0]["gender"]);
                tbx_dob.InnerText = Convert.ToString(dt_data.Rows[0]["dobmmddyyy"]);
                tbx_nationality.InnerText = Convert.ToString(dt_data.Rows[0]["nationlity"]);
                tbx_Social_Status.InnerText = Convert.ToString(dt_data.Rows[0]["social_sts"]);


                tbx_add1.InnerText = Convert.ToString(dt_data.Rows[0]["present_address"]);
                tbx_add2.InnerText = Convert.ToString(dt_data.Rows[0]["permanent_address"]);
                tbx_phone1.InnerText = Convert.ToString(dt_data.Rows[0]["mobile_no"]);
                tbx_emailid.InnerText = Convert.ToString(dt_data.Rows[0]["email_id"]);


                tbx_sslc_course.InnerText = Convert.ToString(dt_data.Rows[0]["predeg_school_course"]);
                tbx_sslc_group.InnerText = Convert.ToString(dt_data.Rows[0]["predeg_school_group"]);
                tbx_sslc_institution.InnerText = Convert.ToString(dt_data.Rows[0]["predeg_school_institute"]);
                tbx_sslc_yearpass.InnerText = Convert.ToString(dt_data.Rows[0]["predeg_school_yearpass"]);
                tbx_sslc_mark.InnerText = Convert.ToString(dt_data.Rows[0]["predeg_school_marks"]);
                tbx_sslc_grade.InnerText = Convert.ToString(dt_data.Rows[0]["predeg_school_grade"]);


                tbx_pg_course.InnerText = Convert.ToString(dt_data.Rows[0]["ug_pg_course"]);
                tbx_pg_group.InnerText = Convert.ToString(dt_data.Rows[0]["ug_pg_group"]);
                tbx_pg_institution.InnerText = Convert.ToString(dt_data.Rows[0]["ug_pg_institute"]);
                tbx_pg_yearpass.InnerText = Convert.ToString(dt_data.Rows[0]["ug_pg_yearpass"]);
                tbx_pg_mark.InnerText = Convert.ToString(dt_data.Rows[0]["ug_pg_marks"]);
                tbx_pg_grade.InnerText = Convert.ToString(dt_data.Rows[0]["ug_pg_grade"]);


                tbx_firstyear.InnerText = Convert.ToString(dt_data.Rows[0]["ug_prefered_lang_firstyr"]);
                tbx_secondyear.InnerText = Convert.ToString(dt_data.Rows[0]["ug_prefered_lang_secondyr"]);
                tbx_thirdyear.InnerText = Convert.ToString(dt_data.Rows[0]["ug_prefered_lang_thirdyr"]);

                tbx_yearofstudy.InnerText = Convert.ToString(dt_data.Rows[0]["concurrent_yearofstudy"]);
                tbx_college.InnerText = Convert.ToString(dt_data.Rows[0]["concurrent_college"]);
                tbx_university.InnerText = Convert.ToString(dt_data.Rows[0]["concurrent_university"]);


            }
        }
    }
}