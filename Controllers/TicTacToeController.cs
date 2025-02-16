using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using MyAspNetCoreApp.Data;
using MyAspNetCoreApp.Hubs;
using MyAspNetCoreApp.Models;

namespace MyAspNetCoreApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TicTacToeController : ControllerBase
    {
        private readonly AppDbContext _dbContext;
          private readonly IHubContext<GameHub> _hubContext;

         public TicTacToeController(AppDbContext dbContext, IHubContext<GameHub> hubContext)
        {
            _dbContext = dbContext;
            _hubContext = hubContext;
        }

        [HttpPost("create")]
public IActionResult CreateGame([FromBody] string playerX)
{
    try
    {
        if (string.IsNullOrWhiteSpace(playerX))
        {
            return BadRequest("Player X name is required.");
        }

        var game = new GameState { PlayerX = playerX , CurrentTurn = "X" };
        _dbContext.GameStates.Add(game);
        _dbContext.SaveChanges();

        return Ok(new { gameId = game.GameId });
    }
    catch (Exception ex)
    {
        Console.WriteLine($"  Error creating game: {ex.Message}");
        return StatusCode(500, new { error = "An unexpected error occurred." });
    }
}


        [HttpPost("join/{gameId}")]
public async Task<IActionResult> JoinGame(string gameId, [FromBody] string playerO)
{
    var game = _dbContext.GameStates.FirstOrDefault(g => g.GameId == gameId);
    if (game == null)
    {
        return NotFound("Game not found.");
    }

    if (!string.IsNullOrEmpty(game.PlayerO))
    {
        return BadRequest("Game already has two players.");
    }

    game.PlayerO = playerO;
    _dbContext.GameStates.Update(game);
    _dbContext.SaveChanges();

     
    await _hubContext.Clients.Group(gameId).SendAsync("PlayerJoined", playerO);

    return Ok(new { gameId = game.GameId, playerX = game.PlayerX, playerO = game.PlayerO });
}


        [HttpGet("{gameId}")]
        public IActionResult GetGameState(string gameId)
        {
            var game = _dbContext.GameStates.FirstOrDefault(g => g.GameId == gameId);
            if (game == null)
            {
                return NotFound("Game not found.");
            }

            return Ok(game);
        }

        [HttpPost("move/{gameId}")]
public async Task<IActionResult> MakeMove(string gameId, int row, int col, string player)
{
    var game = _dbContext.GameStates.FirstOrDefault(g => g.GameId == gameId);
    if (game == null)
    {
        return NotFound("Game not found.");
    }

    Console.WriteLine($"🔍 Player {player} is attempting a move.");
    Console.WriteLine($"🔍 CurrentTurn BEFORE move: {game.CurrentTurn}");

    if (game.Winner != "")
    {
        return BadRequest("Game is already over.");
    }

    if (game.CurrentTurn != player)
    {
        Console.WriteLine("  Move rejected: Not this player's turn.");
        return BadRequest("It's not your turn.");
    }

    var board = game.GetBoard();
    if (!string.IsNullOrEmpty(board[row][col]))
    {
        return BadRequest("Cell is already occupied.");
    }

    board[row][col] = player;
    game.SetBoard(board);

    
    game.CurrentTurn = (game.CurrentTurn == "X") ? "O" : "X";

    Console.WriteLine($"  Move registered! Next turn: {game.CurrentTurn}");

    game.Winner = CheckWinner(board);
    _dbContext.GameStates.Update(game);
    _dbContext.SaveChanges();

    var gameState = new { board = game.GetBoard(), winner = game.Winner, currentTurn = game.CurrentTurn };
    await _hubContext.Clients.Group(gameId).SendAsync("ReceiveGameUpdate", gameState);

    return Ok(gameState);
}




       [HttpPost("reset/{gameId}")]
public async Task<IActionResult> ResetGame(string gameId)
{
    var game = _dbContext.GameStates.FirstOrDefault(g => g.GameId == gameId);
    if (game == null)
    {
        return NotFound("Game not found.");
    }

    game.SetBoard(new string[3][]
    {
        new string[3] { "", "", "" },
        new string[3] { "", "", "" },
        new string[3] { "", "", "" }
    });

    game.Winner = "";
    game.CurrentTurn = "X";  
    _dbContext.GameStates.Update(game);
    _dbContext.SaveChanges();

   await _hubContext.Clients.Group(gameId).SendAsync("GameReset", new { board = game.GetBoard(), currentTurn = game.CurrentTurn });

    return Ok(new { message = "Game reset successfully", currentTurn = game.CurrentTurn });
}


        private string CheckWinner(string[][] board)
        {
            for (int i = 0; i < 3; i++)
            {
                if (!string.IsNullOrEmpty(board[i][0]) &&
                    board[i][0] == board[i][1] &&
                    board[i][1] == board[i][2])
                {
                    return board[i][0];
                }

                if (!string.IsNullOrEmpty(board[0][i]) &&
                    board[0][i] == board[1][i] &&
                    board[1][i] == board[2][i])
                {
                    return board[0][i];
                }
            }

            if (!string.IsNullOrEmpty(board[0][0]) &&
                board[0][0] == board[1][1] &&
                board[1][1] == board[2][2])
            {
                return board[0][0];
            }

            if (!string.IsNullOrEmpty(board[0][2]) &&
                board[0][2] == board[1][1] &&
                board[1][1] == board[2][0])
            {
                return board[0][2];
            }

            return "";
        }
    }
}
