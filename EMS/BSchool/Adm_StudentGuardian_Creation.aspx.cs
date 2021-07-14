using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Adm_StudentGuardian_Creation : System.Web.UI.Page
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
            ies.Fn_Executive_ListWithSelect_WithTitle(drp_Guardians_country, "--Select Country--", "select id, name from tbl_countries	order by name", "id", "Name");

            Bind_Gaudian_Details();



          
        }
    }
    protected void Bind_Gaudian_Details()
    {
        string student_id = Convert.ToString(Request.QueryString["id"]);
        IES_Generic_Function ies = new IES_Generic_Function();
        ies.ies_parameters.Clear();
        ies.ies_parameters.Add("@student_id", student_id);
        ies.ies_parameters.Add("@mode", "studentparent");
        DataTable dt_data = ies.Fn_ExecutiveSql_Datatable("UDP_Guardian", 1, 1);
        if (dt_data != null)
        {

            lv_gaurdiansdetails.DataSource = dt_data;
            lv_gaurdiansdetails.DataBind();
        }
    }

    protected void Bind_Sibling()
    {
        string student_id = Convert.ToString(Request.QueryString["id"]);
        IES_Generic_Function ies = new IES_Generic_Function();
        ies.ies_parameters.Clear();

        if(tbx_sibilings_studentname.Text !="") ies.ies_parameters.Add("@student_name", "%" + Convert.ToString(tbx_sibilings_studentname.Text) + "%");
        if (tbx_sibilings_parent.Text != "") ies.ies_parameters.Add("@first_name", "%" + Convert.ToString(tbx_sibilings_parent.Text) + "%");
        if (tbx_sibilings_parent_email.Text != "") ies.ies_parameters.Add("@email", "%" + Convert.ToString(tbx_sibilings_parent_email.Text) + "%");

        
        ies.ies_parameters.Add("@mode", "bindsibiling");
        DataTable dt_data = ies.Fn_ExecutiveSql_Datatable("UDP_Guardian", 1, 1);
        if (dt_data != null)
        {


            tbx_sibilings_studentname.Text = "";
            tbx_sibilings_parent.Text = "";
            tbx_sibilings_parent_email.Text = "";

            lv_existingguardian.DataSource = dt_data;
            lv_existingguardian.DataBind();
        }
    }
    protected void btn_sibiling_search_Click(object sender, EventArgs e)
    {
        Bind_Sibling();

    }
    protected void lv_existingguardian_PagePropertiesChanging(object sender, PagePropertiesChangingEventArgs e)
    {

    }
    protected void btn_savvenext_Click(object sender, EventArgs e)
    {
        string id = Convert.ToString(Request.QueryString["id"]);
        Response.Redirect("Adm_StudentGuardian_PreviousDetails.aspx?mode=edit&id=" + id);
    }
    protected void drp_Guardians_relation_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (drp_Guardians_relation.Text == "Others") relation_other.Visible = true;
        else relation_other.Visible = false;
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
    protected void emergency_contact_ServerClick(object sender, EventArgs e)
    {

    }
    protected void primary_contact_ServerClick(object sender, EventArgs e)
    {

    }
    protected void edit_guardian_ServerClick(object sender, EventArgs e)
    {

    }
    
    protected void btn_saveguardian_Click(object sender, EventArgs e)
    {
        Save_Guardian();
        Bind_Gaudian_Details();
    }

    protected void Save_Guardian()
    {
        string student_id = Convert.ToString(Request.QueryString["id"]);

        IES_Generic_Function ies = new IES_Generic_Function();
        ies.ies_parameters.Clear();

        string first_name,last_name,relation,email,phone1,phone2,mobile_phone,address_line1,address_line2,city,state,country_id,dob,
            occupation,income,education,created_by,is_deleted,is_active,is_parent_user;

        first_name = Convert.ToString(tbx_Guardians_first_name.Text);
        last_name = Convert.ToString(tbx_Guardians_last_name.Text);
        relation = Convert.ToString(drp_Guardians_relation.SelectedItem.Value);
        if (relation == "Others") relation = Convert.ToString(tbx_Guardians_relation_other.Text);
        email = Convert.ToString(tbx_Guardians_email.Text);
        phone1 = Convert.ToString(tbx_Guardians_office_phone1.Text);
        phone2 = Convert.ToString(tbx_Guardians_office_phone2.Text);
        address_line1 = Convert.ToString(tbx_Guardians_office_address_line1.Text);
        address_line2 = Convert.ToString(tbx_Guardians_office_address_line2.Text);
        city = Convert.ToString(tbx_Guardians_city.Text);
        state = Convert.ToString(tbx_Guardians_state.Text);
        country_id = Convert.ToString(drp_Guardians_country.SelectedItem.Value);
        dob = "";
        if (tbx_Guardians_dob.Text.Trim() != "")
        {
            DateTime dtdate_of_birth = DateTime.ParseExact(tbx_Guardians_dob.Text.Trim(), "dd-MM-yyyy", null);
            dob = dtdate_of_birth.ToString("yyyy-MM-dd");
        }
        occupation = Convert.ToString(tbx_Guardians_occupation.Text);
        income = Convert.ToString(tbx_Guardians_income.Text);
        education = Convert.ToString(tbx_Guardians_education.Text);
        created_by = Convert.ToString(Session["Usrid"]);
        is_deleted = "0";
        is_active = "1";

        is_parent_user="0";
        if (chk_user_create.Checked == true) is_parent_user = "1";

        mobile_phone = Convert.ToString(tbx_Guardians_mobile_phone.Text);


        if (first_name.Trim() != "") ies.ies_parameters.Add("@first_name", first_name.Trim());
        if (last_name.Trim() != "") ies.ies_parameters.Add("@last_name", last_name.Trim());
        if (relation.Trim() != "") ies.ies_parameters.Add("@relation", relation.Trim());
        if (email.Trim() != "") ies.ies_parameters.Add("@email", email.Trim());
        if (phone1.Trim() != "") ies.ies_parameters.Add("@phone1", phone1.Trim());
        if (phone2.Trim() != "") ies.ies_parameters.Add("@phone2", phone2.Trim());
        if (mobile_phone.Trim() != "") ies.ies_parameters.Add("@mobile_phone", mobile_phone.Trim());
        if (address_line1.Trim() != "") ies.ies_parameters.Add("@address_line1", address_line1.Trim());
        if (address_line2.Trim() != "") ies.ies_parameters.Add("@address_line2", address_line2.Trim());
        if (city.Trim() != "") ies.ies_parameters.Add("@city", city.Trim());
        if (state.Trim() != "") ies.ies_parameters.Add("@state", state.Trim());
        if (country_id.Trim() != "0") ies.ies_parameters.Add("@country_id", country_id.Trim());
        if (dob.Trim() != "") ies.ies_parameters.Add("@dob", dob.Trim());
        if (occupation.Trim() != "") ies.ies_parameters.Add("@occupation", occupation.Trim());
        if (income.Trim() != "") ies.ies_parameters.Add("@income", income.Trim());
        if (education.Trim() != "") ies.ies_parameters.Add("@education", education.Trim());
        if (created_by.Trim() != "") ies.ies_parameters.Add("@created_by", created_by.Trim());     
        if (is_deleted.Trim() != "") ies.ies_parameters.Add("@is_deleted", is_deleted.Trim());
        if (is_active.Trim() != "") ies.ies_parameters.Add("@is_active", is_active.Trim());
        if (is_parent_user.Trim() != "") ies.ies_parameters.Add("@is_parent_user", is_parent_user.Trim());
        
        ies.ies_parameters.Add("@student_id", Convert.ToString(student_id));

        ies.ies_parameters.Add("@mode", "add");
        DataTable dt_result = ies.Fn_ExecutiveSql_Datatable("UDP_Guardian", 1, 1);
        if (dt_result != null && dt_result.Rows.Count > 0)
        {
           
            if (Convert.ToString(dt_result.Rows[0]["Error"]) == "0")
            {
                tbx_Guardians_first_name.Text="";
                tbx_Guardians_last_name.Text="";
                drp_Guardians_relation.SelectedIndex=-1;
                tbx_Guardians_relation_other.Text="";
                tbx_Guardians_email.Text="";
                tbx_Guardians_office_phone1.Text="";
                tbx_Guardians_office_phone2.Text="";
                tbx_Guardians_office_address_line1.Text="";
                tbx_Guardians_office_address_line2.Text="";
                tbx_Guardians_city.Text="";
                tbx_Guardians_state.Text="";
                drp_Guardians_country.SelectedIndex = -1;

                tbx_Guardians_dob.Text="";
                tbx_Guardians_occupation.Text="";
                tbx_Guardians_income.Text="";
                tbx_Guardians_education.Text = "";
            }
        }


      

      
       
    }
    protected void btn_makeprimary_ServerClick(object sender, EventArgs e)
    {
        string student_id = Convert.ToString(Request.QueryString["id"]);
        IES_Generic_Function ies = new IES_Generic_Function();
        ies.ies_parameters.Clear();
        ies.ies_parameters.Add("@student_id", Convert.ToString(student_id));
        ies.ies_parameters.Add("@id", Convert.ToString(hd_id.Value));
        ies.ies_parameters.Add("@mode", "makeprimary");
        ies.Fn_ExecutiveSql("UDP_Guardian", 1, 1);
        Bind_Gaudian_Details();
    }
    protected void btn_emergency_ServerClick(object sender, EventArgs e)
    {
        string student_id = Convert.ToString(Request.QueryString["id"]);
        IES_Generic_Function ies = new IES_Generic_Function();
        ies.ies_parameters.Clear();
        ies.ies_parameters.Add("@student_id", Convert.ToString(student_id));
        ies.ies_parameters.Add("@id", Convert.ToString(hd_id.Value));
        ies.ies_parameters.Add("@mode", "makeemergency");
        ies.Fn_ExecutiveSql("UDP_Guardian", 1, 1);
        Bind_Gaudian_Details();
    }
    protected void btn_del_ServerClick(object sender, EventArgs e)
    {
        string student_id = Convert.ToString(Request.QueryString["id"]);
        IES_Generic_Function ies = new IES_Generic_Function();
        ies.ies_parameters.Clear();       
        ies.ies_parameters.Add("@id", Convert.ToString(hd_id.Value));
        ies.ies_parameters.Add("@mode", "deletecontact");
        ies.Fn_ExecutiveSql("UDP_Guardian", 1, 1);
        Bind_Gaudian_Details();
    }
    
    protected void btn_add_link_ServerClick(object sender, EventArgs e)
    {
        string student_id = Convert.ToString(Request.QueryString["id"]);
        IES_Generic_Function ies = new IES_Generic_Function();
        ies.ies_parameters.Clear();
        ies.ies_parameters.Add("@id", Convert.ToString(hd_id.Value));
        ies.ies_parameters.Add("@student_id", Convert.ToString(student_id));
        ies.ies_parameters.Add("@mode", "linkguardian");
        ies.Fn_ExecutiveSql("UDP_Guardian", 1, 1);
        Bind_Gaudian_Details();

        lv_existingguardian.DataSource = null;
        lv_existingguardian.DataBind();
    }
    protected void chk_guardian2_ServerChange(object sender, EventArgs e)
    {
        if (chk_guardian2.Checked==true)
        {
            lv_existingguardian.DataSource = null;
            lv_existingguardian.DataBind();
        }
    }
    protected void chk_guardian2_CheckedChanged(object sender, EventArgs e)
    {
        if (chk_guardian2.Checked == true)
        {
            lv_existingguardian.DataSource = null;
            lv_existingguardian.DataBind();
        }
    }
}