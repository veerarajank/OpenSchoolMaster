using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Services;

/// <summary>
/// Summary description for Chart_Json
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
// [System.Web.Script.Services.ScriptService]
public class Chart_Json : System.Web.Services.WebService {

    public Chart_Json () {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }

    [WebMethod]
    public string GetExpense_Chart(string payeetype, string category, string expensedesc, 
        string recepitno, string customername, string cellno, string searchdate, string status)
    {
        
        IES_Generic_Function ies = new IES_Generic_Function();
        IES_Generic_Function ies2 = new IES_Generic_Function();
        ies.ies_parameters.Clear();
        ies2.ies_parameters.Clear();

        string strqry = "";
        string strqryin = "select expensedesc from tbl_expenseentry where 1=1";
        string strqryconsolidate = @"select (select upper(co.categoryname) from tbl_expensecategory co where co.categoryid=dg.expensetype) as expensetype,
            (select upper(co.descriptionname) from tbl_expensedescription co where co.descid=dg.expensedesc) as expensedesc,                   
            sum(expenseamt) as expenseamt        
            from tbl_expenseentry dg where 1=1 ";

        if (payeetype != "0")
        {
            ies.ies_parameters.Add("@Payeetype", Convert.ToString(payeetype).Trim());
            ies2.ies_parameters.Add("@Payeetype", Convert.ToString(payeetype).Trim());
            strqry = " and Payeetype=@Payeetype";
        }
        if (category != "0")
        {
            ies.ies_parameters.Add("@expensetype", Convert.ToString(category).Trim());
            ies2.ies_parameters.Add("@expensetype", Convert.ToString(category).Trim());
            strqry = " and expensetype=@expensetype";
        }
        if (expensedesc != "0")
        {
            ies.ies_parameters.Add("@expensedesc", Convert.ToString(expensedesc).Trim());
            ies2.ies_parameters.Add("@expensedesc", Convert.ToString(expensedesc).Trim());
            strqry = " and expensedesc=@expensedesc";
        }
        if (recepitno != "")
        {
            ies.ies_parameters.Add("@expenseno", "%" + Convert.ToString(recepitno).Trim() + "%");
            ies2.ies_parameters.Add("@expenseno", "%" + Convert.ToString(recepitno).Trim() + "%");
            strqry = " and expenseno like @expenseno";
        }
        if (customername != "0")
        {
            ies.ies_parameters.Add("@payeeid", Convert.ToString(customername).Trim());
            ies2.ies_parameters.Add("@payeeid", Convert.ToString(customername).Trim());
            strqry = " and payeeid = @payeeid";
        }
        if (cellno != "")
        {
            ies.ies_parameters.Add("@payeecontact", "%" + Convert.ToString(cellno).Trim() + "%");
            ies2.ies_parameters.Add("@payeecontact", "%" + Convert.ToString(cellno).Trim() + "%");
            strqry = " and payeecontact like @payeecontact";
        }
        if (searchdate != "")
        {
            string podate = searchdate.Replace("to", "/");
            string startdate = Convert.ToString(podate.Split('/')[0]).Trim() + " 00:00:00";
            string enddate = Convert.ToString(podate.Split('/')[1]).Trim() + " 23:59:59";

            DateTime dtstartdate = DateTime.ParseExact(startdate, "dd-MM-yyyy HH:mm:ss", null);
            DateTime dtend = DateTime.ParseExact(enddate, "dd-MM-yyyy HH:mm:ss", null);

            ies.ies_parameters.Add("@startadvancereceivedon", Convert.ToString(dtstartdate).Trim());
            ies.ies_parameters.Add("@endadvancereceivedon", Convert.ToString(dtend).Trim());

            ies2.ies_parameters.Add("@startadvancereceivedon", Convert.ToString(dtstartdate).Trim());
            ies2.ies_parameters.Add("@endadvancereceivedon", Convert.ToString(dtend).Trim());

            strqry = " and (Expensedate >= @startadvancereceivedon and Expensedate<=endadvancereceivedon)";


        }
        if (status != "0")
        {
            ies.ies_parameters.Add("@expensestatus", Convert.ToString(status).Trim());
            ies2.ies_parameters.Add("@expensestatus", Convert.ToString(status).Trim());
            strqry = " and expensestatus = @expensestatus";
        }


        strqryin = strqryin + strqry;
        strqryconsolidate = strqryconsolidate + strqry + " group by expensetype,expensedesc";

        string sumofvalue = "";
        string deptname = "";
        string qrydept = @"select distinct upper(descriptionname) as Name from tbl_expensedescription where descid in(" + strqryin + ")";

       
        DataTable dm_table = ies.Fn_ExecutiveSql_Datatable(qrydept, 0, 1);
        if (dm_table != null)
        {
            if (dm_table.Rows.Count > 0)
            {
                for (int irow = 0; irow < dm_table.Rows.Count; irow++)
                {
                    if (deptname == "") deptname = "[" + Convert.ToString(dm_table.Rows[irow]["name"]).ToUpper() + "]";
                    else deptname += ",[" + Convert.ToString(dm_table.Rows[irow]["name"]).ToUpper() + "]";

                    if (sumofvalue == "") sumofvalue = "isnull([" + Convert.ToString(dm_table.Rows[irow]["name"]).ToUpper() + "],0)";
                    else sumofvalue += "+ isnull([" + Convert.ToString(dm_table.Rows[irow]["name"]).ToUpper() + "],0)";

                }
            }
        }


        string qry = @"select *, " + sumofvalue + @" as [Grand Total]  from (select *  from (
                select expensetype as [Category]," + deptname + @"
                from 
                (" + strqryconsolidate + @") src
                pivot
                (
                  sum(expenseamt)
                  for expensedesc in (" + deptname + @")
                ) piv) T) Tmain 
                order by Category";

        
      
        dm_table = ies2.Fn_ExecutiveSql_Datatable(qry, 0, 1);

        JavaScriptSerializer jsSerializer = new JavaScriptSerializer();
        List<Dictionary<string, object>> parentRow = new List<Dictionary<string, object>>();
        Dictionary<string, object> childRow;
        foreach (DataRow row in dm_table.Rows)
        {
            childRow = new Dictionary<string, object>();
            foreach (DataColumn col in dm_table.Columns)
            {
                childRow.Add(col.ColumnName, row[col]);
            }
            parentRow.Add(childRow);
        }
        return jsSerializer.Serialize(parentRow);  
    }
    
}
