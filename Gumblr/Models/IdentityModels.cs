using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.WindowsAzure.Storage.Table;

namespace Gumblr.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public string EmailAddress { get; set; }
        public UserRole Role { get; set; }
    }

    public enum UserRole { User, Administrator }

    public class ApplicationUserEntity : TableEntity
    {
        public virtual string Id { get; set; }
        public virtual string UserName { get; set; }
        public string EmailAddress { get; set; }
        public UserRole Role { get; set; }
    }

    public interface IApplicationUserConverter
    {
        ApplicationUser Convert(ApplicationUserEntity aApplicationUserEntity);
        ApplicationUserEntity Convert(ApplicationUser aApplicationUser);
    }

    public class ApplicationUserConverter : IApplicationUserConverter
    {
        public ApplicationUser Convert(ApplicationUserEntity aSource)
        {
            return new ApplicationUser
            {
                Id = aSource.Id,
                UserName = aSource.UserName,
                EmailAddress = aSource.EmailAddress,
                Role = aSource.Role,
            };
        }

        public ApplicationUserEntity Convert(ApplicationUser aSource)
        {
            return new ApplicationUserEntity
            {
                Id = aSource.Id,
                UserName = aSource.UserName,
                EmailAddress = aSource.EmailAddress,
                Role = aSource.Role,
            };
        }
    }
}