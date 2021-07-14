<%@ Page Title="" Language="C#" MasterPageFile="~/Adm_TimeTable.master" AutoEventWireup="true" CodeFile="Adm_Time_Table_Classtiming.aspx.cs" Inherits="Adm_Time_Table_Classtiming" %>

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

    <style>
        .checkbox, .radio {
            width: 24px;
            height: 25px;
            padding: 0 5px 0 0;
            background: url(./images/nschkb.png) no-repeat;
            display: block;
            clear: left;
            float: left;
            margin: 0px 5px 0px 5px;
        }

        #yellowchk .checkbox, .radio {
            width: 19px;
            height: 20px;
            padding: 0 5px 0 0;
            background: url(./images/check_yellow.png) no-repeat;
            display: block;
            clear: left;
            float: left;
        }

        #greenchk .checkbox, .radio {
            width: 19px;
            height: 20px;
            padding: 0 5px 0 0;
            background: url(./images/check_green.png) no-repeat;
            display: block;
            clear: left;
            float: left;
        }

        #redchk .checkbox, .radio {
            width: 19px;
            height: 20px;
            padding: 0 5px 0 0;
            background: url(./images/check_red.png) no-repeat;
            display: block;
            clear: left;
            float: left;
        }

        #bluechk .checkbox, .radio {
            width: 19px;
            height: 20px;
            padding: 0 5px 0 0;
            background: url(./images/check_blue.png) no-repeat;
            display: block;
            clear: left;
            float: left;
        }

        .radio {
            background: url(radio.png) no-repeat;
        }

        .select {
            position: absolute;
            width: 158px; /* With the padding included, the width is 190 pixels: the actual width of the image. */
            height: 21px;
            padding: 0 24px 0 8px;
            color: #fff;
            font: 12px/21px arial,sans-serif;
            background: url(select.png) no-repeat;
            overflow: hidden;
        }

        input.styled {
            display: none;
        }
    </style>


    <div class="cont_right formWrapper">
        <h1 style="margin-top: .67em;">Class Timings<br>
        </h1>
        <div class="clear"></div>
        <div class="emp_right_contner">
            <div class="emp_tabwrapper">

                <div class="formCon">
                    <div class="formConInner">
                        <div class="txtfld-col-box">

                            <div class="txtfld-col">
                                Select Course
                        <asp:DropDownList ID="drp_course" runat="server" AutoPostBack="true" OnSelectedIndexChanged="drp_course_SelectedIndexChanged"></asp:DropDownList>
                                <asp:RequiredFieldValidator runat="server" Display="None" CssClass="input-notification-error  error-simple png_bg" ValidationGroup="vd_save" ID="reqdocumentname" ControlToValidate="drp_course" InitialValue="0" ErrorMessage="Course cannot be blank." />                   
                            </div>
                            <div class="txtfld-col">
                                Select Batch
                        <asp:DropDownList ID="drp_batch" runat="server" AutoPostBack="true" OnSelectedIndexChanged="drp_batch_SelectedIndexChanged"></asp:DropDownList>
                                <asp:RequiredFieldValidator runat="server" Display="None" CssClass="input-notification-error  error-simple png_bg" ValidationGroup="vd_save" ID="RequiredFieldValidator1" ControlToValidate="drp_batch" InitialValue="0" ErrorMessage="Batch cannot be blank." />                   
                            </div>
                            <div class="txtfld-col">
                                Select Semester								                        
                        <asp:DropDownList ID="drp_semester" runat="server" AutoPostBack="true" OnSelectedIndexChanged="drp_semester_SelectedIndexChanged"></asp:DropDownList>
                                <asp:RequiredFieldValidator runat="server" Display="None" CssClass="input-notification-error  error-simple png_bg" ValidationGroup="vd_save" ID="RequiredFieldValidator2" ControlToValidate="drp_semester" InitialValue="0" ErrorMessage="Semester cannot be blank." />                   
                            </div>                           

                        </div>
                        
                    </div>
                </div>






                <div class="clear"></div>
                <div class="emp_tabwrapper">


                    <div class="clear"></div>


                    <div class="formCon">
                        <div class="formConInner">


                            <div>
                                <div style="padding-top: 10px;">

                                    <span>
                                        <h3>Class Timings</h3>
                                    </span>

                                   <div runat="server" id="search_section">


                                        <div class="button-bg">
                                            <div class="top-hed-btn-left"></div>
                                            <div class="top-hed-btn-right">
                                                <ul>
                                                    <li>
                                                        <button id="btn_new" runat="server" onserverclick="btn_new_ServerClick" class="a_tag-btn"><span>Create Class Timing</span></button>
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
                                                            <th>Start Time
                                                            </th>
                                                            <th>End Time
                                                            </th>
                                                            <th>Is Break
                                                            </th>
                                                            <th>Status
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
                                                        <%# Eval("name") %>
                                                    </td>

                                                    <td>
                                                        <%#  Convert.ToString(Convert.ToDateTime(Eval("start_time")).ToString("hh:mm tt")) %>
                                                    </td>
                                                    <td>
                                                        <%#  Convert.ToString(Convert.ToDateTime(Eval("end_time")).ToString("hh:mm tt")) %>
                                                    </td>
                                                    <td>
                                                        <%# Convert.ToString(Eval("is_break")).ToLower()=="1" ? "<span class='bg-green'>&nbsp;Yes&nbsp;</span>" : "<span class='bg-red'>&nbsp;No&nbsp;</span>"  %>
                                                    </td>
                                                    <td>
                                                        <%# Convert.ToString(Eval("is_status")).ToLower()=="1" ? "<span class='bg-green'>&nbsp;Active&nbsp;</span>" : "<span class='bg-red'>&nbsp;Inactive&nbsp;</span>"  %>
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
                                                        <thead style="border: 1px solid #b9c7d0!important;">

                                                            <th>Name
                                                            </th>
                                                            <th>Start Time
                                                            </th>
                                                            <th>End Time
                                                            </th>
                                                            <th>Is Break
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

                                    <div id="add_section" runat="server">

                                        <h4><b>New Class Timing</b></h4>
                                        <div class="button-bg">
                                            <div class="top-hed-btn-left"></div>
                                            <div class="top-hed-btn-right">
                                                <ul>
                                                    <li>

                                                        <asp:Button ID="btn_manage_classtiming" runat="server" OnClick="btn_manage_classtiming_Click" Text="Manage Class Timing" CssClass="a_tag-btn" />
                                                    </li>
                                                </ul>
                                            </div>
                                        </div>

                                        <p class="note" runat="server" id="field_mandary">Fields with <span class="required">*</span> are required.</p>
                                        <div class="formCon">
                                            <div class="formConInner">
                                                <h3>Enter the details of the class timing details</h3>
                                                <div class="txtfld-col-btn">
                                                    <div class="txtfld-col">
                                                        <label for="name" class="required">Name <span class="required">*</span></label>
                                                        <asp:TextBox ID="tbx_name" AutoCompleteType="Disabled" Font-Bold="false" CssClass="form-control" runat="server" MaxLength="120"></asp:TextBox>

                                                    </div>
                                                    <div class="txtfld-col">
                                                        <label for="start_time" class="required">Start Time<span class="required">*</span></label>
                                                        <asp:TextBox ID="tbx_start_time" runat="server" AutoCompleteType="Disabled" class="form-control hasDatepicker"></asp:TextBox>

                                                    </div>
                                                    <div class="txtfld-col">
                                                        <label for="end_time" class="required">End Time<span class="required">*</span></label>
                                                        <asp:TextBox ID="tbx_end_time" runat="server" AutoCompleteType="Disabled" class="form-control hasDatepicker"></asp:TextBox>
                                                    </div>
                                                    <div class="txtfld-col">
                                                        <label for="isbreak" class="required">Is Break<span class="required">*</span></label>
                                                        <asp:CheckBox ID="chk_break" runat="server" Checked="true" />
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
                                                        <button id="btn_newtiming" validationgroup="vd_save" runat="server" style="width: 150px" onserverclick="btn_newtiming_ServerClick" class="a_tag-btn"><span>Create Class timing</span></button>
                                                    </li>
                                                </ul>
                                            </div>
                                        </div>

                                    </div>


                                    <div id="update_section" runat="server">

                                        <h4><b>Update Class Timing</b></h4>
                                        <div class="form">
                                            <div class="button-bg">
                                                <div class="top-hed-btn-left"></div>
                                                <div class="top-hed-btn-right">
                                                    <ul>
                                                        <li>

                                                            <asp:Button ID="btn_addclasstiming_up_screen" runat="server" Text="Add Class Timing" OnClick="btn_addclasstiming_up_screen_Click" CssClass="a_tag-btn" />
                                                        </li>
                                                        <li>
                                                            <asp:Button ID="btn_manageclasstiming_up_screen" runat="server" Text="Manage Class Timing" OnClick="btn_manageclasstiming_up_screen_Click" CssClass="a_tag-btn" />
                                                        </li>
                                                    </ul>
                                                </div>
                                            </div>


                                            <p class="note" id="edit_field_mandary" runat="server">Fields with <span class="required">*</span> are required.</p>
                                            <div class="formCon">
                                                <div class="formConInner">
                                                    <h3>Update the details of this class timing details</h3>
                                                    <div class="txtfld-col-btn">
                                                        <div class="txtfld-col">
                                                            <label for="name" class="required">Name <span class="required">*</span></label>
                                                            <asp:TextBox ID="tbx_edit_name" AutoCompleteType="Disabled" runat="server" MaxLength="120"></asp:TextBox>

                                                        </div>
                                                        <div class="txtfld-col">
                                                            <label for="start_time" class="required">Start Time<span class="required">*</span></label>
                                                            <asp:TextBox ID="tbx_edit_start_time" runat="server" AutoCompleteType="Disabled" class="hasDatepicker"></asp:TextBox>

                                                        </div>
                                                        <div class="txtfld-col">
                                                            <label for="end_time" class="required">End Time<span class="required">*</span></label>
                                                            <asp:TextBox ID="tbx_edit_end_time" runat="server" AutoCompleteType="Disabled" class="hasDatepicker"></asp:TextBox>
                                                        </div>
                                                        <div class="txtfld-col">
                                                            <label for="isbreak" class="required">Is Break<span class="required">*</span></label>
                                                            <asp:CheckBox ID="chk_edit_break" runat="server" Checked="true" />
                                                        </div>
                                                    </div>




                                                </div>
                                            </div>

                                            <div class="button-bg">
                                                <div class="top-hed-btn-left"></div>
                                                <div class="top-hed-btn-right">
                                                    <ul>
                                                        <li>
                                                            <button id="btn_update" validationgroup="vd_save" runat="server" onserverclick="btn_update_ServerClick" style="width: 150px" class="a_tag-btn"><span>Save</span></button>
                                                        </li>
                                                    </ul>
                                                </div>
                                            </div>


                                        </div>



                                    </div>

                                    <div id="delete_section" runat="server">
                                    </div>
                                    <div id="view_section" runat="server">

                                        <h4><b>View Class Timing</b></h4>

                                        <div class="button-bg">
                                            <div class="top-hed-btn-left"></div>
                                            <div class="top-hed-btn-right">
                                                <ul>
                                                    <li>
                                                        <button id="btn_edit" runat="server" onserverclick="btn_edit_ServerClick" class="a_tag-btn"><span>Edit</span></button>

                                                    </li>
                                                    <li>
                                                        <button id="btn_manage" runat="server" onserverclick="btn_manage_ServerClick" class="a_tag-btn"><span>Manage Class Timing</span></button>
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
                                                            <b>Start Time</b>
                                                        </td>
                                                        <td><strong>:</strong></td>
                                                        <td>
                                                            <label id="lbl_starttime" runat="server"></label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <b>End Time</b>
                                                        </td>
                                                        <td><strong>:</strong></td>
                                                        <td>
                                                            <label id="lbl_endtime" runat="server"></label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <b>Is Break</b>
                                                        </td>
                                                        <td><strong>:</strong></td>
                                                        <td>
                                                            <span id="lbl_breaksts" runat="server"></span>
                                                        </td>
                                                    </tr>
                                                </tbody>
                                            </table>
                                        </div>


                                    </div>



















                                    <br>
                                </div>
                            </div>
                        </div>

                    </div>

                    <br>
                </div>

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
    <script type="text/javascript">
        $(function () {
            $('#' + '<%= tbx_start_time.ClientID %>').datetimepicker({ format: 'hh:mm A' });
            $('#' + '<%= tbx_end_time.ClientID %>').datetimepicker({ format: 'hh:mm A' });
            $('#' + '<%= tbx_edit_start_time.ClientID %>').datetimepicker({ format: 'hh:mm A' });
            $('#' + '<%= tbx_edit_end_time.ClientID %>').datetimepicker({ format: 'hh:mm A' });



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

