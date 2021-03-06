﻿using Gumblr.BusinessLogic.Emails;
using Gumblr.Filters;
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

        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("PlaceBets", "Betting");
            }

            return View();
        }

        public ActionResult RequestAnInvite()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> RequestInvite(RequestInviteModel aRequestInviteModel)
        {
            if (User.Identity.IsAuthenticated)
            {
                throw new HttpException(400, "This action is not for logged in users");
            }

            var generator = new InviteRequestEmailGenerator(aRequestInviteModel);
            await mEmailProvider.Send(generator.GetMessage());
            return RedirectToAction("Index", new { invite = "true" });
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