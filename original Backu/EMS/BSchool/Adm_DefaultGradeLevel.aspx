<%@ Page Title="" Language="C#" MasterPageFile="~/Adm_Examination.master" AutoEventWireup="true" CodeFile="Adm_DefaultGradeLevel.aspx.cs" Inherits="Adm_DefaultGradeLevel" %>

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
        <h4><b>Set Default Grading Levels</b></h4>

        <div class="button-bg">
            <div class="top-hed-btn-left"></div>
            <div class="top-hed-btn-right">
                <ul>
                    <li>
                        <button id="btn_new" runat="server" onserverclick="btn_new_ServerClick" class="a_tag-btn"><span>Create Grading Level</span></button>
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
                                <th>Min Score
                                </th>
                                <th class="col-md-2">Action
                                </th>
                            </thead>
                            <asp:PlaceHolder runat="server" ID="groupPlaceHolder1"></asp:PlaceHolder>
                            <tr style="background-color: #EEEEE0; border: 1px solid #b9c7d0!important" class="tableInfoTH">
                                <td colspan="3" class="tableInfoTD">
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
                            <%# Eval("grade") %>
                        </td>
                        <td>
                            <%# Eval("minmark") %>
                        </td>
                        <td>

                            <asp:LinkButton ID="lnk_search" CommandName="view" CommandArgument='<%# Eval("id") %>' OnCommand="lnk_search_Command" runat="server" Visible='<%# Convert.ToBoolean(Session["Access_View"])%>'>
                                    <img src="gridview/view.png" alt="View">            
                            </asp:LinkButton>
                            &nbsp;
                     <asp:LinkButton ID="lnk_edit" CommandName="edit" CommandArgument='<%# Eval("id") %>' OnCommand="lnk_edit_Command" runat="server" Visible='<%# Convert.ToBoolean(Session["Access_Edit"])%>'>
                                    <img src="gridview/update.png" alt="Update">            
                     </asp:LinkButton>&nbsp;
                      <asp:LinkButton ID="lnk_del" OnClick='DeletePopup("<%# Eval("id") %>")' Style='<%# Convert.ToBoolean(Session["Access_Trash"])==true ? "visibility:visible": "visibility:hidden" %>'>
                        <img src="gridview/delete.png" alt="Delete">                                    
                      </asp:LinkButton>


                        </td>

                    </ItemTemplate>
                    <EmptyDataTemplate>
                        <table class="items" style="border: 1px solid #b9c7d0!important;">
                            <thead style="border: 1px solid #b9c7d0 !important">
                                <th>Name
                                </th>
                                <th>Min Score
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

        <h4><b>Create New Grading Level</b></h4>
        <div class="button-bg">
            <div class="top-hed-btn-left"></div>
            <div class="top-hed-btn-right">
                <ul>
                    <li>
                        <a class="a_tag-btn" href="Adm_DefaultGradeLevel.aspx?mode=search"><span>Manage Grading Level</span></a></li>
                </ul>
            </div>
        </div>

        <p class="note" runat="server" id="field_mandary">Fields with <span class="required">*</span> are required.</p>
        <div class="formCon">
            <div class="formConInner">
                <div class="row">
                    <div class="txtfld-col">
                        <label for="Grade_Name" class="required">Name <span class="required">*</span></label>
                        <asp:TextBox ID="tbx_name" runat="server" AutoCompleteType="Disabled" MaxLength="120"></asp:TextBox>
                        <asp:RequiredFieldValidator runat="server" CssClass="input-notification-error  error-simple png_bg" ValidationGroup="vd_save" ID="reqdocumentname" ControlToValidate="tbx_name" ErrorMessage="Grade Name cannot be blank." />
                    </div>

                </div>
                <div class="row">
                    <div class="txtfld-col">
                        <label for="Grade_Minmark" class="required">Min Score<span class="required">*</span></label>
                        <asp:TextBox ID="tbx_minscore" runat="server" AutoCompleteType="Disabled" MaxLength="3"></asp:TextBox>
                        <asp:RequiredFieldValidator runat="server" CssClass="input-notification-error  error-simple png_bg" ValidationGroup="vd_save" ID="RequiredFieldValidator1" ControlToValidate="tbx_minscore" ErrorMessage="Minimum Score cannot be blank." />
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
                        <button id="btn_createnewgrade" validationgroup="vd_save" runat="server" style="width: 150px" onserverclick="btn_createnewgrade_ServerClick" class="a_tag-btn"><span>Save</span></button>
                    </li>
                </ul>
            </div>
        </div>

    </div>


    <div id="update_section" runat="server">

        <h4><b>Update Grading Level</b></h4>
        <div class="form">
            <div class="button-bg">
                <div class="top-hed-btn-left"></div>
                <div class="top-hed-btn-right">
                    <ul>
                        <li>
                            <a class="a_tag-btn" href="Adm_DefaultGradeLevel.aspx?mode=add"><span>Add Grading Level</span></a>            </li>
                        <li>
                            <a class="a_tag-btn" href="Adm_DefaultGradeLevel.aspx?mode=search"><span>Manage Grading Level</span></a>            </li>
                    </ul>
                </div>
            </div>


            <p class="note" id="edit_field_mandary" runat="server">Fields with <span class="required">*</span> are required.</p>

            <div class="formCon">
                <div class="formConInner">


                    <div class="row">
                        <div class="txtfld-col">
                            <label for="Grade_Name" class="required">Name<span class="required">*</span></label>
                            <asp:TextBox ID="tbx_edit_name" runat="server" AutoCompleteType="Disabled" MaxLength="120"></asp:TextBox>
                            <asp:RequiredFieldValidator runat="server" CssClass="input-notification-error  error-simple png_bg" ValidationGroup="vd_editsave" ID="rd_editdocumentname" ControlToValidate="tbx_edit_name" ErrorMessage="Grade Name cannot be blank." />
                        </div>

                    </div>
                    <div class="row">
                        <div class="txtfld-col">
                           <label for="Grade_Minmark" class="required">Min Score<span class="required">*</span></label>
                        <asp:TextBox ID="tbx_edit_minscore" runat="server" AutoCompleteType="Disabled" MaxLength="3"></asp:TextBox>
                        <asp:RequiredFieldValidator runat="server" CssClass="input-notification-error  error-simple png_bg" ValidationGroup="vd_editsave" ID="rd_editminscore" ControlToValidate="tbx_edit_minscore" ErrorMessage="Minimum Score cannot be blank." />

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

        <h4><b>View Grading Level</b></h4>

        <div class="button-bg">
            <div class="top-hed-btn-left"></div>
            <div class="top-hed-btn-right">
                <ul>
                    <li>
                        <button id="btn_edit" runat="server" onserverclick="btn_edit_ServerClick" class="a_tag-btn"><span>Edit</span></button>

                    </li>
                    <li>
                        <button id="btn_manage" runat="server" onserverclick="btn_manage_ServerClick" class="a_tag-btn"><span>Manage Grading Level</span></button>
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
                            <label id="lbl_name" runat="server"></label>

                        </td>
                    </tr>
                    <tr>
                        <td width="20%">
                            <b>Min Score</b>
                        </td>
                        <td width="3%"><strong>:</strong></td>
                        <td width="77%">
                            <label id="lbl_minscore" runat="server"></label>

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
        $(function () {
            $('#' + '<%= tbx_minscore.ClientID %>').numeric();
            $('#' + '<%= tbx_edit_minscore.ClientID %>').numeric();
            
        });

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

