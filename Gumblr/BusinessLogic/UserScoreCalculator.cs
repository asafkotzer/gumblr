using Gumblr.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Gumblr.BusinessLogic
{
    public interface IUserScoreCalculator 
    {
        UserScore CalculateScore(IEnumerable<Match> aMatches, BettingModel aMatchBets);
    }

    public class UserScoreCalculator : IUserScoreCalculator
    {
        public UserScore CalculateScore(IEnumerable<Match> aMatches, BettingModel aBet)
        {
            // aBet could be null
            return new UserScore() { Score = 9 };
        }
    }
}