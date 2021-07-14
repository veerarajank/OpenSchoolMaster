using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Adm_Online_Application : System.Web.UI.Page
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
            ies = new IES_Generic_Function();
            
            ies.Fn_Executive_ListWithSelect_WithTitle(drp_nationality, "--Select Nationality--", "select id, name from tbl_nationality	order by name", "id", "Name");
            
            
            

            drp_gender.Items.Clear();
            drp_gender.Items.Add(new ListItem("--Select Gender--", "0"));
            drp_gender.Items.Add(new ListItem("Male", "1"));
            drp_gender.Items.Add(new ListItem("Female", "2"));

            







            ies = new IES_Generic_Function();
            ies.ies_parameters.Clear();
            DataTable dt_data = ies.Fn_ExecutiveSql_Datatable("select (select online_admission_no from tbl_academic_years sub where sub.academic_id=pm.academic_id) as admission_no,pm.academic_id,replace(convert(varchar(10),getdate(),103),'/','-') as admissiondate from tbl_onlineregistration_settings pm where id='1'", 0, 0);
            if (dt_data != null && dt_data.Rows.Count > 0)
            {
                tbx_admissionno.Text = "APP" + Convert.ToInt32(dt_data.Rows[0]["admission_no"]).ToString("0000");
                tbx_admissiondate.Text = Convert.ToString(dt_data.Rows[0]["admissiondate"]);
                tbx_admissiondate.Enabled = false;
                tbx_admissionno.Enabled = false;
                hd_academic_yr.Value = Convert.ToString(dt_data.Rows[0]["academic_id"]);
            }






        }
    }
    protected void btn_guradian_Click(object sender, EventArgs e)
    {

        IES_Generic_Function ies = new IES_Generic_Function();
        ies.ies_parameters.Clear();
        string academic_yr = Convert.ToString(hd_academic_yr.Value);
        string admission_no = Convert.ToString(tbx_admissionno.Text);
        string name = Convert.ToString(tbx_name.Text);
        string course_name = Convert.ToString(drp_courseid.SelectedItem.Value);
        string father_name = Convert.ToString(tbx_fathername.Text);
        string gender = Convert.ToString(drp_gender.SelectedItem.Text);

        string dob = "";
        if (tbx_dob.Text.Trim() != "")
        {
            DateTime dtdate_of_birth = DateTime.ParseExact(tbx_dob.Text.Trim(), "dd-MM-yyyy", null);
            dob = dtdate_of_birth.ToString("yyyy-MM-dd");
        }
        string nationlity = Convert.ToString(drp_nationality.SelectedItem.Text);
        string social_sts = "General";
        if (rd_general.Checked == true) social_sts = "General";
        else if (rd_bc.Checked == true) social_sts = "BC";
        else if (rd_obc.Checked == true) social_sts = "OBC";
        else if (rd_others.Checked == true) social_sts = "Others";
        
        string mobile_no = Convert.ToString(tbx_phone1.Text);
        string email_id = Convert.ToString(tbx_emailid.Text);

        string present_address = Convert.ToString(tbx_add1.Text);
        string permanent_address = Convert.ToString(tbx_add2.Text);


        string predeg_school_course = Convert.ToString(tbx_sslc_course.Text);
        string predeg_school_group = Convert.ToString(tbx_sslc_group.Text);
        string predeg_school_institute = Convert.ToString(tbx_sslc_institution.Text);
        string predeg_school_yearpass = Convert.ToString(tbx_sslc_yearpass.Text);
        string predeg_school_marks = Convert.ToString(tbx_sslc_mark.Text);
        string predeg_school_grade = Convert.ToString(tbx_sslc_grade.Text);


        string ug_pg_course = Convert.ToString(tbx_pg_course.Text);
        string ug_pg_group = Convert.ToString(tbx_pg_group.Text);
        string ug_pg_institute = Convert.ToString(tbx_pg_institution.Text);
        string ug_pg_yearpass = Convert.ToString(tbx_pg_yearpass.Text);
        string ug_pg_marks = Convert.ToString(tbx_pg_mark.Text);
        string ug_pg_grade = Convert.ToString(tbx_pg_grade.Text);


        string ug_prefered_lang_firstyr = Convert.ToString(tbx_firstyear.Text);
        string ug_prefered_lang_secondyr = Convert.ToString(tbx_secondyear.Text);
        string ug_prefered_lang_thirdyr = Convert.ToString(tbx_thirdyear.Text);


        string concurrent_yearofstudy = Convert.ToString(tbx_yearofstudy.Text);
        string concurrent_college = Convert.ToString(tbx_college.Text);
        string concurrent_university = Convert.ToString(tbx_university.Text);

        string str_qry = @"insert into tbl_student_online
            (academic_yr,admission_no,name,course_name,father_name,gender,dob,nationlity,social_sts,mobile_no,email_id,present_address,permanent_address,
            predeg_school_course,predeg_school_group,predeg_school_institute,predeg_school_yearpass,predeg_school_marks,predeg_school_grade,
            ug_pg_course,ug_pg_group,ug_pg_institute,ug_pg_yearpass,ug_pg_marks,ug_pg_grade,
            ug_prefered_lang_firstyr,ug_prefered_lang_secondyr,ug_prefered_lang_thirdyr,
            concurrent_yearofstudy,concurrent_college,concurrent_university,
            created_at,is_approved,is_denied,is_waitinglist)
            values
            (@academic_yr,@admission_no,@name,@course_name,@father_name,@gender,@dob,@nationlity,@social_sts,@mobile_no,@email_id,@present_address,@permanent_address,
            @predeg_school_course,@predeg_school_group,@predeg_school_institute,@predeg_school_yearpass,@predeg_school_marks,@predeg_school_grade,
            @ug_pg_course,@ug_pg_group,@ug_pg_institute,@ug_pg_yearpass,@ug_pg_marks,@ug_pg_grade,
            @ug_prefered_lang_firstyr,@ug_prefered_lang_secondyr,@ug_prefered_lang_thirdyr,
            @concurrent_yearofstudy,@concurrent_college,@concurrent_university,
            getdate(),'0','0','0')";
        
        ies = new IES_Generic_Function();
        ies.ies_parameters.Clear();
        ies.ies_parameters.Add("@academic_yr", Convert.ToString(academic_yr));
        ies.ies_parameters.Add("@admission_no", Convert.ToString(admission_no));
        ies.ies_parameters.Add("@name", Convert.ToString(name));
        ies.ies_parameters.Add("@course_name", Convert.ToString(course_name));
        ies.ies_parameters.Add("@father_name", Convert.ToString(father_name));
        ies.ies_parameters.Add("@gender", Convert.ToString(gender));
        ies.ies_parameters.Add("@dob", Convert.ToString(dob));
        ies.ies_parameters.Add("@nationlity", Convert.ToString(nationlity));
        ies.ies_parameters.Add("@social_sts", Convert.ToString(social_sts));
        ies.ies_parameters.Add("@mobile_no", Convert.ToString(mobile_no));
        ies.ies_parameters.Add("@email_id", Convert.ToString(email_id));
        ies.ies_parameters.Add("@present_address", Convert.ToString(present_address));
        ies.ies_parameters.Add("@permanent_address", Convert.ToString(permanent_address));
        ies.ies_parameters.Add("@predeg_school_course", Convert.ToString(predeg_school_course));
        ies.ies_parameters.Add("@predeg_school_group", Convert.ToString(predeg_school_group));
        ies.ies_parameters.Add("@predeg_school_institute", Convert.ToString(predeg_school_institute));
        ies.ies_parameters.Add("@predeg_school_yearpass", Convert.ToString(predeg_school_yearpass));
        ies.ies_parameters.Add("@predeg_school_marks", Convert.ToString(predeg_school_marks));
        ies.ies_parameters.Add("@predeg_school_grade", Convert.ToString(predeg_school_grade));
        ies.ies_parameters.Add("@ug_pg_course", Convert.ToString(ug_pg_course));
        ies.ies_parameters.Add("@ug_pg_group", Convert.ToString(ug_pg_group));
        ies.ies_parameters.Add("@ug_pg_institute", Convert.ToString(ug_pg_institute));
        ies.ies_parameters.Add("@ug_pg_yearpass", Convert.ToString(ug_pg_yearpass));
        ies.ies_parameters.Add("@ug_pg_marks", Convert.ToString(ug_pg_marks));
        ies.ies_parameters.Add("@ug_pg_grade", Convert.ToString(ug_pg_grade));
        ies.ies_parameters.Add("@ug_prefered_lang_firstyr", Convert.ToString(ug_prefered_lang_firstyr));
        ies.ies_parameters.Add("@ug_prefered_lang_secondyr", Convert.ToString(ug_prefered_lang_secondyr));
        ies.ies_parameters.Add("@ug_prefered_lang_thirdyr", Convert.ToString(ug_prefered_lang_thirdyr));
        ies.ies_parameters.Add("@concurrent_yearofstudy", Convert.ToString(concurrent_yearofstudy));
        ies.ies_parameters.Add("@concurrent_college", Convert.ToString(concurrent_college));
        ies.ies_parameters.Add("@concurrent_university", Convert.ToString(concurrent_university));
        ies.Fn_ExecutiveSql(str_qry, 0, 1);


        ies = new IES_Generic_Function();
        ies.ies_parameters.Clear();
        ies.Fn_ExecutiveSql("update tbl_academic_years set online_admission_no=online_admission_no+1 where academic_id='" + academic_yr + "'", 0, 0);        

        Response.Redirect("Adm_Online_Application_view.aspx");

        


       
    }
}