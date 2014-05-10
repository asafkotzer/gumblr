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

            var currentBetsByMatchId = await GetCurrentBets(userId);
            var matches = (await mMatchRepository.GetMatches()).Take(4);
            var teams = matches
                .Select(x => x.Host)
                .Union(matches.Select(x => x.Visitor))
                .Distinct()
                .OrderBy(x => x);

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
                    else
                    {
                        bet.ExpectedResult = MatchResult.Unknown;
                    }

                    return bet;
                }),
                PossibleWinners = teams,
            };

            return View(model);
        }

        private async Task<Dictionary<string, MatchBet>> GetCurrentBets(string userId)
        {
            Dictionary<string, MatchBet> currentBetsByMatchId = new Dictionary<string, MatchBet>();
            var userBets = await mMatchBetRepository.GetUserBets(userId);
            if (userBets != null)
            {
                currentBetsByMatchId = userBets.Matches.ToDictionary(x => x.MatchId);
            }

            return currentBetsByMatchId;
        }

        [HttpPost]
        public async Task<ActionResult> PlaceBets(BettingModel aModel)
        {
            var userId = mIdentityManager.GetUserId(User);
            var currentBets = await mMatchBetRepository.GetUserBets(userId);

            try
            {
                mBettingModelValidator.ValidateModel(aModel, currentBets);
            }
            catch (BettingModelValidationExcpetion ex)
            {
                throw new HttpException(400, ex.Message);
            }

            await mMatchBetRepository.SetUserBet(userId, aModel);

            // returning a JSON for the client side to redirect (jQuery ajax requirement)
            return Json(new { status = "success" });
        }
    }
}
