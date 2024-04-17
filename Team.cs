using System.Text;

namespace GRO_SD_TEST;

class Team(String Name)
{
    public required string Name { get; set; } = Name;
    private int currentPoints = 0;

    public void AddPoints( int points ){
        this.currentPoints += points;
    }

    
    override public String ToString()
    {
        String pointsPosfix;
        StringBuilder toStringValue;
        
        pointsPosfix = "pts";
        toStringValue = new StringBuilder();
        
        if ( 1 == this.currentPoints)
        {
            pointsPosfix = "pt";
        }

        toStringValue.Append($"{this.Name}, {this.currentPoints} {pointsPosfix}");

        return toStringValue.ToString();
    }

}
