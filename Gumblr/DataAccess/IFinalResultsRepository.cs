using Gumblr.Models;
using Gumblr.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Gumblr.DataAccess
{
    public interface IFinalResultsRepository
    {
        Task<FinalResultsMode> GetFinalResults();
        Task SetFinalResults(FinalResultsMode aItem);
    }

    public class FinalResultsRepository : IFinalResultsRepository
    {
        IStorageProvider mStorageProvider;

        public FinalResultsRepository(IStorageProvider aStorageProvider)
        {
            mStorageProvider = aStorageProvider;
        }

        public async Task<FinalResultsMode> GetFinalResults()
        {
            var results = await mStorageProvider.TryRead<FinalResultsMode>("FinalResults", "All");
            return results;
        }

        public async Task SetFinalResults(FinalResultsMode aItem)
        {
            await mStorageProvider.Update("FinalResults", "All", aItem);
        }
    }
}