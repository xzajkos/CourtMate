namespace CourtMate.Models;

public record CreateUserRequest(string Username, string FirstName, string LastName, string Password);
