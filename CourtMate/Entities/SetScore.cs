namespace CourtMate.Entities;

public class SetScore
{
    public int Player1Games { get; set; }
    public int Player2Games { get; set; }
    public int? Player1TiebreakPoints { get; set; }
    public int? Player2TiebreakPoints { get; set; }

    public bool Player1Won => Player1Games > Player2Games;
    public bool Player2Won => Player2Games > Player1Games;
}
