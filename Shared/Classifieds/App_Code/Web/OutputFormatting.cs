using System;
using System.IO;
using System.Configuration;
using System.Web;
using AspNet.StarterKits.Classifieds.BusinessLogicLayer;

namespace AspNet.StarterKits.Classifieds.Web
{
    public sealed class OutputFormatting
    {
        private OutputFormatting()
        {
        }

        public static string AdStatusToString(object adStatus)
        {
            return AdStatusToString((AdStatus)(int)adStatus);
        }

        public static string AdStatusToString(AdStatus adStatus)
        {
            switch (adStatus)
            {
                case AdStatus.Activated:
                    return "Active";
                case AdStatus.ActivationPending:
                    return "Activation Pending";
                case AdStatus.Inactive:
                    return "Inactive / Expired";
                case AdStatus.Deleted:
                    return "Deleted";
                default:
                    return "(other)";
            }
        }

        public static string AdTypeToString(object adType)
        {
            return AdTypeToString((AdType)(int)adType);
        }

        public static string AdTypeToString(AdType adType)
        {
            switch (adType)
            {
                case AdType.ForSale:
                    return "For Sale";
                case AdType.Wanted:
                    return "Wanted";
                default:
                    return "(other)";
            }
        }

    }
}