<%@ Assembly Name="SP.GlobalTopMenu, Version=1.0.0.0, Culture=neutral, PublicKeyToken=4c344b065ad18c3c" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Settings.aspx.cs" 
                Inherits="SP.GlobalTopMenu.Layouts.SP.GlobalTopMenu.Settings" 
                 MasterPageFile="~/_layouts/dialog.master"  %>

<%@ Register tagname="ucSetting" tagprefix="ucSettings" src="~/_controltemplates/SP.GlobalTopMenu/ucSettings.ascx"  %>


<asp:Content ID="Content1" ContentPlaceHolderID="PlaceHolderDialogHeaderPageTitle" runat="server">SP GlobalTopMenu Navigation</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="PlaceHolderAdditionalPageHead" runat="server">

  <SharePoint:CssRegistration ID="owscss" runat="server" Name="ows.css" />

  <SharePoint:ScriptLink ID="corejs" Language="javascript" Name="core.js" runat="server" />

  <script type="text/javascript" language="javascript" src="/_layouts/1033/GlobalMenu/jquery-1.8.2.js"></script>
  <script type="text/javascript" language="javascript" src="/_layouts/1033/GlobalMenu/jquery-ui.js"></script>

  <SharePoint:FormDigest ID="FormDigest1" runat="server" />

  <SharePoint:CssRegistration ID="Menucss" after="corev4" name="/_layouts/SP.GlobalTopMenu/css/Menu.css" runat="server"/>
  <SharePoint:CssRegistration ID="jqueryuicss" after="corev4" name="/_layouts/SP.GlobalTopMenu/css/jquery-ui.css" runat="server"/>
  <SharePoint:CssRegistration ID="settingscss" after="corev4" name="/_layouts/SP.GlobalTopMenu/css/settings.css" runat="server"/>
  <script type="text/javascript" language="javascript">

   
      $(function () {
          //resizeModalDialog();
          resize();
          hideButtons();
      });

      function hideButtons() {

          $("#buttonRow").css('display', 'none');
      }
      function resize() {
                var width = $('#GTMTable').outerWidth()+100;
                window.setTimeout(
                function () {
                    $(window.frameElement).parents('.ms-dlgContent').width(width);
                    $(window.frameElement).parents('.ms-dlgContent').find('.ms-dlgBorder, .ms-dlgTitle, .ms-dlgFlame').width(width);
                    $(window.frameElement).width(width);
                }, 30);
      }


      function resizeModalDialog(width, height) {

          $(window.frameElement).parents('.ms-dlgContent').width(width);
          $(window.frameElement).parents('.ms-dlgContent').find('.ms-dlgBorder, .ms-dlgTitle').width(width);
         
          
          $(window.frameElement).width(width);

          try {
              var _intHeight = parseInt(height);
              height = _intHeight + 34; //34 is added to the height,the title bar's height adds up
          }
          catch (ex) {
          }


          $(window.frameElement).parents('.ms-dlgContent').height(height);
          $(window.frameElement).parents('.ms-dlgContent').find('.ms-dlgBorder').height(height);
          $(window.frameElement).height(height);
      }
    
  </script>


</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="PlaceHolderDialogImage" runat="server">
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="PlaceHolderDialogBodyHeaderSection" runat="server">
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="PlaceHolderDialogDescription" runat="server">
</asp:Content>

<asp:Content ID="Content8" ContentPlaceHolderID="PlaceHolderHelpLink" runat="server">

  <!-- Remove the default help link -->

</asp:Content>

<asp:Content ID="Content6" ContentPlaceHolderID="PlaceHolderDialogBodyMainSection"     runat="server">
      <div>
        <ucSettings:ucSetting id="ucSettings" runat="server" EnableViewState="true"/>

    </div>
</asp:Content>

