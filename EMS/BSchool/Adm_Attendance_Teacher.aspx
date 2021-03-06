<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Adm_Attendance_Teacher.aspx.cs" Inherits="Adm_Attendance_Teacher" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <style>
        .formCon {
            background: url(images/formcon-bg.png) repeat scroll 0 0 #f8fafb;
            border: 0 solid #f68575;
            border-radius: 3px;
            margin: 0 0 20px;
            padding: 0;
            width: 100%;
        }

        .formConInner {
            background: url(images/formcon_new-bg.png) repeat scroll 0 0 #f8fafb;
            border: 1px solid #edbc3a;
            width: auto;
        }
    </style>
    <div id="Div1">
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
                                                <h1>Teacher Attendance Management</h1>

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

                                                <div class="yellow_bx" style="width: 100%">

                                                    <div class="formCon">
                                                        <div class="formConInner_frst">


                                                            <table style="font-weight: normal;">
                                                                <tbody>
                                                                    <tr>
                                                                        <td colspan="4">&nbsp;</td>
                                                                    </tr>

                                                                    <tr id="course_dropdown" style="display: table-row">
                                                                        <td>&nbsp;</td>
                                                                        <td style="width: 200px;"><strong>Select Department</strong></td>
                                                                        <td>&nbsp;</td>
                                                                        <td>
                                                                            <asp:DropDownList ID="drp_dept" CssClass="form-control" runat="server"></asp:DropDownList>
                                                                        </td>
                                                                    </tr>

                                                                    <tr id="filler_1" style="display: table-row">
                                                                        <td colspan="4">&nbsp;</td>
                                                                    </tr>

                                                                    <tr id="day" style="display: table-row">
                                                                        <td>&nbsp;</td>
                                                                        <td style="width: 100px;"><strong>Select Date</strong></td>
                                                                        <td>&nbsp;</td>
                                                                        <td style="width: 200px;">
                                                                            <asp:TextBox ID="tbx_selectdate" runat="server" CssClass="form-control"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr id="Tr1" style="display: table-row">
                                                                        <td>&nbsp;</td>
                                                                        <td style="width: 100px;"></td>
                                                                        <td>&nbsp;</td>
                                                                        <td style="width: 200px;"></td>
                                                                    </tr>


                                                                </tbody>
                                                            </table>


                                                        </div>
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

