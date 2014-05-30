using Gumblr.BusinessLogic;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gumblr.Models
{
    public class Match
    {
        public Match()
        {

        }

        public Match(Match aMatch)
        {
            this.Group = aMatch.Group;
            this.Host = aMatch.Host;
            this.StartTime = aMatch.StartTime;
            this.Venue = aMatch.Venue;
            this.Visitor = aMatch.Visitor;
            this.IsComplete = aMatch.IsComplete;
            this.Stage = aMatch.Stage;
            this.GoalsScoredByHost = aMatch.GoalsScoredByHost;
            this.GoalsScoredByVisitor = aMatch.GoalsScoredByVisitor;
        }

        public string Host { get; set; }
        public string Visitor { get; set; }
        public DateTime StartTime { get; set; }
        public string Venue { get; set; }
        public string Group { get; set; }
        public MatchStage Stage { get; set; }
        public bool IsComplete { get; set; }
        public int GoalsScoredByHost { get; set; }
        public int GoalsScoredByVisitor { get; set; }
        public MatchDependency Dependency { get; set; }

        public string HostLogoUrl { get { return TeamLogoUrlCreator.Instance.GetLogoUrl(Host); } }
        public string VisitorLogoUrl { get { return TeamLogoUrlCreator.Instance.GetLogoUrl(Visitor); } }
        public string MatchId { get; set; }
        public MatchResult ActualResult
        {
            get
            {
                if (!IsComplete) return MatchResult.Unknown;
                if (GoalsScoredByHost < GoalsScoredByVisitor) return MatchResult.Visitor;
                else if (GoalsScoredByHost > GoalsScoredByVisitor) return MatchResult.Host;
                else return MatchResult.Draw;
            }
        }

        public string GetWinner()
        {
            if (ActualResult == MatchResult.Host) return Host;
            if (ActualResult == MatchResult.Visitor) return Visitor;

            throw new Exception("Can't get winner on this match");
        }
    }

    public class MatchDependency
    {
        public MatchDependencyType Type { get; set; }
        public IEnumerable<string> HostDeterminingMatchIds { get; set; }
        public IEnumerable<string> VisitorDeterminingMatchIds { get; set; }

        public IEnumerable<string> GetDeterminigMatchIds()
        {
            return HostDeterminingMatchIds.Union(VisitorDeterminingMatchIds);
        }
    }

    public enum MatchDependencyType { TwoGroups, TwoMatches }
}