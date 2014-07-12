using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;

namespace Gumblr.Filters
{
    public class GumblrAuthorizeAttribute : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);

            var userId = filterContext.HttpContext.User.Identity.GetUserId();
            if (userId == "4d80f537-f5e4-4a6b-a31a-5c28e2a7d278")
            {
                throw new HttpException(403, "Blocked user");
            }
        }
    }
}