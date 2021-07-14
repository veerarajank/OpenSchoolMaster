using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data;

public partial class Adm_CreateFees : System.Web.UI.Page
{
    static int ATypeId = 0;
    static int PId = 0;
    public List<Fees_Particulars> Lst_Fees_Particulars
    {
        get
        {
            if (ViewState["Fees_Particulars"] == null) ViewState["Fees_Particulars"] = new List<Fees_Particulars>();
            return (List<Fees_Particulars>)ViewState["Fees_Particulars"];
        }
        set
        {
            ViewState["Fees_Particulars"] = this;
        }
    }

    [Serializable]
    public class Fees_Particulars
    {
        public int PId { get; set; }
        public string PName { get; set; }
        public string PDesc { get; set; }
        public string PTax { get; set; }
        public string PDiscount { get; set; }
        public string PDiscounttype { get; set; }
        public List<Fees_ApplicableTo> fees_applicable { get; set; }
    }
    [Serializable]
    public class Fees_ApplicableTo
    {
        public int PId { get; set; }
        public int ATypeId { get; set; }
        public string AType { get; set; }
        public string ACourse { get; set; }
        public string ABatch { get; set; }
        public string ACategory { get; set; }
        public string AApplicationNo { get; set; }

        public string Aamount { get; set; }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["Usrid"] == null)
        {
            Response.Redirect("login.aspx");
        }
        if (!IsPostBack)
        {

            int ActualPId = PId++;

            Fees_Particulars fees_partiular = new Fees_Particulars();
            fees_partiular.PId = ActualPId;
            fees_partiular.fees_applicable = new List<Fees_ApplicableTo>();

            fees_partiular.PName = "";


            Fees_ApplicableTo fees_applicable = new Fees_ApplicableTo();
            fees_applicable.ATypeId = ATypeId++;
            fees_applicable.PId = ActualPId;
            fees_partiular.fees_applicable.Add(fees_applicable);

            Lst_Fees_Particulars.Add(fees_partiular);

            lv_particulatars.DataSource = Lst_Fees_Particulars;
            lv_particulatars.DataBind();
           
        }
    }
    protected void OnItemDataBound(object sender, ListViewItemEventArgs e)
    {
        ListViewDataItem dataItem = (ListViewDataItem)e.Item;
        if (dataItem != null)
        {
            DropDownList drp_taxmaster = (DropDownList)dataItem.FindControl("drp_taxmaster");
            if (drp_taxmaster != null)
            {
                IES_Generic_Function ies = new IES_Generic_Function();
                ies = new IES_Generic_Function();
                ies.Fn_Executive_ListWithSelect_WithTitle(drp_taxmaster, "No Tax", "select id, name from tbl_taxmaster	order by name", "id", "Name");
            }

            HiddenField hd_mainpid = (HiddenField)dataItem.FindControl("hd_mainpid");
            Fees_Particulars lst_exist = Lst_Fees_Particulars.Find(p => p.PId == Convert.ToInt32(hd_mainpid.Value));
            if (lst_exist != null)
            {
                HtmlInputText tbx_particularname = (HtmlInputText)dataItem.FindControl("tbx_particularname");
                HtmlInputText tbx_particulardescription = (HtmlInputText)dataItem.FindControl("tbx_particulardescription");
                HtmlInputText tbx_discountvalue = (HtmlInputText)dataItem.FindControl("tbx_discountvalue");
                DropDownList drp_discounttype = (DropDownList)dataItem.FindControl("drp_discounttype");

                tbx_discountvalue.Value = lst_exist.PDiscount;
                tbx_particularname.Value = lst_exist.PName;
                tbx_particulardescription.Value = lst_exist.PDesc;
                drp_discounttype.SelectedValue = lst_exist.PDiscounttype;
                drp_taxmaster.SelectedValue = lst_exist.PTax;


            }

        }
    }
    protected void drp_accesstype_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList drp_accesstype = (DropDownList)sender;
        ListViewItem item1 = (ListViewItem)drp_accesstype.NamingContainer;


        HtmlTableRow tr_admisionno = (HtmlTableRow)item1.FindControl("tr_admisionno");
        HtmlTableRow tr_default = (HtmlTableRow)item1.FindControl("tr_default");



        tr_admisionno.Visible = false;
        tr_default.Visible = false;

        if (drp_accesstype.SelectedValue == "Default")
        {
            tr_default.Visible = true;
        }
        if (drp_accesstype.SelectedValue == "Admission Number")
        {
            tr_admisionno.Visible = true;
        }


    }
    protected void drp_course_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList drp_course = (DropDownList)sender;
        ListViewItem item1 = (ListViewItem)drp_course.NamingContainer;

        DropDownList drp_batches = (DropDownList)item1.FindControl("drp_batches");
        if (drp_batches != null)
        {
            drp_batches.Items.Clear();
            IES_Generic_Function ies = new IES_Generic_Function();
            ies.Fn_Executive_ListWithSelect_WithTitle(drp_batches, "--Select Batch--", "select id, name from tbl_batches where course_id='" + Convert.ToString(drp_course.SelectedValue) + "' order by name", "id", "Name");
        }
    }


    protected void btn_save_ServerClick(object sender, EventArgs e)
    {
        if (ValidInput() == true) { field_mandary.Attributes.Add("style", "color:red"); return; }

        Add_SingleFee();

        IES_Generic_Function ies = new IES_Generic_Function();
        ies.ies_parameters.Clear();

        string name = Convert.ToString(tbx_name.Value);
        string fees_description = Convert.ToString(tbx_description.Value);
        ies.ies_parameters.Add("@name", Convert.ToString(name));
        ies.ies_parameters.Add("@fees_description", Convert.ToString(fees_description));
        ies.ies_parameters.Add("@created_by", Convert.ToString(Session["Usrid"]));
        ies.ies_parameters.Add("@mode", "add");
        DataTable dt_result = ies.Fn_ExecutiveSql_Datatable("UDP_FeeCategory", 1, 1);
        if (dt_result != null && dt_result.Rows.Count > 0)
        {
            if (Convert.ToString(dt_result.Rows[0]["Error"]) == "0")
            {

                string category_id = Convert.ToString(dt_result.Rows[0]["id"]);

             
                for (int irow = 0; irow <= Lst_Fees_Particulars.Count-1; irow++)
                {
                    ies = new IES_Generic_Function();
                    ies.ies_parameters.Clear();
                    ies.ies_parameters.Add("@fee_id", Convert.ToString(category_id));
                    ies.ies_parameters.Add("@item_name", Convert.ToString(Lst_Fees_Particulars[irow].PName));
                    ies.ies_parameters.Add("@item_description", Convert.ToString(Lst_Fees_Particulars[irow].PDesc));
                    ies.ies_parameters.Add("@tax_id", Convert.ToString(Lst_Fees_Particulars[irow].PTax));
                    ies.ies_parameters.Add("@tax_value", Convert.ToString(0));
                    ies.ies_parameters.Add("@discount_value", Convert.ToString(Lst_Fees_Particulars[irow].PDiscount));
                    ies.ies_parameters.Add("@discount_type", Convert.ToString(Lst_Fees_Particulars[irow].PDiscounttype));
                    DataTable dt_resultparticular = ies.Fn_ExecutiveSql_Datatable("UDP_FeeParticular", 1, 1);

                    string itemid = Convert.ToString(dt_resultparticular.Rows[0]["id"]);

                    for (int isub = 0; isub <= Lst_Fees_Particulars[irow].fees_applicable.Count - 1; isub++)
                    {
                        ies = new IES_Generic_Function();
                        ies.ies_parameters.Clear();
                        ies.ies_parameters.Add("@fee_id", Convert.ToString(category_id));
                        ies.ies_parameters.Add("@item_id", Convert.ToString(itemid));
                        ies.ies_parameters.Add("@applicable_type", Convert.ToString(Lst_Fees_Particulars[irow].fees_applicable[isub].AType));
                        ies.ies_parameters.Add("@course_id", Convert.ToString(Lst_Fees_Particulars[irow].fees_applicable[isub].ACourse));
                        ies.ies_parameters.Add("@batch_id", Convert.ToString(Lst_Fees_Particulars[irow].fees_applicable[isub].ABatch));
                        ies.ies_parameters.Add("@category_id", Convert.ToString(Lst_Fees_Particulars[irow].fees_applicable[isub].ACategory));
                        ies.ies_parameters.Add("@application_no", Convert.ToString(Lst_Fees_Particulars[irow].fees_applicable[isub].AApplicationNo));
                        ies.ies_parameters.Add("@amount", Convert.ToString(Lst_Fees_Particulars[irow].fees_applicable[isub].Aamount));

                        ies.ies_parameters.Add("@item_name", Convert.ToString(Lst_Fees_Particulars[irow].PName));
                        ies.ies_parameters.Add("@item_description", Convert.ToString(Lst_Fees_Particulars[irow].PDesc));
                        ies.ies_parameters.Add("@tax_id", Convert.ToString(Lst_Fees_Particulars[irow].PTax));
                        ies.ies_parameters.Add("@tax_value", Convert.ToString(0));
                        ies.ies_parameters.Add("@discount_value", Convert.ToString(Lst_Fees_Particulars[irow].PDiscount));
                        ies.ies_parameters.Add("@discount_type", Convert.ToString(Lst_Fees_Particulars[irow].PDiscounttype));

                        ies.Fn_ExecutiveSql("UDP_FeeApplicableTo", 1, 1);
                    }

                    
                }

                Response.Redirect("Adm_Fee.aspx");
            }
            

        }
    }
    protected bool ValidInput()
    {
        bool iserror = false;
        if (tbx_name.Value.Trim() == "") iserror = true;
        if (tbx_description.Value.Trim() == "") iserror = true;

        return iserror;

    }

    protected void lnk_del_appliacable_ServerClick(object sender, EventArgs e)
    {
        HtmlAnchor lnk_del_appliacable = (HtmlAnchor)sender;
        ListViewItem item1 = (ListViewItem)lnk_del_appliacable.NamingContainer;

        HiddenField hd_feesid = (HiddenField)item1.FindControl("hd_feesid");
        HiddenField hd_pid = (HiddenField)item1.FindControl("hd_pid");
        if (hd_feesid != null)
        {
            Fees_Particulars lst_exist = Lst_Fees_Particulars.Find(p => p.PId == Convert.ToInt32(hd_pid.Value));
            if (lst_exist != null)
            {
                Fees_ApplicableTo lst_exists_sub = lst_exist.fees_applicable.Find(p => p.ATypeId == Convert.ToInt32(hd_feesid.Value));
                if (lst_exists_sub != null)
                {
                    lst_exist.fees_applicable.Remove(lst_exists_sub);
                    lv_particulatars.DataSource = Lst_Fees_Particulars;
                    lv_particulatars.DataBind();
                }
            }

        }

    }
    protected void lnk_del_ServerClick(object sender, EventArgs e)
    {
        HtmlAnchor lnk_del = (HtmlAnchor)sender;
        ListViewItem item1 = (ListViewItem)lnk_del.NamingContainer;

        HiddenField hd_mainpid = (HiddenField)item1.FindControl("hd_mainpid");
        if (hd_mainpid != null)
        {
            Fees_Particulars lst_exist = Lst_Fees_Particulars.Find(p => p.PId == Convert.ToInt32(hd_mainpid.Value));
            if (lst_exist != null)
            {
                Lst_Fees_Particulars.Remove(lst_exist);

                lv_particulatars.DataSource = Lst_Fees_Particulars;
                lv_particulatars.DataBind();

            }

        }

    }
    protected void lnk_add_particulars_ServerClick(object sender, EventArgs e)
    {
        HtmlAnchor lnk_add_particulars = (HtmlAnchor)sender;
        ListViewItem item1 = (ListViewItem)lnk_add_particulars.NamingContainer;


        HiddenField hd_mainpid = (HiddenField)item1.FindControl("hd_mainpid");
        if (hd_mainpid != null)
        {
            HtmlInputText tbx_particularname = (HtmlInputText)item1.FindControl("tbx_particularname");
            HtmlInputText tbx_particulardescription = (HtmlInputText)item1.FindControl("tbx_particulardescription");
            DropDownList drp_taxmaster = (DropDownList)item1.FindControl("drp_taxmaster");
            HtmlInputText tbx_discountvalue = (HtmlInputText)item1.FindControl("tbx_discountvalue");
            DropDownList drp_discounttype = (DropDownList)item1.FindControl("drp_discounttype");


            Fees_Particulars lst_exist = Lst_Fees_Particulars.Find(p => p.PId == Convert.ToInt32(hd_mainpid.Value));
            if (lst_exist != null)
            {


                lst_exist.PName = tbx_particularname.Value;
                lst_exist.PDesc = tbx_particulardescription.Value;
                lst_exist.PDiscount = tbx_discountvalue.Value;
                lst_exist.PDiscounttype = Convert.ToString(drp_discounttype.SelectedValue);
                lst_exist.PTax = Convert.ToString(drp_taxmaster.SelectedValue);


                ListView lv_applicableto = (ListView)item1.FindControl("lv_applicableto");
                foreach (ListViewItem itemRow in lv_applicableto.Items)
                {

                    DropDownList drp_accesstype = (DropDownList)itemRow.FindControl("drp_accesstype");
                    DropDownList drp_course = (DropDownList)itemRow.FindControl("drp_course");
                    DropDownList drp_batches = (DropDownList)itemRow.FindControl("drp_batches");
                    DropDownList drp_category = (DropDownList)itemRow.FindControl("drp_category");

                    HtmlInputText tbx_admisionno = (HtmlInputText)itemRow.FindControl("tbx_admisionno");
                    HtmlInputText tbx_amount = (HtmlInputText)itemRow.FindControl("tbx_amount");

                    HiddenField hd_feesid = (HiddenField)itemRow.FindControl("hd_feesid");
                    HiddenField hd_pid = (HiddenField)itemRow.FindControl("hd_pid");

                    Fees_ApplicableTo lst_exist_data = lst_exist.fees_applicable.Find(p => p.PId == Convert.ToInt32(hd_pid.Value) && p.ATypeId == Convert.ToInt32(hd_feesid.Value));
                    if (lst_exist_data != null)
                    {

                        lst_exist_data.ATypeId = Convert.ToInt32(hd_feesid.Value);
                        lst_exist_data.PId = Convert.ToInt32(hd_mainpid.Value);

                        lst_exist_data.AType = Convert.ToString(drp_accesstype.SelectedValue);
                        lst_exist_data.ACourse = Convert.ToString(drp_course.SelectedValue);
                        lst_exist_data.ABatch = Convert.ToString(drp_batches.SelectedValue);
                        lst_exist_data.ACategory = Convert.ToString(drp_category.SelectedValue);
                        lst_exist_data.Aamount = Convert.ToString(tbx_amount.Value);
                        lst_exist_data.AApplicationNo = Convert.ToString(tbx_admisionno.Value);

                        lst_exist_data.AType = Convert.ToString(drp_accesstype.SelectedValue);
                        lst_exist_data.ACourse = Convert.ToString(drp_course.SelectedValue);
                        lst_exist_data.ABatch = Convert.ToString(drp_batches.SelectedValue);
                        lst_exist_data.ACategory = Convert.ToString(drp_category.SelectedValue);
                        lst_exist_data.Aamount = Convert.ToString(tbx_amount.Value);
                        lst_exist_data.AApplicationNo = Convert.ToString(tbx_admisionno.Value);

                    }
                   
                }








                Fees_ApplicableTo fees_applicable = new Fees_ApplicableTo();
                fees_applicable.ATypeId = ATypeId++;
                fees_applicable.PId = Convert.ToInt32(hd_mainpid.Value);
                lst_exist.fees_applicable.Add(fees_applicable);
                
                lv_particulatars.DataSource = Lst_Fees_Particulars;
                lv_particulatars.DataBind();

            }
        }
    }
    protected void lv_applicableto_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        ListViewDataItem dataItem = (ListViewDataItem)e.Item;
        if (dataItem != null)
        {
            DropDownList drp_course = (DropDownList)dataItem.FindControl("drp_course");
            DropDownList drp_category = (DropDownList)dataItem.FindControl("drp_category");
            if (drp_course != null)
            {
                IES_Generic_Function ies = new IES_Generic_Function();
                ies = new IES_Generic_Function();
                ies.Fn_Executive_ListWithSelect_WithTitle(drp_course, "--Select course--", "select id, name from tbl_courses	order by name", "id", "Name");
                ies.Fn_Executive_ListWithSelect_WithTitle(drp_category, "--Select Category--", "select id, name from tbl_studentcategories	order by name", "id", "Name");
            }


            DropDownList drp_accesstype = (DropDownList)dataItem.FindControl("drp_accesstype");          
            DropDownList drp_batches = (DropDownList)dataItem.FindControl("drp_batches");
            HtmlInputText tbx_admisionno = (HtmlInputText)dataItem.FindControl("tbx_admisionno");
            HtmlInputText tbx_amount = (HtmlInputText)dataItem.FindControl("tbx_amount");

            HiddenField hd_feesid = (HiddenField)dataItem.FindControl("hd_feesid");
            HiddenField hd_pid = (HiddenField)dataItem.FindControl("hd_pid");

            Fees_Particulars lst_exist = Lst_Fees_Particulars.Find(p => p.PId == Convert.ToInt32(hd_pid.Value));
             if (lst_exist != null)
             {

                 Fees_ApplicableTo lst_exist_data = lst_exist.fees_applicable.Find(p => p.PId == Convert.ToInt32(hd_pid.Value) && p.ATypeId == Convert.ToInt32(hd_feesid.Value));
                 if (lst_exist_data != null)
                 {

                     HtmlTableRow tr_admisionno = (HtmlTableRow)dataItem.FindControl("tr_admisionno");
                     HtmlTableRow tr_default = (HtmlTableRow)dataItem.FindControl("tr_default");



                     tr_admisionno.Visible = false;
                     tr_default.Visible = true;
                     drp_accesstype.SelectedValue = lst_exist_data.AType;
                     if (lst_exist_data.AType == "Default")
                     {
                         tr_default.Visible = true;
                         drp_course.SelectedValue = lst_exist_data.ACourse;
                         if (drp_batches != null)
                         {
                             drp_batches.Items.Clear();
                             IES_Generic_Function ies = new IES_Generic_Function();
                             ies.Fn_Executive_ListWithSelect_WithTitle(drp_batches, "--Select Batch--", "select id, name from tbl_batches where course_id='" + Convert.ToString(drp_course.SelectedValue) + "' order by name", "id", "Name");
                         }
                         drp_batches.SelectedValue=lst_exist_data.ABatch;
                         drp_category.SelectedValue=lst_exist_data.ACategory;
                     }

                     if (lst_exist_data.AType == "Admission Number")
                     {
                         tr_admisionno.Visible = true;
                         tr_default.Visible = false;
                         tbx_admisionno.Value = lst_exist_data.AApplicationNo;
                     }                 

                 }
             }

           

        }
    }
    protected void add_fee_particular_ServerClick(object sender, EventArgs e)
    {
        foreach (ListViewItem itemRow in lv_particulatars.Items)
        {
            HiddenField hd_mainpid = (HiddenField)itemRow.FindControl("hd_mainpid");
            if (hd_mainpid != null)
            {
                HtmlInputText tbx_particularname = (HtmlInputText)itemRow.FindControl("tbx_particularname");
                HtmlInputText tbx_particulardescription = (HtmlInputText)itemRow.FindControl("tbx_particulardescription");
                DropDownList drp_taxmaster = (DropDownList)itemRow.FindControl("drp_taxmaster");
                HtmlInputText tbx_discountvalue = (HtmlInputText)itemRow.FindControl("tbx_discountvalue");
                DropDownList drp_discounttype = (DropDownList)itemRow.FindControl("drp_discounttype");
                Fees_Particulars lst_exist = Lst_Fees_Particulars.Find(p => p.PId == Convert.ToInt32(hd_mainpid.Value));
                if (lst_exist != null)
                {
                    lst_exist.PName = tbx_particularname.Value;
                    lst_exist.PDesc = tbx_particulardescription.Value;
                    lst_exist.PDiscount = tbx_discountvalue.Value;
                    lst_exist.PDiscounttype = Convert.ToString(drp_discounttype.SelectedValue);
                    lst_exist.PTax = Convert.ToString(drp_taxmaster.SelectedValue);


                    ListView lv_applicableto = (ListView)itemRow.FindControl("lv_applicableto");
                    foreach (ListViewItem itemRow2 in lv_applicableto.Items)
                    {

                        DropDownList drp_accesstype = (DropDownList)itemRow2.FindControl("drp_accesstype");
                        DropDownList drp_course = (DropDownList)itemRow2.FindControl("drp_course");
                        DropDownList drp_batches = (DropDownList)itemRow2.FindControl("drp_batches");
                        DropDownList drp_category = (DropDownList)itemRow2.FindControl("drp_category");

                        HtmlInputText tbx_admisionno = (HtmlInputText)itemRow2.FindControl("tbx_admisionno");
                        HtmlInputText tbx_amount = (HtmlInputText)itemRow2.FindControl("tbx_amount");

                        HiddenField hd_feesid = (HiddenField)itemRow2.FindControl("hd_feesid");
                        HiddenField hd_pid = (HiddenField)itemRow2.FindControl("hd_pid");

                        Fees_ApplicableTo lst_exist_data = lst_exist.fees_applicable.Find(p => p.PId == Convert.ToInt32(hd_pid.Value) && p.ATypeId == Convert.ToInt32(hd_feesid.Value));
                        if (lst_exist_data != null)
                        {

                            lst_exist_data.ATypeId = Convert.ToInt32(hd_feesid.Value);
                            lst_exist_data.PId = Convert.ToInt32(hd_mainpid.Value);

                            lst_exist_data.AType = Convert.ToString(drp_accesstype.SelectedValue);
                            lst_exist_data.ACourse = Convert.ToString(drp_course.SelectedValue);
                            lst_exist_data.ABatch = Convert.ToString(drp_batches.SelectedValue);
                            lst_exist_data.ACategory = Convert.ToString(drp_category.SelectedValue);
                            lst_exist_data.Aamount = Convert.ToString(tbx_amount.Value);
                            lst_exist_data.AApplicationNo = Convert.ToString(tbx_admisionno.Value);

                            lst_exist_data.AType = Convert.ToString(drp_accesstype.SelectedValue);
                            lst_exist_data.ACourse = Convert.ToString(drp_course.SelectedValue);
                            lst_exist_data.ABatch = Convert.ToString(drp_batches.SelectedValue);
                            lst_exist_data.ACategory = Convert.ToString(drp_category.SelectedValue);
                            lst_exist_data.Aamount = Convert.ToString(tbx_amount.Value);
                            lst_exist_data.AApplicationNo = Convert.ToString(tbx_admisionno.Value);

                        }

                    }

                }
            }
        }

        int ActualPId = PId++;
        Fees_Particulars fees_partiular = new Fees_Particulars();
        fees_partiular.PId = ActualPId;
        fees_partiular.fees_applicable = new List<Fees_ApplicableTo>();
        fees_partiular.PName = "";
        Fees_ApplicableTo fees_applicable = new Fees_ApplicableTo();
        fees_applicable.ATypeId = ATypeId++;
        fees_applicable.PId = ActualPId;
        fees_partiular.fees_applicable.Add(fees_applicable);
        Lst_Fees_Particulars.Add(fees_partiular);
        lv_particulatars.DataSource = Lst_Fees_Particulars;
        lv_particulatars.DataBind();

    }

    protected void Add_SingleFee()
    {
        foreach (ListViewItem item1 in lv_particulatars.Items)
        {

            HiddenField hd_mainpid = (HiddenField)item1.FindControl("hd_mainpid");
            if (hd_mainpid != null)
            {
                HtmlInputText tbx_particularname = (HtmlInputText)item1.FindControl("tbx_particularname");
                HtmlInputText tbx_particulardescription = (HtmlInputText)item1.FindControl("tbx_particulardescription");
                DropDownList drp_taxmaster = (DropDownList)item1.FindControl("drp_taxmaster");
                HtmlInputText tbx_discountvalue = (HtmlInputText)item1.FindControl("tbx_discountvalue");
                DropDownList drp_discounttype = (DropDownList)item1.FindControl("drp_discounttype");


                Fees_Particulars lst_exist = Lst_Fees_Particulars.Find(p => p.PId == Convert.ToInt32(hd_mainpid.Value));
                if (lst_exist != null)
                {


                    lst_exist.PName = tbx_particularname.Value;
                    lst_exist.PDesc = tbx_particulardescription.Value;
                    lst_exist.PDiscount = tbx_discountvalue.Value;
                    lst_exist.PDiscounttype = Convert.ToString(drp_discounttype.SelectedValue);
                    lst_exist.PTax = Convert.ToString(drp_taxmaster.SelectedValue);


                    ListView lv_applicableto = (ListView)item1.FindControl("lv_applicableto");
                    foreach (ListViewItem itemRow in lv_applicableto.Items)
                    {

                        DropDownList drp_accesstype = (DropDownList)itemRow.FindControl("drp_accesstype");
                        DropDownList drp_course = (DropDownList)itemRow.FindControl("drp_course");
                        DropDownList drp_batches = (DropDownList)itemRow.FindControl("drp_batches");
                        DropDownList drp_category = (DropDownList)itemRow.FindControl("drp_category");

                        HtmlInputText tbx_admisionno = (HtmlInputText)itemRow.FindControl("tbx_admisionno");
                        HtmlInputText tbx_amount = (HtmlInputText)itemRow.FindControl("tbx_amount");

                        HiddenField hd_feesid = (HiddenField)itemRow.FindControl("hd_feesid");
                        HiddenField hd_pid = (HiddenField)itemRow.FindControl("hd_pid");

                        Fees_ApplicableTo lst_exist_data = lst_exist.fees_applicable.Find(p => p.PId == Convert.ToInt32(hd_pid.Value) && p.ATypeId == Convert.ToInt32(hd_feesid.Value));
                        if (lst_exist_data != null)
                        {

                            lst_exist_data.ATypeId = Convert.ToInt32(hd_feesid.Value);
                            lst_exist_data.PId = Convert.ToInt32(hd_mainpid.Value);

                            lst_exist_data.AType = Convert.ToString(drp_accesstype.SelectedValue);
                            lst_exist_data.ACourse = Convert.ToString(drp_course.SelectedValue);
                            lst_exist_data.ABatch = Convert.ToString(drp_batches.SelectedValue);
                            lst_exist_data.ACategory = Convert.ToString(drp_category.SelectedValue);
                            lst_exist_data.Aamount = Convert.ToString(tbx_amount.Value);
                            lst_exist_data.AApplicationNo = Convert.ToString(tbx_admisionno.Value);

                            lst_exist_data.AType = Convert.ToString(drp_accesstype.SelectedValue);
                            lst_exist_data.ACourse = Convert.ToString(drp_course.SelectedValue);
                            lst_exist_data.ABatch = Convert.ToString(drp_batches.SelectedValue);
                            lst_exist_data.ACategory = Convert.ToString(drp_category.SelectedValue);
                            lst_exist_data.Aamount = Convert.ToString(tbx_amount.Value);
                            lst_exist_data.AApplicationNo = Convert.ToString(tbx_admisionno.Value);

                        }

                    }
                }
            }
        }
    }
}