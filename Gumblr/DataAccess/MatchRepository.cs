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
            var matches = await mStorageProvider.Read<Match>("Matches", "");
            return matches.OrderBy(x => x.StartTime);
        }

        //public IEnumerable<Match> GetMatches()
        //{
        //    var staticMatchListPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Configuration\MatchList.csv");
        //    var matchParser = new CsvMatchParser(File.ReadAllLines(staticMatchListPath));
        //    var result = matchParser.ParseMatches();

        //    return result;
        //}

        public async Task Update(Match aMatch)
        {
            await mStorageProvider.Update("Matches", aMatch.MatchId, aMatch);
        }
    }
}