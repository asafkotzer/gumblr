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
        Task<FinalResultsModel> GetFinalResults();
        Task SetFinalResults(FinalResultsModel aItem);
    }

    public class FinalResultsRepository : IFinalResultsRepository
    {
        IStorageProvider mStorageProvider;

        public FinalResultsRepository(IStorageProvider aStorageProvider)
        {
            mStorageProvider = aStorageProvider;
        }

        public async Task<FinalResultsModel> GetFinalResults()
        {
            var results = await mStorageProvider.TryRead<FinalResultsModel>("FinalResults", "All");
            return results;
        }

        public async Task SetFinalResults(FinalResultsModel aItem)
        {
            await mStorageProvider.Update("FinalResults", "All", aItem);
        }
    }
}