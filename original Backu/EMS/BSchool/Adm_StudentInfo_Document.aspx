<%@ Page Title="" Language="C#" MasterPageFile="~/GS247.master" AutoEventWireup="true" CodeFile="Adm_StudentInfo_Document.aspx.cs" Inherits="Adm_StudentInfo_Document" %>

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
                                            <li class="active"><a href="#" id="lnk_student_document" runat="server" onserverclick="lnk_student_document_ServerClick">Documents</a></li>
                                            <li><a href="#" id="lnk_student_elective" runat="server" onserverclick="lnk_student_elective_ServerClick">Electives</a></li>
                                            <li><a href="#" id="lnk_student_achievement" runat="server" onserverclick="lnk_student_achievement_ServerClick">Achievements</a></li>
                                            <li><a href="#" id="lnk_student_log" runat="server" onserverclick="lnk_student_log_ServerClick">Log</a></li>
                                        </ul>
                                    </div>
                                    <div class="clear"></div>

                                    <div class="emp_cntntbx" style="border: 1px solid #379bc9; width: 100%">


                                        <div class="formCon">
                                            <div class="formConInner opnsl_form_label">
                                                <div class="emp_cntntbx">

                                                    <div class="document_table">
                                                        <table width="100%" cellspacing="0" cellpadding="0">
                                                            <tbody>
                                                                <tr>
                                                                    <th>Document Name</th>
                                                                </tr>
                                                            </tbody>
                                                        </table>



                                                  

                                                    <div>
                                                        <asp:ListView ID="lv_details" runat="server" GroupPlaceholderID="groupPlaceHolder1"
                                                            ItemPlaceholderID="itemPlaceHolder1">
                                                            <LayoutTemplate>
                                                                <table style="width: 100%" border="0" cellspacing="0" cellpadding="0" style="border-top: none;">
                                                                    <asp:PlaceHolder runat="server" ID="groupPlaceHolder1"></asp:PlaceHolder>
                                                                </table>
                                                            </LayoutTemplate>
                                                            <GroupTemplate>
                                                                <tr>
                                                                    <asp:PlaceHolder runat="server" ID="itemPlaceHolder1"></asp:PlaceHolder>
                                                                </tr>
                                                            </GroupTemplate>
                                                            <ItemTemplate>

                                                                <td style="width: 40%">
                                                                    <%# Convert.ToString(Eval("doc_title")) %>
                                                                </td>
                                                                <td style="width: 30%">
                                                                    <%# Convert.ToString(Eval("is_approved"))=="1" ? "<div class='tag_approved'>Approved</div>" : "<div class='tag_disapproved'>Disapproved</div>"  %>
                                                                </td>
                                                                <td style="width: 30%">
                                                                    <ul class="tt-wrapper">
                                                                        <li>
                                                                            <asp:LinkButton ID="ln_approved" CssClass="tt-approved-disabled" CommandName="approve" CommandArgument='<%# Eval("id") %>' OnCommand="ln_approved_Command" runat="server">
                                        <span>Approved</span>
                                                                            </asp:LinkButton>

                                                                        </li>
                                                                        <li>

                                                                            <asp:LinkButton ID="ln_disapproved" CssClass="tt-disapproved" CommandName="disapprove" CommandArgument='<%# Eval("id") %>' OnCommand="ln_disapproved_Command" runat="server">
                                        <span>Disapprove</span>
                                                                            </asp:LinkButton>

                                                                        </li>
                                                                        <li>
                                                                            <asp:LinkButton ID="ln_deleted" CssClass="tt-delete" CommandName="delete" CommandArgument='<%# Eval("id") %>' OnCommand="ln_deleted_Command" runat="server">
                                        <span>Delete</span>
                                                                            </asp:LinkButton>

                                                                        </li>
                                                                        <li>
                                                                            <asp:LinkButton ID="ln_download" CssClass="tt-download" CommandName="download" CommandArgument='<%# Eval("id") %>' OnCommand="ln_download_Command" runat="server">
                                                                            <span>Download</span>
                                                                            </asp:LinkButton>
                                                                        </li>
                                                                    </ul>
                                                                </td>
                                                            </ItemTemplate>
                                                            <EmptyDataTemplate>
                                                            </EmptyDataTemplate>
                                                        </asp:ListView>

                                                    </div>

                                                          </div>
                                                    <!-- END div class="document_table" -->

                                                </div>


                                                <div class="clear"></div>

                                                <div class="formCon" style="clear: left;">
                                                    <div class="formConInner" id="innerDiv">
                                                        <table style="width: 80%" border="0" cellspacing="0" cellpadding="0" id="documentTable">
                                                            <tbody>
                                                                <tr>
                                                                    <td>
                                                                        <label>Document Name<font color="#FF0000">*</font></label></td>
                                                                    <td>
                                                                        <label>Choose The File Size Is Maximum 200 Kb</label></td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <asp:DropDownList ID="drp_StudentDocument_doc_type" runat="server"></asp:DropDownList>
                                                                    </td>
                                                                    <td>
                                                                        <asp:FileUpload ID="file_upload" CssClass="form-control" runat="server" />
                                                                    </td>

                                                                </tr>
                                                            </tbody>
                                                        </table>
                                                    </div>
                                                </div>


                                                <div class="row buttons">
                                                    &nbsp;
                                            <asp:Button ID="btn_addanother" runat="server" Text="Add Another" Height="30px" OnClick="btn_addanother_Click" CssClass="formbut" />
                                                    <asp:Button ID="btn_save" runat="server" Text="Save" CssClass="formbut" OnClick="btn_save_Click" Height="30px" />

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

