using System;
using System.Linq;
using System.Xml.Linq;
using System.Collections.Specialized;
using Microsoft.SharePoint;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

namespace SP.GlobalTopMenu
{
    public static class clsCommonBL
    {
        const string XMLGLOBALNAVPATH = @"/_layouts/SP.GlobalTopMenu/Data/GlobalNav.xml";
        const string XMLGROUPNAMESPATH = @"/_layouts/SP.GlobalTopMenu/Data/GroupNames.xml";

        public enum FindBy
        {
            BySiteId = 1,
            BySiteUrl
        };
        /// <summary>
        /// Verifies if the Parent element exist.
        /// </summary>
        /// <param name="strParentId">Parent Guid</param>
        /// <returns></returns>
        public static int ParentExist(string strParentId)
        {
            try
            {
                
                XDocument xDoc = XDocument.Load(System.Web.HttpContext.Current.Server.MapPath(XMLGLOBALNAVPATH));

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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="strValue"></param>
        /// <param name="vtType"></param>
        /// <returns></returns>
        public static StringDictionary getSettings(string strValue, FindBy vtType)
        {
            StringDictionary lstProperties = new StringDictionary();
            try
            {
                

                string strFindBy = vtType == FindBy.BySiteUrl ? "SiteUrl" : "SiteId";

                //string strPath = @"/_layouts/SP.GlobalTopMenu/Data/GlobalNav.xml";
                XDocument xDoc = XDocument.Load(System.Web.HttpContext.Current.Server.MapPath(XMLGLOBALNAVPATH));

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
                        lstProperties.Add("title", !string.IsNullOrEmpty(q.Single().NewTitle) ? q.Single().NewTitle.ToString() :(q.Single().Title.ToString() != null ? q.Single().Title.ToString() : ""));
                        lstProperties.Add("description", q.Single().description.ToString() != null ?  q.Single().description.ToString() : "");
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
        /// This method verifies if the user has permissions to access specific SubSite
        /// </summary>
        /// <param name="web">Sub Site where it is need to verify if the user has permissions</param>
        /// <param name="userLoginName">UserName of the user</param>
        /// <returns>True: User has permission to access the sub site. False: User does not have permissions to access the sub site</returns>
        public static bool IsUserHasAccess(string SiteUrl, string userLoginName)
        {
            bool hasAccess = false;
            SPBasePermissions sitePermissions = SPBasePermissions.ViewPages;
            try
            {
                SPSecurity.RunWithElevatedPrivileges(
                    delegate()
                    {
                        if (SPContext.Current.Site.WebApplication.Sites[SiteUrl] != null)
                        {
                            using (SPWeb web = SPContext.Current.Site.WebApplication.Sites[SiteUrl].OpenWeb())
                            {
                                web.Site.CatchAccessDeniedException = false;
                                hasAccess = web.DoesUserHavePermissions(userLoginName, sitePermissions);
                            }
                        }
                        else
                            hasAccess = true;
                    });
            }
            catch
            {
                hasAccess = false;
            }
            return hasAccess;
        }





        public static StringDictionary getGroupSettings(string strValue)
        {
            StringDictionary lstProperties = new StringDictionary();
            try
            {




                
                XDocument xDoc = XDocument.Load(System.Web.HttpContext.Current.Server.MapPath(XMLGROUPNAMESPATH));

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
                        lstProperties.Add("description", !String.IsNullOrEmpty(q.Single().Description)? q.Single().Description.ToString() : "");
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
        /// Converts Linq result to DataTable object.
        /// </summary>
        /// <typeparam name="T">Any type</typeparam>
        /// <param name="varlist">LINQ Result</param>
        /// <returns></returns>
        public static DataTable LINQToDataTable<T>(IEnumerable<T> varlist)
        {

            DataTable dtReturn = new DataTable();
            try
            {
                // column names
                PropertyInfo[] oProps = null;

                if (varlist == null) return dtReturn;

                foreach (T rec in varlist)
                {


                    // Use reflection to get property names, to create table, Only first time, others will follow
                    if (oProps == null)
                    {
                        oProps = ((Type)rec.GetType()).GetProperties();
                        foreach (PropertyInfo pi in oProps)
                        {
                            Type colType = pi.PropertyType;

                            if ((colType.IsGenericType) && (colType.GetGenericTypeDefinition()
                            == typeof(Nullable<>)))
                            {
                                colType = colType.GetGenericArguments()[0];
                            }
                            if (pi.Name != "Item")
                                dtReturn.Columns.Add(new DataColumn(pi.Name, colType));
                        }
                    }

                    DataRow dr = dtReturn.NewRow();

                    foreach (PropertyInfo pi in oProps)
                    {
                        if (pi.Name != "Item" && pi.Name != "Attachments")
                            dr[pi.Name] = pi.GetValue(rec, null) == null ? DBNull.Value : pi.GetValue(rec, null);
                    }

                    dtReturn.Rows.Add(dr);
                }
            }
            catch (Exception ex)
            {
                
                throw;

            }
            return dtReturn;
        }
    }

 


}