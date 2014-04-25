using Gumblr.DataAccess;
using Gumblr.Models;
using log4net;
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
        static readonly ILog sLogger = LogManager.GetLogger(typeof(LocalUserManager));
        ILoginRepository mLoginRepository;
        IUserRepository mUserRepository;

        public LocalUserManager(IUserStore<ApplicationUser> aUserStore, ILoginRepository aLoginRepository, IUserRepository aUserRepository)
            : base(aUserStore)
        {
            mLoginRepository = aLoginRepository;
            mUserRepository = aUserRepository;
        }

        public override async Task<ApplicationUser> FindAsync(string userName, string password)
        {
            // TODO: query Logins and validate the password
            var login = await mLoginRepository.GetLogin("local", userName);
            if (login == null)
            {
                sLogger.Info("Failed login, user not found: " + userName);
                return null;
            }

            var hashCompareResult = PasswordHasher.VerifyHashedPassword(login.PasswordHash, password);
            if (hashCompareResult == PasswordVerificationResult.Failed)
            {
                sLogger.Info("Failed login, wrong password: " + userName);
                return null;
            }

            return await FindByIdAsync(login.UserId.ToString());
        }

        public override async Task<ApplicationUser> FindByIdAsync(string userId)
        {
            var user = await mUserRepository.GetUser(userId);
            return user;
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