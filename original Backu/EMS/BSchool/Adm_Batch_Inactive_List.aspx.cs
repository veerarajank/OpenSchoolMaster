using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Adm_Batch_Inactive_List : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["Usrid"] == null)
        {
            Response.Redirect("login.aspx");
        }
        if (!IsPostBack)
        {
          
            BindListView();


            // BindListView();
        }
    }
    protected void lnk_edit_Command(object sender, CommandEventArgs e)
    {
        string id = string.Empty;
        id = Convert.ToString(e.CommandArgument);
        if (e.CommandName == "edit")
        {
            if (Convert.ToBoolean(Session["Access_Edit"]) == true)
            {

                Response.Redirect("Adm_Courses_Batch.aspx?mode=deleted&id=" + id);
            }
        }
    }
    protected void BindListView()
    {
        
        IES_Generic_Function ies = new IES_Generic_Function();
        ies.ies_parameters.Clear();


        ies.ies_parameters.Add("@mode", "inactivebatchlist");
        DataTable dt_data = ies.Fn_ExecutiveSql_Datatable("UDP_Batches", 1, 1);
        
        lv_listview.DataSource = dt_data;
        lv_listview.DataBind();
    }
    protected void lnk_search_Command(object sender, CommandEventArgs e)
    {
        string id = string.Empty;
        id = Convert.ToString(e.CommandArgument);
        if (e.CommandName == "view")
        {
            if (Convert.ToBoolean(Session["Access_View"]) == true)
            {

                Response.Redirect("Adm_Courses_Batch.aspx?mode=view&id=" + id);
            }
        }
    }
}