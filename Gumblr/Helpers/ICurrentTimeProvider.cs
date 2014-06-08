using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gumblr.Helpers
{
    public interface ICurrentTimeProvider
    {
        DateTime GetCurrentTime();
    }

    public class CurrentTimeProvider : ICurrentTimeProvider
    {
        public DateTime GetCurrentTime()
        {
            return DateTime.UtcNow;
        }
    }
}