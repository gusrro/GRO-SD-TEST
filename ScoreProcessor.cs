namespace GRO_SD_TEST;

using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

// Class which represents the engine which processes all score lines
class ScoreProcessor()
{
    //Initializing our dictionary for managing Teams and their scores
    private readonly Dictionary<string, Team> allTeams = [];


    // Public method AddScoreLine - Method used to process an score line and adding points to Teams
    // Param scoreLine - An string representing the format TEAM 1 score, TEAM 2 score
    public string AddScoreLine(string scoreLine ){
        bool isIntegerValid;
        int [] currentScore;
        int counter;
        int index;
        List <string> splittedScore;
        Match match;
        string[] currentName;
        string errorMessage;

        errorMessage = string.Empty;
        // Reviewing the score line in order to comply with a regular expression
        match = Regex.Match(scoreLine, Constants.REGEX_SCORE_LINE, RegexOptions.IgnoreCase);

        if (!match.Success)
        {
            errorMessage = $"Line '{scoreLine}' does not comply Regex validation.";
        }
        else  // Regular expression succeed, now parsing begis
        {
            splittedScore = scoreLine.Split(',').Select(p => p.Trim()).ToList(); //Splitting and trimming each Team Score
            if ( 2 == splittedScore.Count )
            {
                counter = 0;
                currentName = new string[2];
                currentScore = new int [2];
                foreach(string pair in splittedScore)
                {
                    index = pair.LastIndexOf(' '); //Searching for last space before the score value
                    if (index != -1)
                    {
                        try
                        {
                            currentName[counter] = (pair[..index]).Trim(); //Getting Team name
                            isIntegerValid = int.TryParse((pair[(index + 1)..]).Trim(), out currentScore[counter]); //Getting Team score
                            if ( !isIntegerValid )
                            {
                                throw new Exception($"Integer from pair '{pair}' was not valid");
                            }
                            counter++;
                        }
                        catch(Exception exception)
                        {
                            errorMessage = $"Line '{scoreLine}' could not be parsed into a Team-Score pair. Exception: {exception.Message}";
                            break;    
                        }
                    }
                    else
                    {
                        errorMessage = $"Line '{scoreLine}' could not be parsed into a Team-Score pair. Cause: Separator not found";
                        break;
                    }
                }    
                // After parsing completes, processor tries to update Team points
                errorMessage = this.UpdateTeamPoints( currentName, currentScore);        
            }
            else
            {
                errorMessage = $"Line '{scoreLine}' cannot be proccessed due to lack of form or information";
            }
        }
        return errorMessage;
    }

    // Public method GetScoreResults - Method used to get final results from the processor
    public string GetScoreResults(){
        List<Team> sortedList;
        StringBuilder stringBuilder;

        stringBuilder = new StringBuilder();
        sortedList = this.GetSortedFinalResults();

        for ( int i=0; i<sortedList.Count; i++ )
        {
            stringBuilder.Append($"{Environment.NewLine}{i+1}. {sortedList[i]}");
        }
        return stringBuilder.ToString();
    }

    // Public method GetSortedFinalResults - Method used to sort results in the final results
    // Only reason to create this method separately was to abstract the possibility to implement another sorting options
    private List<Team> GetSortedFinalResults()
    {
        List<Team> sortedList;
        sortedList = new List<Team> (this.allTeams.Values);       
        sortedList = [.. sortedList.OrderBy(p=>p.Name).OrderByDescending(o=>o.CurrentPoints)];
        return sortedList;
    }

    // Public method UpdateTeamPoints - Method used to finally evaluate the scores and assign points
    // Param currentName - An string array with both Teams names
    // Param currentScore - An int array with both Teams scores
    private string UpdateTeamPoints(string[] currentName, int [] currentScore )
    {
        string errorMessage;
        Team team1;
        Team team2;
        TextInfo textInfo;

        errorMessage = string.Empty;       
        // Using TextInfo.ToTitleCase to force name to be written and registered properly
        textInfo = CultureInfo.CurrentCulture.TextInfo;
        currentName[0] = textInfo.ToTitleCase(currentName[0].ToLower());
        currentName[1] = textInfo.ToTitleCase(currentName[1].ToLower());    

        // Validation to avoid the same Team Name for both Teams in the score line
        if( currentName[0].Equals(currentName[1]) )
        {
            errorMessage = $"Points won't be awarded when using same team Name '{currentName[0]}'";
        }
        else
        {
            //Getting Team objects from the main dictionary
            team1 = this.getTeamFromDictionary( currentName[0] );
            team2 = this.getTeamFromDictionary( currentName[1] );

            // If both scores are equal, then, both Teams get the same Tie Points Constant
            if( currentScore[0] == currentScore[1] )
            {
                team1.AddPoints( Constants.POINTS_EARNED_BY_TIE );
                team2.AddPoints( Constants.POINTS_EARNED_BY_TIE );
            }
            else if( currentScore[0] > currentScore[1] )  // If Team 1 scores is bigger than Team 2
            {
                // Team 1 get Win Points Constant, Team 2 get Lose Points Constant
                team1.AddPoints( Constants.POINTS_EARNED_BY_WIN );
                team2.AddPoints( Constants.POINTS_EARNED_BY_LOSE );
            }
            else // If Team 1 scores is lower than Team 2
            {
                // Team 2 get Win Points Constant, Team 1 get Lose Points Constant
                team1.AddPoints( Constants.POINTS_EARNED_BY_LOSE );
                team2.AddPoints( Constants.POINTS_EARNED_BY_WIN );
            }
        }
        return errorMessage;
    }

    // Public method getTeamFromDictionary - Method used to get Team object from the main dictionary
    // Param teamKey - A string representing the Team Key Name
    private Team getTeamFromDictionary(string teamKey)
    {
        Team currentTeam;

        if ( allTeams.TryGetValue(teamKey, out Team? value))
        {
            currentTeam = value;
        }
        else
        {
            currentTeam = new Team( teamKey );
            allTeams.Add( teamKey, currentTeam);
        }
        return currentTeam;
    }
}