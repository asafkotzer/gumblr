﻿using Gumblr.Models;
using SendGridMail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;

namespace Gumblr.Helpers
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

    public class InviteRequestEmailGenerator : IEmailContentGenerator, IEmailSubjectGenerator
    {
        static readonly string ToAddress = "asafkotzer@gmail.com";
        static readonly string ToName = "Asaf";
        RequestInviteModel mModel;

        public InviteRequestEmailGenerator(RequestInviteModel aModel)
        {
            mModel = aModel;
        }

        public string GenerateContent()
        {
            var format = string.Format("<html><body style='font-family: Calibri'><p>Hi Shachar,<br/>A user has requested an invite to Gumblr:</p><p><span style='font-weight:bold'>Email address: </span>{0}</p><p><span style='font-weight:bold'>Request text: </span><span>{1}</span></p>Go to the <a href='{2}/GroupAdmin/Users'>User Administration page to add them</a><p>Thanks,<br/>Gumblr</p></html></body>", mModel.EmailAddress, mModel.Comments, "http://gumblr.azurewebsites.net");
            return format;
        }

        public string GenerateSubject()
        {
            return string.Format("{0} requested an invite to Gumblr", mModel.Name ?? "A new user");
        }

        public EmailMessage GetMessage()
        {
            var message = new EmailMessage
            {
                ToAddress = ToAddress,
                ToName = ToName,
                Content = this,
                Subject = this,
            };
            return message;
        }
    }
}