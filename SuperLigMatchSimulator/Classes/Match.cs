using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperLigMatchSimulator.Classes
{
    public class Match
    {
        public string HomeTeam { get; set; }
        public string AwayTeam { get; set; }
        public int? HomeScore { get; set; }
        public int? AwayScore { get; set; }
        public DateTime MatchDate { get; set; }
        public int Week { get; set; }
        public string HomeLogo { get; set; }
        public string AwayLogo { get; set; }
        public string MatchId { get => HomeTeam + AwayTeam; }
        public bool IsFinished { get; set; }
    }
}
