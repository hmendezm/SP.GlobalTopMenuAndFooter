<%@ Assembly Name="NIAMS.GlobalNavigationBar, Version=1.0.0.0, Culture=neutral, PublicKeyToken=a141880ec93edd04" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Settings.aspx.cs" Inherits="NIAMS.GlobalNavigationBar.Layouts.NIAMS.GlobalNavigationBar.Settings" DynamicMasterPageFile="~masterurl/default.master" %>

<%@ Register tagname="ucSetting" tagprefix="ucSettings" src="~/_controltemplates/NIAMS.GlobalNavigationBar/ucSettings.ascx"  %>





<asp:Content ID="PageHead" ContentPlaceHolderID="PlaceHolderAdditionalPageHead" runat="server">

    <style type="text/css">
                body #s4-leftpanel {
                display:none;
                }
                .s4-ca {
                margin-left:0px;
                }

                #s4-ribbonrow, #s4-ribboncont,.s4-pr s4-ribbonrowhidetitle, #ms-cui-ribbonTopBars,
                .ms-cui-topBar2, .s4-notdlg,
                 
                .s4-notdlg noindex, #s4-titlerow,
                #s4-pr s4-notdlg s4-titlerowhidetitle, #s4-leftpanel-content,                
                .banner,#topNav 
                {
                display:none !important;
                }
        
                .s4-ca{margin-left:0px !important; margin-right:0px !important;}
                #s4-bodyContainer
                {
        
                width: 100% !important;
                clear:both;
                }
        
                //#pageContent
                //{
                //height:100% !important;
                //}

      
            </style>
</asp:Content>

<asp:Content ID="Main" ContentPlaceHolderID="PlaceHolderMain" runat="server">
    <div>
        <ucSettings:ucSetting id="ucSettings" runat="server" EnableViewState="true" AddUrl="true"/>

    </div>

</asp:Content>

<asp:Content ID="PageTitle" ContentPlaceHolderID="PlaceHolderPageTitle" runat="server">
Global Menu Settings
</asp:Content>

<asp:Content ID="PageTitleInTitleArea" ContentPlaceHolderID="PlaceHolderPageTitleInTitleArea" runat="server" >
Global Menu Settings
</asp:Content>
