using System;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using Microsoft.SharePoint;

namespace SP.GlobalTopMenu
{
    public static class XMLHelper
    {
        public enum XMLType
        {
            XMLGLOBALNAV,
            XMLGROUPNAMES,
            XMLSETTINGS,
            XMLLOGS
        }

        #region CONST

        public const string EMPTYXMLGLOBALNAV = "<GlobalNav></GlobalNav>";
        public const string EMPTYXMLGROUPNAMES = "<GroupNames></GroupNames>";
        public const string EMPTYXMLSETTINGS = "<Settings></Settings>";
        public const string EMPTYXMLLOGS = "<Logs></Logs>";

        #endregion CONST

        #region Methods
        public static StringDictionary getGeneralSettings()
        {
            StringDictionary lstProperties = new StringDictionary();
            try
            {
                XDocument xDoc = GetXDocument(XMLType.XMLSETTINGS);

                if (xDoc.Elements("Settings").Elements("Option").Count() > 0)
                {
                    var q = from c in xDoc.Elements("Settings").Elements("Option")
                            select new
                            {
                                AddSiteOwnerOption = (string)c.Element("AddSiteOwnerOption"),
                                IncludeBreadCrumb = (string)c.Element("IncludeBreadCrumb")
                            };

                    if (q.Count() > 0)
                    {
                        lstProperties.Add("AddSiteOwnerOption", !string.IsNullOrEmpty(q.Single().AddSiteOwnerOption) ? q.Single().AddSiteOwnerOption.ToString() : (q.Single().AddSiteOwnerOption.ToString() != null ? q.Single().AddSiteOwnerOption.ToString() : ""));
                        lstProperties.Add("IncludeBreadCrumb", !string.IsNullOrEmpty(q.Single().IncludeBreadCrumb) ? q.Single().IncludeBreadCrumb.ToString() : (q.Single().IncludeBreadCrumb.ToString() != null ? q.Single().IncludeBreadCrumb.ToString() : ""));
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
                Helper.writeLog(ex);
                return lstProperties;
            }
        }
        /// <summary>
        ///
        /// </summary>
        /// <param name="strValue"></param>
        /// <returns></returns>
        public static StringDictionary getGroupSettings(string strValue)
        {
            StringDictionary lstProperties = new StringDictionary();
            try
            {
                XDocument xDoc = GetXDocument(XMLType.XMLGROUPNAMES);

                if (xDoc.Elements("GroupNames").Elements("Group").Count() > 0)
                {
                    var q = from c in xDoc.Elements("GroupNames").Elements("Group")
                            where (string)c.Element("Id") == strValue
                            select new
                            {
                                GroupId = (string)c.Element("Id"),
                                Name = (string)c.Element("Name"),
                                Url =c.Element("Url")==null?String.Empty:(string)c.Element("Url"),
                                Description = (string)c.Element("Description"),
                                Position = (string)c.Element("Position"),
                                ParentId = (string)c.Element("ParentId")
                            };

                    if (q.Count() > 0)
                    {
                        lstProperties.Add("Id", q.Single().GroupId.ToString() != null ? q.Single().GroupId.ToString() : "");
                        lstProperties.Add("name", !string.IsNullOrEmpty(q.Single().Name) ? q.Single().Name.ToString() : (q.Single().Name.ToString() != null ? q.Single().Name.ToString() : ""));
                        lstProperties.Add("url", !string.IsNullOrEmpty(q.Single().Url) ? q.Single().Url.ToString() : (q.Single().Url.ToString() != null ? q.Single().Url.ToString() : ""));
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
                Helper.writeLog(ex);
                return lstProperties;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="strValue"></param>
        /// <param name="vtType"></param>
        /// <returns></returns>
        public static StringDictionary getSettings(string strValue, Helper.FindBy vtType)
        {
            StringDictionary lstProperties = new StringDictionary();
            try
            {
                string strFindBy = vtType == Helper.FindBy.BySiteUrl ? "SiteUrl" : "SiteId";

                XDocument xDoc = GetXDocument(XMLType.XMLGLOBALNAV);

                if (xDoc.Elements("GlobalNav").Elements("Item").Count() > 0)
                {
                    var q = (from c in xDoc.Elements("GlobalNav").Elements("Item")
                             where (string)c.Element(strFindBy) == strValue
                             select new
                             {
                                 position = (string)c.Element("Position"),
                                 groupId = (string)c.Element("GroupId"),
                                 globalNav = (string)c.Element("GlobalNav"),
                                 footer = (string)c.Element("Footer"),
                                 AddInsideParent = (string)c.Element("AddInsideParent"),
                                 ExternalLnk = (string)c.Element("ExternalLnk"),
                                 parentId = (string)c.Element("ParentId"),
                                 siteId = (string)c.Element("SiteId"),
                                 Title = (string)c.Element("SiteTitle"),
                                 NewTitle = (string)c.Element("NewTitle"),
                                 description = (string)c.Element("SiteDescription"),
                                 url = (string)c.Element("SiteUrl")
                             }).GroupBy(x => x.url).Select(x => x.FirstOrDefault());

                    if (q.Count() > 0)
                    {
                        lstProperties.Add("siteId", q.Single().siteId.ToString() != null ? q.Single().siteId.ToString() : "");
                        lstProperties.Add("position", string.IsNullOrEmpty(q.Single().position.ToString().Trim()) ? "" : q.Single().position.ToString().Trim());
                        lstProperties.Add("groupId", string.IsNullOrEmpty(q.Single().groupId.ToString().Trim()) ? "" : q.Single().groupId.ToString().Trim());
                        lstProperties.Add("globalNav", q.Single().globalNav);
                        lstProperties.Add("footer", q.Single().footer);
                        lstProperties.Add("AddInsideParent", q.Single().AddInsideParent);
                        lstProperties.Add("externalLnk", q.Single().ExternalLnk);
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
                Helper.writeLog(ex);

                return lstProperties;
            }
        }

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
                using (SPSite oSite = new SPSite(Helper.SiteRootUrl))
                {
                    using (SPWeb oWeb = oSite.OpenWeb())
                    {
                        SPFolder oTargetFolder = oWeb.Folders[Helper.GTM_LIBRARY];
                        SPFile spFile;

                        if (oWeb.GetFolder(oTargetFolder.Url + "/" + Helper.XML_FOLDER).Exists)
                        {
                            if (oWeb.GetFile(oTargetFolder.SubFolders[Helper.XML_FOLDER].Url + "/" + eXMLName + ".xml").Exists)
                            {
                                spFile = oTargetFolder.SubFolders[Helper.XML_FOLDER].Files[eXMLName + ".xml"];

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
        /// Verifies if the Parent element exist.
        /// </summary>
        /// <param name="strParentId">Parent Guid</param>
        /// <returns></returns>
        public static bool ParentExist(string strParentId)
        {
            try
            {
                XDocument xDoc = GetXDocument(XMLType.XMLGLOBALNAV);

                var q = from c in xDoc.Elements("GlobalNav").Elements("Item")
                        where c.Element("SiteId").Value.ToString().Trim() == strParentId.ToString().Trim() && (bool)c.Element("GlobalNav")
                        select c;

                if (q != null && q.Count()>0)
                    return true; //q.Count();
                else
                    return false;// 0;
            }
            catch (Exception ex)
            {
                Helper.writeLog(ex);

                return false;// 0;
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
                    using (SPSite oSite = new SPSite(Helper.SiteRootUrl))
                    {
                        using (SPWeb oWeb = oSite.OpenWeb())
                        {
                            oSite.AllowUnsafeUpdates = true;
                            oWeb.AllowUnsafeUpdates = true;

                            var stream = new MemoryStream();
                            var writer = XmlWriter.Create(stream);
                            document.WriteTo(writer);
                            writer.Flush();

                            if (oWeb.GetFolder(Helper.GTM_LIBRARY + "\\" + Helper.XML_FOLDER).Exists)
                                oTargetFolder = oWeb.Folders[Helper.GTM_LIBRARY].SubFolders[Helper.XML_FOLDER];
                            else
                            {
                                oWeb.Folders[Helper.GTM_LIBRARY].SubFolders.Add(Helper.XML_FOLDER);
                                oWeb.Folders[Helper.GTM_LIBRARY].Update();
                                oTargetFolder = oWeb.Folders[Helper.GTM_LIBRARY].SubFolders[Helper.XML_FOLDER];
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
                Helper.writeLog(ex);
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
                XmlTextReader reader = null;
                switch (xmlName)
                {
                    case XMLType.XMLGLOBALNAV:
                        reader = new XmlTextReader(new StringReader(EMPTYXMLGLOBALNAV));
                        break;

                    case XMLType.XMLGROUPNAMES:
                        reader = new XmlTextReader(new StringReader(EMPTYXMLGROUPNAMES));
                        break;

                    case XMLType.XMLSETTINGS:
                        reader = new XmlTextReader(new StringReader(EMPTYXMLSETTINGS));
                        break;

                    case XMLType.XMLLOGS:
                        reader = new XmlTextReader(new StringReader(EMPTYXMLLOGS));
                        break;

                    default:
                        break;
                }
                return XDocument.Load(reader);
            }
            catch (Exception ex)
            {
                Helper.writeLog(ex);
                return null;
            }
        }

        #endregion Methods
    }
}