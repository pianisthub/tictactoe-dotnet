using System.Text.Json;

namespace MyAspNetCoreApp.Modelss
{
    public class GameState
    {
        public int Id { get; set; }
        public string GameId { get; set; } = Guid.NewGuid().ToString(); // Unique Game ID
        public string PlayerX { get; set; } = "";
        public string PlayerO { get; set; } = "";

        public string Board { get; set; } = JsonSerializer.Serialize(new string[3][]
        {
            new string[3] { "", "", "" },
            new string[3] { "", "", "" },
            new string[3] { "", "", "" }
        });

        public string Winner { get; set; } = "";

        public string[][] GetBoard()
        {
            return JsonSerializer.Deserialize<string[][]>(Board) ?? CreateEmptyBoard();
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
