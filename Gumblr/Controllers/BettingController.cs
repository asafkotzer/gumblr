using Gumblr.DataAccess;
using Gumblr.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;

namespace Gumblr.Controllers
{
    [Authorize]
    public class BettingController : Controller
    {
        IMatchRepository mMatchRepository;
        IMatchBetRepository mMatchBetRepository;
        IUserRepository mUserRepository;

        public BettingController(IMatchRepository aMatchRepository, IMatchBetRepository aMatchBetRepository, IUserRepository aUserRepository)
        {
            mMatchRepository = aMatchRepository;
            mMatchBetRepository = aMatchBetRepository;
            mUserRepository = aUserRepository;
        }

        public ActionResult Index()
        {
            return RedirectToAction("PlaceBets");
        }

        public ActionResult PlaceBets()
        {
            var matches = mMatchRepository.GetMatches().Take(4);
            var model = new BettingModel { Matches = matches.Select(x => new MatchBet(x)) };
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> PlaceBets(BettingModel aModel)
        {
            var userId = User.Identity.GetUserId();
            await mMatchBetRepository.SetUserBet(userId, aModel.Matches);

            // returning a JSON for the client side to redirect (jQuery ajax requirement)
            return Json(new { redirectUrl = Url.Action("BetSummary") });
        }

        public async Task<ActionResult> BetSummary()
        {
            var userId = User.Identity.GetUserId();
            var user = await mUserRepository.GetUser(userId);
            var userBets = await mMatchBetRepository.GetUserBets(userId);
            var model = new UserBetsModel { User = user, MatchBets = userBets };
            return View(model);
        }
    }
}
