using Gumblr.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gumblr.DataAccess
{
    public interface IMatchBetRepository
    {
        Task SetUserBet(string aUserId, IEnumerable<MatchBet> aBets);
        Task<IEnumerable<MatchBet>> GetUserBets(string aUserId);
    }
}
