using Gumblr.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gumblr.DataAccess
{
    public interface IMatchRepository
    {
        Task<IEnumerable<Match>> GetMatches();
        Task<IEnumerable<Match>> GetAllMatches();
        Task Update(Match aMatch);
        Task Create(Match aMatch);
    }
}
