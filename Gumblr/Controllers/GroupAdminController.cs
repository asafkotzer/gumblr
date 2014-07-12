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
using Gumblr.BusinessLogic.Emails;

namespace Gumblr.Controllers
{
    [GumblrAuthorize]
	[AuthorizedAdministrator]
	public class GroupAdminController : Controller
	{
		IMatchRepository mMatchRepository;
		IUserRepository mUserRepository;
		IIdentityManager mIdentityManager;
	    LocalUserManager mLocalUserManager;
        IMatchResultsHandler mMatchResultsHandler;
        INewMatchUpdatesRepository mNewMatchUpdatesRepository;
        IEmailProvider mEmailProvider;

        public GroupAdminController(IMatchRepository aMatchRepository, IUserRepository aUserRepository, IIdentityManager aIdentityManager, ILoginRepository aLoginRepository, LocalUserManager.Factory aLocalUserManagerFactory, IMatchResultsHandler aMatchResultsHandler, INewMatchUpdatesRepository aNewMatchUpdatesRepository, IEmailProvider aEmailProvider)
		{
			mMatchRepository = aMatchRepository;
			mUserRepository = aUserRepository;
			mIdentityManager = aIdentityManager;
		    mLocalUserManager = aLocalUserManagerFactory(new UserStore<ApplicationUser>(), aLoginRepository, aUserRepository);
            mMatchResultsHandler = aMatchResultsHandler;
            mNewMatchUpdatesRepository = aNewMatchUpdatesRepository;
            mEmailProvider = aEmailProvider;
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
            throw new Exception("Are you sure you want to do that?");

            var filename = @"C:\Temp\Gumblr\Matches.csv";
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
			var matches = (await mMatchRepository.GetMatches()).Where(x => x.HasStarted);

			var model = new MatchesAdminModel() { Matches = matches };
			return View(model);
		}

		[HttpPost]
		public async Task<ActionResult> UpdateMatches(MatchesAdminModel aModel)
		{
            await Task.WhenAll(aModel.Matches.Select(x =>
            {
                return mMatchRepository.Update(x);
            }));

            var newMatches = await mMatchResultsHandler.UpdateNewMatches();

            return Json(new { status = "success", shouldRedirect = newMatches.NewMatches.Count() > 0, redirectUrl = string.Format("/GroupAdmin/PendingMatches/{0}", newMatches.Id) });
		}

        public async Task<ActionResult> PendingMatches(string id)
        {
            var newMatches = await mNewMatchUpdatesRepository.Get(id);
            return View(newMatches);
        }

        [HttpPost]
        public async Task<ActionResult> SendNewMatchUpdate(string id)
        {
            var newMatches = (await mNewMatchUpdatesRepository.Get(id)).NewMatches;

            if (newMatches.Count() > 0)
            {
                // update repository
                await Task.WhenAll(newMatches.Select(x => mMatchRepository.Update(x)));
                await SendNewMatchesEmail(newMatches);
            }

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

        private async Task SendNewMatchesEmail(IEnumerable<Match> aNewMatches)
        {
            var users = await mUserRepository.GetAllUsers();
            var sendEmailTasks = users.Select(u => SendNewMatchesEmail(u, aNewMatches));
            await Task.WhenAll(sendEmailTasks);
        }

        private async Task SendNewMatchesEmail(ApplicationUser aUser, IEnumerable<Match> aNewMatches)
        {
            var model = new NewMatchesModel { User = aUser, NewMatches = aNewMatches };
            var generator = new NewMatchesEmailGenerator(model);
            await mEmailProvider.Send(generator.GetMessage());
        }

	}
}