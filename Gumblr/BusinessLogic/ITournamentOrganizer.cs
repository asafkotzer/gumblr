using Gumblr.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gumblr.BusinessLogic
{
    public interface ITournamentOrganizer
    {
        IEnumerable<Match> GenerateMatches(IEnumerable<Match> aCompleteMatches, IEnumerable<Match> aMatchStubs);
    }

    public class TournamentOrganizer : ITournamentOrganizer
    {
        IMatchGeneratorFactory mMatchGeneratorFactory;

        public TournamentOrganizer(IMatchGeneratorFactory aMatchGeneratorFactory)
        {
            mMatchGeneratorFactory = aMatchGeneratorFactory;
        }

        public IEnumerable<Match> GenerateMatches(IEnumerable<Match> aCompleteMatches, IEnumerable<Match> aMatchStubs)
        {
            if (aCompleteMatches.Any(x => x.ActualResult == MatchResult.Unknown))
            {
                throw new AggregateException("Complete matches list cannot include incomplete matches");
            }

            var completedMatchesByMatchId = aCompleteMatches.ToDictionary(x => x.MatchId);

            var createdStubs = aMatchStubs.Where(x => x.Dependencies.All(d => completedMatchesByMatchId.ContainsKey(d)));

            // we can tell by the stage of the stub - FirstRound will be A, others will be B
            // (Group will also depend on Qualifying, but that's a more complex logic
            // A) if the dependency is on two groups, then we need to get #1 from one and #2 from the other
            // B) if the dependency is on two matches, then we need to get the two winners

            var newMatches = createdStubs.Select(s =>
            {
                var matchGenerator = mMatchGeneratorFactory.GetMatchGenerator(s.Stage);
                var newMatch = matchGenerator.GetMatch(s, s.Dependencies.Select(d => completedMatchesByMatchId[d]));
                return newMatch;
            });

            return newMatches;
        }
    }

    public interface IMatchGeneratorFactory
    {
        IMatchGenerator GetMatchGenerator(MatchStage aMatchStage);
    }

    public class MatchGeneratorFactory : IMatchGeneratorFactory
    {
        public IMatchGenerator GetMatchGenerator(MatchStage aMatchStage)
        {
            var matchGenerator = aMatchStage == MatchStage.FirstRound ?
                (IMatchGenerator)new FirstRoundMatchGenerator() :
                (IMatchGenerator)new PlayoffMatchGenerator();

            return matchGenerator;
        }
    }

    public interface IMatchGenerator
    {
        Match GetMatch(Match aStub, IEnumerable<Match> aDependencies);
    }

    public class FirstRoundMatchGenerator : IMatchGenerator
    {
        public Match GetMatch(Match aStub, IEnumerable<Match> aDependencies)
        {
            throw new NotImplementedException();
        }
    }

    public class PlayoffMatchGenerator : IMatchGenerator
    {
        public Match GetMatch(Match aStub, IEnumerable<Match> aDependencies)
        {
            throw new NotImplementedException();
        }
    }
}