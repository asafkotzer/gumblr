using Gumblr.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gumblr.BusinessLogic
{
    public interface IMatchGeneratorFactory
    {
        IMatchGenerator GetMatchGenerator(MatchStage aMatchStage);
    }

    public class MatchGeneratorFactory : IMatchGeneratorFactory
    {
        IStandingsCalculator mStandingsCalculator;
        public MatchGeneratorFactory(IStandingsCalculator aStandingsCalculator)
        {
            mStandingsCalculator = aStandingsCalculator;
        }

        public IMatchGenerator GetMatchGenerator(MatchStage aStubMatchStage)
        {
            IMatchGenerator generator;
            switch (aStubMatchStage)
            {
                case MatchStage.RoundOfSixteen:
                    generator = new FirstRoundMatchGenerator(mStandingsCalculator);
                    break;
                case MatchStage.QuarterFinals:
                case MatchStage.SemiFinals:
                case MatchStage.Finals:
                    generator = new PlayoffMatchGenerator();
                    break;
                case MatchStage.ThirdPlace:
                    generator = new ThirdPlaceMatchGenerator();
                    break;
                case MatchStage.Qualifying:
                case MatchStage.Group:
                default:
                    throw new NotImplementedException();
            }

            return generator;
        }
    }


}