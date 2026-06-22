namespace CourtMate.Entities;

public class User
{
    public User(string username, string firstName, string lastName, string password)
    {
        Id = Guid.NewGuid();
        Username = username;
        FirstName = firstName;
        LastName = lastName;
        Password = password;
        Created = DateTime.UtcNow;
        IsActive = true;
    }

    public Guid Id { get; set; }
    public string Username { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Password { get; set; }
    public DateTime Created { get; set; }
    public bool IsActive { get; set; }
}