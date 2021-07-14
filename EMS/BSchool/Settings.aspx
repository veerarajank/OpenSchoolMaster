<%@ Page Title="" Language="C#" MasterPageFile="~/Admin_Settings.master" AutoEventWireup="true" CodeFile="Settings.aspx.cs" Inherits="Settings" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <table style="width: 100%" border="0" cellspacing="0" cellpadding="0">
        <tbody>
            <tr>
                <td>
                    <div class="emp_cntntbx">
                        <div>
                            <h1>Settings</h1>

                            <div class="clear"></div>
                        </div>

                        <div class="setbx_con">
                            <div class="block_box">
                                <div class="setbx">
                                    <div class="setbx_top">
                                        <h1>General Settings</h1>
                                    </div>
                                    <div class="setbx_bot">
                                        <ul>
                                            <li class="menu_box">
                                                <div class="set_icon"><i class="fa fa-gears"></i></div>
                                                <a class="name-txt" href="Adm_SchoolSetup.aspx">School Setup<span>School Name,Logo..</span></a></li>
                                            <li class="menu_box">
                                                <div class="set_icon"><i class="fa fa-cloud-download"></i></div>
                                                <a class="name-txt" href="#">Backup / Restore<span>Complete Database Backup</span></a></li>
                                            <li class="menu_box">
                                                <div class="set_icon"><i class="fa fa-calendar-o"></i></div>
                                                <a class="name-txt" href="Adm_academicYears.aspx">Academic Year<span>Set up,Manage previous years...</span></a></li>
                                            <li class="menu_box">
                                                <div class="set_icon"><i class="fa fa-database"></i></div>
                                                <a class="name-txt" href="Admin_Modules.aspx">Modules<span>Manage Modules</span></a></li>
                                            <li class="menu_box">
                                                <div class="set_icon"><i class="fa fa-bell"></i></div>
                                                <a class="name-txt" href="#">Annual Holidays<span>Manage Annual Holidays</span></a></li>
                                            <li class="menu_box">
                                                <div class="set_icon"><i class="fa fa-calendar"></i></div>
                                                <a class="name-txt" href="Adm_eventsType.aspx">Event Types<span>Manage Event Types</span></a></li>
                                            <li class="menu_box">
                                                <div class="set_icon"><i class="fa fa-tasks"></i></div>
                                                <a class="name-txt" href="Adm_features.aspx">Features<span>Manage Features</span></a></li>
                                            <li class="menu_box">
                                                <div class="set_icon"><i class="fa fa-tachometer" aria-hidden="true"></i></div>
                                                <a id="set_grade_level" class="name-txt" href="Adm_settingsDashboard.aspx">Manage Dashboard<span>Manage Dashboard Blocks</span></a>                        </li>
                                        </ul>
                                    </div>
                                    <div class="clear"></div>
                                </div>
                                <!-- block box end -->
                            </div>
                            <div class="clear"></div>
                            <div class="block_box">
                                <div class="setbx" style="width: 700px;">
                                    <div class="setbx_top">
                                        <h1 class="headings">Enrollment Settings</h1>
                                    </div>
                                    <div class="setbx_bot">
                                        <ul>
                                            <li class="menu_box">
                                                <div class="set_icon"><i class="fa fa-bars"></i></div>
                                                <a class="name-txt" href="Adm_Student_Category.aspx">Manage Category<span>Manage Student Category</span></a>                        </li>

                                            <li class="menu_box">
                                                <div class="set_icon"><i class="fa fa-calendar-check-o"></i></div>
                                                <a class="name-txt" href="Adm_SetAcademicSettings.aspx">Academic Year Settings<span>Set up Current Academic Year</span></a>                        </li>

                                            <li class="menu_box">
                                                <div class="set_icon"><i class="fa fa-keyboard-o"></i></div>
                                                <a class="name-txt" href="Adm_OnineSettings.aspx">Online Registration Settings<span>Online Registration Settings</span></a>                        </li>
                                            <li class="menu_box">
                                                <div class="set_icon"><i class="fa fa-book"></i></div>
                                                <a id="" class="name-txt" href="Adm_studentdocumenttype.aspx">Manage Student Document<span>Manage Student Document Types</span></a>
                                            </li>
                                            <li class="menu_box">
                                                <div class="set_icon"><i class="fa fa-list-ol"></i></div>
                                                <a class="help_class name-txt" href="Adm_Rollnosettings.aspx" id="yt0">Roll Number Settings<span>Roll No and Admission No Settings</span></a>                  </li>
                                        </ul>
                                    </div>
                                    <div class="clear"></div>
                                </div>
                            </div>
                            <div class="clear"></div>

                            


                            <div class="clear"></div>
                            <div class="block_box">
                                <div class="setbx" style="width: 700px;">
                                    <div class="setbx_top">
                                        <h1 class="headings">Course / Batch Settings</h1>
                                    </div>
                                    <div class="setbx_bot">
                                        <ul>
                                          
                                            <li class="menu_box">
                                                <div class="set_icon"><i class="fa fa-file"></i></div>
                                                <a class="name-txt" href="Adm_Current_Year_Course.aspx">Manage Courses<span>Create &amp; Manage Courses</span></a>            </li>

                                              <li class="menu_box">
                                                <div class="set_icon"><i class="fa fa-table"></i></div>
                                                <a class="name-txt" href="Adm_Course_Semester.aspx">Semester Settings<span>Enable / Disable Semester System</span></a>                </li>


                                            <li class="menu_box">
                                                <div class="set_icon"><i class="fa fa-clock-o"></i></div>
                                                <a class="name-txt" href="Adm_Course_Common_Class_Timings.aspx">Common Class Timings<span>Create &amp; Manage Institution Class Timings</span></a>        </li>
                                            <li class="menu_box">
                                                <div class="set_icon"><i class="fa fa-sitemap"></i></div>
                                                <a class="name-txt" href="Adm_Course_Subjects.aspx">Manage Subjects Common Pool<span>Manage Subjects Common Pool</span></a>    </li>
                                            <li class="menu_box">
                                                <div class="set_icon"><i class="fa fa-calendar"></i></div>
                                                <a class="name-txt" href="Adm_Time_Table_DefaultSystem_WeekDay.aspx">Default Weekdays<span>Default School Weekdays</span></a></li>
                                            <li class="menu_box">
                                                <div class="set_icon"><i class="fa fa-folder-open"></i></div>
                                                <a id="add_subjects" class="name-txt" href="Add_Subject_Batch.aspx">Add Subjects To Batch<span>Add Subjects To Batch</span></a></li>
                                            <li class="menu_box">
                                                <div class="set_icon"><i class="fa fa-line-chart"></i></div>
                                                <a id="A1" class="name-txt" href="Adm_DefaultGradeLevel.aspx">Set Default Grading Levels<span>Setting the Default Grading Levels</span></a></li>
                                            <li class="menu_box">
                                                <div class="set_icon"><i class="fa fa-mortar-board"></i></div>
                                                <a id="A2" class="name-txt" href="#">Manage Exam Format<span>Choose Exam Format</span></a></li>
                                            <li class="menu_box">
                                                <div class="set_icon"><i class="fa fa-tasks"></i></div>
                                                <a class="name-txt" href="#">Manage Timetable Format<span>Choose Timetable Format</span></a></li>
                                        </ul>
                                    </div>
                                    <div class="clear"></div>
                                </div>
                            </div>

                            <div class="clear"></div>

                            <div class="block_box">
                                <div class="setbx" style="width: 700px;">
                                    <div class="setbx_top">
                                        <h1 class="headings">Attendance Settings</h1>
                                    </div>
                                    <div class="setbx_bot">
                                        <ul>
                                            <li class="menu_box">
                                                <div class="set_icon"><i class="fa fa-file-text-o"></i></div>
                                                <a class="help_class name-txt" href="#" id="yt1">Attendance Settings<span>Manage Attendance Type</span></a>               </li>
                                        </ul>
                                    </div>
                                    <div class="clear"></div>
                                </div>
                            </div>
                            <div class="clear"></div>

                            <div class="block_box">
                                <div class="setbx" style="width: 700px;">
                                    <div class="setbx_top">
                                        <h1 class="headings">Log Settings</h1>
                                    </div>
                                    <div class="setbx_bot">
                                        <ul>

                                            <li class="menu_box">
                                                <div class="set_icon"><i class="fa fa-exchange"></i></div>
                                                <a class="name-txt" href="Adm_Student_Log.aspx">Student Log Category<span>Manage Student Log Category</span></a>                </li>
                                        </ul>
                                    </div>
                                    <div class="clear"></div>
                                </div>
                            </div>

                            <div class="clear"></div>
                            <div class="block_box">
                                <div class="setbx" style="width: 700px;">
                                    <div class="setbx_top">
                                        <h1 class="headings">Financial Settings</h1>
                                    </div>
                                    <div class="setbx_bot">
                                        <ul>
                                            <li class="menu_box">
                                                <div class="set_icon"><i class="fa fa-money"></i></div>
                                                <a class="name-txt" href="Adm_CreateFees.aspx">Manage Fees<span>Create Fees</span></a>                </li>
                                        </ul>
                                    </div>
                                    <div class="clear"></div>
                                </div>
                            </div>

                            <div class="clear"></div>

                            <div class="block_box">
                                <div class="setbx" style="width: 700px;">
                                    <div class="setbx_top">
                                        <h1 class="headings">Downloads Settings</h1>
                                    </div>
                                    <div class="setbx_bot">
                                        <ul>
                                            <li class="menu_box">
                                                <div class="set_icon"><i class="fa fa-align-left"></i></div>
                                                <a class="name-txt" href="#">File Category<span>Create File Categories</span></a>                </li>

                                            <li class="menu_box">
                                                <div class="set_icon"><i class="fa fa-upload"></i></div>
                                                <a class="name-txt" href="#">All File Uploads<span>View All File Uploads</span></a>                </li>
                                        </ul>
                                    </div>
                                    <div class="clear"></div>
                                </div>
                            </div>
                            <div class="clear"></div>
                            <div class="block_box">
                                <div class="setbx" style="width: 700px;">
                                    <div class="setbx_top">
                                        <h1 class="headings">SMS Settings</h1>
                                    </div>
                                    <div class="setbx_bot">
                                        <ul>
                                            <li class="menu_box">
                                                <div class="set_icon"><i class="fa fa-comments"></i></div>
                                                <a class="name-txt" href="#">SMS Gateway<span>SMS Gateway Settings</span></a>                    </li>

                                            <li class="menu_box">
                                                <div class="set_icon"><i class="fa fa-comments-o"></i></div>
                                                <a class="name-txt" href="#">SMS Counter<span>Track SMS Count</span></a>                    </li>
                                        </ul>
                                    </div>
                                    <div class="clear"></div>
                                </div>
                            </div>

                            <div class="clear"></div>
                            <div class="block_box">
                                <div class="setbx" style="width: 700px;">
                                    <div class="setbx_top">
                                        <h1 class="headings">Templates</h1>
                                    </div>
                                    <div class="setbx_bot">
                                        <ul>
                                            <li class="menu_box">
                                                <div class="set_icon"><i class="fa fa-th-list"></i></div>
                                                <a class="name-txt" href="#">SMS Templates<span>Edit SMS Templates</span></a></li>
                                            <li class="menu_box">
                                                <div class="set_icon"><i class="fa fa-envelope"></i></div>
                                                <a class="name-txt" href="#">Email Templates<span>Edit Email Templates</span></a>                    </li>
                                        </ul>
                                    </div>
                                    <div class="clear"></div>
                                </div>
                            </div>
                            <div class="clear"></div>
                            <div class="block_box">
                                <div class="setbx" style="width: 700px;">
                                    <div class="setbx_top">
                                        <h1 class="headings">Mobile Notifications</h1>
                                    </div>
                                    <div class="setbx_bot">
                                        <ul>
                                            <li class="menu_box">
                                                <div class="set_icon"><i class="fa fa-bell" aria-hidden="true"></i></div>
                                                <a class="name-txt" href="#">Push Notification<span>Manage Mobile Push Notification</span></a>                </li>
                                        </ul>
                                    </div>
                                    <div class="clear"></div>
                                </div>
                            </div>
                            <div class="clear"></div>
                            <div class="block_box">
                                <div class="setbx" style="width: 700px;">
                                    <div class="setbx_top">
                                        <h1 class="headings">Notification Settings</h1>
                                    </div>
                                    <div class="setbx_bot">
                                        <ul>
                                            <li class="menu_box">
                                                <div class="set_icon"><i class="fa fa-gear"></i></div>
                                                <a class="name-txt" href="NotificationSettings.aspx">Notification Settings<span>Enable SMS, MAIL notifications</span></a>                </li>
                                            <li class="menu_box">
                                                <div class="set_icon"><i class="fa fa-sliders" aria-hidden="true"></i></div>
                                                <a class="name-txt" href="SMTPConfiguration.aspx">SMTP Settings<span>Configure SMTP server</span></a>                </li>
                                        </ul>
                                    </div>
                                    <div class="clear"></div>
                                </div>
                            </div>

                            <div class="clear"></div>


                        </div>
                    </div>
                </td>
            </tr>
        </tbody>
    </table>

</asp:Content>

