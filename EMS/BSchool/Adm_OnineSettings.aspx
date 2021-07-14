<%@ Page Title="" Language="C#" MasterPageFile="~/Admin_Settings.master" AutoEventWireup="true" CodeFile="Adm_OnineSettings.aspx.cs" Inherits="Adm_OnineSettings" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="cont_right formWrapper">
        <h1>Online Registration Settings</h1>




        <div class="formCon">
            <div class="formConInner">
                <div class="text-fild-bg-block">
                    <div class="text-fild-block inputstyle">
                        <label class="opnsl_label required" for="OnlineRegisterSetting2_academic_year">Academic Year <span class="required">*</span></label>
                        <asp:DropDownList ID="drp_academic_year" runat="server"></asp:DropDownList>


                    </div>
                    <div class="text-fild-block inputstyle">
                        <label class="opnsl_label" for="OnlineRegisterSetting2_show_link">Show Link for Online Admission</label><div>
                            <asp:CheckBox ID="chk_showlink" runat="server" Checked="true" />
                        </div>
                    </div>

                </div>
                <div class="submit-btn">                    
                    <asp:Button ID="btn_apply" runat="server" Text="Apply" OnClick="btn_apply_Click" CssClass="btn_submitcss" />
                </div>
            </div>
</asp:Content>

