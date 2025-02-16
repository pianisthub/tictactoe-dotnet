using System.Text.Json;

namespace MyAspNetCoreApp.Models
{
    public class GameState
    {
        public int Id { get; set; }
        public string GameId { get; set; } = new Random().Next(1000, 9999).ToString();  
        public string PlayerX { get; set; } = "";
        public string PlayerO { get; set; } = "";

        public string CurrentTurn { get; set; } = "X";
        
        
        public string Board { get; set; } = JsonSerializer.Serialize(new string[3][]
        {
            new string[3] { "", "", "" },
            new string[3] { "", "", "" },
            new string[3] { "", "", "" }
        });

        public string Winner { get; set; } = "";

         
        public string[][] GetBoard()
        {
            try
            {
                return JsonSerializer.Deserialize<string[][]>(Board) ?? CreateEmptyBoard();
            }
            catch
            {
                return CreateEmptyBoard();
            }
        }

        
        public void SetBoard(string[][] board)
        {
            Board = JsonSerializer.Serialize(board);
        }

        
        private static string[][] CreateEmptyBoard()
        {
            return new string[3][]
            {
                new string[3] { "", "", "" },
                new string[3] { "", "", "" },
                new string[3] { "", "", "" }
            };
        }
    }
}
