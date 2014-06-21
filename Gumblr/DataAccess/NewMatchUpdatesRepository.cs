using Gumblr.Models;
using Gumblr.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Gumblr.DataAccess
{
    public interface INewMatchUpdatesRepository
    {
        Task Create(NewMatchUpdate aMatchUpdates);
        Task<NewMatchUpdate> Get(string aUpdateName);
    }

    public class NewMatchUpdatesRepository : INewMatchUpdatesRepository
    {
        IStorageProvider mStorageProvider;

        public NewMatchUpdatesRepository(IStorageProvider aStorageProvider)
        {
            mStorageProvider = aStorageProvider;
        }

        public async Task Create(NewMatchUpdate aMatchUpdates)
        {
            await mStorageProvider.Create("NewMatchUpdates", aMatchUpdates.Id, aMatchUpdates);
        }

        public async Task<NewMatchUpdate> Get(string aUpdateName)
        {
            var newMatchUpdate = await mStorageProvider.Read<NewMatchUpdate>("NewMatchUpdates", aUpdateName);
            return newMatchUpdate;
        }
    }
}