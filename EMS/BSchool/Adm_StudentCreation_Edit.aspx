<%@ Page Title="" Language="C#" MasterPageFile="~/Student_Settings.master" AutoEventWireup="true" CodeFile="Adm_StudentCreation_Edit.aspx.cs" Inherits="Adm_StudentCreation_Edit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
     <div class="cont_right formWrapper">

        <style type="text/css">
            .pagetab-bg .li_hide {
                display: none !important;
            }

            .student-postn {
                top: 16px;
                right: 19px;
            }
        </style>
        <div class="right-pg-hd">

            <h1>Enrolment</h1>
        </div>

        <div class="button-bg">
            <div class="top-hed-btn-left"></div>
            <div class="top-hed-btn-right">
                <ul>
                    <li></li>
                </ul>
            </div>
        </div>
        <div class="page-tab">
            <div class="pagetab-bg">
                <ul>
                   <li class="active">
                        <h2 class="hvr-syle"><a href="#" id="lnk_studentdetails" runat="server" onserverclick="lnk_studentdetails_ServerClick">Student Details</a></h2>
                    </li>
                    <li >
                        <h2 class="hvr-syle"><a href="#" id="lnk_guardiandetails" runat="server" onserverclick="lnk_guardiandetails_ServerClick">Guardian Details</a></h2>
                    </li>
                    <li class=" ">
                        <h2 class="hvr-syle"><a href="#" id="lnk_previousdetails" runat="server" onserverclick="lnk_previousdetails_ServerClick">Previous Details</a></h2>
                    </li>
                    <li class=" ">
                        <h2 class="hvr-syle"><a href="#" id="lnk_documentdetails" runat="server" onserverclick="lnk_documentdetails_ServerClick" >Student Documents</a></h2>
                    </li>
                </ul>
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


        <p class="note">Fields with<span class="required">*</span>are required.</p>
        <div class="formCon" style="background: url(images/yellow-pattern.png); width: 100%; border: 0px #fac94a solid; color: #000;">
            <div class="formConInner">
                <table style="width:100%" border="0" cellspacing="0" cellpadding="0">
                    <tbody>
                        <tr>
                            <td>
                                <label for="Students_admission_no" class="required">Admission No <span class="required">*</span></label></td>
                            <td>
                                <asp:TextBox ID="tbx_admissionno" Enabled="false" AutoCompleteType="Disabled" runat="server" MaxLength="255"></asp:TextBox>
                            </td>
                            <td width="50"></td>
                            <td>
                                <label for="Students_admission_date" class="required">Admission Date <span class="required">*</span></label></td>
                            <td>                                
                                <asp:TextBox ID="tbx_admissiondate" Enabled="false" AutoCompleteType="Disabled" runat="server" CssClass="hasDatepicker"></asp:TextBox>
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
        </div>

        <div class="formCon">
            <div class="formConInner opnsl_form_label">
                <h3>Personal Details </h3>

                <div class="txtfld-col">
                    <label for="Students_first_name" class="required">First Name <span class="required">*</span></label>
                    <asp:TextBox ID="tbx_firstname" AutoCompleteType="Disabled" runat="server" MaxLength="255"></asp:TextBox> 
                    <asp:RequiredFieldValidator runat="server" Display="None" CssClass="input-notification-error  error-simple png_bg" ValidationGroup="vd_save" ID="reqdocumentname" ControlToValidate="tbx_firstname" ErrorMessage="First Name cannot be blank." />                   
                </div>

                <div class="txtfld-col">
                    <label for="Students_middle_name">Middle Name</label>                    
                    <asp:TextBox ID="tbx_middlename" AutoCompleteType="Disabled" runat="server" MaxLength="255"></asp:TextBox>
                </div>

                <div class="txtfld-col">
                    <label for="Students_last_name" class="required">Last Name <span class="required">*</span></label>                    
                    <asp:TextBox ID="tbx_lastname" AutoCompleteType="Disabled" runat="server" MaxLength="255"></asp:TextBox>
                    <asp:RequiredFieldValidator runat="server" Display="None" CssClass="input-notification-error  error-simple png_bg" ValidationGroup="vd_save" ID="RequiredFieldValidator1" ControlToValidate="tbx_lastname" ErrorMessage="Last Name cannot be blank." />                   
                </div>

                   <div class="txtfld-col">
                    <label for="Students_student_category_id">Student Category</label>
                    <asp:DropDownList ID="drp_category" runat="server"></asp:DropDownList>                                                                                                                        
                </div>

                <div class="txtfld-col">
                    <label for="Students_batch_id">Batch</label>
                    <asp:DropDownList ID="drp_batchid" runat="server">

                    </asp:DropDownList>                    
                </div>

                <div class="txtfld-col">
                    <label for="Students_date_of_birth" class="required">Date Of Birth <span class="required">*</span></label>                    
                    <asp:TextBox ID="tbx_dob" AutoCompleteType="Disabled" runat="server" MaxLength="255" class="hasDatepicker"></asp:TextBox>
                    <asp:RequiredFieldValidator runat="server" Display="None" CssClass="input-notification-error  error-simple png_bg" ValidationGroup="vd_save" ID="RequiredFieldValidator2" ControlToValidate="tbx_dob" ErrorMessage="DOB cannot be blank." />                   
                </div>

                <div class="txtfld-col">
                    <label for="Students_gender" class="required">Gender <span class="required">*</span></label>
                    <asp:DropDownList ID="drp_gender" runat="server"></asp:DropDownList>   
                    <asp:RequiredFieldValidator runat="server" Display="None" CssClass="input-notification-error  error-simple png_bg" ValidationGroup="vd_save" ID="RequiredFieldValidator3" ControlToValidate="drp_gender" InitialValue="0" ErrorMessage="Gender cannot be blank." />                                                        
                </div>

                <div class="txtfld-col">
                    <label for="Students_blood_group">Blood </label>
                    <asp:DropDownList ID="drp_bloodgroup" runat="server"></asp:DropDownList>                                        
                </div>

                <div class="txtfld-col">
                    <label for="Students_birth_place">Birth Place</label>
                    <asp:TextBox ID="tbx_birthplace" AutoCompleteType="Disabled" runat="server" MaxLength="255"></asp:TextBox>                    
                </div>

                <div class="txtfld-col">
                    <label for="Students_nationality_id" class="required">Nationality <span class="required">*</span></label>
                    <asp:DropDownList ID="drp_nationality" runat="server"></asp:DropDownList> 
                    <asp:RequiredFieldValidator runat="server" Display="None" CssClass="input-notification-error  error-simple png_bg" ValidationGroup="vd_save" ID="RequiredFieldValidator4" ControlToValidate="drp_nationality" InitialValue="0" ErrorMessage="Nationality cannot be blank." />                                                                                                  
                </div>

                <div class="txtfld-col">
                    <label for="Students_language">Language</label>                                         
                     <asp:DropDownList ID="drp_lang" runat="server"></asp:DropDownList>                                                                                
                </div>

                <div class="txtfld-col">
                    <label for="Students_religion">Religion</label>   
                     <asp:DropDownList ID="drp_religion" runat="server"></asp:DropDownList> 
                </div>

           
                
                <div class="clear"></div>
            </div>
        </div>

        <div class="formCon">
            <div class="formConInner opnsl_form_label">
                <h3>Contact Details</h3>

                <div class="txtfld-col">
                    <label for="Students_address_line1">Address Line 1</label>                    
                     <asp:TextBox ID="tbx_add1" AutoCompleteType="Disabled" runat="server" MaxLength="255"></asp:TextBox> 
                </div>

                <div class="txtfld-col">
                    <label for="Students_address_line2">Address Line 2</label>
                    <asp:TextBox ID="tbx_add2" AutoCompleteType="Disabled" runat="server" MaxLength="255"></asp:TextBox> 
                </div>

                <div class="txtfld-col">
                    <label for="Students_city">City</label>
                    <asp:TextBox ID="tbx_city" AutoCompleteType="Disabled" runat="server" MaxLength="255"></asp:TextBox> 
                </div>

                <div class="txtfld-col">
                    <label for="Students_state">State</label>
                    <asp:TextBox ID="tbx_state" AutoCompleteType="Disabled" runat="server" MaxLength="255"></asp:TextBox> 
                </div>

                <div class="txtfld-col">
                    <label for="Students_pin_code">Pin Code</label>
                    <asp:TextBox ID="tbx_pincode" AutoCompleteType="Disabled" runat="server" MaxLength="255"></asp:TextBox> 
                </div>

                <div class="txtfld-col">
                    <label for="Students_country_id">Country</label>
                   <asp:DropDownList ID="drp_contry" runat="server"></asp:DropDownList>
                </div>

                <div class="txtfld-col">
                    <label for="Students_phone1" class="required">Phone Number <span class="required">*</span></label>                                        
                     <asp:TextBox ID="tbx_phone1" AutoCompleteType="Disabled" runat="server" MaxLength="255"></asp:TextBox>
                    <asp:RequiredFieldValidator runat="server" Display="None" CssClass="input-notification-error  error-simple png_bg" ValidationGroup="vd_save" ID="RequiredFieldValidator5" ControlToValidate="tbx_phone1" ErrorMessage="Phone Number cannot be blank." />                   
                </div>

                <div class="txtfld-col">
                    <label for="Students_phone2">Phone 2</label>
                    <asp:TextBox ID="tbx_phone2" AutoCompleteType="Disabled" runat="server" MaxLength="255"></asp:TextBox>
                </div>

                <div class="txtfld-col">
                    <label for="Students_email" class="required">Email <span class="required">*</span></label>
                    <asp:TextBox ID="tbx_emailid" AutoCompleteType="Disabled" runat="server" MaxLength="255"></asp:TextBox>
                    <asp:RequiredFieldValidator runat="server" Display="None"  CssClass="input-notification-error  error-simple png_bg" ValidationGroup="vd_save" ID="RequiredFieldValidator6" ControlToValidate="tbx_emailid" ErrorMessage="Email Id cannot be blank." />                   
                </div>

                
                <div class="clear"></div>
            </div>
        </div>

        <div class="formCon">
            <div class="formConInner" style="padding: 10px 10px;">
                 
                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                    <tbody>
                        <tr>
                            <td>
                                <h3>Upload Photo</h3>    
                                <p>                                
                                 <asp:Image ID="img_student" CssClass="imgbrder" Height="100px" Width="100px"  runat="server" />
                                </p>
                                
                               
                                                            
                                <asp:FileUpload ID="file_upload" runat="server" CssClass="form-control" />
                            </td>

                        </tr>
                        <tr>
                            <td>
                                <div id="image_size_error" style="color: #F00;"></div>
                                <div>Maximum file size is 1MB. Allowed file types are png,gif,jpeg,jpg</div>
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
        </div>
        <!-- form -->

        
        <div class="clear"></div>
        

        <div class="button-bg">
            <div class="top-hed-btn-left"></div>
            <div class="top-hed-btn-right">
                <ul>
                    <li>
                        <asp:Button ID="btn_save" validationgroup="vd_save" runat="server" Text="Save" OnClick="btn_save_Click" CssClass="a_tag-btn" />
                    </li>
                </ul>
            </div>

        </div>
        <asp:ValidationSummary ID="ValidationSummary1"  ShowSummary="true" DisplayMode="BulletList" ValidationGroup="vd_save" runat="server" ForeColor="Red"/>  

        <script src="plugins/jQuery/jquery-2.2.3.min.js"></script>

    <script type="text/javascript">
        $(function () {
            $('#' + '<%= tbx_dob.ClientID %>').datepicker({ autoclose: true });

        });
    </script>


        <script type="text/javascript">
            $('#submit_button_form').click(function (ev) {
                var file_size = $('#Students_photo_data')[0].files[0].size;
                if (file_size > 1048576) { //File upload size limit to 1mb
                    $('#image_size_error').html('File size is greater than 1MB');
                    return false;
                }
            });
        </script>
    </div>
</asp:Content>

