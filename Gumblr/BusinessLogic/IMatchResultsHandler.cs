using Gumblr.BusinessLogic.Emails;
using Gumblr.DataAccess;
using Gumblr.Helpers;
using Gumblr.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Gumblr.BusinessLogic
{
    public interface IMatchResultsHandler
    {
        Task<NewMatchUpdate> UpdateNewMatches();
    }

    public class MatchResultsHandler : IMatchResultsHandler
    {
        IUserRepository mUserRepository;
        IMatchRepository mMatchRepository;
        ITournamentOrganizer mTournamentOrganizer;
        INewMatchUpdatesRepository mNewMatchUpdatesRepository;

        public MatchResultsHandler(IUserRepository aUserRepository, IMatchRepository aMatchRepository, ITournamentOrganizer aTournamentOrganizer, INewMatchUpdatesRepository aNewMatchUpdatesRepository)
        {
            mUserRepository = aUserRepository;
            mMatchRepository = aMatchRepository;
            mTournamentOrganizer = aTournamentOrganizer;
            mNewMatchUpdatesRepository = aNewMatchUpdatesRepository;
        }

        public async Task<NewMatchUpdate> UpdateNewMatches()
        {
            var allMatches = await mMatchRepository.GetAllMatches();
            var newMatches = mTournamentOrganizer.GenerateMatches(
                allMatches.Where(x => x.IsStub() == false),
                allMatches.Where(x => x.IsStub() == true)).ToList();

            var updateId = DateTime.UtcNow.ToString("yyyyMMdd_HHmmss");
            var update = new NewMatchUpdate(updateId, newMatches);
            await mNewMatchUpdatesRepository.Create(update);

            return update;
        }

    }
}