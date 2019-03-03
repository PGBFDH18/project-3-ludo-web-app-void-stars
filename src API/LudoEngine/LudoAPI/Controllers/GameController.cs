using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using LudoEngine;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net;
using LudoAPI.Models;

namespace LudoAPI.Controllers
{

    public class DummyGame
    {
        public string GameName { get; set; }
        public string PlayerName { get; set; }
    }

    public class DummyGamePiece : DummyGame
    {
        public string pieceId { get; set; }
    }

    [Route("api/[controller]")]
    [ApiController]
    public class GameController : ControllerBase
    {

        
        private readonly IGameEngine _gameEngine;
        private Dice _dice = new Dice();

        public GameController(IGameEngine ge)
        {
            _gameEngine = ge;
        }


        [HttpPost]
        public ActionResult CreateGame([FromBody] DummyGame temp)
        {
            if (temp.PlayerName != null && temp.GameName != null)
                return Ok(_gameEngine.PushGame(temp.PlayerName, temp.GameName));
            else
                return NotFound();
        }

        
       
        [HttpGet("GameByID")] 
        public ActionResult<GameModel> getGames([FromQuery] string gameName)
        {
          return Ok(new GameModel(_gameEngine.LoadGames(gameName).FirstOrDefault<Game>()));
        }

        [HttpGet]
        public ActionResult<List<GameModel>> ActiveGames()
        {
            List<GameModel> games = new List<GameModel>();
            _gameEngine.LoadGames("All").ForEach(x => games.Add(new GameModel(x)));
            return games;
        }

        
        [HttpDelete("{id}")]
        public void Delete(string gameName)
        {
            _gameEngine.RemoveGame(gameName);
        }
        

        [HttpPost("AddPlayer")]
        public ActionResult AddPlayer([FromBody]DummyGame temp)
        {
            
            _gameEngine.LoadGames(temp.GameName).Where(x => x.NameOfGameInstance == temp.GameName).
                First().AddPlayer(temp.PlayerName);
            return Ok();
        }

        [HttpGet("RollDie")]
        public ActionResult<int> DiceRoll([FromBody] DummyGame gameInstance)
        {
            Game currentGame = _gameEngine.LoadGames(gameInstance.GameName).FirstOrDefault();
            Player currentPlayer = currentGame.Players.Where(x => x.Name == gameInstance.PlayerName).FirstOrDefault();

            if (currentGame.CurrentTurn == currentPlayer.turnOrder)
            {
                _gameEngine.LoadGames(gameInstance.GameName).FirstOrDefault().LatestRoll = _dice.Roll();

                return Ok(_gameEngine.LoadGames(gameInstance.GameName).First().LatestRoll);
            }
            else
                return -1;
           
        }

        [HttpPost("MovePiece")]
        public void MovePiece([FromBody]DummyGamePiece gameInstance)
        {
            Game currentGame = _gameEngine.LoadGames(gameInstance.GameName).FirstOrDefault();
            Player currentPlayer = currentGame.Players.Where(x => x.Name == gameInstance.PlayerName).FirstOrDefault();

            if (currentGame.CurrentTurn == currentPlayer.turnOrder)
            {

                _gameEngine.LoadGames(gameInstance.GameName).First().PlayerMovePiece(gameInstance.pieceId);

                if (_gameEngine.LoadGames(gameInstance.GameName).First().GameStatus == Utils.Won)
                {
                    Redirect("PlayerVictory");
                }
            }
        }

        [HttpGet("PlayerVictory")]
        public string HasPlayerWon([FromBody] string gameName, string playerName)
        {
            return _gameEngine.LoadGames(gameName).First().Victor;
        }

        [HttpPost("StartGame")]
        public void StartGame([FromQuery] string gameName)
        {
            _gameEngine.LoadGames(gameName).First().StartGame();
            
        }

    }
}
