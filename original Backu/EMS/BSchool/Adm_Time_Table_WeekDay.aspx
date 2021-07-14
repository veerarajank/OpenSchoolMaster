<%@ Page Title="" Language="C#" MasterPageFile="~/Adm_TimeTable.master" AutoEventWireup="true" CodeFile="Adm_Time_Table_WeekDay.aspx.cs" Inherits="Adm_Time_Table_WeekDay" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <style>
        .checkbox, .radio {
            width: 24px;
            height: 25px;
            padding: 0 5px 0 0;
            background: url(./images/nschkb.png) no-repeat;
            display: block;
            clear: left;
            float: left;
            margin: 0px 5px 0px 5px;
        }

        #yellowchk .checkbox, .radio {
            width: 19px;
            height: 20px;
            padding: 0 5px 0 0;
            background: url(./images/check_yellow.png) no-repeat;
            display: block;
            clear: left;
            float: left;
        }

        #greenchk .checkbox, .radio {
            width: 19px;
            height: 20px;
            padding: 0 5px 0 0;
            background: url(./images/check_green.png) no-repeat;
            display: block;
            clear: left;
            float: left;
        }

        #redchk .checkbox, .radio {
            width: 19px;
            height: 20px;
            padding: 0 5px 0 0;
            background: url(./images/check_red.png) no-repeat;
            display: block;
            clear: left;
            float: left;
        }

        #bluechk .checkbox, .radio {
            width: 19px;
            height: 20px;
            padding: 0 5px 0 0;
            background: url(./images/check_blue.png) no-repeat;
            display: block;
            clear: left;
            float: left;
        }

        .radio {
            background: url(radio.png) no-repeat;
        }

        .select {
            position: absolute;
            width: 158px; /* With the padding included, the width is 190 pixels: the actual width of the image. */
            height: 21px;
            padding: 0 24px 0 8px;
            color: #fff;
            font: 12px/21px arial,sans-serif;
            background: url(select.png) no-repeat;
            overflow: hidden;
        }

        input.styled {
            display: none;
        }
    </style>


    <div class="cont_right formWrapper">
        <h1 style="margin-top: .67em;">Set Week Days<br>
        </h1>
        <div class="clear"></div>
        <div class="emp_right_contner">
            <div class="emp_tabwrapper">

                <div class="formCon">
                    <div class="formConInner">
                        <div class="txtfld-col-box">

                            <div class="txtfld-col">
                                Select Course
                        <asp:DropDownList ID="drp_course" runat="server" AutoPostBack="true" OnSelectedIndexChanged="drp_course_SelectedIndexChanged"></asp:DropDownList>
                            </div>
                            <div class="txtfld-col">
                                Select Batch
                        <asp:DropDownList ID="drp_batch" runat="server" AutoPostBack="true" OnSelectedIndexChanged="drp_batch_SelectedIndexChanged" ></asp:DropDownList>
                            </div>
                            <div class="txtfld-col">
                                Select Semester								                        
                        <asp:DropDownList ID="drp_semester" runat="server" AutoPostBack="true" OnSelectedIndexChanged="drp_semester_SelectedIndexChanged"></asp:DropDownList>
                            </div>
                            <div class="txtfld-col">
                                <button id="btn_setdays" runat="server" onserverclick="btn_setdays_ServerClick" class="a-add-btn" style="width: 150px"><span><i class="fa fa-calendar"></i>&nbsp;Set Week days</span></button>
                            </div>


                        </div>
                        <div class="text-fild-block-full">
                        </div>
                    </div>
                </div>






                <div class="clear"></div>
                <div class="emp_tabwrapper">


                    <div class="clear"></div>


                    <div class="formCon">
                        <div class="formConInner">


                            <div>
                                <div style="padding-top: 10px; font-size: 14px; font-weight: bold;">

                                    <span>
                                        <h3>Week Days</h3>
                                    </span>

                                    <div>


                                        <table width="100%" cellpadding="0" cellspacing="0">
                                            <tbody>
                                                <tr>
                                                    <td width="4%">
                                                        <asp:HiddenField ID="hd_sunday" runat="server" Value="0" />
                                                        <span id="hd_1" runat="server" onclick="Check_Status(1)" class="checkbox" style="background-position: 0px 0px;"></span>
                                                        <input value="1" class="styled" id="chk_sunday" runat="server" type="checkbox">
                                                    </td>
                                                    <td width="85%">Sunday</td>
                                                    <td width="11%">&nbsp;</td>
                                                </tr>
                                                <tr>
                                                    <td>&nbsp;</td>
                                                    <td>&nbsp;</td>
                                                    <td>&nbsp;</td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:HiddenField ID="hd_monday" runat="server" Value="0" />
                                                        <span id="hd_2"  runat="server" onclick="Check_Status(2)" class="checkbox" style="background-position: 0px 0px;"></span>
                                                        <input value="2" checked="checked" class="styled" runat="server" id="chk_monday" type="checkbox">
                                                    </td>
                                                    <td>Monday</td>
                                                    <td></td>
                                                </tr>
                                                <tr>
                                                    <td>&nbsp;</td>
                                                    <td>&nbsp;</td>
                                                    <td>&nbsp;</td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:HiddenField ID="hd_tuesday" runat="server" Value="0" />
                                                        <span id="hd_3"  runat="server" onclick="Check_Status(3)" class="checkbox" style="background-position: 0px 0px;"></span>
                                                        <input value="3" checked="checked" class="styled" runat="server" id="chk_tuesday" type="checkbox">
                                                    </td>
                                                    <td>Tuesday</td>
                                                    <td></td>
                                                </tr>
                                                <tr>
                                                    <td>&nbsp;</td>
                                                    <td>&nbsp;</td>
                                                    <td>&nbsp;</td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:HiddenField ID="hd_wednesday" runat="server" Value="0" />
                                                        <span id="hd_4"  runat="server" onclick="Check_Status(4)" class="checkbox" style="background-position: 0px 0px;"></span>
                                                        <input value="4" checked="checked" class="styled" runat="server" id="chk_wednesday" type="checkbox">
                                                    </td>
                                                    <td>Wednesday</td>
                                                    <td></td>
                                                </tr>
                                                <tr>
                                                    <td>&nbsp;</td>
                                                    <td>&nbsp;</td>
                                                    <td>&nbsp;</td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:HiddenField ID="hd_thursday" runat="server" Value="0" />
                                                        <span id="hd_5"  runat="server" onclick="Check_Status(5)" class="checkbox" style="background-position: 0px 0px;"></span>
                                                        <input value="5" checked="checked" class="styled" runat="server" id="chk_thursday" type="checkbox">
                                                    </td>
                                                    <td>Thursday</td>
                                                    <td></td>
                                                </tr>
                                                <tr>
                                                    <td>&nbsp;</td>
                                                    <td>&nbsp;</td>
                                                    <td>&nbsp;</td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:HiddenField ID="hd_friday" runat="server" Value="0" />
                                                        <span id="hd_6"  runat="server" onclick="Check_Status(6)" class="checkbox" style="background-position: 0px 0px;"></span>
                                                        <input value="6" checked="checked" class="styled" runat="server" id="chk_friday" type="checkbox">
                                                    </td>
                                                    <td>Friday</td>
                                                    <td></td>
                                                </tr>
                                                <tr>
                                                    <td>&nbsp;</td>
                                                    <td>&nbsp;</td>
                                                    <td>&nbsp;</td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:HiddenField ID="hd_saturday" runat="server" Value="0" />
                                                        <span id="hd_7"  runat="server" onclick="Check_Status(7)" class="checkbox" style="background-position: 0px 0px;"></span>
                                                        <input value="7" class="styled" id="chk_saturday" runat="server" type="checkbox"></td>
                                                    <td>Saturday</td>
                                                    <td></td>
                                                </tr>
                                            </tbody>
                                        </table>
                                    </div>
                                    <br>
                                </div>
                            </div>
                        </div>

                    </div>

                    <br>
                </div>

            </div>

        </div>

    </div>
     <script src="plugins/jQuery/jquery-2.2.3.min.js"></script>

    <script type="text/javascript">
        function Check_Status(cnt) {
          
            if (cnt == "1") {
                var value = $('#' + '<%= hd_sunday.ClientID %>').val();

                 if (value == "1") $('#' + '<%= hd_1.ClientID %>').attr("style", "background-position: 0px 0px;");
                 else $('#' + '<%= hd_1.ClientID %>').attr("style", "background-position: 0px -64px;");
                if (value == "1") {
                    $('#' + '<%= hd_sunday.ClientID %>').val("0");
                }
                else {
                    $('#' + '<%= hd_sunday.ClientID %>').val("1");
                }
            }
            if(cnt=="2")
            {
                var value = $('#' + '<%= hd_monday.ClientID %>').val();                

                if (value == "1") $('#' + '<%= hd_2.ClientID %>').attr("style", "background-position: 0px 0px;");
                else $('#' + '<%= hd_2.ClientID %>').attr("style", "background-position: 0px -64px;");
                if (value == "1") {
                    $('#' + '<%= hd_monday.ClientID %>').val("0");
                }
                else {
                    $('#' + '<%= hd_monday.ClientID %>').val("1");
                }
            }
            if (cnt == "3") {
                var value = $('#' + '<%= hd_tuesday.ClientID %>').val();

                if (value == "1") $('#' + '<%= hd_3.ClientID %>').attr("style", "background-position: 0px 0px;");
                else $('#' + '<%= hd_3.ClientID %>').attr("style", "background-position: 0px -64px;");
                 if (value == "1") {
                     $('#' + '<%= hd_tuesday.ClientID %>').val("0");
                }
                else {
                    $('#' + '<%= hd_tuesday.ClientID %>').val("1");
                }
            }
            if (cnt == "4") {
                var value = $('#' + '<%= hd_wednesday.ClientID %>').val();

                if (value == "1") $('#' + '<%= hd_4.ClientID %>').attr("style", "background-position: 0px 0px;");
                else $('#' + '<%= hd_4.ClientID %>').attr("style", "background-position: 0px -64px;");
                if (value == "1") {
                    $('#' + '<%= hd_wednesday.ClientID %>').val("0");
                }
                else {
                    $('#' + '<%= hd_wednesday.ClientID %>').val("1");
                }
            }
            if (cnt == "5") {
                var value = $('#' + '<%= hd_thursday.ClientID %>').val();

                 if (value == "1") $('#' + '<%= hd_5.ClientID %>').attr("style", "background-position: 0px 0px;");
                 else $('#' + '<%= hd_5.ClientID %>').attr("style", "background-position: 0px -64px;");
                if (value == "1") {
                    $('#' + '<%= hd_thursday.ClientID %>').val("0");
                }
                else {
                    $('#' + '<%= hd_thursday.ClientID %>').val("1");
                }
            }
            if (cnt == "6") {
                var value = $('#' + '<%= hd_friday.ClientID %>').val();

                if (value == "1") $('#' + '<%= hd_6.ClientID %>').attr("style", "background-position: 0px 0px;");
                else $('#' + '<%= hd_6.ClientID %>').attr("style", "background-position: 0px -64px;");
                 if (value == "1") {
                     $('#' + '<%= hd_friday.ClientID %>').val("0");
                }
                else {
                    $('#' + '<%= hd_friday.ClientID %>').val("1");
                }
            }
            if (cnt == "7") {
                var value = $('#' + '<%= hd_saturday.ClientID %>').val();

                if (value == "1") $('#' + '<%= hd_7.ClientID %>').attr("style", "background-position: 0px 0px;");
                else $('#' + '<%= hd_7.ClientID %>').attr("style", "background-position: 0px -64px;");
                 if (value == "1") {
                     $('#' + '<%= hd_saturday.ClientID %>').val("0");
                }
                else {
                    $('#' + '<%= hd_saturday.ClientID %>').val("1");
                }
            }

        }

        </script>
       

</asp:Content>

