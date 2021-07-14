<%@ Page Title="" Language="C#" MasterPageFile="~/Adm_FeesSettings.master" AutoEventWireup="true" CodeFile="Adm_Fee.aspx.cs" Inherits="Adm_Fee" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="cont_right formWrapper" style="width:100%">
        <h1>Fees Dashboard</h1>
        <div class="overview" style="padding-top: 0px;">
            <div class="overviewbox ovbox1" style="margin-left: 0px;">
                <h1><strong>Total Fee Categories</strong></h1>
                <div class="ovrBtm" id="div_totalcategory" runat="server">
                    0                                  
                </div>
            </div>
            <div class="overviewbox ovbox2">
                <h1><strong>Invoices Generated For</strong></h1>
                <div class="ovrBtm" id="div_totalinvoice" runat="server">
                    0                                  
                </div>
            </div>
            <div class="clear"></div>
        </div>
        <br />
        <!-- categories -->
        <div class="pdtab_Con">
            <div style="font-size: 13px; padding: 5px 0px">
                <strong>Fee Categories</strong>
            </div>

            <asp:ListView ID="lv_category" runat="server" GroupPlaceholderID="groupPlaceHolder1"
                ItemPlaceholderID="itemPlaceHolder1" OnPagePropertiesChanging="lv_category_PagePropertiesChanging">
                <LayoutTemplate>
                    <table width="100%" cellspacing="0" cellpadding="0" border="0">
                        <tr class="pdtab-h">
                            <td height="18" align="center">Category Name</td>
                            <td align="center">Date Created</td>
                            <td align="center">Created By</td>
                            <td align="center">Invoice Generated</td>
                            <td align="center" colspan="2">Actions</td>
                        </tr>
                        <asp:PlaceHolder runat="server" ID="groupPlaceHolder1"></asp:PlaceHolder>
                        <tr style="background-color: #EEEEE0; border: 1px solid #b9c7d0!important" class="tableInfoTH">
                            <td colspan="6" class="tableInfoTD">
                                <asp:DataPager ID="DataPager1" runat="server" PagedControlID="lv_category" PageSize="10" class="googleNavegationBar">
                                    <Fields>
                                        <aspPage:GooglePagerField NextPageImageUrl="~/Images/button_arrow_right.gif" PreviousPageImageUrl="~/Images/button_arrow_left.gif" />
                                    </Fields>
                                </asp:DataPager>
                            </td>
                        </tr>
                    </table>
                </LayoutTemplate>
                <GroupTemplate>
                    <tr style="border: 1px solid #b9c7d0!important;">
                        <asp:PlaceHolder runat="server" ID="itemPlaceHolder1"></asp:PlaceHolder>
                    </tr>
                </GroupTemplate>
                <ItemTemplate>

                    <td>
                        <%# Eval("name") %>
                    </td>                   
                      <td>
                        <%# Eval("created_on") %>
                    </td>      
                      <td>
                        <%# Eval("created_by") %>
                    </td>      
                     <td>
                        <%# Eval("is_invoicegenerated") %>
                    </td> 

                    <td style="color:red">
                        <asp:LinkButton ID="lnk_search" CommandName="createsubscripe" CommandArgument='<%# Eval("id") %>' OnCommand="lnk_search_Command" runat="server" Visible='<%# Convert.ToInt32(Eval("is_subsribe"))==0 ? true : false %>'>
                           <span style="color:red"> Create Subscriptions&nbsp;                 </span>
                        </asp:LinkButton> 
                        <asp:LinkButton ID="btn_generate_invoice" OnClick='Generate_Invoice("<%# Eval("id") %>")'  Style='<%# Convert.ToInt32(Eval("is_generateinvoice"))!=0 && Convert.ToInt32(Eval("is_viewinvoice"))==0 ? "visibility:visible;display:block": "visibility:hidden;display:none" %>'>
                            Generate Invoice(s)&nbsp;                
                        </asp:LinkButton>
                        <asp:LinkButton ID="LinkButton2" CommandName="viewinvoice" CommandArgument='<%# Eval("id") %>' OnCommand="lnk_search_Command" runat="server" Visible='<%# Convert.ToInt32(Eval("is_viewinvoice"))!=0  ? true : false%>'>
                            <span style="color:red"> View Invoice(s)&nbsp;  </span>                
                        </asp:LinkButton>
                    </td>                        
                    <td style="color:red">
                        <asp:LinkButton ID="lnk_edit" CommandName="edit" CommandArgument='<%# Eval("id") %>' OnCommand="lnk_edit_Command" runat="server" Visible='<%# Convert.ToBoolean(Session["Access_Edit"])%>'>
                                     Edit&nbsp;|&nbsp;
                        </asp:LinkButton>&nbsp;
                      <asp:LinkButton ID="lnk_del" OnClick='DeletePopup("<%# Eval("id") %>")' Style='<%# Convert.ToBoolean(Session["Access_Trash"])==true ? "visibility:visible": "visibility:hidden" %>'>
                            Remove
                      </asp:LinkButton>

                    </td>

                </ItemTemplate>
                <EmptyDataTemplate>
                    <table width="100%" cellspacing="0" cellpadding="0" border="0">
                        <tbody>
                            <tr class="pdtab-h">
                                <td height="18" align="center">Category Name</td>
                                <td align="center">Date Created</td>
                                <td align="center">Created By</td>
                                <td align="center">Invoice Generated</td>
                                <td align="center">Actions</td>
                            </tr>
                            <tr>
                                <td colspan="5">No Records Found</td>
                            </tr>
                        </tbody>
                    </table>
                </EmptyDataTemplate>
            </asp:ListView>



            <div class="clear"></div>
        </div>
    </div>
    <script src="plugins/jQuery/jquery-2.2.3.min.js"></script>
      <asp:HiddenField ID="hd_id" runat="server" />
    <script src="plugins/jQuery/jquery-2.2.3.min.js"></script>
    <script type="text/javascript">
        function Generate_Invoice(id) {
            $('#' + "<%=hd_id.ClientID %>").val(id);
            $('.bs-example-modal-sm').modal('show');

        }


    </script>
    <div class="modal fade bs-example-modal-sm" tabindex="-1" role="dialog" aria-labelledby="mySmallModalLabel">
        <div class="modal-dialog modal-sm" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                    <h4 class="modal-title" id="H3">Are you want to generate invoice Yes/No? </h4>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">No</button>
                    <button type="button" id="btn_generateinvoice" causesvalidation="true" validationgroup="vd_save" runat="server" onserverclick="btn_generateinvoice_ServerClick" class="btn btn-primary">Yes</button>
                </div>
            </div>
        </div>
    </div>
</asp:Content>

