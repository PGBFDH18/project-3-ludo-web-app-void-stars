using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LudoWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using RestSharp;

namespace LudoWeb.Controllers
{
    public class LobbyController : Controller
    {
        public IActionResult Index()
        {
            var client = new RestClient("http://ludogame2019.azurewebsites.net");

            var request = new RestRequest("api/Ludo", Method.GET);

            var ludoGameResponse = client.Execute<List<int>>(request);
            var ListOfGames = ludoGameResponse.Data;

            ViewData["ListOfGames"] = ListOfGames;

            Console.WriteLine(ListOfGames);

            return View();
        }

        public IActionResult New()
        {
            var client = new RestClient("http://ludogame2019.azurewebsites.net");

            var request = new RestRequest("api/Ludo", Method.POST);

            var ludoGameResponse = client.Execute<List<int>>(request);
            var NewGameID = ludoGameResponse.Data.First();

            ViewData["NewGameID"] = NewGameID;

            Console.WriteLine(NewGameID);

            return RedirectToAction("Show", new { GameId = NewGameID.ToString() });
        }

        [HttpPost]
        public IActionResult AddPlayer(string playername, string GameID)
        {
            var client = new RestClient("http://ludogame2019.azurewebsites.net");

            var request = new RestRequest($"api/ludo/{GameID}/players", Method.POST);

            request.AddHeader("Content-Type", "application/json; charset=utf-8");
            request.RequestFormat = DataFormat.Json;

            var asdf = new LudoPlayer();
            asdf.Color = playername;
            asdf.Name = playername;
            
            request.AddJsonBody(asdf);

            var ludoGameResponse = client.Execute<bool>(request);
 
            return RedirectToAction("Show", new { GameId = GameID , playername = playername } );
        }

        public IActionResult Show(int GameID, string playername)
        {
            ViewData["NewGameID"] = GameID;

            ViewData["playername"] = playername;

            var client = new RestClient("http://ludogame2019.azurewebsites.net");

            var request = new RestRequest($"api/Ludo/{GameID}/players", Method.GET);

            var ludoGameResponse = client.Execute<List<LudoPlayer>>(request);

            var Players = ludoGameResponse.Data;
           
            ViewData["Players"] = Players;

            var StateRequest = new RestRequest($"api/Ludo/{GameID}/state", Method.GET);

            var StateResponse = client.Execute(StateRequest);

            var State = StateResponse.Content.ToString();

            ViewData["State"] = State;

            return View();
        }

        [HttpPost]
        public IActionResult StartGame(string GameID, string playername)
        {
            var client = new RestClient("http://ludogame2019.azurewebsites.net");

            var request = new RestRequest($"api/ludo/{GameID}/state", Method.PUT);

            // request.AddHeader("Content-Type", "application/json; charset=utf-8");
            // request.RequestFormat = DataFormat.Json;

            var ludoGameResponse = client.Execute(request);

            return RedirectToAction("Index", "Ludo", new { GameId = GameID, playername = playername });
        }
    }
}