namespace Gro.SDTest.ScoreProcessor;

using System.Net.Http;
using System.Text;
using System.Text.Json;

// Class used for getting information from Remote API.
class RemoteAPIService
{

    // Public method getFromAPI - Used for querying NBA API from 'https://api.balldontlie.io'
    // Returns a List with strings, the same result type when reading a file
    public async static Task<List<string>> GetFromAPI()
    {
        HttpClient client;
        List<string> scoreLines;
        string scores;
        StringBuilder lineBuilder;

        client = new HttpClient();
        lineBuilder = new StringBuilder();

        //Request to API - Authorization API Key was provided from the site after signing up
        HttpRequestMessage request = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri("https://api.balldontlie.io/v1/games?start_date=2024-01-01&end_date=2024-04-30&per_page=100"),
            Headers =
            {
                { "Authorization", "5d6365e0-d999-46b7-8934-23167c92a4d7" },
            },
        };

        scoreLines = new List<string>();
        using HttpResponseMessage response = await client.SendAsync(request);
        response.EnsureSuccessStatusCode();

        // Once response is gotten JSON is converted to a List with strings, each string containing a score line
        scores = response.Content.ReadAsStringAsync().Result;
        JsonDocument doc = JsonDocument.Parse(scores);
        JsonElement root = doc.RootElement.GetProperty("data");

        for (int i = 0; i < root.GetArrayLength(); i++)
        {
            lineBuilder.Clear();
            lineBuilder.Append($"{root[i].GetProperty("home_team").GetProperty("full_name").GetString()} ");
            lineBuilder.Append($"{root[i].GetProperty("home_team_score").GetUInt32()}, ");
            lineBuilder.Append($"{root[i].GetProperty("visitor_team").GetProperty("full_name").GetString()} ");
            lineBuilder.Append($"{root[i].GetProperty("visitor_team_score").GetUInt32()}");
            scoreLines.Add(lineBuilder.ToString());
        }
        return scoreLines;
    }

}