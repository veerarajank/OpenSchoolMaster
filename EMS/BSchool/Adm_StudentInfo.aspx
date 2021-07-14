<%@ Page Title="" Language="C#" MasterPageFile="~/GS247.master" AutoEventWireup="true" CodeFile="Adm_StudentInfo.aspx.cs" Inherits="Adm_StudentInfo" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script src="plugins/jQuery/jquery-2.2.3.min.js"></script>
    <style type="text/css">
        .pagetab-bg .li_hide {
            display: none !important;
        }

        h3 {
            color: #58656E;
            font-size: 12px;
            margin: 0;
        }

        .student-postn {
            top: 16px;
            right: 19px;
        }
    </style>
    <style>
        .accordion-item-active .accordion-header h1 {
            float: none;
        }

        .acordn-bg .accordion-header h1 {
            float: none;
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
                                                <li><a class="a_tag-btn" href="#" id="lnk_student_search" runat="server" onserverclick="lnk_student_search_ServerClick"><span>Students</span></a> </li>
                                            </ul>
                                        </div>


                                    </div>
                                    <div class="clear"></div>
                                    <div class="pagetab-bg-tag-a">
                                        <ul>
                                            <li class="active"><a href="#" id="lnk_student_profile" runat="server" onserverclick="lnk_student_profile_ServerClick">Profile</a></li>
                                            <li><a href="#" id="lnk_student_course" runat="server" onserverclick="lnk_student_course_ServerClick">Courses</a></li>
                                            <li><a href="#" id="lnk_student_assessment" runat="server" onserverclick="lnk_student_assessment_ServerClick">Assessments</a></li>
                                            <li><a href="#" id="lnk_student_attendance" runat="server" onserverclick="lnk_student_attendance_ServerClick">Attendance</a></li>
                                            <li><a href="#" id="lnk_student_document" runat="server" onserverclick="lnk_student_document_ServerClick">Documents</a></li>
                                            <li><a href="#" id="lnk_student_elective" runat="server" onserverclick="lnk_student_elective_ServerClick">Electives</a></li>
                                            <li><a href="#" id="lnk_student_achievement" runat="server" onserverclick="lnk_student_achievement_ServerClick">Achievements</a></li>
                                            <li><a href="#" id="lnk_student_log" runat="server" onserverclick="lnk_student_log_ServerClick">Log</a></li>
                                        </ul>
                                    </div>
                                    <div class="clear"></div>

                                    <div class="pdf-box">
                                        <div class="box-one"></div>
                                        <div class="box-two">
                                            <div class="pdf-div">
                                                <a target="_blank" class="pdf_but" href="#">Generate PDF</a>
                                            </div>
                                        </div>
                                    </div>



                                    <div class="emp_cntntbx" style="border: 1px solid #379bc9; width: 100%">
                                        <div class="cordn-h3">

                                            <span>
                                                <h3>STUDENT DETAILS  </h3>


                                            </span>
                                            <div class="accordion">
                                                <div class="drawer">
                                                    <div class="accordion-item accordion-item-active">
                                                        <div class="accordion-header accordion-header-active">
                                                            <h1>Personal details</h1>
                                                            <div class="accordion-header-icon accordion-header-icon-active"></div>
                                                        </div>
                                                        <div class="accordion-content" style="display: block;">

                                                            <div class="tabl">
                                                                <div class="fist-sections">Admission Date</div>
                                                                <div class="midl-sections">:</div>
                                                                <div class="last-sections" runat="server" id="lbl_admissiondate">
                                                                </div>
                                                            </div>
                                                            <div class="tabl">
                                                                <div class="fist-sections">Date Of Birth</div>
                                                                <div class="midl-sections">:</div>
                                                                <div class="last-sections" runat="server" id="lbl_dob">
                                                                </div>
                                                            </div>


                                                            <div class="tabl">
                                                                <div class="fist-sections">Gender</div>
                                                                <div class="midl-sections">:</div>
                                                                <div class="last-sections" runat="server" id="lbl_gender">
                                                                </div>
                                                            </div>


                                                            <div class="tabl">
                                                                <div class="fist-sections">Blood </div>
                                                                <div class="midl-sections">:</div>
                                                                <div class="last-sections" runat="server" id="lbl_bloodgroup">
                                                                </div>
                                                            </div>


                                                            <div class="tabl">
                                                                <div class="fist-sections">Birth Place</div>
                                                                <div class="midl-sections">:</div>
                                                                <div class="last-sections" runat="server" id="lbl_birthplace">
                                                                </div>
                                                            </div>


                                                            <div class="tabl">
                                                                <div class="fist-sections">Nationality</div>
                                                                <div class="midl-sections">:</div>
                                                                <div class="last-sections" runat="server" id="lbl_nationality">
                                                                </div>
                                                            </div>


                                                            <div class="tabl">
                                                                <div class="fist-sections">Language</div>
                                                                <div class="midl-sections">:</div>
                                                                <div class="last-sections" runat="server" id="lbl_lang">
                                                                </div>
                                                            </div>


                                                            <div class="tabl">
                                                                <div class="fist-sections">Religion</div>
                                                                <div class="midl-sections">:</div>
                                                                <div class="last-sections" runat="server" id="lbl_religion">
                                                                </div>
                                                            </div>


                                                            <div class="tabl" style="border-bottom: 0px solid">
                                                                <div class="fist-sections">Student Category</div>
                                                                <div class="midl-sections">:</div>
                                                                <div class="last-sections" runat="server" id="lbl_studentcategory">
                                                                </div>
                                                            </div>

                                                        </div>
                                                    </div>
                                                </div>



                                                <div class="drawer">
                                                    <div class="accordion-item accordion-item-active">
                                                        <div class="accordion-header accordion-header-active">
                                                            <h1>Contact details</h1>
                                                            <div class="accordion-header-icon"></div>
                                                        </div>
                                                        <div class="accordion-content" style="display: block;">

                                                            <div class="tabl">
                                                                <div class="fist-sections">Address Line 1</div>
                                                                <div class="midl-sections">:</div>
                                                                <div class="last-sections" runat="server" id="lbl_addr1">
                                                                </div>
                                                            </div>

                                                            <div class="tabl">
                                                                <div class="fist-sections">Address Line 2</div>
                                                                <div class="midl-sections">:</div>
                                                                <div class="last-sections" runat="server" id="lbl_addr2">
                                                                </div>
                                                            </div>

                                                            <div class="tabl">
                                                                <div class="fist-sections">City</div>
                                                                <div class="midl-sections">:</div>
                                                                <div class="last-sections" runat="server" id="lbl_city">
                                                                </div>
                                                            </div>


                                                            <div class="tabl">
                                                                <div class="fist-sections">State</div>
                                                                <div class="midl-sections">:</div>
                                                                <div class="last-sections" runat="server" id="lbl_state">
                                                                </div>
                                                            </div>

                                                            <div class="tabl">
                                                                <div class="fist-sections">Pin Code</div>
                                                                <div class="midl-sections">:</div>
                                                                <div class="last-sections" runat="server" id="lbl_pincode">
                                                                </div>
                                                            </div>

                                                            <div class="tabl">
                                                                <div class="fist-sections">Country</div>
                                                                <div class="midl-sections">:</div>
                                                                <div class="last-sections" runat="server" id="lbl_country">
                                                                </div>
                                                            </div>

                                                            <div class="tabl">
                                                                <div class="fist-sections">Phone Number</div>
                                                                <div class="midl-sections">:</div>
                                                                <div class="last-sections" runat="server" id="lbl_phone1">
                                                                </div>
                                                            </div>

                                                            <div class="tabl">
                                                                <div class="fist-sections">Phone 2</div>
                                                                <div class="midl-sections">:</div>
                                                                <div class="last-sections" runat="server" id="lbl_phone2">
                                                                </div>
                                                            </div>

                                                            <div class="tabl" style="border-bottom: 0px solid">
                                                                <div class="fist-sections">Email</div>
                                                                <div class="midl-sections">:</div>
                                                                <div class="last-sections" runat="server" id="lbl_email">
                                                                </div>
                                                            </div>

                                                        </div>
                                                    </div>
                                                </div>


                                                <div class="drawer">
                                                    <div class="accordion-item accordion-item-active">
                                                         <div class="prnt-head">
                                                            <h1>GUARDIAN DETAILS</h1>
                                                            <div class="accordion-header-icon"></div>
                                                        </div>
                                                        <div class="accordion-content" style="display: block;">
                                                            <asp:ListView ID="lv_gaurdiansdetails" runat="server" GroupPlaceholderID="groupPlaceHolder1"
                                                                ItemPlaceholderID="itemPlaceHolder1">
                                                                <LayoutTemplate>

                                                                    <asp:PlaceHolder runat="server" ID="groupPlaceHolder1"></asp:PlaceHolder>

                                                                </LayoutTemplate>
                                                                <GroupTemplate>

                                                                    <asp:PlaceHolder runat="server" ID="itemPlaceHolder1"></asp:PlaceHolder>

                                                                </GroupTemplate>
                                                                <ItemTemplate>

                                                                    <div class="box box-success box box-primary collapsed-box">
                                                                        <div class="box-header with-border" style="background-color:#d9edf7;color:white ">
                                                                            <h1 class="box-title" style="color:white"><%# Eval("relation") %></h1>

                                                                            <div class="box-tools pull-right">
                                                                                <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                                                    <i class="fa fa-plus"></i>
                                                                                </button>
                                                                                <button type="button" class="btn btn-box-tool" data-widget="remove"><i class="fa fa-times"></i></button>
                                                                            </div>
                                                                        </div>

                                                                        <div class="box-body no-padding" style="">

                                                                           

                                                                            <div class="prnt-head">
                                                                                <h1>Personal details</h1>
                                                                                <div class="accordion-header-icon accordion-header-icon-active"></div>
                                                                            </div>

                                                                            <div class="accordion-content" style="display: block;">

                                                                                <div class="tabl">
                                                                                    <div class="fist-sections">First Name</div>
                                                                                    <div class="midl-sections">:</div>
                                                                                    <div class="last-sections">
                                                                                        <%# Eval("first_name") %>
                                                                                    </div>
                                                                                </div>
                                                                                <div class="tabl">
                                                                                    <div class="fist-sections">Last Name</div>
                                                                                    <div class="midl-sections">:</div>
                                                                                    <div class="last-sections">
                                                                                        <%# Eval("last_name") %>
                                                                                    </div>
                                                                                </div>
                                                                                <div class="tabl">
                                                                                    <div class="fist-sections">Relation</div>
                                                                                    <div class="midl-sections">:</div>
                                                                                    <div class="last-sections">
                                                                                        <%# Eval("relation") %>
                                                                                    </div>
                                                                                </div>

                                                                                <div class="tabl">
                                                                                    <div class="fist-sections">Date of Birth</div>
                                                                                    <div class="midl-sections">:</div>
                                                                                    <div class="last-sections">
                                                                                        <%# Eval("dob") %>
                                                                                    </div>
                                                                                </div>

                                                                                <div class="tabl">
                                                                                    <div class="fist-sections">Education</div>
                                                                                    <div class="midl-sections">:</div>
                                                                                    <div class="last-sections">
                                                                                        <%# Eval("education") %>
                                                                                    </div>
                                                                                </div>
                                                                                <div class="tabl">
                                                                                    <div class="fist-sections">Occupation</div>
                                                                                    <div class="midl-sections">:</div>
                                                                                    <div class="last-sections">
                                                                                        <%# Eval("occupation") %>
                                                                                    </div>
                                                                                </div>

                                                                                <div class="tabl" style="border-bottom: 0px solid">
                                                                                    <div class="fist-sections">Income</div>
                                                                                    <div class="midl-sections">:</div>
                                                                                    <div class="last-sections">
                                                                                        <%# Eval("income") %>
                                                                                    </div>
                                                                                </div>

                                                                            </div>
                                                                            <div class="prnt-head">
                                                                                <h1>Contact details
                                                                                <div class="pull-right">
                                                                                    <div class="tt-wrapper-new">

                                                                                        <%# Convert.ToString(Eval("is_parent_user"))=="1" ?
                                                                                            "<a class='makeprimary-active' id='lnk_maked'><span style='font-size:11px'>Primary Contact</span></a>"                                                     
                                                                                            : ""                                                                                                                                                     
                                                                                        %>

                                                                                        <%# Convert.ToString(Eval("emergency_user"))=="1" ?
                                                                                            "<a class='makeemrgency-active' id='lnk_makedemergency'><span style='font-size:11px'>Emergency Contact</span></a>"                                                     
                                                                                            : ""                                                                                                                                                     
                                                                                        %>
                                                                                    </div>
                                                                                </div>
                                                                                </h1>
                                                                                <div class="accordion-header-icon accordion-header-icon-active"></div>
                                                                            </div>
                                                                            <div class="accordion-content" style="display: block;">

                                                                                <div class="tabl">
                                                                                    <div class="fist-sections">Address Line 1</div>
                                                                                    <div class="midl-sections">:</div>
                                                                                    <div class="last-sections">
                                                                                        <%# Eval("address_line1") %>
                                                                                    </div>
                                                                                </div>
                                                                                <div class="tabl">
                                                                                    <div class="fist-sections">Address Line 2</div>
                                                                                    <div class="midl-sections">:</div>
                                                                                    <div class="last-sections">
                                                                                        <%# Eval("address_line2") %>
                                                                                    </div>
                                                                                </div>
                                                                                <div class="tabl">
                                                                                    <div class="fist-sections">City</div>
                                                                                    <div class="midl-sections">:</div>
                                                                                    <div class="last-sections">
                                                                                        <%# Eval("city") %>
                                                                                    </div>
                                                                                </div>

                                                                                <div class="tabl">
                                                                                    <div class="fist-sections">State</div>
                                                                                    <div class="midl-sections">:</div>
                                                                                    <div class="last-sections">
                                                                                        <%# Eval("state") %>
                                                                                    </div>
                                                                                </div>


                                                                                <div class="tabl">
                                                                                    <div class="fist-sections">Country</div>
                                                                                    <div class="midl-sections">:</div>
                                                                                    <div class="last-sections">
                                                                                        <%# Eval("country") %>
                                                                                    </div>
                                                                                </div>

                                                                                <div class="tabl">
                                                                                    <div class="fist-sections">Phone Number</div>
                                                                                    <div class="midl-sections">:</div>
                                                                                    <div class="last-sections">
                                                                                        <%# Eval("phone1") %> , 
                                                                                    <%# Eval("phone2") %>,
                                                                                    <%# Eval("mobile_phone") %>
                                                                                    </div>
                                                                                </div>
                                                                                <div class="tabl" style="border-bottom: 0px solid">
                                                                                    <div class="fist-sections">Email</div>
                                                                                    <div class="midl-sections">:</div>
                                                                                    <div class="last-sections">
                                                                                        <%# Eval("email") %>
                                                                                    </div>
                                                                                </div>


                                                                            </div>
                                                                        </div>
                                                                    </div>





                                                                </ItemTemplate>
                                                                <EmptyDataTemplate>
                                                                </EmptyDataTemplate>
                                                            </asp:ListView>


                                                        </div>
                                                    </div>
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

