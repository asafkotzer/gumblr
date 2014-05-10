using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gumblr.Models
{
    public class BettingModel
    {
        public IEnumerable<MatchBet> Matches { get; set; }
        public IEnumerable<string> PossibleWinners { get; set; }
        public DateTime WinnerBetDeadline { get { return Matches.Min(x => x.StartTime); } }
        public string Winner { get; set; }
    }
}