<%@ Page Title="" Language="C#" MasterPageFile="~/Adm_FeesSettings.master" AutoEventWireup="true" CodeFile="Adm_CreateFees.aspx.cs" Inherits="Adm_CreateFees" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="cont_right formWrapper">
        <h1>Create Fees</h1>
        <div class="edit_bttns" style="width: 175px; top: 15px;"></div>
        <style>
            .fee-particular-head {
                padding: 10px 15px;
                background-color: #c5ced9;
                color: #405875;
                font-weight: bold;
                position: relative;
            }

            .feeParticular {
                border: 1px solid #c5ced9;
                padding: 15px;
                background-color: #fff;
                margin-bottom: 20px;
            }


            .applicable-to {
                border: 1px solid #c5ced9;
                padding: 10px;
                margin-bottom: 10px;
                background-color: #F9F9FD;
                margin-bottom: 15px;
                margin-top: 15px;
            }

            .error-brd {
                border-color: #F30 !important;
            }
        </style>


         <p class="note" runat="server" id="field_mandary">Fields with <span class="required">*</span> are required.</p>
        <div class="formCon">
            <div class="formConInner" style="width: 95%;">
                <h3>Fee Category</h3>
                <table width="100%">
                    <tbody>
                        <tr>
                            <td width="10%">
                                <label for="FeeCategories_name" class="required">Name <span class="required">*</span></label></td>
                        </tr>
                        <tr>
                            <td>
                                <input class="FeeCategories_name" runat="server" style="width: 100% !important;" id="tbx_name" type="text" maxlength="250">
                            </td>
                        </tr>
                        <tr>
                            <td>&nbsp;</td>
                        </tr>
                        <tr id="tr_div" runat="server">
                            <td>
                                <label for="FeeCategories_description">Description</label></td>
                        </tr>
                        <tr>
                            <td>
                                <textarea class="FeeCategories_description" style="width: 100% !important; height: 120px;" runat="server" id="tbx_description">                                    
                                    </textarea>
                            </td>
                        </tr>
                    </tbody>
                </table>
                <div class="clear"></div>
                <br>
                <br>
                <h3 style="width: 100%;">Fee Particulars</h3>

                <asp:ListView ID="lv_particulatars" runat="server" GroupPlaceholderID="groupPlaceHolder1"
                    ItemPlaceholderID="itemPlaceHolder1" OnItemDataBound="OnItemDataBound">
                    <ItemTemplate>
                        <div id="fee-particulars">
                            <div class="fee-particular" id="fee-particular-0" data-row="0">
                                <div class="fee-particular-head">
                                    <table style="width: 100%">
                                        <tbody>
                                            <tr>
                                                <td style="width: 22%">
                                                    <label>Name<sup>*</sup></label></td>
                                                <td style="width: 33%">
                                                    <label>Description</label></td>
                                                <td style="width: 17%">
                                                    <label>Tax</label></td>
                                                <td style="width: 13%">
                                                    <label>Discount</label></td>
                                                <td style="width: 45%">
                                                    <label>&nbsp;</label></td>
                                            </tr>
                                        </tbody>
                                    </table>                                   
                                    <a href="javascript:void(0);" runat="server" onserverclick="lnk_del_ServerClick" id="lnk_del" title="Click to remove access" class="remove-access fees-trash"></a>


                                </div>
                                <div class="feeParticular">
                                    <table style="width: 100%">
                                        <tbody>
                                            <tr>
                                                <td style="width: 20%" valign="top">
                                                    <input class="FeeParticulars_name particular-name" value='<%# Eval("PName") %>'  placeholder="Particular Name" style="width: 120px !important;" id="tbx_particularname" type="text" runat="server">
                                                </td>
                                                <td style="width: 35%" valign="top">
                                                    <input class="FeeParticulars_description" value='<%# Eval("PDesc") %>' placeholder="Particular Description" style="width: 200px !important;" id="tbx_particulardescription" runat="server" type="text">
                                                </td>
                                                <td style="width: 45%" valign="top">
                                                    <asp:DropDownList ID="drp_taxmaster" runat="server" CssClass="FeeParticulars_tax"></asp:DropDownList>

                                                </td>
                                                <td style="width: 45%" valign="top">
                                                    <input class="FeeParticulars_tax" placeholder="Discount" value='<%# Eval("PDiscount") %>' style="width: 70px !important;"  id="tbx_discountvalue" runat="server" type="text">
                                                </td>
                                                <td style="width: 45%" valign="top">
                                                    <asp:DropDownList ID="drp_discounttype" Width="100px" runat="server" CssClass="FeeParticulars_discount_type">
                                                        <asp:ListItem Text="%" Value="%"></asp:ListItem>
                                                        <asp:ListItem Text="INR" Value="INR"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                                <td style="width: 10%" valign="middle"></td>

                                            </tr>
                                        </tbody>
                                    </table>

                                    <br>
                                    <h3>Applicable to:</h3>
                                    <asp:ListView ID="lv_applicableto" runat="server" GroupPlaceholderID="groupPlaceHolder1" DataSource='<%# Eval("fees_applicable") %>'
                                        ItemPlaceholderID="itemPlaceHolder1" OnItemDataBound="lv_applicableto_ItemDataBound">
                                        <ItemTemplate>


                                            <div id="particular-accesses-0">

                                                <div class="particular-access" data-row="0">
                                                    <div class="applicable-to">
                                                        <table>
                                                            <tbody>
                                                                <tr>
                                                                    <td>

                                                                        <asp:DropDownList ID="drp_accesstype" runat="server" Width="120px" AutoPostBack="true" OnSelectedIndexChanged="drp_accesstype_SelectedIndexChanged">
                                                                            <asp:ListItem Text="Default" Value="Default"></asp:ListItem>
                                                                            <asp:ListItem Text="Admission Number" Value="Admission Number"></asp:ListItem>
                                                                        </asp:DropDownList>
                                                                        <asp:HiddenField ID="hd_feesid" Value='<%# Eval("ATypeId") %>' runat="server" />
                                                                        <asp:HiddenField ID="hd_pid" Value='<%# Eval("PId") %>' runat="server" />

                                                                    </td>
                                                                    <td class="access-datas">
                                                                        <table>
                                                                            <tbody>
                                                                                <tr id="tr_default" runat="server">
                                                                                    <td>

                                                                                        <asp:DropDownList ID="drp_course" runat="server" Width="120px" AutoPostBack="true" OnSelectedIndexChanged="drp_course_SelectedIndexChanged">
                                                                                        </asp:DropDownList>
                                                                                    </td>
                                                                                    <td>
                                                                                        <asp:DropDownList ID="drp_batches" runat="server" Width="120px">
                                                                                        </asp:DropDownList>


                                                                                    </td>
                                                                                    <td>
                                                                                        <asp:DropDownList ID="drp_category" runat="server" Width="120px">
                                                                                        </asp:DropDownList>

                                                                                    </td>
                                                                                </tr>
                                                                                <tr id="tr_admisionno" runat="server" visible="false">
                                                                                    <td colspan="3">
                                                                                        <input style="width: 364px !important" placeholder="Admission Numbers seperated by commas" id="tbx_admisionno" runat="server" type="text">
                                                                                    </td>
                                                                                </tr>
                                                                            </tbody>
                                                                        </table>
                                                                    </td>
                                                                    <td>
                                                                        <input placeholder="Amount" style="width: 70px !important; padding-top: 6px; padding-bottom: 6px;" id="tbx_amount"  Value='<%# Eval("Aamount") %>' runat="server" type="text">
                                                                    </td>
                                                                    <td>                                                                        
                                                                        <div style="position: relative;">
                                                                            <a href="javascript:void(0);" runat="server" onserverclick="lnk_del_appliacable_ServerClick" id="lnk_del_appliacable" title="Click to remove access" class="remove-access fees-trash" style="top: -10px; right: -15px;" data-row="0"></a>
                                                                        </div>
                                                                    </td>
                                                                </tr>
                                                            </tbody>
                                                        </table>
                                                    </div>
                                                </div>
                                            </div>
                                        </ItemTemplate>

                                    </asp:ListView>

                                    <asp:HiddenField ID="hd_mainpid" Value='<%# Eval("PId") %>' runat="server" />


                                    <a href="javascript:void(0);" id="lnk_add_particulars" runat="server" onserverclick="lnk_add_particulars_ServerClick" title="Click to add access to another group" class="add-particular-access" style="font-size: 12px;" data-row="0"><strong>+ Add access</strong></a>
                                </div>

                            </div>
                        </div>

                    </ItemTemplate>
                </asp:ListView>


                <!-- Fee particulars here -->

                <div>
                    <table width="100%">
                        <tbody>
                            <tr>
                                <td colspan="3">
                                    <a href="javascript:void(0);" title="Click to add another particular" id="add_fee_particular" runat="server" onserverclick="add_fee_particular_ServerClick" style="font-size: 14px;"><strong>+ Add particular</strong></a> <strong>/</strong> <a title="Press `Ctrl + Enter` to add another particular"><strong>{ Press Ctrl + Enter }</strong></a>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </div>

            </div>
        </div>
        <div class="row buttons">
            <div class="col-md-12">
                <button id="btn_save" runat="server" onserverclick="btn_save_ServerClick" class="a_tag-btn"><span>Setup Subscription >></span></button>
            </div>
        </div>

    </div>
</asp:Content>

