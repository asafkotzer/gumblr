using Gumblr.Helpers;
using Gumblr.Models;
using Gumblr.Storage;
using Gumblr.Storage.Azure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Gumblr.DataAccess
{
    public interface IMatchStatisticsRepository
    {
        Task UpdateUserBets(string aUserId, BettingModel aBet);
        Task<MatchStatisticsModel> GetMatchStatistics(string aMatchId);
    }

    public class MatchStatisticsRepository : IMatchStatisticsRepository
    {
        ITableProvider mTableProvider;
        IStorageProvider mStorageProvider;

        public MatchStatisticsRepository(IStorageProvider aStorageProvider, IConfigurationRetriever aConfigurationRetriever, TableStorageProvider.Factory aTableProviderFactory)
        {
            mStorageProvider = aStorageProvider;
            mTableProvider = aTableProviderFactory(aConfigurationRetriever, "MatchStatistics");
        }

        public async Task<MatchStatisticsModel> GetMatchStatistics(string aMatchId)
        {
            var match = await mStorageProvider.TryRead<Match>("Matches", aMatchId);

            var expectedResults = await mTableProvider.GetPartition(aMatchId);

            var model = new MatchStatisticsModel();
            model.Match = match;
            model.ExpectedResultByUserId = expectedResults
                .Select(x => new { Key = x.Properties["UserId"] as string, Value = (MatchResult)x.Properties["ExpectedResult"] })
                .ToDictionary(x => x.Key, x => (MatchResult)x.Value);

            return model;
        }

        public async Task UpdateUserBets(string aUserId, BettingModel aBet)
        {
            await Task.WhenAll(aBet.Matches.Select(x => UpdateMatchStatistics(aUserId, x)));
        }


        private async Task UpdateMatchStatistics(string aUserId, MatchBet aBet)
        {
            var statisticsEntity = new TableEntity();
            statisticsEntity.Properties["MatchId"] = aBet.MatchId;
            statisticsEntity.Properties["UserId"] = aUserId;
            statisticsEntity.Properties["ExpectedResult"] = (int)aBet.ExpectedResult;
            await mTableProvider.CreateOrUpdate(aBet.MatchId, aUserId, statisticsEntity);
        }
    }
}