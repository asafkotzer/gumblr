using Gumblr.BusinessLogic;
using Gumblr.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MsTest = Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GumblrUnitTests
{
    [TestFixture]
    [MsTest.TestClass]
    public class BettingModelValidatorTests
    {
        static readonly DateTime OldStartTime = new DateTime(2010, 1, 1);

        [Test]
        [MsTest.TestMethod]
        public void ValidateModel_FirstBet_NoException()
        {
            var validator = new BettingModelValidator();
            validator.ValidateModel(new BettingModel { Matches = new List<MatchBet>() }, null);            
        }

        [Test]
        public void ValidateModel_PreviousBetExists_NoException()
        {
            var validator = new BettingModelValidator();
            validator.ValidateModel(new BettingModel { Matches = new List<MatchBet>() }, new BettingModel { Matches = new List<MatchBet>() });
        }

        [Test]
        public void ValidateModel_ChangedBetSubmittedAfterStartTime_Throws()
        {
            var validator = new BettingModelValidator();
            var oldBet = new MatchBet { MatchId = "SomeId", StartTime = OldStartTime, ExpectedResult = MatchResult.Draw };
            var previousModel = new BettingModel { Matches = new List<MatchBet> { oldBet } };

            var newBet = new MatchBet { MatchId = "SomeId", StartTime = OldStartTime, ExpectedResult = MatchResult.Host };
            var newModel = new BettingModel { Matches = new List<MatchBet> { oldBet } };

            Assert.Throws<BettingModelValidationExcpetion>(() => validator.ValidateModel(newModel, previousModel));
        }

    }
}
