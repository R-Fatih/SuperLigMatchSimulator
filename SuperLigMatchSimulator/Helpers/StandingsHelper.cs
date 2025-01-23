using SuperLigMatchSimulator.Classes;

namespace SuperLigMatchSimulator.Helpers
{
    public static class StandingsHelper
    {
        public static IList<Standing> StandingsCalculator(IList<WeekMatch> json, Dictionary<string, int> reductedPoints)
        {
            var standindgs = new List<Standing>();
            var allMatches = json.SelectMany(x => x.Matches).ToList();

            var homeStandings = allMatches.GroupBy(x => x.HomeTeam).Select(y => new Standing
            {
                Won = y.Count(z => z.HomeScore > z.AwayScore),
                Drawn = y.Where(x => x.HomeScore != null && x.AwayScore != null).Count(z => z.HomeScore == z.AwayScore),
                Lost = y.Count(z => z.HomeScore < z.AwayScore),
                GoalsFor = y.Sum(z => z.HomeScore ?? 0),
                GoalsAgainst = y.Sum(z => z.AwayScore ?? 0),
                GoalDifference = y.Sum(z => z.HomeScore ?? 0) - y.Sum(z => z.AwayScore ?? 0),
                Points = y.Count(z => z.HomeScore > z.AwayScore) * 3 + y.Where(x => x.HomeScore != null && x.AwayScore != null).Count(z => z.HomeScore == z.AwayScore),
                Team = y.Key,
                Played = y.Count(x => x.HomeScore != null),
                TeamLogo = y.FirstOrDefault().HomeLogo
            });

            var awayStandings = allMatches.GroupBy(x => x.AwayTeam).Select(y => new Standing
            {
                Won = y.Count(z => z.AwayScore > z.HomeScore),
                Drawn = y.Where(x => x.HomeScore != null).Count(z => z.AwayScore == z.HomeScore),
                Lost = y.Count(z => z.AwayScore < z.HomeScore),
                GoalsFor = y.Sum(z => z.AwayScore ?? 0),
                GoalsAgainst = y.Sum(z => z.HomeScore ?? 0),
                GoalDifference = y.Sum(z => z.AwayScore ?? 0) - y.Sum(z => z.HomeScore ?? 0),
                Points = y.Count(z => z.AwayScore > z.HomeScore) * 3 + y.Where(x => x.HomeScore != null && x.AwayScore != null).Count(z => z.AwayScore == z.HomeScore),
                Team = y.Key,
                Played = y.Count(x => x.HomeScore != null && x.AwayScore != null),
                TeamLogo = y.FirstOrDefault().AwayLogo
            });

            var combinedStandings = homeStandings
                .Concat(awayStandings)
                .GroupBy(x => x.Team)
                .Select(g =>
                {
                    var team = g.Key;
                    var standing = new Standing
                    {
                        Team = g.Key,
                        Played = g.Sum(x => x.Played),
                        Won = g.Sum(x => x.Won),
                        Drawn = g.Sum(x => x.Drawn),
                        Lost = g.Sum(x => x.Lost),
                        GoalsFor = g.Sum(x => x.GoalsFor),
                        GoalsAgainst = g.Sum(x => x.GoalsAgainst),
                        GoalDifference = g.Sum(x => x.GoalDifference),
                        Points = g.Sum(x => x.Points),
                        TeamLogo = g.First().TeamLogo,
                        TrendDirection = TrendDirection.Equal,
                        ReductedPoints = g.Sum(x => x.ReductedPoints)
                    };
                    if (reductedPoints.ContainsKey(team))
                    {
                        var reductedPoint = reductedPoints[team];
                        standing.Points += reductedPoint;
                        standing.ReductedPoints = reductedPoint;
                    };
                    return standing;
                })
                .OrderByDescending(x => x.Points)
                .ThenByDescending(x => x.GoalDifference);
            return combinedStandings.ToList();
        }
    }
}
