<%@ Page Title="" Language="C#" MasterPageFile="~/Adm_Teacher_Settings.master" AutoEventWireup="true" CodeFile="TeacherSubjectMapping.aspx.cs" Inherits="TeacherSubjectMapping" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script src="plugins/jQuery/jquery-2.2.3.min.js"></script>
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
            min-height: 30px;
        }

        .row {
            margin-bottom: 0px !important;
        }

        select {
            font-size: 12px;
        }
    </style>

    <div class="cont_right formWrapper">
        <h1>Subject Association</h1>
        <div class="formCon">
            <div class="formConInner">
                <div class="c_batch_tbar-subwise">
                    <div class="filter-box-table">
                        <table style="width: 100%" border="0" cellspacing="0" cellpadding="0" class="s_search">
                            <tbody>
                                <tr>
                                    <td style="width: 13%"><strong>Academic Year</strong></td>
                                    <td style="width: 2%"></td>
                                    <td style="width: 25%">
                                        <asp:DropDownList ID="drp_academic_year" Font-Size="10" runat="server" AutoPostBack="true" OnSelectedIndexChanged="drp_academic_year_SelectedIndexChanged" class="form-control chosen-select">
                                        </asp:DropDownList>
                                    </td>
                                    <td style="width: 2%"></td>
                                    <td style="width: 23%"></td>
                                </tr>
                                <tr>
                                    <td colspan="5" style="height: 3px"></td>
                                </tr>
                                <tr>
                                    <td style="width: 13%"><strong>Course Name</strong></td>
                                    <td style="width: 2%">:</td>
                                    <td style="width: 25%">
                                        <asp:DropDownList ID="drp_course" Font-Size="10" runat="server" AutoPostBack="true" OnSelectedIndexChanged="drp_course_SelectedIndexChanged" class="form-control chosen-select">
                                        </asp:DropDownList>
                                    </td>
                                    <td style="width: 2%"></td>
                                    <td style="width: 23%"></td>
                                </tr>
                                <tr>
                                    <td style="width: 13%"><strong>Semester</strong></td>
                                    <td style="width: 2%">:</td>
                                    <td style="width: 25%">
                                        <asp:DropDownList ID="drp_semester" Font-Size="10" runat="server" AutoPostBack="true" OnSelectedIndexChanged="drp_semester_SelectedIndexChanged" class="form-control chosen-select">
                                        </asp:DropDownList>
                                    </td>
                                    <td style="width: 2%"></td>
                                    <td style="width: 23%"></td>
                                </tr>
                                <tr>
                                    <td colspan="5" style="height: 3px"></td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <div runat="server" id="search_section">
        <div class="cont_right formWrapper">


            <div class="formCon">
                <div class="formConInner">
                    <div class="two-Inputarea">
                        <span>Department</span>&nbsp;&nbsp;&nbsp;&nbsp;
                         <asp:DropDownList ID="drp_dept" Font-Size="10" runat="server" AutoPostBack="true" OnSelectedIndexChanged="drp_dept_SelectedIndexChanged" class="form-control chosen-select">
                         </asp:DropDownList>
                    </div>
                    <div class="two-Inputarea">
                        <span>Teacher</span>&nbsp;&nbsp;        
                        <asp:DropDownList ID="drp_teacher" Font-Size="10" runat="server"  class="form-control chosen-select">
                         </asp:DropDownList>
                    </div>
                    <div class="two-Inputarea">
                        <span>Subject</span>&nbsp;&nbsp;  
                         <asp:DropDownList ID="drp_subject" Font-Size="10" runat="server"  class="form-control chosen-select">
                         </asp:DropDownList>      
                    </div>
                    <div class="two-Inputarea">
                       <br /> &nbsp;&nbsp;  
                        <asp:Button ID="btn_assign" runat="server" Text="Assign" class="a_tag-btn" Width="150px" OnClick="btn_assign_Click" />
                    </div>
                    <div class="clear"></div>
                </div>
            </div>

            <div runat="server" id="Div1">
                <div id="academic-years-grid" class="grid-view">

                    <asp:ListView ID="lv_subjectmapping" runat="server" GroupPlaceholderID="groupPlaceHolder1"
                        ItemPlaceholderID="itemPlaceHolder1">
                        <LayoutTemplate>
                            <table class="items" style="border: 1px solid #b9c7d0!important;">
                                <thead style="border: 1px solid #b9c7d0!important;">
                                    <th>Subject
                                    </th>
                                    <th>Teacher Name
                                    </th>
                                    <th>Department
                                    </th>
                                    <th class="col-md-3">Action
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
                                <%# Eval("name") %>
                            </td>

                            <td>
                                <%# Eval("Teacher") %>
                            </td>
                            <td>
                                <%# Eval("Department") %>
                            </td>
                            <td>





                                <asp:LinkButton ID="lnk_subject" CommandName="subject" CommandArgument='<%# Eval("academic_subject_id") %>' OnCommand="lnk_subject_Command" runat="server" Visible='<%# Convert.ToBoolean(Session["Access_Edit"])==true && Convert.ToString(Eval("teacher_id"))!="0"    %>'>
                                                     <span style="color: #F60;"> Remove</span>
                                </asp:LinkButton>&nbsp;


                            </td>

                        </ItemTemplate>
                        <EmptyDataTemplate>
                            <table class="items" style="border: 1px solid #b9c7d0!important;">
                                <thead style="border: 1px solid #b9c7d0!important;">

                                    <th>Subject
                                    </th>
                                    <th>Teacher Name
                                    </th>
                                    <th>Department
                                    </th>
                                    <th class="col-md-3">Action
                                    </th>
                                </thead>
                                <tr style="border: 1px solid #b9c7d0!important;">
                                    <td colspan="4">No Record Found
                                    </td>
                                </tr>
                            </table>
                        </EmptyDataTemplate>
                    </asp:ListView>

                    <br />
                    <br />
                </div>
            </div>
        </div>
    </div>

















</asp:Content>

