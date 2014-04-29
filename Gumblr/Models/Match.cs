using Gumblr.BusinessLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gumblr.Models
{
    public class Match
    {
        public Match()
        {

        }

        public Match(Match aMatch)
        {
            this.Group = aMatch.Group;
            this.Host = aMatch.Host;
            this.StartTime = aMatch.StartTime;
            this.Venue = aMatch.Venue;
            this.Visitor = aMatch.Visitor;
        }

        public string Host { get; set; }
        public string Visitor { get; set; }
        public DateTime StartTime { get; set; }
        public string Venue { get; set; }
        public string Group { get; set; }

        public string HostLogoUrl { get { return TeamLogoUrlCreator.Instance.GetLogoUrl(Host); } }
        public string VisitorLogoUrl { get { return TeamLogoUrlCreator.Instance.GetLogoUrl(Visitor); } }

        public string GetMatchId()
        {
            return string.Format("{0}_{1}_{2}", Host.ToLower(), Visitor.ToLower(), StartTime.Ticks);
        }
    }
}