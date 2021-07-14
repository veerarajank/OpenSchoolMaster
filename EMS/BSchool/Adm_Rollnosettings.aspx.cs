using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Adm_Rollnosettings : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["Usrid"] == null)
        {
            Response.Redirect("login.aspx");
        }
        if (!IsPostBack)
        {          
            BindDisplay();
        }
    }
    protected void BindDisplay()
    {

        IES_Generic_Function ies = new IES_Generic_Function();
        ies.ies_parameters.Clear();
        DataTable dt_data = ies.Fn_ExecutiveSql_Datatable("select * from tbl_rollno_settings where id='1'", 0, 0);
        if (dt_data != null && dt_data.Rows.Count > 0)
        {

            string only_rollno = Convert.ToString(dt_data.Rows[0]["only_rollno"]);
            string only_applicationno = Convert.ToString(dt_data.Rows[0]["only_applicationno"]);
            string both = Convert.ToString(dt_data.Rows[0]["both"]);

            if (only_rollno == "1") rd_rollno.Checked = true;
            else if (only_applicationno == "1") rd_applicationno.Checked = true;
            else rd_both.Checked = true;

        }
    }
    protected void btn_apply_Click(object sender, EventArgs e)
    {
        int only_rollno = 0;
        if (rd_rollno.Checked == true) only_rollno = 1;

        int only_applicationno = 0;
        if (rd_applicationno.Checked == true) only_applicationno = 1;

        int both = 0;
        if (rd_both.Checked == true) both = 1;

      
        IES_Generic_Function ies = new IES_Generic_Function();
        ies.ies_parameters.Clear();
        ies.ies_parameters.Add("@only_rollno", Convert.ToString(only_rollno));
        ies.ies_parameters.Add("@only_applicationno", Convert.ToString(only_applicationno));
        ies.ies_parameters.Add("@both", Convert.ToString(both));

        string str_qry = "Update tbl_rollno_settings set only_rollno=@only_rollno,only_applicationno=@only_applicationno,both=@both where id=1";
        ies.Fn_ExecutiveSql(str_qry, 0, 1);
        Response.Redirect("Adm_Rollnosettings.aspx");
    }
}