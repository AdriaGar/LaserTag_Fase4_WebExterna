using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

public class RankingModel : PageModel
{
    public List<RankingJugador> Ranking { get; set; } = new();

    public void OnGet()
    {
        try
        {
            var client = new HttpClient();

            var url = "http://192.168.0.100:3000/ranking";

            var response = client.GetAsync(url).Result;

            if (response.IsSuccessStatusCode)
            {
                var json = response.Content.ReadAsStringAsync().Result;

                var data = JsonSerializer.Deserialize<RankingResponse>(
                    json,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                Ranking = data?.ranking ?? new List<RankingJugador>();
            }
        }
        catch
        {
            Ranking = new List<RankingJugador>();
        }
    }
}