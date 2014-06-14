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
using Gumblr.BusinessLogic;

namespace Gumblr.Controllers
{
    [Authorize]
    public class BettingController : Controller
    {
        IMatchRepository mMatchRepository;
        IMatchBetRepository mMatchBetRepository;
        IUserRepository mUserRepository;
        IIdentityManager mIdentityManager;
        IBettingModelValidator mBettingModelValidator;

        public BettingController(IMatchRepository aMatchRepository, IMatchBetRepository aMatchBetRepository, IUserRepository aUserRepository, IIdentityManager aIdentityManager, IBettingModelValidator aBettingModelValidator)
        {
            mMatchRepository = aMatchRepository;
            mMatchBetRepository = aMatchBetRepository;
            mUserRepository = aUserRepository;
            mIdentityManager = aIdentityManager;
            mBettingModelValidator = aBettingModelValidator;
        }

        public ActionResult Index()
        {
            return RedirectToAction("PlaceBets");
        }

        public async Task<ActionResult> PlaceBets()
        {
            var userId = mIdentityManager.GetUserId(User);

            var userBets = await mMatchBetRepository.GetUserBets(userId);
            var currentBetsByMatchId = GetCurrentBets(userBets);
            var matches = (await mMatchRepository.GetMatches());
            var teams = matches
                .Select(x => x.Host)
                .Union(matches.Select(x => x.Visitor))
                .Distinct()
                .OrderBy(x => x);

            var model = new BettingModel
            {
                Matches = matches.Select(x => GetCurrentBet(currentBetsByMatchId, x)),
                PossibleWinners = teams,
                Winner = userBets == null ? null : userBets.Winner,
            };

            return View(model);
        }

        private MatchBet GetCurrentBet(Dictionary<string, MatchBet> currentBetsByMatchId, Match x)
        {
            var bet = new MatchBet(x);

            MatchBet currentBet;
            if (currentBetsByMatchId.TryGetValue(x.MatchId, out currentBet))
            {
                bet.ExpectedResult = currentBet.ExpectedResult;
            }
            else
            {
                bet.ExpectedResult = MatchResult.Unknown;
            }

            return bet;
        }

        private Dictionary<string, MatchBet> GetCurrentBets(BettingModel aUserBets)
        {
            Dictionary<string, MatchBet> currentBetsByMatchId = new Dictionary<string, MatchBet>();
            if (aUserBets != null)
            {
                currentBetsByMatchId = aUserBets.Matches.ToDictionary(x => x.MatchId);
            }

            return currentBetsByMatchId;
        }

        [HttpPost]
        public async Task<ActionResult> PlaceBets(BettingModel aModel)
        {
            var userId = mIdentityManager.GetUserId(User);
            var username = User.Identity.Name;

            var currentBets = await mMatchBetRepository.GetUserBets(userId);

            try
            {
                mBettingModelValidator.ValidateModel(aModel, currentBets);
            }
            catch (BettingModelValidationExcpetion ex)
            {
                throw new HttpException(400, ex.Message);
            }

            await mMatchBetRepository.SetUserBet(userId, username, aModel);

            // returning a JSON for the client side to redirect (jQuery ajax requirement)
            return Json(new { status = "success" });
        }
    }
}
