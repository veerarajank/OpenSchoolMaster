using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Security.AccessControl;
using System.Security.Principal;
using System.IO;
using System.Configuration;
using System.Net;

public partial class GS247 : System.Web.UI.MasterPage
{   
    protected void Page_Load(object sender, EventArgs e)
    {
        Page.MaintainScrollPositionOnPostBack = true;
        
        if (!IsPostBack)
        {         

            p_login_person.InnerHtml = Convert.ToString(Session["Firstname"]);
            spn_login_person.InnerHtml = Convert.ToString(Session["Firstname"]);
            p_login_layout.InnerHtml = Convert.ToString(Session["Firstname"]) +
                 "<small>Member since " + Convert.ToString(Session["Createdon"]) + "</small>";

            Session["Access_Edit"] = true;
            Session["Access_View"] = true;
            Session["Access_Trash"] = true;
            Session["Usrid"] = "1";
        }
       
    }
   

}
