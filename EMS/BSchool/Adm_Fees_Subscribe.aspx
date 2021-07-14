<%@ Page Title="" Language="C#" MasterPageFile="~/Adm_FeesSettings.master" AutoEventWireup="true" CodeFile="Adm_Fees_Subscribe.aspx.cs" Inherits="Adm_Fees_Subscribe" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="cont_right formWrapper">
        <h1>Create Subscription</h1>
        <div class="edit_bttns" style="width: 175px; top: 15px;"></div>
        <div class="formCon">
            <div class="formConInner">
                <div>
                    <strong>Fee Category</strong> :
                    <label id="lbl_category" runat="server"></label>
                </div>
                <br>
                <div>
                    <strong>Date Created</strong> :
                    <label id="lbl_createdon" runat="server"></label>
                </div>
                <br>
                <div>
                    <strong>Description</strong> :
                    <label id="lbl_desc" runat="server"></label>
                </div>
            </div>
        </div>

        <style>
            .sub_type {
            }

                .sub_type label {
                    display: inline-block;
                    margin-right: 10px;
                }

            .white_bx {
                border: 1px solid #c5ced9;
                padding: 15px;
                background-color: #fff;
                margin-bottom: 20px;
                width: 91%;
                position: relative;
            }

                .white_bx input[type="text"] {
                    width: 146px;
                }

            .triangle-up {
                position: absolute;
                top: -11px;
                left: 29px;
                width: 0;
                height: 0;
                border-left: 10px solid transparent;
                border-right: 10px solid transparent;
                border-bottom: 10px solid #c5ced9;
            }

            .cust_duedate {
                float: left;
                position: relative;
                width: 190px;
                margin-bottom: 10px;
            }

            .fees-trash {
                position: absolute;
                top: 5px;
                right: 13px;
                width: 15px;
                height: 19px;
            }

            .error-brd {
                border-color: #F30 !important;
            }
        </style>

         <p class="note" runat="server" id="field_mandary">Fields with <span class="required">*</span> are required.</p>
        <div class="formCon">
            <div class="formConInner">
                <h3>Setup a Subscription Method</h3>
                <table style="width: 90%">
                    <tbody>
                        <tr>
                            <td>
                                <label for="FeeCategories_start_date" class="required">Start Date <span class="required">*</span></label></td>
                            <td>
                                <input readonly="readonly" id="tbx_start_date" runat="server" type="text" class="hasDatepicker">
                            </td>
                            <td>
                                <label for="FeeCategories_end_date" class="required">End Date <span class="required">*</span></label></td>
                            <td>
                                <input readonly="readonly" id="tbx_end_date" runat="server" type="text" class="hasDatepicker">
                            </td>
                        </tr>
                        <tr>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <asp:CheckBox ID="chk_amtdivide" runat="server" /><label for="FeeCategories_amount_divided">Divide the fee amount by number of subscriptions</label></td>
                        </tr>
                        <tr>
                            <td colspan="4">&nbsp;</td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <h3>
                                    <label for="FeeCategories_subscription_type">Subscription Due Dates</label></h3>
                            </td>
                        </tr>
                        <tr>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                        </tr>
                    </tbody>
                </table>
                <div id="payment_types">

                    <div class="white_bx">
                        <div class="triangle-up" style="left: 110px;"></div>

                        <div id="payment_recurring_types">

                            <div id="custom_due_dates">
                                <br>
                                <div>
                                    <label>Due Dates <span class="required">*</span></label>
                                </div>
                                <div class="duedates">
                                    <asp:ListView ID="lv_listview" runat="server" GroupPlaceholderID="groupPlaceHolder1"
                                    ItemPlaceholderID="itemPlaceHolder1">
                                    <LayoutTemplate>

                                        <asp:PlaceHolder runat="server" ID="groupPlaceHolder1"></asp:PlaceHolder>

                                    </LayoutTemplate>
                                    <GroupTemplate>
                                        <div class="cust_duedate">
                                            <asp:PlaceHolder runat="server" ID="itemPlaceHolder1"></asp:PlaceHolder>
                                        </div>
                                    </GroupTemplate>
                                    <ItemTemplate>
                                        <asp:TextBox ID="tbx_duedate" Text='<%# Eval("DueDates")%>' CssClass="duedate" runat="server"></asp:TextBox>
                                         <asp:LinkButton ID="lnk_del" class="fees-trash" runat="server" OnClick="lnk_del_Click">                                    
                                        </asp:LinkButton>
                                        <asp:HiddenField ID="hd_refid" runat="server" Value='<%# Eval("RefId")%>' />
                                      
                                    </ItemTemplate>
                                    <EmptyDataTemplate>
                                    </EmptyDataTemplate>
                                </asp:ListView>
                                </div>

                                <div class="clear"></div>
                            </div>
                            <div style="padding-top: 10px;">
                                <a href="javascript:void(0);" id="add_custom_due_date" runat="server" onserverclick="add_custom_due_date_ServerClick">+ Add custom due date</a>
                                

                            </div>
                        </div>

                    </div>
                </div>
            </div>
        </div>
        <div class="row buttons">
            <asp:Button ID="btn_createsubscribe" OnClick="btn_createsubscribe_Click" runat="server" Text="Submit" Height="30px" Width="150px" CssClass="formbut" />
        </div>


    </div>

    <script src="plugins/jQuery/jquery-2.2.3.min.js"></script>

    <script type="text/javascript">
        $(function () {
            $('.duedate').datepicker({ autoclose: true });
            $('#' + '<%= tbx_start_date.ClientID %>').datepicker({ autoclose: true });
            $('#' + '<%= tbx_end_date.ClientID %>').datepicker({ autoclose: true });

        });
    </script>
</asp:Content>

