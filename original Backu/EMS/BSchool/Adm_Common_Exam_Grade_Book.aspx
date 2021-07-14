<%@ Page Title="" Language="C#" MasterPageFile="~/Adm_Examination.master" AutoEventWireup="true" CodeFile="Adm_Common_Exam_Grade_Book.aspx.cs" Inherits="Adm_Common_Exam_Grade_Book" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
       <div class="cont_right">
        <h1>Grade Book</h1>
        <div class="formCon">
            <div class="formConInner">
                <div class="txtfld-col-box">
                    <div class="txtfld-col">
                        Exam
                        <asp:DropDownList ID="drp_exam" runat="server"></asp:DropDownList>
                    </div>
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
                 <div class="txtfld-col">
                     <br />
                        <button id="btn_getresult" runat="server" onserverclick="btn_getresult_ServerClick" class="a-add-btn" style="width:150px"><span><i class="fa fa-book"></i>&nbsp;Get Result</span></button>
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
                    <li><a target="_blank" class="excel_but-input" href="#">Generate Excel</a></li>
                    <li><a target="_blank" class="pdf_but-input" href="#">Generate PDF</a></li>
                </ul>
            </div>
        </div>
        <div class="pdtab_Con">
            <div style="font-size: 13px; padding: 5px 0px"><strong>Exam Results & Scores</strong></div>
            <div class="table-scroll">
                <div class="table-scroll-width">

                    
                </div>
            </div>
        </div>
    </div>
</asp:Content>

