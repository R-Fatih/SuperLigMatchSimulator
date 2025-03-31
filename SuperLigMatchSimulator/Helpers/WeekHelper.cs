using SuperLigMatchSimulator.Classes;

namespace SuperLigMatchSimulator.Helpers
{
    public class WeekHelper
    {
        public static string GetByeTeamOfWeek(IList<Standing> standings,Match lastMatches)
        {
            var allTeams = standings.Select(x => x.Team).Distinct().ToList();
            var byeTeamOnThisWeek = allTeams.Except(new[] { lastMatches.HomeTeam, lastMatches.AwayTeam }).FirstOrDefault();
            return byeTeamOnThisWeek;
        }
        public static string GetByeTeamOfWeek(IList<Match> weekMatches, Match lastMatches)
        {

            var allTeams = weekMatches
                                      .SelectMany(m => new[] { m.HomeTeam, m.AwayTeam })
                                      .Distinct()
                                      .ToList();
            var byeTeamOnThisWeek = allTeams.Except(new[] { lastMatches.HomeTeam, lastMatches.AwayTeam }).FirstOrDefault();
            return byeTeamOnThisWeek;
        }
    }
}
