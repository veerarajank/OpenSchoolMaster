<%@ Page Title="" Language="C#" MasterPageFile="~/GS247.master" AutoEventWireup="true" CodeFile="AttendanceMain.aspx.cs" Inherits="AttendanceMain" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div id="content">
        <div style="background: #fff; min-height: 800px;">
            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                <tbody>
                    <tr>

                        <td valign="top">
                            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                <tbody>
                                    <tr>
                                        <td valign="top" width="75%">
                                            <div class="full-formWrapper">
                                                <h1>Attendance Management</h1>

                                                <div class="button-bg">
                                                    <div class="top-hed-btn-left"></div>
                                                    <div class="top-hed-btn-right">
                                                        <ul>
                                                            <li>
                                                                <a id="explorer_change_1" class="a_tag-btn" href="Adm_Attendance_Student.aspx"><span>Student Attendance</span></a>           		</li>
                                                            <li>
                                                                <a class="a_tag-btn" href="Adm_Attendance_Teacher.aspx"><span>Teacher Attendance</span></a>                 </li>
                                                        </ul>
                                                    </div>

                                                </div>

                                                	<div class="yellow_bx yb_attendance" style="width:100%">
                	<div class="y_bx_head">
                    	Before recording the Attendance, make sure you follow the following instructions                    </div>
                	<div class="y_bx_list timetable_list">
                    	<h1>Set Weekdays</h1>
                        <p>
                        Set the weekdays, where the specific Batch has classes, You can use the school default or custom weekdays.						</p>
                    </div>
                    <div class="y_bx_list timetable_list">
                    	<h1>Set Class Timings</h1>
                        <p>
                        Create class timings for each Batch Enter each period start time and end time,Add break timings etc.						</p>
                    </div>
                    <div class="y_bx_list timetable_list">
                    	<h1>Subjects and Subject Allocation</h1>
                        <p>
                        Add existing subjects to the Batch or create a new subject. Associate each subject with the teacher.						</p>
                    </div>
                    <div class="y_bx_list timetable_list">
                    	<h1>Create Timetable</h1>
                        <p>Assigning each timing/period from the dropdown.</p>
                    </div>
    			</div>

                                                <div class="clear"></div>
                                            </div>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
    </div>
</asp:Content>

