using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Adm_Course_Batches_Semester_SubjectList : System.Web.UI.Page
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
            IES_Generic_Function ies = new IES_Generic_Function();
            ies.ies_parameters.Clear();
            ies.ies_parameters.Add("@id", Convert.ToString(id));
            DataTable dt_data = ies.Fn_ExecutiveSql_Datatable("select * from tbl_academic_semester where id=@id", 0, 1);
            if (dt_data != null && dt_data.Rows.Count > 0)
            {

                hd_courseid.Value = Convert.ToString(dt_data.Rows[0]["course_id"]);
                hd_semesterid.Value = Convert.ToString(dt_data.Rows[0]["semester_id"]);
            }

            Bind_Semester_subject();
            Bind_View_Semester_subject();

        }
    }
    protected void Bind_Semester_subject()
    {
        string course_id = Convert.ToString(hd_courseid.Value);
        IES_Generic_Function ies = new IES_Generic_Function();
        ies.ies_parameters.Clear();

        ies.ies_parameters.Add("@course_id", course_id);
        ies.ies_parameters.Add("@mode", "search");
        DataTable dt_data = ies.Fn_ExecutiveSql_Datatable("UDP_Subjects", 1, 1);
        if (dt_data != null)
        {
            lbl_cnt2.InnerHtml = "&nbsp;Displaying " + Convert.ToString(dt_data.Rows.Count) + " result(s).";
            lst_addsubjects.DataSource = dt_data;
            lst_addsubjects.DataBind();
        }
    }

    protected void Bind_View_Semester_subject()
    {
        string id = Convert.ToString(Request.QueryString["id"]);

        string str_qry = @"select *,
        (select sub.name from tbl_subjects sub where sub.id=c.subject_id) as subject
        from tbl_academic_subjects  c where academic_semester=@academic_semester";
        IES_Generic_Function ies = new IES_Generic_Function();
        ies.ies_parameters.Clear();
        ies.ies_parameters.Add("@academic_semester", id);

        DataTable dt_data = ies.Fn_ExecutiveSql_Datatable(str_qry, 0, 1);
        if (dt_data != null)
        {
            lbl_cnt.InnerHtml = "&nbsp;Displaying " + Convert.ToString(dt_data.Rows.Count) + " result(s).";
            lv_subject.DataSource = dt_data;
            lv_subject.DataBind();
        }
    }
    protected void btn_new_ServerClick(object sender, EventArgs e)
    {
        add_section.Visible = true;
        search_section.Visible = false;
    }
    protected void btn_manage_ServerClick(object sender, EventArgs e)
    {
        add_section.Visible = false;
        search_section.Visible = true;
    }
    protected void Delete_Action(string id)
    {
        string str_qry = "delete from tbl_academic_subjects where academic_semester=@academic_semester";
        IES_Generic_Function ies = new IES_Generic_Function();
        ies.ies_parameters.Clear();
        ies.ies_parameters.Add("@academic_semester", id);
        ies.Fn_ExecutiveSql(str_qry, 0, 1);
    }
    protected void Insert_Action(string academic_semester, string subject_id, string max_weekly_classes, string is_elective)
    {
        string str_qry = "insert into tbl_academic_subjects(academic_semester,subject_id,max_weekly_classes,is_elective) values(@academic_semester,@subject_id,@max_weekly_classes,@is_elective)";
        IES_Generic_Function ies = new IES_Generic_Function();
        ies.ies_parameters.Clear();
        ies.ies_parameters.Add("@academic_semester", academic_semester);
        ies.ies_parameters.Add("@subject_id", subject_id);
        ies.ies_parameters.Add("@max_weekly_classes", max_weekly_classes);
        ies.ies_parameters.Add("@is_elective", is_elective);
        ies.Fn_ExecutiveSql(str_qry, 0, 1);
    }
    protected void btn_save_ServerClick(object sender, EventArgs e)
    {
        string id = Convert.ToString(Request.QueryString["id"]);
        for (int iparent = 0; iparent <= lst_addsubjects.Items.Count - 1; iparent++)
        {
            CheckBox chk_select = (CheckBox)lst_addsubjects.Items[iparent].FindControl("chk_select");
            if (chk_select != null)
            {
                CheckBox chk_elective = (CheckBox)lst_addsubjects.Items[iparent].FindControl("chk_elective");
                HiddenField hd_subject_id = (HiddenField)lst_addsubjects.Items[iparent].FindControl("hd_subject_id");
                TextBox tbx_max_class = (TextBox)lst_addsubjects.Items[iparent].FindControl("tbx_max_class");

                if (chk_elective != null && hd_subject_id != null && tbx_max_class != null)
                {
                    if (iparent == 0)
                    {
                        Delete_Action(id);
                    }
                    if (chk_select.Checked == true)
                    {
                        string is_elective = "0";
                        if (chk_elective.Checked) is_elective = "1";

                        string max_weekly_classes = Convert.ToString(tbx_max_class.Text).Trim();
                        string subject_id = Convert.ToString(hd_subject_id.Value).Trim();
                        Insert_Action(id, subject_id, max_weekly_classes, is_elective);



                    }
                }


            }


        }

        Bind_View_Semester_subject();
        add_section.Visible = false;
        search_section.Visible = true;
    }
}