﻿using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gumblr.Models
{
    public class MatchStatisticsModel : TableEntity
    {
        public Match Match { get; set; }
        public Dictionary<string, MatchResult> ExpectedResultByUserId { get; set; }

        public MatchStatisticsModel()
        {
            ExpectedResultByUserId = new Dictionary<string, MatchResult>();
        }
    }
}