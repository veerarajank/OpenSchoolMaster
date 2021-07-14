<%@ Page Title="" Language="C#" MasterPageFile="~/AcademicConfiguration.master" AutoEventWireup="true" CodeFile="Previous_Year_settings.aspx.cs" Inherits="Previous_Year_settings" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="cont_right formWrapper">
        <h1>Previous Year Settings</h1>
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

                    <table style="width:100%">
                        <tbody>
                            <tr>
                                <td>
                                    <h3>By default, you will be able to perform only View, Edit and Approve actions in the other academic years.</h3>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:RadioButton ID="rd_defaultsettings" runat="server" GroupName="settings" />
                                    <label for="PreviousYearSettings_setting_0">Use Default Settings</label><br>
                                    <br>
                                    <asp:RadioButton ID="rd_customsettings" runat="server" GroupName="settings" />
                                    <label for="PreviousYearSettings_setting_1">Use Custom Settings</label>
                                </td>
                            </tr>
                        </tbody>
                    </table>

                </div>
            </div>


            <div class="formCon" id="custom_form">
                <div class="formConInner">
                    <h3>Select the actions that should be available while viewing the previous academic years.</h3>
                    <table style="width:60%" border="0" cellspacing="0" cellpadding="0">
                        <tbody>
                            <tr>
                                <td>
                                     <asp:CheckBox ID="chk_create" runat="server" />
                                    Create                    </td>
                            </tr>
                            <tr>
                                <td>&nbsp;</td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:CheckBox ID="chk_insert" runat="server" />
                                    Insert                    </td>
                            </tr>
                            <tr>
                                <td>&nbsp;</td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:CheckBox ID="chk_edit" runat="server" />
                                    Edit                    </td>
                            </tr>
                            <tr>
                                <td>&nbsp;</td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:CheckBox ID="chk_delete" runat="server" />
                                    Delete                    </td>
                            </tr>
                            <tr>
                                <td>&nbsp;</td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:CheckBox ID="chk_approve" runat="server" />
                                    Approve                    </td>
                            </tr>
                            <tr>
                                <td>&nbsp;</td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:CheckBox ID="chk_disapprove" runat="server" />
                                    Disapprove                    </td>
                            </tr>
                            <tr>
                                <td>&nbsp;</td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:CheckBox ID="chk_active" runat="server" />
                                    Active                    </td>
                            </tr>
                            <tr>
                                <td>&nbsp;</td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:CheckBox ID="chk_inactive" runat="server" />
                                    Inactive / Deactivate                    </td>
                            </tr>
                        </tbody>
                    </table>
                </div>

            </div>

            <br>
            <div class="row buttons">
                <asp:Button ID="btn_save" ValidationGroup="vd_save" runat="server" Text="Save" OnClick="btn_save_Click" Width="150px" CssClass="a_tag-btn" />
            </div>


        </div>
        <!-- form -->
    </div>
</asp:Content>

