namespace GRO_SD_TEST;

class Program
{
    static void Main(string[] args)
    {
        bool isFileReadable;
        ScoreProcessor scoreProcessor;
        String fileName;
        String errorMessage;
        int lineCounter;
        
        fileName = String.Empty;
        isFileReadable = false;
        scoreProcessor = new ScoreProcessor();
        lineCounter = 0;

        do
        {
            Console.WriteLine("Welcome to Coding Test .... by Gustavo Ramos");
            try
            {
                Console.WriteLine($"{Environment.NewLine}Please provide Input File...");
                fileName = Console.ReadLine() ?? String.Empty;
                if ( !String.Empty.Equals( fileName ) && File.Exists( fileName ))
                {
                    foreach (String line in File.ReadLines( fileName ))
                    {
                        errorMessage = scoreProcessor.AddScoreLine( line );
                        lineCounter++;

                        if ( !String.Empty.Equals( errorMessage ) )
                        {
                            Console.WriteLine($"System could not process line #{lineCounter}: {line}");
                        }
                    }
                    isFileReadable = true;
                }

                Console.WriteLine( scoreProcessor.GetScoreResults() );
            }
            catch( Exception exception )
            {
                Console.WriteLine($"Unable to read and process file {fileName}, exception '{exception.Message}' was thrown!!, try again...");
                isFileReadable = false;
                continue;
            }
        }
        while( !isFileReadable );

        Console.Write($"{Environment.NewLine}Press any key to exit...");
        Console.ReadKey(true);

    }
}


