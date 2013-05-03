using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Publishing.Navigation;

namespace SP.GlobalTopMenu
{
    public partial class ucBreadcrumb : UserControl
    {
        private string siteMapProvider;
        private string nodeSeparator = "&gt;";

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
            SPSecurity.RunWithElevatedPrivileges(
                delegate()
                {
                    var provider1 = SiteMap.Providers["CurrentNavSiteMapProviderNoEncode"] as PortalSiteMapProvider;
                    //if (Page is UnsecuredLayoutsPageBase)
                    //{
                    //    ContentMap.SiteMapProvider = "SPXmlContentMapProvider";
                    //}
                    //else if (Page is PublishingLayoutPage)
                    //{
                    if (provider1 != null)
                    {
                        provider1.IncludePages = PortalSiteMapProvider.IncludeOption.Always;
                    }
                    //}

                    //using (SPSite site = new SPSite("http://localhost"))
                    //{
                    //    using (SPWeb web = site.OpenWeb())
                    //    {
                    //        SPNavigationNode toplinkbar = web.Navigation.GetNodeById(1002);
                    //        if (toplinkbar != null)
                    //        {
                    //            foreach (SPNavigationNode node in toplinkbar.Children)
                    //            {
                    //                Console.Write("| {0} ", node.Title);
                    //            }
                    //            Console.WriteLine("|");
                    //        }
                    //    }
                    //}
                });

            //HtmlGenericControl htmlul = new HtmlGenericControl("ul");

            //StringBuilder sb = new StringBuilder();

            //sb.AppendLine("<ul class='dp-breadcrumb'>");

            //htmlul.Attributes.Add("class", "xbreadcrumbs");

            SiteMapProvider provider = GetSiteMapProvider();

            Stack<SiteMapNode> nodes = new Stack<SiteMapNode>();

            SiteMapNode current = provider.CurrentNode;
            while (current != null)
            {
                nodes.Push(current);
                current = current.ParentNode;
            }

            while (nodes.Count > 0)
            {
                HtmlGenericControl htmlli = new HtmlGenericControl("li");
                HtmlAnchor htmla = new HtmlAnchor();

                SiteMapNode node = nodes.Pop();

                //htmlli.Attributes.Add("class", "dp-breadcrumbitem");
                htmla.HRef = node.Url;
                htmla.Title = node.Title;
                htmla.InnerHtml = node.Title;

                htmlli.Controls.Add(htmla);

                //sb.AppendFormat("<li class='dp-breadcrumbitem'><a href='{0}' title='{1}'>{1}</a>", node.Url, node.Title);

                //why not use SiteMapNode.HasChildNodes? see: http://social.msdn.microsoft.com/Forums/en-US/sharepointdevelopment/thread/37d10f92-140f-4ce8-b71c-388163721737/
                if (node.ChildNodes.Count > 0)
                {
                    //sb.Append("<img src='/_layouts/images/marr.gif' class='dp-breadcrumbitemimage'/>");
                    HtmlImage htmlI = new HtmlImage();
                    htmlI.Src = "/_layouts/images/marr.gif";
                    htmlI.Attributes.Add("class", "dp-breadcrumbitemimage");

                    //sb.AppendFormat("<ul id='dp-submenu-{0}' class='ms-topNavFlyOuts dp-breadcrumbsubmenu'>", node.Key);
                    HtmlGenericControl htmlul2 = new HtmlGenericControl("ul");
                    //htmlul2.ID = "dp-submenu-" + node.Key;
                    htmlul2.Attributes.Add("style", "display: none;");

                    foreach (SiteMapNode subNode in node.ChildNodes)
                    {
                        //sb.AppendFormat("<li class='dp-breadcrumbsubmenuitem'><a href='{0}' title='{1}' class='dp-submenulink'>{1}</a></li>", subNode.Url, subNode.Title);

                        HtmlGenericControl htmlli2 = new HtmlGenericControl("li");
                        //htmlli2.Attributes.Add("class", "dp-breadcrumbsubmenuitem");

                        HtmlAnchor htmla2 = new HtmlAnchor();
                        htmla2.HRef = subNode.Url;
                        htmla2.Title = subNode.Title;
                        htmla2.InnerHtml = subNode.Title;

                        //htmla2.Attributes.Add("class", "dp-submenulink");
                        htmlli2.Controls.Add(htmla2);

                        htmlul2.Controls.Add(htmlli2);
                    }
                    //sb.Append("</ul>");
                    htmlli.Controls.Add(htmlul2);
                }
                //else
                //{
                //    if (nodes.Count > 0)
                //    {
                //        sb.AppendFormat("<span class='dp-breadcrumbseperator'>{0}</span>", nodeSeparator);

                //        HtmlGenericControl htmlspan = new HtmlGenericControl("span");
                //        htmlspan.InnerHtml = nodeSeparator;
                //        htmlspan.Attributes["class"] = "dp-breadcrumbseperator";
                //        htmlli.Controls.Add(htmlspan);
                //    }
                //}
                //sb.Append("</li>");

                breadcrumbs.Controls.Add(htmlli);
            }

            //sb.AppendLine("</ul>");

            // this.Controls.Add(htmlul);
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

        #endregion Methods
    }
}