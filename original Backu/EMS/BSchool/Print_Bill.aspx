<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Print_Bill.aspx.cs" Inherits="Print_Bill" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">


  

	

    <table style="width: 100%" border="0" cellspacing="0" cellpadding="0">
        <tbody>
            <tr>
                <td valign="top" style="width: 75%">
                    <div class="cont_right formWrapper">
                        <h1 id="head_invoiceno" runat="server">Invoice - </h1>
                        <div class="pdf-box">
                            <div class="box-one">
                                <div class="bttns_addstudent-n">
                                    <ul>
                                        <li></li>
                                        <li></li>
                                    </ul>
                                </div>
                            </div>
                           
                        </div>

                        <div style="width: 100%; padding-top: 0px;" class="pdtab_Con">
                            <div class="invoice-table">
                                <table style="width: 100%" border="0" cellspacing="0" cellpadding="0">
                                    <tbody>
                                        <tr>
                                            <td style="width: 25%"><strong>Invoice ID</strong></td>
                                            <td>
                                                <label id="lbl_invoiceid" runat="server"></label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td><strong>Recipient</strong></td>
                                            <td>
                                                <a href="#" id="lbl_student_href" runat="server">
                                                    <label id="lbl_studentname" runat="server"></label>
                                                </a></td>
                                        </tr>
                                        <tr>
                                            <td><strong>Fee Category</strong></td>
                                            <td>
                                                <label id="lbl_feecategory" runat="server"></label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td><strong>Invoice Date</strong></td>
                                            <td>
                                                <label id="lbl_invoicedate" runat="server"></label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td><strong>Invoice Amount</strong></td>
                                            <td>
                                                <label id="lbl_invoiceamt" runat="server"></label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td><strong>Adjustments</strong></td>
                                            <td>
                                                <label id="lbl_adjustment" runat="server"></label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td><strong>Payment Details</strong></td>
                                            <td>
                                                <label id="lbl_paymentdetails" runat="server"></label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td><strong>Amount Payable</strong></td>
                                            <td>
                                                <label id="lbl_amtpayable" runat="server"></label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td><strong>Due Date</strong></td>
                                            <td>
                                                <label id="lbl_duedate" runat="server"></label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td><strong>Last payment date</strong></td>
                                            <td>
                                                <label id="lbl_lastpaymentdate" runat="server"></label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="font-weight: bold; font-size: 12px;"><strong>Status</strong> </td>
                                            <td style="font-size: 20px; font-weight: bold;">
                                                <span style="color: #F00" id="lbl_paymentstatus" runat="server"></span></td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                            <br>

                            <div class="invoice-table">
                                <input id="invoice_id" type="hidden" value="124" name="invoice_id">


                                <table style="width: 100%" cellspacing="0" cellpadding="0" border="0" id="invoice-particualrs">
                                    <tbody>
                                        <asp:ListView ID="lv_category" runat="server" GroupPlaceholderID="groupPlaceHolder1"
                                            ItemPlaceholderID="itemPlaceHolder1">
                                            <LayoutTemplate>

                                                <tr class="pdtab-h">
                                                    <td height="18" align="center">#</td>
                                                    <td align="center">Particulars</td>
                                                    <td height="18">Description</td>
                                                    <td align="center">Unit Price</td>
                                                    <td align="center">Discount</td>
                                                    <td align="center">Tax</td>
                                                    <td align="center">Amount</td>
                                                </tr>



                                                <asp:PlaceHolder runat="server" ID="groupPlaceHolder1"></asp:PlaceHolder>


                                            </LayoutTemplate>
                                            <GroupTemplate>
                                                <tr style="border: 1px solid #b9c7d0!important;">
                                                    <asp:PlaceHolder runat="server" ID="itemPlaceHolder1"></asp:PlaceHolder>
                                                </tr>
                                            </GroupTemplate>
                                            <ItemTemplate>
                                                <td>
                                                    <%# Container.DataItemIndex+1 %>
                                                </td>
                                                <td>
                                                    <%# Eval("item_name") %>
                                                </td>
                                                <td>
                                                    <%# Eval("item_description") %>
                                                </td>
                                                <td>
                                                    <%# Eval("amount") %>
                                                </td>
                                                <td>
                                                    <%# Eval("discount_value") %>&nbsp;<%# Eval("discount_type") %> 
                                                </td>
                                                <td>
                                                    <%# Eval("tax_value") %> %
                                                </td>

                                                <td>
                                                    <%# Eval("grossamt") %>
                                                </td>





                                            </ItemTemplate>
                                            <EmptyDataTemplate>
                                                <table style="width: 100%" cellspacing="0" cellpadding="0" border="0" id="invoice-particualrs">
                                                    <tbody>
                                                        <tr class="pdtab-h">
                                                            <td height="18" align="center">#</td>
                                                            <td align="center">Particulars</td>
                                                            <td height="18">Description</td>
                                                            <td align="center">Unit Price</td>
                                                            <td align="center">Discount</td>
                                                            <td align="center">Tax</td>
                                                            <td align="center">Amount</td>
                                                        </tr>
                                                        <tr class="invoice-particular-box">
                                                            <td align="center"></td>
                                                            <td></td>
                                                            <td></td>
                                                            <td align="center"></td>
                                                            <td align="center"></td>
                                                            <td align="center"></td>
                                                            <td align="center"></td>
                                                        </tr>
                                            </EmptyDataTemplate>
                                        </asp:ListView>

                                        <tr>
                                            <td colspan="6" align="right" style="padding-right: 10px;"><strong>Sub Total (INR)</strong></td>
                                            <td align="center" id="td_subtotal" runat="server"></td>
                                        </tr>
                                        <tr>
                                            <td colspan="6" align="right" style="padding-right: 10px;"><strong>Discount (INR)</strong>
                                            </td>
                                            <td align="center" id="td_discountamt" runat="server"></td>
                                        </tr>
                                        <tr>
                                            <td colspan="6" align="right" style="padding-right: 10px;"><strong>Tax (INR)</strong>(+)</td>
                                            <td align="center" id="td_taxamt" runat="server"></td>
                                        </tr>
                                        <tr>
                                            <td colspan="6" align="right" style="padding-right: 10px;"><strong>Total (INR)</strong></td>
                                            <td align="center" id="td_grossamt" runat="server"></td>
                                        </tr>

                                    </tbody>
                                </table>
                            </div>
                        </div>
                    </div>
                </td>
            </tr>
        </tbody>
    </table>

</asp:Content>

