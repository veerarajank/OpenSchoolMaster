<%@ Page Title="" Language="C#" MasterPageFile="~/GS247.master" AutoEventWireup="true" CodeFile="AdminDashboard.aspx.cs" Inherits="AdminDashboard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script src="plugins/bootstrap.min.js"></script>
    <script src="plugins/jquery-1.11.1.min.js"></script>
    <script src="plugins/process_snippets.js"></script>
    <script src="Chartjs/Chart.js"></script>
    <script src="Chartjs/utils.js"></script>
      <link href="EventCal/tab.css" rel="stylesheet" type="text/css" />
    <script src="EventCal/tabulous.js"></script>
    <script src="EventCal/dash_tab.js"></script>
    <script type="text/javascript">

        function dynamicColors(i) {

            if (i == 0) return "rgb(255,0,0)";
            else if (i == 1) return "rgb(128,0,128)";
            else if (i == 2) return "rgb(0,0,255)";
            else if (i == 3) return "rgb(0,255,255)";
            else if (i == 4) return "rgb(255,0,255)";
            else if (i == 5) return "rgb(255,165,0)";
            else if (i == 6) return "rgb(107,142,35)";
            else if (i == 7) return "rgb(218,165,32)";
            else if (i == 8) return "rgb(0,139,139)";
            else if (i == 9) return "rgb(148,0,211)";
            else if (i == 10) return "rgb(139,69,19)";
            else {

                var r = Math.floor(Math.random() * 255);
                var g = Math.floor(Math.random() * 255);
                var b = Math.floor(Math.random() * 255);
                return "rgb(" + r + "," + g + "," + b + ")";
            }
        }

    </script>


    <div class="dashboard_bg ui-sortable" id="dashboard_block">
        <div class="clear"></div>

        <div class="os_dash_box block_class active_class" block-id="1" order-id="1">
            <div class="dash_subhed">
                <div class="dash_subhed_hide_one news">
                    News               
                </div>
                <div class="dash_subhed_hide_two" title="Hide this block" id="1"><i class="fa fa-times" aria-hidden="true"></i></div>
            </div>

            <div class="Default dash_box1_iner-pdng dash_bord-scrol left-icon dash_box-pading ps-container">
                <div id="div_news" runat="server">
                </div>

                <div class="ps-scrollbar-x-rail" style="width: 280px; display: inherit; left: 0px;">
                    <div class="ps-scrollbar-x" style="left: 0px; width: 254px;"></div>
                </div>
                <div class="ps-scrollbar-y-rail" style="top: 0px; height: 202px; display: inherit; right: 3px;">
                    <div class="ps-scrollbar-y" style="top: 0px; height: 177px;"></div>
                </div>
            </div>

            <div class="dash_bottom dashboard-action-blk">
                <ul>
                    <li><a href="Adm_My_News.aspx">View</a></li>

                    <li><a href="Adm_My_News.aspx?mode=add">Create</a></li>

                </ul>
            </div>
        </div>


        <div class="os_dash_box block_class active_class" block-id="2" order-id="2">
            <div class="dash_subhed">
                <div class="dash_subhed_hide_one events">Events</div>
                <div class="dash_subhed_hide_two" title="Hide this block" id="2"><i class="fa fa-times" aria-hidden="true"></i></div>
            </div>
            <div class="dash_box-pading">
                <div id="tabs4" class="event-tab">


                    <ul id="tm23">
                        <li><a href="#tabs-1" title="" class="tabulous_active">Today</a></li>
                        <li><a href="#tabs-2" title="" class="">Current Week</a></li>
                        <li><a href="#tabs-3" title="" class="">Next Week</a></li>
                        <li><a href="#tabs-4" title="" class="">Next month</a></li>
                        <span class="tabulousclear"></span>
                    </ul>
                    <div id="tabs_container" class="mousescroll Default ps-container transition" style="height: 122px;">
                        <div id="tabs-1" style="position: absolute; top: 10px; right: 10px; left: 10px; height: 122px;" class="showflip make_transist">
                            <a id="my_tabs" class="make_transist showflip" runat="server" ></a>

                        </div>

                        <div id="tabs-2"  class="hideflip make_transist" style="position: absolute; top: 10px; right: 10px; left: 10px; height: 122px;">
                            <p class="mail_dashnew_error">No Upcoming Events This week</p>
                        </div>

                        <div id="tabs-3" class="hideflip make_transist" style="position: absolute; top: 10px; right: 10px; left: 10px; height: 122px;">
                            <p class="mail_dashnew_error">No Upcoming Events Next Week</p>
                        </div>

                        <div id="tabs-4"  class="hideflip make_transist" style="position: absolute; top: 10px; right: 10px; left: 10px; height: 122px;">
                            <p class="mail_dashnew_error">No Upcoming Events Next Month</p>
                        </div>
                        <div class="ps-scrollbar-x-rail" style="width: 250px; display: inherit; left: 0px;">
                            <div class="ps-scrollbar-x" style="left: 0px; width: 223px;"></div>
                        </div>
                        <div class="ps-scrollbar-y-rail" style="top: 0px; height: 122px; display: inherit; right: 3px;">
                            <div class="ps-scrollbar-y" style="top: 0px; height: 97px;"></div>
                        </div>
                    </div>


                </div>
            </div>
            <div class="dash_bottom dashboard-action-blk">
                <ul>

                    <li><a href="#">View</a></li>
                    <li><a href="#">Create</a></li>

                </ul>
            </div>
        </div>



        <div class="os_dash_box block_class active_class" block-id="3" order-id="3">
            <div class="dash_subhed">
                <div class="dash_subhed_hide_one  student">Student</div>
                <div class="dash_subhed_hide_two" title="Hide this block" id="3"><i class="fa fa-times" aria-hidden="true"></i></div>
            </div>
            <div class="dash_box-pading">
                <a href="#">
                    <div class="dash_st_grid st_grid_colr1" style="width: 30%">
                        <h3 id="lbl_studentcnt" runat="server">0</h3>
                        <p>Total</p>
                        <p>Student</p>

                    </div>
                </a>
                <a href="#">
                    <div class="dash_st_grid st_grid_colr2" style="width: 30%">
                        <h3 id="lbl_newstudentcnt" runat="server">0</h3>
                        <p>New</p>
                        <p>Admissions</p>
                    </div>
                </a>
                <a href="#">
                    <div class="dash_st_grid st_grid_colr3" style="width: 30%">
                        <h3 id="lbl_inactivestudentcnt" runat="server">0</h3>
                        <p>Inactive</p>
                        <p>Students</p>
                    </div>
                </a>
                <div class="clear-fix"></div>
                <div class="cv">
                    <div id="container" style="width: 100%; height: 555px; margin: 0 auto;">
                        <div style="border: 1px solid #e0e0e0">
                            <asp:Literal ID="L_ltChart_1" runat="server"></asp:Literal>
                            <input id="L_ht1" type="hidden" runat="server" />
                        </div>
                    </div>

                </div>
            </div>
            <div class="dash_bottom dashboard-action-blk">
                <ul>

                    <li><a href="Adm_Student_List.aspx">View</a></li>
                    <li><a href="Adm_StudentCreation.aspx">Create</a></li>
                </ul>
            </div>
        </div>



        <div class="os_dash_box block_class active_class" block-id="4" order-id="4">
            <div class="dash_subhed ">
                <div class="dash_subhed_hide_one  teacher">Teacher</div>
                <div class="dash_subhed_hide_two" title="Hide this block" id="4"><i class="fa fa-times" aria-hidden="true"></i></div>
            </div>
            <div class="dash_box-pading">
                <a href="#">
                    <div class="dash_st_grid2 t_grid_colr1">
                        <h3 runat="server" id="lbl_teachercnt">0</h3>
                        <p>Total Teachers</p>
                    </div>
                </a>
                <a href="#">
                    <div class="dash_st_grid2 st_grid_colr2">
                        <h3 id="lbl_newteachercnt" runat="server">0</h3>
                        <p>Recently Hired</p>
                    </div>
                </a>
            </div>
            <div class="dash_bottom dashboard-action-blk">
                <ul>

                    <li><a href="Adm_Teacher_List.aspx">View</a></li>
                    <li><a href="Adm_Teacher.aspx">Create</a></li>

                </ul>
            </div>
        </div>



        <div class="os_dash_box block_class active_class" block-id="5" order-id="5">
            <div class="dash_subhed">
                <div class="dash_subhed_hide_one examination">Examination</div>
                <div class="dash_subhed_hide_two" title="Hide this block" id="5"><i class="fa fa-times" aria-hidden="true"></i></div>

            </div>
            <div class="dash_box-pading">
                <div class="clear"></div>
                <div id="dg1" class="exm-graph" style="width: 100%; height: 100px;">

                    <asp:Literal ID="L_ltChart_2" runat="server"></asp:Literal>
                            <input id="L_ht2" type="hidden" runat="server" />

                </div>
                 <div id="dg2" class="exm-graph" style="width: 100%; height: 100px;">

                    <asp:Literal ID="L_ltChart_3" runat="server"></asp:Literal>
                            <input id="L_ht3" type="hidden" runat="server" />

                </div>
                <div class="dash_bottom dashboard-action-blk">
                    <ul>
                        <li><a href="#">View</a></li>
                    </ul>
                </div>
            </div>
        </div>





        <div class="os_dash_box block_class active_class" block-id="6" order-id="6">
            <div class="dash_subhed">
                <div class="dash_subhed_hide_one attendance">Attendance</div>
                <div class="dash_subhed_hide_two" title="Hide this block" id="6"><i class="fa fa-times" aria-hidden="true"></i></div>
            </div>
            <div class="dash_box-pading">
                <div class="dash_attnd_grid attnd_grid_colr1">
                    <a href="#">
                        <div class="innet_attnd-block1 innet_attnd-orang1">
                            <h3>Student</h3>
                            <p>Attendance</p>
                        </div>
                    </a>
                    <div class="innet_attnd-block2 innet_attnd-orang2">
                        <h3>0/<span>0</span></h3>
                    </div>
                </div>
                <div class="dash_attnd_grid attnd_grid_colr2">
                    <a href="#">
                        <div class="innet_attnd-block1 innet_attnd-gray1">
                            <h3>Teacher</h3>
                            <p>Attendance</p>
                        </div>
                    </a>
                    <div class="innet_attnd-block2 innet_attnd-gray2">
                        <h3>0/<span>0</span></h3>
                    </div>
                </div>
            </div>
            <div class="dash_bottom dashboard-action-blk">
                <ul>
                    <li><a href="#">View</a></li>
                </ul>
            </div>
        </div>


        <div class="os_dash_box1 block_class active_class" block-id="7" order-id="7">
            <div class="dash_subhed_mail">

                <div class="dash_subhed_hide_one mail_list">Mailbox</div>
                <div class="dash_subhed_hide_two" title="Hide this block" id="7"><i class="fa fa-times" aria-hidden="true"></i></div>

            </div>
            <div class="Default ps-container dash_box1_iner-pdng dash_bord-scrol dash_box-pading">
                <ul>
                    <div id="mailbox" class="list-view">
                        <div class="items">

                              <asp:ListView ID="lv_msg" runat="server" GroupPlaceholderID="groupPlaceHolder1"
                                ItemPlaceholderID="itemPlaceHolder1">
                                <LayoutTemplate>


                                    <asp:PlaceHolder runat="server" ID="groupPlaceHolder1"></asp:PlaceHolder>


                                </LayoutTemplate>
                                <GroupTemplate>

                                    <asp:PlaceHolder runat="server" ID="itemPlaceHolder1"></asp:PlaceHolder>

                                </GroupTemplate>
                                <ItemTemplate>










                                    <a href="#">
                                        <li class="mail_orange">
                                            <div class="mail-box-bg">
                                                <h3><%# Eval("mail_subject") %></h3>
                                                <%# Eval("mail_content") %>
                                                <div class="mail_box_view"></div>
                                            </div>
                                            <div class="mail-box-sender">

                                                <div class="mail_box_name">
                                                    <p>- By <%# Eval("mail_to") %> </p>
                                                </div>
                                            </div>

                                        </li>
                                        <div class="clear"></div>
                                    </a>









                                </ItemTemplate>
                                <EmptyDataTemplate>
                                </EmptyDataTemplate>
                            </asp:ListView>

                        </div>
                    </div>
                </ul>
                <div class="ps-scrollbar-x-rail" style="width: 599px; display: inherit; left: 0px;">
                    <div class="ps-scrollbar-x" style="left: 0px; width: 572px;"></div>
                </div>
                <div class="ps-scrollbar-y-rail" style="top: 0px; height: 202px; display: inherit; right: 3px;">
                    <div class="ps-scrollbar-y" style="top: 0px; height: 177px;"></div>
                </div>
            </div>
        </div>


        <div class="os_dash_box block_class active_class" block-id="8" order-id="8">
            <div class="dash_subhed">
                <div class="dash_subhed_hide_one  fees">Fees</div>
                <div class="dash_subhed_hide_two" title="Hide this block" id="8"><i class="fa fa-times" aria-hidden="true"></i></div>
            </div>
            <div class="dash_box-pading">
                <div class="dash_attnd_grid fee_grid_colr1">
                    <a href="#">
                        <div class="innet_attnd-block1 innet_attnd-orang1">
                            <h3>Total Fee </h3>
                            <p>Categories</p>
                        </div>
                    </a>
                    <div class="innet_attnd-block2 innet_attnd-orang2" id="div_fee_totalcategory" runat="server">
                        <h3>0</h3>
                    </div>
                </div>

                <div class="dash_attnd_grid attnd_grid_colr1">
                    <a href="#">
                        <div class="innet_attnd-block1 innet_attnd-orang1">
                            <h3>Invoices</h3>
                            <p>Generated</p>
                        </div>
                    </a>
                    <div class="innet_attnd-block2 innet_attnd-orang2" id="div_fee_totalinvoice" runat="server">
                        <h3>0</h3>
                    </div>
                </div>

                <div class="dash_attnd_grid attnd_grid_colr2">
                    <a href="#">
                        <div class="innet_attnd-block1 innet_attnd-gray1">
                            <h3>Fees</h3>
                            <p>Collected</p>
                        </div>
                    </a>
                    <div class="innet_attnd-block2 innet_attnd-gray2">
                        <h3 id="div_fee_totalcollect" runat="server">0</h3>
                    </div>
                </div>

            </div>
            <div class="dash_bottom dashboard-action-blk">
                <ul>
                    <li></li>
                    <li><a href="Adm_Fee.aspx">View</a></li>
                    <li><a href="Adm_CreateFees.aspx">Create</a></li>
                </ul>
            </div>
        </div>

        <div id="dashboard_err" style="display: none;">
            <div class="yellow_bx" style="background-image: none; padding-bottom: 45px;">
                <div class="y_bx_head" style="width: 650px;">
                    All blocks in the dashboard are disabled. You can change it from Dashboard Settings                   
                </div>
                <div class="y_bx_list" style="width: 650px;">
                    <h1><a href="/index.php?r=dashboard/settings">Dashboard Settings</a></h1>
                </div>
            </div>
        </div>
        <br style="">

        <div class="clear"></div>
        <div id="jobDialog"></div>
    </div>


    <style>
        @charset "utf-8";
        /* CSS Document */
        /**************************dashboard css starts*****************************************/

        .white_space {
            min-height: 800px;
            background-color: #Fff;
            padding: 12px;
        }

        .dash_box1 {
            width: 255px;
            height: 317px;
            border: 1px solid #e0e0e0;
            float: left;
        }

        .for_admin_box1 {
            width: 50% !important;
        }

        .dash_box2 {
            width: 440px;
            height: 317px;
            margin-left: 10px;
            border: 1px solid #e0e0e0;
            float: left;
        }

        .for_admin_box2 {
            width: 48.5% !important;
        }

        .dash_box3 {
            width: 255px;
            height: 317px;
            margin-left: 10px;
            border: 1px solid #e0e0e0;
            float: left;
        }

        .dash_box4 {
            width: 316px;
            height: 316px;
            margin: 10px 0 0 0px;
            border: 1px solid #e0e0e0;
            float: left;
        }

        .news_boxnew ul li a {
            display: block !important;
        }

        .for_student_box4 {
            margin-right: 10px;
        }

        .dash_box5 { /* width:317px;
	height:316px;
	margin:10px 0 0 10px;
	border:1px solid #e0e0e0;
	float:left;*/
            border: 1px solid #e0e0e0;
            float: left;
            height: 316px;
            margin: 10px 11px 0 0;
            width: 347px;
        }

        .dash_box6 {
            width: 254px;
            height: 316px;
            margin: 10px 0 0 0px;
            border: 1px solid #e0e0e0;
            float: left;
        }

        .for_student_exam {
            width: 285px !important;
        }

        .dash_box7 { /* width:707px;
	height:316px;
	margin:10px 0 0 10px;
	border:1px solid #e0e0e0;
	float:left;*/
            border: 1px solid #e0e0e0;
            float: left;
            height: 516px;
            margin: 10px 0 0;
            width: 99.8%;
        }

        .dash_subhed_mail {
            cursor: move !important;
        }

        .dash_subhed {
            border-bottom: 1px solid #fdc93e;
            font-size: 17px;
            font-weight: 500;
            height: 43px;
            position: relative;
            width: 100%;
            padding: 6px;
            box-sizing: border-box;
            clear: both;
            width: 100%;
            display: -ms-flexbox;
            display: -webkit-flex;
            display: flex;
            justify-content: space-between;
            align-items: center;
        }

        .subhed_date {
            font-size: 11px;
            font-weight: bold;
            position: absolute;
            right: 25px;
            top: 13px;
        }

        .attendance_date {
            font-size: 11px;
            font-weight: bold;
            position: relative;
            padding: 3px 11px 2px;
        }

        .dash_subhed_mail {
            border-bottom: 1px solid #fdc93e;
            font-size: 17px;
            font-weight: 500;
            height: 43px;
            position: relative;
            width: 100%;
            padding: 6px;
            box-sizing: border-box;
            clear: both;
            width: 100%;
            display: -ms-flexbox;
            display: -webkit-flex;
            display: flex;
            justify-content: space-between;
            align-items: center;
            background-color: #fff;
        }

            .dash_subhed_mail p {
                font-size: 12px;
                color: #666;
                margin: 0px;
            }




        .todo {
            background: url("../../images/dashboard_icons.png") no-repeat scroll 1px -152px #f1f1f1;
        }

        .time_table {
            background: url("../../images/dashboard_icons.png") no-repeat scroll 1px -225px #f1f1f1;
        }

        .assignment {
            background: url("../../images/dashboard_icons.png") no-repeat scroll 1px -298px #f1f1f1;
        }

        .attendance {
            background: url("../../images/dashboard_icons.png") no-repeat scroll 1px -370px #f1f1f1;
        }

        .class_exams {
            background: url("../../images/dashboard_icons.png") no-repeat scroll 1px -442px #f1f1f1;
        }



        .news_boxnew ul {
            float: left !important;
        }

            .news_boxnew ul li {
            }

        .white_space ul {
            margin: 0 7px 0 24px;
            padding: 0px;
            float: right;
        }

            /*.dash_box1 ul{ float:none !important;}*/


            .white_space ul li {
                border-bottom: 1px solid #ededed;
            }

        .Default {
            height: 456px;
        }

        .mousescroll ul li {
            list-style: url("../../images/red_dot.png");
        }

        div.mousescroll {
            overflow: hidden;
            position: relative;
        }

            div.mousescroll:hover {
            }

        .slimScrollDiv {
            border: 1px solid #ccc;
            margin: 10px;
        }

        .dash_bottom {
            height: 47px;
            width: 100%;
            background-color: #f8f8f8;
            border-top: 1px solid #e0e0e0;
            position: absolute;
            bottom: 0px;
        }

            .dash_bottom ul {
                margin: 0px;
                padding: 0px;
            }

            .dash_bottom a {
                color: #227195;
                font-weight: 100;
            }

            .dash_bottom ul li {
                border-left: 1px solid #ccc;
                border-bottom: 0px solid #ededed;
                float: left;
                padding: 15px 14px 13px;
                list-style: none;
            }

                .dash_bottom ul li:first-child {
                    border-left: 0px solid #ccc;
                }

        .dash_eye {
            background: rgba(0, 0, 0, 0) url("../../images/dash_icon_1.png") no-repeat scroll -1px -120px;
            float: left;
            height: 20px;
            width: 27px;
        }


        .dash_create {
            background: url("../../images/dash_icon_1.png") no-repeat scroll -1px -232px rgba(0, 0, 0, 0);
            float: left;
            height: 21px;
            width: 24px;
        }

        .dash_sub {
            background: url("../../images/dash_icon_1.png") no-repeat scroll -1px -175px rgba(0, 0, 0, 0);
            float: left;
            height: 22px;
            width: 26px;
        }

        .more {
            background: url("../../images/dash_icon_1.png") no-repeat scroll -1px -286px rgba(0, 0, 0, 0);
            float: left;
            height: 22px;
            width: 26px;
        }

        .dash_submit {
            background: url("../../images/dash_icon_1.png") no-repeat scroll -1px -64px rgba(0, 0, 0, 0);
            float: left;
            height: 24px;
            width: 24px;
        }


        .dash_leave {
            background: url("../../images/dash_icon_1.png") no-repeat scroll -1px -10px rgba(0, 0, 0, 0);
            float: left;
            height: 22px;
            width: 26px;
        }

        .events_box {
            padding: 12px;
            box-sizing: border-box;
        }

        .dash_chek { /*float:left;*/
        }

            .dash_chek ul {
                margin: 0px;
                padding: 0px;
                float: none;
            }

                .dash_chek ul li {
                    list-style: none outside none;
                    padding: 13px 7px;
                    /*width: 247px;*/
                }


        .att_bottom {
            width: 100%;
            margin-top: 10px;
        }

            .att_bottom ul {
                margin: 0px;
                border-top: 1px solid #dddddd;
                padding: 0px;
                float: left;
                width: 100%;
            }

            .att_bottom a {
                color: #227195;
                font-weight: 100;
            }


            .att_bottom ul li:last-child {
                border-bottom: 0px solid #ededed;
            }

            .att_bottom ul li {
                border-bottom: 1px solid #dddddd;
                padding: 18px 5px 12px 5px;
                list-style: none;
                background-color: #f8f8f8;
            }



        #tabs-4 a {
            background-color: #f1f1f1;
            color: #848484;
            font-weight: bold;
            padding: 1px 10px;
            cursor: pointer;
            display: block;
            position: relative;
            border: 1px solid #EDEDED;
            margin-bottom: 10px;
        }

        #tabs-4 h3 {
            font-size: 12px;
            margin: 11px 0 0;
            text-transform: uppercase;
        }

        #tabs-4 h5 {
            margin: 2px 2px 9px;
            color: #A5A4A4;
        }

        #tabs-4 h4.tab_date {
            color: #424242 !important;
            position: absolute;
            top: 9px;
            right: 11px;
        }


        #tabs-3 a {
            background-color: #f1f1f1;
            color: #848484;
            font-weight: bold;
            padding: 1px 10px;
            cursor: pointer;
            display: block;
            position: relative;
            border: 1px solid #EDEDED;
            margin-bottom: 10px;
        }

        #tabs-3 h3 {
            font-size: 12px;
            margin: 11px 0 0;
            text-transform: uppercase;
        }

        #tabs-3 h5 {
            margin: 2px 2px 9px;
            color: #A5A4A4;
        }

        #tabs-3 h4.tab_date {
            color: #424242 !important;
            position: absolute;
            top: 9px;
            right: 11px;
        }

        #tabs-2 a {
            background-color: #f1f1f1;
            color: #848484;
            font-weight: bold;
            padding: 1px 10px;
            display: block;
            position: relative;
            border: 1px solid #EDEDED;
            margin-bottom: 10px;
        }

        #tabs-4 a:hover {
            background-color: #F9F4D1;
            -webkit-transition: all .5s ease-in-out;
            -moz-transition: all .5s ease-in-out;
            -o-transition: all .5s ease-in-out;
            transition: all .5s ease-in-out;
            border: 1px solid #F4ECB5;
        }

        #tabs-3 a:hover {
            background-color: #F9F4D1;
            -webkit-transition: all .5s ease-in-out;
            -moz-transition: all .5s ease-in-out;
            -o-transition: all .5s ease-in-out;
            transition: all .5s ease-in-out;
            border: 1px solid #F4ECB5;
        }

        #tabs-2 a:hover {
            background-color: #F9F4D1;
            -webkit-transition: all .5s ease-in-out;
            -moz-transition: all .5s ease-in-out;
            -o-transition: all .5s ease-in-out;
            transition: all .5s ease-in-out;
            border: 1px solid #F4ECB5;
        }

        #tabs-1 a:hover {
            background-color: #F9F4D1;
            -webkit-transition: all .5s ease-in-out;
            -moz-transition: all .5s ease-in-out;
            -o-transition: all .5s ease-in-out;
            transition: all .5s ease-in-out;
            border: 1px solid #F4ECB5;
        }

        #tabs-2 h3 {
            font-size: 12px;
            margin: 11px 0 0;
            text-transform: uppercase;
        }

        #tabs-2 h5 {
            margin: 2px 2px 9px;
            color: #A5A4A4;
        }

        #tabs-2 h4.tab_date {
            color: #424242 !important;
            position: absolute;
            top: 9px;
            right: 11px;
        }

        #tabs-1 a {
            background-color: #f1f1f1;
            color: #848484;
            font-weight: bold;
            padding: 1px 10px;
            display: block;
            position: relative;
            border: 1px solid #EDEDED;
            margin-bottom: 10px;
        }

        #tabs-1 h3 {
            font-size: 12px;
            margin: 11px 0 0;
            text-transform: uppercase;
        }

        #tabs-1 h5 {
            margin: 2px 2px 9px;
            color: #A5A4A4;
        }

        #tabs-1 h4.tab_date {
            color: #424242 !important;
            position: absolute;
            top: 9px;
            right: 11px;
        }



        #tabs-1 a {
            background-color: #f1f1f1;
            border-radius: 3px;
            color: #6d6d33;
            font-weight: bold;
        }

        #tabs-2 p.none {
            color: #900;
        }

        .mail_dashnew {
            padding: 10px 20px;
        }


            .mail_dashnew ul li {
                border-bottom: 1px dashed #dbdbdb;
                list-style: none outside none;
                margin: 14px 0;
                padding: 10px 12px;
                position: relative;
            }

        .mail_blue {
            border-left: 5px solid #67acf0;
        }

        .mail_orange {
            border-left: 5px solid #EA8E64;
        }

        .mail_green {
            border-left: 5px solid #adce5c;
        }

        .mail_dashnew p {
            color: #999;
            margin: 3px;
            padding: 0;
        }

        .mail_dashnew h3 {
            color: #474747;
            font-size: 15px;
            padding: 0px;
            margin: 0px;
        }

        .mail_box_view {
            position: absolute;
            right: 5px;
            top: 28px;
        }

        .mail_dashnew_error {
            text-align: center;
            color: #C30;
            font-weight: bold;
        }

        .mail_dashnew ul {
            float: left;
            margin: 0;
            width: 100%;
        }

        .mousescroll table tr td, th {
            padding: 14px 15px;
            border-bottom: 1px solid #ededed;
        }

        .mousescroll table tr th {
            color: #d53f2b;
            text-align: left;
        }

        .assign ul li { /* width:303px !important*/
        }


        .time_table_dash {
            height: 216px;
        }

            .time_table_dash table tr td, th {
                padding: 12px 15px;
                border-bottom: 1px solid #ededed;
                word-break: break-all;
            }

            .time_table_dash table tr th {
                color: #000;
                text-align: left;
            }

        .dash_blue {
            background-color: #3083aa;
            border-radius: 3px;
            color: #fff;
            font-size: 12px;
            font-weight: bold;
            padding: 3px 7px;
            width: 121px;
        }

        .att_listbox {
            padding: 4px 8px;
            height: 87px;
        }

            .att_listbox ul {
                padding: 0px;
                margin: 0px;
                float: left;
            }

                .att_listbox ul:first-child {
                    border-left: 1px solid #e5e8ee;
                }

            .att_listbox ul {
                border-bottom: 1px solid #e5e8ee;
            }

                .att_listbox ul li {
                    width: 46px;
                    height: 30px;
                    font-size: 11px;
                    padding-top: 10px;
                    border: 1px solid #e5e8ee;
                    border-left: 0px solid;
                    border-bottom: 0px solid;
                    list-style: none;
                    background-color: #f3f7fd;
                    text-align: center;
                    word-break: break-all;
                }

                    .att_listbox ul li.bg_white {
                        background-color: #FFF;
                    }

                    .att_listbox ul li.bg_white_tick {
                        background: url("../../images/att_tick.png") no-repeat scroll 14px 14px #fff !important;
                        );
                    }

                    .att_listbox ul li.bg_white_cross {
                        background: url("../../images/att_cross.png") no-repeat scroll 14px 14px #fff !important;
                        );
                    }

        .inner_tabs {
            height: 100px;
        }



        /*------dashboerd-----------*/
        .dash_subhed_hide_one {
            background: url(../../images/dashboard_icons.png) no-repeat right;
        }

        .news {
            background-position: 1px -11px;
        }

        .mail_list {
            background-position: 1px -303px;
        }

        .events {
            background-position: 1px -84px;
        }

        .student {
            background-position: 1px -616px;
        }

        .examination {
            background-position: 1px -446px;
        }

        .attendance {
            background-position: 1px -529px;
        }

        .timetable {
            background-position: 1px -583px;
        }

        .teacher {
            background-position: 1px -682px;
        }

        .fees {
            background-position: 1px -751px;
        }

        .dashboard_bg {
            min-height: 800px;
            background-color: #e4e4e4;
            padding: 18px;
        }

            .dashboard_bg .events_box .tabs4 ul li a {
                display: block;
                padding: 9px 7px;
                background: #fff;
                font-size: 12px;
                text-decoration: none;
                color: #605E5E;
                font-weight: bold;
                position: relative;
                top: 1px !important;
                z-index: 1000;
            }


        .left-icon ul li {
            background: url("../../images/red_dot.png") no-repeat;
        }

        .dash_box-pading {
            padding: 14px;
            box-sizing: border-box;
        }

        .dash_box1_iner-pdng ul {
            margin: 0px;
            padding: 0px;
        }

            .dash_box1_iner-pdng ul li {
                border-bottom: 1px solid #ececec;
                border-bottom-style: dashed;
                list-style: none;
                padding: 7px;
                margin-bottom: 4px;
                background: linear-gradient(#f3f3f3, #f3f3f3);
            }

        .dash_box7 ul {
            margin: 0px;
            padding: 0px;
        }

        .dash_bord-scrol {
            height: 230px;
            overflow: hidden;
            position: relative;
        }

        .exm-graph {
            margin-top: 8px;
        }

        .dash_box1_iner-pdng ul li h3 {
            color: #605E5E;
            font-weight: bold;
            font-size: 13px;
            margin: 4px 0px;
        }

        .dash_box1_iner-pdng ul li p {
            color: #7d7d7d;
            font-weight: 500;
            font-size: 12px;
            margin: 4px 0px;
            font-family: sans-serif;
        }

        .dash_box1_iner-pdng ul li span {
            color: #7a9cab;
            font-weight: 500;
            font-size: 11px;
            font-family: sans-serif;
            text-align: right;
            display: inline-block;
            width: 100%;
            padding-bottom: 0px;
        }

        .os_dash_box .dash_st_grid:hover {
            opacity: 0.8;
            -webkit-transition-duration: 0.3s;
            transition-duration: 0.3s;
        }

        .os_dash_box .dash_st_grid2:hover {
            opacity: 0.8;
            -webkit-transition-duration: 0.3s;
            transition-duration: 0.3s;
        }

        .os_dash_box .dash_attnd_grid:hover {
            opacity: 0.8;
            -webkit-transition-duration: 0.3s;
            transition-duration: 0.3s;
        }

        .os_dash_box {
            width: 32%;
            height: 333px;
            padding-left: -1px;
            border: 1px solid #e0e0e0;
            float: left;
            margin-left: 7px;
            margin-bottom: 10px;
            position: relative;
            box-shadow: 0px 0px 4px #d0d0d0;
            background-color: #fff;
        }

        .os_dash_box1 {
            width: 65%;
            height: 333px;
            padding-left: -1px;
            border: 1px solid #e0e0e0;
            float: left;
            margin-left: 7px;
            margin-bottom: 10px;
            position: relative;
            box-shadow: 0px 0px 10px #ececec;
            background-color: #fff;
        }
        /*---student--dashboad---start------*/
        .dash_st_grid {
            width: 67px;
            padding: 11px;
            float: left;
            text-align: center;
            margin: 2px;
        }

            .dash_st_grid h3 {
                font-family: sans-serif;
                font-size: 27px;
                color: #fff;
                margin: 0px;
                font-weight: 400;
            }

            .dash_st_grid p {
                font-family: sans-serif;
                font-size: 11px;
                color: #fff;
                margin: 0px;
            }

        .st_grid_colr1 {
            background-color: #fcb52e;
        }

        .st_grid_colr2 {
            background-color: #f36942;
        }

        .st_grid_colr3 {
            background-color: #4c4744;
        }

        .cv tspan {
            font-family: sans-serif;
            font-size: 9px;
            color: #fff;
            margin: 0px;
        }
        /*---student--dashboad----end-----*/
        /*---Teacher--dashboad---start------*/
        .dash_st_grid2 {
            width: 108px;
            padding: 27px 10px;
            float: left;
            text-align: center;
            margin: 5px;
        }

            .dash_st_grid2 h3 {
                font-family: sans-serif;
                font-size: 27px;
                color: #fff;
                margin: 0px;
                font-weight: 400;
            }

            .dash_st_grid2 p {
                font-family: sans-serif;
                font-size: 14px;
                color: #fff;
                margin: 0px;
                font-weight: 500;
            }

        .t_grid_colr1 h3 {
            font-family: sans-serif;
            font-size: 27px;
            color: #4c4744;
            margin: 0px;
            font-weight: 400;
        }

        .t_grid_colr1 p {
            font-family: sans-serif;
            font-size: 14px;
            color: #4c4744;
            margin: 0px;
            font-weight: 500;
        }

        .t_grid_colr1 {
            background-color: #f1f1f1;
        }

        /*---Teacher--dashboad---End------*/
        /*---attendance--dashboad---start------*/
        .dash_attnd_grid {
            display: inline-block;
            width: 100%;
            padding: 10px 7px;
            box-sizing: border-box;
            margin-bottom: 5px;
        }

        .attnd_grid_colr1 {
            background-color: #f36942;
        }

        .fee_grid_colr1 {
            background-color: #fcb52e;
        }

        .attnd_grid_colr2 {
            background-color: #f1f1f1;
        }

        .innet_attnd-block1 {
            /*    width: 157px;*/
            float: left;
            margin: 5px;
        }

        .innet_attnd-block2 {
            /*    width: 85px;*/
            float: right;
            margin: 5px;
            text-align: center;
        }

        .innet_attnd-block1 h3 {
            font-family: sans-serif;
            font-size: 18px;
            margin: 0px;
            font-weight: 600;
        }

        .innet_attnd-block1 p {
            font-family: sans-serif;
            font-size: 12px;
            margin: 0px;
            font-weight: 300;
            font-style: italic;
        }

        .innet_attnd-block2 h3 {
            font-family: sans-serif;
            font-size: 34px;
            margin: 0px;
            font-weight: 400;
        }

            .innet_attnd-block2 h3 span {
                font-family: sans-serif;
                font-size: 22px;
                margin: 0px;
                font-weight: 600;
            }

        .innet_attnd-orang1 h3 {
            color: #FFF;
        }

        .innet_attnd-orang1 p {
            color: #FFF;
        }

        .innet_attnd-orang2 h3 {
            color: #FFF;
        }

            .innet_attnd-orang2 h3 span {
                color: #febba8;
            }

        .innet_attnd-gray1 h3 {
            color: #4c4744;
        }

        .innet_attnd-gray1 p {
            color: #4c4744;
        }

        .innet_attnd-gray2 h3 {
            color: #4c4744;
        }

            .innet_attnd-gray2 h3 span {
                color: #c8c4c1;
            }

        /*---attendance--dashboad---End------*/
        .os_dash_box .dash_bottom {
            height: 40px;
            width: 100%;
            background-color: #f9f9f9;
            border: none;
            position: absolute;
            bottom: 0px;
        }

            .os_dash_box .dash_bottom ul li:first-child {
                border-left: 0px solid #ccc;
            }

            .os_dash_box .dash_bottom ul li {
                border: none !important;
                float: left;
                padding: 15px;
                list-style: none;
            }

                .os_dash_box .dash_bottom ul li a {
                    font-family: sans-serif;
                    font-size: 11px;
                    margin: 0px;
                    font-weight: 600;
                    text-transform: uppercase;
                    color: #848484;
                }

                    .os_dash_box .dash_bottom ul li a:hover {
                        color: #FDC93E;
                        -webkit-transition-duration: 0.3s;
                        transition-duration: 0.3s;
                    }

        .dash_subhed_hide_one {
            padding: 6px 0px 6px 41px;
            box-sizing: border-box;
        }

        .dash_subhed_hide_two {
            float: left;
            width: 5%;
            padding: 6px 0px 6px 0px;
        }

            .dash_subhed_hide_two:hover {
                cursor: pointer;
            }

        .dash_subhed_hide_one {
            cursor: move !important;
        }

        .dash_subhed .fa-times {
            color: #adce5c;
        }

        .dash_subhed_mail .fa-times {
            color: #adce5c;
        }
    </style>

    <asp:HiddenField ID="hd_acyear" runat="server" />
</asp:Content>

