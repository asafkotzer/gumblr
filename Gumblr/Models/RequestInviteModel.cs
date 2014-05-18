using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gumblr.Models
{
    public class RequestInviteModel
    {
        public string EmailAddress { get; set; }
        public string Comments { get; set; }
        public string Name { get; set; }
    }
}