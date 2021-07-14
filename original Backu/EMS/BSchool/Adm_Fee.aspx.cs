using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Adm_Fee : System.Web.UI.Page
{


    [Serializable]
    public class Student_Details
    {
        public int fee_id { get; set; }
        public string appid { get; set; }
        public string admission_no { get; set; }
        public string student_id { get; set; }
        public string course_id { get; set; }
        public string batch_id { get; set; }
        public string semester_id { get; set; }
        public string category_id { get; set; }

        public string item_id { get; set; }
        public string item_name { get; set; }
        public string item_description { get; set; }
        public string amount { get; set; }
        public string tax_id { get; set; }
        public string tax_value { get; set; }
        public string discount_value { get; set; }
        public string discount_type { get; set; }

        public string actualamt { get; set; }
        public string discountedamt { get; set; }
        public string netamt { get; set; }
        public string taxamt { get; set; }
        public string grossamt { get; set; }






    }

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

        IES_Generic_Function ies = new IES_Generic_Function();
        ies.ies_parameters.Clear();
        ies.ies_parameters.Add("@mode", "search");
        DataTable dt_data = ies.Fn_ExecutiveSql_Datatable("UDP_FeeCategory", 1, 1);
        lv_category.DataSource = dt_data;
        lv_category.DataBind();

        if (dt_data != null)
        {
            div_totalcategory.InnerHtml = Convert.ToString(dt_data.Rows.Count);
        }
        else
        {
            div_totalcategory.InnerHtml = Convert.ToString(0);
        }

        int invoice_cnt =
            dt_data.AsEnumerable()
               .Count(row => row.Field<int>("is_viewinvoice") != 0);

        div_totalinvoice.InnerHtml = Convert.ToString(invoice_cnt);
            
    }
    protected void lnk_edit_Command(object sender, CommandEventArgs e)
    {
        string id = string.Empty;
        id = Convert.ToString(e.CommandArgument);
        if (e.CommandName == "edit")
        {
            if (Convert.ToBoolean(Session["Access_Edit"]) == true)
            {
                Response.Redirect("Adm_Fees_Subscribe.aspx");
            }
        }
    }
    protected void lnk_search_Command(object sender, CommandEventArgs e)
    {
        string id = string.Empty;
        id = Convert.ToString(e.CommandArgument);
        if (e.CommandName == "createsubscripe")
        {
            if (Convert.ToBoolean(Session["Access_View"]) == true)
            {
                Response.Redirect("Adm_Fees_Subscribe.aspx?id=" + id);
            }
        }
        if (e.CommandName == "viewinvoice")
        {
            if (Convert.ToBoolean(Session["Access_View"]) == true)
            {
                Response.Redirect("Adm_manageinvoice.aspx?id=" + id);
            }
        }
    }
    protected void btn_del_ServerClick(object sender, EventArgs e)
    {

        if (Convert.ToBoolean(Session["Access_Trash"]) == true)
        {

        }
    }
    protected void lv_category_PagePropertiesChanging(object sender, PagePropertiesChangingEventArgs e)
    {

    }
    protected void btn_generateinvoice_ServerClick(object sender, EventArgs e)
    {
        string id = Convert.ToString(hd_id.Value);
        Generate_Invoice_Process();
        IES_Generic_Function ies = new IES_Generic_Function();
        ies.ies_parameters.Clear();
        ies.ies_parameters.Add("@mode", "updateinvoice");
        ies.ies_parameters.Add("@id", id);
        ies.Fn_ExecutiveSql("UDP_FeeCategory", 1, 1);
        Response.Redirect("Adm_Fee.aspx");
    }



    protected void Generate_Invoice_Process()
    {
        string id = Convert.ToString(hd_id.Value);
        IES_Generic_Function ies = new IES_Generic_Function();
        ies.ies_parameters.Clear();
        ies.ies_parameters.Add("@mode", "search");
        ies.ies_parameters.Add("@id", id);
        DataTable dt_data = ies.Fn_ExecutiveSql_Datatable("UDP_FeeCategory", 1, 1);
        if (dt_data != null)
        {
            if (dt_data.Rows.Count > 0)
            {
                string is_divide = Convert.ToString(dt_data.Rows[0]["is_divide"]);
                decimal total_netamt = Convert.ToDecimal(Convert.ToString(dt_data.Rows[0]["amount"]).Trim());

                List<Student_Details> lst_student = new List<Student_Details>();

                ies = new IES_Generic_Function();
                ies.ies_parameters.Clear();
                ies.ies_parameters.Add("@fee_id", id);
                DataTable dt_particulars = ies.Fn_ExecutiveSql_Datatable("select * from tbl_fee_applicableto where fee_id=@fee_id", 0, 1);
                if (dt_particulars != null)
                {

                    for (int it = 0; it <= dt_particulars.Rows.Count - 1; it++)
                    {
                        decimal total_grossamt = 0;
                        decimal total_deduction = 0;
                        decimal total_taxamt = 0;

                        string course_id = Convert.ToString(dt_particulars.Rows[it]["course_id"]);
                        string batch_id = Convert.ToString(dt_particulars.Rows[it]["batch_id"]);
                        string category_id = Convert.ToString(dt_particulars.Rows[it]["category_id"]);
                        string[] application_no = Convert.ToString(dt_particulars.Rows[it]["application_no"]).Split(',');
                        string appid = Convert.ToString(dt_particulars.Rows[it]["appid"]);


                        decimal tax_value = Convert.ToDecimal(dt_particulars.Rows[it]["tax_value"]);
                        string tax_id = Convert.ToString(dt_particulars.Rows[it]["tax_id"]);
                        decimal discount_value = Convert.ToDecimal(dt_particulars.Rows[it]["discount_value"]);
                        string discount_type = Convert.ToString(dt_particulars.Rows[it]["discount_type"]);
                        string item_id = Convert.ToString(dt_particulars.Rows[it]["item_id"]);
                        string item_name = Convert.ToString(dt_particulars.Rows[it]["item_name"]);
                        string item_description = Convert.ToString(dt_particulars.Rows[it]["item_description"]);
                        decimal item_totalamt = Convert.ToDecimal(dt_data.Rows[0]["amount"]);
                        decimal actual_itemamt = Convert.ToDecimal(dt_data.Rows[0]["amount"]);


                        if (discount_type == "%")
                        {
                            if (discount_value != 0)
                            {
                                item_totalamt = item_totalamt - (item_totalamt * (discount_value / 100));
                                total_deduction = (actual_itemamt * (discount_value / 100));
                            }
                        }
                        else
                        {
                            if (discount_value != 0)
                            {
                                item_totalamt = item_totalamt - discount_value;
                                total_deduction = discount_value;
                            }
                        }


                        if (tax_value == 0)
                        {
                            total_grossamt = item_totalamt;
                        }
                        else
                        {
                            total_grossamt = item_totalamt + (item_totalamt * (tax_value / 100));
                            total_taxamt = (item_totalamt * (tax_value / 100));
                        }


                        string admin_no = "";
                        foreach (string appno in application_no)
                        {
                            if (appno.Trim() != "")
                            {
                                if (admin_no == "") admin_no = "'" + appno + "'";
                                else admin_no = ",'" + appno + "'";
                            }
                        }

                        string str_qry = @"select id,admission_no,course_id,batch_id,semester_id,student_category_id 
                        from tbl_students where (course_id='" + course_id + "' and batch_id='" + batch_id + "' and student_category_id='" + category_id + "')";

                        if (admin_no != "")
                        {
                            str_qry += " or admission_no in(" + admin_no + ")";
                        }


                        ies = new IES_Generic_Function();
                        ies.ies_parameters.Clear();
                        ies.ies_parameters.Add("@fee_id", id);
                        DataTable dt_student_details = ies.Fn_ExecutiveSql_Datatable(str_qry, 0, 1);
                        if (dt_student_details != null)
                        {
                            for (int istud = 0; istud <= dt_student_details.Rows.Count - 1; istud++)
                            {
                                Student_Details obj_stud = new Student_Details();
                                obj_stud.fee_id = Convert.ToInt32(id);
                                obj_stud.appid = appid;
                                obj_stud.admission_no = Convert.ToString(dt_student_details.Rows[istud]["admission_no"]);
                                obj_stud.student_id = Convert.ToString(dt_student_details.Rows[istud]["id"]);
                                obj_stud.course_id = Convert.ToString(dt_student_details.Rows[istud]["course_id"]);
                                obj_stud.batch_id = Convert.ToString(dt_student_details.Rows[istud]["batch_id"]);
                                obj_stud.semester_id = Convert.ToString(dt_student_details.Rows[istud]["semester_id"]);
                                obj_stud.category_id = Convert.ToString(dt_student_details.Rows[istud]["student_category_id"]);


                                obj_stud.item_id = Convert.ToString(item_id);
                                obj_stud.item_name = Convert.ToString(item_name);
                                obj_stud.item_description = Convert.ToString(item_description);
                                obj_stud.amount = Convert.ToString(actual_itemamt);
                                obj_stud.tax_id = Convert.ToString(tax_id);
                                obj_stud.tax_value = Convert.ToString(tax_value);
                                obj_stud.discount_value = Convert.ToString(discount_value);
                                obj_stud.discount_type = Convert.ToString(discount_type);

                                obj_stud.actualamt = Convert.ToString(actual_itemamt);
                                obj_stud.discountedamt = Convert.ToString(total_deduction);
                                obj_stud.netamt = Convert.ToString(item_totalamt);
                                obj_stud.taxamt = Convert.ToString(total_taxamt);
                                obj_stud.grossamt = Convert.ToString(total_grossamt);

                                lst_student.Add(obj_stud);

                            }
                        }

                    }
                }



                ///////////////////////////////////////////////////////////////////////////////////////////////////////
                int schedule_cnt = 0;
                IEnumerable<string> glc_studentid = lst_student.Select(x => x.student_id).Distinct();
                ies = new IES_Generic_Function();
                ies.ies_parameters.Clear();
                ies.ies_parameters.Add("@fee_id", id);
                DataTable dt_schedule = ies.Fn_ExecutiveSql_Datatable("select * from tbl_fee_schedule where fee_id=@fee_id", 0, 1);
                if (dt_schedule != null)
                {

                    if (is_divide == "1")
                    {
                        schedule_cnt = dt_schedule.Rows.Count;
                    }
                    else
                    {
                        schedule_cnt = 1;
                    }
                }

                bool isfirst = true;


                for (int ic = 0; ic <= dt_schedule.Rows.Count - 1; ic++)
                {
                    string schedule_id = Convert.ToString(dt_schedule.Rows[ic]["schedule_id"]);
                    string schedule_date = Convert.ToString(dt_schedule.Rows[ic]["schedule_date"]);
                    string fee_id = Convert.ToString(dt_schedule.Rows[ic]["fee_id"]);

                    if (ic != 0) isfirst = false;

                    foreach (string student_id in glc_studentid)
                    {
                        string stud_id = student_id;
                        decimal totalgrossamt = lst_student.Where(st => st.student_id == stud_id).Sum(st => Convert.ToDecimal(st.grossamt));
                        decimal invoiceamt = totalgrossamt / Convert.ToDecimal(schedule_cnt);



                        ies = new IES_Generic_Function();
                        ies.ies_parameters.Clear();
                        ies.ies_parameters.Add("@fee_id", Convert.ToString(fee_id));
                        ies.ies_parameters.Add("@student_id", Convert.ToString(stud_id));
                        ies.ies_parameters.Add("@schedule_id", Convert.ToString(schedule_id));
                        ies.ies_parameters.Add("@schedule_date", Convert.ToString(schedule_date));
                        ies.ies_parameters.Add("@invoice_amt", Convert.ToString(invoiceamt));
                        ies.ies_parameters.Add("@invoice_adjustment", Convert.ToString(0));
                        ies.ies_parameters.Add("@invoice_paid_amt", Convert.ToString(0));
                        ies.ies_parameters.Add("@invoice_due_amt", Convert.ToString(invoiceamt));
                        DataTable dt_resultparticular = ies.Fn_ExecutiveSql_Datatable("USP_Invoice", 1, 1);

                        if (dt_resultparticular != null && dt_resultparticular.Rows.Count > 0)
                        {
                            string invoiceid = Convert.ToString(dt_resultparticular.Rows[0]["id"]);

                            List<Student_Details> QSResult = lst_student.Where(st => st.student_id == stud_id).ToList();
                            foreach (Student_Details lstitem in QSResult)
                            {
                                decimal amt = Convert.ToDecimal(lstitem.amount) / schedule_cnt;
                                Save_Invoice_Data(lstitem, amt, invoiceid, isfirst, schedule_cnt);

                            }
                        }
                    }
                }



            }
        }
    }
    protected void Save_Invoice_Data(Student_Details lstitem, decimal amt, string invoiceid, bool isfirst,int schedule_cnt)
    {
        decimal total_grossamt = 0;
        decimal total_deduction = 0;
        decimal total_taxamt = 0;

        int fee_id = lstitem.fee_id;
        string appid = lstitem.appid;
        string admission_no = lstitem.admission_no;
        string student_id = lstitem.student_id;
        string course_id = lstitem.course_id;
        string batch_id = lstitem.batch_id;
        string semester_id = lstitem.semester_id;
        string category_id = lstitem.category_id;

        string item_id = lstitem.item_id;
        string item_name = lstitem.item_name;
        string item_description = lstitem.item_description;
        string amount = Convert.ToString(amt);
        string tax_id = lstitem.tax_id;
        decimal tax_value = Convert.ToDecimal(lstitem.tax_value);
        decimal discount_value = Convert.ToDecimal(lstitem.discount_value);
        string discount_type = lstitem.discount_type;

       

        decimal item_totalamt = Convert.ToDecimal(amt);
        decimal actual_itemamt = Convert.ToDecimal(amt);


        if (discount_type == "%")
        {
            if (discount_value != 0)
            {
                item_totalamt = item_totalamt - (item_totalamt * (discount_value / 100));
                total_deduction = (actual_itemamt * (discount_value / 100));
            }
        }
        else
        {
            if (discount_value != 0)
            {
                item_totalamt = item_totalamt - (discount_value/schedule_cnt);
                total_deduction = (discount_value / schedule_cnt);
            }
        }


        if (tax_value == 0)
        {
            total_grossamt = item_totalamt;
        }
        else
        {
            total_grossamt = item_totalamt + (item_totalamt * (tax_value / 100));
            total_taxamt = (item_totalamt * (tax_value / 100));
        }


        Student_Details obj_stud = new Student_Details();
        obj_stud.fee_id = fee_id;
        obj_stud.appid = appid;
        obj_stud.admission_no = admission_no;
        obj_stud.student_id = student_id;
        obj_stud.course_id = course_id;
        obj_stud.batch_id = batch_id;
        obj_stud.semester_id = semester_id;
        obj_stud.category_id = category_id;


        obj_stud.item_id = item_id;
        obj_stud.item_name = item_name;
        obj_stud.item_description = item_description;
        obj_stud.amount = Convert.ToString(actual_itemamt);
        obj_stud.tax_id = Convert.ToString(tax_id);
        obj_stud.tax_value = Convert.ToString(tax_value);
        obj_stud.discount_value = Convert.ToString(discount_value);
        obj_stud.discount_type = Convert.ToString(discount_type);

        obj_stud.actualamt = Convert.ToString(actual_itemamt);
        obj_stud.discountedamt = Convert.ToString(total_deduction);
        obj_stud.netamt = Convert.ToString(item_totalamt);
        obj_stud.taxamt = Convert.ToString(total_taxamt);
        obj_stud.grossamt = Convert.ToString(total_grossamt);

        string str_qry = @"insert into tbl_fee_invoicedetails(invoiceid,fee_id,appid,admission_no,student_id,course_id,batch_id,semester_id,category_id,
        item_id,item_name,item_description,amount,tax_id,tax_value,discount_value,discount_type,actualamt,discountedamt,netamt,taxamt,grossamt)
        values(@invoiceid,@fee_id,@appid,@admission_no,@student_id,@course_id,@batch_id,@semester_id,@category_id,
        @item_id,@item_name,@item_description,@amount,@tax_id,@tax_value,@discount_value,@discount_type,@actualamt,@discountedamt,@netamt,@taxamt,@grossamt)";

        IES_Generic_Function ies = new IES_Generic_Function();
        ies.ies_parameters.Clear();
        ies.ies_parameters.Add("@invoiceid", Convert.ToString(invoiceid));
        ies.ies_parameters.Add("@fee_id", Convert.ToString(obj_stud.fee_id));
        ies.ies_parameters.Add("@appid", Convert.ToString(obj_stud.appid));
        ies.ies_parameters.Add("@admission_no", Convert.ToString(obj_stud.admission_no));
        ies.ies_parameters.Add("@student_id", Convert.ToString(obj_stud.student_id));
        ies.ies_parameters.Add("@course_id", Convert.ToString(obj_stud.course_id));
        ies.ies_parameters.Add("@batch_id", Convert.ToString(obj_stud.batch_id));
        ies.ies_parameters.Add("@semester_id", Convert.ToString(obj_stud.semester_id));
        ies.ies_parameters.Add("@category_id", Convert.ToString(obj_stud.category_id));
        ies.ies_parameters.Add("@item_id", Convert.ToString(obj_stud.item_id));
        ies.ies_parameters.Add("@item_name", Convert.ToString(obj_stud.item_name));
        ies.ies_parameters.Add("@item_description", Convert.ToString(obj_stud.item_description));
        ies.ies_parameters.Add("@amount", Convert.ToString(obj_stud.amount));
        ies.ies_parameters.Add("@tax_id", Convert.ToString(obj_stud.tax_id));
        ies.ies_parameters.Add("@tax_value", Convert.ToString(obj_stud.tax_value));
        ies.ies_parameters.Add("@discount_value", Convert.ToString(obj_stud.discount_value));
        ies.ies_parameters.Add("@discount_type", Convert.ToString(obj_stud.discount_type));
        ies.ies_parameters.Add("@actualamt", Convert.ToString(obj_stud.actualamt));
        ies.ies_parameters.Add("@discountedamt", Convert.ToString(obj_stud.discountedamt));
        ies.ies_parameters.Add("@netamt", Convert.ToString(obj_stud.netamt));
        ies.ies_parameters.Add("@taxamt", Convert.ToString(obj_stud.taxamt));
        ies.ies_parameters.Add("@grossamt", Convert.ToString(obj_stud.grossamt));
        ies.Fn_ExecutiveSql(str_qry, 0, 1);

        if (isfirst == true)
        {
            str_qry = @"insert into tbl_fee_actual_invoicedetails(fee_id,appid,admission_no,student_id,course_id,batch_id,semester_id,category_id,
            item_id,item_name,item_description,amount,tax_id,tax_value,discount_value,discount_type,actualamt,discountedamt,netamt,taxamt,grossamt)
            values(@fee_id,@appid,@admission_no,@student_id,@course_id,@batch_id,@semester_id,@category_id,
            @item_id,@item_name,@item_description,@amount,@tax_id,@tax_value,@discount_value,@discount_type,@actualamt,@discountedamt,@netamt,@taxamt,@grossamt)";

            ies = new IES_Generic_Function();
            ies.ies_parameters.Clear();
            
            ies.ies_parameters.Add("@fee_id", Convert.ToString(lstitem.fee_id));
            ies.ies_parameters.Add("@appid", Convert.ToString(lstitem.appid));
            ies.ies_parameters.Add("@admission_no", Convert.ToString(lstitem.admission_no));
            ies.ies_parameters.Add("@student_id", Convert.ToString(lstitem.student_id));
            ies.ies_parameters.Add("@course_id", Convert.ToString(lstitem.course_id));
            ies.ies_parameters.Add("@batch_id", Convert.ToString(lstitem.batch_id));
            ies.ies_parameters.Add("@semester_id", Convert.ToString(lstitem.semester_id));
            ies.ies_parameters.Add("@category_id", Convert.ToString(lstitem.category_id));
            ies.ies_parameters.Add("@item_id", Convert.ToString(lstitem.item_id));
            ies.ies_parameters.Add("@item_name", Convert.ToString(lstitem.item_name));
            ies.ies_parameters.Add("@item_description", Convert.ToString(lstitem.item_description));
            ies.ies_parameters.Add("@amount", Convert.ToString(lstitem.amount));
            ies.ies_parameters.Add("@tax_id", Convert.ToString(lstitem.tax_id));
            ies.ies_parameters.Add("@tax_value", Convert.ToString(lstitem.tax_value));
            ies.ies_parameters.Add("@discount_value", Convert.ToString(lstitem.discount_value));
            ies.ies_parameters.Add("@discount_type", Convert.ToString(lstitem.discount_type));
            ies.ies_parameters.Add("@actualamt", Convert.ToString(lstitem.actualamt));
            ies.ies_parameters.Add("@discountedamt", Convert.ToString(lstitem.discountedamt));
            ies.ies_parameters.Add("@netamt", Convert.ToString(lstitem.netamt));
            ies.ies_parameters.Add("@taxamt", Convert.ToString(lstitem.taxamt));
            ies.ies_parameters.Add("@grossamt", Convert.ToString(lstitem.grossamt));
            ies.Fn_ExecutiveSql(str_qry, 0, 1);
        }

    }

}