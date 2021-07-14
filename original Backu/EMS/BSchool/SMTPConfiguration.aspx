<%@ Page Title="" Language="C#" MasterPageFile="~/ServerConfiguration.master" AutoEventWireup="true" CodeFile="SMTPConfiguration.aspx.cs" Inherits="SMTPConfiguration" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <div runat="server" id="search_section">

    <div class="cont_right formWrapper">
        <h1>Mail Server Details</h1>
        <div class="formCon">
            <div class="formConInner">
                <h3>SMTP Server details</h3>
                <table style="width: 100%" border="0" cellspacing="0" cellpadding="0" class="bottom-tbl">
                    <tbody>
                        <tr>
                            <td colspan="2">
                                <a class="fa fa-cogs" style="font-weight: 500;font-size: 14px;text-decoration: none;color: #F60;" href="SMTPConfiguration.aspx?mode=add">&nbsp;Configure SMTP server</a>            </td>
                        </tr>
                        <tr>
                            <td style="width: 80px">
                                <label>
                                    Host             
                                </label>
                            </td>
                            <td style="width: 300px">
                                <label id="lbl_hostip" runat="server">-</label>
                                 </td>
                        </tr>
                        <tr>
                            <td>
                                <label>
                                    Username             
                                </label>
                            </td>
                            <td><label id="lbl_username" runat="server">-</label></td>
                        </tr>
                        <tr>
                            <td>
                                <label>
                                    Password             
                                </label>
                            </td>
                            <td><label id="lbl_pwd" runat="server">-</label>                               
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label>
                                    Port                                                   
                                </label>
                            </td>
                            <td><label id="lbl_port" runat="server">-</label></td>
                        </tr>

                        <tr>
                            <td>
                                <label>
                                    Connection Security                 
                                </label>
                            </td>
                            <td><label id="lbl_ssltsl" runat="server">-</label>                             
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label>
                                    Status                 
                                </label>
                            </td>
                            <td><label id="lbl_smtpenable" runat="server">-</label>    </td>
                        </tr>
                    </tbody>
                </table>
            </div>
        </div>
    </div>

        </div>
     <div id="add_section" runat="server">

    <div class="cont_right formWrapper">
        <h1><b>Configure SMTP Server</b></h1>
        <p class="note">Fields with <span class="required">*</span>are required.</p>
        <div class="formCon">
            <div class="formConInner">
                <h3>SMTP Server details</h3>
                <table style="width: 100%" border="0" cellspacing="0" cellpadding="0">
                    <tbody>
                        <tr>
                            <td style="width: 80px">
                                <label for="MailConfig_host" class="required">Host <span class="required">*</span></label>
                            </td>
                            <td style="width: 300px">
                                <asp:TextBox ID="tbx_host" runat="server" MaxLength="255" Width="300px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">&nbsp;</td>
                        </tr>
                        <tr>
                            <td>
                                <label for="MailConfig_username" class="required">Username <span class="required">*</span></label>
                            </td>
                            <td>
                                <asp:TextBox ID="tbx_username" runat="server" MaxLength="255" Width="300px"></asp:TextBox>

                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">&nbsp;</td>
                        </tr>
                        <tr>
                            <td>
                                <label for="MailConfig_password" class="required">Password <span class="required">*</span></label>
                            </td>
                            <td>
                                <asp:TextBox ID="tbx_pwd" runat="server" MaxLength="255" Width="300px"></asp:TextBox>

                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">&nbsp;</td>
                        </tr>
                        <tr>
                            <td>
                                <label for="MailConfig_port" class="required">Port <span class="required">*</span></label>
                            </td>
                            <td>
                                <asp:TextBox ID="tbx_port" runat="server" MaxLength="255"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">&nbsp;</td>
                        </tr>
                        <tr>
                            <td>
                                <label for="MailConfig_security" class="required">Connection Security <span class="required">*</span></label>
                            </td>
                            <td>
                                <asp:RadioButton ID="rd_tls" GroupName="consecurity" runat="server" />
                                <label style="display: inline;">TLS</label>
                                <asp:RadioButton ID="rd_ssl" GroupName="consecurity" runat="server" />
                                <label style="display: inline;">SSL</label>

                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">&nbsp;</td>
                        </tr>
                        <tr>
                            <td>
                                <label>Enable SMTP <span class="required">*</span></label>
                            </td>
                            <td>
                                <asp:RadioButton ID="rd_yes" GroupName="smtp" runat="server" />
                                <label style="display: inline;">Yes</label>
                                <asp:RadioButton ID="rd_no" GroupName="smtp" runat="server" />
                                <label style="display: inline;">No</label>
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
            <asp:Button ID="btn_save" ValidationGroup="vd_save" runat="server" Text="Save" OnClick="btn_save_Click" Width="150px" CssClass="a_tag-btn" />
        </div>

    </div>
         </div>
</asp:Content>

