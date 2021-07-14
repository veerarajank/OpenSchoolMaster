<%@ Page Title="" Language="C#" MasterPageFile="~/Admin_MyAccount.master" AutoEventWireup="true" CodeFile="Adm_My_News.aspx.cs" Inherits="Adm_My_News" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script src="dist/js/jscolor.js"></script>

    <style>
        .jscolor {
            height: 22px;
            width: 22px;
            background: url(images/trigger.png) center no-repeat;
            vertical-align: middle;
            margin: 0 .25em;
            display: inline-block;
        }

        .txtfld-col {
            float: left;
            width: 31%;
            padding: 0px 7px 0px 7px;
            margin: 0px;
            min-height: 30px;
        }

        .row {
            margin-bottom: 0px !important;
        }
    </style>
    <div runat="server" id="search_section">

        <table style="width: 100%" border="0" cellspacing="0" cellpadding="0">
            <tbody>
                <tr>
                    <td valign="top" style="width: 100%">
                        <div class="cont_right formWrapper" style="padding: 0px; width: 753px;">
                            <div id="parent_rightSect">
                                <div class="parentright_innercon">
                                    <div class="mail_head">
                                        <h1 style="margin: 0px;"><b>Site News</b></h1>
                                        <span>Latest news listed here</span>
                                    </div>

                                    <div class="button-bg" style="padding: 8px">
                                        <div class="top-hed-btn-left"></div>
                                        <div class="top-hed-btn-right">
                                            <ul>
                                                <li><a class="a_tag-btn" href="Adm_My_News.aspx?mode=add"><span>Create News</span></a></li>
                                                <li><a class="a_tag-btn" href="#"><span>Publish News</span></a></li>
                                            </ul>
                                        </div>
                                    </div>

                                    <div class="news-list ui-helper-clearfix" sortby="modified.desc">
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








                                        <div id="academic-years-grid" class="grid-view">

                                            <div class="summary" id="lbl_cnt" runat="server" style="font-size: 12px;">
                                                Displaying 2 result(s).
                                            </div>
                                            <div class="grid_table_con">
                                                <asp:ListView ID="lv_listview" runat="server" GroupPlaceholderID="groupPlaceHolder1"
                                                    ItemPlaceholderID="itemPlaceHolder1" OnPagePropertiesChanging="lv_listview_PagePropertiesChanging">
                                                    <LayoutTemplate>
                                                        <table class="items" style="border: 1px solid #b9c7d0!important;">

                                                            <asp:PlaceHolder runat="server" ID="groupPlaceHolder1"></asp:PlaceHolder>
                                                            <tr style="background-color: #EEEEE0; border: 1px solid #b9c7d0!important" class="tableInfoTH">
                                                                <td colspan="5" class="tableInfoTD">
                                                                    <asp:DataPager ID="DataPager1" runat="server" PagedControlID="lv_listview" PageSize="10" class="googleNavegationBar">
                                                                        <Fields>
                                                                            <aspPage:GooglePagerField NextPageImageUrl="~/Images/button_arrow_right.gif" PreviousPageImageUrl="~/Images/button_arrow_left.gif" />
                                                                        </Fields>
                                                                    </asp:DataPager>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </LayoutTemplate>
                                                    <GroupTemplate>
                                                        <tr class="mailbox-item msg-read mailbox-draggable-row">
                                                            <asp:PlaceHolder runat="server" ID="itemPlaceHolder1"></asp:PlaceHolder>
                                                        </tr>
                                                    </GroupTemplate>
                                                    <ItemTemplate>





                                                        <td align="left" style="width: 40px;" class="mailbox-item-even">
                                                            <label class="ui-helper-reset" for="conv_48">
                                                                <div class="mailbox-item-wrapper mailbox-check">
                                                                    <div class="ckbox-default ckbox">
                                                                        <input class="mailbox-check " id="conv_48" type="checkbox" name="convs[]" value="48">
                                                                        <asp:CheckBox ID="chk"
                                                                        <label for="conv_48"></label>
                                                                    </div>
                                                                </div>
                                                            </label>
                                                        </td>
                                                        <td class="mailbox-subject-brief mailbox-item-even" align="center" style="width: 450px; text-align: left;">
                                                            <div class="mailbox-item-wrapper mailbox-subject mailbox-ellipsis">
                                                                <a class="mailbox-link" title="View this news update" href="Adm_My_News.aspx?mode=view&id=<%# Eval("News_Id") %>">
                                                                    <div class="mailbox-subject-text"><%# Eval("News_Title") %></div>
                                                                    <div class="mailbox-msg-brief"><%# Eval("News_Description") %></div>
                                                                </a>
                                                            </div>
                                                        </td>
                                                        <td style="width: 50px;" align="right" class="mailbox-item-even">
                                                            <div class="mailbox-item-wrapper mailbox-ellipsis">
                                                                <a class="mailbox-link" title="View this news update" href="Adm_My_News.aspx?mode=view&id=<%# Eval("News_Id") %>"><%# Eval("created_on") %>            </a>
                                                            </div>

                                                        </td>










                                                    </ItemTemplate>
                                                    <EmptyDataTemplate>
                                                    </EmptyDataTemplate>
                                                </asp:ListView>
                                            </div>

                                        </div>


                                    </div>
                                </div>
                            </div>
                        </div>
                    </td>
                </tr>
            </tbody>
        </table>

    </div>

    <div id="add_section" runat="server">



        <div class="cont_right formWrapper">
            <h1>Create News</h1>

            <style type="text/css">
                td.rah textarea {
                    width: 100% !important;
                    height: auto;
                }
            </style>

            <p class="note" runat="server" id="field_mandary">Fields with <span class="required">*</span> are required.</p>
            <div class="formCon">
                <div class="formConInner">

                    <table width="80%" border="0" cellspacing="0" cellpadding="0">
                        <tbody>
                            <tr>
                                <td>
                                    <label for="Publish_title" class="required">Title <span class="required">*</span></label></td>
                                <td>

                                    <asp:TextBox ID="tbx_logname" runat="server" AutoCompleteType="Disabled" MaxLength="255" Width="250px"></asp:TextBox>
                                    <asp:RequiredFieldValidator runat="server" CssClass="input-notification-error  error-simple png_bg" ValidationGroup="vd_save" ID="reqdocumentname" ControlToValidate="tbx_logname" ErrorMessage="Log cannot be blank." />

                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">&nbsp;</td>
                            </tr>
                            <tr>
                                <td>
                                    <label for="Publish_content" class="required">Description <span class="required">*</span></label></td>
                                <td class="rah">
                                    <textarea rows="5" runat="server" id="Publish_content"></textarea>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                    <div style="padding-top: 20px;">
                        <button id="btn_createeventtype" validationgroup="vd_save" runat="server" style="width: 150px" onserverclick="btn_createeventtype_ServerClick" class="a_tag-btn"><span>Create</span></button>
                    </div>
                </div>
            </div>
        </div>






        <p style="color: red; font-size: 12px; font-weight: bold" runat="server" id="lbl_error"></p>




    </div>


    <div id="update_section" runat="server">

        <h4><b>Update Student Log</b></h4>
        <div class="form">
            <div class="button-bg">
                <div class="top-hed-btn-left"></div>
                <div class="top-hed-btn-right">
                    <ul>
                        <li>
                            <a class="a_tag-btn" href="Adm_Student_Log.aspx?mode=add"><span>Add Student Log</span></a>            </li>
                        <li>
                            <a class="a_tag-btn" href="Adm_Student_Log.aspx?mode=search"><span>Manage Student Log</span></a>            </li>
                    </ul>
                </div>
            </div>


            <p class="note" id="edit_field_mandary" runat="server">Fields with <span class="required">*</span> are required.</p>

            <div class="formCon">
                <div class="formConInner">


                    <div class="row">
                        <div class="txtfld-col">
                            <label for="Log_Name" class="required">Name<span class="required">*</span></label>
                            <asp:TextBox ID="tbx_edit_logname" runat="server" AutoCompleteType="Disabled" MaxLength="120"></asp:TextBox>
                            <asp:RequiredFieldValidator runat="server" CssClass="input-notification-error  error-simple png_bg" ValidationGroup="vd_editsave" ID="rd_editdocumentname" ControlToValidate="tbx_edit_logname" ErrorMessage="Log cannot be blank." />
                        </div>

                    </div>
                    <div class="row">
                        <div class="txtfld-col">
                            <label for="Publish_content" class="required">Description <span class="required">*</span></label>
                            <textarea rows="5" runat="server" id="edit_Publish_content"></textarea>

                        </div>
                    </div>







                </div>
            </div>


            <div class="button-bg">
                <div class="top-hed-btn-left"></div>
                <div class="top-hed-btn-right">
                    <ul>
                        <li>
                            <button id="btn_update" runat="server" validationgroup="vd_editsave" onserverclick="btn_update_ServerClick" style="width: 150px" class="a_tag-btn"><span>Save</span></button>
                        </li>
                    </ul>
                </div>
            </div>


        </div>



    </div>

    <div id="view_section" runat="server">

        <table style="width: 100%" border="0" cellspacing="0" cellpadding="0">
            <tbody>
                <tr>
                    <td valign="top" style="width: 100%">
                        <div class="cont_right formWrapper" style="padding: 0px; width: 753px;">
                            <div id="Div1">
                                <div class="parentright_innercon">
                                    <div class="mail_head">
                                        <h1 style="margin: 0px;"><b>Site News</b></h1>
                                        <span>Latest news listed here</span>
                                    </div>

                                    <div class="table_listbx">
                                        <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                            <tbody>
                                                <tr>
                                                    <td width="20%">
                                                        <b>Name</b>
                                                    </td>
                                                    <td width="3%"><strong>:</strong></td>
                                                    <td width="77%">
                                                        <label id="lbl_logname" runat="server"></label>

                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td width="20%">
                                                        <b>Description</b>
                                                    </td>
                                                    <td width="3%"><strong>:</strong></td>
                                                    <td width="77%">
                                                        <label id="lbl_description" runat="server"></label>

                                                    </td>
                                                </tr>



                                            </tbody>
                                        </table>
                                        <br />
                                        <br />

                                        <div class="mailbox-message-header">
                                            <div class="message-sender">
                                                Author:
                                                <label id="lbl_createdby" runat="server"></label>

                                            </div>

                                            <div class="message-date">Last Updated:
                                                <label id="lbl_createdon" runat="server"></label>
                                            </div>
                                            <br>
                                        </div>
                                    </div>

                                </div>
                            </div>
                        </div>
                    </td>
                </tr>
            </tbody>
        </table>


    </div>




    <script src="plugins/jQuery/jquery-2.2.3.min.js"></script>


    <asp:HiddenField ID="hd_id" runat="server" />
    <script src="plugins/jQuery/jquery-2.2.3.min.js"></script>
    <script type="text/javascript">
        function DeletePopup(id) {
            $('#' + "<%=hd_id.ClientID %>").val(id);
            $('.bs-example-modal-sm').modal('show');

        }


    </script>
    <script>
        $(document).ready(function () {
            $(".s_no_but").click(function () {
                $('.gridact_drop').hide();

                if ($("#" + this.id + 'l').is(':hidden')) {
                    $('.ns_drop').hide();
                    $(".s_no_but").removeClass("ns_drop_hand");
                    $("#" + this.id + 'l').show();
                    $("#" + this.id).addClass("ns_drop_hand");
                    $(".gridact_drop").hide();

                }
                else {
                    $("#" + this.id + 'l').hide();
                    $("#" + this.id).removeClass("ns_drop_hand");
                }
                return false;
            });
            $("#" + this.id + 'l').click(function (e) {
                e.stopPropagation();
            });

        });
        $(document).click(function () {

            $('.ns_drop').hide();
            $(".s_no_but").removeClass("ns_drop_hand");

        });
    </script>
    <div class="modal fade bs-example-modal-sm" tabindex="-1" role="dialog" aria-labelledby="mySmallModalLabel">
        <div class="modal-dialog modal-sm" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                    <h4 class="modal-title" id="gridSystemModalLabel">Are you want to delete Yes/No? </h4>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">No</button>
                    <button type="button" id="btn_del" runat="server" onserverclick="btn_del_ServerClick" class="btn btn-primary">Yes</button>
                </div>
            </div>
        </div>
    </div>
</asp:Content>

