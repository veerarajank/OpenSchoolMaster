using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Adm_My_ActivityFeed : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Bind_Activity();
    }
    protected void Bind_Activity()
    {
        IES_Generic_Function ies = new IES_Generic_Function();
        ies.ies_parameters.Clear();
        DataTable dt_data = ies.Fn_ExecutiveSql_Datatable(@"select *,'iikm' as initiator,dbo.UDF_DateFormat(activity_time,1,0) as activity_date,dbo.UDF_DateFormat(activity_time,0,1) as val_activity_time
         from tbl_activity_feed", 0, 0);
        lv_listview.DataSource = dt_data;
        lv_listview.DataBind();
    }
}