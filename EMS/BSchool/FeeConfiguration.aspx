<%@ Page Title="" Language="C#" MasterPageFile="~/Adm_FeesSettings.master" AutoEventWireup="true" CodeFile="FeeConfiguration.aspx.cs" Inherits="FeeConfiguration" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="cont_right formWrapper">
        <h1>Fees Configurations</h1>
        <div class="edit_bttns" style="top: 20px; right: 20px;">
            <ul>
            </ul>
        </div>


        <style>
            #status input {
                float: left;
            }

            #status label {
                float: left;
            }
        </style>
        <div class="formCon">
            <div class="formConInner">


                <h3>Setup Fees Configurations</h3>

                <table style="width:100%" border="0" cellspacing="0" cellpadding="0">
                    <tbody>
                        <tr>
                            <td colspan="2">
                                <asp:CheckBox ID="chk_enablefee" runat="server" />
                                &nbsp;&nbsp;<label for="FeeConfigurations_tax_in_fee">Enable Tax in Fee</label></td>
                        </tr>
                        <tr>
                            <td colspan="2">&nbsp;</td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:CheckBox ID="chk_enablediscount" runat="server" />
                                &nbsp;&nbsp;<label for="FeeConfigurations_discount_in_fee">Enable Discount in Fee</label></td>
                        </tr>
                        <tr>
                            <td colspan="2">&nbsp;</td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:CheckBox ID="chk_showdiscount" runat="server" />
                                &nbsp;&nbsp;<label for="FeeConfigurations_discount_in_invoice">Show Discount Column in Invoice Template</label></td>
                        </tr>
                        <tr>
                            <td colspan="2">&nbsp;</td>
                        </tr>
                        <tr>
                            <td style="width:20%">
                                <label for="FeeConfigurations_invoice_template" class="required">Invoice Template <span class="required">*</span></label></td>
                            <td>
                                <asp:DropDownList ID="drp_template" runat="server" Width="200px">
                                    <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>
                                    <asp:ListItem Text="Template without Header" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="Template with Header" Value="2"></asp:ListItem>
                                </asp:DropDownList>                                
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">&nbsp;</td>
                        </tr>
                        <tr>
                            <td colspan="2">                                
                                <button id="btn_save" validationgroup="vd_save" runat="server" style="width: 150px" onserverclick="btn_save_ServerClick" class="a_tag-btn"><span>Save</span></button>

                            </td>

                        </tr>
                    </tbody>
                </table>


            </div>
        </div>

        <script>
            $(':checkbox#FeeConfigurations_discount_in_fee').change(function (e) {
                if (!$(this).is(':checked')) {
                    $(':checkbox#FeeConfigurations_discount_in_invoice').attr('checked', false);
                }
            });
        </script>
    </div>
</asp:Content>

