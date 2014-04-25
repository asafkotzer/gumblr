using Gumblr.Models;
using Gumblr.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Gumblr.DataAccess
{
    public class UserRepository : IUserRepository
    {
        IStorageProvider mStorageProvider;

        public UserRepository(IStorageProvider aStorageProvider)
        {
            mStorageProvider = aStorageProvider;
        }

        public async Task<ApplicationUser> GetUser(string aUserId)
        {
            ApplicationUser user = null;
            try
            {
                user = await mStorageProvider.Read<ApplicationUser>("Users", aUserId);
            }
            catch (ItemDoesNotExitException) { }

            return user;
        }

        public async Task CreateUser(ApplicationUser userProfile)
        {
            await mStorageProvider.Create("Users", userProfile.Id.ToString(), userProfile);
        }
    }
}