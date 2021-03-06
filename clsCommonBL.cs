﻿using System;
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
   
        public enum FindBy
        {
            BySiteId = 1,
            BySiteUrl
        };
       

     

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