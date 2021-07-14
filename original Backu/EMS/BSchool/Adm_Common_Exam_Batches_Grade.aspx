<%@ Page Title="" Language="C#" MasterPageFile="~/Adm_Examination.master" AutoEventWireup="true" CodeFile="Adm_Common_Exam_Batches_Grade.aspx.cs" Inherits="Adm_Common_Exam_Batches_Grade" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="cont_right">
        <h1>Set Grading Levels</h1>
        <div class="formCon">
            <div class="formConInner">
                <div class="txtfld-col-box">                   
                    <div class="txtfld-col">
                        Select Course
                        <asp:DropDownList ID="drp_course" runat="server"></asp:DropDownList>
                    </div>
                    <div class="txtfld-col">
                        Select Batch
                        <asp:DropDownList ID="drp_batch" runat="server"></asp:DropDownList>
                    </div>
                    <div class="txtfld-col">
                        Select Semester								                        
                        <asp:DropDownList ID="drp_semester" runat="server"></asp:DropDownList>
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
                    <li><a target="_blank" class="cbut" href="#">Create Grade Level</a></li>
                    <li><a target="_blank" class="cbut" href="#">Set Default Grading Levels</a></li>
                </ul>
            </div>
        </div>
        <div class="pdtab_Con">
           
           
        </div>
    </div>
</asp:Content>

