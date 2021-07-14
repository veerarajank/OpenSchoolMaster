using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Adm_Time_Table_SetTimeTable : System.Web.UI.Page
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
            //ies.Fn_PageLevel_Access("Brand", Convert.ToString(Session["Usrid"]));

            string academic_id = Get_Academic_Year();
            ies.Fn_Executive_ListWithSelect_WithTitle(drp_course, "--Select Course--", "select id, name from tbl_courses	order by name", "id", "Name");



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
     protected void drp_course_SelectedIndexChanged(object sender, EventArgs e)
    {
        drp_batch.Items.Clear();
        drp_semester.Items.Clear();
        IES_Generic_Function ies = new IES_Generic_Function();
        ies.Fn_Executive_ListWithSelect_WithTitle(drp_batch, "--Select Batch--", "select id, name from tbl_batches where course_id='" + Convert.ToString(drp_course.SelectedValue) + "' 	order by name", "id", "Name");

    }
    protected void drp_batch_SelectedIndexChanged(object sender, EventArgs e)
    {
        drp_semester.Items.Clear();
        IES_Generic_Function ies = new IES_Generic_Function();
        ies.Fn_Executive_ListWithSelect_WithTitle(drp_semester, "--Select Semester--", "select id, name from tbl_semester order by id", "id", "Name");

    }
    protected void drp_semester_SelectedIndexChanged(object sender, EventArgs e)
    {
        Bind_Result();
    }
    protected void Bind_Result()
    {
    }
}