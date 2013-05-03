<%@ Assembly Name="SP.GlobalTopMenu, Version=1.0.0.0, Culture=neutral, PublicKeyToken=4c344b065ad18c3c" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucBreadcrumb.ascx.cs" Inherits="SP.GlobalTopMenu.ucBreadcrumb" %>
   
<SharePoint:CssRegistration after="corev4" name="/_layouts/SP.GlobalTopMenu/xbreadcrumbs.css" runat="server"/>

<script type="text/javascript" language="javascript" src="/_layouts/1033/GlobalMenu/xbreadcrumbs.js"></script>

<script type="text/javascript">
	$(document).ready(function(){
        //  Initialize xBreadcrumbs
        //$('#breadcrumbs-1').xBreadcrumbs({ collapsible: true });
	    $('#<%=breadcrumbs.ClientID%>').xBreadcrumbs();
	});
</script>


<style type="text/css">
    .xbreadcrumbs LI {
        border-right: none;
        background: url('/_layouts/SP.GlobalTopMenu/Images/xbreadcrumb/separator.gif') no-repeat right center;
        padding-right: 15px;
        padding-left: 10px;
    }
    
    
    
    .xbreadcrumbs LI.current { background: none; }
    .xbreadcrumbs LI UL LI { background: none; }
    
    .xbreadcrumbs LI A.home {
        background: url('/_layouts/SP.GlobalTopMenu/Images/xbreadcrumb/home.gif') no-repeat left center;
        padding-left: 20px;
    }
</style>

<ul  class="xbreadcrumbs" id="breadcrumbs"  runat="server" >
</ul>
<%-- <ul class="xbreadcrumbs" id="breadcrumbs-1">
            <li>
               <a href="#" class="home">Home</a>
               <ul>
                   <li><a href="#">Scripts</a></li>
                   <li><a href="#">Tutorials</a></li>
                   <li><a href="#">About Us</a></li>
                   <li><a href="#">Advertise With Us</a></li>
                   <li><a href="#">Contact Us</a></li>
               </ul>
            </li>
            <li>
               <a href="#">Scripts</a>
               <ul>
                   <li><a href="#">jQuery</a></li>
                   <li><a href="#">MooTools</a></li>
                   <li><a href="#">script.aculo.us</a></li>
                   <li><a href="#">ExtJS</a></li>
               </ul>
            </li>
            <li>
               <a href="#">jQuery Framework</a>
               <ul>
                   <li><a href="#">bgStretcher</a></li>
                   <li><a href="#">QueryLoader</a></li>
                   <li><a href="#">qTip</a></li>
                   <li><a href="#">jGrowl</a></li>
                   <li><a href="#">FancyBox</a></li>
               </ul>
            </li>
            <li class="current"><a href="#">xBreadcrumbs (Extended Breadcrumbs) jQuery Plugin Demo</a></li>
        </ul>
          <div class="clear"></div>
        <pre class="code"><code>
                $('#breadcrumbs-1').xBreadcrumbs({ collapsible: true });        
        </code></pre>
        <div class="vspacer">&nbsp;</div>--%>
