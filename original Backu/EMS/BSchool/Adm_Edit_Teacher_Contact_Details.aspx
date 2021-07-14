<%@ Page Title="" Language="C#" MasterPageFile="~/Adm_Teacher_Settings.master" AutoEventWireup="true" CodeFile="Adm_Edit_Teacher_Contact_Details.aspx.cs" Inherits="Adm_Edit_Teacher_Contact_Details" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
   
                        <style>
                            a.add {
                                display: block;
                                margin: 10px 0 0;
                                padding: 2px 5px;
                                width: 60px;
                                height: 30px;
                                background-color: #379bc9;
                                border-radius: 3px;
                                color: #fff;
                            }

                                a.add:hover {
                                    background-color: #318db7;
                                }

                            .add img {
                                color: #98adb5;
                                display: inline;
                                font-size: 14px;
                                margin-right: 5px;
                                width: 19px;
                                height: 20px;
                                float: left;
                                margin-left: 0px;
                                background: none;
                            }

                            .add span.fcount {
                                display: inline-block;
                                padding-top: 8px;
                                padding-left: 4px;
                                color: #FFF;
                                font-weight: bold;
                                font-size: 16px;
                            }

                            .accordion-header {
                                background-color: #fff;
                                padding: 7px;
                                cursor: pointer;
                                border-right: 1px solid #E0E0E0;
                                border-left: 1px solid #E0E0E0;
                                min-height: 30px;
                                transition: .25s;
                            }
                        </style>
                      
                        <div class="cont_right formWrapper">
                            <h1>TEACHER PROFILE</h1>
                            <div class="emp_right_contner">
                                <div class="emp_tabwrapper">
                                    <div class="clear"></div>
                                    <div class="page-tab">
                                        <div class="pagetab-bg">
                                            <ul>
                                                <li>
                                                  <a href="#" runat="server" id="teacher_info" onserverclick="teacher_info_ServerClick">  <h2 class="no_hvr_style">Teacher Details</h2></a>
                                                </li>
                                                <li class="active ">
                                                    <a href="#" runat="server" id="teacher_contact" onserverclick="teacher_contact_ServerClick"><h2 class="no_hvr_style" >Teacher Contact Details</h2></a>
                                                </li>
                                                <li class=" ">
                                                    <a href="#" runat="server" id="teacher_document" onserverclick="teacher_document_ServerClick"><h2 class="no_hvr_style">Teacher Documents</h2></a>
                                                </li>
                                            </ul>
                                        </div>
                                    </div>
                                </div>

                                <style>
                                    .formCon input[type="text"], input[type="password"], textArea, select {
                                        border-radius: 0px !important;
                                        border: 1px #c2cfd8 solid;
                                        padding: 7px 3px;
                                        background: #fff;
                                        box-shadow: none !important;
                                        width: 100%;
                                    }
                                </style>


                             <br />
                                


                            

                                <div class="formCon">
                                    <div class="formConInner opnsl_form_label">
                                        <h3>Contact Details</h3>

                                        <div class="txtfld-col">
                                            <label for="teacher_address_line1">Address Line 1</label>
                                            <asp:TextBox ID="tbx_add1" AutoCompleteType="Disabled" runat="server" MaxLength="255"></asp:TextBox>
                                        </div>

                                        <div class="txtfld-col">
                                            <label for="teacher_address_line2">Address Line 2</label>
                                            <asp:TextBox ID="tbx_add2" AutoCompleteType="Disabled" runat="server" MaxLength="255"></asp:TextBox>
                                        </div>

                                        <div class="txtfld-col">
                                            <label for="teacher_city">City</label>
                                            <asp:TextBox ID="tbx_city" AutoCompleteType="Disabled" runat="server" MaxLength="255"></asp:TextBox>
                                        </div>

                                        <div class="txtfld-col">
                                            <label for="teacher_state">State</label>
                                            <asp:TextBox ID="tbx_state" AutoCompleteType="Disabled" runat="server" MaxLength="255"></asp:TextBox>
                                        </div>

                                        <div class="txtfld-col">
                                            <label for="teacher_pin_code">Pin Code</label>
                                            <asp:TextBox ID="tbx_pincode" AutoCompleteType="Disabled" runat="server" MaxLength="255"></asp:TextBox>
                                        </div>

                                        <div class="txtfld-col">
                                            <label for="teacher_country_id">Country</label>
                                            <asp:DropDownList ID="drp_contry" AutoCompleteType="Disabled" runat="server"></asp:DropDownList>
                                        </div>

                                        <div class="txtfld-col">
                                            <label for="teacher_phone1" class="required">Phone Number <span class="required">*</span></label>
                                            <asp:TextBox ID="tbx_phone1" AutoCompleteType="Disabled" runat="server" MaxLength="255"></asp:TextBox>
                                            <asp:RequiredFieldValidator runat="server" Display="None" CssClass="input-notification-error  error-simple png_bg" ValidationGroup="vd_save" ID="RequiredFieldValidator5" ControlToValidate="tbx_phone1" ErrorMessage="Phone Number cannot be blank." />
                                        </div>

                                        <div class="txtfld-col">
                                            <label for="teacher_phone2">Phone 2</label>
                                            <asp:TextBox ID="tbx_phone2" AutoCompleteType="Disabled" runat="server" MaxLength="255"></asp:TextBox>
                                        </div>

                                        <div class="txtfld-col">
                                            <label for="teacher_email" class="required">Email <span class="required">*</span></label>
                                            <asp:TextBox ID="tbx_emailid" AutoCompleteType="Disabled" runat="server" MaxLength="255"></asp:TextBox>
                                            <asp:RequiredFieldValidator runat="server" Display="None" CssClass="input-notification-error  error-simple png_bg" ValidationGroup="vd_save" ID="RequiredFieldValidator6" ControlToValidate="tbx_emailid" ErrorMessage="Email Id cannot be blank." />
                                        </div>


                                        <div class="clear"></div>
                                    </div>
                                </div>

                            
                                <!-- form -->



                                <div class="clear"></div>


                                <div class="button-bg">
                                    <div class="top-hed-btn-left">


                                        <asp:Button ID="btn_save" ValidationGroup="vd_save" runat="server" Text="Save" OnClick="btn_save_Click" Width="150px" CssClass="a_tag-btn" />

                                    </div>


                                </div>
                            </div>
                        </div>
                   


        <asp:ValidationSummary ID="ValidationSummary1" ShowSummary="true" DisplayMode="BulletList" ValidationGroup="vd_save" runat="server" ForeColor="Red" />

        <script src="plugins/jQuery/jquery-2.2.3.min.js"></script>

      


        <script type="text/javascript">
            $('#submit_button_form').click(function (ev) {
                var file_size = $('#Students_photo_data')[0].files[0].size;
                if (file_size > 1048576) { //File upload size limit to 1mb
                    $('#image_size_error').html('File size is greater than 1MB');
                    return false;
                }
            });
        </script>
  
</asp:Content>
