using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Adm_Common_Exam_Batches : System.Web.UI.Page
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
    protected void BindListView()
    {
        string id = Convert.ToString(Request.QueryString["id"]);
        IES_Generic_Function ies = new IES_Generic_Function();
        ies.ies_parameters.Clear();
        ies.ies_parameters.Add("@exam_id", Convert.ToString(id).Trim());
        DataTable dt_data = ies.Fn_ExecutiveSql_Datatable(@"select *,
        (select c.name from tbl_courses c where c.id=cc.course_id) as Course,
        (select c.name from tbl_batches c where c.id=cc.batch_id) as Batch,
        (select c.name from tbl_semester c where c.id=cc.semester_id) as Semester
        from tbl_exam_list_semester cc where exam_id=@exam_id", 0, 1);
        lv_batches.DataSource = dt_data;
        lv_batches.DataBind();
    }
 
    protected void lnk_manage_Command(object sender, CommandEventArgs e)
    {
         string id = string.Empty;
        id = Convert.ToString(e.CommandArgument);
        if (e.CommandName == "manage")
        {
            if (Convert.ToBoolean(Session["Access_Edit"]) == true)
            {


                IES_Generic_Function ies = new IES_Generic_Function();
                ies.ies_parameters.Clear();
                ies.ies_parameters.Add("@id", Convert.ToString(id).Trim());
                string str_qry = "select * from tbl_exam_list_semester where id=@id";
                DataTable dt_result = ies.Fn_ExecutiveSql_Datatable(str_qry, 0, 1);
                if (dt_result != null && dt_result.Rows.Count > 0)
                {

                    string exam_batch_id = Convert.ToString(dt_result.Rows[0]["id"]);
                    string academic_semester_id = Convert.ToString(dt_result.Rows[0]["academic_semester_id"]);
                    ies = new IES_Generic_Function();
                    ies.ies_parameters.Clear();
                    ies.ies_parameters.Add("@exam_batch_id", Convert.ToString(exam_batch_id).Trim());
                    DataTable dt_batchsubject = ies.Fn_ExecutiveSql_Datatable("select * from tbl_exam_batch_subject where exam_batch_id=@exam_batch_id", 0, 1);
                    if (dt_batchsubject != null && dt_batchsubject.Rows.Count == 0)
                    {
                        ies = new IES_Generic_Function();
                        ies.ies_parameters.Clear();
                        ies.ies_parameters.Add("@academic_semester_id", Convert.ToString(academic_semester_id).Trim());
                        str_qry = "select subject_id from tbl_academic_subjects where academic_semester=@academic_semester_id";
                        DataTable dt_academic_subject = ies.Fn_ExecutiveSql_Datatable(str_qry, 0, 1);
                        if (dt_academic_subject != null && dt_academic_subject.Rows.Count > 0)
                        {
                            for (int irow = 0; irow <= dt_academic_subject.Rows.Count - 1; irow++)
                            {
                                string subject_id = Convert.ToString(dt_academic_subject.Rows[irow]["subject_id"]);
 
                                str_qry = @"insert into tbl_exam_batch_subject(exam_batch_id,subject_id)
                                                                     values(@exam_batch_id,@subject_id)";

                                ies = new IES_Generic_Function();
                                ies.ies_parameters.Clear();
                                ies.ies_parameters.Add("@exam_batch_id", Convert.ToString(exam_batch_id).Trim());
                                ies.ies_parameters.Add("@subject_id", Convert.ToString(subject_id).Trim());                              
                                ies.Fn_ExecutiveSql(str_qry, 0, 1);
                            }
                        }
                    }
                }
            }
            Response.Redirect("Adm_Common_Exam_Subject.aspx?mode=edit&id=" + id);
        }
    }
    protected void btn_save_Click(object sender, EventArgs e)
    {
        for (int iparent = 0; iparent <= lv_batches.Items.Count - 1; iparent++)
        {
            CheckBox chk_applicable = (CheckBox)lv_batches.Items[iparent].FindControl("chk_applicable");
            CheckBox chk_date = (CheckBox)lv_batches.Items[iparent].FindControl("chk_date");
            CheckBox chk_result = (CheckBox)lv_batches.Items[iparent].FindControl("chk_result");
            HiddenField hd_examlistid = (HiddenField)lv_batches.Items[iparent].FindControl("hd_examlistid");

            string is_applicable = "0";
            if (chk_applicable.Checked) is_applicable = "1";

            string date_is_published = "0";
            if (chk_date.Checked) date_is_published = "1";

            string result_is_published = "0";
            if (chk_result.Checked) result_is_published = "1";






            string str_qry = @"update tbl_exam_list_semester set is_applicable=@is_applicable,date_is_published=@date_is_published,
                    result_is_published=@result_is_published where id=@id";

            IES_Generic_Function ies = new IES_Generic_Function();
            ies.ies_parameters.Clear();
            ies.ies_parameters.Add("@is_applicable", is_applicable);
            ies.ies_parameters.Add("@date_is_published", date_is_published);
            ies.ies_parameters.Add("@result_is_published", result_is_published);
            ies.ies_parameters.Add("@id", Convert.ToString(hd_examlistid.Value));
            ies.Fn_ExecutiveSql(str_qry, 0, 1);

        }
        BindListView();
    }
}