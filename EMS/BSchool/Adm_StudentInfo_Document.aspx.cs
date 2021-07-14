using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Adm_StudentInfo_Document : System.Web.UI.Page
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
            ies.Fn_Executive_ListWithSelect_WithTitle(drp_StudentDocument_doc_type, "--Select Document Type--", "select id, name from tbl_student_document_list	order by name", "id", "Name");

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
    ////////Document Info//////////////
    protected string Find_Admission_Id()
    {
        string admission_no = "";
        string id = Convert.ToString(Request.QueryString["id"]);
        IES_Generic_Function ies = new IES_Generic_Function();
        ies.ies_parameters.Clear();
        ies.ies_parameters.Add("@id", Convert.ToString(id));
        ies.ies_parameters.Add("@mode", "search");
        DataTable dt_data = ies.Fn_ExecutiveSql_Datatable("UDP_Create_Student", 1, 1);
        if (dt_data != null && dt_data.Rows.Count > 0)
        {
            admission_no = Convert.ToString(dt_data.Rows[0]["admission_no"]);
        }
        return admission_no;
    }
    protected void BindListView()
    {
        string id = Convert.ToString(Request.QueryString["id"]);

        IES_Generic_Function ies = new IES_Generic_Function();
        ies.ies_parameters.Clear();
        ies.ies_parameters.Add("@student_id", id);
        DataTable dt_data = ies.Fn_ExecutiveSql_Datatable("select * from tbl_student_document where student_id=@student_id", 0, 1);
        if (dt_data != null)
        {

            lv_details.DataSource = dt_data;
            lv_details.DataBind();
        }
    }
    protected string Student_Acedemic_year()
    {
        string student_id = Convert.ToString(Request.QueryString["id"]);
        string academicyear = "";
        IES_Generic_Function ies = new IES_Generic_Function();
        ies.ies_parameters.Clear();
        ies.ies_parameters.Add("@student_id", student_id);
        ies.ies_parameters.Add("@mode", "studentacademic");
        DataTable dt_data = ies.Fn_ExecutiveSql_Datatable("UDP_Academic_Years", 1, 1);
        if (dt_data != null && dt_data.Rows.Count > 0)
        {
            academicyear = Convert.ToString(dt_data.Rows[0]["academic_name"]).Replace(" ", "");
        }
        return academicyear;
    }
    protected void btn_addanother_Click(object sender, EventArgs e)
    {
        SaveData();
    }
    protected void SaveData()
    {
        if (System.IO.Directory.Exists(HttpContext.Current.Server.MapPath("~/student")) == false)
        {
            System.IO.Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~/student"));
        }
        if (System.IO.Directory.Exists(HttpContext.Current.Server.MapPath("~/student/Documents")) == false)
        {
            System.IO.Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~/student/Documents"));
        }

        string year = Student_Acedemic_year();
        if (System.IO.Directory.Exists(HttpContext.Current.Server.MapPath("~/student/Documents/" + year)) == false)
        {
            System.IO.Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~/student/Documents/" + year));
        }
        string admission_no = Find_Admission_Id();

        if (System.IO.Directory.Exists(HttpContext.Current.Server.MapPath("~/student/Documents/" + year + "/" + admission_no)) == false)
        {
            System.IO.Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~/student/Documents/" + year + "/" + admission_no));
        }

        string doc_type = Convert.ToString(drp_StudentDocument_doc_type.SelectedItem.Value);
        string doc_title = Convert.ToString(drp_StudentDocument_doc_type.SelectedItem.Text);
        string doc_filename = "";
        string doc_map_path = "";

        string doc_filepath = "";
        string doc_file_type = "";
        if (file_upload.HasFile)
        {
            doc_map_path = "~/student/Documents/" + year + "/" + admission_no + "/" + System.IO.Path.GetFileName(file_upload.FileName);
            doc_filename = System.IO.Path.GetFileName(file_upload.FileName);
            string file_folder = Convert.ToString(HttpContext.Current.Server.MapPath("~/student/Documents/" + year + "/" + admission_no));
            doc_filepath = System.IO.Path.Combine(file_folder, doc_filename);
            try
            {
                if (File.Exists(doc_filepath) == true)
                {
                    try
                    {
                        File.Delete(doc_filepath);
                    }
                    catch { }
                }
                file_upload.PostedFile.SaveAs(doc_filepath);
            }
            catch
            {
            }



            double filecontentlength = 0;
            int photo_file_size = file_upload.PostedFile.ContentLength;
            string sizeunit = "Byte";
            filecontentlength = Convert.ToDouble(file_upload.PostedFile.ContentLength);
            doc_file_type = Convert.ToString(file_upload.PostedFile.ContentType);
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
            //******************************************************
            byte[] file;
            using (var stream = new FileStream(doc_filepath, FileMode.Open, FileAccess.Read))
            {
                using (var reader = new BinaryReader(stream))
                {
                    file = reader.ReadBytes((int)stream.Length);
                }
            }
            //******************************************************
            //cmd.Parameters.Add("@photo_data", SqlDbType.VarBinary, file.Length).Value = file;
            //cmd.Parameters.AddWithValue("@photo_file_size", Convert.ToString(photo_file_size).Trim());
        }
        string student_id = Convert.ToString(Request.QueryString["id"]);
        IES_Generic_Function ies = new IES_Generic_Function();
        ies.ies_parameters.Clear();
        ies.ies_parameters.Add("@student_id", Convert.ToString(student_id));
        ies.ies_parameters.Add("@doc_type", Convert.ToString(doc_type).Trim());
        ies.ies_parameters.Add("@doc_title", Convert.ToString(doc_title).Trim());
        ies.ies_parameters.Add("@doc_filename", Convert.ToString(doc_filename).Trim());
        ies.ies_parameters.Add("@doc_map_path", Convert.ToString(doc_map_path).Trim());

        ies.ies_parameters.Add("@doc_filepath", Convert.ToString(doc_filepath).Trim());
        ies.ies_parameters.Add("@doc_file_type", Convert.ToString(doc_file_type).Trim());

        ies.ies_parameters.Add("@created_by", Convert.ToString(Session["Usrid"]));
        ies.ies_parameters.Add("@mode", "add");
        DataTable dt_result = ies.Fn_ExecutiveSql_Datatable("UDP_Student_Document_Upload", 1, 1);
        if (dt_result != null && dt_result.Rows.Count > 0)
        {
            if (Convert.ToString(dt_result.Rows[0]["Error"]) == "0")
            {
                BindListView();
            }
        }

    }


    protected void ln_approved_Command(object sender, CommandEventArgs e)
    {
        string id = string.Empty;
        id = Convert.ToString(e.CommandArgument);
        if (e.CommandName == "approve")
        {
            IES_Generic_Function ies = new IES_Generic_Function();
            ies.ies_parameters.Clear();
            ies.ies_parameters.Add("@id", id);
            ies.ies_parameters.Add("@mode", "approve");
            ies.Fn_ExecutiveSql("UDP_Student_Document_Upload", 1, 1);
        }
        BindListView();
    }
    protected void ln_disapproved_Command(object sender, CommandEventArgs e)
    {
        string id = string.Empty;
        id = Convert.ToString(e.CommandArgument);
        if (e.CommandName == "disapprove")
        {
            IES_Generic_Function ies = new IES_Generic_Function();
            ies.ies_parameters.Clear();
            ies.ies_parameters.Add("@id", id);
            ies.ies_parameters.Add("@mode", "disapprove");
            ies.Fn_ExecutiveSql("UDP_Student_Document_Upload", 1, 1);
        }
        BindListView();
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
            ies.Fn_ExecutiveSql("UDP_Student_Document_Upload", 1, 1);
        }
        BindListView();
    }
    protected void ln_download_Command(object sender, CommandEventArgs e)
    {
        string id = string.Empty;
        id = Convert.ToString(e.CommandArgument);
        if (e.CommandName == "download")
        {
            IES_Generic_Function ies = new IES_Generic_Function();
            ies.ies_parameters.Clear();
            ies.ies_parameters.Add("@id", id);
            ies.ies_parameters.Add("@mode", "download");
            DataTable dt_result = ies.Fn_ExecutiveSql_Datatable("UDP_Student_Document_Upload", 1, 1);
            if (dt_result != null && dt_result.Rows.Count > 0)
            {
                if (Convert.ToString(dt_result.Rows[0]["doc_filepath"]) != "")
                {
                    string strURL = Convert.ToString(dt_result.Rows[0]["doc_map_path"]);
                    string file_name = Convert.ToString(dt_result.Rows[0]["doc_filename"]);
                    WebClient req = new WebClient();
                    HttpResponse response = HttpContext.Current.Response;
                    response.Clear();
                    response.ClearContent();
                    response.ClearHeaders();
                    response.Buffer = true;
                    response.AddHeader("Content-Disposition", "attachment;filename=\"" + file_name + "\"");
                    byte[] data = req.DownloadData(Server.MapPath(strURL));
                    response.BinaryWrite(data);
                    response.End();
                }
            }
        }
        BindListView();
    }
    protected void btn_save_Click(object sender, EventArgs e)
    {
        SaveData();
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