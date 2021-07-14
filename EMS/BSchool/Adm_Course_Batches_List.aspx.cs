using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Adm_Course_Batches_List : System.Web.UI.Page
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


            string course_id = Convert.ToString(Request.QueryString["courseid"]); 

            string academic_id = "0";
            ies = new IES_Generic_Function();
            ies.ies_parameters.Clear();
            DataTable dt_data = ies.Fn_ExecutiveSql_Datatable("select * from tbl_setcurrentacademicyear_settings where id='1'", 0, 0);
            if (dt_data != null && dt_data.Rows.Count > 0)
            {
                academic_id = Convert.ToString(dt_data.Rows[0]["academic_id"]);

                if (drp_academic_year.Items.FindByValue(Convert.ToString(dt_data.Rows[0]["academic_id"])) != null)
                {
                    drp_academic_year.SelectedValue = Convert.ToString(dt_data.Rows[0]["academic_id"]);
                }
            }
            lbl_course.Text = Get_Course_Name(course_id);
            BindListView(academic_id, course_id);

           
            // BindListView();
        }
    }

    protected string Get_Course_Name(string course_id)
    {
        string coursename = "";
        IES_Generic_Function ies = new IES_Generic_Function();
        ies.ies_parameters.Clear();
        ies.ies_parameters.Add("@course_id", course_id);
        DataTable dt_data = ies.Fn_ExecutiveSql_Datatable("select * from tbl_courses where id=@course_id", 0, 1);
        if (dt_data != null && dt_data.Rows.Count > 0)
        {
            coursename = Convert.ToString(dt_data.Rows[0]["name"]);
        }
        return coursename;
    }

    protected void BindListView(string academic_id, string course_id)
    {
        IES_Generic_Function ies = new IES_Generic_Function();
        ies.ies_parameters.Clear();
        ies.ies_parameters.Add("@mode", "academicbatchsearch");
        ies.ies_parameters.Add("@academic_id", academic_id);
        ies.ies_parameters.Add("@course_id", course_id);
        DataTable dt_data = ies.Fn_ExecutiveSql_Datatable("UDP_Course", 1, 1);
        if (dt_data != null)
        {           
            lv_listview.DataSource = dt_data;
            lv_listview.DataBind();
        }
    }
    protected void btn_new_Click(object sender, EventArgs e)
    {
       lv_listview.Visible=false;
       lst_linkbatch.Visible = true;
            string course_id = Convert.ToString(Request.QueryString["courseid"]);
        string academic_id = "0";
        academic_id = Convert.ToString(drp_academic_year.SelectedItem.Value);
        BindListView(academic_id, course_id);
        Link_BindListView(academic_id, course_id);

    }

    protected void Link_BindListView(string academic_id, string course_id)
    {
        IES_Generic_Function ies = new IES_Generic_Function();
        ies.ies_parameters.Clear();
        ies.ies_parameters.Add("@mode", "linkacademicbatchsearch");
        ies.ies_parameters.Add("@academic_id", academic_id);
        ies.ies_parameters.Add("@course_id", course_id);
        DataTable dt_data = ies.Fn_ExecutiveSql_Datatable("UDP_Course", 1, 1);
        if (dt_data != null)
        {
            lst_linkbatch.DataSource = dt_data;
            lst_linkbatch.DataBind();
        }
    }
    protected void drp_academic_year_SelectedIndexChanged(object sender, EventArgs e)
    {
        string course_id = Convert.ToString(Request.QueryString["courseid"]);
        string academic_id = "0";
        academic_id = Convert.ToString(drp_academic_year.SelectedItem.Value);
        BindListView(academic_id, course_id);

        lv_listview.Visible = true;
        lst_linkbatch.Visible = false;
            
    }
   
    protected void lnk_edit_Command(object sender, CommandEventArgs e)
    {
        string id = string.Empty;
        id = Convert.ToString(e.CommandArgument);
        if (e.CommandName == "edit")
        {
            if (Convert.ToBoolean(Session["Access_Edit"]) == true)
            {

                Response.Redirect("Adm_Courses_Batch.aspx?mode=edit&id=" + id);
            }
        }
    }
    protected void btn_del_ServerClick(object sender, EventArgs e)
    {
        string course_id = Convert.ToString(Request.QueryString["courseid"]);
        if (Convert.ToBoolean(Session["Access_Trash"]) == true)
        {
            IES_Generic_Function ies = new IES_Generic_Function();
            ies.ies_parameters.Clear();
            ies.ies_parameters.Add("@academic_batchidid", Convert.ToString(hd_id.Value));
            ies.ies_parameters.Add("@mode", "acedamicbatchdel");
            ies.Fn_ExecutiveSql("UDP_Course", 1, 1);
            Response.Redirect("Adm_Course_Batches_List.aspx?courseid=" + course_id);
        }
    }
    protected void btn_reset_ServerClick(object sender, EventArgs e)
    {
        string course_id = Convert.ToString(Request.QueryString["courseid"]);
        
            IES_Generic_Function ies = new IES_Generic_Function();
            ies.ies_parameters.Clear();
            ies.ies_parameters.Add("@academic_batchidid", Convert.ToString(hd_id.Value));
            ies.ies_parameters.Add("@mode", "acedamicbatchreset");
            ies.Fn_ExecutiveSql("UDP_Course", 1, 1);
            Response.Redirect("Adm_Course_Batches_List.aspx?courseid=" + course_id);
       
    }
    protected void lnk_linkbatch_ServerClick(object sender, EventArgs e)
    {
        string course_id = Convert.ToString(Request.QueryString["courseid"]);
        
        string academic_id = "0";
        academic_id = Convert.ToString(drp_academic_year.SelectedItem.Value);

        IES_Generic_Function ies = new IES_Generic_Function();
        ies.ies_parameters.Clear();
        ies.ies_parameters.Add("@batch_id", Convert.ToString(hd_id.Value));
        ies.ies_parameters.Add("@course_id", Convert.ToString(course_id));
        ies.ies_parameters.Add("@academic_id", Convert.ToString(academic_id));
        ies.ies_parameters.Add("@is_active", Convert.ToString("1"));
        ies.ies_parameters.Add("@mode", "add");
        DataTable dt_data =ies.Fn_ExecutiveSql_Datatable("UDP_Course_Semester", 1, 1);
        Response.Redirect("Adm_Course_Batches_List.aspx?courseid=" + course_id);
    }
}