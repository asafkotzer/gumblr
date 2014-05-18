using Gumblr.Helpers;
using Gumblr.Models;
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
        IEmailProvider mEmailProvider;
        IConfigurationRetriever mConfigurationRetriever;

        public HomeController(IEmailProvider aEmailProvider, IConfigurationRetriever aConfigurationRetriever)
        {
            mEmailProvider = aEmailProvider;
            mConfigurationRetriever = aConfigurationRetriever;
        }

        public async Task<ActionResult> Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> RequestInvite(RequestInviteModel aRequestInviteModel)
        {
            var generator = new InviteRequestEmailGenerator(aRequestInviteModel);
            var message = new EmailMessage
            {
                ToAddress = "asafkoter@gmail.com",
                ToName = "Shachar",
                Content = generator,
                Subject = generator, 
            };

            await mEmailProvider.Send(message);
            return Json(new { result = "success" });
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