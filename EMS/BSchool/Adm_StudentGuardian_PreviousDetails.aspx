<%@ Page Title="" Language="C#" MasterPageFile="~/Student_Settings.master" AutoEventWireup="true" CodeFile="Adm_StudentGuardian_PreviousDetails.aspx.cs" Inherits="Adm_StudentGuardian_PreviousDetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script src="plugins/jQuery/jquery-2.2.3.min.js"></script>
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
                    <li><a class="a_tag-btn" href="#">View Profile</a></li>
                </ul>
            </div>
        </div>
        <div class="page-tab">
            <div class="pagetab-bg">
                <ul>
                    <li>
                        <h2 class="hvr-syle"><a href="#" id="lnk_studentdetails" runat="server" onserverclick="lnk_studentdetails_ServerClick">Student Details</a></h2>
                    </li>
                    <li>
                        <h2 class="hvr-syle"><a href="#" id="lnk_guardiandetails" runat="server" onserverclick="lnk_guardiandetails_ServerClick">Guardian Details</a></h2>
                    </li>
                    <li class="active">
                        <h2 class="hvr-syle"><a href="#" id="lnk_previousdetails" runat="server" onserverclick="lnk_previousdetails_ServerClick">Previous Details</a></h2>
                    </li>
                    <li class=" ">
                        <h2 class="hvr-syle"><a href="#" id="lnk_documentdetails" runat="server" onserverclick="lnk_documentdetails_ServerClick">Student Documents</a></h2>
                    </li>
                </ul>
            </div>
        </div>


        <style>
            .accordion-item-active .accordion-header h1 {
                float: none;
            }

            .acordn-bg .accordion-header h1 {
                float: none;
            }
        </style>


        <div class="acordn-bg">
            <div class="acrdn-tab-bg">
                <div class="accordion">

                    <div class="drawer">
                        <div class="accordion-item accordion-item-active">
                            <div class="accordion-header accordion-header-active">
                                <h1>Previous Detail</h1>
                                <div class="box-btn">
                                    <div class="tt-wrapper-new">                                        
                                    </div>
                                </div>                                
                            </div>
                            <div class="pdtab_Con" style="display: block;padding-top:0px !important">

                                <asp:ListView ID="lv_previousedetails" runat="server" GroupPlaceholderID="groupPlaceHolder1"
                                    ItemPlaceholderID="itemPlaceHolder1">
                                    <LayoutTemplate>
                                        <table style="width:100%" cellpadding="0" cellspacing="0">
                                            <thead style="background-color:#E8ECF1">
                                                <td align="left" width="300">Institution                       
                                                </td>
                                                <td align="center" width="150">Year</td>
                                                <td align="center" width="150">Course</td>
                                                <td align="center" width="150">Total Mark</td>
                                                <td align="center" width="150"></td>
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

                                        <td align="left" width="300"><%# Eval("institution") %></td>
                                        <td align="center" width="150"><%# Eval("year") %></td>
                                        <td align="center" width="150"><%# Eval("course") %></td>
                                        <td align="center" width="150"><%# Eval("total_mark") %></td>
                                        <td>
                                            <asp:LinkButton ID="lnk_del" OnClick='DeletePopup("<%# Eval("id") %>")' Style='<%# Convert.ToBoolean(Session["Access_Trash"])==true ? "visibility:visible": "visibility:hidden" %>'>
                                                        <img src="gridview/delete.png" alt="Delete">                                    
                                            </asp:LinkButton>

                                        </td>
                                    </ItemTemplate>
                                    <EmptyDataTemplate>
                                        <table width="100%" cellpadding="0" cellspacing="0">
                                            <thead style="border: 1px solid #b9c7d0!important;background-color:#E8ECF1">
                                                <td align="left" width="300">Institution                       
                                                </td>
                                                <td align="center" width="150">Year</td>
                                                <td align="center" width="150">Course</td>
                                                <td align="center" width="150">Total Mark</td>
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



                </div>
            </div>
        </div>
       

        <style type="text/css">
            .formCon input[type="text"], input[type="password"], textArea, select {
                background: none repeat scroll 0 0 #FFFFFF;
                border: 1px solid #C2CFD8;
                border-radius: 2px;
                box-shadow: -1px 1px 2px #D5DBE0 inset;
                padding: 6px 3px;
                width: 100%!important;
            }

            .select-style select {
                width: 135% !important;
            }

            .formCon select {
                background: none repeat scroll 0 0 #FFFFFF;
                border: 1px solid #C2CFD8;
                border-radius: 2px;
                box-shadow: -1px 1px 2px #D5DBE0 inset;
                padding: 6px 3px;
                width: 93% !important;
            }

            .formCon input[type="text"] {
                background: none repeat scroll 0 0 #FFFFFF;
                border: 1px solid #C2CFD8;
                border-radius: 2px;
                box-shadow: -1px 1px 2px #D5DBE0 inset;
                padding: 6px 3px;
                width: 175px !important;
            }

            .formbut {
                padding: 10px 15px;
            }
        </style>



        <div class="formCon">
            <div class="formConInner opnsl_form_label">
                <h3>Institution Details </h3>
                <div class="txtfld-col">
                    <label for="StudentPreviousDatas_institution" class="required">Institution <span class="required">*</span></label>
                    <asp:TextBox ID="tbx_StudentPreviousDatas_institution" runat="server" MaxLength="255"></asp:TextBox>
                    <asp:RequiredFieldValidator runat="server" CssClass="input-notification-error  error-simple png_bg" ValidationGroup="vd_editsave" ID="RequiredFieldValidator3" ControlToValidate="tbx_StudentPreviousDatas_institution" ErrorMessage="Institution cannot be blank." />
                </div>

                <div class="txtfld-col">
                    <label for="StudentPreviousDatas_year">Year</label>
                    <asp:DropDownList ID="drp_StudentPreviousDatas_year" runat="server"></asp:DropDownList>
                    <asp:RequiredFieldValidator runat="server" CssClass="input-notification-error  error-simple png_bg" ValidationGroup="vd_editsave" ID="RequiredFieldValidator1" ControlToValidate="drp_StudentPreviousDatas_year" InitialValue="0" ErrorMessage="Year cannot be blank." />
                </div>

                <div class="txtfld-col">
                    <label for="StudentPreviousDatas_course">Course</label>
                    <asp:TextBox ID="tbx_StudentPreviousDatas_course" runat="server" MaxLength="255"></asp:TextBox>
                    <asp:RequiredFieldValidator runat="server" CssClass="input-notification-error  error-simple png_bg" ValidationGroup="vd_editsave" ID="RequiredFieldValidator2" ControlToValidate="tbx_StudentPreviousDatas_course" ErrorMessage="Course cannot be blank." />
                </div>

                <div class="txtfld-col">
                    <label for="StudentPreviousDatas_total_mark">Total Mark</label>
                    <asp:TextBox ID="tbx_StudentPreviousDatas_total_mark" runat="server" MaxLength="255"></asp:TextBox>
                    <asp:RequiredFieldValidator runat="server" CssClass="input-notification-error  error-simple png_bg" ValidationGroup="vd_editsave" ID="RequiredFieldValidator4" ControlToValidate="tbx_StudentPreviousDatas_total_mark" ErrorMessage="Mark cannot be blank." />
                </div>


                <div class="clear"></div>

            </div>
        </div>

        <p style="color: red; font-size: 12px; font-weight: bold" runat="server" id="lbl_error"></p>
        <div style="padding: 0px 0 0 0px; text-align: left;">
            <asp:Button ID="btn_addanother" runat="server" CssClass="formbut-n" OnClick="btn_save_Click" Text="Add Another" />
            <div style="float: right;">
                <asp:Button ID="btn_save" ValidationGroup="vd_editsave" OnClick="btn_save_Click" runat="server" CssClass="formbut-n" Text="Save and Continue" />
                &nbsp;	
                &nbsp;
                <asp:Button ID="btn_next" runat="server" OnClick="btn_next_Click" CssClass="formbut-n" Text="Next" />
            </div>
        </div>


    </div>


    <script src="plugins/jQuery/jquery-2.2.3.min.js"></script>


    <asp:HiddenField ID="hd_id" runat="server" />
    <script src="plugins/jQuery/jquery-2.2.3.min.js"></script>
    <script type="text/javascript">
        function DeletePopup(id) {
            $('#' + "<%=hd_id.ClientID %>").val(id);
            $('.bs-example-modal-sm').modal('show');

        }


    </script>

    <div class="modal fade bs-example-modal-sm" tabindex="-1" role="dialog" aria-labelledby="mySmallModalLabel">
        <div class="modal-dialog modal-sm" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                    <h4 class="modal-title" id="gridSystemModalLabel">Are you want to delete Yes/No? </h4>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">No</button>
                    <button type="button" id="btn_del" runat="server" onserverclick="btn_del_ServerClick" class="btn btn-primary">Yes</button>
                </div>
            </div>
        </div>
    </div>
</asp:Content>

