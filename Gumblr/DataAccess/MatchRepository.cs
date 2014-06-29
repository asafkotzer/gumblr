using Gumblr.Helpers;
using Gumblr.Models;
using Gumblr.Storage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Gumblr.DataAccess
{
    public class MatchRepository : IMatchRepository
    {
        IStorageProvider mStorageProvider;
        IRatioRetriever mRatioRetriever;

        public MatchRepository(IStorageProvider aStorageProvider, IRatioRetriever aRatioRetriever)
        {
            mStorageProvider = aStorageProvider;
            mRatioRetriever = aRatioRetriever;
        }

        public async Task<IEnumerable<Match>> GetAllMatches()
        {
            var allMatches = await GetMatches("Matches");

            return allMatches
                .OrderBy(x => x.StartTime);
        }

        public async Task<IEnumerable<Match>> GetMatches()
        {
            var allMatches = await GetMatches("Matches");
            
            return allMatches
                .Where(x => x.IsStub() == false)
                .OrderBy(x => x.StartTime);
        }

        private async Task<IEnumerable<Match>> GetMatches(string aContainerName)
        {
            var tasks = (await mStorageProvider.List(aContainerName))
                .Select(GetMatch);

            var matches = (await Task.WhenAll(tasks))
                .OrderBy(x => x.StartTime);

            return matches;
        }

        private async Task<Match> GetMatch(IItemDescriptor aItemDescriptor)
        {
            var match = await mStorageProvider.Read<Match>(aItemDescriptor.Container, aItemDescriptor.Key);
            if (!string.IsNullOrEmpty(match.Index))
            {
                // match #52 was Greece vs. Costa Rica - that's where the change takes place
                int index = 0;
                if (int.TryParse(match.Index, out index) && index > 52)
                {
                    match.Ratio = await mRatioRetriever.GetRatio(match);
                }
            }

            return match;
        }

        public async Task Update(Match aMatch)
        {
            await mStorageProvider.Update("Matches", aMatch.MatchId, aMatch);
        }

        public async Task Create(Match aMatch)
        {
            await mStorageProvider.Create("Matches", aMatch.MatchId, aMatch);
        }
    }
}