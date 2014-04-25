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
    [Authorize]
    public class BettingController : Controller
    {
        IMatchRepository mRepository;
        IMatchBetRepository mMatchBetRepository;

        public BettingController(IMatchRepository aMatchRepository, IMatchBetRepository aMatchBetRepository)
        {
            mRepository = aMatchRepository;
            mMatchBetRepository = aMatchBetRepository;
        }

        public ActionResult Index()
        {
            return RedirectToAction("PlaceBets");
        }

        public ActionResult PlaceBets()
        {
            var matches = mRepository.GetMatches().Take(4);
            var model = new BettingModel { Matches = matches.Select(x => new MatchBet(x)) };
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> PlaceBets(BettingModel aModel)
        {
            //TODO: user id should be based on logged-in user
            await mMatchBetRepository.SetUserBet("UserId", aModel.Matches);
            return View();
        }
    }
}
