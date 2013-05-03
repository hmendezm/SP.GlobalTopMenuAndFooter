using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Linq;
using System.Drawing;
using System.Drawing.Text;
using System.Drawing.Drawing2D;
using System.Data;
using System.IO;
using System.Drawing.Imaging;
using Microsoft.SharePoint.Utilities;

namespace SP.GlobalTopMenu
{
    public partial class ucSettings : UserControl
    {
        public const string GTM_LIBRARY = "SPGlobalTopMenu";

        public const string GTM_FOLDER = "Images";

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private DataSet GetViewState()
        {
            //Gets the ViewState
            return (DataSet)ViewState["myDataSet"];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="myDataSet"></param>
        private void SetViewState(DataSet myDataSet)
        {
            //Sets the ViewState
            ViewState["myDataSet"] = myDataSet;
        }

        /// <summary>
        /// 
        /// </summary>
        private string GridViewSortDirection
        {
            get
            {
                return ViewState["SortDirection"] as string ?? "ASC";
            }
            set
            {
                ViewState["SortDirection"] = value;
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        private string GridViewSortExpression
        {
            get
            {
                return ViewState["SortExpression"] as string ?? string.Empty;
            }
            set
            {
                ViewState["SortExpression"] = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (!this.Page.IsPostBack)
            {
                this.createTrvGlobalNavFooter();
                this.createTrvGroups();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void createTrvGroups()
        {
            try
            {
                Int64 iMaxPosition;

                TreeNode rtnSelectedNode = null;
                int iselectedNodeIndex = 0;

                if (this.trvGroups.SelectedNode != null)
                {
                    iselectedNodeIndex = trvGroups.Nodes.IndexOf(trvGroups.SelectedNode);
                    rtnSelectedNode = this.trvGroups.SelectedNode;
                }

                this.trvGroups.Nodes.Clear();

                SPSecurity.RunWithElevatedPrivileges(
                    delegate()
                    {
                        XDocument xDoc = XMLFiles.GetXDocument(XMLFiles.XMLType.XMLGROUPNAMES);

                        if (xDoc.DescendantNodes().ToList().Count > 1)
                        {
                            iMaxPosition = ((from c in xDoc.Elements("GroupNames").Elements("Group")
                                             select (int.Parse(c.Element("Position").Value.Trim().Length > 0 ? c.Element("Position").Value : "0"))).Max() + 1);

                            var q = from c in xDoc.Elements("GroupNames").Elements("Group")
                                    where (string.IsNullOrEmpty(c.Element("ParentId").Value.ToString()))
                                    orderby Convert.ToInt64(c.Element("Position").Value.Trim().Length) == 0 ? iMaxPosition : Convert.ToInt64(c.Element("Position").Value) ascending
                                    select c;

                            foreach (var item in q)
                            {
                                TreeNode newNode = new TreeNode();

                                newNode.Text = item.Element("Name").Value;
                                newNode.Value = item.Element("Id").Value;

                                //Add the SubGroups

                                var qSubgroups = from c in xDoc.Elements("GroupNames").Elements("Group")
                                                 where (!string.IsNullOrEmpty(c.Element("ParentId").Value.ToString()) && c.Element("ParentId").Value.ToString() == newNode.Value)
                                                 orderby Convert.ToInt64(c.Element("Position").Value) == 0 ? iMaxPosition : Convert.ToInt64(c.Element("Position").Value) ascending
                                                 select c;

                                foreach (var subgroup in qSubgroups)
                                {
                                    TreeNode newChildNode = new TreeNode();
                                    newChildNode.Text = subgroup.Element("Name").Value;
                                    newChildNode.Value = subgroup.Element("Id").Value;

                                    newNode.ChildNodes.Add(newChildNode);
                                }

                                trvGroups.Nodes.Add(newNode);
                            }
                        }
                    });
                
                trvGroups.ExpandAll();

                getGroupsSubgroups();
                getAllparents();

                if (trvGroups.Nodes.Count == 0)
                {
                    btnAddGroup.Visible = true;
                    btnSaveGroup.Visible = false;
                    btnDeleteGroup.Visible = false;
                }
                else
                {
                    this.trvGroups.Nodes[0].Selected = true;
                    getSelectedNodeGroupInfo(trvGroups.SelectedNode);
                }
            }
            catch (Exception ex)
            {
               
                throw;
            }
        }

        /// <summary>
        /// Creates the treeview with all the site and subsite which the current user has access.
        /// </summary>
        private void createTrvGlobalNavFooter()
        {
            try
            {
                TreeNode parentItem = null;
                TreeNode SubSiteItem = null;
                TreeNode GroupSiteCollections = null;
                TreeNode SiteCollectionItem = null;
                TreeNode rtnSelectedNode = null;
                int iselectedNodeIndex = 0;

                //Update rcbPositions values.
                fillPosition();

                if (this.trvGlobalNavFooter.SelectedNode != null)
                {
                    //iselectedNodeIndex = trvGlobalNavFooter.Nodes.IndexOf(trvGlobalNavFooter.SelectedNode);
                    FindNode(trvGlobalNavFooter, trvGlobalNavFooter.SelectedNode.Value.ToString()).Selected = true;
                    rtnSelectedNode = this.trvGlobalNavFooter.SelectedNode;
                }

                this.trvGlobalNavFooter.Nodes.Clear();
                string userLoginName = SPContext.Current.Web.CurrentUser.LoginName;

                SPSecurity.RunWithElevatedPrivileges(
                    delegate()
                    {
                        string strServerRelativeUrl = string.Empty;

                        //Get  current web application
                        SPWebApplication webApp = SPContext.Current.Site.WebApplication;

                        foreach (SPSite site in webApp.Sites)
                        {
                            if (clsCommonBL.IsUserHasAccess(site.RootWeb.Url, userLoginName))
                            {
                                strServerRelativeUrl = site.ServerRelativeUrl;
                            }
                            else
                            {
                                strServerRelativeUrl = string.Empty;
                            }

                            if (strServerRelativeUrl.Trim().Length > 0)
                            {
                                SiteCollectionItem = createTreeViewNode(strServerRelativeUrl, site.RootWeb.Title, site.RootWeb.Description != null ? site.RootWeb.Description : "", GroupSiteCollections, this.getIcons(strServerRelativeUrl));
                            }

                            //Get all Su Site of the Site collection
                            foreach (SPWeb web in site.AllWebs)
                            {
                                if (clsCommonBL.IsUserHasAccess(web.Url, userLoginName))
                                {
                                    if (web.ParentWeb != null && web.ParentWeb.Name.Trim().Length > 0)
                                    {
                                        parentItem = SubSiteItem;
                                    }
                                    else
                                    {
                                        parentItem = SiteCollectionItem;
                                    }

                                    if (web.ServerRelativeUrl.Trim() != site.ServerRelativeUrl.Trim() && clsCommonBL.IsUserHasAccess(web.Url, userLoginName))
                                    {
                                        SubSiteItem = this.createTreeViewNode(web.ServerRelativeUrl, web.Title, web.Description != null ? web.Description : "", parentItem, this.getIcons(web.ServerRelativeUrl));
                                    }
                                }
                            }
                        }
                        if (rtnSelectedNode != null)
                        {
                            this.trvGlobalNavFooter.Nodes[iselectedNodeIndex].Selected = true;
                            this.getSelectedNodeInfo(rtnSelectedNode);
                        }
                        else if (this.trvGlobalNavFooter.Nodes.Count > 0)
                        {
                            this.trvGlobalNavFooter.Nodes[0].Selected = true;
                            this.getSelectedNodeInfo(this.trvGlobalNavFooter.SelectedNode);
                        }
                    });
            }
            catch (Exception ex)
            {
               
                throw;
            }
        }

        /// <summary>
        /// Defines what cssClass to use for the node.
        /// </summary>
        /// <param name="SiteUrl">Site URL</param>
        /// <returns></returns>
        private List<string> getIcons(string SiteUrl)
        {
            List<string> strIcons = new List<string>();

            bool HasGlobalNav;
            bool HasFooter;
            try
            {
                StringDictionary strdSettings = XMLFiles.getSettings(SiteUrl, clsCommonBL.FindBy.BySiteUrl);
                if (strdSettings != null)
                {
                    int value;
                    bool IsNumber = int.TryParse(strdSettings["GlobalNav"].ToString(), out value);

                    if (IsNumber)
                    {
                        HasGlobalNav = Convert.ToBoolean(value);
                    }
                    else
                    {
                        HasGlobalNav = Convert.ToBoolean(strdSettings["GlobalNav"]);
                    }

                    IsNumber = int.TryParse(strdSettings["Footer"].ToString(), out value);
                    if (IsNumber)
                    {
                        HasFooter = Convert.ToBoolean(value);
                    }
                    else
                    {
                        HasFooter = Convert.ToBoolean(strdSettings["Footer"]);
                    }

                    if (HasGlobalNav && HasFooter)
                    {
                        strIcons.Add("GlobalNav.png");
                        strIcons.Add("Footer.png");
                    }
                    else if (HasGlobalNav)
                    {
                        strIcons.Add("GlobalNav.png");
                    }
                    else if (HasFooter)
                    {
                        strIcons.Add("Footer.png");
                    }
                }

                return strIcons;
            }
            catch (Exception ex)
            {
                
                throw;
                //return strIcons;
            }
        }



        /// <summary>
        /// Fills the positions in the RadCombobox
        /// </summary>
        private void fillPosition()
        {
            ListItem rcbEmptyPosition = new ListItem();

            try
            {
                XDocument xDoc =XMLFiles.GetXDocument(XMLFiles.XMLType.XMLGLOBALNAV);
                string strTitle;

                if (xDoc.Elements("GlobalNav").Elements("Item").Count() > 0)
                {
                    rcbPositions.Items.Clear();

                    rcbEmptyPosition.Text = String.Empty;
                    rcbEmptyPosition.Value = String.Empty;
                    rcbEmptyPosition.Enabled = true;
                    rcbPositions.Items.Add(rcbEmptyPosition);

                    int iMaxPosition = ((from c in xDoc.Elements("GlobalNav").Elements("Item")
                                         select (
                                             int.Parse(c.Element("Position").Value.Trim().Length > 0 ? c.Element("Position").Value : "0"))).Max() + 1);

                    bool bEnabled;

                    for (int iPosition = 1; iPosition <= iMaxPosition; iPosition++)
                    {
                        var xeSiteTitleInhePosition = from c in xDoc.Elements("GlobalNav").Elements("Item")
                                                      where int.Parse(c.Element("Position").Value.Trim().Length > 0 ? c.Element("Position").Value : "-1") == iPosition
                                                      select new
                                                      {
                                                          Title = c.Element("SiteTitle").Value
                                                      };

                        if (xeSiteTitleInhePosition.Count() > 0)
                        {
                            strTitle = string.Format("{0} [{1}]", iPosition.ToString(), xeSiteTitleInhePosition.Single().Title);
                            bEnabled = false;
                        }
                        else
                        {
                            strTitle = iPosition.ToString();
                            bEnabled = true;
                        }

                        ListItem rcbPosition = new ListItem();
                        rcbPosition.Text = strTitle;
                        rcbPosition.Value = iPosition.ToString();

                        //rcbPosition.Enabled = bEnabled;

                        rcbPositions.Items.Add(rcbPosition);
                    }
                }
                else
                {
                    rcbEmptyPosition.Text = String.Empty;
                    rcbEmptyPosition.Value = "0";
                    rcbEmptyPosition.Enabled = true;
                    rcbPositions.Items.Add(rcbEmptyPosition);

                    ListItem rcbPosition = new ListItem();
                    rcbPosition.Text = "1";
                    rcbPosition.Value = "1";
                    rcbPosition.Enabled = true;

                    rcbPositions.Items.Add(rcbPosition);
                }
            }
            catch (Exception ex)
            {
                
                throw;
            }
        }

        /// <summary>
        /// Creates the treeview node.
        /// </summary>
        /// <param name="strUrl"></param>
        /// <param name="strTitle"></param>
        /// <param name="strToolTip"></param>
        /// <param name="mniParent"></param>
        /// <param name="strIcons"></param>
        /// <returns></returns>
        private TreeNode createTreeViewNode(string strUrl, string strTitle, string strToolTip, TreeNode mniParent, List<string> strIcons)
        {
            TreeNode mniChild = new TreeNode();
            try
            {
                //mniChild.NavigateUrl = strUrl;
                strTitle += "&nbsp;&nbsp;";

                foreach (string strIcon in strIcons)
                {
                    strTitle += string.Format("<span></span><img height='15' align='middle' src='{0}/_layouts/SP.GlobalTopMenu/Images/Common/{1}' alt=''/>", SPContext.Current.Web.Url, strIcon);
                }

                StringDictionary strdSettings = XMLFiles.getSettings(strUrl, clsCommonBL.FindBy.BySiteUrl);

                if (strdSettings != null)
                {
                    if (Convert.ToInt16(String.IsNullOrEmpty(strdSettings["position"].ToString()) ? "0" : strdSettings["position"].ToString()) > 0)
                    {
                        this.createImage(strdSettings["position"]);
                        strTitle += string.Format("<span style='position: absolute;float:right;'></span><img height='15' align='middle' src='{0}/{1}.jpg' alt=''></img>", 
                                                    clsCommonBL.SiteRootUrl+"/"+clsCommonBL.GTM_LIBRARY+"/"+clsCommonBL.IMG_FOLDER, strdSettings["position"]);
                    }
                }
                mniChild.Text = strTitle;
                mniChild.Value = strUrl;
                mniChild.ToolTip = strToolTip;

                if (mniParent == null)
                {
                    this.trvGlobalNavFooter.Nodes.Add(mniChild);
                }
                else
                {
                    //if (this.trvGlobalNavFooter.Nodes.IndexOf(mniParent) > 0)
                    if (FindNode(trvGlobalNavFooter, mniParent.Value) != null)
                        mniParent.ChildNodes.Add(mniChild);
                    //this.trvGlobalNavFooter.Nodes[this.trvGlobalNavFooter.Nodes.IndexOf(mniParent)].ChildNodes.Add(mniChild);
                }
                return mniChild;
            }
            catch (Exception ex)
            {
                
                throw;
                //return mniChild;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="tvSelection"></param>
        /// <param name="matchText"></param>
        /// <returns></returns>
        private TreeNode FindNode(TreeView tvSelection, string matchText)
        {
            foreach (TreeNode node in tvSelection.Nodes)
            {
                if (node.Value.ToString() == matchText)
                {
                    return node;
                }
                else
                {
                    TreeNode nodeChild = FindChildNode(node, matchText);
                    if (nodeChild != null)
                        return nodeChild;
                }
            }
            return (TreeNode)null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tvSelection"></param>
        /// <param name="matchText"></param>
        /// <returns></returns>
        private TreeNode FindChildNode(TreeNode tvSelection, string matchText)
        {
            foreach (TreeNode node in tvSelection.ChildNodes)
            {
                if (node.Value.ToString() == matchText)
                {
                    return node;
                }
                else
                {
                    TreeNode nodeChild = FindChildNode(node, matchText);
                    if (nodeChild != null)
                        return nodeChild;
                }
            }
            return (TreeNode)null;
        }

        ///// <summary>
        ///// Creates the number images
        ///// </summary>
        ///// <param name="strText">Text</param>
        //private void createImage(string strText)
        //{
        //    try
        //    {
        //        SPSecurity.RunWithElevatedPrivileges(
        //            delegate()
        //            {
        //                // Configure font to use for text         
        //                Font objFont = new Font("Arial", 8, FontStyle.Bold);
        //                SizeF objSizeF = new SizeF();

        //                using (Bitmap bitMapImage = new Bitmap(15, 15))
        //                {
        //                    using (Graphics graphicImage = Graphics.FromImage(bitMapImage))
        //                    {
        //                        using (SolidBrush whiteBrush = new SolidBrush(Color.White))
        //                        {
        //                            graphicImage.FillEllipse(whiteBrush, 0, 0, bitMapImage.Width - 1, bitMapImage.Height - 1);
        //                            graphicImage.DrawEllipse(Pens.Black, 0, 0, bitMapImage.Width - 1, bitMapImage.Height - 1);
        //                        }

        //                        objSizeF = graphicImage.MeasureString(strText, objFont);

        //                        //Smooth graphics is nice.
        //                        graphicImage.SmoothingMode = SmoothingMode.AntiAlias;

        //                        graphicImage.TextRenderingHint = TextRenderingHint.AntiAlias;

        //                        //Write your text.
        //                        graphicImage.DrawString(strText, objFont, SystemBrushes.WindowText,
        //                            new Point((bitMapImage.Width - Convert.ToInt32(objSizeF.Width)) / 2, (bitMapImage.Height - Convert.ToInt32(objSizeF.Height)) / 2));

        //                        //Save the new image to the response output stream.
        //                        //bitMapImage.Save(this.Server.MapPath(string.Format("Images/{0}.jpg", strText)));

                                
               
        //                    }
        //                }
        //            });
        //    }
        //    catch (Exception ex)
        //    {
                
        //        throw;
        //    }
        //}

        /// <summary>
        /// Get infomation about the selected site.
        /// </summary>
        /// <param name="SiteUrl">Site Url</param>
        private void getSiteInformation(string SiteUrl)
        {
            try
            {
                using (SPWeb web = SPContext.Current.Site.WebApplication.Sites[SiteUrl].OpenWeb())
                {
                    lblSiteDescription.Text = string.IsNullOrEmpty(web.Description) ? "None" : web.Description;
                    lblCustomMasterUrl.Text = web.CustomMasterUrl;
                    lblAlternateCssUrl.Text = web.AlternateCssUrl;
                    lblHasUniquePerm.Text = web.HasUniquePerm.ToString();
                    aSettingsPage.Title = string.Format("Go to Settings Page{0}", web.Title);
                    aSettingsPage.InnerText = string.Format("Go to the Settings Page of {0} site", web.Title);
                    aSettingsPage.HRef = string.Format("{0}/_layouts/Settings.aspx", web.Url);
                }
            }
            catch (Exception ex)
            {
                
                throw;
            }
        }

        /// <summary>
        /// Gets selected User roles.
        /// </summary>
        /// <param name="strUser">User login.</param>
        /// <param name="web">Web.</param>
        /// <param name="strField">Field of the user to find.</param>
        /// <returns>ul structure with all the information.</returns>
        private string getUserRoles(string strUser, SPWeb web, string strField)
        {
            String strGroups = string.Empty;
            try
            {
                SPUser spUser = web.EnsureUser(strUser);
                XDocument usersInfoXml = XDocument.Parse(spUser.Groups.Xml);

                if (usersInfoXml.Descendants("Group").Count() > 0)
                {
                    var query = from userInfo in usersInfoXml.Descendants("Group")
                                select new
                                {
                                    fieldValue = userInfo.Attribute(strField).Value
                                };
                    strGroups = "<ul>";
                    foreach (var info in query)
                    {
                        strGroups += string.Format("<li>{0}</li>", info.fieldValue);
                    }
                    strGroups += "</ul>";
                }
                else
                {
                    strGroups = "-------";
                }

                return Context.Server.HtmlDecode(strGroups);
            }
            catch (Exception ex)
            {
                
                throw;
                //return "-------";
            }
        }

        /// <summary>
        /// Gets all information of a selected Node.
        /// </summary>
        /// <param name="e"></param>
        private void getSelectedNodeInfo(TreeNode e)
        {
            try
            {
                lblSiteTite.Text = e.Text;

                this.getSiteInformation(e.Value);

                StringDictionary strdSettings = XMLFiles.getSettings(e.Value, clsCommonBL.FindBy.BySiteUrl);

                this.createSiteAdminsList(e.Value);

                if (strdSettings != null)
                {
                    //Get the Global Navigation value
                    int value;
                    bool bIsNumber = int.TryParse(strdSettings["GlobalNav"].ToString(), out value);

                    if (bIsNumber)
                        chkAddToGlobalNav.Checked = Convert.ToBoolean(value);
                    else
                        chkAddToGlobalNav.Checked = Convert.ToBoolean(strdSettings["GlobalNav"]);

                    bIsNumber = int.TryParse(strdSettings["Footer"].ToString(), out value);
                    if (bIsNumber)
                        chkAddToFooter.Checked = Convert.ToBoolean(value);
                    else
                        chkAddToFooter.Checked = Convert.ToBoolean(strdSettings["Footer"]);

                    if (ddlGroupNames.Items.Count > 0)
                        ddlGroupNames.SelectedValue = strdSettings["GroupId"].ToString() ; 

                    if (ddlGroupNames.SelectedValue.Trim().Length == 0)
                        ddlGroupNames.Text = string.Empty;

                    rcbPositions.SelectedValue = strdSettings["Position"].ToString() == "0" ? "" : strdSettings["Position"].ToString();

                    if (rcbPositions.SelectedValue.Trim().Length == 0)
                        rcbPositions.Text = string.Empty;

                    rdtxtChangeTitle.Text = strdSettings["NewTitle"].ToString();

                    if (rdtxtChangeTitle.Text.Trim().Length == 0)
                        rdtxtChangeTitle.Text = string.Empty;
                }
                else
                {
                    chkAddToGlobalNav.Checked = false;
                    chkAddToFooter.Checked = false;
                    ddlGroupNames.ClearSelection();
                    rcbPositions.ClearSelection();
                    rdtxtChangeTitle.Text = string.Empty;
                }
            }
            catch (Exception ex)
            {
                
                throw;
            }
        }

        /// <summary>
        /// Click event for the treeview nodes.
        /// </summary>
        /// <param name="o"></param>
        /// <param name="e"></param>
        protected void trvGlobalNavFooter_SelectedNodeChanged(object sender, EventArgs e)
        {
            try
            {
                getSelectedNodeInfo(trvGlobalNavFooter.SelectedNode);
            }
            catch (Exception ex)
            {
                
                throw;
            }
        }

        /// <summary>
        /// event the save tha information of the group name selected by the user.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSaveMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                UpdateGlobalNavItem(this.trvGlobalNavFooter.SelectedValue);

                createTrvGlobalNavFooter();

                trvGlobalNavFooter.ExpandAll();
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnDeleteMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /*XML File Methods*/
        /// <summary>
        /// Add the new Group Name to the GroupNames.xml file.
        /// </summary>
        /// <param name="strGroupName">Name of the group</param>
        protected void addNewGroupNameToXMLFile(string strGroupName)
        {
            try
            {
                if (strGroupName.Trim().Length > 0)
                {
                    
                    int GroupCount = 1;
                    XDocument xDoc =  XMLFiles.GetXDocument(XMLFiles.XMLType.XMLGLOBALNAV); 

                    //Check if the Group name already exist
                    ListItem rcbItem = ddlGroupNames.Items.FindByText(strGroupName);

                    foreach (ListItem item in ddlGroupNames.Items)
                    {
                        if (Convert.ToInt32(item.Value) >= GroupCount)
                        {
                            GroupCount = Convert.ToInt32(item.Value) + 1;
                        }
                    }
                    //if (rcbItem == null)
                    //{
                    //    xDoc.Element("GroupNames").Add(new XElement("Group", new XElement("Id", GroupCount), new XElement("Name", strGroupName)));
                    //    xDoc.Save(this.MapPath(strPath));
                    //    ddlGroupNames.DataSourceID = "XmlDSGroupNames";
                    //    ddlGroupNames.DataBind();
                    //    ddlGroupNames.SelectedValue = GroupCount.ToString();
                    //}
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void getGroupsSubgroups()
        {
            try
            {
                SPSecurity.RunWithElevatedPrivileges(
                    delegate()
                    {
                        XDocument xDoc = XMLFiles.GetXDocument(XMLFiles.XMLType.XMLGROUPNAMES);

                        ddlGroupNames.Items.Clear();

                        ListItem lstiEmpty = new ListItem();

                        lstiEmpty.Text = String.Empty;
                        lstiEmpty.Value = "0";

                        ddlGroupNames.Items.Add(lstiEmpty);

                        if (xDoc.DescendantNodes().ToList().Count > 1)
                        {
                            var q = from c in xDoc.Elements("GroupNames").Elements("Group")
                                    where (string.IsNullOrEmpty(c.Element("ParentId").Value.ToString()))
                                    //orderby Convert.ToInt32(c.Element("Position").Value.Trim().Length == 0 ? strMaxPosition : c.Element("Position").Value) ascending
                                    select c;

                            foreach (var item in q)
                            {
                                ListItem lstigroup = new ListItem();

                                lstigroup.Text = item.Element("Name").Value;
                                lstigroup.Value = item.Element("Id").Value;

                                //Add the SubGroups

                                var qSubgroups = from c in xDoc.Elements("GroupNames").Elements("Group")
                                                 where (!string.IsNullOrEmpty(c.Element("ParentId").Value.ToString()) && c.Element("ParentId").Value.ToString() == lstigroup.Value)
                                                 //orderby Convert.ToInt32(c.Element("Position").Value.Trim().Length == 0 ? strMaxPosition : c.Element("Position").Value) ascending
                                                 select c;
                                ddlGroupNames.Items.Add(lstigroup);
                                foreach (var subgroup in qSubgroups)
                                {
                                    ListItem lstiSubgroup = new ListItem();
                                    lstiSubgroup.Text = subgroup.Element("Name").Value;
                                    lstiSubgroup.Value = subgroup.Element("Id").Value;

                                    ddlGroupNames.Items.Add(lstiSubgroup);
                                }
                            }
                        }
                    });
            }
            catch (Exception ex)
            { 
            }
        }

        /// <summary>
        /// Updates the GlobalNav.xml file 
        /// </summary>
        /// <param name="SiteUrl">Site Url</param>
        private void UpdateGlobalNavItem(string SiteUrl)
        {
            try
            {

                SPSecurity.RunWithElevatedPrivileges(
                    delegate()
                    {

                        using (SPWeb web = SPContext.Current.Site.WebApplication.Sites[SiteUrl].OpenWeb())
                        {

                            XDocument xDoc = XMLFiles.GetXDocument(XMLFiles.XMLType.XMLGLOBALNAV);
                            if (!chkAddToGlobalNav.Checked && !chkAddToFooter.Checked)
                            {
                                this.removeGlobalNavItemFromXML(xDoc, web.ID.ToString());
                            }
                            else if (rcbPositions.SelectedValue.Trim().Length > 0 || chkAddToGlobalNav.Checked || chkAddToFooter.Checked)
                            {
                                this.removeGlobalNavItemFromXML(xDoc, web.ID.ToString());

                                string strGroupId = ddlGroupNames.SelectedValue;

                                xDoc.Element("GlobalNav").Add(new XElement("Item", new XElement("SiteId", web.ID),
                                    new XElement("SiteTitle", web.Title),
                                    new XElement("NewTitle", rdtxtChangeTitle.Text),
                                    new XElement("SiteDescription", web.Description),
                                    new XElement("SiteUrl", web.ServerRelativeUrl),
                                    new XElement("Position", String.IsNullOrEmpty(rcbPositions.SelectedValue) ? "0" : rcbPositions.SelectedValue),
                                    new XElement("GroupId", strGroupId),
                                    new XElement("GlobalNav", chkAddToGlobalNav.Checked.ToString()),
                                    new XElement("Footer", chkAddToFooter.Checked.ToString()),
                                    new XElement("ParentId", string.IsNullOrEmpty(strGroupId) ? (web.ParentWebId != Guid.Empty ? web.ParentWebId.ToString() : string.Empty) : string.Empty)));

                                XMLFiles.UploadXDocumentToDocLib(xDoc, true, XMLFiles.XMLType.XMLGLOBALNAV);
                                 //xDoc.Save(this.MapPath(XMLGLOBALNAVPATH));
                            }
                            else
                            {
                                this.removeGlobalNavItemFromXML(xDoc, web.ID.ToString());
                            }
                        }
                    });
            }
            catch (Exception ex)
            {
                
                throw;
            }
        }

        /// <summary>
        /// Update a specific Item in the Globalnav.xml file
        /// </summary>
        /// <param name="strSiteId">Site Guid</param>
        /// <returns></returns>
        private bool updateGlobalNavItemElement(string strSiteId)
        {
            bool bElementChanged = false;
            try
            {
                SPSecurity.RunWithElevatedPrivileges(
                    delegate()
                    {
                        XDocument xDoc = XMLFiles.GetXDocument(XMLFiles.XMLType.XMLGLOBALNAV);

                        var q = from c in xDoc.Elements("GlobalNav").Elements("Item")
                                where (string)c.Element("SiteId") == strSiteId
                                select c;

                        // convert the list to an array so that we're not modifying the     
                        // collection that we're iterating over     
                        foreach (XElement e in q.ToArray())
                        {
                            e.SetAttributeValue("NewTitle", rdtxtChangeTitle.Text);
                            e.SetAttributeValue("Position", String.IsNullOrEmpty(rcbPositions.SelectedValue) ? "0" : rcbPositions.SelectedValue);
                            e.SetAttributeValue("GroupId", ddlGroupNames.SelectedValue);

                            e.SetAttributeValue("GlobalNav", chkAddToGlobalNav.Checked.ToString());
                            e.SetAttributeValue("Footer", chkAddToFooter.Checked.ToString());

                            bElementChanged = true;
                        }

                        if (bElementChanged)
                        {
                            XMLFiles.UploadXDocumentToDocLib(xDoc, true, XMLFiles.XMLType.XMLGLOBALNAV);
                            //xDoc.Save(this.MapPath(XMLGLOBALNAVPATH));
                        }
                    });
                return bElementChanged;
            }
            catch (Exception ex)
            {
                
                throw;
                //return bElementChanged;
            }
        }

        /// <summary>
        /// Removes a specific item from the GlobalNav.xml file
        /// </summary>
        /// <param name="xDoc">xDocument object</param>
        /// <param name="strSiteId">Site Guid</param>
        private void removeGlobalNavItemFromXML(XDocument xDoc, string strSiteId)
        {
            try
            {
                SPSecurity.RunWithElevatedPrivileges(
                    delegate()
                    {
                        var q = from c in xDoc.Elements("GlobalNav").Elements("Item")
                                where (string)c.Element("SiteId") == strSiteId
                                select c;

                        // convert the list to an array so that we're not modifying the     
                        // collection that we're iterating over     
                        foreach (XElement e in q.ToArray())
                        {
                            e.Remove();
                        }
                        XMLFiles.UploadXDocumentToDocLib(xDoc, true, XMLFiles.XMLType.XMLGLOBALNAV);
                        //xDoc.Save(this.MapPath(XMLGLOBALNAVPATH));
                    });
            }
            catch (Exception ex)
            {
                
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void trvGroups_SelectedNodeChanged(object sender, EventArgs e)
        {
            getSelectedNodeGroupInfo(trvGroups.SelectedNode);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        private void getSelectedNodeGroupInfo(TreeNode e)
        {
            try
            {
                StringDictionary strdSettings =XMLFiles.getGroupSettings(e.Value);

                if (strdSettings != null)
                {
                    txtGroupName.Text = strdSettings["Name"].ToString();
                    txtGroupDescription.Text = strdSettings["Description"].ToString();
                    ddlParentGroups.SelectedValue = strdSettings["ParentId"].ToString();
                    txtGroupID.Text = strdSettings["Id"].ToString();
                    txtPosition.Text = strdSettings["Position"].ToString();
                }
            }
            catch (Exception ex)
            {
               
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void getAllparents()
        {
            SPSecurity.RunWithElevatedPrivileges(
                delegate()
                {
                    XDocument xDoc = XMLFiles.GetXDocument(XMLFiles.XMLType.XMLGROUPNAMES);
                    ListItem lstEmpty = new ListItem();

                    ddlParentGroups.Items.Clear();
                    
                    lstEmpty.Text = "";
                    lstEmpty.Value = "";

                    ddlParentGroups.Items.Add(lstEmpty);

                    if (xDoc.DescendantNodes().ToList().Count > 1)
                    {
                        var q = from c in xDoc.Elements("GroupNames").Elements("Group")
                                where (string.IsNullOrEmpty(c.Element("ParentId").Value.ToString()))
                                select c;

                        foreach (var item in q)
                        {
                            ListItem lstParent = new ListItem();

                            lstParent.Text = item.Element("Name").Value;
                            lstParent.Value = item.Element("Id").Value;

                            ddlParentGroups.Items.Add(lstParent);
                        }
                    }
                });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strGroupId"></param>
        /// <returns></returns>
        private bool updateGroupInfo(string strGroupId)
        {
            bool bElementChanged = false;
            try
            {
                SPSecurity.RunWithElevatedPrivileges(
                    delegate()
                    {
                        XDocument xDoc = XMLFiles.GetXDocument(XMLFiles.XMLType.XMLGROUPNAMES);

                        if (xDoc.DescendantNodes().ToList().Count > 1)
                        {
                            var q = from c in xDoc.Elements("GroupNames").Elements("Group")
                                    where (c.Element("Id").Value.ToString() == strGroupId)
                                    select c;

                            if (q.Count() > 0)
                            {
                                // convert the list to an array so that we're not modifying the     
                                // collection that we're iterating over     
                                foreach (XElement e in q.ToArray())
                                {
                                    e.SetElementValue("Name", txtGroupName.Text);
                                    e.SetElementValue("Position", String.IsNullOrEmpty(txtPosition.Text) ? "0" : txtPosition.Text);
                                    e.SetElementValue("Description", txtGroupDescription.Text);

                                    e.SetElementValue("ParentId", ddlParentGroups.SelectedValue);

                                    bElementChanged = true;
                                }

                                if (bElementChanged)
                                {
                                    XMLFiles.UploadXDocumentToDocLib(xDoc, true,XMLFiles.XMLType.XMLGROUPNAMES );
                                    //xDoc.Save(this.MapPath(XMLGROUPNAMESPATH));
                                }
                            }
                            else 
                            {
                                xDoc.Element("GroupNames").Add(new XElement("Group",
                                    new XElement("Id", strGroupId),
                                    new XElement("Name", txtGroupName.Text),
                                    new XElement("Description", txtGroupDescription.Text),
                                    new XElement("Position", txtPosition.Text),
                                    new XElement("ParentId", ddlParentGroups.SelectedValue)));
                                XMLFiles.UploadXDocumentToDocLib(xDoc, true, XMLFiles.XMLType.XMLGROUPNAMES);
                                //xDoc.Save(this.MapPath(XMLGROUPNAMESPATH));                    
                            }
                        }
                        else
                        {
                            xDoc.Element("GroupNames").Add(new XElement("Group",
                                new XElement("Id", strGroupId),
                                new XElement("Name", txtGroupName.Text),
                                new XElement("Description", txtGroupDescription.Text),
                                new XElement("Position", txtPosition.Text),
                                new XElement("ParentId", ddlParentGroups.SelectedValue)));

                            XMLFiles.UploadXDocumentToDocLib(xDoc, true, XMLFiles.XMLType.XMLGROUPNAMES);
                            //xDoc.Save(this.MapPath(XMLGROUPNAMESPATH));
                        }
                    });
                createTrvGroups();
                return bElementChanged;
            }
            catch (Exception ex)
            {
                
                throw;
                //return bElementChanged;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSaveGroup_Click(object sender, EventArgs e)
        {
            if (btnSaveGroup.Value == "Edit")
            {
                btnAddGroup.Value = "Cancel";
                btnSaveGroup.Value = "Save";
                btnDeleteGroup.Visible = false;
                txtGroupDescription.Enabled = true;
                txtGroupName.Enabled = true;
                txtPosition.Enabled = true;
                ddlParentGroups.Enabled = true;
            }
            else
            {
                btnAddGroup.Value = "Add";
                btnSaveGroup.Value = "Edit";
                btnDeleteGroup.Visible = true;
                txtGroupDescription.Enabled = false;
                txtGroupName.Enabled = false;
                txtPosition.Enabled = false;
                ddlParentGroups.Enabled = false;

                if (!String.IsNullOrEmpty(txtGroupName.Text))
                    updateGroupInfo(txtGroupID.Text);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAddGroup_Click(object sender, EventArgs e)
        {
            if (btnAddGroup.Value == "Add")
            {
                txtGroupName.Text = String.Empty;
                txtGroupDescription.Text = String.Empty;
                txtGroupID.Text = Guid.NewGuid().ToString();

                btnAddGroup.Value = "Cancel";
                btnSaveGroup.Value = "Save";
                btnDeleteGroup.Visible = false;
                btnSaveGroup.Visible = true;

                txtGroupDescription.Enabled = true;
                txtGroupName.Enabled = true;
                txtPosition.Enabled = true;
                ddlParentGroups.Enabled = true;
            }
            else
            {
                btnAddGroup.Value = "Add";
                btnSaveGroup.Value = "Edit";
                btnDeleteGroup.Visible = true;

                txtGroupDescription.Enabled = false;
                txtGroupName.Enabled = false;
                txtPosition.Enabled = false;
                ddlParentGroups.Enabled = false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnDeleteGroup_Click(object sender, EventArgs e)
        {
            removeGroupItemFromXML(txtGroupID.Text);
        }

        /// <summary>
        /// Removes a specific item from the GlobalNav.xml file
        /// </summary>
        /// <param name="xDoc">xDocument object</param>
        /// <param name="strSiteId">Site Guid</param>
        private void removeGroupItemFromXML(string strGroupId)
        {
            try
            {
                SPSecurity.RunWithElevatedPrivileges(
                    delegate()
                    {
                        XDocument xDoc = XMLFiles.GetXDocument(XMLFiles.XMLType.XMLGROUPNAMES);

                        if (xDoc.DescendantNodes().ToList().Count > 1)
                        {
                            var q = from c in xDoc.Elements("GroupNames").Elements("Group")
                                    where (string)c.Element("Id") == strGroupId
                                    select c;

                            // convert the list to an array so that we're not modifying the     
                            // collection that we're iterating over     
                            foreach (XElement e in q.ToArray())
                            {
                                e.Remove();
                            }

                            XMLFiles.UploadXDocumentToDocLib(xDoc, true, XMLFiles.XMLType.XMLGROUPNAMES);
                            //xDoc.Save(this.MapPath(XMLGROUPNAMESPATH));
                        }
                    });
                createTrvGroups();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void createImage(string strText)
        {
            try
            {
                SPFolder oTargetFolder = null;
                SPSecurity.RunWithElevatedPrivileges(delegate()
                {
                    using (SPSite oSite = new SPSite(clsCommonBL.SiteRootUrl))
                    {
                        using (SPWeb oWeb = oSite.OpenWeb())
                        {
                            oSite.AllowUnsafeUpdates = true;
                            oWeb.AllowUnsafeUpdates = true;

                            //SPUtility.ValidateFormDigest();

                            //Get Images Folder
                            if (oWeb.GetFolder(clsCommonBL.GTM_LIBRARY + "\\" + clsCommonBL.IMG_FOLDER).Exists)
                                oTargetFolder = oWeb.Folders[clsCommonBL.GTM_LIBRARY].SubFolders[clsCommonBL.IMG_FOLDER];
                            else
                            {
                                oWeb.Folders[clsCommonBL.GTM_LIBRARY].SubFolders.Add(clsCommonBL.IMG_FOLDER);
                                oWeb.Folders[clsCommonBL.GTM_LIBRARY].Update();
                                oTargetFolder = oWeb.Folders[clsCommonBL.GTM_LIBRARY].SubFolders[clsCommonBL.IMG_FOLDER];
                            }


                            // Configure font to use for text         
                            Font objFont = new Font("Arial", 8, FontStyle.Bold);
                            SizeF objSizeF = new SizeF();

                            using (Bitmap bitMapImage = new Bitmap(15, 15))
                            {
                                using (Graphics graphicImage = Graphics.FromImage(bitMapImage))
                                {
                                    using (SolidBrush whiteBrush = new SolidBrush(Color.White))
                                    {
                                        graphicImage.FillEllipse(whiteBrush, 0, 0, bitMapImage.Width - 1, bitMapImage.Height - 1);
                                        graphicImage.DrawEllipse(Pens.Black, 0, 0, bitMapImage.Width - 1, bitMapImage.Height - 1);
                                    }

                                    objSizeF = graphicImage.MeasureString(strText, objFont);

                                    //Smooth graphics is nice.
                                    graphicImage.SmoothingMode = SmoothingMode.AntiAlias;

                                    graphicImage.TextRenderingHint = TextRenderingHint.AntiAlias;

                                    //Write your text.
                                    graphicImage.DrawString(strText, objFont, SystemBrushes.WindowText,
                                        new Point((bitMapImage.Width - Convert.ToInt32(objSizeF.Width)) / 2, (bitMapImage.Height - Convert.ToInt32(objSizeF.Height)) / 2));

                                    var streamImage = new MemoryStream();

                                    bitMapImage.Save(streamImage, ImageFormat.Jpeg);





                                    SPFile pic =oTargetFolder.Files.Add(string.Format("{0}.jpg", strText), streamImage, true);
                                    pic.Update();

                                    //oWeb.Folders[clsCommonBL.GTM_LIBRARY].Update();
                                }
                            }
  
                            oWeb.AllowUnsafeUpdates = false;
                            oSite.AllowUnsafeUpdates = false;
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                throw;
            }
        }



        public static string GetImagefromLibrary(string strImageName)
        {
            try
            {
                using (SPSite oSite = new SPSite(clsCommonBL.SiteRootUrl))
                {
                    using (SPWeb oWeb = oSite.OpenWeb())
                    {
                        SPFolder oTargetFolder = oWeb.Folders[clsCommonBL.GTM_LIBRARY];
                        SPFile spFile;

                        if (oWeb.GetFolder(oTargetFolder.Url + "/" + clsCommonBL.IMG_FOLDER).Exists)
                        {
                            if (oWeb.GetFile(oTargetFolder.SubFolders[clsCommonBL.IMG_FOLDER].Url + "/" + strImageName + ".jpg").Exists)
                            {
                                spFile = oTargetFolder.SubFolders[clsCommonBL.XML_FOLDER].Files[strImageName + ".jpg"];

                                //StreamReader sr = new StreamReader(spFile.OpenBinaryStream());

                                return spFile.ServerRelativeUrl;
                            }
                        }
                        return string.Empty;
                    }
                }
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }











        #region Grid Methods
        /// <summary>
        /// Gets all user of the selected Site
        /// </summary>
        /// <param name="SiteUrl">Site Url</param>
        private void createSiteAdminsList(string SiteUrl)
        {
            DataSet dtUserInfo = new DataSet();
            try
            {
                using (SPWeb web = SPContext.Current.Site.WebApplication.Sites[SiteUrl].OpenWeb())
                {
                    XDocument usersXml = XDocument.Parse(web.AllUsers.Xml);
                    if (usersXml.Descendants("User").Count() > 0)
                    {
                        var queryUsers = from user in usersXml.Descendants("User")
                                         select new
                                         {
                                             Name = user.Attribute("Name").Value,
                                             LoginName = user.Attribute("LoginName").Value,
                                             IsSiteAdmin = user.Attribute("IsSiteAdmin").Value,
                                             Group = this.getUserRoles(user.Attribute("LoginName").Value, web, "Name")
                                         };

                        //int iCurrentPageIndex = rgSiteAdmins..CurrentPageIndex;
                        //int iCount = rgSiteAdmins.Items.Count;

                        //rgSiteAdmins.CurrentPageIndex = 0;

                        dtUserInfo.Tables.Add(clsCommonBL.LINQToDataTable(queryUsers));

                        SetViewState(dtUserInfo);

                        rgSiteAdmins.DataSource = GetViewState();
                        rgSiteAdmins.DataBind();
                    }
                }
            }
            catch (Exception ex)
            {
                
                throw;

            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rgSiteAdmins_PageIndexChanging(object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
        {
            rgSiteAdmins.PageIndex = e.NewPageIndex;
            rgSiteAdmins.DataSource = GetViewState();
            rgSiteAdmins.DataBind();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rgSiteAdmins_Sorting(object sender, System.Web.UI.WebControls.GridViewSortEventArgs e)
        {
            DataTable dataTable = rgSiteAdmins.DataSource as DataTable;

            if (dataTable != null)
            {
                DataView dataView = new DataView(dataTable);
                dataView.Sort = e.SortExpression + " " + ConvertSortDirectionToSql(e.SortDirection);

                rgSiteAdmins.DataSource = dataView;
                rgSiteAdmins.DataBind();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sortDirection"></param>
        /// <returns></returns>
        private string ConvertSortDirectionToSql(SortDirection sortDirection)
        {
            string newSortDirection = String.Empty;

            switch (sortDirection)
            {
                case SortDirection.Ascending:
                    newSortDirection = "ASC";
                    break;
                case SortDirection.Descending:
                    newSortDirection = "DESC";
                    break;
            }
            return newSortDirection;
        }

        #endregion

    }
}