<%@ Page Title="" Language="C#" MasterPageFile="~/Adm_Teacher_Settings.master" AutoEventWireup="true" CodeFile="Adm_Teacher_List.aspx.cs" Inherits="Adm_Teacher_List" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
     <script src="plugins/jQuery/jquery-2.2.3.min.js"></script>
    <div runat="server" id="search_section">
        <h4><b>Manage Teacher</b></h4>

        <div class="button-bg">
            <div class="top-hed-btn-left"></div>
            <div class="top-hed-btn-right">
                <ul>
                    <li>
                        <button id="btn_new" runat="server" onserverclick="btn_new_ServerClick" class="a_tag-btn"><span>Add Teacher</span></button>
                    </li>
                </ul>
            </div>
        </div>


        <div id="academic-years-grid" class="grid-view">

            <div class="summary" id="lbl_cnt" runat="server" style="font-size: 12px;">
                Displaying 2 result(s).
            </div>
            <asp:ListView ID="lv_teacherdetails" runat="server" GroupPlaceholderID="groupPlaceHolder1"
                ItemPlaceholderID="itemPlaceHolder1" OnPagePropertiesChanging="lv_teacherdetails_PagePropertiesChanging">
                <LayoutTemplate>
                    <table class="items" style="border: 1px solid #b9c7d0!important;">
                        <thead style="border: 1px solid #b9c7d0!important;">

                            <th>Teacher Name	
                            </th>
                            <th>Teacher No	
                            </th>
                            <th>Department
                            </th>                         
                            <th>Gender
                            </th>
                            <th>Status
                            </th>
                            <th class="col-md-2">Action
                            </th>
                        </thead>
                        <asp:PlaceHolder runat="server" ID="groupPlaceHolder1"></asp:PlaceHolder>
                        <tr style="background-color: #EEEEE0; border: 1px solid #b9c7d0!important" class="tableInfoTH">
                            <td colspan="6" class="tableInfoTD">
                                <asp:DataPager ID="DataPager1" runat="server" PagedControlID="lv_teacherdetails" PageSize="10" class="googleNavegationBar">
                                    <Fields>
                                        <aspPage:GooglePagerField NextPageImageUrl="~/Images/button_arrow_right.gif" PreviousPageImageUrl="~/Images/button_arrow_left.gif" />
                                    </Fields>
                                </asp:DataPager>
                            </td>
                        </tr>
                    </table>
                </LayoutTemplate>
                <GroupTemplate>
                    <tr style="border: 1px solid #b9c7d0!important;">
                        <asp:PlaceHolder runat="server" ID="itemPlaceHolder1"></asp:PlaceHolder>
                    </tr>
                </GroupTemplate>
                <ItemTemplate>

                    <td>
                        <%# Convert.ToString(Eval("first_name")) + " " +  Convert.ToString(Eval("middle_name")) + " " + Convert.ToString(Eval("last_name")) %>
                    </td>
                    <td>
                        <%# Eval("employee_number") %>
                    </td>
                    <td>
                        <%# Convert.ToString(Eval("department")) %>
                    </td>
                   
                    <td>
                        <%# Eval("gender_txt") %>
                    </td>

                    <td>
                        <%# Convert.ToString(Eval("is_active")).ToLower()=="1" ? "<span class='bg-green'>&nbsp;Active&nbsp;</span>" : "<span class='bg-red'>&nbsp;Inactive&nbsp;</span>"  %>
                    </td>
                    <td>

                        <asp:LinkButton ID="lnk_search" CommandName="view" CommandArgument='<%# Eval("id") %>' OnCommand="lnk_search_Command" runat="server" Visible='<%# Convert.ToBoolean(Session["Access_View"])%>'>
                                    <img src="gridview/view.png" alt="View">            
                        </asp:LinkButton>
                        &nbsp;
                     <asp:LinkButton ID="lnk_edit" CommandName="edit" CommandArgument='<%# Eval("id") %>' OnCommand="lnk_edit_Command" runat="server" Visible='<%# Convert.ToBoolean(Session["Access_Edit"])%>'>
                                    <img src="gridview/update.png" alt="Update">            
                     </asp:LinkButton>&nbsp;
                      <asp:LinkButton ID="lnk_del" OnClick='DeletePopup("<%# Eval("id") %>")' Style='<%# Convert.ToBoolean(Session["Access_Trash"])==true ? "visibility:visible": "visibility:hidden" %>'>
                        <img src="gridview/delete.png" alt="Delete">                                    
                      </asp:LinkButton>

                    </td>

                </ItemTemplate>
                <EmptyDataTemplate>
                    <table class="items" style="border: 1px solid #b9c7d0!important;">
                        <thead style="border: 1px solid #b9c7d0!important;">
                          
                   
                            <th>Teacher Name	
                            </th>
                            <th>Teacher No	
                            </th>
                            <th>Department
                            </th>                         
                            <th>Gender
                            </th>
                            <th>Status
                            </th>
                            <th class="col-md-2">Action
                            </th>
                        </thead>
                        <tr style="border: 1px solid #b9c7d0!important;">
                            <td colspan="6">No Results Found
                            </td>
                        </tr>
                    </table>
                </EmptyDataTemplate>
            </asp:ListView>

            <br />
            <br />
        </div>
    </div>
</asp:Content>

