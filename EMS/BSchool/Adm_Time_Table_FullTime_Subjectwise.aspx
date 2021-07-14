<%@ Page Title="" Language="C#" MasterPageFile="~/Adm_TimeTable.master" AutoEventWireup="true" CodeFile="Adm_Time_Table_FullTime_Subjectwise.aspx.cs" Inherits="Adm_Time_Table_FullTime_Subjectwise" %>

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
    </style>
    <div class="cont_right formWrapper">
        <div class="clear"></div>
        <div class="emp_right_contner">
            <div class="emp_tabwrapper">
                <div class="clear"></div>
                <div class="emp_cntntbx" style="padding-top: 10px;">
                    <h1>View Full Timetable </h1>
                    <div class="formCon">
                        <div class="formConInner_frst">
                            <table style="font-weight: normal;">
                                <tbody>
                                      <tr>
                                        <td colspan="4">&nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td>&nbsp;</td>
                                        <td style="width: 200px;"><strong>Select Academic Year</strong></td>
                                        <td>&nbsp;</td>
                                        <td>
                                            <asp:DropDownList ID="drp_academic_year"  runat="server"></asp:DropDownList>
                                        </td>
                                    </tr>

                                    <tr>
                                        <td colspan="4">&nbsp;</td>
                                    </tr>

                                    <tr id="course_dropdown" style="display: table-row">
                                        <td>&nbsp;</td>
                                        <td style="width: 200px;"><strong>Select Course</strong></td>
                                        <td>&nbsp;</td>
                                        <td>
                                           <asp:DropDownList ID="drp_course"    runat="server" AutoPostBack="true" OnSelectedIndexChanged="drp_course_SelectedIndexChanged"></asp:DropDownList>
                                        </td>
                                    </tr>

                                    <tr id="filler_1" style="display: table-row">
                                        <td colspan="4">&nbsp;</td>
                                    </tr>
                                    <tr id="batch_dropdown" style="display: table-row">
                                        <td>&nbsp;</td>
                                        <td><strong>Select Batch</strong></td>
                                        <td>&nbsp;</td>
                                        <td>
                                             <asp:DropDownList ID="drp_batch"    runat="server" AutoPostBack="true" OnSelectedIndexChanged="drp_batch_SelectedIndexChanged"></asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr id="filler_2" style="display: table-row">
                                        <td colspan="4">&nbsp;</td>
                                    </tr>

                                     <tr id="Tr2" style="display: table-row">
                                        <td>&nbsp;</td>
                                        <td><strong>Select Semester</strong></td>
                                        <td>&nbsp;</td>
                                        <td>
                                           <asp:DropDownList ID="drp_semester"  runat="server" AutoPostBack="true" OnSelectedIndexChanged="drp_semester_SelectedIndexChanged"></asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr id="Tr3" style="display: table-row">
                                        <td colspan="4">&nbsp;</td>
                                    </tr>

                                    <tr>
                                        <td>&nbsp;</td>
                                        <td style="width: 100px;"><strong>Select Mode</strong></td>
                                        <td>&nbsp;</td>
                                        <td>
                                            <asp:DropDownList ID="drp_mode" runat="server">
                                                <asp:ListItem Text="Select Mode" Value="0"></asp:ListItem>
                                                <asp:ListItem Text="Week" Value="Week"></asp:ListItem>
                                                <asp:ListItem Text="Day" Value="Day"></asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr id="filler_3" style="display: table-row">
                                        <td colspan="4">&nbsp;</td>
                                    </tr>

                                    <tr id="day" style="display: table-row">
                                        <td>&nbsp;</td>
                                        <td style="width: 100px;"><strong>Select Day</strong></td>
                                        <td>&nbsp;</td>
                                        <td style="width: 200px;">
                                            <asp:DropDownList ID="drp_days"  runat="server">
                                                <asp:ListItem Text="Select Days" Value="0"></asp:ListItem>
                                                <asp:ListItem Text="Sunday" Value="1"></asp:ListItem>
                                                <asp:ListItem Text="Monday" Value="2"></asp:ListItem>
                                                <asp:ListItem Text="Tuesday" Value="3"></asp:ListItem>
                                                <asp:ListItem Text="Wednesday" Value="4"></asp:ListItem>
                                                <asp:ListItem Text="Thursday" Value="5"></asp:ListItem>
                                                <asp:ListItem Text="Friday" Value="6"></asp:ListItem>
                                                <asp:ListItem Text="Saturday" Value="7"></asp:ListItem>
                                            </asp:DropDownList>
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


                    <div class="formCon">
                        <div class="formConInner">
                            <table style="text-align: left;">
                                <tbody>
                                    <tr>
                                        <td style="width: auto; min-width: 200px;">Course : 
                                                <asp:Label ID="lbl_course" runat="server"></asp:Label>
                                        </td>
                                        <td width="20px">&nbsp;</td>
                                        <td style="width: auto; min-width: 200px;">Batch : 
                                                <asp:Label ID="lbl_batch" runat="server"></asp:Label>
                                        </td>
                                        <td width="20px">&nbsp;</td>
                                        <td>Class Teacher : 
                                                <asp:Label ID="lbl_teacher" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                    </div>

                    <style>
                        .timetable i {
                            color: #fff !important;
                        }
                    </style>


                    <div class="button-bg button-bg-oneside">
                        <div class="top-hed-btn-right">
                            <a class="a-tag-pdf-btn" target="_blank" href="#">Generate PDF</a>
                        </div>
                    </div>
                    <div class="timetable" style="text-align: center">
                    </div>
                    <br>
                    <br>
                </div>

            </div>
        </div>
    </div>

</asp:Content>

