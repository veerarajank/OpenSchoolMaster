<%@ Page Title="" Language="C#" MasterPageFile="~/Student_Settings.master" AutoEventWireup="true" CodeFile="Adm_StudentGuardian_Creation.aspx.cs" Inherits="Adm_StudentGuardian_Creation" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script src="plugins/jQuery/jquery-2.2.3.min.js"></script>
    <div class="cont_right formWrapper">
        <style type="text/css">
            .pagetab-bg .li_hide {
                display: none !important;
            }

            h3 {
                color: #58656E;
                font-size: 12px;
                margin: 0;
            }

            .student-postn {
                top: 16px;
                right: 19px;
            }
        </style>
        <style>
            .accordion-item-active .accordion-header h1 {
                float: none;
            }

            .acordn-bg .accordion-header h1 {
                float: none;
            }
        </style>
        <div class="right-pg-hd">

            <h1>Enrolment</h1>
        </div>

        <div class="button-bg">
            <div class="top-hed-btn-left"></div>
            <div class="top-hed-btn-right">
                <ul>
                    <li><a class="a_tag-btn" href="#">View Profile</a></li>
                </ul>
            </div>
        </div>
        <div class="page-tab">
            <div class="pagetab-bg">
                <ul>
                    <li class=" ">
                        <h2 class="hvr-syle"><a href="#" id="lnk_studentdetails" runat="server" onserverclick="lnk_studentdetails_ServerClick">Student Details</a></h2>
                    </li>
                    <li class="active ">
                        <h2 class="hvr-syle"><a href="#" id="lnk_guardiandetails" runat="server" onserverclick="lnk_guardiandetails_ServerClick">Guardian Details</a></h2>
                    </li>
                    <li class=" ">
                        <h2 class="hvr-syle"><a href="#" id="lnk_previousdetails" runat="server" onserverclick="lnk_previousdetails_ServerClick">Previous Details</a></h2>
                    </li>
                    <li class=" ">
                        <h2 class="hvr-syle"><a href="#" id="lnk_documentdetails" runat="server" onserverclick="lnk_documentdetails_ServerClick">Student Documents</a></h2>
                    </li>
                </ul>
            </div>
        </div>


        <div class="acordn-bg">
            <div class="acrdn-tab-bg">
                <div class="accordion">

                    <div class="drawer">
                        <div class="accordion-item accordion-item-active">
                            <div class="accordion-header accordion-header-active">
                                <h1>Guardian(s) Details</h1>
                                <div class="box-btn">
                                    <div class="tt-wrapper-new">
                                    </div>
                                </div>
                            </div>
                            <div class="pdtab_Con" style="display: block; padding-top: 0px !important">

                                <asp:ListView ID="lv_gaurdiansdetails" runat="server" GroupPlaceholderID="groupPlaceHolder1"
                                    ItemPlaceholderID="itemPlaceHolder1">
                                    <LayoutTemplate>
                                        <table style="width: 100%" cellpadding="0" cellspacing="0">
                                            <thead style="background-color: #E8ECF1">
                                                <td align="left" width="300">Name                       
                                                </td>
                                                <td align="center" width="150">Relation</td>
                                                <td align="center" width="150">Email</td>
                                                <td align="center" width="300"></td>

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

                                        <td align="left" width="300"><%# Eval("name") %></td>
                                        <td align="center" width="150"><%# Eval("relation") %></td>
                                        <td align="center" width="150"><%# Eval("email") %></td>
                                        <td>
                                            <div class="tt-wrapper-new">

                                                <%# Convert.ToString(Eval("is_parent_user"))=="1" ?
                                                    "<a class='makeprimary-active' id='lnk_maked'><span>Primary Contact</span></a>"                                                     
                                                    :
                                                    "<a class='makeprimary' onclick='Primary_contact_Popup(\"" + Convert.ToString(Eval("id"))  + "\")' id='lnk_maked'><span>Make Primary Contact</span></a>"                                                                                                     
                                                
                                                 %>

                                                 <%# Convert.ToString(Eval("emergency_user"))=="1" ?
                                                    "<a class='makeemrgency-active' id='lnk_makedemergency'><span>Emergency Contact</span></a>"                                                     
                                                    :
                                                    "<a class='makeemrgency' onclick='Emergency_contact_Popup(\"" + Convert.ToString(Eval("id"))  + "\")' id='lnk_maked'><span>Make Emergency Contact</span></a>"                                                                                                     
                                                
                                                 %>
                                                                                                  
                                                
                                                
                                                <a class="makeedit" visible="false" href="#" runat="server" id="edit_guardian" onserverclick="edit_guardian_ServerClick"><span>Edit</span></a>
                                                <a class="makedelete" id="lnk_del" onclick='DeletePopup("<%# Eval("id") %>")' style='<%# Convert.ToBoolean(Session["Access_Trash"])==true ? "visibility:visible": "visibility:hidden" %>'>
                                                    <span>Remove from guardian list</span>
                                                </a>
                                                

                                            </div>



                                        </td>
                                    </ItemTemplate>
                                    <EmptyDataTemplate>
                                        <table width="100%" cellpadding="0" cellspacing="0">
                                            <thead style="border: 1px solid #b9c7d0!important; background-color: #E8ECF1">
                                                <td align="left" width="300">Name                       
                                                </td>
                                                <td align="center" width="150">Relation</td>
                                                <td align="center" width="150">Email</td>
                                                <td align="center" width="300"></td>
                                            </thead>
                                            <tr style="border: 1px solid #b9c7d0!important;">
                                                <td colspan="4">No Results Found
                                                </td>
                                            </tr>
                                        </table>
                                    </EmptyDataTemplate>
                                </asp:ListView>



                            </div>
                        </div>
                    </div>



                </div>
            </div>
        </div>

        <!-- Radio Box -->
        <div class="formCon_existng_g">
            <div class="formConInner" style="padding: 0px; width: auto;">
                <div class="guardin-exixt">

                    <input type="radio" id="chk_guardian1" onchange="getmode(1);" runat="server" name="type"  />
                    <label for="guardian1">Already Existing Guardian<span></span></label>

                </div>
            </div>
        </div>




        <div class="formCon" id="search" style="display: none; margin-bottom: 0px; margin-top: 0px;">
            <div class="formConInner">
                Search                            
                <span class="guard_search">
                    <asp:TextBox ID="tbx_sibilings_studentname" runat="server" CssClass="ui-autocomplete-input" AutoCompleteType="Disabled" placeholder="Sibilings"></asp:TextBox>
                    <span class="or_img"></span>
                </span>


                <span class="guard_search">
                    <asp:TextBox ID="tbx_sibilings_parent" runat="server" CssClass="ui-autocomplete-input" AutoCompleteType="Disabled" placeholder="Parent"></asp:TextBox>
                    <span class="or_img"></span>
                </span>

                <span class="guard_search" style="padding-right: 10px;">
                    <asp:TextBox ID="tbx_sibilings_parent_email" runat="server" CssClass="ui-autocomplete-input" AutoCompleteType="Disabled" placeholder="Parent Email"></asp:TextBox>
                </span>
                <div class="clear"></div>
                <div style="margin-top: 10px; margin-left: 49px;">
                    <asp:Button ID="btn_sibiling_search" CssClass="formbut-n" runat="server" Text="Search" OnClick="btn_sibiling_search_Click" />
                </div>
            </div>
        </div>

        <div class="formCon_existng_g">
            <div class="formConInner">
                <div class="tableinnerlist">
                    
                    <input type="radio" id="chk_guardian2" checked name="type" onchange="getmode(0);" runat="server" AutoPostBack="true" OnCheckedChanged="chk_guardian2_CheckedChanged" />
                    <label for="guardian2">New Guardian<span></span></label>
                </div>
            </div>
        </div>
        <!-- END Radio Box -->
        <style type="text/css">
            .ui-widget-content {
                height: 300px;
            }


            .select-style select {
                width: 135% !important;
            }


            .pdtab_Con {
                margin: 0px;
                padding: 15px 0 0;
            }

            .formbut {
                padding: 10px 15px;
            }
        </style>

        <div class="pdtab_Con">
            <h3>Existing Guardian(s)</h3>

            <asp:ListView ID="lv_existingguardian" runat="server" GroupPlaceholderID="groupPlaceHolder1"
                ItemPlaceholderID="itemPlaceHolder1" OnPagePropertiesChanging="lv_existingguardian_PagePropertiesChanging">
                <LayoutTemplate>
                    <table width="100%" cellpadding="0" cellspacing="0">
                        <thead>
                            <td align="center" width="40">Select                                
                            </td>
                            <td align="center" width="150">Name</td>
                            <td align="center" width="150">Relation</td>
                            <td align="center" width="150">Email</td>
                        </thead>
                        <asp:PlaceHolder runat="server" ID="groupPlaceHolder1"></asp:PlaceHolder>
                        <tr style="background-color: #EEEEE0; border: 1px solid #b9c7d0!important" class="tableInfoTH">
                            <td colspan="4" class="tableInfoTD">
                                <asp:DataPager ID="DataPager1" runat="server" PagedControlID="lv_existingguardian" PageSize="10" class="googleNavegationBar">
                                    <Fields>
                                        <aspPage:GooglePagerField NextPageImageUrl="~/Images/button_arrow_right.gif" PreviousPageImageUrl="~/Images/button_arrow_left.gif" />
                                    </Fields>
                                </asp:DataPager>
                            </td>
                        </tr>
                    </table>
                </LayoutTemplate>
                <GroupTemplate>
                    <tr style="border: 1px solid #b9c7d0!important;">
                        <asp:PlaceHolder runat="server" ID="itemPlaceHolder1"></asp:PlaceHolder>
                    </tr>
                </GroupTemplate>
                <ItemTemplate>

                    <td align="center" width="40">
                         <div class="tt-wrapper-new">
                         <a class="makeplus" id="lnk_makegaurdian" onclick='MakeContact("<%# Eval("id") %>")' >
                            <span>Make guardian</span>
                            </a>
                            </div>
                                                
                    </td>

                    <td align="left" width="150"><%# Eval("name") %></td>
                    <td align="center" width="150"><%# Eval("relation") %></td>
                    <td align="center" width="150"><%# Eval("email") %></td>

                </ItemTemplate>
                <EmptyDataTemplate>
                    <table width="100%" cellpadding="0" cellspacing="0">
                        <thead style="border: 1px solid #b9c7d0!important;">
                            <td align="center" width="40">Select                             
                            </td>
                            <td align="center" width="150">Name</td>
                            <td align="center" width="150">Relation</td>
                            <td align="center" width="150">Email</td>
                        </thead>
                        <tr style="border: 1px solid #b9c7d0!important;">
                            <td colspan="4">No Results Found
                            </td>
                        </tr>
                    </table>
                </EmptyDataTemplate>
            </asp:ListView>
           
        </div>


        <p class="note">Fields with <span class="required">*</span> are required.</p>
        <div class="formCon">
            <div class="formConInner opnsl_form_label">
                <h3>Personal Details</h3>

                <div class="txtfld-col">
                    <label for="Guardians_first_name" class="required">First Name <span class="required">*</span></label>
                    <asp:TextBox ID="tbx_Guardians_first_name" AutoCompleteType="Disabled" runat="server" MaxLength="255"></asp:TextBox>
                </div>

                <div class="txtfld-col">
                    <label for="Guardians_last_name" class="required">Last Name <span class="required">*</span></label>
                    <asp:TextBox ID="tbx_Guardians_last_name" AutoCompleteType="Disabled" runat="server" MaxLength="255"></asp:TextBox>
                </div>

                <div class="txtfld-col">
                    <label for="Guardians_relation" class="required">Relation <span class="required">*</span></label>
                    <asp:DropDownList ID="drp_Guardians_relation" runat="server" AutoPostBack="true" OnSelectedIndexChanged="drp_Guardians_relation_SelectedIndexChanged">
                        <asp:ListItem Value="Father" Text="Father"></asp:ListItem>
                        <asp:ListItem Value="Mother" Text="Mother"></asp:ListItem>
                        <asp:ListItem Value="Grand Father" Text="Grand Father"></asp:ListItem>
                        <asp:ListItem Value="Grand Mother" Text="Grand Mother"></asp:ListItem>
                        <asp:ListItem Value="Brother" Text="Brother"></asp:ListItem>
                        <asp:ListItem Value="Sister" Text="Sister"></asp:ListItem>
                        <asp:ListItem Value="Others" Text="Others"></asp:ListItem>

                    </asp:DropDownList>

                    <div id="relation_other" runat="server" visible="false" style="margin-top: 10px;">
                        <label class="relation-star" for="Guardians_relation_other">Specify Relation</label><span class="required"> *</span>
                        <asp:TextBox ID="tbx_Guardians_relation_other" AutoCompleteType="Disabled" runat="server" MaxLength="255"></asp:TextBox>
                    </div>

                </div>

                <div class="txtfld-col">
                    <label for="Guardians_dob">Date Of Birth</label>
                    <asp:TextBox ID="tbx_Guardians_dob" runat="server" AutoCompleteType="Disabled" MaxLength="255" class="hasDatepicker"></asp:TextBox>
                </div>

                <div class="txtfld-col">
                    <label for="Guardians_education">Education</label>
                    <asp:TextBox ID="tbx_Guardians_education" runat="server" AutoCompleteType="Disabled" MaxLength="255"></asp:TextBox>
                </div>

                <div class="txtfld-col">
                    <label for="Guardians_occupation">Occupation</label>
                    <asp:TextBox ID="tbx_Guardians_occupation" runat="server" AutoCompleteType="Disabled" MaxLength="255"></asp:TextBox>
                </div>

                <div class="txtfld-col">
                    <label for="Guardians_income">Income</label>
                    <asp:TextBox ID="tbx_Guardians_income" runat="server" AutoCompleteType="Disabled" MaxLength="255"></asp:TextBox>
                </div>



                <div class="clear"></div>
            </div>
        </div>

        <div class="formCon">
            <div class="formConInner opnsl_form_label">
                <h3>Contact Details</h3>
                <div class="txtfld-col">
                    <label for="Guardians_email" class="required">Email <span class="required">*</span></label>
                    <asp:TextBox ID="tbx_Guardians_email" runat="server" AutoCompleteType="Disabled" MaxLength="255"></asp:TextBox>
                </div>

                <div class="txtfld-col">
                    <label for="Guardians_mobile_phone" class="required">Mobile Phone <span class="required">*</span></label>
                    <asp:TextBox ID="tbx_Guardians_mobile_phone" runat="server" AutoCompleteType="Disabled" MaxLength="255"></asp:TextBox>
                </div>

                <div class="txtfld-col">
                    <div class="show">
                        <label for="Guardians_office_phone1">Phone 1</label>
                        <asp:TextBox ID="tbx_Guardians_office_phone1" runat="server" AutoCompleteType="Disabled" MaxLength="255"></asp:TextBox>
                    </div>
                </div>

                <div class="txtfld-col">
                    <div class="show">
                        <label for="Guardians_office_phone2">Phone 2</label>
                        <asp:TextBox ID="tbx_Guardians_office_phone2" runat="server" AutoCompleteType="Disabled" MaxLength="255"></asp:TextBox>
                    </div>
                </div>

                <div class="txtfld-col show">
                    <label for="Guardians_office_address_line1">Address Line 1</label>
                    <asp:TextBox ID="tbx_Guardians_office_address_line1" runat="server" AutoCompleteType="Disabled" MaxLength="255"></asp:TextBox>
                </div>

                <div class="txtfld-col show">
                    <label for="Guardians_office_address_line2">Address Line 2</label>
                    <asp:TextBox ID="tbx_Guardians_office_address_line2" runat="server" AutoCompleteType="Disabled" MaxLength="255"></asp:TextBox>
                </div>

                <div class="txtfld-col show">
                    <label for="Guardians_city">City </label>
                    <asp:TextBox ID="tbx_Guardians_city" runat="server" AutoCompleteType="Disabled" MaxLength="255"></asp:TextBox>
                </div>

                <div class="txtfld-col show">
                    <label for="Guardians_state">State</label>
                    <asp:TextBox ID="tbx_Guardians_state" runat="server" AutoCompleteType="Disabled" MaxLength="255"></asp:TextBox>
                </div>

                <div class="txtfld-col show">
                    <label for="Guardians_country">Country</label>
                    <asp:DropDownList ID="drp_Guardians_country" runat="server"></asp:DropDownList>
                </div>



                <div class="clear"></div>

                <div class="txtfld-col">

                    <asp:CheckBox ID="chk_user_create" runat="server" />
                    <label>
                        <label for="Guardians_Don't create parent user">Don't Create Parent User</label></label>
                </div>

                <div class="clear"></div>
            </div>
        </div>
        <div class="clear"></div>

        <div>
            <asp:Button ID="btn_saveguardian" runat="server" OnClick="btn_saveguardian_Click" CssClass="formbut-n" Text="Save and Add Another" />&nbsp;			
            <div style="float: right;">
                <asp:Button ID="btn_savvenext" runat="server" OnClick="btn_savvenext_Click" CssClass="formbut-n" Text="Next" />&nbsp;			
            </div>

        </div>
        <asp:HiddenField ID="hd_id" runat="server" />
         <script type="text/javascript">
             function DeletePopup(id) {
                 $('#' + "<%=hd_id.ClientID %>").val(id);
                 $('.bs-example-modal-sm').modal('show');

            }
             function Primary_contact_Popup(id) {
                 $('#' + "<%=hd_id.ClientID %>").val(id);
                 $('.primary_contact_model').modal('show');

             }
             function Emergency_contact_Popup(id) {
                 $('#' + "<%=hd_id.ClientID %>").val(id);
                 $('.emergency_contact_model').modal('show');

             }

             function MakeContact(id) {
                 $('#' + "<%=hd_id.ClientID %>").val(id);
                 $('.make_contact_model').modal('show');

             }

             

             

    </script>

        <script type="text/javascript">
            $('#which_btn').val('0');
            $('#cnt_btn').click(function (ev) {
                $('#which_btn').val('1');
            });


            $(".check_all").change(function () {
                if (this.checked) {
                    $('.guardian_checkbox').attr('checked', true);
                }
                else {
                    $('.guardian_checkbox').attr('checked', false);
                }
            });

            $(".guardian_checkbox").change(function () {
                if ($('.guardian_checkbox:checked').length == $('.guardian_checkbox').length) {
                    $('.check_all').attr('checked', true);
                }
                else {
                    $('.check_all').attr('checked', false);
                }
            });

            $('#ex_submit_btn').click(function (ev) {
                var chks = $("[type='checkbox'][name='ex_guardian_id[]']:checked");
                if (chks.length == 0) {
                    alert('Select any Guardian');
                    return false;
                }
            });

            $(document).ready(function () {
                if ($('#relation_dropdown').val() == "Others") {
                    $("#relation_other").show();
                }
                $('#relation_dropdown').change(function () {
                    if (this.value == "Others") {
                        $("#relation_other").show("slow");
                    }
                    else {
                        $('#Guardians_relation_other').val('');
                        $("#relation_other").hide("slow");
                    }
                })
            });

        </script>

    </div>
    <script type="text/javascript">
        function getmode(guardian_mode) {
            if (guardian_mode == 1) {
                $("#search").show("slow");
                $("#chk_guardian2").attr('checked', false);
                $("#chk_guardian1").attr('checked', true);
                $("#chk_guardian2").attr('checked', '');
                $("#chk_guardian1").attr('checked', 'checked');
            }
            else {
                $("#search").hide("slow");
                $("#chk_guardian2").attr('checked', true);
                $("#chk_guardian1").attr('checked', false);
                $("#chk_guardian2").attr('checked', 'checked');
                $("#chk_guardian1").attr('checked', '');
            }
        }
    </script>
    <style type="text/css">
        .guard_search {
            position: relative;
            width: auto;
            padding-left: 5px;
            padding-right: 27px;
        }

        .radio_bx {
            margin: 0px;
            padding: 0px;
        }

        input[type="radio"]:checked + label span {
            background: url(images/radio_btn_css3.png) 0px top no-repeat;
        }

        div#show {
            padding: 10px;
        }

        .formConInner {
            padding: 15px;
            position: relative;
        }

        .formCon {
            margin-bottom: 20px;
        }

        .pdtab_Con table td {
            padding: 8px 2px !important;
        }
    </style>

    <script src="plugins/jQuery/jquery-2.2.3.min.js"></script>

    <script type="text/javascript">
        $(function () {
            $('#' + '<%= tbx_Guardians_dob.ClientID %>').datepicker({ autoclose: true });

        });
    </script>

     <div class="modal fade primary_contact_model" tabindex="-1" role="dialog" aria-labelledby="mySmallModalLabel">
        <div class="modal-dialog modal-sm" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                    <h4 class="modal-title" id="gridSystemModalLabel">Are you want to make primary contact Yes/No? </h4>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">No</button>
                    <button type="button" id="btn_makeprimary" runat="server" onserverclick="btn_makeprimary_ServerClick" class="btn btn-primary">Yes</button>
                </div>
            </div>
        </div>
    </div>

    <div class="modal fade emergency_contact_model" tabindex="-1" role="dialog" aria-labelledby="mySmallModalLabel">
        <div class="modal-dialog modal-sm" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                    <h4 class="modal-title" id="H1">Are you want to make emergency contact Yes/No? </h4>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">No</button>
                    <button type="button" id="btn_emergency" runat="server" onserverclick="btn_emergency_ServerClick" class="btn btn-primary">Yes</button>
                </div>
            </div>
        </div>
    </div>

       <div class="modal fade bs-example-modal-sm" tabindex="-1" role="dialog" aria-labelledby="mySmallModalLabel">
        <div class="modal-dialog modal-sm" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                    <h4 class="modal-title" id="H2">Are you want to delete Yes/No? </h4>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">No</button>
                    <button type="button" id="btn_del" runat="server" onserverclick="btn_del_ServerClick" class="btn btn-primary">Yes</button>
                </div>
            </div>
        </div>
    </div>

     <div class="modal fade make_contact_model" tabindex="-1" role="dialog" aria-labelledby="mySmallModalLabel">
        <div class="modal-dialog modal-sm" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                    <h4 class="modal-title" id="H3">Are you want to add guardian Yes/No? </h4>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">No</button>
                    <button type="button" id="btn_add_link" runat="server" onserverclick="btn_add_link_ServerClick" class="btn btn-primary">Yes</button>
                </div>
            </div>
        </div>
    </div>

    

</asp:Content>

