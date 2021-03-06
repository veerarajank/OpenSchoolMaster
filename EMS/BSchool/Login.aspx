<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
   <title>IIKM - B-School Management System</title>
    <!-- Tell the browser to be responsive to screen width -->
    <meta content="width=device-width, initial-scale=1, maximum-scale=1, user-scalable=no" name="viewport" />
    <!-- Bootstrap 3.3.6 -->
    <link rel="stylesheet" href="bootstrap/css/bootstrap.min.css" />
     <link rel="icon" href="favicon.ico" />
    <!-- Font Awesome -->
    <link rel="stylesheet" href="bootstrap/css/font-awesome.min.css" />
    <!-- Ionicons -->
    <link rel="stylesheet" href="bootstrap/css/ionicons.min.css" />
    <!-- Theme style -->
    <link rel="stylesheet" href="dist/css/googleapicss.css" />
    <link rel="stylesheet" href="dist/css/AdminLTE.css" />
    <!-- iCheck -->
    <link rel="stylesheet" href="plugins/iCheck/square/blue.css" />

    <!-- HTML5 Shim and Respond.js IE8 support of HTML5 elements and media queries -->
    <!-- WARNING: Respond.js doesn't work if you view the page via file:// -->
    <!--[if lt IE 9]>
  <script src="https://oss.maxcdn.com/html5shiv/3.7.3/html5shiv.min.js"></script>
  <script src="https://oss.maxcdn.com/respond/1.4.2/respond.min.js"></script>
  <![endif]-->

    <style>
        .embossed{
  color: #f0f0f0;
  font-size: 100px;
  font-family: Futura;
  background-color: #666666;
  text-shadow: 1px 4px 4px #555;
  text-align: center;
  -webkit-background-clip: text;
    -moz-background-clip: text;

}
    </style>
</head>
<body class="hold-transition login-page" style="background-color:#d2d6de;background-image:">
    <form id="form1" runat="server">


      


        <div class="login-box" style="width: 500px;">            
            <div class="login-box-body" style="width: 500px;background-color:#fff;border:1px solid #808080;background-repeat: no-repeat;background-size: 100% 100%;">
                    
                <a href="http://www.iikm.in/"><b>                    
                    
                    <img src="Images/logo.jpg" class="img-responsive"  alt=" IIKM, The Corporate B-School in Chennai" style="margin-left: -15px;margin-top:-20px"></b></a>                   
                
               
                        <br />
                
                <div class="login-box" style="margin-top: 0px; margin-bottom: 0px;margin-left:-10px;width:350px;">

                    

                    <p style="margin-left: 0px;color:#125490">Sign in to start your session</p>
                    <span class="os-error" runat="server" id="spn_error" style="color:red" visible="false">The username or password you entered is incorrect.</span>
                    <div class="form-group has-feedback">
                        <div class="input-group">
                          <span class="input-group-addon" style="background-color: transparent;">
                                <i class="fa fa-user"></i>
                            </span>
                        <input type="text" id="txt_username" style="background-color: transparent;color:#125490"  runat="server" autocomplete="off" class="form-control" placeholder="Employee ID"/>
                        </div>

                    </div>
                    <div class="form-group has-feedback embossed" >
                        <div class="input-group" >
                            <span class="input-group-addon"  style="background-color: transparent;">
                                <i class="fa fa-key"></i>
                            </span>

                        <input type="password" id="txt_password" style="background-color: transparent;font-weight:bold;color:#125490" runat="server" class="form-control" placeholder="Password" />
                        </div>
                    </div>
                     
                    <div class="form-group has-feedback" style="margin-bottom:-12px;">
                        

                        <div class="row" style="margin-top:5px;">
                            <div class="col-xs-4">
                            
                            </div>       
                            <div class="col-xs-4">
                                         </div>                   
                            <div class="col-xs-4">                                
                                <button type="submit" id="btn_submit" runat="server" onserverclick="btn_submit_onserverclick" class="btn btn-primary btn-block btn-flat">
                                     <i class="fa fa-sign-in"></i>
                                    Sign In</button>
                                <img src="Images/gs247_logo.png" class="img-responsive"  alt=" IIKM, The Corporate B-School in Chennai" style="margin-top:-115px;margin-left:120px;padding-top:-20px" />
                            </div>
                            <!-- /.col -->
                             
                
                            
                        </div>
                        <div class="row" style="margin-top:5px;">
                            <div class="col-xs-4">
                        <label id="lbl_error" class="label label-warning" style="font-size:12px" runat="server"></label>
                              </div></div>
                    </div>


                </div>
               
                  

            </div>

            

             <footer class="btn btn-block btn-flat" style="background-color: rgba(0,0,0,0.55); color: white; border: 1px solid #808080; text-align: left;" >
                    <div class="pull-right hidden-xs">
                        <b>V</b> 1.0.0
                    </div>
                    <strong>© 2019 <a href="http://gs247.co.in/"><span style="color: white">Global Serve 247 (GLSRV247)</span></a></strong>
                </footer>
        <!-- /.login-box -->
            </div>       
       

        <!-- jQuery 2.2.3 -->
        <script src="plugins/jQuery/jquery-2.2.3.min.js"></script>
        <!-- Bootstrap 3.3.6 -->
        <script src="bootstrap/js/bootstrap.min.js"></script>
        <!-- iCheck -->
        <script src="plugins/iCheck/icheck.min.js"></script>
        <script>
            $(function () {
                $('input').iCheck({
                    checkboxClass: 'icheckbox_square-blue',
                    radioClass: 'iradio_square-blue',
                    increaseArea: '20%' // optional
                });
            });
        </script>

    </form>
</body>
</html>
