using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Adm_My_MailBox : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void btn_sendmsg_Click(object sender, EventArgs e)
    {
        string mail_to = Convert.ToString(tbx_toaddress.Text);
        string mail_subject =  Convert.ToString(tbx_subject.Text);

        string mail_content = tbx_composetextarea.Value.ToString();
        mail_content = mail_content.Replace("&#9;\"", "\"");
        mail_content = mail_content.Replace("&#9;", "");



        string mail_errorinfo = "";
        string mail_exceptionmsg = "";
        string mail_attempt = "1";
        string is_attachment = "0";
        string mail_attachmentpathlist = "";
        string mail_sentby = "1";


        

        string str_qry = "insert into tbl_email_details(mail_to,mail_subject,mail_content,mail_delivered_on,mail_status,mail_errorinfo,mail_exceptionmsg,mail_attempt,is_attachment,mail_attachmentpathlist,mail_sentby)values(@mail_to,@mail_subject,@mail_content,getdate(),@mail_status,@mail_errorinfo,@mail_exceptionmsg,@mail_attempt,@is_attachment,@mail_attachmentpathlist,@mail_sentby)";
        IES_Generic_Function ies = new IES_Generic_Function();
        ies.ies_parameters.Clear();
        ies.ies_parameters.Add("@mail_to", mail_to);
        ies.ies_parameters.Add("@mail_subject", mail_subject);
        ies.ies_parameters.Add("@mail_content", mail_content);
        ies.ies_parameters.Add("@mail_status", "1");
        ies.ies_parameters.Add("@mail_errorinfo", mail_errorinfo);
        ies.ies_parameters.Add("@mail_exceptionmsg", mail_exceptionmsg);
        ies.ies_parameters.Add("@mail_attempt", mail_attempt);
        ies.ies_parameters.Add("@is_attachment", is_attachment);
        ies.ies_parameters.Add("@mail_attachmentpathlist", mail_attachmentpathlist);
        ies.ies_parameters.Add("@mail_sentby", mail_sentby);

        ies.Fn_ExecutiveSql(str_qry, 0, 1);

        tbx_subject.Text = "";
        tbx_toaddress.Text = "";
        tbx_composetextarea.Value = ""; 

    }
}