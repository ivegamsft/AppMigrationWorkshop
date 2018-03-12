<%@ WebHandler Language="C#" Class="AspNet.StarterKits.Classifieds.Web.PhotoDisplay" %>

using System;
using System.Web;
using AspNet.StarterKits.Classifieds.BusinessLogicLayer;


namespace AspNet.StarterKits.Classifieds.Web
{
    public class PhotoDisplay : IHttpHandler
    {
        public const string QueryStringFullSize = "full";
        public const string QueryStringMediumSize = "medium";

        public void ProcessRequest(HttpContext context)
        {

            HttpResponse Response = context.Response;
            HttpRequest Request = context.Request;

            int photoId = 0;
            string photoIdQs = Request.QueryString["photoid"];
            if (photoIdQs != null && Int32.TryParse(photoIdQs, out photoId))
            {
                // checking if a particular size was requested in the querystring (otherwise, small is default)
                string sizeQs = Request.QueryString["size"];
                PhotoSize size = PhotoSize.Small;
                if (sizeQs != null)
                {
                    if (sizeQs.Equals(QueryStringFullSize))
                        size = PhotoSize.Full;
                    else if (sizeQs.Equals(QueryStringMediumSize))
                        size = PhotoSize.Medium;
                }

                if (SiteSettings.GetSharedSettings().StorePhotosInDatabase)
                {
                    byte[] photo = PhotosDB.GetPhotoBytesById(photoId, size);
                    if (photo != null)
                    {
                        Response.ContentType = "image/jpeg";
                        Response.Cache.SetCacheability(HttpCacheability.Public);
                        Response.BufferOutput = false;
                        Response.OutputStream.Write(photo, 0, photo.Length);
                    }
                }
                else
                {
                    string url = PhotosDB.GetFilePath(photoId, true, size);
                    Response.Redirect(url, true);
                }
            }
        }

        public bool IsReusable
        {
            get
            {
                return true;
            }
        }

    }
}