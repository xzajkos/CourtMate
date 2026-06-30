namespace CourtMate.Models;

public record MatchResponse(
    Guid Id,
    UserResponse Player1,
    UserResponse Player2,
    DateTime Date,
    string Status,
    string? Score,
    UserResponse? Winner);
