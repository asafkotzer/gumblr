using Gumblr.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Gumblr.DataAccess
{
    public interface IUserRepository
    {
        Task<ApplicationUser> GetUser(string aUserId);
        Task CreateUser(ApplicationUser userProfile);
    }
}
