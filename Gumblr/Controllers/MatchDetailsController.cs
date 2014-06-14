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
    public class MatchDetailsController : Controller
    {
        IMatchStatisticsRepository mMatchStatisticsRepository;

        public MatchDetailsController(IMatchStatisticsRepository aMatchStatisticsRepository)
        {
            mMatchStatisticsRepository = aMatchStatisticsRepository;
        }

        public async Task<ActionResult> Index(string id)
        {
            var statistics = await mMatchStatisticsRepository.GetMatchStatistics(id);

            var model = new MatchDetailsModel
            {
                Match = statistics.Match,
                ExpectedResultByUsername = statistics.ExpectedResultByUsername,
                UserNamesByExpectedResult = GetUserNamesByExpectedResult(statistics.ExpectedResultByUsername),
            };

            return View(model);
        }

        private Dictionary<string, List<string>> GetUserNamesByExpectedResult(Dictionary<string, MatchResult> aExpectedResultByUsername)
        {
            var result = aExpectedResultByUsername
                .GroupBy(x => x.Value)
                .ToDictionary(x => x.Key.ToString(), x => x.Select(a => a.Key).ToList());

            return result;
        }
	}
}