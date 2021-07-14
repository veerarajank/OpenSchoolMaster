<%@ Page Title="" Language="C#" MasterPageFile="~/Admin_MyAccount.master" AutoEventWireup="true" CodeFile="Adm_My_MailBox.aspx.cs" Inherits="Adm_My_MailBox" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
<script src="my_plugin/datepicker/jquery-1.10.2.js"></script>
   
    <style>
        .mailbox-menu .ui-widget {
            border: none;
            padding: 0px;
        }


        .mailbox-menu-item {
            text-align: center;
            margin-right: 5px;
            margin-top: 0px;
            margin-left: 5px;
            float: left;
            font-weight: bold;
            cursor: pointer;
            text-align: left;
            border-right: 1px #cbcbcb solid !important;
            background: none !important;
        }

        ui-corner-all {
            border: 0px solid #ccc !important;
        }

        .ui-state-default {
            background: none !important;
            border: 0px solid !important;
        }

        .ui-widget-header {
            background: #363B45 !important;
            border: 0 solid #aaa !important;
            color: #222;
            font-weight: bold;
        }

        .ui-corner-all {
            border-radius: 0px !important;
        }

        .ui-widget-header {
            background: none;
            border: none;
            font-size: 13px;
            border-bottom: 1px #d1d1d1 dashed;
            -moz-border-radius: 0px;
            -webkit-border-radius: 0px;
            border-radius: 0px;
        }
    </style>
    <div class="cont_right formWrapper" style="padding: 0px; width: 753px;">
        <div id="parent_rightSect">
            <div class="parentright_innercon">
                <div class="mail_head" style="height: 75px;">Mailbox<span>Check your mails here.</span></div>
                <table style="width: 100%" border="0" cellspacing="0" cellpadding="0">
                    <tbody>
                        <tr>
                            <td valign="top" style="width: 75%">
                                <div class="mailbox-menu  ui-helper-clearfix" style="height: 45px;">
                                    <div class="mailbox-menu-folders ui-helper-clearfix  ui-widget ui-widget-content ui-corner-all">
                                        <div id="mailbox-inbox" class="mailbox-menu-item mailbox-menu-current ui-button ui-widget ui-state-default ui-corner-all ui-button-text-icon-primary" role="button">
                                            <a href="#" onclick="js:return false;">Inbox</a>
                                        </div>
                                        <div id="mailbox-sent" class="mailbox-menu-item  ui-button ui-widget ui-state-default ui-corner-all ui-button-text-icon-primary" role="button">
                                            <a href="#" onclick="js:return false;">Sent Mail</a>
                                        </div>
                                        <div id="mailbox-trash" class="mailbox-menu-item  ui-button ui-widget ui-state-default ui-corner-all ui-button-text-icon-primary ui-droppable" role="button">
                                            <a href="#" onclick="js:return false;">Trash </a>
                                        </div>
                                        <div class="mailbox-group-mdg icon-group"><a href="#">Group Message</a></div>
                                        <div class="mailbox-group-mdg icon-new-msg"><a href="#">New Message</a></div>
                                    </div>
                                </div>


                                <div id="mailbox-list" class="mailbox-list ui-helper-clearfix" sortby="modified.desc">
                                    <style type="text/css">
                                        .flash-notice {
                                            right: 35%;
                                            font-size: 11px;
                                            top: 26px;
                                            width: 342px;
                                            right: 30%;
                                        }

                                        .flash-success {
                                            top: 32px;
                                            font-size: 11px;
                                            right: 230px;
                                            width: 339px;
                                        }
                                    </style>

                                </div>


                                <div class="mailbox-compose ui-helper-clearfix ui-widget ui-widget-content ui-corner-all">

                                    <style type="text/css">
                                        .flash-notice {
                                            right: 35%;
                                            font-size: 11px;
                                            top: 26px;
                                            width: 342px;
                                            right: 30%;
                                        }

                                        .flash-success {
                                            top: 32px;
                                            font-size: 11px;
                                            right: 230px;
                                            width: 339px;
                                        }
                                    </style>


                                    <div class="formCon" style="margin: 10px; padding: 10px; width: 710px;">

                                        <div class="formConInner" style="width: 660px;">
                                            <table style="width:100%" border="0" cellspacing="0" cellpadding="0">
                                                <tbody>
                                                    <tr>
                                                        <td><strong>
                                                            <label for="Mailbox_to">To</label></strong></td>
                                                        <td>
                                                            <asp:TextBox ID="tbx_toaddress" runat="server" Width="300px" CssClass="form-control"></asp:TextBox>
                                                            
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>&nbsp;</td>
                                                        <td>&nbsp;</td>
                                                    </tr>
                                                    <tr>
                                                        <td><strong>
                                                            <label class="mailbox-label" for="Mailbox_subject">Subject</label></strong></td>
                                                        <td>
                                                            <asp:TextBox ID="tbx_subject" runat="server" Width="600px"  CssClass="form-control"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>&nbsp;</td>
                                                        <td>&nbsp;</td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">
                                                           <textarea id="tbx_composetextarea" class="form-control"  Width="600px" style="height: 300px" runat="server"></textarea>
                                                        </td>
                                                    </tr>
                                                </tbody>
                                            </table>

                                            <div id="mform" align="left">                                                
                                                <asp:Button ID="btn_sendmsg" runat="server" OnClick="btn_sendmsg_Click" CssClass="formbut-a" Text="Send Message" />


                                            </div>
                                        </div>
                                    </div>
                                </div>



                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
        </div>
    </div>

 <script src="plugins/bootstrap-wysihtml5/bootstrap3-wysihtml5.all.js"></script>    
     <script src="plugins/slimScroll/jquery.slimscroll.min.js"></script>
    <!-- FastClick -->
    <script src="plugins/fastclick/fastclick.js"></script>
    <!-- iCheck -->
    <script src="plugins/iCheck/icheck.min.js"></script>
    <asp:HiddenField ID="mydata" runat="server" />
    <script>
        $(function () {
            //Add text editor
            $('#' + '<%= tbx_composetextarea.ClientID %>').wysihtml5({
            });
        });

    </script>
     <asp:HiddenField ID="hd_tempid" runat="server" />
    
    
    <asp:HiddenField ID="hdn_templatecode" runat="server" />

</asp:Content>

