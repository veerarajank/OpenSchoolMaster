using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Adm_Time_Table_DefaultSystem_WeekDay : System.Web.UI.Page
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
            //ies.Fn_PageLevel_Access("Brand", Convert.ToString(Session["Usrid"]));

            Bind_Result();


        }
    }
    protected void btn_setdays_ServerClick(object sender, EventArgs e)
    {
       

        string is_sunday = Convert.ToString(hd_sunday.Value);
        string is_monday = Convert.ToString(hd_monday.Value);
        string is_tuesday = Convert.ToString(hd_tuesday.Value);
        string is_wednesday = Convert.ToString(hd_wednesday.Value);
        string is_thursday = Convert.ToString(hd_thursday.Value);
        string is_friday = Convert.ToString(hd_friday.Value);
        string is_saturday = Convert.ToString(hd_saturday.Value);

        string str_qry = @"select isnull(is_sunday,0) as is_sunday,
        isnull(is_monday,0) as is_monday,
        isnull(is_tuesday,0) as is_tuesday,     
        isnull(is_wednesday,0) as is_wednesday,
        isnull(is_thursday,0) as is_thursday,
        isnull(is_friday,0) as is_friday,
        isnull(is_saturday,0) as is_saturday
        from tbl_timetable_defaultweekday";
        IES_Generic_Function ies = new IES_Generic_Function();
        ies.ies_parameters.Clear();
        
        DataTable dt_data = ies.Fn_ExecutiveSql_Datatable(str_qry, 0, 1);
        if (dt_data != null && dt_data.Rows.Count > 0)
        {
            string str_update_qry =
                @"update tbl_timetable_defaultweekday set is_sunday=@is_sunday,is_monday=@is_monday,is_tuesday=@is_tuesday,
                is_wednesday=@is_wednesday,is_thursday=@is_thursday,is_friday=@is_friday,is_saturday=@is_saturday";
            ies = new IES_Generic_Function();
            ies.ies_parameters.Clear();
            

            ies.ies_parameters.Add("@is_sunday", Convert.ToString(is_sunday));
            ies.ies_parameters.Add("@is_monday", Convert.ToString(is_monday));
            ies.ies_parameters.Add("@is_tuesday", Convert.ToString(is_tuesday));
            ies.ies_parameters.Add("@is_wednesday", Convert.ToString(is_wednesday));
            ies.ies_parameters.Add("@is_thursday", Convert.ToString(is_thursday));
            ies.ies_parameters.Add("@is_friday", Convert.ToString(is_friday));
            ies.ies_parameters.Add("@is_saturday", Convert.ToString(is_saturday));
            ies.Fn_ExecutiveSql(str_update_qry, 0, 1);
        }
        else
        {
            string str_update_qry =
               @"insert into tbl_timetable_defaultweekday(is_sunday,is_monday,is_tuesday,is_wednesday,is_thursday,is_friday,is_saturday)
                values(@is_sunday,@is_monday,@is_tuesday,@is_wednesday,@is_thursday,@is_friday,@is_saturday)";
            ies = new IES_Generic_Function();
            ies.ies_parameters.Clear();
           
            ies.ies_parameters.Add("@is_sunday", Convert.ToString(is_sunday));
            ies.ies_parameters.Add("@is_monday", Convert.ToString(is_monday));
            ies.ies_parameters.Add("@is_tuesday", Convert.ToString(is_tuesday));
            ies.ies_parameters.Add("@is_wednesday", Convert.ToString(is_wednesday));
            ies.ies_parameters.Add("@is_thursday", Convert.ToString(is_thursday));
            ies.ies_parameters.Add("@is_friday", Convert.ToString(is_friday));
            ies.ies_parameters.Add("@is_saturday", Convert.ToString(is_saturday));
            ies.Fn_ExecutiveSql(str_update_qry, 0, 1);
        }
        Bind_Result();
    }
    protected void Bind_Result()
    {      

        string str_qry = @"select isnull(is_sunday,0) as is_sunday,
        isnull(is_monday,0) as is_monday,
        isnull(is_tuesday,0) as is_tuesday,     
        isnull(is_wednesday,0) as is_wednesday,
        isnull(is_thursday,0) as is_thursday,
        isnull(is_friday,0) as is_friday,
        isnull(is_saturday,0) as is_saturday
        from tbl_timetable_defaultweekday";
        IES_Generic_Function ies = new IES_Generic_Function();
        ies.ies_parameters.Clear();
     
        DataTable dt_data = ies.Fn_ExecutiveSql_Datatable(str_qry, 0, 1);
        if (dt_data != null && dt_data.Rows.Count > 0)
        {
            hd_1.Attributes.Add("style", "background-position: 0px 0px;");
            hd_2.Attributes.Add("style", "background-position: 0px 0px;");
            hd_3.Attributes.Add("style", "background-position: 0px 0px;");
            hd_4.Attributes.Add("style", "background-position: 0px 0px;");
            hd_5.Attributes.Add("style", "background-position: 0px 0px;");
            hd_6.Attributes.Add("style", "background-position: 0px 0px;");
            hd_7.Attributes.Add("style", "background-position: 0px 0px;");

            int is_sunday = Convert.ToInt32(dt_data.Rows[0]["is_sunday"]);
            int is_monday = Convert.ToInt32(dt_data.Rows[0]["is_monday"]);
            int is_tuesday = Convert.ToInt32(dt_data.Rows[0]["is_tuesday"]);
            int is_wednesday = Convert.ToInt32(dt_data.Rows[0]["is_wednesday"]);
            int is_thursday = Convert.ToInt32(dt_data.Rows[0]["is_thursday"]);
            int is_friday = Convert.ToInt32(dt_data.Rows[0]["is_friday"]);
            int is_saturday = Convert.ToInt32(dt_data.Rows[0]["is_saturday"]);

            if (is_sunday == 1) hd_1.Attributes.Add("style", "background-position: 0px -64px;");
            if (is_monday == 1) hd_2.Attributes.Add("style", "background-position: 0px -64px;");
            if (is_tuesday == 1) hd_3.Attributes.Add("style", "background-position: 0px -64px;");
            if (is_wednesday == 1) hd_4.Attributes.Add("style", "background-position: 0px -64px;");
            if (is_thursday == 1) hd_5.Attributes.Add("style", "background-position: 0px -64px;");
            if (is_friday == 1) hd_6.Attributes.Add("style", "background-position: 0px -64px;");
            if (is_saturday == 1) hd_7.Attributes.Add("style", "background-position: 0px -64px;");

            hd_sunday.Value = Convert.ToString(dt_data.Rows[0]["is_sunday"]);
            hd_monday.Value = Convert.ToString(dt_data.Rows[0]["is_monday"]);
            hd_tuesday.Value = Convert.ToString(dt_data.Rows[0]["is_tuesday"]);
            hd_wednesday.Value = Convert.ToString(dt_data.Rows[0]["is_wednesday"]);
            hd_thursday.Value = Convert.ToString(dt_data.Rows[0]["is_thursday"]);
            hd_friday.Value = Convert.ToString(dt_data.Rows[0]["is_friday"]);
            hd_saturday.Value = Convert.ToString(dt_data.Rows[0]["is_saturday"]);

        }
        else
        {
            hd_1.Attributes.Add("style", "background-position: 0px 0px;");
            hd_2.Attributes.Add("style", "background-position: 0px 0px;");
            hd_3.Attributes.Add("style", "background-position: 0px 0px;");
            hd_4.Attributes.Add("style", "background-position: 0px 0px;");
            hd_5.Attributes.Add("style", "background-position: 0px 0px;");
            hd_6.Attributes.Add("style", "background-position: 0px 0px;");
            hd_7.Attributes.Add("style", "background-position: 0px 0px;");

            hd_sunday.Value = Convert.ToString("0");
            hd_monday.Value = Convert.ToString("0");
            hd_tuesday.Value = Convert.ToString("0");
            hd_wednesday.Value = Convert.ToString("0");
            hd_thursday.Value = Convert.ToString("0");
            hd_friday.Value = Convert.ToString("0");
            hd_saturday.Value = Convert.ToString("0");

        }
    }
}