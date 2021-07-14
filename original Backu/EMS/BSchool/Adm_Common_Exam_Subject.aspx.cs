using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Adm_Common_Exam_Subject : System.Web.UI.Page
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
        ies.ies_parameters.Add("@id", Convert.ToString(id).Trim());
        DataTable dt_data = ies.Fn_ExecutiveSql_Datatable(@"select *,dbo.UDF_DateFormat(exam_date,1,0) as exam_dateddMMyyy,
        (select c.name from tbl_subjects c where c.id=cc.subject_id) as subjectname
        from tbl_exam_batch_subject cc where exam_batch_id=@id", 0, 1);
        lv_subjects.DataSource = dt_data;
        lv_subjects.DataBind();
    }

   
    protected void btn_save_Click(object sender, EventArgs e)
    {
        for (int iparent = 0; iparent <= lv_subjects.Items.Count - 1; iparent++)
        {
            TextBox tbx_maximummark = (TextBox)lv_subjects.Items[iparent].FindControl("tbx_maximummark");
            TextBox tbx_minimummark = (TextBox)lv_subjects.Items[iparent].FindControl("tbx_minimummark");
            TextBox tbx_examdate = (TextBox)lv_subjects.Items[iparent].FindControl("tbx_examdate");
            HiddenField hd_examsubjectlistid = (HiddenField)lv_subjects.Items[iparent].FindControl("hd_examsubjectlistid");


            string max_mark = Convert.ToString(tbx_maximummark.Text);
            string min_mark = Convert.ToString(tbx_minimummark.Text);
           

            string exam_date = "";
            if (tbx_examdate.Text.Trim() != "")
            {
                DateTime dtexam_date = DateTime.ParseExact(tbx_examdate.Text.Trim(), "dd-MM-yyyy", null);
                exam_date = dtexam_date.ToString("yyyy-MM-dd");
            }

            string str_qry = @"update tbl_exam_batch_subject set max_mark=@max_mark,min_mark=@min_mark,
                    exam_date=@exam_date where id=@id";

            IES_Generic_Function ies = new IES_Generic_Function();
            ies.ies_parameters.Clear();
            ies.ies_parameters.Add("@max_mark", max_mark);
            ies.ies_parameters.Add("@min_mark", min_mark);
            ies.ies_parameters.Add("@exam_date", exam_date);
            ies.ies_parameters.Add("@id", Convert.ToString(hd_examsubjectlistid.Value));
            ies.Fn_ExecutiveSql(str_qry, 0, 1);

        }
        BindListView();
    }
}