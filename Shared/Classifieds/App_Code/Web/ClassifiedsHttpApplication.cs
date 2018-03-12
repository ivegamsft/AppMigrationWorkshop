using System;
using System.Web;
using System.Web.Security;
using System.IO;
using System.Configuration;
using System.Threading;

using AspNet.StarterKits.Classifieds.BusinessLogicLayer;

namespace AspNet.StarterKits.Classifieds.Web
{
    /// <summary>
    /// Summary description for ClassifiedsHttpApplication
    /// </summary>
    public class ClassifiedsHttpApplication : HttpApplication
    {

        private static object _settingsLock = new object();
        private static object _notificationLock = new object();

        private static SiteSettings _settings;
        private static DateTime _lastNotificationTime;
        private static string _siteUrl;

        public static SiteSettings ClassifiedsApplicationSettings
        {
            get
            {
                if (_settings == null)
                    ClassifiedsApplicationSettings = SiteSettings.LoadFromConfiguration();
                return _settings;
            }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("ClassifiedsApplicationSettings cannot be set to null");
                lock (_settingsLock)
                {
                    _settings = value;
                }
            }
        }

        public static DateTime LastNotificationSent
        {
            get
            {
                return _lastNotificationTime;
            }
            set
            {
                lock (_notificationLock)
                {
                    _lastNotificationTime = value;
                }
            }
        }

        public static string SiteUrl
        {
            get
            {
                return _siteUrl;
            }
        }

        public void Application_Start(Object sender, EventArgs e)
        {
            _siteUrl = GetSiteUrl();

            // set-up Settings
            ClassifiedsHttpApplication.ClassifiedsApplicationSettings = SiteSettings.LoadFromConfiguration();

            // set-up maintenance timer
            _lastNotificationTime = DateTime.Now;
            TimerCallback callback = new TimerCallback(Maintenance.HourlyMaintenanceTimer);
            Timer _hourlyTimer = new Timer(callback, null, TimeSpan.Zero, TimeSpan.FromHours(1.0));
        }

        private string GetSiteUrl()
        {
            string baseUrl = null;
            HttpContext c = HttpContext.Current;
            if (c != null)
            {
                string port = c.Request.ServerVariables["SERVER_PORT"];
                if (port == null || port.Equals("80") || port.Equals("443"))
                    port = String.Empty;
                else
                    port = ":" + port;

                string protocol = c.Request.ServerVariables["SERVER_PORT_SECURE"];

                if (protocol == null || protocol.Equals("0"))
                    protocol = "http://";
                else
                    protocol = "https://";

                baseUrl = protocol + c.Request.ServerVariables["SERVER_NAME"] + port + c.Request.ApplicationPath;
            }
            return baseUrl;
        }

        void Application_End(Object sender, EventArgs e)
        {
            
        }

        void Application_Error(Object sender, EventArgs e)
        {

        }
    }
}
