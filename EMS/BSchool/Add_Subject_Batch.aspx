<%@ Page Title="" Language="C#" MasterPageFile="~/AcademicConfiguration.master" AutoEventWireup="true" CodeFile="Add_Subject_Batch.aspx.cs" Inherits="Add_Subject_Batch" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
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

        select {
            font-size: 12px;
        }
    </style>

    <div class="cont_right formWrapper">
        <h1>Course Semester Details</h1>
        <div class="formCon">
            <div class="formConInner">
                <div class="c_batch_tbar-subwise">
                    <div class="filter-box-table">
                        <table style="width: 100%" border="0" cellspacing="0" cellpadding="0" class="s_search">
                            <tbody>
                                <tr>
                                    <td style="width: 13%"><strong>Academic Year</strong></td>
                                    <td style="width: 2%"></td>
                                    <td style="width: 25%">
                                        <asp:DropDownList ID="drp_academic_year" Font-Size="10" runat="server" AutoPostBack="true" OnSelectedIndexChanged="drp_academic_year_SelectedIndexChanged" class="form-control chosen-select">
                                        </asp:DropDownList>
                                    </td>
                                    <td style="width: 2%"></td>
                                    <td style="width: 23%">
                                        <asp:Button ID="btn_new" runat="server" OnClick="btn_new_Click" CssClass="formbut-n" Text="Add Semester" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="5" style="height: 3px"></td>
                                </tr>
                                <tr>
                                    <td style="width: 13%"><strong>Course Name</strong></td>
                                    <td style="width: 2%">:</td>
                                    <td style="width: 25%">
                                        <asp:DropDownList ID="drp_course" runat="server" CssClass="form-control"></asp:DropDownList>
                                    </td>
                                    <td style="width: 2%"></td>
                                    <td style="width: 23%"></td>
                                </tr>
                                <tr>
                                    <td colspan="5" style="height: 3px"></td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <div runat="server" id="search_section">



        <div class="cont_right formWrapper">






            <div runat="server" id="Div1">
                <div id="academic-years-grid" class="grid-view">

                    <asp:ListView ID="lv_semesterdetails" runat="server" GroupPlaceholderID="groupPlaceHolder1"
                        ItemPlaceholderID="itemPlaceHolder1">
                        <LayoutTemplate>
                            <table class="items" style="border: 1px solid #b9c7d0!important;">
                                <thead style="border: 1px solid #b9c7d0!important;">


                                    <th>Name
                                    </th>
                                    <th>Class Teacher
                                    </th>
                                    <th>Start Date
                                    </th>
                                    <th>End Date
                                    </th>
                                    <th>Status
                                    </th>
                                    <th class="col-md-3">Action
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
                                <%# Eval("Semester") %>
                            </td>

                            <td>
                                <%# Eval("TeacherName") %>
                            </td>
                            <td>
                                <%# Eval("semester_start_date") %>
                            </td>
                            <td>
                                <%# Eval("semester_end_date") %>
                            </td>


                            <td>
                                <%# Convert.ToString(Eval("is_active")).ToLower()=="1" ? "<span class='bg-green'>&nbsp;Active&nbsp;</span>" : "<span class='bg-red'>&nbsp;Inactive&nbsp;</span>"  %>
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
                      </asp:LinkButton>&nbsp;

                               
                                                     <asp:LinkButton ID="lnk_subject" CommandName="subject" CommandArgument='<%# Eval("id") %>' OnCommand="lnk_subject_Command" runat="server" Visible='<%# Convert.ToBoolean(Session["Access_Edit"])%>'>
                                                        <img src="gridview/book.png" alt="Subject" style="height:20px;width:20px">            
                                                    </asp:LinkButton>&nbsp;


                            </td>

                        </ItemTemplate>
                        <EmptyDataTemplate>
                            <table class="items" style="border: 1px solid #b9c7d0!important;">
                                <thead style="border: 1px solid #b9c7d0!important;">

                                    <th>Name
                                    </th>
                                    <th>Class Teacher
                                    </th>
                                    <th>Start Date
                                    </th>
                                    <th>End Date
                                    </th>
                                    <th>Status
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
        </div>
    </div>

    <div id="add_section" runat="server">



        <p class="note" runat="server" id="field_mandary">Fields with <span class="required">*</span> are required.</p>
        <div class="formCon">
            <div class="formConInner">
                <h3>Enter the details of the upcoming semester details</h3>
                <div class="txtfld-col-btn">
                    <div class="txtfld-col">
                        <label for="semester_name" class="required">Semester <span class="required">*</span></label>
                        <asp:DropDownList ID="drp_semester" runat="server"></asp:DropDownList>

                    </div>
                    <div class="txtfld-col">
                        <label for="semester_start" class="required">Starts On <span class="required">*</span></label>
                        <asp:TextBox ID="tbx_semester_start" runat="server" AutoCompleteType="Disabled" class="hasDatepicker"></asp:TextBox>

                    </div>
                    <div class="txtfld-col">
                        <label for="AcademicYears_end" class="required">Ends On <span class="required">*</span></label>
                        <asp:TextBox ID="tbx_semester_end" runat="server" AutoCompleteType="Disabled" class="hasDatepicker"></asp:TextBox>
                    </div>
                </div>

                <div class="txtfld-col-btn">
                    <div class="txtfld-col">
                        <label for="semester_status" class="required">Status <span class="required">*</span></label>
                        <asp:DropDownList ID="ddl_semester_status" runat="server">
                            <asp:ListItem Text="Active" Value="1"></asp:ListItem>
                            <asp:ListItem Text="Inactive" Value="0"></asp:ListItem>
                        </asp:DropDownList>

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
                        <button id="btn_newsemester" runat="server" style="width: 150px" onserverclick="btn_newsemester_ServerClick" class="a_tag-btn"><span>Create Semester</span></button>
                        <button id="btn_cancel" runat="server" style="width: 150px" onserverclick="btn_cancel_ServerClick" class="a_tag-btn"><span>Cancel</span></button>

                    </li>
                </ul>
            </div>
        </div>

    </div>


    <div id="update_section" runat="server">


        <p class="note" id="edit_field_mandary" runat="server">Fields with <span class="required">*</span> are required.</p>
        <div class="formCon">
            <div class="formConInner">
                <h3>Update the details of this semester details</h3>
                <div class="txtfld-col-btn">
                    <div class="txtfld-col">
                        <label for="Semester_name" class="required">Semester <span class="required">*</span></label>
                        <asp:TextBox ID="tbx_edit_semester_name" Enabled="false" runat="server" AutoCompleteType="Disabled" MaxLength="120"></asp:TextBox>
                    </div>
                    <div class="txtfld-col">
                        <label for="Semester_start" class="required">Starts On <span class="required">*</span></label>
                        <asp:TextBox ID="tbx_edit_semester_start" runat="server" AutoCompleteType="Disabled" class="hasDatepicker"></asp:TextBox>
                    </div>
                    <div class="txtfld-col">
                        <label for="Semester_end" class="required">Ends On <span class="required">*</span></label>
                        <asp:TextBox ID="tbx_edit_semester_end" runat="server" AutoCompleteType="Disabled" class="hasDatepicker"></asp:TextBox>
                    </div>
                </div>

                <div class="txtfld-col-btn">
                    <div class="txtfld-col">
                        <label for="Semester_status" class="required">Status <span class="required">*</span></label>
                        <asp:DropDownList ID="ddl_edit_semester_status" runat="server">
                            <asp:ListItem Text="Active" Value="1"></asp:ListItem>
                            <asp:ListItem Text="Inactive" Value="0"></asp:ListItem>
                        </asp:DropDownList>
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
                        <button id="Button1" runat="server" style="width: 150px" onserverclick="btn_cancel_ServerClick" class="a_tag-btn"><span>Cancel</span></button>
                    </li>
                </ul>
            </div>
        </div>


    </div>





    <div id="delete_section" runat="server">
    </div>
    <div id="view_section" runat="server">

        <div class="table_listbx">
            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                <tbody>
                    <tr>
                        <td width="20%">
                            <b>Semester</b>
                        </td>
                        <td width="3%"><strong>:</strong></td>
                        <td width="77%">
                            <label id="lbl_semestername" runat="server"></label>

                        </td>
                    </tr>

                    <tr>
                        <td>
                            <b>Starts on</b>
                        </td>
                        <td><strong>:</strong></td>
                        <td>
                            <label id="lbl_semestertart" runat="server"></label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <b>Ends on</b>
                        </td>
                        <td><strong>:</strong></td>
                        <td>
                            <label id="lbl_semesterend" runat="server"></label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <b>Status</b>
                        </td>
                        <td><strong>:</strong></td>
                        <td>
                            <label id="lbl_semestersts" runat="server"></label>
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
            $('#' + '<%= tbx_semester_start.ClientID %>').datepicker({ autoclose: true });
            $('#' + '<%= tbx_semester_end.ClientID %>').datepicker({ autoclose: true });
            $('#' + '<%= tbx_edit_semester_start.ClientID %>').datepicker({ autoclose: true });
            $('#' + '<%= tbx_edit_semester_end.ClientID %>').datepicker({ autoclose: true });
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

