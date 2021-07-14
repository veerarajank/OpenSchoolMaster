using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Adm_Edit_Teacher_Contact_Details : System.Web.UI.Page
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
            ies = new IES_Generic_Function();
            ies.Fn_Executive_ListWithSelect_WithTitle(drp_contry, "--Select Country--", "select id, name from tbl_countries	order by name", "id", "Name");           

            BindDisplay();          


        }
    }
    protected void BindDisplay()
    {
        string id = Convert.ToString(Request.QueryString["id"]);
        IES_Generic_Function ies = new IES_Generic_Function();
        ies.ies_parameters.Clear();
        ies.ies_parameters.Add("@id", Convert.ToString(id));
        ies.ies_parameters.Add("@mode", "search");
        DataTable dt_data = ies.Fn_ExecutiveSql_Datatable("UDP_Create_Teacher", 1, 1);
        if (dt_data != null && dt_data.Rows.Count > 0)
        {
          
         

      

            tbx_add1.Text = Convert.ToString(dt_data.Rows[0]["address_line1"]);
            tbx_add2.Text = Convert.ToString(dt_data.Rows[0]["address_line2"]);
            tbx_city.Text = Convert.ToString(dt_data.Rows[0]["city"]);
            tbx_state.Text = Convert.ToString(dt_data.Rows[0]["state"]);
            tbx_pincode.Text = Convert.ToString(dt_data.Rows[0]["pin_code"]);
            drp_contry.SelectedValue = Convert.ToString(dt_data.Rows[0]["country_id"]);
            tbx_phone1.Text = Convert.ToString(dt_data.Rows[0]["phone1"]);
            tbx_phone2.Text = Convert.ToString(dt_data.Rows[0]["phone2"]);
            tbx_emailid.Text = Convert.ToString(dt_data.Rows[0]["email"]);




        }
    }
    protected void btn_save_Click(object sender, EventArgs e)
    {
        string id = Convert.ToString(Request.QueryString["id"]);
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conn"].ConnectionString);

        if (con.State == ConnectionState.Closed) con.Open();
        SqlCommand cmd = new SqlCommand("UDP_Create_Teacher", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.Clear();

        string address_line1 = Convert.ToString(tbx_add1.Text);
        string address_line2 = Convert.ToString(tbx_add2.Text);
        string city = Convert.ToString(tbx_city.Text);
        string state = Convert.ToString(tbx_state.Text);
        string pin_code = Convert.ToString(tbx_pincode.Text);
        string country_id = Convert.ToString(drp_contry.SelectedValue);
        string phone1 = Convert.ToString(tbx_phone1.Text);
        string phone2 = Convert.ToString(tbx_phone2.Text);
        string email = Convert.ToString(tbx_emailid.Text); 
       


        cmd.Parameters.AddWithValue("@id", Convert.ToString(id));
        cmd.Parameters.AddWithValue("@address_line1", Convert.ToString(address_line1));
        cmd.Parameters.AddWithValue("@address_line2", Convert.ToString(address_line2).Trim());
        cmd.Parameters.AddWithValue("@city", Convert.ToString(city).Trim());
        cmd.Parameters.AddWithValue("@state", Convert.ToString(state).Trim());
        cmd.Parameters.AddWithValue("@pin_code", Convert.ToString(pin_code).Trim());

        cmd.Parameters.AddWithValue("@country_id", Convert.ToString(country_id));
        cmd.Parameters.AddWithValue("@phone1", Convert.ToString(phone1).Trim());
        cmd.Parameters.AddWithValue("@phone2", Convert.ToString(phone2).Trim());
        cmd.Parameters.AddWithValue("@email", Convert.ToString(email).Trim());

        cmd.Parameters.AddWithValue("@mode", "update");
        DataTable dt_result = new DataTable();
        SqlDataAdapter da = new SqlDataAdapter(cmd);
        da.Fill(dt_result);
        da.SelectCommand.Connection.Close();
        cmd.Connection.Close();
        if (con != null && con.State == ConnectionState.Open) con.Close();
        if (con != null) con = null;
        if (dt_result != null && dt_result.Rows.Count > 0)
        {
            if (Convert.ToString(dt_result.Rows[0]["Error"]) == "0")
            {
                Response.Redirect("Adm_Edit_Teacher_Documents.aspx?id=" + id);               
            }
        }
    }
    protected void teacher_contact_ServerClick(object sender, EventArgs e)
    {
        string id = Convert.ToString(Request.QueryString["id"]);
        Response.Redirect("Adm_Edit_Teacher_Contact_Details.aspx?id=" + id);
    }
    protected void teacher_document_ServerClick(object sender, EventArgs e)
    {

        string id = Convert.ToString(Request.QueryString["id"]);
        Response.Redirect("Adm_Edit_Teacher_Documents.aspx?id=" + id);
    }
    protected void teacher_info_ServerClick(object sender, EventArgs e)
    {
        string id = Convert.ToString(Request.QueryString["id"]);
        Response.Redirect("Adm_Edit_Teacher.aspx?id=" + id);
    }
}