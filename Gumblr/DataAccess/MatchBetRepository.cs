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

        public async Task SetUserBet(string aUserId, BettingModel aBet)
        {
            // update user bets
            await mStorageProvider.CreateOrUpdate("UserBets", aUserId, aBet);

            // update statistics
            await mMatchStatisticsRepository.UpdateUserBets(aUserId, aBet);
        }

        public async Task<BettingModel> GetUserBets(string aUserId)
        {
            BettingModel result = null;
            try
            {
                result = await mStorageProvider.Read<BettingModel>("UserBets", aUserId);
            }
            catch (ItemDoesNotExitException) { }

            return result;
        }
    }
}