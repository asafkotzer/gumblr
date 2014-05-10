using Gumblr.Account;
using Gumblr.DataAccess;
using Gumblr.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Gumblr.Filters;

namespace Gumblr.Controllers
{
    [Authorize]
    [AuthorizedAdministrator]
    public class MatchAdminController : Controller
    {
        IMatchRepository mMatchRepository;
        IUserRepository mUserRepository;
        IIdentityManager mIdentityManager;

        public MatchAdminController(IMatchRepository aMatchRepository, IUserRepository aUserRepository, IIdentityManager aIdentityManager)
        {
            mMatchRepository = aMatchRepository;
            mUserRepository = aUserRepository;
            mIdentityManager = aIdentityManager;
        }

        //
        // GET: /MatchAdmin/
        public async Task<ActionResult> Index()
        {
            var matches = (await mMatchRepository.GetMatches()).Take(4);

            var model = new MatchAdminModel() { Matches = matches };
            return View("EnterResults", model);
        }

        [HttpPost]
        public async Task<ActionResult> Update(MatchAdminModel aModel)
        {
            var userId = mIdentityManager.GetUserId(User);

            await Task.WhenAll(aModel.Matches.Select(x => mMatchRepository.Update(x)));

            return Json(new { status = "success" });
        }


	}
}