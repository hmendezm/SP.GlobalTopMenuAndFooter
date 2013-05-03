using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using Microsoft.SharePoint;
using System.Xml.Linq;
using System.Linq;

namespace NIAMS.GlobalNavigationBar
{
    public partial class ucGlobalFooter : UserControl
    {

        const string XMLGLOBALNAVPATH = @"/_layouts/NIAMS.GlobalNavigationBar/Data/GlobalNav.xml";
        const string XMLGROUPNAMESPATH = @"/_layouts/NIAMS.GlobalNavigationBar/Data/GroupNames.xml";

        public string AddUrl
        {
            get
            {
                return ViewState["AddUrl"].ToString();
            }
            set
            {
                ViewState["AddUrl"] = value;
            }

        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CreateFooter();
            }
        }

        
        /// <summary>
        /// Get the information of the sites and create the footer. All the sites that has NIAMSFooter==1 will be added to the footer.
        /// </summary>
        private void CreateFooter()
        {

            try
            {
                int iGlobalNavItemsCount = 0;
                string userLoginName = SPContext.Current.Web.CurrentUser.LoginName;
                int sitesCounter = 1;
                int sitesTotal = 0;
                string strMaxPosition = string.Empty;
                
               
                

                SPSecurity.RunWithElevatedPrivileges(
                    delegate()
                    {
                        //string strPath = @"/_layouts/NIAMS.GlobalNavAdmnSettings/GlobalNav.xml";
                        XDocument xDoc = XDocument.Load(MapPath(XMLGLOBALNAVPATH));
                         iGlobalNavItemsCount = xDoc.DescendantNodes().ToList().Count;

                         if (iGlobalNavItemsCount > 1)
                         {
                             strMaxPosition = ((from c in xDoc.Elements("GlobalNav").Elements("Item")
                                                select
                                                (
                                                            int.Parse(c.Element("Position").Value.Trim().Length > 0 ? c.Element("Position").Value : "0")
                                                )).Max() + 1).ToString();



                             var q = from c in xDoc.Elements("GlobalNav").Elements("Item")
                                     where //(string.IsNullOrEmpty(c.Element("ParentId").Value.ToString())) 
                                         //|| 
                                         //(clsCommonBL.ParentExist(c.Element("ParentId").Value.ToString()) == 0)
                                         //&& 
                                     (bool)c.Element("Footer")
                                     orderby Convert.ToInt32(c.Element("Position").Value.Trim().Length == 0 ? strMaxPosition : c.Element("Position").Value) ascending
                                     select c;


                             if (q.Count() > 0)
                             {
                                 sitesTotal = q.Count();
                                 string strServerRelativeUrl = string.Empty;

                                 foreach (var item in q)
                                 {
                                     

                                     if (clsCommonBL.IsUserHasAccess(item.Element("SiteUrl").Value, userLoginName))
                                     {
                                         strServerRelativeUrl = item.Element("SiteUrl").Value;
                                     }
                                     else
                                     {
                                         strServerRelativeUrl = string.Empty;
                                     }


                                     if (strServerRelativeUrl.Trim().Length > 0)
                                     {
                                         HtmlGenericControl li = new HtmlGenericControl("li");
                                         HtmlAnchor htmlanchor = new HtmlAnchor();
                                         if (Convert.ToBoolean(AddUrl))
                                             htmlanchor.HRef = item.Element("SiteUrl").Value;
                                         htmlanchor.Title = item.Element("SiteDescription").Value != null ? item.Element("SiteDescription").Value : string.Empty; //tooltip        
                                         htmlanchor.InnerText = item.Element("SiteTitle").Value;

                                         if (sitesCounter == sitesTotal)
                                             li.Attributes.Add("class", "last");

                                         if (li != null)
                                         {
                                             li.Controls.Add(htmlanchor);
                                             firstrow.Controls.Add(li);
                                         }
                                     }

                                     sitesCounter++;
                                 }
                             }
                         }
                    });
            }
            catch (Exception ex)
            {
                //NIAMSUtilities.WriteLog(ex, 0);
                throw;

            }
        }

    }
}
