<%@ Page Title="" Language="C#" MasterPageFile="~/Adm_Examination.master" AutoEventWireup="true" CodeFile="Adm_Exam_Batch.aspx.cs" Inherits="Adm_Exam_Batch" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <h1>Exams</h1>
    <div class="button-bg">
        <div class="top-hed-btn-left"></div>
        <div class="top-hed-btn-right">
            <ul>
                <li>
                    <a id="add_exam-groups" class="a_tag-btn" href="Adm_Common_Exam.aspx?mode=add"><span>Create Exam</span></a>              </li>
                <li><a class="a_tag-btn" href="/index.php?r=examination/gradingLevels&amp;id=5"><span>Grading Levels</span></a> </li>
            </ul>
        </div>
    </div>

    <div class="button-bg">
        <div class="top-hed-btn-left"></div>
        <div class="top-hed-btn-right">
            <ul>

                <li></li>
                <li>
                    <a id="explorer_change" class="a_tag-btn" style="right: 80px;" href="#">Change Batch</a>
                </li>

                <li><a class="sb_but_close-atndnce" href="/index.php?r=examination"><span>close</span></a></li>
            </ul>
        </div>

    </div>

    <div class="clear"></div>

    <div class="formCon">
        <div class="attnd-tab-inner-blk">
            <div class="exam-table">
                <table border="0" cellpadding="0" cellspacing="0p" width="100%">
                    <thead>
                        <tr>
                            <th class="course-icon">Course</th>
                            <th class="semester-icon">Semester </th>
                            <th class="batch-icon">Batch</th>


                        </tr>
                    </thead>

                    <tbody>
                        <tr>
                            <td>Bachelor Degree                    </td>
                            <td>First Semester                    </td>
                            <td>B.Sc - Chemistry                    </td>

                        </tr>


                    </tbody>
                </table>
            </div>


            <div class="exam-table-blk">
                <table border="0" cellpadding="0" cellspacing="0p" width="100%">
                    <thead>
                        <tr>
                            <td></td>
                        </tr>
                    </thead>
                </table>
            </div>
        </div>
    </div>

    <br>




  
    <div class="clear"></div>
    <div class="emp_right_contner">
        <div class="emp_tabwrapper">
            <div class="clear"></div>
            <div>
                <div class="grid-view clear" id="exam-groups-grid">
                    <div class="summary">Displaying 1-1 of 1 result(s).</div>
                    <table class="items">
                        <thead>
                            <tr>
                                <th style="color: #FF6600" id="exam-groups-grid_c0">Name</th>
                                <th id="exam-groups-grid_c1"><a href="/index.php?r=examination/exam/index&amp;id=5&amp;ExamGroups_sort=exam_type">Exam Type</a></th>
                                <th id="exam-groups-grid_c2"><a href="/index.php?r=examination/exam/index&amp;id=5&amp;ExamGroups_sort=is_published">Date Is Published</a></th>
                                <th id="exam-groups-grid_c3"><a href="/index.php?r=examination/exam/index&amp;id=5&amp;ExamGroups_sort=result_published">Result Published</a></th>
                                <th style="font-size: 12px; font-weight: bold;" id="exam-groups-grid_c4">Action</th>
                                <th style="color: #FF6600" id="exam-groups-grid_c5">Manage</th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr class="odd">
                                <td class="link-column"><a href="/index.php?r=examination/exams/create&amp;exam_group_id=1&amp;id=5">Internal Exam</a></td>
                                <td>Marks&nbsp;</td>
                                <td>Yes</td>
                                <td>No</td>
                                <td class="button-column"><a class="fan_view" title="View" href="1">
                                    <img src="/js_plugins/ajaxform/images/icons/properties.png" alt="View"></a><a class="fan_update" title="Update" href="1"><img src="/js_plugins/ajaxform/images/icons/pencil.png" alt="Update"></a><a class="fan_del" title="Delete" href="1"><img src="/js_plugins/ajaxform/images/icons/cross.png" alt="Delete"></a></td>
                                <td style="width: 17%"><a title="Manage This Exam" href="/index.php?r=examination/exams/create&amp;exam_group_id=1&amp;id=5">Manage This Exam</a><a class="res_pub" title=" / Publish Result" href="/index.php?r=examination/exam/publishresult&amp;exam_group_id=1&amp;id=5"> / Publish Result</a></td>
                            </tr>
                        </tbody>
                    </table>
                    <div class="keys" style="display: none" title="/index.php?r=examination/exam&amp;id=5"><span>1</span></div>
                </div>
            </div>
        </div>
        <!-- END div class="emp_tabwrapper" -->
    </div>
    <!-- END div class="emp_right_contner" -->
   
      
</asp:Content>

