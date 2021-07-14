<%@ Page Title="" Language="C#" MasterPageFile="~/Admin_Settings.master" AutoEventWireup="true" CodeFile="Adm_SetAcademicSettings.aspx.cs" Inherits="Adm_SetAcademicSettings" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
      <div class="cont_right formWrapper">
        <h1>Set up Current Academic Settings</h1>
        <div class="formCon">
            <div class="formConInner">
                <div class="text-fild-bg-block">
                    <div class="text-fild-block inputstyle">
                        <label class="opnsl_label required" for="OnlineRegisterSetting2_academic_year">Academic Year <span class="required">*</span></label>
                        <asp:DropDownList ID="drp_academic_year" runat="server"></asp:DropDownList>
                    </div>
                 

                </div>
                <div class="submit-btn">                    
                    <asp:Button ID="btn_apply" runat="server" Text="Apply" OnClick="btn_apply_Click" CssClass="btn_submitcss" />
                </div>
            </div>
        </div>
</asp:Content>

