namespace CourtMate.Entities;

public class LeagueTable
{
    public LeagueTable(string leagueName, params User[] players)
    {
        Id = Guid.NewGuid();
        LeagueName = leagueName;
        Players = players;
    }
    public Guid Id { get; set; }
    public string LeagueName { get; set; }
    public User[] Players { get; set; }
    public LeagueMatch[] Matches { get; set; }
}