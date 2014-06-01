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

        public MatchRepository(IStorageProvider aStorageProvider)
        {
            mStorageProvider = aStorageProvider;
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
                .Select(x => mStorageProvider.Read<Match>(x.Container, x.Key));

            var matches = (await Task.WhenAll(tasks))
                .OrderBy(x => x.StartTime);

            return matches;
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