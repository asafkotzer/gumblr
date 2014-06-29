using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gumblr.Models
{
    public class BettingModel
    {
        public IEnumerable<MatchBet> Matches { get; set; }
        public IEnumerable<string> PossibleWinners { get; set; }
        public DateTime WinnerBetDeadline { get { return Matches.Min(x => x.StartTime); } }
        public string Winner { get; set; }
        public Dictionary<string, string> TeamLogoUrlByTeamName { get; set; }
        public ScoreValues ScoreValues { get { return new ScoreValues(); } }
    }

    public class ScoreValues
    {
        public int CorrectWinner { get { return 10; } }
        public int CorrectMatchBet_GroupStage { get { return 10; } }
        public int CorrectMatchBet_RoundOfSixteen { get { return 12; } }
        public int CorrectMatchBet_QuarterFinal { get { return 15; } }
        public int CorrectMatchBet_SemiFinal { get { return 18; } }
        public int CorrectMatchBet_ThirdPlace { get { return 18; } }
        public int CorrectMatchBet_Final { get { return 20; } }
    }


}