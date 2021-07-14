<%@ Page Title="" Language="C#" MasterPageFile="~/Adm_Course_Settings.master" AutoEventWireup="true" CodeFile="Adm_Course_Batches_Semester_SubjectList.aspx.cs" Inherits="Adm_Course_Batches_Semester_SubjectList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script src="plugins/jQuery/jquery-2.2.3.min.js"></script>
    <asp:HiddenField ID="hd_semesterid" runat="server" />
    <asp:HiddenField ID="hd_courseid" runat="server" />

    <div runat="server" id="search_section">
        <h4><b>Manage Semester Subject</b></h4>



        <div class="button-bg">
            <div class="top-hed-btn-left"></div>
            <div class="top-hed-btn-right">
                <ul>
                    <li>
                        <button id="btn_new" runat="server" onserverclick="btn_new_ServerClick" class="a_tag-btn"><span>Link Subject</span></button>
                    </li>
                </ul>
            </div>
        </div>


        <div id="academic-years-grid" class="grid-view">

            <div class="summary" id="lbl_cnt" runat="server" style="font-size: 12px;">
                Displaying 2 result(s).
            </div>
            <asp:ListView ID="lv_subject" runat="server" GroupPlaceholderID="groupPlaceHolder1"
                ItemPlaceholderID="itemPlaceHolder1">
                <LayoutTemplate>
                    <table class="items" style="border: 1px solid #b9c7d0!important;">
                        <thead style="border: 1px solid #b9c7d0!important;">

                            <th>Subject
                            </th>
                            <th>Maximum Weekly Classes
                            </th>
                            <th>Elective
                            </th>

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

                    <td>
                        <%# Eval("subject") %>
                    </td>
                    <td>
                        <%# Eval("max_weekly_classes") %>
                    </td>


                    <td>
                        <%# Convert.ToString(Eval("is_elective")).ToLower()=="1" ? "<span class='bg-green'>&nbsp;Yes&nbsp;</span>" : "<span class='bg-red'>&nbsp;No&nbsp;</span>"  %>
                    </td>


                </ItemTemplate>
                <EmptyDataTemplate>
                    <table class="items" style="border: 1px solid #b9c7d0!important;">
                        <thead style="border: 1px solid #b9c7d0!important;">

                            <th>Subject
                            </th>
                            <th>Maximum Weekly Classes
                            </th>
                            <th>Elective
                            </th>
                        </thead>
                        <tr style="border: 1px solid #b9c7d0!important;">
                            <td colspan="3">No Subject(s) Found
                            </td>
                        </tr>
                    </table>
                </EmptyDataTemplate>
            </asp:ListView>

            <br />
            <br />
        </div>
    </div>

    <div id="add_section" runat="server" visible="false">


        <div class="cont_right formWrapper" >
          <h1><b>Create Semester Subject</b></h1>

            <div class="button-bg">
                <div class="top-hed-btn-left"></div>
                <div class="top-hed-btn-right">
                    <ul>
                        <li>
                            <button id="btn_manage" runat="server" onserverclick="btn_manage_ServerClick" class="a_tag-btn"><span>Manage Semester Subject</span></button>
                        </li>
                    </ul>
                </div>
            </div>

            <div class="formCon">
                <div class="formConInner">
                     <div id="Div1" class="grid-view">

            <div class="summary" id="lbl_cnt2" runat="server" style="font-size: 12px;">
                Displaying 2 result(s).
            </div>
            <asp:ListView ID="lst_addsubjects" runat="server" GroupPlaceholderID="groupPlaceHolder1"
                ItemPlaceholderID="itemPlaceHolder1">
                <LayoutTemplate>
                    <table class="items" style="border: 1px solid #b9c7d0!important;">
                        <thead style="border: 1px solid #b9c7d0!important;">
                            <th>Select</th>
                            <th>Subject
                            </th>
                            <th>Maximum Weekly Classes
                            </th>
                            <th>Elective
                            </th>

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
                      <td>
                        <asp:CheckBox ID="chk_select" runat="server" />  
                    </td>
                    <td>
                        <%# Eval("name") %>
                    </td>
                    <td>
                       <asp:TextBox ID="tbx_max_class" runat="server"></asp:TextBox>
                    </td>
                    <td>
                        <asp:CheckBox ID="chk_elective" runat="server" />  
                        <asp:HiddenField id="hd_subject_id" runat="server" Value='<%# Eval("id") %>' />
                    </td>


                </ItemTemplate>
                <EmptyDataTemplate>
                    <table class="items" style="border: 1px solid #b9c7d0!important;">
                        <thead style="border: 1px solid #b9c7d0!important;">

                            <th>Subject
                            </th>
                            <th>Maximum Weekly Classes
                            </th>
                            <th>Elective
                            </th>
                        </thead>
                        <tr style="border: 1px solid #b9c7d0!important;">
                            <td colspan="3">No Subject(s) Found
                            </td>
                        </tr>
                    </table>
                </EmptyDataTemplate>
            </asp:ListView>

            <br />
            <br />

                          <div style="padding: 0px 0 0 0px; text-align: left">
                            <button id="btn_save" validationgroup="vd_save" runat="server" style="width: 150px" onserverclick="btn_save_ServerClick" class="a_tag-btn"><span>Save</span></button>

                        </div>

        </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>

