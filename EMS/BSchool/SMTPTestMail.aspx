<%@ Page Title="" Language="C#" MasterPageFile="~/ServerConfiguration.master" AutoEventWireup="true" CodeFile="SMTPTestMail.aspx.cs" Inherits="SMTPTestMail" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="cont_right formWrapper">
        <h1>Test Email</h1>
        <!-- School Information -->
        <div class="formCon">
            <div class="formConInner">
                <h3>SMTP Server details</h3>
                <table style="width:100%" border="0" cellspacing="0" cellpadding="0">
                    <tbody>
                        <tr>
                            <td style="width:80px">
                                <label class="required">Email ID <span class="required">*</span></label>
                            </td>
                            <td style="width:300px">                                
                                <asp:TextBox ID="tbx_emailid" runat="server" AutoCompleteType="Disabled" Width="300px"></asp:TextBox>
                                <div class="errorMessage" id="div_email_id_errtxt" style="display: none"></div>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">&nbsp;</td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:CheckBox ID="chk_emailattachment" runat="server" />                                
                                <label>Attach a sample file</label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">&nbsp;</td>
                        </tr>
                    </tbody>
                </table>
            </div>
        </div>
        <div style="padding: 0px 0 0 0px; text-align: left">            
            <button id="btn_sendmail" runat="server" style="width: 150px" onserverclick="btn_sendmail_ServerClick" class="a_tag-btn"><span>Send</span></button>
        </div>
    </div>
</asp:Content>

