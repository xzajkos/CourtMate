namespace CourtMate.Models;

public record LeagueResponse(Guid Id, string LeagueName, UserResponse[] Players);
