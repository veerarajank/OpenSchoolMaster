using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;


public class Event
{   
    public int Event_id { get; set; }
    public string Event_user_id { get; set; }
    public string Event_title { get; set; }
    public string Event_desc { get; set; }
    public string Event_type { get; set; }
    public string Event_Color { get; set; }
    public string Event_allDay { get; set; }
    public string Event_start { get; set; }
    public string Event_end { get; set; }
    public string Event_editable { get; set; }
    public string Event_placeholder { get; set; }
    public string Event_code { get; set; }
    public string Event_organizer { get; set; }
}


public partial class Adm_My_Event : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    [WebMethod]
    public static IList GetEvents()
    {
        IList events = new List<Event>();
        IES_Generic_Function ies = new IES_Generic_Function();
        ies.ies_parameters.Clear();
        DataTable dt_data = ies.Fn_ExecutiveSql_Datatable("select * from tbl_events", 0, 0);
        if (dt_data != null)
        {
            for (int i = 0; i < dt_data.Rows.Count; i++)
            {
                events.Add(new Event
                {
                    Event_id = Convert.ToInt32(dt_data.Rows[i]["Event_id"]),
                    Event_user_id = Convert.ToString(dt_data.Rows[i]["Event_user_id"]),
                    Event_title = Convert.ToString(dt_data.Rows[i]["Event_title"]),
                    Event_desc = Convert.ToString(dt_data.Rows[i]["Event_desc"]),
                    Event_type = Convert.ToString(dt_data.Rows[i]["Event_type"]),
                    Event_allDay = Convert.ToString(dt_data.Rows[i]["Event_allDay"]),
                    Event_start = Convert.ToString(dt_data.Rows[i]["Event_start"]),
                    Event_end = Convert.ToString(dt_data.Rows[i]["Event_end"]),
                    Event_editable = Convert.ToString(dt_data.Rows[i]["Event_editable"]),
                    Event_placeholder = Convert.ToString(dt_data.Rows[i]["Event_placeholder"]),
                    Event_code = Convert.ToString(dt_data.Rows[i]["Event_code"]),
                    Event_organizer = Convert.ToString(dt_data.Rows[i]["organizer"])                   
                });
            }
        }
        return events;
    }
}