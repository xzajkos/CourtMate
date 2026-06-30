namespace CourtMate.Models;

public record ScheduleMatchRequest(Guid Player1Id, Guid Player2Id, DateTime Date);
