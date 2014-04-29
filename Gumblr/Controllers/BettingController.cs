using Gumblr.DataAccess;
using Gumblr.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Gumblr.Storage;

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

        public async Task<ActionResult> PlaceBets()
        {
            var userId = User.Identity.GetUserId();

            Dictionary<string, MatchBet> currentBetsByMatchId = await GetCurrentBets(userId);
            var matches = mMatchRepository.GetMatches().Take(4);
            var model = new BettingModel
            {
                Matches = matches.Select(x =>
                {
                    MatchBet bet;
                    if (!currentBetsByMatchId.TryGetValue(x.GetMatchId(), out bet))
                    {
                        bet = new MatchBet(x);
                    }

                    return bet;
                })
            };
            return View(model);
        }

        private async Task<Dictionary<string, MatchBet>> GetCurrentBets(string userId)
        {
            Dictionary<string, MatchBet> currentBetsByMatchId = new Dictionary<string, MatchBet>();
            try
            {
                var userBets = await mMatchBetRepository.GetUserBets(userId);
                currentBetsByMatchId = (userBets).ToDictionary(x => x.GetMatchId());
            }
            catch (ItemDoesNotExitException) { }

            return currentBetsByMatchId;
        }

        [HttpPost]
        public async Task<ActionResult> PlaceBets(BettingModel aModel)
        {
            if (aModel.Matches.Any(x => x.StartTime > DateTime.UtcNow))
            {
                throw new HttpException(400, "A bet was made after the match started");
            }

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
