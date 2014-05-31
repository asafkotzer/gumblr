using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gumblr.Models
{
    public class NewMatchesModel
    {
        public IEnumerable<Match> NewMatches { get; set; }
        public ApplicationUser User { get; set; }
    }
}