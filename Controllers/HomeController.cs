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


        [HttpPost]
        public object GetNext()
        {
            var limit = 20;
            var ip = System.Web.HttpContext.Current.Request.UserHostAddress;
            var CurrentDate = DateTime.Now;
            var Next = DateTime.Parse(CurrentDate.AddDays(1).ToShortDateString());
            var Prev = DateTime.Parse(CurrentDate.ToShortDateString());
            var count = db.Rotation.Where(o => o.IP == ip && (o.Created <= Next && o.Created > Prev)).Count();
            var cannext = count <= limit ? false : true;
            return Json(new { cannext=cannext });
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}
