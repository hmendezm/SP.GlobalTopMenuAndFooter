using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;

namespace SP.GlobalTopMenu
{
    public partial class ucSettings : UserControl
    {
        #region properties

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

        #endregion properties

        #region events

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (!this.Page.IsPostBack)
            {
                this.createTrvGlobalNavFooter();

                this.createTrvGroups();

                getGeneralSettings();
            }
        }

        protected void trvGlobalNavFooter_SelectedNodeChanged(object sender, EventArgs e)
        {
            try
            {
                if (trvGlobalNavFooter.SelectedNode.Value != "ExternalLnks")
                {
                    if (trvGlobalNavFooter.SelectedNode.Parent == null || trvGlobalNavFooter.SelectedNode.Parent.Value != "ExternalLnks")
                    {
                        getSelectedNodeInfo(trvGlobalNavFooter.SelectedNode);
                        hidAccordionIndex.Text = "0";
                    }
                    else
                    {
                        getSelectedExternalLnkNodeInfo(trvGlobalNavFooter.SelectedNode);
                        hidAccordionIndex.Text = "4";
                    }
                }
            }
            catch (Exception ex)
            {
                Helper.writeLog(ex);
            }
        }


        protected void btnSaveGeneralSettings_Click(object sender, EventArgs e)
        {
            try
            {
                UpdateGeneralSettings();
            }
            catch (Exception ex)
            {
 
               Helper.writeLog(ex);
            }
        
        }

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
                Helper.writeLog(ex);
            }
        }

        protected void btnDeleteMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
            }
            catch (Exception ex)
            {
                Helper.writeLog(ex);
            }
        }

        protected void trvGroups_SelectedNodeChanged(object sender, EventArgs e)
        {
            getSelectedNodeGroupInfo(trvGroups.SelectedNode);
        }

        protected void btnSaveGroup_Click(object sender, EventArgs e)
        {
            if (btnSaveGroup.Value == "Edit")
            {
                btnAddGroup.Value = "Cancel";
                btnSaveGroup.Value = "Save";
                btnDeleteGroup.Visible = false;
                txtGroupDescription.Enabled = true;
                txtGroupName.Enabled = true;
                txtGroupUrl.Enabled = true;
                ddlGrpPositions.Enabled = true;
                ddlParentGroups.Enabled = true;
            }
            else
            {
                btnAddGroup.Value = "Add";
                btnSaveGroup.Value = "Edit";
                btnDeleteGroup.Visible = true;
                txtGroupDescription.Enabled = false;
                txtGroupName.Enabled = false;
                txtGroupUrl.Enabled = false;
                ddlGrpPositions.Enabled = false;
                ddlParentGroups.Enabled = false;

                if (!String.IsNullOrEmpty(txtGroupName.Text))
                    updateGroupInfo(txtGroupID.Text);
            }
        }

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
                txtGroupUrl.Enabled = true;
                txtGroupName.Enabled = true;
                ddlGrpPositions.Enabled = true;
                ddlParentGroups.Enabled = true;
            }
            else
            {
                btnAddGroup.Value = "Add";
                btnSaveGroup.Value = "Edit";
                btnDeleteGroup.Visible = true;

                txtGroupDescription.Enabled = false;
                txtGroupUrl.Enabled = false;
                txtGroupName.Enabled = false;
                ddlGrpPositions.Enabled = false;
                ddlParentGroups.Enabled = false;
            }
        }

        protected void btnDeleteGroup_Click(object sender, EventArgs e)
        {
            removeGroupItemFromXML(txtGroupID.Text);
        }

        protected void btnExternalLnkSave_Click(object sender, EventArgs e)
        {
            if (btnExternalLnkSave.Value == "Edit")
            {
                btnExternalLnkAdd.Value = "Cancel";
                btnExternalLnkSave.Value = "Save";
                btnExternalLnkDelete.Visible = false;
                txtExternalLnkDescription.Enabled = true;
                txtExternalLnkTitle.Enabled = true;
                txtExternalLnkUrl.Enabled = true;
                rcbExternalLnkPosition.Enabled = true;
                ddlExternalLnkParent.Enabled = true;

                chkExternalLnkAddToFooter.Enabled = true;
                chkExternalLnkAddToGlobalNav.Enabled = true;
            }
            else
            {
                btnExternalLnkAdd.Value = "Add";
                btnExternalLnkSave.Value = "Edit";
                btnExternalLnkDelete.Visible = true;
                txtExternalLnkDescription.Enabled = false;
                txtExternalLnkTitle.Enabled = false;
                txtExternalLnkUrl.Enabled = false;
                rcbExternalLnkPosition.Enabled = false;
                ddlExternalLnkParent.Enabled = false;

                chkExternalLnkAddToFooter.Enabled = false;
                chkExternalLnkAddToGlobalNav.Enabled = false;

                if (!String.IsNullOrEmpty(txtExternalLnkTitle.Text))
                    updateExternalLnkInfo(txtExternalLnkUrl.Text);
            }
        }

        protected void btnExternalLnkAdd_Click(object sender, EventArgs e)
        {
            if (btnExternalLnkAdd.Value == "Add")
            {
                txtExternalLnkTitle.Text = String.Empty;
                txtExternalLnkUrl.Text = String.Empty;
                txtExternalLnkDescription.Text = String.Empty;
                txtExternalLnkID.Text = Guid.NewGuid().ToString();

                chkExternalLnkAddToFooter.Checked = false;
                chkExternalLnkAddToGlobalNav.Checked = false;
                chkExternalLnkAddToFooter.Enabled = true;
                chkExternalLnkAddToGlobalNav.Enabled = true;

                btnExternalLnkAdd.Value = "Cancel";
                btnExternalLnkSave.Value = "Save";
                btnExternalLnkDelete.Visible = false;
                btnExternalLnkSave.Visible = true;

                txtExternalLnkTitle.Enabled = true;
                txtExternalLnkUrl.Enabled = true;
                txtExternalLnkDescription.Enabled = true;
                rcbExternalLnkPosition.Enabled = true;
                ddlExternalLnkParent.Enabled = true;
            }
            else
            {
                btnExternalLnkAdd.Value = "Add";
                btnExternalLnkSave.Value = "Edit";
                btnExternalLnkDelete.Visible = true;

                txtExternalLnkTitle.Enabled = false;
                txtExternalLnkUrl.Enabled = false;
                txtExternalLnkDescription.Enabled = false;
                rcbExternalLnkPosition.Enabled = false;
                ddlExternalLnkParent.Enabled = false;

                chkExternalLnkAddToFooter.Enabled = false;
                chkExternalLnkAddToGlobalNav.Enabled = false;
            }
        }

        protected void btnExternalLnkDelete_Click(object sender, EventArgs e)
        {
        }

        #endregion events

        #region Group Methods


        private void getGeneralSettings()
        {
            try
            {
                StringDictionary lstSettings = XMLHelper.getGeneralSettings();
                if (lstSettings != null)
                {
                    chkIncludeBreadCrumb.Checked = Convert.ToBoolean(lstSettings["IncludeBreadCrumb"]);
                    chkAddSiteOwnerOption.Checked = Convert.ToBoolean(lstSettings["AddSiteOwnerOption"]);
                }

                SPSecurity.RunWithElevatedPrivileges(
                   delegate()
                   {
                       lblSPGlobalTopMenuUser.Text=lblSPGlobalTopMenuUser.Text.Replace("[0]", SPContext.Current.Web.CurrentUser.LoginName);
                   
                   });

            }
            catch (Exception ex)
            {
               Helper.writeLog(ex);
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

                //Update rcbPositions values.
                fillPosition(ref ddlGrpPositions);
                
                if (this.trvGroups.SelectedNode != null)
                {
                    iselectedNodeIndex = trvGroups.Nodes.IndexOf(trvGroups.SelectedNode);
                    rtnSelectedNode = this.trvGroups.SelectedNode;
                }

                this.trvGroups.Nodes.Clear();

                SPSecurity.RunWithElevatedPrivileges(
                    delegate()
                    {
                        XDocument xDoc = XMLHelper.GetXDocument(XMLHelper.XMLType.XMLGROUPNAMES);

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
                getAllGroupParents();

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
                Helper.writeLog(ex);
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
                        XDocument xDoc = XMLHelper.GetXDocument(XMLHelper.XMLType.XMLGROUPNAMES);

                        ddlGroupNames.Items.Clear();

                        ListItem lstiEmpty = new ListItem();

                        lstiEmpty.Text = String.Empty;
                        lstiEmpty.Value = "0";

                        ddlGroupNames.Items.Add(lstiEmpty);

                        ddlExternalLnkParent.Items.Clear();
                        ddlExternalLnkParent.Items.Add(lstiEmpty);

                        if (xDoc.DescendantNodes().ToList().Count > 1)
                        {
                            var q = from c in xDoc.Elements("GroupNames").Elements("Group")
                                    where (string.IsNullOrEmpty(c.Element("ParentId").Value.ToString()))
                                    select c;

                            foreach (var item in q)
                            {
                                ListItem lstigroup = new ListItem();

                                lstigroup.Text = item.Element("Name").Value;
                                lstigroup.Value = item.Element("Id").Value;

                                //Add the SubGroups

                                var qSubgroups = from c in xDoc.Elements("GroupNames").Elements("Group")
                                                 where (!string.IsNullOrEmpty(c.Element("ParentId").Value.ToString()) && c.Element("ParentId").Value.ToString() == lstigroup.Value)
                                                 select c;
                                ddlGroupNames.Items.Add(lstigroup);
                                ddlExternalLnkParent.Items.Add(lstigroup);
                                foreach (var subgroup in qSubgroups)
                                {
                                    ListItem lstiSubgroup = new ListItem();
                                    lstiSubgroup.Text = subgroup.Element("Name").Value;
                                    lstiSubgroup.Value = subgroup.Element("Id").Value;

                                    ddlGroupNames.Items.Add(lstiSubgroup);
                                    ddlExternalLnkParent.Items.Add(lstigroup);
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
        ///
        /// </summary>
        /// <param name="e"></param>
        private void getSelectedNodeGroupInfo(TreeNode e)
        {
            try
            {
                StringDictionary strdSettings = XMLHelper.getGroupSettings(e.Value);

                if (strdSettings != null)
                {
                    txtGroupName.Text = strdSettings["Name"].ToString();
                    txtGroupUrl.Text = strdSettings["Url"].ToString();
                    txtGroupDescription.Text = strdSettings["Description"].ToString();
                    ddlParentGroups.SelectedValue = strdSettings["ParentId"].ToString();
                    txtGroupID.Text = strdSettings["Id"].ToString();
                    ddlGrpPositions.SelectedValue = strdSettings["Position"].ToString();
                }
            }
            catch (Exception ex)
            {
                Helper.writeLog(ex);
            }
        }

        /// <summary>
        ///
        /// </summary>
        private void getAllGroupParents()
        {
            SPSecurity.RunWithElevatedPrivileges(
                delegate()
                {
                    XDocument xDoc = XMLHelper.GetXDocument(XMLHelper.XMLType.XMLGROUPNAMES);
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
                        XDocument xDoc = XMLHelper.GetXDocument(XMLHelper.XMLType.XMLGROUPNAMES);

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
                                    e.SetElementValue("Url", txtGroupUrl.Text);
                                    e.SetElementValue("Position", String.IsNullOrEmpty(ddlGrpPositions.SelectedValue) ? "0" : ddlGrpPositions.SelectedValue);
                                    e.SetElementValue("Description", txtGroupDescription.Text);

                                    e.SetElementValue("ParentId", ddlParentGroups.SelectedValue);

                                    bElementChanged = true;
                                }

                                if (bElementChanged)
                                {
                                    XMLHelper.UploadXDocumentToDocLib(xDoc, true, XMLHelper.XMLType.XMLGROUPNAMES);
                                }
                            }
                            else
                            {
                                xDoc.Element("GroupNames").Add(new XElement("Group",
                                    new XElement("Id", strGroupId),
                                    new XElement("Name", txtGroupName.Text),
                                    new XElement("Url", txtGroupUrl.Text),
                                    new XElement("Description", txtGroupDescription.Text),
                                    new XElement("Position", ddlGrpPositions.SelectedValue),
                                    new XElement("ParentId", ddlParentGroups.SelectedValue)));
                                XMLHelper.UploadXDocumentToDocLib(xDoc, true, XMLHelper.XMLType.XMLGROUPNAMES);
                            }
                        }
                        else
                        {
                            xDoc.Element("GroupNames").Add(new XElement("Group",
                                new XElement("Id", strGroupId),
                                new XElement("Name", txtGroupName.Text),
                                new XElement("Url", txtGroupUrl.Text),
                                new XElement("Description", txtGroupDescription.Text),
                                new XElement("Position", ddlGrpPositions.SelectedValue),
                                new XElement("ParentId", ddlParentGroups.SelectedValue)));

                            XMLHelper.UploadXDocumentToDocLib(xDoc, true, XMLHelper.XMLType.XMLGROUPNAMES);
                        }
                    });
                createTrvGroups();
                return bElementChanged;
            }
            catch (Exception ex)
            {
                Helper.writeLog(ex);
                return bElementChanged;
            }
        }

        private void UpdateGeneralSettings()
        {

            bool bElementChanged = false;
            try
            {
                SPSecurity.RunWithElevatedPrivileges(
                    delegate()
                    {
                        XDocument xDoc = XMLHelper.GetXDocument(XMLHelper.XMLType.XMLSETTINGS);

                        if (xDoc.DescendantNodes().ToList().Count > 1)
                        {
                            var q = from c in xDoc.Elements("Settings").Elements("Option")
                                    select c;

                            if (q.Count() > 0)
                            {
                                // convert the list to an array so that we're not modifying the
                                // collection that we're iterating over
                                foreach (XElement e in q.ToArray())
                                {
                                    e.SetElementValue("AddSiteOwnerOption",chkAddSiteOwnerOption.Checked.ToString());
                                    e.SetElementValue("IncludeBreadCrumb", chkIncludeBreadCrumb.Checked.ToString());
                                    bElementChanged = true;
                                }

                                if (bElementChanged)
                                {
                                    XMLHelper.UploadXDocumentToDocLib(xDoc, true, XMLHelper.XMLType.XMLSETTINGS);
                                }
                            }
                            else
                            {
                                xDoc.Element("Settings").Add(new XElement("Option",
                                    new XElement("AddSiteOwnerOption", chkAddSiteOwnerOption.Checked.ToString()),
                                    new XElement("IncludeBreadCrumb", chkIncludeBreadCrumb.Checked.ToString())));

                                XMLHelper.UploadXDocumentToDocLib(xDoc, true, XMLHelper.XMLType.XMLSETTINGS);
                            }
                        }
                        else
                        {
                            xDoc.Element("Settings").Add(new XElement("Option",
                                new XElement("AddSiteOwnerOption", chkAddSiteOwnerOption.Checked.ToString()),
                                new XElement("IncludeBreadCrumb", chkIncludeBreadCrumb.Checked.ToString())));

                            XMLHelper.UploadXDocumentToDocLib(xDoc, true, XMLHelper.XMLType.XMLSETTINGS);
                        }
                    });
            }
            catch (Exception ex)
            {
                Helper.writeLog(ex);
            }
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
                        XDocument xDoc = XMLHelper.GetXDocument(XMLHelper.XMLType.XMLGROUPNAMES);

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

                            XMLHelper.UploadXDocumentToDocLib(xDoc, true, XMLHelper.XMLType.XMLGROUPNAMES);
                        }
                    });
                createTrvGroups();
            }
            catch (Exception ex)
            {
                Helper.writeLog(ex);
            }
        }

        #endregion Group Methods

        #region External Links Methods

        /// <summary>
        ///
        /// </summary>
        private void AddExternalLinksTotrvGlobalNav()
        {
            try
            {
                int iMaxPosition = 0;
                SPSecurity.RunWithElevatedPrivileges(
                       delegate()
                       {
                           XDocument xDoc = XMLHelper.GetXDocument(XMLHelper.XMLType.XMLGLOBALNAV);

                           if (xDoc.DescendantNodes().ToList().Count > 1)
                           {
                               if (trvGlobalNavFooter.FindNode("ExternalLnks") != null)
                                   if (!string.IsNullOrEmpty(trvGlobalNavFooter.FindNode("ExternalLnks").Text))
                                   {
                                       trvGlobalNavFooter.FindNode("ExternalLnks").Selected = true;
                                       trvGlobalNavFooter.Nodes.Remove(trvGlobalNavFooter.SelectedNode);
                                   }

                               iMaxPosition = ((from c in xDoc.Elements("GlobalNav").Elements("Item")
                                                select (int.Parse(c.Element("Position").Value.Trim().Length > 0 ? c.Element("Position").Value : "0"))).Max() + 1);

                               var q = from c in xDoc.Elements("GlobalNav").Elements("Item")
                                       where (string.IsNullOrEmpty(c.Element("ParentId").Value.ToString())) &&
                                       c.Element("ExternalLnk") != null ? (bool.Parse(c.Element("ExternalLnk").Value.Trim().Length > 0 ? c.Element("ExternalLnk").Value : "false")) : false
                                       orderby Convert.ToInt64(c.Element("Position").Value.Trim().Length) == 0 ? iMaxPosition : Convert.ToInt64(c.Element("Position").Value) ascending
                                       select c;
                               if (q.Count() > 0)
                               {
                                   TreeNode newNodeExternalLink = new TreeNode();
                                   newNodeExternalLink.Text = "External Links";
                                   newNodeExternalLink.Value = "ExternalLnks";

                                   foreach (var item in q)
                                   {
                                       TreeNode newNode = new TreeNode();

                                       string strTitle = item.Element("SiteTitle").Value + "&nbsp;&nbsp;";

                                       foreach (string strIcon in this.getIcons(item.Element("SiteUrl").Value))
                                       {
                                           strTitle += string.Format("<span></span><img height='15' align='middle' src='{0}/_layouts/SP.GlobalTopMenu/Images/Common/{1}' alt=''/>", SPContext.Current.Web.Url, strIcon);
                                       }

                                       StringDictionary strdSettings = XMLHelper.getSettings(item.Element("SiteUrl").Value, Helper.FindBy.BySiteUrl);

                                       if (strdSettings != null)
                                       {
                                           if (Convert.ToInt16(String.IsNullOrEmpty(strdSettings["position"].ToString()) ? "0" : strdSettings["position"].ToString()) > 0)
                                           {
                                               this.createImage(strdSettings["position"]);
                                               strTitle += string.Format("<span style='position: absolute;float:right;'></span><img height='15' align='middle' src='{0}/{1}.jpg' alt=''></img>",
                                                                           Helper.SiteRootUrl + "/" + Helper.GTM_LIBRARY + "/" + Helper.IMG_FOLDER, strdSettings["position"]);
                                           }
                                       }

                                       newNode.Text = strTitle;
                                       newNode.Value = item.Element("SiteUrl").Value;
                                       newNodeExternalLink.ChildNodes.Add(newNode);
                                   }
                                   trvGlobalNavFooter.Nodes.Add(newNodeExternalLink);
                               }
                           }
                       });
            }
            catch (Exception ex) { Helper.writeLog(ex); }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="strExternalLnkId"></param>
        /// <returns></returns>
        private bool updateExternalLnkInfo(string strExternalLnkId)
        {
            try
            {
                SPSecurity.RunWithElevatedPrivileges(
                    delegate()
                    {
                        XDocument xDoc = XMLHelper.GetXDocument(XMLHelper.XMLType.XMLGLOBALNAV);
                        if (!chkExternalLnkAddToGlobalNav.Checked && !chkExternalLnkAddToFooter.Checked)
                        {
                            this.removeGlobalNavItemFromXML(xDoc, txtExternalLnkID.Text);
                        }
                        else if (rcbPositions.SelectedValue.Trim().Length > 0 || chkExternalLnkAddToGlobalNav.Checked || chkExternalLnkAddToFooter.Checked)
                        {
                            this.removeGlobalNavItemFromXML(xDoc, txtExternalLnkID.Text);

                            string strGroupId = ddlExternalLnkParent.SelectedValue;

                            xDoc.Element("GlobalNav").Add(new XElement("Item", new XElement("SiteId", txtExternalLnkID.Text),
                                   new XElement("SiteTitle", txtExternalLnkTitle.Text),
                                   new XElement("NewTitle", string.Empty),
                                   new XElement("SiteDescription", txtExternalLnkDescription.Text),
                                   new XElement("SiteUrl", txtExternalLnkUrl.Text),
                                   new XElement("Position", String.IsNullOrEmpty(rcbExternalLnkPosition.SelectedValue) ? "0" : rcbExternalLnkPosition.SelectedValue),
                                   new XElement("GroupId", ddlExternalLnkParent.SelectedValue),
                                   new XElement("GlobalNav", chkExternalLnkAddToGlobalNav.Checked.ToString()),
                                   new XElement("Footer", chkExternalLnkAddToFooter.Checked.ToString()),
                                   new XElement("ExternalLnk", true),
                                   new XElement("ParentId", string.Empty)));

                            XMLHelper.UploadXDocumentToDocLib(xDoc, true, XMLHelper.XMLType.XMLGLOBALNAV);
                        }
                        else
                        {
                            this.removeGlobalNavItemFromXML(xDoc, txtExternalLnkID.Text);
                        }

                        AddExternalLinksTotrvGlobalNav();
                    });
            }
            catch (Exception ex) { Helper.writeLog(ex); }
            return true;
        }

        /// <summary>
        /// Gets all information of a selected Node.
        /// </summary>
        /// <param name="e"></param>
        private void getSelectedExternalLnkNodeInfo(TreeNode e)
        {
            try
            {
                txtExternalLnkTitle.Text = (e.Text.Split(';')[0].ToString()).Split('&')[0].ToString();

                StringDictionary strdSettings = XMLHelper.getSettings(e.Value, Helper.FindBy.BySiteUrl);

                if (strdSettings != null)
                {
                    //Get the Global Navigation value
                    int value;
                    bool bIsNumber = int.TryParse(strdSettings["GlobalNav"].ToString(), out value);

                    if (bIsNumber)
                        chkExternalLnkAddToGlobalNav.Checked = Convert.ToBoolean(value);
                    else
                        chkExternalLnkAddToGlobalNav.Checked = Convert.ToBoolean(strdSettings["GlobalNav"]);

                    bIsNumber = int.TryParse(strdSettings["Footer"].ToString(), out value);
                    if (bIsNumber)
                        chkExternalLnkAddToFooter.Checked = Convert.ToBoolean(value);
                    else
                        chkExternalLnkAddToFooter.Checked = Convert.ToBoolean(strdSettings["Footer"]);

                    if (ddlExternalLnkParent.Items.Count > 0)
                        ddlExternalLnkParent.SelectedValue = strdSettings["GroupId"].ToString();

                    if (ddlExternalLnkParent.SelectedValue.Trim().Length == 0)
                        ddlExternalLnkParent.Text = string.Empty;

                    rcbExternalLnkPosition.SelectedValue = strdSettings["Position"].ToString() == "0" ? "" : strdSettings["Position"].ToString();

                    if (rcbExternalLnkPosition.SelectedValue.Trim().Length == 0)
                        rcbExternalLnkPosition.Text = string.Empty;

                    txtExternalLnkUrl.Text = strdSettings["url"].ToString();
                    txtExternalLnkID.Text = strdSettings["SiteId"].ToString();

                    txtExternalLnkDescription.Text = strdSettings["description"].ToString();
                }
                else
                {
                    chkAddToGlobalNav.Checked = false;
                    chkAddToFooter.Checked = false;
                    ddlGroupNames.ClearSelection();
                    ddlExternalLnkParent.ClearSelection();
                    rcbExternalLnkPosition.ClearSelection();
                    rcbPositions.ClearSelection();
                    rdtxtChangeTitle.Text = string.Empty;
                }
            }
            catch (Exception ex)
            {
                Helper.writeLog(ex);
            }
        }

        #endregion External Links Methods

        #region Menu Items Methods

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

                //get rcbPositions values.
                fillPosition(ref rcbPositions);

                //get rcbExternalLnkPosition values.
                fillPosition(ref rcbExternalLnkPosition);


                if (this.trvGlobalNavFooter.SelectedNode != null)
                {
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
                            if (Helper.IsUserHasAccess(site.RootWeb.Url, userLoginName))
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


                            AddChildren(SiteCollectionItem, userLoginName, site.RootWeb);

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
                        //Add External Links to the Treeview
                        AddExternalLinksTotrvGlobalNav();
                    });
            }
            catch (Exception ex)
            {
                Helper.writeLog(ex);
            }
        }

        private void AddChildren(TreeNode ParentNode, string userLoginName, SPWeb SPParentSite)
        {
            TreeNode SubSiteItem = null;
            //Get all Sub Site of the Site collection
            foreach (SPWeb web in SPParentSite.Webs)
            {
                if (Helper.IsUserHasAccess(web.Url, userLoginName))
                {

                    if (web.ParentWeb != null && web.ParentWeb.Url == SPParentSite.Url)
                    {
                        SubSiteItem=this.createTreeViewNode(web.ServerRelativeUrl, web.Title, web.Description != null ? web.Description : "", ParentNode, this.getIcons(web.ServerRelativeUrl));
                        if (web.Webs.Count > 0)
                        {
                            AddChildren(SubSiteItem, userLoginName, web);
                        }
                    }
       
                }
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
                strTitle += "&nbsp;&nbsp;";

                foreach (string strIcon in strIcons)
                {
                    strTitle += string.Format("<span></span><img height='15' align='middle' src='{0}/_layouts/SP.GlobalTopMenu/Images/Common/{1}' alt=''/>", SPContext.Current.Web.Url, strIcon);
                }

                StringDictionary strdSettings = XMLHelper.getSettings(strUrl, Helper.FindBy.BySiteUrl);

                if (strdSettings != null)
                {
                    if (Convert.ToInt16(String.IsNullOrEmpty(strdSettings["position"].ToString()) ? "0" : strdSettings["position"].ToString()) > 0)
                    {
                        this.createImage(strdSettings["position"]);
                        strTitle += string.Format("<span style='position: absolute;float:right;'></span><img height='15' align='middle' src='{0}/{1}.jpg' alt=''></img>",
                                                    Helper.SiteRootUrl + "/" + Helper.GTM_LIBRARY + "/" + Helper.IMG_FOLDER, strdSettings["position"]);
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
                    if (FindNode(trvGlobalNavFooter, mniParent.Value) != null)
                        mniParent.ChildNodes.Add(mniChild);
                }
                return mniChild;
            }
            catch (Exception ex)
            {
                Helper.writeLog(ex);
                return mniChild;
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

                StringDictionary strdSettings = XMLHelper.getSettings(e.Value, Helper.FindBy.BySiteUrl);

                this.createSiteAdminsList(e.Value);
                
                
                ShowAddInsideParent(this.trvGlobalNavFooter.SelectedValue);

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


                    chkAddInsideParent.Checked = strdSettings["AddInsideParent"] == null ? false : Convert.ToBoolean(strdSettings["AddInsideParent"]);


                    if (ddlGroupNames.Items.Count > 0)
                        ddlGroupNames.SelectedValue = strdSettings["GroupId"].ToString();

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
                Helper.writeLog(ex);
            }
        }


        private void ShowAddInsideParent(string SiteUrl)
        {
            try
            {
                SPSecurity.RunWithElevatedPrivileges(
                    delegate()
                    {
                        using (SPWeb web = SPContext.Current.Site.WebApplication.Sites[SiteUrl].OpenWeb())
                        {
                            XDocument xDoc = XMLHelper.GetXDocument(XMLHelper.XMLType.XMLGLOBALNAV);

                            chkAddInsideParent.Visible = lblAddInsideParent.Visible = isSiteInXMLFile(xDoc, web.ParentWebId.ToString());
                                
                        }
                    });
            }
            catch (Exception ex)
            {
                Helper.writeLog(ex);
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
                            XDocument xDoc = XMLHelper.GetXDocument(XMLHelper.XMLType.XMLGLOBALNAV);
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
                                    new XElement("AddInsideParent",chkAddInsideParent.Checked.ToString()),
                                    new XElement("Footer", chkAddToFooter.Checked.ToString()),
                                    new XElement("ExternalLnk", false),
                                    new XElement("ParentId", !chkAddInsideParent.Checked?string.Empty:
                                                            (string.IsNullOrEmpty(strGroupId) || Convert.ToInt16(strGroupId) == 0 ? 
                                                                    (web.ParentWebId != Guid.Empty ? web.ParentWebId.ToString() : string.Empty) : string.Empty)))); 

                                XMLHelper.UploadXDocumentToDocLib(xDoc, true, XMLHelper.XMLType.XMLGLOBALNAV);
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
                Helper.writeLog(ex);
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
                        XDocument xDoc = XMLHelper.GetXDocument(XMLHelper.XMLType.XMLGLOBALNAV);

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
                            e.SetAttributeValue("AddInsideParent", chkAddInsideParent.Checked.ToString());

                            bElementChanged = true;
                        }

                        if (bElementChanged)
                        {
                            XMLHelper.UploadXDocumentToDocLib(xDoc, true, XMLHelper.XMLType.XMLGLOBALNAV);
                        }
                    });
                return bElementChanged;
            }
            catch (Exception ex)
            {
                Helper.writeLog(ex);
                return bElementChanged;
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
                        XMLHelper.UploadXDocumentToDocLib(xDoc, true, XMLHelper.XMLType.XMLGLOBALNAV);
                    });
            }
            catch (Exception ex)
            {
                Helper.writeLog(ex);
            }
        }

        private bool isSiteInXMLFile(XDocument xDoc, string strSiteId)
        {
            bool bexist = false;
            try
            {
                SPSecurity.RunWithElevatedPrivileges(
                    delegate()
                    {
                        var q = from c in xDoc.Elements("GlobalNav").Elements("Item")
                                where (string)c.Element("SiteId") == strSiteId
                                select c;
                        bexist = q.Count() > 0;
                       
                    });
                return bexist;
            }
            catch (Exception ex)
            {
                Helper.writeLog(ex);
                return false;
            }
        }
        #endregion Menu Items Methods

        #region utility

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
                StringDictionary strdSettings = XMLHelper.getSettings(SiteUrl, Helper.FindBy.BySiteUrl);
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
                Helper.writeLog(ex);
                return strIcons;
            }
        }

        /// <summary>
        /// Fills the positions in the RadCombobox
        /// </summary>
        private void fillPosition(ref DropDownList dllSource)
        {
            try
            {
                XDocument xDoc = XMLHelper.GetXDocument(XMLHelper.XMLType.XMLGLOBALNAV);
                string strTitle;

                if (xDoc.Elements("GlobalNav").Elements("Item").Count() > 0)
                {
                    dllSource.Items.Clear();
                    //rcbExternalLnkPosition.Items.Clear();

                    clearDropdownList(ref dllSource);

                   // clearDropdownList(ref rcbExternalLnkPosition);

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

                        if (!bEnabled)
                        {
                            rcbPosition.Attributes.Add("disabled", "disabled");
                        }

                        dllSource.Items.Add(rcbPosition);
                        //rcbExternalLnkPosition.Items.Add(rcbPosition);
                    }
                }
                else
                {
                    clearDropdownList(ref dllSource);
                    //clearDropdownList(ref rcbExternalLnkPosition);

                    ListItem rcbPosition = new ListItem();
                    rcbPosition.Text = "1";
                    rcbPosition.Value = "1";
                    rcbPosition.Enabled = true;

                    dllSource.Items.Add(rcbPosition);
                    //rcbExternalLnkPosition.Items.Add(rcbPosition);
                }
            }
            catch (Exception ex)
            {
                Helper.writeLog(ex);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="ddlSource"></param>
        private void clearDropdownList(ref DropDownList ddlSource)
        {
            try
            {
                ListItem rcbEmptyItem = new ListItem();
                rcbEmptyItem.Text = String.Empty;
                rcbEmptyItem.Value = String.Empty;
                rcbEmptyItem.Enabled = true;
                ddlSource.Items.Add(rcbEmptyItem);
            }
            catch (Exception ex)
            {
                Helper.writeLog(ex);
            }
        }

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
                Helper.writeLog(ex);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="strText"></param>
        private void createImage(string strText)
        {
            try
            {
                SPFolder oTargetFolder = null;
                SPSecurity.RunWithElevatedPrivileges(delegate()
                {
                    using (SPSite oSite = new SPSite(Helper.SiteRootUrl))
                    {
                        using (SPWeb oWeb = oSite.OpenWeb())
                        {
                            oSite.AllowUnsafeUpdates = true;
                            oWeb.AllowUnsafeUpdates = true;

                            //Get Images Folder
                            if (oWeb.GetFolder(Helper.GTM_LIBRARY + "\\" + Helper.IMG_FOLDER).Exists)
                                oTargetFolder = oWeb.Folders[Helper.GTM_LIBRARY].SubFolders[Helper.IMG_FOLDER];
                            else
                            {
                                oWeb.Folders[Helper.GTM_LIBRARY].SubFolders.Add(Helper.IMG_FOLDER);
                                oWeb.Folders[Helper.GTM_LIBRARY].Update();
                                oTargetFolder = oWeb.Folders[Helper.GTM_LIBRARY].SubFolders[Helper.IMG_FOLDER];
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

                                    SPFile pic = oTargetFolder.Files.Add(string.Format("{0}.jpg", strText), streamImage, true);
                                    pic.Update();
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
                Helper.writeLog(ex);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="strImageName"></param>
        /// <returns></returns>
        public static string GetImagefromLibrary(string strImageName)
        {
            try
            {
                using (SPSite oSite = new SPSite(Helper.SiteRootUrl))
                {
                    using (SPWeb oWeb = oSite.OpenWeb())
                    {
                        SPFolder oTargetFolder = oWeb.Folders[Helper.GTM_LIBRARY];
                        SPFile spFile;

                        if (oWeb.GetFolder(oTargetFolder.Url + "/" + Helper.IMG_FOLDER).Exists)
                        {
                            if (oWeb.GetFile(oTargetFolder.SubFolders[Helper.IMG_FOLDER].Url + "/" + strImageName + ".jpg").Exists)
                            {
                                spFile = oTargetFolder.SubFolders[Helper.XML_FOLDER].Files[strImageName + ".jpg"];
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

        #endregion utility

        #region rgSiteAdmins Grid Methods

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

                        dtUserInfo.Tables.Add(Helper.LINQToDataTable(queryUsers));

                        SetViewState(dtUserInfo);

                        rgSiteAdmins.DataSource = GetViewState();
                        rgSiteAdmins.DataBind();
                    }
                }
            }
            catch (Exception ex)
            {
                Helper.writeLog(ex);
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
                Helper.writeLog(ex);
                return "-------";
            }
        }

        #endregion rgSiteAdmins Grid Methods
    }
}