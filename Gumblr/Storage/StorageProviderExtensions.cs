using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Gumblr.Storage
{
    public static class StorageProviderExtensions
    {
        public static async Task<T> TryRead<T>(this IStorageProvider aProvider, string aContainer, string aKey) where T : class, ITableEntity, new()
        {
            T item = null;
            try
            {
                item = await aProvider.ReadOne<T>(aContainer, aKey);
            }
            catch (ItemDoesNotExitException) { }

            return item;
        }
    }
}