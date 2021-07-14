using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Adm_OnineSettings : System.Web.UI.Page
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
            ies.Fn_Executive_ListWithSelect(drp_academic_year, "select academic_id as academic_id,academic_name from tbl_academic_years where academic_status=1 order by academic_name", "academic_id", "academic_name");
            BindDisplay();

        }
        
    }
    protected void BindDisplay()
    {

        IES_Generic_Function ies = new IES_Generic_Function();
        ies.ies_parameters.Clear();
        DataTable dt_data = ies.Fn_ExecutiveSql_Datatable("select * from tbl_onlineregistration_settings where id='1'", 0, 0);
        if (dt_data != null && dt_data.Rows.Count > 0)
        {

            string academic_id = Convert.ToString(dt_data.Rows[0]["academic_id"]);
            if(drp_academic_year.Items.FindByValue(academic_id).Value!=null)
            {
                drp_academic_year.SelectedValue = academic_id;
            }
            string show_link = Convert.ToString(dt_data.Rows[0]["show_link"]);
            if (show_link == "1") chk_showlink.Checked = true;
            else chk_showlink.Checked = false;
           
        }
    }
    protected void btn_apply_Click(object sender, EventArgs e)
    {
        int show_link = 0;
        if (chk_showlink.Checked == true) show_link = 1;

        int academic_id = 0;
        if (Convert.ToString(drp_academic_year.SelectedValue) != "0") academic_id = Convert.ToInt32(drp_academic_year.SelectedValue);

        IES_Generic_Function ies = new IES_Generic_Function();
        ies.ies_parameters.Clear();
        ies.ies_parameters.Add("@show_link", Convert.ToString(show_link));
        ies.ies_parameters.Add("@academic_id", Convert.ToString(academic_id));
        string str_qry = "Update tbl_onlineregistration_settings set academic_id=@academic_id,show_link=@show_link where id=1";
        ies.Fn_ExecutiveSql(str_qry, 0, 1);
        Response.Redirect("Adm_OnineSettings.aspx");
    }
}