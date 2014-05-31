using Gumblr.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gumblr.BusinessLogic
{
    public interface IBettingModelValidator 
    {
        void ValidateModel(BettingModel aModel, BettingModel aCurrentBet);
    }

    public class BettingModelValidator : IBettingModelValidator
    {
        public void ValidateModel(BettingModel aModel, BettingModel aCurrentBet)
        {
            Dictionary<string,MatchBet> currentBetByMatchId;
            if (aCurrentBet != null)
            {
                currentBetByMatchId = aCurrentBet.Matches.ToDictionary(x => x.MatchId);
            }
            else
            {
                currentBetByMatchId = new Dictionary<string, MatchBet>();
            }

            var changedBets = aModel.Matches.Where(match =>
            {
                MatchBet currentBet;
                if (currentBetByMatchId.TryGetValue(match.MatchId, out currentBet))
                {
                    return match.ExpectedResult != currentBet.ExpectedResult;
                }

                return match.ExpectedResult != MatchResult.Unknown;
            });

            if (changedBets.Any(x => x.StartTime < DateTime.UtcNow))
            {
                throw new BettingModelValidationExcpetion("A bet was made after the match started");
            }
        }
    }
}