using CanBuyWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
namespace CanBuyWeb.Controllers
{
    public class BaseController : Controller
    {
        protected BaseController()
        {
            if (System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
            {
                UserId = System.Web.HttpContext.Current.User.Identity.GetUserId();
            }
        }

        protected canbuydbEntities db = new canbuydbEntities();

        protected string UserId { get; set; } 
        // GET: Base
        protected string getUserId()
        {
         var claimsIdentity = User.Identity as ClaimsIdentity;
         string userIdValue = string.Empty;
                if (claimsIdentity != null)
                {
                    // the principal identity is a claims identity.
                    // now we need to find the NameIdentifier claim
                    var userIdClaim = claimsIdentity.Claims
                        .FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);

                    if (userIdClaim != null)
                    {
                         userIdValue = userIdClaim.Value;
                    }
                }
                return userIdValue;
        }


    }
}