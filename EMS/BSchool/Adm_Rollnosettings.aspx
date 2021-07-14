<%@ Page Title="" Language="C#" MasterPageFile="~/Admin_Settings.master" AutoEventWireup="true" CodeFile="Adm_Rollnosettings.aspx.cs" Inherits="Adm_Rollnosettings" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
      <div class="cont_right formWrapper">
        <h1>Roll Number Settings</h1>

           <div class="formCon">
          <div class="formConInner">
            <label>Select Settings for Student Listing</label>
            <br>
            <table style="width:100%" border="0" cellspacing="0" cellpadding="0">
                <tbody><tr>
                    <td> 
                        <asp:RadioButton ID="rd_rollno" runat="server" GroupName="rdgrp" /><label for="type_0">&nbsp;Roll No Only&nbsp;&nbsp;</label> 
                        <asp:RadioButton ID="rd_applicationno" runat="server" GroupName="rdgrp" /><label for="type_1">&nbsp;Admission No Only&nbsp;&nbsp;</label> 
                        <asp:RadioButton ID="rd_both" runat="server" GroupName="rdgrp" /><label for="type_2">&nbsp;Both&nbsp;&nbsp;</label>                    </td>
                </tr>
            </tbody></table>
        </div>
                <div class="submit-btn">                    
                    &nbsp;<asp:Button ID="btn_apply" runat="server" Text="Save" OnClick="btn_apply_Click" CssClass="btn_submitcss" />
                </div>

               </div>

    </div>
</asp:Content>

