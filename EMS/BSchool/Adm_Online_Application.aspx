<%@ Page Title="" Language="C#" MasterPageFile="~/Student_Settings.master" AutoEventWireup="true" CodeFile="Adm_Online_Application.aspx.cs" Inherits="Adm_Online_Application" %>

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
            <center>
            <h1><b>ADMISSION FORM</b></h1>
                </center>
            <hr />
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
                                <label for="Students_admission_no" class="required">Admission No <span class="required">*</span></label></td>
                            <td>
                                <asp:TextBox ID="tbx_admissionno" AutoCompleteType="Disabled" runat="server" MaxLength="255"></asp:TextBox>
                            </td>
                            <td width="50"></td>
                            <td>
                                <label for="Students_admission_date" class="required">Admission Date <span class="required">*</span></label></td>
                            <td>
                                <asp:TextBox ID="tbx_admissiondate" AutoCompleteType="Disabled" runat="server" CssClass="hasDatepicker"></asp:TextBox>
                                <asp:HiddenField ID="hd_academic_yr" runat="server" />
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
                    <label for="Students_name" class="required">Name <span class="required">*</span></label>
                    <asp:TextBox ID="tbx_name" AutoCompleteType="Disabled" runat="server" MaxLength="200"></asp:TextBox>
                    <asp:RequiredFieldValidator runat="server" Display="None" CssClass="input-notification-error  error-simple png_bg" ValidationGroup="vd_save" ID="reqdocumentname" ControlToValidate="tbx_name" ErrorMessage="Name cannot be blank." />
                </div>

                <div class="txtfld-col">
                    <label for="Students_fathername">Father Name<span class="required">*</span></label>
                    <asp:TextBox ID="tbx_fathername" AutoCompleteType="Disabled" runat="server" MaxLength="200"></asp:TextBox>
                    <asp:RequiredFieldValidator runat="server" Display="None" CssClass="input-notification-error  error-simple png_bg" ValidationGroup="vd_save" ID="RequiredFieldValidator7" ControlToValidate="tbx_fathername" ErrorMessage="Father Name cannot be blank." />
                </div>

                <div class="txtfld-col">
                    <label for="Students_courseid">Course<span class="required">*</span></label>
                    <asp:DropDownList ID="drp_courseid" runat="server">
                        <asp:ListItem Text="Course Applied for..." Value="0"></asp:ListItem>
                        <asp:ListItem Text="CourseWork Integrated MBA" Value="Work Integrated MBA"></asp:ListItem>
                        <asp:ListItem Text="International MBA at UK" Value="International MBA at UK"></asp:ListItem>
                        <asp:ListItem Text="MBA - Airline & Airport Management" Value="MBA - Airline& Airport Management"></asp:ListItem>
                        <asp:ListItem Text="MBA - shipping and logistics" Value="MBA - shipping and logistics"></asp:ListItem>
                        <asp:ListItem Text="MBA + GSM" Value="MBA + GSM"></asp:ListItem>
                        <asp:ListItem Text="BBA Aviation" Value="BBA Aviation"></asp:ListItem>
                        <asp:ListItem Text="BBA shipping and logistics" Value="BBA shipping and logistics"></asp:ListItem>
                        <asp:ListItem Text="Integrated MBA(BBA+MBA)-5 Years" Value="Integrated MBA(BBA+MBA)-5 Years"></asp:ListItem>
                        <asp:ListItem Text="B.Com + CMA (US)" Value="B.Com + CMA (US)"></asp:ListItem>
                        <asp:ListItem Text="Post Graduate Program" Value="Post Graduate Program"></asp:ListItem>
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator runat="server" Display="None" CssClass="input-notification-error  error-simple png_bg" ValidationGroup="vd_save" ID="RequiredFieldValidator1" ControlToValidate="drp_courseid" InitialValue="0" ErrorMessage="Course cannot be blank." />
                </div>
                    <div class="txtfld-col">
                    <label for="Students_gender" class="required">Gender <span class="required">*</span></label>
                    <asp:DropDownList ID="drp_gender" runat="server"></asp:DropDownList>
                    <asp:RequiredFieldValidator runat="server" Display="None" CssClass="input-notification-error  error-simple png_bg" ValidationGroup="vd_save" ID="RequiredFieldValidator3" ControlToValidate="drp_gender" InitialValue="0" ErrorMessage="Gender cannot be blank." />
                </div>

                <div class="txtfld-col">
                    <label for="Students_date_of_birth" class="required">Date Of Birth <span class="required">*</span></label>
                    <asp:TextBox ID="tbx_dob" AutoCompleteType="Disabled" runat="server" MaxLength="255" class="hasDatepicker"></asp:TextBox>
                    <asp:RequiredFieldValidator runat="server" Display="None" CssClass="input-notification-error  error-simple png_bg" ValidationGroup="vd_save" ID="RequiredFieldValidator2" ControlToValidate="tbx_dob" ErrorMessage="DOB cannot be blank." />
                </div>

            <div class="txtfld-col">
                    <label for="Students_nationality_id" class="required">Nationality <span class="required">*</span></label>
                    <asp:DropDownList ID="drp_nationality" runat="server"></asp:DropDownList>
                    <asp:RequiredFieldValidator runat="server" Display="None" CssClass="input-notification-error  error-simple png_bg" ValidationGroup="vd_save" ID="RequiredFieldValidator4" ControlToValidate="drp_nationality" InitialValue="0" ErrorMessage="Nationality cannot be blank." />
                </div>

                <div class="txtfld-col" style="width:100%">
                    <label for="Students_blood_group">Social Status<span class="required">*</span></label>
                    &nbsp;&nbsp;&nbsp;&nbsp;<asp:RadioButton ID="rd_general" GroupName="community" runat="server" Text="General" />
                    &nbsp;&nbsp;&nbsp;&nbsp;<asp:RadioButton ID="rd_bc" GroupName="community" runat="server" Text="BC" />
                    &nbsp;&nbsp;&nbsp;&nbsp;<asp:RadioButton ID="rd_obc" GroupName="community" runat="server" Text="OBC" />
                    &nbsp;&nbsp;&nbsp;&nbsp;<asp:RadioButton ID="rd_others" GroupName="community" runat="server" Text="Others" />

                </div>
                <div class="clear"></div>
            </div>
        </div>

        <div class="formCon">
            <div class="formConInner opnsl_form_label">
                <h3>Contact Details</h3>

                <div class="txtfld-col">
                    <label for="Students_presentaddress">Present Address<span class="required">*</span></label>
                    <asp:TextBox ID="tbx_add1" AutoCompleteType="Disabled" Rows="3" Columns="5" runat="server" MaxLength="255"></asp:TextBox>
                </div>

                <div class="txtfld-col">
                    <label for="Students_permanentaddress">Permanent Address<span class="required">*</span></label>
                    <asp:TextBox ID="tbx_add2" AutoCompleteType="Disabled" Rows="3" Columns="5" runat="server" MaxLength="255"></asp:TextBox>
                </div>

               

                <div class="txtfld-col">
                    <label for="Students_phone1" class="required">Mobile Number <span class="required">*</span></label>
                    <asp:TextBox ID="tbx_phone1" AutoCompleteType="Disabled" runat="server" MaxLength="255"></asp:TextBox>
                    <asp:RequiredFieldValidator runat="server" Display="None" CssClass="input-notification-error  error-simple png_bg" ValidationGroup="vd_save" ID="RequiredFieldValidator5" ControlToValidate="tbx_phone1" ErrorMessage="Mobile Number cannot be blank." />
                </div>

                

                <div class="txtfld-col">
                    <label for="Students_email" class="required">Email <span class="required">*</span></label>
                    <asp:TextBox ID="tbx_emailid" AutoCompleteType="Disabled" runat="server" MaxLength="255"></asp:TextBox>
                    <asp:RequiredFieldValidator runat="server" Display="None" CssClass="input-notification-error  error-simple png_bg" ValidationGroup="vd_save" ID="RequiredFieldValidator6" ControlToValidate="tbx_emailid" ErrorMessage="Email Id cannot be blank." />
                </div>


                <div class="clear"></div>
            </div>
        </div>

         <div class="formCon">
            <div class="formConInner opnsl_form_label">
                <h3>Edutcation Details - S.S.L.C / P.U.C / H.Sc / Pre-Degree</h3>
                <div class="txtfld-col">
                    <label for="Students_sslc_course" class="required">Course</label>
                    <asp:TextBox ID="tbx_sslc_course" AutoCompleteType="Disabled" runat="server" MaxLength="255"></asp:TextBox>
                    
                </div>

                

                <div class="txtfld-col">
                    <label for="Students_sslc_group" class="required">Group</label>
                    <asp:TextBox ID="tbx_sslc_group" AutoCompleteType="Disabled" runat="server" MaxLength="255"></asp:TextBox>
                    
                </div>

                  <div class="txtfld-col">
                    <label for="Students_sslc_institution" class="required">Institution</label>
                    <asp:TextBox ID="tbx_sslc_institution" AutoCompleteType="Disabled" runat="server" MaxLength="255"></asp:TextBox>
                    
                </div>

                  <div class="txtfld-col">
                    <label for="Students_sslc_yearpass" class="required">Year of Passing</label>
                    <asp:TextBox ID="tbx_sslc_yearpass" AutoCompleteType="Disabled" runat="server" MaxLength="255"></asp:TextBox>
                    
                </div>

                 <div class="txtfld-col">
                    <label for="Students_sslc_mark" class="required">% of Marks</label>
                    <asp:TextBox ID="tbx_sslc_mark" AutoCompleteType="Disabled" runat="server" MaxLength="255"></asp:TextBox>
                    
                </div>

                <div class="txtfld-col">
                    <label for="Students_sslc_grade" class="required">Grade/Class</label>
                    <asp:TextBox ID="tbx_sslc_grade" AutoCompleteType="Disabled" runat="server" MaxLength="255"></asp:TextBox>
                    
                </div>

                <div class="clear"></div>
            </div>
        </div>

         <div class="formCon">
            <div class="formConInner opnsl_form_label">
                <h3>Edutcation Details - Diploma / UG / PG</h3>


                

               
               

                <div class="txtfld-col">
                    <label for="Students_pg_course" class="required">Course</label>
                    <asp:TextBox ID="tbx_pg_course" AutoCompleteType="Disabled" runat="server" MaxLength="255"></asp:TextBox>
                    
                </div>

                

                <div class="txtfld-col">
                    <label for="Students_pg_group" class="required">Group </label>
                    <asp:TextBox ID="tbx_pg_group" AutoCompleteType="Disabled" runat="server" MaxLength="255"></asp:TextBox>
                    
                </div>

                  <div class="txtfld-col">
                    <label for="Students_pg_institution" class="required">Institution</label>
                    <asp:TextBox ID="tbx_pg_institution" AutoCompleteType="Disabled" runat="server" MaxLength="255"></asp:TextBox>
                    
                </div>

                  <div class="txtfld-col">
                    <label for="Students_pg_yearpass" class="required">Year of Passing </label>
                    <asp:TextBox ID="tbx_pg_yearpass" AutoCompleteType="Disabled" runat="server" MaxLength="255"></asp:TextBox>
                    
                </div>

                 <div class="txtfld-col">
                    <label for="Students_pg_mark" class="required">% of Marks</label>
                    <asp:TextBox ID="tbx_pg_mark" AutoCompleteType="Disabled" runat="server" MaxLength="255"></asp:TextBox>
                    
                </div>

                <div class="txtfld-col">
                    <label for="Students_pg_grade" class="required">Grade/Class</label>
                    <asp:TextBox ID="tbx_pg_grade" AutoCompleteType="Disabled" runat="server" MaxLength="255"></asp:TextBox>
                    
                </div>

                <div class="clear"></div>
            </div>
        </div>

         <div class="formCon">
            <div class="formConInner opnsl_form_label">
                <h3>Preferred Language under Part-1(for UG Program Only)</h3>              

               
               

                <div class="txtfld-col">
                    <label for="Students_1year" class="required">1<sup>st</sup> Year</label>
                    <asp:TextBox ID="tbx_firstyear" AutoCompleteType="Disabled" runat="server" MaxLength="255"></asp:TextBox>
                    
                </div>

                

                <div class="txtfld-col">
                    <label for="Students_2year" class="required">2<sup>nd</sup> Year </label>
                    <asp:TextBox ID="tbx_secondyear" AutoCompleteType="Disabled" runat="server" MaxLength="255"></asp:TextBox>
                    
                </div>

                  <div class="txtfld-col">
                    <label for="Students_3year" class="required">3<sup>rd</sup> Year</label>
                    <asp:TextBox ID="tbx_thirdyear" AutoCompleteType="Disabled" runat="server" MaxLength="255"></asp:TextBox>
                    
                </div>

            
                <div class="clear"></div>
            </div>
        </div>

        <div class="formCon">
            <div class="formConInner opnsl_form_label">
                <h3>Concurrent Admission Course Currently pursuing</h3>              

               
               

                <div class="txtfld-col">
                    <label for="Students_yearofstudy" class="required">Year of Study</label>
                    <asp:TextBox ID="tbx_yearofstudy" AutoCompleteType="Disabled" runat="server" MaxLength="255"></asp:TextBox>
                    
                </div>

                

                <div class="txtfld-col">
                    <label for="Students_college" class="required">College </label>
                    <asp:TextBox ID="tbx_college" AutoCompleteType="Disabled" runat="server" MaxLength="255"></asp:TextBox>
                    
                </div>

                  <div class="txtfld-col">
                    <label for="Students_university" class="required">University</label>
                    <asp:TextBox ID="tbx_university" AutoCompleteType="Disabled" runat="server" MaxLength="255"></asp:TextBox>
                    
                </div>

                

                <div class="clear"></div>
            </div>
        </div>


        <div class="clear"></div>


        <div class="button-bg">
            <div class="top-hed-btn-left"></div>
            <div class="top-hed-btn-right">
                <ul>
                    <li>
                        <asp:Button ID="btn_guradian" ValidationGroup="vd_save" runat="server" Text="Submit" OnClick="btn_guradian_Click" CssClass="a_tag-btn" />
                    </li>
                </ul>
            </div>

        </div>
        <asp:ValidationSummary ID="ValidationSummary1" ShowSummary="true" DisplayMode="BulletList" ValidationGroup="vd_save" runat="server" ForeColor="Red" />

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

