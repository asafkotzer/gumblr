using Gumblr.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GumblrUnitTests
{
    public class FakeCurrentTimeProvider : ICurrentTimeProvider
    {
        public DateTime Time { get; set; }

        public FakeCurrentTimeProvider(DateTime aTime)
        {
            Time = aTime;
        }

        public DateTime GetCurrentTime()
        {
            return Time;
        }
    }

}
