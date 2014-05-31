using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace Gumblr.BusinessLogic
{
    public class TeamLogoUrlCreator
    {
        static TeamLogoUrlCreator sInstance;
        static object sInstanceLock = new object();
        public static TeamLogoUrlCreator Instance
        {
            get
            {
                if (sInstance == null)
                {
                    lock (sInstanceLock)
                    {
                        if (sInstance == null)
                        {
                            sInstance = new TeamLogoUrlCreator();
                        }
                    }
                }

                return sInstance;
            }
        }
        protected Dictionary<string, string> AbbreviationByFullName { get; set; }

        protected TeamLogoUrlCreator()
        {
            var resourceSet = NationalTeamLogoNames.ResourceManager.GetResourceSet(CultureInfo.InvariantCulture, true, true);
            AbbreviationByFullName = new Dictionary<string, string>();
            foreach (DictionaryEntry item in resourceSet)
            {
                AbbreviationByFullName[item.Key.ToString()] = item.Value.ToString();
            }
        }

        public string GetLogoUrl(string aTeam)
        {
            if (string.IsNullOrEmpty(aTeam))
            {
                return string.Empty;
            }

            var teamName = aTeam.Replace(' ', '_');
            return string.Format("/Images/Flags/{0}.png", AbbreviationByFullName[teamName].ToLower());
        }

        
    }
}