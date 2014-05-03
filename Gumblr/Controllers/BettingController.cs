using Gumblr.DataAccess;
using Gumblr.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Gumblr.Storage;
using Gumblr.Account;

namespace Gumblr.Controllers
{
    [Authorize]
    public class BettingController : Controller
    {
        IMatchRepository mMatchRepository;
        IMatchBetRepository mMatchBetRepository;
        IUserRepository mUserRepository;
        IIdentityManager mIdentityManager;

        public BettingController(IMatchRepository aMatchRepository, IMatchBetRepository aMatchBetRepository, IUserRepository aUserRepository, IIdentityManager aIdentityManager)
        {
            mMatchRepository = aMatchRepository;
            mMatchBetRepository = aMatchBetRepository;
            mUserRepository = aUserRepository;
            mIdentityManager = aIdentityManager;
        }

        public ActionResult Index()
        {
            return RedirectToAction("PlaceBets");
        }

        public async Task<ActionResult> PlaceBets()
        {
            var userId = mIdentityManager.GetUserId(User);

            var currentBetsByMatchId = await GetCurrentBets(userId);
            var matches = mMatchRepository.GetMatches().Take(4);
            var model = new BettingModel
            {
                Matches = matches.Select(x =>
                {
                    var bet = new MatchBet(x);

                    MatchBet currentBet;
                    if (currentBetsByMatchId.TryGetValue(x.MatchId, out currentBet))
                    {
                        bet.ExpectedResult = currentBet.ExpectedResult;
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
                currentBetsByMatchId = (userBets).ToDictionary(x => x.MatchId);
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

            var userId = mIdentityManager.GetUserId(User);
            await mMatchBetRepository.SetUserBet(userId, aModel.Matches);

            // returning a JSON for the client side to redirect (jQuery ajax requirement)
            return Json(new { redirectUrl = Url.Action("BetSummary") });
        }

        public async Task<ActionResult> BetSummary()
        {
            var userId = mIdentityManager.GetUserId(User);
            var user = await mUserRepository.GetUser(userId);
            var userBets = await mMatchBetRepository.GetUserBets(userId);
            var model = new UserBetsModel { User = user, MatchBets = userBets };
            return View(model);
        }
    }
}
