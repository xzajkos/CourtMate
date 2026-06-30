namespace CourtMate.Models;

public record StandingResponse(
    int Position,
    UserResponse Player,
    int Points,
    int MatchesPlayed,
    int MatchesWon,
    int MatchesLost,
    int SetsWon,
    int SetsLost,
    int GamesWon,
    int GamesLost);
