using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gumblr.Models
{
    public class UserBetsModel
    {
        public ApplicationUser User { get; set; }
        public IEnumerable<MatchBet> MatchBets { get; set; }
    }
}