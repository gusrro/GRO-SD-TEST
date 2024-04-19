namespace GRO_SD_TEST;

using Gro.SDTest.ScoreProcessor;

// Class used as Program execution
class Program
{
    //As some Async call was made, Main changed its original return type
    static async Task Main(string[] args)
    {
        bool areScoresAvailable;
        List<string>? allScores; // Collection to store all scores read from sources
        ScoreService scoreProcessor; //Class used for processing scores
        string fileName;
        string source;
        String executionLog;

        allScores = null;
        executionLog = string.Empty;
        scoreProcessor = new ScoreService();
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
                    allScores = await RemoteAPIService.GetFromAPI();
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
                    Console.WriteLine($"Source selected... {source}... {Environment.NewLine}");
                    
                    if (0 < allScores.Count)
                    {
                        executionLog = scoreProcessor.ProcessScoreLines( allScores );
                        Console.WriteLine( executionLog );
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

    
}


