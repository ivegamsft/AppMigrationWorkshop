using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Text;
using System.Xml.Serialization;

using AspNet.StarterKits.Classifieds.Web;


namespace AspNet.StarterKits.Classifieds.BusinessLogicLayer
{
    /// <summary>
    /// Contains the implementation for the site settings.
    /// </summary>
    public class SiteSettings
    {

        private const string XmlConfigFile = "~/App_Data/site-config.xml";

        private int _maxPhotosPerAd;
        private bool _storePhotosInDatabase;
        private string _serverPhotoUploadDirectory;

        private bool _adApprovalRequired;
        
        private bool _allowUsersToEditAds;
        private bool _allowUsersToDeleteAdsInDB;

        private int _maxAdRunningDays;
        private AdminNotificationSetting _adminNotification;
       
        private string _siteName;
        private string _siteEmail;

        public bool AllowImageUploads
        {
            get
            {
                return (_maxPhotosPerAd > 0);
            }

        }
        
        public int MaxPhotosPerAd
        {
            get
            {
                return _maxPhotosPerAd;
            }
            set
            {
                lock (this)
                {
                    _maxPhotosPerAd = value;
                }
            }
        }

        public bool StorePhotosInDatabase
        {
            get
            {
                return _storePhotosInDatabase;
            }
            set
            {
                lock (this)
                {
                    _storePhotosInDatabase = value;
                }
            }

        }

        public string ServerPhotoUploadDirectory
        {
            get
            {
                return _serverPhotoUploadDirectory;
            }
            set
            {
                lock (this)
                {
                    _serverPhotoUploadDirectory = value;
                }
            }

        }

        public bool AdActivationRequired
        {
            get
            {
                return _adApprovalRequired;
            }
            set
            {
                lock (this)
                {
                    _adApprovalRequired = value;
                }
            }

        }
        public bool AllowUsersToEditAds
        {
            get
            {
                return _allowUsersToEditAds;
            }
            set
            {
                lock (this)
                {
                    _allowUsersToEditAds = value;
                }
            }
        }
        public bool AllowUsersToDeleteAdsInDB
        {
            get
            {
                return _allowUsersToDeleteAdsInDB;
            }
            set
            {
                lock (this)
                {
                    _allowUsersToDeleteAdsInDB = value;
                }
            }
        }
        public int MaxAdRunningDays
        {
            get
            {
                return _maxAdRunningDays;
            }
            set
            {
                lock (this)
                {
                    _maxAdRunningDays = value;
                }
            }

        }

        [System.ComponentModel.TypeConverter(typeof(System.ComponentModel.EnumConverter))]
        public AdminNotificationSetting AdminNotification
        {
            get
            {
                return _adminNotification;
            }
            set
            {
                lock (this)
                {
                    _adminNotification = value;
                }
            }

        }

        public string SiteName
        {
            get
            {
                return _siteName;
            }
            set
            {
                lock (this)
                {
                    _siteName = value;
                }
            }
        }

        public string SiteEmailAddress
        {
            get
            {
                return _siteEmail;
            }
            set
            {
                lock (this)
                {
                    _siteEmail = value;
                }
            }
        }

        public string SiteEmailFromField
        {
            get
            {
                return String.Format("{0} <{1}>", _siteName, _siteEmail);
            }
        }

        public static SiteSettings LoadFromConfiguration()
        {
            SiteSettings s = LoadFromXml();

            if (s == null)
            {
                s = new SiteSettings();
                s.MaxPhotosPerAd = 5;
                s.StorePhotosInDatabase = true;
                s.ServerPhotoUploadDirectory = "Upload";
                s.AdActivationRequired = false;
                s.AllowUsersToEditAds = true;
                s.AllowUsersToDeleteAdsInDB = true;
                s.MaxAdRunningDays = 21;
                s.AdminNotification = AdminNotificationSetting.None;
                s.SiteName = "ASP.NET Classifieds";
                s.SiteEmailAddress = "classifieds@yoursite.com";
                SaveToXml(s);
            }
            return s;
        }

        public static SiteSettings GetSharedSettings()
        {
            return ClassifiedsHttpApplication.ClassifiedsApplicationSettings;
        }

        public static bool UpdateSettings(SiteSettings newSettings)
        {
            // write settings to code or db

            // update Application-wide settings, only over-writing settings that users should edit
            lock (ClassifiedsHttpApplication.ClassifiedsApplicationSettings)
            {
                // Ads must be activated before appearing on the site
                ClassifiedsHttpApplication.ClassifiedsApplicationSettings.AdActivationRequired = newSettings.AdActivationRequired;

                // Store Photos in Database
                ClassifiedsHttpApplication.ClassifiedsApplicationSettings.StorePhotosInDatabase = newSettings.StorePhotosInDatabase;
                // ... else: use the following directory to save uploaded Photos:
                ClassifiedsHttpApplication.ClassifiedsApplicationSettings.ServerPhotoUploadDirectory = newSettings.ServerPhotoUploadDirectory;


                // Maximum Number of Photos to Upload
                ClassifiedsHttpApplication.ClassifiedsApplicationSettings.MaxPhotosPerAd = newSettings.MaxPhotosPerAd;
                
                // Maximum Number of Days for which an Ad is active
                ClassifiedsHttpApplication.ClassifiedsApplicationSettings.MaxAdRunningDays = newSettings.MaxAdRunningDays;

                // Allow Users to edit their own Ads
                ClassifiedsHttpApplication.ClassifiedsApplicationSettings.AllowUsersToEditAds = newSettings.AllowUsersToEditAds;

                // Users to delete ads direclty in the database
                ClassifiedsHttpApplication.ClassifiedsApplicationSettings.AllowUsersToDeleteAdsInDB = newSettings.AllowUsersToDeleteAdsInDB;

                // Notifications to Administrators
                ClassifiedsHttpApplication.ClassifiedsApplicationSettings.AdminNotification = newSettings.AdminNotification;

                // Site Name
                ClassifiedsHttpApplication.ClassifiedsApplicationSettings.SiteName = newSettings.SiteName;

                // Contact Email Address for Site
                ClassifiedsHttpApplication.ClassifiedsApplicationSettings.SiteEmailAddress = newSettings.SiteEmailAddress;

                // Serialize to Xml Config File
                return SaveToXml(ClassifiedsHttpApplication.ClassifiedsApplicationSettings);
            }
        }

        private static SiteSettings LoadFromXml()
        {
            SiteSettings settings = null;

            HttpContext context = HttpContext.Current;
            if (context != null)
            {
                string configPath = context.Server.MapPath(XmlConfigFile);

                XmlSerializer xml = null;
                FileStream fs = null;

                bool success = false;
                int numAttempts = 0;

                while (!success && numAttempts < 2)
                {
                    try
                    {
                        numAttempts++;
                        xml = new XmlSerializer(typeof(SiteSettings));
                        fs = new FileStream(configPath, System.IO.FileMode.Open, System.IO.FileAccess.Read);
                        settings = xml.Deserialize(fs) as SiteSettings;
                        success = true;
                    }
                    catch (Exception x)
                    {
                        // if an exception is thrown, there might have been a sharing violation;
                        // we wait and try again (max: two attempts)
                        success = false;
                        System.Threading.Thread.Sleep(1000);
                        if (numAttempts == 2)
                            throw new Exception("The Site Configuration could not be loaded.", x);
                    }
                }

                if (fs != null)
                    fs.Close();
            }

            return settings;

        }

        public string GetXml()
        {
            StringBuilder result = new StringBuilder();
            StringWriter s = new StringWriter(result);
            try
            {
                XmlSerializer xml = new XmlSerializer(typeof(SiteSettings));
                xml.Serialize(s, this);
            }
            finally
            {
                s.Close();
            }
            
            return result.ToString();

        }

        private static bool SaveToXml(SiteSettings settings)
        {
            if (settings == null)
                return false;

            HttpContext context = HttpContext.Current;
            if (context == null)
                return false;

            string configPath = context.Server.MapPath(XmlConfigFile);

            XmlSerializer xml = null;
            System.IO.FileStream fs = null;

            bool success = false;
            int numAttempts = 0;

            while (!success && numAttempts < 2)
            {
                try
                {
                    numAttempts++;
                    xml = new XmlSerializer(typeof(SiteSettings));
                    fs = new FileStream(configPath, FileMode.Create, FileAccess.ReadWrite, FileShare.None);
                    xml.Serialize(fs, settings);
                    success = true;
                }
                catch
                {
                    // if an exception is thrown, there might have been a sharing violation;
                    // we wait and try again (max: two attempts)
                    success = false;
                    System.Threading.Thread.Sleep(1000);
                }
            }

            if (fs != null)
                fs.Close();

            return success;

        }
    }
}

