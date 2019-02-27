using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LudoApp.Models
{
    public class GameModel
    {
        public Guid Id { get; set; }
        public string GameName { get; set; }
        public int CurrentTurn { get; set; }
        public string GameStatus { get; set; }
        public List<PlayerModel> Players { get; set; }
    }
}
