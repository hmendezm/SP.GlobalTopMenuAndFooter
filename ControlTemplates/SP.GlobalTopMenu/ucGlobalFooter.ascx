<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucGlobalFooter.ascx.cs" Inherits="SP.GlobalTopMenu.ucGlobalFooter" %>

<SharePoint:CssRegistration ID="CssRegistration1" After="corev4" name="/_layouts/SP.GlobalTopMenu/css/GlobalNavAndFooter.css" runat="server"/>

<div id="Footer">
    <h2>Page Footer</h2>
    <ul runat="server" id="firstrow">
    </ul>
   <%-- <ul>
        <li class="last">
            <a href="http://niamsinside.niams.nih.gov/" title="Link to NIAMS Intranet" target="ExternalLink">
                <img src="/_layouts/SP.GlobalTopMenu/Images/intranet_logo.gif" alt="Link to NIAMS Intranet" title="Link to NIAMS Intranet" border="0">
            </a>
        </li>
        <li class="last">
            <a href="http://www.niams.nih.gov/" title="Link to NIAMS Web Site" target="ExternalLink">
                <img src="/_layouts/SP.GlobalTopMenu/Images/niams_logo.gif" alt="Link to NIAMS Web Site" title="Link to NIAMS Web Site" border="0">
            </a>
        </li>
        <li class="last">
            <a href="http://www.nih.gov/" title="Link to National Institutes of Health Web Site" target="ExternalLink">
                <img src="/_layouts/SP.GlobalTopMenu/Images/nih_logo.gif" alt="Link to National Institutes of Health Homepage" title="Link to National Institutes of Health Homepage" border="0">
            </a>
        </li>
        <li class="last">
            <a href="http://www.hhs.gov/" title="Link to Department of Health and Human Services Homepage" target="ExternalLink">
                <img src="/_layouts/SP.GlobalTopMenu/Images/dhhs_logo.gif" alt="Link to Department of Health and Human Services Homepage" title="Link to Department of Health and Human Services Homepage" border="0">
            </a>
        </li>
        <li class="last">
            <a href="http://www.usa.gov" title="Link to the U.S. Government's official web portal." target="ExternalLink">
                <img src="/_layouts/SP.GlobalTopMenu/Images/usagov.gif" alt="Link to the U.S. Government's official web portal." title="NIH Logo" border="0">
            </a>
        </li>

    </ul>--%>
    <p class="address">Briefing Description</p>
  
</div>