using Gumblr.BusinessLogic;
using Gumblr.DataAccess;
using Gumblr.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Gumblr.Controllers
{
    public class RankingController : Controller
    {
        IMatchBetRepository mMatchBetRepository;
        IMatchRepository mMatchRepository;
        IUserRepository mUserRepository;
        IUserScoreCalculator mUserScoreCalculator;

        public RankingController(IMatchBetRepository aMatchBetRepository, IUserRepository aUserRepository, IMatchRepository aMatchRepository, IUserScoreCalculator aUserScoreCalculator)
        {
            mMatchBetRepository = aMatchBetRepository;
            mUserRepository = aUserRepository;
            mMatchRepository = aMatchRepository;
            mUserScoreCalculator = aUserScoreCalculator;
        }

        public async Task<ActionResult> Index()
        {
            var users = await mUserRepository.GetAllUsers();
            var getBetsTasks = users.Select(u => mMatchBetRepository.GetUserBets(u.Id).ContinueWith(t => new UserBets() { User = u, Bets = t.Result}));
            var allUsersBets = await Task.WhenAll(getBetsTasks);
            var actualResults = await mMatchRepository.GetMatches();

            var scoreByUser = allUsersBets.ToDictionary(x => x.User.Id, x => mUserScoreCalculator.CalculateScore(actualResults, x.Bets));
            var model = new RankingOverviewModel() { ScoreByUserId = scoreByUser, Users = users };
            return View(model);
        }
    }
}