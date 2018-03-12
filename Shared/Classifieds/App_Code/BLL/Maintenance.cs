using System;
using System.Web;
using System.Web.Security;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mail;
using AdsDataComponentTableAdapters;
using StatsDataComponentTableAdapters;

using AspNet.StarterKits.Classifieds.Web;

namespace AspNet.StarterKits.Classifieds.BusinessLogicLayer
{

    public enum AdminNotificationSetting
    {
        None = 0,
        EachAd = 1,
        Hourly = 2,
        Daily = 3
    }

    /// <summary>
    /// Classifieds maintenance functions.
    /// </summary>
    public sealed class Maintenance
    {
        private Maintenance() { }

        public static void HourlyMaintenanceTimer(object state)
        {
            CheckAdExpirations();
            ProcessSummaryNotification();            
        }

        private static string GetAdminRecipients()
        {
            // Enhancement: Cache List
            StringBuilder list = new StringBuilder();
            string[] admins = Roles.GetUsersInRole(DefaultValues.AdministratorRole);
            foreach (string username in admins)
            {
                MembershipUser u = Membership.GetUser(username);
                list.Append(u.Email);
                list.Append(";");
            }
            return list.ToString();
        }

        public static void SendAdNotification(AdsDataComponent.AdsRow ad)
        {
            SiteSettings s = SiteSettings.GetSharedSettings();
            
            StringBuilder messageBody = new StringBuilder();
            
            messageBody.AppendFormat("A new Ad has just been to the Classifieds site '{0}':", s.SiteName);
            messageBody.AppendLine();

            messageBody.AppendLine();

            messageBody.Append("Title: ");
            messageBody.AppendLine(ad.Title);

            messageBody.Append("Category: ");
            messageBody.AppendLine(ad.CategoryName);

            messageBody.AppendLine();

            messageBody.AppendLine("For Details and Activation: ");
            messageBody.Append(ClassifiedsHttpApplication.SiteUrl);
            messageBody.AppendFormat("/EditAd.aspx?id={0}", ad.Id);

            messageBody.AppendLine();

            if ((AdStatus)ad.AdStatus != AdStatus.Activated)
            {
                messageBody.AppendLine("Status: The Ad is not yet active.");
            }
            else
            {
                messageBody.AppendLine("Status: The Ad was activated automatically.");
            }


            try
            {
                MailMessage m = new MailMessage(s.SiteEmailFromField, GetAdminRecipients());
                m.Subject = String.Format("New Ad posted: {0}", ad.Title);
                m.Body = messageBody.ToString();
                SmtpClient client = new SmtpClient();
                client.Send(m);
            }
            catch { }

        }

        private static void ProcessSummaryNotification()
        {
            SiteSettings s = SiteSettings.GetSharedSettings();
            DateTime lastNotification = ClassifiedsHttpApplication.LastNotificationSent;

            bool sendSummary = false;
            if (s.AdminNotification == AdminNotificationSetting.Hourly)
            {
                // the thread is called once an hour, so send the latest notifications
                sendSummary = true;
            }
            else if (s.AdminNotification == AdminNotificationSetting.Daily)
            {
                // check when the last notification was sent
                DateTime oneDayAgo = DateTime.Now.Subtract(TimeSpan.FromDays(-1));
                if (oneDayAgo >= lastNotification)
                {
                    sendSummary = true;
                }
            }

            if (sendSummary)
            {
                int numPendingNew = 0, numPendingTotal = 0;
                using (StatsDataAdapter db = new StatsDataAdapter())
                {
                    numPendingNew = Convert.ToInt32(db.CountAdsByStatus((int)AdStatus.ActivationPending, lastNotification));
                    numPendingTotal = Convert.ToInt32(db.CountAdsByStatus((int)AdStatus.ActivationPending, null));
                }
                SendSummaryMail(numPendingNew, numPendingTotal);
                ClassifiedsHttpApplication.LastNotificationSent = DateTime.Now;
            }
        }

        

        private static void SendSummaryMail(int numPendingNew, int numPendingTotal)
        {
            SiteSettings s = SiteSettings.GetSharedSettings();

            // only send mail when there are pending ads to report:
            //   if reporting each hour, only send mail when there were *new* ads within the hour
            //   if reporting each day, send mail if there are *any* unactivated ads

            if (numPendingNew > 0 || (s.AdminNotification == AdminNotificationSetting.Daily && numPendingTotal > 0))
            {
                StringBuilder messageBody = new StringBuilder();
                StringBuilder recipients = new StringBuilder();
                
                messageBody.AppendLine  ("There are new Ads waiting for activation:");

                messageBody.AppendLine();

                messageBody.AppendFormat("... {0} new Ads since the last notification was sent", numPendingNew);
                messageBody.AppendLine();

                messageBody.AppendFormat("... {0} total Ads waiting for activation", numPendingTotal);
                messageBody.AppendLine();

                messageBody.AppendLine();

                messageBody.AppendLine("You can access all pending activations at:");
                messageBody.Append(ClassifiedsHttpApplication.SiteUrl);
                messageBody.Append("/Admin/Activations.aspx");

                try
                {
                    MailMessage m = new MailMessage(s.SiteEmailFromField, GetAdminRecipients());
                    m.Subject = "Ad Notification Summary";
                    m.Body = messageBody.ToString();
                    SmtpClient client = new SmtpClient();
                    client.Send(m);
                }
                catch { }

            }
        }

        private static void CheckAdExpirations()
        {
            AdsDataComponent.AdsDataTable ads = null;
            using (AdsDataAdapter db = new AdsDataAdapter())
            {
                DateTime expiration = DateTime.Now;
                ads = db.GetExpiredAds(expiration, (int)AdStatus.Activated);
                foreach (AdsDataComponent.AdsRow ad in ads.Rows)
                {
                    db.UpdateAdStatus(ad.Id, (int)AdStatus.Inactive);
                }
            }
        }
    }
}