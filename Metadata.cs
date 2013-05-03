using System;
using System.Data;
using System.Linq;
using System.Reflection;


namespace SP.GlobalTopMenu
{
    [Serializable]
    public class Metadata:IDisposable
    {
        #region properties


        public double SiteId { get; set; }

        public string SiteTitle { get; set; }
        public string NewTitle { get; set; }
        public string SiteDescription { get; set; }
        public string SiteUrl { get; set; }
        public string Position { get; set; }
        public string GroupId { get; set; }
        public string GlobalNav { get; set; }
        public string Footer { get; set; }
        public string ParentId { get; set; }
        

        #endregion

        #region Constractors
        /// <summary>
        /// 
        /// </summary>
        public Metadata() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="drDoc"></param>
        /// <param name="oMetadata"></param>
        public Metadata(DataRow drDoc,Metadata oMetadata)
        {
            try
            {


                SiteId = oMetadata.SiteId;
                SiteTitle = oMetadata.SiteTitle;
                NewTitle = oMetadata.NewTitle;
                SiteDescription = oMetadata.SiteDescription;
                SiteUrl = oMetadata.SiteUrl;
                Position = oMetadata.Position;
                GroupId = oMetadata.GroupId;
                GlobalNav = oMetadata.GlobalNav;

                Footer = drDoc["FileName"].ToString();

                ParentId = drDoc["File"].ToString();

            
            }
            catch (Exception ex)
            {
                 throw;
            }
        }

      
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dtDocInfo"></param>
        public Metadata(DataTable dtDocInfo)
        {
            try
            {

                SiteId = Convert.ToDouble(dtDocInfo.Rows[0]["SiteId"]);
                SiteTitle = dtDocInfo.Rows[0]["SiteTitle"].ToString();
                NewTitle = dtDocInfo.Rows[0]["NewTitle"].ToString();
                SiteDescription = dtDocInfo.Rows[0]["SiteDescription"].ToString();
                SiteUrl = dtDocInfo.Rows[0]["SiteUrl"].ToString();
                Position = dtDocInfo.Rows[0]["Position"].ToString();
                GroupId = dtDocInfo.Rows[0]["GroupId"].ToString();
                GlobalNav = dtDocInfo.Rows[0]["GlobalNav"].ToString();
                Footer = dtDocInfo.Rows[0]["Footer"].ToString();
                ParentId = dtDocInfo.Rows[0]["ParentId"].ToString();
             
            }
            catch (Exception ex)
            {
                 throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="drDocInfo"></param>
        public Metadata(DataRow drDocInfo)
        {
            try
            {

                SiteId = Convert.ToDouble(drDocInfo["SiteId"]);
                SiteTitle = string.IsNullOrEmpty(drDocInfo["SiteTitle"].ToString()) ? "1" : drDocInfo["SiteTitle"].ToString();
                NewTitle = drDocInfo["NewTitle"].ToString();
                SiteDescription = drDocInfo["SiteDescription"].ToString();
                SiteUrl = drDocInfo["SiteUrl"].ToString();
                Position = drDocInfo["Position"].ToString();
                GroupId = drDocInfo["GroupId"].ToString();
                GlobalNav = drDocInfo["GlobalNav"].ToString();
                Footer = string.IsNullOrEmpty(drDocInfo["Footer"].ToString()) ? "0" : drDocInfo["Footer"].ToString(); ;
                ParentId = drDocInfo["ParentId"].ToString();
            
            }
            catch (Exception ex)
            {
                 throw;
            }
        }

        public Metadata(DataRow drReference, Metadata oMetadata, bool bReference)
        {
            try
            {

                SiteId = Convert.ToDouble(drReference["SiteId"]);
                SiteTitle = string.IsNullOrEmpty(drReference["SiteTitle"].ToString()) ? "1" : drReference["SiteTitle"].ToString();
                NewTitle = oMetadata.NewTitle;
                SiteDescription = oMetadata.SiteDescription;
                SiteUrl = oMetadata.SiteUrl;
                Position = oMetadata.Position;
                GroupId = oMetadata.GroupId;
                GlobalNav = drReference["GlobalNav"].ToString();
                Footer = string.IsNullOrEmpty(drReference["Footer"].ToString()) ? "0" : drReference["Footer"].ToString();
                ParentId = drReference["ParentId"].ToString();
         
            }
            catch (Exception ex)
            {
                 throw;
            }
        }

        
        public bool EqualTo(Metadata oMetadata)
        {
            PropertyInfo[] properties = this.GetType().GetProperties();

            foreach (PropertyInfo property in properties)
            {
                if (this.GetType().GetProperty(property.Name).GetValue(this, null) != oMetadata.GetType().GetProperty(property.Name).GetValue(oMetadata, null))
                {
                    return false;
                
                }
            
            }
            return true;
        
        }
        #endregion

        #region IDisposable Members

        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        #endregion
    }

   
}
