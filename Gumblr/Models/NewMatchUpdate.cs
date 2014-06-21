using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gumblr.Models
{
    public class NewMatchUpdate
    {
        public List<Match> NewMatches { get; set; }
        public string Id { get; set; }

        public NewMatchUpdate(string aId, List<Match> aNewMatches)
        {
            Id = aId;
            NewMatches = aNewMatches;
        }
    }
}