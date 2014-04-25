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
}