<%@ Page Title="" Language="C#" MasterPageFile="~/GS247.master" AutoEventWireup="true" CodeFile="Adm_Teacher_View_Primary.aspx.cs" Inherits="Adm_Teacher_View_Primary" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
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

    <div class="midnav">
        <span onclick="Goto('Settings.aspx')">Teacher Information</span>
    </div>

    <div id="content">
        <table style="width: 100%" border="1" cellspacing="0" cellpadding="0">
            <tbody>
                <tr>
                    <td width="247" valign="top">
                        <div class="emp_cont_left">
                            <div class="empleftbx-profile">
                                <div class="empimgbx-profile">

                                    <asp:Image ID="img_student" runat="server" />
                                </div>
                                <div class="left-profile-name-sctn">
                                    <div class="left-profile-blk">
                                        <p id="lbl_teachername" runat="server">
                                        </p>
                                        <a href="#" id="lbl_teachermailid" runat="server"></a>
                                    </div>
                                </div>
                                <div class="clear"></div>



                            </div>
                        </div>
                    </td>
                    <td valign="top">
                        <div class="cont_right formWrapper">
                            <h1>Teacher Profile</h1>
                            <div class="button-bg">
                                <div class="top-hed-btn-left"></div>
                                <div class="top-hed-btn-right">
                                    <ul>
                                        <li><a class="a_tag-btn" href="#"><span>Edit</span></a> </li>
                                        <li><a class="a_tag-btn" href="#"><span>Teachers</span></a> </li>
                                    </ul>
                                </div>
                            </div>


                            <div class="emp_right_contner">
                                <div class="emp_tabwrapper">

                                    <div class="pagetab-bg-tag-a">
                                        <ul>

                                            <li class="active"><a href="#" id="lnk_teacher_profile" runat="server" onserverclick="lnk_teacher_profile_ServerClick">Profile</a></li>
                                            <li><a href="#"  id="lnk_teacher_achievement" runat="server" onserverclick="lnk_teacher_achievement_ServerClick">Achievements</a></li>
                                            <li><a href="#" id="lnk_teacher_log" runat="server" onserverclick="lnk_teacher_log_ServerClick">Log</a></li>
                                            <li><a href="#" id="lnk_teacher_document" runat="server" onserverclick="lnk_teacher_document_ServerClick">Documents</a></li>
                                            <li><a href="#" id="lnk_teacher_attendance" runat="server" onserverclick="lnk_teacher_attendance_ServerClick">Attendance</a></li>
                                            <li><a href="#"  id="lnk_teacher_subject" runat="server" onserverclick="lnk_teacher_subject_ServerClick">Subject Association</a></li>
                                        </ul>
                                    </div>
                                    <div class="clear"></div>


                                    <div class="button-bg"> 
                                          <div class="top-hed-btn-left">                                            
                                        </div>                                       
                                        <div class="top-hed-btn-right">
                                            <ul>
                                                <li><a target="_blank" class="pdf_but" href="#">Generate PDF</a></li>
                                            </ul>
                                        </div>
                                    </div>
                                    <div class="emp_cntntbx">

                                        <div class="cordn-h3">                                           
                                            <div class="accordion">
                                                <div class="drawer">
                                                    <div class="accordion-item accordion-item-active">
                                                        <div class="accordion-header accordion-header-active" style="background-color:#E8ECF1">
                                                            <h1>General Details</h1>                                                            
                                                        </div>
                                                        <div class="accordion-content" style="display: block;">
                                                            <div class="tabl">
                                                                <div class="fist-sections">Teacher Number</div>
                                                                <div class="midl-sections">:</div>
                                                                <div class="last-sections" id="lbl_teacherno" runat="server">
                                                                </div>
                                                            </div>
                                                            <div class="tabl">
                                                                <div class="fist-sections">Joining Date</div>
                                                                <div class="midl-sections">:</div>
                                                                <div class="last-sections" id="lbl_teacherjoindate" runat="server">
                                                                </div>
                                                            </div>
                                                            <div class="tabl">
                                                                <div class="fist-sections">Gender</div>
                                                                <div class="midl-sections">:</div>
                                                                <div class="last-sections" id="lbl_teachergender" runat="server">
                                                                </div>
                                                            </div>
                                                            <div class="tabl">
                                                                <div class="fist-sections">Date Of Birth</div>
                                                                <div class="midl-sections">:</div>
                                                                <div class="last-sections" id="lbl_teacherdob" runat="server">
                                                                </div>
                                                            </div>
                                                            <div class="tabl">
                                                                <div class="fist-sections">Teacher Department</div>
                                                                <div class="midl-sections">:</div>
                                                                <div class="last-sections" id="lbl_teacherdept" runat="server">
                                                                </div>
                                                            </div>
                                                            <div class="tabl">
                                                                <div class="fist-sections">Teacher Position</div>
                                                                <div class="midl-sections">:</div>
                                                                <div class="last-sections" id="lbl_teacherposition" runat="server">
                                                                </div>
                                                            </div>
                                                            <div class="tabl">
                                                                <div class="fist-sections">Teacher Category</div>
                                                                <div class="midl-sections">:</div>
                                                                <div class="last-sections" id="lbl_teachercategory" runat="server">
                                                                </div>
                                                            </div>
                                                            <div class="tabl">
                                                                <div class="fist-sections">Teacher Grade</div>
                                                                <div class="midl-sections">:</div>
                                                                <div class="last-sections" id="lbl_teachergrade" runat="server">
                                                                </div>
                                                            </div>
                                                            <div class="tabl">
                                                                <div class="fist-sections">Job Title</div>
                                                                <div class="midl-sections">:</div>
                                                                <div class="last-sections" id="lbl_teachertitle" runat="server">
                                                                </div>
                                                            </div>
                                                            <div class="tabl">
                                                                <div class="fist-sections">Qualification</div>
                                                                <div class="midl-sections">:</div>
                                                                <div class="last-sections" id="lbl_teacherqualification" runat="server">
                                                                </div>
                                                            </div>
                                                            <div class="tabl">
                                                                <div class="fist-sections">Total Experience</div>
                                                                <div class="midl-sections">:</div>
                                                                <div class="last-sections" id="lbl_teachertotalexperience" runat="server">
                                                                </div>
                                                            </div>
                                                            <div class="tabl" style="border-bottom:0px solid">
                                                                <div class="fist-sections">Experience Details</div>
                                                                <div class="midl-sections">:</div>
                                                                <div class="last-sections" id="lbl_teacherexpereincedetails" runat="server">
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>

                                                <div class="drawer">
                                                    <div class="accordion-item">
                                                        <div class="accordion-header" style="background-color:#E8ECF1">
                                                            <h1>Personal Details</h1>                                                            
                                                        </div>
                                                        <div class="accordion-content" style="display: block;">
                                                            <div class="tabl">
                                                                <div class="fist-sections">Marital Status</div>
                                                                <div class="midl-sections">:</div>
                                                                <div class="last-sections" id="lbl_teachermarital_sts" runat="server">
                                                                </div>
                                                            </div>
                                                            <div class="tabl">
                                                                <div class="fist-sections">Children Count</div>
                                                                <div class="midl-sections">:</div>
                                                                <div class="last-sections" id="lbl_teacherchildcnt" runat="server">
                                                                </div>
                                                            </div>
                                                            <div class="tabl">
                                                                <div class="fist-sections">Father Name</div>
                                                                <div class="midl-sections">:</div>
                                                                <div class="last-sections" id="lbl_teacher_fathername" runat="server">
                                                                </div>
                                                            </div>
                                                            <div class="tabl">
                                                                <div class="fist-sections">Mother Name</div>
                                                                <div class="midl-sections">:</div>
                                                                <div class="last-sections" id="lbl_teacher_mothername" runat="server">
                                                                </div>
                                                            </div>
                                                            <div class="tabl">
                                                                <div class="fist-sections">Spouse Name</div>
                                                                <div class="midl-sections">:</div>
                                                                <div class="last-sections" id="lbl_teacher_spousename" runat="server">
                                                                </div>
                                                            </div>
                                                            <div class="tabl">
                                                                <div class="fist-sections">Birth Place</div>
                                                                <div class="midl-sections">:</div>
                                                                <div class="last-sections" id="lbl_teacher_birthplace" runat="server">
                                                                </div>
                                                            </div>
                                                            <div class="tabl">
                                                                <div class="fist-sections">Nationality</div>
                                                                <div class="midl-sections">:</div>
                                                                <div class="last-sections" id="lbl_teachernationality" runat="server">
                                                                </div>
                                                            </div>
                                                             <div class="tabl">
                                                                <div class="fist-sections">Language</div>
                                                                <div class="midl-sections">:</div>
                                                                <div class="last-sections" id="lbl_lang" runat="server">
                                                                </div>
                                                            </div>
                                                              <div class="tabl" style="border-bottom:0px solid">
                                                                <div class="fist-sections">Religion</div>
                                                                <div class="midl-sections">:</div>
                                                                <div class="last-sections" id="lbl_religion" runat="server">
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>

                                                <div class="drawer">
                                                    <div class="accordion-item">
                                                        <div class="accordion-header" style="background-color:#E8ECF1">
                                                            <h1>Contact Details</h1>                                                            
                                                        </div>
                                                        <div class="accordion-content"  style="display: block;">
                                                            <div class="tabl">
                                                                <div class="fist-sections">Address Line 1</div>
                                                                <div class="midl-sections">:</div>
                                                                <div class="last-sections" id="lbl_teacher_addr1" runat="server">
                                                                </div>
                                                            </div>
                                                            <div class="tabl">
                                                                <div class="fist-sections">Address Line 2</div>
                                                                <div class="midl-sections">:</div>
                                                                <div class="last-sections" id="lbl_teacher_addr2" runat="server">
                                                                </div>
                                                            </div>
                                                            <div class="tabl">
                                                                <div class="fist-sections">City</div>
                                                                <div class="midl-sections">:</div>
                                                                <div class="last-sections" id="lbl_teacher_city" runat="server">
                                                                </div>
                                                            </div>
                                                            <div class="tabl">
                                                                <div class="fist-sections">State</div>
                                                                <div class="midl-sections">:</div>
                                                                <div class="last-sections" id="lbl_teacher_state" runat="server">
                                                                </div>
                                                            </div>
                                                            <div class="tabl">
                                                                <div class="fist-sections">Country</div>
                                                                <div class="midl-sections">:</div>
                                                                <div class="last-sections" id="lbl_teacher_country" runat="server">
                                                                </div>
                                                            </div>
                                                            <div class="tabl">
                                                                <div class="fist-sections">Pin Code</div>
                                                                <div class="midl-sections">:</div>
                                                                <div class="last-sections" id="lbl_teacher_pincode" runat="server">
                                                                </div>
                                                            </div>
                                                            <div class="tabl">
                                                                <div class="fist-sections">Phone 1</div>
                                                                <div class="midl-sections">:</div>
                                                                <div class="last-sections" id="lbl_teacher_phone1" runat="server">
                                                                </div>
                                                            </div>
                                                            <div class="tabl">
                                                                <div class="fist-sections">Phone 2</div>
                                                                <div class="midl-sections">:</div>
                                                                <div class="last-sections" id="lbl_teacher_phone2" runat="server">
                                                                </div>
                                                            </div>
                                                            <div class="tabl" style="border-bottom:0px solid">
                                                                <div class="fist-sections">Email</div>
                                                                <div class="midl-sections">:</div>
                                                                <div class="last-sections" id="lbl_teacher_emailid" runat="server">
                                                                </div>
                                                            </div>

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

