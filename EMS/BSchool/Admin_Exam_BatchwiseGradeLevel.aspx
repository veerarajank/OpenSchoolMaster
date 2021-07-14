<%@ Page Title="" Language="C#" MasterPageFile="~/Adm_Examination.master" AutoEventWireup="true" CodeFile="Admin_Exam_BatchwiseGradeLevel.aspx.cs" Inherits="Admin_Exam_BatchwiseGradeLevel" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="cont_right formWrapper">
                <h1>Set Grading Levels</h1>
                
<div class="button-bg">
<div class="top-hed-btn-left"> </div>
<div class="top-hed-btn-right">
<ul>                                    
                            <li>
                            <a class="a_tag-btn" href="/index.php?r=examination/exam&amp;id=5"><span>Assessments</span></a>                            </li>                                  
</ul>
</div> 

</div>

                <!--<div class="searchbx_area">
                <div class="searchbx_cntnt">
                <ul>
                <li><a href="#"><img src="images/search_icon.png" width="46" height="43" /></a></li>
                <li><input class="textfieldcntnt"  name="" type="text" /></li>
                </ul>
                </div>
                
                </div>-->
                
                
                
                <!--<div class="edit_bttns">
                <ul>
                <li>
                <a class=" edit last" href="#">Edit</a>    </li>
                </ul>
                </div>-->
                
                                
                 
                
                <div class="clear"></div>
                <div class="emp_right_contner">
                    <div class="emp_tabwrapper">
						
<div class="button-bg">
<div class="top-hed-btn-left"> </div>
<div class="top-hed-btn-right">
<ul>                                    

<li> </li>
<li>
    <a id="explorer_change" class="a_tag-btn" style="right:80px;" href="#">Change Batch</a>       
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
                    <td>
                        Bachelor Degree                    </td>
                                        <td>
                        First Semester                    </td>
                                            <td>
                        B.Sc - Chemistry                    </td>
					                                            
            </tr>
            
            
        </tbody>	
    </table>
    </div>

    
    <div class="exam-table-blk">
<table border="0" cellpadding="0" cellspacing="0p" width="100%">
    <thead>
        <tr>
            <td>
                
                                            </td>
        </tr> 
    </thead>
</table>
</div>	
    </div>
    </div>

<br>




<script>
    function newSubject() {
        var exam_id = $('#new_exam_id').val();
        if (exam_id == '')
            exam_id = 0;
        window.location.href = "/index.php?r=examination/gradingLevels/create" + '&examid=' + exam_id + '&id=' + 5;
    }
</script>                        <div class="clear"></div>
                        <div class="emp_cntntbx" style="padding-top:0px;">
                        	                            <div>
                                <div> 
                                    <h3>Grading Levels</h3>
                                   <div class="button-bg">
<div class="top-hed-btn-left">

                                    	<a id="add_grading-levels" class="cbut" href="/index.php?r=examination/gradingLevels/#">Create Grading Levels</a>                                        <a class="cbut" href="/index.php?r=examination/gradingLevels/default&amp;id=5" id="yt0">Set Default Grading Levels</a>   </div>
                                     <div class="top-hed-btn-right"></div>
                                   </div>
                                    <div id="success_flash" align="center" style=" color:#F00; display:none;">
	                                    <h4>Selected Grading Level Deleted Successfully!</h4>    
                                    </div>
                                    
                                                                        <div class="grid-view clear" id="grading-levels-grid">
<div class="summary">Displaying 1-6 of 6 result(s).</div>
<table class="items">
<thead>
<tr>
<th id="grading-levels-grid_c0"><a href="/index.php?r=examination/gradingLevels/index&amp;id=5&amp;GradingLevels_sort=name">Name</a></th><th id="grading-levels-grid_c1"><a href="/index.php?r=examination/gradingLevels/index&amp;id=5&amp;GradingLevels_sort=min_score">Min Score</a></th><th style="color:#FF6600" id="grading-levels-grid_c2">Actions</th></tr>
</thead>
<tbody>
<tr class="odd"><td>A</td><td>90</td><td class="button-column"><a class="fan_view" title="View" href="16"><img src="/js_plugins/ajaxform/images/icons/properties.png" alt="View"></a><a class="fan_update" title="Update" href="16"><img src="/js_plugins/ajaxform/images/icons/pencil.png" alt="Update"></a><a class="fan_del" title="Delete" href="16"><img src="/js_plugins/ajaxform/images/icons/cross.png" alt="Delete"></a></td></tr>
<tr class="even"><td>B</td><td>80</td><td class="button-column"><a class="fan_view" title="View" href="17"><img src="/js_plugins/ajaxform/images/icons/properties.png" alt="View"></a><a class="fan_update" title="Update" href="17"><img src="/js_plugins/ajaxform/images/icons/pencil.png" alt="Update"></a><a class="fan_del" title="Delete" href="17"><img src="/js_plugins/ajaxform/images/icons/cross.png" alt="Delete"></a></td></tr>
<tr class="odd"><td>C</td><td>70</td><td class="button-column"><a class="fan_view" title="View" href="18"><img src="/js_plugins/ajaxform/images/icons/properties.png" alt="View"></a><a class="fan_update" title="Update" href="18"><img src="/js_plugins/ajaxform/images/icons/pencil.png" alt="Update"></a><a class="fan_del" title="Delete" href="18"><img src="/js_plugins/ajaxform/images/icons/cross.png" alt="Delete"></a></td></tr>
<tr class="even"><td>D</td><td>60</td><td class="button-column"><a class="fan_view" title="View" href="19"><img src="/js_plugins/ajaxform/images/icons/properties.png" alt="View"></a><a class="fan_update" title="Update" href="19"><img src="/js_plugins/ajaxform/images/icons/pencil.png" alt="Update"></a><a class="fan_del" title="Delete" href="19"><img src="/js_plugins/ajaxform/images/icons/cross.png" alt="Delete"></a></td></tr>
<tr class="odd"><td>E</td><td>50</td><td class="button-column"><a class="fan_view" title="View" href="20"><img src="/js_plugins/ajaxform/images/icons/properties.png" alt="View"></a><a class="fan_update" title="Update" href="20"><img src="/js_plugins/ajaxform/images/icons/pencil.png" alt="Update"></a><a class="fan_del" title="Delete" href="20"><img src="/js_plugins/ajaxform/images/icons/cross.png" alt="Delete"></a></td></tr>
<tr class="even"><td>F</td><td>40</td><td class="button-column"><a class="fan_view" title="View" href="21"><img src="/js_plugins/ajaxform/images/icons/properties.png" alt="View"></a><a class="fan_update" title="Update" href="21"><img src="/js_plugins/ajaxform/images/icons/pencil.png" alt="Update"></a><a class="fan_del" title="Delete" href="21"><img src="/js_plugins/ajaxform/images/icons/cross.png" alt="Delete"></a></td></tr>
</tbody>
</table>
<div class="keys" style="display:none" title="/index.php?r=examination/gradingLevels&amp;id=5"><span>16</span><span>17</span><span>18</span><span>19</span><span>20</span><span>21</span></div>
</div>                                </div>
                            </div>
                        </div> <!-- END div class="emp_cntntbx" -->
                    </div> <!-- END div class="emp_tabwrapper" -->
                </div> <!-- END div class="emp_right_contner" -->
            </div>


</asp:Content>

