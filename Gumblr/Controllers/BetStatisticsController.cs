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
            return Json(statistics, JsonRequestBehavior.AllowGet);
        }
	}
}