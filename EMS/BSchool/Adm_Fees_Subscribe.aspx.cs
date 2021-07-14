using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Adm_Fees_Subscribe : System.Web.UI.Page
{
    [Serializable]
    public class Custom_DueDates
    {
        public string DueDates { get; set; }        
        public int RefId { get; set; }       
    }
    public List<Custom_DueDates> Lst_Custom_DueDates
    {
        get
        {
            if (ViewState["Custom_DueDates"] == null) ViewState["Custom_DueDates"] = new List<Custom_DueDates>();
            return (List<Custom_DueDates>)ViewState["Custom_DueDates"];
        }
        set
        {
            ViewState["Custom_DueDates"] = this;
        }
    }
    public static int refid = 0;
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
        IES_Generic_Function ies = new IES_Generic_Function();
        ies.ies_parameters.Clear();
        ies.ies_parameters.Add("@mode", "search");
        ies.ies_parameters.Add("@id", id);
        DataTable dt_data = ies.Fn_ExecutiveSql_Datatable("UDP_FeeCategory", 1, 1);      
        if (dt_data != null)
        {
            if (dt_data.Rows.Count > 0)
            {
                lbl_category.InnerText = Convert.ToString(dt_data.Rows[0]["name"]);
                lbl_createdon.InnerText = Convert.ToString(dt_data.Rows[0]["created_on"]);
                lbl_desc.InnerText = Convert.ToString(dt_data.Rows[0]["fees_description"]);
            }
        }
      
    }
    protected void lnk_del_Click(object sender, EventArgs e)
    {
        LinkButton lnk = (LinkButton)sender;
        ListViewDataItem parent = (ListViewDataItem)lnk.Parent;
        int int_RefId = Lst_Custom_DueDates[parent.DisplayIndex].RefId;
        Custom_DueDates lst_exist = Lst_Custom_DueDates.Find(p => p.RefId == int_RefId);
        if (lst_exist != null)
        {
            Lst_Custom_DueDates.Remove(lst_exist);
            lv_listview.DataSource = Lst_Custom_DueDates;
            lv_listview.DataBind();
        }
    }
    protected void add_custom_due_date_ServerClick(object sender, EventArgs e)
    {

        foreach (ListViewItem item1 in lv_listview.Items)
        {
            TextBox tbx_duedate = (TextBox)item1.FindControl("tbx_duedate");
            if (tbx_duedate != null)
            {
                HiddenField hd_refid = (HiddenField)item1.FindControl("hd_refid");
                Custom_DueDates lst_exist = Lst_Custom_DueDates.Find(p => p.RefId == Convert.ToInt32(hd_refid.Value));
                if (lst_exist != null)
                {
                    lst_exist.DueDates = tbx_duedate.Text;
                 }
            }
        }
        Custom_DueDates objdate = new Custom_DueDates();
        objdate.DueDates = "";
        objdate.RefId = refid++;
        Lst_Custom_DueDates.Add(objdate);
        lv_listview.DataSource = Lst_Custom_DueDates;
        lv_listview.DataBind();
    }
    protected void btn_createsubscribe_Click(object sender, EventArgs e)
    {
        if (ValidInput() == true) { field_mandary.Attributes.Add("style", "color:red"); return; }

        string id = Convert.ToString(Request.QueryString["id"]);

        IES_Generic_Function ies = new IES_Generic_Function();
        ies.ies_parameters.Clear();      

        string fees_start_date = "";
        if (tbx_start_date.Value.Trim() != "")
        {
            DateTime dtfees_start_date = DateTime.ParseExact(tbx_start_date.Value.Trim(), "dd-MM-yyyy", null);
            fees_start_date = dtfees_start_date.ToString("yyyy-MM-dd");
        }

        string fees_end_date = "";
        if (tbx_end_date.Value.Trim() != "")
        {
            DateTime dtfees_end_date = DateTime.ParseExact(tbx_end_date.Value.Trim(), "dd-MM-yyyy", null);
            fees_end_date = dtfees_end_date.ToString("yyyy-MM-dd");
        }
        string is_divide = "0";
        if (chk_amtdivide.Checked == true) is_divide = "1";
        ies.ies_parameters.Add("@id", id);
        ies.ies_parameters.Add("@fees_start_date", Convert.ToString(fees_start_date));
        ies.ies_parameters.Add("@fees_end_date", Convert.ToString(fees_end_date).Trim());
        ies.ies_parameters.Add("@schedule_mode", Convert.ToString("1").Trim());
        ies.ies_parameters.Add("@is_divide", Convert.ToString(is_divide).Trim());
        ies.ies_parameters.Add("@mode", "updatesubscripe");
        ies.Fn_ExecutiveSql("UDP_FeeCategory", 1, 1);

        foreach (ListViewItem item1 in lv_listview.Items)
        {
            TextBox tbx_duedate = (TextBox)item1.FindControl("tbx_duedate");
            if (tbx_duedate != null)
            {
                
                string schedule_date = "";
                if (tbx_duedate.Text.Trim() != "")
                {
                    DateTime dtschedule_date = DateTime.ParseExact(tbx_duedate.Text.Trim(), "dd-MM-yyyy", null);
                    schedule_date = dtschedule_date.ToString("yyyy-MM-dd");
                    ies = new IES_Generic_Function();
                    ies.ies_parameters.Clear();    
                    ies.ies_parameters.Add("@fee_id", id);
                    ies.ies_parameters.Add("@schedule_mode", Convert.ToString("1").Trim());
                    ies.ies_parameters.Add("@schedule_date", Convert.ToString(schedule_date));
                    ies.ies_parameters.Add("@mode", "add");
                    ies.Fn_ExecutiveSql("USP_FeeCreateSchedule", 1, 1);
                }
            }
        }
        Response.Redirect("Adm_Fee.aspx");
    }
    protected bool ValidInput()
    {
        bool iserror = false;
        if (tbx_start_date.Value.Trim() == "") iserror = true;
        if (tbx_end_date.Value.Trim() == "") iserror = true;        

        return iserror;

    }
}