using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Adm_manageinvoice : System.Web.UI.Page
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
            ies.Fn_Executive_ListWithSelect_WithTitle(drp_course, "--Select course--", "select id, name from tbl_courses	order by name", "id", "Name");

            ies = new IES_Generic_Function();
            ies.Fn_Executive_ListWithSelect_WithTitle(drp_fee_id, "--Select Category--", "select id, name from tbl_fee_category	order by name", "id", "Name");
            BindDisplay();

        }
    }

    protected void BindDisplay()
    {
        string str_qry = @"select invoiceid,fee_id,student_id,schedule_id,
                dbo.UDF_DateFormat(schedule_date,1,0) as schedule_date,invoice_no, dbo.UDF_DateFormat(invoice_date,1,0) as invoice_date,invoice_amt,invoice_adjustment,invoice_paid_amt,invoice_due_amt,
                (select st.first_name from tbl_students st where st.id=c.student_id) as student_name,
                (select st.name from tbl_fee_category st where st.id=c.fee_id) as category_name,
                case when ISNULL(invoice_status,'0')='0' then 'Unpaid'  when ISNULL(invoice_status,'0')='1' then 'Paid' 
                when ISNULL(invoice_status,'0')='-1' then 'Cancelled'  end invoice_status
                from tbl_fee_invoiceno c";
        IES_Generic_Function ies = new IES_Generic_Function();
        ies.ies_parameters.Clear();
        DataTable dt_data = ies.Fn_ExecutiveSql_Datatable(str_qry, 0, 0);
        lv_category.DataSource = dt_data;
        lv_category.DataBind();

        int paidtotalinvoicecnt = dt_data.AsEnumerable()
               .Count(row => row.Field<string>("invoice_status") == "Paid");

        int cancelledinvoicecnt = dt_data.AsEnumerable()
               .Count(row => row.Field<string>("invoice_status") == "Cancelled");

        int unpaidinvoicecnt= dt_data.AsEnumerable()
               .Count(row => row.Field<string>("invoice_status") == "Unpaid");


        div_totalinvoicecnt.InnerHtml = Convert.ToString(dt_data.Rows.Count);
        div_paidnvoicecnt.InnerHtml = Convert.ToString(paidtotalinvoicecnt);
        div_unpaidnvoicecnt.InnerHtml = Convert.ToString(unpaidinvoicecnt);
        div_cancellednvoicecnt.InnerHtml = Convert.ToString(cancelledinvoicecnt);





      
    }


    protected void drp_course_SelectedIndexChanged(object sender, EventArgs e)
    {
        drp_batches.Items.Clear();
        IES_Generic_Function ies = new IES_Generic_Function();
        ies.Fn_Executive_ListWithSelect_WithTitle(drp_batches, "--Select Batch--", "select id, name from tbl_batches where course_id='" + Convert.ToString(drp_course.SelectedValue) + "' order by name", "id", "Name");

    }

    protected void lnk_search_Command(object sender, CommandEventArgs e)
    {
        string id = string.Empty;
        id = Convert.ToString(e.CommandArgument);
        if (e.CommandName == "view")
        {

            Response.Redirect("Adm_Fee_Manage_Invoice.aspx?id=" + id);
            
        }
    }
    protected void lv_category_PagePropertiesChanging(object sender, PagePropertiesChangingEventArgs e)
    {

    }
}