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
    public class BetStatisticsController : Controller
    {
        IMatchStatisticsRepository mMatchStatisticsRepository;

        public BetStatisticsController(IMatchStatisticsRepository aMatchStatisticsRepository)
        {
            mMatchStatisticsRepository = aMatchStatisticsRepository;
        }

        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> Match(string id)
        {
            var statistics = await mMatchStatisticsRepository.GetMatchStatistics(id);

            // Dummy data:
            //var statistics = new MatchStatisticsModel
            //{
            //    Match = new Match { Host = "England", Visitor = "France", StartTime = DateTime.UtcNow.Date },
            //    ExpectedResultByUserId = new Dictionary<string, MatchResult> 
            //    { 
            //        { "user1", MatchResult.Host },
            //        { "user2", MatchResult.Host },
            //        { "user3", MatchResult.Host },
            //        { "user4", MatchResult.Host },
            //        { "user5", MatchResult.Draw },
            //        { "user6", MatchResult.Visitor },
            //        { "user7", MatchResult.Visitor },
            //        { "user8", MatchResult.Visitor },
            //    },
            //};

            return Json(statistics, JsonRequestBehavior.AllowGet);
        }
	}
}