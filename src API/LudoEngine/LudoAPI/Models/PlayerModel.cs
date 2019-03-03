using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LudoAPI.Models
{
    public class PlayerModel
    {
        public string Name { get; set; }
        public string PlayerColor { get; set; }
        public List<PieceModel> Pieces { get; set; }
        public int turnOrder { get; set; }

        public PlayerModel(LudoEngine.Player player)
        {
            Name = player.Name;
            PlayerColor = player.PlayerColor.ToString();
            Pieces = new List<PieceModel>();
            turnOrder = player.turnOrder;

            foreach(LudoEngine.Piece libPiece in player.Pieces)
            {
                this.Pieces.Add(new PieceModel(libPiece));
            }
        }

    }
}
