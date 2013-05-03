using System;
using System.Web;

namespace SP.GlobalTopMenu
{
    public static class CacheHelper
    {
        #region Methods

        /// <summary>
        /// Insert value into the cache using
        /// appropriate name/value pairs
        /// </summary>
        /// <typeparam name="T">Type of cached item</typeparam>
        /// <param name="o">Item to be cached</param>
        /// <param name="key">Name of item</param>
        public static void Add<T>(T o, string key) where T : class
        {
            try
            {
                key = System.Security.Principal.WindowsIdentity.GetCurrent().User.AccountDomainSid.ToString() + key;
                // NOTE: Apply expiration parameters as you see fit.
                // In this example, I want an absolute
                // timeout so changes will always be reflected
                // at that time. Hence, the NoSlidingExpiration.
                HttpContext.Current.Cache.Insert(
                    key,
                    o,
                    null,
                    DateTime.Now.AddMinutes(1440),
                    System.Web.Caching.Cache.NoSlidingExpiration);
            }
            catch (Exception ex)
            {
                Helper.writeLog(ex);
            }
        }

        /// <summary>
        /// Remove item from cache
        /// </summary>
        /// <param name="key">Name of cached item</param>
        public static void Clear(string key)
        {
            try
            {
                key = System.Security.Principal.WindowsIdentity.GetCurrent().User.AccountDomainSid.ToString() + key;
                HttpContext.Current.Cache.Remove(key);
            }
            catch (Exception ex)
            {
                Helper.writeLog(ex);
            }
        }

        /// <summary>
        /// Check for item in cache
        /// </summary>
        /// <param name="key">Name of cached item</param>
        /// <returns></returns>
        public static bool Exists(string key)
        {
            try
            {
                key = System.Security.Principal.WindowsIdentity.GetCurrent().User.AccountDomainSid.ToString() + key;
                return HttpContext.Current.Cache[key] != null;
            }
            catch (Exception ex)
            {
                Helper.writeLog(ex);
                return false;
            }
        }

        /// <summary>
        /// Retrieve cached item
        /// </summary>
        /// <typeparam name="T">Type of cached item</typeparam>
        /// <param name="key">Name of cached item</param>
        /// <returns>Cached item as type</returns>
        public static T Get<T>(string key) where T : class
        {
            try
            {
                key = System.Security.Principal.WindowsIdentity.GetCurrent().User.AccountDomainSid.ToString() + key;
                return (T)HttpContext.Current.Cache[key];
            }
            catch
            {
                return null;
            }
        }

        #endregion Methods
    }
}