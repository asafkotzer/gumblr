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

}