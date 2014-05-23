using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gumblr.Models
{
    public class MatchesAdminModel
    {
        public IEnumerable<Match> Matches { get; set; }
    }

    public class UsersAdminModel
    {
        public IEnumerable<NewUserModel> Users { get; set; }   
    }

    public class NewUserModel
    {
        public string EmailAddress { get; set; }
    }
}