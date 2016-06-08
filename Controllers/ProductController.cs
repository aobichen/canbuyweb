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
                            Image_Bytes = image_handel.ImageToBuffer(newImage, System.Drawing.Imaging.ImageFormat.Jpeg),
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

        public ActionResult Detail(int Id)
        {
            var model = db.Products.Where(o => o.ID == Id).Select(x => new ProductDetail
            {
                ID = x.ID,
                description = x.Description,
                MaxPrice = x.MaxPrice,
                MinPrice = x.MinPrice,
                Name = x.Name,
                Images = x.Product_Images.ToList()
            }).FirstOrDefault();
            return View(model);
        }


        

        [HttpPost]
        public object GetPrice(int id)
        {
            var p = db.Products.Where(o => o.ID == id).FirstOrDefault();
            var min = int.Parse(p.MinPrice.ToString());
            var max = int.Parse(p.MaxPrice.ToString());
            Random rnd = new Random();
            var price = rnd.Next(min, max);
            var unLoginLimit = 20;
            var LoginLimit = 50;
           
             var count = 0;
            var ip = System.Web.HttpContext.Current.Request.UserHostAddress;
            var cannext = true;
            if (!User.Identity.IsAuthenticated)
            {
                var Rotation = db.Rotation.Add(new Rotation { Created = DateTime.Now, IP = ip, ProductId = id });
                db.SaveChanges();
                count = db.Rotation.Where(o => o.IP == ip && o.Created <= DateTime.Now).Count();
                cannext = count >= unLoginLimit ? false : true;
                if (Session[ip] == null)
                {
                    Session[ip] = new ProductOrderSession { Price = price, ProductId = id };
                }
            }
            else
            {

                var Rotation = db.Rotation.Add(new Rotation { Created = DateTime.Now, IP = ip, UserId = Convert.ToInt32(UserId), ProductId = Convert.ToInt32(id) });
                db.SaveChanges();
                count = db.Rotation.Where(o => o.IP == ip && o.Created <= DateTime.Now && o.ProductId == id).Count();
                cannext = count >= LoginLimit ? false : true;
                if (Session[UserId] == null)
                {
                    Session[UserId] = new ProductOrderSession { Price = price, ProductId = id };
                }
            }

            if (!cannext){
                 return Json(new { price = 999999, success = false,message="加入會員可以有更多機會喔!", cannext = cannext });
            }
           
            return Json(new { price = price, success = true, cannext = cannext });
        }
    }
}