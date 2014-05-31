using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Gumblr.Models;

namespace Gumblr.BusinessLogic
{
    public interface IStandingsCalculator
    {
        Group GetGroupStandings(IEnumerable<Match> aMatches);
        Group GetGroupStandings(IEnumerable<Match> aMatches, IEnumerable<CoinFlip> aCoinFlips);
    }

    public class StandingsCalculator : IStandingsCalculator
    {
        public Group GetGroupStandings(IEnumerable<Match> aMatches)
        {
            return GetGroupStandings(aMatches, Enumerable.Empty<CoinFlip>());
        }

        public Group GetGroupStandings(IEnumerable<Match> aMatches, IEnumerable<CoinFlip> aCoinFlips)
        {
            if (aMatches == null) throw new ArgumentNullException("aMatches");
            if (!aMatches.Any()) throw new ArgumentException("Match list cannot be empty", "aMatches");
            if (aCoinFlips == null) throw new ArgumentNullException("aCoinFlips");

            var teamByTeamName = new Dictionary<string, Team>();
            foreach (var match in aMatches)
            {
                var host = GetOrCreateTeam(teamByTeamName, match.Host);
                var visitor = GetOrCreateTeam(teamByTeamName, match.Visitor);

                if (match.GoalsScoredByHost > match.GoalsScoredByVisitor)
                {
                    host.Score += 3;
                }
                else if (match.GoalsScoredByHost < match.GoalsScoredByVisitor)
                {
                    visitor.Score += 3;
                }
                else
                {
                    host.Score++;
                    visitor.Score++;
                }

                host.ScoredGoals += match.GoalsScoredByHost;
                host.ReceivedGoals += match.GoalsScoredByVisitor;

                visitor.ScoredGoals += match.GoalsScoredByVisitor;
                visitor.ReceivedGoals += match.GoalsScoredByHost;
            }

            var orderedList = teamByTeamName.Values
                .OrderByDescending(x => x.Score)
                .ThenByDescending(x => x.ScoredGoals - x.ReceivedGoals)
                .ThenByDescending(x => x.ScoredGoals)
                .ThenByDescending(x => aCoinFlips.Count(c => c.Winner == x.Name))
                .ToList();

            var result = new Group { Teams = orderedList };
            return result;
        }

        private static Team GetOrCreateTeam(Dictionary<string, Team> teamByTeamName, string aTeamName)
        {
            Team team = null;
            if (!teamByTeamName.TryGetValue(aTeamName, out team))
            {
                teamByTeamName[aTeamName] = team = new Team(aTeamName);
            }
            return team;
        }
    }

    public class Group
    {
        public List<Team> Teams { get; set; }

        public string GetWinnerName() { return Teams[0].Name; }
        public string GetSecondName() { return Teams[1].Name; }
    }

    public class Team
    {
        public string Name { get; set; }
        public int Score { get; set; }
        public int ScoredGoals { get; set; }
        public int ReceivedGoals { get; set; }

        public Team(string aName)
        {
            Name = aName;
        }
    }

    public class CoinFlip
    {
        public string Winner { get; set; }
        public string Loser { get; set; }

        public CoinFlip(string aWinner, string aLoser)
        {
            Winner = aWinner;
            Loser = aLoser;
        }
    }
}