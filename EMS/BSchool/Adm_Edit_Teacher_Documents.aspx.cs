using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Adm_Edit_Teacher_Documents : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["Usrid"] == null)
        {
            Response.Redirect("login.aspx");
        }
        if (!IsPostBack)
        {
            BindListView();
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
    protected void btn_save_Click(object sender, EventArgs e)
    {
        SaveData();
    }
    protected string Find_Employee_Id()
    {
        string employee_number = "";
        string id = Convert.ToString(Request.QueryString["id"]);
        IES_Generic_Function ies = new IES_Generic_Function();
        ies.ies_parameters.Clear();
        ies.ies_parameters.Add("@id", Convert.ToString(id));
        ies.ies_parameters.Add("@mode", "search");
        DataTable dt_data = ies.Fn_ExecutiveSql_Datatable("UDP_Create_Teacher", 1, 1);
        if (dt_data != null && dt_data.Rows.Count > 0)
        {
            employee_number = Convert.ToString(dt_data.Rows[0]["employee_number"]);
        }
        return employee_number;
    }
    protected void SaveData()
    {
        if (System.IO.Directory.Exists(HttpContext.Current.Server.MapPath("~/employee")) == false)
        {
            System.IO.Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~/employee"));
        }
        if (System.IO.Directory.Exists(HttpContext.Current.Server.MapPath("~/employee/Teacher")) == false)
        {
            System.IO.Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~/employee/Teacher"));
        }

        if (System.IO.Directory.Exists(HttpContext.Current.Server.MapPath("~/employee/Teacher/Documents")) == false)
        {
            System.IO.Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~/employee/Teacher/Documents"));
        }
        string employee_number = Find_Employee_Id();

        if (System.IO.Directory.Exists(HttpContext.Current.Server.MapPath("~/employee/Teacher/Documents/" + employee_number)) == false)
        {
            System.IO.Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~/employee/Teacher/Documents/" + employee_number));
        }

        string doc_type = Convert.ToString(tbx_tech_documentname.Text);
        string doc_title = Convert.ToString(tbx_tech_documentname.Text);
        string doc_filename = "";
        string doc_map_path = "";

        string doc_filepath = "";
        string doc_file_type = "";
        if (file_upload.HasFile)
        {
            doc_map_path = "~/employee/Teacher/Documents/" + employee_number + "/" + System.IO.Path.GetFileName(file_upload.FileName);
            doc_filename = System.IO.Path.GetFileName(file_upload.FileName);
            string file_folder = Convert.ToString(HttpContext.Current.Server.MapPath("~/employee/Teacher/Documents/" + employee_number));
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
        string teacher_id = Convert.ToString(Request.QueryString["id"]);
        IES_Generic_Function ies = new IES_Generic_Function();
        ies.ies_parameters.Clear();
        ies.ies_parameters.Add("@teacher_id", Convert.ToString(teacher_id));
        ies.ies_parameters.Add("@doc_type", Convert.ToString(doc_type).Trim());
        ies.ies_parameters.Add("@doc_title", Convert.ToString(doc_title).Trim());
        ies.ies_parameters.Add("@doc_filename", Convert.ToString(doc_filename).Trim());
        ies.ies_parameters.Add("@doc_map_path", Convert.ToString(doc_map_path).Trim());

        ies.ies_parameters.Add("@doc_filepath", Convert.ToString(doc_filepath).Trim());
        ies.ies_parameters.Add("@doc_file_type", Convert.ToString(doc_file_type).Trim());

        ies.ies_parameters.Add("@created_by", Convert.ToString(Session["Usrid"]));
        ies.ies_parameters.Add("@mode", "add");
        DataTable dt_result = ies.Fn_ExecutiveSql_Datatable("UDP_Teacher_Document_Upload", 1, 1);
        if (dt_result != null && dt_result.Rows.Count > 0)
        {
            if (Convert.ToString(dt_result.Rows[0]["Error"]) == "0")
            {
                BindListView();
            }
        }

    }
    protected void BindListView()
    {
        string id = Convert.ToString(Request.QueryString["id"]);

        IES_Generic_Function ies = new IES_Generic_Function();
        ies.ies_parameters.Clear();
        ies.ies_parameters.Add("@teacher_id", id);
        DataTable dt_data = ies.Fn_ExecutiveSql_Datatable("select * from tbl_teacher_document where teacher_id=@teacher_id", 0, 1);
        if (dt_data != null)
        {

            lv_details.DataSource = dt_data;
            lv_details.DataBind();
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
            ies.Fn_ExecutiveSql("UDP_Teacher_Document_Upload", 1, 1);
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
            ies.Fn_ExecutiveSql("UDP_Teacher_Document_Upload", 1, 1);
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
            ies.Fn_ExecutiveSql("UDP_Teacher_Document_Upload", 1, 1);
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
            DataTable dt_result = ies.Fn_ExecutiveSql_Datatable("UDP_Teacher_Document_Upload", 1, 1);
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
   
}