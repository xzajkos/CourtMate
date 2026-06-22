namespace CourtMate.Entities;

public class LeagueMatch
{
    public Guid Id { get; set; }
    public User Player1 { get; set; }
    public User Player2 { get; set; }
    public DateTime Date { get; set; }
}