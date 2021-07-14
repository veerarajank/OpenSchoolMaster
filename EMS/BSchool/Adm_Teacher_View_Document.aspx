<%@ Page Title="" Language="C#" MasterPageFile="~/GS247.master" AutoEventWireup="true" CodeFile="Adm_Teacher_View_Document.aspx.cs" Inherits="Adm_Teacher_View_Document" %>

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
                                            <li><a href="#" id="lnk_teacher_profile" runat="server" onserverclick="lnk_teacher_profile_ServerClick">Profile</a></li>
                                            <li><a href="#"  id="lnk_teacher_achievement" runat="server" onserverclick="lnk_teacher_achievement_ServerClick">Achievements</a></li>
                                            <li ><a href="#" id="lnk_teacher_log" runat="server" onserverclick="lnk_teacher_log_ServerClick">Log</a></li>
                                            <li class="active"><a href="#" id="lnk_teacher_document" runat="server" onserverclick="lnk_teacher_document_ServerClick">Documents</a></li>
                                            <li><a href="#" id="lnk_teacher_attendance" runat="server" onserverclick="lnk_teacher_attendance_ServerClick">Attendance</a></li>
                                            <li><a href="#"  id="lnk_teacher_subject" runat="server" onserverclick="lnk_teacher_subject_ServerClick">Subject Association</a></li>
                                        </ul>
                                    </div>
                                    <div class="clear"></div>



                                    <div class="emp_cntntbx">

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
                                                                        <asp:TextBox AutoCompleteType="Disabled" runat="server" ID="tbx_tech_documentname" MaxLength="225"></asp:TextBox>
                                                                    </td>
                                                                    <td>
                                                                        <asp:FileUpload ID="flg_fileupload" CssClass="form-control" runat="server" />
                                                                    </td>

                                                                </tr>
                                                            </tbody>
                                                        </table>
                                                    </div>
                                                </div>


                                                <div class="row buttons">
                                                    &nbsp;
                                            <asp:Button ID="btn_addanother" runat="server" Text="Add Another" Height="30px" CssClass="formbut" />
                                                    <asp:Button ID="btn_save" runat="server" Text="Save" CssClass="formbut" Height="30px" />

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

