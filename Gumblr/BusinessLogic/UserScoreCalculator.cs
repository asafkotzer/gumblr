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
        UserScore CalculateScore(IEnumerable<Match> aMatchesWithActualResults, FinalResultsMode aFinalResultsModel, BettingModel aMatchBets);
    }

    public class UserScoreCalculator : IUserScoreCalculator
    {
        static readonly int CorrectWinner_Value = 10;
        static readonly int CorrectMatchBet_GroupStage_Value = 8;
        static readonly int CorrectMatchBet_RoundOfSixteen_Value = 10;
        static readonly int CorrectMatchBet_QuarterFinal_Value = 12;
        static readonly int CorrectMatchBet_SemiFinal_Value = 15;
        static readonly int CorrectMatchBet_ThirdPlace_Value = 10;
        static readonly int CorrectMatchBet_Final_Value = 20;

        public UserScore CalculateScore(IEnumerable<Match> aMatchesWithActualResults, FinalResultsMode aFinalResultsModel, BettingModel aBet)
        {
            var score = new UserScore();
            
            if (aBet == null)
            {
                return score;
            }

            if (aFinalResultsModel != null && aFinalResultsModel.Winner == aBet.Winner)
            {
                score.ScoreElements.Add(new ScoreElement 
                { 
                    Title = string.Format("the champion is {0}", aFinalResultsModel.Winner),
                    Value = CorrectWinner_Value 
                });
            }

            var betByMatchId = aBet.Matches.ToDictionary(x => x.MatchId);
            foreach (var match in aMatchesWithActualResults.Where(x => x.ActualResult != MatchResult.Unknown).OrderBy(x => x.StartTime))
            {
                MatchBet matchBet;
                if (betByMatchId.TryGetValue(match.MatchId, out matchBet) && matchBet.ExpectedResult == match.ActualResult)
                {
                    string title;
                    if (match.ActualResult == MatchResult.Draw)
                    {
                        title = string.Format("{0} vs. {1} ended with tie ({2})", match.Host, match.Visitor, match.StageString);
                    }
                    else
                    {
                        title = string.Format("{0} defeated {1} ({2})", match.GetWinner(), match.GetLoser(), match.StageString);
                    }

                    score.ScoreElements.Add(new ScoreElement
                    {
                        Title = title,
                        Value = GetCorrectMatchValue(match.Stage),
                    });
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
                case MatchStage.RoundOfSixteen:
                    return CorrectMatchBet_RoundOfSixteen_Value;
                case MatchStage.QuarterFinals:
                    return CorrectMatchBet_QuarterFinal_Value;
                case MatchStage.SemiFinals:
                    return CorrectMatchBet_SemiFinal_Value;
                case MatchStage.ThirdPlace:
                    return CorrectMatchBet_ThirdPlace_Value;
                case MatchStage.Finals:
                    return CorrectMatchBet_Final_Value;
                case MatchStage.Qualifying:
                default:
                    return 0;
            }
        }
    }
}