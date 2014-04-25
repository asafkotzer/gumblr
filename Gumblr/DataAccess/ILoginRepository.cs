using Gumblr.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Gumblr.DataAccess
{
    public interface ILoginRepository
    {
        Task<LoginEntity> GetLogin(string aProvider, string aIdentifier);
        Task CreateLogin(LoginEntity aLoginEntity);
    }
}