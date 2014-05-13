using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Gumblr.Controllers
{
    public class HomeController : Controller
    {
        public async Task<ActionResult> Index()
        {
            
            //TODO: also insert the user and Login!









            //var table = new Gumblr.Storage.Azure.TableDataContext();
            //var fs = new Gumblr.Storage.FileSystemProvider(new Gumblr.Storage.JsonSerializer());
            //var matchDescriptors = await fs.List("Matches");
            //var context = new Gumblr.Storage.Azure.TableDataContext();
            //foreach (var descriptor in matchDescriptors)
            //{
            //    var match = await fs.Read<Gumblr.Models.Match1>("Matches", descriptor.Key);
            //    var newMatch = new Gumblr.Models.Match()
            //    {
            //        Group = match.Group,
            //        Host = match.Host,
            //        StartTime = match.StartTime,
            //        Venue = match.Venue,
            //        Visitor = match.Visitor,
            //        ActualResult = match.ActualResult,
            //        Stage = match.Stage,
            //    };

            //    var key = Path.GetFileNameWithoutExtension(descriptor.Key);
            //    await context.Create("Matches", key, newMatch);
            //}
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}