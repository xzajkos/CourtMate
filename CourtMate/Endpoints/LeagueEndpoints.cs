using CourtMate.Models;
using CourtMate.Services;

namespace CourtMate.Endpoints;

public static class LeagueEndpoints
{
  public static void MapLeagueEndpoints(this WebApplication app)
  {
    var api = app.MapGroup("/api");

    api.MapGet("/users", (LeagueService service) =>
      Results.Ok(service.GetUsers().Select(u => u.ToResponse())));

    api.MapPost("/users", (CreateUserRequest request, LeagueService service) =>
    {
      try
      {
        var user = service.CreateUser(request);
        return Results.Created($"/api/users/{user.Id}", user.ToResponse());
      }
      catch (InvalidOperationException ex)
      {
        return Results.BadRequest(new { error = ex.Message });
      }
    });

    api.MapGet("/leagues", (LeagueService service) =>
      Results.Ok(service.GetLeagues().Select(l => l.ToResponse())));

    api.MapPost("/leagues", (CreateLeagueRequest request, LeagueService service) =>
    {
      try
      {
        var league = service.CreateLeague(request);
        return Results.Created($"/api/leagues/{league.Id}", league.ToResponse());
      }
      catch (InvalidOperationException ex)
      {
        return Results.BadRequest(new { error = ex.Message });
      }
    });

    api.MapGet("/leagues/{leagueId:guid}", (Guid leagueId, LeagueService service) =>
      service.GetLeague(leagueId) is { } league
        ? Results.Ok(league.ToResponse())
        : Results.NotFound());

    api.MapGet("/leagues/{leagueId:guid}/matches", (Guid leagueId, LeagueService service) =>
      service.GetLeague(leagueId) is { } league
        ? Results.Ok(league.Matches.Select(m => m.ToResponse()))
        : Results.NotFound());

    api.MapPost("/leagues/{leagueId:guid}/matches", (
      Guid leagueId, ScheduleMatchRequest request, LeagueService service) =>
    {
      try
      {
        var match = service.ScheduleMatch(leagueId, request);
        return Results.Created($"/api/leagues/{leagueId}/matches/{match.Id}", match.ToResponse());
      }
      catch (InvalidOperationException ex)
      {
        return Results.BadRequest(new { error = ex.Message });
      }
    });

    api.MapPut("/leagues/{leagueId:guid}/matches/{matchId:guid}/score", (
      Guid leagueId, Guid matchId, RecordScoreRequest request, LeagueService service) =>
    {
      try
      {
        var match = service.RecordScore(leagueId, matchId, request);
        return Results.Ok(match.ToResponse());
      }
      catch (InvalidOperationException ex)
      {
        return Results.BadRequest(new { error = ex.Message });
      }
    });

    api.MapGet("/leagues/{leagueId:guid}/standings", (Guid leagueId, LeagueService service) =>
    {
      try
      {
        return Results.Ok(service.GetStandings(leagueId).Select(s => s.ToResponse()));
      }
      catch (InvalidOperationException)
      {
        return Results.NotFound();
      }
    });
  }
}
