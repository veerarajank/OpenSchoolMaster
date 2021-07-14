using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Adm_Student_List : System.Web.UI.Page
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
         }
    }
    protected void btn_new_ServerClick(object sender, EventArgs e)
    {
        Response.Redirect("Adm_StudentCreation.aspx");
    }
    protected void BindListView()
    {
        IES_Generic_Function ies = new IES_Generic_Function();
        ies.ies_parameters.Clear();

        ies.ies_parameters.Add("@mode", "search");
        DataTable dt_data = ies.Fn_ExecutiveSql_Datatable("UDP_Create_Student", 1, 1);
        if (dt_data != null)
        {
            lbl_cnt.InnerHtml = "&nbsp;Displaying " + Convert.ToString(dt_data.Rows.Count) + " result(s).";
            lv_studentdetails.DataSource = dt_data;
            lv_studentdetails.DataBind();
        }
    }
    protected void lnk_search_Command(object sender, CommandEventArgs e)
    {
        string id = string.Empty;
        id = Convert.ToString(e.CommandArgument);
        if (e.CommandName == "view")
        {
            if (Convert.ToBoolean(Session["Access_View"]) == true)
            {

                Response.Redirect("Adm_StudentInfo.aspx?mode=view&id=" + id);
            }
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

                Response.Redirect("Adm_StudentCreation_Edit.aspx?mode=edit&id=" + id);
            }
        }
    }
    protected void lv_studentdetails_PagePropertiesChanging(object sender, PagePropertiesChangingEventArgs e)
    {
        (lv_studentdetails.FindControl("DataPager1") as DataPager).SetPageProperties(e.StartRowIndex, e.MaximumRows, false);
        this.BindListView();
    }

    
}