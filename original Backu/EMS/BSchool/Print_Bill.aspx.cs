using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Print_Bill : System.Web.UI.Page
{

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["Usrid"] == null)
        {
            Response.Redirect("login.aspx");
        }
        if (!IsPostBack)
        {
            BindDisplay();
        }
    }
    protected void BindDisplay()
    {
        string id = Convert.ToString(Request.QueryString["id"]);
        string str_qry = @"select invoiceid,fee_id,student_id,schedule_id,
                dbo.UDF_DateFormat(schedule_date,1,0) as schedule_date,invoice_no, dbo.UDF_DateFormat(invoice_date,1,0) as invoice_date,invoice_amt,invoice_adjustment,
                invoice_paid_amt,invoice_due_amt,
                (select st.first_name from tbl_students st where st.id=c.student_id) as student_name,
                (select st.name from tbl_fee_category st where st.id=c.fee_id) as category_name,
                case when ISNULL(invoice_status,'0')='0' then 'Unpaid'  when ISNULL(invoice_status,'0')='1' then 'Paid' 
                when ISNULL(invoice_status,'0')='-1' then 'Cancelled'  end invoice_status
                from tbl_fee_invoiceno c where invoiceid=@invoiceid";
        IES_Generic_Function ies = new IES_Generic_Function();
        ies.ies_parameters.Clear();
        ies.ies_parameters.Add("@invoiceid", id);
        DataTable dt_data = ies.Fn_ExecutiveSql_Datatable(str_qry, 0, 1);
        if (dt_data != null)
        {
            if (dt_data.Rows.Count > 0)
            {
                head_invoiceno.InnerHtml = "<b>Invoice - " + Convert.ToString(dt_data.Rows[0]["invoice_no"]) + "</b>";
                lbl_invoiceid.InnerHtml = Convert.ToString(dt_data.Rows[0]["invoice_no"]);
                lbl_student_href.InnerHtml = Convert.ToString(dt_data.Rows[0]["student_name"]);
                lbl_feecategory.InnerHtml = Convert.ToString(dt_data.Rows[0]["category_name"]);
                lbl_invoicedate.InnerHtml = Convert.ToString(dt_data.Rows[0]["invoice_date"]);
                lbl_invoiceamt.InnerHtml = Convert.ToString(dt_data.Rows[0]["invoice_amt"]);
                lbl_adjustment.InnerHtml = "-";
                lbl_paymentdetails.InnerHtml = Convert.ToString(dt_data.Rows[0]["invoice_paid_amt"]);
                lbl_amtpayable.InnerHtml = Convert.ToString(dt_data.Rows[0]["invoice_due_amt"]);
                lbl_duedate.InnerHtml = Convert.ToString(dt_data.Rows[0]["schedule_date"]);
                lbl_lastpaymentdate.InnerHtml = "-";
                lbl_paymentstatus.InnerHtml = Convert.ToString(dt_data.Rows[0]["invoice_status"]);
            }
        }


        str_qry = @"select *,convert(float,amount) as amt,convert(float,grossamt) as gsamt,convert(float,discountedamt) as dsamt,convert(float,taxamt) as tsamt from tbl_fee_invoicedetails where invoiceid=@invoiceid";
        ies = new IES_Generic_Function();
        ies.ies_parameters.Clear();
        ies.ies_parameters.Add("@invoiceid", id);
        dt_data = ies.Fn_ExecutiveSql_Datatable(str_qry, 0, 1);
        lv_category.DataSource = dt_data;
        lv_category.DataBind();
        if (dt_data != null)
        {
            var grossamt = dt_data.AsEnumerable().Sum(x => x.Field<double>("gsamt"));
            var subtotal = dt_data.AsEnumerable().Sum(x => x.Field<double>("amt"));
            td_subtotal.InnerHtml = Convert.ToString(subtotal);
            td_grossamt.InnerHtml = Convert.ToString(grossamt);

            var discountedamt = dt_data.AsEnumerable().Sum(x => x.Field<double>("dsamt"));
            var taxamt = dt_data.AsEnumerable().Sum(x => x.Field<double>("tsamt"));
            td_discountamt.InnerHtml = "(-)" + Convert.ToString(discountedamt);
            td_taxamt.InnerHtml = "(+)" + Convert.ToString(taxamt);
        }
    }
}