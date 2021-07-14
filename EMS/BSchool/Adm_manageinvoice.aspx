<%@ Page Title="" Language="C#" MasterPageFile="~/Adm_FeesSettings.master" AutoEventWireup="true" CodeFile="Adm_manageinvoice.aspx.cs" Inherits="Adm_manageinvoice" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <table style="width: 100%" border="0" cellspacing="0" cellpadding="0">
        <tbody>
            <tr>
                <td valign="top" style="width: 75%">
                    <div class="cont_right formWrapper">
                        <h1>Manage Invoices</h1>
                        <div class="overview">
                            <div class="overviewbox overviewbox-onerow  ovbox1">
                                <h1><strong>Total Invoices</strong></h1>
                                <div class="ovrBtm" runat="server" id="div_totalinvoicecnt">
                                    0
                                </div>
                            </div>
                            <div class="overviewbox overviewbox-onerow  ovbox2">
                                <h1><strong>Paid Invoices</strong></h1>
                                <div class="ovrBtm" runat="server" id="div_paidnvoicecnt">
                                    0
                                </div>
                            </div>
                            <div class="overviewbox overviewbox-onerow  ovbox2">
                                <h1><strong>Un-Paid Invoices</strong></h1>
                                <div class="ovrBtm" runat="server" id="div_unpaidnvoicecnt">
                                    0
                                </div>
                            </div>
                            <div class="overviewbox overviewbox-onerow  ovbox2">
                                <h1><strong>Cancelled</strong></h1>
                                <div class="ovrBtm" runat="server" id="div_cancellednvoicecnt">
                                    0
                                </div>
                            </div>
                            <div class="clear"></div>
                        </div>
                        <!-- categories -->

                        <div class="clear"></div>
                        <br>


                        <!--filters-->
                        <style>
                            .invoice-fltr input[type="text"] {
                                width: 85%;
                            }

                            .invoice-fltr select {
                                width: 85%;
                            }
                        </style>

                        <div class="formCon">
                            <div class="formConInner invoice-fltr">
                                <h3>Search Invoices</h3>
                                <table style="width: 98%">
                                    <tbody>
                                        <tr>
                                            <td>Fee Category</td>
                                            <td>Invoice ID</td>
                                            <td colspan="2">Recipient name</td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:DropDownList ID="drp_fee_id" Width="212px" runat="server"></asp:DropDownList>
                                            </td>
                                            <td>
                                                <input id="tbx_FeeInvoices_id" runat="server" type="text"></td>
                                            <td colspan="2">
                                                <input style="width: 212px;" id="tbx_FeeInvoices_uid" runat="server" type="text" value=""></td>
                                        </tr>
                                        <tr>
                                            <td colspan="4">&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td>Course</td>
                                            <td>Batch</td>
                                            <td>Status</td>
                                            <td>Invoice Date</td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:DropDownList ID="drp_course" runat="server" Width="212px" AutoPostBack="true" OnSelectedIndexChanged="drp_course_SelectedIndexChanged">
                                                </asp:DropDownList>

                                            </td>
                                            <td>
                                                <asp:DropDownList ID="drp_batches" runat="server">
                                                </asp:DropDownList></td>
                                            <td>
                                                <select runat="server" id="FeeInvoices_is_paid">
                                                    <option value="">All</option>
                                                    <option value="1">Paid</option>
                                                    <option value="0">Unpaid</option>
                                                    <option value="-1">Cancelled</option>
                                                </select></td>
                                            <td>
                                                <input class="custom-date hasDatepicker" id="tbx_FeeInvoices_created_at" runat="server" type="text">
                                            </td>

                                        </tr>

                                        <tr>
                                            <td colspan="4" style="padding-top: 9px;">
                                                <input name="" class="formbut" type="submit" value="Search"></td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                        </div>



                        <div class="pdf-box">
                            <div class="box-one">
                                <div class="bttns_addstudent-n">
                                    <ul>
                                        <li><a href="javascript:void(0);" class="formbut-n" id="mark-as-cancel">Cancel</a></li>
                                        <li><a href="javascript:void(0);" class="formbut-n" id="send-reminder">Send Reminder</a></li>
                                    </ul>
                                </div>
                            </div>
                            <div class="box-two">
                                <div class="bttns_addstudent-n">
                                    <ul>
                                        <li>
                                            <a class="expertcv" href="#">Export CSV</a>                                        </li>
                                        <li>
                                            <a target="_blank" class="pdf_but" href="#">Generate PDF</a>                                        </li>
                                    </ul>

                                </div>
                            </div>
                        </div>

                        <div class="clear"></div>
                        <div style="width: 100%; padding-top: 0px;" class="pdtab_Con">


                            <asp:ListView ID="lv_category" runat="server" GroupPlaceholderID="groupPlaceHolder1"
                                ItemPlaceholderID="itemPlaceHolder1" OnPagePropertiesChanging="lv_category_PagePropertiesChanging">
                                <LayoutTemplate>
                                    <table style="width: 100%" cellspacing="0" cellpadding="0" border="0">
                                        <tbody>
                                            <tr class="pdtab-h">
                                                <td>
                                                    <input id="check-all" type="checkbox" value="1" name="check-all"></td>
                                                <td height="18" align="center">Invoice ID</td>
                                                <td align="center">Recipient</td>
                                                <td height="18" align="center">Fee Category</td>
                                                <td align="center">Amount</td>
                                                <td align="center">Balance</td>
                                                <td align="center">Date</td>
                                                <td align="center">Status</td>
                                                <td align="center">Actions</td>
                                            </tr>



                                            <asp:PlaceHolder runat="server" ID="groupPlaceHolder1"></asp:PlaceHolder>
                                            <tr style="background-color: #EEEEE0; border: 1px solid #b9c7d0!important" class="tableInfoTH">
                                                <td colspan="9" class="tableInfoTD">
                                                    <asp:DataPager ID="DataPager1" runat="server" PagedControlID="lv_category" PageSize="10" class="googleNavegationBar">
                                                        <Fields>
                                                            <aspPage:GooglePagerField NextPageImageUrl="~/Images/button_arrow_right.gif" PreviousPageImageUrl="~/Images/button_arrow_left.gif" />
                                                        </Fields>
                                                    </asp:DataPager>
                                                </td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </LayoutTemplate>
                                <GroupTemplate>
                                    <tr style="border: 1px solid #b9c7d0!important;">
                                        <asp:PlaceHolder runat="server" ID="itemPlaceHolder1"></asp:PlaceHolder>
                                    </tr>
                                </GroupTemplate>
                                <ItemTemplate>
                                    <td>
                                        <asp:CheckBox ID="chk_invoice" runat="server" />
                                    </td>
                                    <td>
                                        <%# Eval("invoice_no") %>
                                    </td>
                                    <td>
                                        <%# Eval("student_name") %>
                                    </td>
                                    <td>
                                        <%# Eval("category_name") %>
                                    </td>
                                    <td>
                                        <%# Eval("invoice_amt") %>
                                    </td>
                                    <td>
                                        <%# Eval("invoice_due_amt") %>
                                    </td>

                                    <td>
                                        <%# Eval("schedule_date") %>
                                    </td>
                                    <td>
                                        <%# Eval("invoice_status") %>
                                    </td>



                                    <td>
                                        <asp:LinkButton ID="lnk_search" CommandName="view" CommandArgument='<%# Eval("invoiceid") %>' OnCommand="lnk_search_Command" runat="server">
                                    View
                                        </asp:LinkButton>&nbsp;
                     
                                    </td>

                                </ItemTemplate>
                                <EmptyDataTemplate>
                                     <table style="width: 100%" cellspacing="0" cellpadding="0" border="0">
                                    <tbody>
                                        <tr class="pdtab-h">
                                            <td>
                                                <input id="check-all" type="checkbox" value="1" name="check-all"></td>
                                            <td height="18" align="center">Invoice ID</td>
                                            <td align="center">Recipient</td>
                                            <td height="18" align="center">Fee Category</td>
                                            <td align="center">Amount</td>
                                            <td align="center">Balance</td>
                                            <td align="center">Date</td>
                                            <td align="center">Status</td>
                                            <td align="center">Actions</td>
                                        </tr>
                                    </tbody>
                                    </table>
                                </EmptyDataTemplate>
                            </asp:ListView>



                            <div class="clear"></div>
                        </div>
                    </div>
                </td>
            </tr>
        </tbody>
    </table>


    <script src="plugins/jQuery/jquery-2.2.3.min.js"></script>

    <script type="text/javascript">
        $(function () {

            $('#' + '<%= tbx_FeeInvoices_created_at.ClientID %>').datepicker({ autoclose: true });


        });
    </script>
</asp:Content>

