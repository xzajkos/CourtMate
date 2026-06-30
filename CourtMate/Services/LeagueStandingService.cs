using CourtMate.Entities;

namespace CourtMate.Services;

public class LeagueStandingService
{
  private const int PointsPerWin = 2;

  public IReadOnlyList<LeagueStanding> Calculate(LeagueTable league)
  {
    var stats = league.Players.ToDictionary(
      p => p.Id,
      p => new MutableStanding(p));

    foreach (var match in league.Matches.Where(m => m.Status is MatchStatus.Completed or MatchStatus.Walkover))
    {
      var player1Stats = stats[match.Player1.Id];
      var player2Stats = stats[match.Player2.Id];

      player1Stats.MatchesPlayed++;
      player2Stats.MatchesPlayed++;

      if (match.Winner is null)
        continue;

      var loser = match.Winner.Id == match.Player1.Id ? match.Player2 : match.Player1;
      var winnerStats = stats[match.Winner.Id];
      var loserStats = stats[loser.Id];

      winnerStats.MatchesWon++;
      winnerStats.Points += PointsPerWin;
      loserStats.MatchesLost++;

      if (match.Score is null)
        continue;

      var (p1Sets, p2Sets) = (match.Score.Player1SetsWon, match.Score.Player2SetsWon);
      player1Stats.SetsWon += p1Sets;
      player1Stats.SetsLost += p2Sets;
      player2Stats.SetsWon += p2Sets;
      player2Stats.SetsLost += p1Sets;

      foreach (var set in match.Score.Sets)
      {
        player1Stats.GamesWon += set.Player1Games;
        player1Stats.GamesLost += set.Player2Games;
        player2Stats.GamesWon += set.Player2Games;
        player2Stats.GamesLost += set.Player1Games;
      }
    }

    return stats.Values
      .OrderByDescending(s => s.Points)
      .ThenByDescending(s => s.SetsWon - s.SetsLost)
      .ThenByDescending(s => s.GamesWon - s.GamesLost)
      .ThenBy(s => s.Player.LastName)
      .Select((s, index) => new LeagueStanding
      {
        Position = index + 1,
        Player = s.Player,
        Points = s.Points,
        MatchesPlayed = s.MatchesPlayed,
        MatchesWon = s.MatchesWon,
        MatchesLost = s.MatchesLost,
        SetsWon = s.SetsWon,
        SetsLost = s.SetsLost,
        GamesWon = s.GamesWon,
        GamesLost = s.GamesLost
      })
      .ToList();
  }

  private sealed class MutableStanding(User player)
  {
    public User Player { get; } = player;
    public int Points { get; set; }
    public int MatchesPlayed { get; set; }
    public int MatchesWon { get; set; }
    public int MatchesLost { get; set; }
    public int SetsWon { get; set; }
    public int SetsLost { get; set; }
    public int GamesWon { get; set; }
    public int GamesLost { get; set; }
  }
}
