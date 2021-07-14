<%@ Page Title="" Language="C#" MasterPageFile="~/Adm_Examination.master" AutoEventWireup="true" CodeFile="Adm_Common_Exam_Subject.aspx.cs" Inherits="Adm_Common_Exam_Subject" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

    <div class="cont_right formWrapper">
        <h1><b>Scheduled Subject</b></h1>
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
                    <asp:ListView ID="lv_subjects" runat="server" GroupPlaceholderID="groupPlaceHolder1"
                        ItemPlaceholderID="itemPlaceHolder1">
                        <LayoutTemplate>
                            <table class="table table-bordered table-responsive table-hover dataTable" style="border: 2px solid #d2d3d4!important;">
                                <tr class="bg-blue">
                                    <th>Subject
                                    </th>
                                    <th>Maximum Mark
                                    </th>
                                    <th>Minimum Mark
                                    </th>
                                    <th>Exam Date
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
                                <%# Eval("subjectname") %>
                            </td>
                            <td>
                               <asp:TextBox ID="tbx_maximummark" runat="server" Text='<%# Convert.ToString(Eval("max_mark"))%>' CssClass="form-control"></asp:TextBox>
                            </td>
                            <td>
                              <asp:TextBox ID="tbx_minimummark" runat="server" Text='<%# Convert.ToString(Eval("min_mark"))%>' CssClass="form-control"></asp:TextBox>
                                   <asp:HiddenField ID="hd_examsubjectlistid" runat="server" Value='<%# Convert.ToString(Eval("id"))%>' />

                            </td>
                            <td>
                              <asp:TextBox ID="tbx_examdate"  Text='<%# Convert.ToString(Eval("exam_dateddMMyyy"))%>' runat="server" CssClass="form-control hasdate"></asp:TextBox>
                            </td>                                                       

                        </ItemTemplate>
                        <EmptyDataTemplate>
                           <table class="table table-bordered table-responsive table-hover dataTable" style="border: 2px solid #d2d3d4!important;">
                                <tr class="bg-blue">
                                    <th>Subject
                                    </th>
                                    <th>Maximum Mark
                                    </th>
                                    <th>Minimum Mark
                                    </th>
                                    <th>Exam Date
                                    </th>                                    
                                </tr>
                                <tr>
                                    <td colspan="4">No Subject Found
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

      <script src="plugins/jQuery/jquery-2.2.3.min.js"></script>

    <script type="text/javascript">
        $(function () {
            $('.hasdate').datepicker({ autoclose: true });
          
        });
    </script>


</asp:Content>

