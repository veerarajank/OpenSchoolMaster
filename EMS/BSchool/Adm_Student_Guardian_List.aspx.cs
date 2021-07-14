using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Adm_Student_Guardian_List : System.Web.UI.Page
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


            search_section.Visible = false;          
            update_section.Visible = false;

            string mode = Convert.ToString(Request.QueryString["mode"]);
            if (mode != null && mode == "search")
            {
                search_section.Visible = true;
                Bind_Gaudian_Details();
            }          
            else if (mode != null && mode == "edit")
            {
                update_section.Visible = true;
                BindDisplay();
            }           
            else if (mode == null)
            {
                search_section.Visible = true;
                Bind_Gaudian_Details();
            }

        }
    }
    protected void btn_search_Click(object sender, EventArgs e)
    {
        Bind_Gaudian_Details();
    }
    protected void Bind_Gaudian_Details()
    {
        string student_id = Convert.ToString(Request.QueryString["id"]);
        IES_Generic_Function ies = new IES_Generic_Function();
        ies.ies_parameters.Clear();
        if (tbx_studentname.Text.Trim() != "") ies.ies_parameters.Add("@student_name", "%" + tbx_studentname.Text.Trim() + "%");
        ies.ies_parameters.Add("@mode", "searchlist");
        DataTable dt_data = ies.Fn_ExecutiveSql_Datatable("UDP_Guardian", 1, 1);
        if (dt_data != null)
        {
            lbl_cnt.InnerHtml = "&nbsp;Displaying " + Convert.ToString(dt_data.Rows.Count) + " result(s).";
            lv_gaurdiansdetails.DataSource = dt_data;
            lv_gaurdiansdetails.DataBind();
        }
    }
    protected void btn_makeprimary_ServerClick(object sender, EventArgs e)
    {
        string student_id = Convert.ToString(Request.QueryString["id"]);
        IES_Generic_Function ies = new IES_Generic_Function();
        ies.ies_parameters.Clear();        
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
        
        ies.ies_parameters.Add("@id", Convert.ToString(hd_id.Value));
        ies.ies_parameters.Add("@mode", "makeemergency");
        ies.Fn_ExecutiveSql("UDP_Guardian", 1, 1);
        Bind_Gaudian_Details();
    }
    protected void btn_del_ServerClick(object sender, EventArgs e)
    {
        
        IES_Generic_Function ies = new IES_Generic_Function();
        ies.ies_parameters.Clear();
        ies.ies_parameters.Add("@id", Convert.ToString(hd_id.Value));
        ies.ies_parameters.Add("@mode", "deletecontact");
        ies.Fn_ExecutiveSql("UDP_Guardian", 1, 1);
        Bind_Gaudian_Details();
    }
   
    protected void btn_update_ServerClick(object sender, EventArgs e)
    {
        string id = Convert.ToString(Request.QueryString["id"]);

        IES_Generic_Function ies = new IES_Generic_Function();
        ies.ies_parameters.Clear();

        string first_name, last_name, relation, email, phone1, phone2, mobile_phone, address_line1, address_line2, city, state, country_id, dob,
            occupation, income, education, created_by, is_deleted, is_active, is_parent_user;

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
        if (created_by.Trim() != "") ies.ies_parameters.Add("@updated_by", created_by.Trim());
        if (is_deleted.Trim() != "") ies.ies_parameters.Add("@is_deleted", is_deleted.Trim());
        if (is_active.Trim() != "") ies.ies_parameters.Add("@is_active", is_active.Trim());
      

        ies.ies_parameters.Add("@id", Convert.ToString(id));

        ies.ies_parameters.Add("@mode", "update");
        DataTable dt_result = ies.Fn_ExecutiveSql_Datatable("UDP_Guardian", 1, 1);
        if (dt_result != null && dt_result.Rows.Count > 0)
        {

            if (Convert.ToString(dt_result.Rows[0]["Error"]) == "0")
            {
                tbx_Guardians_first_name.Text = "";
                tbx_Guardians_last_name.Text = "";
                drp_Guardians_relation.SelectedIndex = -1;
                tbx_Guardians_relation_other.Text = "";
                tbx_Guardians_email.Text = "";
                tbx_Guardians_office_phone1.Text = "";
                tbx_Guardians_office_phone2.Text = "";
                tbx_Guardians_office_address_line1.Text = "";
                tbx_Guardians_office_address_line2.Text = "";
                tbx_Guardians_city.Text = "";
                tbx_Guardians_state.Text = "";
                drp_Guardians_country.SelectedIndex = -1;

                tbx_Guardians_dob.Text = "";
                tbx_Guardians_occupation.Text = "";
                tbx_Guardians_income.Text = "";
                tbx_Guardians_education.Text = "";

                Response.Redirect("Adm_Student_Guardian_List.aspx");
            }
        }
    }
    protected void drp_Guardians_relation_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (drp_Guardians_relation.Text == "Others") relation_other.Visible = true;
        else relation_other.Visible = false;
    }
    protected void lnk_edit_Command(object sender, CommandEventArgs e)
    {
        string id = string.Empty;
        id = Convert.ToString(e.CommandArgument);
        if (e.CommandName == "edit")
        {
            if (Convert.ToBoolean(Session["Access_Edit"]) == true)
            {
                Response.Redirect("Adm_Student_Guardian_List.aspx?mode=edit&id=" + id);
            }
        }
    }
    protected void BindDisplay()
    {
        string id = Convert.ToString(Request.QueryString["id"]);
        IES_Generic_Function ies = new IES_Generic_Function();
        ies.ies_parameters.Clear();
        ies.ies_parameters.Add("@id", Convert.ToString(id));
        ies.ies_parameters.Add("@mode", "search");
        DataTable dt_data = ies.Fn_ExecutiveSql_Datatable("UDP_Guardian", 1, 1);
        if (dt_data != null && dt_data.Rows.Count > 0)
        {

            //Edit Section
            tbx_Guardians_first_name.Text = Convert.ToString(dt_data.Rows[0]["first_name"]);
            tbx_Guardians_last_name.Text = Convert.ToString(dt_data.Rows[0]["last_name"]);
            ies = new IES_Generic_Function();
            drp_Guardians_relation.SelectedValue = ies.Fn_Dropdown_FindByValue(drp_Guardians_relation, "0", Convert.ToString(dt_data.Rows[0]["relation"]));
            if (drp_Guardians_relation.SelectedValue == "0")
            {
                tbx_Guardians_relation_other.Text = Convert.ToString(dt_data.Rows[0]["relation"]);
            }


            tbx_Guardians_dob.Text = Convert.ToString(dt_data.Rows[0]["dob"]);
            tbx_Guardians_education.Text = Convert.ToString(dt_data.Rows[0]["education"]);
            tbx_Guardians_occupation.Text = Convert.ToString(dt_data.Rows[0]["occupation"]);
            tbx_Guardians_income.Text = Convert.ToString(dt_data.Rows[0]["income"]);
            tbx_Guardians_email.Text = Convert.ToString(dt_data.Rows[0]["email"]);
            tbx_Guardians_mobile_phone.Text = Convert.ToString(dt_data.Rows[0]["mobile_phone"]);
            tbx_Guardians_office_phone1.Text = Convert.ToString(dt_data.Rows[0]["phone1"]);
            tbx_Guardians_office_phone2.Text = Convert.ToString(dt_data.Rows[0]["phone2"]);
            tbx_Guardians_office_address_line1.Text = Convert.ToString(dt_data.Rows[0]["address_line1"]);
            tbx_Guardians_office_address_line2.Text = Convert.ToString(dt_data.Rows[0]["address_line2"]);
            tbx_Guardians_city.Text = Convert.ToString(dt_data.Rows[0]["city"]);
            tbx_Guardians_state.Text = Convert.ToString(dt_data.Rows[0]["state"]);
            drp_Guardians_country.SelectedValue = Convert.ToString(dt_data.Rows[0]["country_id"]);


        }
    }
}