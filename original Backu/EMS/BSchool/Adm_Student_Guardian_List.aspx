<%@ Page Title="" Language="C#" MasterPageFile="~/Student_Settings.master" AutoEventWireup="true" CodeFile="Adm_Student_Guardian_List.aspx.cs" Inherits="Adm_Student_Guardian_List" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">



    <div id="content">
        <br />
        <div runat="server" id="search_section">
            <div class="formCon">
                <div class="formConInner">
                    <table width="100%" border="0" cellspacing="0" cellpadding="0" class="s_search">
                        <tbody>
                            <tr>
                                <td width="5%">Name</td>
                                <td width="23%">
                                    <div class="exp_but_input-full">

                                        <asp:TextBox ID="tbx_studentname" placeholder="Student Name" runat="server" AutoCompleteType="Disabled"></asp:TextBox>
                                    </div>
                                </td>
                                <td width="5%"></td>
                                <td width="23%">
                                    <div>
                                        <asp:Button ID="btn_search" runat="server" Text="Search" OnClick="btn_search_Click" class="formbut-n" />

                                    </div>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </div>
                <!-- END div class="formConInner" -->
            </div>
            <!--  END div class="formCon" -->

            <div id="academic-years-grid" class="grid-view">

                <div class="summary" id="lbl_cnt" runat="server" style="font-size: 12px;">
                    Displaying 2 result(s).
                </div>
                <div class="grid_table_con">
                    <asp:ListView ID="lv_gaurdiansdetails" runat="server" GroupPlaceholderID="groupPlaceHolder1"
                        ItemPlaceholderID="itemPlaceHolder1">
                        <LayoutTemplate>
                            <table style="width: 100%" cellpadding="0" cellspacing="0">
                                <thead style="background-color: #E8ECF1">
                                    <td align="left">First Name                      
                                    </td>
                                    <td align="left">Last Name                       
                                    </td>
                                    <td align="left">Student Name                       
                                    </td>
                                    <td align="center">Relation</td>
                                    <td align="center">Email</td>
                                    <td align="center"></td>

                                </thead>
                                <asp:PlaceHolder runat="server" ID="groupPlaceHolder1"></asp:PlaceHolder>

                            </table>
                        </LayoutTemplate>
                        <GroupTemplate>
                            <tr style="border: 1px solid #b9c7d0!important;">
                                <asp:PlaceHolder runat="server" ID="itemPlaceHolder1"></asp:PlaceHolder>
                            </tr>
                        </GroupTemplate>
                        <ItemTemplate>

                            <td align="left"><%# Eval("first_name") %></td>
                            <td align="left"><%# Eval("last_name") %></td>
                            <td align="left"><%# Eval("Student_name") %></td>
                            <td align="center"><%# Eval("relation") %></td>
                            <td align="center"><%# Eval("email") %></td>
                            <td>
                                <div class="tt-wrapper-new">

                                    <%# Convert.ToString(Eval("is_parent_user"))=="1" ?
                                                    "<a class='makeprimary-active' id='lnk_maked'><span>Primary Contact</span></a>"                                                     
                                                    :
                                                    "<a class='makeprimary' onclick='Primary_contact_Popup(\"" + Convert.ToString(Eval("id"))  + "\")' id='lnk_maked'><span>Make Primary Contact</span></a>"                                                                                                     
                                                
                                    %>

                                    <%# Convert.ToString(Eval("emergency_user"))=="1" ?
                                                    "<a class='makeemrgency-active' id='lnk_makedemergency'><span>Emergency Contact</span></a>"                                                     
                                                    :
                                                    "<a class='makeemrgency' onclick='Emergency_contact_Popup(\"" + Convert.ToString(Eval("id"))  + "\")' id='lnk_maked'><span>Make Emergency Contact</span></a>"                                                                                                     
                                                
                                    %>





                                    <asp:LinkButton ID="lnk_edit" CssClass="makeedit" CommandName="edit" CommandArgument='<%# Eval("id") %>' OnCommand="lnk_edit_Command" runat="server" Visible='<%# Convert.ToBoolean(Session["Access_Edit"])%>'>
                                    <span>Edit</span>
                                    </asp:LinkButton>&nbsp;

                                <a class="makedelete" id="lnk_del" onclick='DeletePopup("<%# Eval("id") %>")' style='<%# Convert.ToBoolean(Session["Access_Trash"])==true ? "visibility:visible": "visibility:hidden" %>'>
                                    <span>Remove from guardian list</span>
                                </a>


                                </div>



                            </td>
                        </ItemTemplate>
                        <EmptyDataTemplate>
                            <table width="100%" cellpadding="0" cellspacing="0">
                                <thead style="border: 1px solid #b9c7d0!important; background-color: #E8ECF1">
                                    <td align="left" width="300">Name                       
                                    </td>
                                    <td align="center" width="150">Relation</td>
                                    <td align="center" width="150">Email</td>
                                    <td align="center" width="300"></td>
                                </thead>
                                <tr style="border: 1px solid #b9c7d0!important;">
                                    <td colspan="4">No Results Found
                                    </td>
                                </tr>
                            </table>
                        </EmptyDataTemplate>
                    </asp:ListView>

                </div>
            </div>


        </div>
        <asp:HiddenField ID="hd_id" runat="server" />



        <div id="update_section" runat="server" visible="false">
            <p class="note">Fields with <span class="required">*</span> are required.</p>
            <div class="formCon">
                <div class="formConInner opnsl_form_label">
                    <h3>Personal Details</h3>

                    <div class="txtfld-col">
                        <label for="Guardians_first_name" class="required">First Name <span class="required">*</span></label>
                        <asp:TextBox ID="tbx_Guardians_first_name" AutoCompleteType="Disabled" runat="server" MaxLength="255"></asp:TextBox>
                    </div>

                    <div class="txtfld-col">
                        <label for="Guardians_last_name" class="required">Last Name <span class="required">*</span></label>
                        <asp:TextBox ID="tbx_Guardians_last_name" AutoCompleteType="Disabled" runat="server" MaxLength="255"></asp:TextBox>
                    </div>

                    <div class="txtfld-col">
                        <label for="Guardians_relation" class="required">Relation <span class="required">*</span></label>
                        <asp:DropDownList ID="drp_Guardians_relation" runat="server" AutoPostBack="true" OnSelectedIndexChanged="drp_Guardians_relation_SelectedIndexChanged">
                             <asp:ListItem Value="0" Text="--Select--"></asp:ListItem>
                            <asp:ListItem Value="Father" Text="Father"></asp:ListItem>
                            <asp:ListItem Value="Mother" Text="Mother"></asp:ListItem>
                            <asp:ListItem Value="Grand Father" Text="Grand Father"></asp:ListItem>
                            <asp:ListItem Value="Grand Mother" Text="Grand Mother"></asp:ListItem>
                            <asp:ListItem Value="Brother" Text="Brother"></asp:ListItem>
                            <asp:ListItem Value="Sister" Text="Sister"></asp:ListItem>
                            <asp:ListItem Value="Others" Text="Others"></asp:ListItem>

                        </asp:DropDownList>

                        <div id="relation_other" runat="server" visible="false" style="margin-top: 10px;">
                            <label class="relation-star" for="Guardians_relation_other">Specify Relation</label><span class="required"> *</span>
                            <asp:TextBox ID="tbx_Guardians_relation_other" AutoCompleteType="Disabled" runat="server" MaxLength="255"></asp:TextBox>
                        </div>

                    </div>

                    <div class="txtfld-col">
                        <label for="Guardians_dob">Date Of Birth</label>
                        <asp:TextBox ID="tbx_Guardians_dob" runat="server" AutoCompleteType="Disabled" MaxLength="255" class="hasDatepicker"></asp:TextBox>
                    </div>

                    <div class="txtfld-col">
                        <label for="Guardians_education">Education</label>
                        <asp:TextBox ID="tbx_Guardians_education" runat="server" AutoCompleteType="Disabled" MaxLength="255"></asp:TextBox>
                    </div>

                    <div class="txtfld-col">
                        <label for="Guardians_occupation">Occupation</label>
                        <asp:TextBox ID="tbx_Guardians_occupation" runat="server" AutoCompleteType="Disabled" MaxLength="255"></asp:TextBox>
                    </div>

                    <div class="txtfld-col">
                        <label for="Guardians_income">Income</label>
                        <asp:TextBox ID="tbx_Guardians_income" runat="server" AutoCompleteType="Disabled" MaxLength="255"></asp:TextBox>
                    </div>



                    <div class="clear"></div>
                </div>
            </div>

            <div class="formCon">
                <div class="formConInner opnsl_form_label">
                    <h3>Contact Details</h3>
                    <div class="txtfld-col">
                        <label for="Guardians_email" class="required">Email <span class="required">*</span></label>
                        <asp:TextBox ID="tbx_Guardians_email" runat="server" AutoCompleteType="Disabled" MaxLength="255"></asp:TextBox>
                    </div>

                    <div class="txtfld-col">
                        <label for="Guardians_mobile_phone" class="required">Mobile Phone <span class="required">*</span></label>
                        <asp:TextBox ID="tbx_Guardians_mobile_phone" runat="server" AutoCompleteType="Disabled" MaxLength="255"></asp:TextBox>
                    </div>

                    <div class="txtfld-col">
                        <div class="show">
                            <label for="Guardians_office_phone1">Phone 1</label>
                            <asp:TextBox ID="tbx_Guardians_office_phone1" runat="server" AutoCompleteType="Disabled" MaxLength="255"></asp:TextBox>
                        </div>
                    </div>

                    <div class="txtfld-col">
                        <div class="show">
                            <label for="Guardians_office_phone2">Phone 2</label>
                            <asp:TextBox ID="tbx_Guardians_office_phone2" runat="server" AutoCompleteType="Disabled" MaxLength="255"></asp:TextBox>
                        </div>
                    </div>

                    <div class="txtfld-col show">
                        <label for="Guardians_office_address_line1">Address Line 1</label>
                        <asp:TextBox ID="tbx_Guardians_office_address_line1" runat="server" AutoCompleteType="Disabled" MaxLength="255"></asp:TextBox>
                    </div>

                    <div class="txtfld-col show">
                        <label for="Guardians_office_address_line2">Address Line 2</label>
                        <asp:TextBox ID="tbx_Guardians_office_address_line2" runat="server" AutoCompleteType="Disabled" MaxLength="255"></asp:TextBox>
                    </div>

                    <div class="txtfld-col show">
                        <label for="Guardians_city">City </label>
                        <asp:TextBox ID="tbx_Guardians_city" runat="server" AutoCompleteType="Disabled" MaxLength="255"></asp:TextBox>
                    </div>

                    <div class="txtfld-col show">
                        <label for="Guardians_state">State</label>
                        <asp:TextBox ID="tbx_Guardians_state" runat="server" AutoCompleteType="Disabled" MaxLength="255"></asp:TextBox>
                    </div>

                    <div class="txtfld-col show">
                        <label for="Guardians_country">Country</label>
                        <asp:DropDownList ID="drp_Guardians_country" runat="server"></asp:DropDownList>
                    </div>



                   

                    

                    <div class="clear"></div>
                </div>
            </div>
            <div class="clear"></div>

            <div class="top-hed-btn-right">
                <ul>
                    <li>
                        <button id="btn_update" runat="server" onserverclick="btn_update_ServerClick" style="width: 150px" class="a_tag-btn"><span>Save</span></button>
                    </li>
                </ul>
            </div>

        </div>

    </div>

</asp:Content>

