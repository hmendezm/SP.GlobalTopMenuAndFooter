<%@ Assembly Name="NIAMS.GlobalNavigationBar, Version=1.0.0.0, Culture=neutral, PublicKeyToken=a141880ec93edd04" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucGlobalNav.ascx.cs" Inherits="NIAMS.GlobalNavigationBar.ucGlobalNav" %>

<%@ Register src="ucBreadcrumb.ascx" tagname="ucBreadcrumb" tagprefix="uc1" %>


<SharePoint:CssRegistration after="corev4" name="/_layouts/NIAMS.GlobalNavigationBar/GlobalNavAndFooter.css" runat="server"/>
<SharePoint:CssRegistration after="corev4" name="/_layouts/NIAMS.GlobalNavigationBar/Menu.css" runat="server"/>

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

        $(function () {
            resizeModalDialog();


        });

        // wrapper which ensures SP.UI.Dialog.js is loaded before re-size fires.
        // (if you need to trigger a re-size in your init scripts as the JS library is loaded asyncronously)
        function resizeModalDialog() {
            SP.SOD.executeOrDelayUntilScriptLoaded(_resizeModalDialog, 'sp.ui.dialog.js');
        }
        
        function _resizeModalDialog() {
            // get the top-most dialog
            var dlg = SP.UI.ModalDialog.get_childDialog();
            if (dlg != null) {
                // dlg.$S_0 - is dialog maximized
                // dlg.get_$Z_0() - is dialog a modal
                if (!dlg.$S_0 && dlg.get_$Z_0()) {
                    // resize the dialog
                    dlg.autoSize();
                    var xPos, yPos, //x & y co-ordinates to move modal to...win = SP.UI.Dialog.get_$1(), // the very bottom browser window objectxScroll = SP.UI.Dialog.$24(win), // browser x-scroll posyScroll = SP.UI.Dialog.$26(win); // browser y-scroll pos
                    //SP.UI.Dialog.$1d(win) - get browser viewport width
                    //SP.UI.Dialog.$1c(win) - get browser viewport height
                    //dlg.$2_0 - modal's DOM element
                    // caculate x-pos based on viewport and dialog width
                    xPos = ((SP.UI.Dialog.$1d(win) - dlg.$2_0.offsetWidth) / 2) + xScroll;
                    // if x-pos is out of view (content too wide), re-position to left edge + 10px
                    if (xPos < xScroll + 10) {
                        xPos = xScroll + 10;
                    }
                    // caculate y-pos based on viewport and dialog height
                    yPos = ((SP.UI.Dialog.$1c(win) - dlg.$2_0.offsetHeight) / 2) + yScroll;
                    // if x-pos is out of view (content too high), re-position to top edge + 10px
                    if (yPos < yScroll + 10) {
                        yPos = yScroll + 10;
                    }
                    // store dialog's new x-y co-ordinates
                    dlg.$T_0 = xPos;
                    dlg.$U_0 = yPos;
                    // move dialog to x-y pos
                    dlg.$m_0(dlg.$T_0, dlg.$U_0);
                    // set dialog title bar text width
                    //dlg.$H_0 - dialog title text SPAN
                    //dlg.$6_0 - dialog title bar
                    dlg.$H_0.style.width = Math.max(dlg.$6_0.offsetWidth - 64, 0) + 'px';
                    // size down the dialog width/height if it's larger than browser viewport
                    dlg.$2B_0();
                }
            }
        }
    </script>
