using System;
using System.Collections.Specialized;
using System.Linq;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Navigation;

//using Microsoft.SharePoint.Publishing;

namespace SP.GlobalTopMenu
{
    public partial class ucGlobalNav : UserControl
    {
        #region Properties

        public string AddUrl
        {
            get
            {
                return CacheHelper.Get<string>("AddUrl");
            }
            set
            {
                CacheHelper.Add<string>(value, "AddUrl");
            }
        }

        public Boolean LocalPostBack
        {
            get
            {
                return Convert.ToBoolean(CacheHelper.Get<string>("isPostBack"));
            }
            set
            {
                CacheHelper.Add<string>(value.ToString(), "isPostBack");
            }
        }

        #endregion Properties

        #region Events

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            CreateGlobalMenu();
            StringDictionary lstSettings = XMLHelper.getGeneralSettings();
            if (lstSettings != null)
            {
                if (Convert.ToBoolean(lstSettings["AddSiteOwnerOption"]))
                    AddSiteNavigation();

                if (!Convert.ToBoolean(lstSettings["IncludeBreadCrumb"]))
                    this.Controls.Remove(this.FindControl("ucBreadcrumb1"));
            }
        }

        #endregion Events

        #region Using SPNavigation

        /// <summary>
        ///
        /// </summary>
        /// <param name="iChildrenCount"></param>
        /// <param name="userLoginName"></param>
        /// <param name="ChildrenNodes"></param>
        /// <param name="htmlul"></param>
        private void CreateNewOptionsToMenuForNavigation(ref Int64 iChildrenCount, string userLoginName, SPNavigationNodeCollection ChildrenNodes, ref HtmlGenericControl htmlul)
        {
            try
            {
                HtmlGenericControl htmlh2SubGroup = new HtmlGenericControl("h2");
                HtmlGenericControl htmlliSubGroup = new HtmlGenericControl("li");
                htmlliSubGroup.Attributes.Add("class", "heading");

                htmlh2SubGroup.Controls.Add(CreateAnchor("", ChildrenNodes.Parent.Title, "", "parent"));

                htmlliSubGroup.Controls.Add(htmlh2SubGroup);

                htmlul.Attributes.Add("class", "simple");
                htmlul.Controls.Add(htmlliSubGroup);

                foreach (SPNavigationNode node in ChildrenNodes)
                {
                    if (Helper.IsUserHasAccess(node.Url, userLoginName))
                    {
                        HtmlGenericControl htmlli = new HtmlGenericControl("li");
                        HtmlAnchor htmlAnchor = new HtmlAnchor();

                        htmlli.Controls.AddAt(0, CreateAnchor(node.Url, node.Title, node.Title, string.Empty));

                        ////Children Count
                        ++iChildrenCount;
                        htmlul.Controls.Add(htmlli);
                    }
                }
            }
            catch (Exception ex)
            {
                Helper.writeLog(ex);
            }
        }

        /// <summary>
        /// Add the Site Owner option to the Global navigation
        /// </summary>
        private void AddSiteNavigation()
        {
            Int64 iChildrenCount = 0;

            try
            {
                string userLoginName = SPContext.Current.Web.CurrentUser.LoginName;

                HtmlGenericControl li = new HtmlGenericControl("li");

                HtmlGenericControl htmlFirstDiv = new HtmlGenericControl("DIV");

                SPSecurity.RunWithElevatedPrivileges(
                    delegate()
                    {
                        using (SPSite site = new SPSite(SPContext.Current.Site.Url))
                        {
                            using (SPWeb web = site.OpenWeb(SPContext.Current.Web.ID))
                            {
                                //var pubWeb = PublishingWeb.GetPublishingWeb(web);

                                //if (pubWeb.Navigation.GlobalNavigationNodes.Count > 0)
                                if (web.Navigation.TopNavigationBar.Count > 0)
                                {
                                    HtmlGenericControl htmlSecondDiv = new HtmlGenericControl("DIV");
                                    htmlSecondDiv.Attributes.Add("class", "row");

                                    bool bLeftDivWasCreated = false;
                                    bool bMiddleDivWasCreated = false;
                                    bool bRightDivWasCreated = false;

                                    li.Controls.Add(CreateAnchor(SPContext.Current.Web.Url, SPContext.Current.Web.Title, SPContext.Current.Web.Description, "drop"));

                                    HtmlGenericControl htmlLeftThirdDiv = new HtmlGenericControl("DIV");
                                    htmlLeftThirdDiv.Attributes.Add("class", "col_left");

                                    HtmlGenericControl htmlMiddleThirdDiv = new HtmlGenericControl("DIV");
                                    htmlMiddleThirdDiv.Attributes.Add("class", "col_middle");

                                    HtmlGenericControl htmlRightThirdDiv = new HtmlGenericControl("DIV");
                                    htmlRightThirdDiv.Attributes.Add("class", "col_right");

                                    HtmlGenericControl htmlLeftul = new HtmlGenericControl("ul");
                                    HtmlGenericControl htmlMiddleul = new HtmlGenericControl("ul");
                                    HtmlGenericControl htmlRightul = new HtmlGenericControl("ul");

                                    // AddnavigationInfoToMenu(ref iChildrenCount, userLoginName, pubWeb,
                                    //     ref bLeftDivWasCreated, ref bMiddleDivWasCreated, ref bRightDivWasCreated, ref htmlLeftul, ref htmlMiddleul, ref htmlRightul);

                                    if (bLeftDivWasCreated && !bMiddleDivWasCreated)
                                    {
                                        htmlLeftThirdDiv.Controls.Add(htmlLeftul);
                                        htmlSecondDiv.Controls.Add(htmlLeftThirdDiv);
                                        htmlFirstDiv.Attributes.Add("class", "dropdown_column");
                                    }
                                    else if (bMiddleDivWasCreated && !bRightDivWasCreated)
                                    {
                                        htmlLeftThirdDiv.Controls.Add(htmlLeftul);
                                        htmlSecondDiv.Controls.Add(htmlLeftThirdDiv);

                                        htmlMiddleThirdDiv.Controls.Add(htmlMiddleul);
                                        htmlSecondDiv.Controls.Add(htmlMiddleThirdDiv);
                                        htmlFirstDiv.Attributes.Add("class", "dropdown_2columns");
                                    }
                                    else
                                    {
                                        htmlLeftThirdDiv.Controls.Add(htmlLeftul);
                                        htmlSecondDiv.Controls.Add(htmlLeftThirdDiv);

                                        htmlMiddleThirdDiv.Controls.Add(htmlMiddleul);
                                        htmlSecondDiv.Controls.Add(htmlMiddleThirdDiv);

                                        htmlRightThirdDiv.Controls.Add(htmlRightul);
                                        htmlSecondDiv.Controls.Add(htmlRightThirdDiv);
                                        htmlFirstDiv.Attributes.Add("class", "dropdown_3columns");
                                    }

                                    htmlFirstDiv.Controls.Add(htmlSecondDiv);
                                }
                                else
                                    li.Controls.Add(CreateAnchor(SPContext.Current.Web.Url, SPContext.Current.Web.Title, SPContext.Current.Web.Description, ""));

                                li.ID = web.Title;
                            }
                        }
                    });

                //Add all suboption to the Menu group.
                li.Controls.Add(htmlFirstDiv);

                GlobalMenu.Controls.Add(li);
            }
            catch (Exception ex)
            {
                Helper.writeLog(ex);
            }
        }

        //private void AddnavigationInfoToMenu(ref Int64 iChildrenCount, string userLoginName, PublishingWeb pubWeb,
        //    ref bool bLeftDivWasCreated, ref bool bMiddleDivWasCreated, ref bool bRightDivWasCreated, ref HtmlGenericControl htmlLeftul,
        //    ref HtmlGenericControl htmlMiddleul, ref HtmlGenericControl htmlRightul)
        //{
        //    foreach (SPNavigationNode node in pubWeb.Navigation.GlobalNavigationNodes)
        //    {
        //        ++iChildrenCount;

        //        if ((iChildrenCount + node.Children.Count) >= 1 && (iChildrenCount + node.Children.Count) <= 7)
        //            bLeftDivWasCreated = true;
        //        else if ((iChildrenCount + node.Children.Count) >= 8 && (iChildrenCount + node.Children.Count) <= 14)
        //            bMiddleDivWasCreated = true;
        //        else
        //            bRightDivWasCreated = true;

        //        if (node.Children.Count > 0)
        //        {
        //            if (bLeftDivWasCreated && !bMiddleDivWasCreated)
        //                CreateNewOptionsToMenuForNavigation(ref iChildrenCount, userLoginName, node.Children, ref htmlLeftul);
        //            else if (bMiddleDivWasCreated & !bRightDivWasCreated)
        //            {
        //                iChildrenCount = 8;
        //                CreateNewOptionsToMenuForNavigation(ref iChildrenCount, userLoginName, node.Children, ref htmlMiddleul);
        //            }
        //            else if (bRightDivWasCreated)
        //            {
        //                iChildrenCount = 15;
        //                CreateNewOptionsToMenuForNavigation(ref iChildrenCount, userLoginName, node.Children, ref htmlRightul);
        //            }
        //        }
        //        else
        //        {
        //            HtmlGenericControl htmlli = new HtmlGenericControl("li");
        //            htmlli.Attributes.Add("class", "heading");

        //            htmlli.Controls.AddAt(0, CreateAnchor(node.Url, node.Title, node.Title, string.Empty));

        //            if (bLeftDivWasCreated && !bMiddleDivWasCreated)
        //                htmlLeftul.Controls.Add(htmlli);
        //            else if (bMiddleDivWasCreated & !bRightDivWasCreated)
        //            {
        //                htmlMiddleul.Controls.Add(htmlli);
        //            }
        //            else if (bRightDivWasCreated)
        //            {
        //                htmlRightul.Controls.Add(htmlli);
        //            }
        //        }
        //    }
        //}

        #endregion Using SPNavigation

        #region XML Files

        /// <summary>
        ///
        /// </summary>
        private void CreateGlobalMenu()
        {
            try
            {
                StringDictionary strbAddedGroups = new StringDictionary();

                string userLoginName = SPContext.Current.Web.CurrentUser.LoginName;

                SPSecurity.RunWithElevatedPrivileges(
                    delegate()
                    {
                        /*All the options that do not have group assigned*/
                        XDocument xdMenuItems = XMLHelper.GetXDocument(XMLHelper.XMLType.XMLGLOBALNAV);

                        int iGlobalNavItemsCount = xdMenuItems.DescendantNodes().ToList().Count;

                        if (iGlobalNavItemsCount > 1)
                        {
                            Int64 iMaxItemPosition = ((from c in xdMenuItems.Elements("GlobalNav").Elements("Item")
                                                       select (int.Parse(int.Parse(c.Element("Position").Value) == 0 ? "0" : c.Element("Position").Value))).Max() + 1);

                            var q = from c in xdMenuItems.Elements("GlobalNav").Elements("Item")
                                    where (bool)c.Element("GlobalNav")
                                    orderby Convert.ToInt32(c.Element("Position").Value) == 0 ? iMaxItemPosition : Convert.ToInt32(c.Element("Position").Value) ascending
                                    select c;

                            foreach (XElement item in q.ToArray())
                            {
                                if (Helper.IsUserHasAccess(item.Element("SiteUrl").Value, userLoginName))
                                {
                                    StringDictionary lstSettings = XMLHelper.getSettings(item.Element("SiteUrl").Value, Helper.FindBy.BySiteUrl);
                                    String strGroupParentId = getGroupInfobyGroupId(lstSettings["groupid"].ToString())["ParentID"];
                                    string strGroupId;
                                    if (lstSettings != null)
                                    {
                                        if (String.IsNullOrEmpty(lstSettings["groupid"].ToString()) || lstSettings["groupid"].ToString() == "0")
                                        {
                                            if (String.IsNullOrEmpty(lstSettings["parentid"].ToString()) || lstSettings["parentid"].ToString() == "0")
                                            {
                                                if (!ItemHasChildren(lstSettings["siteid"].ToString()))
                                                {
                                                    CreateMenuItem(lstSettings);
                                                }
                                            }
                                            else
                                            {
                                                if (!strbAddedGroups.ContainsKey(lstSettings["parentid"].ToString())
                                                    && XMLHelper.ParentExist(lstSettings["parentid"].ToString()))
                                                {
                                                    AddParentWithChildren(lstSettings);
                                                    strbAddedGroups.Add(lstSettings["parentid"].ToString(), lstSettings["parentid"].ToString());
                                                }
                                                else if (!XMLHelper.ParentExist(lstSettings["parentid"].ToString()) && !ItemHasChildren(lstSettings["siteid"].ToString()))
                                                {
                                                    CreateMenuItem(lstSettings);
                                                }
                                            }
                                        }
                                        else
                                        {
                                            strGroupId = String.IsNullOrEmpty(strGroupParentId) ? lstSettings["groupid"].ToString() : strGroupParentId;
                                            if (!strbAddedGroups.ContainsKey(strGroupId))
                                            {
                                                //Add groups with items
                                                AddGroupWithChildren(lstSettings);
                                                strbAddedGroups.Add(strGroupId, strGroupId);
                                            }
                                        }

                                        //Add Navigation from Settings
                                    }
                                }
                            }
                        }
                    });

                //Add a Setting option to the Global navigation if user is Site Collection administrator.
                if (SPContext.Current.Site.RootWeb.CurrentUser.IsSiteAdmin && Convert.ToBoolean(AddUrl))
                {
                    ImageButton btnSettings = new ImageButton();
                    btnSettings.ImageUrl = "/_layouts/SP.GlobalTopMenu/Images/Menu/tools.png";
                    btnSettings.ID = "lmgbtnSettings1";
                    btnSettings.Attributes.Add("onClick", "return openDialogModal('/_layouts/SP.GlobalTopMenu/settings.aspx', 'Settings')");
                    btnSettings.CssClass = "Tools";

                    GlobalMenu.Controls.AddAt(0, btnSettings);
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
        /// <param name="htmlli"></param>
        /// <param name="iAtPosition"></param>
        private void SetTopGroupsOrder(HtmlGenericControl htmlli, ref int iAtPosition)
        {
            HtmlGenericControl htmlul = new HtmlGenericControl("ul");
            try
            {
                foreach (Control c in GlobalMenu.Controls.OfType<HtmlGenericControl>())
                {
                    HtmlGenericControl ctrl = (HtmlGenericControl)c;

                    if (ctrl.TagName == "li" && !String.IsNullOrEmpty(ctrl.ID))
                    {
                        if (Convert.ToInt16(ctrl.ID.Split('_')[1]) > Convert.ToInt16(htmlli.ID.Split('_')[1]))
                            break;

                        ++iAtPosition;
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
        /// <param name="lstSettings"></param>
        private void AddGroupWithChildren(StringDictionary lstSettings)
        {
            try
            {
                XDocument xDoc = XMLHelper.GetXDocument(XMLHelper.XMLType.XMLGROUPNAMES);

                var q = from c in xDoc.Elements("GroupNames").Elements("Group")
                        where c.Element("Id").Value.Trim() == lstSettings["groupId"].ToString().Trim()
                        orderby Convert.ToInt32(c.Element("Position").Value) == 0 ? 999 : Convert.ToInt32(c.Element("Position").Value) ascending
                        select c;

                foreach (var item in q)
                {
                    HtmlGenericControl li = new HtmlGenericControl("li");
                    int iAtPosition = 0;

                    //Add the group to the Menu.
                    if (AddGroupToMenu(ref li, item.Element("Name").Value, item.Element("Id").Value))
                    {
                        SetTopGroupsOrder(li, ref iAtPosition);

                        //Add the new li to Ul
                        if (iAtPosition > GlobalMenu.Controls.Count - 1)
                            GlobalMenu.Controls.Add(li);
                        else
                            GlobalMenu.Controls.AddAt(iAtPosition, li);
                    }
                }
            }
            catch (Exception ex)
            {
                Helper.writeLog(ex);
            }
        }

        private bool ItemHasChildren(string strid)
        {
            try
            {
                XDocument xdMenuItems = XMLHelper.GetXDocument(XMLHelper.XMLType.XMLGLOBALNAV);

                var q = from c in xdMenuItems.Elements("GlobalNav").Elements("Item")
                        where //(string.IsNullOrEmpty(c.Element("ParentId").Value.ToString()) || (XMLHelper.ParentExist(c.Element("ParentId").Value.ToString()) == 0)) &&
                              (bool)c.Element("GlobalNav") && (c.Element("ParentId").Value.Trim().Length > 0 ? c.Element("ParentId").Value : "0") == (strid)
                        orderby Convert.ToInt32(c.Element("Position").Value) == 0 ? 999 : Convert.ToInt32(c.Element("Position").Value) ascending
                        select c;

                if (q.Count() > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                Helper.writeLog(ex);
                return false;
            }
        }

        private void AddParentWithChildren(StringDictionary lstSettings)
        {
            try
            {
                XDocument xDoc = XMLHelper.GetXDocument(XMLHelper.XMLType.XMLGLOBALNAV);

                var parent = from c in xDoc.Elements("GlobalNav").Elements("Item")
                             where c.Element("SiteId").Value.Trim() == lstSettings["ParentId"].ToString().Trim()
                             orderby Convert.ToInt32(c.Element("Position").Value) == 0 ? 999 : Convert.ToInt32(c.Element("Position").Value) ascending
                             select c;

                foreach (var item in parent)
                {
                    HtmlGenericControl li = new HtmlGenericControl("li");
                    int iAtPosition = 0;

                    //Add the group to the Menu.
                    if (AddParentToMenu(ref li, string.IsNullOrEmpty(item.Element("NewTitle").Value) ? item.Element("SiteTitle").Value : item.Element("NewTitle").Value, item.Element("SiteId").Value, item.Element("Position").Value, item.Element("SiteDescription").Value, item.Element("SiteUrl").Value))
                    {
                        SetTopGroupsOrder(li, ref iAtPosition);

                        //Add the new li to Ul
                        if (iAtPosition > GlobalMenu.Controls.Count - 1)
                            GlobalMenu.Controls.Add(li);
                        else
                            GlobalMenu.Controls.AddAt(iAtPosition, li);
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
        /// <param name="li"></param>
        /// <param name="strGroupName"></param>
        /// <param name="strGroupId"></param>
        /// <returns></returns>
        private bool AddGroupToMenu(ref HtmlGenericControl li, string strGroupName, string strGroupId)
        {
            HtmlAnchor htmlGroupAnchor = new HtmlAnchor();
            bool bHasChildren = false;
            try
            {
                //Get the Parent Group Id
                String strGroupParentId = getGroupInfobyGroupId(strGroupId)["ParentID"];
                if (!String.IsNullOrEmpty(strGroupParentId))
                {
                    htmlGroupAnchor.InnerText = getGroupInfobyGroupId(strGroupParentId)["Title"];
                    htmlGroupAnchor.Title = getGroupInfobyGroupId(strGroupParentId)["Title"];
                    htmlGroupAnchor.Attributes.Add("class", "drop");
                    li.Controls.Add(htmlGroupAnchor);
                    li.ID = getGroupInfobyGroupId(strGroupParentId)["Title"] + "_" + getGroupInfobyGroupId(strGroupParentId)["Position"];
                }
                else
                {
                    HtmlAnchor htmlAnchor = new HtmlAnchor();
                    htmlAnchor.InnerText = strGroupName;
                    htmlAnchor.Title = strGroupName;
                    htmlAnchor.Attributes.Add("class", "drop");
                    li.Controls.Add(htmlAnchor);
                    li.ID = getGroupInfobyGroupId(strGroupId)["Title"] + "_" + getGroupInfobyGroupId(strGroupId)["Position"];
                }
                if (!CreateMenu(ref li, strGroupId, null))
                {
                    bHasChildren = false;
                }
                else
                {
                    bHasChildren = true;
                }
                return bHasChildren;
            }
            catch (Exception ex)
            {
                Helper.writeLog(ex);
                return false;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="li"></param>
        /// <param name="strParentName"></param>
        /// <param name="strParentId"></param>
        /// <param name="strParentPosition"></param>
        /// <returns></returns>
        private bool AddParentToMenu(ref HtmlGenericControl li, string strParentName, string strParentId, string strParentPosition, string strParentDescription, string strParentUrl)
        {
            HtmlAnchor htmlParentAnchor = new HtmlAnchor();
            bool bHasChildren = false;
            try
            {
                htmlParentAnchor = CreateAnchor(strParentUrl, strParentName, strParentName, "drop");
                //htmlParentAnchor.InnerText = strParentName;
                //htmlParentAnchor.Title = strParentName;
                //htmlParentAnchor.Attributes.Add("class", "drop");
                //htmlParentAnchor.HRef = "";
                li.Controls.Add(htmlParentAnchor);
                li.ID = strParentName + "_" + strParentPosition;

                if (!CreateMenu(ref li, null, strParentId))
                {
                    bHasChildren = false;
                }
                else
                {
                    bHasChildren = true;
                }
                return bHasChildren;
            }
            catch (Exception ex)
            {
                Helper.writeLog(ex);
                return false;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="strdChildSettings"></param>
        private void CreateMenuItem(StringDictionary strdChildSettings)
        {
            try
            {
                string userLoginName = SPContext.Current.Web.CurrentUser.LoginName;
                if (Helper.IsUserHasAccess(strdChildSettings["Url"].ToString(), userLoginName))
                {
                    HtmlGenericControl htmlli = new HtmlGenericControl("li");
                    HtmlAnchor htmlAnchor = new HtmlAnchor();

                    htmlAnchor = CreateAnchor(strdChildSettings["Url"].ToString(), strdChildSettings["Title"].ToString(), strdChildSettings["description"].ToString(), string.Empty);

                    htmlli.ID = strdChildSettings["Title"].ToString() + "_" + strdChildSettings["position"].ToString();

                    htmlli.Controls.AddAt(0, htmlAnchor);

                    int iAtPosition = 0;

                    SetTopGroupsOrder(htmlli, ref iAtPosition);

                    //Add new Item to the Menu group
                    if (iAtPosition > GlobalMenu.Controls.Count - 1)
                        GlobalMenu.Controls.Add(htmlli);
                    else
                        GlobalMenu.Controls.AddAt(iAtPosition, htmlli);
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
        /// <param name="li"></param>
        /// <param name="strGroupId"></param>
        /// <returns></returns>
        private bool CreateMenu(ref HtmlGenericControl li, string strGroupId, string strParentId)
        {
            Int64 iChildrenCount = 0;
            bool bHasChildren = false;
            int iGlobalNavItemsCount = 0;

            try
            {
                string userLoginName = SPContext.Current.Web.CurrentUser.LoginName;

                HtmlGenericControl htmlFirstDiv = new HtmlGenericControl("DIV");

                SPSecurity.RunWithElevatedPrivileges(
                    delegate()
                    {
                        XDocument xDoc = XMLHelper.GetXDocument(XMLHelper.XMLType.XMLGLOBALNAV);

                        iGlobalNavItemsCount = xDoc.DescendantNodes().ToList().Count;

                        if (iGlobalNavItemsCount > 1)
                        {
                            HtmlGenericControl htmlSecondDiv = new HtmlGenericControl("DIV");
                            htmlSecondDiv.Attributes.Add("class", "row");

                            bool bLeftDivWasCreated = false;
                            bool bMiddleDivWasCreated = false;
                            bool bRightDivWasCreated = false;

                            String strGroupParentId = getGroupInfobyGroupId(strGroupId)["ParentID"];
                            if (!String.IsNullOrEmpty(strGroupParentId))
                            {
                                XDocument xGroups = XMLHelper.GetXDocument(XMLHelper.XMLType.XMLGROUPNAMES);
                                if (xGroups.DescendantNodes().ToList().Count > 1)
                                {
                                    var qSubgroups = from c in xGroups.Elements("GroupNames").Elements("Group")
                                                     where (c.Element("ParentId").Value.ToString() == strGroupParentId)
                                                     orderby Convert.ToInt32(c.Element("Position").Value) == 0 ? 999 : Convert.ToInt32(c.Element("Position").Value) ascending
                                                     select c;

                                    AddSubGroupToMenu(ref iChildrenCount, userLoginName, xDoc, htmlSecondDiv, ref bLeftDivWasCreated, ref bMiddleDivWasCreated, ref bRightDivWasCreated, qSubgroups);
                                }
                            }
                            else
                            {
                                if (strGroupId != null)
                                {
                                    var q = from c in xDoc.Elements("GlobalNav").Elements("Item")
                                            where (string.IsNullOrEmpty(c.Element("ParentId").Value.ToString()) || (XMLHelper.ParentExist(c.Element("ParentId").Value.ToString()))) &&
                                                  (bool)c.Element("GlobalNav") && (c.Element("GroupId").Value.Trim().Length > 0 ? c.Element("GroupId").Value : "0") == (strGroupId)
                                            orderby Convert.ToInt32(c.Element("Position").Value) == 0 ? 999 : Convert.ToInt32(c.Element("Position").Value) ascending
                                            select c;

                                    if (q.Count() > 0)
                                    {
                                        HtmlGenericControl htmlLeftThirdDiv = new HtmlGenericControl("DIV");
                                        htmlLeftThirdDiv.Attributes.Add("class", "col_left");

                                        HtmlGenericControl htmlul = new HtmlGenericControl("ul");
                                        CreateNewOptionsToMenu(ref iChildrenCount, userLoginName, q, ref htmlul, null);

                                        htmlLeftThirdDiv.Controls.Add(htmlul);
                                        htmlSecondDiv.Controls.Add(htmlLeftThirdDiv);
                                    }
                                }
                                else if (strParentId != null)
                                {
                                    var q = from c in xDoc.Elements("GlobalNav").Elements("Item")
                                            where //(string.IsNullOrEmpty(c.Element("ParentId").Value.ToString()) || (XMLHelper.ParentExist(c.Element("ParentId").Value.ToString()) == 0)) &&
                                                  (bool)c.Element("GlobalNav") && (c.Element("ParentId").Value.Trim().Length > 0 ? c.Element("ParentId").Value : "0") == (strParentId)
                                            orderby Convert.ToInt32(c.Element("Position").Value) == 0 ? 999 : Convert.ToInt32(c.Element("Position").Value) ascending
                                            select c;

                                    if (q.Count() > 0)
                                    {
                                        HtmlGenericControl htmlLeftThirdDiv = new HtmlGenericControl("DIV");
                                        htmlLeftThirdDiv.Attributes.Add("class", "col_left");

                                        HtmlGenericControl htmlul = new HtmlGenericControl("ul");
                                        CreateNewOptionsToMenu(ref iChildrenCount, userLoginName, q, ref htmlul, null);

                                        htmlLeftThirdDiv.Controls.Add(htmlul);
                                        htmlSecondDiv.Controls.Add(htmlLeftThirdDiv);
                                    }
                                }
                            }
                            //Add Item from the Settings/Navigation options.

                            htmlFirstDiv.Controls.Add(htmlSecondDiv);

                            if (bMiddleDivWasCreated && bRightDivWasCreated)
                                htmlFirstDiv.Attributes.Add("class", "dropdown_3columns");
                            else if (bMiddleDivWasCreated)
                                htmlFirstDiv.Attributes.Add("class", "dropdown_2columns");
                            else
                                htmlFirstDiv.Attributes.Add("class", "dropdown_column");

                            //Verifies that Menu Group has sub options.
                            bHasChildren = iChildrenCount > 0;
                        }
                    });

                //Add all suboption to the Menu group.
                li.Controls.Add(htmlFirstDiv);
                return bHasChildren;
            }
            catch (Exception ex)
            {
                Helper.writeLog(ex);
                return bHasChildren;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="iChildrenCount"></param>
        /// <param name="userLoginName"></param>
        /// <param name="q"></param>
        /// <param name="htmlul"></param>
        /// <param name="subgroup"></param>
        private void CreateNewOptionsToMenu(ref Int64 iChildrenCount, string userLoginName, System.Collections.Generic.IEnumerable<XElement> q, ref HtmlGenericControl htmlul, XElement subgroup)
        {
            try
            {
                if (subgroup != null)
                {
                    HtmlGenericControl htmlh2SubGroup = new HtmlGenericControl("h2");
                    HtmlGenericControl htmlliSubGroup = new HtmlGenericControl("li");
                    htmlliSubGroup.Attributes.Add("class", "heading");

                    htmlh2SubGroup.Controls.Add(CreateAnchor("", subgroup.Element("Name").Value, "", "parent"));

                    htmlliSubGroup.Controls.Add(htmlh2SubGroup);

                    htmlul.Attributes.Add("class", "simple");
                    htmlul.Controls.Add(htmlliSubGroup);
                }
                foreach (var item in q)
                {
                    if (Helper.IsUserHasAccess(item.Element("SiteUrl").Value, userLoginName))
                    {
                        HtmlGenericControl htmlli = new HtmlGenericControl("li");
                        htmlli.Attributes.Add("class", "heading");
                        HtmlAnchor htmlAnchor = new HtmlAnchor();

                        StringDictionary strdChildSettings = XMLHelper.getSettings(item.Element("SiteId").Value, Helper.FindBy.BySiteId);

                        htmlAnchor = CreateAnchor(strdChildSettings["Url"].ToString(), strdChildSettings["Title"].ToString(),
                            strdChildSettings["description"].ToString(), string.Empty);

                        /*Verifies if the Sub option has Children*/
                        if (HasChildren(ref htmlli, item.Element("SiteId").Value, ref iChildrenCount))
                        {
                            HtmlGenericControl htmlh2 = new HtmlGenericControl("h2");
                            htmlAnchor.Attributes.Add("class", "parent");
                            htmlAnchor.InnerText = htmlAnchor.InnerText.ToUpper();
                            htmlh2.Controls.Add(htmlAnchor);
                            htmlli.Controls.AddAt(0, htmlh2);
                        }
                        else
                        {
                            htmlli.Controls.AddAt(0, htmlAnchor);
                        }

                        ////Children Count
                        ++iChildrenCount;
                        htmlul.Controls.Add(htmlli);
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
        /// <param name="iChildrenCount"></param>
        /// <param name="userLoginName"></param>
        /// <param name="xDoc"></param>
        /// <param name="htmlSecondDiv"></param>
        /// <param name="bLeftDivWasCreated"></param>
        /// <param name="bMiddleDivWasCreated"></param>
        /// <param name="bRightDivWasCreated"></param>
        /// <param name="qSubgroups"></param>
        private void AddSubGroupToMenu(ref Int64 iChildrenCount, string userLoginName, XDocument xDoc,
                                            HtmlGenericControl htmlSecondDiv, ref bool bLeftDivWasCreated, ref bool bMiddleDivWasCreated,
                                            ref bool bRightDivWasCreated, IOrderedEnumerable<XElement> qSubgroups)
        {
            try
            {
                HtmlGenericControl htmlLeftThirdDiv = new HtmlGenericControl("DIV");
                htmlLeftThirdDiv.Attributes.Add("class", "col_left");

                HtmlGenericControl htmlMiddleThirdDiv = new HtmlGenericControl("DIV");
                htmlMiddleThirdDiv.Attributes.Add("class", "col_middle");

                HtmlGenericControl htmlRightThirdDiv = new HtmlGenericControl("DIV");
                htmlRightThirdDiv.Attributes.Add("class", "col_right");

                HtmlGenericControl htmlLeftul = new HtmlGenericControl("ul");
                HtmlGenericControl htmlMiddleul = new HtmlGenericControl("ul");
                HtmlGenericControl htmlRightul = new HtmlGenericControl("ul");

                foreach (var subgroup in qSubgroups)
                {
                    var qSubgroupItems = from c in xDoc.Elements("GlobalNav").Elements("Item")
                                         where (string.IsNullOrEmpty(c.Element("ParentId").Value.ToString()) || (XMLHelper.ParentExist(c.Element("ParentId").Value.ToString()))) &&
                                               (bool)c.Element("GlobalNav") && (c.Element("GroupId").Value.Trim().Length > 0 ? c.Element("GroupId").Value : "0") == (subgroup.Element("Id").Value)
                                         orderby Convert.ToInt32(c.Element("Position").Value) == 0 ? 999 : Convert.ToInt32(c.Element("Position").Value) ascending
                                         select c;

                    if (qSubgroupItems.Count() > 0)
                    {
                        ++iChildrenCount;

                        if ((iChildrenCount + qSubgroupItems.Count()) >= 1 && (iChildrenCount + qSubgroupItems.Count()) <= 7)
                        {
                            if (!bLeftDivWasCreated)
                                bLeftDivWasCreated = true;

                            CreateNewOptionsToMenu(ref iChildrenCount, userLoginName, qSubgroupItems, ref htmlLeftul, subgroup);
                        }
                        else if ((iChildrenCount + qSubgroupItems.Count()) >= 8 && (iChildrenCount + qSubgroupItems.Count()) <= 14)
                        {
                            if (!bMiddleDivWasCreated)
                                bMiddleDivWasCreated = true;

                            CreateNewOptionsToMenu(ref iChildrenCount, userLoginName, qSubgroupItems, ref htmlMiddleul, subgroup);
                        }
                        else
                        {
                            if (!bRightDivWasCreated)
                                bRightDivWasCreated = true;

                            CreateNewOptionsToMenu(ref iChildrenCount, userLoginName, qSubgroupItems, ref htmlRightul, subgroup);
                        }
                    }
                }
                if (bLeftDivWasCreated && !bMiddleDivWasCreated)
                {
                    htmlLeftThirdDiv.Controls.Add(htmlLeftul);
                    htmlSecondDiv.Controls.Add(htmlLeftThirdDiv);
                }
                else if (bMiddleDivWasCreated && !bRightDivWasCreated)
                {
                    htmlLeftThirdDiv.Controls.Add(htmlLeftul);
                    htmlSecondDiv.Controls.Add(htmlLeftThirdDiv);

                    htmlMiddleThirdDiv.Controls.Add(htmlMiddleul);
                    htmlSecondDiv.Controls.Add(htmlMiddleThirdDiv);
                }
                else
                {
                    htmlLeftThirdDiv.Controls.Add(htmlLeftul);
                    htmlSecondDiv.Controls.Add(htmlLeftThirdDiv);

                    htmlMiddleThirdDiv.Controls.Add(htmlMiddleul);
                    htmlSecondDiv.Controls.Add(htmlMiddleThirdDiv);

                    htmlRightThirdDiv.Controls.Add(htmlRightul);
                    htmlSecondDiv.Controls.Add(htmlRightThirdDiv);
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
        /// <param name="strGroup"></param>
        /// <returns></returns>
        private StringDictionary getGroupInfobyGroupId(string strGroup)
        {
            StringDictionary strGroupInfo = new StringDictionary();
            try
            {
                SPSecurity.RunWithElevatedPrivileges(
                    delegate()
                    {
                        XDocument xDoc = XMLHelper.GetXDocument(XMLHelper.XMLType.XMLGROUPNAMES);

                        if (xDoc.DescendantNodes().ToList().Count > 1)
                        {
                            var qSubgroups = from c in xDoc.Elements("GroupNames").Elements("Group")
                                             where (c.Element("Id").Value.ToString() == strGroup)
                                             select c;

                            foreach (var subgroup in qSubgroups)
                            {
                                strGroupInfo.Add("Id", subgroup.Element("Id").Value);
                                strGroupInfo.Add("Title", subgroup.Element("Name").Value);
                                strGroupInfo.Add("Description", subgroup.Element("Description").Value);
                                strGroupInfo.Add("Position", subgroup.Element("Position").Value);
                                strGroupInfo.Add("ParentId", subgroup.Element("ParentId").Value);
                            }
                        }
                    });
                return strGroupInfo;
            }
            catch (Exception ex)
            {
                Helper.writeLog(ex);
                return strGroupInfo;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="Parentli"></param>
        /// <param name="strParentSiteId"></param>
        /// <param name="iChildrenCount"></param>
        /// <returns></returns>
        private bool HasChildren(ref HtmlGenericControl Parentli, string strParentSiteId, ref Int64 iChildrenCount)
        {
            bool bHasChildren = false;
            string userLoginName = SPContext.Current.Web.CurrentUser.LoginName;
            try
            {
                XDocument xDoc = XMLHelper.GetXDocument(XMLHelper.XMLType.XMLGLOBALNAV);

                var Children = from c in xDoc.Elements("GlobalNav").Elements("Item")
                               where c.Element("ParentId").Value == strParentSiteId && (bool)c.Element("GlobalNav")
                               orderby Convert.ToInt32(c.Element("Position").Value) == 0 ? 999 : Convert.ToInt32(c.Element("Position").Value) ascending
                               select c;

                if (Children != null && Children.Count() > 0)
                {
                    HtmlGenericControl htmlul = new HtmlGenericControl("ul");

                    foreach (var Child in Children)
                    {
                        if (Helper.IsUserHasAccess(Child.Element("SiteUrl").Value, userLoginName))
                        {
                            StringDictionary strdChildSettings = XMLHelper.getSettings(Child.Element("SiteId").Value, Helper.FindBy.BySiteId);

                            HtmlGenericControl htmlli = new HtmlGenericControl("li");
                            HtmlAnchor htmlAnchor = new HtmlAnchor();

                            htmlAnchor = CreateAnchor(strdChildSettings["Url"].ToString(), strdChildSettings["Title"].ToString(), strdChildSettings["description"].ToString(), "child");

                            htmlli.Controls.Add(htmlAnchor);

                            //Add new Item to the Menu group
                            htmlul.Controls.Add(htmlli);

                            //Children Count
                            ++iChildrenCount;
                        }
                    }
                    if (iChildrenCount > 0)
                    {
                        Parentli.Controls.Add(htmlul);
                        bHasChildren = true;
                    }
                    else
                        bHasChildren = false;
                }
                return bHasChildren;
            }
            catch (Exception ex)
            {
                Helper.writeLog(ex);
                return bHasChildren;
            }
        }

        #endregion XML Files

        #region Common

        /// <summary>
        ///
        /// </summary>
        /// <param name="strUrl"></param>
        /// <param name="strTitle"></param>
        /// <param name="strTooltip"></param>
        /// <param name="strClass"></param>
        /// <returns></returns>
        private HtmlAnchor CreateAnchor(string strUrl, string strTitle, string strTooltip, string strClass)
        {
            HtmlAnchor htmlanchor = new HtmlAnchor();
            try
            {
                if (Convert.ToBoolean(AddUrl))
                {
                    htmlanchor.HRef = strUrl;
                }
                else
                {
                    htmlanchor.HRef = string.Empty;
                }

                if (String.IsNullOrEmpty(strUrl))
                {
                    htmlanchor.HRef = string.Empty;
                }

                htmlanchor.Title = strTooltip;
                htmlanchor.InnerText = strTitle;
                if (!String.IsNullOrEmpty(strClass))
                    htmlanchor.Attributes.Add("class", strClass);
                return htmlanchor;
            }
            catch (Exception ex)
            {
                Helper.writeLog(ex);
                return htmlanchor;
            }
        }

        #endregion Common
    }
}