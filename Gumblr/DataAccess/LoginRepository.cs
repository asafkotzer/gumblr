using Gumblr.Models;
using Gumblr.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Gumblr.DataAccess
{
    public class LoginRepository : ILoginRepository
    {
        IStorageProvider mStorageProvider;

        public LoginRepository(IStorageProvider aStorageProvider)
        {
            mStorageProvider = aStorageProvider;
        }

        public async Task<LoginEntity> GetLogin(string aProvider, string aIdentifier)
        {
            var key = string.Format("{0}_{1}", aProvider, aIdentifier);
            return await mStorageProvider.Read<LoginEntity>("Logins", key);
        }

        public async Task CreateLogin(LoginEntity aLoginEntity)
        {
            var key = string.Format("{0}_{1}", aLoginEntity.Provider, aLoginEntity.Identifier);
            await mStorageProvider.Create("Logins", key, aLoginEntity);
        }
    }
}