<%@ Page Title="" Language="C#" MasterPageFile="~/Student_Settings.master" AutoEventWireup="true" CodeFile="Adm_Online_Application_FormView.aspx.cs" Inherits="Adm_Online_Application_FormView" %>

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
             <center><div class="online_academic_yr" id="lbl_acyear" runat="server">Academic Year - AY 2018-2019</div></center>
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

       

        <div class="formCon" style="background: url(images/yellow-pattern.png); width: 100%; border: 0px #fac94a solid; color: #000;">
            <div class="formConInner">
                
                      

                <table style="width: 100%" border="0" cellspacing="0" cellpadding="0">
                    <tbody>
                        <tr>
                            <td>
                                <label for="Students_admission_no" class="required">Admission No <span class="required">*</span></label></td>
                            <td>
                                <asp:Label ID="tbx_admissionno" AutoCompleteType="Disabled" runat="server" MaxLength="255"></asp:Label>
                            </td>
                            <td width="50"></td>
                            <td>
                                <label for="Students_admission_date" class="required">Admission Date <span class="required">*</span></label></td>
                            <td>
                                <asp:Label ID="tbx_admissiondate" AutoCompleteType="Disabled" runat="server" CssClass="hasDatepicker"></asp:Label>
                                <asp:HiddenField ID="hd_academic_yr" runat="server" />
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
        </div>

        <div class="formCon">
             
                        <asp:HiddenField ID="hd_acyear" runat="server" />
                   

            <div class="formConInner opnsl_form_label">
                <h3>Personal Details </h3>

                <div class="table_listbx">
                    <table width="100%" border="0" cellspacing="0" cellpadding="0">
                        <tbody>
                            <tr>
                                <td width="20%">
                                    <b>Name</b>
                                </td>
                                <td width="3%"><strong>:</strong></td>
                                <td width="77%">
                                    <label id="tbx_name" runat="server"></label>
                                </td>
                            </tr>
                            <tr>
                                <td width="20%">
                                    <b>Father Name</b>
                                </td>
                                <td width="3%"><strong>:</strong></td>
                                <td width="77%">
                                    <label id="tbx_fathername" runat="server"></label>
                                </td>
                            </tr>
                            <tr>
                                <td width="20%">
                                    <b>Course</b>
                                </td>
                                <td width="3%"><strong>:</strong></td>
                                <td width="77%">
                                    <label id="tbx_course" runat="server"></label>
                                </td>
                            </tr>
                            <tr>
                                <td width="20%">
                                    <b>Gender</b>
                                </td>
                                <td width="3%"><strong>:</strong></td>
                                <td width="77%">
                                    <label id="tbx_gender" runat="server"></label>
                                </td>
                            </tr>
                            <tr>
                                <td width="20%">
                                    <b>Date Of Birth</b>
                                </td>
                                <td width="3%"><strong>:</strong></td>
                                <td width="77%">
                                    <label id="tbx_dob" runat="server"></label>
                                </td>
                            </tr>
                            <tr>
                                <td width="20%">
                                    <b>Nationality</b>
                                </td>
                                <td width="3%"><strong>:</strong></td>
                                <td width="77%">
                                    <label id="tbx_nationality" runat="server"></label>
                                </td>
                            </tr>
                            <tr>
                                <td width="20%">
                                    <b>Social Status</b>
                                </td>
                                <td width="3%"><strong>:</strong></td>
                                <td width="77%">
                                    <label id="tbx_Social_Status" runat="server"></label>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </div>
                <div class="clear"></div>
            </div>
        </div>

        <div class="formCon">
            <div class="formConInner opnsl_form_label">
                <h3>Contact Details</h3>

                <div class="table_listbx">
                    <table width="100%" border="0" cellspacing="0" cellpadding="0">
                        <tbody>
                            <tr>
                                <td width="20%">
                                    <b>Present Address</b>
                                </td>
                                <td width="3%"><strong>:</strong></td>
                                <td width="77%">
                                    <label id="tbx_add1" runat="server"></label>
                                </td>
                            </tr>
                            <tr>
                                <td width="20%">
                                    <b>Permanent Address</b>
                                </td>
                                <td width="3%"><strong>:</strong></td>
                                <td width="77%">
                                    <label id="tbx_add2" runat="server"></label>
                                </td>
                            </tr>
                            <tr>
                                <td width="20%">
                                    <b>Mobile Number</b>
                                </td>
                                <td width="3%"><strong>:</strong></td>
                                <td width="77%">
                                    <label id="tbx_phone1" runat="server"></label>
                                </td>
                            </tr>
                            <tr>
                                <td width="20%">
                                    <b>Email</b>
                                </td>
                                <td width="3%"><strong>:</strong></td>
                                <td width="77%">
                                    <label id="tbx_emailid" runat="server"></label>
                                </td>
                            </tr>
                        </tbody>
                    </table>

                </div>

                <div class="clear"></div>
            </div>
        </div>
        <div class="formCon">
            <div class="formConInner opnsl_form_label">
                <h3>Edutcation Details - S.S.L.C / P.U.C / H.Sc / Pre-Degree</h3>

                <div class="table_listbx">
                    <table width="100%" border="0" cellspacing="0" cellpadding="0">
                        <tbody>
                            <tr>
                                <td width="20%">
                                    <b>Course</b>
                                </td>
                                <td width="3%"><strong>:</strong></td>
                                <td width="77%">
                                    <label id="tbx_sslc_course" runat="server"></label>
                                </td>
                            </tr>
                            <tr>
                                <td width="20%">
                                    <b>Group</b>
                                </td>
                                <td width="3%"><strong>:</strong></td>
                                <td width="77%">
                                    <label id="tbx_sslc_group" runat="server"></label>
                                </td>
                            </tr>

                            <tr>
                                <td width="20%">
                                    <b>Institution</b>
                                </td>
                                <td width="3%"><strong>:</strong></td>
                                <td width="77%">
                                    <label id="tbx_sslc_institution" runat="server"></label>
                                </td>
                            </tr>
                            <tr>
                                <td width="20%">
                                    <b>Year of Passing</b>
                                </td>
                                <td width="3%"><strong>:</strong></td>
                                <td width="77%">
                                    <label id="tbx_sslc_yearpass" runat="server"></label>
                                </td>
                            </tr>

                            <tr>
                                <td width="20%">
                                    <b>% of Marks</b>
                                </td>
                                <td width="3%"><strong>:</strong></td>
                                <td width="77%">
                                    <label id="tbx_sslc_mark" runat="server"></label>
                                </td>
                            </tr>
                            <tr>
                                <td width="20%">
                                    <b>Grade/Class</b>
                                </td>
                                <td width="3%"><strong>:</strong></td>
                                <td width="77%">
                                    <label id="tbx_sslc_grade" runat="server"></label>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </div>





                <div class="clear"></div>
            </div>
        </div>

        <div class="formCon">
            <div class="formConInner opnsl_form_label">
                <h3>Edutcation Details - Diploma / UG / PG</h3>



                 <div class="table_listbx">
                    <table width="100%" border="0" cellspacing="0" cellpadding="0">
                        <tbody>
                            <tr>
                                <td width="20%">
                                    <b>Course</b>
                                </td>
                                <td width="3%"><strong>:</strong></td>
                                <td width="77%">
                                    <label id="tbx_pg_course" runat="server"></label>
                                </td>
                            </tr>
                            <tr>
                                <td width="20%">
                                    <b>Group</b>
                                </td>
                                <td width="3%"><strong>:</strong></td>
                                <td width="77%">
                                    <label id="tbx_pg_group" runat="server"></label>
                                </td>
                            </tr>

                            <tr>
                                <td width="20%">
                                    <b>Institution</b>
                                </td>
                                <td width="3%"><strong>:</strong></td>
                                <td width="77%">
                                    <label id="tbx_pg_institution" runat="server"></label>
                                </td>
                            </tr>
                            <tr>
                                <td width="20%">
                                    <b>Year of Passing</b>
                                </td>
                                <td width="3%"><strong>:</strong></td>
                                <td width="77%">
                                    <label id="tbx_pg_yearpass" runat="server"></label>
                                </td>
                            </tr>

                            <tr>
                                <td width="20%">
                                    <b>% of Marks</b>
                                </td>
                                <td width="3%"><strong>:</strong></td>
                                <td width="77%">
                                    <label id="tbx_pg_mark" runat="server"></label>
                                </td>
                            </tr>
                            <tr>
                                <td width="20%">
                                    <b>Grade/Class</b>
                                </td>
                                <td width="3%"><strong>:</strong></td>
                                <td width="77%">
                                    <label id="tbx_pg_grade" runat="server"></label>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </div>



             

                <div class="clear"></div>
            </div>
        </div>

        <div class="formCon">
            <div class="formConInner opnsl_form_label">
                <h3>Preferred Language under Part-1(for UG Program Only)</h3>


                 <div class="table_listbx">
                    <table width="100%" border="0" cellspacing="0" cellpadding="0">
                        <tbody>
                            <tr>
                                <td width="20%">
                                    <b>1<sup>st</sup> Year</b>
                                </td>
                                <td width="3%"><strong>:</strong></td>
                                <td width="77%">
                                    <label id="tbx_firstyear" runat="server"></label>
                                </td>
                            </tr>
                            <tr>
                                <td width="20%">
                                    <b>2<sup>nd</sup> Year</b>
                                </td>
                                <td width="3%"><strong>:</strong></td>
                                <td width="77%">
                                    <label id="tbx_secondyear" runat="server"></label>
                                </td>
                            </tr>

                            <tr>
                                <td width="20%">
                                    <b>3<sup>rd</sup> Year</b>
                                </td>
                                <td width="3%"><strong>:</strong></td>
                                <td width="77%">
                                    <label id="tbx_thirdyear" runat="server"></label>
                                </td>
                            </tr>
                           
                        </tbody>
                    </table>
                </div>

            


              


                <div class="clear"></div>
            </div>
        </div>

        <div class="formCon">
            <div class="formConInner opnsl_form_label">
                <h3>Concurrent Admission Course Currently pursuing</h3>

                 <div class="table_listbx">
                    <table width="100%" border="0" cellspacing="0" cellpadding="0">
                        <tbody>
                            <tr>
                                <td width="20%">
                                    <b>Year of Study</b>
                                </td>
                                <td width="3%"><strong>:</strong></td>
                                <td width="77%">
                                    <label id="tbx_yearofstudy" runat="server"></label>
                                </td>
                            </tr>
                            <tr>
                                <td width="20%">
                                    <b>College</b>
                                </td>
                                <td width="3%"><strong>:</strong></td>
                                <td width="77%">
                                    <label id="tbx_college" runat="server"></label>
                                </td>
                            </tr>

                            <tr>
                                <td width="20%">
                                    <b>University</b>
                                </td>
                                <td width="3%"><strong>:</strong></td>
                                <td width="77%">
                                    <label id="tbx_university" runat="server"></label>
                                </td>
                            </tr>
                           
                        </tbody>
                    </table>
                </div>


                



                <div class="clear"></div>
            </div>
        </div>


        <div class="clear"></div>




    </div>
</asp:Content>

