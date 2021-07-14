<%@ Page Title="" Language="C#" MasterPageFile="~/Admin_Settings.master" AutoEventWireup="true" CodeFile="Adm_settingsDashboard.aspx.cs" Inherits="Adm_settingsDashboard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="cont_right formWrapper">
        <h1>Manage Dashboard Blocks</h1>

        <div class="button-bg">
            <div class="top-hed-btn-left"></div>
            <div class="top-hed-btn-right">
                <ul>
                    <li><asp:LinkButton runat="server" id="btn_reset" class="a_tag-btn" OnClick="btn_reset_Click" ><span>Reset Order</span></asp:LinkButton></li>
                    <li><asp:LinkButton runat="server" id="btn_enableall" class="a_tag-btn" OnClick="btn_enableall_Click" ><span>Enable All</span></asp:LinkButton></li>
                    <li><asp:LinkButton runat="server" id="btn_disable" class="a_tag-btn" OnClick="btn_disable_Click" ><span>Disable All</span></asp:LinkButton></li>
                    
                </ul>
            </div>
        </div>

        <div class="tablebx" id="div_module_list" runat="server">
        </div>
    </div>


    <script src="plugins/jQuery/jquery-2.2.3.min.js"></script>


    <asp:HiddenField ID="hd_id" runat="server" />
    <script src="plugins/jQuery/jquery-2.2.3.min.js"></script>
    <script type="text/javascript">
        function DisableModule(id) {
            $('#' + "<%=hd_id.ClientID %>").val(id);
             $('.bs-example-modal-sm').modal('show');

         }
         function EnableModule(id) {
             $('#' + "<%=hd_id.ClientID %>").val(id);
             $('.bs-example-modal-sm2').modal('show');

         }
    </script>
    <div class="modal fade bs-example-modal-sm" tabindex="-1" role="dialog" aria-labelledby="mySmallModalLabel">
        <div class="modal-dialog modal-info" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                    <h4 class="modal-title" id="gridSystemModalLabel">Are you want to disable this block? </h4>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">Cancel</button>
                    <button type="button" id="btn_del" runat="server" onserverclick="btn_del_ServerClick" class="btn btn-primary">OK</button>
                </div>
            </div>
        </div>
    </div>
    <div class="modal fade bs-example-modal-sm2" tabindex="-1" role="dialog" aria-labelledby="mySmallModalLabel">
        <div class="modal-dialog modal-info" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                    <h4 class="modal-title" id="H1">Are you want to enable this block? </h4>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">Cancel</button>
                    <button type="button" id="btn_enable" runat="server" onserverclick="btn_enable_ServerClick" class="btn btn-primary">OK</button>
                </div>
            </div>
        </div>
    </div>
</asp:Content>

