<%@ Page Title="" Language="C#" MasterPageFile="~/Adm_Teacher_Settings.master" AutoEventWireup="true" CodeFile="Adm_Teacher_Log.aspx.cs" Inherits="Adm_Teacher_Log" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
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
        <h4><b>Manage Log Category</b></h4>

        <div class="button-bg">
            <div class="top-hed-btn-left"></div>
            <div class="top-hed-btn-right">
                <ul>
                    <li>
                        <button id="btn_new" runat="server" onserverclick="btn_new_ServerClick" class="a_tag-btn"><span>Create Log Category</span></button>
                    </li>
                </ul>
            </div>
        </div>


        <div id="academic-years-grid" class="grid-view">

            <div class="summary" id="lbl_cnt" runat="server" style="font-size: 12px;">
                Displaying 2 result(s).
            </div>
            <div class="grid_table_con">
                <asp:ListView ID="lv_listview" runat="server" GroupPlaceholderID="groupPlaceHolder1"
                    ItemPlaceholderID="itemPlaceHolder1" OnPagePropertiesChanging="lv_listview_PagePropertiesChanging">
                    <LayoutTemplate>
                        <table class="items" style="border: 1px solid #b9c7d0!important;">
                            <thead>

                                <th>Name
                                </th>                                                           
                                <th class="col-md-2">Action
                                </th>
                            </thead>
                            <asp:PlaceHolder runat="server" ID="groupPlaceHolder1"></asp:PlaceHolder>
                            <tr style="background-color: #EEEEE0; border: 1px solid #b9c7d0!important" class="tableInfoTH">
                                <td colspan="2" class="tableInfoTD">
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
                        <tr style="border: 1px solid #b9c7d0!important;">
                            <asp:PlaceHolder runat="server" ID="itemPlaceHolder1"></asp:PlaceHolder>
                        </tr>
                    </GroupTemplate>
                    <ItemTemplate>

                        <td>
                            <%# Eval("log_name") %>
                        </td>                                            
                        <td>

                            <asp:LinkButton ID="lnk_search" CommandName="view" CommandArgument='<%# Eval("log_id") %>' OnCommand="lnk_search_Command" runat="server" Visible='<%# Convert.ToBoolean(Session["Access_View"])%>'>
                                    <img src="gridview/view.png" alt="View">            
                            </asp:LinkButton>
                            &nbsp;
                     <asp:LinkButton ID="lnk_edit" CommandName="edit" CommandArgument='<%# Eval("log_id") %>' OnCommand="lnk_edit_Command" runat="server" Visible='<%# Convert.ToBoolean(Session["Access_Edit"])%>'>
                                    <img src="gridview/update.png" alt="Update">            
                     </asp:LinkButton>&nbsp;
                      <asp:LinkButton ID="lnk_del" OnClick='DeletePopup("<%# Eval("log_id") %>")' Style='<%# Convert.ToBoolean(Session["Access_Trash"])==true ? "visibility:visible": "visibility:hidden" %>'>
                        <img src="gridview/delete.png" alt="Delete">                                    
                      </asp:LinkButton>


                        </td>

                    </ItemTemplate>
                    <EmptyDataTemplate>
                        <table class="items" style="border: 1px solid #b9c7d0!important;">
                            <thead style="border: 1px solid #b9c7d0 !important">
                               <th>Name
                                </th>                                                         
                                <th class="col-md-2">Action
                                </th>
                            </thead>
                            <tr style="border: 1px solid #b9c7d0!important;">
                                <td colspan="3">No Results Found
                                </td>
                            </tr>
                        </table>
                    </EmptyDataTemplate>
                </asp:ListView>
            </div>

        </div>
    </div>

    <div id="add_section" runat="server">

        <h4><b>Create New Log Category</b></h4>
        <div class="button-bg">
            <div class="top-hed-btn-left"></div>
            <div class="top-hed-btn-right">
                <ul>
                    <li>
                        <a class="a_tag-btn" href="Adm_Teacher_Log.aspx?mode=search"><span>Manage Log Category</span></a></li>
                </ul>
            </div>
        </div>

        <p class="note" runat="server" id="field_mandary">Fields with <span class="required">*</span> are required.</p>
        <div class="formCon">
            <div class="formConInner">
                <div class="row">
                    <div class="txtfld-col">
                        <label for="Log_Name" class="required">Name <span class="required">*</span></label>
                        <asp:TextBox ID="tbx_logname" runat="server" AutoCompleteType="Disabled" MaxLength="120"></asp:TextBox>
                        <asp:RequiredFieldValidator runat="server" CssClass="input-notification-error  error-simple png_bg" ValidationGroup="vd_save" ID="reqdocumentname" ControlToValidate="tbx_logname" ErrorMessage="Log cannot be blank." />
                    </div>

                </div>

            </div>
        </div>
        <p style="color: red; font-size: 12px; font-weight: bold" runat="server" id="lbl_error"></p>


        <div class="button-bg">
            <div class="top-hed-btn-left"></div>
            <div class="top-hed-btn-right">
                <ul>
                    <li>
                        <button id="btn_createeventtype" validationgroup="vd_save" runat="server" style="width: 150px" onserverclick="btn_createeventtype_ServerClick" class="a_tag-btn"><span>Save</span></button>
                    </li>
                </ul>
            </div>
        </div>

    </div>


    <div id="update_section" runat="server">

        <h4><b>Update Log Category</b></h4>
        <div class="form">
            <div class="button-bg">
                <div class="top-hed-btn-left"></div>
                <div class="top-hed-btn-right">
                    <ul>
                        <li>
                            <a class="a_tag-btn" href="Adm_Teacher_Log.aspx?mode=add"><span>Add Log Category</span></a>            </li>
                        <li>
                            <a class="a_tag-btn" href="Adm_Teacher_Log.aspx?mode=search"><span>Manage Log Category</span></a>            </li>
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

        <h4><b>View Log Category</b></h4>

        <div class="button-bg">
            <div class="top-hed-btn-left"></div>
            <div class="top-hed-btn-right">
                <ul>
                    <li>
                        <button id="btn_edit" runat="server" onserverclick="btn_edit_ServerClick" class="a_tag-btn"><span>Edit</span></button>

                    </li>
                    <li>
                        <button id="btn_manage" runat="server" onserverclick="btn_manage_ServerClick" class="a_tag-btn"><span>Manage Log Category</span></button>
                    </li>
                </ul>
            </div>

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
                    
                </tbody>
            </table>
        </div>


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

