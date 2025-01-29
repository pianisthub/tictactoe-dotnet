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
            var game = new GameState { PlayerX = playerX };
            _dbContext.GameStates.Add(game);
            _dbContext.SaveChanges();
            return Ok(new { gameId = game.GameId });
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

    return Ok(game);
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

    var board = game.GetBoard();
    if (!string.IsNullOrEmpty(board[row][col]))
    {
        return BadRequest("Cell is already occupied.");
    }

    board[row][col] = player;
    game.SetBoard(board);
    game.Winner = CheckWinner(board);
    _dbContext.GameStates.Update(game);
    _dbContext.SaveChanges();

     
    var gameState = new { board = game.GetBoard(), winner = game.Winner };
    await _hubContext.Clients.Group(gameId).SendAsync("ReceiveGameUpdate", gameState);

    return Ok(gameState);
}


        [HttpPost("reset/{gameId}")]
        public IActionResult ResetGame(string gameId)
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
            _dbContext.GameStates.Update(game);
            _dbContext.SaveChanges();
            return Ok(game);
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
