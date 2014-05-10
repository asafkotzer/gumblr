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
        public async Task SetUserBet(string aUserId, BettingModel aBets)
        {
            
        }

        public async Task<BettingModel> GetUserBets(string aUserId)
        {
            return new BettingModel();
        }
    }
}
