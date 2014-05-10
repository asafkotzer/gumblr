using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gumblr.Models
{
    public class RankingOverviewModel
    {
        public Dictionary<string, UserScore> ScoreByUserId { get; set; }
        public IEnumerable<ApplicationUser> Users { get; set; }
    }

    public class UserScore
    {
        public int Score { get; set; }
    }
}