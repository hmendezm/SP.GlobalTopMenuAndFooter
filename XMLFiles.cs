using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Xml.Linq;
using Microsoft.SharePoint;
using System.Configuration;
using System.IO;
using System.Xml;
using Microsoft.SharePoint.Utilities;
using System.Collections.Specialized;

namespace SP.GlobalTopMenu
{

    public static class XMLFiles
    {
        public  enum  XMLType
        {
            XMLGLOBALNAV,
            XMLGROUPNAMES
        }
        public const string EMPTYXMLGLOBALNAV = "<GlobalNav></GlobalNav>";
        public const string EMPTYXMLGROUPNAMES = "<GroupNames></GroupNames>";

        public const string XML_LIBRARY = "SPGlobalTopMenu";

        public const string XML_FOLDER = "Data";

     

        public static string SiteRootUrl
        {
            get { return SPContext.Current.Web.Site.WebApplication.Sites[0].Url; }
        }
        

        /*SP.GlobalTopMenu*/
        /// <summary>
        /// Builds the xDocument of the specific color and Fiscal Year.
        /// </summary>
        /// <param name="strFiscalYearTitle">Fiscal Year title from the Fiscal Year Custom list.</param>
        /// <param name="strColorTitle">Color title from the Color custom list.</param>
        /// <returns></returns>
        public static XDocument GetXDocument(XMLType eXMLName)
        {
            try
            {
                using (SPSite oSite = new SPSite(SiteRootUrl))
                {
                    using (SPWeb oWeb = oSite.OpenWeb())
                    {
                        SPFolder oTargetFolder = oWeb.Folders[XML_LIBRARY];
                        SPFile spFile;

                        if (oWeb.GetFolder(oTargetFolder.Url + "/" + XML_FOLDER).Exists)
                        {
                            if (oWeb.GetFile(oTargetFolder.SubFolders[XML_FOLDER].Url + "/" + eXMLName + ".xml").Exists)
                            {
                                spFile = oTargetFolder.SubFolders[XML_FOLDER].Files[eXMLName + ".xml"];

                                StreamReader sr = new StreamReader(spFile.OpenBinaryStream());

                                return XDocument.Parse(sr.ReadToEnd());
                            }
                        }
                        return emptyXMLFile(eXMLName);
                    }
                }
            }
            catch (Exception ex)
            {
                return emptyXMLFile(eXMLName);
            }
        }

        /// <summary>
        /// Updates the XML of the specific color to the XMLs Document Library
        /// </summary>
        /// <param name="document">XML Document to upload to the XMLs Library.</param>
        /// <param name="replaceExistingFile">True: replace the exist document.</param>
        /// <param name="oMetadata">Metadata of the current document.</param>
        public static void UploadXDocumentToDocLib(XDocument document, bool replaceExistingFile, XMLType eXMLName)
        {
            try
            {
                SPFolder oTargetFolder = null;
                SPSecurity.RunWithElevatedPrivileges(delegate()
                {
                    using (SPSite oSite = new SPSite(SiteRootUrl))
                    {
                        using (SPWeb oWeb = oSite.OpenWeb())
                        {
                            oSite.AllowUnsafeUpdates = true;
                            oWeb.AllowUnsafeUpdates = true;

                            SPUtility.ValidateFormDigest();

                            var stream = new MemoryStream();
                            var writer = XmlWriter.Create(stream);
                            document.WriteTo(writer);
                            writer.Flush();

                            if (oWeb.GetFolder(XML_LIBRARY + "\\" + XML_FOLDER).Exists)
                                oTargetFolder = oWeb.Folders[XML_LIBRARY].SubFolders[XML_FOLDER];
                            else
                            {
                                oWeb.Folders[XML_LIBRARY].SubFolders.Add(XML_FOLDER);
                                oWeb.Folders[XML_LIBRARY].Update();
                                oTargetFolder = oWeb.Folders[XML_LIBRARY].SubFolders[XML_FOLDER];
                            }

                            oTargetFolder.Files.Add(eXMLName + ".xml", stream, replaceExistingFile);

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
        /// <summary>
        /// Returns a empty xml.
        /// </summary>
        /// <returns></returns>
        private static XDocument emptyXMLFile(XMLType xmlName)
        {
            try
            {
                XmlTextReader reader=null;
                if(xmlName== XMLType.XMLGROUPNAMES)
                   reader = new XmlTextReader(new StringReader(EMPTYXMLGROUPNAMES));
                else
                    reader = new XmlTextReader(new StringReader(EMPTYXMLGLOBALNAV));

                return XDocument.Load(reader);
            }
            catch (Exception ex)
            {
                throw;
                return null;
            }
        }

        public static StringDictionary getGroupSettings(string strValue)
        {
            StringDictionary lstProperties = new StringDictionary();
            try
            {





                XDocument xDoc = GetXDocument(XMLType.XMLGROUPNAMES);// XDocument.Load(System.Web.HttpContext.Current.Server.MapPath(XMLGROUPNAMESPATH));

                if (xDoc.Elements("GroupNames").Elements("Group").Count() > 0)
                {
                    var q = from c in xDoc.Elements("GroupNames").Elements("Group")
                            where (string)c.Element("Id") == strValue
                            select new
                            {

                                GroupId = (string)c.Element("Id"),
                                Name = (string)c.Element("Name"),
                                Description = (string)c.Element("Description"),
                                Position = (string)c.Element("Position"),
                                ParentId = (string)c.Element("ParentId")



                            };

                    if (q.Count() > 0)
                    {

                        lstProperties.Add("Id", q.Single().GroupId.ToString() != null ? q.Single().GroupId.ToString() : "");
                        lstProperties.Add("name", !string.IsNullOrEmpty(q.Single().Name) ? q.Single().Name.ToString() : (q.Single().Name.ToString() != null ? q.Single().Name.ToString() : ""));
                        lstProperties.Add("description", !String.IsNullOrEmpty(q.Single().Description) ? q.Single().Description.ToString() : "");
                        lstProperties.Add("position", string.IsNullOrEmpty(q.Single().Position.ToString().Trim()) ? "0" : q.Single().Position.ToString().Trim());
                        lstProperties.Add("parentId", q.Single().ParentId.ToString() != null ? q.Single().ParentId.ToString() : "");
                        return lstProperties;
                    }
                    else
                        return null;
                }
                else
                    return null;
            }
            catch (Exception ex)
            {

                throw;
                //return lstProperties;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="strValue"></param>
        /// <param name="vtType"></param>
        /// <returns></returns>
        public static StringDictionary getSettings(string strValue,clsCommonBL.FindBy vtType)
        {
            StringDictionary lstProperties = new StringDictionary();
            try
            {


                string strFindBy = vtType == clsCommonBL.FindBy.BySiteUrl ? "SiteUrl" : "SiteId";

                //string strPath = @"/_layouts/SP.GlobalTopMenu/Data/GlobalNav.xml";
                XDocument xDoc = GetXDocument(XMLType.XMLGLOBALNAV); // XDocument.Load(System.Web.HttpContext.Current.Server.MapPath(XMLGLOBALNAVPATH));

                if (xDoc.Elements("GlobalNav").Elements("Item").Count() > 0)
                {
                    var q = from c in xDoc.Elements("GlobalNav").Elements("Item")
                            where (string)c.Element(strFindBy) == strValue
                            select new
                            {
                                position = (string)c.Element("Position"),
                                groupId = (string)c.Element("GroupId"),
                                globalNav = (string)c.Element("GlobalNav"),
                                footer = (string)c.Element("Footer"),
                                parentId = (string)c.Element("ParentId"),
                                siteId = (string)c.Element("SiteId"),
                                Title = (string)c.Element("SiteTitle"),
                                NewTitle = (string)c.Element("NewTitle"),
                                description = (string)c.Element("SiteDescription"),
                                url = (string)c.Element("SiteUrl")
                            };

                    if (q.Count() > 0)
                    {

                        //,
                        //Convert.ToInt16(string.IsNullOrEmpty(strdChildSettings["Position"].ToString().Trim()) ? "0" : strdChildSettings["Position"].ToString().Trim()))
                        lstProperties.Add("siteId", q.Single().siteId.ToString() != null ? q.Single().siteId.ToString() : "");
                        lstProperties.Add("position", string.IsNullOrEmpty(q.Single().position.ToString().Trim()) ? "" : q.Single().position.ToString().Trim());
                        lstProperties.Add("groupId", string.IsNullOrEmpty(q.Single().groupId.ToString().Trim()) ? "" : q.Single().groupId.ToString().Trim());
                        lstProperties.Add("globalNav", q.Single().globalNav);
                        lstProperties.Add("footer", q.Single().footer);
                        lstProperties.Add("parentId", q.Single().parentId.ToString() != null ? q.Single().parentId.ToString() : "");
                        lstProperties.Add("title", !string.IsNullOrEmpty(q.Single().NewTitle) ? q.Single().NewTitle.ToString() : (q.Single().Title.ToString() != null ? q.Single().Title.ToString() : ""));
                        lstProperties.Add("description", q.Single().description.ToString() != null ? q.Single().description.ToString() : "");
                        lstProperties.Add("url", q.Single().url.ToString() != null ? q.Single().url.ToString() : "");
                        lstProperties.Add("newtitle", !string.IsNullOrEmpty(q.Single().NewTitle) ? q.Single().NewTitle.ToString() : string.Empty);

                        return lstProperties;
                    }
                    else
                        return null;
                }
                else
                    return null;
            }
            catch (Exception ex)
            {

                throw;

                //return lstProperties;
            }
        }

        /// <summary>
        /// Verifies if the Parent element exist.
        /// </summary>
        /// <param name="strParentId">Parent Guid</param>
        /// <returns></returns>
        public static int ParentExist(string strParentId)
        {
            try
            {

                XDocument xDoc = GetXDocument(XMLType.XMLGLOBALNAV); //XDocument.Load(System.Web.HttpContext.Current.Server.MapPath(XMLGLOBALNAVPATH));

                var q = from c in xDoc.Elements("GlobalNav").Elements("Item")
                        where c.Element("SiteId").Value.ToString().Trim() == strParentId.ToString().Trim() && (bool)c.Element("GlobalNav")
                        select c;
                if (q != null)
                    return q.Count();
                else
                    return 0;
            }
            catch (Exception ex)
            {

                throw;

                //return 0;
            }
        }
     
    }
}