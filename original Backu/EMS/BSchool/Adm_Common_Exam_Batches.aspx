<%@ Page Title="" Language="C#" MasterPageFile="~/Adm_Examination.master" AutoEventWireup="true" CodeFile="Adm_Common_Exam_Batches.aspx.cs" Inherits="Adm_Common_Exam_Batches" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <div class="cont_right formWrapper">
        <h1><b>Scheduled Batches</b></h1>
        <style>
            .check {
                width: 10px;
            }

            #spec {
                display: none;
            }
        </style>
        <div class="form">
            <div class="formCon">
                <div class="formConInner">
                    <span style="color: red">*Please select batches.</span>
                    <asp:ListView ID="lv_batches" runat="server" GroupPlaceholderID="groupPlaceHolder1"
                        ItemPlaceholderID="itemPlaceHolder1">
                        <LayoutTemplate>
                            <table class="table table-bordered table-responsive table-hover dataTable" style="border: 2px solid #d2d3d4!important;">
                                <tr class="bg-blue">
                                    <th>Course
                                    </th>
                                    <th>Batch
                                    </th>
                                    <th>Semeser
                                    </th>
                                    <th>Applicable
                                    </th>
                                    <th>Date Published
                                    </th>
                                    <th>Result Published
                                    </th>
                                    <th>Action
                                    </th>
                                </tr>

                                <asp:PlaceHolder runat="server" ID="groupPlaceHolder1"></asp:PlaceHolder>
                            </table>
                        </LayoutTemplate>
                        <GroupTemplate>
                            <tr>
                                <asp:PlaceHolder runat="server" ID="itemPlaceHolder1"></asp:PlaceHolder>
                            </tr>
                        </GroupTemplate>
                        <ItemTemplate>
                            <td>
                                <%# Eval("Course") %>
                            </td>
                            <td>
                                <%# Eval("Batch") %>
                            </td>
                            <td>
                                <%# Eval("Semester") %>
                            </td>
                            <td>
                                <asp:CheckBox ID="chk_applicable" Text="" runat="server"  Checked='<%# Convert.ToInt32(Eval("is_applicable"))==1 ? true : false %>'  />
                            </td>
                            <td>
                                <asp:CheckBox ID="chk_date" Text="" runat="server"  Checked='<%# Convert.ToInt32(Eval("date_is_published"))==1 ? true : false %>'  />
                            </td>
                            <td>
                                <asp:CheckBox ID="chk_result" Text="" runat="server"  Checked='<%# Convert.ToInt32(Eval("result_is_published"))==1 ? true : false %>'  />
                                 <asp:HiddenField ID="hd_examlistid" runat="server" Value='<%# Convert.ToString(Eval("id"))%>' />
                            </td>

                            <td>
                                <asp:LinkButton ID="lnk_manage" CommandName="manage" CommandArgument='<%# Eval("id") %>' OnCommand="lnk_manage_Command" runat="server" Visible='<%# Convert.ToBoolean(Session["Access_Edit"])%>'>
                                                <img src="gridview/mark_a.png" alt="Manage">            
                                </asp:LinkButton>&nbsp;
                            </td>

                        </ItemTemplate>
                        <EmptyDataTemplate>
                            <table class="table table-bordered table-responsive table-hover dataTable">
                                 <tr class="bg-blue">
                                    <th>Course
                                    </th>
                                    <th>Batch
                                    </th>
                                    <th>Semeser
                                    </th>
                                    <th>Applicable
                                    </th>
                                    <th>Date Published
                                    </th>
                                    <th>Result Published
                                    </th>
                                    <th>Action
                                    </th>
                                </tr>
                                <tr>
                                    <td colspan="7">No Results Found
                                    </td>
                                </tr>
                            </table>
                        </EmptyDataTemplate>
                    </asp:ListView>

                    <div class="row buttons">
                        <asp:Button ID="btn_save" ValidationGroup="vd_save" runat="server" Text="Save" OnClick="btn_save_Click" Width="150px" CssClass="a_tag-btn" />
                    </div>

                </div>

            </div>
        </div>
    </div>

</asp:Content>

