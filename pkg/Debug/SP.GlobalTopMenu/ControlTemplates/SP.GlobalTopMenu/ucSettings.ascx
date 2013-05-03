<%@ Assembly Name="SP.GlobalTopMenu, Version=1.0.0.0, Culture=neutral, PublicKeyToken=4c344b065ad18c3c" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucSettings.ascx.cs"
    Inherits="SP.GlobalTopMenu.ucSettings" %>
<%@ Register Assembly="Obout.Ajax.UI, Version=2.13.412.4, Culture=neutral, PublicKeyToken=24f3c5cea456f322"
    Namespace="Obout.Ajax.UI.TreeView" TagPrefix="obout" %>
<%@ Register Src="ucGlobalNav.ascx" TagName="ucGlobalNav" TagPrefix="uc1" %>
<asp:XmlDataSource ID="XmlDSGroupNames" DataFile="GroupNames.xml" TransformFile="GroupNames.xslt"
    runat="server"></asp:XmlDataSource>
<script type="text/javascript">

    $(function () {
        var activeIndex = parseInt($('#<%=hidAccordionIndex.ClientID %>').val());
        $("#accordion").accordion({
            active: activeIndex,
            collapsible: true,
            heightStyle: "auto",
            autoHeight: false,
            change: function (event, ui) {
                var activeIndex = $("#accordion").accordion("option", "active");
                $("#<% =hidAccordionIndex.ClientID %>").val(activeIndex);

            }
        });

        $("input[type=submit],button").button();

    });

    function OnClientClickGroupDelete() {
        return window.confirm("Are you sure you want to delete this Group?");
    }

    function OnClientClickGroupDelete() {
        return window.confirm("Are you sure you want to delete this External Link?");
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
<h2>
    Global Menu Settings</h2>
<table style="height: 100%; width: 880px" id="GTMTable">
    <tr style="display: none">
        <td width="30px" valign="baseline">
            &nbsp;
        </td>
        <td width="600px">
            <asp:CheckBox ID="chkIncludeBreadCrumb" runat="server" Text="Include BreadCrumb" />
        </td>
    </tr>
    <tr>
        <td width="30px" valign="top">
            <asp:ImageButton CssClass="Tools" ID="lmgbtnSettings" OnClientClick="return openDialogModal('/_layouts/SP.GlobalTopMenu/preview.aspx', 'Global Navigation Preview')"
                ImageUrl="/_layouts/SP.GlobalTopMenu/Images/Menu/tools.png" runat="server" />
            <asp:TreeView ID="trvGlobalNavFooter" NodeStyle-CssClass="treeNode" RootNodeStyle-CssClass="rootNode"
                LeafNodeStyle-CssClass="leafNode" OnSelectedNodeChanged="trvGlobalNavFooter_SelectedNodeChanged"
                runat="server">
            </asp:TreeView>
        </td>
        <td width="600px" valign="top">
            <div id="accordion">
                <h3>
                    <a href="#">Option Setting</a></h3>
                <div id="EditSettings_DIV">
                    <div class="space">
                        <asp:Label ID="lblSelectedSite" CssClass="SubTitle" runat="server">Selected Site: </asp:Label>
                        <asp:Label ID="lblSiteTite" CssClass="Text" runat="server"></asp:Label>
                    </div>
                    <div class="space">
                        <asp:CheckBox ID="chkAddToFooter" CssClass="Text" runat="server" />
                        <asp:Label ID="lblAddFooter" CssClass="SubTitle" runat="server">Add this Site to  Footer</asp:Label>
                    </div>
                    <div class="space">
                        <asp:CheckBox ID="chkAddToGlobalNav" runat="server" />
                        <asp:Label ID="lblNavBar" CssClass="SubTitle" runat="server">Add this Site to  Global Navigation Bar</asp:Label>
                    </div>
                    <div class="space">
                        <asp:Label ID="lblPosition" CssClass="SubTitle" runat="server">Position: </asp:Label>
                        <asp:DropDownList ID="rcbPositions" CssClass="Text" runat="server" Width="448px">
                        </asp:DropDownList>
                    </div>
                    <div class="space">
                        <asp:Label ID="lblQuestion" CssClass="SubTitle" runat="server">Select the name of the group that you want this site be included?(Optional)</asp:Label>
                        <asp:DropDownList ID="ddlGroupNames" CssClass="Text" runat="server" Width="448px">
                        </asp:DropDownList>
                    </div>
                    <div class="space">
                        <asp:Label ID="lblChangeTitle" CssClass="SubTitle" runat="server">Change Title to : </asp:Label>
                        <asp:TextBox ID="rdtxtChangeTitle" CssClass="Text" runat="server"></asp:TextBox>
                    </div>
                    <div class="space">
                        <div class="row">
                            <div>
                                <input type="submit" runat="server" class="Text" value="Save" onserverclick="btnSaveMenuItem_Click"
                                    id="btnSaveMenuItem" />
                            </div>
                        </div>
                    </div>
                </div>
                <h3>
                    <a href="#">Site Information</a></h3>
                <div id="SiteInfo_DIV">
                    <div class="space">
                        <asp:Label ID="SiteInfoDescription" CssClass="SubTitle" runat="server">Description : </asp:Label>
                        <asp:Label ID="lblSiteDescription" class="Text" runat="server"></asp:Label>
                    </div>
                    <div class="space">
                        <asp:Label ID="SiteInfoMaster" CssClass="SubTitle" runat="server">Custom Master Url:</asp:Label>
                        <asp:Label ID="lblCustomMasterUrl" class="Text" runat="server"></asp:Label>
                    </div>
                    <div class="space">
                        <asp:Label ID="SiteInfoCss" CssClass="SubTitle" runat="server">Alternate Css Url:</asp:Label>
                        <asp:Label ID="lblAlternateCssUrl" class="Text" runat="server"></asp:Label>
                    </div>
                    <div class="space">
                        <asp:Label ID="SiteInfoPermissions" CssClass="SubTitle" runat="server">Has Unique Permissions:</asp:Label>
                        <asp:Label ID="lblHasUniquePerm" class="Text" runat="server"></asp:Label>
                    </div>
                    <div class="space">
                        <asp:Label ID="SiteInfoSettingsPage" CssClass="SubTitle" runat="server">Settings Page:</asp:Label>
                        <a id="aSettingsPage" class="Text" runat="server"></a>
                    </div>
                </div>
                <h3>
                    <a href="#">Site Security</a></h3>
                <div id="Site_SecurityDIV">
                    <asp:GridView ID="rgSiteAdmins" runat="server" AllowPaging="True" AllowSorting="True"
                        PageSize="4" AutoGenerateColumns="false" CellPadding="0" CellSpacing="0" border="0"
                        class="mGrid" OnPageIndexChanging="rgSiteAdmins_PageIndexChanging" OnSorting="rgSiteAdmins_Sorting"
                        CssClass="mGrid" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt">
                        <Columns>
                            <asp:BoundField HeaderText="Name" DataField="Name" />
                            <asp:BoundField HeaderText="LoginName" DataField="LoginName" />
                            <asp:BoundField HeaderText="Group" DataField="Group" HtmlEncode="false" />
                            <asp:BoundField HeaderText="IsSiteAdmin" DataField="IsSiteAdmin" />
                        </Columns>
                    </asp:GridView>
                </div>
                <h3>
                    <a href="#">Groups and Subgroups</a></h3>
                <div id="Groups_SubGroupsDIV">
                    <table width="100%">
                        <tr>
                            <td style="width: 40%; padding-right: 10px" valign="top">
                                <asp:TreeView ID="trvGroups" NodeStyle-CssClass="treeNode" RootNodeStyle-CssClass="rootNode"
                                    LeafNodeStyle-CssClass="leafNode" OnSelectedNodeChanged="trvGroups_SelectedNodeChanged"
                                    runat="server">
                                </asp:TreeView>
                            </td>
                            <td style="width: 60%" valign="top">
                                <asp:TextBox Style="display: none" ID="txtGroupID" CssClass="Text" Text="0" runat="server"></asp:TextBox>
                                <asp:Label ID="GroupSubgroupTitle" CssClass="SubTitle" runat="server">Title</asp:Label>
                                <div class="right">
                                    <asp:TextBox ID="txtGroupName" CssClass="Text" runat="server" Width="375px" Enabled="False"></asp:TextBox>
                                </div>
                                <asp:Label ID="GroupSubgroupDescription" CssClass="SubTitle" runat="server">Description</asp:Label>
                                <div class="right">
                                    <asp:TextBox ID="txtGroupDescription" CssClass="Text" runat="server" Height="120px"
                                        TextMode="MultiLine" Width="381px" Enabled="False"></asp:TextBox>
                                </div>
                                <asp:Label ID="GroupSubgroupPosition" CssClass="SubTitle" runat="server">Position</asp:Label>
                                <div class="right">
                                    <asp:TextBox ID="txtPosition" CssClass="Text" runat="server" Width="100px" Enabled="False"></asp:TextBox>
                                </div>
                                <asp:Label ID="GroupSubgroupParent" CssClass="SubTitle" runat="server">Parent</asp:Label>
                                <div class="right">
                                    <asp:DropDownList ID="ddlParentGroups" CssClass="Text" runat="server" Width="400px"
                                        Enabled="False">
                                    </asp:DropDownList>
                                </div>
                                <div id="Group_SubGroupMenuDIV">
                                    <div class="row">
                                        <div style="float: left;">
                                            <input type="submit" runat="server" class="Text" value="Edit" onserverclick="btnSaveGroup_Click"
                                                id="btnSaveGroup" />
                                        </div>
                                        <div style="float: left;">
                                            <input type="submit" runat="server" class="Text" value="Add" onserverclick="btnAddGroup_Click"
                                                id="btnAddGroup" />
                                        </div>
                                        <div style="float: left;">
                                            <input type="submit" runat="server" class="Text" value="Delete" onserverclick="btnDeleteGroup_Click"
                                                id="btnDeleteGroup" onclick="if (! OnClientClickGroupDelete())  return false;" />
                                        </div>
                                    </div>
                                </div>
                            </td>
                        </tr>
                    </table>
                </div>
                <h3>
                    <a href="#">External Links</a></h3>
                <div id="ExternalLinksDOV">
                    <div class="space">
                        <asp:CheckBox ID="chkExternalLnkAddToFooter" CssClass="Text" runat="server" Enabled="false" />
                        <asp:Label ID="lblExternalLnkAddToFooter" CssClass="SubTitle" runat="server">Add this Site to  Footer</asp:Label>
                    </div>
                    <div class="space">
                        <asp:CheckBox ID="chkExternalLnkAddToGlobalNav" CssClass="Text" runat="server" Enabled="false" />
                        <asp:Label ID="lblExternalLnkAddToGlobalNav" CssClass="SubTitle" runat="server">Add this Site to  Global Navigation Bar</asp:Label>
                    </div>
                    <asp:TextBox Style="display: none" ID="txtExternalLnkID" CssClass="Text" Text="0"
                        runat="server"></asp:TextBox>
                    <asp:Label ID="lblExternalLnkTitle" CssClass="SubTitle" runat="server">Title</asp:Label>
                    <div class="right">
                        <asp:TextBox ID="txtExternalLnkTitle" CssClass="Text" runat="server" Width="375px"
                            Enabled="False"></asp:TextBox>
                    </div>
                    <asp:Label ID="lblExternalLnkUrl" CssClass="SubTitle" runat="server">Url</asp:Label>
                    <div class="right">
                        <asp:TextBox ID="txtExternalLnkUrl" CssClass="Text" runat="server" Width="375px"
                            Enabled="False"></asp:TextBox>
                    </div>
                    <asp:Label ID="lblExternalLnkDescription" CssClass="SubTitle" runat="server">Description</asp:Label>
                    <div class="right">
                        <asp:TextBox ID="txtExternalLnkDescription" CssClass="Text" runat="server" Height="120px"
                            TextMode="MultiLine" Width="381px" Enabled="False"></asp:TextBox>
                    </div>
                    <asp:Label ID="lblExternalLnkPosition" CssClass="SubTitle" runat="server">Position: </asp:Label>
                    <div class="right">
                        <asp:DropDownList ID="rcbExternalLnkPosition" CssClass="Text" Enabled="False" runat="server"
                            Width="448px">
                        </asp:DropDownList>
                    </div>
                    <asp:Label ID="lblExternalLnkParent" CssClass="SubTitle" runat="server">Select the name of the group that you want this site be included?(Optional)</asp:Label>
                    <div class="right">
                        <asp:DropDownList ID="ddlExternalLnkParent" CssClass="Text" runat="server" Width="400px"
                            Enabled="False">
                        </asp:DropDownList>
                    </div>
                    <div id="ExternalLnkMenuDIV">
                        <div class="row">
                            <div style="float: left;">
                                <input type="submit" runat="server" class="Text" value="Edit" onserverclick="btnExternalLnkSave_Click"
                                    id="btnExternalLnkSave" />
                            </div>
                            <div style="float: left;">
                                <input type="submit" runat="server" class="Text" value="Add" onserverclick="btnExternalLnkAdd_Click"
                                    id="btnExternalLnkAdd" />
                            </div>
                            <div style="float: left;">
                                <input type="submit" runat="server" class="Text" value="Delete" onserverclick="btnExternalLnkDelete_Click"
                                    id="btnExternalLnkDelete" onclick="if (! OnClientClickGroupDelete())  return false;" />
                            </div>
                        </div>
                    </div>
                    <%-- <asp:XmlDataSource ID="XmlDataSource1" DataFile="~/spglobaltopmenu/data/GlobalTopMenu.xml" XPath="Groups/Group" runat="server">
                    </asp:XmlDataSource>
                    <obout:Tree ID="ObClassicTree" CssClass="vista" DataSourceID="XmlDataSource1" runat="server">
                        <DataBindings>
                            <obout:NodeBinding DataMember="Group" ImageUrl="/_layouts/SP.GlobalTopMenu/images/treeview/vista_book.png"
                                TextField="Title"
                                Expanded="true" />
                            <obout:NodeBinding DataMember="Subgroup" ImageUrl="/_layouts/SP.GlobalTopMenu/images/treeview/vista_note.png"
                                TextField="Title" />
                            <obout:NodeBinding DataMember="Item" ImageUrl="/_layouts/SP.GlobalTopMenu/images/treeview/vista_note.png"
                                TextField="Title" />
                        </DataBindings>
                    </obout:Tree>--%>
                </div>
            </div>
        </td>
    </tr>
</table>
<asp:TextBox Style="display: none" ID="hidAccordionIndex" Text="0" runat="server"></asp:TextBox>
