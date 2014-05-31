using Gumblr.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Gumblr.BusinessLogic.Emails
{
    public class NewMatchesEmailGenerator : IEmailContentGenerator, IEmailSubjectGenerator
    {
        NewMatchesModel mModel;

        public NewMatchesEmailGenerator(NewMatchesModel aModel)
        {
            mModel = aModel;
        }

        public string GenerateContent()
        {
            string intro = mModel.NewMatches.Count() == 1 ? 
                "There's a new match to bet on!" : 
                string.Format("There are {0} new matches to bet on!", mModel.NewMatches.Count());

            StringBuilder matchListBuilder = new StringBuilder();
            foreach (var match in mModel.NewMatches)
            {
                matchListBuilder.AppendLine(string.Format(
                    "<span style='font-weight:bold'>{0}: </span><span>{1} vs. {2} ({3})</span><br/>",
                    GetStageString(match.Stage), 
                    match.Host, 
                    match.Visitor, 
                    match.StartTime.ToString("dd/MM HH:mm")));
            }

            var format = string.Format(
                "<html><body style='font-family: Calibri'; font-size: 12px><p>Hi {0},</p><p>{1}</p><p>{2}</p>Go to <a href='{3}/Betting/PlaceBets/'>your bets page</a> to place your bets.<p>Thanks,<br/>Gumblr</p></html></body>", 
                mModel.User.UserName, 
                intro,
                matchListBuilder.ToString(),
                "http://gumblr.azurewebsites.net");

            return format;
        }

        private string GetStageString(MatchStage aMatchStage)
        {
            string stage = null;

            switch (aMatchStage)
            {
                case MatchStage.Qualifying:
                    stage = "Qualifying";
                    break;
                case MatchStage.Group:
                    stage = "Group stage";
                    break;
                case MatchStage.RoundOfSixteen:
                    stage = "Round of sixteen";
                    break;
                case MatchStage.QuarterFinals:
                    stage = "Quarter final";
                    break;
                case MatchStage.SemiFinals:
                    stage = "Third place";
                    break;
                case MatchStage.ThirdPlace:
                    stage = "Third place";
                    break;
                case MatchStage.Finals:
                    stage = "The Final";
                    break;
                default:
                    stage = aMatchStage.ToString();
                    break;
            }

            return stage;
        }

        public string GenerateSubject()
        {
            string subject = string.Empty;
            if (mModel.NewMatches.Count() == 1)
            {
                var match = mModel.NewMatches.First();
                subject = string.Format("{0} or {1}?", match.Host, match.Visitor);
            }
            else
            {
                subject = string.Format("You have {0} new bets to make", mModel.NewMatches.Count());
            }

            return subject;
        }

        public EmailMessage GetMessage()
        {
            var message = new EmailMessage
            {
                ToAddress = mModel.User.EmailAddress,
                ToName = mModel.User.UserName,
                Content = this,
                Subject = this,
            };
            return message;
        }
    }
}