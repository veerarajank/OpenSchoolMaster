using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Adm_StudentGuardian_PreviousDetails : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
       if (Session["Usrid"] == null)
        {
            Response.Redirect("login.aspx");
        }
       if (!IsPostBack)
       {
           int currentyear = DateTime.Now.Year;
           drp_StudentPreviousDatas_year.Items.Clear();
           drp_StudentPreviousDatas_year.Items.Add(new ListItem("Year","0"));

           for (int i = 0; i <= 20; i++)
           {
               drp_StudentPreviousDatas_year.Items.Add(new ListItem((currentyear - i).ToString(), (currentyear - i).ToString()));
           }
           BindListView();
       }
    }

    protected void BindListView()
    {
        string student_id = Convert.ToString(Request.QueryString["id"]);
        IES_Generic_Function ies = new IES_Generic_Function();
        ies.ies_parameters.Clear();
        ies.ies_parameters.Add("@student_id", student_id);
        ies.ies_parameters.Add("@mode", "search");
        DataTable dt_data = ies.Fn_ExecutiveSql_Datatable("UDP_Student_PreviousData", 1, 1);
        if (dt_data != null)
        {
          
            lv_previousedetails.DataSource = dt_data;
            lv_previousedetails.DataBind();
        }
    }
   
    protected void btn_save_Click(object sender, EventArgs e)
    {

        string student_id = Convert.ToString(Request.QueryString["id"]);

        IES_Generic_Function ies = new IES_Generic_Function();
        ies.ies_parameters.Clear();

        string institution = Convert.ToString(tbx_StudentPreviousDatas_institution.Text);
        string year = Convert.ToString(drp_StudentPreviousDatas_year.SelectedItem.Value);
        string course = Convert.ToString(tbx_StudentPreviousDatas_course.Text);
        string total_mark = Convert.ToString(tbx_StudentPreviousDatas_total_mark.Text);

        ies.ies_parameters.Add("@student_id", student_id);
        ies.ies_parameters.Add("@institution", Convert.ToString(institution));
        ies.ies_parameters.Add("@year", Convert.ToString(year));
        ies.ies_parameters.Add("@course", Convert.ToString(course));
        ies.ies_parameters.Add("@total_mark", Convert.ToString(total_mark));


        ies.ies_parameters.Add("@mode", "add");
        DataTable dt_result = ies.Fn_ExecutiveSql_Datatable("UDP_Student_PreviousData", 1, 1);
        if (dt_result != null && dt_result.Rows.Count > 0)
        {
            if (Convert.ToString(dt_result.Rows[0]["Error"]) == "1")
            {
                lbl_error.InnerHtml = Convert.ToString(dt_result.Rows[0]["Msg"]);
            }
            if (Convert.ToString(dt_result.Rows[0]["Error"]) == "0")
            {
                tbx_StudentPreviousDatas_institution.Text = "";
                tbx_StudentPreviousDatas_course.Text = "";
                tbx_StudentPreviousDatas_total_mark.Text = "";
                drp_StudentPreviousDatas_year.SelectedIndex = -1;
            }
        }
        

        BindListView();
    }
    protected void btn_next_Click(object sender, EventArgs e)
    {
        string id = Convert.ToString(Request.QueryString["id"]);
        Response.Redirect("Adm_StudentGuardian_Documentype.aspx?mode=edit&id=" + id);
    }
    protected void lnk_studentdetails_ServerClick(object sender, EventArgs e)
    {
        string id = Convert.ToString(Request.QueryString["id"]);
        Response.Redirect("Adm_StudentCreation_Edit.aspx?mode=edit&id=" + id);
    }
    protected void lnk_guardiandetails_ServerClick(object sender, EventArgs e)
    {
        string id = Convert.ToString(Request.QueryString["id"]);
        Response.Redirect("Adm_StudentGuardian_Creation.aspx?mode=edit&id=" + id);
    }
    protected void lnk_previousdetails_ServerClick(object sender, EventArgs e)
    {
        string id = Convert.ToString(Request.QueryString["id"]);
        Response.Redirect("Adm_StudentGuardian_PreviousDetails.aspx?mode=edit&id=" + id);
    }
    protected void lnk_documentdetails_ServerClick(object sender, EventArgs e)
    {
        string id = Convert.ToString(Request.QueryString["id"]);
        Response.Redirect("Adm_StudentGuardian_Documentype.aspx?mode=edit&id=" + id);
    }
    protected void btn_del_ServerClick(object sender, EventArgs e)
    {
        IES_Generic_Function ies = new IES_Generic_Function();
        ies.ies_parameters.Clear();
        ies.ies_parameters.Add("@id", Convert.ToString(hd_id.Value));
        ies.ies_parameters.Add("@mode", "del");
        ies.Fn_ExecutiveSql("UDP_Student_PreviousData", 1, 1);
        BindListView();
    }
    
}