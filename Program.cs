namespace GRO_SD_TEST;

using System.Net.Http;
using System.Text;
using System.Text.Json;

// Class used as Program execution
class Program
{

    //As some Async call was made, Main changed its original return type
    static async Task Main(string[] args)
    {
        bool areScoresAvailable;
        int lineCounter;
        List<string>? allScores; // Collection to store all scores read from sources
        ScoreProcessor scoreProcessor; //Class used for processing scores
        string errorMessage; // Variable used for setting error messages when a line is processed
        string fileName;
        string source;
        StringBuilder logger; // Logger (as string) for all OK events
        StringBuilder loggerError; // Logger (as string) for all ERROR events

        allScores = null;
        lineCounter = 0;
        logger = new StringBuilder();
        loggerError = new StringBuilder();
        scoreProcessor = new ScoreProcessor();
        source = string.Empty;

        do //Single loop to force a source wich produce results
        {
            Console.WriteLine($"{Environment.NewLine}{Environment.NewLine}Welcome to Coding Test .... by Gustavo Ramos");
            try
            {
                // To process score lines, a file can be used (sampleInput.txt) or you can write NBA for consuming a test API
                Console.WriteLine($"{Environment.NewLine}Please provide Input File or write 'NBA' for using API...");
                fileName = Console.ReadLine() ?? string.Empty;
                areScoresAvailable = false;
                //Once source is received, all lines are set in a List in order to process them
                if ("NBA".Equals(fileName.ToUpper()))
                {
                    source = "NBA API";
                    allScores = await Program.getFromAPI();
                    areScoresAvailable = true;
                }
                else if (!string.Empty.Equals(fileName) && File.Exists(fileName))
                {
                    source = $"File {fileName}";
                    allScores = File.ReadLines(fileName).ToList();
                    areScoresAvailable = true;
                }
                else if (!File.Exists(fileName))
                {
                    source = $"File {fileName}";
                    Console.WriteLine($"Unable to read and process Score Lines by source '{source}', File was not found!!, try again...");
                }

                // If scores are available and are inserted in the allScores list then process begins
                if (areScoresAvailable && null != allScores)
                {
                    logger.Append($"Source selected... {source}... {Environment.NewLine}");
                    if (0 < allScores.Count)
                    {
                        foreach (string line in allScores)
                        {
                            logger.Append($"Processing... {line}... ");
                            // Each line is processed, if an error message returns then the line is ignored and process continue
                            errorMessage = scoreProcessor.AddScoreLine(line);
                            lineCounter++;

                            if (!string.Empty.Equals(errorMessage))
                            {
                                loggerError.Append($"System could not proccess line #{lineCounter}: {line} {Environment.NewLine} Error Message: {errorMessage}");
                                logger.Append($"ERROR{Environment.NewLine}");
                            }
                            else
                            {
                                logger.Append($"OK{Environment.NewLine}");
                            }
                        }

                        Console.WriteLine($"{Environment.NewLine}Process begin... {Environment.NewLine}{logger.ToString()}");
                        // When all score lines has been processed, results are returned with the GetScoreResults() method from ScoreProccesor
                        Console.WriteLine($"{Environment.NewLine}Final results: {scoreProcessor.GetScoreResults()}");
                        Console.WriteLine($"{Environment.NewLine}Process end... Error Log{Environment.NewLine}{loggerError.ToString()}");
                    }
                    else
                    {
                        Console.WriteLine($"There are no records to process!!, try again...");
                        areScoresAvailable = false;
                    }
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine($"Unable to read and process Score Lines by source '{source}', exception '{exception.Message}' was thrown!!, try again...");
                areScoresAvailable = false;
                continue;
            }
        }
        while (!areScoresAvailable);

        Console.Write($"{Environment.NewLine}Exiting...");

    }

    // Public method getFromAPI - Used for querying NBA API from 'https://api.balldontlie.io'
    // Returns a List with strings, the same result type when reading a file
    public async static Task<List<string>> getFromAPI()
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


