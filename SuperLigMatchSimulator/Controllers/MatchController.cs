using Microsoft.AspNetCore.Mvc;
using SuperLigMatchSimulator.Classes;
using SuperLigMatchSimulator.Helpers;
using System.Text.Json;

namespace SuperLigMatchSimulator.Controllers
{
    public class MatchController : Controller
    {
        private const int TEAM_COUNT = 19;
        private const string url = "https://raw.githubusercontent.com/R-Fatih/SuperLig2024-25ResultSimulator/refs/heads/main/matchesFullScoreV2.json";
        private Dictionary<string, int> reductedPoints = new Dictionary<string, int>
        {
            {"Adana Demirspor",-3 }
        };
        public async Task<IActionResult> Index(bool isFirst = true)
        {
            if (isFirst)
            {
                var client = new HttpClient();
                var json = await client.GetFromJsonAsync<IList<WeekMatch>>(url);

                var standings = StandingsHelper.StandingsCalculator(json, reductedPoints);
                
                // ViewBag'e shouldInitialize flag'i ekle
                ViewBag.ShouldInitialize = true;
                ViewBag.InitialStandings = JsonSerializer.Serialize(standings);
                ViewBag.InitialMatches = JsonSerializer.Serialize(json);

                var lastMatches = json.FirstOrDefault(x => x.Week == "21");
                ViewBag.CurrentWeekMatches = lastMatches;
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> GetWeekMatches(string week, [FromForm] string allMatches)
        {
            IList<WeekMatch> existingMatches;
            
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                existingMatches = JsonSerializer.Deserialize<IList<WeekMatch>>(allMatches, options);
            
            var lastMatches = existingMatches.FirstOrDefault(x => x.Week == week);

            return Json(lastMatches);
        }

        [HttpPost]
        public async Task<IActionResult> SavePrediction(Match match, [FromForm] string allMatches)
        {
            try
            {
                // JSON string'i List<WeekMatch>'e dönüştür
                IList<WeekMatch> existingMatches;
                try
                {
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };
                    existingMatches = JsonSerializer.Deserialize<IList<WeekMatch>>(allMatches, options);
                }
                catch (JsonException ex)
                {
                    Console.WriteLine($"JSON Deserialize Error: {ex.Message}");
                    // JSON parse edilemezse API'den al
                    var client = new HttpClient();
                    existingMatches = await client.GetFromJsonAsync<IList<WeekMatch>>(url);
                }

                if (existingMatches == null)
                {
                    var client = new HttpClient();
                    existingMatches = await client.GetFromJsonAsync<IList<WeekMatch>>(url);
                }

                var weekMatch = existingMatches.FirstOrDefault(x => x.Matches.Any(m => m.MatchId == match.MatchId));
                if (weekMatch != null)
                {
                    var matchIndex = weekMatch.Matches.FindIndex(m => m.MatchId == match.MatchId);
                    if (matchIndex != -1)
                    {
                        weekMatch.Matches[matchIndex].HomeScore = match.HomeScore;
                        weekMatch.Matches[matchIndex].AwayScore = match.AwayScore;

                        var weekIndex = existingMatches.ToList().FindIndex(w => w.Week == weekMatch.Week);
                        if (weekIndex != -1)
                        {
                            existingMatches[weekIndex] = weekMatch;
                        }
                    }
                }

                var standings = StandingsHelper.StandingsCalculator(existingMatches, reductedPoints);
                
                var result = new { 
                    standings = standings,
                    matches = existingMatches
                };

                return Json(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in SavePrediction: {ex.Message}");
                Console.WriteLine($"Stack Trace: {ex.StackTrace}");
                return StatusCode(500, new { error = "Internal server error", details = ex.Message });
            }
        }
    }
}
