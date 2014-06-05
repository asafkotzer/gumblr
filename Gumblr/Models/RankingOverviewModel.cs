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
        public int MaxScore { get; set; }
        public int MinScore { get; set; }
    }

    public class UserScore
    {
        public int Score { get { return ScoreElements.Sum(x => x.Value); } }
        public List<ScoreElement> ScoreElements { get; set; }

        public UserScore()
        {
            ScoreElements = new List<ScoreElement>();
        }
    }

    public class ScoreElement
    {
        public string Title { get; set; }
        public int Value { get; set; }
    }
}