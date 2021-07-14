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

public partial class Adm_StudentCreation : System.Web.UI.Page
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
            ies.Fn_Executive_ListWithSelect_WithTitle(drp_batchid, "--Select Batch--", "select id,(select sub.name from tbl_courses sub where sub.id=c.course_id) + ' - ' + name as name from tbl_batches c where ISNULL(is_deleted,0)='0'	and id in(select batch_id from tbl_academic_batches where academic_id=(select academic_id from tbl_setcurrentacademicyear_settings where id=1))	order by name", "id", "Name");
            ies.Fn_Executive_ListWithSelect_WithTitle(drp_nationality, "--Select Nationality--", "select id, name from tbl_nationality	order by name", "id", "Name");
            ies.Fn_Executive_ListWithSelect_WithTitle(drp_lang, "--Select Language--", "select id, name from tbl_languages	order by name", "id", "Name");
            ies.Fn_Executive_ListWithSelect_WithTitle(drp_religion, "--Select Religion--", "select id, name from tbl_religion	order by name", "id", "Name");
            ies.Fn_Executive_ListWithSelect_WithTitle(drp_category, "--Select Category--", "select id, name from tbl_studentcategories	order by name", "id", "Name");
            ies.Fn_Executive_ListWithSelect_WithTitle(drp_contry, "--Select Country--", "select id, name from tbl_countries	order by name", "id", "Name");
            

            drp_gender.Items.Clear();
            drp_gender.Items.Add(new ListItem("--Select Gender--", "0"));
            drp_gender.Items.Add(new ListItem("Male", "1"));
            drp_gender.Items.Add(new ListItem("Female", "2"));

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

            
            
            
            



            ies = new IES_Generic_Function();
            ies.ies_parameters.Clear();
            DataTable dt_data = ies.Fn_ExecutiveSql_Datatable("select (select admission_no from tbl_academic_years sub where sub.academic_id=pm.academic_id) as admission_no,replace(convert(varchar(10),getdate(),103),'/','-') as admissiondate from tbl_setcurrentacademicyear_settings pm where id='1'", 0, 0);
            if (dt_data != null && dt_data.Rows.Count > 0)
            {
                tbx_admissionno.Text = "IIKM" + Convert.ToInt32(dt_data.Rows[0]["admission_no"]).ToString("0000");
                tbx_admissiondate.Text = Convert.ToString(dt_data.Rows[0]["admissiondate"]);
                tbx_admissiondate.Enabled = false;
                tbx_admissionno.Enabled = false;
            }






        }
    }
    protected void btn_guradian_Click(object sender, EventArgs e)
    {

        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conn"].ConnectionString);

        if (con.State == ConnectionState.Closed) con.Open();
        SqlCommand cmd = new SqlCommand("UDP_Create_Student", con);
        cmd.CommandType = CommandType.StoredProcedure;      
        cmd.Parameters.Clear();

        string admission_no = Convert.ToString(tbx_admissionno.Text);
        string admission_date = "";
        if (tbx_admissiondate.Text.Trim() != "")
        {
            DateTime dtadmission_date = DateTime.ParseExact(tbx_admissiondate.Text.Trim(), "dd-MM-yyyy", null);
            admission_date = dtadmission_date.ToString("yyyy-MM-dd");
        }
        string first_name = Convert.ToString(tbx_firstname.Text);
        string middle_name = Convert.ToString(tbx_middlename.Text);
        string last_name = Convert.ToString(tbx_lastname.Text);
        string batch_id = Convert.ToString(drp_batchid.SelectedItem.Value);
        string date_of_birth = "";
        if (tbx_dob.Text.Trim() != "")
        {
            DateTime dtdate_of_birth = DateTime.ParseExact(tbx_dob.Text.Trim(), "dd-MM-yyyy", null);
            date_of_birth = dtdate_of_birth.ToString("yyyy-MM-dd");
        }
        string gender = Convert.ToString(drp_gender.SelectedItem.Value);
        string blood_group = Convert.ToString(drp_bloodgroup.SelectedItem.Value);
        string birth_place = Convert.ToString(tbx_birthplace.Text);
        string nationality_id = Convert.ToString(drp_nationality.SelectedItem.Value);
        string language_id = Convert.ToString(drp_lang.SelectedItem.Value);
        string religion_id = Convert.ToString(drp_religion.SelectedItem.Value);
        string student_category_id = Convert.ToString(drp_category.SelectedItem.Value);
        string address_line1 = Convert.ToString(tbx_add1.Text);
        string address_line2 = Convert.ToString(tbx_add2.Text);
        string city = Convert.ToString(tbx_city.Text);
        string state = Convert.ToString(tbx_state.Text);
        string pin_code = Convert.ToString(tbx_pincode.Text);
        string country_id = Convert.ToString(drp_contry.SelectedItem.Value);
        string phone1 = Convert.ToString(tbx_phone1.Text);
        string phone2 = Convert.ToString(tbx_phone2.Text);
        string email = Convert.ToString(tbx_emailid.Text);




        cmd.Parameters.AddWithValue("@admission_no", Convert.ToString(admission_no));
        cmd.Parameters.AddWithValue("@admission_date", Convert.ToString(admission_date).Trim());
        cmd.Parameters.AddWithValue("@first_name", Convert.ToString(first_name).Trim());
        cmd.Parameters.AddWithValue("@middle_name", Convert.ToString(middle_name).Trim());
        cmd.Parameters.AddWithValue("@last_name", Convert.ToString(last_name).Trim());
        
        cmd.Parameters.AddWithValue("@batch_id", Convert.ToString(batch_id));
        cmd.Parameters.AddWithValue("@date_of_birth", Convert.ToString(date_of_birth).Trim());
        cmd.Parameters.AddWithValue("@gender", Convert.ToString(gender).Trim());
        cmd.Parameters.AddWithValue("@blood_group", Convert.ToString(blood_group).Trim());
        cmd.Parameters.AddWithValue("@birth_place", Convert.ToString(birth_place).Trim());
        
        cmd.Parameters.AddWithValue("@nationality_id", Convert.ToString(nationality_id));
        cmd.Parameters.AddWithValue("@language_id", Convert.ToString(language_id).Trim());
        cmd.Parameters.AddWithValue("@religion_id", Convert.ToString(religion_id).Trim());
        cmd.Parameters.AddWithValue("@student_category_id", Convert.ToString(student_category_id).Trim());
        cmd.Parameters.AddWithValue("@address_line1", Convert.ToString(address_line1).Trim());
        
        cmd.Parameters.AddWithValue("@address_line2", Convert.ToString(address_line2));
        cmd.Parameters.AddWithValue("@city", Convert.ToString(city).Trim());
        cmd.Parameters.AddWithValue("@state", Convert.ToString(state).Trim());
        cmd.Parameters.AddWithValue("@pin_code", Convert.ToString(pin_code).Trim());
        cmd.Parameters.AddWithValue("@country_id", Convert.ToString(country_id).Trim());
        
        cmd.Parameters.AddWithValue("@phone1", Convert.ToString(phone1));
        cmd.Parameters.AddWithValue("@phone2", Convert.ToString(phone2).Trim());
        cmd.Parameters.AddWithValue("@email", Convert.ToString(email).Trim());

        string photo_file_name = "";
        string photo_content_type = "";
        string sourcefilepath = "";
        if (file_upload.HasFile)
        {

            photo_file_name = System.IO.Path.GetFileName(file_upload.FileName);
           

            if (System.IO.Directory.Exists(HttpContext.Current.Server.MapPath("~/student")) == false)
            {
                System.IO.Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~/student"));
            }
            sourcefilepath = System.IO.Path.Combine(HttpContext.Current.Server.MapPath("~/student"), photo_file_name);
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
        cmd.Parameters.AddWithValue("@created_by", Convert.ToString(Session["Usrid"]));
        cmd.Parameters.AddWithValue("@mode", "add");
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

                string studentid= Convert.ToString(dt_result.Rows[0]["studentid"]);
                Response.Redirect("Adm_StudentGuardian_Creation.aspx?id=" + studentid);
            }
        }
    }
}