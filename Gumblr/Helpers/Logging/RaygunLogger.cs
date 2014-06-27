using log4net.Appender;
using log4net.Core;
using Mindscape.Raygun4Net;
using Mindscape.Raygun4Net.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gumblr.Helpers.Logging
{
    public class RaygunLogger : AppenderSkeleton
    {
        protected override void Append(LoggingEvent loggingEvent)
        {
            var messageObject = loggingEvent.MessageObject as LogMessage;

            var error = RaygunMessageBuilder.New
                .SetUser(new RaygunIdentifierMessage(messageObject.Username))
                .SetExceptionDetails(loggingEvent.ExceptionObject)
                .SetUserCustomData(messageObject.ToDictionary())
                .Build();

            var raygunClient = new RaygunClient();
            raygunClient.Send(error);
        }
    }
}