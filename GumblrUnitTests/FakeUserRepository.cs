using Gumblr.DataAccess;
using Gumblr.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GumblrUnitTests
{
    public class FakeUserRepository : IUserRepository
    {
        public async Task CreateUser(ApplicationUser userProfile)
        {
            
        }

        public async Task<ApplicationUser> GetUser(string aUserId)
        {
            throw new NotImplementedException();
        }


        public Task<IEnumerable<ApplicationUser>> GetAllUsers()
        {
            throw new NotImplementedException();
        }

        public Task UpdateUser(ApplicationUser aUserProfile)
        {
            throw new NotImplementedException();
        }
    }
}
