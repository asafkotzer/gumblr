using Gumblr.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gumblr.BusinessLogic.Emails
{
    public class InviteRequestEmailGenerator : IEmailContentGenerator, IEmailSubjectGenerator
    {
        static readonly string ToAddress = "shacharmanor@gmail.com";
        static readonly string ToName = "Shachar";
        RequestInviteModel mModel;

        public InviteRequestEmailGenerator(RequestInviteModel aModel)
        {
            mModel = aModel;
        }

        public string GenerateContent()
        {
            var format = string.Format(
                "<html><body style='font-family: Calibri'><p>Hi Shachar,<br/>A user has requested an invite to Gumblr:</p><p><span style='font-weight:bold'>Email address: </span>{0}</p><p><span style='font-weight:bold'>Request text: </span><span>{1}</span></p>Go to the <a href='{2}/GroupAdmin/Users?email={3}&username={4}'>User Administration page</a> to add them<p>Thanks,<br/>Gumblr</p></html></body>",
                mModel.EmailAddress,
                mModel.Comments,
                "http://gumblr.azurewebsites.net",
                HttpUtility.UrlEncode(mModel.EmailAddress),
                HttpUtility.UrlEncode(mModel.Name));

            return format;
        }

        public string GenerateSubject()
        {
            return string.Format("{0} requested an invite to Gumblr", string.IsNullOrWhiteSpace(mModel.Name) ? "A new user" : mModel.Name);
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