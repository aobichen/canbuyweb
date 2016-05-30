using CanBuyWeb.Helper;
using CanBuyWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;



namespace CanBuyWeb.Controllers
{
    public class ProductController : BaseController
    {
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult Create(ProductCreate Model)
        {
            var chkimg = 0;
            var imgList = new List<int>();
            if (Model.MaxPrice <= Model.MinPrice)
            {
                ModelState.AddModelError("", "最高價不應該低於最低價");
                return View();
            }
            try
            {
                var u = UserId;

                var product = db.Products.Add(new Products { Name = Model.Name, Description = Model.description, MinPrice = Model.MinPrice, MaxPrice = Model.MaxPrice, Enable = true, UserId = u });

                if (Request["chkimg"] != null)
                {
                    chkimg = int.Parse( Request["chkimg"].ToString());
                }

                if (Request["hidimages"] != null)
                {
                    imgList = Request["hidimages"].ToString().Split(',').Select(Int32.Parse).ToList();
                }

                var tempimg = (from temp in db.temp_image where imgList.Contains(temp.ID) select temp).ToList();

                

                foreach (var temp in tempimg)
                {
                    if (temp.ID == chkimg)
                    {
                        db.Product_Images.Add(new Product_Images
                        {
                            Image_Byte = temp.image_Byte,
                            Image_Path = temp.image_Path,
                            IsPrimary = true,
                            Product_Id = product.ID
                        });


                        var image_handel = new ImageHandle();
                        var newImage = image_handel.BufferToImage(temp.image_Byte);
                        var img = image_handel.ResizeImage(newImage, 500, 600);
                        var filename = Guid.NewGuid().ToString();
                        var imagpath = image_handel.path + filename + ".jpeg";
                        img.Save(Server.MapPath(imagpath));
                        db.HomeImage.Add(new HomeImage
                        {
                            Image_Bytes = image_handel.ImageToBuffer(img, System.Drawing.Imaging.ImageFormat.Jpeg),
                            Image_Path = imagpath,
                           Product_Id = product.ID
                        });
                    }
                    else
                    {
                        db.Product_Images.Add(new Product_Images
                        {
                            Image_Byte = temp.image_Byte,
                            Image_Path = temp.image_Path,
                            IsPrimary = false,
                            Product_Id = product.ID,
                        });
                    }
                }

                db.SaveChanges();
            }
            catch
            {
                return View();
            }
          return  RedirectToAction("Create");
        }

        [Authorize]
        public ActionResult Edit(int Id)
        {
            
            var product = db.Products.Where(o => o.ID == Id).FirstOrDefault();

            var model = new ProductEdit();
            model.ID = product.ID;
            model.Name = product.Name;
            model.MinPrice = product.MinPrice;
            model.MaxPrice = product.MaxPrice;
            model.Images = product.Product_Images.ToList();
            model.description = product.Description;

           
            return View(model);
        }

        [Authorize]
        [HttpPost]
        public ActionResult Edit(ProductEdit model)
        {
            var chkimg = 0;
            var imgList = new List<int>();


            var product = db.Products.Where(o => o.ID == model.ID).FirstOrDefault();

            //db.Product_Images.RemoveRange(db.Product_Images.Where(o=>o.Product_Id == product.ID));

            product.Description = model.description;
            product.MaxPrice = model.MaxPrice;
            product.MinPrice = model.MinPrice;
            product.Name = model.Name;
            var b = Request["chkimg"].ToString();
            if (Request["chkimg"] != null)
            {
                chkimg = int.Parse(Request["chkimg"].ToString());
            }

            var a = Request["hidimages"].ToString();

            if (Request["hidimages"] != null)
            {
                imgList = Request["hidimages"].ToString().Split(',').Select(Int32.Parse).ToList();
            }

            var tempimg = (from temp in db.temp_image where imgList.Contains(temp.ID) select temp).ToList();



            foreach (var temp in tempimg)
            {
                if (temp.ID == chkimg)
                {
                    db.Product_Images.Add(new Product_Images
                    {
                        Image_Byte = temp.image_Byte,
                        Image_Path = temp.image_Path,
                        IsPrimary = true,
                        Product_Id = product.ID
                    });
                }
                else
                {
                    db.Product_Images.Add(new Product_Images
                    {
                        Image_Byte = temp.image_Byte,
                        Image_Path = temp.image_Path,
                        IsPrimary = false,
                        Product_Id = product.ID,
                    });
                }
            }

            db.SaveChanges();

            return RedirectToAction("Edit", new { id = model.ID });
        }
        


        public ActionResult List()
        {
            var model = db.Products.Where(o => o.UserId == UserId).ToList();
            return View(model);
        }
        // GET: Product
        public ActionResult Index()
        {
            var model = db.Products.Select(x => new ProductIndex
            {
                Name = x.Name,
                MinPrice = x.MinPrice,
                MaxPrice = x.MaxPrice,
                ImageId = x.Product_Images.Where(o=>o.IsPrimary).FirstOrDefault().ID,
                ImagePath = x.Product_Images.Where(o => o.IsPrimary).FirstOrDefault().Image_Path
            });
            return View(model);
        }
    }
}