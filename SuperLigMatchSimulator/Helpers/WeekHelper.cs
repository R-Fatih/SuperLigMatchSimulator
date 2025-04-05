using SuperLigMatchSimulator.Classes;

namespace SuperLigMatchSimulator.Helpers
{
    public class WeekHelper
    {
        public static string GetByeTeamOfWeek(IList<Match> weekMatches, IList<Standing> teams)
        {

            var allTeams = teams
                                      .SelectMany(m => new[] { m.Team})
                                      .Distinct()
                                      .ToList(); 
            var byeTeamOnThisWeek = allTeams.Except(weekMatches.Select(x => x.HomeTeam).Union(weekMatches.Select(x => x.AwayTeam))).FirstOrDefault();
            return byeTeamOnThisWeek;
        }

    }   
}
