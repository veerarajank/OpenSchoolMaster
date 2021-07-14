<%@ Page Title="" Language="C#" MasterPageFile="~/Adm_Course_Settings.master" AutoEventWireup="true" CodeFile="Adm_Previous_Year_Course.aspx.cs" Inherits="Adm_Previous_Year_Course" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
     <div class="cont_right formWrapper">
        <h1>Manage Previous Year Courses and Batch</h1>

         <div class="formCon">
                <div class="formConInner">
                    <div class="c_batch_tbar-subwise">
                        <div class="filter-box-table">
                            <table style="width: 100%" border="0" cellspacing="0" cellpadding="0" class="s_search">
                                <tbody>
                                    <tr>
                                        <td style="width: 13%"><strong>Academic Year</strong></td>
                                        <td style="width: 2%">:</td>
                                        <td style="width: 25%">
                                            <asp:DropDownList ID="drp_academic_year" Font-Size="10" runat="server" AutoPostBack="true" OnSelectedIndexChanged="drp_academic_year_SelectedIndexChanged" class="form-control chosen-select">
                                            </asp:DropDownList>
                                        </td>
                                        <td style="width: 2%"></td>
                                        <td style="width: 23%">                                         
                                        </td>
                                    </tr>
                                   
                                </tbody>
                            </table>
                        </div>
                    </div>
                </div>
            </div>

        
        <div id="academic-years-grid" class="mcb_Con">
            <asp:ListView ID="lv_listview" runat="server" GroupPlaceholderID="groupPlaceHolder1"
                ItemPlaceholderID="itemPlaceHolder1">
                <LayoutTemplate>
                    <asp:PlaceHolder runat="server" ID="groupPlaceHolder1"></asp:PlaceHolder>
                </LayoutTemplate>
                <GroupTemplate>

                    <asp:PlaceHolder runat="server" ID="itemPlaceHolder1"></asp:PlaceHolder>

                </GroupTemplate>
                <ItemTemplate>
                    <div class="according-bg" style="border: 1px solid #e4d6d6; margin-bottom: 0px; height: 55px; background-color: #f6f3f3">
                        <div class="mcb-strip-bg">
                            <ul class="posctn-ul2">
                                <li class="gtcol1" style="cursor: pointer;">
                                    <asp:LinkButton ID="lnk_search" CommandName="view" CommandArgument='<%# Eval("id") %>' OnCommand="lnk_search_Command" runat="server" Visible='<%# Convert.ToBoolean(Session["Access_View"])%>'>
                                    <%# Eval("name") %>																	<span>
                                    <div style="float: left" id="batchcount1"><%# Eval("batchescnt") %>	</div>
                                     <div style="float: left">- Batch(s), &nbsp;</div>        
                                            <div style="float: left" id="subcount1"><%# Eval("semestercnt") %>	</div>
                                    <div style="float: left">- Semester(s)</div>                                        
                                </span>
                                    </asp:LinkButton>
                                </li>

                            </ul>
                            <ul class="posctn-ul1">
                                <li class="e-gtcol1">

                                    <asp:LinkButton ID="lnk_edit" class="edit" CommandName="edit" CommandArgument='<%# Eval("id") %>' OnCommand="lnk_edit_Command" runat="server" Visible='<%# Convert.ToBoolean(Session["Access_Edit"])%>'>
                                                 Edit
                                    </asp:LinkButton>&nbsp;

                                </li>
                                <li class="e-gtcol1">

                                    <asp:LinkButton ID="lnk_del" OnClick='DeletePopup("<%# Eval("id") %>")' Style='<%# Convert.ToBoolean(Session["Access_Trash"])==true ? "visibility:visible": "visibility:hidden" %>'>
                                        Delete
                                    </asp:LinkButton>

                                </li>

                                <li class="e-gtcol1">
                                    <asp:LinkButton ID="lnk_batches" class="add" CommandName="batch" CommandArgument='<%# Eval("id") %>' OnCommand="lnk_batches_Command" runat="server" Visible='<%# Convert.ToBoolean(Session["Access_Edit"])%>'>
                                                 Manage Batch
                                    </asp:LinkButton>&nbsp;
                                </li>

                                <li class="e-gtcol1">
                                    <asp:LinkButton ID="lnk_semester" class="add" CommandName="semester" CommandArgument='<%# Eval("id") %>' OnCommand="lnk_semester_Command" runat="server" Visible='<%# Convert.ToBoolean(Session["Access_Edit"])%>'>
                                                 Manage Semester
                                    </asp:LinkButton>&nbsp;
                                </li>

                            </ul>
                        </div>
                    </div>


                    <div class="clear"></div>

                </ItemTemplate>

            </asp:ListView>
        </div>
        <br />
        <br />

        <div class="cours-strip" id="dropwin1" runat="server" visible="false">
            <div class="semester-block">
                <div class="cours-table-hed"><h3>Batch</h3></div>
                
                    
                    <asp:ListView ID="lv_batchlist" runat="server" GroupPlaceholderID="groupPlaceHolder1"
                        ItemPlaceholderID="itemPlaceHolder1">
                        <LayoutTemplate>
                            <table  style="width:100%" border="0" cellspacing="0" cellpadding="0" class="cours-inner-table">
                                <thead>

                                    <tr>
                                        <th style="width:15%">Batch Name</th>
                                        <th style="width:25%">Batch Code</th>
                                        <th style="width:15%">Batch Status</th>
                                        <th style="width:30%">Actions</th>
                                    </tr>
                                </thead>
                                <asp:PlaceHolder runat="server" ID="groupPlaceHolder1"></asp:PlaceHolder>

                            </table>
                        </LayoutTemplate>
                        <GroupTemplate>
                            <tr style="border: 1px solid #b9c7d0!important;">
                                <asp:PlaceHolder runat="server" ID="itemPlaceHolder1"></asp:PlaceHolder>
                            </tr>
                        </GroupTemplate>
                        <ItemTemplate>

                            <td style="text-align: left; padding-left: 10px; font-weight: bold;">
                                <%# Eval("name") %>
                            </td>
                            <td>
                                <%# Eval("code") %>
                            </td>
                            <td>
                                <%# Convert.ToString(Eval("status")).ToLower()=="1" ? "<span class='bg-green'>&nbsp;Active&nbsp;</span>" : "<span class='bg-red'>&nbsp;Inactive&nbsp;</span>"  %>
                            </td>
                            <td class="sub_act">


                                <asp:LinkButton ID="lnk_batchedit" class="add" CommandName="edit" CommandArgument='<%# Eval("id") %>' OnCommand="lnk_batchedit_Command" runat="server" Visible='<%# Convert.ToBoolean(Session["Access_Edit"])%>'>
										Edit
                                </asp:LinkButton>&nbsp;                              
                                <asp:LinkButton ID="lnk_batchsemester" class="add" CommandName="manange" CommandArgument='<%# Eval("id") %>' OnCommand="lnk_batchsemester_Command" runat="server" Visible='<%# Convert.ToBoolean(Session["Access_Edit"])%>'>
										Manage Semester
                                </asp:LinkButton>&nbsp;

                               


                            </td>

                        </ItemTemplate>
                        <EmptyDataTemplate>
                             <table  style="width:100%" border="0" cellspacing="0" cellpadding="0" class="cours-inner-table">
                                <thead>

                                    <tr>
                                       <th style="width:15%">Batch Name</th>
                                        <th style="width:25%">Batch Code</th>
                                        <th style="width:15%">Batch Status</th>
                                        <th style="width:30%">Actions</th>
                                    </tr>
                                </thead>

                                <tr style="border: 1px solid #b9c7d0!important;">
                                    <td colspan="4">No Results Found
                                    </td>
                                </tr>
                            </table>
                        </EmptyDataTemplate>
                    </asp:ListView>
               
            </div>
        </div>

    </div>


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

