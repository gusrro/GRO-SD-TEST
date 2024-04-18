namespace GRO_SD_TEST;

// Class which represents Constants for the solution
// Constant values could be changed in this class 
class Constants()
{
    // Constant representing the points earned by winning games.
    public static int POINTS_EARNED_BY_WIN = 3;

    // Constant representing the points earned by tying games.
    public static int POINTS_EARNED_BY_TIE = 1;

    // Constant representing the points earned by losing games.
    public static int POINTS_EARNED_BY_LOSE = 0;

    // Constant representing the regular expression needed for a score line.
    public static string REGEX_SCORE_LINE = @"(\s*([A-Za-z0-9]+\s*)+)\s[0-9]+\s*,(\s*([A-Za-z0-9]+\s*)+)\s[0-9]+\s*";

}
