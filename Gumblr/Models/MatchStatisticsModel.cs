using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gumblr.Models
{
    public class MatchStatisticsModel
    {
        public Match Match { get; set; }
        public Dictionary<string, MatchResult> ExpectedResultByUsername { get; set; }

        public MatchStatisticsModel()
        {
            ExpectedResultByUsername = new Dictionary<string, MatchResult>();
        }
    }
}