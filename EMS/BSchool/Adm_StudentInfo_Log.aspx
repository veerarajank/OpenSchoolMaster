<%@ Page Title="" Language="C#" MasterPageFile="~/GS247.master" AutoEventWireup="true" CodeFile="Adm_StudentInfo_Log.aspx.cs" Inherits="Adm_StudentInfo_Log" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="midnav">
        <span onclick="Goto('Settings.aspx')">Student Information</span>
    </div>
    <div id="content">

        <table style="width: 100%" border="1" cellspacing="0" cellpadding="0">
            <tbody>
                <tr>
                    <td style="width: 247px" valign="top">
                        <style>
                            a.add {
                                display: block;
                                margin: 10px 0 0;
                                padding: 2px 5px;
                                width: 60px;
                                height: 30px;
                                background-color: #379bc9;
                                border-radius: 3px;
                                color: #fff;
                            }

                                a.add:hover {
                                    background-color: #318db7;
                                }

                            .add img {
                                color: #98adb5;
                                display: inline;
                                font-size: 14px;
                                margin-right: 5px;
                                width: 19px;
                                height: 20px;
                                float: left;
                                margin-left: 0px;
                                background: none;
                            }

                            .add span.fcount {
                                display: inline-block;
                                padding-top: 8px;
                                padding-left: 4px;
                                color: #FFF;
                                font-weight: bold;
                                font-size: 16px;
                            }

                            .accordion-header {
                                background-color: #fff;
                                padding: 7px;
                                cursor: pointer;
                                border-right: 1px solid #E0E0E0;
                                border-left: 1px solid #E0E0E0;
                                min-height: 30px;
                                transition: .25s;
                            }
                        </style>
                        <div class="empleftbx-profile">
                            <div class="empimgbx-profile">
                                <asp:Image ID="img_student" runat="server" />
                            </div>

                            <div class="left-profile-name-sctn">
                                <div class="left-profile-blk">
                                    <p id="lbl_studentname" runat="server"></p>
                                    <a href="#" id="lbl_studentmailid" runat="server"></a>
                                </div>
                                <div id="jobDialog"></div>
                                <div class="clear"></div>
                                <div class="prof_detail-blk">
                                    <ul>
                                        <li>
                                            <span id="lbl_studentcourse" runat="server"><strong>Course&nbsp;:</strong>&nbsp;
                                            </span>
                                        </li>
                                        <li>
                                            <span id="lbl_studentbatch" runat="server"><strong>Batch&nbsp;:</strong>&nbsp;
                                            </span>
                                        </li>


                                        <li>
                                            <span id="lbl_studentaddmissionno" runat="server"><strong>Admission No&nbsp;:</strong>&nbsp;</span>
                                        </li>
                                    </ul>
                                </div>

                            </div>
                        </div>
                    </td>
                    <td style="vertical-align: top">
                        <div class="cont_right formWrapper">
                            <h1>STUDENT PROFILE</h1>
                            <div class="emp_right_contner">
                                <div class="emp_tabwrapper">


                                    <div class="button-bg">
                                        <div class="top-hed-btn-left"></div>
                                        <div class="top-hed-btn-right">
                                            <ul>
                                               <li><a class="a_tag-btn" href="#" id="lnk_student_edit" runat="server" onserverclick="lnk_student_edit_ServerClick"><span>Edit</span></a> </li>
                                                <li><a class="a_tag-btn" href="#"  id="lnk_student_search" runat="server" onserverclick="lnk_student_search_ServerClick"><span>Students</span></a> </li>
                                            </ul>
                                        </div>


                                    </div>
                                    <div class="clear"></div>
                                    <div class="pagetab-bg-tag-a">
                                        <ul>
                                            <li><a href="#" id="lnk_student_profile" runat="server" onserverclick="lnk_student_profile_ServerClick">Profile</a></li>
                                            <li><a href="#" id="lnk_student_course" runat="server" onserverclick="lnk_student_course_ServerClick">Courses</a></li>
                                            <li><a href="#" id="lnk_student_assessment" runat="server" onserverclick="lnk_student_assessment_ServerClick">Assessments</a></li>
                                            <li><a href="#" id="lnk_student_attendance" runat="server" onserverclick="lnk_student_attendance_ServerClick">Attendance</a></li>
                                            <li><a href="#" id="lnk_student_document" runat="server" onserverclick="lnk_student_document_ServerClick">Documents</a></li>
                                            <li><a href="#" id="lnk_student_elective" runat="server" onserverclick="lnk_student_elective_ServerClick">Electives</a></li>
                                            <li><a href="#" id="lnk_student_achievement" runat="server" onserverclick="lnk_student_achievement_ServerClick">Achievements</a></li>
                                            <li class="active"><a href="#" id="lnk_student_log" runat="server" onserverclick="lnk_student_log_ServerClick">Log</a></li>
                                        </ul>
                                    </div>
                                    <div class="clear"></div>

                                    <div class="emp_cntntbx" style="width: 100%">

                                        <div class="pdtab_Con">
                                            <asp:ListView ID="lv_details" runat="server" GroupPlaceholderID="groupPlaceHolder1"
                                                ItemPlaceholderID="itemPlaceHolder1">
                                                <LayoutTemplate>
                                                    <table style="width: 100%" border="0" cellspacing="0" cellpadding="0" style="border-top: none;">
                                                        <thead>

                                                            <tr class="pdtab-h">
                                                                <td style="width: 125px"><strong>Log Category</strong></td>
                                                                <td style="width: 150px"><strong>Log Details</strong></td>
                                                                <td style="width: 100px"><strong>Log Created On</strong></td>
                                                                <td style="width: 150px"><strong>Actions</strong></td>
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

                                                    <td>
                                                        <%# Eval("log_category") %>
                                                    </td>
                                                    <td>
                                                        <%# Eval("log_comment") %>
                                                            
                                                    </td>
                                                    <td>
                                                        <%# Eval("log_date") %>
                                                    </td>
                                                    <td>

                                                        <ul class="tt-wrapper">

                                                            <li>

                                                                <asp:LinkButton ID="ln_edit" CssClass="tt-edit" CommandName="editvalue" CommandArgument='<%# Eval("id") %>' OnCommand="ln_edit_Command" runat="server">
                                                                    <span>Edit</span>
                                                                </asp:LinkButton>

                                                            </li>
                                                            <li>
                                                                <asp:LinkButton ID="ln_deleted" CssClass="tt-delete" CommandName="delete" CommandArgument='<%# Eval("id") %>' OnCommand="ln_deleted_Command" runat="server">
                                                                      <span>Delete</span>
                                                                </asp:LinkButton>

                                                            </li>

                                                        </ul>


                                                    </td>

                                                </ItemTemplate>
                                                <EmptyDataTemplate>
                                                    <table style="width: 100%" border="0" cellspacing="0" cellpadding="0" style="border-top: none;">
                                                        <thead>


                                                            <tr class="pdtab-h">
                                                             <td style="width: 125px"><strong>Log Category</strong></td>
                                                                <td style="width: 150px"><strong>Log Details</strong></td>
                                                                <td style="width: 100px"><strong>Log Created On</strong></td>
                                                                <td style="width: 150px"><strong>Actions</strong></td>
                                                            </tr>
                                                        </thead>

                                                        <tr style="border: 1px solid #b9c7d0!important;">
                                                            <td colspan="4">No Records Found
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </EmptyDataTemplate>
                                            </asp:ListView>
                                        </div>

                                        <div style="width: 712px;">
                                            <div class="form">

                                                <p class="note">Fields with <span class="required">*</span> are required.</p>


                                                <div class="formCon">
                                                    <div class="formConInner" id="innerDiv">

                                                        <div class="txtfld-col-box">
                                                            <div class="txtfld-col">
                                                                <label>Category<span class="required">*</span></label>
                                                                <asp:DropDownList ID="drp_category" runat="server" Width="250px"></asp:DropDownList>

                                                            </div>
                                                            <div class="txtfld-col-box">
                                                                <div class="text-fild-block-full">
                                                                    <label>Log Details<span class="required">*</span></label>
                                                                    <asp:TextBox ID="tbx_description" AutoCompleteType="Disabled" TextMode="MultiLine" runat="server" Rows="12" Width="100%" Height="100px" Columns="48"></asp:TextBox>


                                                                </div>
                                                            </div>
                                                            <div class="txtfld-col-box">
                                                                <div class="text-fild-block-full">
                                                                    <div class="ckbox ckbox-default">
                                                                        <asp:CheckBox ID="chk_notice_student"  runat="server" Text="Notification to Student" /><br />
                                                                        <asp:CheckBox ID="chk_notice_parent" runat="server" Text="Notification to Parent" /><br />
                                                                        <asp:CheckBox ID="chk_notice_teacher" runat="server" Text="Notification to Teacher" /><br />
                                                                        <asp:CheckBox ID="chk_visible_student" runat="server" Text="Visible to Student" /><br />
                                                                        <asp:CheckBox ID="chk_visible_parent" runat="server" Text="Visible to Parent" /><br />
                                                                        <asp:CheckBox ID="chk_visible_teacher" runat="server" Text="Visible to Teacher" />
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>

                                                        <asp:HiddenField ID="hd_logid" runat="server" />



                                                    </div>
                                                </div>
                                                <div class="row buttons">
                                                    <asp:Button ID="btn_save" ValidationGroup="vd_save" runat="server" Text="Submit" OnClick="btn_save_Click" Width="150px" CssClass="a_tag-btn" />
                                                </div>
                                            </div>
                                        </div>













                                    </div>
                                </div>
                            </div>
                        </div>
                    </td>
                </tr>
            </tbody>
        </table>

    </div>
</asp:Content>

