﻿using Gumblr.Models;
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

        public async Task SetUserBet(string aUserId, string aUsername, BettingModel aBet)
        {
            // update user bets
            await mStorageProvider.CreateOrUpdate("UserBets", aUserId, aBet);

            // update statistics
            await mMatchStatisticsRepository.UpdateUserBets(aUserId, aUsername, aBet);
        }

        public async Task<BettingModel> GetUserBets(string aUserId)
        {
            var result = await mStorageProvider.TryRead<BettingModel>("UserBets", aUserId);
            return result;
        }
    }
}