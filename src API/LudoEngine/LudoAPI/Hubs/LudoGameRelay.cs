using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using LudoAPI.Models;
using System.Threading;

namespace LudoAPI.Hubs
{
    public class LudoGameRelay
    {
        public LudoGameRelay(IHubContext<LudoHub> hubContext)
        {
            Task.Factory.StartNew(() =>
            {
                GameModel g = new GameModel(new LudoEngine.Game("name", "game"));
                while (true)
                {
                    hubContext.Clients.All.SendAsync("LudoData", g);
                    Thread.Sleep(1000);
                }
            });
        }
    }
}
