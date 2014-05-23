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

        public async Task<IEnumerable<Match>> GetMatches()
        {
            // get all predefined matches
            var matches = await GetMatches("Matches");

            // TODO: if we want to use the IStandingsCalculator, we have to get all stub matches
            // var stubMatches = await GetMatches("StubMatches");

            return matches.OrderBy(x => x.StartTime);
        }

        private async Task<IEnumerable<Match>> GetMatches(string aContainerName)
        {
            var tasks = (await mStorageProvider.List(aContainerName))
                .Select(x => mStorageProvider.Read<Match>(x.Container, x.Key));

            var matches = await Task.WhenAll(tasks);
            return matches;
        }

        public async Task Update(Match aMatch)
        {
            await mStorageProvider.Update("Matches", aMatch.MatchId, aMatch);
        }
    }
}