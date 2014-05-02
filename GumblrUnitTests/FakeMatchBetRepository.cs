using Gumblr.DataAccess;
using Gumblr.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GumblrUnitTests
{
    public class FakeMatchBetRepository : IMatchBetRepository
    {
        public async Task SetUserBet(string aUserId, IEnumerable<Gumblr.Models.MatchBet> aBets)
        {
            
        }

        public async Task<IEnumerable<MatchBet>> GetUserBets(string aUserId)
        {
            return new List<MatchBet>();
        }
    }
}
