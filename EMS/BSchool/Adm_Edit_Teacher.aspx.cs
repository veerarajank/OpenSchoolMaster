using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Adm_Edit_Teacher : System.Web.UI.Page
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
            img_teacher.ImageUrl = "EmployeeHandler.ashx?imgid=" + id;

            IES_Generic_Function ies = new IES_Generic_Function();
            ies = new IES_Generic_Function();
            ies.Fn_Executive_ListWithSelect_WithTitle(drp_nationality, "--Select Nationality--", "select id, name from tbl_nationality	order by name", "id", "Name");
            ies.Fn_Executive_ListWithSelect_WithTitle(drp_lang, "--Select Language--", "select id, name from tbl_languages	order by name", "id", "Name");
            ies.Fn_Executive_ListWithSelect_WithTitle(drp_religion, "--Select Religion--", "select id, name from tbl_religion	order by name", "id", "Name");
            ies.Fn_Executive_ListWithSelect_WithTitle(drp_category, "--Select Category--", "select id, name from tbl_studentcategories	order by name", "id", "Name");            
            ies.Fn_Executive_ListWithSelect_WithTitle(drp_teacherdept, "--Select Department--", "select id, name from tbl_department	order by name", "id", "Name");

            ies.Fn_Executive_ListWithSelect_WithTitle(drp_position, "--Select Position--", "select id, name from tbl_teacher_position	order by name", "id", "Name");
            ies.Fn_Executive_ListWithSelect_WithTitle(drp_grade, "--Select Grade--", "select id, name from tbl_teacher_grade order by name", "id", "Name");


            drp_gender.Items.Clear();
            drp_gender.Items.Add(new ListItem("--Select Gender--", "0"));
            drp_gender.Items.Add(new ListItem("Male", "1"));
            drp_gender.Items.Add(new ListItem("Female", "2"));

            drp_maritalsts.Items.Clear();
            drp_maritalsts.Items.Add(new ListItem("--Select Marital Status--", "0"));
            drp_maritalsts.Items.Add(new ListItem("Married", "1"));
            drp_maritalsts.Items.Add(new ListItem("Unmarried", "2"));

            drp_bloodgroup.Items.Clear();
            drp_bloodgroup.Items.Add(new ListItem("Unknown", "Unknown"));
            drp_bloodgroup.Items.Add(new ListItem("A+", "A+"));
            drp_bloodgroup.Items.Add(new ListItem("A-", "A-"));
            drp_bloodgroup.Items.Add(new ListItem("B+", "B+"));
            drp_bloodgroup.Items.Add(new ListItem("B-", "B-"));
            drp_bloodgroup.Items.Add(new ListItem("O+", "O+"));
            drp_bloodgroup.Items.Add(new ListItem("O-", "O-"));
            drp_bloodgroup.Items.Add(new ListItem("AB+", "AB+"));
            drp_bloodgroup.Items.Add(new ListItem("AB-", "AB-"));

            drp_experienceyear.Items.Clear();
            drp_experienceyear.Items.Add(new ListItem("--Select Years--", "-1"));
            for (int i = 0; i <= 20; i++)
            {
                drp_experienceyear.Items.Add(new ListItem(i.ToString(), i.ToString()));
            }

            drp_experiencemonth.Items.Clear();
            drp_experiencemonth.Items.Add(new ListItem("--Select Month--", "-1"));
            for (int i = 0; i <= 20; i++)
            {
                drp_experiencemonth.Items.Add(new ListItem(i.ToString(), i.ToString()));
            }
            
            
             

            BindDisplay();

        }
    }
    protected void btn_save_Click(object sender, EventArgs e)
    {
        string id = Convert.ToString(Request.QueryString["id"]);
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conn"].ConnectionString);

        if (con.State == ConnectionState.Closed) con.Open();
        SqlCommand cmd = new SqlCommand("UDP_Create_Teacher", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.Clear();

        string employee_number = Convert.ToString(tbx_teacherno.Text);
        string joining_date = "";
        if (tbx_joindate.Text.Trim() != "")
        {
            DateTime dtadmission_date = DateTime.ParseExact(tbx_joindate.Text.Trim(), "dd-MM-yyyy", null);
            joining_date = dtadmission_date.ToString("yyyy-MM-dd");
        }
        string first_name = Convert.ToString(tbx_firstname.Text);
        string middle_name = Convert.ToString(tbx_middlename.Text);
        string last_name = Convert.ToString(tbx_lastname.Text);

        string date_of_birth = "";
        if (tbx_dob.Text.Trim() != "")
        {
            DateTime dtdate_of_birth = DateTime.ParseExact(tbx_dob.Text.Trim(), "dd-MM-yyyy", null);
            date_of_birth = dtdate_of_birth.ToString("yyyy-MM-dd");
        }
        string gender = Convert.ToString(drp_gender.SelectedItem.Value);
        string blood_group = Convert.ToString(drp_bloodgroup.SelectedItem.Value);

        string employee_department_id = Convert.ToString(drp_teacherdept.SelectedItem.Value);
        string employee_category_id = Convert.ToString(drp_category.SelectedItem.Value);
        string employee_position_id = Convert.ToString(drp_position.SelectedItem.Value);
        string employee_grade_id = Convert.ToString(drp_grade.SelectedItem.Value);

        string job_title = Convert.ToString(tbx_jobtitle.Text);
        string qualification = Convert.ToString(tbx_qualification.Text);

        string experience_detail = Convert.ToString(tbx_experiencedetails.Text);
        string experience_year = Convert.ToString(drp_experienceyear.SelectedItem.Value);
        string experience_month = Convert.ToString(drp_experiencemonth.SelectedItem.Value);

        string marital_status = Convert.ToString(drp_maritalsts.SelectedItem.Value);
        string children_count = Convert.ToString(tbx_childcnt.Text);
        string father_name = Convert.ToString(tbx_fathername.Text);
        string mother_name = Convert.ToString(tbx_mothername.Text);
        string spouse_name = Convert.ToString(tbx_spousename.Text);
        string birth_place = Convert.ToString(tbx_birthplace.Text);
        string nationality_id = Convert.ToString(drp_nationality.SelectedItem.Value);
        string language_id = Convert.ToString(drp_lang.SelectedItem.Value);
        string religion_id = Convert.ToString(drp_religion.SelectedItem.Value);






        cmd.Parameters.AddWithValue("@id", Convert.ToString(id));
        cmd.Parameters.AddWithValue("@employee_number", Convert.ToString(employee_number));
        cmd.Parameters.AddWithValue("@joining_date", Convert.ToString(joining_date).Trim());
        cmd.Parameters.AddWithValue("@first_name", Convert.ToString(first_name).Trim());
        cmd.Parameters.AddWithValue("@middle_name", Convert.ToString(middle_name).Trim());
        cmd.Parameters.AddWithValue("@last_name", Convert.ToString(last_name).Trim());

        cmd.Parameters.AddWithValue("@date_of_birth", Convert.ToString(date_of_birth));
        cmd.Parameters.AddWithValue("@gender", Convert.ToString(gender).Trim());
        cmd.Parameters.AddWithValue("@blood_group", Convert.ToString(blood_group).Trim());
        cmd.Parameters.AddWithValue("@employee_department_id", Convert.ToString(employee_department_id).Trim());
        cmd.Parameters.AddWithValue("@employee_category_id", Convert.ToString(employee_category_id).Trim());

        cmd.Parameters.AddWithValue("@employee_position_id", Convert.ToString(employee_position_id));
        cmd.Parameters.AddWithValue("@employee_grade_id", Convert.ToString(employee_grade_id).Trim());
        cmd.Parameters.AddWithValue("@job_title", Convert.ToString(job_title).Trim());
        cmd.Parameters.AddWithValue("@qualification", Convert.ToString(qualification).Trim());
        cmd.Parameters.AddWithValue("@experience_detail", Convert.ToString(experience_detail).Trim());

        cmd.Parameters.AddWithValue("@experience_year", Convert.ToString(experience_year));
        cmd.Parameters.AddWithValue("@experience_month", Convert.ToString(experience_month).Trim());
        cmd.Parameters.AddWithValue("@marital_status", Convert.ToString(marital_status).Trim());
        cmd.Parameters.AddWithValue("@children_count", Convert.ToString(children_count).Trim());
        cmd.Parameters.AddWithValue("@father_name", Convert.ToString(father_name).Trim());

        cmd.Parameters.AddWithValue("@mother_name", Convert.ToString(mother_name));
        cmd.Parameters.AddWithValue("@spouse_name", Convert.ToString(spouse_name).Trim());
        cmd.Parameters.AddWithValue("@birth_place", Convert.ToString(birth_place).Trim());
        cmd.Parameters.AddWithValue("@nationality_id", Convert.ToString(nationality_id));
        cmd.Parameters.AddWithValue("@language_id", Convert.ToString(language_id).Trim());
        cmd.Parameters.AddWithValue("@religion_id", Convert.ToString(religion_id).Trim());


        string photo_file_name = "";
        string photo_content_type = "";
        string sourcefilepath = "";
        if (file_upload.HasFile)
        {

            photo_file_name = System.IO.Path.GetFileName(file_upload.FileName);


            if (System.IO.Directory.Exists(HttpContext.Current.Server.MapPath("~/employee")) == false)
            {
                System.IO.Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~/employee"));
            }
            sourcefilepath = System.IO.Path.Combine(HttpContext.Current.Server.MapPath("~/employee"), photo_file_name);
            try
            {
                if (File.Exists(sourcefilepath) == true)
                {
                    try
                    {
                        File.Delete(sourcefilepath);
                    }
                    catch { }
                }
                file_upload.PostedFile.SaveAs(sourcefilepath);
            }
            catch
            {
            }



            double filecontentlength = 0;
            int photo_file_size = file_upload.PostedFile.ContentLength;
            string sizeunit = "Byte";
            filecontentlength = Convert.ToDouble(file_upload.PostedFile.ContentLength);
            photo_content_type = Convert.ToString(file_upload.PostedFile.ContentType);
            if (file_upload.PostedFile.ContentLength >= 1048576)
            {
                filecontentlength = Convert.ToDouble(filecontentlength) / 1048576;
                sizeunit = "MB";
            }
            else if (file_upload.FileContent.Length >= 1024)
            {
                filecontentlength = Convert.ToDouble(filecontentlength) / 1024;
                sizeunit = "KB";
            }
            byte[] file;
            using (var stream = new FileStream(sourcefilepath, FileMode.Open, FileAccess.Read))
            {
                using (var reader = new BinaryReader(stream))
                {
                    file = reader.ReadBytes((int)stream.Length);
                }
            }
            cmd.Parameters.Add("@photo_data", SqlDbType.VarBinary, file.Length).Value = file;
            cmd.Parameters.AddWithValue("@photo_file_size", Convert.ToString(photo_file_size).Trim());
        }


        cmd.Parameters.AddWithValue("@photo_file_name", Convert.ToString(photo_file_name).Trim());
        cmd.Parameters.AddWithValue("@photo_content_type", Convert.ToString(photo_content_type).Trim());

        cmd.Parameters.AddWithValue("@is_active", Convert.ToString("1").Trim());
        cmd.Parameters.AddWithValue("@is_deleted", Convert.ToString("0").Trim());
        cmd.Parameters.AddWithValue("@updated_by", Convert.ToString(Session["Usrid"]));
        cmd.Parameters.AddWithValue("@mode", "update_teacher");
        DataTable dt_result = new DataTable();
        SqlDataAdapter da = new SqlDataAdapter(cmd);
        da.Fill(dt_result);
        da.SelectCommand.Connection.Close();
        cmd.Connection.Close();
        if (con != null && con.State == ConnectionState.Open) con.Close();
        if (con != null) con = null;
        if (dt_result != null && dt_result.Rows.Count > 0)
        {
            if (Convert.ToString(dt_result.Rows[0]["Error"]) == "0")
            {
                if (sourcefilepath != "")
                {
                    if (File.Exists(sourcefilepath) == true)
                    {
                        try
                        {
                            File.Delete(sourcefilepath);
                        }
                        catch { }
                    }
                }

            
                Response.Redirect("Adm_Edit_Teacher_Contact_Details.aspx?id=" + id);
                //Response.Redirect("Adm_Teacher_List.aspx");
            }
        }
    }
    protected void teacher_contact_ServerClick(object sender, EventArgs e)
    {
        string id = Convert.ToString(Request.QueryString["id"]);
        Response.Redirect("Adm_Edit_Teacher_Contact_Details.aspx?id=" + id);
    }
    protected void teacher_document_ServerClick(object sender, EventArgs e)
    {
     
        string id = Convert.ToString(Request.QueryString["id"]);
        Response.Redirect("Adm_Edit_Teacher_Documents.aspx?id=" + id);
    }
    protected void teacher_info_ServerClick(object sender, EventArgs e)
    {
        string id = Convert.ToString(Request.QueryString["id"]);
        Response.Redirect("Adm_Edit_Teacher.aspx?id=" + id);
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
           
            tbx_teacherno.Text = Convert.ToString(dt_data.Rows[0]["employee_number"]);
            tbx_joindate.Text = Convert.ToString(dt_data.Rows[0]["joining_date_ddmmyy"]);
            drp_gender.SelectedValue = Convert.ToString(dt_data.Rows[0]["gender"]);

            tbx_firstname.Text = Convert.ToString(dt_data.Rows[0]["first_name"]);
            tbx_middlename.Text = Convert.ToString(dt_data.Rows[0]["middle_name"]);
            tbx_lastname.Text = Convert.ToString(dt_data.Rows[0]["last_name"]);
            tbx_dob.Text = Convert.ToString(dt_data.Rows[0]["date_of_birth_ddmmyy"]);
            drp_bloodgroup.SelectedValue = Convert.ToString(dt_data.Rows[0]["blood_group"]);
            drp_teacherdept.SelectedValue = Convert.ToString(dt_data.Rows[0]["employee_department_id"]);
            drp_category.SelectedValue = Convert.ToString(dt_data.Rows[0]["employee_category_id"]);
            drp_position.SelectedValue = Convert.ToString(dt_data.Rows[0]["employee_position_id"]);
            drp_grade.SelectedValue = Convert.ToString(dt_data.Rows[0]["employee_grade_id"]);
            tbx_jobtitle.Text = Convert.ToString(dt_data.Rows[0]["job_title"]);
            tbx_qualification.Text = Convert.ToString(dt_data.Rows[0]["qualification"]);
            drp_experienceyear.SelectedValue = Convert.ToString(dt_data.Rows[0]["experience_year"]);
            drp_experiencemonth.SelectedValue = Convert.ToString(dt_data.Rows[0]["experience_month"]);
            tbx_experiencedetails.Text = Convert.ToString(dt_data.Rows[0]["experience_detail"]);
            
            drp_maritalsts.SelectedValue = Convert.ToString(dt_data.Rows[0]["marital_status"]);
            tbx_childcnt.Text = Convert.ToString(dt_data.Rows[0]["children_count"]);
            tbx_fathername.Text = Convert.ToString(dt_data.Rows[0]["father_name"]);
            tbx_mothername.Text = Convert.ToString(dt_data.Rows[0]["mother_name"]);
            tbx_spousename.Text = Convert.ToString(dt_data.Rows[0]["spouse_name"]);
            tbx_birthplace.Text = Convert.ToString(dt_data.Rows[0]["birth_place"]);
            drp_nationality.SelectedValue = Convert.ToString(dt_data.Rows[0]["nationality_id"]);
            drp_lang.SelectedValue = Convert.ToString(dt_data.Rows[0]["language_id"]);
            drp_religion.SelectedValue = Convert.ToString(dt_data.Rows[0]["religion_id"]);
            
           


      



        }
    }
}