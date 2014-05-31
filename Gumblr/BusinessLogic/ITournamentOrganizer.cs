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
            if (aMatchGeneratorFactory == null)
            {
                throw new ArgumentNullException("aMatchGeneratorFactory");
            }

            mMatchGeneratorFactory = aMatchGeneratorFactory;
        }

        public IEnumerable<Match> GenerateMatches(IEnumerable<Match> aCompleteMatches, IEnumerable<Match> aMatchStubs)
        {
            if (aCompleteMatches == null)
            {
                throw new ArgumentNullException("aCompleteMatches");
            }

            if (aCompleteMatches.Any(x => x.ActualResult == MatchResult.Unknown))
            {
                throw new ArgumentException("Complete matches list cannot include incomplete matches");
            }

            if (aMatchStubs == null)
            {
                throw new ArgumentNullException("aMatchStubs");
            }

            var completedMatchesByMatchId = aCompleteMatches.ToDictionary(x => x.MatchId);

            var createdStubs = aMatchStubs.Where(x => x.Dependency.GetDeterminigMatchIds().All(d => completedMatchesByMatchId.ContainsKey(d)));

            // we can tell by the stage of the stub - FirstRound will be A, others will be B
            // (Group will also depend on Qualifying, but that's a more complex logic
            // A) if the dependency is on two groups, then we need to get #1 from one and #2 from the other
            // B) if the dependency is on two matches, then we need to get the two winners

            var newMatches = createdStubs.Select(s =>
            {
                var matchGenerator = mMatchGeneratorFactory.GetMatchGenerator(s.Stage);
                var newMatch = matchGenerator.GetMatch(s, new MatchDependencyContainer(s.Dependency, completedMatchesByMatchId));
                return newMatch;
            });

            return newMatches;
        }
    }

    public class MatchDependencyContainer
    {
        public MatchDependencyType Type { get; set; }
        public IEnumerable<Match> HostDeterminingMatches { get; set; }
        public IEnumerable<Match> VisitorDeterminingMatches { get; set; }

        public MatchDependencyContainer(MatchDependency aDependency, Dictionary<string, Match> aCompleteMatchesByMatchId)
        {
            Type = aDependency.Type;
            HostDeterminingMatches = aDependency.HostDeterminingMatchIds.Select(d => aCompleteMatchesByMatchId[d]);
            VisitorDeterminingMatches = aDependency.VisitorDeterminingMatchIds.Select(d => aCompleteMatchesByMatchId[d]);
        }

        public IEnumerable< Match> GetDeterminigMatches()
        {
            return HostDeterminingMatches.Union(VisitorDeterminingMatches);
        }
    }

}