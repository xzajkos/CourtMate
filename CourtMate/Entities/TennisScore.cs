namespace CourtMate.Entities;

public class TennisScore
{
    public SetScore[] Sets { get; set; } = [];

    public int Player1SetsWon => Sets.Count(s => s.Player1Won);
    public int Player2SetsWon => Sets.Count(s => s.Player2Won);

    public string ToDisplayString()
    {
        return string.Join(", ", Sets.Select(FormatSet));
    }

    private static string FormatSet(SetScore set)
    {
        var score = $"{set.Player1Games}-{set.Player2Games}";

        if (set.Player1TiebreakPoints is not null || set.Player2TiebreakPoints is not null)
        {
            var loserPoints = set.Player1Games > set.Player2Games
                ? set.Player2TiebreakPoints
                : set.Player1TiebreakPoints;
            score += $"({loserPoints})";
        }

        return score;
    }
}
