using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gumblr.Models
{
    public enum MatchResult
    {
        Unknown = -1,
        Host,
        Draw, 
        Visitor
    }
}