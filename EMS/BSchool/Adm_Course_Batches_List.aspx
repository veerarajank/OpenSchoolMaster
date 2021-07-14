<%@ Page Title="" Language="C#" MasterPageFile="~/Adm_Course_Settings.master" AutoEventWireup="true" CodeFile="Adm_Course_Batches_List.aspx.cs" Inherits="Adm_Course_Batches_List" %>

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
            <h1>Batch Details</h1>

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
                                            <asp:Button ID="btn_new" runat="server" OnClick="btn_new_Click" CssClass="formbut-n" Text="Link Batch" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="5" style="height:3px">&nbsp;</td>
                                    </tr>
                                     <tr>
                                        <td style="width: 13%"><strong>Course Name</strong></td>
                                         <td style="width: 2%">:</td>
                                        <td style="width: 25%">
                                            <asp:Label ID="lbl_course" runat="server" Font-Bold="true"></asp:Label>
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

            <div class="mcb_Con" runat="server" id="active_batches">
                <asp:ListView ID="lv_listview" runat="server" GroupPlaceholderID="groupPlaceHolder1"
                    ItemPlaceholderID="itemPlaceHolder1">
                    <LayoutTemplate>
                       
                        <asp:PlaceHolder runat="server" ID="groupPlaceHolder1"></asp:PlaceHolder>
                    </LayoutTemplate>
                    <GroupTemplate>

                        <asp:PlaceHolder runat="server" ID="itemPlaceHolder1"></asp:PlaceHolder>

                    </GroupTemplate>
                    <ItemTemplate>
                        <div class="according-bg" style="border: 1px solid #e4d6d6; margin-bottom: 0px; height: 30px; background-color: #f6f3f3">
                            <div class="mcb-strip-bg">
                                <ul class="posctn-ul2">
                                    <li class="gtcol1" style="cursor: pointer;padding: 6px 9px;"><%# Eval("course_name") %>																	
                                    </li>
                                    <li class="gtcol1" style="cursor: pointer;padding: 6px 9px;"><%# Eval("name") %>																	
                                    </li>
                                     <li class="gtcol1" style='cursor: pointer;padding: 6px 9px;background-color: <%# Convert.ToString(Eval("status")).ToLower()=="1" ? "green;color:white" : "red;color:white"  %>'>
                                          <%# Convert.ToString(Eval("status")).ToLower()=="1" ? "&nbsp;Active&nbsp;" : "&nbsp;Inactive&nbsp;"  %>																
                                    </li>
                                </ul>
                                <ul class="posctn-ul1">                                  
                                 <li class="e-gtcol1" style='padding: 6px 9px;display:<%# Convert.ToString(Eval("status")).ToLower()=="0" ? "none" : "block"%>' >
                                    <a ID="lnk_del" class="add"  OnClick='DeletePopup("<%# Eval("id") %>")' Style='<%# Convert.ToBoolean(Session["Access_Trash"])==true ? "visibility:visible": "visibility:hidden;" %>'>
                                       <img src="gridview/delete.png" alt="Delete">    
                                    </a>
                                </li>
                                    <li class="e-gtcol1" style='padding: 6px 9px;display:<%# Convert.ToString(Eval("status")).ToLower()=="0" ? "block" : "none"%>'  >
                                    <a ID="A1" class="add"  OnClick='UpdatePopup("<%# Eval("id") %>")' Style='<%# Convert.ToBoolean(Session["Access_Trash"])==true ? "visibility:visible": "visibility:hidden;" %>'>
                                       <img src="gridview/os-genarateicon.png" alt="Update">    
                                    </a>
                                </li>
                                   

                                </ul>
                            </div>
                        </div>


                        <div class="clear" ></div>

                    </ItemTemplate>

                </asp:ListView>

                 <asp:ListView ID="lst_linkbatch" runat="server" GroupPlaceholderID="groupPlaceHolder1"
                    ItemPlaceholderID="itemPlaceHolder1">
                    <LayoutTemplate>
                         <b>LINK BATCH DETAILS</b>
                        <asp:PlaceHolder runat="server" ID="groupPlaceHolder1"></asp:PlaceHolder>
                    </LayoutTemplate>
                    <GroupTemplate>

                        <asp:PlaceHolder runat="server" ID="itemPlaceHolder1"></asp:PlaceHolder>

                    </GroupTemplate>
                    <ItemTemplate>
                        <div class="according-bg" style="border: 1px solid #e4d6d6; margin-bottom: 0px; height: 30px; background-color: #f6f3f3">
                            <div class="mcb-strip-bg">
                                <ul class="posctn-ul2">
                                    <li class="gtcol1" style="cursor: pointer;padding: 6px 9px;"><%# Eval("course_name") %>																	
                                    </li>
                                    <li class="gtcol1" style="cursor: pointer;padding: 6px 9px;"><%# Eval("name") %>																	
                                    </li>
                                     <li class="gtcol1" style='cursor: pointer;padding: 6px 9px;background-color: <%# Convert.ToString(Eval("status")).ToLower()=="1" ? "green;color:white" : "red;color:white"  %>'>
                                          <%# Convert.ToString(Eval("status")).ToLower()=="1" ? "&nbsp;Active&nbsp;" : "&nbsp;Inactive&nbsp;"  %>																
                                    </li>
                                </ul>
                                <ul class="posctn-ul1">                                  
                                 <li class="e-gtcol1" style='padding: 6px 9px;' >
                                    <a ID="lnk_del" class="add"  OnClick='Link_Batch("<%# Eval("id") %>")' >
                                       <img src="gridview/os-approveicon.png" alt="link">    
                                    </a>
                                </li>
                                  
                                   

                                </ul>
                            </div>
                        </div>


                        <div class="clear" ></div>

                    </ItemTemplate>

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

        function Link_Batch(id) {
            $('#' + "<%=hd_id.ClientID %>").val(id);
            $('.link_batch').modal('show');

         }
        function UpdatePopup(id) {
            $('#' + "<%=hd_id.ClientID %>").val(id);
            $('.update_batch').modal('show');

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

    <div class="modal fade update_batch" tabindex="-1" role="dialog" aria-labelledby="mySmallModalLabel">
        <div class="modal-dialog modal-sm" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                    <h4 class="modal-title" id="H1">Are you want to reset Yes/No? </h4>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">No</button>
                    <button type="button" id="btn_reset" runat="server" onserverclick="btn_reset_ServerClick" class="btn btn-primary">Yes</button>
                </div>
            </div>
        </div>
    </div>

    <div class="modal fade link_batch" tabindex="-1" role="dialog" aria-labelledby="mySmallModalLabel">
        <div class="modal-dialog modal-sm" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                    <h4 class="modal-title" id="H2">Are you want to link batch  Yes/No? </h4>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">No</button>
                    <button type="button" id="lnk_linkbatch" runat="server" onserverclick="lnk_linkbatch_ServerClick" class="btn btn-primary">Yes</button>
                </div>
            </div>
        </div>
    </div>

</asp:Content>

