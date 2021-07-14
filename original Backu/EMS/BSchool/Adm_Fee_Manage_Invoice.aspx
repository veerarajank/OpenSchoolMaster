<%@ Page Title="" Language="C#" MasterPageFile="~/Adm_FeesSettings.master" AutoEventWireup="true" CodeFile="Adm_Fee_Manage_Invoice.aspx.cs" Inherits="Adm_Fee_Manage_Invoice" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

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
                            <div class="box-two">
                                <div class="pdf-div">
                                    <a class="pdf_but" target="_blank" runat="server" id="pdf_print" href="#">Generate PDF</a>&nbsp;
                                    <a class="expertcv" href="#">Generate RTF</a>
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
                            <br>

                            <div class="clear"></div>
                            <br>
                            <br>
                            <br>
                            <h1>Transactions</h1>

                            <div class="pdf-box">
                                <div class="box-one">
                                    <div class="bttns_addstudent-n">
                                        <ul>
                                            <li></li>
                                            <li></li>
                                        </ul>
                                    </div>
                                </div>
                                <div class="box-two">
                                    <div class="pdf-div">
                                        <a class="pdf_but" target="_blank" href="#">Generate PDF </a>
                                    </div>
                                </div>
                            </div>


                            <div class="invoice-table">


                                <table style="width: 100%" cellspacing="0" cellpadding="0" border="0">
                                    <tbody>
                                        <tr class="pdtab-h">
                                            <td align="center">#</td>
                                            <td height="18">Date *</td>
                                            <td align="center">Type</td>
                                            <td align="center">Transaction ID</td>
                                            <td align="center">Description</td>
                                            <td align="center">Amount *</td>
                                            <td align="center">Proof</td>
                                            <td align="center">Status</td>
                                            <td align="center">Actions</td>
                                        </tr>                                       
                                        <tr>
                                            <td align="center"><span id="new-count">-</span></td>
                                            <td height="18">
                                                <input name="FeeTransactions[invoice_id]" id="FeeTransactions_invoice_id" type="hidden" value="124">
                                                <input style="width: 80px;" readonly="readonly" id="tbx_FeeTransactions_date" runat="server" type="text" class="hasDatepicker">
                                            </td>
                                            <td height="18">
                                                <asp:DropDownList ID="drp_FeeTransactions_payment_type" Width="80" runat="server"></asp:DropDownList>

                                               
                                            </td>
                                            <td height="18">
                                                <input style="width: 80px;" runat="server" id="tbx_FeeTransactions_transaction_id" type="text">
                                            </td>
                                            <td height="18">
                                                <input style="width: 80px;" runat="server" id="tbx_FeeTransactions_description" type="text">
                                            </td>
                                            <td height="18">
                                                <input style="width: 50px;" runat="server" id="tbx_FeeTransactions_amount" type="text">
                                            </td>
                                            <td height="18">                                                
                                                    <select runat="server" id="drp_FeeTransactions_proof">                                                
                                                    <option value="1">Yes</option>
                                                    <option value="0">No</option>
                                                </select>
                                                
                                                
                                            </td>
                                            <td height="18">-
                                            </td>
                                            <td height="18">                                                
                                                <asp:Button ID="btn_additem" runat="server" Text="Add" Height="28px" OnClick="btn_additem_Click" CssClass="formbut" />
                                            </td>
                                        </tr>

                                        <asp:ListView ID="lst_collection" runat="server" GroupPlaceholderID="groupPlaceHolder1"
                                            ItemPlaceholderID="itemPlaceHolder1">
                                            <LayoutTemplate>
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
                                                    <%# Eval("payment_date") %>
                                                </td>
                                                <td>
                                                    <%# Eval("payment_info") %>
                                                </td>
                                                <td>
                                                    <%# Eval("payment_transid") %>
                                                </td>
                                                <td>
                                                    <%# Eval("payment_description") %>
                                                </td>
                                                <td>
                                                    <%# Eval("payment_amt") %> 
                                                </td>
                                                 <td>
                                                    <%# Eval("is_proof") %> 
                                                </td>

                                                <td>
                                                    <%# Eval("payment_status") %>
                                                </td>
                                                <td></td>
                                               


                                                

                                            </ItemTemplate>
                                            <EmptyDataTemplate>
                                              </EmptyDataTemplate>
                                        </asp:ListView>




                                    </tbody>
                                </table>

                            </div>
                        </div>
                    </div>
                </td>
            </tr>
        </tbody>
    </table>

     <script src="plugins/jQuery/jquery-2.2.3.min.js"></script>

    <script type="text/javascript">
        $(function () {

            $('#' + '<%= tbx_FeeTransactions_date.ClientID %>').datepicker({ autoclose: true });
            $('#' + '<%= tbx_FeeTransactions_amount.ClientID %>').numeric()
            

        });
    </script>

</asp:Content>

