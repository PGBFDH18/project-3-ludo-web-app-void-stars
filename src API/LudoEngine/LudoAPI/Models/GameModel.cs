using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LudoEngine;

namespace LudoAPI.Models
{
    public class GameModel
    {
        public Guid Id { get; set; }
        public string GameName { get; set; }
        public int CurrentTurn { get; set; }
        public string GameStatus { get; set; }
        public List<PlayerModel> Players { get; set; }
        public int LastRollResult { get; set; }

        public GameModel(LudoEngine.Game game)
        {
            Id = game.Id;
            CurrentTurn = game.CurrentTurn;
            Players = new List<PlayerModel>();
            GameName = game.NameOfGameInstance;
            GameStatus = game.GameStatus.ToString();
            LastRollResult = game.LatestRoll;

            foreach(LudoEngine.Player libPlayer in game.Players)
            {
                this.Players.Add(new PlayerModel(libPlayer));
            }
        }
    }
}
