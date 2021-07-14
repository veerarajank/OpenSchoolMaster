<%@ Page Title="" Language="C#" MasterPageFile="~/Adm_Teacher_Settings.master" AutoEventWireup="true" CodeFile="Adm_Edit_Teacher_Documents.aspx.cs" Inherits="Adm_Edit_Teacher_Documents" %>

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

                                <a href="#" runat="server" id="teacher_info" onserverclick="teacher_info_ServerClick">
                                    <h2 class="no_hvr_style">Teacher Details</h2>
                                </a>
                            </li>
                            <li>
                                <a href="#" runat="server" id="teacher_contact" onserverclick="teacher_contact_ServerClick">
                                    <h2 class="no_hvr_style">Teacher Contact Details</h2>
                                </a>
                            </li>
                            <li class="active">
                                <a href="#" runat="server" id="teacher_document" onserverclick="teacher_document_ServerClick">
                                    <h2 class="no_hvr_style">Teacher Documents</h2>
                                </a>
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
                    <div class="emp_cntntbx">

                        <div class="document_table">
                            <table width="100%" cellspacing="0" cellpadding="0">
                                <tbody>
                                    <tr>
                                        <th>Document Name</th>
                                    </tr>
                                </tbody>
                            </table>

                            <div>
                                   <asp:ListView ID="lv_details" runat="server" GroupPlaceholderID="groupPlaceHolder1"
                    ItemPlaceholderID="itemPlaceHolder1">
                    <LayoutTemplate>
                        <table style="width: 100%" border="0" cellspacing="0" cellpadding="0" style="border-top: none;">
                            <asp:PlaceHolder runat="server" ID="groupPlaceHolder1"></asp:PlaceHolder>
                        </table>
                    </LayoutTemplate>
                    <GroupTemplate>
                        <tr>
                            <asp:PlaceHolder runat="server" ID="itemPlaceHolder1"></asp:PlaceHolder>
                        </tr>
                    </GroupTemplate>
                    <ItemTemplate>

                        <td style="width: 40%">
                            <%# Convert.ToString(Eval("doc_title")) %>
                        </td>
                        <td style="width: 30%">
                            <%# Convert.ToString(Eval("is_approved"))=="1" ? "<div class='tag_approved'>Approved</div>" : "<div class='tag_disapproved'>Disapproved</div>"  %>
                        </td>
                        <td style="width: 30%">
                            <ul class="tt-wrapper">
                                <li>
                                     <asp:LinkButton ID="ln_approved"  CssClass="tt-approved-disabled" CommandName="approve" CommandArgument='<%# Eval("id") %>' OnCommand="ln_approved_Command" runat="server" >
                                        <span>Approved</span>
                                    </asp:LinkButton>

                                </li>
                                <li>

                                    <asp:LinkButton ID="ln_disapproved"  CssClass="tt-disapproved" CommandName="disapprove" CommandArgument='<%# Eval("id") %>' OnCommand="ln_disapproved_Command" runat="server" >
                                        <span>Disapprove</span>
                                    </asp:LinkButton>
                                    
                                </li>
                                <li>
                                    <asp:LinkButton ID="ln_deleted"  CssClass="tt-delete" CommandName="delete" CommandArgument='<%# Eval("id") %>' OnCommand="ln_deleted_Command" runat="server" >
                                        <span>Delete</span>
                                    </asp:LinkButton>
                                    
                                </li>
                                <li>
                                     <asp:LinkButton ID="ln_download"  CssClass="tt-download" CommandName="download" CommandArgument='<%# Eval("id") %>' OnCommand="ln_download_Command" runat="server" >
                                        <span>Downlad</span>
                                    </asp:LinkButton>
                                </li>
                            </ul>
                        </td>
                    </ItemTemplate>
                    <EmptyDataTemplate>
                    </EmptyDataTemplate>
                </asp:ListView>
                            </div>

                        </div>
                        <!-- END div class="document_table" -->

                    </div>


                    <div class="clear"></div>

                    <div class="formCon" style="clear: left;">
                        <div class="formConInner" id="innerDiv">
                            <table style="width: 80%" border="0" cellspacing="0" cellpadding="0" id="documentTable">
                                <tbody>
                                    <tr>
                                        <td>
                                            <label>Document Name<font color="#FF0000">*</font></label></td>
                                        <td>
                                            <label>Choose The File Size Is Maximum 200 Kb</label></td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:TextBox AutoCompleteType="Disabled" runat="server" ID="tbx_tech_documentname" MaxLength="225"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:FileUpload ID="file_upload" CssClass="form-control" runat="server" />
                                        </td>

                                    </tr>
                                </tbody>
                            </table>
                        </div>
                    </div>


                    <div class="row buttons">
                        &nbsp;
                                            <asp:Button ID="btn_addanother" runat="server" Text="Add Another" OnClick="btn_save_Click" Height="30px" CssClass="formbut" />
                        <asp:Button ID="btn_save" runat="server" Text="Save" OnClick="btn_save_Click"  CssClass="formbut" Height="30px" />

                    </div>



                </div>
            </div>


            <!-- form -->



            <div class="clear"></div>


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

