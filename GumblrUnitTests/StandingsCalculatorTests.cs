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
    public class StandingsCalculatorTests
    {
        [Test]
        public void GetStandingsByGroup_NullList_ThrowsNullArgumentException()
        {
            var calculator = new StandingsCalculator();

            Assert.Throws<ArgumentNullException>(() => calculator.GetGroupStandings(null));
        }

        [Test]
        public void GetStandingsByGroup_EmptyList_ReturnsEmptyResult()
        {
            var calculator = new StandingsCalculator();

            Assert.Throws<ArgumentException>(() => calculator.GetGroupStandings(new List<Match>()));
        }

        [Test]
        public void GetStandingsByGroup_ListWithOneMatch_ReturnsOneGroupWithTwoTeams()
        {
            var calculator = new StandingsCalculator();
            var matches = new List<Match>()
            {
                new Match { Host = "host", Visitor = "visitor", Stage = MatchStage.Group, Group = "A", GoalsScoredByHost = 0, GoalsScoredByVisitor = 0 }
            };

            var result = calculator.GetGroupStandings(matches);

            Assert.AreEqual(2, result.Teams.Count());
        }

        [Test]
        public void GetStandingsByGroup_ListWithOneMatchWhereHostWins_CorrectStandings()
        {
            var calculator = new StandingsCalculator();
            var matches = new List<Match>()
            {
                new Match { Host = "host", Visitor = "visitor", Stage = MatchStage.Group, Group = "A", GoalsScoredByHost = 1, GoalsScoredByVisitor = 0 }
            };

            var result = calculator.GetGroupStandings(matches);

            Assert.AreEqual("host", result.Teams.First().Name);
            Assert.AreEqual(3, result.Teams.First().Score);
            Assert.AreEqual(1, result.Teams.First().ScoredGoals);
            Assert.AreEqual(0, result.Teams.First().ReceivedGoals);

            Assert.AreEqual("visitor", result.Teams.Last().Name);
            Assert.AreEqual(0, result.Teams.Last().Score);
            Assert.AreEqual(0, result.Teams.Last().ScoredGoals);
            Assert.AreEqual(1, result.Teams.Last().ReceivedGoals);
        }

        [Test]
        public void GetStandingsByGroup_OneMatch_Tie_CorrectStandings()
        {
            var calculator = new StandingsCalculator();
            var matches = new List<Match>()
            {
                new Match { Host = "host", Visitor = "visitor", Stage = MatchStage.Group, Group = "A", GoalsScoredByHost = 1, GoalsScoredByVisitor = 0 }
            };

            var result = calculator.GetGroupStandings(matches);

            Assert.AreEqual("host", result.Teams.First().Name);
            Assert.AreEqual(3, result.Teams.First().Score);
            Assert.AreEqual(1, result.Teams.First().ScoredGoals);
            Assert.AreEqual(0, result.Teams.First().ReceivedGoals);

            Assert.AreEqual("visitor", result.Teams.Last().Name);
            Assert.AreEqual(0, result.Teams.Last().Score);
            Assert.AreEqual(0, result.Teams.Last().ScoredGoals);
            Assert.AreEqual(1, result.Teams.Last().ReceivedGoals);
        }

        [TestCase]
        public void GetStandingsByGroup_TwoMacthesFourTeams_TwoTeamsWithSameScoreDifferentGoalCount_CorrectScore()
        {
            var calculator = new StandingsCalculator();
            var matches = new List<Match>()
            {
                new Match { Host = "team1", Visitor = "team2", GoalsScoredByHost = 2, GoalsScoredByVisitor = 0 },
                new Match { Host = "team3", Visitor = "team4", GoalsScoredByHost = 1, GoalsScoredByVisitor = 0 },
            };

            var result = calculator.GetGroupStandings(matches);

            Assert.AreEqual("team1", result.Teams[0].Name);
            Assert.AreEqual("team3", result.Teams[1].Name);
            Assert.AreEqual("team4", result.Teams[2].Name);
            Assert.AreEqual("team2", result.Teams[3].Name);
        }

        [TestCase]
        public void GetStandingsByGroup_OneMacthTie_CoinFlip_CorrectScore()
        {
            var calculator = new StandingsCalculator();
            var matches = new List<Match>()
            {
                new Match { Host = "team1", Visitor = "team2", GoalsScoredByHost = 0, GoalsScoredByVisitor = 0 },
            };

            var result = calculator.GetGroupStandings(matches, new List<CoinFlip> { new CoinFlip("team2", "team1") });

            Assert.AreEqual("team2", result.Teams.First().Name);
            Assert.AreEqual("team1", result.Teams.Last().Name);
        }

    }
}
