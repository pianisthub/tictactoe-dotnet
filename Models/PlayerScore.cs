namespace MyAspNetCoreApp.Models
{
    public class PlayerScore
    {
        public int Id { get; set; }
        public string GameId { get; set; } // --Link score to a specific game
        public string PlayerName { get; set; }
        public int Score { get; set; }
    }
}
