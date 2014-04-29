using Gumblr.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Gumblr.DataAccess
{
    public class MatchRepository : IMatchRepository
    {
        public IEnumerable<Match> GetMatches()
        {
            var staticMatchListPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Configuration\MatchList.csv");
            var result = File.ReadAllLines(staticMatchListPath).Select(x => ParseMatchLine(x));
            return result;
        }

        private Match ParseMatchLine(string aLine)
        {
            // times in UTC can be found here: http://www.espnfc.com/fixtures/_/league/fifa.world/fifa-world-cup?cc=4716
            var split = aLine.Split(',');

            var match = new Match
            {
                Group = split[0],
                StartTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.Parse(split[1]), TimeZoneInfo.Local),
                Venue = split[2],
                Host = split[3],
                Visitor = split[4],
            };
            return match;
        }
    }
}