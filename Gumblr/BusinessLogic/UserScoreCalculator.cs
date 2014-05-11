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
        UserScore CalculateScore(IEnumerable<Match> aMatchesWithActualResults, FinalResultsModel aFinalResultsModel, BettingModel aMatchBets);
    }

    public class UserScoreCalculator : IUserScoreCalculator
    {
        static readonly int CorrectWinner_Value = 10;
        static readonly int CorrectMatchBet_GroupStage_Value = 1;
        static readonly int CorrectMatchBet_TournamentStage_Value = 2;

        public UserScore CalculateScore(IEnumerable<Match> aMatchesWithActualResults, FinalResultsModel aFinalResultsModel, BettingModel aBet)
        {
            var score = new UserScore() { Score = 0 };
            
            if (aBet == null)
            {
                return score;
            }

            if (aFinalResultsModel != null && aFinalResultsModel.Winner == aBet.Winner)
            {
                score.Score += CorrectWinner_Value;
            }

            var betByMatchId = aBet.Matches.ToDictionary(x => x.MatchId);
            foreach (var match in aMatchesWithActualResults)
            {
                MatchBet matchBet;
                if (betByMatchId.TryGetValue(match.MatchId, out matchBet) && matchBet.ExpectedResult == match.ActualResult)
                {
                    score.Score += GetCorrectMatchValue(match.Stage);
                }
            }

            return score;
        }

        private int GetCorrectMatchValue(MatchStage aMatchStage)
        {
            switch (aMatchStage)
            {
                case MatchStage.Group:
                    return CorrectMatchBet_GroupStage_Value;
                case MatchStage.FirstRound:
                case MatchStage.RoundOfSixteen:
                case MatchStage.QuarterFinals:
                case MatchStage.SemiFinals:
                case MatchStage.ThirdPlace:
                case MatchStage.Finals:
                    return CorrectMatchBet_TournamentStage_Value;
                case MatchStage.Qualifying:
                default:
                    return 0;
            }
        }
    }
}