using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using RestSharp;
using LudoApp.Models;
using Microsoft.AspNetCore.Http;
using System.Linq;
using Newtonsoft.Json;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;

namespace LudoApp.Controllers
{

    public class DummyGame
    {
        [Required]
        [StringLength(50)]
        public string GameName { get; set; }
        [Required]
        [StringLength(50)]
        public string PlayerName { get; set; }
    }

    public class DummyGamePiece : DummyGame
    {
        [Required]
        [Range(0,3)]
        public string PieceId { get; set; }
    }

    public class GameController : Controller
    {
        private RestClient client = new RestClient("https://localhost:44350");
        private ILogger logger;

        public GameController(ILogger<GameController> l)
        {
            logger = l;
        }

        public ActionResult Index()
        {
            RestRequest request = new RestRequest("/api/game", Method.GET);
            request.RequestFormat = DataFormat.Json;
            var ludoGameResponse = client.Execute<List<GameModel>>(request);

            ViewBag.Player = HttpContext.Session.GetString("Player");

            logger.LogInformation($"Show index page for player {ViewBag.Player}");


            

            return View(ludoGameResponse.Data);
        }

        [HttpGet]
        public ActionResult GetGame([FromQuery] string gameName)
        {
            
            if (gameName == null || gameName == "")
                return NotFound();

            RestRequest request = new RestRequest($"/api/game/gamebyid?gameName={gameName}", Method.GET);
            
            request.RequestFormat = DataFormat.Json;
            var LudoGameResponse = client.Execute<GameModel>(request);
            GameModel tempmodel = LudoGameResponse.Data;


            switch (tempmodel.GameStatus)
            {
                case "Lobby":
                    {
                        ViewBag.Player = HttpContext.Session.GetString("Player");
                        logger.LogInformation($"Show lobby information about '{gameName}' for player '{ViewBag.Player}'");
                        return View("Lobby", tempmodel);
                    }

                case "Running":
                    {
                       ViewBag.Player = HttpContext.Session.GetString("Player");

                       ViewBag.CurrentPlayer = tempmodel.Players.Where(player =>
                        player.TurnOrder == tempmodel.CurrentTurn
                       ).First().Name;

                        logger.LogInformation($"Show information about '{gameName}' for player '{ViewBag.Player}'");
                        logger.LogInformation($"Game '{gameName}' is running and it's player {ViewBag.CurrentPlayer}'s turn");

                        return View("GameBoard", tempmodel);
                    }

                case "Won":
                    {
                        logger.LogInformation($"Show information about finished game '{gameName}'");
                        return View("Victory", tempmodel);
                    }

                default:
                    {
                        return NotFound();
                    }
            }
        }

        [HttpGet]
        public string GetGameModel([FromQuery] string gameName)
        {
            RestRequest request = new RestRequest($"/api/game/gamebyid?gameName={gameName}", Method.GET);

            request.RequestFormat = DataFormat.Json;
            var LudoGameResponse = client.Execute<GameModel>(request);
            GameModel tempmodel = LudoGameResponse.Data;

            return  JsonConvert.SerializeObject(tempmodel);
        }

        [HttpPost]
        public IActionResult PostGame()
        {
            if (ModelState.IsValid)
            {



                string name = Request.Form["playerName"];
                string gamename = Request.Form["gameName"];

                HttpContext.Session.SetString("Player", name);

                RestRequest request = new RestRequest($"/api/game", Method.POST);
                request.AddJsonBody(new { GameName = gamename, PlayerName = name });
                var ludoGameResponse = client.Execute(request);

                return RedirectToAction("GetGame", "Game", new { gameName = gamename });
            }
            else
            {
                return NotFound();

            }

        }

        [HttpGet]
        public IActionResult CreateGame()
        {
            return View();
        }
        
        [HttpPost]
        public IActionResult AddPlayer([FromBody] DummyGame dummyGame)
        {

            if (ModelState.IsValid)
            {
                
                HttpContext.Session.SetString("Player", dummyGame.PlayerName);

                RestRequest request = new RestRequest("/api/game/addplayer", Method.POST);
                request.AddHeader("Content-type", "application/json");
                request.AddJsonBody(new { gameName = dummyGame.GameName, PlayerName = dummyGame.PlayerName });

                logger.LogInformation($"Adding player '{dummyGame.PlayerName}' to game '{dummyGame.GameName}'");

                var clientresponse = client.Execute(request);
                return Ok();
            }
            else
                return NotFound();
            
        }



        [HttpPost]
        public void MovePiece([FromBody] DummyGamePiece gameInstance)
        {
            RestRequest request = new RestRequest($"/api/game/MovePiece", Method.POST);
            request.AddHeader("Content-type", "application/json");
            request.AddJsonBody(new { GameName = gameInstance.GameName,
                                      PlayerName = gameInstance.PlayerName,
                                      PieceId = gameInstance.PieceId });

            logger.LogInformation($"Player '{gameInstance.PlayerName}' is moving piece {gameInstance.PieceId} in game '{gameInstance.GameName}'");

            var ludoGameResponse = client.Execute(request); 
        }

        [HttpPost]
        public IActionResult StartGame([FromBody] DummyGame gameInstance)
        {
            RestRequest request = new RestRequest($"/api/game/StartGame?gameName={gameInstance.GameName}", Method.POST);
            var ludoGameResponse = client.Execute(request);

            logger.LogInformation($"Player '{gameInstance.PlayerName}' is starting game '{gameInstance.GameName}'");

            return Ok();
        }

        [HttpPost]
        public int RollDie([FromBody] DummyGame gameInstance)
        {
            RestRequest request = new RestRequest($"/api/game/RollDie", Method.POST);
            request.AddHeader("Content-type", "application/json");
            request.AddJsonBody(new
            {
                GameName = gameInstance.GameName,
                PlayerName = gameInstance.PlayerName
            });

            var ludoGameResponse = client.Execute<int>(request);

            logger.LogInformation($"Player '{gameInstance.PlayerName}' is rolling the dice in game '{gameInstance.GameName}'");

            return ludoGameResponse.Data;
        }
    }
}