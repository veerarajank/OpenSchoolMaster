<%@ Page Title="" Language="C#" MasterPageFile="~/Admin_Settings.master" AutoEventWireup="true" CodeFile="NotificationSettings.aspx.cs" Inherits="NotificationSettings" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

    <div class="cont_right formWrapper">
        <h1><b>Notification Settings</b></h1>
        <style>
            .check {
                width: 10px;
            }

            #spec {
                display: none;
            }
        </style>
        <div class="form">
            <div class="formCon">
                <div class="formConInner">
                    <span style="color:red">
                    *Please select recipients (Student, Guardians, Teacher) for SMS / Email / Message function.</span>
                     <asp:ListView ID="lv_notificationsettings" runat="server" GroupPlaceholderID="groupPlaceHolder1"
                                ItemPlaceholderID="itemPlaceHolder1">
                                <LayoutTemplate>
                                    <table class="table table-bordered table-responsive table-hover dataTable" style="border: 2px solid #d2d3d4!important;">
                                        <tr class="bg-blue">
                                           
                                            <th rowspan="2">Function
                                            </th>
                                            <th>SMS
                                            </th>
                                            <th>Email
                                            </th>
                                            <th>Message
                                            </th>
                                            <th>Student
                                            </th>
                                            <th>Guardian
                                            </th>
                                            <th>Teacher
                                            </th>

                                        </tr>
                                        <tr class="bg-blue">

                                            <th>
                                                <asp:CheckBox ID="chk_sms" Text="" runat="server" AutoPostBack="true" OnCheckedChanged="chk_sms_CheckedChanged" />
                                            </th>
                                            <th>
                                                <asp:CheckBox ID="chk_email" Text="" runat="server" AutoPostBack="true" OnCheckedChanged="chk_email_CheckedChanged" />
                                            </th>
                                            <th>
                                                <asp:CheckBox ID="chk_msg" Text="" runat="server" AutoPostBack="true" OnCheckedChanged="chk_msg_CheckedChanged" />
                                            </th>
                                            <th>
                                                <asp:CheckBox ID="chk_student" Text="" runat="server" AutoPostBack="true" OnCheckedChanged="chk_student_CheckedChanged" />
                                            </th>
                                            <th>
                                                <asp:CheckBox ID="chk_guardian" Text="" runat="server" AutoPostBack="true" OnCheckedChanged="chk_guardian_CheckedChanged" />
                                            </th>
                                            <th>
                                                <asp:CheckBox ID="chk_teacher" Text="All" runat="server" AutoPostBack="true" OnCheckedChanged="chk_teacher_CheckedChanged" />
                                            </th>

                                        </tr>
                                        <asp:PlaceHolder runat="server" ID="groupPlaceHolder1"></asp:PlaceHolder>
                                    </table>
                                </LayoutTemplate>
                                <GroupTemplate>
                                    <tr>
                                        <asp:PlaceHolder runat="server" ID="itemPlaceHolder1"></asp:PlaceHolder>
                                    </tr>
                                </GroupTemplate>
                                <ItemTemplate>
                                    <td style="border: 1px solid #b0adb0">
                                        <%# Eval("FunctionName") %>
                                    </td>
                                    <td style="border: 1px solid #b0adb0">
                                        <asp:CheckBox ID="chk_sms_add" runat="server" Visible='<%# Convert.ToInt32(Eval("issms"))==1 ? true : false %>' Checked='<%# Convert.ToInt32(Eval("sms_enable"))==1 ? true : false %>' />
                                    </td>
                                    <td style="border: 1px solid #b0adb0">
                                        <asp:CheckBox ID="chk_email_add" runat="server" Visible='<%# Convert.ToInt32(Eval("isemail"))==1 ? true : false  %>' Checked='<%# Convert.ToInt32(Eval("email_enable"))==1 ? true : false %>' />
                                    </td>
                                    <td style="border: 1px solid #b0adb0">
                                        <asp:CheckBox ID="chk_msg_add" runat="server" Visible='<%# Convert.ToInt32(Eval("ismsg"))==1 ? true : false  %>' Checked='<%# Convert.ToInt32(Eval("msg_enable"))==1 ? true : false %>' />
                                    </td>
                                    <td style="border: 1px solid #b0adb0">
                                        <asp:CheckBox ID="chk_student_add" runat="server" Visible='<%# Convert.ToInt32(Eval("isstudent"))==1 ? true : false  %>' Checked='<%# Convert.ToInt32(Eval("student_enable"))==1 ? true : false %>' />
                                    </td>
                                    <td style="border: 1px solid #b0adb0">
                                        <asp:CheckBox ID="chk_guardian_add" runat="server" Visible='<%# Convert.ToInt32(Eval("isguardian"))==1 ? true : false  %>' Checked='<%# Convert.ToInt32(Eval("guardian_enable"))==1 ? true : false %>' />
                                    </td>
                                    <td style="border: 1px solid #b0adb0">
                                        <asp:CheckBox ID="chk_teacher_add" runat="server" Visible='<%# Convert.ToInt32(Eval("isteacher"))==1 ? true : false  %>' Checked='<%# Convert.ToInt32(Eval("teacher_enable"))==1 ? true : false %>' />
                                        <asp:HiddenField ID="hd_notifiyid" runat="server" Value='<%# Convert.ToString(Eval("notifyid"))%>' />
                                    </td>

                                </ItemTemplate>
                                <EmptyDataTemplate>
                                    <table class="table table-bordered table-responsive table-hover dataTable">
                                        <tr>
                                            <td>No Page Retriction Found!
                                            </td>
                                        </tr>
                                    </table>
                                </EmptyDataTemplate>
                            </asp:ListView>

                     <div class="row buttons">
                <asp:Button ID="btn_save" ValidationGroup="vd_save" runat="server" Text="Save" OnClick="btn_save_Click" Width="150px" CssClass="a_tag-btn" />
            </div>

                </div>
                  
            </div>
        </div>
    </div>
</asp:Content>

