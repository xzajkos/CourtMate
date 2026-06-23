namespace CourtMate.Entities;

public class LeagueMatch
{
    public Guid Id { get; set; }
    public User Player1 { get; set; }
    public User Player2 { get; set; }
    public DateTime Date { get; set; }
    public MatchStatus Status { get; set; } = MatchStatus.Scheduled;
    public TennisScore? Score { get; set; }
    public User? Winner { get; set; }
}