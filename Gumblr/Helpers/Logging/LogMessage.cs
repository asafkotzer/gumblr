using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gumblr.Helpers.Logging
{
    public class LogMessage
    {
        public DateTime TimeOccurred { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public string Username { get; set; }
    }
}