using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gumblr.Models
{
    public class MatchBet : Match
    {
        public MatchBet()
        {

        }

        public MatchBet(Match aMatch) : base(aMatch)
        {
        }

        public MatchResult ExpectedResult { get; set; }
    }

    public class UserBets
    {
        public ApplicationUser User { get; set; }
        public IEnumerable<MatchBet> Bets { get; set; }
    }
}