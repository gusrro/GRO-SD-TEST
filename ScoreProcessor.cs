using System.Text;

namespace GRO_SD_TEST;

class ScoreProcessor()
{
    private Dictionary<String, Team> allScores = new Dictionary<String, Team>();

    public String AddScoreLine( String scoreLine ){
        String errorMessage;
        String[] splittedString;

        errorMessage = String.Empty;
        splittedString = scoreLine.Split( scoreLine, ',');


        return errorMessage;
    }
    
    public String GetScoreResults(){
        StringBuilder stringBuilder;
        List<Team> sortedList;

        stringBuilder = new StringBuilder();
        sortedList = this.GetSortedFinalResults();

        for ( int i=0; i<sortedList.Count; i++ )
        {
            stringBuilder.Append($"{i+1}. {sortedList[i]}");
        }
        return stringBuilder.ToString();
    }

    private List<Team> GetSortedFinalResults()
    {
        List<Team> sortedList;
        sortedList = new List<Team>();

        sortedList = new List<Team> (this.allScores.Values);
        //TODO sort list mf!!!

        return sortedList;
    }

}