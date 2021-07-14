<%@ Page Title="" Language="C#" MasterPageFile="~/Adm_Teacher_Settings.master" AutoEventWireup="true" CodeFile="Adm_Teacher.aspx.cs" Inherits="Adm_Teacher" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
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

            <h1>TEACHER PROFILE</h1>
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
                    <li class="active ">
                        <h2 class="no_hvr_style">Teacher Details</h2>
                    </li>
                    <li class=" ">
                        <h2 class="no_hvr_style">Teacher Contact Details</h2>
                    </li>
                    <li class=" ">
                        <h2 class="no_hvr_style">Teacher Documents</h2>
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
                <table style="width: 100%" border="0" cellspacing="0" cellpadding="0">
                    <tbody>
                        <tr>
                            <td>
                                <label for="teacher_no" class="required">Teacher No <span class="required">*</span></label></td>
                            <td>
                                <asp:TextBox ID="tbx_teacherno"  AutoCompleteType="Disabled" runat="server" MaxLength="255"></asp:TextBox>
                            </td>
                            <td width="50"></td>
                            <td>
                                <label for="teacher_join_date" class="required">Joining Date <span class="required">*</span></label></td>
                            <td>
                                <asp:TextBox ID="tbx_joindate" AutoCompleteType="Disabled" runat="server" CssClass="hasDatepicker"></asp:TextBox>
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
                    <label for="teacher_first_name" class="required">First Name <span class="required">*</span></label>
                    <asp:TextBox ID="tbx_firstname" AutoCompleteType="Disabled" runat="server" MaxLength="255"></asp:TextBox>
                    <asp:RequiredFieldValidator runat="server" Display="None" CssClass="input-notification-error  error-simple png_bg" ValidationGroup="vd_save" ID="reqdocumentname" ControlToValidate="tbx_firstname" ErrorMessage="First Name cannot be blank." />
                </div>

                <div class="txtfld-col">
                    <label for="teacher_middle_name">Middle Name</label>
                    <asp:TextBox ID="tbx_middlename" AutoCompleteType="Disabled" runat="server" MaxLength="255"></asp:TextBox>
                </div>

                <div class="txtfld-col">
                    <label for="teacher_last_name" class="required">Last Name <span class="required">*</span></label>
                    <asp:TextBox ID="tbx_lastname" AutoCompleteType="Disabled" runat="server" MaxLength="255"></asp:TextBox>
                    <asp:RequiredFieldValidator runat="server" Display="None" CssClass="input-notification-error  error-simple png_bg" ValidationGroup="vd_save" ID="RequiredFieldValidator1" ControlToValidate="tbx_lastname" ErrorMessage="Last Name cannot be blank." />
                </div>

                <div class="txtfld-col">
                    <label for="teacher_date_of_birth" class="required">Date Of Birth <span class="required">*</span></label>
                    <asp:TextBox ID="tbx_dob" AutoCompleteType="Disabled" runat="server" MaxLength="255" class="hasDatepicker"></asp:TextBox>
                    <asp:RequiredFieldValidator runat="server" Display="None" CssClass="input-notification-error  error-simple png_bg" ValidationGroup="vd_save" ID="RequiredFieldValidator2" ControlToValidate="tbx_dob" ErrorMessage="DOB cannot be blank." />
                </div>

                <div class="txtfld-col">
                    <label for="teacher_gender" class="required">Gender <span class="required">*</span></label>
                    <asp:DropDownList ID="drp_gender" runat="server"></asp:DropDownList>
                    <asp:RequiredFieldValidator runat="server" Display="None" CssClass="input-notification-error  error-simple png_bg" ValidationGroup="vd_save" ID="RequiredFieldValidator3" ControlToValidate="drp_gender" InitialValue="0" ErrorMessage="Gender cannot be blank." />
                </div>

                  <div class="txtfld-col">
                    <label for="teacher_blood_group">Blood </label>
                    <asp:DropDownList ID="drp_bloodgroup" runat="server"></asp:DropDownList>                                        
                </div>

                <div class="txtfld-col">
                    <label for="teacher_batch_id">Teacher Department</label>
                    <asp:DropDownList ID="drp_teacherdept" runat="server">
                    </asp:DropDownList>
                </div>

                <div class="txtfld-col">
                    <label for="teacher_category_id">Teacher Category</label>
                    <asp:DropDownList ID="drp_category" runat="server"></asp:DropDownList>
                </div>




                <div class="txtfld-col">
                    <label for="teacher_position">Teacher Position</label>
                    <asp:DropDownList ID="drp_position" runat="server"></asp:DropDownList>
                </div>

                <div class="txtfld-col">
                    <label for="teacher_grade">Teacher Grade</label>
                    <asp:DropDownList ID="drp_grade" runat="server"></asp:DropDownList>
                </div>

                <div class="txtfld-col">
                    <label for="teacher_jobtitle">Job Title</label>
                    <asp:TextBox ID="tbx_jobtitle" AutoCompleteType="Disabled" runat="server" MaxLength="255"></asp:TextBox>
                </div>

                <div class="txtfld-col">
                    <label for="teacher_qualification">Qualification</label>
                    <asp:TextBox ID="tbx_qualification"  AutoCompleteType="Disabled" runat="server" MaxLength="255"></asp:TextBox>
                </div>
                <div class="clear"></div>
                
                <div class="txtfld-col">                    
                    <label for="teacher_noofexperience" class="required">Total Experience<span class="required">*</span></label>
                    <asp:DropDownList ID="drp_experienceyear" runat="server"></asp:DropDownList>                         
                    
                    
                </div>
                <div class="txtfld-col">                    
                <label for="teacher_noofexperiencemonth" class="required">Month<span class="required">*</span></label>
                    <asp:DropDownList ID="drp_experiencemonth" runat="server"></asp:DropDownList>                    
                    </div>
                    

                <div class="txtfld-col" style="width:100%">
                    
                    <label for="teacher_experiencedetails" class="required">Experience Details<span class="required">*</span></label>
                    <asp:TextBox ID="tbx_experiencedetails" AutoCompleteType="Disabled" TextMode="MultiLine" runat="server" Rows="12" Width="100%" Height="100px" Columns="48"></asp:TextBox>
                    
                </div>



             



                <div class="clear"></div>
            </div>
        </div>


         <div class="formCon">
            <div class="formConInner opnsl_form_label">
                <h3>Personal Details</h3>

                <div class="txtfld-col">
                    <label for="teacher_maritalsts">Marital Status</label>
                    <asp:DropDownList ID="drp_maritalsts" runat="server">

                    </asp:DropDownList>
                </div>

                <div class="txtfld-col">
                    <label for="teacher_chilcnt">Children Count</label>
                    <asp:TextBox ID="tbx_childcnt" AutoCompleteType="Disabled"  runat="server" MaxLength="255"></asp:TextBox>
                </div>

                <div class="txtfld-col">
                    <label for="teacher_fathername">Father Name</label>
                    <asp:TextBox ID="tbx_fathername" AutoCompleteType="Disabled" runat="server" MaxLength="255"></asp:TextBox>
                </div>

                <div class="txtfld-col">
                    <label for="teacher_mothername">Mother Name</label>
                    <asp:TextBox ID="tbx_mothername" AutoCompleteType="Disabled" runat="server" MaxLength="255"></asp:TextBox>
                </div>
                <div class="txtfld-col">
                    <label for="teacher_spousename">Spouse Name</label>
                    <asp:TextBox ID="tbx_spousename" AutoCompleteType="Disabled" runat="server" MaxLength="255"></asp:TextBox>
                </div>

              

                   <div class="txtfld-col">
                    <label for="teacher_birth_place">Birth Place</label>
                    <asp:TextBox ID="tbx_birthplace" AutoCompleteType="Disabled"  runat="server" MaxLength="255"></asp:TextBox>
                </div>

                <div class="txtfld-col">
                    <label for="teacher_nationality_id" class="required">Nationality <span class="required">*</span></label>
                    <asp:DropDownList ID="drp_nationality" runat="server"></asp:DropDownList>
                    <asp:RequiredFieldValidator runat="server" Display="None" CssClass="input-notification-error  error-simple png_bg" ValidationGroup="vd_save" ID="RequiredFieldValidator4" ControlToValidate="drp_nationality" InitialValue="0" ErrorMessage="Nationality cannot be blank." />
                </div>

                <div class="txtfld-col">
                    <label for="teacher_language">Language</label>
                    <asp:DropDownList ID="drp_lang" runat="server"></asp:DropDownList>
                </div>

                <div class="txtfld-col">
                    <label for="teacher_religion">Religion</label>
                    <asp:DropDownList ID="drp_religion" runat="server"></asp:DropDownList>
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
            <div class="top-hed-btn-left">
            
                
                        <asp:Button ID="btn_save" ValidationGroup="vd_save" runat="server" Text="Save" OnClick="btn_save_Click" Width="150px" CssClass="a_tag-btn" />
                
                </div>
            

        </div>
        <asp:ValidationSummary ID="ValidationSummary1" ShowSummary="true" DisplayMode="BulletList" ValidationGroup="vd_save" runat="server" ForeColor="Red" />

        <script src="plugins/jQuery/jquery-2.2.3.min.js"></script>

        <script type="text/javascript">
            $(function () {
                $('#' + '<%= tbx_dob.ClientID %>').datepicker({ autoclose: true });
                $('#' + '<%= tbx_joindate.ClientID %>').datepicker({ autoclose: true });
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

