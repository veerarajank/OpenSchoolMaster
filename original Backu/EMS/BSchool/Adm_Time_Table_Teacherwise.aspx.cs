using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Adm_Time_Table_Teacherwise : System.Web.UI.Page
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

            ies = new IES_Generic_Function();
            ies.Fn_Executive_ListWithSelect_WithTitle(drp_teacherdept, "--Select Department--", "select id, name from tbl_department	order by name", "id", "Name");

            ies = new IES_Generic_Function();
            ies.Fn_Executive_ListWithSelect_WithTitle(drp_teacher, "--Select Teacher--", "select id,first_name + ' ' + middle_name + ' ' + last_name as empname from tbl_employees	order by empname", "id", "empname");
        

        }

        

    }
}