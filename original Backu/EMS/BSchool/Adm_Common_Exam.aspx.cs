using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Adm_Common_Exam : System.Web.UI.Page
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

            search_section.Visible = false;
            view_section.Visible = false;
            add_section.Visible = false;
            update_section.Visible = false;
            delete_section.Visible = false;

            string mode = Convert.ToString(Request.QueryString["mode"]);
            if (mode != null && mode == "search")
            {
                search_section.Visible = true;
                BindListView();
            }
            else if (mode != null && mode == "add")
            {
                add_section.Visible = true;
            }
            else if (mode != null && mode == "edit")
            {
                update_section.Visible = true;
                BindDisplay();
            }
            else if (mode != null && mode == "view")
            {
                view_section.Visible = true;
                BindDisplay();
            }
            else if (mode != null && mode == "del")
            {
                delete_section.Visible = true;
            }
            else if (mode == null)
            {
                search_section.Visible = true;
                BindListView();
            }




        }


    }


    protected void btn_del_ServerClick(object sender, EventArgs e)
    {
        if (Convert.ToBoolean(Session["Access_Trash"]) == true)
        {
            IES_Generic_Function ies = new IES_Generic_Function();
            ies.ies_parameters.Clear();
            ies.ies_parameters.Add("@id", Convert.ToString(hd_id.Value));
            ies.ies_parameters.Add("@mode", "del");
            ies.Fn_ExecutiveSql("UDP_Common_Exam", 1, 1);
            Response.Redirect("Adm_Common_Exam.aspx?mode=search");
        }
    }
    protected void BindListView()
    {


        IES_Generic_Function ies = new IES_Generic_Function();
        ies.ies_parameters.Clear();
        ies.ies_parameters.Add("@mode", "search");
        DataTable dt_data = ies.Fn_ExecutiveSql_Datatable("UDP_Common_Exam", 1, 1);
        if (dt_data != null)
        {
            lv_details.DataSource = dt_data;
            lv_details.DataBind();
        }
    }
    protected void lnk_search_Command(object sender, CommandEventArgs e)
    {

        string id = string.Empty;
        id = Convert.ToString(e.CommandArgument);
        if (e.CommandName == "view")
        {
            if (Convert.ToBoolean(Session["Access_Edit"]) == true)
            {
                Response.Redirect("Adm_Common_Exam.aspx?mode=view&id=" + id);
            }
        }
    }
    protected void lnk_edit_Command(object sender, CommandEventArgs e)
    {
        string id = string.Empty;
        id = Convert.ToString(e.CommandArgument);
        if (e.CommandName == "edit")
        {
            if (Convert.ToBoolean(Session["Access_Edit"]) == true)
            {

                Response.Redirect("Adm_Common_Exam.aspx?mode=edit&id=" + id);
            }
        }

    }

    protected void btn_update_ServerClick(object sender, EventArgs e)
    {
        string id = Convert.ToString(Request.QueryString["id"]);
        if (EditValidInput() == true) { edit_field_mandary.Attributes.Add("style", "color:red"); return; }


        IES_Generic_Function ies = new IES_Generic_Function();
        ies.ies_parameters.Clear();


        string exam_status = "1";

        string exam_name = Convert.ToString(tbx_edit_name.Text);



        string exam_type = Convert.ToString(drp_edit_examtype.SelectedValue);



        string date_is_published = "0";
        if (chk_edit_isdatepublished.Checked == true) date_is_published = "1";

        string result_is_published = "0";
        if (chk_edit_isresultpublished.Checked == true) result_is_published = "1";


        string exam_startdate = "";
        if (tbx_edit_examdate.Text.Trim() != "")
        {
            DateTime dtexam_startdate = DateTime.ParseExact(tbx_edit_examdate.Text.Trim(), "dd-MM-yyyy", null);
            exam_startdate = dtexam_startdate.ToString("yyyy-MM-dd");
        }

        ies.ies_parameters.Add("@id", Convert.ToString(id).Trim());
        ies.ies_parameters.Add("@exam_name", Convert.ToString(exam_name).Trim());
        ies.ies_parameters.Add("@exam_type", Convert.ToString(exam_type).Trim());
        ies.ies_parameters.Add("@exam_status", Convert.ToString(exam_status).Trim());
        ies.ies_parameters.Add("@date_is_published", Convert.ToString(date_is_published).Trim());
        ies.ies_parameters.Add("@result_is_published", Convert.ToString(result_is_published).Trim());
        ies.ies_parameters.Add("@exam_startdate", Convert.ToString(exam_startdate).Trim());


        ies.ies_parameters.Add("@mode", "update");
        DataTable dt_result = ies.Fn_ExecutiveSql_Datatable("UDP_Common_Exam", 1, 1);
        if (dt_result != null && dt_result.Rows.Count > 0)
        {
            if (Convert.ToString(dt_result.Rows[0]["Error"]) == "0")
            {
                Response.Redirect("Adm_Common_Exam.aspx?mode=search");
            }
            else if (Convert.ToString(dt_result.Rows[0]["Error"]) == "1")
            {
                lbl_error.InnerHtml = Convert.ToString(dt_result.Rows[0]["Msg"]);
            }

        }
    }
    protected void btn_edit_ServerClick(object sender, EventArgs e)
    {
        string id = Convert.ToString(Request.QueryString["id"]);
        Response.Redirect("Adm_Common_Exam.aspx?mode=edit&id=" + id);
    }
    protected void btn_manage_ServerClick(object sender, EventArgs e)
    {
        Response.Redirect("Adm_Common_Exam.aspx?mode=search");
    }
    protected void BindDisplay()
    {
        string id = Convert.ToString(Request.QueryString["id"]);
        IES_Generic_Function ies = new IES_Generic_Function();
        ies.ies_parameters.Clear();
        ies.ies_parameters.Add("@id", Convert.ToString(id));
        ies.ies_parameters.Add("@mode", "search");
        DataTable dt_data = ies.Fn_ExecutiveSql_Datatable("UDP_Common_Exam", 1, 1);
        if (dt_data != null && dt_data.Rows.Count > 0)
        {
            lbl_name.InnerHtml = Convert.ToString(dt_data.Rows[0]["exam_name"]);
            tbx_edit_name.Text = Convert.ToString(dt_data.Rows[0]["exam_name"]);
            drp_edit_examtype.SelectedValue = Convert.ToString(dt_data.Rows[0]["exam_type"]);
            if (Convert.ToString(dt_data.Rows[0]["exam_type"]) == "1")
            {
                lbl_examtype.InnerHtml = "Marks";

            }
            else if (Convert.ToString(dt_data.Rows[0]["exam_type"]) == "2")
            {
                lbl_examtype.InnerHtml = "Grades";
            }
            else if (Convert.ToString(dt_data.Rows[0]["exam_type"]) == "3")
            {
                lbl_examtype.InnerHtml = "Marks & Grades";
            }

            if (Convert.ToString(dt_data.Rows[0]["date_is_published"]) == "1")
            {
                lbl_isdatepublished.InnerHtml = "<i class='fa fa-check'></i>";
                lbl_isdatepublished.Attributes.Add("class", "label label-success");
                chk_edit_isdatepublished.Checked = true;
            }
            else
            {
                lbl_isdatepublished.InnerHtml = "<i class='fa fa-remove'></i>";
                lbl_isdatepublished.Attributes.Add("class", "label label-warning");
                chk_edit_isdatepublished.Checked = false;
            }

            if (Convert.ToString(dt_data.Rows[0]["result_is_published"]) == "1")
            {
                lbl_isresultpublished.InnerHtml = "<i class='fa fa-check'></i>";
                lbl_isresultpublished.Attributes.Add("class", "label label-success");
                chk_edit_isresultpublished.Checked = true;
            }
            else
            {
                lbl_isresultpublished.InnerHtml = "<i class='fa fa-remove'></i>";
                lbl_isresultpublished.Attributes.Add("class", "label label-warning");
                chk_edit_isresultpublished.Checked = true;
            }

            lbl_examdate.InnerHtml = Convert.ToString(dt_data.Rows[0]["exam_startdate"]);
            tbx_edit_examdate.Text = Convert.ToString(dt_data.Rows[0]["exam_startdate"]);





        }
    }

    protected bool ValidInput()
    {
        bool iserror = false;
        if (tbx_name.Text.Trim() == "") iserror = true;
        if (drp_examtype.SelectedValue.Trim() == "0") iserror = true;
        if (tbx_examdate.Text.Trim() == "") iserror = true;


        return iserror;

    }
    protected bool EditValidInput()
    {
        bool iserror = false;
        if (tbx_edit_name.Text.Trim() == "") iserror = true;
        if (drp_edit_examtype.SelectedValue.Trim() == "0") iserror = true;
        if (tbx_edit_examdate.Text.Trim() == "") iserror = true;
        return iserror;
    }



    protected void btn_manage_classtiming_Click(object sender, EventArgs e)
    {
        Response.Redirect("Adm_Common_Exam.aspx?mode=search");
    }
    protected void btn_newtiming_ServerClick(object sender, EventArgs e)
    {
        if (ValidInput() == true) { field_mandary.Attributes.Add("style", "color:red"); return; }


        IES_Generic_Function ies = new IES_Generic_Function();
        ies.ies_parameters.Clear();

        string is_active = "1";

        string exam_status = "1";

        string exam_name = Convert.ToString(tbx_name.Text);



        string exam_type = Convert.ToString(drp_examtype.SelectedValue);



        string date_is_published = "0";
        if (chk_isdatepublished.Checked == true) date_is_published = "1";

        string result_is_published = "0";
        if (chk_isresultpublished.Checked == true) result_is_published = "1";


        string exam_startdate = "";
        if (tbx_examdate.Text.Trim() != "")
        {
            DateTime dtexam_startdate = DateTime.ParseExact(tbx_examdate.Text.Trim(), "dd-MM-yyyy", null);
            exam_startdate = dtexam_startdate.ToString("yyyy-MM-dd");
        }


        ies.ies_parameters.Add("@exam_name", Convert.ToString(exam_name).Trim());
        ies.ies_parameters.Add("@exam_type", Convert.ToString(exam_type).Trim());
        ies.ies_parameters.Add("@exam_status", Convert.ToString(exam_status).Trim());
        ies.ies_parameters.Add("@date_is_published", Convert.ToString(date_is_published).Trim());
        ies.ies_parameters.Add("@result_is_published", Convert.ToString(result_is_published).Trim());
        ies.ies_parameters.Add("@exam_startdate", Convert.ToString(exam_startdate).Trim());




        ies.ies_parameters.Add("@mode", "add");
        DataTable dt_result = ies.Fn_ExecutiveSql_Datatable("UDP_Common_Exam", 1, 1);
        if (dt_result != null && dt_result.Rows.Count > 0)
        {
            if (Convert.ToString(dt_result.Rows[0]["Error"]) == "0")
            {

                Response.Redirect("Adm_Common_Exam.aspx?mode=search");
            }
            else if (Convert.ToString(dt_result.Rows[0]["Error"]) == "1")
            {
                lbl_error.InnerHtml = Convert.ToString(dt_result.Rows[0]["Msg"]);
            }

        }
    }
    protected void btn_addclasstiming_up_screen_Click(object sender, EventArgs e)
    {
        Response.Redirect("Adm_Common_Exam.aspx?mode=add");
    }
    protected void btn_manageclasstiming_up_screen_Click(object sender, EventArgs e)
    {
        Response.Redirect("Adm_Common_Exam.aspx?mode=search");
    }

    protected void btn_new_ServerClick(object sender, EventArgs e)
    {
        Response.Redirect("Adm_Common_Exam.aspx?mode=add");
    }

    protected void lnk_manage_Command(object sender, CommandEventArgs e)
    {
        string id = string.Empty;
        id = Convert.ToString(e.CommandArgument);
        if (e.CommandName == "manage")
        {
            if (Convert.ToBoolean(Session["Access_Edit"]) == true)
            {

                string exam_id = id;
                IES_Generic_Function ies = new IES_Generic_Function();
                ies.ies_parameters.Clear();
                ies.ies_parameters.Add("@exam_id", Convert.ToString(exam_id).Trim());
                string str_qry = "select * from tbl_exam_list_semester where exam_id=@exam_id";
                DataTable dt_result = ies.Fn_ExecutiveSql_Datatable(str_qry, 0, 1);
                if (dt_result != null && dt_result.Rows.Count == 0)
                {


                    string academic_id = "";
                    ies = new IES_Generic_Function();
                    ies.ies_parameters.Clear();
                    DataTable dt_academic = ies.Fn_ExecutiveSql_Datatable("select academic_id from tbl_setcurrentacademicyear_settings", 0, 1);
                    if (dt_academic != null && dt_academic.Rows.Count > 0)
                    {
                        academic_id = Convert.ToString(dt_academic.Rows[0]["academic_id"]);
                    }



                    ies = new IES_Generic_Function();
                    ies.ies_parameters.Clear();
                    ies.ies_parameters.Add("@academic_id", Convert.ToString(academic_id).Trim());
                    str_qry = "select * from tbl_academic_batches where academic_id=@academic_id";
                    DataTable dt_batches = ies.Fn_ExecutiveSql_Datatable(str_qry, 0, 1);
                    if (dt_batches != null && dt_batches.Rows.Count > 0)
                    {
                        for (int irow = 0; irow <= dt_batches.Rows.Count - 1; irow++)
                        {


                            string course_id = Convert.ToString(dt_batches.Rows[irow]["course_id"]);
                            string batch_id = Convert.ToString(dt_batches.Rows[irow]["batch_id"]);
                            string academic_batch_id = Convert.ToString(dt_batches.Rows[irow]["id"]);


                            ies = new IES_Generic_Function();
                            ies.ies_parameters.Clear();
                            ies.ies_parameters.Add("@academic_id", Convert.ToString(academic_id).Trim());
                            ies.ies_parameters.Add("@course_id", Convert.ToString(course_id).Trim());
                            str_qry = "select * from tbl_academic_semester where academic_id=@academic_id and course_id=@course_id";
                            DataTable dt_semester = ies.Fn_ExecutiveSql_Datatable(str_qry, 0, 1);
                            if (dt_semester != null && dt_semester.Rows.Count > 0)
                            {
                                for (int is_row = 0; is_row <= dt_semester.Rows.Count - 1; is_row++)
                                {

                                    string semester_id = Convert.ToString(dt_semester.Rows[is_row]["semester_id"]);
                                    string academic_semester_id = Convert.ToString(dt_semester.Rows[is_row]["id"]);

                                    str_qry = @"insert into tbl_exam_list_semester(exam_id,course_id,batch_id,
                                                                     academic_id,semester_id,
                                                                     academic_semester_id,academic_batch_id,
                                                                     is_applicable,date_is_published,result_is_published)
                                                                     values(@exam_id,@course_id,@batch_id,
                                                                     @academic_id,@semester_id,
                                                                     @academic_semester_id,@academic_batch_id,
                                                                     '0','0','0')";

                                    ies = new IES_Generic_Function();
                                    ies.ies_parameters.Clear();
                                    ies.ies_parameters.Add("@exam_id", Convert.ToString(exam_id).Trim());
                                    ies.ies_parameters.Add("@course_id", Convert.ToString(course_id).Trim());
                                    ies.ies_parameters.Add("@batch_id", Convert.ToString(batch_id).Trim());
                                    ies.ies_parameters.Add("@academic_id", Convert.ToString(academic_id).Trim());
                                    ies.ies_parameters.Add("@semester_id", Convert.ToString(semester_id).Trim());
                                    ies.ies_parameters.Add("@academic_semester_id", Convert.ToString(academic_semester_id).Trim());
                                    ies.ies_parameters.Add("@academic_batch_id", Convert.ToString(academic_batch_id).Trim());
                                    ies.Fn_ExecutiveSql(str_qry, 0, 1);

                                }
                            }








                        }

                    }


                }

                Response.Redirect("Adm_Common_Exam_Batches.aspx?mode=edit&id=" + id);
            }
        }
    }
}