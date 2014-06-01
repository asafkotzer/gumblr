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
using Gumblr.BusinessLogic;

namespace Gumblr.Controllers
{
	[Authorize]
	[AuthorizedAdministrator]
	public class GroupAdminController : Controller
	{
		IMatchRepository mMatchRepository;
		IUserRepository mUserRepository;
		IIdentityManager mIdentityManager;
	    LocalUserManager mLocalUserManager;
        IMatchResultsHandler mTournamentGenerator;

        public GroupAdminController(IMatchRepository aMatchRepository, IUserRepository aUserRepository, IIdentityManager aIdentityManager, ILoginRepository aLoginRepository, LocalUserManager.Factory aLocalUserManagerFactory, IMatchResultsHandler aTournamentGenerator)
		{
			mMatchRepository = aMatchRepository;
			mUserRepository = aUserRepository;
			mIdentityManager = aIdentityManager;
		    mLocalUserManager = aLocalUserManagerFactory(new UserStore<ApplicationUser>(), aLoginRepository, aUserRepository);
            mTournamentGenerator = aTournamentGenerator;
		}

        public async Task<ActionResult> Users(string email, string username)
        {
            ViewBag.Email = email;
            ViewBag.Username = username;
            ViewBag.Password = GenerateSuggestedPassword(username);
            return View();
        }

        private string GenerateSuggestedPassword(string username)
        {
            if (username == null) username = string.Empty;

            var password = "";
            var words = username.Split(' ');
            if (words.Length > 1 && words.All(x => x.Length > 0))
            {
                password = new string(words.Select(x => x.First()).ToArray()) + "-" + Guid.NewGuid().ToString().Split('-').Skip(1).First();
            }
            else if (words.Length > 0 && words.First().Length > 1)
            {
                password = new string(words.First().Take(2).ToArray()) + "-" + Guid.NewGuid().ToString().Split('-').Skip(1).First();
            }
            else
            {
                password = Guid.NewGuid().ToString().Split('-').First();
            }

            return password;
        }

        public async Task<ActionResult> MatchUpload()
        {
            var filename = @"C:\Temp\Gumblr\Matches-Fake.csv";
            var parser = new CsvMatchParser(System.IO.File.ReadAllLines(filename));
            var matches = parser.ParseMatches();
            foreach (var match in matches)
            {
                await mMatchRepository.Create(match);
            }

            return Json("OK", JsonRequestBehavior.AllowGet);
        }

		public async Task<ActionResult> Matches()
		{
			var matches = (await mMatchRepository.GetMatches());

			var model = new MatchesAdminModel() { Matches = matches };
			return View(model);
		}

		[HttpPost]
		public async Task<ActionResult> UpdateMatches(MatchesAdminModel aModel)
		{
			var userId = mIdentityManager.GetUserId(User);

			await Task.WhenAll(aModel.Matches.Select(x => 
            {
                x.IsComplete = true;
                return mMatchRepository.Update(x);
            }));

            // generate new matches if needed
            await mTournamentGenerator.UpdateNewMatches();

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