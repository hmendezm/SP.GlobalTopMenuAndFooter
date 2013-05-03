<%@ Assembly Name="SP.GlobalTopMenu, Version=1.0.0.0, Culture=neutral, PublicKeyToken=4c344b065ad18c3c" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucGlobalNav.ascx.cs" Inherits="SP.GlobalTopMenu.ucGlobalNav" %>

<%@ Register src="ucBreadcrumb.ascx" tagname="ucBreadcrumb" tagprefix="uc1" %>


<SharePoint:CssRegistration after="corev4" name="/_layouts/SP.GlobalTopMenu/css/GlobalNavAndFooter.css" runat="server"/>
<SharePoint:CssRegistration after="corev4" name="/_layouts/SP.GlobalTopMenu/css/Menu.css" runat="server"/>

<script type="text/javascript" language="javascript" src="/_layouts/1033/GlobalMenu/jquery-1.9.1.js"></script>
<script type="text/javascript" language="javascript" src="/_layouts/1033/GlobalMenu/jquery-ui.js"></script>

<ul  runat="server" id="GlobalMenu" class="GlobalMenu">
</ul>


<uc1:ucBreadcrumb ID="ucBreadcrumb1" runat="server" />

    <script type="text/javascript">

        function openDialogModal(strUrl, strTitle) {
            var options = SP.UI.$create_DialogOptions();
            options.width = 900;
            options.height = 700;
            options.resizable = 1;
            options.scroll = 1;
            options.title = strTitle;
            options.url = strUrl;
            
            SP.UI.ModalDialog.showModalDialog(options);
            return false;
        }

     
    </script>
