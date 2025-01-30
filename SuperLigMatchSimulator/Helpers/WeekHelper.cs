using SuperLigMatchSimulator.Classes;

namespace SuperLigMatchSimulator.Helpers
{
    public class WeekHelper
    {
        public static string GetByeTeamOfWeek(IList<Standing> standings,WeekMatch lastMatches)
        {
            var allTeams = standings.Select(x => x.Team).Distinct().ToList();
            var byeTeamOnThisWeek = allTeams.Except(lastMatches.Matches.Select(x => x.HomeTeam).Union(lastMatches.Matches.Select(x => x.AwayTeam))).FirstOrDefault();
            return byeTeamOnThisWeek;
        }
        public static string GetByeTeamOfWeek(IList<WeekMatch> weekMatches, WeekMatch lastMatches)
        {

            var allTeams = weekMatches.SelectMany(wm => wm.Matches)
                                      .SelectMany(m => new[] { m.HomeTeam, m.AwayTeam })
                                      .Distinct()
                                      .ToList(); var byeTeamOnThisWeek = allTeams.Except(lastMatches.Matches.Select(x => x.HomeTeam).Union(lastMatches.Matches.Select(x => x.AwayTeam))).FirstOrDefault();
            return byeTeamOnThisWeek;
        }
    }
}
