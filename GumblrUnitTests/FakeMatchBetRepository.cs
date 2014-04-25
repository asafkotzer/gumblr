using Gumblr.DataAccess;
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
    }
}
