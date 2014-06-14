using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gumblr.Models
{
    public class MatchDetailsModel
    {
        public Match Match { get; set; }
        public Dictionary<string, List<string>> UserNamesByExpectedResult { get; set; }
        public Dictionary<string, MatchResult> ExpectedResultByUserName { get; set; }

        public Dictionary<string, MatchResult> ExpectedResultByUsername { get; set; }
    }
}