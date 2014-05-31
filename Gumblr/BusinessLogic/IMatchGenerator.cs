using Gumblr.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gumblr.BusinessLogic
{
    public interface IMatchGenerator
    {
        Match GetMatch(Match aStub, MatchDependencyContainer aMatchDependencyContainer);
    }

    public class FirstRoundMatchGenerator : IMatchGenerator
    {
        IStandingsCalculator mStandingsCalculator;
        public FirstRoundMatchGenerator(IStandingsCalculator aStandingsCalculator)
        {
            mStandingsCalculator = aStandingsCalculator;
        }

        public Match GetMatch(Match aStub, MatchDependencyContainer aMatchDependencyContainer)
        {
            if (aMatchDependencyContainer.HostDeterminingMatches.Select(x => x.Group).Distinct().Count() != 1 ||
                aMatchDependencyContainer.VisitorDeterminingMatches.Select(x => x.Group).Distinct().Count() != 1)
            {
                throw new MatchGeneratorException("A match in the first round can depend only on one group from each side");
            }

            aStub.Host = mStandingsCalculator.GetGroupStandings(aMatchDependencyContainer.HostDeterminingMatches).GetWinnerName();
            aStub.Visitor = mStandingsCalculator.GetGroupStandings(aMatchDependencyContainer.VisitorDeterminingMatches).GetSecondName();

            return aStub;
        }
    }

    public abstract class BasePlayoffMatchGenerator : IMatchGenerator
    {
        public Match GetMatch(Match aStub, MatchDependencyContainer aMatchDependencyContainer)
        {
            if (aMatchDependencyContainer.HostDeterminingMatches.Count() != 1 || aMatchDependencyContainer.VisitorDeterminingMatches.Count() != 1)
            {
                throw new MatchGeneratorException("A playoff match can depend only on one match from each side");
            }

            if (aMatchDependencyContainer.GetDeterminigMatches().Any(x => x.ActualResult == MatchResult.Draw))
            {
                throw new MatchGeneratorException("A playoff match cannot end with a tie");
            }

            return GetMatch(aStub, aMatchDependencyContainer.HostDeterminingMatches.First(), aMatchDependencyContainer.VisitorDeterminingMatches.First());
        }

        protected abstract Match GetMatch(Match aStub, Match aHostDeterminer, Match aVisitorDeterminer);
    }

    public class PlayoffMatchGenerator : BasePlayoffMatchGenerator
    {
        protected override Match GetMatch(Match aStub, Match aHostDeterminer, Match aVisitorDeterminer)
        {
            aStub.Host = aHostDeterminer.GetWinner();
            aStub.Visitor = aVisitorDeterminer.GetWinner();
            return aStub;
        }
    }

    public class ThirdPlaceMatchGenerator : BasePlayoffMatchGenerator
    {
        protected override Match GetMatch(Match aStub, Match aHostDeterminer, Match aVisitorDeterminer)
        {
            aStub.Host = aHostDeterminer.GetLoser();
            aStub.Visitor = aVisitorDeterminer.GetLoser();
            return aStub;
        }

    }

    public class MatchGeneratorException : Exception
    {
        public MatchGeneratorException(string aMessage)
            : base(aMessage)
        {

        }
    }
}