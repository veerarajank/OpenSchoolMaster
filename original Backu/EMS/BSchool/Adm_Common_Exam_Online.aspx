<%@ Page Title="" Language="C#" MasterPageFile="~/Adm_Examination.master" AutoEventWireup="true" CodeFile="Adm_Common_Exam_Online.aspx.cs" Inherits="Adm_Common_Exam_Online" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="cont_right formWrapper">
        <h1><b>Online Exams List</b></h1>

        <div class="button-bg">
            <div class="top-hed-btn-left"></div>
            <div class="top-hed-btn-right">
                <ul>
                    <li><a class="a_tag-btn" href="#"><span>Add New Exam</span></a></li>
                </ul>
            </div>

        </div>
        <div class="pdtab_Con" style="width: 100%">
            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                <tbody>
                    <tr class="pdtab-h">
                        <td align="center" width="7%">Sl No</td>
                        <td height="" align="center" width="20%">Name</td>
                        <td align="center" width="15%">Course</td>
                        <td align="center" width="15%">Batch</td>
                        <td align="center" width="15%">Semester</td>
                        <td align="center" width="20%">Status</td>
                        <td align="center" width="25%">Manage</td>
                    </tr>
                   

                </tbody>
            </table>
        </div>
        <div class="pagecon">
        </div>

    </div>
</asp:Content>

