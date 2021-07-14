<%@ Page Title="" Language="C#" MasterPageFile="~/AcademicConfiguration.master" AutoEventWireup="true" CodeFile="Adm_academicYears.aspx.cs" Inherits="Adm_academicYears" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    
    <script src="plugins/jQuery/jquery-2.2.3.min.js"></script>
    <div runat="server" id="search_section">
        <h4><b>Manage Academic Years</b></h4>

        <div class="button-bg">
            <div class="top-hed-btn-left"></div>
            <div class="top-hed-btn-right">
                <ul>
                    <li>
                        <button id="btn_new" runat="server" onserverclick="btn_new_ServerClick" class="a_tag-btn"><span>Add Academic Year</span></button>
                    </li>
                </ul>
            </div>
        </div>


        <div id="academic-years-grid" class="grid-view">

            <div class="summary" id="lbl_cnt" runat="server" style="font-size: 12px;">
                Displaying 2 result(s).
            </div>
            <asp:ListView ID="lv_academicyear" runat="server" GroupPlaceholderID="groupPlaceHolder1"
                ItemPlaceholderID="itemPlaceHolder1" OnPagePropertiesChanging="lv_academicyear_PagePropertiesChanging">
                <LayoutTemplate>
                    <table class="items" style="border: 1px solid #b9c7d0!important;">
                        <thead style="border: 1px solid #b9c7d0!important;">

                            <th>Name
                            </th>
                            <th>Start On
                            </th>
                            <th>Ends On
                            </th>
                            <th>Description
                            </th>
                            <th>Status
                            </th>
                            <th class="col-md-2">Action
                            </th>
                        </thead>
                        <asp:PlaceHolder runat="server" ID="groupPlaceHolder1"></asp:PlaceHolder>
                        <tr style="background-color: #EEEEE0; border: 1px solid #b9c7d0!important" class="tableInfoTH">
                            <td colspan="6" class="tableInfoTD">
                                <asp:DataPager ID="DataPager1" runat="server" PagedControlID="lv_academicyear" PageSize="10" class="googleNavegationBar">
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
                        <%# Eval("academic_name") %>
                    </td>
                    <td>
                        <%# Eval("academic_start") %>
                    </td>
                    <td>
                        <%# Eval("academic_end") %>
                    </td>
                    <td>
                        <%# Eval("academic_description") %>
                    </td>

                    <td>
                        <%# Convert.ToString(Eval("academic_status")).ToLower()=="1" ? "<span class='bg-green'>&nbsp;Active&nbsp;</span>" : "<span class='bg-red'>&nbsp;Inactive&nbsp;</span>"  %>
                    </td>
                    <td>

                        <asp:LinkButton ID="lnk_search" CommandName="view" CommandArgument='<%# Eval("academic_id") %>' OnCommand="lnk_search_Command" runat="server" Visible='<%# Convert.ToBoolean(Session["Access_View"])%>'>
                                    <img src="gridview/view.png" alt="View">            
                        </asp:LinkButton>
                        &nbsp;
                     <asp:LinkButton ID="lnk_edit" CommandName="edit" CommandArgument='<%# Eval("academic_id") %>' OnCommand="lnk_edit_Command" runat="server" Visible='<%# Convert.ToBoolean(Session["Access_Edit"])%>'>
                                    <img src="gridview/update.png" alt="Update">            
                     </asp:LinkButton>&nbsp;
                      <asp:LinkButton ID="lnk_del" OnClick='DeletePopup("<%# Eval("academic_id") %>")' Style='<%# Convert.ToBoolean(Session["Access_Trash"])==true ? "visibility:visible": "visibility:hidden" %>'>
                        <img src="gridview/delete.png" alt="Delete">                                    
                      </asp:LinkButton>

                    </td>

                </ItemTemplate>
                <EmptyDataTemplate>
                    <table class="items" style="border: 1px solid #b9c7d0!important;">
                        <thead style="border: 1px solid #b9c7d0!important;">
                          
                        <th>Start On
                        </th>
                            <th>Ends On
                            </th>
                            <th>Description
                            </th>
                            <th>Status
                            </th>
                            <th class="col-md-1">Action
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

    <div id="add_section" runat="server">

        <h4><b>New Academic Year Setup</b></h4>
        <div class="button-bg">
            <div class="top-hed-btn-left"></div>
            <div class="top-hed-btn-right">
                <ul>
                    <li>
                        <a class="a_tag-btn" href="Adm_academicYears.aspx?mode=search"><span>Manage Academic Years</span></a></li>
                </ul>
            </div>
        </div>
        <div class="formCon">
            <div class="formConInner">
                <h3 id="h3_current_year" runat="server">Current Academic Year :</h3>
                <table style="width: 80%; border: 0; font-size: 12px;">
                    <tbody>
                        <tr>
                            <td>
                                <b>Starts on :</b>
                            </td>
                            <td id="td_current_year_start" runat="server"></td>
                            <td id="td_current_year_end" runat="server">
                                <b>Ends on :</b>
                            </td>
                            <td></td>
                        </tr>
                        <tr>
                            <td colspan="4">&nbsp;</td>
                        </tr>
                        <tr>
                            <td style="width: 20%">
                                <b>Description :</b>
                            </td>
                            <td colspan="3" id="td_current_year_desc" runat="server"></td>
                        </tr>
                    </tbody>
                </table>
            </div>
        </div>
        <p class="note" runat="server" id="field_mandary">Fields with <span class="required">*</span> are required.</p>
        <div class="formCon">
            <div class="formConInner">
                <h3>Enter the details of the upcoming academic year</h3>
                <div class="txtfld-col-btn">
                    <div class="txtfld-col">
                        <label for="AcademicYears_name" class="required">Name <span class="required">*</span></label>
                        <asp:TextBox ID="tbx_AcademicYears_name" runat="server" AutoCompleteType="Disabled" MaxLength="120"></asp:TextBox>

                    </div>
                    <div class="txtfld-col">
                        <label for="AcademicYears_start" class="required">Starts On <span class="required">*</span></label>
                        <asp:TextBox ID="tbx_AcademicYears_start" runat="server" AutoCompleteType="Disabled" class="hasDatepicker"></asp:TextBox>

                    </div>
                    <div class="txtfld-col">
                        <label for="AcademicYears_end" class="required">Ends On <span class="required">*</span></label>
                        <asp:TextBox ID="tbx_AcademicYears_end" runat="server" AutoCompleteType="Disabled" class="hasDatepicker"></asp:TextBox>
                    </div>
                </div>
                <div class="txtfld-col-btn">
                    <div class="txtfld-col" style="width:100%">
                    <label for="AcademicYears_description" class="required">Description <span class="required">*</span></label>
                    <textarea style="width: 100% !important; height: 120px;" runat="server" id="tbx_AcademicYears_description"></textarea>
                        </div>
                </div>
                <div class="txtfld-col-btn">
                    <div class="txtfld-col">
                        <label for="AcademicYears_status" class="required">Status <span class="required">*</span></label>
                        <asp:DropDownList ID="ddl_AcademicYears_status" runat="server">
                            <asp:ListItem Text="Active" Value="1"></asp:ListItem>
                            <asp:ListItem Text="Inactive" Value="0"></asp:ListItem>
                        </asp:DropDownList>

                    </div>
                </div>

            </div>
        </div>
        <p style="color:red;font-size:12px;font-weight:bold" runat="server" id="lbl_error" ></p>


        <div class="button-bg">
            <div class="top-hed-btn-left"></div>
            <div class="top-hed-btn-right">
                <ul>
                    <li>
                        <button id="btn_createyear" runat="server" style="width:150px"  onserverclick="btn_createyear_ServerClick" class="a_tag-btn"><span>Create Year</span></button>
                    </li>
                </ul>
            </div>
        </div>

    </div>


    <div id="update_section" runat="server">

        <h4><b>Update Academic Year</b></h4>
        <div class="form">
            <div class="button-bg">
                <div class="top-hed-btn-left"></div>
                <div class="top-hed-btn-right">
                    <ul>
                        <li>
                            <a class="a_tag-btn" href="Adm_academicYears.aspx?mode=add"><span>Add Academic Year</span></a>            </li>
                        <li>
                            <a class="a_tag-btn" href="Adm_academicYears.aspx?mode=search"><span>Manage Academic Years</span></a>            </li>
                    </ul>
                </div>
            </div>


            <p class="note" id="edit_field_mandary" runat="server">Fields with <span class="required">*</span> are required.</p>
            <div class="formCon">
                <div class="formConInner">
                    <h3>Update the details of this academic year</h3>
                    <div class="txtfld-col-btn">
                        <div class="txtfld-col">
                            <label for="AcademicYears_name" class="required">Name <span class="required">*</span></label>
                            <asp:TextBox ID="tbx_edit_AcademicYears_name" runat="server" AutoCompleteType="Disabled" MaxLength="120"></asp:TextBox>
                        </div>
                        <div class="txtfld-col">
                            <label for="AcademicYears_start" class="required">Starts On <span class="required">*</span></label>
                            <asp:TextBox ID="tbx_edit_AcademicYears_start" runat="server" AutoCompleteType="Disabled" class="hasDatepicker"></asp:TextBox>
                        </div>
                        <div class="txtfld-col">
                            <label for="AcademicYears_end" class="required">Ends On <span class="required">*</span></label>
                            <asp:TextBox ID="tbx_edit_AcademicYears_end" runat="server" AutoCompleteType="Disabled" class="hasDatepicker"></asp:TextBox>
                        </div>
                    </div>
                    <div class="txtfld-col-btn">
                          <div class="txtfld-col" style="width:100%">
                        <label for="AcademicYears_description" class="required">Description <span class="required">*</span></label>                        
                        <textarea style="width: 100% !important; height: 120px;" runat="server" id="tbx_edit_AcademicYears_description"></textarea>
                              </div>
                    </div>
                    <div class="txtfld-col-btn">
                        <div class="txtfld-col">
                            <label for="AcademicYears_status" class="required">Status <span class="required">*</span></label>
                            <asp:DropDownList ID="ddl_edit_AcademicYears_status" runat="server">
                            <asp:ListItem Text="Active" Value="1"></asp:ListItem>
                            <asp:ListItem Text="Inactive" Value="0"></asp:ListItem>
                        </asp:DropDownList>
                        </div>
                    </div>




                </div>
            </div>

              <div class="button-bg">
            <div class="top-hed-btn-left"></div>
            <div class="top-hed-btn-right">
                <ul>
                    <li>
                        <button id="btn_update" runat="server" onserverclick="btn_update_ServerClick" style="width:150px" class="a_tag-btn"><span>Save</span></button>
                    </li>
                </ul>
            </div>
        </div>


        </div>



    </div>

    <div id="delete_section" runat="server">
    </div>
    <div id="view_section" runat="server">

        <h4><b>View Academic Year</b></h4>

        <div class="button-bg">
            <div class="top-hed-btn-left"></div>
            <div class="top-hed-btn-right">
                <ul>
                    <li>
                        <button id="btn_edit" runat="server" onserverclick="btn_edit_ServerClick" class="a_tag-btn"><span>Edit</span></button>

                    </li>
                    <li>
                        <button id="btn_manage" runat="server" onserverclick="btn_manage_ServerClick" class="a_tag-btn"><span>Manage Academic Years</span></button>
                    </li>
                </ul>
            </div>

        </div>



        <div class="table_listbx">
            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                <tbody>
                    <tr>
                        <td width="20%">
                            <b>Name</b>
                        </td>
                        <td width="3%"><strong>:</strong></td>
                        <td width="77%">
                            <label id="lbl_academicyearname" runat="server"></label>

                        </td>
                    </tr>
                    <tr>
                        <td>
                            <b>Description</b>
                        </td>
                        <td><strong>:</strong></td>
                        <td>
                            <label id="lbl_academicyeardesc" runat="server"></label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <b>Starts on</b>
                        </td>
                        <td><strong>:</strong></td>
                        <td>
                            <label id="lbl_academicyearstart" runat="server"></label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <b>Ends on</b>
                        </td>
                        <td><strong>:</strong></td>
                        <td>
                            <label id="lbl_academicyearend" runat="server"></label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <b>Status</b>
                        </td>
                        <td><strong>:</strong></td>
                        <td>
                            <label id="lbl_academicyearsts" runat="server"></label>
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>


    </div>



    <script src="plugins/jQuery/jquery-2.2.3.min.js"></script>

    <script type="text/javascript">
        $(function () {
            $('#' + '<%= tbx_AcademicYears_start.ClientID %>').datepicker({ autoclose: true });
            $('#' + '<%= tbx_AcademicYears_end.ClientID %>').datepicker({ autoclose: true });
            $('#' + '<%= tbx_edit_AcademicYears_start.ClientID %>').datepicker({ autoclose: true });
            $('#' + '<%= tbx_edit_AcademicYears_end.ClientID %>').datepicker({ autoclose: true });
        });
    </script>
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
        <button type="button" id="btn_del"  runat="server" onserverclick="btn_del_ServerClick" class="btn btn-primary">Yes</button>
      </div>
            </div>
        </div>
    </div>


</asp:Content>

