using Gumblr.Storage.Azure;
using log4net.Appender;
using log4net.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Gumblr.Helpers.Logging
{
    public class TableLogger : AppenderSkeleton
    {
        ITableProvider mTableProvider;

        public TableLogger()
        {
            mTableProvider = new TableStorageProvider(new ConfigurationRetriever(), "LogEntries");
        }

        protected override void Append(LoggingEvent loggingEvent)
        {
            try
            {
                var tableEntity = new TableEntity();

                var messageObject = loggingEvent.MessageObject as LogMessage;

                tableEntity.Properties["Controller"] = messageObject.Controller;
                tableEntity.Properties["Action"] = messageObject.Action;
                tableEntity.Properties["User"] = messageObject.Username;
                tableEntity.Properties["Message"] = loggingEvent.RenderedMessage;
                tableEntity.Properties["Level"] = loggingEvent.Level.Name;
                tableEntity.Properties["LoggerName"] = loggingEvent.LoggerName;
                tableEntity.Properties["Domain"] = loggingEvent.Domain;
                tableEntity.Properties["ThreadName"] = loggingEvent.ThreadName;
                tableEntity.Properties["Exception"] = loggingEvent.ExceptionObject.ToString();

                var partitionKey = string.Format("{0:yyyyMMdd_HHmmss}", DateTime.UtcNow);
                var rowKey = string.Format("{0}_{1}", messageObject.Controller, messageObject.Action);

                Task.Run(() => mTableProvider.Create(partitionKey, rowKey, tableEntity)).Wait();
            }
            catch (Exception ex)
            {
                ErrorHandler.Error("Could not write log entry", ex);
            }
        }
    }
}