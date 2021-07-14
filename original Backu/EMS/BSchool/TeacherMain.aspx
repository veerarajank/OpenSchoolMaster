<%@ Page Title="" Language="C#" MasterPageFile="~/Adm_Teacher_Settings.master" AutoEventWireup="true" CodeFile="TeacherMain.aspx.cs" Inherits="TeacherMain" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="cont_right">
        <h1>Teacher Dashboard</h1>
        <div class="overview">
            <div class="overviewbox ovbox1" style="margin-left: 0px;">
                <h1><strong>Total Teachers</strong></h1>
                <div class="ovrBtm">0</div>
            </div>
            <div class="overviewbox ovbox2">
                <h1><strong>Recently Hired</strong></h1>
                <div class="ovrBtm">0</div>
            </div>
            <div class="clear"></div>

        </div>
        <div class="clear"></div>
        <div style="margin-top: 20px; width: 90%" id="container"></div>

        <div class="pdtab_Con" style="width: 97%">
            <div style="font-size: 13px; padding: 5px 0px"><strong>Recent Teacher Admissions</strong></div>
            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                <tbody>
                    <tr class="pdtab-h">
                        <td align="center" height="18">Date</td>
                        <td align="center">Teacher Name</td>
                        <td align="center">Teacher No</td>
                        <td align="center">Department</td>
                        <td align="center">Position</td>

                    </tr>
                </tbody>
                <tbody>
                    <tr>
                        <td style="text-align: center" colspan="5">No Data Available!</td>
                    </tr>

                </tbody>
            </table>
        </div>
    </div>
</asp:Content>

