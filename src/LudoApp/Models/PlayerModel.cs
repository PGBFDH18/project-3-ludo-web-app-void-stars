using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LudoApp.Models
{
    public class PlayerModel
    {
        public string Name { get; set; }
        public string PlayerColor { get; set; }
        public List<PieceModel> Pieces { get; set; }
        public int TurnOrder { get; set; }
    }
}
