using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Navigation;

namespace SP.GlobalTopMenu
{
    public partial class ucBreadcrumb : UserControl
    {
        private string siteMapProvider;
        private string nodeSeparator = "&gt;";

        ///// <summary>
        ///// Variables.
        ///// </summary>
        private string subtle = string.Empty;

        private string subtle2 = string.Empty;
        private StringBuilder sb = null;
        private string similarLink1 = string.Empty;
        private string similarLink2 = string.Empty;
        private string weburl = SPContext.Current.Web.Url + "/";

        #region Properties

        /// <summary>
        /// The name of the site map provider to use for this breadcrumb. This provider should be one of the
        /// named site map providers in the web.config.
        /// </summary>
        public string SiteMapProvider
        {
            get { return siteMapProvider; }
            set { siteMapProvider = value; }
        }

        /// <summary>
        /// The text to use to separate the nodes if the node doesn't have subnodes.
        /// </summary>
        public string NodeSeparator
        {
            get { return nodeSeparator; }
            set { nodeSeparator = value; }
        }

        #endregion Properties

        #region Events

        public void Page_Load(object sender, EventArgs e)
        {
            BreadcrumbFormer();
            //QuickLaunch();

            //SPSecurity.RunWithElevatedPrivileges(
            //    delegate()
            //    {
            //var provider1 = SiteMap.Providers["CurrentNavSiteMapProviderNoEncode"] as PortalSiteMapProvider;

            //if (provider1 != null)
            //{
            //    provider1.IncludePages = PortalSiteMapProvider.IncludeOption.Always;
            //}

            //});

            //SiteMapProvider provider = GetSiteMapProvider();

            //Stack<SiteMapNode> nodes = new Stack<SiteMapNode>();

            //SiteMapNode current = provider.CurrentNode;
            //while (current != null)
            //{
            //    nodes.Push(current);
            //    current = current.ParentNode;
            //}

            //while (nodes.Count > 0)
            //{
            //    HtmlGenericControl htmlli = new HtmlGenericControl("li");
            //    HtmlAnchor htmla = new HtmlAnchor();

            //    SiteMapNode node = nodes.Pop();

            //    htmla.HRef = node.Url;
            //    htmla.Title = node.Title;
            //    htmla.InnerHtml = node.Title;

            //    htmlli.Controls.Add(htmla);

            //    //why not use SiteMapNode.HasChildNodes? see: http://social.msdn.microsoft.com/Forums/en-US/sharepointdevelopment/thread/37d10f92-140f-4ce8-b71c-388163721737/
            //    //if (node.ChildNodes.Count > 0)
            //    //{
            //    //    HtmlImage htmlI = new HtmlImage();
            //    //    htmlI.Src = "/_layouts/images/marr.gif";
            //    //    htmlI.Attributes.Add("class", "dp-breadcrumbitemimage");

            //    //    HtmlGenericControl htmlul2 = new HtmlGenericControl("ul");
            //    //    htmlul2.Attributes.Add("style", "display: none;");

            //    //    foreach (SiteMapNode subNode in node.ChildNodes)
            //    //    {
            //    //        HtmlGenericControl htmlli2 = new HtmlGenericControl("li");

            //    //        HtmlAnchor htmla2 = new HtmlAnchor();
            //    //        htmla2.HRef = subNode.Url;
            //    //        htmla2.Title = subNode.Title;
            //    //        htmla2.InnerHtml = subNode.Title;

            //    //       htmlli2.Controls.Add(htmla2);

            //    //        htmlul2.Controls.Add(htmlli2);
            //    //    }
            //    //    htmlli.Controls.Add(htmlul2);
            //    //}

            //    breadcrumbs.Controls.Add(htmlli);
            //}
        }

        #endregion Events

        #region Methods

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        private SiteMapProvider GetSiteMapProvider()
        {
            SiteMapProvider provider = null;

            if (string.IsNullOrEmpty(this.SiteMapProvider))
            {
                if (SiteMap.Provider == null) throw new Exception("No provider specified for application.");
                else provider = SiteMap.Provider;
            }
            else
            {
                provider = SiteMap.Providers[this.SiteMapProvider];

                if (provider == null) throw new Exception(string.Format("SiteMapProvider '{0}' does not exist.", SiteMapProvider));
            }

            return provider;
        }

        private void AddNewTagToBreadcrumbs(string strTitle, string strUrl)
        {
            HtmlGenericControl htmlli = new HtmlGenericControl("li");
            HtmlAnchor htmla = new HtmlAnchor();

            htmla.HRef = strUrl;
            htmla.Title = strTitle;
            htmla.InnerHtml = strTitle;
            htmlli.Controls.Add(htmla);
            if (!String.IsNullOrEmpty(strUrl.Trim()))
            {
                AddContextMenuToTag(strTitle, strUrl, ref htmlli);
            }
            else
            {
                htmlli.Attributes.Add("class", "current");
            }

            breadcrumbs.Controls.Add(htmlli);
        }

        public static void QuickLaunch(string strUrlTag, ref  HtmlGenericControl htmlli)
        {
            using (SPSite site = new SPSite(strUrlTag))
            {
                using (SPWeb web = site.OpenWeb())
                {
                    SPNavigationNodeCollection quickLaunchNodes = web.Navigation.QuickLaunch;
                    HtmlImage htmlI = new HtmlImage();
                    htmlI.Src = "/_layouts/images/marr.gif";
                    htmlI.Attributes.Add("class", "dp-breadcrumbitemimage");

                    HtmlGenericControl htmlul2 = new HtmlGenericControl("ul");
                    htmlul2.Attributes.Add("style", "display: none;");

                    //Parent
                    foreach (SPNavigationNode node in quickLaunchNodes)
                    {
                        HtmlGenericControl htmlli2 = new HtmlGenericControl("li");

                        HtmlAnchor htmla2 = new HtmlAnchor();
                        htmla2.HRef = node.Url.ToString();
                        htmla2.Title = node.Title;
                        htmla2.InnerHtml = node.Title;

                        htmlli2.Controls.Add(htmla2);

                        htmlul2.Controls.Add(htmlli2);
                    }
                    htmlli.Controls.Add(htmlul2);
                }
            }
        }

        private void AddContextMenuToTag(string strNameTag, string strUrl, ref  HtmlGenericControl htmlli)
        {
            SPList spList = SPContext.Current.Web.Lists.TryGetList(strNameTag);
            if (spList != null)
            {
                if (spList.Views.Count > 0)
                {
                    HtmlImage htmlI = new HtmlImage();
                    htmlI.Src = "/_layouts/images/marr.gif";
                    htmlI.Attributes.Add("class", "dp-breadcrumbitemimage");

                    HtmlGenericControl htmlul2 = new HtmlGenericControl("ul");
                    htmlul2.Attributes.Add("style", "display: none;");

                    foreach (SPView view in spList.Views)
                    {
                        if (!view.Hidden)
                        {
                            HtmlGenericControl htmlli2 = new HtmlGenericControl("li");

                            HtmlAnchor htmla2 = new HtmlAnchor();
                            htmla2.HRef = SPContext.Current.Web.Url + "/" + view.Url;
                            htmla2.Title = view.Title;
                            htmla2.InnerHtml = view.Title;

                            htmlli2.Controls.Add(htmla2);

                            htmlul2.Controls.Add(htmlli2);
                        }
                    }
                    htmlli.Controls.Add(htmlul2);
                }

            }
            else
            {
                if (Helper.isSiteExists(strUrl))
                    QuickLaunch(strUrl, ref htmlli);
            }
        }

        /// <summary>
        /// The function to form a Breadcrumb from a url.
        /// </summary>
        /// <returns></returns>
        public void BreadcrumbFormer()
        {
            string inputString = HttpContext.Current.Request.Url.ToString();
            //if(SPContext.Current.File != null)
            //    inputString =  SPContext.Current.Web.Url + "/" +SPContext.Current.File.Url;
            //else
            //    inputString = SPContext.Current.Web.Url + "/" + SPContext.Current.RootFolderUrl;

            string[] parts = Regex.Split(inputString, @"/");

            //sb = new StringBuilder();
            string strTagUrl = "";
            int iCurrentIndex = 0;
            foreach (string strTag in parts)
            {
                strTagUrl += strTag + "/";

                iCurrentIndex = Array.IndexOf(parts, strTag);

                if (!strTag.ToUpper().Contains("HTTP") && !String.IsNullOrEmpty(strTag.Trim()))
                {
                    if (strTag.Contains(".aspx"))
                    {
                        if (!strTag.Contains("?"))
                            AddNewTagToBreadcrumbs(strTag.Replace(".aspx", "").Replace("-", " "), "");
                        else
                            AddNewTagToBreadcrumbs(strTag.Replace(".aspx", "").Split('?')[0], "");
                    }
                    else
                    {
                        if (Helper.isSiteExists(strTagUrl)  || SPContext.Current.Web.Lists.TryGetList(strTag) != null)
                        {
                            if (strTagUrl.ToUpper() == SPContext.Current.Site.WebApplication.Sites[0].Url.ToUpper() + "/")
                                AddNewTagToBreadcrumbs("Home", strTagUrl);
                            else
                                AddNewTagToBreadcrumbs(strTag.Replace("-", " "), strTagUrl);
                        }
                    }
                }
            }
        }

        #endregion Methods
    }
}