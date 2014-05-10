using Gumblr.Models;
using Gumblr.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Gumblr.DataAccess
{
    public interface IMatchStatisticsRepository
    {
        Task UpdateUserBets(string aUserId, IEnumerable<MatchBet> aBets);
        Task<MatchStatisticsModel> GetMatchStatistics(string aMatchId);
    }

    public class MatchStatisticsRepository : IMatchStatisticsRepository
    {
        IStorageProvider mStorageProvider;

        public MatchStatisticsRepository(IStorageProvider aStorageProvider)
        {
            mStorageProvider = aStorageProvider;
        }

        public async Task<MatchStatisticsModel> GetMatchStatistics(string aMatchId)
        {
            MatchStatisticsModel result = null;
            try
            {
                result = await mStorageProvider.Read<MatchStatisticsModel>("MatchStatistics", aMatchId);
            }
            catch (ItemDoesNotExitException) { }
            return result;
        }

        public async Task UpdateUserBets(string aUserId, IEnumerable<MatchBet> aBets)
        {
            // TODO: not scalable because of potential collisions
            await Task.WhenAll(aBets.Select(x => UpdateMatchStatistics(aUserId, x)));
        }


        private async Task UpdateMatchStatistics(string aUserId, MatchBet aBet)
        {
            var currentStatistics = await mStorageProvider.TryRead<MatchStatisticsModel>("MatchStatistics", aBet.MatchId);
            var updatedData = UpdateMatchData(aUserId, aBet, currentStatistics);
            await mStorageProvider.CreateOrUpdate("MatchStatistics", aBet.MatchId, updatedData);
        }

        private MatchStatisticsModel UpdateMatchData(string aUserId, MatchBet aBet, MatchStatisticsModel aCurrentStatistics)
        {
            if (aCurrentStatistics == null)
            {
                aCurrentStatistics = new MatchStatisticsModel();
            }

            if (aCurrentStatistics.Match == null)
            {
                aCurrentStatistics.Match = new Match(aBet);
            }

            aCurrentStatistics.ExpectedResultByUserId[aUserId] = aBet.ExpectedResult;
            return aCurrentStatistics;
        }
    }
}