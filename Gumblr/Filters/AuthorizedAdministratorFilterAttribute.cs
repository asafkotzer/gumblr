using System.Threading.Tasks;
using Gumblr.Account;
using Gumblr.DataAccess;
using Gumblr.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Autofac.Integration.Mvc;

namespace Gumblr.Filters
{
    public class AuthorizedAdministratorAttribute : ActionFilterAttribute
    {
        public IIdentityManager IdentityManager { get; set; }
        public IUserRepository UserRepository { get; set; }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            var userId = IdentityManager.GetUserId(filterContext.HttpContext.User);
            var user = Task.Run(() => UserRepository.GetUser(userId)).Result;
            if (user.Role != UserRole.Administrator)
            {
                throw new HttpException(401, "Only match administrators can access this page");
            }
        }
    }
}