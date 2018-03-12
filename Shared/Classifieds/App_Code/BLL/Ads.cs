using System;
using System.Web;
using System.Web.Security;
using System.Text;
using System.Net.Mail;
using System.Collections;
using System.Collections.Generic;
using AdsDataComponentTableAdapters;

using AspNet.StarterKits.Classifieds.Web;

namespace AspNet.StarterKits.Classifieds.BusinessLogicLayer
{
    /// <summary>
    /// Describes the type of an ad: "For Sale" or "Wanted."
    /// "Unspecified" is used to indicate a variable of this type has not yet been set.
    /// </summary>
    public enum AdType
    {
        Unspecified = 0,
        ForSale = 1,
        Wanted = 2
    }

    /// <summary>
    /// Describes the level, or priority, of an ad.
    /// This value is used when ordering results (highest to lowest)
    /// on search queries and it can affect the display of rows.
    /// 
    /// "Featured" indicates that the ad will be selected by the "Featured Ad" control
    /// that rotates ads to display on the site.
    /// "Unspecified" is used to indicate a variable of this type has not yet been set.
    /// 
    /// Use this enumeration to differentiate ads with different priorities
    /// or even price points.
    /// </summary>
    public enum AdLevel
    {
        Unspecified = 0,
        Normal = 10,
        Featured = 50
    }

    /// <summary>
    /// Describes the status of an ad:
    /// Only ads with positive AdStatus values will be displayed on the public site;
    /// administrators will be able to see all.
    /// 
    /// 
    /// 
    /// "Unspecified" is used to indicate a variable of this type has not yet been set.
    /// </summary>
    public enum AdStatus
    {
        ActivationPending = -200,
        Deleted = -100,
        Inactive = -50,
        Unspecified = 0,
        Activated = 100
    }
    
    public sealed class AdsDB 
    {

        private AdsDB()
        {
        }

        #region Inserting Ads
        public static int InsertAd(int memberId, int categoryId, string title, string description, string url, decimal price, string location, int numDaysActive, AdLevel adLevel, AdStatus adStatus, AdType adType)
        {
            SiteSettings s = SiteSettings.GetSharedSettings();

            int numViews = 0, numResponses = 0;
            DateTime dateCreated = DateTime.Now;
            DateTime? dateApproved = null;

            if (numDaysActive > s.MaxAdRunningDays)
                numDaysActive = s.MaxAdRunningDays;

            if (numDaysActive < 1)
                numDaysActive = 1;

            DateTime expirationDate = DateTime.Today.AddDays(numDaysActive);

            if (s.AdActivationRequired)
            {
                adStatus = AdStatus.ActivationPending;
                dateApproved = null;
            }
            else
            {
                adStatus = AdStatus.Activated;
                dateApproved = dateCreated;
            }

            if (adLevel == AdLevel.Unspecified)
                adLevel = AdLevel.Normal;

            int adId = DefaultValues.IdNullValue;
            AdsDataComponent.AdsRow ad = null;
            using (AdsDataAdapter db = new AdsDataAdapter())
            {
                adId = (int) db.InsertAd(memberId, categoryId, title, description, url, price, location, expirationDate, dateCreated, dateApproved, numViews, numResponses, (int)adLevel, (int)adStatus, (int)adType);
                
                if (s.AdminNotification == AdminNotificationSetting.EachAd)
                    ad = GetFirstRow(db.GetAdById(adId));
            }

            if (ad != null)
                Maintenance.SendAdNotification(ad);
            
            return adId;
        }

        public static void InsertSavedAd(int adId, int memberId)
        {
            using (AdsDataAdapter db = new AdsDataAdapter())
            {
                db.InsertSavedAd(adId, memberId);
            }
        }

        public static void InsertSavedAdList(List<int> adIds, int memberId)
        {
            using (AdsDataAdapter db = new AdsDataAdapter())
            {
                for (int i = 0; i < adIds.Count; i++)
                    db.InsertSavedAd(adIds[i], memberId);
            }
        }
        #endregion

        #region GetAds...
        public static AdsDataComponent.AdsRow GetAdById(int adId)
        {
            using (AdsDataAdapter db = new AdsDataAdapter())
            {
                return GetFirstRow(db.GetAdById(adId));
            }
        }

        public static AdsDataComponent.AdsDataTable GetActiveAdsByQuery(int recordLimit, int categoryId, int memberId, decimal maxPrice, string searchTerm, string location, int adType, int adLevel, int dayRange, bool mustHaveImage)
        {
            String escapedSearchTerm = AdsDB.EscapeWildcardCharacters(searchTerm);
            String escapedLocation = AdsDB.EscapeWildcardCharacters(location);
            return GetAdsByQuery(recordLimit,
                categoryId, memberId, maxPrice,
                HttpUtility.HtmlEncode(escapedSearchTerm),
                HttpUtility.HtmlEncode(escapedLocation),
                adType, (int)AdStatus.Activated,
                adLevel, dayRange, mustHaveImage);
        }


        public static AdsDataComponent.AdsDataTable GetAdsByQuery(int recordLimit, int categoryId, int memberId, decimal maxPrice, string searchTerm, string location, int adType, int adStatus, int adLevel, int dayRange, bool mustHaveImage)
        {
            DateTime? minCreatedDate = null;
            if (location == null) location = "";
            if (dayRange > 0)
                minCreatedDate = DateTime.Now.Subtract(TimeSpan.FromDays(dayRange));
            // To return no results if the searchTerm is empty remove the following
            // check on the searchTerm.
            if (searchTerm == null)
                searchTerm = "";

            using (AdsDataAdapter db = new AdsDataAdapter())
            {
                AdsDataComponent.AdsDataTable aa = null;
                aa = db.GetAllAdsByQuery(recordLimit, categoryId, memberId, maxPrice, searchTerm, location, adType, adStatus, adLevel, minCreatedDate, mustHaveImage);
                return aa;
            }
        }

        public static AdsDataComponent.AdsDataTable GetPendingAds()
        {
            return GetAdsByStatus(AdStatus.ActivationPending, DefaultValues.IdNullValue);
        }

        public static AdsDataComponent.AdsDataTable GetPendingAds(int memberId)
        {
            return GetAdsByStatus(AdStatus.ActivationPending, memberId);
        }

        public static AdsDataComponent.AdsDataTable GetActiveAds(int memberId)
        {
            return GetAdsByStatus(AdStatus.Activated, memberId);
        }

        public static AdsDataComponent.AdsDataTable GetInactiveAds(int memberId)
        {
            return GetAdsByStatus(AdStatus.Inactive, memberId);
        }

        public static AdsDataComponent.AdsDataTable GetAdsByStatus(AdStatus adStatus, int memberId)
        {
            using (AdsDataAdapter db = new AdsDataAdapter())
            {
                return db.GetAdsByStatus((int)adStatus, memberId);
            }
        }

        public static AdsDataComponent.AdsDataTable GetAdsByStatus(AdStatus adStatus)
        {
            return GetAdsByStatus(adStatus, DefaultValues.IdNullValue);
        }

        public static AdsDataComponent.AdsDataTable GetFeaturedAdsSelection(int maxNumAds)
        {
            if (maxNumAds < 1)
                return null;

            using (AdsDataAdapter db = new AdsDataAdapter())
            {
                return db.GetAdsByRandomOrder(maxNumAds, (int)AdStatus.Activated, (int)AdLevel.Featured);
            }
        }

        public static AdsDataComponent.AdsDataTable GetSavedAds(int memberId)
        {
            using (AdsDataAdapter db = new AdsDataAdapter())
            {
                return db.GetSavedAds(memberId);
            }
        }
        #endregion

        #region Update Ads
        public static void UpdateAd(int original_Id, int memberId, string title, string description, string url, decimal price, string location, bool isRelisting)
        {
            if (url == null)
                url = String.Empty;
            using (AdsDataAdapter db = new AdsDataAdapter())
            {
                db.UpdateAd(original_Id, memberId, 
                    HttpUtility.HtmlEncode(title), 
                    HttpUtility.HtmlEncode(description), 
                    HttpUtility.UrlEncode(url), 
                    price, 
                    HttpUtility.HtmlEncode(location));
            }
        }

        public static void RelistAd(int adId, int categoryId, string title, string description, string url, decimal price, string location, int numDaysActive, AdLevel adLevel, AdStatus adStatus, AdType adType)
        {

            SiteSettings s = SiteSettings.GetSharedSettings();

            DateTime dateCreated = DateTime.Now;
            DateTime? dateApproved = null;

            if (numDaysActive > s.MaxAdRunningDays)
                numDaysActive = s.MaxAdRunningDays;

            if (numDaysActive < 1)
                numDaysActive = 1;

            DateTime expirationDate = DateTime.Today.AddDays(numDaysActive);

            if (s.AdActivationRequired)
            {
                adStatus = AdStatus.ActivationPending;
                dateApproved = null;
            }
            else
            {
                adStatus = AdStatus.Activated;
                dateApproved = dateCreated;
            }

            if (adLevel == AdLevel.Unspecified)
                adLevel = AdLevel.Normal;

            AdsDataComponent.AdsRow ad = null;
            using (AdsDataAdapter db = new AdsDataAdapter())
            {
                db.RelistAd(adId, categoryId, title, description, url, price, location, expirationDate, dateCreated, dateApproved, (int)adLevel, (int)adStatus, (int)adType);

                if (s.AdminNotification == AdminNotificationSetting.EachAd)
                    ad = GetFirstRow(db.GetAdById(adId));
            }

            if (ad != null)
                Maintenance.SendAdNotification(ad);
        }

        public static void IncrementViewCount(int adId)
        {
            using (AdsDataAdapter db = new AdsDataAdapter())
            {
                db.UpdateAdStats(adId, 1, 0);
            }
        }

        public static void IncrementResponseCount(int adId)
        {
            using (AdsDataAdapter db = new AdsDataAdapter())
            {
                db.UpdateAdStats(adId, 0, 1);
            }
        }

        public static void UpdateAdCategory(int adId, int categoryId)
        {
            using (AdsDataAdapter db = new AdsDataAdapter())
            {
                db.UpdateAdCategory(adId, categoryId);
            }
        }
      
        public static void UpdateAdLevel(int adId, AdLevel adLevel)
        {
            using (AdsDataAdapter db = new AdsDataAdapter())
            {
                db.UpdateAdLevel(adId, (int)adLevel);
            }
        }

        public static void UpdateAdLevelList(List<int> adIds, AdLevel adLevel)
        {
            using (AdsDataAdapter db = new AdsDataAdapter())
            {
                for (int i = 0; i < adIds.Count; i++)
                    db.UpdateAdLevel(adIds[i], (int)adLevel);
            }
        }

        public static void UpdateAdStatus(int adId, AdStatus adStatus)
        {
            using (AdsDataAdapter db = new AdsDataAdapter())
            {
                db.UpdateAdStatus(adId, (int)adStatus);
            }
        }

        public static void UpdateAdStatusList(List<int> adIds, AdStatus adStatus)
        {
            using (AdsDataAdapter db = new AdsDataAdapter())
            {
                for (int i = 0; i < adIds.Count; i++)
                    db.UpdateAdStatus(adIds[i], (int)adStatus);
            }
        }

        public static void ExpireAd(int adId, int memberId)
        {
            using (AdsDataAdapter db = new AdsDataAdapter())
            {
                db.ExpireAd(adId, memberId, (int)AdStatus.Inactive);
            }
        }
        #endregion

        #region Remove Ads
        public static void RemoveFromUserList(int id)
        {
            SiteSettings s = SiteSettings.GetSharedSettings();
            if (s.AllowUsersToDeleteAdsInDB)
            {
                RemoveFromDatabase(id);
            }
            else
            {
                UpdateAdStatus(id, AdStatus.Deleted);
            }

        }

        public static void RemoveFromDatabase(int adId)
        {
            using (AdsDataAdapter db = new AdsDataAdapter())
            {
                PhotosDB.RemovePhotosByAdId(adId);
                db.RemoveAd(adId);
            }
        }

        public static void RemoveListFromDatabase(List<int> adIds)
        {
            using (AdsDataAdapter db = new AdsDataAdapter())
            {
                for (int i = 0; i < adIds.Count; i++)
                {
                    PhotosDB.RemovePhotosByAdId(adIds[i]);
                    db.RemoveAd(adIds[i]);
                }
            }
        }

        public static void RemoveFromDatabaseByStatus(AdStatus adStatus)
        {
            PhotosDB.RemovePhotosOfDeletedAds();
            using (AdsDataAdapter db = new AdsDataAdapter())
            {
                db.RemoveAdsByStatus((int)adStatus);
            }
        }

        public static void RemoveSavedAd(int id, int memberId)
        {
            using (AdsDataAdapter db = new AdsDataAdapter())
            {
                db.RemoveSavedAd(id, memberId);
            }
        }
        #endregion

        #region Various Helper and Maintenance Methods
        public static void MoveAdsToCategory(int currentCategoryId, int newCategoryId)
        {
            using (AdsDataAdapter db = new AdsDataAdapter())
            {
                db.MoveAds(currentCategoryId, newCategoryId);
            }
        }

        public static bool SendAdInEmail(int adId, string senderName, string senderAddress, string recipientEmail, string subject, string message)
        {
            bool sent = false;
            string from = String.Format("{0} <{1}>", senderName, senderAddress);
            try
            {
                MailMessage m = new MailMessage(from, recipientEmail);
                m.Subject = subject;
                m.Body = message;
                SmtpClient client = new SmtpClient();
                client.Send(m);
                sent = true;
            }
            catch
            {
                // Consider customizing the message for the EmailNotSentPanel in the ShowAds page.
                sent = false;
            }
            return sent;
        }

        public static bool SendResponse(int adId, string senderName, string senderAddress, string comments)
        {
            bool sent = false;
            SiteSettings s = SiteSettings.GetSharedSettings();
            AdsDataComponent.AdsRow ad = GetAdById(adId);
            if (ad != null)
            {
                MembershipUser adUser = Membership.GetUser(ad.MemberName);
                if (adUser != null)
                {
                    
                    StringBuilder sb = new StringBuilder();
                    sb.AppendFormat("A user has sent a response to your Ad '{0}' at {1}", ad.Title, s.SiteName);
                    sb.AppendLine();

                    sb.AppendLine();

                    sb.AppendLine("The contact information of the user is below:");
                    
                    sb.AppendLine();

                    sb.AppendLine("Name: " + senderName);
                    sb.AppendLine("Email: " + senderAddress);
                    
                    sb.AppendLine();
                    
                    sb.AppendLine("User comments/questions:");
                    sb.AppendLine(comments);
                    
                    sb.AppendLine();
                    
                    sb.AppendLine("You can respond to the user by using the Reply feature of your email client.");
                    
                    sb.AppendLine();
                    sb.AppendLine(s.SiteName);
                    sb.AppendLine(ClassifiedsHttpApplication.SiteUrl);


                    string from = String.Format("{0} <{1}>", senderName, senderAddress);
                    try
                    {
                        MailMessage m = new MailMessage(from, adUser.Email);
                        m.Subject = "Response for Ad: " + ad.Title;
                        m.Body = sb.ToString();
                        SmtpClient client = new SmtpClient();
                        client.Send(m);

                        IncrementResponseCount(adId);
                        sent = true;
                    }
                    catch
                    {
                        // Consider customizing the message for the EmailNotSentPanel in the ShowAds page.
                        sent = false;
                    }
                }
            }
            return sent;
        }

        private static AdsDataComponent.AdsRow GetFirstRow(AdsDataComponent.AdsDataTable table)
        {
            if (table != null && table.Rows.Count > 0)
            {
                AdsDataComponent.AdsRow row;
                row = table.Rows[0] as AdsDataComponent.AdsRow;
                row.URL = System.Web.HttpUtility.UrlDecode(row.URL);
                row.Title = System.Web.HttpUtility.HtmlDecode(row.Title);
                row.Description = System.Web.HttpUtility.HtmlDecode(row.Description);
                row.Location = System.Web.HttpUtility.HtmlDecode(row.Location);
                return row;
            }
            else
                return null;
        }

        private static String EscapeWildcardCharacters(String input)
        {
            string output = input;
            if (!String.IsNullOrEmpty(input))
            {
                output = output.Replace("%", "[%]");
                output = output.Replace("_", "[_]");
                output = output.Replace("[", "[[]");
                // Could also remove common search terms.
            }
            return output;
        }
        #endregion

    }
}
