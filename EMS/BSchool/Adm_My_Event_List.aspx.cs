using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Adm_My_Event_List : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
      
        IES_Generic_Function ies = new IES_Generic_Function();
        ies.ies_parameters.Clear();
        DataTable dt_data = ies.Fn_ExecutiveSql_Datatable("select *,Datepart(day,convert(datetime,Event_start)) as dayvalue,left(Datename(month,convert(datetime,Event_start)),3) as monthvalue from tbl_events", 0, 0);
        lv_listview.DataSource = dt_data;
        lv_listview.DataBind();
    }
}