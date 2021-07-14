<%@ Page Title="" Language="C#" MasterPageFile="~/Adm_Course_Settings.master" AutoEventWireup="true" CodeFile="Adm_Batch_Inactive_List.aspx.cs" Inherits="Adm_Batch_Inactive_List" %>

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

        select {
            font-size: 12px;
        }
    </style>

    <div runat="server" id="search_section">

        <div class="cont_right formWrapper">
            <h1>Deactivated Batch Details</h1>


             <div id="academic-years-grid" class="grid-view">
              

                    <asp:ListView ID="lv_listview" runat="server" GroupPlaceholderID="groupPlaceHolder1"
                        ItemPlaceholderID="itemPlaceHolder1">
                        <LayoutTemplate>
                            <table class="items" style="border: 1px solid #b9c7d0!important;width:100%" >
                                <thead style="border: 1px solid #b9c7d0!important;">

                                    <th>Course
                                    </th>
                                    <th>Batch
                                    </th>
                                    <th>Batch Code
                                    </th>
                                    <th>Status
                                    </th>
                                    <th>Action
                                    </th>
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

                            <td>
                                <%# Eval("course_name") %>
                            </td>
                            <td>
                                <%# Eval("name") %>
                            </td>
                            <td>
                                <%# Eval("code") %>
                            </td>
                            <td>
                                <%# Convert.ToString(Eval("status")).ToLower()=="0" ? "&nbsp;Inactive&nbsp;" : "&nbsp;Deleted&nbsp;"  %>
                            </td>


                            <td>

                                <asp:LinkButton ID="lnk_search" CommandName="view" CommandArgument='<%# Eval("id") %>' OnCommand="lnk_search_Command" runat="server" Visible='<%# Convert.ToBoolean(Session["Access_View"])%>'>
                                    <img src="gridview/view.png" alt="View">            
                                </asp:LinkButton>
                                &nbsp;
                                <asp:LinkButton ID="lnk_edit" CommandName="edit" CommandArgument='<%# Eval("id") %>' OnCommand="lnk_edit_Command" runat="server" Visible='<%# Convert.ToBoolean(Session["Access_Edit"])%>'>
                                    <img src="gridview/update.png" alt="Update">            
                                </asp:LinkButton>&nbsp;
                     
                            </td>

                        </ItemTemplate>
                        <EmptyDataTemplate>
                            <table class="items" style="border: 1px solid #b9c7d0!important;width:100%">
                                <thead style="border: 1px solid #b9c7d0!important;">

                                    <th>Course
                                    </th>
                                    <th>Batch
                                    </th>
                                    <th>Batch Code
                                    </th>
                                    <th>Status
                                    </th>
                                    <th>Action
                                    </th>
                                </thead>
                                <tr style="border: 1px solid #b9c7d0!important;">
                                    <td colspan="5">No Results Found
                                    </td>
                                </tr>
                            </table>
                        </EmptyDataTemplate>
                    </asp:ListView>
                 </div>




           






        </div>
    </div>


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



</asp:Content>

