using Gumblr.DataAccess;
using Gumblr.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GumblrUnitTests
{
    public class FakeMatchRepository : IMatchRepository
    {
        public Task<IEnumerable<Match>> GetMatches()
        {
            return Task.FromResult<IEnumerable<Match>>(new List<Match> { new Match 
            {
                Group = "A",
                Host = "FakeHost",
                StartTime = new DateTime(2013, 1, 1),
                Venue = "FakeVenue",
                Visitor = "FakeVisitor"
            }});
        }
    }
}
