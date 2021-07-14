<%@ Page Title="" Language="C#" MasterPageFile="~/Adm_TimeTable.master" AutoEventWireup="true" CodeFile="Adm_Time_Table_SetTimeTable.aspx.cs" Inherits="Adm_Time_Table_SetTimeTable" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
     <div class="cont_right">
        <h1><b>Set Time Table</b></h1>
        <div class="formCon">
            <div class="formConInner">
                <div class="txtfld-col-box">                   
                    <div class="txtfld-col">
                                Select Course
                        <asp:DropDownList ID="drp_course" runat="server" AutoPostBack="true" OnSelectedIndexChanged="drp_course_SelectedIndexChanged"></asp:DropDownList>
                            </div>
                            <div class="txtfld-col">
                                Select Batch
                        <asp:DropDownList ID="drp_batch" runat="server" AutoPostBack="true" OnSelectedIndexChanged="drp_batch_SelectedIndexChanged"></asp:DropDownList>
                            </div>
                            <div class="txtfld-col">
                                Select Semester								                        
                        <asp:DropDownList ID="drp_semester" runat="server" AutoPostBack="true" OnSelectedIndexChanged="drp_semester_SelectedIndexChanged"></asp:DropDownList>
                            </div>                                
                    
                </div>
                <div class="text-fild-block-full">
                </div>
            </div>
        </div>
        <div class="button-bg">
            <div class="top-hed-btn-left"></div>
            <div class="top-hed-btn-right">
                <ul>
                    <li><a target="_blank" class="a-add-btn" href="Adm_Time_Table_WeekDay.aspx">Set Week Days</a></li>
                    <li><a target="_blank" class="a-add-btn" href="Adm_Time_Table_Classtiming.aspx">Set Class Timings</a></li>
                </ul>
            </div>
        </div>
        <div class="pdtab_Con">
           
           
        </div>
    </div>
</asp:Content>

