<%@ Assembly Name="SP.GlobalTopMenu, Version=1.0.0.0, Culture=neutral, PublicKeyToken=4c344b065ad18c3c" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucGlobalFooter.ascx.cs"
    Inherits="SP.GlobalTopMenu.ucGlobalFooter" %>
<SharePoint:CssRegistration ID="CssRegistration1" After="corev4" Name="/_layouts/SP.GlobalTopMenu/css/Footer.css"
    runat="server" />
<div id="Footer">
    <ul runat="server" id="firstrow">
    </ul>
    <ul>
        <li class="last"><a href="http://niamsinside.niams.nih.gov/" title="Link to NIAMS Intranet"
            target="ExternalLink">NIAMS Intranet</a> </li>
        <li class="last"><a href="http://www.niams.nih.gov/" title="Link to NIAMS Web Site"
            target="ExternalLink">NIAMS Web Site</a> </li>
        <li class="last"><a href="http://www.nih.gov/" title="Link to National Institutes of Health Web Site"
            target="ExternalLink">National Institutes of Health</a> </li>
        <li class="last"><a href="http://www.hhs.gov/" title="Link to Department of Health and Human Services Homepage"
            target="ExternalLink">Department of Health and Human Services</a> </li>
        <li class="last"><a href="http://www.usa.gov" title="Link to the U.S. Government's official web portal."
            target="ExternalLink">U.S. Government's official</a> </li>
    </ul>
</div>
