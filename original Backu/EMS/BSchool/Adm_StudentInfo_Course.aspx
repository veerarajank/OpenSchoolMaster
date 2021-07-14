<%@ Page Title="" Language="C#" MasterPageFile="~/GS247.master" AutoEventWireup="true" CodeFile="Adm_StudentInfo_Course.aspx.cs" Inherits="Adm_StudentInfo_Course" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <style>
        td {
            padding:2px;
        }
    </style>

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
                                             <li ><a href="#" id="lnk_student_profile" runat="server" onserverclick="lnk_student_profile_ServerClick">Profile</a></li>
                                            <li class="active"><a href="#" id="lnk_student_course" runat="server" onserverclick="lnk_student_course_ServerClick">Courses</a></li>
                                            <li><a href="#" id="lnk_student_assessment" runat="server" onserverclick="lnk_student_assessment_ServerClick">Assessments</a></li>
                                            <li><a href="#" id="lnk_student_attendance" runat="server" onserverclick="lnk_student_attendance_ServerClick">Attendance</a></li>
                                            <li><a href="#" id="lnk_student_document" runat="server" onserverclick="lnk_student_document_ServerClick">Documents</a></li>
                                            <li><a href="#" id="lnk_student_elective" runat="server" onserverclick="lnk_student_elective_ServerClick">Electives</a></li>
                                            <li ><a href="#" id="lnk_student_achievement" runat="server" onserverclick="lnk_student_achievement_ServerClick">Achievements</a></li>
                                            <li><a href="#" id="lnk_student_log" runat="server" onserverclick="lnk_student_log_ServerClick">Log</a></li>
                                        </ul>
                                    </div>
                                    <div class="clear"></div>

                                    <div class="emp_cntntbx" style="border: 1px solid #379bc9; width: 100%">

                                        <table style="width: 100%" cellpadding="1" cellspacing="0">
                                            <tbody>
                                                <tr>
                                                    <th>Sl No</th>
                                                    <th>Academic Year</th>
                                                    <th>Course / Batch</th>
                                                    <th>Semester</th>
                                                    <th>Status</th>
                                                    <th>Action</th>
                                                </tr>
                                                <tr id="div_manage" runat="server">
                                                    <td>
                                                       <center>1.</center>
                                                    </td>
                                                    <td>
                                                        <label id="lbl_academicyear" runat="server"></label>

                                                    </td>
                                                    <td>
                                                        <label id="lbl_batch" runat="server"></label>
                                                    </td>
                                                    <td>
                                                        <label id="lbl_semester" runat="server"></label>
                                                    </td>
                                                    <td>
                                                        <span style="color: #006633">
                                                            <label id="lbl_status" runat="server"></label>
                                                        </span>
                                                    </td>
                                                    <td> 
                                                          <a href="#" runat="server" id="A1" onserverclick="btn_manage_ServerClick">Manage</a> |
                                                        <a href="#" runat="server" id="btn_edit" onserverclick="btn_edit_ServerClick">Edit</a>
                                                        
                                                    </td>
                                                </tr>
                                                <tr id="div_update" runat="server" visible="false">
                                                    <td>
                                                        <center>1.</center>
                                                    </td>
                                                    <td>
                                                        <label id="lbl_edit_academicyear" runat="server"></label>

                                                    </td>
                                                    <td>
                                                      <label id="lbl_edit_batch" runat="server"></label>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="drp_semester" runat="server"></asp:DropDownList>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="drp_status" runat="server">
                                                            <asp:ListItem Text="--Select Status--" Value=""></asp:ListItem>
                                                            <asp:ListItem Text="Passed" Value="Passed"></asp:ListItem>
                                                            <asp:ListItem Text="Failed" Value="Failed"></asp:ListItem>
                                                            <asp:ListItem Text="Alumni" Value="Alumni"></asp:ListItem>
                                                            <asp:ListItem Text="In Progress" Value="In Progress"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td>
                                                        <a href="#" runat="server" id="btn_manage" onserverclick="btn_manage_ServerClick">Manage</a> |
                                                        <a href="#" runat="server" id="btn_update" onserverclick="btn_update_ServerClick">Update</a>
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>














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

