using System;
using System.Web;
using System.IO;
using System.Text;
using System.Drawing;
using System.Web.Mail;
using System.Collections;
using System.Web.Security;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Collections.Generic;
using AdsDataComponentTableAdapters;

using AspNet.StarterKits.Classifieds.Web;

namespace AspNet.StarterKits.Classifieds.BusinessLogicLayer
{
    public enum PhotoSize
    {
        None = 0,
        Small = 1,
        Medium = 2,
        Full = 3
    }

    public sealed class PhotosDB
    {
        private PhotosDB()
        {
        }

        public static AdsDataComponent.PhotosDataTable GetPhotosByAdId(int adId)
        {
            using (PhotosDataAdapter db = new PhotosDataAdapter())
            {
                return db.GetPhotosByAdId(adId);
            }
        }

        public static int InsertPhoto(int adId, byte[] bytesFull, byte[] bytesMedium, byte[] bytesSmall, bool useAsPreview)
        {
            int photoId = -1;
            if (bytesFull != null && bytesMedium != null && bytesSmall != null)
            {
                SiteSettings s = SiteSettings.GetSharedSettings();
                using (PhotosDataAdapter db = new PhotosDataAdapter())
                {
                    if (s.StorePhotosInDatabase)
                        photoId = Convert.ToInt32(db.InsertPhoto(adId, bytesFull, bytesMedium, bytesSmall, useAsPreview, DateTime.Now));
                    else
                    {
                        photoId = Convert.ToInt32(db.InsertPhoto(adId, null, null, null, useAsPreview, DateTime.Now));

                        string fullSizePath = GetFilePath(photoId, false, PhotoSize.Full);
                        string mediumSizePath = GetFilePath(photoId, false, PhotoSize.Medium);
                        string smallSizePath = GetFilePath(photoId, false, PhotoSize.Small);

                        WriteToFile(fullSizePath, bytesFull);
                        WriteToFile(mediumSizePath, bytesMedium);
                        WriteToFile(smallSizePath, bytesSmall);

                    }
                }
            }
            return photoId;
        }

        public static string GetFilePath(int photoId, bool forUrl, PhotoSize size)
        {
            string result = null;

            string filenameToken;
            if (size == PhotoSize.Full)
                filenameToken = "Lg";
            else if (size == PhotoSize.Medium)
                filenameToken = "Md";
            else
                filenameToken = "Sm";

            SiteSettings s = SiteSettings.GetSharedSettings();
            {
                if (forUrl)
                {
                    result = String.Format("{0}/{1}/{2}.{3}.jpg", ClassifiedsHttpApplication.SiteUrl, s.ServerPhotoUploadDirectory, photoId, filenameToken);
                }
                else
                {
                    HttpContext context = HttpContext.Current;
                    if (context != null)
                    {
                        string serverDirectory = context.Server.MapPath(s.ServerPhotoUploadDirectory);
                        string file = String.Format("{0}.{1}.jpg", photoId, filenameToken);
                        result = Path.Combine(serverDirectory, file);
                    }
                }
            }
            return result;

        }

        public static void RemovePhotosOfDeletedAds()
        {
            SiteSettings s = SiteSettings.GetSharedSettings();

            // must remove photos from file system before removing from DB
            using (PhotosDataAdapter db = new PhotosDataAdapter())
            {
                if (s.StorePhotosInDatabase)
                {
                    AdsDataComponent.PhotosDataTable deletedAdPhotos = db.GetPhotosByAdStatus((int)AdStatus.Deleted);
                    RemovePhotoFilesInTable(deletedAdPhotos);
                }

                db.RemovePhotosByAdStatus((int)AdStatus.Deleted);
            }

        }

        public static void RemovePhotosByAdId(int adId)
        {
            SiteSettings s = SiteSettings.GetSharedSettings();
            using (PhotosDataAdapter db = new PhotosDataAdapter())
            {
                if (s.StorePhotosInDatabase)
                {
                    AdsDataComponent.PhotosDataTable photos = db.GetPhotosByAdStatus((int)AdStatus.Deleted);
                    RemovePhotoFilesInTable(photos);
                }
            }
        }

        public static bool RemovePhotoById(int id)
        {
            bool result = false;

            using (PhotosDataAdapter db = new PhotosDataAdapter())
            {
                db.RemovePhoto(id);
            }

            SiteSettings s = SiteSettings.GetSharedSettings();
            if (s.StorePhotosInDatabase)
            {
                result = true;
            }
            {
                result = DeleteLocalPhotoFiles(id);
            }

            return result;
        }

        public static void SetAdPreviewPhoto(int adId, int photoId)
        {
            using (PhotosDataAdapter db = new PhotosDataAdapter())
            {
                db.SetAdPreviewPhoto(photoId, adId);
            }
        }

        public static byte[] ResizeImageFile(byte[] imageFile, PhotoSize size)
        {
            using (System.Drawing.Image original = System.Drawing.Image.FromStream(new MemoryStream(imageFile)))
            {
                int targetH, targetW;
                if (size == PhotoSize.Small || size == PhotoSize.Medium)
                {
                    // regardless of orientation,
                    // the *height* is constant for thumbnail images (UI constraint)

                    if (original.Height > original.Width)
                    {
                        if (size == PhotoSize.Small)
                            targetH = DefaultValues.FixedSmallImageHeight;
                        else
                            targetH = DefaultValues.FixedMediumImageHeight;

                        targetW = (int)(original.Width * ((float)targetH / (float)original.Height));
                    }
                    else
                    {
                        if (size == PhotoSize.Small)
                            targetW = DefaultValues.FixedSmallImageWidth;
                        else
                            targetW = DefaultValues.FixedMediumImageWidth;

                        targetH = (int)(original.Height * ((float)targetW / (float)original.Width));
                    }
                }
                else
                {
                    // for full preview, we scale proportionally according to orienation
                    if (original.Height > original.Width)
                    {
                        targetH = Math.Min(original.Height, DefaultValues.MaxFullImageSize);
                        targetW = (int)(original.Width * ((float)targetH / (float)original.Height));
                    }
                    else
                    {
                        targetW = Math.Min(original.Width, DefaultValues.MaxFullImageSize);
                        targetH = (int)(original.Height * ((float)targetW / (float)original.Width));
                    }
                }

                using (System.Drawing.Image imgPhoto = System.Drawing.Image.FromStream(new MemoryStream(imageFile)))
                {
                    // Create a new blank canvas.  The resized image will be drawn on this canvas.
                    using (Bitmap bmPhoto = new Bitmap(targetW, targetH, PixelFormat.Format24bppRgb))
                    {
                        bmPhoto.SetResolution(72, 72);

                        using (Graphics grPhoto = Graphics.FromImage(bmPhoto))
                        {
                            grPhoto.SmoothingMode = SmoothingMode.AntiAlias;
                            grPhoto.InterpolationMode = InterpolationMode.HighQualityBicubic;
                            grPhoto.PixelOffsetMode = PixelOffsetMode.HighQuality;
                            grPhoto.DrawImage(imgPhoto, new Rectangle(0, 0, targetW, targetH), 0, 0, original.Width, original.Height, GraphicsUnit.Pixel);

                            MemoryStream mm = new MemoryStream();
                            bmPhoto.Save(mm, System.Drawing.Imaging.ImageFormat.Jpeg);
                            return mm.GetBuffer();
                        }
                    }
                }
            }
        }

        public static byte[] GetPhotoBytesById(int photoId, PhotoSize size)
        {
            using (PhotosDataAdapter db = new PhotosDataAdapter())
            {
                return db.GetPhotoBytesById(photoId, (int)size) as byte[];
            }
        }

        private static bool DeleteLocalPhotoFiles(int photoId)
        {
            bool result = false;
            try
            {
                string fullPhotoPath = GetFilePath(photoId, false, PhotoSize.Full);
                string mediumPhotoPath = GetFilePath(photoId, false, PhotoSize.Medium);
                string smallPhotoPath = GetFilePath(photoId, false, PhotoSize.Small);

                DeleteFile(fullPhotoPath);
                DeleteFile(mediumPhotoPath);
                DeleteFile(smallPhotoPath);

                result = true;
            }
            catch
            {
                result = false;
            }

            return result;
        }

        private static void RemovePhotoFilesInTable(AdsDataComponent.PhotosDataTable photosToDelete)
        {
            if (photosToDelete != null)
            {
                for (int i = 0; i < photosToDelete.Rows.Count; i++)
                {
                    AdsDataComponent.PhotosRow photo = photosToDelete.Rows[i] as AdsDataComponent.PhotosRow;
                    DeleteLocalPhotoFiles(photo.Id);
                }
            }
        }

        private static void WriteToFile(string filename, byte[] bytes)
        {
            if (filename != null)
            {
                using (FileStream full = File.Open(filename, FileMode.Create))
                {
                    full.Write(bytes, 0, bytes.Length);
                    full.Flush();
                }
            }
        }

        private static void DeleteFile(string filename)
        {
            if (filename != null)
            {
                if (File.Exists(filename))
                    File.Delete(filename);
            }
        }
    }
}

