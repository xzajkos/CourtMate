using CourtMate.Entities;

namespace CourtMate.Models;

public record RecordScoreRequest(MatchStatus Status, SetScore[]? Sets, Guid? WinnerId);
