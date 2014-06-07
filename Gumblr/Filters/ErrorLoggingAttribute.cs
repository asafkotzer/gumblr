using Gumblr.Helpers.Logging;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Gumblr.Filters
{
    public class ErrorLoggingAttribute : FilterAttribute, IExceptionFilter
    {
        public void OnException(ExceptionContext filterContext)
        {
            var logger = LogManager.GetLogger("TableLogger");

            var message = new LogMessage
            {
                TimeOccurred = DateTime.UtcNow,
                Username = filterContext.HttpContext.User.Identity.Name,
                Controller = filterContext.RouteData.Values["controller"].ToString(),
                Action = filterContext.RouteData.Values["action"].ToString(),
            };

            logger.Error(message, filterContext.Exception);
        }
    }
}