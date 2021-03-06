﻿using Gumblr.Helpers;
using Gumblr.Models;
using SendGridMail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;

namespace Gumblr.BusinessLogic.Emails
{
    public interface IEmailProvider
    {
        Task Send(EmailMessage aMessage);
    }

    public class SendGridEmailProvider : IEmailProvider
    {
        static readonly MailAddress GumblrAddress = new MailAddress("asafkotzer@gmail.com", "Gumblr");
        IConfigurationRetriever mConfigurationRetriever;

        public SendGridEmailProvider(IConfigurationRetriever aConfigurationRetriever)
        {
            mConfigurationRetriever = aConfigurationRetriever;
        }

        public async Task Send(EmailMessage aMessage)
        {
            var sendGridMessage = SendGrid.GetInstance();
            sendGridMessage.AddTo(aMessage.ToAddress);
            sendGridMessage.From = GumblrAddress;
            sendGridMessage.Subject = aMessage.Subject.GenerateSubject();
            sendGridMessage.Html = aMessage.Content.GenerateContent();

            var username = mConfigurationRetriever.GetSetting("SendgridUsername");
            var password = mConfigurationRetriever.GetSetting("SendgridPassword");
            var credentials = new NetworkCredential(username, password);
            var client = Web.GetInstance(credentials);

            // Send the email.
            await client.DeliverAsync(sendGridMessage);
        }
    }

    public class EmailMessage
    {
        public IEmailContentGenerator Content { get; set; }
        public IEmailSubjectGenerator  Subject { get; set; }
        public string ToName { get; set; }
        public string ToAddress { get; set; }
    }

    public interface IEmailContentGenerator { string GenerateContent(); }
    public interface IEmailSubjectGenerator { string GenerateSubject(); }

    public class InviteRequestEmailModel
    {
        public string RequesterName { get; set; }
        public string RequesterAddress { get; set; }
        public string RequestDetails { get; set; }
    } 

}