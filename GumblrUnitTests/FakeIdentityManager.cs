using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gumblr.Account;

namespace GumblrUnitTests
{
    public class FakeIdentityManager : IIdentityManager
    {
        public string GetUserId(System.Security.Principal.IPrincipal aUser)
        {
            return "FakeUserId";
        }
    }
}
