<%@ Page Title="" Language="C#" MasterPageFile="~/Student_Settings.master" AutoEventWireup="true" CodeFile="Adm_StudentGuardian_Documentype.aspx.cs" Inherits="Adm_StudentGuardian_Documentype" %>

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
                    <li class=" ">
                        <h2 class="hvr-syle"><a href="#" id="lnk_studentdetails" runat="server" onserverclick="lnk_studentdetails_ServerClick">Student Details</a></h2>
                    </li>
                    <li>
                        <h2 class="hvr-syle"><a href="#" id="lnk_guardiandetails" runat="server" onserverclick="lnk_guardiandetails_ServerClick">Guardian Details</a></h2>
                    </li>
                    <li class=" ">
                        <h2 class="hvr-syle"><a href="#" id="lnk_previousdetails" runat="server" onserverclick="lnk_previousdetails_ServerClick">Previous Details</a></h2>
                    </li>
                    <li class="active ">
                        <h2 class="hvr-syle"><a href="#" id="lnk_documentdetails" runat="server" onserverclick="lnk_documentdetails_ServerClick">Student Documents</a></h2>
                    </li>
                </ul>
            </div>
        </div>

        <style type="text/css">
            .document_table {
                margin-top: 10px;
            }
        </style>
        <div class="document_table">
            <table style="width: 100%" cellspacing="0" cellpadding="0">
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
        <style type="text/css">
            .formbut {
                padding: 10px 15px;
            }
        </style>
        <div class="form">

            <br />
            <p class="note" style="float: left">Fields with <span class="required">*</span> are required.</p>


            <div class="formCon" style="clear: left;">
                <div class="formConInner" id="innerDiv">
                    <table width="100%" border="0" cellspacing="0" cellpadding="0" id="documentTable">
                        <tbody>
                            <tr>
                                <td class="doc_drop" width="33%">
                                    <asp:DropDownList ID="drp_StudentDocument_doc_type" runat="server"></asp:DropDownList>
                                </td>
                                <td class="doc_title" style="display: none;" width="35%">
                                    <asp:TextBox ID="tbx_StudentDocument_title" runat="server" MaxLength="225"></asp:TextBox>
                                </td>
                                <td width="33%">

                                    <div class="formCon_os_infpbox">

                                        <asp:FileUpload ID="file_upload" runat="server" />
                                    </div>
                                </td>
                            </tr>
                        </tbody>
                    </table>

                </div>
            </div>

            <div class="button-bg">

                <div class="top-hed-btn-right">

                    <asp:Button ID="btn_addanother" runat="server" CssClass="formbut-n" OnClick="btn_addanother_Click" Text="Add Another" />
                    <ul>
                        <li>
                            <div style="float: right">
                            </div>
                        </li>

                    </ul>
                </div>
                <div class="top-hed-btn-left">
                    <asp:Button ID="btn_save" runat="server" CssClass="formbut-n" OnClick="btn_save_Click" Text="Save and Continue" />
                    &nbsp;			 
                </div>
            </div>

        </div>
        <!-- form -->


    </div>

</asp:Content>

