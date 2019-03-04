using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using RestSharp;
using LudoApp.Models;
using Microsoft.AspNetCore.Http;
using System.Linq;

namespace LudoApp.Controllers
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

    public class GameController : Controller
    {
        private RestClient client = new RestClient("https://localhost:44350");
        public ActionResult Index()
        {
            RestRequest request = new RestRequest("/api/game", Method.GET);
            request.RequestFormat = DataFormat.Json;
            var ludoGameResponse = client.Execute<List<GameModel>>(request);
            return View(ludoGameResponse.Data);
        }

        [HttpGet]
        public ActionResult GetGame([FromQuery] string gameName)
        {
            RestRequest request = new RestRequest($"/api/game/gamebyid?gameName={gameName}", Method.GET);
            
            request.RequestFormat = DataFormat.Json;
            var LudoGameResponse = client.Execute<GameModel>(request);
            GameModel tempmodel = LudoGameResponse.Data;
            
            switch(tempmodel.GameStatus)
            {
                case "Lobby":
                    {
                        ViewBag.Player = HttpContext.Session.GetString("Player");

                        return View("Lobby", tempmodel);
                    }

                case "Running":
                    {
                       ViewBag.Player = HttpContext.Session.GetString("Player");

                       ViewBag.CurrentPlayer = tempmodel.Players.Where(player =>
                        player.TurnOrder == tempmodel.CurrentTurn
                       ).First().Name;
          
                        
                        return View("GameBoard", tempmodel);
                    }

                case "Won":
                    {
                        return View("Victory", tempmodel);
                    }

                default:
                    {
                        return NotFound();
                    }
            }
        }
        
        [HttpPost]
        public IActionResult PostGame()
        {
            string name = Request.Form["playerName"];
            string gamename = Request.Form["gameName"];
            
            HttpContext.Session.SetString("Player", name);

            RestRequest request = new RestRequest($"/api/game", Method.POST);
            request.AddJsonBody(new { GameName = gamename, PlayerName = name });
            var ludoGameResponse = client.Execute(request);

            return RedirectToAction("GetGame", "Game", new { gameName = gamename });
        }

        [HttpGet]
        public IActionResult CreateGame()
        {
            return View();
        }
        
        [HttpPost]
        public IActionResult AddPlayer([FromBody] DummyGame dummyGame)
        {

            HttpContext.Session.SetString("Player",dummyGame.PlayerName);

            RestRequest request = new RestRequest("/api/game/addplayer", Method.POST);
            request.AddHeader("Content-type", "application/json");
            request.AddJsonBody(new { gameName = dummyGame.GameName, PlayerName = dummyGame.PlayerName });

            var clientresponse = client.Execute(request);
            return Ok();
        }



        [HttpPost]
        public void MovePiece([FromBody] DummyGamePiece gameInstance)
        {
            RestRequest request = new RestRequest($"/api/game/MovePiece", Method.POST);
            request.AddHeader("Content-type", "application/json");
            request.AddJsonBody(new { GameName = gameInstance.GameName,
                                      PlayerName = gameInstance.PlayerName,
                                      pieceId = gameInstance.pieceId });

            var ludoGameResponse = client.Execute(request);
        }

        [HttpPost]
        public IActionResult StartGame([FromBody] DummyGame gameInstance)
        {
            RestRequest request = new RestRequest($"/api/game/StartGame?gameName={gameInstance.GameName}", Method.POST);
            var ludoGameResponse = client.Execute(request);

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

            return ludoGameResponse.Data;
        }
    }
}