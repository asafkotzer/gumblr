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
        Task Create(string aContainer, string aKey, object aItem);
        Task<T> Read<T>(string aContainer, string aKey);
        Task Update(string aContainer, string aKey, object aItem);
        Task CreateOrUpdate(string aContainer, string aKey, object aItem);
        void Delete(string aContainer, string aKey);
    }
}
