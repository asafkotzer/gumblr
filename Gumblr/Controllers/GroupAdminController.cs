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
	public class GroupAdminController : Controller
	{
		IMatchRepository mMatchRepository;
		IUserRepository mUserRepository;
		IIdentityManager mIdentityManager;

		public GroupAdminController(IMatchRepository aMatchRepository, IUserRepository aUserRepository, IIdentityManager aIdentityManager)
		{
			mMatchRepository = aMatchRepository;
			mUserRepository = aUserRepository;
			mIdentityManager = aIdentityManager;
		}

        public ActionResult Users()
        {
            return View();
        }

		public async Task<ActionResult> Matches()
		{
			var matches = (await mMatchRepository.GetMatches()).Take(4);

			var model = new MatchesAdminModel() { Matches = matches };
			return View(model);
		}

		[HttpPost]
		public async Task<ActionResult> UpdateMatches(MatchesAdminModel aModel)
		{
			var userId = mIdentityManager.GetUserId(User);

			await Task.WhenAll(aModel.Matches.Select(x => mMatchRepository.Update(x)));

			return Json(new { status = "success" });
		}

	    [HttpPost]
	    public async Task<ActionResult> UpdateUsers(UsersAdminModel aModel)
	    {
//            var users = aModel.Users.Select(x => new )
            return Json(new { status = "success" });
	    }
	}
}