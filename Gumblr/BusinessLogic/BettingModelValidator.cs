using Gumblr.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gumblr.BusinessLogic
{
    public interface IBettingModelValidator 
    {
        void ValidateModel(BettingModel aNewBet, BettingModel aPreviousBet);
    }

    public class BettingModelValidator : IBettingModelValidator
    {
        public void ValidateModel(BettingModel aNewBet, BettingModel aPreviousBet)
        {
            Dictionary<string,MatchBet> currentBetByMatchId;
            if (aPreviousBet != null)
            {
                currentBetByMatchId = aPreviousBet.Matches.ToDictionary(x => x.MatchId);
            }
            else
            {
                currentBetByMatchId = new Dictionary<string, MatchBet>();
            }

            var changedBets = aNewBet.Matches.Where(match =>
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

            if (changedBets.Any(x => x.Stage != MatchStage.Group && x.ExpectedResult == MatchResult.Draw))
            {
                throw new BettingModelValidationExcpetion("Betting on draw is impossible in the playoff stage");
            }

            if (!IsValidWinnerBet(aNewBet, aPreviousBet))
            {
                aNewBet.Winner = aPreviousBet == null ? string.Empty : aPreviousBet.Winner;
            }
        }

        private static bool IsValidWinnerBet(BettingModel aNewBet, BettingModel aPreviousBet)
        {
            var isTooLate = aNewBet.WinnerBetDeadline < DateTime.UtcNow;
            var didBetChange = aPreviousBet == null || aPreviousBet.Winner != aNewBet.Winner;
            if (isTooLate && didBetChange)
            {
                return false;
            }

            return true;
        }
    }
}