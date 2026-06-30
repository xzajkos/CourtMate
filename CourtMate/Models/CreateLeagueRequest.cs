namespace CourtMate.Models;

public record CreateLeagueRequest(string LeagueName, Guid[] PlayerIds);
