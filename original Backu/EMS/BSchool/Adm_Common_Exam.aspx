<%@ Page Title="" Language="C#" MasterPageFile="~/Adm_Examination.master" AutoEventWireup="true" CodeFile="Adm_Common_Exam.aspx.cs" Inherits="Adm_Common_Exam" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script src="plugins/jQuery/jquery-2.2.3.min.js"></script>
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


        <h4><b>Manage Common Exams</b></h4>

        <div class="button-bg">
            <div class="top-hed-btn-left"></div>
            <div class="top-hed-btn-right">
                <ul>
                    <li>
                        <button id="btn_new" runat="server" onserverclick="btn_new_ServerClick" class="a_tag-btn"><span>Create Exam</span></button>
                    </li>
                </ul>
            </div>
        </div>


        <div id="academic-years-grid" class="grid-view">

            <asp:ListView ID="lv_details" runat="server" GroupPlaceholderID="groupPlaceHolder1"
                ItemPlaceholderID="itemPlaceHolder1">
                <LayoutTemplate>
                    <table class="items" style="border: 1px solid #b9c7d0!important;">
                        <thead style="border: 1px solid #b9c7d0!important;">
                            <th>Name
                            </th>
                            <th>Exam Type
                            </th>
                            <th>Date Is Published
                            </th>
                            <th>Result Published
                            </th>
                            <th>Exam Date
                            </th>
                            <th class="col-md-2">Action
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
                        <%# Eval("exam_name") %>
                    </td>

                    <td>

                        <%# Convert.ToString(Eval("exam_type")).ToLower()=="1" ? "&nbsp;Marks&nbsp;" : Convert.ToString(Eval("exam_type")).ToLower()=="2" ? "&nbsp;Grades&nbsp;" : "Marks & Grades"  %>

                        
                    </td>
                    <td>
                        <%# Convert.ToString(Eval("date_is_published")).ToLower()=="1" ? "<span class='bg-green'>&nbsp;Yes&nbsp;</span>" : "<span class='bg-red'>&nbsp;No&nbsp;</span>"  %>
                    </td>
                    <td>
                        <%# Convert.ToString(Eval("result_is_published")).ToLower()=="1" ? "<span class='bg-green'>&nbsp;Yes&nbsp;</span>" : "<span class='bg-red'>&nbsp;No&nbsp;</span>"  %>
                    </td>
                    <td>
                        <%# Eval("exam_startdate") %>
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
                     <asp:LinkButton ID="lnk_manage" CommandName="manage" CommandArgument='<%# Eval("id") %>' OnCommand="lnk_manage_Command" runat="server" Visible='<%# Convert.ToBoolean(Session["Access_Edit"])%>'>
                                    <img src="gridview/exams_sub.png" alt="Manage">            
                     </asp:LinkButton>&nbsp;
                    </td>

                </ItemTemplate>
                <EmptyDataTemplate>
                    <table class="items" style="border: 1px solid #b9c7d0!important;">
                        <thead style="border: 1px solid #b9c7d0!important;">

                            <th>Name
                            </th>
                            <th>Exam Type
                            </th>
                            <th>Date Is Published
                            </th>
                            <th>Result Published
                            </th>
                            <th>Exam Date
                            </th>
                            <th class="col-md-2">Action
                            </th>
                        </thead>
                        <tr style="border: 1px solid #b9c7d0!important;">
                            <td colspan="6">No Results Found
                            </td>
                        </tr>
                    </table>
                </EmptyDataTemplate>
            </asp:ListView>

            <br />
            <br />
        </div>


    </div>

    <div id="add_section" runat="server">

        <h4><b>Create Common Exam</b></h4>
        <div class="button-bg">
            <div class="top-hed-btn-left"></div>
            <div class="top-hed-btn-right">
                <ul>
                    <li>

                        <asp:Button ID="btn_manage_classtiming" runat="server" OnClick="btn_manage_classtiming_Click" Text="Manage Common Exam" CssClass="a_tag-btn" />
                    </li>
                </ul>
            </div>
        </div>

        <p class="note" runat="server" id="field_mandary">Fields with <span class="required">*</span> are required.</p>
        <div class="formCon">
            <div class="formConInner">
                <h3>Enter the details of the exam details</h3>
                <div class="txtfld-col-btn">
                    <div class="txtfld-col">
                        <label for="name" class="required">Name <span class="required">*</span></label>
                        <asp:TextBox ID="tbx_name" AutoCompleteType="Disabled" runat="server" MaxLength="120"></asp:TextBox>

                    </div>
                    <div class="txtfld-col">
                        <label for="exam_type" class="required">Exam Type<span class="required">*</span></label>
                        <asp:DropDownList ID="drp_examtype" runat="server">
                            <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>
                            <asp:ListItem Text="Marks" Value="1"></asp:ListItem>
                            <asp:ListItem Text="Grades" Value="2"></asp:ListItem>
                            <asp:ListItem Text="Marks & Grades" Value="3"></asp:ListItem>
                        </asp:DropDownList>

                    </div>
                    <div class="txtfld-col">
                        <label for="exam_date" class="required">Exam Date<span class="required">*</span></label>
                        <asp:TextBox ID="tbx_examdate" runat="server" AutoCompleteType="Disabled" class="hasDatepicker"></asp:TextBox>
                    </div>
                </div>
                <div class="txtfld-col-btn">
                    <div class="txtfld-col">

                        <label for="isdate" class="required">Date Is Published<span class="required">*</span></label>
                        <asp:CheckBox ID="chk_isdatepublished" runat="server" Checked="true" />
                    </div>

                    <div class="txtfld-col">

                        <label for="isresult" class="required">Result Published<span class="required">*</span></label>
                        <asp:CheckBox ID="chk_isresultpublished" runat="server" Checked="true" />
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
                        <button id="btn_newtiming" runat="server" style="width: 150px" onserverclick="btn_newtiming_ServerClick" class="a_tag-btn"><span>Create Common Exam</span></button>
                    </li>
                </ul>
            </div>
        </div>

    </div>


    <div id="update_section" runat="server">

        <h4><b>Update Common Exam</b></h4>
        <div class="form">
            <div class="button-bg">
                <div class="top-hed-btn-left"></div>
                <div class="top-hed-btn-right">
                    <ul>
                        <li>

                            <asp:Button ID="btn_addclasstiming_up_screen" runat="server" Text="Add Exam" OnClick="btn_addclasstiming_up_screen_Click" CssClass="a_tag-btn" />
                        </li>
                        <li>
                            <asp:Button ID="btn_manageclasstiming_up_screen" runat="server" Text="Manage Exam" OnClick="btn_manageclasstiming_up_screen_Click" CssClass="a_tag-btn" />
                        </li>
                    </ul>
                </div>
            </div>


            <p class="note" id="edit_field_mandary" runat="server">Fields with <span class="required">*</span> are required.</p>
            <div class="formCon">
                <div class="formConInner">
                    <h3>Update the details of this exam details</h3>
                    <div class="txtfld-col-btn">
                        <div class="txtfld-col">
                            <label for="name" class="required">Name <span class="required">*</span></label>
                            <asp:TextBox ID="tbx_edit_name" AutoCompleteType="Disabled" runat="server" MaxLength="120"></asp:TextBox>

                        </div>
                        <div class="txtfld-col">
                            <label for="exam_type" class="required">Exam Type<span class="required">*</span></label>
                            <asp:DropDownList ID="drp_edit_examtype" runat="server">
                                <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>
                                <asp:ListItem Text="Marks" Value="1"></asp:ListItem>
                                <asp:ListItem Text="Grades" Value="2"></asp:ListItem>
                                <asp:ListItem Text="Marks & Grades" Value="3"></asp:ListItem>
                            </asp:DropDownList>

                        </div>
                        <div class="txtfld-col">
                            <label for="exam_date" class="required">Exam Date<span class="required">*</span></label>
                            <asp:TextBox ID="tbx_edit_examdate" runat="server" AutoCompleteType="Disabled" class="hasDatepicker"></asp:TextBox>
                        </div>
                    </div>
                    <div class="txtfld-col-btn">
                        <div class="txtfld-col">
                            <label for="isdate" class="required">Date Is Published<span class="required">*</span></label>
                            <asp:CheckBox ID="chk_edit_isdatepublished" runat="server" Checked="true" />
                        </div>
                        <div class="txtfld-col">
                            <label for="isresult" class="required">Result Published<span class="required">*</span></label>
                            <asp:CheckBox ID="chk_edit_isresultpublished" runat="server" Checked="true" />
                        </div>


                    </div>




                </div>
            </div>

            <div class="button-bg">
                <div class="top-hed-btn-left"></div>
                <div class="top-hed-btn-right">
                    <ul>
                        <li>
                            <button id="btn_update" runat="server" onserverclick="btn_update_ServerClick" style="width: 150px" class="a_tag-btn"><span>Save</span></button>
                        </li>
                    </ul>
                </div>
            </div>


        </div>



    </div>

    <div id="delete_section" runat="server">
    </div>
    <div id="view_section" runat="server">

        <h4><b>View Common Examg</b></h4>

        <div class="button-bg">
            <div class="top-hed-btn-left"></div>
            <div class="top-hed-btn-right">
                <ul>
                    <li>
                        <button id="btn_edit" runat="server" onserverclick="btn_edit_ServerClick" class="a_tag-btn"><span>Edit</span></button>

                    </li>
                    <li>
                        <button id="btn_manage" runat="server" onserverclick="btn_manage_ServerClick" class="a_tag-btn"><span>Manage Exam</span></button>
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
                        <td>
                            <b>Exam Type</b>
                        </td>
                        <td><strong>:</strong></td>
                        <td>
                            <label id="lbl_examtype" runat="server"></label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <b>Date Is Published</b>
                        </td>
                        <td><strong>:</strong></td>
                        <td>
                            <label id="lbl_isdatepublished" runat="server"></label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <b>Result Published</b>
                        </td>
                        <td><strong>:</strong></td>
                        <td>
                            <label id="lbl_isresultpublished" runat="server"></label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <b>Exam Date</b>
                        </td>
                        <td><strong>:</strong></td>
                        <td>
                            <span id="lbl_examdate" runat="server"></span>
                        </td>
                    </tr>
                </tbody>
            </table>
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
    <script type="text/javascript">
        $(function () {
            $('#' + "<%=tbx_examdate.ClientID %>").datepicker(
                { autoclose: true, format: "dd-mm-yyyy" }
           );

            $('#' + "<%=tbx_edit_examdate.ClientID %>").datepicker(
              { autoclose: true, format: "dd-mm-yyyy" }
         );




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

