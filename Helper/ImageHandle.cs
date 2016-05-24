using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;

namespace CanBuyWeb.Helper
{
    public class ImageHandle
    {
       public string path = "/UserFolder/Images/";

      
        public ImageHandle()
        {
          
            if (!Directory.Exists(HttpContext.Current.Server.MapPath(path))){
                Directory.CreateDirectory(HttpContext.Current.Server.MapPath(path));
            }
        }

        public void Save(Image img ,string filename)
        {
            var imgpath = path + filename + ".jpeg";
            img.Save(HttpContext.Current.Server.MapPath(imgpath));
        }
        public Image ResizeImage(Image img, int width, int height)
        {
            Bitmap b = new Bitmap(width, height);
            using (Graphics g = Graphics.FromImage((Image)b))
            {
                g.DrawImage(img, 0, 0, width, height);
            }

            return (Image)b;
        }

        public byte[] ImageToBuffer(Image Image, System.Drawing.Imaging.ImageFormat imageFormat)
        {
            if (Image == null) { return null; }
            byte[] data = null;
            using (MemoryStream oMemoryStream = new MemoryStream())
            {
                //建立副本
                using (Bitmap oBitmap = new Bitmap(Image))
                {
                    //儲存圖片到 MemoryStream 物件，並且指定儲存影像之格式
                    oBitmap.Save(oMemoryStream, imageFormat);
                    //設定資料流位置
                    oMemoryStream.Position = 0;
                    //設定 buffer 長度
                    data = new byte[oMemoryStream.Length];
                    //將資料寫入 buffer
                    oMemoryStream.Read(data, 0, Convert.ToInt32(oMemoryStream.Length));
                    //將所有緩衝區的資料寫入資料流
                    oMemoryStream.Flush();
                }
            }
            return data;
        }
    }


}