using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gumblr.Models
{
    public class LoginEntity
    {
        public string Provider { get; set; }
        public string Identifier { get; set; }
        public Guid UserId { get; set; }
        public string PasswordHash { get; set; }
    }
}