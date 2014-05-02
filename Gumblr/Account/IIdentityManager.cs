using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using Microsoft.AspNet.Identity;

namespace Gumblr.Account
{
    public interface IIdentityManager
    {
        string GetUserId(IPrincipal aUser);
    }

    public class IdentityManager : IIdentityManager
    {
        public string GetUserId(IPrincipal aUser)
        {
            return aUser.Identity.GetUserId();
        }
    }
}