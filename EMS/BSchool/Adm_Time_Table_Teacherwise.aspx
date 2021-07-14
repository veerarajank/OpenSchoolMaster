<%@ Page Title="" Language="C#" MasterPageFile="~/Adm_TimeTable.master" AutoEventWireup="true" CodeFile="Adm_Time_Table_Teacherwise.aspx.cs" Inherits="Adm_Time_Table_Teacherwise" %>

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
    <div class="cont_right formWrapper">
        <div class="clear"></div>
        <div class="emp_right_contner">
            <div class="emp_tabwrapper">
                <div class="clear"></div>
                <div class="emp_cntntbx" style="padding-top: 10px;">
                    <h1><b>View Teacher Timetable</b></h1>
                    <div class="formCon">
                        <div class="formConInner_frst">
                            <table style="font-weight: normal;">
                                <tbody>
                                      <tr>
                                        <td colspan="4">&nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td>&nbsp;</td>
                                        <td style="width: 200px;"><strong>Select Department</strong></td>
                                        <td>&nbsp;</td>
                                        <td>
                                            <asp:DropDownList ID="drp_teacherdept" runat="server"></asp:DropDownList>
                                        </td>
                                    </tr>

                                    <tr>
                                        <td colspan="4">&nbsp;</td>
                                    </tr>

                                    <tr id="course_dropdown" style="display: table-row">
                                        <td>&nbsp;</td>
                                        <td style="width: 200px;"><strong>Select Teacher</strong></td>
                                        <td>&nbsp;</td>
                                        <td>
                                            <asp:DropDownList ID="drp_teacher" runat="server"></asp:DropDownList>
                                        </td>
                                    </tr>

                                    <tr id="filler_1" style="display: table-row">
                                        <td colspan="4">&nbsp;</td>
                                    </tr>
                                    <tr id="batch_dropdown" style="display: table-row">
                                        <td>&nbsp;</td>
                                        <td><strong>Select Day</strong></td>
                                        <td>&nbsp;</td>
                                        <td>
                                            <asp:DropDownList ID="drp_day" runat="server">
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
                                    <tr id="filler_3" style="display: table-row">
                                        <td colspan="4">&nbsp;</td>
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
                                        <td style="width: auto; min-width: 200px;">Name : 															</td>
                                        <td width="20px">&nbsp;&nbsp;</td>
                                        <td width="20px">&nbsp;</td>
                                        <td style="width: auto; min-width: 200px;">Teacher Number : 													</td>
                                        <td width="20px">&nbsp;</td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                    </div>







                </div>

            </div>
        </div>
    </div>

</asp:Content>

