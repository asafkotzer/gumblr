using Gumblr.BusinessLogic;
using Gumblr.Helpers;
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
        static readonly DateTime PresentTime = new DateTime(2001, 1, 1);
        static readonly DateTime FutureTime = new DateTime(2001, 1, 2);

        private BettingModel GetModel(DateTime aMatchStartTime, MatchResult? aResult = null, string aWinner = null)
        {
            var model = new BettingModel { Matches = new List<MatchBet> { new MatchBet { MatchId = "SomeId", StartTime = aMatchStartTime } } };
            if (aResult.HasValue)
            {
                model.Matches.First().ExpectedResult = aResult.Value;
            }

            model.Winner = aWinner;

            return model;
        }

        [Test]
        public void ValidateModel_FirstBet_NoException()
        {
            var validator = new BettingModelValidator(new FakeCurrentTimeProvider(PresentTime));
            validator.ValidateModel(GetModel(FutureTime), null);
        }

        [Test]
        public void ValidateModel_PreviousBetExistsNewModelSimilar_NoException()
        {
            var validator = new BettingModelValidator(new FakeCurrentTimeProvider(PresentTime));
            validator.ValidateModel(GetModel(FutureTime), GetModel(FutureTime));
        }

        [Test]
        public void ValidateModel_ChangedBetSubmittedAfterStartTime_Throws()
        {
            var validator = new BettingModelValidator(new FakeCurrentTimeProvider(FutureTime));

            var previousModel = GetModel(PresentTime, MatchResult.Draw);
            var newModel = GetModel(PresentTime, MatchResult.Host);

            Assert.Throws<BettingModelValidationExcpetion>(() => validator.ValidateModel(newModel, previousModel));
        }

        [Test]
        public void ValidateModel_DrawBetOnPlayoffMatch_Throws()
        {
            var validator = new BettingModelValidator(new FakeCurrentTimeProvider(PresentTime));

            var newModel = GetModel(FutureTime, MatchResult.Draw);

            Assert.Throws<BettingModelValidationExcpetion>(() => validator.ValidateModel(newModel, null));
        }

        [Test]
        public void ValidateModel_WinnerSetInTime_NoException()
        {
            var validator = new BettingModelValidator(new FakeCurrentTimeProvider(PresentTime));
            validator.ValidateModel(GetModel(FutureTime, null, "Winner"), null);
        }

        [Test]
        public void ValidateModel_WinnerSetTooLate_Throws()
        {
            var validator = new BettingModelValidator(new FakeCurrentTimeProvider(FutureTime));
            Assert.Throws<BettingModelValidationExcpetion>(() => validator.ValidateModel(GetModel(PresentTime, null, "Winner"), null));

        }

    }

}
