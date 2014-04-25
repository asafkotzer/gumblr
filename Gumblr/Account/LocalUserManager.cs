using Gumblr.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace Gumblr.Account
{
    public class LocalUserManager : UserManager<ApplicationUser>
    {
        public LocalUserManager(IUserStore<ApplicationUser> aUserStore)
            : base(aUserStore)
        {

        }

        public override async Task<ApplicationUser> FindAsync(string userName, string password)
        {
            // TODO: query Logins and validate the password
            var hash = PasswordHasher.HashPassword(password);
            return new ApplicationUser { UserName = userName, PasswordHash = hash };
        }

        public override Task<ApplicationUser> FindByIdAsync(string userId)
        {
            // TODO: query Users and return the object
            return base.FindByIdAsync(userId);
        }

        public override async Task<ClaimsIdentity> CreateIdentityAsync(ApplicationUser user, string authenticationType)
        {
            var claims = new List<Claim>() 
            { 
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
            };
            return new ClaimsIdentity(claims, authenticationType);
        }
    }
}