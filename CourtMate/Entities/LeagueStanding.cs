namespace CourtMate.Entities;

public class LeagueStanding
{
    public User Player { get; set; }
    public int Position { get; set; }
    public int Points { get; set; }
    public int MatchesPlayed { get; set; }
    public int MatchesWon { get; set; }
    public int MatchesLost { get; set; }
    public int SetsWon { get; set; }
    public int SetsLost { get; set; }
    public int GamesWon { get; set; }
    public int GamesLost { get; set; }
}
