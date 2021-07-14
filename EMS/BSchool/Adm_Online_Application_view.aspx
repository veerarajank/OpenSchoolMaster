<%@ Page Title="" Language="C#" MasterPageFile="~/Student_Settings.master" AutoEventWireup="true" CodeFile="Adm_Online_Application_view.aspx.cs" Inherits="Adm_Online_Application_view" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">


    <div id="content">
        <br />
        <button id="btn_new" runat="server" onserverclick="btn_new_ServerClick" class="a_tag-btn"><span>New Online Admission</span></button>
        <div runat="server" id="search_section">
            <div class="formCon">
                <div class="formConInner">
                    <div class="a_feed_cntnr" id="a_feed_cntnr">
                        <center><div class="online_academic_yr" id="lbl_acyear" runat="server">Academic Year - AY 2018-2019</div></center>
                        <br />
                        <asp:HiddenField ID="hd_acyear" runat="server" />


                    
                            <asp:ListView ID="lv_details" runat="server" GroupPlaceholderID="groupPlaceHolder1"
                                ItemPlaceholderID="itemPlaceHolder1">
                                <LayoutTemplate>
                                    <asp:PlaceHolder runat="server" ID="groupPlaceHolder1"></asp:PlaceHolder>

                                </LayoutTemplate>
                                <GroupTemplate>

                                    <asp:PlaceHolder runat="server" ID="itemPlaceHolder1"></asp:PlaceHolder>

                                </GroupTemplate>
                                <ItemTemplate>

                                    <div class="individual_feed">
                                        <div class="a_feed_online">
                                            <div class="a_boy"></div>
                                            <div class="a_feed_innercntnt">
                                                <div class="onln-adm-list" style="width:100%">
                                                    <div class="onln-adm-name">
                                                        
                                                    </div>
                                                    <div class="onln-adm-date">
                                                        <p>at <strong id="adm_time" runat="server"><%# Eval("adm_time") %></strong> - <strong id="adm_date" runat="server"><%# Eval("adm_date") %></strong></p>
                                                    </div>
                                                </div>
                                                <div class="onln-adm-list">
                                                    <div class="onln-adm-table">
                                                        <table class="reg_bx" width="100%" border="0" cellspacing="0" cellpadding="0">
                                                            <tbody>
                                                                <tr>
                                                                    <td width="30%">

                                                                        <p>Name</p>
                                                       
                                                                    </td>
                                                                    <td>:</td>
                                                                    <td> <strong>
                                                                    <a href='<%# Eval("link") %>'  id="lbl_name" runat="server" >
                                                                <%# Eval("name") %></a></strong></td>
                                                                </tr>
                                                                <tr>
                                                                    <td width="30%">
                                                                        <p>App ID</p>
                                                                    </td>
                                                                    <td>:</td>
                                                                    <td><%# Eval("admission_no") %></td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <p>Email</p>
                                                                    </td>
                                                                    <td>:</td>
                                                                    <td><%# Eval("email_id") %></td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <p>Phone</p>
                                                                    </td>
                                                                    <td>:</td>
                                                                    <td><%# Eval("mobile_no") %></td>
                                                                </tr>

                                                                <tr>
                                                                    <td>
                                                                        <p>Course</p>
                                                                    </td>
                                                                    <td>:</td>
                                                                    <td><%# Eval("course_name") %></td>
                                                                </tr>

                                                            </tbody>
                                                        </table>
                                                    </div>

                                                    <div class="onln-adm-table-icon">
                                                        <div class="online_time onln-adm-stus">

                                                            
                                                            <div class="online_status">
                                                                <div class='<%# Eval("status_class") %>'><%# Eval("status") %></div>
                                                            </div>
                                                        </div>

                                                        <div class="online_but onln-adm-stus">
                                                            
                                                              <ul class="tt-wrapper">
                                                                        <li>
                                                                            <a class='tt-approved-disabled' runat="server" id="dr_appr" Visible='<%# Convert.ToString(Eval("status"))=="Approved" ? true : false %>'>Approved</a>
                                                                            <asp:LinkButton ID='ln_approved' CssClass='tt-approved' CommandName='approve' CommandArgument='<%# Eval("id") %>' OnCommand="ln_approved_Command" runat='server' Visible='<%# Convert.ToString(Eval("status"))=="Approved" ? false : true %>'><span>Approve</span></asp:LinkButton> 
                                                                        </li>
                                                                        <li>

                                                                             <a class='tt-disapproved' runat="server" id="dr_disaapr" Visible='<%# Convert.ToString(Eval("status"))=="Disapproved" ? true : false %>'>Disapproved</a>                                                                            
                                                                             <asp:LinkButton ID='ln_disapproved' CssClass='tt-disapproved' CommandName='disapprove' CommandArgument='<%# Eval("id") %>' OnCommand="ln_disapproved_Command" runat='server' Visible='<%# Convert.ToString(Eval("status"))=="Disapproved" || Convert.ToString(Eval("status"))=="Approved"  ? false : true %>'><span>Disapprove</span></asp:LinkButton> 
                                                                        </li>
                                                                        <li>
                                                                             
                                                                                <asp:LinkButton ID='ln_deleted' CssClass="tt-delete" CommandName='delete' CommandArgument='<%# Eval("id")%>' OnCommand="ln_deleted_Command" runat='server' Visible='<%# Convert.ToString(Eval("status"))=="Approved" ? false : true %>'><span>Delete</span></asp:LinkButton>

                                                                        </li>
                                                                        <li>
                                                                            
                                                                                <asp:LinkButton ID='ln_waiting' CssClass='tt-waiting' CommandName='waiting' CommandArgument='<%# Eval("id") %>' OnCommand="ln_waiting_Command" runat='server' Visible='<%# Convert.ToString(Eval("status"))=="Approved" ? false : true %>'><span>Waiting List</span></asp:LinkButton>
                                                                        </li>
                                                                    </ul>
                                                        </div>

                                                    </div>
                                                </div>

                                            </div>
                                        </div>
                                    </div>

                                </ItemTemplate>
                                <EmptyDataTemplate>
                                </EmptyDataTemplate>
                            </asp:ListView>

                      




                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>

