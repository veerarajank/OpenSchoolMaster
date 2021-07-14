using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class NotificationSettings : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["Usrid"] == null)
        {
            Response.Redirect("login.aspx");
        }
        if (!IsPostBack)
        {
            BindListView();
        }
    }
    protected void BindListView()
    {
        IES_Generic_Function ies = new IES_Generic_Function();
        ies.ies_parameters.Clear();
        DataTable dt_data = ies.Fn_ExecutiveSql_Datatable("select * from tbl_notification_settings", 0, 0);
        lv_notificationsettings.DataSource = dt_data;
        lv_notificationsettings.DataBind();
    }
    protected void chk_sms_CheckedChanged(object sender, EventArgs e)
    {
        bool sms = false;
        CheckBox chk_sms = (CheckBox)sender;
        if (chk_sms.Checked == true) sms = true;
        for (int iparent = 0; iparent <= lv_notificationsettings.Items.Count - 1; iparent++)
        {
            CheckBox chk_sms_add = (CheckBox)lv_notificationsettings.Items[iparent].FindControl("chk_sms_add");
            chk_sms_add.Checked = sms;
        }
    }
    protected void chk_email_CheckedChanged(object sender, EventArgs e)
    {
        bool email = false;
        CheckBox chk_email = (CheckBox)sender;
        if (chk_email.Checked == true) email = true;
        for (int iparent = 0; iparent <= lv_notificationsettings.Items.Count - 1; iparent++)
        {
            CheckBox chk_email_add = (CheckBox)lv_notificationsettings.Items[iparent].FindControl("chk_email_add");
            chk_email_add.Checked = email;
        }
    }
    protected void chk_msg_CheckedChanged(object sender, EventArgs e)
    {
        bool msg = false;
        CheckBox chk_msg = (CheckBox)sender;
        if (chk_msg.Checked == true) msg = true;
        for (int iparent = 0; iparent <= lv_notificationsettings.Items.Count - 1; iparent++)
        {
            CheckBox chk_msg_add = (CheckBox)lv_notificationsettings.Items[iparent].FindControl("chk_msg_add");
            chk_msg_add.Checked = msg;
        }
    }
    protected void chk_student_CheckedChanged(object sender, EventArgs e)
    {
        bool student = false;
        CheckBox chk_student = (CheckBox)sender;
        if (chk_student.Checked == true) student = true;
        for (int iparent = 0; iparent <= lv_notificationsettings.Items.Count - 1; iparent++)
        {
            CheckBox chk_student_add = (CheckBox)lv_notificationsettings.Items[iparent].FindControl("chk_student_add");
            chk_student_add.Checked = student;
        }
    }
    protected void chk_guardian_CheckedChanged(object sender, EventArgs e)
    {
        bool guardian = false;
        CheckBox chk_guardian = (CheckBox)sender;
        if (chk_guardian.Checked == true) guardian = true;
        for (int iparent = 0; iparent <= lv_notificationsettings.Items.Count - 1; iparent++)
        {
            CheckBox chk_guardian_add = (CheckBox)lv_notificationsettings.Items[iparent].FindControl("chk_guardian_add");
            chk_guardian_add.Checked = guardian;
        }
    }
    protected void chk_teacher_CheckedChanged(object sender, EventArgs e)
    {
        bool teacher = false;
        CheckBox chk_teacher = (CheckBox)sender;
        if (chk_teacher.Checked == true) teacher = true;
        for (int iparent = 0; iparent <= lv_notificationsettings.Items.Count - 1; iparent++)
        {
            CheckBox chk_teacher_add = (CheckBox)lv_notificationsettings.Items[iparent].FindControl("chk_teacher_add");
            chk_teacher_add.Checked = teacher;
        }
    }

    protected void btn_save_Click(object sender, EventArgs e)
    {
        for (int iparent = 0; iparent <= lv_notificationsettings.Items.Count - 1; iparent++)
        {
            CheckBox chk_sms_add = (CheckBox)lv_notificationsettings.Items[iparent].FindControl("chk_sms_add");
            CheckBox chk_email_add = (CheckBox)lv_notificationsettings.Items[iparent].FindControl("chk_email_add");
            CheckBox chk_msg_add = (CheckBox)lv_notificationsettings.Items[iparent].FindControl("chk_msg_add");
            CheckBox chk_student_add = (CheckBox)lv_notificationsettings.Items[iparent].FindControl("chk_student_add");
            CheckBox chk_guardian_add = (CheckBox)lv_notificationsettings.Items[iparent].FindControl("chk_guardian_add");
            CheckBox chk_teacher_add = (CheckBox)lv_notificationsettings.Items[iparent].FindControl("chk_teacher_add");
            HiddenField hd_notifiyid = (HiddenField)lv_notificationsettings.Items[iparent].FindControl("hd_notifiyid");

            string sms_enable = "0";
            if (chk_sms_add.Checked) sms_enable = "1";

            string email_enable = "0";
            if (chk_email_add.Checked) email_enable = "1";

            string msg_enable = "0";
            if (chk_msg_add.Checked) msg_enable = "1";

            string student_enable = "0";
            if (chk_student_add.Checked) student_enable = "1";

            string guardian_enable = "0";
            if (chk_guardian_add.Checked) guardian_enable = "1";

            string teacher_enable = "0";
            if (chk_teacher_add.Checked) teacher_enable = "1";

            string notifyid = Convert.ToString(hd_notifiyid.Value);



            string str_qry = @"update tbl_notification_settings set sms_enable=@sms_enable,email_enable=@email_enable,
                    msg_enable=@msg_enable,student_enable=@student_enable,guardian_enable=@guardian_enable,teacher_enable=@teacher_enable where notifyid=@notifyid";
        
            IES_Generic_Function ies = new IES_Generic_Function();
            ies.ies_parameters.Clear();
            ies.ies_parameters.Add("@sms_enable", sms_enable);
            ies.ies_parameters.Add("@email_enable", email_enable);
            ies.ies_parameters.Add("@msg_enable", msg_enable);
            ies.ies_parameters.Add("@student_enable", student_enable);
            ies.ies_parameters.Add("@guardian_enable", guardian_enable);
            ies.ies_parameters.Add("@teacher_enable", teacher_enable);
            ies.ies_parameters.Add("@notifyid", notifyid);
            ies.Fn_ExecutiveSql(str_qry, 0, 1);

        }
        BindListView();
    }
}