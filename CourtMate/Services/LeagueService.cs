using CourtMate.Entities;
using CourtMate.Models;

namespace CourtMate.Services;

public class LeagueService
{
  private readonly List<User> _users = [];
  private readonly List<LeagueTable> _leagues = [];
  private readonly LeagueStandingService _standingService;

  public LeagueService(LeagueStandingService standingService)
  {
    _standingService = standingService;
  }

  public IReadOnlyList<User> GetUsers() => _users;

  public User CreateUser(CreateUserRequest request)
  {
    if (_users.Any(u => u.Username.Equals(request.Username, StringComparison.OrdinalIgnoreCase)))
      throw new InvalidOperationException($"User '{request.Username}' already exists.");

    var user = new User(request.Username, request.FirstName, request.LastName, request.Password);
    _users.Add(user);
    return user;
  }

  public IReadOnlyList<LeagueTable> GetLeagues() => _leagues;

  public LeagueTable? GetLeague(Guid id) => _leagues.FirstOrDefault(l => l.Id == id);

  public LeagueTable CreateLeague(CreateLeagueRequest request)
  {
    var players = request.PlayerIds
      .Select(id => _users.FirstOrDefault(u => u.Id == id)
        ?? throw new InvalidOperationException($"Player '{id}' not found."))
      .ToArray();

    if (players.Length < 2)
      throw new InvalidOperationException("League requires at least 2 players.");

    var league = new LeagueTable(request.LeagueName, players);
    _leagues.Add(league);
    return league;
  }

  public LeagueMatch ScheduleMatch(Guid leagueId, ScheduleMatchRequest request)
  {
    var league = GetLeague(leagueId)
      ?? throw new InvalidOperationException($"League '{leagueId}' not found.");

    var player1 = league.Players.FirstOrDefault(p => p.Id == request.Player1Id)
      ?? throw new InvalidOperationException($"Player '{request.Player1Id}' is not in this league.");
    var player2 = league.Players.FirstOrDefault(p => p.Id == request.Player2Id)
      ?? throw new InvalidOperationException($"Player '{request.Player2Id}' is not in this league.");

    if (player1.Id == player2.Id)
      throw new InvalidOperationException("A player cannot play against themselves.");

    var match = new LeagueMatch
    {
      Id = Guid.NewGuid(),
      Player1 = player1,
      Player2 = player2,
      Date = request.Date
    };

    league.Matches = [..league.Matches, match];
    return match;
  }

  public LeagueMatch RecordScore(Guid leagueId, Guid matchId, RecordScoreRequest request)
  {
    var league = GetLeague(leagueId)
      ?? throw new InvalidOperationException($"League '{leagueId}' not found.");

    var match = league.Matches.FirstOrDefault(m => m.Id == matchId)
      ?? throw new InvalidOperationException($"Match '{matchId}' not found.");

    match.Status = request.Status;

    if (request.Sets is { Length: > 0 })
      match.Score = new TennisScore { Sets = request.Sets };

    match.Winner = ResolveWinner(match, request);

    return match;
  }

  public IReadOnlyList<LeagueStanding> GetStandings(Guid leagueId)
  {
    var league = GetLeague(leagueId)
      ?? throw new InvalidOperationException($"League '{leagueId}' not found.");

    return _standingService.Calculate(league);
  }

  public void SeedDemoData()
  {
    if (_users.Count > 0)
      return;

    var players = new[]
    {
      CreateUser(new CreateUserRequest("anowak", "Adam", "Nowak", "demo")),
      CreateUser(new CreateUserRequest("pkowalski", "Piotr", "Kowalski", "demo")),
      CreateUser(new CreateUserRequest("mwisniewska", "Maria", "Wiśniewska", "demo")),
      CreateUser(new CreateUserRequest("jzielinski", "Jan", "Zieliński", "demo"))
    };

    var league = CreateLeague(new CreateLeagueRequest(
      "Liga Wiosenna 2026",
      players.Select(p => p.Id).ToArray()));

    var match1 = ScheduleMatch(league.Id, new ScheduleMatchRequest(
      players[0].Id, players[1].Id, DateTime.UtcNow.AddDays(-7)));
    RecordScore(league.Id, match1.Id, new RecordScoreRequest(
      MatchStatus.Completed,
      [
        new SetScore { Player1Games = 6, Player2Games = 4 },
        new SetScore { Player1Games = 6, Player2Games = 3 }
      ],
      null));

    var match2 = ScheduleMatch(league.Id, new ScheduleMatchRequest(
      players[2].Id, players[3].Id, DateTime.UtcNow.AddDays(-5)));
    RecordScore(league.Id, match2.Id, new RecordScoreRequest(
      MatchStatus.Completed,
      [
        new SetScore { Player1Games = 4, Player2Games = 6 },
        new SetScore { Player1Games = 7, Player2Games = 6, Player1TiebreakPoints = 7, Player2TiebreakPoints = 5 },
        new SetScore { Player1Games = 6, Player2Games = 2 }
      ],
      null));

    ScheduleMatch(league.Id, new ScheduleMatchRequest(
      players[0].Id, players[2].Id, DateTime.UtcNow.AddDays(3)));
  }

  private static User? ResolveWinner(LeagueMatch match, RecordScoreRequest request)
  {
    if (request.Status is MatchStatus.Walkover)
    {
      if (request.WinnerId is null)
        throw new InvalidOperationException("Walkover requires WinnerId.");

      return match.Player1.Id == request.WinnerId ? match.Player1
        : match.Player2.Id == request.WinnerId ? match.Player2
        : throw new InvalidOperationException("Winner must be one of the match players.");
    }

    if (request.Status is not MatchStatus.Completed)
      return null;

    if (match.Score is null)
      throw new InvalidOperationException("Completed match requires a score.");

    if (match.Score.Player1SetsWon == match.Score.Player2SetsWon)
      throw new InvalidOperationException("Score must have a winner.");

    return match.Score.Player1SetsWon > match.Score.Player2SetsWon
      ? match.Player1
      : match.Player2;
  }
}
