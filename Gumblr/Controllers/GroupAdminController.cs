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
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Gumblr.Controllers
{
	[Authorize]
	[AuthorizedAdministrator]
	public class GroupAdminController : Controller
	{
		IMatchRepository mMatchRepository;
		IUserRepository mUserRepository;
		IIdentityManager mIdentityManager;
	    private LocalUserManager mLocalUserManager;

		public GroupAdminController(IMatchRepository aMatchRepository, IUserRepository aUserRepository, IIdentityManager aIdentityManager, ILoginRepository aLoginRepository, LocalUserManager.Factory aLocalUserManagerFactory)
		{
			mMatchRepository = aMatchRepository;
			mUserRepository = aUserRepository;
			mIdentityManager = aIdentityManager;
		    mLocalUserManager = aLocalUserManagerFactory(new UserStore<ApplicationUser>(), aLoginRepository, aUserRepository);
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
            try
            {
                foreach (var modelItem in aModel.Users)
                {
                    var user = new ApplicationUser() { UserName = modelItem.Name, EmailAddress = modelItem.EmailAddress, Id = Guid.NewGuid().ToString() };
                    var result = await mLocalUserManager.CreateAsync(user, modelItem.Password);
                }
            }
            catch (Exception es)
            {
                return Json(new { status = "fail" });
            }
            return Json(new { status = "success" });
	    }
	}
}