using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class ViewerJS_Viewdoc : System.Web.UI.Page
{
    protected void Page_Init(object sender, EventArgs e)
    {
        mypanel.Style.Add("oncontextmenu", "return false;");
        mypanel.Style.Add("class", "NonPrintable");
        mypanel.Style.Add("oncopy", "return false;");
        mypanel.Style.Add("onpaste", "return false;");
        mypanel.Style.Add("oncut", "return false;");


        bodyid.Style.Add("oncontextmenu", "return false;");
        bodyid.Style.Add("class", "NonPrintable");
        bodyid.Style.Add("oncopy", "return false;");
        bodyid.Style.Add("onpaste", "return false;");
        bodyid.Style.Add("oncut", "return false;");

    }
    protected void Page_Load(object sender, EventArgs e)
    {
        bodyid.Style.Add("oncontextmenu", "return false;");
        bodyid.Style.Add("class", "NonPrintable");
        bodyid.Style.Add("oncopy", "return false;");
        bodyid.Style.Add("onpaste", "return false;");
        bodyid.Style.Add("oncut", "return false;");

        
        

    }
}