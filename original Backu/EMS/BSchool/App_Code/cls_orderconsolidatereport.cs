using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

/// <summary>
/// Summary description for cls_orderconsolidatereport
/// </summary>
public class cls_orderconsolidatereport
{
	public cls_orderconsolidatereport()
	{
		//
		// TODO: Add constructor logic here
		//
	}
    public string Generate_stagewiseOrderCnt(string stageid , string finnancialyear,string stagename,int rowcnt)
    {
        int cnt = 0;
        string stagelevelname = @"<table class='table table-bordered' style='border-collapse:collapse;'><tr><td colspan='2' style='
            background-color:#00C0EF;text-align:left;font-weight:bolder;font-size:large;color:white;'            
            >" + stagename + "</td><tr>";
        IES_Generic_Function ies = new IES_Generic_Function();
        ies.ies_parameters.Add("@passout_acedemic", Convert.ToString(finnancialyear).Trim());
        ies.ies_parameters.Add("@stageid", Convert.ToString(stageid).Trim());
        DataTable dm_table = ies.Fn_ExecutiveSql_Datatable("USP_DashboardOrderConsolidateCount", 1, 1);
        if (dm_table !=null)
        {
            if (dm_table.Rows.Count > 0)
            {
                for (int irow = 0; irow < dm_table.Rows.Count; irow++)
                {
                    cnt += Convert.ToInt32(dm_table.Rows[irow]["ordercnt"]);
                    stagelevelname += "<tr><td>" + Convert.ToString(dm_table.Rows[irow]["stagelevelname"]).ToUpper() + "</td><td style='text-align: right;'>" + Convert.ToString(dm_table.Rows[irow]["ordercnt"]).ToUpper() + "</td></tr>";                   
                }
                if (rowcnt > (dm_table.Rows.Count+1))
                {
                    for (int idata = dm_table.Rows.Count; idata <= rowcnt-1; idata++)
                    {
                        stagelevelname += "<tr><td>&nbsp;</td><td>&nbsp;</td></tr>";                   
                    }
                }
            }
        }
        stagelevelname += @"<tr 
        style='background-color: #00a65a !important; color: white; font-weight: bolder; font-size: large'
            ><td style='text-align: right;'>Grand Total</td><td style='text-align: right;'>" + Convert.ToString(cnt) + "</td></tr>";
        stagelevelname += "</table>";
        return stagelevelname;

    }
}