namespace Gro.SDTest.ScoreProcessor;

using System.Text;

// Class which represents a Team entity
// Parameter Name - It's used for setting Name Property
class TeamEntity(string Name)
{
    // Property Name - Represents name given to Team
    public string Name { get; set; } = Name;
    // Property CurrentPoints - Represents the point the Team has won with completed games
    public int CurrentPoints { get; set; } = 0;

    // Public method AddPoints - Used for adding points to the current total
    // Param Points - Added points per call
    public void AddPoints(int points)
    {
        this.CurrentPoints += points;
    }

    // ToString method override - Just for printing our own label representing Team
    override public string ToString()
    {
        string pointsPosfix;
        StringBuilder toStringValue;

        pointsPosfix = "pts";
        toStringValue = new StringBuilder();

        // Evaluating if prefix must be shorter when value == 1
        if (1 == this.CurrentPoints)
        {
            pointsPosfix = "pt";
        }
        toStringValue.Append($"{this.Name}, {this.CurrentPoints} {pointsPosfix}");
        return toStringValue.ToString();
    }

}
