using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;



public partial class AdminDashboard : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {            
            IES_Generic_Function ies = new IES_Generic_Function();
            ies.ies_parameters.Clear();
            DataTable dt_data = ies.Fn_ExecutiveSql_Datatable("select (select academic_name from tbl_academic_years sub where sub.academic_id=pm.academic_id) as academic_name,pm.academic_id,replace(convert(varchar(10),getdate(),103),'/','-') as admissiondate from tbl_onlineregistration_settings pm where id='1'", 0, 0);
            if (dt_data != null && dt_data.Rows.Count > 0)
            {                
                hd_acyear.Value = Convert.ToString(dt_data.Rows[0]["academic_id"]);
            }
            Fee_BindDisplay();
            Student_BindDisplay(hd_acyear.Value);
            News_Bind();
            Bind_Event();
            Bind_Mail();
            Teacher_BindDisplay(hd_acyear.Value);
            Bind_Examination(hd_acyear.Value);

        }
    }
    protected void Bind_Examination(string academic_yr)
    {


        IES_Generic_Function ies = new IES_Generic_Function();
        DataTable dm_datatable = ies.Fn_ExecutiveSql_Datatable(@"select [tMonth],sum(tcount) [No of Admission] from
                (select left('January',3) as [tMonth],1 as [Cnt],0 as tCount union all
                select left('February',3) as [tMonth],2 as [Cnt],0 as tCount union all
                select left('March',3) as [tMonth],3 as [Cnt],0 as tCount union all
                select left('April',3) as [tMonth],4 as [Cnt],0 as tCount union all
                select left('May',3) as [tMonth],5 as [Cnt],0 as tCount union all
                select left('June',3) as [tMonth],6 as [Cnt],0 as tCount union all
                select left('July',3) as [tMonth],7 as [Cnt],0 as tCount union all
                select left('August',3) as [tMonth],8 as [Cnt],0 as tCount union all
                select left('September',3) as [tMonth],9 as [Cnt],0 as tCount union all
                select left('October',3) as [tMonth],10 as [Cnt],0 as tCount union all
                select left('November',3) as [tMonth],11 as [Cnt],0 as tCount union all
                select left('December',3) as [tMonth],12 as [Cnt],0 as tCount union all
                select left(Datename(month,created_at),3) as [tMonth],
                Datepart(month,created_at) as [Cnt],
                1 as tCount from tbl_students where academic_yr='" + academic_yr + "' ) T group by tMonth order by min(Cnt)", 0, 0);


        Chart2(dm_datatable, L_ltChart_2, "Consolidate_linechart2", L_ht2.ClientID);
        Chart3(dm_datatable, L_ltChart_3, "Consolidate_linechart3", L_ht3.ClientID);
    }

    public void Bind_Mail()
    {
        IES_Generic_Function ies = new IES_Generic_Function();
        ies.ies_parameters.Clear();
        DataTable dt_data = ies.Fn_ExecutiveSql_Datatable("select * from tbl_email_details", 0, 0);
        if (dt_data != null)
        {          
            lv_msg.DataSource = dt_data;
           lv_msg.DataBind();
        }

    }
    public void Bind_Event()
    {
        string Day_Event = "";

        IES_Generic_Function ies = new IES_Generic_Function();
        ies.ies_parameters.Clear();
        DataTable dt_data = ies.Fn_ExecutiveSql_Datatable("select * from tbl_events", 0, 0);
        if (dt_data != null)
        {
            for (int i = 0; i < dt_data.Rows.Count; i++)
            {
                Day_Event += "<a id=\"showJobDialog\" class=\"add event-inner-blk\" href=\"#\"><h3>" + Convert.ToString(dt_data.Rows[i]["Event_title"]) + "</h3>";
                Day_Event += "<p> " + Convert.ToString(dt_data.Rows[i]["Event_desc"]) + "</p>";
                Day_Event += "<span>" + Convert.ToString(dt_data.Rows[i]["Event_start"]) + "</span></a>";
            }
        }
        my_tabs.InnerHtml = Day_Event;
    }

    protected void News_Bind()
    {

        string str_news= "<ul>";
        IES_Generic_Function ies = new IES_Generic_Function();
        ies.ies_parameters.Clear();
        ies.ies_parameters.Add("@mode", "search");
        DataTable dt_data = ies.Fn_ExecutiveSql_Datatable("UDP_News_Log", 1, 1);
        if (dt_data != null)
        {
            for(int irow=0;irow<=dt_data.Rows.Count-1;irow++)
            {
                str_news+="<li class=\"mail_green\">";
                str_news+=" <h3>" + Convert.ToString(dt_data.Rows[irow]["News_Title"]) + "</h3>";
                str_news+=" <p>" + Convert.ToString(dt_data.Rows[irow]["News_Description"]) + "</p>";
                str_news+=" <span>" + Convert.ToString(dt_data.Rows[irow]["created_on"]) + "</span>";
                 str_news+=" </li>" ;
            }
        }
           str_news+=" </ul>";
           div_news.InnerHtml = str_news;
                    
                           
    }

    protected void Teacher_BindDisplay(string academic_yr)
    {
        IES_Generic_Function ies = new IES_Generic_Function();
        ies.ies_parameters.Clear();
        DataTable dt_data = ies.Fn_ExecutiveSql_Datatable("select * from tbl_employees where isnull(staff_type,'1')='1'", 0, 0);
        if (dt_data != null)
        {
            lbl_teachercnt.InnerText = Convert.ToString(dt_data.Rows.Count);
        }
        else
        {
            lbl_teachercnt.InnerText = Convert.ToString(0);
        }

        ies = new IES_Generic_Function();
        ies.ies_parameters.Clear();
        dt_data = ies.Fn_ExecutiveSql_Datatable("select * from tbl_employees where isnull(staff_type,'1')='1' and (created_at>=(select academic_start from tbl_academic_years where academic_id='" + academic_yr + "') and created_at<=(select academic_end from tbl_academic_years where academic_id='" + academic_yr + "'))", 0, 0);
        if (dt_data != null)
        {
            lbl_newteachercnt.InnerText = Convert.ToString(dt_data.Rows.Count);
        }
        else
        {
            lbl_newteachercnt.InnerText = Convert.ToString(0);
        }

        
    }

    protected void Student_BindDisplay(string academic_yr)
    {
        IES_Generic_Function ies = new IES_Generic_Function();
        ies.ies_parameters.Clear();
        DataTable dt_data = ies.Fn_ExecutiveSql_Datatable("select * from tbl_students where academic_yr='" + academic_yr  + "'", 0, 0);      
        if (dt_data != null)
        {
            lbl_studentcnt.InnerText  =  Convert.ToString(dt_data.Rows.Count);
        }
        else
        {
            lbl_studentcnt.InnerText  =   Convert.ToString(0);
        }

        ies = new IES_Generic_Function();
        ies.ies_parameters.Clear();
        dt_data = ies.Fn_ExecutiveSql_Datatable("select * from tbl_students where created_at>=(select academic_start from tbl_academic_years where academic_id='" + academic_yr + "') and created_at<=(select academic_end from tbl_academic_years where academic_id='" + academic_yr + "') ", 0, 0);
        if (dt_data != null)
        {
            lbl_newstudentcnt.InnerText = Convert.ToString(dt_data.Rows.Count);
        }
        else
        {
            lbl_newstudentcnt.InnerText = Convert.ToString(0);
        }

        ies = new IES_Generic_Function();
        ies.ies_parameters.Clear();
        dt_data = ies.Fn_ExecutiveSql_Datatable("select * from tbl_students where academic_yr='" + academic_yr + "' and is_active='0'", 0, 0);
        if (dt_data != null)
        {
            lbl_inactivestudentcnt.InnerText = Convert.ToString(dt_data.Rows.Count);
        }
        else
        {
            lbl_inactivestudentcnt.InnerText = Convert.ToString(0);
        }

        ies = new IES_Generic_Function();
        DataTable dm_datatable = ies.Fn_ExecutiveSql_Datatable(@"select [tMonth],sum(tcount) [No of Admission] from
                (select left('January',3) as [tMonth],1 as [Cnt],0 as tCount union all
                select left('February',3) as [tMonth],2 as [Cnt],0 as tCount union all
                select left('March',3) as [tMonth],3 as [Cnt],0 as tCount union all
                select left('April',3) as [tMonth],4 as [Cnt],0 as tCount union all
                select left('May',3) as [tMonth],5 as [Cnt],0 as tCount union all
                select left('June',3) as [tMonth],6 as [Cnt],0 as tCount union all
                select left('July',3) as [tMonth],7 as [Cnt],0 as tCount union all
                select left('August',3) as [tMonth],8 as [Cnt],0 as tCount union all
                select left('September',3) as [tMonth],9 as [Cnt],0 as tCount union all
                select left('October',3) as [tMonth],10 as [Cnt],0 as tCount union all
                select left('November',3) as [tMonth],11 as [Cnt],0 as tCount union all
                select left('December',3) as [tMonth],12 as [Cnt],0 as tCount union all
                select left(Datename(month,created_at),3) as [tMonth],
                Datepart(month,created_at) as [Cnt],
                1 as tCount from tbl_students where academic_yr='" + academic_yr + "' ) T group by tMonth order by min(Cnt)", 0, 0);


        Chart(dm_datatable, L_ltChart_1, "Consolidate_linechart", L_ht1.ClientID);
    }
    protected void Chart(DataTable dm_table, Literal ltChart, string line_chart, string hidden_control_name)
    {
        String chart = "";
        // You can change your chart height by modify height value
        chart = "<canvas id=\"" + line_chart + "\" width=\"100%\" height=\"40\"></canvas>";
        chart += "<script>";
        chart += "new Chart(document.getElementById(\"" + line_chart + "\"), { type: 'bar', data: {labels: [";

        // more details in x-axis               

        for (int i = 0; i <= dm_table.Rows.Count - 1; i++)
        {
            chart += "\"" + Convert.ToString(dm_table.Rows[i]["tMonth"]) + "\",";

        }


        chart = chart.Substring(0, chart.Length - 1);

        chart += "],datasets: [";

        for (int j = 1; j <= dm_table.Columns.Count - 1; j++)
        {

            if (j == 1)
            {
                chart += "{ data: [";
            }
            else
            {
                chart += ",{ data: [";
            }

            String value = "";
            for (int i = 0; i <= dm_table.Rows.Count - 1; i++)
            {
                if (value == "") value += Convert.ToString(dm_table.Rows[i][j]);
                else value += "," + Convert.ToString(dm_table.Rows[i][j]);
            }
            chart += value;

            if (Convert.ToString(dm_table.Columns[j].ColumnName) == "Month")
            {
                chart += "],label: \"" + Convert.ToString(dm_table.Columns[j].ColumnName) + "\", borderColor: 'rgb(0,128,0)',fill: false}"; // Chart color
            }
            else
            {
                chart += "],label: \"" + Convert.ToString(dm_table.Columns[j].ColumnName) + "\", borderColor: dynamicColors(" + Convert.ToString(j) + "),fill: false}"; // Chart color
            }
        }
        chart += "]},options: { legend: {display: false},title: { display: true,text: 'Monthly Average Admission '  },bezierCurve : false,animation: {onComplete:  function(){ document.getElementById(\"" + hidden_control_name + "\").value = this.toBase64Image();}},";
        chart += @"
                    scales: {
                        xAxes: [{
                        scaleLabel: {
                        display: true,
                        labelString: 'Month'
                        }
                        }],
                        yAxes: [{
                        scaleLabel: {
                        display: true,
                        labelString: 'No of Admission'
                        }
                        }]
                        
                    },

 layout: {
            padding: {
                left: 10,
                right: 0,
                top: 0,
                bottom: 0
            }
        }     
                   ";





        chart += "}});";
        chart += "</script>";

        ltChart.Text = chart;
    }
    protected void Chart2(DataTable dm_table, Literal ltChart, string line_chart, string hidden_control_name)
    {
        String chart = "";
        // You can change your chart height by modify height value
        chart = "<canvas id=\"" + line_chart + "\" width=\"100%\" height=\"40\"></canvas>";
        chart += "<script>";
        chart += "new Chart(document.getElementById(\"" + line_chart + "\"), { type: 'doughnut', data: {labels: [";

        // more details in x-axis               

        for (int i = 0; i <= dm_table.Rows.Count - 1; i++)
        {
            chart += "\"" + Convert.ToString(dm_table.Rows[i]["tMonth"]) + "\",";

        }


        chart = chart.Substring(0, chart.Length - 1);

        chart += "],datasets: [";

        for (int j = 1; j <= dm_table.Columns.Count - 1; j++)
        {

            if (j == 1)
            {
                chart += "{ data: [";
            }
            else
            {
                chart += ",{ data: [";
            }

            String value = "";
            for (int i = 0; i <= dm_table.Rows.Count - 1; i++)
            {
                if (value == "") value += Convert.ToString(dm_table.Rows[i][j]);
                else value += "," + Convert.ToString(dm_table.Rows[i][j]);
            }
            chart += value;

            if (Convert.ToString(dm_table.Columns[j].ColumnName) == "Month")
            {
                chart += "]}"; // Chart color
            }
            else
            {
                chart += "]}"; // Chart color
            }
        }
        chart += "]},options: { legend: {display: false},title: { display: true,text: 'Annual Exam Pass'  },bezierCurve : false,animation: {onComplete:  function(){ document.getElementById(\"" + hidden_control_name + "\").value = this.toBase64Image();}},";
        chart += @"                  


                   ";





        chart += "}});";
        chart += "</script>";

        ltChart.Text = chart;
    }

    protected void Chart3(DataTable dm_table, Literal ltChart, string line_chart, string hidden_control_name)
    {
        String chart = "";
        // You can change your chart height by modify height value
        chart = "<canvas id=\"" + line_chart + "\" width=\"100%\" height=\"40\"></canvas>";
        chart += "<script>";
        chart += "new Chart(document.getElementById(\"" + line_chart + "\"), { type: 'doughnut', data: {labels: [";

        // more details in x-axis               

        for (int i = 0; i <= dm_table.Rows.Count - 1; i++)
        {
            chart += "\"" + Convert.ToString(dm_table.Rows[i]["tMonth"]) + "\",";

        }


        chart = chart.Substring(0, chart.Length - 1);

        chart += "],datasets: [";

        for (int j = 1; j <= dm_table.Columns.Count - 1; j++)
        {

            if (j == 1)
            {
                chart += "{ data: [";
            }
            else
            {
                chart += ",{ data: [";
            }

            String value = "";
            for (int i = 0; i <= dm_table.Rows.Count - 1; i++)
            {
                if (value == "") value += Convert.ToString(dm_table.Rows[i][j]);
                else value += "," + Convert.ToString(dm_table.Rows[i][j]);
            }
            chart += value;

            if (Convert.ToString(dm_table.Columns[j].ColumnName) == "Month")
            {
                chart += "]}"; // Chart color
            }
            else
            {
                chart += "]}"; // Chart color
            }
        }
        chart += "]},options: { legend: {display: false},title: { display: true,text: 'Annual Exam Average Marks'  },bezierCurve : false,animation: {onComplete:  function(){ document.getElementById(\"" + hidden_control_name + "\").value = this.toBase64Image();}},";
        chart += @"                  


                   ";





        chart += "}});";
        chart += "</script>";

        ltChart.Text = chart;
    }
    protected void Fee_BindDisplay()
    {

        IES_Generic_Function ies = new IES_Generic_Function();
        ies.ies_parameters.Clear();
        ies.ies_parameters.Add("@mode", "search");
        DataTable dt_data = ies.Fn_ExecutiveSql_Datatable("UDP_FeeCategory", 1, 1);      

        if (dt_data != null)
        {
            div_fee_totalcategory.InnerHtml = "<h3>" + Convert.ToString(dt_data.Rows.Count) + "</h3>";
        }
        else
        {
            div_fee_totalcategory.InnerHtml = "<h3>" +  Convert.ToString(0) + "</h3>";
        }

        int invoice_cnt =
            dt_data.AsEnumerable()
               .Count(row => row.Field<int>("is_viewinvoice") != 0);

        div_fee_totalinvoice.InnerHtml = "<h3>" + Convert.ToString(invoice_cnt) + "</h3>";


        ies = new IES_Generic_Function();
        ies.ies_parameters.Clear();
      
        ies.ies_parameters.Add("@mode", "search");
        dt_data = ies.Fn_ExecutiveSql_Datatable("USP_CreatePayment_Collection", 1, 1);

        IEnumerable<DataRow> fee_amt_list =
           dt_data.AsEnumerable().Where(row => row.Field<string>("payment_date") == DateTime.Now.ToString("dd-MM-yyyy"));

        decimal fee_amt = 0;
        foreach (DataRow feelist in fee_amt_list)
        {
            fee_amt += Convert.ToDecimal(feelist.ItemArray[6]);
        }

        div_fee_totalcollect.InnerHtml = "<h3>" + Convert.ToString(fee_amt) + "</h3>";
    }
}