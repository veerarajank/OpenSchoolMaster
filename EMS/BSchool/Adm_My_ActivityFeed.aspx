<%@ Page Title="" Language="C#" MasterPageFile="~/Admin_MyAccount.master" AutoEventWireup="true" CodeFile="Adm_My_ActivityFeed.aspx.cs" Inherits="Adm_My_ActivityFeed" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <style>
        .a_feed_innercntnt h3 {
    color: #666666;
    font-size: 13px;
    font-weight: 400;
    margin: 0px;
    padding: 0px;
    line-height: 21px;
}
    </style>
    <div class="cont_right formWrapper">
        <h1>Activity Feed</h1>
        <div class="search_but"><i class="fa fa-search"></i></div>

        <div id="act_search_con" class="formCon" style="border: none; padding: 0px; display: none;">

            <div class="formCon">
                <div class="formConInner opnsl_form_label">

                    <div style="display: none">
                        <input type="hidden" value="activityFeed/index" name="r">
                    </div>
                    <div class="txtfld-col-bg">
                        <div class="txtfld-col">
                            <label>Feed Type</label>
                            <select name="activity_type" id="activity_type">
                                <option value="">Select</option>
                                <option value="29">Database Backup Creation</option>
                                <option value="31">Database Backup Deletion</option>
                                <option value="32">Database Backup Download</option>
                                <option value="30">Database Backup Restore</option>
                                <option value="33">Database Backup Upload</option>
                                <option value="26">Employee Attendance Creation</option>
                                <option value="28">Employee Attendance Deletion</option>
                                <option value="27">Employee Attendance Update</option>
                                <option value="23">Employee Creation</option>
                                <option value="25">Employee Deletion</option>
                                <option value="24">Employee Update</option>
                                <option value="17">Exam Creation</option>
                                <option value="19">Exam Deletion</option>
                                <option value="11">Exam Group Creation</option>
                                <option value="13">Exam Group Deletion</option>
                                <option value="12">Exam Group Update</option>
                                <option value="20">Exam Score Creation</option>
                                <option value="22">Exam Score Deletion</option>
                                <option value="21">Exam Score Update</option>
                                <option value="18">Exam Update</option>
                                <option value="16">Guardian Deletion</option>
                                <option value="15">Guardian Update</option>
                                <option value="14">Guaridan Creation</option>
                                <option value="1">Log In</option>
                                <option value="2">Log Out</option>
                                <option value="6">Student Activation</option>
                                <option value="8">Student Attendance Creation</option>
                                <option value="10">Student Attendance Deletion</option>
                                <option value="9">Student Attendance Update</option>
                                <option value="3">Student Creation</option>
                                <option value="7">Student Deletion</option>
                                <option value="5">Student Inactivation</option>
                                <option value="4">Student Update</option>
                            </select>
                        </div>
                        <div class="txtfld-col">
                            <label>Start date</label><input id="start_date" name="start_date" type="text" class="hasDatepicker">
                        </div>
                        <div class="txtfld-col">
                            <label>End Date</label>
                            <input id="end_date" name="end_date" type="text" class="hasDatepicker">
                        </div>
                    </div>
                    <div class="txtfld-col-bg">
                        <div class="os_btn">
                            <div class="div_1"></div>
                            <div class="div_1">
                                <input name="find" type="submit" value="Find">
                            </div>
                        </div>
                    </div>





                </div>
            </div>
        </div>
        <!-- END Activity Feed Search -->



        <div class="a_feed_cntnr">

            <div class="a_feed_bx" id="feed_content_box">




                <asp:ListView ID="lv_listview" runat="server" GroupPlaceholderID="groupPlaceHolder1"
                    ItemPlaceholderID="itemPlaceHolder1">
                    <LayoutTemplate>


                        <asp:PlaceHolder runat="server" ID="groupPlaceHolder1"></asp:PlaceHolder>


                    </LayoutTemplate>
                    <GroupTemplate>

                        <asp:PlaceHolder runat="server" ID="itemPlaceHolder1"></asp:PlaceHolder>

                    </GroupTemplate>
                    <ItemTemplate>





                        <div class="individual_feed">
                            <!-- Individual Feed -->
                            <div class="a_feed_innerbx">
                                <div class="a_feed_create a_feed"></div>
                                <div class="a_feed_innercntnt">
                                    <div class="a_feed_inner_arrow"></div>
                                    <h3>
                                        <strong>
                                            <a href="#"><%# Eval("initiator") %></a></strong>


                                        <%# Eval("goal_name") %>											<strong>
                                            <a href="#"><%# Eval("field_name") %></a>												</strong>
                                    </h3>
                                    <p>at <strong><%# Eval("val_activity_time") %></strong>  - <strong><%# Eval("activity_date") %></strong>.</p>
                                    <p><strong>User Role       : </strong><%# Eval("user_role") %></p>
                                    <p><strong>User IP address : </strong><%# Eval("system_ip") %></p>
                                    <div class="clear"></div>
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



</asp:Content>

