using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Adm_Online_Application_view : System.Web.UI.Page
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
            ies.ies_parameters.Clear();
            DataTable dt_data = ies.Fn_ExecutiveSql_Datatable("select (select academic_name from tbl_academic_years sub where sub.academic_id=pm.academic_id) as academic_name,pm.academic_id,replace(convert(varchar(10),getdate(),103),'/','-') as admissiondate from tbl_onlineregistration_settings pm where id='1'", 0, 0);
            if (dt_data != null && dt_data.Rows.Count > 0)
            {
                lbl_acyear.InnerHtml = "Academic Year - " + Convert.ToString(dt_data.Rows[0]["academic_name"]);
                hd_acyear.Value = Convert.ToString(dt_data.Rows[0]["academic_id"]); 
            }
            Bind_Details(Convert.ToString(hd_acyear.Value));
        }
    }
    protected void Bind_Details(string academic_yr)
    {

        string mode = Convert.ToString(Request.QueryString["mode"]);

        string str_qry = @"select online_id as id,'Adm_Online_Application_FormView.aspx?id=' + Convert(varchar,online_id) as link,upper(name) as name, CONVERT(varchar(20),created_at,24) as adm_time,
                        CONVERT(varchar(20),created_at,106) as adm_date,
                        admission_no,email_id,mobile_no,course_name,
                        case when isnull(is_approved,'0')='0' or isnull(is_denied,'0')='0' and ISNULL(is_waitinglist,'0')='0' then 'tag_pending'
	                    when isnull(is_approved,'0')='1' and isnull(is_denied,'0')='0' and ISNULL(is_waitinglist,'0')='0' then 'tag_approved'
	                    when isnull(is_approved,'0')='0' and isnull(is_denied,'0')='1' and ISNULL(is_waitinglist,'0')='0' then 'tag_disapproved'
	                    when isnull(is_approved,'0')='0' and isnull(is_denied,'0')='0' and ISNULL(is_waitinglist,'0')='1' then 'tag_waiting'
	                    end as status_class,
                        case when isnull(is_approved,'0')='0' and isnull(is_denied,'0')='0' and ISNULL(is_waitinglist,'0')='0' then 'Pending'
	                    when isnull(is_approved,'0')='1' and isnull(is_denied,'0')='0' and ISNULL(is_waitinglist,'0')='0' then 'Approved'
	                    when isnull(is_approved,'0')='0' and isnull(is_denied,'0')='1' and ISNULL(is_waitinglist,'0')='0' then 'Disapproved'
	                    when isnull(is_approved,'0')='0' and isnull(is_denied,'0')='0' and ISNULL(is_waitinglist,'0')='1' then 'Waiting'
	                    end as status
                        from tbl_student_online where academic_yr=@academic_yr and isnull(is_active,'1')='1' ";

        if (mode == "approved")
        {
            str_qry += " and isnull(is_approved,'0')='1'";
        }
        if (mode == "waiting")
        {
            str_qry += " and isnull(is_waitinglist,'0')='1'";
        }
        if (mode == "dropped")
        {
            str_qry += " and isnull(is_denied,'0')='1'";
        }
       
        IES_Generic_Function ies = new IES_Generic_Function();
        ies.ies_parameters.Clear();
        ies.ies_parameters.Add("@academic_yr", Convert.ToString(academic_yr));
        DataTable dt_data = ies.Fn_ExecutiveSql_Datatable(str_qry, 0, 1);
        if (dt_data != null)
        {
            lv_details.DataSource = dt_data;
            lv_details.DataBind();
        }
    }
    protected void btn_new_ServerClick(object sender, EventArgs e)
    {
        Response.Redirect("Adm_Online_Application.aspx");
    }
    protected void ln_approved_Command(object sender, CommandEventArgs e)
    {
        string id = string.Empty;
        id = Convert.ToString(e.CommandArgument);
        if (e.CommandName == "approve")
        {

            string str_qry = "update tbl_student_online set is_approved='1',is_denied='0',is_waitinglist='0' where online_id=@online_id";
            IES_Generic_Function ies = new IES_Generic_Function();
            ies.ies_parameters.Clear();
            ies.ies_parameters.Add("@online_id", id);
            ies.Fn_ExecutiveSql(str_qry, 0, 1);
        }
        Bind_Details(Convert.ToString(hd_acyear.Value));
    }
    protected void ln_disapproved_Command(object sender, CommandEventArgs e)
    {
        string id = string.Empty;
        id = Convert.ToString(e.CommandArgument);
        if (e.CommandName == "disapprove")
        {

            string str_qry = "update tbl_student_online set is_approved='0',is_denied='1',is_waitinglist='0' where online_id=@online_id";
            IES_Generic_Function ies = new IES_Generic_Function();
            ies.ies_parameters.Clear();
            ies.ies_parameters.Add("@online_id", id);
            ies.Fn_ExecutiveSql(str_qry, 0, 1);
        }
        Bind_Details(Convert.ToString(hd_acyear.Value));
    }
    protected void ln_deleted_Command(object sender, CommandEventArgs e)
    {
        string id = string.Empty;
        id = Convert.ToString(e.CommandArgument);
        if (e.CommandName == "delete")
        {

            string str_qry = "update tbl_student_online set is_approved='0',is_denied='1',is_waitinglist='0',is_active='0' where online_id=@online_id";
            IES_Generic_Function ies = new IES_Generic_Function();
            ies.ies_parameters.Clear();
            ies.ies_parameters.Add("@online_id", id);
            ies.Fn_ExecutiveSql(str_qry, 0, 1);
        }
        Bind_Details(Convert.ToString(hd_acyear.Value));
    }
    
    protected void ln_waiting_Command(object sender, CommandEventArgs e)
    {
        string id = string.Empty;
        id = Convert.ToString(e.CommandArgument);
        if (e.CommandName == "waiting")
        {

            string str_qry = "update tbl_student_online set is_approved='0',is_denied='0',is_waitinglist='1' where online_id=@online_id";
            IES_Generic_Function ies = new IES_Generic_Function();
            ies.ies_parameters.Clear();
            ies.ies_parameters.Add("@online_id", id);
            ies.Fn_ExecutiveSql(str_qry, 0, 1);
        }
        Bind_Details(Convert.ToString(hd_acyear.Value));
    }
}