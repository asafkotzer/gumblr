using Gumblr.DataAccess;
using Gumblr.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Gumblr.Controllers
{
    public class MatchAdminController : Controller
    {
        IMatchRepository mMatchRepository;

        public MatchAdminController(IMatchRepository aMatchRepository)
        {
            mMatchRepository = aMatchRepository;
        }

        //
        // GET: /MatchAdmin/
        public ActionResult Index()
        {
            var matches = mMatchRepository.GetMatches().Take(4);

            var model = new MatchAdminModel() { Matches = matches };
            return View("EnterResults", model);
        }
	}
}