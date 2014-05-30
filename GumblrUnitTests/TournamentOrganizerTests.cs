using Gumblr.BusinessLogic;
using Gumblr.Models;
using MsTest = Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GumblrUnitTests
{
    [TestFixture]
    [MsTest.TestClass]
    public class TournamentOrganizerTests
    {
        [Test]
        public void GenerateMatches_NullCompleteMatches_Throws()
        {
            var generator = new TournamentOrganizer(new MatchGeneratorFactory(new StandingsCalculator()));
            Assert.Throws<ArgumentNullException>(() => generator.GenerateMatches(null, Enumerable.Empty<Match>()));
        }

        [Test]
        public void GenerateMatches_CompleteMatchesWithUnknownResult_Throws()
        {
            var generator = new TournamentOrganizer(new MatchGeneratorFactory(new StandingsCalculator()));
            Assert.Throws<ArgumentException>(() => generator.GenerateMatches(new List<Match> { new Match() }, Enumerable.Empty<Match>()));
        }

        [Test]
        public void GenerateMatches_NullMatchStubs_Throws()
        {
            var generator = new TournamentOrganizer(new MatchGeneratorFactory(new StandingsCalculator()));
            Assert.Throws<ArgumentNullException>(() => generator.GenerateMatches(new List<Match> { new Match { IsComplete = true } }, null));
        }

        [Test]
        public void GenerateMatches_PlayoffMatchGenerator_OnlyOneDependency_Throws()
        {
            var generator = new TournamentOrganizer(new MatchGeneratorFactory(new StandingsCalculator()));
            var match1 = new Match { MatchId = "id1", Host = "host1", Visitor = "visitor1", IsComplete = true, GoalsScoredByHost = 1, GoalsScoredByVisitor = 0, };

            var dependency = new MatchDependency { HostDeterminingMatchIds = new List<string> { "id1" }, VisitorDeterminingMatchIds = new List<string>() };
            var stubMatch = new Match { Stage = MatchStage.Finals, Dependency = dependency };

            Assert.Throws<MatchGeneratorException>(() => generator.GenerateMatches(new List<Match> { match1 }, new List<Match> { stubMatch }).ToList());
        }

        [Test]
        public void GenerateMatches_PlayoffMatchGenerator_DrawInDependency_Throws()
        {
            var generator = new TournamentOrganizer(new MatchGeneratorFactory(new StandingsCalculator()));
            var match1 = new Match { MatchId = "id1", Host = "host1", Visitor = "visitor1", IsComplete = true, GoalsScoredByHost = 1, GoalsScoredByVisitor = 0, };
            var match2 = new Match { MatchId = "id2", Host = "host2", Visitor = "visitor2", IsComplete = true, GoalsScoredByHost = 0, GoalsScoredByVisitor = 0, };
            var dependency = new MatchDependency { HostDeterminingMatchIds = new List<string> { "id1" }, VisitorDeterminingMatchIds = new List<string> { "id2" } };
            var stubMatch = new Match { Stage = MatchStage.Finals, Dependency = dependency };

            Assert.Throws<MatchGeneratorException>(() => generator.GenerateMatches(new List<Match> { match1, match2 }, new List<Match> { stubMatch }).ToList());
        }

        [Test]
        public void GenerateMatches_PlayoffMatchGenerator_MatchTeamsAreTheWinners()
        {
            var generator = new TournamentOrganizer(new MatchGeneratorFactory(new StandingsCalculator()));
            var match1 = new Match  { MatchId = "id1", Host = "host1", Visitor = "visitor1", IsComplete = true, GoalsScoredByHost = 1, GoalsScoredByVisitor = 0, };
            var match2 = new Match { MatchId = "id2", Host = "host2", Visitor = "visitor2", IsComplete = true, GoalsScoredByHost = 1, GoalsScoredByVisitor = 0, };

            var dependency = new MatchDependency { Type = MatchDependencyType.TwoMatches, HostDeterminingMatchIds = new List<string> { "id1" }, VisitorDeterminingMatchIds = new List<string> { "id2" } };
            var stubMatch = new Match { Stage = MatchStage.Finals, Dependency = dependency };

            var result = generator.GenerateMatches(new List<Match> { match1, match2 }, new List<Match> { stubMatch });

            Assert.AreEqual(1, result.Count());
            Assert.AreEqual("host1", result.First().Host);
            Assert.AreEqual("host2", result.First().Visitor);
        }

        [Test]
        public void GenerateMatches_FirstRoundMatchGenerator_HostIsWinnerInFirstGroup_VisitorIsSecondInSecondGroup()
        {
            var generator = new TournamentOrganizer(new MatchGeneratorFactory(new StandingsCalculator()));
            var match1 = new Match { Group = "A", MatchId = "id1", Host = "host1", Visitor = "visitor1", IsComplete = true, GoalsScoredByHost = 1, GoalsScoredByVisitor = 0, };
            var match2 = new Match { Group = "B", MatchId = "id2", Host = "host2", Visitor = "visitor2", IsComplete = true, GoalsScoredByHost = 1, GoalsScoredByVisitor = 0, };
            
            var dependency = new MatchDependency { Type = MatchDependencyType.TwoGroups, HostDeterminingMatchIds = new List<string> { "id1" }, VisitorDeterminingMatchIds = new List<string> { "id2" } };
            var stubMatch = new Match { Stage = MatchStage.FirstRound, Dependency = dependency };

            var result = generator.GenerateMatches(new List<Match> { match1, match2 }, new List<Match> { stubMatch });

            Assert.AreEqual(1, result.Count());
            Assert.AreEqual("host1", result.First().Host);
            Assert.AreEqual("visitor2", result.First().Visitor);
        }
    }
}
