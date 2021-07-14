using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Adm_Current_Year_Course : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["Usrid"] == null)
        {
            Response.Redirect("login.aspx");
        }
        if (!IsPostBack)
        {

            string academic_id = Get_Academic_Year();
            

            BindListView(academic_id);




        }
    }


    protected string Get_Academic_Year()
    {
        string academic_id = "0";
        IES_Generic_Function ies = new IES_Generic_Function();
        ies.ies_parameters.Clear();
        DataTable dt_data = ies.Fn_ExecutiveSql_Datatable("select * from tbl_setcurrentacademicyear_settings where id='1'", 0, 0);
        if (dt_data != null && dt_data.Rows.Count > 0)
        {
            academic_id = Convert.ToString(dt_data.Rows[0]["academic_id"]);
        }
        return academic_id;
    }
    protected void BindListView(string academic_id)
    {
        IES_Generic_Function ies = new IES_Generic_Function();
        ies.ies_parameters.Clear();
        ies.ies_parameters.Add("@mode", "academicsearch");
        ies.ies_parameters.Add("@academic_id", academic_id);
        DataTable dt_data = ies.Fn_ExecutiveSql_Datatable("UDP_Course", 1, 1);
        if (dt_data != null)
        {
            lv_listview.DataSource = dt_data;
            lv_listview.DataBind();
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

                Response.Redirect("Adm_CreateCourses.aspx?mode=edit&id=" + id);
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
            ies.Fn_ExecutiveSql("UDP_Course", 1, 1);
            Response.Redirect("Adm_CreateCourses.aspx");
        }
    }

    protected void lnk_batches_Command(object sender, CommandEventArgs e)
    {
        string id = string.Empty;
        id = Convert.ToString(e.CommandArgument);
        if (e.CommandName == "batch")
        {
            if (Convert.ToBoolean(Session["Access_Edit"]) == true)
            {

                Response.Redirect("Adm_Course_Batches_List.aspx?courseid=" + id);
            }
        }

    }
    protected void lnk_search_Command(object sender, CommandEventArgs e)
    {
        string id = string.Empty;
        id = Convert.ToString(e.CommandArgument);
        if (e.CommandName == "view")
        {
            if (Convert.ToBoolean(Session["Access_View"]) == true)
            {
                

            }
        }

    }
    protected void lnk_batchedit_Command(object sender, CommandEventArgs e)
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
    
    protected void lnk_batchsemester_Command(object sender, CommandEventArgs e)
    {
        string id = string.Empty;
        id = Convert.ToString(e.CommandArgument);
        if (e.CommandName == "manange")
        {
            if (Convert.ToBoolean(Session["Access_Edit"]) == true)
            {
                string courseid = Bind_Course_ID(id);
                Response.Redirect("Adm_Course_Batches_Semester_List.aspx?batchid=" + id + "&courseid=" + courseid);
            }
        }
        
    }

    protected string  Bind_Course_ID(string id)
    {
        string Course_ID = "0";
        IES_Generic_Function ies = new IES_Generic_Function();
        ies.ies_parameters.Clear();
        ies.ies_parameters.Add("@id", Convert.ToString(id));
        ies.ies_parameters.Add("@mode", "edit");
        DataTable dt_data = ies.Fn_ExecutiveSql_Datatable("UDP_Batches", 1, 1);
        if (dt_data != null && dt_data.Rows.Count > 0)
        {

            Course_ID = Convert.ToString(dt_data.Rows[0]["course_id"]); 
        }
        return Course_ID;
    }
    protected void lnk_semester_Command(object sender, CommandEventArgs e)
    {
        
        string course_id = string.Empty;
        course_id = Convert.ToString(e.CommandArgument);
        if (e.CommandName == "semester")
        {
            if (Convert.ToBoolean(Session["Access_Edit"]) == true)
            {
                Response.Redirect("Adm_Course_Batches_Semester_List.aspx?courseid=" + course_id );
            }
        }
    }
}