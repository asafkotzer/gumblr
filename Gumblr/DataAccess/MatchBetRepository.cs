using Gumblr.Models;
using Gumblr.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Gumblr.DataAccess
{
    public class MatchBetRepository : IMatchBetRepository
    {
        IStorageProvider mStorageProvider;
        IMatchStatisticsRepository mMatchStatisticsRepository;

        public MatchBetRepository(IStorageProvider aStorageProvider, IMatchStatisticsRepository aMatchStatisticsRepository)
        {
            mStorageProvider = aStorageProvider;
            mMatchStatisticsRepository = aMatchStatisticsRepository;
        }

        public async Task SetUserBet(string aUserId, IEnumerable<MatchBet> aBets)
        {
            // update user bets
            await mStorageProvider.CreateOrUpdate("UserBets", aUserId, aBets);

            // update statistics
            await mMatchStatisticsRepository.UpdateUserBets(aUserId, aBets);
        }

        public async Task<IEnumerable<MatchBet>> GetUserBets(string aUserId)
        {
            return await mStorageProvider.Read<IEnumerable<MatchBet>>("UserBets", aUserId);
        }
    }
}