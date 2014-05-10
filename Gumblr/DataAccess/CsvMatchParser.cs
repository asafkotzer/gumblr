using Gumblr.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gumblr.DataAccess
{
    public interface IMatchParser
    {
        IEnumerable<Match> ParseMatches();
    }

    public class CsvMatchParser : IMatchParser
    {
        IEnumerable<string> mLines;

        public CsvMatchParser(IEnumerable<string> aLines)
        {
            mLines = aLines;
        }

        public IEnumerable<Match> ParseMatches()
        {
            return mLines.Select(x => ParseMatchLine(x));
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

            MatchResult actualResult = MatchResult.Unknown;
            if (split.Length > 5)
            {
                Enum.TryParse<MatchResult>(split[5], true, out actualResult);
            }
            match.ActualResult = actualResult;

            MatchStage stage = MatchStage.Qualifying;   // not a valid state really, it's before the tournament
            if (split.Length > 6)
            {
                Enum.TryParse<MatchStage>(split[6], true, out stage);
            }
            match.Stage = stage;

            return match;
        }

    }
}