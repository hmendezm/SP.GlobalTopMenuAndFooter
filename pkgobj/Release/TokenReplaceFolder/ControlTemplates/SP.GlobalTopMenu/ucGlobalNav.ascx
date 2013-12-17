<%@ Assembly Name="SP.GlobalTopMenu, Version=1.0.0.0, Culture=neutral, PublicKeyToken=4c344b065ad18c3c" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucGlobalNav.ascx.cs"
    Inherits="SP.GlobalTopMenu.ucGlobalNav" %>
<%@ Register Src="ucBreadcrumb.ascx" TagName="ucBreadcrumb" TagPrefix="uc1" %>
<SharePoint:CssRegistration After="corev4" Name="/_layouts/SP.GlobalTopMenu/css/Footer.css"
    runat="server" />
<SharePoint:CssRegistration After="corev4" Name="/_layouts/SP.GlobalTopMenu/css/Menu.css"
    runat="server" />
<%--
<script type="text/javascript">
if (typeof jQuery == 'undefined') {
var script = document.createElement('script');
script.type = "text/javascript";
script.src = "/_layouts/1033/GlobalMenu/jquery-1.8.2.js";
document.getElementsByTagName('head')[0].appendChild(script);

var script2 = document.createElement('script');
script.type = "text/javascript";
script.src = "/_layouts/1033/GlobalMenu/jquery-ui.js";
document.getElementsByTagName('head')[0].appendChild(script2);

}
</script>

<script type="text/javascript" language="javascript" src="/_layouts/1033/GlobalMenu/jquery-1.8.2.js"></script>
<script type="text/javascript" language="javascript" src="/_layouts/1033/GlobalMenu/jquery-ui.js"></script>
--%>
<ul runat="server" id="GlobalMenu" class="GlobalMenu">
</ul>
<uc1:ucBreadcrumb ID="ucBreadcrumb1" runat="server" />
<script type="text/javascript">
    $(document).ready(function () {

        if (typeof jQuery.ui == 'undefined') {
            var jqueryUIScript = document.createElement('script');
            jqueryUIScript.type = 'text/javascript';
            // Path to jquery-ui.js file.
            jqueryUIScript.src = '/_layouts/1033/GlobalMenu/jquery-ui.js';
            document.getElementsByTagName('head')[0].appendChild(jqueryUIScript);

        }
    });
    function openDialogModal(strUrl, strTitle) {
        var options = SP.UI.$create_DialogOptions();
        options.width = 900;
        options.height = 700;
        options.resizable = 1;
        options.scroll = 1;
        options.title = strTitle;
        options.url = strUrl;
        options.dialogReturnValueCallback = RefreshOnDialogClose;

        SP.UI.ModalDialog.showModalDialog(options);
        return false;
    }

    if (!window.jQuery) {
        var jqueryScript = document.createElement('script');
        jqueryScript.type = 'text/javascript';
        // Path to jquery-1.8.2.js file.
        jqueryScript.src = '/_layouts/1033/GlobalMenu/jquery-1.8.2.js';
        document.getElementsByTagName('head')[0].appendChild(jqueryScript);
    }
</script>
