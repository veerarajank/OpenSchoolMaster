using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

public partial class Login : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Session.Abandon();
        }
    }
    protected void btn_submit_onserverclick(object sender, EventArgs e)
    {
        Session["Access_Edit"] = true;
        Session["Access_View"] = true;
        Session["Access_Trash"] = true;
        Session["Usrid"] = "1";


        //if (txt_username.Value.ToString() == "iikm" && txt_password.Value.ToString() == "iikm247$")               
        if(1==1)
        {
            //Add Activity Feed
            Save_Activity_Feed();
            Response.Redirect("Adm_academicYears.aspx");
        }
        else
        {
            spn_error.Visible = true;
        }       

    }
    protected void Save_Activity_Feed()
    {
        string initiator_id="1";
        string activity_type="24";
        string goal_id="1";
        string goal_name = "logged into the system";
        string user_role="Administrator";
        string system_ip = string.Empty;
        if (HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != null)
        {
            system_ip = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString();
        }
        else if (HttpContext.Current.Request.UserHostAddress.Length != 0)
        {
            system_ip = HttpContext.Current.Request.UserHostAddress;
        }
        string field_name="";
        string initial_field_value="";
        string new_field_value="";

        string str_qry = "insert into tbl_activity_feed(initiator_id,activity_type,goal_id,goal_name,user_role,system_ip,field_name,initial_field_value,new_field_value,activity_time)values(@initiator_id,@activity_type,@goal_id,@goal_name,@user_role,@system_ip,@field_name,@initial_field_value,@new_field_value,getdate())";
        IES_Generic_Function ies = new IES_Generic_Function();
        ies.ies_parameters.Clear();
        ies.ies_parameters.Add("@initiator_id",initiator_id);
        ies.ies_parameters.Add("@activity_type",activity_type);
        ies.ies_parameters.Add("@goal_id", goal_id);
        ies.ies_parameters.Add("@goal_name", goal_name);
        ies.ies_parameters.Add("@user_role", user_role);
        ies.ies_parameters.Add("@system_ip", system_ip);
        ies.ies_parameters.Add("@field_name", field_name);
        ies.ies_parameters.Add("@initial_field_value", initial_field_value);
        ies.ies_parameters.Add("@new_field_value", new_field_value);

        ies.Fn_ExecutiveSql(str_qry, 0, 1);
       
        
    }
   
}