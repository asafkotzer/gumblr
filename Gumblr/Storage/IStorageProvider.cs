using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gumblr.Storage
{
    /*
     * We should consider using MongoDB as the backend
     * http://blogs.msdn.com/b/henrikn/archive/2012/02/19/using-web-api-with-mongodb.aspx
     */

    public interface IStorageProvider
    {
        Task Create(string aContainer, string aKey, ITableEntity aItem);
        Task<T> ReadOne<T>(string aContainer, string aKey) where T : class, ITableEntity, new();
        Task<IEnumerable<T>> Read<T>(string aContainer, string aPrefix) where T : class, ITableEntity, new();
        Task Update(string aContainer, string aKey, ITableEntity aItem);
        Task CreateOrUpdate(string aContainer, string aKey, ITableEntity aItem);
        Task Delete(string aContainer, string aKey);
    }
}
