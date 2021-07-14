<%@ Page Title="" Language="C#" MasterPageFile="~/Admin_MyAccount.master" AutoEventWireup="true" CodeFile="Adm_My_Event_List.aspx.cs" Inherits="Adm_My_Event_List" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="cont_right os-left-block">
        <h1>Events</h1>


        <div id="academic-years-grid" class="grid-view">

            
                <div id="content">
                    <div class="events_con">
                        <asp:ListView ID="lv_listview" runat="server" GroupPlaceholderID="groupPlaceHolder1"
                            ItemPlaceholderID="itemPlaceHolder1">
                            <LayoutTemplate>
                               <table width="100%" cellspacing="0" cellpadding="0">

                                    <asp:PlaceHolder runat="server" ID="groupPlaceHolder1"></asp:PlaceHolder>

                                </table>
                            </LayoutTemplate>
                            <GroupTemplate>
                                <tr class="read">
                                    <asp:PlaceHolder runat="server" ID="itemPlaceHolder1"></asp:PlaceHolder>
                                </tr>
                            </GroupTemplate>
                            <ItemTemplate>
                                <td width="5%" valign="top">
                                    <div class="stripbx" style="position: relative;">
                                        <div class="strip-clr" style="background-color: #f74b11"></div>
                                        <%# Eval("dayvalue") %><span><%# Eval("monthvalue") %></span>
                                    </div>
                                </td>
                                <td>
                                    <div class="hdng_events"><a id="showJobDialog113" class="add" href="#"><%# Eval("Event_title") %></a></div>
                                    <div style="width: 580px;">
                                        <%# Eval("Event_desc") %>
                                    </div>
                                    <div id="jobDialog"></div>
                                </td>
                                <td align="left" class="date" style="width: 60px;">
                                    <span class="date"><%# Eval("Event_start") %>   </span>
                                </td>









                            </ItemTemplate>
                            <EmptyDataTemplate>
                            </EmptyDataTemplate>
                        </asp:ListView>
                    </div>

                </div>
            </div>


    </div>
</asp:Content>

