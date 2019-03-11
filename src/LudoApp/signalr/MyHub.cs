using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using LudoApp.Models;

namespace LudoApp.signalr
{
    public class MyHub : Hub
    {

       public async Task ShowPlayersInGame(GameModel game)
        {
            await Clients.All.SendAsync("RecievePlayers", game.Players);
        }

        public async Task Send(string gamename, string gamestatus)
        {
            await Clients.All.SendAsync("ShowGames", gamename, gamestatus);
        }

    }
}
