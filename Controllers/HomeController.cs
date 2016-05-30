using CanBuyWeb.Controllers;
using CanBuyWeb.Helper;
using CanBuyWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace IdentitySample.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {

            var model = db.Products.Select(x => new ProductIndex
            {
                ID = x.ID,
                Name = x.Name,
                MinPrice = x.MinPrice,
                MaxPrice = x.MaxPrice,
                ImageId = x.HomeImage.FirstOrDefault() != null ?  x.HomeImage.FirstOrDefault().ID :0,
                ImagePath = x.HomeImage.FirstOrDefault() != null ? x.HomeImage.FirstOrDefault().Image_Path : string.Empty,
               // Image = x.HomeImage.FirstOrDefault() != null ? x.HomeImage.FirstOrDefault().Image_Bytes : null
            });
            return View(model);
        }

        [Authorize]
        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}
