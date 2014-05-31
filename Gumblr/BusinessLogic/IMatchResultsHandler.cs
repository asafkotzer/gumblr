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
        Task UpdateNewMatches();
    }

    public class MatchResultsHandler : IMatchResultsHandler
    {
        IUserRepository mUserRepository;
        IMatchRepository mMatchRepository;
        ITournamentOrganizer mTournamentOrganizer;
        IEmailProvider mEmailProvider;

        public MatchResultsHandler(IUserRepository aUserRepository, IMatchRepository aMatchRepository, ITournamentOrganizer aTournamentOrganizer, IEmailProvider aEmailProvider)
        {
            mUserRepository = aUserRepository;
            mMatchRepository = aMatchRepository;
            mTournamentOrganizer = aTournamentOrganizer;
            mEmailProvider = aEmailProvider;
        }

        public async Task UpdateNewMatches()
        {
            var allMatches = await mMatchRepository.GetAllMatches();
            var newMatches = mTournamentOrganizer.GenerateMatches(
                allMatches.Where(x => x.IsStub() == false),
                allMatches.Where(x => x.IsStub() == true));

            if (newMatches.Count() > 0)
            {
                // update repository
                await Task.WhenAll(newMatches.Select(x => mMatchRepository.Create(x)));

                // send emails to all users
                var users = await mUserRepository.GetAllUsers();
                var sendEmailTasks = users.Select(u => SendNewMatchesEmail(u, newMatches));
                await Task.WhenAll(sendEmailTasks);

            }
        }

        private async Task SendNewMatchesEmail(ApplicationUser aUser, IEnumerable<Match> aNewMatches)
        {
            var model = new NewMatchesModel { User = aUser, NewMatches = aNewMatches };
            var generator = new NewMatchesEmailGenerator(model);
            await mEmailProvider.Send(generator.GetMessage());
        }
    }
}