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
            this.MatchId = aMatch.MatchId;
            this.Group = aMatch.Group;
            this.Host = aMatch.Host;
            this.StartTime = aMatch.StartTime;
            this.Venue = aMatch.Venue;
            this.Visitor = aMatch.Visitor;
            this.IsComplete = aMatch.IsComplete;
            this.Stage = aMatch.Stage;
            this.GoalsScoredByHost = aMatch.GoalsScoredByHost;
            this.GoalsScoredByVisitor = aMatch.GoalsScoredByVisitor;
            this.Index = aMatch.Index;
            this.Ratio = aMatch.Ratio;
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

        public string Index { get; set; }
        public Ratio Ratio { get; set; }
        public double CorrectBetValue { get { return UserScoreCalculator.GetCorrectBetValue(Stage); } }

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

        public bool HasStarted { get { return DateTime.UtcNow > StartTime.ToUniversalTime(); } }

        public string StageString { get 
            {
                string stage = null;

                switch (this.Stage)
                {
                    case MatchStage.Qualifying:
                        stage = "Qualifying";
                        break;
                    case MatchStage.Group:
                        stage = "Group stage";
                        break;
                    case MatchStage.RoundOfSixteen:
                        stage = "Round of sixteen";
                        break;
                    case MatchStage.QuarterFinals:
                        stage = "Quarter final";
                        break;
                    case MatchStage.SemiFinals:
                        stage = "Semi final";
                        break;
                    case MatchStage.ThirdPlace:
                        stage = "Third place";
                        break;
                    case MatchStage.Finals:
                        stage = "The Final";
                        break;
                    default:
                        stage = this.Stage.ToString();
                        break;
                }

                return stage;
            } 
        }

        public string GetWinner()
        {
            if (ActualResult == MatchResult.Host) return Host;
            if (ActualResult == MatchResult.Visitor) return Visitor;

            throw new Exception("Can't get winner on this match");
        }

        public string GetLoser()
        {
            if (ActualResult == MatchResult.Host) return Visitor;
            if (ActualResult == MatchResult.Visitor) return Host;

            throw new Exception("Can't get loser on this match");
        }

        public bool IsStub()
        {
            return string.IsNullOrEmpty(Host);
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

    public class Ratio
    {
        private MatchStage mMatchStage;

        public int HostValue { get; set; }
        public int VisitorValue { get; set; }
        public int DrawValue { get; set; }

        private double mHostPercent;
        private double mVisitorPercent;

        public Ratio()
        {

        }

        public Ratio(MatchStage aMatchStage, double aHostPercent, double aVisitorPercent)
        {
            mMatchStage = aMatchStage;

            mHostPercent = aHostPercent;
            mVisitorPercent = aVisitorPercent;

            var normalizedHostProbability = Normalize(mHostPercent);
            var normalizedVisitorProbability = Normalize(mVisitorPercent);

            if (normalizedHostProbability > 0.5)
            {
                normalizedHostProbability = normalizedHostProbability * Math.Pow(0.8, normalizedHostProbability) + 0.06;
            }
            else if (normalizedHostProbability < 0.5)
            {
                normalizedHostProbability = 1 - (normalizedVisitorProbability * Math.Pow(0.8, normalizedVisitorProbability) + 0.06);
            }

            var correctMatchValue = UserScoreCalculator.GetCorrectBetValue(mMatchStage);
            var calculatedHostvalue = (int)(2 * correctMatchValue * (1 - normalizedHostProbability));
            HostValue = Math.Max(correctMatchValue, calculatedHostvalue);
            VisitorValue = Math.Max(correctMatchValue, (2 * correctMatchValue) - calculatedHostvalue);
            DrawValue = 0;
        }

        public Ratio(MatchStage aMatchStage, double aHostPercent, double aDrawPercent, double aVisitorPercent)
        {
            throw new NotImplementedException("Including draw in ratio is not implemented");
        }

        private double Normalize(double aPercent)
        {
            return (aPercent / (mHostPercent + mVisitorPercent));
        }
    }

}
