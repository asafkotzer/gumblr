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
        IApplicationUserConverter mApplicationUserConverter;

        public UserRepository(IStorageProvider aStorageProvider, IApplicationUserConverter aApplicationUserConverter)
        {
            mStorageProvider = aStorageProvider;
            mApplicationUserConverter = aApplicationUserConverter;
        }

        public async Task<ApplicationUser> GetUser(string aUserId)
        {
            var user = await mStorageProvider.TryRead<ApplicationUserEntity>("Users", aUserId);
            return mApplicationUserConverter.Convert(user);
        }

        public async Task CreateUser(ApplicationUser aUserProfile)
        {
            var user = mApplicationUserConverter.Convert(aUserProfile);
            await mStorageProvider.Create("Users", user.Id.ToString(), user);
        }

        public async Task UpdateUser(ApplicationUser aUserProfile)
        {
            var user = mApplicationUserConverter.Convert(aUserProfile);
            await mStorageProvider.Update("Users", user.Id.ToString(), user);
        }

        public async Task<IEnumerable<ApplicationUser>> GetAllUsers()
        {
            var users = await mStorageProvider.Read<ApplicationUserEntity>("Users", "");
            return users.Select(x => mApplicationUserConverter.Convert(x));
        }
    }
}