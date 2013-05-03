<%@ Assembly Name="NIAMS.GlobalNavigationBar, Version=1.0.0.0, Culture=neutral, PublicKeyToken=a141880ec93edd04" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucSettings.ascx.cs" Inherits="NIAMS.GlobalNavigationBar.ControlTemplates.NIAMS.GlobalNavigationBar.ucSettings" %>

<%@ Register src="ucGlobalNav.ascx" tagname="ucGlobalNav" tagprefix="uc1" %>

<asp:XmlDataSource ID="XmlDSGroupNames" DataFile="GroupNames.xml"  TransformFile="GroupNames.xslt" runat="server"></asp:XmlDataSource>

<SharePoint:CssRegistration after="corev4" name="/_layouts/NIAMS.GlobalNavigationBar/Menu.css" runat="server"/>
<SharePoint:CssRegistration after="corev4" name="/_layouts/NIAMS.GlobalNavigationBar/jquery-ui.css" runat="server"/>
<SharePoint:CssRegistration after="corev4" name="/_layouts/NIAMS.GlobalNavigationBar/settings.css" runat="server"/>


<script type="text/javascript" language="javascript" src="/_layouts/1033/GlobalMenu/jquery-1.9.1.js"></script>
<script type="text/javascript" language="javascript" src="/_layouts/1033/GlobalMenu/jquery-ui.js"></script>

<script type="text/javascript">
    $(function () {

        var activeIndex = parseInt($('#<%=hidAccordionIndex.ClientID %>').val());
        $("#accordion").accordion({
            active: activeIndex,
            collapsible: true,
            heightStyle: "content",
            autoHeight: true,
            activate: function (event, ui) {
                var activeIndex = $("#accordion").accordion("option", "active");
                $("#<% =hidAccordionIndex.ClientID %>").val(activeIndex);

            }
        });
    
        $("input[type=submit],button")
        .button()
    
    });

    function OnClientClickGroupDelete() {
        return window.confirm("Are you sure you want to delete this Group?");
    }

    function OnClientClickGroupDeleteMenuItem() {
        return window.confirm("Are you sure you want to delete this Option from the Menu?");
    }
        
    function openDialogModal(strUrl, strTitle) {
        var options = SP.UI.$create_DialogOptions();
        options.width = 900;
        options.height = 500;
        options.resizable = 1;
        options.scroll = 1;
        options.title = strTitle;
        options.url = strUrl;

        SP.UI.ModalDialog.showModalDialog(options);
        return false;
    }

</script>

<h2>Global Menu Settings</h2>

<table  style="height:100%;width:880px">
    <tr>
        <td width="30px" valign="baseline">

            <asp:ImageButton CssClass="Tools" 
                             ID="lmgbtnSettings" 
                             OnClientClick="return openDialogModal('/_layouts/Niams.GlobalNavigationBar/preview.aspx', 'Global Navigation Preview')" 
                             ImageUrl="/_layouts/NIAMS.GlobalNavigationBar/Images/Menu/tools.png" runat="server" />

            <asp:TreeView ID="trvGlobalNavFooter" 
                          OnSelectedNodeChanged="trvGlobalNavFooter_SelectedNodeChanged"  runat="server" >
            </asp:TreeView>
        </td>
        <td width="600px">
            <div id="accordion">
                <h3>Setting</h3>
                <div id="EditSettings_DIV" >
                    <br/>
                    <div class="SiteTitle">
                        <asp:Label ID="lblSelectedSite" runat="server">Selected Site: </asp:Label>
                        <asp:Label ID="lblSiteTite" runat="server"></asp:Label>
                    </div>
                    <br />

                    <div class="PanelContent" >
                        <asp:CheckBox ID="chkAddToFooter"   runat="server" />
                        <asp:Label ID="lblAddNIAMSFooter" runat="server">Add this Site to NIAMS Footer</asp:Label>
                    </div>

                    <br/>

                    <div class="PanelContent" >
                        <asp:CheckBox ID="chkAddToGlobalNav" runat="server" />
                        <asp:Label ID="lblNIAMSNavBar" runat="server">Add this Site to NIAMS Global Navigation Bar</asp:Label>
                    </div>

                    <br/>

                    <div class="PanelContent" >
                        <asp:Label ID="lblPosition" runat="server">Position: </asp:Label>
                        <asp:DropDownList ID="rcbPositions" runat="server" Width="448px"></asp:DropDownList>

                    </div>

                    <br/>
                    <div class="PanelContent" >
                        <asp:Label ID="lblQuestion" runat="server">Select the name of the group that you want this site be included?(Optional)</asp:Label>
                    </div>

                    <div class="PanelContent" >

                        <asp:DropDownList ID="ddlGroupNames" runat="server" Width="448px"></asp:DropDownList>

                    </div>
                    <br />
                    <div class="PanelContent" >
                        <asp:Label ID="lblChangeTitle" runat="server">Change Title to : </asp:Label>
                        <asp:TextBox ID="rdtxtChangeTitle" runat="server"></asp:TextBox>
                    </div>
                    <br/>
                    <br />
                    <div id="Div1">
                        <div class="row">
                            <div>
                                <input type="submit" runat="server" value="Save" onserverclick="btnSaveMenuItem_Click" id="btnSaveMenuItem" />
                            </div>
                        </div>
                    </div>

                </div>

                <h3>Site Information</h3>
                <div id="SiteInfo_DIV" >
                    <br />
                    <div class="row" >
                        <div class="left">
                            <h6>Description: </h6>
                        </div>
                        <div class="right">
                            <asp:Label ID="lblSiteDescription" runat="server"></asp:Label>
                        </div>
                    </div>
                    <br />
                    <div class="row" >
                        <div class="left">
                            <h6>Custom Master Url: </h6>
                        </div>
                        <div class="right">
                            <asp:Label ID="lblCustomMasterUrl" runat="server"></asp:Label>
                        </div>
                    </div>
                    <br />
                    <div class="row" >
                        <div class="left">
                            <h6>Alternate Css Url: </h6>
                        </div>
                        <div class="right">
                            <asp:Label ID="lblAlternateCssUrl" runat="server"></asp:Label>
                        </div>
                    </div>
                    <br />
                    <div class="row" >
                        <div class="left">
                            <h6>Has Unique Permissions: </h6>
                        </div>
                        <div class="right">
                            <asp:Label ID="lblHasUniquePerm" runat="server"></asp:Label>
                        </div>
                    </div>
                    <br />
                    <div class="row" >
                        <div class="left">
                            <h6>Settings Page: </h6>
                        </div>
                        <div class="right">
                            <a id="aSettingsPage" runat="server" ></a>
                        </div>
                    </div>

                    <br />
                </div>


                <h3>Site Security</h3>
                <div>
                    <asp:GridView ID="rgSiteAdmins" runat="server" AllowPaging="True" 
                                  AllowSorting="True" PageSize="4"
                                  AutoGenerateColumns="false" cellpadding="0" cellspacing="0" 
                                  border="0" class="display" 
                        onpageindexchanging="rgSiteAdmins_PageIndexChanging" 
                        onsorting="rgSiteAdmins_Sorting" 
                        CssClass="mGrid"  
                        PagerStyle-CssClass="pgr"  
                        AlternatingRowStyle-CssClass="alt">
                        <columns>
                            <asp:BoundField HeaderText="Name" DataField="Name"  />
                            <asp:BoundField HeaderText="LoginName" DataField="LoginName" />
                            <asp:BoundField HeaderText="Group" DataField="Group" HtmlEncode="false"/>
                            <asp:BoundField HeaderText="IsSiteAdmin" DataField="IsSiteAdmin" />
                        </columns>
                    </asp:GridView>

                </div>

                <h3>Groups and Subgroups</h3>
                <div id="Groups_SubGroupsDIV" >
                    <div class="row">
                        <div class="left">
                            <asp:TreeView ID="trvGroups" runat="server" 
                                          onselectednodechanged="trvGroups_SelectedNodeChanged" >
                            </asp:TreeView>

                        </div>
                        <div class="right">
                            <br />
                            <asp:TextBox style="display:none" ID="txtGroupID"  Text="0" runat="server"></asp:TextBox>
                            <h8>Title: </h8>
                            <div class="right">
                                <asp:TextBox ID="txtGroupName" runat="server" Width="375px" Enabled="False"></asp:TextBox>
                            </div>
                            <br />
                            <h8>Description: </h8>
                            <div class="right">
                                <asp:TextBox ID="txtGroupDescription" runat="server" Height="120px" TextMode="MultiLine" 
                                             Width="381px" Enabled="False"></asp:TextBox>
                            </div>
                            <br />
                            <h8>Position: </h8>
                            <div class="right">
                                <asp:TextBox ID="txtPosition" runat="server" Width="100px" Enabled="False"></asp:TextBox>
                            </div>
                            <br />
                            <h8>Parent: </h8>
                            <div class="right">
                                <asp:DropDownList ID="ddlParentGroups" runat="server" Width="400px" 
                                                  Enabled="False"></asp:DropDownList>
                            </div>
                            <br />
                            <div id="Group_SubGroupMenuDIV">
                                <div class="row">
                                    <div style="float:left;">

                                        <input type="submit" runat="server" value="Edit" onserverclick="btnSaveGroup_Click"  id="btnSaveGroup" />

                                    </div>
                                    <div style="float:left;">
                                        <input type="submit" runat="server" value="Add" onserverclick="btnAddGroup_Click" id="btnAddGroup" />

                                    </div>
                                    <div style="float:left;">
                                        <input type="submit" runat="server" value="Delete" onserverclick="btnDeleteGroup_Click" id="btnDeleteGroup" onclick="if (! OnClientClickGroupDelete())  return false;" />

                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>


            </div>
        </td>
    </tr>
</table>

<asp:TextBox style="display:none" ID="hidAccordionIndex"  Text="0" runat="server"></asp:TextBox>
