<%@ Page Title="" Language="C#" MasterPageFile="~/Adm_TimeTable.master" AutoEventWireup="true" CodeFile="TimeTableMain.aspx.cs" Inherits="TimeTableMain" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
     <div id="content">
    <div class="yellow_bx yb_timetable" style="width:100%">
                	<div class="y_bx_head">
                    	Before creating the time table make sure you follow the following instructions                    </div>
                	<div class="y_bx_list timetable_list">
                    	<h1>Set Weekdays</h1>
                        <p>Set the weekdays, where the specific Batch has classes, You can use the school default or custom weekdays.</p>
                    </div>
                    <div class="y_bx_list timetable_list">
                    	<h1>Set Class Timings</h1>
                        <p>Create class timings for each Batch Enter each period start time and end time,Add break timings etc.</p>
                    </div>
                    <div class="y_bx_list timetable_list">
                    	<h1>Subjects &amp; Subject Allocation</h1>
                        <p>Add existing subjects to the Batch or create a new subject. Associate each subject with the teacher.</p>
                    </div>
                    <div class="y_bx_list timetable_list">
                    	<h1>Create Timetable</h1>
                        <p>Assigning each timing/period from the dropdown.</p>
                    </div>
    			</div>
         </div>
</asp:Content>

