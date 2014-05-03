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
            var matchParser = new CsvMatchParser(File.ReadAllLines(staticMatchListPath));
            var result = matchParser.ParseMatches();
            return result;
        }

    }
}