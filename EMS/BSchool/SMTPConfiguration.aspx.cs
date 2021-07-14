using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class SMTPConfiguration : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["Usrid"] == null)
        {
            Response.Redirect("login.aspx");
        }
        if (!IsPostBack)
        {
           

            search_section.Visible = false;
         
            add_section.Visible = false;
         


            string mode = Convert.ToString(Request.QueryString["mode"]);
            if (mode != null && mode == "search")
            {
                search_section.Visible = true;
              
            }
            else if (mode != null && mode == "add")
            {
                add_section.Visible = true;
            }
           
            else if (mode == null)
            {
                search_section.Visible = true;
                
            }




        }
    }
    protected void btn_save_Click(object sender, EventArgs e)
    {

    }
}