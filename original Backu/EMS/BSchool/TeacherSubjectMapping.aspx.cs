using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class TeacherSubjectMapping : System.Web.UI.Page
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
             ies.Fn_Executive_ListWithSelect(drp_academic_year, "select academic_id as academic_id,academic_name from tbl_academic_years where academic_status=1 order by academic_id", "academic_id", "academic_name");

             ies = new IES_Generic_Function();
             ies.Fn_Executive_ListWithSelect(drp_dept, "select id as id,name from tbl_department where status=1 order by id", "id", "name");         



         }
    }

    protected void drp_course_SelectedIndexChanged(object sender, EventArgs e)
    {
        IES_Generic_Function ies = new IES_Generic_Function();
        ies.Fn_Executive_ListWithSelect_WithTitle(drp_semester, "--Select Semester--", @"select id,Name from tbl_semester where status=1
        and id in(SELECT  semester_id FROM tbl_academic_semester where academic_id='" + Convert.ToString(drp_academic_year.SelectedValue) + @"' and 
        course_id='" + Convert.ToString(drp_course.SelectedValue) + "') order by Name", "id", "Name");

    }
    private void Bind_Result()
    {
        string str_qry = @"select s.id,s.name,ac.id as academic_subject_id,
                            isnull(ac.teacher_id,0) as teacher_id,
                            (select first_name + ' ' + middle_name + ' ' + last_name  from tbl_employees emp where emp.id=ac.teacher_id) as Teacher,
                            (select (select dt.name from tbl_department dt where dt.id=emp.employee_department_id) as Dpt from tbl_employees emp where emp.id=ac.teacher_id) as Department 
                            from tbl_subjects s inner join tbl_academic_subjects ac on ac.subject_id=s.id and academic_semester=@academic_semester";


        string academic_semester = Convert.ToString(drp_semester.SelectedValue);
        IES_Generic_Function ies = new IES_Generic_Function();
        ies.ies_parameters.Clear();
        ies.ies_parameters.Add("@academic_semester", academic_semester);
        DataTable dt_data = ies.Fn_ExecutiveSql_Datatable(str_qry, 0, 1);
        if (dt_data != null)
        {
            lv_subjectmapping.DataSource = dt_data;
            lv_subjectmapping.DataBind();


            DataRow newdata = dt_data.NewRow();
            newdata["academic_subject_id"] = "0";
            newdata["name"] = "--Select Subject--";
            dt_data.Rows.InsertAt(newdata, 0);

            drp_subject.DataValueField = "academic_subject_id";
            drp_subject.DataTextField = "name";
            drp_subject.DataSource = dt_data;
            drp_subject.DataBind();
            drp_subject.SelectedIndex = -1;
        }
    }
    protected void drp_semester_SelectedIndexChanged(object sender, EventArgs e)
    {

        Bind_Result();
        


    }
    protected void drp_academic_year_SelectedIndexChanged(object sender, EventArgs e)
    {
        IES_Generic_Function ies = new IES_Generic_Function();        
        ies.Fn_Executive_ListWithSelect_WithTitle(drp_course, "--Select Courses--", @"select id,Name from tbl_courses where status=1
        and id in(SELECT  course_id FROM tbl_academic_batches where academic_id='" + Convert.ToString(drp_academic_year.SelectedValue) + "') order by Name", "id", "Name");
    }
    protected void lnk_subject_Command(object sender, CommandEventArgs e)
    {
        string academic_subject_id = string.Empty;
        academic_subject_id = Convert.ToString(e.CommandArgument);
        if (e.CommandName == "subject")
        {
            if (Convert.ToBoolean(Session["Access_Edit"]) == true)
            {
                string str_qry = "update tbl_academic_subjects set teacher_id=0 where id=@academic_subject_id";
                IES_Generic_Function ies = new IES_Generic_Function();
                ies.ies_parameters.Clear();
                ies.ies_parameters.Add("@academic_subject_id", Convert.ToString(academic_subject_id).Trim());
                ies.Fn_ExecutiveSql(str_qry, 0, 1);

                drp_teacher.SelectedIndex = -1;
                drp_dept.SelectedIndex = -1;
                drp_subject.SelectedIndex = -1;

                Bind_Result();               
            }
        }
    }
    protected void btn_assign_Click(object sender, EventArgs e)
    {
        string academic_subject_id = Convert.ToString(drp_subject.SelectedItem.Value);
        string teacher_id = Convert.ToString(drp_teacher.SelectedItem.Value);

        string str_qry = "update tbl_academic_subjects set teacher_id=@teacher_id where id=@academic_subject_id";



        
        IES_Generic_Function ies = new IES_Generic_Function();
        ies.ies_parameters.Clear();
        ies.ies_parameters.Add("@teacher_id", Convert.ToString(teacher_id));
        ies.ies_parameters.Add("@academic_subject_id", Convert.ToString(academic_subject_id).Trim());
        ies.Fn_ExecutiveSql(str_qry, 0, 1);

        drp_teacher.SelectedIndex = -1;
        drp_dept.SelectedIndex = -1;
        drp_subject.SelectedIndex = -1;

        Bind_Result();
        
    }
    protected void drp_dept_SelectedIndexChanged(object sender, EventArgs e)
    {

           IES_Generic_Function ies = new IES_Generic_Function();        
        ies.Fn_Executive_ListWithSelect_WithTitle(drp_teacher, "--Select Teacher--", @"select id,first_name + ' ' + middle_name + ' ' + last_name  as  Name from tbl_employees where is_active=1
        and employee_department_id in(SELECT  id FROM tbl_department where id='" + Convert.ToString(drp_dept.SelectedValue) + "') order by Name", "id", "Name");

     
    }
}