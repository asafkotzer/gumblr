using Gumblr.Filters;
using System.Web;
using System.Web.Mvc;

namespace Gumblr
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new ErrorLoggingAttribute());
        }
    }
}
