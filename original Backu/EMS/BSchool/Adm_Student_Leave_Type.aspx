<%@ Page Title="" Language="C#" MasterPageFile="~/Student_Settings.master" AutoEventWireup="true" CodeFile="Adm_Student_Leave_Type.aspx.cs" Inherits="Adm_Student_Leave_Type" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
     <script src="dist/js/jscolor.js"></script>
    <style>
        .jscolor {
            height: 22px;
            width: 22px;
            background: url(images/trigger.png) center no-repeat;
            vertical-align: middle;
            margin: 0 .25em;
            display: inline-block;
        }
        .txtfld-col {
            float: left;
            width: 31%;
            padding: 0px 7px 0px 7px;
            margin: 0px;
            min-height: 0px;
        }
           .row {
             margin-bottom: 0px !important;
         }

    </style>
    <div runat="server" id="search_section">
        <h4><b>Events Types</b></h4>

        <div class="button-bg">
            <div class="top-hed-btn-left"></div>
            <div class="top-hed-btn-right">
                <ul>
                    <li>
                        <button id="btn_new" runat="server" onserverclick="btn_new_ServerClick" class="a_tag-btn"><span>Create Leave Type</span></button>
                    </li>
                </ul>
            </div>
        </div>


        <div id="academic-years-grid" class="grid-view">

            <div class="summary" id="lbl_cnt" runat="server" style="font-size: 12px;">
                Displaying 2 result(s).
            </div>
            <h4>Active Leave types</h4>
            <asp:ListView ID="lv_leavetype" runat="server" GroupPlaceholderID="groupPlaceHolder1"
                ItemPlaceholderID="itemPlaceHolder1" OnPagePropertiesChanging="lv_leavetype_PagePropertiesChanging">
                <LayoutTemplate>
                    <table class="items" style="border: 1px solid #b9c7d0!important;">
                        <thead>

                            <th>Leave Type
                            </th>
                             <th>Code
                            </th>
                             <th>Label
                            </th>                             
                            <th>Color Code
                            </th>
                            <th>Is Excluded
                            </th>                                                      
                            <th class="col-md-2">Action
                            </th>
                        </thead>
                        <asp:PlaceHolder runat="server" ID="groupPlaceHolder1"></asp:PlaceHolder>
                        <tr style="background-color: #EEEEE0; border: 1px solid #b9c7d0!important" class="tableInfoTH">
                            <td colspan="6" class="tableInfoTD">
                                <asp:DataPager ID="DataPager1" runat="server" PagedControlID="lv_leavetype" PageSize="10" class="googleNavegationBar">
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
                        <%# Eval("leave_name") %>
                    </td>
                     <td>
                        <%# Eval("leave_code") %>
                    </td>
                     <td>
                        <%# Eval("leave_label") %>
                    </td>
                    <td>
                        <%# Convert.ToString(Eval("Is_exclude")).ToLower()=="1" ? "<span class='bg-green'>&nbsp;Yes&nbsp;</span>" : "<span class='bg-red'>&nbsp;No&nbsp;</span>"  %>
                    </td>
                    <td>
                        <%# Eval("leave_colour_code") %>
                        <span style='background-color:<%# Eval("leave_colour_code") %>'>&nbsp;&nbsp;&nbsp;<span>&nbsp;</span></span>
                    </td>
                   
                    <td>
                     <asp:LinkButton ID="lnk_search" CommandName="view" CommandArgument='<%# Eval("leave_id") %>' OnCommand="lnk_search_Command" runat="server" Visible='<%# Convert.ToBoolean(Session["Access_View"])%>'>
                                    <img src="gridview/view.png" alt="View">            
                        </asp:LinkButton>
                        &nbsp;
                     <asp:LinkButton ID="lnk_edit" CommandName="edit" CommandArgument='<%# Eval("leave_id") %>' OnCommand="lnk_edit_Command" runat="server" Visible='<%# Convert.ToBoolean(Session["Access_Edit"])%>'>
                                    <img src="gridview/update.png" alt="Update">            
                     </asp:LinkButton>&nbsp;
                      <asp:LinkButton ID="lnk_del" OnClick='DeletePopup("<%# Eval("leave_id") %>")' Style='<%# Convert.ToBoolean(Session["Access_Trash"])==true ? "visibility:visible": "visibility:hidden" %>'>
                        <img src="gridview/delete.png" alt="Delete">                                    
                      </asp:LinkButton>

                    </td>

                </ItemTemplate>
                <EmptyDataTemplate>
                    <table class="items" style="border: 1px solid #b9c7d0!important;">
                        <thead style="border: 1px solid #b9c7d0!important;">
                          <th>Leave Type
                            </th>
                             <th>Code
                            </th>
                             <th>Label
                            </th>                             
                            <th>Color Code
                            </th>
                            <th>Is Excluded
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
             <div class="summary" id="lbl_cnt2" runat="server" style="font-size: 12px;">
                Displaying 2 result(s).
            </div>
            <h4>Inactive Leave types</h4>
            <asp:ListView ID="lv_inactiveleavetype" runat="server" GroupPlaceholderID="groupPlaceHolder1"
                ItemPlaceholderID="itemPlaceHolder1" OnPagePropertiesChanging="lv_inactiveleavetype_PagePropertiesChanging">
                <LayoutTemplate>
                    <table class="items" style="border: 1px solid #b9c7d0!important;">
                        <thead>

                            <th>Leave Type
                            </th>
                             <th>Code
                            </th>
                             <th>Label
                            </th>                             
                            <th>Color Code
                            </th>
                            <th>Is Excluded
                            </th>                                                      
                            <th class="col-md-2">Action
                            </th>
                        </thead>
                        <asp:PlaceHolder runat="server" ID="groupPlaceHolder1"></asp:PlaceHolder>
                        <tr style="background-color: #EEEEE0; border: 1px solid #b9c7d0!important" class="tableInfoTH">
                            <td colspan="6" class="tableInfoTD">
                                <asp:DataPager ID="DataPager1" runat="server" PagedControlID="lv_inactiveleavetype" PageSize="10" class="googleNavegationBar">
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
                        <%# Eval("leave_name") %>
                    </td>
                     <td>
                        <%# Eval("leave_code") %>
                    </td>
                     <td>
                        <%# Eval("leave_label") %>
                    </td>
                    <td>
                        <%# Convert.ToString(Eval("Is_exclude")).ToLower()=="1" ? "<span class='bg-green'>&nbsp;Yes&nbsp;</span>" : "<span class='bg-red'>&nbsp;No&nbsp;</span>"  %>
                    </td>
                    <td>
                        <%# Eval("leave_colour_code") %>
                        <span style='background-color:<%# Eval("leave_colour_code") %>'>&nbsp;&nbsp;&nbsp;<span>&nbsp;</span></span>
                    </td>
                   
                    <td>
                     <asp:LinkButton ID="lnk_search" CommandName="view" CommandArgument='<%# Eval("leave_id") %>' OnCommand="lnk_search_Command" runat="server" Visible='<%# Convert.ToBoolean(Session["Access_View"])%>'>
                                    <img src="gridview/view.png" alt="View">            
                        </asp:LinkButton>
                        &nbsp;
                     <asp:LinkButton ID="lnk_edit" CommandName="edit" CommandArgument='<%# Eval("leave_id") %>' OnCommand="lnk_edit_Command" runat="server" Visible='<%# Convert.ToBoolean(Session["Access_Edit"])%>'>
                                    <img src="gridview/update.png" alt="Update">            
                     </asp:LinkButton>&nbsp;
                      <asp:LinkButton ID="lnk_del" OnClick='DeletePopup("<%# Eval("leave_id") %>")' Style='<%# Convert.ToBoolean(Session["Access_Trash"])==true ? "visibility:visible": "visibility:hidden" %>'>
                        <img src="gridview/delete.png" alt="Delete">                                    
                      </asp:LinkButton>

                    </td>

                </ItemTemplate>
                <EmptyDataTemplate>
                    <table class="items" style="border: 1px solid #b9c7d0!important;">
                        <thead style="border: 1px solid #b9c7d0!important;">
                          <th>Leave Type
                            </th>
                             <th>Code
                            </th>
                             <th>Label
                            </th>                             
                            <th>Color Code
                            </th>
                            <th>Is Excluded
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
        </div>
    </div>

    <div id="add_section" runat="server">

        <h4><b>Create New Leave Type</b></h4>
        <div class="button-bg">
            <div class="top-hed-btn-left"></div>
            <div class="top-hed-btn-right">
                <ul>
                    <li>
                        <a class="a_tag-btn" href="Adm_Student_Leave_Type.aspx?mode=search"><span>Manage Leave Type</span></a></li>
                </ul>
            </div>
        </div>
        
        <p class="note" runat="server" id="field_mandary">Fields with <span class="required">*</span> are required.</p>
        <div class="formCon">
            <div class="formConInner">                
                <div class="row">
                    <div class="txtfld-col">
                        <label for="Leave_type" class="required">Name <span class="required">*</span></label>
                        <asp:TextBox ID="tbx_leave_type" runat="server" AutoCompleteType="Disabled" MaxLength="120"></asp:TextBox>
                    </div>                                     
                    <div class="txtfld-col">
                        <label for="Leave_code" class="required">Code <span class="required">*</span></label>
                        <asp:TextBox ID="tbx_leavecode" runat="server" AutoCompleteType="Disabled" MaxLength="120"></asp:TextBox>
                    </div>   
                    
                </div>     
                <div class="row">
                    <div class="txtfld-col">
                        <label for="Leave_label" class="required">Label <span class="required">*</span></label>
                        <asp:TextBox ID="tbx_leavelable" runat="server" AutoCompleteType="Disabled" MaxLength="120"></asp:TextBox>
                    </div>    
                    <div class="txtfld-col">
                    <label for="Event_colortype" class="required">Color Code<span class="required">*</span></label>
                    <asp:TextBox ID="tbx_Event_colortype" runat="server" AutoCompleteType="Disabled" MaxLength="120"></asp:TextBox>
                        <button class="jscolor  
                            {valueElement:'ContentPlaceHolder1_ContentPlaceHolder1_tbx_Event_colortype', styleElement:'styleInput'}"
                            style="height: 22px;width: 22px;background: url(images/trigger.png) center no-repeat;vertical-align: middle; margin: 0 .25em;display: inline-block;" id="styleInput"></button>
                    </div>   
                </div>
               
                 <div class="row">
                     <div class="txtfld-col">
                        <label for="Leave_exclude" class="required">Exclude in Attendance % calculation <span class="required">*</span></label>
                        <asp:CheckBox ID="chk_exclude" runat="server"/>
                    </div>    
                    <div class="txtfld-col">
                        <label for="Leave_status" class="required">Status <span class="required">*</span></label>
                        <asp:DropDownList ID="ddl_leave_status" runat="server">
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
                        <button id="btn_leavetype" runat="server" style="width:150px"  onserverclick="btn_leavetype_ServerClick" class="a_tag-btn"><span>Create Leave Type</span></button>
                    </li>
                </ul>
            </div>
        </div>

    </div>


    <div id="update_section" runat="server">

        <h4><b>Update Leave Type</b></h4>
        <div class="form">
            <div class="button-bg">
                <div class="top-hed-btn-left"></div>
                <div class="top-hed-btn-right">
                    <ul>
                        <li>
                            <a class="a_tag-btn" href="Adm_Student_Leave_Type.aspx?mode=add"><span>Add Leave Type</span></a>            </li>
                        <li>
                            <a class="a_tag-btn" href="Adm_Student_Leave_Type.aspx?mode=search"><span>Manage Leave Type</span></a>            </li>
                    </ul>
                </div>
            </div>


            <p class="note" id="edit_field_mandary" runat="server">Fields with <span class="required">*</span> are required.</p>
          <div class="formCon">
            <div class="formConInner">                
                <div class="row">
                    <div class="txtfld-col">
                        <label for="Leave_type" class="required">Name <span class="required">*</span></label>
                        <asp:TextBox ID="tbx_edit_leave_type" runat="server" AutoCompleteType="Disabled" MaxLength="120"></asp:TextBox>
                    </div>                                     
                    <div class="txtfld-col">
                        <label for="Leave_code" class="required">Code <span class="required">*</span></label>
                        <asp:TextBox ID="tbx_edit_leavecode" runat="server" AutoCompleteType="Disabled" MaxLength="120"></asp:TextBox>
                    </div>     
                </div>     
                <div class="row">
                    <div class="txtfld-col">
                        <label for="Leave_label" class="required">Label <span class="required">*</span></label>
                        <asp:TextBox ID="tbx_edit_leavelable" runat="server" AutoCompleteType="Disabled" MaxLength="120"></asp:TextBox>
                    </div>     
                    <div class="txtfld-col">
                    <label for="Event_colortype" class="required">Color Code<span class="required">*</span></label>
                    <asp:TextBox ID="tbx_edit_Event_colortype" runat="server" AutoCompleteType="Disabled" MaxLength="120"></asp:TextBox>
                         <button class="jscolor  
                            {valueElement:'ContentPlaceHolder1_ContentPlaceHolder1_tbx_edit_Event_colortype', styleElement:'styleInputEdit'}"
                            style="height: 22px;width: 22px;background: url(images/trigger.png) center no-repeat;vertical-align: middle; margin: 0 .25em;display: inline-block;" id="styleInputEdit"></button>

                    </div>   
                </div>
                 <div class="row">
                    <div class="txtfld-col">
                        <label for="Leave_exclude" class="required">Exclude in Attendance % calculation <span class="required">*</span></label>
                        <asp:CheckBox ID="chk_edit_exclude" runat="server"/>
                    </div>     
                    <div class="txtfld-col">
                        <label for="Leave_status" class="required">Status <span class="required">*</span></label>
                        <asp:DropDownList ID="ddl_edit_leave_status" runat="server">
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

   <div id="view_section" runat="server">

        <h4><b>View Leave Type</b></h4>

        <div class="button-bg">
            <div class="top-hed-btn-left"></div>
            <div class="top-hed-btn-right">
                <ul>
                    <li>
                        <button id="btn_edit" runat="server" onserverclick="btn_edit_ServerClick" class="a_tag-btn"><span>Edit</span></button>

                    </li>
                    <li>
                        <button id="btn_manage" runat="server" onserverclick="btn_manage_ServerClick" class="a_tag-btn"><span>Manage Leave Type</span></button>
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
                            <label id="lbl_eventname" runat="server"></label>

                        </td>
                    </tr>
                    <tr>
                        <td width="20%">
                            <b>Code</b>
                        </td>
                        <td width="3%"><strong>:</strong></td>
                        <td width="77%">
                            <label id="lbl_code" runat="server"></label>

                        </td>
                    </tr>
                     <tr>
                        <td width="20%">
                            <b>Label</b>
                        </td>
                        <td width="3%"><strong>:</strong></td>
                        <td width="77%">
                            <label id="lbl_label" runat="server"></label>

                        </td>
                    </tr>
                    <tr>
                        <td>
                            <b>Color Code</b>
                        </td>
                        <td><strong>:</strong></td>
                        <td>
                            <label id="lbl_colorcode" runat="server"></label>
                             <button disabled style="height: 22px;width: 22px;background: url(images/trigger.png) center no-repeat;vertical-align: middle; margin: 0 .25em;display: inline-block;" id="lbl_viewcolorcode" runat="server"></button>

                        </td>
                    </tr>
                    <tr>
                        <td>
                            <b>Exclude in Attendance % calculation</b>
                        </td>
                        <td><strong>:</strong></td>
                        <td>
                            <label id="lbl_exclude" runat="server"></label>
                        </td>
                    </tr>
                    
                    <tr>
                        <td>
                            <b>Status</b>
                        </td>
                        <td><strong>:</strong></td>
                        <td>
                            <label id="lbl_eventsts" runat="server"></label>
                        </td>
                    </tr>
                </tbody>
            </table>
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
        <button type="button" id="btn_del"  runat="server" onserverclick="btn_del_ServerClick" class="btn btn-primary">Yes</button>
      </div>
            </div>
        </div>
    </div>

</asp:Content>

