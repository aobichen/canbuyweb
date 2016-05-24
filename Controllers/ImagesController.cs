using CanBuyWeb.Helper;
using CanBuyWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CanBuyWeb.Controllers
{
    public class ImagesController : BaseController
    {
        [HttpPost]
        public object getImages(ImageVM img)
        {
            var id = int.Parse(img.id);
            var image = (from images in db.Product_Images
                          where images.Product_Id == id
                         select new ImageVM
                          {
                              id = images.ID.ToString(),
                              path = images.Image_Path,
                              isTemp = false
                          }).ToList();
            return Json(image);
        }

        public ActionResult UploadFile(List<Product_Images> model=null)
        {
            return View("Image_Upload",model);
        }


        [HttpPost]
        public object DeleteImage(ImageVM img)
        {
           
            var ID = int.Parse(img.id);
            var tempimg = db.temp_image.Where(o => o.ID == ID).FirstOrDefault();
            tempimg.deleted = true;
           
            db.SaveChanges();
            return Json(new {message="true"});
        }
 

        [Authorize]
        [HttpPost]
        public object UploadImage()
        {
            var files = Request.Files;
            var idList = new List<ImageVM>();
            for (var i = 0; i < files.Count; i++)
            {
                var name = files[i].FileName;
                var imghandle = new ImageHandle();
                if (MatchImageFromImag(System.IO.Path.GetExtension(name)))
                {
                    var filename = Guid.NewGuid().ToString();
                    var imagpath = imghandle.path + filename + ".jpeg";
                    var postedFile = files[i].InputStream;
                    var image = System.Drawing.Image.FromStream(postedFile);
                    var newImage = imghandle.ResizeImage(image, 500, 600);

                    var tempimg = db.temp_image.Add(new temp_image
                    {
                        image_Byte = imghandle.ImageToBuffer(newImage, System.Drawing.Imaging.ImageFormat.Jpeg),
                        userId = UserId,
                        image_Path = imagpath,
                        created = DateTime.Now,
                        deleted = false
                    });
                    newImage.Save(Server.MapPath(imagpath));
                    db.SaveChanges();
                    idList.Add(new ImageVM { id = tempimg.ID.ToString(), path = tempimg.image_Path, isTemp = true });
                }
            }

            return Json(idList);
        }


        public object  DeleteProductImage(int id)
        {
            var image = db.Product_Images.Where(o => o.ID == id).FirstOrDefault();
            db.Product_Images.Remove(image);
            db.SaveChanges();
            //return View("Edit","Product", new { id = image .Product_Id});
            return Json(new { message = true},JsonRequestBehavior.AllowGet);
        }

        public bool MatchImageFromImag(string extents)
        {
            var Match = false;
            switch (extents)
            {
                case ".jpg":
                    Match = true;
                    break;
                case ".png":
                    Match = true;
                    break;
            }

            return Match;
        }

        public ActionResult Image_Upload(){
            return View();
        }

        // GET: Images
        public ActionResult Index()
        {
            return View();
        }
    }
}