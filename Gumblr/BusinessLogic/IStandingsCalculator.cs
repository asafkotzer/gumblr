using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Gumblr.Models;

namespace Gumblr.BusinessLogic
{
    public interface IStandingsCalculator
    {
        Dictionary<string, List<string>> GetStandingsByGroup(IEnumerable<Match> aCompletedMatches, IEnumerable<Match> aStubMatches);
    }

    public class StandingsCalculator : IStandingsCalculator
    {
        public Dictionary<string, List<string>> GetStandingsByGroup(IEnumerable<Match> aCompletedMatches, IEnumerable<Match> aStubMatches)
        {
            throw new NotImplementedException();
        }
    }
}