using CourtMate.Entities;
using CourtMate.Models;

namespace CourtMate.Services;

public static class EntityMapper
{
    public static UserResponse ToResponse(this User user) =>
        new(user.Id, user.Username, user.FirstName, user.LastName);

    public static LeagueResponse ToResponse(this LeagueTable league) =>
        new(league.Id, league.LeagueName, league.Players.Select(p => p.ToResponse()).ToArray());

    public static MatchResponse ToResponse(this LeagueMatch match) =>
        new(
            match.Id,
            match.Player1.ToResponse(),
            match.Player2.ToResponse(),
            match.Date,
            match.Status.ToString(),
            match.Score?.ToDisplayString(),
            match.Winner?.ToResponse());

    public static StandingResponse ToResponse(this LeagueStanding standing) =>
        new(
            standing.Position,
            standing.Player.ToResponse(),
            standing.Points,
            standing.MatchesPlayed,
            standing.MatchesWon,
            standing.MatchesLost,
            standing.SetsWon,
            standing.SetsLost,
            standing.GamesWon,
            standing.GamesLost);
}
