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
                    Value = new ScoreValues().CorrectWinner
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

                    var value = GetCorrectBetValue(match.Stage);
                    if (match.Ratio != null)
                    {
                        if (match.ActualResult == MatchResult.Host)
                        {
                            value = match.Ratio.HostValue;
                        }
                        else if (match.ActualResult == MatchResult.Visitor)
                        {
                            value = match.Ratio.VisitorValue;
                        }
                        else
                        {
                            throw new NotImplementedException("Draw on a calculated score");
                        }
                    }

                    score.ScoreElements.Add(new ScoreElement
                    {
                        Title = title,
                        Value = value,
                    });
                }
            }

            return score;
        }

        public static int GetCorrectBetValue(MatchStage aMatchStage)
        {
            switch (aMatchStage)
            {
                case MatchStage.Group:
                    return new ScoreValues().CorrectMatchBet_GroupStage;
                case MatchStage.RoundOfSixteen:
                    return new ScoreValues().CorrectMatchBet_RoundOfSixteen;
                case MatchStage.QuarterFinals:
                    return new ScoreValues().CorrectMatchBet_QuarterFinal;
                case MatchStage.SemiFinals:
                    return new ScoreValues().CorrectMatchBet_SemiFinal;
                case MatchStage.ThirdPlace:
                    return new ScoreValues().CorrectMatchBet_ThirdPlace;
                case MatchStage.Finals:
                    return new ScoreValues().CorrectMatchBet_Final;
                case MatchStage.Qualifying:
                default:
                    return 0;
            }
        }
    }
}